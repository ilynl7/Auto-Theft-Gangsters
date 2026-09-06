"""End-to-end tests: run the real server on ephemeral ports and drive the
full client login flow through it."""

import asyncio
import os
import sys
import tempfile

sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

import pytest
import pytest_asyncio

from server import config
from server import protocol as P
from server import sproto
from server.main import GameServer


class Client:
    """Minimal test client speaking the game protocol."""

    def __init__(self, reader, writer) -> None:
        self.reader = reader
        self.writer = writer
        self.decoder = sproto.FrameDecoder()
        self.session_counter = 100

    async def send_request(self, tag: int, body: dict = None):
        self.session_counter += 1
        payload = (
            sproto.encode_object({0: tag, 1: self.session_counter})
            + sproto.encode_object(body or {})
        )
        self.writer.write(sproto.frame_encode(payload))
        await self.writer.drain()
        return self.session_counter

    async def recv_response(self, timeout: float = 5.0):
        """Receive the response matching our session, skipping server pushes
        (frames whose Package carries a type but no session)."""
        import time
        deadline = time.monotonic() + timeout
        while True:
            remaining = deadline - time.monotonic()
            assert remaining > 0, "no response received"
            data = await asyncio.wait_for(self.reader.read(8192), remaining)
            frames = self.decoder.feed(data)
            for payload in frames:
                frame = P.parse_frame(payload, response=True)
                if frame.type is not None and frame.session is not None \
                        and frame.session == self.session_counter:
                    return frame
                # else: server push (aoi/npc/sync/...) — skip

    async def rpc(self, tag: int, body: dict = None, timeout: float = 5.0):
        await self.send_request(tag, body)
        return await self.recv_response(timeout)

    async def drain(self, quiet: float = 0.3, timeout: float = 2.0):
        """Read and discard frames until the socket is quiet for `quiet` s."""
        import time
        deadline = time.monotonic() + timeout
        while time.monotonic() < deadline:
            try:
                data = await asyncio.wait_for(self.reader.read(8192), quiet)
                if not data:
                    break
                self.decoder.feed(data)   # discard
            except asyncio.TimeoutError:
                break

    async def close(self):
        self.writer.close()
        try:
            await self.writer.wait_closed()
        except Exception:
            pass


GAME_SERVER_SPEC = {0: "i", 1: "s", 2: "s", 3: "i", 4: "i"}
CHAR_OVERVIEW_SPEC = {0: "i", 1: "s", 2: "i", 3: "i"}


@pytest_asyncio.fixture
async def server():
    """Start a GameServer on ephemeral ports with a temp database."""
    tmp = tempfile.mkdtemp()
    old_db = config.DB_PATH
    config.DB_PATH = os.path.join(tmp, "test.db")

    srv = GameServer(advertise_ip="127.0.0.1")
    gate = await asyncio.start_server(srv._handle_conn, "127.0.0.1", 0)
    game = await asyncio.start_server(srv._handle_conn, "127.0.0.1", 0)
    srv.gate = gate
    srv.game = game

    gate_port = gate.sockets[0].getsockname()[1]
    game_port = game.sockets[0].getsockname()[1]
    # handlers advertise config ports; sync them to the bound ephemeral ports
    config.GATE_PORT = gate_port
    config.GAME_PORT = game_port

    yield srv, gate_port, game_port

    gate.close()
    game.close()
    await gate.wait_closed()
    await game.wait_closed()
    srv.db.close()
    config.DB_PATH = old_db


async def _connect(port):
    reader, writer = await asyncio.open_connection("127.0.0.1", port)
    return Client(reader, writer)


def decode_object_list(blob) -> list:
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
async def test_update_game_server_lists_revival_server(server):
    srv, gate_port, game_port = server
    c = await _connect(gate_port)
    resp = await c.rpc(P.UPDATE_GAME_SERVER, {})
    assert resp.type == P.UPDATE_GAME_SERVER
    servers = decode_object_list(resp.body[2])
    assert len(servers) == 1
    gs = sproto.decode_typed(servers[0], GAME_SERVER_SPEC)
    assert gs[1] == config.SERVER_NAME
    assert gs[3] == game_port
    await c.close()


@pytest.mark.asyncio
async def test_visitor_creates_account(server):
    srv, gate_port, _ = server
    c = await _connect(gate_port)
    resp = await c.rpc(P.VISITOR, {})
    assert resp.type == P.VISITOR
    account_id = sproto.as_str(resp.body[0])
    key = sproto.as_str(resp.body[1])
    assert account_id.isdigit()
    assert len(key) == 32
    await c.close()


@pytest.mark.asyncio
async def test_verfiy_bad_key_rejected(server):
    srv, gate_port, _ = server
    c = await _connect(gate_port)
    resp = await c.rpc(P.VISITOR, {})
    account_id = sproto.as_str(resp.body[0])
    resp = await c.rpc(P.VERFIY, {0: account_id, 1: "wrong-key", 2: "14119"})
    assert resp.body[0] == 1  # rejected
    await c.close()


@pytest.mark.asyncio
async def test_full_login_flow(server):
    srv, gate_port, game_port = server

    # --- gate: server list ---
    c = await _connect(gate_port)
    resp = await c.rpc(P.UPDATE_GAME_SERVER, {})
    servers = decode_object_list(resp.body[2])
    gs = sproto.decode_typed(servers[0], GAME_SERVER_SPEC)
    assert gs[3] == game_port

    # --- gate: create visitor account ---
    resp = await c.rpc(P.VISITOR, {})
    account_id = sproto.as_str(resp.body[0])
    key = sproto.as_str(resp.body[1])

    # --- gate: verify account ---
    resp = await c.rpc(P.VERFIY, {0: account_id, 1: key, 2: "14119"})
    assert resp.body[0] == 0, "verfiy should succeed"
    session_id = resp.body[1]
    assert session_id > 0
    await c.close()

    # --- game server: login ---
    g = await _connect(game_port)
    resp = await g.rpc(P.LOGIN, {
        0: session_id, 1: account_id, 2: 0,
        3: "1.012.017", 4: "Unity4.7", 5: 1, 6: 12345,
    })
    assert resp.type == P.LOGIN
    assert resp.body[0] == P.LOGIN

    # --- game server: character list (empty) ---
    resp = await g.rpc(P.CHARACTER_LIST, {})
    assert resp.type == P.CHARACTER_LIST

    # --- create a character ---
    general = sproto.encode_object({0: "TestGangster", 2: 0})
    resp = await g.rpc(P.CHARACTER_CREATE, {0: general})
    assert resp.body[1] == 0, "character_create errno must be 0"
    char_overview = sproto.decode_typed(sproto.as_bytes(resp.body[0]),
                                        CHAR_OVERVIEW_SPEC)
    char_id = char_overview[0]
    assert char_overview[1] == "TestGangster"

    # --- pick the character ---
    resp = await g.rpc(P.CHARACTER_PICK, {0: char_id})
    assert resp.body[0] == 0

    # --- enter map ---
    resp = await g.rpc(P.ENTER_MAP, {0: "1", 1: 0, 2: 1})
    assert 0 in resp.body  # main_player_create blob
    mp = sproto.decode_fields(sproto.as_bytes(resp.body[0]))
    own_char = sproto.decode_fields(sproto.as_bytes(mp[0]))
    assert own_char[0] == char_id

    # --- heartbeat ---
    resp = await g.rpc(P.HEART_BEAT, {0: 999, 1: 999})
    assert resp.body[0] == 999
    assert resp.body[1] > 0

    # --- second player joins the same map; movement must broadcast ---
    g2 = await _connect(game_port)
    resp = await g2.rpc(P.LOGIN, {0: session_id, 1: account_id})
    general2 = sproto.encode_object({0: "SecondGangster", 2: 1})
    resp = await g2.rpc(P.CHARACTER_CREATE, {0: general2})
    char_id2 = sproto.decode_typed(sproto.as_bytes(resp.body[0]),
                                   CHAR_OVERVIEW_SPEC)[0]
    await g2.rpc(P.CHARACTER_PICK, {0: char_id2})
    await g2.rpc(P.ENTER_MAP, {0: "1", 1: 0, 2: 1})

    pos = sproto.encode_object({0: 100, 1: 0, 2: 200, 3: 90})
    await g2.send_request(P.MOVE, {0: pos, 1: 1, 2: 1, 3: 0})
    got_move = False
    for _ in range(3):
        data = await asyncio.wait_for(g.reader.read(8192), 5)
        for payload in g.decoder.feed(data):
            frame = P.parse_frame(payload, response=True)
            if frame.type == P.AOI_UPDATE_MOVE:
                got_move = True
                break
        if got_move:
            break
    assert got_move, "g should receive aoi_update_move for g2's movement"

    await g.close()
    await g2.close()
