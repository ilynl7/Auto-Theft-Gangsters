# Auto Theft Gangsters v1.19 Revival Project

A research and preservation project focused on reverse engineering, documenting, and recreating the backend infrastructure of **Auto Theft Gangsters v1.19**, an abandoned Android multiplayer game.

The goal of this project is to understand how the original client works, recover lost functionality, and develop a new compatible server implementation to bring the game back online.

---

## Project Status

✅ **Revival server implemented** — a protocol-compatible Python backend now
lives in [`server/`](server/) with docs in
[`research/server/README.md`](research/server/README.md).

```bash
python3 -m server.main   # gate :9777 + game server :9555
```

Ongoing focus:

- Client compatibility testing with the v1.19 APK
- Implementing the remaining game systems (PvP, rides, guilds)

Implemented game systems: login/character flow, world AOI + movement sync,
chat, missions (accept/progress/complete/rewards), shops (browse/buy),
backpack + currency persistence.

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
