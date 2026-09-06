using System.Collections.Generic;
using Sproto;

public class NetReceiver
{
	private static ProtocolFunctionDictionary protocol = Protocol.Instance.Protocol;

	private static Dictionary<int, RpcReqHandler> rpcReqHandlerDict;

	public static void Init()
	{
		rpcReqHandlerDict = new Dictionary<int, RpcReqHandler>();
		AddHandler<Protocol.aoi_add>(aoi_add_handler.aoi_add_request);
		AddHandler<Protocol.aoi_relife_player>(aoi_relife_player_handler.aoi_relife_player_request);
		AddHandler<Protocol.aoi_remove>(aoi_remove_handler.aoi_remove_request);
		AddHandler<Protocol.aoi_social_dance>(aoi_social_dance_handler.aoi_social_dance_request);
		AddHandler<Protocol.aoi_stop_move>(aoi_stop_move_handler.aoi_stop_move_request);
		AddHandler<Protocol.aoi_update_attribute>(aoi_update_attribute_handler.aoi_update_attribute_request);
		AddHandler<Protocol.aoi_update_move>(aoi_update_move_handler.aoi_update_move_request);
		AddHandler<Protocol.apply_join_state>(apply_join_state_handler.apply_join_state_request);
		AddHandler<Protocol.apply_join_team>(apply_join_team_handler.apply_join_team_request);
		AddHandler<Protocol.ask_confirm>(ask_confirm_handler.ask_confirm_request);
		AddHandler<Protocol.ask_confirm_multi_copy_scene>(ask_confirm_multi_copy_scene_handler.ask_confirm_multi_copy_scene_request);
		AddHandler<Protocol.bar_fight_notify>(bar_fight_notify_handler.bar_fight_notify_request);
		AddHandler<Protocol.be_deleted_friend>(be_deleted_friend_handler.be_deleted_friend_request);
		AddHandler<Protocol.cancel_apply_join_team>(cancel_apply_join_team_handler.cancel_apply_join_team_request);
		AddHandler<Protocol.car_copy_result>(car_copy_result_handler.car_copy_result_request);
		AddHandler<Protocol.comb_value_up_tip>(comb_value_up_tip_handler.comb_value_up_tip_request);
		AddHandler<Protocol.copy_scene_result>(copy_scene_result_handler.copy_scene_result_request);
		AddHandler<Protocol.count_down>(count_down_handler.count_down_request);
		AddHandler<Protocol.drop_item_info>(drop_item_info_handler.drop_item_info_request);
		AddHandler<Protocol.enter_map>(enter_map_handler.enter_map_request);
		AddHandler<Protocol.get_level_reward>(get_level_reward_handler.get_level_reward_request);
		AddHandler<Protocol.grant_activity_reward>(grant_activity_reward_handler.grant_activity_reward_request);
		AddHandler<Protocol.grant_daily_mission_reward>(grant_daily_mission_reward_handler.grant_daily_mission_reward_request);
		AddHandler<Protocol.guild_battle_finish_info>(guild_battle_finish_info_handler.guild_battle_finish_info_request);
		AddHandler<Protocol.guild_battle_start>(guild_battle_start_handler.guild_battle_start_request);
		AddHandler<Protocol.guild_invite_accept>(guild_invite_accept_handler.guild_invite_accept_request);
		AddHandler<Protocol.hit_action>(hit_action_handler.hit_action_request);
		AddHandler<Protocol.invite_join_team>(invite_join_team_handler.invite_join_team_request);
		AddHandler<Protocol.login_max_count>(login_max_count_handler.login_max_count_request);
		AddHandler<Protocol.mail_delete>(mail_delete_handler.mail_delete_request);
		AddHandler<Protocol.mail_update>(mail_update_handler.mail_update_request);
		AddHandler<Protocol.main_player_create>(main_player_create_handler.main_player_create_request);
		AddHandler<Protocol.next_wave>(next_wave_handler.next_wave_request);
		AddHandler<Protocol.notice>(notice_handler.notice_request);
		AddHandler<Protocol.notice_add_friend>(notice_add_friend_handler.notice_add_friend_request);
		AddHandler<Protocol.notice_copy_scene_info>(notice_copy_scene_info_handler.notice_copy_scene_info_request);
		AddHandler<Protocol.notice_guild_battle_rank>(notice_guild_battle_rank_handler.notice_guild_battle_rank_request);
		AddHandler<Protocol.notice_money_copy_reward>(notice_money_copy_reward_handler.notice_money_copy_reward_request);
		AddHandler<Protocol.notice_relife_player>(notice_relife_player_handler.notice_relife_player_request);
		AddHandler<Protocol.notice_urge_team_leader>(notice_urge_team_leader_handler.notice_urge_team_leader_request);
		AddHandler<Protocol.notify_confirm_state>(notify_confirm_state_handler.notify_confirm_state_request);
		AddHandler<Protocol.notify_copy_start_info>(notify_copy_start_info_handler.notify_copy_start_info_request);
		AddHandler<Protocol.npc_create>(npc_create_handler.npc_create_request);
		AddHandler<Protocol.random_select_ok>(random_select_ok_handler.random_select_ok_request);
		AddHandler<Protocol.rank_pvp_create_zombie_user>(rank_pvp_create_zombie_user_handler.rank_pvp_create_zombie_user_request);
		AddHandler<Protocol.rank_pvp_history>(rank_pvp_history_handler.rank_pvp_history_request);
		AddHandler<Protocol.rank_pvp_reward>(rank_pvp_reward_handler.rank_pvp_reward_request);
		AddHandler<Protocol.rank_pvp_start>(rank_pvp_start_handler.rank_pvp_start_request);
		AddHandler<Protocol.real_pvp_start>(real_pvp_start_handler.real_pvp_start_request);
		AddHandler<Protocol.real_pvp_state>(real_pvp_state_handler.real_pvp_state_request);
		AddHandler<Protocol.req_invite_team_result>(req_invite_team_result_handler.req_invite_team_result_request);
		AddHandler<Protocol.ret_abandon_mission>(ret_abandon_mission_handler.ret_abandon_mission_request);
		AddHandler<Protocol.ret_accept_mission>(ret_accept_mission_handler.ret_accept_mission_request);
		AddHandler<Protocol.ret_add_friend>(ret_add_friend_handler.ret_add_friend_request);
		AddHandler<Protocol.ret_ask_shop_list>(ret_ask_shop_list_handler.ret_ask_shop_list_request);
		AddHandler<Protocol.ret_battle_info>(ret_battle_info_handler.ret_battle_info_request);
		AddHandler<Protocol.ret_buy_car_shop>(ret_buy_car_shop_handler.ret_buy_car_shop_request);
		AddHandler<Protocol.ret_buy_guild_goods>(ret_buy_guild_goods_handler.ret_buy_guild_goods_request);
		AddHandler<Protocol.ret_buy_invest_pack>(ret_buy_invest_pack_handler.ret_buy_invest_pack_request);
		AddHandler<Protocol.ret_buy_shop_item>(ret_buy_shop_item_handler.ret_buy_shop_item_request);
		AddHandler<Protocol.ret_chat>(ret_chat_handler.ret_chat_request);
		AddHandler<Protocol.ret_commercail_reward>(ret_commercail_reward_handler.ret_commercail_reward_request);
		AddHandler<Protocol.ret_complete_mission>(ret_complete_mission_handler.ret_complete_mission_request);
		AddHandler<Protocol.ret_consign_ask_items_info>(ret_consign_ask_items_info_handler.ret_consign_ask_items_info_request);
		AddHandler<Protocol.ret_consign_ask_my_items>(ret_consign_ask_my_items_handler.ret_consign_ask_my_items_request);
		AddHandler<Protocol.ret_consign_buy_item>(ret_consign_buy_item_handler.ret_consign_buy_item_request);
		AddHandler<Protocol.ret_consign_cancel_sale>(ret_consign_cancel_sale_handler.ret_consign_cancel_sale_request);
		AddHandler<Protocol.ret_consign_sale_item>(ret_consign_sale_item_handler.ret_consign_sale_item_request);
		AddHandler<Protocol.ret_del_friend>(ret_del_friend_handler.ret_del_friend_request);
		AddHandler<Protocol.ret_domin_info>(ret_domin_info_handler.ret_domin_info_request);
		AddHandler<Protocol.ret_enter_guild_battle>(ret_enter_guild_battle_handler.ret_enter_guild_battle_request);
		AddHandler<Protocol.ret_get_team_list>(ret_get_team_list_handler.ret_get_team_list_request);
		AddHandler<Protocol.ret_grant_tower_reward>(ret_grant_tower_reward_handler.ret_grant_tower_reward_request);
		AddHandler<Protocol.ret_guild_approve_resverve>(ret_guild_approve_resverve_handler.ret_guild_approve_resverve_request);
		AddHandler<Protocol.ret_guild_battle_guess>(ret_guild_battle_guess_handler.ret_guild_battle_guess_request);
		AddHandler<Protocol.ret_guild_battle_info>(ret_guild_battle_info_handler.ret_guild_battle_info_request);
		AddHandler<Protocol.ret_guild_battle_member>(ret_guild_battle_member_handler.ret_guild_battle_member_request);
		AddHandler<Protocol.ret_guild_battle_rank>(ret_guild_battle_rank_handler.ret_guild_battle_rank_request);
		AddHandler<Protocol.ret_guild_battle_state>(ret_guild_battle_state_handler.ret_guild_battle_state_request);
		AddHandler<Protocol.ret_guild_create>(ret_guild_create_handler.ret_guild_create_request);
		AddHandler<Protocol.ret_guild_donate>(ret_guild_donate_handler.ret_guild_donate_request);
		AddHandler<Protocol.ret_guild_job_change>(ret_guild_job_change_handler.ret_guild_job_change_request);
		AddHandler<Protocol.ret_guild_join>(ret_guild_join_handler.ret_guild_join_request);
		AddHandler<Protocol.ret_guild_kick>(ret_guild_kick_handler.ret_guild_kick_request);
		AddHandler<Protocol.ret_guild_leave>(ret_guild_leave_handler.ret_guild_leave_request);
		AddHandler<Protocol.ret_guild_log>(ret_guild_log_handler.ret_guild_log_request);
		AddHandler<Protocol.ret_guild_map_domine_top>(ret_guild_map_domine_top_handler.ret_guild_map_domine_top_request);
		AddHandler<Protocol.ret_guild_map_reward>(ret_guild_map_reward_handler.ret_guild_map_reward_request);
		AddHandler<Protocol.ret_guild_member_info>(ret_guild_member_info_handler.ret_guild_member_info_request);
		AddHandler<Protocol.ret_guild_req_info>(ret_guild_req_info_handler.ret_guild_req_info_request);
		AddHandler<Protocol.ret_guild_req_list>(ret_guild_req_list_handler.ret_guild_req_list_request);
		AddHandler<Protocol.ret_guild_score_info>(ret_guild_score_info_handler.ret_guild_score_info_request);
		AddHandler<Protocol.ret_guild_skill_level>(ret_guild_skill_level_handler.ret_guild_skill_level_request);
		AddHandler<Protocol.ret_guild_star>(ret_guild_star_handler.ret_guild_star_request);
		AddHandler<Protocol.ret_level_reward>(ret_level_reward_handler.ret_level_reward_request);
		AddHandler<Protocol.ret_mount_equip>(ret_mount_equip_handler.ret_mount_equip_request);
		AddHandler<Protocol.ret_mount_info>(ret_mount_info_handler.ret_mount_info_request);
		AddHandler<Protocol.ret_mount_use_color>(ret_mount_use_color_handler.ret_mount_use_color_request);
		AddHandler<Protocol.ret_offline_chat>(ret_offline_chat_handler.ret_offline_chat_request);
		AddHandler<Protocol.ret_open_guild_boss>(ret_open_guild_boss_handler.ret_open_guild_boss_request);
		AddHandler<Protocol.ret_open_guild_shop>(ret_open_guild_shop_handler.ret_open_guild_shop_request);
		AddHandler<Protocol.ret_open_item_package>(ret_open_item_package_handler.ret_open_item_package_request);
		AddHandler<Protocol.ret_random_online_character_list>(ret_random_online_character_list_handler.ret_random_online_character_list_request);
		AddHandler<Protocol.ret_re_name>(ret_re_name_handler.ret_re_name_request);
		AddHandler<Protocol.ret_req_guild_skill>(ret_req_guild_skill_handler.ret_req_guild_skill_request);
		AddHandler<Protocol.ret_request_30_day_info>(ret_request_30_day_info_handler.ret_request_30_day_info_request);
		AddHandler<Protocol.ret_request_activity_info>(ret_request_activity_info_handler.ret_request_activity_info_request);
		AddHandler<Protocol.ret_request_big_pack>(ret_request_big_pack_handler.ret_request_big_pack_request);
		AddHandler<Protocol.ret_request_daily_active>(ret_request_daily_active_handler.ret_request_daily_active_request);
		AddHandler<Protocol.ret_request_daily_buy>(ret_request_daily_buy_handler.ret_request_daily_buy_request);
		AddHandler<Protocol.ret_request_dance_info>(ret_request_dance_info_handler.ret_request_dance_info_request);
		AddHandler<Protocol.ret_request_first_buy>(ret_request_first_buy_handler.ret_request_first_buy_request);
		AddHandler<Protocol.ret_request_guild_boss>(ret_request_guild_boss_handler.ret_request_guild_boss_request);
		AddHandler<Protocol.ret_request_guild_map_info>(ret_request_guild_map_info_handler.ret_request_guild_map_info_request);
		AddHandler<Protocol.ret_request_invest_pack>(ret_request_invest_pack_handler.ret_request_invest_pack_request);
		AddHandler<Protocol.ret_request_level_pack>(ret_request_level_pack_handler.ret_request_level_pack_request);
		AddHandler<Protocol.ret_request_random_rank_pvp_opponent>(ret_request_random_rank_pvp_opponent_handler.ret_request_random_rank_pvp_opponent_request);
		AddHandler<Protocol.ret_request_retrieve_info>(ret_request_retrieve_info_handler.ret_request_retrieve_info_request);
		AddHandler<Protocol.ret_request_sign_week_info>(ret_request_sign_week_info_handler.ret_request_sign_week_info_request);
		AddHandler<Protocol.ret_request_survive_top>(ret_request_survive_top_handler.ret_request_survive_top_request);
		AddHandler<Protocol.ret_request_top_rank_pvp_list>(ret_request_top_rank_pvp_list_handler.ret_request_top_rank_pvp_list_request);
		AddHandler<Protocol.ret_request_tower_copy_info>(ret_request_tower_copy_info_handler.ret_request_tower_copy_info_request);
		AddHandler<Protocol.ret_request_update_friend_useinfo>(ret_request_update_friend_useinfo_handler.ret_request_update_friend_useinfo_request);
		AddHandler<Protocol.ret_request_update_storagepack>(ret_request_update_storagepack_handler.ret_request_update_storagepack_request);
		AddHandler<Protocol.ret_request_wild_boss_info>(ret_request_wild_boss_info_handler.ret_request_wild_boss_info_request);
		AddHandler<Protocol.ret_require_vip_info>(ret_require_vip_info_handler.ret_require_vip_info_request);
		AddHandler<Protocol.ret_require_vip_reward>(ret_require_vip_reward_handler.ret_require_vip_reward_request);
		AddHandler<Protocol.ret_search_guild>(ret_search_guild_handler.ret_search_guild_request);
		AddHandler<Protocol.ret_search_online_character_by_name>(ret_search_online_character_by_name_handler.ret_search_online_character_by_name_request);
		AddHandler<Protocol.ret_set_guild_battle_member>(ret_set_guild_battle_member_handler.ret_set_guild_battle_member_request);
		AddHandler<Protocol.ret_sign_30_day>(ret_sign_30_day_handler.ret_sign_30_day_request);
		AddHandler<Protocol.ret_sign_week>(ret_sign_week_handler.ret_sign_week_request);
		AddHandler<Protocol.ret_skill_use>(ret_skill_use_handler.ret_skill_use_request);
		AddHandler<Protocol.ret_slot_info>(ret_slot_info_handler.ret_slot_info_request);
		AddHandler<Protocol.ret_slot_sum_reward>(ret_slot_sum_reward_handler.ret_slot_sum_reward_request);
		AddHandler<Protocol.ret_special_big_pack>(ret_special_big_pack_handler.ret_special_big_pack_request);
		AddHandler<Protocol.ret_spin_slot>(ret_spin_slot_handler.ret_spin_slot_request);
		AddHandler<Protocol.ret_title_req_level_up>(ret_title_req_level_up_handler.ret_title_req_level_up_request);
		AddHandler<Protocol.ret_top_rank_list>(ret_top_rank_list_handler.ret_top_rank_list_request);
		AddHandler<Protocol.ret_tower_reset>(ret_tower_reset_handler.ret_tower_reset_request);
		AddHandler<Protocol.ret_tower_wipe_out>(ret_tower_wipe_out_handler.ret_tower_wipe_out_request);
		AddHandler<Protocol.ret_update_guild_star>(ret_update_guild_star_handler.ret_update_guild_star_request);
		AddHandler<Protocol.ret_use_item>(ret_use_item_handler.ret_use_item_request);
		AddHandler<Protocol.ret_watch_video_info>(ret_watch_video_info_handler.ret_watch_video_info_request);
		AddHandler<Protocol.retrieve_account>(retrieve_account_handler.retrieve_account_request);
		AddHandler<Protocol.sample_activity_result>(sample_activity_result_handler.sample_activity_result_request);
		AddHandler<Protocol.sample_copy_result>(sample_copy_result_handler.sample_copy_result_request);
		AddHandler<Protocol.send_daily_mission>(send_daily_mission_handler.send_daily_mission_request);
		AddHandler<Protocol.send_dialog_notify>(send_dialog_notify_handler.send_dialog_notify_request);
		AddHandler<Protocol.send_escort_info>(send_escort_info_handler.send_escort_info_request);
		AddHandler<Protocol.set_mission_param>(set_mission_param_handler.set_mission_param_request);
		AddHandler<Protocol.set_mission_state>(set_mission_state_handler.set_mission_state_request);
		AddHandler<Protocol.show_damage_board>(show_damage_board_handler.show_damage_board_request);
		AddHandler<Protocol.show_player_damage_board>(show_player_damage_board_handler.show_player_damage_board_request);
		AddHandler<Protocol.show_reward_items_tips>(show_reward_items_tips_handler.show_reward_items_tips_request);
		AddHandler<Protocol.start_enter_game>(start_enter_game_handler.start_enter_game_request);
		AddHandler<Protocol.start_participate_dance>(start_participate_dance_handler.start_participate_dance_request);
		AddHandler<Protocol.survive_battle_finish>(survive_battle_finish_handler.survive_battle_finish_request);
		AddHandler<Protocol.syn_friend_info>(syn_friend_info_handler.syn_friend_info_request);
		AddHandler<Protocol.syn_rank_pvp_data>(syn_rank_pvp_data_handler.syn_rank_pvp_data_request);
		AddHandler<Protocol.sync_backpack_item>(sync_backpack_item_handler.sync_backpack_item_request);
		AddHandler<Protocol.sync_badgepack_item>(sync_badgepack_item_handler.sync_badgepack_item_request);
		AddHandler<Protocol.sync_common_data>(sync_common_data_handler.sync_common_data_request);
		AddHandler<Protocol.sync_copyscenes_info>(sync_copyscenes_info_handler.sync_copyscenes_info_request);
		AddHandler<Protocol.sync_dance_state_info>(sync_dance_state_info_handler.sync_dance_state_info_request);
		AddHandler<Protocol.sync_fashion_backpack_item>(sync_fashion_backpack_item_handler.sync_fashion_backpack_item_request);
		AddHandler<Protocol.sync_guild_new_member>(sync_guild_new_member_handler.sync_guild_new_member_request);
		AddHandler<Protocol.sync_item_pack>(sync_item_pack_handler.sync_item_pack_request);
		AddHandler<Protocol.sync_mission>(sync_mission_handler.sync_mission_request);
		AddHandler<Protocol.sync_random_team_state>(sync_random_team_state_handler.sync_random_team_state_request);
		AddHandler<Protocol.sync_skill_info>(sync_skill_info_handler.sync_skill_info_request);
		AddHandler<Protocol.sync_watch_video_info>(sync_watch_video_info_handler.sync_watch_video_info_request);
		AddHandler<Protocol.tiantti_result>(tiantti_result_handler.tiantti_result_request);
		AddHandler<Protocol.update_copyscene_info>(update_copyscene_info_handler.update_copyscene_info_request);
		AddHandler<Protocol.update_item>(update_item_handler.update_item_request);
		AddHandler<Protocol.update_line_state>(update_line_state_handler.update_line_state_request);
		AddHandler<Protocol.update_queue_rank>(update_queue_rank_handler.update_queue_rank_request);
		AddHandler<Protocol.update_team>(update_team_handler.update_team_request);
		AddHandler<Protocol.update_team_member>(update_team_member_handler.update_team_member_request);
	}

	public static void AddHandler(int tag, RpcReqHandler rpcReqHandler)
	{
		rpcReqHandlerDict.Add(tag, rpcReqHandler);
	}

	public static int AddHandler<T>(RpcReqHandler rpcReqHandler)
	{
		int num = protocol[typeof(T)];
		AddHandler(num, rpcReqHandler);
		return num;
	}

	public static void RemoveHandler(int tag)
	{
		if (rpcReqHandlerDict.ContainsKey(tag))
		{
			rpcReqHandlerDict.Remove(tag);
		}
	}

	public static void RemoveHandler<T>()
	{
		RemoveHandler(protocol[typeof(T)]);
	}

	public static RpcReqHandler GetHandler(int tag)
	{
		rpcReqHandlerDict.TryGetValue(tag, out var value);
		return value;
	}

	public static RpcReqHandler GetHandler<T>()
	{
		return GetHandler(protocol[typeof(T)]);
	}
}
