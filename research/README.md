# Research Vault

Reverse-engineering artifacts for Auto Theft Gangsters v1.19 (`com.doodlemobile.vicecity`).

```
research/
├── apk/          Original APK backup + sha256
├── extracted/    Full APK contents (assets, lib, dex, Unity data)
├── decompiled/   ilspycmd C# decompilation of Assembly-CSharp.dll (key types)
├── notes/        protocol_tags.json (409 tags), hashes, findings
├── captures/     (reserved) network captures
└── server/       custom compatible server (in progress)
```

## Key facts

- Engine: Unity 4.7.1f1, Mono build (not IL2CPP).
- Package: com.doodlemobile.vicecity, versionCode 14119 (v1.19).
- Original servers (dead): `gangsterlogin.galaxyaura.com:9777`,
  `gangsterlogin2.galaxyaura.com:9777`, game port `9555`.
- Protocol: raw TCP + Sproto binary serialization.
- Tag registry: `notes/protocol_tags.json`.
- Decompiled sources include NetLogic, NetManager, SocketAPI, PlayerData,
  all SprotoType.* schemas and the full Protocol tag table.