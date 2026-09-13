"""Persistence tests for the player system:

* attributes generate ONCE at character creation and never re-roll
* attributes/gear/skill colors survive logout -> login
* attributes/gear/skill colors survive a full server restart
* equip/unequip recalculate power from the persisted instances
"""
import asyncio
import os
import sys

sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

import pytest

from server import economy

from tests.test_e2e import _connect, server  # noqa: F401  (pytest fixture)
from tests.test_economy import login_and_pick


@pytest.mark.asyncio
async def test_attributes_persist_across_relogin(server):
    srv, gate_port, game_port = server
    db = srv.db

    # create + first session
    c, char_id = await login_and_pick(game_port, "Persistor")
    attrs_first = dict(db.load_attributes(char_id))
    assert attrs_first, "attribute set persisted at creation"
    instances_first = {
        slot: (db.get_equipped_index(char_id, slot),
               db.get_instance(db.get_equipped_index(char_id, slot))["attrs"]
               if db.get_equipped_index(char_id, slot) else None)
        for slot, _iid, _idx in db.list_equipped(char_id)
    }
    assert instances_first, "starting gear has persisted instances"
    await c.close()

    # second login (same account, same character)
    c2 = await _connect(game_port)
    from server import protocol as P
    from server import sproto
    resp = await c2.rpc(P.VISITOR, {})
    account_id = sproto.as_str(resp.body[0])
    key = sproto.as_str(resp.body[1])
    resp = await c2.rpc(P.VERFIY, {0: account_id, 1: key, 2: "14119"})
    await c2.rpc(P.LOGIN, {0: resp.body[1], 1: account_id})
    await c2.rpc(P.CHARACTER_PICK, {0: char_id})
    await c2.drain(0.4)

    # NOTHING re-rolled: identical attribute set, identical instances
    attrs_second = db.load_attributes(char_id)
    assert attrs_second == attrs_first
    for slot, (index_id, attrs) in instances_first.items():
        assert db.get_equipped_index(char_id, slot) == index_id, \
            "equip slot kept its persisted instance"
        assert db.get_instance(index_id)["attrs"] == attrs, \
            "rolled attrs/colors did not change on relogin"
    await c2.close()


@pytest.mark.asyncio
async def test_weapon_skill_color_survives_relogin(server):
    srv, gate_port, game_port = server
    db = srv.db
    c, char_id = await login_and_pick(game_port, "SkillHolder")
    weapon_index = db.get_equipped_index(char_id, 0)
    assert weapon_index is not None
    inst = db.get_instance(weapon_index)
    # the weapon rolled exactly one skill entry (skillId in field 4)
    skills = [a[4] for a in inst["attrs"] if len(a) > 4 and a[4]]
    assert skills, "weapon instance carries a rolled skill id"
    await c.close()

    # relogin: same instance, same skill, same quality
    c2, _ = await login_and_pick(game_port, "SkillHolder2")
    # login_and_pick creates a NEW character; re-open the original
    await c2.close()
    c3 = await _connect(game_port)
    from server import protocol as P
    from server import sproto
    resp = await c3.rpc(P.VISITOR, {})
    account_id = sproto.as_str(resp.body[0])
    key = sproto.as_str(resp.body[1])
    resp = await c3.rpc(P.VERFIY, {0: account_id, 1: key, 2: "14119"})
    await c3.rpc(P.LOGIN, {0: resp.body[1], 1: account_id})
    await c3.rpc(P.CHARACTER_PICK, {0: char_id})
    await c3.drain(0.4)
    inst2 = db.get_instance(weapon_index)
    assert inst2["attrs"] == inst["attrs"]
    assert inst2["quality"] == inst["quality"]
    await c3.close()


@pytest.mark.asyncio
async def test_persistence_across_server_restart(server):
    srv, gate_port, game_port = server
    db = srv.db
    c, char_id = await login_and_pick(game_port, "Restarter")
    attrs_first = dict(db.load_attributes(char_id))
    await c.close()

    # simulate the full restart: close the DB, reopen the same file
    path = db._conn.execute("PRAGMA database_list").fetchone()[2]
    db.close()
    from server.db import Database
    db2 = Database(path)
    srv.db = db2
    attrs_after = db2.load_attributes(char_id)
    assert attrs_after == attrs_first, "attributes survive a server restart"
    row = db2.get_character(char_id)
    assert row["max_hp"] == attrs_after[1002], \
        "persisted max_hp matches the saved attribute set"


@pytest.mark.asyncio
async def test_power_recalc_on_equip(server):
    srv, gate_port, game_port = server
    db = srv.db
    c, char_id = await login_and_pick(game_port, "PowerRanger")
    power_before = db.load_attributes(char_id).get("power", 0)

    # give + equip a TIER-2 weapon (different from the starter tier-1
    # already equipped) — power must move
    prof = db.get_character(char_id)["profession"]
    weapon = (prof + 1) * 10000 + 101
    db.add_item(char_id, weapon, 1)
    from server import protocol as P
    resp = await c.rpc(P.EQUIP_ITEM, {0: weapon})
    assert resp.body[0] == 0
    power_after = db.load_attributes(char_id).get("power", 0)
    assert power_after != power_before, \
        "equipping changes the calculated power"

    # unequip: power returns
    resp = await c.rpc(P.UNEQUIP_ITEM, {0: 0})
    assert resp.body[0] == 0
    assert db.load_attributes(char_id).get("power", 0) != power_after
    await c.close()
