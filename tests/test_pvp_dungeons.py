"""End-to-end tests for dungeons (copy scenes), rank PvP ladder, the tower,
the slot machine, and map/line helpers — using the real decompiled tag flows."""

import asyncio
import os
import sys

sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

import pytest

from server import economy
from server import protocol as P
from server import sproto

from tests.test_e2e import _connect, server  # noqa: F401  (pytest fixture)
from tests.test_economy import decode_object_array, login_and_pick


def _drain_pushes(c, want_type, want_count=1, timeout=3.0):
    """Legacy helper kept for compatibility: collect pushes via the client."""
    return []


async def _collect_push(c, want_type, timeout=3.0):
    """Read from the socket until one push of `want_type` arrives."""
    return await c.next_push(want_type, timeout)


def _server_session(srv, char_id):
    """The live Session object for a logged-in character (test hook)."""
    for m in srv.world.maps.values():
        for wp in m.values():
            if wp.char_id == char_id:
                return wp.conn
    return None


@pytest.mark.asyncio
async def test_copy_scene_full_dungeon_run(server):
    """enter_copy_scene -> kill wave npcs -> next_wave -> clear -> reward."""
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "Dungeoneer")
    gold_before = srv.db.get_currency(char_id, economy.CURRENCY_GOLD)

    resp = await c.rpc(P.ENTER_COPY_SCENE, {0: 1})
    assert resp.body[0] == 0
    cdef = economy.COPY_SCENES[1]

    # countdown push arrives
    cd = await c.next_push(P.COUNT_DOWN)
    assert cd is not None and cd.body[0] == cdef["countdown"]

    # kill every wave's npcs; the server session owns the authoritative
    # wave npc ids (push parsing races with the login burst)
    sess = _server_session(srv, char_id)
    assert sess is not None and sess.copy_scene is not None
    for wave_index in range(len(cdef["waves"])):
        state = sess.copy_scene
        # ensure this wave's npcs have been spawned and pushed
        deadline = asyncio.get_event_loop().time() + 2.0
        while state["wave"] <= wave_index and \
                asyncio.get_event_loop().time() < deadline:
            await asyncio.sleep(0.05)
        wave_npc_ids = list(state["npc_ids"])
        assert wave_npc_ids, "wave %d npcs spawned" % wave_index
        for npc_id in wave_npc_ids:
            resp = await c.rpc(P.SINGLE_COPY_SCENE_NPC_DIE, {0: npc_id})
            assert resp.body[0] == 0
        if wave_index + 1 < len(cdef["waves"]):
            nxt = await c.next_push(P.NEXT_WAVE)
            assert nxt is not None, "next_wave push after clearing a wave"

    # final clear -> reward tips push with the dungeon reward
    tips = await c.next_push(P.SHOW_REWARD_ITEMS_TIPS)
    assert tips is not None, "reward tips on dungeon clear"
    reward = cdef["reward"]
    assert tips.body[0] == reward["gold"]
    assert tips.body[1] == reward["diamond"]

    assert srv.db.get_currency(char_id, economy.CURRENCY_GOLD) >= \
        gold_before + reward["gold"]
    assert srv.db.get_progress(char_id, "copy_best_1") == \
        len(cdef["waves"])

    # dungeon state cleared; asking the scene list shows it done
    resp = await c.rpc(P.ASK_COPYSCENES_INFO, {})
    elems = resp.body[0]              # decoded via SYNC_COPYSCENES_INFO "oa"
    if not isinstance(elems, list):
        elems = decode_object_array(sproto.as_bytes(elems))
    entries = [sproto.decode_typed(e, {0: "i", 1: "i", 2: "i"})
               for e in elems]
    mine = next(e for e in entries if e[0] == 1)
    assert mine[2] == 1, "copy scene 1 marked done"

    # leaving without an active run is still ok
    resp = await c.rpc(P.LEAVE_COPY_SCENE, {})
    assert resp.body[0] == 0
    await c.close()


@pytest.mark.asyncio
async def test_copy_scene_unknown_id_rejected(server):
    srv, gate_port, game_port = server
    c, _ = await login_and_pick(game_port, "LostDiver")
    resp = await c.rpc(P.ENTER_COPY_SCENE, {0: 999})
    assert resp.body[0] == 2
    await c.close()


@pytest.mark.asyncio
async def test_rank_pvp_ladder_flow(server):
    """request opponent -> attack to death -> tiantti_result + rewards."""
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "PvPPro")
    score_before = economy.RANK_PVP["base_score"]

    resp = await c.rpc(P.REQUEST_RANDOM_RANK_PVP_OPPONENT, {})
    assert len(resp.body) >= 3   # opp_name, opp_score, opp_hp
    opp_name = sproto.as_str(resp.body[0])
    assert opp_name.startswith("Rival")

    start = await _collect_push(c, P.RANK_PVP_START)
    assert start is not None, "rank_pvp_start push after matchmaking"

    # attack until the server reports the opponent dead
    for _ in range(40):
        resp = await c.rpc(P.RANK_PVP_PLAYER_ATTACK, {0: 60})
        if resp.body[1] == 0:
            break
    assert resp.body[1] == 0, "opponent hp reaches 0"

    result = await _collect_push(c, P.TIANTTI_RESULT)
    assert result is not None, "tiantti_result push"
    assert result.body[0] == 1, "win"
    assert result.body[1] == score_before + economy.RANK_PVP["win_score"]

    reward = await _collect_push(c, P.RANK_PVP_REWARD)
    assert reward is not None
    assert reward.body[0] == economy.RANK_PVP["win_gold"]

    # ladder data updated
    resp = await c.rpc(P.REQUEST_RANK_PVP_DATA, {})
    assert resp.body[0] == score_before + economy.RANK_PVP["win_score"]
    assert resp.body[1] == 1

    # history has the battle
    resp = await c.rpc(P.REQUEST_RANK_PVP_HISTORY, {})
    hist = decode_object_array(resp.body[0])
    assert len(hist) == 1

    # win-count reward: 1 win -> not yet claimable (needs 5)
    resp = await c.rpc(P.TIANTI_REQ_WIN_COUNT_REWARDS, {})
    assert resp.body[0] == 2

    await c.close()


@pytest.mark.asyncio
async def test_tower_climb_and_rewards(server):
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "Climber")

    resp = await c.rpc(P.REQUEST_TOWER_COPY_INFO, {})
    assert resp.body[0] == 0            # current floor
    assert resp.body[1] == economy.TOWER["max_floor"]

    resp = await c.rpc(P.CONTINUE_TOWER_COPY, {})
    assert resp.body[0] == 0
    assert resp.body[1] == 1            # now on floor 1
    f = await _collect_push(c, P.NPC_CREATE)
    assert f is not None, "tower floor npc spawned"

    resp = await c.rpc(P.GRANT_TOWER_REWARD, {})
    assert resp.body[0] == 0
    assert resp.body[1] == economy.TOWER["gold_per_floor"]

    # wipe out advances a floor instantly
    resp = await c.rpc(P.TOWER_WIPE_OUT, {})
    assert resp.body[0] == 0
    assert resp.body[1] == 2
    assert srv.db.get_progress(char_id, "tower_floor") == 2

    # reset spends diamonds and rewinds
    diamonds = srv.db.get_currency(char_id, economy.CURRENCY_DIAMOND)
    resp = await c.rpc(P.TOWER_RESET, {})
    assert resp.body[0] == 0
    assert srv.db.get_progress(char_id, "tower_floor") == 0
    assert srv.db.get_currency(char_id, economy.CURRENCY_DIAMOND) == \
        diamonds - economy.TOWER["reset_diamond_cost"]
    await c.close()


@pytest.mark.asyncio
async def test_slot_machine_spin_and_pool(server):
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "Gambler")
    srv.db.add_currency(char_id, economy.CURRENCY_GOLD, 10000)
    gold_before = srv.db.get_currency(char_id, economy.CURRENCY_GOLD)

    resp = await c.rpc(P.REQUEST_SLOT_INFO, {})
    assert resp.body[0] == economy.SLOT["spin_cost"]

    spins = 5
    for _ in range(spins):
        resp = await c.rpc(P.SPIN_SLOT, {})
        reels = resp.body[0]          # decoded via RET_SPIN_SLOT "ia" spec
        assert isinstance(reels, list) and len(reels) == 3
    # each spin cost the entry fee (minus any payout)
    spent = spins * economy.SLOT["spin_cost"]
    assert srv.db.get_currency(char_id, economy.CURRENCY_GOLD) <= \
        gold_before - spent + spent * economy.SLOT["triple_multiplier"]

    # pool accrues from losses; force some losses then claim
    pool = srv.db.get_progress(char_id, "slot_pool")
    if pool == 0:
        srv.db.set_progress(char_id, "slot_pool", 50)
        pool = 50
    resp = await c.rpc(P.REQUEST_SLOT_SUM_REWARD, {})
    assert resp.body[0] == pool
    assert srv.db.get_progress(char_id, "slot_pool") == 0
    await c.close()


@pytest.mark.asyncio
async def test_map_and_line_helpers(server):
    srv, gate_port, game_port = server
    a, a_id = await login_and_pick(game_port, "Mapper")
    b, _ = await login_and_pick(game_port, "LineHop")

    resp = await a.rpc(P.REQUEST_LINE_STATE, {0: "1"})
    counts = resp.body[0]             # decoded via UPDATE_LINE_STATE "ia"
    assert sum(counts) == 2, "both players on map 1"

    # b switches to line 1 — a no longer sees b's line
    resp = await b.rpc(P.CHANGE_SCENE_LINE, {0: 1})
    assert resp.body[0] == 0
    resp = await a.rpc(P.REQUEST_LINE_STATE, {0: "1"})
    counts = resp.body[0]
    assert counts[0] == 1 and counts[1] == 1

    # enter_new_map to map "2"
    resp = await a.rpc(P.ENTER_NEW_MAP, {0: "2"})
    assert resp.body[0] == 0
    assert a_id not in [p.char_id for p in
                        srv.world.maps.get("1", {}).values()]
    resp = await a.rpc(P.ENTER_TELEPORT_POINT, {0: 1})
    assert resp.body[0] == 0
    assert a_id in [p.char_id for p in srv.world.maps.get("1", {}).values()]

    resp = await a.rpc(P.UPDATE_PLAYER_MAP_INFO, {0: "1", 1: 0})
    assert resp.body[0] == "1"
    await a.close()
    await b.close()


@pytest.mark.asyncio
async def test_misc_progress_tags(server):
    srv, gate_port, game_port = server
    c, char_id = await login_and_pick(game_port, "Progressor")

    resp = await c.rpc(P.TUTORIAL_FINISH, {})
    assert resp.body[0] == 0
    assert srv.db.get_progress(char_id, "tutorial_done") == 1

    resp = await c.rpc(P.UNLOCK_FUNCTION_COMPLETE, {0: 7})
    assert resp.body[0] == 0
    assert srv.db.get_progress(char_id, "unlock_func_7") == 1

    resp = await c.rpc(P.RE_NAME, {0: "NewAlias"})
    assert resp.body[0] == 0
    assert srv.db.get_character(char_id)["name"] == "NewAlias"

    resp = await c.rpc(P.CHANGE_SHOW_TYPE, {0: 2})
    assert resp.body[0] == 0

    resp = await c.rpc(P.START_DOWNLOAD, {})
    assert resp.body is not None
    resp = await c.rpc(P.DOWNLOAD_FINISH, {})
    assert resp.body is not None
    await c.close()
