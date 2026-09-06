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

# --- missions (tags from Protocol registry; schemas provisional)  ----------
ACCEPT_MISSION = 112
COMPLETE_MISSION = 113
ABANDON_MISSION = 114
USE_ITEM = 115
SELL_ITEM = 129
SYNC_MISSION = 519
RET_ACCEPT_MISSION = 520
RET_COMPLETE_MISSION = 521
RET_ABANDON_MISSION = 522
SET_MISSION_STATE = 523
SET_MISSION_PARAM = 524
UPDATE_ITEM = 525
RET_USE_ITEM = 526
SYNC_BACKPACK_ITEM = 592
SHOW_REWARD_ITEMS_TIPS = 638

# --- shop -----------------------------------------------------------------
ASK_SHOP_LIST = 143
BUY_SHOP_ITEM = 144
RET_ASK_SHOP_LIST = 554
RET_BUY_SHOP_ITEM = 652

# --- inventory / equipment -------------------------------------------------
EQUIP_ITEM = 116
UNEQUIP_ITEM = 117
EQUIP_BADGE = 197
UNEQUIP_BADGE = 198
EQUIP_FASHION_ITEM = 221
UNEQUIP_FASHION_ITEM = 222
OPEN_ITEM_PACKAGE = 224
RET_OPEN_ITEM_PACKAGE = 617
REQUEST_UPDATE_STORAGEPACK = 139
PUT_ITEM_STORAGEPACK = 140
TAKE_ITEM_STORAGEPACK = 141
RET_REQUEST_UPDATE_STORAGEPACK = 550
REQUEST_RANDOM_NAME = 118
CHANGE_ITEM_STATE = 240
SYNC_BADGEPACK_ITEM = 604
SYNC_FASHION_BACKPACK_ITEM = 616
SYNC_ITEM_PACK = 611

# --- combat / npcs ---------------------------------------------------------
SKILL_USE = 102
ACCEPT_DAMGE = 111
LOCAL_NPC_DIE = 307
ATTACK_LOCAL_NPC = 317
RELIFE_PLAYER = 132
NPC_CREATE = 509
SHOW_DAMAGE_BOARD = 511
AOI_RELIFE_PLAYER = 512
DROP_ITEM_INFO = 527
NOTICE_RELIFE_PLAYER = 618
SYNC_SKILL_INFO = 540
SKILL_LEVEL_UP = 130
SYNC_COMMON_DATA = 614

# --- guilds ----------------------------------------------------------------
GUILD_CREATE = 146
GUILD_JOIN = 147
GUILD_LEAVE = 148
GUILD_KICK = 149
GUILD_JOB_CHANGE = 150
GUILD_REQ_LIST = 152
GUILD_REQ_INFO = 153
GUILD_APPROVE_RESVERVE = 154
REQ_GUILD_NOTICE = 169
REQ_OPEN_GUILD_SHOP = 171
REQ_BUY_GUILD_GOODS = 172
GUILD_LOG = 174
GUILD_DONATE = 175
SEARCH_GUILD = 176
RET_GUILD_CREATE = 567
RET_GUILD_JOIN = 566
RET_GUILD_LEAVE = 565
RET_GUILD_KICK = 590
RET_GUILD_REQ_LIST = 562
RET_GUILD_REQ_INFO = 563
RET_GUILD_JOB_CHANGE = 589
RET_GUILD_MEMBER_INFO = 581
RET_OPEN_GUILD_SHOP = 582
RET_BUY_GUILD_GOODS = 583
RET_GUILD_LOG = 584
RET_GUILD_DONATE = 585
RET_SEARCH_GUILD = 586
SYNC_GUILD_NEW_MEMBER = 580

# --- friends / social ------------------------------------------------------
ADD_FRIEND = 124
DEL_FRIEND = 125
ASK_CHARACTER_INFO = 142
RET_ADD_FRIEND = 533
RET_DEL_FRIEND = 535
NOTICE_ADD_FRIEND = 536
BE_DELETED_FRIEND = 537
SYN_FRIEND_INFO = 538

# --- mail ------------------------------------------------------------------
SEND_MAIL = 122
MAIL_OPERATION = 123
SEND_MAIL_BOX = 284
MAIL_UPDATE = 531
MAIL_DELETE = 532

# --- daily / sign-in -------------------------------------------------------
REQUEST_DAILY_MISSION = 121
SEND_DAILY_MISSION = 530
GRANT_DAILY_MISSION_REWARD = 612
SIGN_WEEK = 255
RET_SIGN_WEEK = 643
SIGN_30_DAY = 254
RET_SIGN_30_DAY = 642

# --- mounts / cars ---------------------------------------------------------
REQUEST_MOUNT_INFO = 235
MOUNT_EQUIP = 236
MOUNT_UNEQUIP = 237
USE_MOUNT = 238
UNUSE_MOUNT = 239
RET_MOUNT_INFO = 630
BUY_CAR_SHOP = 323
RET_BUY_CAR_SHOP = 691

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
    ACCEPT_MISSION: "accept_mission",
    COMPLETE_MISSION: "complete_mission",
    ABANDON_MISSION: "abandon_mission",
    USE_ITEM: "use_item",
    SELL_ITEM: "sell_item",
    SYNC_MISSION: "sync_mission",
    RET_ACCEPT_MISSION: "ret_accept_mission",
    RET_COMPLETE_MISSION: "ret_complete_mission",
    RET_ABANDON_MISSION: "ret_abandon_mission",
    SET_MISSION_STATE: "set_mission_state",
    SET_MISSION_PARAM: "set_mission_param",
    UPDATE_ITEM: "update_item",
    RET_USE_ITEM: "ret_use_item",
    SYNC_BACKPACK_ITEM: "sync_backpack_item",
    SHOW_REWARD_ITEMS_TIPS: "show_reward_items_tips",
    ASK_SHOP_LIST: "ask_shop_list",
    BUY_SHOP_ITEM: "buy_shop_item",
    RET_ASK_SHOP_LIST: "ret_ask_shop_list",
    RET_BUY_SHOP_ITEM: "ret_buy_shop_item",
    EQUIP_ITEM: "equip_item",
    UNEQUIP_ITEM: "unequip_item",
    EQUIP_BADGE: "equip_badge",
    UNEQUIP_BADGE: "unequip_badge",
    EQUIP_FASHION_ITEM: "equip_fashion_item",
    UNEQUIP_FASHION_ITEM: "unequip_fashion_item",
    OPEN_ITEM_PACKAGE: "open_item_package",
    RET_OPEN_ITEM_PACKAGE: "ret_open_item_package",
    REQUEST_UPDATE_STORAGEPACK: "request_update_storagepack",
    PUT_ITEM_STORAGEPACK: "put_item_storagepack",
    TAKE_ITEM_STORAGEPACK: "take_item_storagepack",
    RET_REQUEST_UPDATE_STORAGEPACK: "ret_request_update_storagepack",
    REQUEST_RANDOM_NAME: "request_random_name",
    CHANGE_ITEM_STATE: "change_item_state",
    SYNC_BADGEPACK_ITEM: "sync_badgepack_item",
    SYNC_FASHION_BACKPACK_ITEM: "sync_fashion_backpack_item",
    SYNC_ITEM_PACK: "sync_item_pack",
    SKILL_USE: "skill_use",
    ACCEPT_DAMGE: "accept_damge",
    LOCAL_NPC_DIE: "local_npc_die",
    ATTACK_LOCAL_NPC: "attack_local_npc",
    NPC_CREATE: "npc_create",
    SHOW_DAMAGE_BOARD: "show_damage_board",
    DROP_ITEM_INFO: "drop_item_info",
    NOTICE_RELIFE_PLAYER: "notice_relife_player",
    SYNC_SKILL_INFO: "sync_skill_info",
    SKILL_LEVEL_UP: "skill_level_up",
    SYNC_COMMON_DATA: "sync_common_data",
    GUILD_CREATE: "guild_create",
    GUILD_JOIN: "guild_join",
    GUILD_LEAVE: "guild_leave",
    GUILD_KICK: "guild_kick",
    GUILD_JOB_CHANGE: "guild_job_change",
    GUILD_REQ_LIST: "guild_req_list",
    GUILD_REQ_INFO: "guild_req_info",
    GUILD_APPROVE_RESVERVE: "guild_approve_resverve",
    REQ_GUILD_NOTICE: "req_guild_notice",
    REQ_OPEN_GUILD_SHOP: "req_open_guild_shop",
    REQ_BUY_GUILD_GOODS: "req_buy_guild_goods",
    GUILD_LOG: "guild_log",
    GUILD_DONATE: "guild_donate",
    SEARCH_GUILD: "search_guild",
    RET_GUILD_CREATE: "ret_guild_create",
    RET_GUILD_JOIN: "ret_guild_join",
    RET_GUILD_LEAVE: "ret_guild_leave",
    RET_GUILD_KICK: "ret_guild_kick",
    RET_GUILD_REQ_LIST: "ret_guild_req_list",
    RET_GUILD_REQ_INFO: "ret_guild_req_info",
    RET_GUILD_JOB_CHANGE: "ret_guild_job_change",
    RET_GUILD_MEMBER_INFO: "ret_guild_member_info",
    RET_OPEN_GUILD_SHOP: "ret_open_guild_shop",
    RET_BUY_GUILD_GOODS: "ret_buy_guild_goods",
    RET_GUILD_LOG: "ret_guild_log",
    RET_GUILD_DONATE: "ret_guild_donate",
    RET_SEARCH_GUILD: "ret_search_guild",
    SYNC_GUILD_NEW_MEMBER: "sync_guild_new_member",
    ADD_FRIEND: "add_friend",
    DEL_FRIEND: "del_friend",
    ASK_CHARACTER_INFO: "ask_character_info",
    RET_ADD_FRIEND: "ret_add_friend",
    RET_DEL_FRIEND: "ret_del_friend",
    NOTICE_ADD_FRIEND: "notice_add_friend",
    BE_DELETED_FRIEND: "be_deleted_friend",
    SYN_FRIEND_INFO: "syn_friend_info",
    SEND_MAIL: "send_mail",
    MAIL_OPERATION: "mail_operation",
    SEND_MAIL_BOX: "send_mail_box",
    MAIL_UPDATE: "mail_update",
    MAIL_DELETE: "mail_delete",
    REQUEST_DAILY_MISSION: "request_daily_mission",
    SEND_DAILY_MISSION: "send_daily_mission",
    GRANT_DAILY_MISSION_REWARD: "grant_daily_mission_reward",
    SIGN_WEEK: "sign_week",
    RET_SIGN_WEEK: "ret_sign_week",
    SIGN_30_DAY: "sign_30_day",
    RET_SIGN_30_DAY: "ret_sign_30_day",
    REQUEST_MOUNT_INFO: "request_mount_info",
    MOUNT_EQUIP: "mount_equip",
    MOUNT_UNEQUIP: "mount_unequip",
    USE_MOUNT: "use_mount",
    UNUSE_MOUNT: "unuse_mount",
    RET_MOUNT_INFO: "ret_mount_info",
    BUY_CAR_SHOP: "buy_car_shop",
    RET_BUY_CAR_SHOP: "ret_buy_car_shop",
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
    # provisional (no decompiled SprotoType survived for these):
    ACCEPT_MISSION: {0: "i"},
    COMPLETE_MISSION: {0: "i"},
    ABANDON_MISSION: {0: "i"},
    USE_ITEM: {0: "i", 1: "i", 2: "i"},
    SELL_ITEM: {0: "i", 1: "i"},
    ASK_SHOP_LIST: {0: "i"},
    BUY_SHOP_ITEM: {0: "i", 1: "i"},
    # inventory / equipment (provisional)
    EQUIP_ITEM: {0: "i"},          # item instance id
    UNEQUIP_ITEM: {0: "i"},        # equip slot
    EQUIP_BADGE: {0: "i"},
    UNEQUIP_BADGE: {0: "i"},
    EQUIP_FASHION_ITEM: {0: "i"},
    UNEQUIP_FASHION_ITEM: {0: "i"},
    OPEN_ITEM_PACKAGE: {0: "i"},   # item id of the package
    PUT_ITEM_STORAGEPACK: {0: "i", 1: "i"},   # item id, count
    TAKE_ITEM_STORAGEPACK: {0: "i", 1: "i"},  # item id, count
    REQUEST_RANDOM_NAME: {0: "i"},  # sex
    CHANGE_ITEM_STATE: {0: "i", 1: "i"},
    SKILL_USE: {0: "i", 1: "i"},   # skill id, target id
    ACCEPT_DAMGE: {0: "i", 1: "i", 2: "i"},  # attacker, damage, hp left
    LOCAL_NPC_DIE: {0: "i"},       # npc id
    ATTACK_LOCAL_NPC: {0: "i", 1: "i"},      # npc id, skill id
    RELIFE_PLAYER: {},
    # guilds (provisional)
    GUILD_CREATE: {0: "s"},                        # name
    GUILD_JOIN: {0: "i"},                          # guild id
    GUILD_LEAVE: {},
    GUILD_KICK: {0: "s"},                          # member name
    GUILD_JOB_CHANGE: {0: "s", 1: "i"},           # member name, job
    GUILD_APPROVE_RESVERVE: {0: "s", 1: "i"},     # name, approve(1)/deny(0)
    REQ_GUILD_NOTICE: {},
    GUILD_DONATE: {0: "i"},                        # gold amount
    SEARCH_GUILD: {0: "s"},                        # name substring
    # friends (provisional)
    ADD_FRIEND: {0: "s"},          # character name
    DEL_FRIEND: {0: "s"},
    ASK_CHARACTER_INFO: {0: "s"},
    # mail (provisional)
    SEND_MAIL: {0: "s", 1: "s", 2: "s"},   # to, title, body
    MAIL_OPERATION: {0: "i", 1: "i"},       # mail id, op (0 read, 1 collect, 2 delete)
    # daily / sign-in
    REQUEST_DAILY_MISSION: {},
    SIGN_WEEK: {},
    SIGN_30_DAY: {},
    # mounts
    REQUEST_MOUNT_INFO: {},
    MOUNT_EQUIP: {0: "i"},
    MOUNT_UNEQUIP: {},
    USE_MOUNT: {0: "i"},
    UNUSE_MOUNT: {},
    BUY_CAR_SHOP: {0: "i"},        # car goods id
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
    # provisional (see server/economy.py for the field layout notes)
    ACCEPT_MISSION: {0: "i"},
    COMPLETE_MISSION: {0: "i"},
    ABANDON_MISSION: {0: "i"},
    RET_ACCEPT_MISSION: {0: "i", 1: "i"},
    RET_COMPLETE_MISSION: {0: "i", 1: "i"},
    RET_ABANDON_MISSION: {0: "i", 1: "i"},
    SYNC_MISSION: {0: "oa"},
    RET_USE_ITEM: {0: "i", 1: "i"},
    UPDATE_ITEM: {0: "i", 1: "i", 2: "i"},
    SYNC_BACKPACK_ITEM: {0: "oa"},
    RET_ASK_SHOP_LIST: {0: "i", 1: "oa"},
    RET_BUY_SHOP_ITEM: {0: "i", 1: "i"},
    EQUIP_ITEM: {0: "i"},
    UNEQUIP_ITEM: {0: "i"},
    EQUIP_BADGE: {0: "i"},
    UNEQUIP_BADGE: {0: "i"},
    EQUIP_FASHION_ITEM: {0: "i"},
    UNEQUIP_FASHION_ITEM: {0: "i"},
    RET_OPEN_ITEM_PACKAGE: {0: "i", 1: "oa"},
    RET_REQUEST_UPDATE_STORAGEPACK: {0: "oa"},
    REQUEST_RANDOM_NAME: {0: "s"},
    SYNC_BADGEPACK_ITEM: {0: "oa"},
    SYNC_FASHION_BACKPACK_ITEM: {0: "oa"},
    SYNC_ITEM_PACK: {0: "oa"},
    RET_SKILL_USE: {0: "i", 1: "i"},
    SYNC_SKILL_INFO: {0: "oa"},
    SHOW_DAMAGE_BOARD: {0: "i", 1: "i", 2: "i"},
    DROP_ITEM_INFO: {0: "i", 1: "i", 2: "i"},
    SYNC_COMMON_DATA: {0: "i", 1: "i", 2: "i", 3: "i"},
    RET_GUILD_CREATE: {0: "i", 1: "i"},
    RET_GUILD_JOIN: {0: "i", 1: "i"},
    RET_GUILD_LEAVE: {0: "i"},
    RET_GUILD_KICK: {0: "i"},
    RET_GUILD_REQ_LIST: {0: "oa"},
    RET_GUILD_REQ_INFO: {0: "o"},
    RET_GUILD_JOB_CHANGE: {0: "i"},
    RET_GUILD_MEMBER_INFO: {0: "o"},
    RET_OPEN_GUILD_SHOP: {0: "i", 1: "oa"},
    RET_BUY_GUILD_GOODS: {0: "i"},
    RET_GUILD_LOG: {0: "oa"},
    RET_GUILD_DONATE: {0: "i", 1: "i"},
    RET_SEARCH_GUILD: {0: "oa"},
    SYNC_GUILD_NEW_MEMBER: {0: "s"},
    RET_ADD_FRIEND: {0: "i", 1: "s"},
    RET_DEL_FRIEND: {0: "i"},
    SYN_FRIEND_INFO: {0: "oa"},
    MAIL_UPDATE: {0: "o"},
    SEND_MAIL_BOX: {0: "oa"},
    MAIL_DELETE: {0: "i"},
    SEND_DAILY_MISSION: {0: "oa"},
    RET_SIGN_WEEK: {0: "i", 1: "i"},
    RET_SIGN_30_DAY: {0: "i", 1: "i"},
    RET_MOUNT_INFO: {0: "oa"},
    RET_BUY_CAR_SHOP: {0: "i"},
}


# Requests whose typed response body is registered under the ret_* tag.
RESPONSE_ALIASES = {
    UPDATE_GAME_SERVER: UPDATE_GAME_SERVER,
    ACCEPT_MISSION: RET_ACCEPT_MISSION,
    COMPLETE_MISSION: RET_COMPLETE_MISSION,
    ABANDON_MISSION: RET_ABANDON_MISSION,
    USE_ITEM: RET_USE_ITEM,
    ASK_SHOP_LIST: RET_ASK_SHOP_LIST,
    BUY_SHOP_ITEM: RET_BUY_SHOP_ITEM,
    OPEN_ITEM_PACKAGE: RET_OPEN_ITEM_PACKAGE,
    REQUEST_UPDATE_STORAGEPACK: RET_REQUEST_UPDATE_STORAGEPACK,
    GUILD_CREATE: RET_GUILD_CREATE,
    GUILD_JOIN: RET_GUILD_JOIN,
    GUILD_LEAVE: RET_GUILD_LEAVE,
    GUILD_KICK: RET_GUILD_KICK,
    GUILD_JOB_CHANGE: RET_GUILD_JOB_CHANGE,
    GUILD_DONATE: RET_GUILD_DONATE,
    SEARCH_GUILD: RET_SEARCH_GUILD,
    ADD_FRIEND: RET_ADD_FRIEND,
    DEL_FRIEND: RET_DEL_FRIEND,
    SIGN_WEEK: RET_SIGN_WEEK,
    SIGN_30_DAY: RET_SIGN_30_DAY,
    REQUEST_MOUNT_INFO: RET_MOUNT_INFO,
    BUY_CAR_SHOP: RET_BUY_CAR_SHOP,
}


def decode_request(msg_type: int, data: bytes) -> dict:
    spec = REQUEST_SPECS.get(msg_type, {})
    return _enc.decode_typed(data, spec)


def decode_response(msg_type: int, data: bytes) -> dict:
    spec = RESPONSE_SPECS.get(RESPONSE_ALIASES.get(msg_type, msg_type), {})
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


# --- mission / shop / item wire objects (provisional schemas) --------------

def encode_mission_state(mission_id: int, progress: int, state: int) -> bytes:
    """sync_mission element: {mission_id(0), progress(1), state(2)}.

    state: 0 = active, 1 = complete (reward claimable), 2 = finished/claimed.
    """
    return _enc.encode_object({0: mission_id, 1: progress, 2: state})


def encode_item_stack(item_id: int, count: int) -> bytes:
    """Backpack element: {item_id(0), count(1)}."""
    return _enc.encode_object({0: item_id, 1: count})


def encode_shop_good(goods_id: int, item_id: int, count: int,
                     currency: int, price: int) -> bytes:
    """ret_ask_shop_list element: {goods_id(0), item_id(1), count(2),
    currency(3), price(4)}."""
    return _enc.encode_object({
        0: goods_id, 1: item_id, 2: count, 3: currency, 4: price,
    })


def encode_reward_tips(items: dict, gold: int = 0, diamond: int = 0) -> bytes:
    """show_reward_items_tips body: {gold(0), diamond(1), items(2, object array
    of encode_item_stack)} — the client shows a reward popup for this."""
    stacks = [_enc.encode_object({0: iid, 1: cnt}) for iid, cnt in items.items()]
    return _enc.encode_object({
        0: gold,
        1: diamond,
        2: _enc.encode_object_array(stacks),
    })


# character_aoi_move {id(0), movement(1), walk(2)}
def encode_character_aoi_move(char_id: int, movement: bytes,
                              walk: bool = True) -> bytes:
    return _enc.encode_object({0: char_id, 1: movement, 2: walk})


# --- npc / combat wire objects (provisional schemas) ------------------------

def encode_npc(npc_id: int, kind: int, level: int, hp: int, max_hp: int,
               pos: dict) -> bytes:
    """npc_create / aoi element: {npc_id(0), kind(1), level(2), hp(3),
    max_hp(4), pos(5: position object)}."""
    return _enc.encode_object({
        0: npc_id, 1: kind, 2: level, 3: hp, 4: max_hp,
        5: encode_position(**pos),
    })


def encode_npc_create(npc) -> bytes:
    """npc_create push body: {npc(0)}."""
    return _enc.encode_object({0: npc})


def encode_skill_info(skill_id: int, level: int) -> bytes:
    """sync_skill_info element: {skill_id(0), level(1)}."""
    return _enc.encode_object({0: skill_id, 1: level})


def encode_friend_entry(char_id: int, name: str, level: int = 1,
                        online: bool = False) -> bytes:
    """syn_friend_info element: {char_id(0), name(1), level(2), online(3)}."""
    return _enc.encode_object({
        0: char_id, 1: name, 2: level, 3: 1 if online else 0,
    })


def encode_mail(mail_id: int, sender: str, title: str, body: str,
                gold: int = 0, diamond: int = 0, collected: int = 0) -> bytes:
    """mail element: {mail_id(0), sender(1), title(2), body(3), gold(4),
    diamond(5), collected(6)}."""
    return _enc.encode_object({
        0: mail_id, 1: sender, 2: title, 3: body,
        4: gold, 5: diamond, 6: collected,
    })


def encode_guild_info(guild_id: int, name: str, leader: str,
                      members: int = 1, notice: str = "", gold: int = 0,
                      level: int = 1) -> bytes:
    """Guild object: {guild_id(0), name(1), leader(2), members(3), notice(4),
    gold(5), level(6)}."""
    return _enc.encode_object({
        0: guild_id, 1: name, 2: leader, 3: members, 4: notice,
        5: gold, 6: level,
    })


def encode_guild_member(name: str, job: int, level: int = 1,
                        online: bool = False) -> bytes:
    """Guild member: {name(0), job(1), level(2), online(3)}
    job: 0 leader, 1 officer, 2 member."""
    return _enc.encode_object({
        0: name, 1: job, 2: level, 3: 1 if online else 0,
    })


def encode_mount_info(car_id: int, equipped: bool = False,
                      in_use: bool = False) -> bytes:
    """ret_mount_info element: {car_id(0), equipped(1), in_use(2)}."""
    return _enc.encode_object({
        0: car_id, 1: 1 if equipped else 0, 2: 1 if in_use else 0,
    })
