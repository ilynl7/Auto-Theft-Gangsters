"""World state: per-map player + NPC registry with AOI broadcast helpers.

The original game used interest-management pushes (aoi_add / aoi_remove /
aoi_update_move ...). This revival keeps it simple: every player in the same
map + line sees every other player and every NPC in that map + line, and
movement/chat/combat are broadcast to the map.

NPCs are server-simulated: they stand at their spawn point, take damage from
player attacks, die, drop loot + exp + gold, and respawn after a delay.
"""

import random

from . import economy
from . import protocol as P


class WorldPlayer:
    __slots__ = ("conn", "char_id", "name", "level", "sex", "profession",
                 "map_id", "line_index", "pos", "moving", "walk", "dead",
                 "comb_value")

    def __init__(self, conn, char_id: int, name: str, level: int = 1,
                 sex: int = 0, profession: int = 0) -> None:
        self.conn = conn
        self.char_id = char_id
        self.name = name
        self.level = level
        self.sex = sex
        self.profession = profession
        self.map_id = economy.MAIN_CITY_MAP
        self.line_index = 0
        self.pos = {"x": 0, "y": 0, "z": 0, "o": 0}
        self.moving = False
        self.walk = True
        self.dead = False
        self.comb_value = 100 * level

    def movement_blob(self) -> bytes:
        return P._enc.encode_object({
            0: P.encode_position(**self.pos),
        })


class WorldNpc:
    __slots__ = ("npc_id", "kind", "map_id", "pos", "hp", "max_hp",
                 "dead", "level", "name")

    _next_id = [9000]

    def __init__(self, kind: int, map_id: str, pos: dict) -> None:
        WorldNpc._next_id[0] += 1
        self.npc_id = WorldNpc._next_id[0]
        kind_def = economy.NPC_KINDS[kind]
        self.kind = kind
        self.name = kind_def["name"]
        self.level = kind_def["level"]
        self.map_id = map_id
        self.pos = dict(pos)
        self.max_hp = kind_def["max_hp"]
        self.hp = self.max_hp
        self.dead = False

    def blob(self) -> bytes:
        return P.encode_npc(self.npc_id, self.kind, self.level, self.hp,
                            self.max_hp, self.pos)


class World:
    def __init__(self) -> None:
        # map_id -> {char_id: WorldPlayer}
        self.maps: dict = {}
        # map_id -> {npc_id: WorldNpc}
        self.npcs: dict = {}
        # map_id -> the live wild boss (raid boss) WorldNpc, if spawned
        self.bosses: dict = {}
        self._spawn_npcs()

    def _spawn_npcs(self) -> None:
        for map_id, spawns in economy.NPC_SPAWNS.items():
            for kind, x, y, z in spawns:
                npc = WorldNpc(kind, map_id, {"x": x, "y": y, "z": z, "o": 0})
                self.npcs.setdefault(map_id, {})[npc.npc_id] = npc

    # -- players --------------------------------------------------------
    def join(self, player: WorldPlayer) -> None:
        self.maps.setdefault(player.map_id, {})[player.char_id] = player

    def leave(self, player: WorldPlayer) -> None:
        m = self.maps.get(player.map_id)
        if m and player.char_id in m:
            del m[player.char_id]
            if not m:
                del self.maps[player.map_id]
        # notify everyone else
        self.broadcast(player.map_id, P.AOI_REMOVE,
                       {0: _encode_aoi_remove(player.char_id)},
                       exclude=player.char_id)

    def others(self, map_id: str, exclude_char_id: int):
        return [p for p in self.maps.get(map_id, {}).values()
                if p.char_id != exclude_char_id]

    def get(self, map_id: str, char_id: int):
        return self.maps.get(map_id, {}).get(char_id)

    def get_player_anywhere(self, char_id: int):
        for m in self.maps.values():
            if char_id in m:
                return m[char_id]
        return None

    def npcs_in(self, map_id: str):
        return list(self.npcs.get(map_id, {}).values())

    def get_npc(self, map_id: str, npc_id: int):
        return self.npcs.get(map_id, {}).get(npc_id)

    # -- messaging ------------------------------------------------------
    def broadcast(self, map_id: str, tag: int, body: dict,
                  exclude: int = None, session: int = None) -> None:
        frame = P.encode_frame(tag, session, body)
        for p in self.maps.get(map_id, {}).values():
            if exclude is not None and p.char_id == exclude:
                continue
            p.conn.send_raw(frame)

    # -- combat -----------------------------------------------------------
    def player_attack(self, player: WorldPlayer, attack: int) -> int:
        return max(1, attack + random.randint(-2, 2))

    def npc_attack(self, npc: WorldNpc) -> int:
        kind_def = economy.NPC_KINDS[npc.kind]
        return max(1, kind_def["damage"] + random.randint(-1, 1))

    def kill_npc(self, npc: WorldNpc, char_id: int, char_name: str):
        """Kill an NPC; returns (exp, gold, drops {item_id: count})."""
        kind_def = economy.NPC_KINDS[npc.kind]
        npc.dead = True
        npc.hp = 0
        rng = random.Random()
        drops = {}
        for item_id, (chance, count) in kind_def["loot"].items():
            if rng.random() < chance:
                drops[item_id] = drops.get(item_id, 0) + count
        return kind_def["exp"], kind_def["gold"], drops

    def respawn_npc(self, npc: WorldNpc) -> None:
        kind_def = economy.NPC_KINDS[npc.kind]
        npc.dead = False
        npc.hp = npc.max_hp


# --- wire blob helpers shared by handlers -----------------------------------

def _encode_aoi_remove(char_id: int) -> bytes:
    # SprotoType.aoi_remove.request {characterId(0)}
    return P._enc.encode_object({0: char_id})


# --- wire schemas, straight from the decompiled Assembly-CSharp.dll ----------
#
# The client's sproto types are distinguished ONLY by tag order, so every
# blob must use the exact wire tags of the matching SprotoType.* class.
# WARNING: the has_field bitset indices in the decompiled classes are NOT
# the wire tags — always read the decode() switch.
#
# SprotoType.character (main_player_create): id(0) general(1)
#   attribute_other(2) property(5) visual(6) movement(7) skills(8)
#   runtime(13) download(15) skill_index(16)
# SprotoType.character_aoi (aoi_add): id(0) visual(1) general(2)
#   attribute_other(3) movement(5) runtime(6)
# SprotoType.character_overview (character_list / character_create resp):
#   id(0) general(1) attribute_other(2, attribute_overview) visual(3)
#   createtime(4) forbidden(5)
#
# Sub-schemas (tags the client hard-dereferences in ObjInitPlayerData /
# main_player_create_handler / CreateMainPlayer — a missing OBJECT crashes
# the handler and the loading widget hangs forever):
#   general:            name(0) profession(1) lineIndex(2) mapInfoId(3)
#                       tutorial(4)
#   attribute_other:    hp(0) exp(1) level(2) combValue(3) camp(15)
#                       pkMode(16) dance_state(17) dance_id(18)
#   attribute_overview: level(0) combValue(1)
#   property:           money1(13)..money6(18)
#   visual:             name(0) ModeId(1) HeadId(2) BodyId(3) LegId(4)
#                       WeaponId(5) showType(10) MountId(12) mount_state(13)
#                       mount_color(14)
#   runtime_agent:      attribute(6) attribute_all(7)  (both SprotoType.attribute)
#   attribute:          max_hp(0) exp(1) atk(2) def(3) hit(4) eva(5) cri(6)
#                       res(7) mov(13) rec(14)

# CharacterModelData row ids (from the APK's Data.bundle): 100=XD_A
# (Batfighter), 104=QJ_A (Boxer), 105=NQS_A (Gunner). Body parts live in
# the ModelData table: *_WQ weapon / *_T head / *_S body / *_X legs
# (their ModelType ints map onto the client's MODEL_TYPE enum 0..3).
PROFESSION_MODELS = {
    0: ("100", "XD_A"),
    1: ("104", "QJ_A"),
    2: ("105", "NQS_A"),
}


# Default movement speed the client derives as attribute_all.mov / 100.
_DEFAULT_MOV = 500
_DEFAULT_HP = 120


def _general_blob(name: str, profession: int, line_index: int = 0,
                  map_id: str = economy.MAIN_CITY_MAP) -> bytes:
    return P._enc.encode_object({
        0: name, 1: profession, 2: line_index, 3: map_id,
    })


def _visual_blob(profession: int, name: str) -> bytes:
    model_id, base = PROFESSION_MODELS.get(profession, PROFESSION_MODELS[0])
    return P._enc.encode_object({
        0: name,
        1: model_id,          # ModeId -> CharacterModelData row
        2: base + "_T",       # HeadId
        3: base + "_S",       # BodyId
        4: base + "_X",       # LegId
        5: base + "_WQ",      # WeaponId
        10: 0,                # showType: 0 = normal parts, not fashion
        12: "",               # MountId
        13: 0,                # mount_state
        14: "",               # mount_color
    })


def _attribute_blob(hp: int, exp: int, atk: int = 10) -> bytes:
    return P._enc.encode_object({
        0: hp,                # max_hp
        1: exp,
        2: atk,
        3: 2,                 # def
        4: 10,                # hit
        5: 5,                 # eva
        13: _DEFAULT_MOV,     # mov -> client speed = mov / 100
        14: 0,                # rec
    })


def _runtime_blob(hp: int, exp: int) -> bytes:
    attr = _attribute_blob(hp, exp)
    return P._enc.encode_object({6: attr, 7: attr})


def _attribute_other_blob(hp: int, exp: int, level: int,
                          comb_value: int = 0) -> bytes:
    return P._enc.encode_object({
        0: hp, 1: exp, 2: level,
        3: comb_value,        # combValue (combat power shown on the profile)
        4: 0, 5: 0,           # title_level / title_exp
        15: 0,                # camp
        16: 0,                # pkMode
        17: 0,                # dance_state
        18: "",               # dance_id
    })


def _property_blob(gold: int = 0, diamond: int = 0) -> bytes:
    return P._enc.encode_object({13: gold, 14: diamond, 15: 0, 16: 0,
                                 17: 0, 18: 0})


def _attribute_overview_blob(level: int, comb_value: int = 0) -> bytes:
    return P._enc.encode_object({0: level, 1: comb_value})


def _default_hp(level: int) -> int:
    return _DEFAULT_HP + 20 * max(0, level - 1)


def _char_stats(level: int, exp: int = 0):
    """(hp, exp) pair used by the character blobs."""
    return _default_hp(level), exp


def encode_character_blob(char_id: int, name: str, level: int,
                          pos_blob: bytes, profession: int = 0,
                          line_index: int = 0,
                          map_id: str = economy.MAIN_CITY_MAP,
                          exp: int = 0, gold: int = 0, diamond: int = 0,
                          hp: int = None, skills: list = None,
                          comb_value: int = 0) -> bytes:
    """Full SprotoType.character blob for main_player_create.

    The client's ObjInitPlayerData.InitData(character) hard-dereferences
    general, attribute_other, property (money1..6), visual, movement and
    runtime.attribute_all — every one of those objects must be present at
    its exact wire tag or the handler throws and the main player never
    spawns (eternal loading). download=2 also marks the client as fully
    downloaded (main_player_create_handler sets IsFinishDownload).

    `skills` is an optional [(skill_id, level)] list encoded at tag 8 as
    map<string, skill_info>; skillId is decoded with read_string, so the id
    MUST be a string ("101"), never an integer.
    """
    if hp is None:
        hp, _ = _char_stats(level, exp)
    fields = {
        0: char_id,
        1: _general_blob(name, profession, line_index, map_id),
        2: _attribute_other_blob(hp, exp, level, comb_value),
        5: _property_blob(gold, diamond),
        6: _visual_blob(profession, name),
        7: pos_blob,
        13: _runtime_blob(hp, exp),
        15: 2,                # download = 2 -> IsFinishDownload = true
    }
    if skills:
        skill_blobs = [P.encode_skill_info(sid, lvl) for sid, lvl in skills]
        fields[8] = P._enc.encode_object_array(skill_blobs)
        fields[16] = 0        # skill_index: hard-dereferenced by InitData
    return P._enc.encode_object(fields)


def encode_character_aoi_blob(char_id: int, name: str, level: int,
                              pos_blob: bytes, profession: int = 0,
                              line_index: int = 0,
                              map_id: str = economy.MAIN_CITY_MAP,
                              exp: int = 0, hp: int = None) -> bytes:
    """SprotoType.character_aoi blob for aoi_add (NOT SprotoType.character!).

    aoi_add_handler calls ObjInitPlayerData.InitData(character_aoi), which
    hard-dereferences visual, general, attribute_other, movement and
    runtime.attribute_all — at character_aoi's own wire tags.
    """
    if hp is None:
        hp, _ = _char_stats(level, exp)
    return P._enc.encode_object({
        0: char_id,
        1: _visual_blob(profession, name),
        2: _general_blob(name, profession, line_index, map_id),
        3: _attribute_other_blob(hp, exp, level, 100 * level),
        5: pos_blob,
        6: _runtime_blob(hp, exp),
    })


def encode_character_overview(row, visual_profession: int = None,
                              comb_value: int = None) -> bytes:
    """SprotoType.character_overview for character_list / character_create.

    The client dereferences .general.profession, .attribute_other.level,
    .visual and .createtime on this type (CreateRoleRootLogic /
    ChooseRoleRootLogic), so all four sub-objects must be present.
    attribute_overview.combValue (field 1) is the combat power the role
    select screen shows — a rolled base power, not 0.
    """
    profession = row["profession"] if visual_profession is None \
        else visual_profession
    if comb_value is None:
        comb_value = 100 * row["level"]
    return P._enc.encode_object({
        0: row["id"],
        1: _general_blob(row["name"], profession),
        2: _attribute_overview_blob(row["level"], comb_value),
        3: _visual_blob(profession, row["name"]),
        4: row["created_at"],
        5: 0,                 # forbidden
    })


def encode_aoi_add(player: WorldPlayer) -> bytes:
    """SprotoType.aoi_add.request {character(0)} — a character_aoi blob."""
    character = encode_character_aoi_blob(
        player.char_id, player.name, player.level, player.movement_blob(),
        profession=player.profession, line_index=player.line_index,
        map_id=player.map_id)
    return P._enc.encode_object({0: character})


def encode_aoi_update_move(player: WorldPlayer) -> bytes:
    move = P.encode_character_aoi_move(player.char_id, player.movement_blob(),
                                       player.walk)
    return P._enc.encode_object({0: move})


def comb_value_for(db, char_id: int, level: int) -> int:
    """Real combat power: base attack + equipped weapon/badge power, scaled
    like the client's CombValue (atk + hp/10)."""
    attack = economy.PLAYER_BASE_ATTACK
    weapon = db.get_equipped(char_id, 0)
    if weapon is not None:
        attack += economy.ITEMS.get(weapon, {}).get("power", 0)
    badge = db.get_equipped(char_id, 2)
    if badge is not None:
        attack += economy.ITEMS.get(badge, {}).get("power", 0)
    hp = db.get_hp(char_id)[1]
    return attack * 10 + hp // 10 + level


def encode_main_player_create(player: WorldPlayer,
                              skills: list = None) -> dict:
    """Field dict for the main_player_create PUSH body:
    {character(0), movement(1)}.

    The client's NetLogic.ProcessPack decodes a push body DIRECTLY as
    main_player_create.request (GenRequest(tag, buffer, offset, len)), so
    the body must BE the request fields — NOT an extra object wrapping
    them. Wrapping the request in {0: request} makes the client decode the
    request blob itself as SprotoType.character, whose first field is a
    length dword that read_integer rejects:
    "Exception: read invalid integer size (362)" -> the packet handler dies
    and the loading screen hangs forever.

    character is a full SprotoType.character blob (see encode_character_blob);
    a partial one crashes the client's spawn handler and hangs the loading
    screen. `skills` is an optional [(skill_id, level)] list encoded as the
    client's map<string, skill_info> (string skillId key!).
    """
    character = encode_character_blob(
        player.char_id, player.name, player.level, player.movement_blob(),
        profession=player.profession, line_index=player.line_index,
        map_id=player.map_id, skills=skills,
        comb_value=getattr(player, "comb_value", 0))
    return {
        0: character,
        1: player.movement_blob(),
    }
