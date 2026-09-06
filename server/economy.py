"""Content tables + world simulation config: NPCs, combat, skills, cars.

Provisional revival-server data (see research/server/README.md). Extend these
tables to add content — the handlers and world simulation are data-driven.
"""

# --- items -----------------------------------------------------------------
# item_id -> {"name", "type", "slot", "price"(gold), "power"}
# type: "consumable" | "equipment" | "badge" | "fashion" | "package"
# slot: 0 weapon, 1 armor, 2 badge, 3 fashion (equipment/badge/fashion only)
ITEMS = {
    1:  {"name": "Health Potion", "type": "consumable", "price": 250,
         "heal": 50},
    2:  {"name": "Bandage", "type": "consumable", "price": 120, "heal": 20},
    3:  {"name": "Body Armor", "type": "equipment", "slot": 1, "price": 3000,
         "power": 20},
    4:  {"name": "Gold Pack (S)", "type": "package", "price": 1000,
         "gold": 2000},
    5:  {"name": "Energy Drink", "type": "consumable", "price": 300,
         "heal": 30},
    10: {"name": "Pistol", "type": "equipment", "slot": 0, "price": 8000,
         "power": 15},
    11: {"name": "Shotgun", "type": "equipment", "slot": 0, "price": 15000,
         "power": 35},
    12: {"name": "Rifle", "type": "equipment", "slot": 0, "price": 40000,
         "power": 60},
    20: {"name": "Badge of Strength", "type": "badge", "slot": 2, "price": 5000,
         "power": 10},
    21: {"name": "Lucky Badge", "type": "badge", "slot": 2, "price": 9000,
         "power": 25},
    30: {"name": "Leather Jacket", "type": "fashion", "slot": 3, "price": 2000},
    31: {"name": "Street Suit", "type": "fashion", "slot": 3, "price": 6000},
    40: {"name": "Starter Crate", "type": "package", "price": 1500,
         "contents": {1: 3, 2: 5}},
}

# shops: shop_id -> goods list (as before, plus car shop id 9)
SHOPS = {
    1: {
        "name": "General Store",
        "goods": [
            {"goods_id": 101, "item_id": 1, "count": 1,
             "currency": 1, "price": 250},
            {"goods_id": 102, "item_id": 2, "count": 5,
             "currency": 1, "price": 600},
            {"goods_id": 103, "item_id": 3, "count": 1,
             "currency": 1, "price": 3000},
            {"goods_id": 104, "item_id": 4, "count": 1,
             "currency": 2, "price": 10},
            {"goods_id": 105, "item_id": 5, "count": 10,
             "currency": 2, "price": 25},
            {"goods_id": 106, "item_id": 40, "count": 1,
             "currency": 1, "price": 1500},
        ],
    },
    2: {
        "name": "Weapon Shop",
        "goods": [
            {"goods_id": 201, "item_id": 10, "count": 1,
             "currency": 1, "price": 8000},
            {"goods_id": 202, "item_id": 11, "count": 1,
             "currency": 1, "price": 15000},
            {"goods_id": 203, "item_id": 12, "count": 1,
             "currency": 2, "price": 60},
        ],
    },
    3: {
        "name": "Badge & Fashion",
        "goods": [
            {"goods_id": 301, "item_id": 20, "count": 1,
             "currency": 1, "price": 5000},
            {"goods_id": 302, "item_id": 21, "count": 1,
             "currency": 1, "price": 9000},
            {"goods_id": 303, "item_id": 30, "count": 1,
             "currency": 1, "price": 2000},
            {"goods_id": 304, "item_id": 31, "count": 1,
             "currency": 2, "price": 20},
        ],
    },
    9: {
        "name": "Car Shop",
        "car_shop": True,
        "goods": [
            {"goods_id": 901, "car_id": 1, "name": "Sedan",
             "currency": 1, "price": 20000},
            {"goods_id": 902, "car_id": 2, "name": "Muscle Car",
             "currency": 1, "price": 50000},
            {"goods_id": 903, "car_id": 3, "name": "Sports Coupe",
             "currency": 2, "price": 100},
        ],
    },
}

CURRENCY_GOLD = 1
CURRENCY_DIAMOND = 2

# guild shop (bought with guild-donated gold via req_buy_guild_goods)
GUILD_SHOP = [
    {"goods_id": 801, "item_id": 1, "count": 5, "price": 500},
    {"goods_id": 802, "item_id": 2, "count": 10, "price": 800},
    {"goods_id": 803, "item_id": 20, "count": 1, "price": 4000},
    {"goods_id": 804, "item_id": 40, "count": 1, "price": 2500},
]

# --- missions ---------------------------------------------------------------
MISSIONS = {
    1001: {
        "name": "Stock Up",
        "type": "buy",
        "item_id": 1,
        "count": 2,
        "reward": {"gold": 1000, "diamond": 2, "items": {2: 2}},
        "next": 1002,
    },
    1002: {
        "name": "Armed and Ready",
        "type": "buy",
        "item_id": 10,
        "count": 1,
        "reward": {"gold": 3000, "diamond": 5, "items": {3: 1}},
        "next": None,
    },
    2001: {
        "name": "Courier Run",
        "type": "visit",
        "map_id": "1",
        "count": 1,
        "reward": {"gold": 800, "diamond": 1, "items": {1: 3}},
        "next": None,
    },
}

DAILY_MISSION_IDS = [2001]

# --- combat / npcs ------------------------------------------------------------
SKILLS = {
    1: {"name": "Straight Punch", "damage": 10},
    2: {"name": "Quick Shot", "damage": 18},
    3: {"name": "Power Blow", "damage": 30},
}

SKILL_LEVELUP_COST = 1000       # gold per skill level

# NPC kinds per map: kind -> (name, level, max_hp, damage, exp, loot table)
# loot: {item_id: (drop chance 0..1, count)}
NPC_KINDS = {
    1: {"name": "Street Thug", "level": 1, "max_hp": 60, "damage": 6,
        "exp": 40, "gold": 80, "loot": {1: (0.5, 1), 2: (0.4, 2)}},
    2: {"name": "Gang Enforcer", "level": 3, "max_hp": 120, "damage": 12,
        "exp": 90, "gold": 200, "loot": {10: (0.15, 1), 1: (0.6, 2)}},
    3: {"name": "Mob Boss", "level": 6, "max_hp": 300, "damage": 25,
        "exp": 300, "gold": 800, "loot": {11: (0.08, 1), 20: (0.2, 1)}},
}

# npc spawn tables: map_id -> [(kind, x, y, z)]
NPC_SPAWNS = {
    "1": [(1, 300, 0, 300), (1, 320, 0, 310), (2, 500, 0, 500),
          (3, 900, 0, 900)],
}

# player base combat values (a weapon's `power` is added to attack)
PLAYER_BASE_ATTACK = 12
PLAYER_BASE_HP = 100
RESPAWN_HP_FRACTION = 1.0

# cars (car shop): car_id -> {"name", "speed"}
CARS = {
    1: {"name": "Sedan", "speed": 14},
    2: {"name": "Muscle Car", "speed": 18},
    3: {"name": "Sports Coupe", "speed": 24},
}
