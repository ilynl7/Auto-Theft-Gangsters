"""World state: per-map player registry with AOI broadcast helpers.

The original game used interest-management pushes (aoi_add / aoi_remove /
aoi_update_move ...). This revival keeps it simple: every player in the same
map + line sees every other player in that map + line, and movement/chat are
broadcast to the map.
"""

from . import protocol as P


class WorldPlayer:
    __slots__ = ("conn", "char_id", "name", "level", "sex", "map_id",
                 "line_index", "pos", "moving", "walk")

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

    def movement_blob(self) -> bytes:
        return P._enc.encode_object({
            0: P.encode_position(**self.pos),
        })


class World:
    def __init__(self) -> None:
        # map_id -> {char_id: WorldPlayer}
        self.maps: dict = {}

    # -- membership -----------------------------------------------------
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
                       {"character": _encode_aoi_remove(player.char_id)},
                       exclude=player.char_id)

    def others(self, map_id: str, exclude_char_id: int):
        return [p for p in self.maps.get(map_id, {}).values()
                if p.char_id != exclude_char_id]

    def get(self, map_id: str, char_id: int):
        return self.maps.get(map_id, {}).get(char_id)

    # -- messaging ------------------------------------------------------
    def broadcast(self, map_id: str, tag: int, body: dict,
                  exclude: int = None, session: int = None) -> None:
        frame = P.encode_frame(tag, session, body)
        for p in self.maps.get(map_id, {}).values():
            if exclude is not None and p.char_id == exclude:
                continue
            p.conn.send_raw(frame)


# --- wire blob helpers shared by handlers -----------------------------------

def _encode_aoi_remove(char_id: int) -> bytes:
    # SprotoType.aoi_remove.request {characterId(0)}
    return P._enc.encode_object({0: char_id})


def encode_aoi_add(player: WorldPlayer) -> bytes:
    """SprotoType.aoi_add.request {character(0)} with a minimal character blob.

    character tags used: 0 id, 1 general{name(0),level(1),sex(2)...},
    5 movement{pos(0)}.  The client tolerates missing fields; it fills
    defaults for anything absent.
    """
    general = P._enc.encode_object({0: player.name, 1: player.level, 2: player.sex})
    character = P._enc.encode_object({
        0: player.char_id,
        1: general,
        5: player.movement_blob(),
    })
    return P._enc.encode_object({0: character})


def encode_aoi_update_move(player: WorldPlayer) -> bytes:
    move = P.encode_character_aoi_move(player.char_id, player.movement_blob(),
                                       player.walk)
    return P._enc.encode_object({0: move})


def encode_main_player_create(player: WorldPlayer) -> bytes:
    """main_player_create.request {character(0), movement(1)}."""
    general = P._enc.encode_object({0: player.name, 1: player.level, 2: player.sex})
    character = P._enc.encode_object({
        0: player.char_id,
        1: general,
        5: player.movement_blob(),
    })
    return P._enc.encode_object({
        0: character,
        1: player.movement_blob(),
    })
