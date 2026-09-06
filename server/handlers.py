"""Protocol handlers — the revival server's game logic.

Implements the login flow from docs/protocol.md §8:
    update_game_server -> visitor / verfiy -> login -> character_list ->
    character_create -> character_pick -> map_ready -> enter_map ->
    move / chat / heart_beat / leave_game
"""

import logging
import time

from . import config
from . import economy
from . import protocol as P
from . import sproto
from . import world as W
from .session import Session

log = logging.getLogger("atg.handlers")

# verfiy response states (observed in client LoginRootLogic)
VERIFY_OK = 0
VERIFY_NEW_ACCOUNT = 2


class Handlers:
    """Registry of tag -> async handler(session, msg)."""

    def __init__(self, server) -> None:
        self.server = server
        self.map = {
            P.UPDATE_GAME_SERVER: self.h_update_game_server,
            P.VISITOR: self.h_visitor,
            P.VERFIY: self.h_verfiy,
            P.LOGIN: self.h_login,
            P.CHARACTER_LIST: self.h_character_list,
            P.CHARACTER_CREATE: self.h_character_create,
            P.CHARACTER_PICK: self.h_character_pick,
            P.MAP_READY: self.h_map_ready,
            P.ENTER_MAP: self.h_enter_map,
            P.MOVE: self.h_move,
            P.CHAT: self.h_chat,
            P.HEART_BEAT: self.h_heart_beat,
            P.LEAVE_GAME: self.h_leave_game,
            P.REFRESH_ONLINE_STATE: self.h_refresh_online_state,
            P.UPDATE_CLIENT_STATE: self.h_update_client_state,
            P.GAME_CHECK: self.h_game_check,
            P.RETRIEVE_ACCOUNT: self.h_retrieve_account,
            # missions + items
            P.ACCEPT_MISSION: self.h_accept_mission,
            P.COMPLETE_MISSION: self.h_complete_mission,
            P.ABANDON_MISSION: self.h_abandon_mission,
            P.USE_ITEM: self.h_use_item,
            P.SELL_ITEM: self.h_sell_item,
            # shop
            P.ASK_SHOP_LIST: self.h_ask_shop_list,
            P.BUY_SHOP_ITEM: self.h_buy_shop_item,
        }

    # ------------------------------------------------------------------
    # gate (login server, port 9777)
    # ------------------------------------------------------------------
    async def h_update_game_server(self, s: Session, msg) -> None:
        servers = [P.encode_game_server(
            server_id=config.SERVER_ID,
            name=config.SERVER_NAME,
            ip=self.server.advertise_ip,
            port=config.GAME_PORT,
            state=0,
            player_state=0,
            area=0,
            rank=1,
            tz=8,
            weight=0,
            new_server=0,
        )]
        s.respond(msg, {2: sproto.encode_object_array(servers)})

    async def h_visitor(self, s: Session, msg) -> None:
        account_id, key = self.server.db.create_account()
        s.account_id = account_id
        log.info("created visitor account %d", account_id)
        # response {id(0), key(1), state(2)}
        s.respond(msg, {
            0: str(account_id),
            1: key,
            2: VERIFY_OK,
        })

    async def h_verfiy(self, s: Session, msg) -> None:
        account_id = int(msg.body.get(0, "0"))
        key = msg.body.get(1, "")
        # tag 2 = versionCode (string) — accepted but not enforced
        row = self.server.db.verify_account(account_id, key)
        if row is None:
            log.info("verfiy failed for account %s", account_id)
            s.respond(msg, {0: 1})   # state 1 = bad account
            return
        s.account_id = account_id
        s.verified = True
        session_id = self.server.next_session()
        log.info("verfiy ok account %d session %d", account_id, session_id)
        servers = [P.encode_game_server(
            server_id=config.SERVER_ID,
            name=config.SERVER_NAME,
            ip=self.server.advertise_ip,
            port=config.GAME_PORT,
        )]
        # response {state(0), session(1), game_server(2), user_server(3),
        #           facebook_bind(4), versionCode(5), dataVersionCode(6),
        #           downloadFlag(7), notice(8), notice_version(9)}
        s.respond(msg, {
            0: VERIFY_OK,
            1: session_id,
            2: sproto.encode_object_array(servers),
            3: config.SERVER_NAME,
            4: 0,
            5: config.GAME_VERSION,
            6: config.DATA_VERSION,
            7: 0,
        })

    # ------------------------------------------------------------------
    # game server (port 9555)
    # ------------------------------------------------------------------
    async def h_login(self, s: Session, msg) -> None:
        account_id = int(msg.body.get(1, "0"))
        row = self.server.db.verify_account(account_id, "")
        # login.request {session(0), id(1), logintype(2), version(3),
        #                unityVersion(4), serverId(5), time(6)}
        # The game server trusts the connection: account id comes from the
        # verfiy step on the gate. Here we just accept and record.
        s.account_id = account_id or s.account_id
        s.logged_in = True
        log.info("login account %s logintype=%s", account_id,
                 msg.body.get(2))
        # response {type(0), versionCode(1), dataVersionCode(2), serverLevel(3)}
        s.respond(msg, {
            0: P.LOGIN,
            1: config.GAME_VERSION,
            2: config.DATA_VERSION,
            3: 0,
        })

    async def h_character_list(self, s: Session, msg) -> None:
        rows = self.server.db.list_characters(s.account_id or 0)
        chars = [
            sproto.encode_object({
                0: r["id"],
                1: r["name"],
                2: r["level"],
                3: r["sex"],
            })
            for r in rows
        ]
        # character_list.response {character(0)} — map of overview objects
        s.respond(msg, {0: sproto.encode_object_array(chars)})

    async def h_character_create(self, s: Session, msg) -> None:
        # character_create.request {character: general{...}(0)}
        # The client sends a `general` object; we extract name/sex if present,
        # otherwise generate a random name.
        name = None
        sex = 0
        raw = msg.body.get(0)
        if isinstance(raw, (bytes, bytearray)):
            g = sproto.decode_fields(bytes(raw))
            name = g.get(0) if isinstance(g.get(0), str) else None
            sex = g.get(2, 0) if isinstance(g.get(2), int) else 0
        if not name:
            name = "Gangster%d" % (self.server.next_session() % 100000)
        row = self.server.db.create_character(s.account_id or 0, name, sex=sex)
        if row is None:
            s.respond(msg, {1: 1})  # errno: name taken
            return
        overview = sproto.encode_object({
            0: row["id"], 1: row["name"], 2: row["level"], 3: row["sex"],
        })
        s.respond(msg, {0: overview, 1: 0})

    async def h_character_pick(self, s: Session, msg) -> None:
        char_id = msg.body.get(0)
        row = self.server.db.get_character(char_id)
        if row is None or (row["account_id"] != (s.account_id or 0)):
            s.respond(msg, {0: 1})  # errno
            return
        s.picked_character = row
        s.respond(msg, {0: 0})  # errno 0 = ok

    async def h_map_ready(self, s: Session, msg) -> None:
        # Client finished loading; nothing to do server-side.
        s.respond(msg, {})

    async def h_enter_map(self, s: Session, msg) -> None:
        map_id = msg.body.get(0, "1")
        line_index = msg.body.get(1, 0)
        row = getattr(s, "picked_character", None)
        if row is None:
            rows = self.server.db.list_characters(s.account_id or 0)
            row = rows[0] if rows else None
        if row is None:
            return
        if isinstance(map_id, int):
            map_id = str(map_id)
        wp = W.WorldPlayer(s, row["id"], row["name"], row["level"], row["sex"])
        wp.map_id = map_id
        wp.line_index = line_index
        wp.pos = {
            "x": row["pos_x"], "y": row["pos_y"],
            "z": row["pos_z"], "o": row["pos_o"],
        }
        s.world_player = wp

        # tell the entering player about everyone already here
        for other in self.server.world.others(map_id, row["id"]):
            s.push(P.AOI_ADD, {0: W.encode_aoi_add(other)})

        # announce the new arrival
        self.server.world.broadcast(map_id, P.AOI_ADD,
                                    {0: W.encode_aoi_add(wp)},
                                    exclude=row["id"])
        self.server.world.join(wp)

        # confirm entry with the player's own spawn data
        s.respond(msg, {0: W.encode_main_player_create(wp)})
        log.info("%s entered map %s (line %s)", row["name"], map_id, line_index)

    async def h_move(self, s: Session, msg) -> None:
        wp = s.world_player
        if wp is None:
            return
        # move.request {pos(0), moving(1), index(2), parm(3)}
        pos_raw = msg.body.get(0)
        if isinstance(pos_raw, (bytes, bytearray)):
            wp.pos = P.decode_position(bytes(pos_raw))
        moving = bool(msg.body.get(1, False))
        wp.moving = moving
        wp.walk = moving
        self.server.world.broadcast(
            wp.map_id, P.AOI_UPDATE_MOVE,
            {0: W.encode_aoi_update_move(wp)},
            exclude=wp.char_id,
        )
        self._progress_visit_missions(s, wp.char_id, wp.map_id)

    def _progress_visit_missions(self, s: Session, char_id: int,
                                 map_id: str) -> None:
        """Advance active 'visit' missions whose target map the player entered."""
        for row in self.server.db.list_missions(char_id):
            if row["state"] != 0:
                continue
            mdef = economy.MISSIONS.get(row["mission_id"])
            if not mdef or mdef.get("type") != "visit":
                continue
            if str(mdef.get("map_id")) != str(map_id):
                continue
            target = mdef.get("count", 1)
            progress = min(row["progress"] + 1, target)
            self.server.db.set_mission_progress(
                char_id, row["mission_id"], progress,
                1 if progress >= target else 0)
        self._sync_missions(s, char_id)

    async def h_chat(self, s: Session, msg) -> None:
        wp = s.world_player
        if wp is None:
            return
        # echo the chat request to the map (ret_chat pushes the same body)
        self.server.world.broadcast(
            wp.map_id, P.RET_CHAT, dict(msg.body), exclude=None
        )

    async def h_heart_beat(self, s: Session, msg) -> None:
        # heart_beat.request {time(0), time2(1)}
        client_time = msg.body.get(0, 0)
        s.respond(msg, {0: client_time, 1: int(time.time())})

    async def h_leave_game(self, s: Session, msg) -> None:
        wp = s.world_player
        if wp is not None:
            row = self.server.db.get_character(wp.char_id)
            if row is not None:
                self.server.db.save_position(
                    wp.char_id, wp.map_id,
                    wp.pos["x"], wp.pos["y"], wp.pos["z"], wp.pos["o"])
            self.server.world.leave(wp)
            s.world_player = None
        s.respond(msg, {})
        await s.close()

    async def h_refresh_online_state(self, s: Session, msg) -> None:
        s.respond(msg, {})

    async def h_update_client_state(self, s: Session, msg) -> None:
        s.respond(msg, {})

    async def h_game_check(self, s: Session, msg) -> None:
        s.respond(msg, {})

    async def h_retrieve_account(self, s: Session, msg) -> None:
        # Not supported in the revival (Facebook/Google bind unavailable).
        s.respond(msg, {0: 1})

    # ------------------------------------------------------------------
    # economy helpers
    # ------------------------------------------------------------------
    def _require_char(self, s: Session):
        """Return the picked character row, or None (client not ready)."""
        row = getattr(s, "picked_character", None)
        if row is not None:
            return row
        rows = self.server.db.list_characters(s.account_id or 0)
        return rows[0] if rows else None

    def _grant_mission_rewards(self, s: Session, char_id: int,
                               reward: dict) -> None:
        db = self.server.db
        gold = reward.get("gold", 0)
        diamond = reward.get("diamond", 0)
        items = reward.get("items", {})
        if gold:
            db.add_currency(char_id, economy.CURRENCY_GOLD, gold)
        if diamond:
            db.add_currency(char_id, economy.CURRENCY_DIAMOND, diamond)
        for item_id, count in items.items():
            db.add_item(char_id, item_id, count)
        stacks = [P.encode_item_stack(int(k), v)
                  for k, v in items.items()]
        s.push(P.SHOW_REWARD_ITEMS_TIPS, {
            0: gold,
            1: diamond,
            2: sproto.encode_object_array(stacks),
        })

    def _sync_missions(self, s: Session, char_id: int) -> None:
        rows = self.server.db.list_missions(char_id)
        blobs = [P.encode_mission_state(r["mission_id"], r["progress"],
                                        r["state"]) for r in rows]
        s.push(P.SYNC_MISSION, {0: sproto.encode_object_array(blobs)})

    def _sync_backpack(self, s: Session, char_id: int) -> None:
        rows = self.server.db.list_items(char_id)
        blobs = [P.encode_item_stack(r["item_id"], r["count"]) for r in rows]
        s.push(P.SYNC_BACKPACK_ITEM, {0: sproto.encode_object_array(blobs)})

    def _check_auto_complete(self, s: Session, char_id: int,
                             mission_row) -> None:
        """Flip a mission to 'complete' when its progress target is met."""
        mdef = economy.MISSIONS.get(mission_row["mission_id"])
        if mdef is None or mission_row["state"] != 0:
            return
        target = mdef.get("count", 1)
        if mission_row["progress"] >= target:
            self.server.db.set_mission_progress(
                char_id, mission_row["mission_id"], mission_row["progress"], 1)

    def _progress_buy_missions(self, s: Session, char_id: int,
                               item_id: int, count: int) -> None:
        """Advance any active 'buy' missions matching a shop purchase."""
        for row in self.server.db.list_missions(char_id):
            if row["state"] != 0:
                continue
            mdef = economy.MISSIONS.get(row["mission_id"])
            if not mdef or mdef.get("type") != "buy" or mdef.get("item_id") != item_id:
                continue
            progress = min(row["progress"] + count, mdef.get("count", 1))
            self.server.db.set_mission_progress(
                char_id, row["mission_id"], progress,
                1 if progress >= mdef.get("count", 1) else 0)
        self._sync_missions(s, char_id)

    # ------------------------------------------------------------------
    # missions
    # ------------------------------------------------------------------
    async def h_accept_mission(self, s: Session, msg) -> None:
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        char_id = row["id"]
        mission_id = msg.body.get(0)
        mdef = economy.MISSIONS.get(mission_id)
        if mdef is None:
            s.respond(msg, {0: 2})   # unknown mission
            return
        if not self.server.db.accept_mission(char_id, mission_id):
            s.respond(msg, {0: 3})   # already active
            return
        s.respond(msg, {0: 0})
        self._sync_missions(s, char_id)
        log.info("char %d accepted mission %d", char_id, mission_id)

    async def h_complete_mission(self, s: Session, msg) -> None:
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        char_id = row["id"]
        mission_id = msg.body.get(0)
        mrow = self.server.db.get_mission(char_id, mission_id)
        mdef = economy.MISSIONS.get(mission_id)
        if mrow is None or mdef is None:
            s.respond(msg, {0: 2})   # not accepted / unknown
            return
        target = mdef.get("count", 1)
        if mrow["progress"] < target or mrow["state"] != 1:
            s.respond(msg, {0: 3})   # objectives not met
            return
        s.respond(msg, {0: 0})
        # pay out and remove the mission (rewards push after the response)
        self.server.db.finish_mission(char_id, mission_id)
        self._grant_mission_rewards(s, char_id, mdef.get("reward", {}))
        self._sync_missions(s, char_id)
        self._sync_backpack(s, char_id)
        nxt = mdef.get("next")
        if nxt and nxt in economy.MISSIONS:
            self.server.db.accept_mission(char_id, nxt)
            self._sync_missions(s, char_id)
        log.info("char %d completed mission %d", char_id, mission_id)

    async def h_abandon_mission(self, s: Session, msg) -> None:
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        char_id = row["id"]
        mission_id = msg.body.get(0)
        if self.server.db.get_mission(char_id, mission_id) is None:
            s.respond(msg, {0: 2})
            return
        self.server.db.finish_mission(char_id, mission_id)
        s.respond(msg, {0: 0})
        self._sync_missions(s, char_id)

    # ------------------------------------------------------------------
    # items
    # ------------------------------------------------------------------
    async def h_use_item(self, s: Session, msg) -> None:
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        char_id = row["id"]
        item_id = msg.body.get(0)
        count = msg.body.get(1, 1)
        have = self.server.db.get_item_count(char_id, item_id)
        if have < count:
            s.respond(msg, {0: 2})   # not enough items
            return
        self.server.db.add_item(char_id, item_id, -count)
        s.respond(msg, {0: 0})
        s.push(P.UPDATE_ITEM, {0: item_id,
                               1: self.server.db.get_item_count(char_id, item_id),
                               2: 0})
        self._sync_backpack(s, char_id)

    async def h_sell_item(self, s: Session, msg) -> None:
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        char_id = row["id"]
        item_id = msg.body.get(0)
        count = msg.body.get(1, 1)
        if self.server.db.get_item_count(char_id, item_id) < count:
            s.respond(msg, {0: 2})
            return
        self.server.db.add_item(char_id, item_id, -count)
        # flat 50% of a nominal 1000-gold item value — provisional economy
        gold = 500 * count
        new_gold = self.server.db.add_currency(char_id, economy.CURRENCY_GOLD, gold)
        s.respond(msg, {0: 0})
        s.push(P.UPDATE_ITEM, {0: item_id,
                               1: self.server.db.get_item_count(char_id, item_id),
                               2: 0})
        self._sync_backpack(s, char_id)

    # ------------------------------------------------------------------
    # shop
    # ------------------------------------------------------------------
    async def h_ask_shop_list(self, s: Session, msg) -> None:
        shop_id = msg.body.get(0, 1)
        shop = economy.SHOPS.get(shop_id)
        if shop is None:
            s.respond(msg, {0: 1})   # unknown shop
            return
        goods = [P.encode_shop_good(g["goods_id"], g["item_id"], g["count"],
                                    g["currency"], g["price"])
                 for g in shop["goods"]]
        s.respond(msg, {0: 0, 1: sproto.encode_object_array(goods)})

    async def h_buy_shop_item(self, s: Session, msg) -> None:
        row = self._require_char(s)
        if row is None:
            s.respond(msg, {0: 1})
            return
        char_id = row["id"]
        goods_id = msg.body.get(0)
        count = msg.body.get(1, 1)
        good = None
        for shop in economy.SHOPS.values():
            for g in shop["goods"]:
                if g["goods_id"] == goods_id:
                    good = g
        if good is None:
            s.respond(msg, {0: 2})   # unknown goods
            return
        total = good["price"] * count
        db = self.server.db
        if db.get_currency(char_id, good["currency"]) < total:
            s.respond(msg, {0: 3})   # not enough currency
            return
        db.add_currency(char_id, good["currency"], -total)
        new_count = db.add_item(char_id, good["item_id"], good["count"] * count)
        s.respond(msg, {0: 0})
        s.push(P.UPDATE_ITEM, {0: good["item_id"], 1: new_count, 2: 0})
        self._sync_backpack(s, char_id)
        self._progress_buy_missions(s, char_id, good["item_id"], good["count"] * count)
        log.info("char %d bought goods %d x%d for %d", char_id, goods_id,
                 count, total)
