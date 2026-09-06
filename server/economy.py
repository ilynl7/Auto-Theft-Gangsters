"""Game content: shop catalog + mission definitions.

These are revival-server data tables. The original game shipped its item/quest
data inside `Bundle/Data/Data.bundle`; until that bundle is fully parsed, the
catalog here is a small, sensible default set (documented in
research/server/README.md) so the shop and mission loops are fully playable.

Extend these tables (or load them from a parsed Data.bundle) to add content —
the handlers below are fully data-driven.
"""

# ---------------------------------------------------------------------------
# Shop
# ---------------------------------------------------------------------------
# One shop per shop_id. Each good is sold for one of the two currencies.

# currency codes used across the revival server
CURRENCY_GOLD = 1     # in-game cash (soft currency)
CURRENCY_DIAMOND = 2  # premium currency

SHOPS = {
    # shop_id -> {"name": str, "goods": [goods dict]}
    1: {
        "name": "General Store",
        "goods": [
            {"goods_id": 101, "item_id": 1, "count": 1,
             "currency": CURRENCY_GOLD, "price": 500},
            {"goods_id": 102, "item_id": 2, "count": 5,
             "currency": CURRENCY_GOLD, "price": 1200},
            {"goods_id": 103, "item_id": 3, "count": 1,
             "currency": CURRENCY_GOLD, "price": 3000},
            {"goods_id": 104, "item_id": 4, "count": 1,
             "currency": CURRENCY_DIAMOND, "price": 10},
            {"goods_id": 105, "item_id": 5, "count": 10,
             "currency": CURRENCY_DIAMOND, "price": 25},
        ],
    },
    2: {
        "name": "Weapon Shop",
        "goods": [
            {"goods_id": 201, "item_id": 10, "count": 1,
             "currency": CURRENCY_GOLD, "price": 8000},
            {"goods_id": 202, "item_id": 11, "count": 1,
             "currency": CURRENCY_GOLD, "price": 15000},
            {"goods_id": 203, "item_id": 12, "count": 1,
             "currency": CURRENCY_DIAMOND, "price": 60},
        ],
    },
}

# item_id -> display name (provisional; real names live in Data.bundle)
ITEM_NAMES = {
    1: "Health Potion",
    2: "Bandage",
    3: "Body Armor",
    4: "Gold Pack (S)",
    5: "Energy Drink",
    10: "Pistol",
    11: "Shotgun",
    12: "Rifle",
}

# ---------------------------------------------------------------------------
# Missions
# ---------------------------------------------------------------------------
# mission_id -> definition dict.
#
#  type "collect": complete when the player holds >= `count` of `item_id`
#                  (tracked via backpack; `granted_items` are added on accept
#                   when `grant_on_accept` is true — used for "buy X" quests)
#  type "visit":   complete when the player sends use_item/spend events
#                  totalling `count` of `item_id` (buy/consume missions)
#
# reward: {"gold": int, "diamond": int, "items": {item_id: count}}

MISSIONS = {
    1001: {
        "name": "Stock Up",
        "type": "buy",          # buy `count` of `item_id` from any shop
        "item_id": 1,
        "count": 2,
        "reward": {"gold": 1000, "diamond": 2, "items": {2: 2}},
        "next": 1002,           # chained follow-up mission
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
        "type": "visit",        # stand in map `map_id` and report position
        "map_id": "1",
        "count": 1,
        "reward": {"gold": 800, "diamond": 1, "items": {1: 3}},
        "next": None,
    },
}

DAILY_MISSION_IDS = [2001]
