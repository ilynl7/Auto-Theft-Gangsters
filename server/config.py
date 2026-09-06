"""Server configuration (environment-overridable)."""

import os

GATE_PORT = int(os.environ.get("ATG_GATE_PORT", "9777"))
GAME_PORT = int(os.environ.get("ATG_GAME_PORT", "9555"))
BIND_HOST = os.environ.get("ATG_BIND_HOST", "0.0.0.0")

DB_PATH = os.environ.get("ATG_DB_PATH", "atg_server.db")

GAME_VERSION = "1.012.017"
DATA_VERSION = "1.012.017"
UNITY_VERSION = "Unity4.7"
SERVER_ID = 1
SERVER_NAME = os.environ.get("ATG_SERVER_NAME", "Revival-1")
