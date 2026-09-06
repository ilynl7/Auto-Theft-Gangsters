# Auto Theft Gangsters v1.19 Revival Project

A research and preservation project focused on reverse engineering, documenting, and recreating the backend infrastructure of **Auto Theft Gangsters v1.19**, an abandoned Android multiplayer game.

The goal of this project is to understand how the original client works, recover lost functionality, and develop a new compatible server implementation to bring the game back online.

---

## Project Status

✅ **Revival server implemented** — a protocol-compatible Python backend now
lives in [`server/`](server/) with docs in
[`research/server/README.md`](research/server/README.md).

```bash
python3 -m server.main   # single port :13103 (gate + game on one listener)
# legacy two-port mode: ATG_SINGLE_PORT=0  -> gate :9777 + game :9555
```

The client connects its login gate to the patched address/port and then hops
to whatever `game_server.serverPort` the gate advertises — the gate advertises
the same single port, so one open port is enough.

Ongoing focus:

- Client compatibility testing with the v1.19 APK
- Fine-tuning provisional field layouts for systems whose SprotoType classes
  were not preserved in the decompiled dump (dungeons, PvP, tower, slots)

Implemented game systems: login/character flow, world AOI + movement sync,
chat, missions + daily missions, shops + car shop, full inventory (weapons,
armor, badges, fashion, storage, item packages), NPC combat with loot/exp/
level-ups, skills, respawn, guilds (create/join/donate/guild shop/ranks),
friends, mail, sign-in rewards, car/mount ownership, copy scenes (wave-based
dungeons with countdown + rewards), the rank PvP ladder (天梯 matchmaking,
score/history/win-count rewards), the endless tower climb (floors, rewards,
reset, wipe-out), the slot machine (spins + accumulating sum-reward pool),
map/line switching + teleport points, client progress tags (tutorial,
function unlocks, renames), the open-world wild boss raid (shared boss HP
pool, boss info + entry sync), the survive mode (escalating npc waves,
wave-scaled rewards, best-wave leaderboard), and the weekly guild battle
(guild-vs-guild war, tags 285–295 / 662–677 with the real schedule from the
client's GuildBattleData: weekday 5/6/7 rounds, 1800 s per battle, 10
fighters per guild, gold-bar betting, weekly score ranking).

Content now uses the real values recovered from the APK's Data.bundle
(`server/game_data.py`): the three playable professions (Batfighter,
Boxer, Gunner — the `character_create` profession selects the weapon
class), 24+ weapons across 8 tiers per class with the real attack values,
7 real vehicles (North Star, Thunder, Conqueror, Bison, Night Walker,
Golden King, Christmas Sleigh) with the real CarData stats, real
consumables/potions/tickets with ItemData prices, and the real per-class
skill groups (Bat Combo / Straight Punch / Snap Shot + dodge).

---

# Objectives

The project aims to:

- Preserve the original game client
- Analyze the Android application structure
- Recover game assets and configuration data
- Reverse engineer client-server communication
- Document the original protocol
- Build a custom replacement server
- Restore online functionality

---

# Planned Architecture

```
Original Game Client
        |
        |
        v
Custom Server Backend
        |
 +------+------+
 |             |
 v             v
Authentication  Game Server
 |
 v
Database
```

---

# Repository Structure

```
Auto-Theft-Gangsters-Revival/

├── apk/
├── analysis/
├── assets/
├── network/
├── server/
└── docs/
```

---

# Reverse Engineering Roadmap

## Phase 1 - Client Analysis

- Extract APK contents
- Analyze AndroidManifest
- Identify frameworks and libraries
- Analyze Java/native code
- Locate important classes

Tools:

- JADX
- APKTool
- Ghidra

---

## Phase 2 - Asset Recovery

Recover:

- Maps
- Models
- Textures
- Vehicles
- Weapons
- Configuration files
- Game data tables

---

## Phase 3 - Network Research

Analyze:

- Server addresses
- Ports
- Protocol type
- Login process
- Player synchronization
- Game commands

---

## Phase 4 - Protocol Reconstruction

Document:

- Packet structure
- Headers
- Commands
- Data formats
- Encryption/compression if present

Example:

```
[Header]
[Command ID]
[Length]
[Payload]
```

---

## Phase 5 - Server Development

Create replacement services:

### Authentication Server

- Account creation
- Login
- Session handling

### Player System

- Player data
- Progress saving
- Inventory
- Currency

### World Server

- Player movement
- Multiplayer synchronization
- World events

---

# Technology Stack

Possible backend technologies:

- Node.js
- C#
- Java
- Go
- Python

Database:

- PostgreSQL
- MySQL
- SQLite

---

# Research Tools

Android:

- JADX
- APKTool
- Frida
- Ghidra

Network:

- Wireshark
- mitmproxy
- tcpdump

Development:

- Git
- Docker
- VS Code

---

# Current Findings

```
Game:
Auto Theft Gangsters

Version:
v1.19

Platform:
Android
```

Initial observations:

- Traditional Android APK structure
- Contains classes.dex
- Contains native libraries
- No major protection systems detected
- Suitable for static and dynamic analysis

---

# Goals

Long-term goals:

- Fully understand the original client
- Reconstruct the missing backend
- Make the original client communicate with a custom server
- Preserve the game for historical and educational purposes

---

# Disclaimer

This repository is intended for:

- Software preservation
- Educational research
- Reverse engineering study
- Historical documentation

Respect the original developers and intellectual property owners.

---

# Contributions

Useful contributions:

- Old APK versions
- Server information
- Gameplay recordings
- Technical research
- Documentation
- Reverse engineering findings

---

# Credits

Original Game:

**Auto Theft Gangsters v1.19**

Original Developer:

**Doodle Mobile**

Project:

**Community Revival Research**
