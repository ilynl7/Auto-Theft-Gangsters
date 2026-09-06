"""End-to-end tests for wild boss (raid boss) and survive mode — using the
real decompiled tag flows: request_wild_boss_info(200)/enter_wild_boss(201)
and request_survive_top(245)/enter_survive_batttle(246)/
survive_battle_finish(637)."""

import asyncio
import os
import sys

sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

import pytest

from server import economy
from server import protocol as P
from server import sproto

from tests.test_e2e import server  # noqa: F401  (pytest fixture)
from tests.test_economy import login_and_pick


@pytest.mark.asyncio
async def test_wild_boss_info_and_enter(server):
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "BossHunter")

    kind_def = economy.NPC_KINDS[economy.WILD_BOSS["kind"]]

    resp = await c.rpc(P.REQUEST_WILD_BOSS_INFO, {})
    assert sproto.as_str(resp.body[0]) == kind_def["name"]
    assert resp.body[3] == kind_def["max_hp"]
    assert resp.body[4] == 1            # alive

    resp = await c.rpc(P.ENTER_WILD_BOSS, {})
    assert resp.body[0] == 0

    # the boss sync arrives as npc_create + countdown pushes (older map-npc
    # pushes from login may still be queued, so match on the boss kind)
    boss_kind = economy.WILD_BOSS["kind"]
    npc = await c.next_push(P.NPC_CREATE)
    deadline = asyncio.get_event_loop().time() + 3.0
    while npc is not None:
        blob = sproto.decode_typed(sproto.as_bytes(npc.body[0]),
                                   {0: "i", 1: "i", 2: "i", 3: "i", 4: "i",
                                    5: "o"})
        if blob[1] == boss_kind:
            break
        npc = await c.next_push(P.NPC_CREATE, timeout=1.0) if \
            asyncio.get_event_loop().time() < deadline else None
    assert blob[1] == boss_kind
    assert blob[4] == kind_def["max_hp"]
    cd = await c.next_push(P.COUNT_DOWN)
    assert cd is not None and cd.body[0] == economy.WILD_BOSS["countdown"]

    # the boss exists in the world registry for map 1
    map_bosses = srv.world.bosses.get("11")
    assert map_bosses is not None
    await c.close()


@pytest.mark.asyncio
async def test_survive_run_full_flow(server):
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "Survivor")
    gold_before = srv.db.get_currency(char_id, economy.CURRENCY_GOLD)

    # leaderboard starts empty for this character
    resp = await c.rpc(P.REQUEST_SURVIVE_TOP, {})
    entries = resp.body[0]              # decoded "oa"
    names_before = []
    if entries:
        elems = entries if isinstance(entries, list) else [entries]
        for e in elems:
            d = sproto.decode_typed(sproto.as_bytes(e), {0: "s", 1: "i"})
            names_before.append(d[0])

    # enter survive wave 0 -> 2 thugs spawn
    resp = await c.rpc(P.ENTER_SURVIVE_BATTLE, {0: 0})
    assert resp.body[0] == 0
    cd = await c.next_push(P.COUNT_DOWN)
    assert cd is not None and cd.body[0] == economy.SURVIVE["countdown"]

    # server session owns the wave npc ids
    sess = None
    for m in srv.world.maps.values():
        for wp in m.values():
            if wp.char_id == char_id:
                sess = wp.conn
    assert sess is not None and sess.copy_scene is not None
    npc_ids = list(sess.copy_scene["npc_ids"])
    assert len(npc_ids) == 2

    # kill the wave
    for npc_id in npc_ids:
        resp = await c.rpc(P.SINGLE_COPY_SCENE_NPC_DIE, {0: npc_id})
        assert resp.body[0] == 0

    # finish at wave 3 -> scaled reward + best recorded
    resp = await c.rpc(P.SURVIVE_BATTLE_FINISH, {0: 3})
    assert resp.body[0] == 0
    assert resp.body[1] == 3            # new best
    reward_gold = economy.SURVIVE["reward"]["gold"] * 3
    assert resp.body[2] == reward_gold
    # wave kills also pay per-kill gold, so the balance grows at least by
    # the survive reward
    assert srv.db.get_currency(char_id, economy.CURRENCY_GOLD) >= \
        gold_before + reward_gold

    # finish again at wave 1 -> best stays 3
    resp = await c.rpc(P.SURVIVE_BATTLE_FINISH, {0: 1})
    assert resp.body[0] == 0
    assert resp.body[1] == 3

    # leaderboard now shows the character
    resp = await c.rpc(P.REQUEST_SURVIVE_TOP, {})
    elems = resp.body[0] if isinstance(resp.body[0], list) \
        else [resp.body[0]]
    found = False
    for e in elems:
        if isinstance(e, list):
            name, best = e[0], e[1]
        else:
            d = sproto.decode_typed(sproto.as_bytes(e), {0: "s", 1: "i"})
            name, best = d[0], d[1]
        if name == "Survivor":
            assert best == 3
            found = True
    assert found, "survivor appears on the leaderboard"
    await c.close()


@pytest.mark.asyncio
async def test_survive_enter_spawns_escalating_waves(server):
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "DeepWave")

    # wave 4 -> 6 npcs, kinds include enforcers (kind 2)
    resp = await c.rpc(P.ENTER_SURVIVE_BATTLE, {0: 4})
    assert resp.body[0] == 0
    sess = None
    for m in srv.world.maps.values():
        for wp in m.values():
            if wp.char_id == char_id:
                sess = wp.conn
    state = sess.copy_scene
    assert len(state["npc_ids"]) == 6
    await c.close()
