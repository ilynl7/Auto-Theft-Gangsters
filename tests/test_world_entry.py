"""Regression tests for the real client world-entry flow.

The actual v1.19 client never sends an enter_map request: after character_pick
the SERVER pushes enter_map(503), the client loads the map scene and answers
map_ready(100), and the server then pushes main_player_create(504) + aoi/npc
bursts. These tests drive that exact sequence and parse the wire blobs with
the decompiled client's field tags, which caught the loading-hang bug
(character.movement encoded at tag 5 instead of 7).
"""

import asyncio
import os
import sys

sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

import pytest

from server import protocol as P
from server import sproto
from server.world import encode_character_blob, encode_character_aoi_blob, \
    encode_character_overview

from tests.test_e2e import _connect, server  # noqa: F401  (pytest fixture)

GENERAL_SPEC = {0: "s", 1: "i", 2: "i", 3: "s"}
CHARACTER_SPEC = {0: "i", 1: "o", 2: "o", 5: "o", 6: "o", 7: "o",
                  13: "o"}
CHARACTER_AOI_SPEC = {0: "i", 1: "o", 2: "o", 3: "o", 5: "o", 6: "o"}
ATTRIBUTE_OTHER_SPEC = {0: "i", 1: "i", 2: "i", 15: "i", 16: "i",
                        17: "i"}
PROPERTY_SPEC = {13: "i", 14: "i"}
VISUAL_SPEC = {0: "s", 1: "s", 2: "s", 3: "s", 4: "s", 5: "s", 10: "i"}
RUNTIME_SPEC = {6: "o", 7: "o"}
OVERVIEW_SPEC = {0: "i", 1: "o", 2: "o", 3: "o", 4: "i", 5: "i"}
MOVEMENT_SPEC = {0: "o", 1: "o"}
POSITION_SPEC = {0: "i", 1: "i", 2: "i", 3: "i"}


async def _login_and_create(game_port, name, profession=0):
    """visitor -> verfiy -> login -> character_create -> character_pick.

    Returns (client, char_id). Does NOT send the legacy enter_map request —
    the real client never does.
    """
    c = await _connect(game_port)
    resp = await c.rpc(P.VISITOR, {})
    account_id = sproto.as_str(resp.body[0])
    key = sproto.as_str(resp.body[1])
    resp = await c.rpc(P.VERFIY, {0: account_id, 1: key, 2: "14119"})
    session_id = resp.body[1]
    await c.rpc(P.LOGIN, {0: session_id, 1: account_id, 2: 0,
                          3: "1.012.017", 4: "Unity4.7", 5: 1, 6: 12345})
    general = sproto.encode_object({0: name, 2: profession})
    resp = await c.rpc(P.CHARACTER_CREATE, {0: general})
    # character_create.response {character(0): character_overview, errno(1)}
    overview = sproto.decode_typed(sproto.as_bytes(resp.body[0]),
                                   OVERVIEW_SPEC)
    char_id = overview[0]
    await c.rpc(P.CHARACTER_PICK, {0: char_id})
    return c, char_id


def test_character_blob_movement_at_wire_tag_7():
    """SprotoType.character: movement is wire tag 7, tag 5 is property.

    Encoding movement at tag 5 makes the client parse it as `property`,
    leaving character.movement null — the main player never spawns and the
    loading screen hangs forever.
    """
    pos = P.encode_position(11, 0, 22, 90)
    movement = P.encode_movement({"x": 11, "y": 0, "z": 22, "o": 90})
    blob = encode_character_blob(42, "TagSeven", 3, movement,
                                 profession=1)

    d = sproto.decode_typed(blob, CHARACTER_SPEC)
    assert d[0] == 42
    general = sproto.decode_typed(sproto.as_bytes(d[1]), GENERAL_SPEC)
    assert general == {0: "TagSeven", 1: 1, 2: 0, 3: "1"}
    # attribute_other / property / visual / runtime must be present too —
    # the client hard-dereferences all of them in ObjInitPlayerData.
    attr = sproto.decode_typed(sproto.as_bytes(d[2]), ATTRIBUTE_OTHER_SPEC)
    assert attr[2] == 3                      # level
    prop = sproto.decode_typed(sproto.as_bytes(d[5]), PROPERTY_SPEC)
    assert 13 in prop and 14 in prop         # money1 / money2
    vis = sproto.decode_typed(sproto.as_bytes(d[6]), VISUAL_SPEC)
    assert vis[1] == "104"                   # QJ_A CharacterModelData row
    assert vis[0] == "TagSeven"
    # movement must NOT be at tags 3/5/6 — only 7
    move = sproto.decode_typed(sproto.as_bytes(d[7]), MOVEMENT_SPEC)
    assert sproto.decode_typed(sproto.as_bytes(move[0]), POSITION_SPEC) == \
        {0: 11, 1: 0, 2: 22, 3: 90}
    rt = sproto.decode_typed(sproto.as_bytes(d[13]), RUNTIME_SPEC)
    assert 6 in rt and 7 in rt               # attribute / attribute_all
    assert pos in blob  # sanity: position bytes present


def test_character_aoi_blob_uses_aoi_tags():
    """aoi_add carries character_aoi (movement=5, visual=1), NOT character."""
    movement = P.encode_movement({"x": 1, "y": 0, "z": 2, "o": 0})
    blob = encode_character_aoi_blob(7, "AoiGuy", 2, movement,
                                     profession=2)
    d = sproto.decode_typed(blob, CHARACTER_AOI_SPEC)
    assert d[0] == 7
    vis = sproto.decode_typed(sproto.as_bytes(d[1]), VISUAL_SPEC)
    assert vis[1] == "105"                   # NQS_A model
    general = sproto.decode_typed(sproto.as_bytes(d[2]), GENERAL_SPEC)
    assert general[1] == 2                   # profession
    move = sproto.decode_typed(sproto.as_bytes(d[5]), MOVEMENT_SPEC)
    assert 0 in move
    rt = sproto.decode_typed(sproto.as_bytes(d[6]), RUNTIME_SPEC)
    assert 7 in rt                           # attribute_all (mov speed)


def test_character_overview_blob():
    """character_list / character_create carry character_overview objects
    whose general/attribute_other/visual/createtime are all dereferenced."""
    row = {"id": 5, "name": "Over", "level": 4, "sex": 0,
           "profession": 1, "created_at": 1700000000}
    blob = encode_character_overview(row)
    d = sproto.decode_typed(blob, OVERVIEW_SPEC)
    assert d[0] == 5
    general = sproto.decode_typed(sproto.as_bytes(d[1]), GENERAL_SPEC)
    assert general[1] == 1                   # profession
    attr = sproto.decode_typed(sproto.as_bytes(d[2]), {0: "i", 1: "i"})
    assert attr[0] == 4                      # attribute_overview.level
    vis = sproto.decode_typed(sproto.as_bytes(d[3]), VISUAL_SPEC)
    assert vis[1] == "104"
    assert d[4] == 1700000000                # createtime


@pytest.mark.asyncio
async def test_real_client_world_entry_flow(server):
    """pick -> (push) enter_map -> map_ready -> (push) main_player_create."""
    srv, gate_port, game_port = server
    c, char_id = await _login_and_create(game_port, "FlowRider")

    # 1) the server must PUSH enter_map {mapInfoId(0), line_index(1),
    # line_count(2)} — parsed by the test client with the push spec
    enter = await c.next_push(P.ENTER_MAP)
    assert enter is not None, "server must push enter_map after pick"
    assert sproto.as_str(enter.body[0]) == "1"

    # 2) the client loads the map and answers map_ready
    await c.send_request(P.MAP_READY, {})

    # 3) the server delivers main_player_create with a parseable character
    mpc = await c.next_push(P.MAIN_PLAYER_CREATE)
    assert mpc is not None, "main_player_create must arrive after map_ready"
    top = sproto.decode_typed(sproto.as_bytes(mpc.body[0]),
                              {0: "o", 1: "o"})
    char = sproto.decode_typed(sproto.as_bytes(top[0]), CHARACTER_SPEC)
    assert char[0] == char_id
    move = sproto.decode_typed(sproto.as_bytes(char[7]), MOVEMENT_SPEC)
    assert move is not None and 0 in move
    pos = sproto.decode_typed(sproto.as_bytes(move[0]), POSITION_SPEC)
    assert set(pos.keys()) == {0, 1, 2, 3}
    # everything ObjInitPlayerData hard-dereferences must be present
    assert 2 in char and 5 in char and 6 in char and 13 in char

    # the standalone movement field (tag 1) is a valid movement blob too
    move2 = sproto.decode_typed(sproto.as_bytes(top[1]), MOVEMENT_SPEC)
    assert 0 in move2

    await c.close()


@pytest.mark.asyncio
async def test_map_ready_without_pick_is_ignored(server):
    srv, gate_port, game_port = server
    c = await _connect(game_port)
    await c.rpc(P.VISITOR, {})
    # map_ready with no pending world entry must not crash or join a map
    await c.send_request(P.MAP_READY, {})
    await asyncio.sleep(0.1)
    assert srv.world.maps == {}
    await c.close()


@pytest.mark.asyncio
async def test_aoi_add_uses_character_aoi_blob(server):
    """aoi_add bodies carry SprotoType.character_aoi (movement at tag 5)."""
    srv, gate_port, game_port = server
    a, _ = await _login_and_create(game_port, "AoiA")
    b, b_id = await _login_and_create(game_port, "AoiB")

    for c in (a, b):
        await c.next_push(P.ENTER_MAP)
        await c.send_request(P.MAP_READY, {})
        await c.next_push(P.MAIN_PLAYER_CREATE)

    # AoiA must receive AoiB through aoi_add with movement at tag 7
    aoi = await a.next_push(P.AOI_ADD, timeout=3.0)
    assert aoi is not None, "aoi_add for the second player"
    char = sproto.decode_typed(sproto.as_bytes(aoi.body[0]), {0: "o"})[0]
    char = sproto.decode_typed(sproto.as_bytes(char), CHARACTER_AOI_SPEC)
    assert char[0] == b_id
    # character_aoi: movement=5, visual=1, general=2, attribute_other=3
    assert 7 not in char, "aoi_add must use character_aoi, not character"
    assert 5 in char and 1 in char and 2 in char

    await a.close()
    await b.close()
