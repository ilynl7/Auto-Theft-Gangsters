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
from server.world import encode_character_blob

from tests.test_e2e import _connect, server  # noqa: F401  (pytest fixture)

GENERAL_SPEC = {0: "s", 1: "i", 2: "i"}
CHARACTER_SPEC = {0: "i", 1: "o", 5: "o", 6: "o", 7: "o"}
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
    char_id = sproto.decode_typed(sproto.as_bytes(resp.body[0]),
                                  {0: "i", 1: "s", 2: "i", 3: "i"})[0]
    await c.rpc(P.CHARACTER_PICK, {0: char_id})
    return c, char_id


def test_character_blob_movement_at_wire_tag_7():
    """The client's SprotoType.character decodes movement at wire tag 7.

    Encoding it at tag 5 makes the client parse the movement blob as
    `property`, leaving character.movement null — the main player never
    spawns and the loading screen hangs forever.
    """
    pos = P.encode_position(11, 0, 22, 90)
    movement = P.encode_movement({"x": 11, "y": 0, "z": 22, "o": 90})
    blob = encode_character_blob(42, "TagSeven", 3, 1, movement)

    d = sproto.decode_typed(blob, CHARACTER_SPEC)
    assert d[0] == 42
    general = sproto.decode_typed(sproto.as_bytes(d[1]), GENERAL_SPEC)
    assert general == {0: "TagSeven", 1: 3, 2: 1}
    # movement must NOT be at tags 3/5/6 — only 7
    assert 5 not in d and 6 not in d
    move = sproto.decode_typed(sproto.as_bytes(d[7]), MOVEMENT_SPEC)
    assert sproto.decode_typed(sproto.as_bytes(move[0]), POSITION_SPEC) == \
        {0: 11, 1: 0, 2: 22, 3: 90}
    assert pos in blob  # sanity: position bytes present


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
    assert 5 not in char, "movement must not be misencoded as property (5)"
    move = sproto.decode_typed(sproto.as_bytes(char[7]), MOVEMENT_SPEC)
    assert move is not None and 0 in move
    pos = sproto.decode_typed(sproto.as_bytes(move[0]), POSITION_SPEC)
    assert set(pos.keys()) == {0, 1, 2, 3}

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
async def test_aoi_add_uses_tag7_character_blob(server):
    """aoi_add characters are parsed with the same SprotoType.character."""
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
    char = sproto.decode_typed(sproto.as_bytes(char), CHARACTER_SPEC)
    assert char[0] == b_id
    assert 5 not in char
    assert 7 in char

    await a.close()
    await b.close()
