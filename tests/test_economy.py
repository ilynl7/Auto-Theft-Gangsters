"""End-to-end mission/shop tests: drive accept -> buy -> complete through the
real server using the provisional economy protocol."""

import asyncio
import os
import sys

sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

import pytest

from server import economy
from server import protocol as P
from server import sproto

from tests.test_e2e import _connect, server  # noqa: F401  (pytest fixture)


async def login_and_pick(game_port, name):
    """Visitor -> verfiy -> login -> create -> pick -> enter map."""
    c = await _connect(game_port)
    resp = await c.rpc(P.VISITOR, {})
    account_id = sproto.as_str(resp.body[0])
    key = sproto.as_str(resp.body[1])
    resp = await c.rpc(P.VERFIY, {0: account_id, 1: key, 2: "14119"})
    session_id = resp.body[1]
    await c.rpc(P.LOGIN, {0: session_id, 1: account_id, 2: 0,
                          3: "1.012.017", 4: "Unity4.7", 5: 1, 6: 12345})
    general = sproto.encode_object({0: name, 2: 0})
    resp = await c.rpc(P.CHARACTER_CREATE, {0: general})
    # response carries a character_overview {id(0), ...}
    char_id = sproto.decode_typed(sproto.as_bytes(resp.body[0]),
                                  {0: "i", 1: "o", 2: "o", 3: "o",
                                   4: "i", 5: "i"})[0]
    await c.rpc(P.CHARACTER_PICK, {0: char_id})
    await c.rpc(P.ENTER_MAP, {0: "11", 1: 0, 2: 1})
    # drain the post-enter push burst (aoi_add, npc_create, sync_skill_info,
    # storage sync) so subsequent rpc() calls see their own responses
    await c.drain(0.4)
    return c, char_id


def decode_object_array(blob) -> list:
    if isinstance(blob, list):
        return [sproto.as_bytes(e) for e in blob]
    blob = sproto.as_bytes(blob)
    total = int.from_bytes(blob[0:4], "little")
    off = 4
    out = []
    while off < 4 + total:
        n = int.from_bytes(blob[off:off + 4], "little")
        off += 4
        out.append(blob[off:off + n])
        off += n
    return out


@pytest.mark.asyncio
async def test_shop_list_and_buy(server):
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "Shopper")

    good = economy.SHOPS[1]["goods"][0]   # 101: 1x item 1, 500 gold

    # ask shop list
    resp = await c.rpc(P.ASK_SHOP_LIST, {0: 1})
    assert resp.body[0] == 0
    goods = [sproto.decode_typed(g, {0: "i", 1: "i", 2: "i", 3: "i", 4: "i"})
             for g in decode_object_array(resp.body[1])]
    assert any(g[0] == good["goods_id"] for g in goods)

    gold_before = srv.db.get_currency(char_id, economy.CURRENCY_GOLD)
    item_before = srv.db.get_item_count(char_id, good["item_id"])

    # buy
    resp = await c.rpc(P.BUY_SHOP_ITEM, {0: good["goods_id"], 1: 1})
    assert resp.body[0] == 0, "purchase must succeed"

    assert srv.db.get_currency(char_id, economy.CURRENCY_GOLD) == \
        gold_before - good["price"]
    assert srv.db.get_item_count(char_id, good["item_id"]) == \
        item_before + good["count"]

    # buying more than the balance can afford must fail with errno 3.
    # New test characters start with a maxed wallet (TEST_START_GOLD), so
    # drain it first to actually hit the insufficient-funds path.
    srv.db.add_currency(char_id, economy.CURRENCY_GOLD,
                        -srv.db.get_currency(char_id, economy.CURRENCY_GOLD))
    resp = await c.rpc(P.BUY_SHOP_ITEM, {0: good["goods_id"], 1: 1})
    assert resp.body[0] == 3

    await c.close()


@pytest.mark.asyncio
async def test_mission_chain_synced_on_pick(server):
    """After pick the server pushes sync_mission (519) with the real
    ownmission schema: string missionId + 8-entry parm, and auto-accepts the
    first tutorial mission on new accounts."""
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "Chainer")

    push = await c.next_push(P.SYNC_MISSION)
    assert push is not None, "sync_mission pushed after pick"
    entries = decode_object_array(push.body[0])
    assert entries, "first tutorial mission must be synced"
    for entry in entries:
        d = sproto.decode_typed(sproto.as_bytes(entry),
                                {0: "s", 1: "i", 2: "i", 3: "ia"})
        mid = sproto.as_str(d[0])
        assert mid.isdigit(), "missionId must be a numeric STRING"
        assert d[1] in (1, 2), "state: ACCEPTED=1 or COMPLETE=2"
        parm = d[3]
        assert len(parm) >= 8, "client reads parm[7] unconditionally"
    # the tutorial mission must be the real chain start (1001)
    mids = [sproto.decode_typed(sproto.as_bytes(e),
                                {0: "s", 1: "i", 2: "i", 3: "ia"})[0]
            for e in entries]
    assert "1001" in [sproto.as_str(m) for m in mids]
    await c.close()


@pytest.mark.asyncio
async def test_main_mission_accept_complete_flow(server):
    """accept_mission(112, string id) -> ret_accept_mission(520) with the
    ownmission blob; complete_mission(113) pays exp + gold and advances the
    main chain via last_missionId."""
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "MainQuest")
    await c.drain(0.4)

    mission_id = "1001"
    # accept
    resp = await c.rpc(P.ACCEPT_MISSION, {0: mission_id})
    # ret_accept_mission {missionId(0), missionquality(1), ret(2), mission(3)}
    assert sproto.as_str(resp.body[0]) == mission_id
    assert resp.body[2] == 0, "accept must succeed"
    assert srv.db.get_mission(char_id, mission_id) is not None

    # duplicate accept is idempotent (the client auto-accepts from
    # last_missionId then re-sends accept_mission): success, no dup row.
    resp = await c.rpc(P.ACCEPT_MISSION, {0: mission_id})
    assert resp.body[2] == 0   # idempotent success
    rows = srv.db.list_missions(char_id)
    assert sum(1 for r in rows if str(r["mission_id"]) == mission_id) == 1

    # completing pays the level-scaled reward and clears the row
    gold_before = srv.db.get_currency(char_id, economy.CURRENCY_GOLD)
    exp_before = srv.db.get_character(char_id)["exp"]
    level_before = srv.db.get_character(char_id)["level"]
    resp = await c.rpc(P.COMPLETE_MISSION, {0: mission_id})
    assert sproto.as_str(resp.body[0]) == mission_id
    assert resp.body[1] == 0, "complete must succeed"
    assert srv.db.get_mission(char_id, mission_id) is None

    reward = economy is not None and __import__(
        "server.mission_data", fromlist=["mission_reward"]).mission_reward(
        mission_id)
    assert srv.db.get_currency(char_id, economy.CURRENCY_GOLD) == \
        gold_before + reward["gold"]
    char_row = srv.db.get_character(char_id)
    assert (char_row["exp"] + 1000 * (char_row["level"] - level_before)
            - exp_before) == reward["exp"], "exp paid (level-ups rolled over)"

    # main chain cursor advanced
    assert srv.db.get_progress(char_id, "main_mission") == 1001

    # abandon the follow-up: accept it first (post-completion the client
    # would auto-accept 1002 via last_missionId, then send accept_mission)
    resp = await c.rpc(P.ACCEPT_MISSION, {0: "1002"})
    assert resp.body[2] == 0
    resp = await c.rpc(P.ABANDON_MISSION, {0: "1002"})
    assert sproto.as_str(resp.body[0]) == "1002"
    assert resp.body[1] == 0
    assert srv.db.get_mission(char_id, "1002") is None

    await c.close()


@pytest.mark.asyncio
async def test_visit_mission_completes_on_map_entry(server):
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "Visitor")

    mission_id = "900101"   # legacy visit mission, map "1"
    resp = await c.rpc(P.ACCEPT_MISSION, {0: mission_id})
    assert resp.body[2] == 0

    # a move inside the target map advances the mission (move has no ack;
    # the server answers with a sync_mission push)
    pos = sproto.encode_object({0: 100, 1: 0, 2: 200, 3: 90})
    await c.send_request(P.MOVE, {0: pos, 1: 1, 2: 1, 3: 0})
    await asyncio.sleep(0.5)

    mrow = srv.db.get_mission(char_id, mission_id)
    assert mrow is not None
    assert mrow["progress"] >= 1
    assert mrow["state"] == 1, "visit mission should complete after moving"

    resp = await c.rpc(P.COMPLETE_MISSION, {0: mission_id})
    assert resp.body[1] == 0
    assert srv.db.get_mission(char_id, mission_id) is None

    await c.close()


@pytest.mark.asyncio
async def test_use_item_consumes_from_backpack(server):
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "PotionDrinker")

    srv.db.add_item(char_id, 1, 3)   # 3x Health Potion

    resp = await c.rpc(P.USE_ITEM, {0: 1, 1: 1})
    assert resp.body[0] == 0
    assert srv.db.get_item_count(char_id, 1) == 2

    # using more than owned fails
    resp = await c.rpc(P.USE_ITEM, {0: 1, 1: 99})
    assert resp.body[0] == 2
    assert srv.db.get_item_count(char_id, 1) == 2

    await c.close()
