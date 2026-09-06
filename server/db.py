"""SQLite persistence for accounts and characters.

Accounts store the visitor id/key pair the client saves on first launch; the
key acts as the account password for `verfiy`. Characters keep a JSON blob of
the create-request fields plus live world state (position, level).
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
    map_id      TEXT NOT NULL DEFAULT '1',
    pos_x       INTEGER NOT NULL DEFAULT 0,
    pos_y       INTEGER NOT NULL DEFAULT 0,
    pos_z       INTEGER NOT NULL DEFAULT 0,
    pos_o       INTEGER NOT NULL DEFAULT 0,
    data        TEXT NOT NULL DEFAULT '{}',
    created_at  INTEGER NOT NULL,
    UNIQUE(name)
);
"""


class Database:
    def __init__(self, path: str = "atg_server.db") -> None:
        os.makedirs(os.path.dirname(path) or ".", exist_ok=True)
        self._conn = sqlite3.connect(path)
        self._conn.row_factory = sqlite3.Row
        self._conn.executescript(SCHEMA)
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

    def close(self) -> None:
        self._conn.close()
