"""Protocol tag registry + message schema definitions.

Ported from the decompiled `SprotoType.*` classes of Assembly-CSharp.dll.

Every message is a dict {sproto_tag: value}. Composite fields (nested objects
and arrays) are pre-encoded with the helpers in `sproto.py` and passed as
`bytes`.
"""

from . import sproto as _enc

# --- protocol tags (from Protocol.*.cs) ------------------------------------
VISITOR = 2
VERFIY = 3
LOGIN = 4
FACEBOOK_LINK = 5
FACEBOOK_UNLINK = 6
UPDATE_GAME_SERVER = 7

MAP_READY = 100
MOVE = 101
SKILL_USE = 102
CHARACTER_LIST = 103
CHARACTER_CREATE = 104
CHARACTER_PICK = 105

CHAT = 120
RELIFE_PLAYER = 132

UPDATE_CLIENT_STATE = 280
REFRESH_ONLINE_STATE = 281
GAME_CHECK = 308

ENTER_MAP = 503
MAIN_PLAYER_CREATE = 504
AOI_ADD = 505
AOI_REMOVE = 506
AOI_UPDATE_MOVE = 507
RET_SKILL_USE = 508
AOI_UPDATE_ATTRIBUTE = 510
AOI_RELIFE_PLAYER = 512
AOI_STOP_MOVE = 513
RET_CHAT = 528

HEART_BEAT = 218
LEAVE_GAME = 234

LOGIN_MAX_COUNT = 578
RETRIEVE_ACCOUNT = 660

TAG_NAMES = {
    VISITOR: "visitor",
    VERFIY: "verfiy",
    LOGIN: "login",
    FACEBOOK_LINK: "facebook_link",
    FACEBOOK_UNLINK: "facebook_unlink",
    UPDATE_GAME_SERVER: "update_game_server",
    MAP_READY: "map_ready",
    MOVE: "move",
    SKILL_USE: "skill_use",
    CHARACTER_LIST: "character_list",
    CHARACTER_CREATE: "character_create",
    CHARACTER_PICK: "character_pick",
    CHAT: "chat",
    HEART_BEAT: "heart_beat",
    LEAVE_GAME: "leave_game",
    UPDATE_CLIENT_STATE: "update_client_state",
    REFRESH_ONLINE_STATE: "refresh_online_state",
    GAME_CHECK: "game_check",
    ENTER_MAP: "enter_map",
    MAIN_PLAYER_CREATE: "main_player_create",
    AOI_ADD: "aoi_add",
    AOI_REMOVE: "aoi_remove",
    AOI_UPDATE_MOVE: "aoi_update_move",
    AOI_STOP_MOVE: "aoi_stop_move",
    RELIFE_PLAYER: "relife_player",
    LOGIN_MAX_COUNT: "login_max_count",
    RETRIEVE_ACCOUNT: "retrieve_account",
}


def tag_name(tag: int) -> str:
    return TAG_NAMES.get(tag, "tag_%d" % tag)


# --- wire frame + package header -------------------------------------------

def encode_frame(tag: int, session: int, body: dict) -> bytes:
    """Build a full wire frame for a server->client message.

    Payload layout (matching the client's ProcessPack reader):
        sproto(Package{type=tag, session=session?}) || sproto(body)
    """
    pkg = {0: tag}
    if session is not None:
        pkg[1] = session
    payload = _enc.encode_object(pkg) + _enc.encode_object(body or {})
    return _enc.frame_encode(payload)


class IncomingMessage:
    """Parsed client->server frame: Package header + request body."""

    __slots__ = ("type", "session", "body")

    def __init__(self, type_tag, session, body: dict) -> None:
        self.type = type_tag
        self.session = session
        self.body = body or {}

    def __repr__(self) -> str:  # pragma: no cover - debug aid
        return "IncomingMessage(type=%s(%r) session=%r body=%r)" % (
            tag_name(self.type) if self.type is not None else "?",
            self.type,
            self.session,
            self.body,
        )


def parse_frame(payload: bytes, response: bool = False) -> "IncomingMessage":
    """Split an unpacked frame payload into the Package header and body.

    `response=True` decodes the body with server->client field types
    (used when parsing frames received *from* the server, e.g. in tests).
    """
    dec = _enc.Decoder(payload)
    ptype = None
    session = None
    while (tag := dec.next_tag()) is not None:
        if tag == 0:
            ptype = dec.read_integer()
        elif tag == 1:
            session = dec.read_integer()
        else:
            dec.skip_field()
    consumed = dec.pos
    body = {}
    if consumed < len(payload):
        if response:
            body = decode_response(ptype, payload[consumed:])
        else:
            body = decode_request(ptype, payload[consumed:])
    return IncomingMessage(ptype, session, body)


# --- typed schema snippets --------------------------------------------------
# Per-message field type specs for decoding client requests.
# kinds: i=int, b=bool, s=string, o=nested object, ia=int array,
#        sa=string array, oa=object array
REQUEST_SPECS = {
    VISITOR: {},
    VERFIY: {0: "s", 1: "s", 2: "s"},
    LOGIN: {0: "i", 1: "s", 2: "i", 3: "s", 4: "s", 5: "i", 6: "i"},
    CHARACTER_CREATE: {0: "o"},
    CHARACTER_PICK: {0: "i"},
    ENTER_MAP: {0: "s", 1: "i", 2: "i"},
    MOVE: {0: "o", 1: "b", 2: "i", 3: "i"},
    CHAT: {0: "i", 1: "s", 2: "s", 3: "i", 4: "i", 5: "ia", 6: "sa"},
    HEART_BEAT: {0: "i", 1: "i"},
}

# Server->client response field specs (used by tests / client-side parsing).
RESPONSE_SPECS = {
    VISITOR: {0: "s", 1: "s", 2: "i"},
    VERFIY: {0: "i", 1: "i", 2: "oa", 3: "s", 4: "i", 5: "s", 6: "s", 7: "i"},
    LOGIN: {0: "i", 1: "s", 2: "s", 3: "i"},
    UPDATE_GAME_SERVER: {2: "oa"},
    CHARACTER_LIST: {0: "oa"},
    CHARACTER_CREATE: {0: "o", 1: "i"},
    CHARACTER_PICK: {0: "i"},
    MAP_READY: {},
    ENTER_MAP: {0: "o"},
    HEART_BEAT: {0: "i", 1: "i"},
}


def decode_request(msg_type: int, data: bytes) -> dict:
    spec = REQUEST_SPECS.get(msg_type, {})
    return _enc.decode_typed(data, spec)


def decode_response(msg_type: int, data: bytes) -> dict:
    spec = RESPONSE_SPECS.get(msg_type, {})
    return _enc.decode_typed(data, spec)


# position {x,y,z,o} — all integers
def encode_position(x: int, y: int, z: int, o: int) -> bytes:
    return _enc.encode_object({0: x, 1: y, 2: z, 3: o})


def decode_position(data: bytes) -> dict:
    d = _enc.decode_typed(data, {0: "i", 1: "i", 2: "i", 3: "i"})
    return {
        "x": d.get(0, 0),
        "y": d.get(1, 0),
        "z": d.get(2, 0),
        "o": d.get(3, 0),
    }


# movement {pos, pos2} — position objects
def encode_movement(pos: dict, pos2: dict = None) -> bytes:
    fields = {0: encode_position(**pos)}
    if pos2:
        fields[1] = encode_position(**pos2)
    return _enc.encode_object(fields)


# character_overview: {id(0), name(1), level(2), sex(3), online(4), ...}
def encode_character_overview(char_id: int, name: str, level: int = 1,
                              sex: int = 0) -> bytes:
    return _enc.encode_object({
        0: char_id,
        1: name,
        2: level,
        3: sex,
    })


# game_server object (tags per SprotoType.game_server)
def encode_game_server(server_id, name, ip, port, state=0,
                       player_state=0, area=0, rank=1, tz=8,
                       weight=0, new_server=0) -> bytes:
    return _enc.encode_object({
        0: server_id,
        1: name,
        2: ip,
        3: port,
        4: state,
        5: player_state,
        6: area,
        7: rank,
        8: tz,
        9: weight,
        10: new_server,
    })


# character_aoi_move {id(0), movement(1), walk(2)}
def encode_character_aoi_move(char_id: int, movement: bytes,
                              walk: bool = True) -> bytes:
    return _enc.encode_object({0: char_id, 1: movement, 2: walk})
