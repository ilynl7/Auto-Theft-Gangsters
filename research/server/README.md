# Revival Server

Reverse-engineering output for Auto Theft Gangsters v1.19.

## Wire protocol summary (recovered from Assembly-CSharp.dll)

- Transport: raw TCP.
- Login gate: `gangsterlogin.galaxyaura.com:9777` (now dead NXDOMAIN).
- Game server default port: `9555`.
- Frame: `[u16be len][sproto-pack(payload)]`.
- payload = `sproto(Package{type?,session?})` + `sproto(body)`.
- Sproto is the standard compact binary serialization (Skynet-style).

Key tags: visitor=2, verfiy=3, login=4, update_game_server=7, map_ready=100,
move=101, skill_use=102, character_list=103, character_pick=105, chat=120,
heart_beat=218, leave_game=234, refresh_online_state=281, game_check=308,
enter_map=503, login_max_count=578, retrieve_account=660.

Full tag table: `research/notes/protocol_tags.json` (409 entries).
Full protocol notes: `docs/protocol.md`.

## Phase 6-8 status

A Python compatible server (sproto codec + login flow) is planned here.
This snapshot preserves all reverse-engineering artifacts (decompiled C#,
protocol spec, tag table, APK backup). The in-progress Python codec was
dropped from this commit to keep the repository clean; it will be added in
a follow-up commit.

## Server bullet-point plan

1. sproto encode/decode (port of SprotoTypeSerialize/Deserialize).
2. pack/unpack (port of SprotoPack).
3. frame: 2-byte BE length + pack(payload).
4. Package header (type/session).
5. Handlers: visitor, verfiy, login, update_game_server, character_list,
   character_pick, enter_map, heart_beat, move, chat, leave_game.
6. SQLite storage for accounts/characters.
