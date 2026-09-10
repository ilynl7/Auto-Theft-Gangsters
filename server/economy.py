"""Content tables + world simulation config: NPCs, combat, skills, cars.

Item/weapon/car/skill content is merged from `game_data.py`, which holds the
real values recovered from the APK's Data.bundle. A small set of legacy
placeholder goods is kept for the shop flow tests. See research/server/README.md.
"""

from . import game_data as GD
from .game_data import CARS as REAL_CARS
from .game_data import ITEMS as REAL_ITEMS
from .game_data import PROFESSIONS
from .game_data import SKILLS as REAL_SKILLS
from .game_data import WEAPONS as REAL_WEAPONS

# The main open-world map the client loads after character pick.
#
# CRITICAL: map ids must match the client's MapInfoData table (recovered from
# the APK's Data.bundle). In that table map 1's scene name is literally
# "LoadingScene" — entering map 1 makes LoadingWindow.LoadScene load the
# loading screen as the game map, so the loading window never finishes.
# The main city (scene DSJ_GTA) is map id 11.
MAIN_CITY_MAP = "11"

# --- testing build --------------------------------------------------------
# Every new character starts maxed out so all content is reachable while
# testing (user request: level 80, huge gold/diamond wallets).
TEST_MAX_LEVEL = 80
TEST_START_GOLD = 99_999_999
TEST_START_DIAMOND = 999_999

# --- items -----------------------------------------------------------------
# item_id -> {"name", "type", "slot", "price"(gold), "power"}
# type: "consumable" | "equipment" | "badge" | "fashion" | "package"
# slot: 0 weapon, 1 armor, 2 badge, 3 fashion (equipment/badge/fashion only)
# Real items from the APK ItemData table + the legacy placeholders below.
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
ITEMS.update(REAL_ITEMS)
# weapons are equipment with slot 0 and `atk` as power
for _wid, _w in REAL_WEAPONS.items():
    ITEMS[_wid] = {
        "name": _w["name"], "type": "equipment", "slot": 0,
        "price": 0, "power": _w["atk"], "weapon_class": _w["class"],
        "tier": _w["tier"], "skills": _w["skills"],
    }

# --- cars --------------------------------------------------------------------
# legacy placeholder cars (shop goods 901-903 reference ids 1-3)
LEGACY_CARS = {
    1: {"name": "Sedan", "speed": 14},
    2: {"name": "Muscle Car", "speed": 18},
    3: {"name": "Sports Coupe", "speed": 24},
}
CARS = dict(LEGACY_CARS)
CARS.update(REAL_CARS)

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
        # the real car shop sold the car exchange vouchers (ItemData 9301-9307)
        # which the client exchanges into the actual vehicles (CarData 1001-1007)
        "goods": [
            {"goods_id": 901, "item_id": 9301, "count": 1, "car_id": 1001,
             "name": "North Star", "currency": 1, "price": 1888},
            {"goods_id": 902, "item_id": 9303, "count": 1, "car_id": 1002,
             "name": "Thunder", "currency": 1, "price": 5888},
            {"goods_id": 903, "item_id": 9304, "count": 1, "car_id": 1003,
             "name": "Conqueror", "currency": 1, "price": 8888},
            {"goods_id": 904, "item_id": 9302, "count": 1, "car_id": 1004,
             "name": "Bison", "currency": 1, "price": 18888},
            {"goods_id": 905, "item_id": 9305, "count": 1, "car_id": 1005,
             "name": "Night Walker", "currency": 1, "price": 28888},
            {"goods_id": 906, "item_id": 9306, "count": 1, "car_id": 1006,
             "name": "Golden King", "currency": 2, "price": 100},
            {"goods_id": 907, "item_id": 9307, "count": 1, "car_id": 1007,
             "name": "Christmas Sleigh", "currency": 2, "price": 50},
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
# The REAL main mission chain lives in server/mission_data.py (extracted
# from the APK's MissionData table). The entries below are legacy side
# missions kept for the buy/visit progression hooks.
MISSIONS = {
    "900001": {
        "name": "Stock Up",
        "type": "buy",
        "item_id": 1,
        "count": 2,
        "reward": {"gold": 1000, "diamond": 2, "items": {2: 2}},
        "next": "900002",
    },
    "900002": {
        "name": "Armed and Ready",
        "type": "buy",
        "item_id": 10,
        "count": 1,
        "reward": {"gold": 3000, "diamond": 5, "items": {3: 1}},
        "next": None,
    },
    "900101": {
        "name": "Courier Run",
        "type": "visit",
        "map_id": MAIN_CITY_MAP,
        "count": 1,
        "reward": {"gold": 800, "diamond": 1, "items": {1: 3}},
        "next": None,
    },
}

DAILY_MISSION_IDS = ["900101"]

# --- combat / npcs ------------------------------------------------------------
# skills: real per-class skill groups from SkillData merged over the legacy
# generic set
SKILLS = {
    1: {"name": "Basic Attack", "damage": 5},
    2: {"name": "Heavy Strike", "damage": 15},
    3: {"name": "Power Blow", "damage": 30},
}
SKILLS.update(REAL_SKILLS)

SKILL_LEVELUP_COST = 1000       # gold per skill level

# NPC kinds per map: kind -> (name, level, max_hp, damage, exp, loot table)
# loot: {item_id: (drop chance 0..1, count)}
#
# npcdataid/atk/def are REAL NpcData rows recovered from the APK's
# Data.bundle (server/game_data.py-style extraction): the client's
# npc_create_handler -> ObjInitNpcData.InitData hard-looks-up
# DataManager.GetNpcDataByID(npc_attribute.npcdataid) — an id that is not in
# the client's NpcData table returns null and crashes the client (loading
# window freezes at 90%). Rows used (NpcData CSV):
#   21131 RepairMan  Lv.1 Atk 20 Hp 2800 Def 100  (model NPC_Nan_009)
#   21132 Horro      Lv.2 Atk 25 Hp 3640 Def 110  (model NPC_Nan_006)
#   21133 Punk       Lv.3 Atk 30 Hp 4480 Def 120  (model NPC_Nan_015)
#   61101 Office lady Lv.6 Atk 90 Hp 2500 Def 150 (model NPC_Nv_002)  (boss)
NPC_KINDS = {
    1: {"name": "RepairMan", "npcdataid": "21131", "level": 1, "max_hp": 60,
        "damage": 6, "exp": 40, "gold": 80,
        "loot": {1: (0.5, 1), 2: (0.4, 2)}},
    2: {"name": "Horro", "npcdataid": "21132", "level": 2, "max_hp": 120,
        "damage": 12, "exp": 90, "gold": 200,
        "loot": {10: (0.15, 1), 1: (0.6, 2)}},
    3: {"name": "Punk", "npcdataid": "21133", "level": 3, "max_hp": 300,
        "damage": 25, "exp": 300, "gold": 800,
        "loot": {11: (0.08, 1), 20: (0.2, 1)}},
}

# The main open-world map the client loads after character pick.
# npc spawn tables: map_id -> [(kind, x, y, z)]
NPC_SPAWNS = {
    MAIN_CITY_MAP: [(1, 300, 0, 300), (1, 320, 0, 310), (2, 500, 0, 500),
                    (3, 900, 0, 900)],
}

# player base combat values (a weapon's `power` is added to attack)
PLAYER_BASE_ATTACK = 12
PLAYER_BASE_HP = 100
RESPAWN_HP_FRACTION = 1.0

# Star-1 starter gear set: [(equip_slot, item_id)] granted on character
# creation. Slot 0 weapon comes from the profession's tier-1 weapon.
STARTER_GEAR = [
    (1, 3),    # Body Armor (slot 1)
    (2, 20),   # Badge of Strength (slot 2)
]

# EQUIP_QUALITY.KUANG_WHITE - the 1-star quality for starter gear.
STARTER_QUALITY = 1

# Random attribute roll ranges for freshly created gear (gameitem
# random_attri entries): (attr_id, min, max). Attr ids follow the client's
# ATTRIBUTE_TYPE ids; the character screen just lists whatever arrives.
RANDOM_ATTR_POOL = [
    (1, 5, 20),     # atk
    (2, 5, 20),     # def
    (3, 20, 100),   # hp
    (4, 1, 5),      # hit
    (5, 1, 5),      # eva
    (6, 1, 3),      # cri
]


def roll_random_attrs(count: int = 2, rng=None):
    """Roll `count` random attributes from RANDOM_ATTR_POOL.

    Returns [(index, attr_id, value)] for gameitem.random_attri.
    """
    import random as _random
    rng = rng or _random
    pool = rng.sample(RANDOM_ATTR_POOL, min(count, len(RANDOM_ATTR_POOL)))
    return [(i, aid, rng.randint(lo, hi))
            for i, (aid, lo, hi) in enumerate(pool)]

# skills: real per-class skill groups from SkillData merged over the legacy
# generic set
SKILLS = {
    1: {"name": "Basic Attack", "damage": 5},
    2: {"name": "Heavy Strike", "damage": 15},
    3: {"name": "Power Blow", "damage": 30},
}
SKILLS.update(REAL_SKILLS)

# --- copy scenes / dungeons -------------------------------------------------
# Wave-based PvE instances entered from the open world (client flow:
# enter_copy_scene -> npc_create per wave -> single_copy_scene_npc_die ->
# next_wave -> copy_scene_result). The client drives combat; the server
# validates reports and pays rewards on success.
#
# copy_id -> {"name", "waves": [[npc kind, ...], ...], "reward": {...},
#             "countdown": seconds per wave}
COPY_SCENES = {
    1: {
        "name": "Back Alley Brawl",
        "waves": [[1, 1], [1, 1, 2]],
        "countdown": 60,
        "reward": {"gold": 1500, "diamond": 1, "items": {1: 2}, "exp": 120},
    },
    2: {
        "name": "Docks Raid",
        "waves": [[2, 1], [2, 2], [3]],
        "countdown": 90,
        "reward": {"gold": 5000, "diamond": 3, "items": {20: 1}, "exp": 400},
    },
}

# --- tower (climb endless npc floors) ----------------------------------------
# client flow: request_tower_copy_info -> enter_tower_copy_info ->
# fight (single_copy_scene_npc_die) -> grant_tower_reward per floor block
TOWER = {
    "max_floor": 100,
    "floors_per_reward_block": 5,
    "gold_per_floor": 150,
    "exp_per_floor": 60,
    "reset_diamond_cost": 20,
}

# --- rank pvp (tianTi ladder) ------------------------------------------------
# client flow: request_random_rank_pvp_opponent -> rank_pvp_start ->
# simulated 1v1 resolved server-side from reported attacks
# (rank_pvp_player_attack / rank_pvp_other_player_die) -> tiantti_result
RANK_PVP = {
    "base_score": 1000,
    "win_score": 25,
    "lose_score": -15,
    "win_gold": 1200,
    "win_diamond": 2,
    "win_count_reward": 500,     # gold per 5 cumulative wins
}

# --- slot machine ------------------------------------------------------------
# three reels of 0..5; three of a kind pays 10x, a pair pays 2x
SLOT = {
    "spin_cost": 100,
    "reels": 6,
    "triple_multiplier": 10,
    "pair_multiplier": 2,
}

# --- wild boss (open-world raid boss) ----------------------------------------
# client flow: request_wild_boss_info(200) -> enter_wild_boss(201) ->
# fight via the standard combat tags -> shared HP pool per map; respawns
# on demand at full strength once killed
WILD_BOSS = {
    "kind": 3,                      # Mob Boss stats
    "spawn": {"x": 1200, "y": 0, "z": 1200, "o": 0},
    "countdown": 120,
}

# --- survive (survival assault) ----------------------------------------------
# client flow: request_survive_top(245) -> enter_survive_batttle(246) ->
# endless escalating npc waves (kills reported via single_copy_scene_npc_die)
# -> survive_battle_finish(637) pays scaled rewards and records the best wave
SURVIVE = {
    "countdown": 90,
    "reward": {"gold": 300, "exp": 100},   # multiplied by the wave reached
}
