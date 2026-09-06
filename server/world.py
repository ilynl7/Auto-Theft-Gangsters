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
    __slots__ = ("conn", "char_id", "name", "level", "sex", "map_id",
                 "line_index", "pos", "moving", "walk", "dead")

    def __init__(self, conn, char_id: int, name: str, level: int = 1,
                 sex: int = 0) -> None:
        self.conn = conn
        self.char_id = char_id
        self.name = name
        self.level = level
        self.sex = sex
        self.map_id = "1"
        self.line_index = 0
        self.pos = {"x": 0, "y": 0, "z": 0, "o": 0}
        self.moving = False
        self.walk = True
        self.dead = False

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


# The decompiled client's SprotoType.character wire tags (decode() switch in
# SprotoType.character.cs). NOTE: the has_field bitset indices in that class
# are NOT the wire tags — property/visual/movement serialize at tags 5/6/7.
CHARACTER_TAG_GENERAL = 1
CHARACTER_TAG_MOVEMENT = 7


def encode_character_blob(char_id: int, name: str, level: int, sex: int,
                          pos_blob: bytes) -> bytes:
    """Minimal SprotoType.character blob the client can parse.

    Tags used: 0 id, 1 general{name(0),level(1),sex(2)}, 7 movement{pos(0)}.
    The client reads movement.pos to spawn/position the object — a wrong tag
    makes the main player never appear and the loading widget hang forever.
    """
    general = P._enc.encode_object({0: name, 1: level, 2: sex})
    return P._enc.encode_object({
        0: char_id,
        1: general,
        CHARACTER_TAG_MOVEMENT: pos_blob,
    })


def encode_aoi_add(player: WorldPlayer) -> bytes:
    """SprotoType.aoi_add.request {character(0)} with a minimal character blob.

    The client tolerates missing fields; it fills defaults for anything
    absent — but movement must be at the real wire tag (7).
    """
    character = encode_character_blob(player.char_id, player.name,
                                      player.level, player.sex,
                                      player.movement_blob())
    return P._enc.encode_object({0: character})


def encode_aoi_update_move(player: WorldPlayer) -> bytes:
    move = P.encode_character_aoi_move(player.char_id, player.movement_blob(),
                                       player.walk)
    return P._enc.encode_object({0: move})


def encode_main_player_create(player: WorldPlayer) -> bytes:
    """main_player_create.request {character(0), movement(1)}.

    character.movement must be at wire tag 7 (see encode_character_blob) or
    the client fails to spawn the main player and hangs on the loading screen.
    """
    character = encode_character_blob(player.char_id, player.name,
                                      player.level, player.sex,
                                      player.movement_blob())
    return P._enc.encode_object({
        0: character,
        1: player.movement_blob(),
    })
