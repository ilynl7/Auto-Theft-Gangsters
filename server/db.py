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
    gold        INTEGER NOT NULL DEFAULT 5000,
    diamond     INTEGER NOT NULL DEFAULT 20,
    map_id      TEXT NOT NULL DEFAULT '1',
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
                         level: int = 1, extra: dict = None):
        extra = extra or {}
        now = int(time.time())
        try:
            cur = self._conn.execute(
                "INSERT INTO characters (account_id, name, level, sex, data, created_at)"
                " VALUES (?, ?, ?, ?, ?, ?)",
                (account_id, name, level, sex, json.dumps(extra), now),
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

    def close(self) -> None:
        self._conn.close()
