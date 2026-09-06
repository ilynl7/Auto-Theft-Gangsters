"""Guild battle — the weekly guild-vs-guild war, from the APK's real data.

The client's flow (tags from Protocol.cs; schedule from GuildBattleData
row 1501 in Data.bundle):

    req_guild_battle_info (285)    -> ret (662): next round time, signups
    set_guild_battle_member (289)  -> ret (666): leader picks the fighters
    req_guild_battle_member (288)  -> ret (665): the signed-up fighters
    guild_battle_guess (290)       -> ret (669): bet gold bars on a guild
    req_guild_battle_rank (287)    -> ret (663): guild score ranking
    req_guild_score_info (291)     -> ret (667): my guild's score/rank
    req_guild_battle_state (295)   -> ret (673): current round state
    enter_guild_battle (286)       -> ret (677): joins the battle scene
    server push: guild_battle_start (670) when the round opens,
    guild_battle_finish_info (668) with results when the round ends.

The schedule is the real one from the client data: three weekly rounds on
weekdays 5/6/7 (encoded weekday*10000 + seconds-of-day), 1800 s per battle,
10 fighters per guild, gold-bar betting that pays 2.5x on a correct guess.
Scores accumulate across the week; the winner is the guild with the most
kills in the round. Battles between the single live guild resolve as
win-by-default with score for participating.
"""

import logging
import time

from . import economy
from . import game_data
from . import protocol as P
from . import sproto
from .session import Session

log = logging.getLogger("atg.handlers.guildbattle")

GB = game_data.GUILD_BATTLE


def _week_index(now: int) -> int:
    """Monday-based week number for the schedule keys."""
    return int(now) // 604800


def _client_wday(now: int) -> int:
    """Client weekday 1..7 = Mon..Sun for a unix timestamp."""
    return ((int(now // 86400) + 3) % 7) + 1


def _current_round(now: int) -> int:
    """The round index (0..2) whose window contains `now`, else -1."""
    wday = _client_wday(now)
    secs = now % 86400
    for i, rnd in enumerate(GB["rounds"]):
        if (rnd["weekday"] == wday
                and rnd["start"] <= secs < rnd["start"] + GB["duration"]):
            return i
    return -1


def _next_round_time(now: int) -> int:
    """Unix timestamp of the next round window opening."""
    day0 = int(now // 86400)
    for delta in range(8):
        for rnd in GB["rounds"]:
            ts = (day0 + delta) * 86400 + rnd["start"]
            if ts > now and _client_wday(ts) == rnd["weekday"]:
                return ts
    return 0


class GuildBattleHandlersMixin:
    """Mixin added onto Handlers; uses self.server, self._require_guild."""

    # ------------------------------------------------------------------
    # info / state
    # ------------------------------------------------------------------
    async def h_req_guild_battle_info(self, s: Session, msg) -> None:
        row, g = self._require_guild(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        now = int(time.time())
        week = _week_index(now)
        if g is None:
            s.respond(msg, {0: 2})
            return
        members = self.server.db.list_guild_battle_members(
            g["id"], week)
        s.respond(msg, {
            0: 0,
            1: _next_round_time(now),
            2: GB["duration"],
            3: GB["max_members"],
            4: sproto.encode_object_array([
                sproto.encode_object({0: m["name"], 1: m["job"]})
                for m in members]),
        })

    async def h_req_guild_battle_state(self, s: Session, msg) -> None:
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        rnd = _current_round(int(time.time()))
        s.respond(msg, {0: 1 if rnd >= 0 else 0, 1: max(0, rnd)})

    # ------------------------------------------------------------------
    # signup (leader/officer sets the fighter list)
    # ------------------------------------------------------------------
    async def h_set_guild_battle_member(self, s: Session, msg) -> None:
        db = self.server.db
        row, g = self._require_guild(s)
        if row is None or g is None:
            s.respond(msg, {0: 1})
            return
        me = db.get_guild_member(g["id"], row["id"])
        if me is None or me["job"] not in (0, 1):
            s.respond(msg, {0: 2})   # only leader/officer
            return
        names = msg.body.get(0, [])
        if isinstance(names, str):
            names = [names]
        if isinstance(names, (bytes, bytearray)):
            names = [sproto.as_str(names)]
        if not isinstance(names, list):
            s.respond(msg, {0: 3})
            return
        if len(names) > GB["max_members"]:
            s.respond(msg, {0: 4})   # too many fighters
            return
        week = _week_index(int(time.time()))
        db.set_guild_battle_members(g["id"], week, [str(n) for n in names])
        s.respond(msg, {0: 0})

    async def h_req_guild_battle_member(self, s: Session, msg) -> None:
        row, g = self._require_guild(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        if g is None:
            s.respond(msg, {0: 2})
            return
        week = _week_index(int(time.time()))
        members = self.server.db.list_guild_battle_members(g["id"], week)
        s.respond(msg, {0: sproto.encode_object_array([
            sproto.encode_object({0: m["name"]}) for m in members])})

    # ------------------------------------------------------------------
    # betting (gold bars on which guild wins the round)
    # ------------------------------------------------------------------
    async def h_guild_battle_guess(self, s: Session, msg) -> None:
        db = self.server.db
        row, g = self._require_guild(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        guild_name = msg.body.get(0, "")
        amount = msg.body.get(1, 0)
        if amount <= 0 or amount > db.get_currency(row["id"],
                                                   economy.CURRENCY_GOLD):
            s.respond(msg, {0: 2})   # invalid stake
            return
        week = _week_index(int(time.time()))
        db.set_guild_battle_guess(row["id"], week, str(guild_name), amount)
        db.add_currency(row["id"], economy.CURRENCY_GOLD, -amount)
        s.respond(msg, {1: 0})   # ret_guild_battle_guess {state(1)}

    async def h_req_guild_battle_guess(self, s: Session, msg) -> None:
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        week = _week_index(int(time.time()))
        guess = self.server.db.get_guild_battle_guess(row["id"], week)
        s.respond(msg, {0: guess["guild"] if guess else "",
                        1: guess["amount"] if guess else 0})

    # ------------------------------------------------------------------
    # ranking / scores
    # ------------------------------------------------------------------
    async def h_req_guild_battle_rank(self, s: Session, msg) -> None:
        db = self.server.db
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        week = _week_index(int(time.time()))
        ranked = db.top_guild_battle_scores(week)
        s.respond(msg, {0: sproto.encode_object_array([
            sproto.encode_object({0: r["name"], 1: r["score"]})
            for r in ranked])})

    async def h_req_guild_score_info(self, s: Session, msg) -> None:
        db = self.server.db
        row, g = self._require_guild(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        if g is None:
            s.respond(msg, {0: 2, 1: 0})
            return
        week = _week_index(int(time.time()))
        score = db.get_guild_battle_score(g["id"], week)
        rank = next((i + 1 for i, r in
                     enumerate(db.top_guild_battle_scores(week))
                     if r["guild_id"] == g["id"]), 0)
        s.respond(msg, {0: score, 1: rank})

    # ------------------------------------------------------------------
    # entering the battle
    # ------------------------------------------------------------------
    async def h_enter_guild_battle(self, s: Session, msg) -> None:
        db = self.server.db
        row, g = self._require_guild(s)
        if row is None or g is None:
            s.respond(msg, {0: 1})
            return
        now = int(time.time())
        rnd = _current_round(now)
        if rnd < 0:
            s.respond(msg, {0: 2})   # no battle running
            return
        week = _week_index(now)
        # score for joining the war: 100 per entry, only once per round
        key = "gb_%d_%d" % (week, rnd)
        if not db.get_flag(row["id"], key):
            db.set_flag(row["id"], key, 1)
            db.add_guild_battle_score(g["id"], week, 100)
        s.respond(msg, {0: 0})
        # broadcast the battle-open push to the guild
        for m in db.list_guild_members(g["id"]):
            other = self.server.world.get_player_anywhere(m["char_id"])
            if other is not None:
                other.conn.push(P.GUILD_BATTLE_START, {0: rnd})
        log.info("char %s entered guild battle round %d (guild %s)",
                 row["name"], rnd, g["name"])
