"""Server configuration (environment-overridable)."""

import os

# --- ports -------------------------------------------------------------------
# The real client connects its login gate to a fixed port (LoginPort from the
# server list / patched APK) and its game connection to whatever
# `game_server.serverPort` the gate advertises in update_game_server / verfiy.
#
# Single-port mode (default): one listener serves both the gate and game
# protocol — the connection handler is identical (the dispatcher accepts every
# tag), and the gate advertises the same port it listens on. The original
# servers used 9777 (gate) + 9555 (game); set ATG_SINGLE_PORT=0 to restore
# two-port mode with the original defaults.
SINGLE_PORT = os.environ.get("ATG_SINGLE_PORT", "1") != "0"
SINGLE_PORT_NUM = int(os.environ.get("ATG_PORT", "13103"))

GATE_PORT = int(os.environ.get("ATG_GATE_PORT", "9777"))
GAME_PORT = int(os.environ.get("ATG_GAME_PORT", "9555"))
BIND_HOST = os.environ.get("ATG_BIND_HOST", "0.0.0.0")

# the port advertised to clients in the server list (defaults to the actual
# listener port in single-port mode)
ADVERTISE_PORT = int(os.environ.get(
    "ATG_ADVERTISE_PORT",
    str(SINGLE_PORT_NUM if SINGLE_PORT else GAME_PORT)))

DB_PATH = os.environ.get("ATG_DB_PATH", "atg_server.db")

GAME_VERSION = "1.012.017"
DATA_VERSION = "1.012.017"
UNITY_VERSION = "Unity4.7"
SERVER_ID = 1
SERVER_NAME = os.environ.get("ATG_SERVER_NAME", "Revival-1")

# IP advertised to clients for the game-server hop. The client uses whatever
# IP/hostname the APK points at for login by default; override for hosting
# behind a different public address.
ADVERTISE_IP = os.environ.get("ATG_ADVERTISE_IP", "78.154.103.21")
