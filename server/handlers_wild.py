"""Wild boss ("raid boss") and survive (battle-royale survival) handlers.

Implemented strictly from the decompiled APK's real tag flow
(research/notes/protocol_tags.json + Protocol.cs):

Wild boss (open-world raid boss with a shared HP pool):
    request_wild_boss_info (200)  -> ret_request_wild_boss_info (605)
    enter_wild_boss (201)         -> joins the fight; boss hp/attack pushed
    (boss damage is reported through the standard combat tags:
    attack_local_npc / local_npc_die against the boss npc id)

Survive (survival battle with a timed assault; finish report pays out):
    request_survive_top (245)     -> ret_request_survive_top (636): leaderboard
    enter_survive_batttle (246)   -> spawns the survive npc wave + countdown
    survive_battle_finish (637)   -> client-confirmed run end; pays rewards
    (top list stored per character in progress: survive_best)

Field layouts for these messages have no surviving SprotoType classes in the
decompiled dump, so the bodies follow the same conventions used by the rest
of this server (documented as provisional in research/server/README.md).
"""

import logging
import time

from . import economy
from . import protocol as P
from . import sproto
from . import world as W
from .session import Session

log = logging.getLogger("atg.handlers.wild")


class WildHandlersMixin:
    """Mixin added onto Handlers; uses self.server, self._require_char."""

    # ------------------------------------------------------------------
    # wild boss (raid boss)
    # ------------------------------------------------------------------
    def _boss_npc(self, map_id: str):
        """The live wild boss for a map, spawned on demand."""
        boss = self.server.world.bosses.get(map_id)
        if boss is None or boss.dead:
            kind = economy.WILD_BOSS["kind"]
            boss = W.WorldNpc(kind, map_id, dict(economy.WILD_BOSS["spawn"]))
            self.server.world.npcs.setdefault(map_id, {})[boss.npc_id] = boss
            self.server.world.bosses[map_id] = boss
        return boss

    async def h_request_wild_boss_info(self, s: Session, msg) -> None:
        """request_wild_boss_info -> boss name, level, hp, max hp, alive."""
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        map_id = s.world_player.map_id if s.world_player \
            else economy.MAIN_CITY_MAP
        boss = self._boss_npc(map_id)
        kind_def = economy.NPC_KINDS[economy.WILD_BOSS["kind"]]
        s.respond(msg, {
            0: kind_def["name"],
            1: boss.level,
            2: boss.hp,
            3: boss.max_hp,
            4: 0 if boss.dead else 1,
        })

    async def h_enter_wild_boss(self, s: Session, msg) -> None:
        """enter_wild_boss -> sync the boss state to the entering player."""
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        if s.world_player is None:
            s.respond(msg, {0: 2})
            return
        map_id = s.world_player.map_id
        boss = self._boss_npc(map_id)
        kind_def = economy.NPC_KINDS[economy.WILD_BOSS["kind"]]
        s.respond(msg, {0: 0})
        s.push(P.NPC_CREATE, {0: boss.blob()})
        s.push(P.COUNT_DOWN, {0: economy.WILD_BOSS["countdown"]})
        log.info("char %s entered wild boss %s (hp %d/%d)",
                 row["name"], kind_def["name"], boss.hp, boss.max_hp)

    # ------------------------------------------------------------------
    # survive (survival assault mode)
    # ------------------------------------------------------------------
    async def h_request_survive_top(self, s: Session, msg) -> None:
        """request_survive_top -> ranked best-wave list (ret tag 636)."""
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        entries = self.server.db.top_survive_scores()
        blobs = [sproto.encode_object(
            {0: e["name"], 1: e["best"]}) for e in entries]
        s.respond(msg, {0: sproto.encode_object_array(blobs)})

    async def h_enter_survive_battle(self, s: Session, msg) -> None:
        """enter_survive_batttle {wave(0)} -> spawn the wave + countdown.

        Survive reuses the copy-scene wave machinery: endless escalating
        waves of NPCs; the client reports kills with
        single_copy_scene_npc_die; survive_battle_finish ends the run.
        """
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        wave = msg.body.get(0, 0)
        # wave kinds escalate: wave 0 -> thugs, deeper waves add enforcers
        kinds = [1 + min(1, wave // 2) for _ in range(2 + wave)]
        s.copy_scene = {
            "copy_id": -1,                      # -1 marks a survive run
            "survive_wave": wave,
            "wave": 0,
            "waves": [kinds],                   # single live wave
            "reward": economy.SURVIVE["reward"],
            "npc_ids": [],
        }
        s.respond(msg, {0: 0})
        await self._push_copy_wave(s)
        s.push(P.COUNT_DOWN, {0: economy.SURVIVE["countdown"]})
        log.info("char %s entered survive battle wave %d",
                 row["name"], wave)

    async def h_survive_battle_finish(self, s: Session, msg) -> None:
        """survive_battle_finish {wave(0), result(1)} -> pay + record best."""
        db = self.server.db
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        char_id = row["id"]
        wave = msg.body.get(0, 0)
        s.copy_scene = None
        best = db.get_progress(char_id, "survive_best")
        if wave > best:
            db.set_progress(char_id, "survive_best", wave)
            best = wave
        reward = {
            "gold": economy.SURVIVE["reward"]["gold"] * max(1, wave),
            "exp": economy.SURVIVE["reward"]["exp"] * max(1, wave),
        }
        db.add_currency(char_id, economy.CURRENCY_GOLD, reward["gold"])
        db.add_exp(char_id, reward["exp"])
        s.respond(msg, {0: 0, 1: best, 2: reward["gold"]})
        s.push(P.SHOW_REWARD_ITEMS_TIPS,
               {0: reward["gold"], 1: 0, 2: sproto.encode_object_array([])})
        log.info("char %s survive finish wave %d (best %d)",
                 row["name"], wave, best)
