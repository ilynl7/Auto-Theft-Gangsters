# Revival Server

A working, protocol-compatible replacement backend for **Auto Theft Gangsters
v1.19** (`com.doodlemobile.vicecity`), implemented in Python 3.10+ with no
third-party dependencies. Reverse-engineered from the client's
`Assembly-CSharp.dll` (see `docs/protocol.md` for the full protocol spec).

## Quick start

```bash
python3 -m server.main
```

The server listens on the same endpoints the original client expects:

| Endpoint   | Port | Purpose                                   |
|------------|------|-------------------------------------------|
| Login gate | 9777 | `update_game_server`, `visitor`, `verfiy` |
| Game server| 9555 | `login`, characters, world/AOI, chat      |

## Pointing the APK at the server

The client hardcodes `gangsterlogin.galaxyaura.com:9777`. Options:

1. **Repack the APK** — edit the server host string in
   `assets/bin/Data/…` / smali constants (`gangsterlogin.galaxyaura.com`)
   to your server IP, re-sign, install.
2. **DNS override** — on a rooted device or via ADB, redirect
   `gangsterlogin.galaxyaura.com` to your server IP (hosts file or VPN DNS),
   keeping port 9777.
3. **Local VPN app** — e.g. an on-device redirector mapping the original
   host:port to your server.

The game server address is returned by the gate in
`update_game_server`/`verfiy` responses, so only the login host needs
redirecting. The advertised game-server IP can be set with
`ATG_ADVERTISE_IP` (defaults to the machine's primary address).

## Configuration (environment variables)

| Variable          | Default        | Meaning                          |
|-------------------|----------------|----------------------------------|
| `ATG_GATE_PORT`   | `9777`         | Login gate port                  |
| `ATG_GAME_PORT`   | `9555`         | Game server port                 |
| `ATG_BIND_HOST`   | `0.0.0.0`      | Listen address                   |
| `ATG_DB_PATH`     | `atg_server.db`| SQLite database file             |
| `ATG_SERVER_NAME` | `Revival-1`    | Server name shown in server list |
| `ATG_ADVERTISE_IP`| (local IP)     | IP advertised in server list     |

## Implemented protocol

Tags handled (see `server/protocol.py`):

- **Gate**: `update_game_server` (7), `visitor` (2), `verfiy` (3)
- **Game**: `login` (4), `character_list` (103), `character_create` (104),
  `character_pick` (105), `map_ready` (100), `enter_map` (503),
  `move` (101, broadcast as `aoi_update_move` 507 / `aoi_add` 505 /
  `aoi_remove` 506), `chat` (120, echoed as `ret_chat` 528),
  `heart_beat` (218), `leave_game` (234), plus no-op acknowledgements for
  `refresh_online_state`, `update_client_state`, `game_check` and
  `retrieve_account` (account recovery is not supported).

## Architecture

```
server/
├── main.py      asyncio entry point: gate + game listeners, dispatcher
├── session.py   per-connection state + frame pump
├── handlers.py  protocol logic (login flow, world, chat)
├── world.py     per-map player registry, AOI broadcast helpers
├── db.py        SQLite accounts + characters persistence
├── protocol.py  tag constants, typed request/response schemas
├── sproto.py    SprotoPack compression + sproto binary codec + framing
└── config.py    environment-based configuration
```

## Tests

```bash
python3 -m pip install pytest pytest-asyncio
python3 -m pytest tests/ -q
```

- `tests/test_sproto.py` — codec unit tests (pack/unpack round-trips,
  object encode/decode, frame framing).
- `tests/test_e2e.py` — drives the real server over TCP through the full
  client login flow: server list → visitor → verify → login → character
  create/pick → enter map → heartbeat → movement broadcast between two
  players.

## What works with a real client

- Account creation (visitor) and verification — matches the client's saved
  `id`/`key` flow.
- Server list and login — the client proceeds to character select.
- Character create/pick and entering the map — the client loads into the
  world and other players see each other (`aoi_add`/movement/`aoi_remove`).
- World chat and heartbeat.

Character persistence (position, level, name) is stored in SQLite and
restored on re-entry. Rides/missions/shops/PvP and the remaining ~380
protocol tags are not yet implemented; unknown tags are logged, not
crashed on.

## Wire protocol summary (recovered from Assembly-CSharp.dll)

- Transport: raw TCP.
- Frame: `[u16be len][sproto-pack(payload)]`.
- payload = `sproto(Package{type?,session?})` + `sproto(body)`.
- Sproto is the standard compact binary serialization (Skynet-style).

Full tag table: `research/notes/protocol_tags.json` (409 entries).
Full protocol notes: `docs/protocol.md`.
