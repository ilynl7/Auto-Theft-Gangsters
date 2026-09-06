"""Client session: one instance per TCP connection, wraps the wire codec."""

import asyncio
import logging

from . import protocol as P
from . import sproto

log = logging.getLogger("atg.session")


class Session:
    """Per-connection state machine + raw sender."""

    def __init__(self, reader: asyncio.StreamReader,
                 writer: asyncio.StreamWriter, server) -> None:
        self.reader = reader
        self.writer = writer
        self.server = server          # GameServer (owns handlers, world, db)
        self.decoder = sproto.FrameDecoder()
        # local address this connection arrived on — the reachable IP the
        # client used, advertised back for the game-server hop
        try:
            self.local_ip = writer.get_extra_info("sockname")[0]
        except Exception:
            self.local_ip = ""
        if self.local_ip in ("0.0.0.0", "::"):
            self.local_ip = ""
        self.account_id = None
        self.verified = False
        self.logged_in = False
        self.world_player = None      # set once the character enters the map
        self.closed = False

    # -- outbound -------------------------------------------------------
    def send_raw(self, frame: bytes) -> None:
        if self.closed:
            return
        try:
            self.writer.write(frame)
        except Exception:  # pragma: no cover - transport errors surfaced later
            log.exception("write failed")

    def send(self, tag: int, body: dict = None, session: int = None) -> None:
        self.send_raw(P.encode_frame(tag, session, body))

    def respond(self, msg: P.IncomingMessage, body: dict) -> None:
        """Reply to a client request, echoing its session id."""
        self.send_raw(P.encode_frame(msg.type, msg.session, body))

    def push(self, tag: int, body: dict = None) -> None:
        """Server-initiated push (no session)."""
        self.send(tag, body, session=None)

    # -- inbound --------------------------------------------------------
    async def run(self) -> None:
        try:
            while not self.closed:
                data = await self.reader.read(8192)
                if not data:
                    break
                frames = self.decoder.feed(data)
                for payload in frames:
                    msg = P.parse_frame(payload)
                    await self.server.dispatch(self, msg)
        except (sproto.SprotoError, Exception) as exc:
            if not self.closed:
                log.warning("connection error: %r", exc)
        finally:
            await self.close()

    async def close(self) -> None:
        if self.closed:
            return
        self.closed = True
        try:
            if self.world_player is not None:
                await self.server.on_disconnect(self)
            self.writer.close()
            try:
                await self.writer.wait_closed()
            except Exception:
                pass
        except Exception:
            log.exception("error during close")
