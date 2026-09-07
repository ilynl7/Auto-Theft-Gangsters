"""SQLite persistence for accounts, characters, inventory, and missions.

Accounts store the visitor id/key pair the client saves on first launch; the
key acts as the account password for `verfiy`. Characters keep a JSON blob of
the create-request fields plus live world state (position, level).

Economy tables:
  * currency columns on `characters` (gold / diamond)
  * `items`      — backpack: one row per (character, item) stack
  * `missions`   — one row per accepted mission per character
"""

import json
import os
import secrets
import sqlite3
import time

SCHEMA = """
CREATE TABLE IF NOT EXISTS accounts (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    account_key TEXT NOT NULL,
    created_at  INTEGER NOT NULL,
    last_login  INTEGER
);

CREATE TABLE IF NOT EXISTS characters (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    account_id  INTEGER NOT NULL REFERENCES accounts(id),
    name        TEXT NOT NULL,
    level       INTEGER NOT NULL DEFAULT 1,
    sex         INTEGER NOT NULL DEFAULT 0,
    profession  INTEGER NOT NULL DEFAULT 0,   -- 0 Batfighter, 1 Boxer, 2 Gunner
    exp         INTEGER NOT NULL DEFAULT 0,
    hp          INTEGER NOT NULL DEFAULT 100,
    max_hp      INTEGER NOT NULL DEFAULT 100,
    gold        INTEGER NOT NULL DEFAULT 5000,
    car_id      INTEGER,
    using_car   INTEGER NOT NULL DEFAULT 0,
    diamond     INTEGER NOT NULL DEFAULT 20,
    -- map 1 is the client's LoadingScene placeholder; the real main city is 11
    map_id      TEXT NOT NULL DEFAULT '11',
    pos_x       INTEGER NOT NULL DEFAULT 0,
    pos_y       INTEGER NOT NULL DEFAULT 0,
    pos_z       INTEGER NOT NULL DEFAULT 0,
    pos_o       INTEGER NOT NULL DEFAULT 0,
    data        TEXT NOT NULL DEFAULT '{}',
    created_at  INTEGER NOT NULL,
    UNIQUE(name)
);

CREATE TABLE IF NOT EXISTS items (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    item_id     INTEGER NOT NULL,
    count       INTEGER NOT NULL DEFAULT 1,
    UNIQUE(char_id, item_id)
);

CREATE TABLE IF NOT EXISTS missions (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    mission_id  INTEGER NOT NULL,
    progress    INTEGER NOT NULL DEFAULT 0,
    state       INTEGER NOT NULL DEFAULT 0,   -- 0 active, 1 done (claimable), 2 finished
    accepted_at INTEGER NOT NULL,
    UNIQUE(char_id, mission_id)
);

CREATE TABLE IF NOT EXISTS equip_slots (
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    slot        INTEGER NOT NULL,             -- 0 weapon, 1 armor, 2 badge, 3 fashion
    item_id     INTEGER NOT NULL,
    UNIQUE(char_id, slot)
);

CREATE TABLE IF NOT EXISTS storagepack (
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    item_id     INTEGER NOT NULL,
    count       INTEGER NOT NULL DEFAULT 1,
    UNIQUE(char_id, item_id)
);

CREATE TABLE IF NOT EXISTS guilds (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    name        TEXT NOT NULL UNIQUE,
    leader      TEXT NOT NULL,
    notice      TEXT NOT NULL DEFAULT '',
    gold        INTEGER NOT NULL DEFAULT 0,
    level       INTEGER NOT NULL DEFAULT 1,
    created_at  INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS guild_members (
    guild_id    INTEGER NOT NULL REFERENCES guilds(id),
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    name        TEXT NOT NULL,
    job         INTEGER NOT NULL DEFAULT 2,   -- 0 leader, 1 officer, 2 member
    joined_at   INTEGER NOT NULL,
    UNIQUE(guild_id, char_id)
);

CREATE TABLE IF NOT EXISTS guild_requests (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    guild_id    INTEGER NOT NULL REFERENCES guilds(id),
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    name        TEXT NOT NULL,
    created_at  INTEGER NOT NULL,
    UNIQUE(guild_id, char_id)
);

CREATE TABLE IF NOT EXISTS guild_log (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    guild_id    INTEGER NOT NULL REFERENCES guilds(id),
    entry       TEXT NOT NULL,
    created_at  INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS friends (
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    friend_id   INTEGER NOT NULL REFERENCES characters(id),
    UNIQUE(char_id, friend_id)
);

CREATE TABLE IF NOT EXISTS mails (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    sender      TEXT NOT NULL,
    title       TEXT NOT NULL,
    body        TEXT NOT NULL DEFAULT '',
    gold        INTEGER NOT NULL DEFAULT 0,
    diamond     INTEGER NOT NULL DEFAULT 0,
    collected   INTEGER NOT NULL DEFAULT 0,
    created_at  INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS progress (
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    key         TEXT NOT NULL,
    value       INTEGER NOT NULL DEFAULT 0,
    UNIQUE(char_id, key)
);

CREATE TABLE IF NOT EXISTS skills (
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    skill_id    INTEGER NOT NULL,
    level       INTEGER NOT NULL DEFAULT 1,
    UNIQUE(char_id, skill_id)
);

CREATE TABLE IF NOT EXISTS pvp_history (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    blob        BLOB NOT NULL,
    created_at  INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS guild_battle (
    guild_id    INTEGER NOT NULL REFERENCES guilds(id),
    week        INTEGER NOT NULL,
    score       INTEGER NOT NULL DEFAULT 0,
    PRIMARY KEY (guild_id, week)
);

CREATE TABLE IF NOT EXISTS guild_battle_members (
    guild_id    INTEGER NOT NULL REFERENCES guilds(id),
    week        INTEGER NOT NULL,
    name        TEXT NOT NULL,
    job         INTEGER NOT NULL DEFAULT 2,
    PRIMARY KEY (guild_id, week, name)
);

CREATE TABLE IF NOT EXISTS guild_battle_guess (
    char_id     INTEGER NOT NULL REFERENCES characters(id),
    week        INTEGER NOT NULL,
    guild       TEXT NOT NULL,
    amount      INTEGER NOT NULL,
    settled     INTEGER NOT NULL DEFAULT 0,
    PRIMARY KEY (char_id, week)
);
"""


class Database:
    def __init__(self, path: str = "atg_server.db") -> None:
        os.makedirs(os.path.dirname(path) or ".", exist_ok=True)
        self._conn = sqlite3.connect(path)
        self._conn.row_factory = sqlite3.Row
        self._conn.executescript(SCHEMA)
        # migrate pre-economy databases: add the currency columns if missing
        cols = {r["name"] for r in self._conn.execute(
            "PRAGMA table_info(characters)")}
        if "gold" not in cols:
            self._conn.execute(
                "ALTER TABLE characters ADD COLUMN gold INTEGER NOT NULL DEFAULT 5000")
        if "diamond" not in cols:
            self._conn.execute(
                "ALTER TABLE characters ADD COLUMN diamond INTEGER NOT NULL DEFAULT 20")
        if "profession" not in cols:
            self._conn.execute(
                "ALTER TABLE characters ADD COLUMN profession INTEGER"
                " NOT NULL DEFAULT 0")
        # migrate legacy rows saved with map_id '1' — that id is the client's
        # LoadingScene placeholder and hangs the world-entry loader
        self._conn.execute(
            "UPDATE characters SET map_id = '11' WHERE map_id = '1'")
        self._conn.commit()

    # -- accounts -----------------------------------------------------
    def create_account(self):
        """Create a visitor account, returning (id, key)."""
        key = secrets.token_hex(16)
        now = int(time.time())
        cur = self._conn.execute(
            "INSERT INTO accounts (account_key, created_at) VALUES (?, ?)",
            (key, now),
        )
        self._conn.commit()
        return cur.lastrowid, key

    def verify_account(self, account_id: int, key: str):
        """Return the account row if id/key match, else None."""
        row = self._conn.execute(
            "SELECT * FROM accounts WHERE id = ? AND account_key = ?",
            (account_id, key),
        ).fetchone()
        if row is None:
            return None
        self._conn.execute(
            "UPDATE accounts SET last_login = ? WHERE id = ?",
            (int(time.time()), account_id),
        )
        self._conn.commit()
        return row

    # -- characters -----------------------------------------------------
    def list_characters(self, account_id: int):
        return self._conn.execute(
            "SELECT * FROM characters WHERE account_id = ? ORDER BY id",
            (account_id,),
        ).fetchall()

    def get_character(self, char_id: int):
        return self._conn.execute(
            "SELECT * FROM characters WHERE id = ?", (char_id,)
        ).fetchone()

    def create_character(self, account_id: int, name: str, sex: int = 0,
                         level: int = 1, profession: int = 0,
                         extra: dict = None):
        extra = extra or {}
        now = int(time.time())
        try:
            cur = self._conn.execute(
                "INSERT INTO characters (account_id, name, level, sex,"
                " profession, data, created_at)"
                " VALUES (?, ?, ?, ?, ?, ?, ?)",
                (account_id, name, level, sex, profession,
                 json.dumps(extra), now),
            )
            self._conn.commit()
        except sqlite3.IntegrityError:
            return None  # duplicate name
        return self.get_character(cur.lastrowid)

    def save_position(self, char_id: int, map_id: str, x: int, y: int,
                      z: int, o: int) -> None:
        self._conn.execute(
            "UPDATE characters SET map_id = ?, pos_x = ?, pos_y = ?, pos_z = ?,"
            " pos_o = ? WHERE id = ?",
            (map_id, x, y, z, o, char_id),
        )
        self._conn.commit()

    def save_level(self, char_id: int, level: int) -> None:
        self._conn.execute(
            "UPDATE characters SET level = ? WHERE id = ?", (level, char_id)
        )
        self._conn.commit()

    # -- currency -------------------------------------------------------
    def get_currency(self, char_id: int, currency: int) -> int:
        col = "gold" if currency == 1 else "diamond"
        row = self._conn.execute(
            "SELECT %s AS v FROM characters WHERE id = ?" % col, (char_id,)
        ).fetchone()
        return row["v"] if row else 0

    def add_currency(self, char_id: int, currency: int, amount: int) -> int:
        """Add (or spend, negative amount) currency; returns the new balance."""
        col = "gold" if currency == 1 else "diamond"
        bal = self.get_currency(char_id, currency)
        new_bal = max(0, bal + amount)
        self._conn.execute(
            "UPDATE characters SET %s = ? WHERE id = ?" % col, (new_bal, char_id)
        )
        self._conn.commit()
        return new_bal

    # -- items / backpack -----------------------------------------------
    def list_items(self, char_id: int):
        return self._conn.execute(
            "SELECT item_id, count FROM items WHERE char_id = ? ORDER BY item_id",
            (char_id,),
        ).fetchall()

    def get_item_count(self, char_id: int, item_id: int) -> int:
        row = self._conn.execute(
            "SELECT count FROM items WHERE char_id = ? AND item_id = ?",
            (char_id, item_id),
        ).fetchone()
        return row["count"] if row else 0

    def add_item(self, char_id: int, item_id: int, count: int) -> int:
        """Add `count` of `item_id` to the backpack; returns the new count."""
        new = max(0, self.get_item_count(char_id, item_id) + count)
        if new == 0:
            self._conn.execute(
                "DELETE FROM items WHERE char_id = ? AND item_id = ?",
                (char_id, item_id),
            )
        else:
            self._conn.execute(
                "INSERT INTO items (char_id, item_id, count) VALUES (?, ?, ?)"
                " ON CONFLICT(char_id, item_id) DO UPDATE SET count = ?",
                (char_id, item_id, new, new),
            )
        self._conn.commit()
        return new

    # -- missions ---------------------------------------------------------
    def list_missions(self, char_id: int):
        return self._conn.execute(
            "SELECT * FROM missions WHERE char_id = ? ORDER BY mission_id",
            (char_id,),
        ).fetchall()

    def get_mission(self, char_id: int, mission_id: int):
        return self._conn.execute(
            "SELECT * FROM missions WHERE char_id = ? AND mission_id = ?",
            (char_id, mission_id),
        ).fetchone()

    def accept_mission(self, char_id: int, mission_id: int) -> bool:
        try:
            self._conn.execute(
                "INSERT INTO missions (char_id, mission_id, accepted_at)"
                " VALUES (?, ?, ?)",
                (char_id, mission_id, int(time.time())),
            )
            self._conn.commit()
            return True
        except sqlite3.IntegrityError:
            return False  # already accepted

    def set_mission_progress(self, char_id: int, mission_id: int,
                             progress: int, state: int) -> None:
        self._conn.execute(
            "UPDATE missions SET progress = ?, state = ?"
            " WHERE char_id = ? AND mission_id = ?",
            (progress, state, char_id, mission_id),
        )
        self._conn.commit()

    def finish_mission(self, char_id: int, mission_id: int) -> None:
        self._conn.execute(
            "DELETE FROM missions WHERE char_id = ? AND mission_id = ?",
            (char_id, mission_id),
        )
        self._conn.commit()

    # -- exp / hp --------------------------------------------------------
    def add_exp(self, char_id: int, amount: int) -> int:
        """Add exp and level up (1000 exp per level); returns (level, exp)."""
        self._conn.execute(
            "UPDATE characters SET exp = exp + ? WHERE id = ?",
            (amount, char_id),
        )
        row = self._conn.execute(
            "SELECT level, exp FROM characters WHERE id = ?", (char_id,)
        ).fetchone()
        level, exp = row["level"], row["exp"]
        while exp >= 1000 * level:
            exp -= 1000 * level
            level += 1
            self._conn.execute(
                "UPDATE characters SET max_hp = max_hp + 10, hp = max_hp + 10"
                " WHERE id = ?", (char_id,))
        self._conn.execute(
            "UPDATE characters SET level = ?, exp = ? WHERE id = ?",
            (level, exp, char_id),
        )
        self._conn.commit()
        return level, exp

    def get_hp(self, char_id: int):
        row = self._conn.execute(
            "SELECT hp, max_hp FROM characters WHERE id = ?", (char_id,)
        ).fetchone()
        return (row["hp"], row["max_hp"]) if row else (0, 0)

    def set_hp(self, char_id: int, hp: int) -> int:
        row = self._conn.execute(
            "SELECT max_hp FROM characters WHERE id = ?", (char_id,)
        ).fetchone()
        max_hp = row["max_hp"] if row else 100
        hp = max(0, min(hp, max_hp))
        self._conn.execute(
            "UPDATE characters SET hp = ? WHERE id = ?", (hp, char_id))
        self._conn.commit()
        return hp

    # -- equipment / storage ---------------------------------------------
    def get_equipped(self, char_id: int, slot: int):
        row = self._conn.execute(
            "SELECT item_id FROM equip_slots WHERE char_id = ? AND slot = ?",
            (char_id, slot),
        ).fetchone()
        return row["item_id"] if row else None

    def set_equipped(self, char_id: int, slot: int, item_id) -> None:
        self._conn.execute(
            "DELETE FROM equip_slots WHERE char_id = ? AND slot = ?",
            (char_id, slot),
        )
        if item_id is not None:
            self._conn.execute(
                "INSERT INTO equip_slots (char_id, slot, item_id) VALUES (?, ?, ?)",
                (char_id, slot, item_id),
            )
        self._conn.commit()

    def list_equipped(self, char_id: int):
        return self._conn.execute(
            "SELECT slot, item_id FROM equip_slots WHERE char_id = ?", (char_id,)
        ).fetchall()

    def list_storage(self, char_id: int):
        return self._conn.execute(
            "SELECT item_id, count FROM storagepack WHERE char_id = ?"
            " ORDER BY item_id", (char_id,),
        ).fetchall()

    def get_storage_count(self, char_id: int, item_id: int) -> int:
        row = self._conn.execute(
            "SELECT count FROM storagepack WHERE char_id = ? AND item_id = ?",
            (char_id, item_id),
        ).fetchone()
        return row["count"] if row else 0

    def add_storage(self, char_id: int, item_id: int, count: int) -> int:
        new = max(0, self.get_storage_count(char_id, item_id) + count)
        if new == 0:
            self._conn.execute(
                "DELETE FROM storagepack WHERE char_id = ? AND item_id = ?",
                (char_id, item_id),
            )
        else:
            self._conn.execute(
                "INSERT INTO storagepack (char_id, item_id, count) VALUES (?, ?, ?)"
                " ON CONFLICT(char_id, item_id) DO UPDATE SET count = ?",
                (char_id, item_id, new, new),
            )
        self._conn.commit()
        return new

    # -- guilds ------------------------------------------------------------
    def create_guild(self, name: str, leader_name: str):
        try:
            cur = self._conn.execute(
                "INSERT INTO guilds (name, leader, created_at) VALUES (?, ?, ?)",
                (name, leader_name, int(time.time())),
            )
            self._conn.commit()
        except sqlite3.IntegrityError:
            return None
        return self.get_guild(cur.lastrowid)

    def get_guild(self, guild_id: int):
        return self._conn.execute(
            "SELECT * FROM guilds WHERE id = ?", (guild_id,)
        ).fetchone()

    def get_guild_by_member(self, char_id: int):
        row = self._conn.execute(
            "SELECT g.* FROM guilds g JOIN guild_members m ON m.guild_id = g.id"
            " WHERE m.char_id = ?", (char_id,),
        ).fetchone()
        return row

    def list_guilds(self):
        return self._conn.execute(
            "SELECT * FROM guilds ORDER BY id").fetchall()

    def search_guilds(self, pattern: str):
        return self._conn.execute(
            "SELECT * FROM guilds WHERE name LIKE ? ORDER BY id",
            ("%" + pattern + "%",),
        ).fetchall()

    def guild_member_count(self, guild_id: int) -> int:
        row = self._conn.execute(
            "SELECT COUNT(*) AS c FROM guild_members WHERE guild_id = ?",
            (guild_id,),
        ).fetchone()
        return row["c"] if row else 0

    def list_guild_members(self, guild_id: int):
        return self._conn.execute(
            "SELECT * FROM guild_members WHERE guild_id = ? ORDER BY job, name",
            (guild_id,),
        ).fetchall()

    def get_guild_member(self, guild_id: int, char_id: int):
        return self._conn.execute(
            "SELECT * FROM guild_members WHERE guild_id = ? AND char_id = ?",
            (guild_id, char_id),
        ).fetchone()

    def add_guild_member(self, guild_id: int, char_id: int, name: str,
                         job: int = 2) -> None:
        self._conn.execute(
            "INSERT OR REPLACE INTO guild_members (guild_id, char_id, name, job,"
            " joined_at) VALUES (?, ?, ?, ?, ?)",
            (guild_id, char_id, name, job, int(time.time())),
        )
        self._conn.commit()

    def remove_guild_member(self, guild_id: int, char_id: int) -> None:
        self._conn.execute(
            "DELETE FROM guild_members WHERE guild_id = ? AND char_id = ?",
            (guild_id, char_id),
        )
        self._conn.commit()

    def set_guild_job(self, guild_id: int, char_id: int, job: int) -> None:
        self._conn.execute(
            "UPDATE guild_members SET job = ? WHERE guild_id = ? AND char_id = ?",
            (job, guild_id, char_id),
        )
        self._conn.commit()

    def add_guild_gold(self, guild_id: int, amount: int) -> int:
        row = self._conn.execute(
            "SELECT gold FROM guilds WHERE id = ?", (guild_id,)
        ).fetchone()
        gold = max(0, (row["gold"] if row else 0) + amount)
        self._conn.execute(
            "UPDATE guilds SET gold = ? WHERE id = ?", (gold, guild_id))
        self._conn.commit()
        return gold

    def set_guild_notice(self, guild_id: int, notice: str) -> None:
        self._conn.execute(
            "UPDATE guilds SET notice = ? WHERE id = ?", (notice, guild_id))
        self._conn.commit()

    def add_guild_log(self, guild_id: int, entry: str) -> None:
        self._conn.execute(
            "INSERT INTO guild_log (guild_id, entry, created_at) VALUES (?, ?, ?)",
            (guild_id, entry, int(time.time())),
        )
        self._conn.commit()

    def list_guild_log(self, guild_id: int, limit: int = 20):
        return self._conn.execute(
            "SELECT entry, created_at FROM guild_log WHERE guild_id = ?"
            " ORDER BY id DESC LIMIT ?", (guild_id, limit),
        ).fetchall()

    def add_guild_request(self, guild_id: int, char_id: int, name: str) -> bool:
        try:
            self._conn.execute(
                "INSERT INTO guild_requests (guild_id, char_id, name, created_at)"
                " VALUES (?, ?, ?, ?)",
                (guild_id, char_id, name, int(time.time())),
            )
            self._conn.commit()
            return True
        except sqlite3.IntegrityError:
            return False

    def list_guild_requests(self, guild_id: int):
        return self._conn.execute(
            "SELECT * FROM guild_requests WHERE guild_id = ? ORDER BY id",
            (guild_id,),
        ).fetchall()

    def remove_guild_request(self, guild_id: int, char_id: int) -> None:
        self._conn.execute(
            "DELETE FROM guild_requests WHERE guild_id = ? AND char_id = ?",
            (guild_id, char_id),
        )
        self._conn.commit()

    # -- friends ------------------------------------------------------------
    def list_friends(self, char_id: int):
        return self._conn.execute(
            "SELECT friend_id FROM friends WHERE char_id = ? ORDER BY friend_id",
            (char_id,),
        ).fetchall()

    def add_friend(self, char_id: int, friend_id: int) -> None:
        self._conn.execute(
            "INSERT OR IGNORE INTO friends (char_id, friend_id) VALUES (?, ?)",
            (char_id, friend_id),
        )
        self._conn.commit()

    def remove_friend(self, char_id: int, friend_id: int) -> None:
        self._conn.execute(
            "DELETE FROM friends WHERE char_id = ? AND friend_id = ?",
            (char_id, friend_id),
        )
        self._conn.commit()

    # -- mail ---------------------------------------------------------------
    def list_mails(self, char_id: int):
        return self._conn.execute(
            "SELECT * FROM mails WHERE char_id = ? ORDER BY id DESC", (char_id,)
        ).fetchall()

    def get_mail(self, char_id: int, mail_id: int):
        return self._conn.execute(
            "SELECT * FROM mails WHERE char_id = ? AND id = ?",
            (char_id, mail_id),
        ).fetchone()

    def send_mail(self, char_id: int, sender: str, title: str, body: str,
                  gold: int = 0, diamond: int = 0) -> int:
        cur = self._conn.execute(
            "INSERT INTO mails (char_id, sender, title, body, gold, diamond,"
            " created_at) VALUES (?, ?, ?, ?, ?, ?, ?)",
            (char_id, sender, title, body, gold, diamond, int(time.time())),
        )
        self._conn.commit()
        return cur.lastrowid

    def set_mail_collected(self, mail_id: int) -> None:
        self._conn.execute(
            "UPDATE mails SET collected = 1 WHERE id = ?", (mail_id,))
        self._conn.commit()

    def delete_mail(self, char_id: int, mail_id: int) -> None:
        self._conn.execute(
            "DELETE FROM mails WHERE char_id = ? AND id = ?", (char_id, mail_id))
        self._conn.commit()

    # -- generic progress key/values (sign-in, daily, skills…) --------------
    def get_progress(self, char_id: int, key: str) -> int:
        row = self._conn.execute(
            "SELECT value FROM progress WHERE char_id = ? AND key = ?",
            (char_id, key),
        ).fetchone()
        return row["value"] if row else 0

    def set_progress(self, char_id: int, key: str, value: int) -> None:
        self._conn.execute(
            "INSERT INTO progress (char_id, key, value) VALUES (?, ?, ?)"
            " ON CONFLICT(char_id, key) DO UPDATE SET value = ?",
            (char_id, key, value, value),
        )
        self._conn.commit()

    # -- skills ---------------------------------------------------------------
    def list_skills(self, char_id: int):
        return self._conn.execute(
            "SELECT skill_id, level FROM skills WHERE char_id = ?"
            " ORDER BY skill_id", (char_id,),
        ).fetchall()

    def get_skill(self, char_id: int, skill_id: int):
        return self._conn.execute(
            "SELECT level FROM skills WHERE char_id = ? AND skill_id = ?",
            (char_id, skill_id),
        ).fetchone()

    def learn_skill(self, char_id: int, skill_id: int) -> int:
        row = self.get_skill(char_id, skill_id)
        level = (row["level"] if row else 0) + 1
        self._conn.execute(
            "INSERT INTO skills (char_id, skill_id, level) VALUES (?, ?, ?)"
            " ON CONFLICT(char_id, skill_id) DO UPDATE SET level = ?",
            (char_id, skill_id, level, level),
        )
        self._conn.commit()
        return level

    # -- pvp ladder -------------------------------------------------------------
    def append_pvp_history(self, char_id: int, blob: bytes) -> None:
        """Record a ladder battle (blob is the pre-encoded wire object)."""
        self._conn.execute(
            "INSERT INTO pvp_history (char_id, blob, created_at)"
            " VALUES (?, ?, ?)", (char_id, blob, int(time.time())))
        self._conn.commit()

    def list_pvp_history(self, char_id: int, limit: int = 20):
        return self._conn.execute(
            "SELECT blob FROM pvp_history WHERE char_id = ?"
            " ORDER BY id DESC LIMIT ?", (char_id, limit)
        ).fetchall()

    def top_pvp_scores(self, limit: int = 10):
        """Top ladder scores joined with character names."""
        progress = self._conn.execute(
            "SELECT char_id, value FROM progress WHERE key = 'pvp_score'"
            " ORDER BY value DESC LIMIT ?", (limit,)).fetchall()
        out = []
        for r in progress:
            row = self._conn.execute(
                "SELECT name FROM characters WHERE id = ?", (r["char_id"],)
            ).fetchone()
            if row is not None:
                out.append({"name": row["name"], "score": r["value"]})
        return out

    # -- guild battle (weekly guild-vs-guild war) ----------------------------
    def set_guild_battle_members(self, guild_id: int, week: int,
                                 names: list) -> None:
        self._conn.execute(
            "DELETE FROM guild_battle_members WHERE guild_id = ? AND week = ?",
            (guild_id, week))
        for i, name in enumerate(names):
            self._conn.execute(
                "INSERT OR IGNORE INTO guild_battle_members"
                " (guild_id, week, name, job) VALUES (?, ?, ?, ?)",
                (guild_id, week, name, 0 if i == 0 else 2))
        self._conn.commit()

    def list_guild_battle_members(self, guild_id: int, week: int):
        return self._conn.execute(
            "SELECT name, job FROM guild_battle_members"
            " WHERE guild_id = ? AND week = ? ORDER BY job, rowid",
            (guild_id, week)).fetchall()

    def add_guild_battle_score(self, guild_id: int, week: int,
                               amount: int) -> int:
        self._conn.execute(
            "INSERT INTO guild_battle (guild_id, week, score) VALUES (?, ?, ?)"
            " ON CONFLICT(guild_id, week)"
            " DO UPDATE SET score = score + ?",
            (guild_id, week, amount, amount))
        self._conn.commit()
        return self.get_guild_battle_score(guild_id, week)

    def get_guild_battle_score(self, guild_id: int, week: int) -> int:
        row = self._conn.execute(
            "SELECT score FROM guild_battle WHERE guild_id = ? AND week = ?",
            (guild_id, week)).fetchone()
        return row["score"] if row else 0

    def top_guild_battle_scores(self, week: int, limit: int = 10):
        rows = self._conn.execute(
            "SELECT guild_id, score FROM guild_battle WHERE week = ?"
            " ORDER BY score DESC LIMIT ?", (week, limit)).fetchall()
        out = []
        for r in rows:
            g = self._conn.execute(
                "SELECT name FROM guilds WHERE id = ?", (r["guild_id"],)
            ).fetchone()
            if g is not None:
                out.append({"guild_id": r["guild_id"], "name": g["name"],
                            "score": r["score"]})
        return out

    def set_guild_battle_guess(self, char_id: int, week: int,
                               guild: str, amount: int) -> None:
        self._conn.execute(
            "INSERT INTO guild_battle_guess (char_id, week, guild, amount)"
            " VALUES (?, ?, ?, ?)"
            " ON CONFLICT(char_id, week)"
            " DO UPDATE SET guild = ?, amount = ?, settled = 0",
            (char_id, week, guild, amount, guild, amount))
        self._conn.commit()

    def get_guild_battle_guess(self, char_id: int, week: int):
        return self._conn.execute(
            "SELECT guild, amount, settled FROM guild_battle_guess"
            " WHERE char_id = ? AND week = ?", (char_id, week)).fetchone()

    def get_flag(self, char_id: int, key: str) -> int:
        return self.get_progress(char_id, key)

    def set_flag(self, char_id: int, key: str, value: int) -> None:
        self.set_progress(char_id, key, value)

    def top_survive_scores(self, limit: int = 10):
        """Best survive waves joined with character names."""
        progress = self._conn.execute(
            "SELECT char_id, value FROM progress WHERE key = 'survive_best'"
            " ORDER BY value DESC LIMIT ?", (limit,)).fetchall()
        out = []
        for r in progress:
            row = self._conn.execute(
                "SELECT name FROM characters WHERE id = ?", (r["char_id"],)
            ).fetchone()
            if row is not None:
                out.append({"name": row["name"], "best": r["value"]})
        return out

    # -- cars / mounts ----------------------------------------------------------
    def list_cars(self, char_id: int):
        row = self._conn.execute(
            "SELECT car_id, using_car FROM characters WHERE id = ?", (char_id,)
        ).fetchone()
        return [row] if row and row["car_id"] is not None else []

    def buy_car(self, char_id: int, car_id: int) -> None:
        self._conn.execute(
            "UPDATE characters SET car_id = ? WHERE id = ?", (car_id, char_id))
        self._conn.commit()

    def set_using_car(self, char_id: int, car_id) -> None:
        self._conn.execute(
            "UPDATE characters SET car_id = ?, using_car = ? WHERE id = ?",
            (car_id, car_id if car_id is not None else 0, char_id))
        self._conn.commit()

    def close(self) -> None:
        self._conn.close()
