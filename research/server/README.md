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
- **Missions**: `accept_mission` (112) / `ret_accept_mission` (520),
  `complete_mission` (113) / `ret_complete_mission` (521),
  `abandon_mission` (114) / `ret_abandon_mission` (522), with state pushed
  via `sync_mission` (519) and rewards via `show_reward_items_tips` (638).
- **Shop**: `ask_shop_list` (143) / `ret_ask_shop_list` (554),
  `buy_shop_item` (144) / `ret_buy_shop_item` (652).
- **Items**: `use_item` (115) / `ret_use_item` (526), `sell_item` (129),
  backpack sync via `sync_backpack_item` (592) and `update_item` (525).

### Economy content and provisional schemas

The mission/shop *tags* were recovered from the client, but the decompiled
repo preserved no `SprotoType` classes for them, so their field layouts are
**provisional** (documented in `server/protocol.py` `REQUEST_SPECS` /
`RESPONSE_SPECS`):

- `accept_mission.request {mission_id(0)}` → `{errno(0)}`
  (0 ok, 1 no character, 2 unknown mission, 3 already active)
- `complete_mission.request {mission_id(0)}` → `{errno(0)}`
  (0 ok, 1 no character, 2 not accepted, 3 objectives not met)
- `abandon_mission.request {mission_id(0)}` → `{errno(0)}`
- `use_item.request {item_id(0), count(1)}` → `{errno(0)}` (2 = not owned)
- `sell_item.request {item_id(0), count(1)}` → `{errno(0)}`
- `ask_shop_list.request {shop_id(0)}` → `{errno(0), goods(1)}` where each
  good is `{goods_id(0), item_id(1), count(2), currency(3), price(4)}`
- `buy_shop_item.request {goods_id(0), count(1)}` → `{errno(0)}`
  (3 = insufficient currency)
- `sync_mission` push: `{missions(0)}` = array of
  `{mission_id(0), progress(1), state(2)}` (0 active, 1 claimable)
- `sync_backpack_item` push: `{items(0)}` = array of `{item_id(0), count(1)}`
- `show_reward_items_tips` push: `{gold(0), diamond(1), items(2)}`

Game content (shops, missions, item catalog) lives in `server/economy.py` —
small default tables until `Bundle/Data/Data.bundle` is fully parsed; the
handlers are fully data-driven, so new content only requires extending those
tables. Mission types: `buy` (progresses on shop purchases of the target
item) and `visit` (progresses on entering the target map); missions can chain
via the `next` field and pay gold/diamond/item rewards.

## Architecture

```
server/
├── main.py      asyncio entry point: gate + game listeners, dispatcher
├── session.py   per-connection state + frame pump
├── handlers.py  protocol logic (login flow, world, chat, missions, shop)
├── world.py     per-map player registry, AOI broadcast helpers
├── economy.py   shop catalog + mission definitions (content tables)
├── db.py        SQLite accounts, characters, backpack, missions persistence
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
- `tests/test_economy.py` — end-to-end mission/shop flow: shop list +
  purchase (currency debit, backpack credit), mission accept → auto-complete
  on purchase → claim reward → chained mission → abandon, visit missions via
  map movement, and item consumption.

## What works with a real client

- Account creation (visitor) and verification — matches the client's saved
  `id`/`key` flow.
- Server list and login — the client proceeds to character select.
- Character create/pick and entering the map — the client loads into the
  world and other players see each other (`aoi_add`/movement/`aoi_remove`).
- World chat and heartbeat.

Character persistence (position, level, name), currency (gold/diamond),
backpack and mission state are stored in SQLite and restored on re-entry.
Rides/PvP and the remaining ~370 protocol tags are not yet implemented;
unknown tags are logged, not crashed on.

## Wire protocol summary (recovered from Assembly-CSharp.dll)

- Transport: raw TCP.
- Frame: `[u16be len][sproto-pack(payload)]`.
- payload = `sproto(Package{type?,session?})` + `sproto(body)`.
- Sproto is the standard compact binary serialization (Skynet-style).

Full tag table: `research/notes/protocol_tags.json` (409 entries).
Full protocol notes: `docs/protocol.md`.
