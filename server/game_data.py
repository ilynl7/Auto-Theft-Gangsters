"""Real game content extracted from the APK's `Bundle/Data/Data.bundle`.

Everything here is recovered data, not invention:

* Cars — `CarData` table (7 buyable vehicles + the guild-boss placeholder
  cars 900101+). Names/descriptions are string-table keys resolved to the
  official English localization (North Star, Bison, Conqueror, Thunder,
  Night Walker, Golden King + the Christmas special).
* Weapons — `EquipData` table rows with Position=0, Job=-1. There are three
  weapon classes (the client's `WeaponType`): 0 = Batfighter (械斗, bat),
  1 = Boxer (拳击, fists), 2 = Gunner (枪手, gun) — one per playable
  profession. Each class has 8 tiers (`Class` column 1..8) plus event
  variants; BaseSkills 101-104/201-204/301-304 are the profession's skill
  set (normal attack x3 + dodge).
* Professions — the client's character create offers exactly three
  (string table 100128-100130): Batfighter, Boxer, Gunner. The profession
  value sent in `character_create.general` field 2 selects the weapon class.
* Items — `ItemData` table subset with prices: health potions (定值-血药),
  percent potions, revive potion (复活药), stamina potions (药剂), car
  exchange vouchers (汽车兑换券), sweep/exp/gold tickets, rename card.

Localizations from the client string table:
  101410 North Star  101411 Bison  101412 Conqueror  101413 Thunder
  101414 Night Walker 101415 Golden King 101428 Christmas car
  100128 Batfighter  100129 Boxer  100130 Gunner
"""

# --- professions -------------------------------------------------------------
# value sent in character_create.general field 2 -> weapon class
PROFESSIONS = {
    0: {"name": "Batfighter", "weapon_type": 0, "model": "XD",
        "skills": [101, 102, 103, 104]},
    1: {"name": "Boxer", "weapon_type": 1, "model": "QJ",
        "skills": [201, 202, 203, 204]},
    2: {"name": "Gunner", "weapon_type": 2, "model": "NQS",
        "skills": [301, 302, 303, 304]},
}

# --- cars (real CarData rows) ------------------------------------------------
# id -> {"name", "item" (voucher item), "hp", "atk", "speed", "accel", "model"}
CARS = {
    1001: {"name": "North Star",  "item": 9301, "hp": 20000, "atk": 2400,
           "speed": 15, "accel": 2400, "model": "DJ_Car_01"},
    1002: {"name": "Thunder",     "item": 9303, "hp": 20000, "atk": 2700,
           "speed": 16, "accel": 2700, "model": "DJ_Car_03"},
    1003: {"name": "Conqueror",   "item": 9304, "hp": 25000, "atk": 3000,
           "speed": 18, "accel": 3000, "model": "DJ_Car_04"},
    1004: {"name": "Bison",       "item": 9302, "hp": 40000, "atk": 3300,
           "speed": 15, "accel": 3300, "model": "DJ_Car_02"},
    1005: {"name": "Night Walker", "item": 9305, "hp": 25000, "atk": 3600,
           "speed": 20, "accel": 3600, "model": "DJ_Car_05"},
    1006: {"name": "Golden King", "item": 9306, "hp": 35000, "atk": 4000,
           "speed": 20, "accel": 4000, "model": "DJ_Car_06"},
    1007: {"name": "Christmas Sleigh", "item": 9307, "hp": 20000,
           "atk": 3150, "speed": 17, "accel": 3150,
           "model": "DJ_Car_shengDanJie_01"},
    1008: {"name": "North Star GT", "item": 9301, "hp": 10000, "atk": 2400,
           "speed": 16, "accel": 2400, "model": "DJ_Car_101"},
}

# --- weapons (real EquipData rows, Position=0 Job=-1) -------------------------
# id -> {"name", "class" (weapon class 0/1/2), "tier" (1..8), "atk", "skills"}
# The BaseSkills column is the tier's skill group; attack uses the BSValue
# (Basestatus 1001 = attack) column.
def _weapons():
    out = {}
    # (wtype, tier, voucher-drop id offset, atk) recovered per class:
    # tier attack values from BSValue: 180, 300, 486, 732, 1044, 1416, 1848, 2340
    tier_atk = {1: 180, 2: 300, 3: 486, 4: 732, 5: 1044, 6: 1416, 7: 1848,
                8: 2340}
    names = {
        0: "Bat",      # LVLn武器 (械斗)
        1: "Fists",    # LVLn武器 (拳击)
        2: "Gun",      # LVLn武器 (枪手)
    }
    for wtype in (0, 1, 2):
        for tier in range(1, 9):
            # real EquipData ids: 10001/10101/.../10701 (wtype 0),
            # 20001/20101/.../20701 (wtype 1), 30001/.../30701 (wtype 2)
            item_id = (wtype + 1) * 10000 + (tier - 1) * 100 + 1
            out[item_id] = {
                "name": "%s Weapon Lv.%d" % (names[wtype], tier),
                "class": wtype, "tier": tier, "atk": tier_atk[tier],
                "skills": [(wtype + 1) * 100 + k for k in (1, 2, 3, 4)],
                "slot": 0,
                "equip_type": 2,       # ItemData type 2 = equipment
            }
    return out


WEAPONS = _weapons()

# --- consumables & goods (real ItemData rows with prices) ---------------------
# id -> {"name", "type", "heal"/"effect", "price"} (price from ItemData; the
# voucher/kit items were paid with the premium currency in the original shop)
ITEMS = {
    9001: {"name": "Health Potion S", "type": "consumable", "heal": 500,
           "price": 5},
    9002: {"name": "Health Potion M", "type": "consumable", "heal": 1500,
           "price": 10},
    9003: {"name": "Health Potion L", "type": "consumable", "heal": 3000,
           "price": 20},
    9004: {"name": "Percent Health Potion 1", "type": "consumable",
           "heal_pct": 20, "price": 100},
    9005: {"name": "Percent Health Potion 2", "type": "consumable",
           "heal_pct": 40, "price": 100},
    9006: {"name": "Percent Health Potion 3", "type": "consumable",
           "heal_pct": 60, "price": 100},
    9011: {"name": "Revive Potion", "type": "consumable", "revive": 1,
           "price": 100},
    9501: {"name": "Stamina Potion - HP", "type": "consumable", "buff": "hp",
           "price": 1},
    9502: {"name": "Stamina Potion - ATK", "type": "consumable",
           "buff": "atk", "price": 1},
    9503: {"name": "Stamina Potion - DEF", "type": "consumable", "buff": "def",
           "price": 1},
    9201: {"name": "Sweep Ticket", "type": "ticket", "price": 1},
    9202: {"name": "EXP Ticket", "type": "ticket", "price": 1},
    9203: {"name": "Gold Ticket", "type": "ticket", "price": 1},
    9204: {"name": "Equip Ticket", "type": "ticket", "price": 1},
    9205: {"name": "Street Race Ticket", "type": "ticket", "price": 1},
    5024: {"name": "Rename Card", "type": "consumable", "rename": 1,
           "price": 1},
    5026: {"name": "World Chat Mic", "type": "consumable", "chat": 1,
           "price": 1},
    # car exchange vouchers (real items; cars are exchanged from these)
    9301: {"name": "Car Voucher 1 (North Star)", "type": "car_voucher",
           "car": 1001, "price": 1888},
    9302: {"name": "Car Voucher 4 (Bison)", "type": "car_voucher",
           "car": 1004, "price": 1888},
    9303: {"name": "Car Voucher 2 (Thunder)", "type": "car_voucher",
           "car": 1002, "price": 5888},
    9304: {"name": "Car Voucher 3 (Conqueror)", "type": "car_voucher",
           "car": 1003, "price": 5888},
    9305: {"name": "Car Voucher 5 (Night Walker)", "type": "car_voucher",
           "car": 1005, "price": 1888},
    9306: {"name": "Car Voucher 6 (Golden King)", "type": "car_voucher",
           "car": 1006, "price": 0},
    9307: {"name": "Car Voucher 7 (Christmas)", "type": "car_voucher",
           "car": 1007, "price": 0},
}

# --- skills (real SkillData: per-class 3 attacks + 1 dodge) -------------------
SKILLS = {
    101: {"name": "Bat Combo", "class": 0, "damage": 8, "cd": 1.0},
    102: {"name": "Bat Combo", "class": 0, "damage": 8, "cd": 1.0},
    103: {"name": "Bat Combo", "class": 0, "damage": 12, "cd": 1.0},
    104: {"name": "Roll", "class": 0, "damage": 0, "cd": 3.0, "dodge": 1},
    201: {"name": "Straight Punch", "class": 1, "damage": 8, "cd": 1.0},
    202: {"name": "Straight Punch", "class": 1, "damage": 8, "cd": 1.0},
    203: {"name": "Straight Punch", "class": 1, "damage": 12, "cd": 1.0},
    204: {"name": "Sidestep", "class": 1, "damage": 0, "cd": 3.0, "dodge": 1},
    301: {"name": "Snap Shot", "class": 2, "damage": 8, "cd": 1.0},
    302: {"name": "Snap Shot", "class": 2, "damage": 8, "cd": 1.0},
    303: {"name": "Snap Shot", "class": 2, "damage": 12, "cd": 1.0},
    304: {"name": "Backroll", "class": 2, "damage": 0, "cd": 3.0, "dodge": 1},
}

# --- guild battle (real GuildBattleData row 1501) ----------------------------
# map 1501, 10 entries per guild, 1800s survival duration, weekly schedule:
# round 1 signup/siege weekday 5, round 2 weekday 6, round 3 (final) weekday 7,
# battle windows start at 70200/73800 minutes-of-week (19:30 UTC-8 style times
# encoded as seconds-of-week in the client's convention).
GUILD_BATTLE = {
    "copy_id": 1501,
    "map_id": "1501",
    "max_members": 10,
    "duration": 1800,
    "wait_close": 10,
    "guess_item": 1002,          # gold-bar betting (guild_battle_guess)
    "guess_amount": 200,
    "guess_win": 500,
    # weekly schedule (client convention: weekday 1..7 = Mon..Sun; the time
    # value is seconds-of-day: 70200 = 19:30, 73800 = 20:30)
    #   signup/filter: weekday 5 at 19:30; battles: weekdays 5/6/7 at 19:30/20:30
    "rounds": [
        {"weekday": 5, "start": 70200},
        {"weekday": 6, "start": 73800},
        {"weekday": 7, "start": 73800},
    ],
    "rewards": {1: 37001, 2: 37002, 3: 37003},   # top-3 reward drop ids
}
