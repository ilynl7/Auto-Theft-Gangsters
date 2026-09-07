using Sproto;
using SprotoType;

public class Protocol : ProtocolBase
{
	public class abandon_mission
	{
		public const int Tag = 114;
	}

	public class accept_damge
	{
		public const int Tag = 111;
	}

	public class accept_mission
	{
		public const int Tag = 112;
	}

	public class add_friend
	{
		public const int Tag = 124;
	}

	public class aoi_add
	{
		public const int Tag = 505;
	}

	public class aoi_relife_player
	{
		public const int Tag = 512;
	}

	public class aoi_remove
	{
		public const int Tag = 506;
	}

	public class aoi_social_dance
	{
		public const int Tag = 657;
	}

	public class aoi_stop_move
	{
		public const int Tag = 513;
	}

	public class aoi_update_attribute
	{
		public const int Tag = 510;
	}

	public class aoi_update_move
	{
		public const int Tag = 507;
	}

	public class apply_join_result
	{
		public const int Tag = 162;
	}

	public class apply_join_state
	{
		public const int Tag = 576;
	}

	public class apply_join_team
	{
		public const int Tag = 572;
	}

	public class approve_resverve_friend
	{
		public const int Tag = 158;
	}

	public class ask_character_info
	{
		public const int Tag = 142;
	}

	public class ask_confirm
	{
		public const int Tag = 601;
	}

	public class ask_confirm_multi_copy_scene
	{
		public const int Tag = 602;
	}

	public class ask_copyscenes_info
	{
		public const int Tag = 145;
	}

	public class ask_pickup_item
	{
		public const int Tag = 119;
	}

	public class ask_shop_list
	{
		public const int Tag = 143;
	}

	public class attack_local_npc
	{
		public const int Tag = 317;
	}

	public class attribute_inhert
	{
		public const int Tag = 302;
	}

	public class badge_merge
	{
		public const int Tag = 199;
	}

	public class bar_fight_notify
	{
		public const int Tag = 659;
	}

	public class be_deleted_friend
	{
		public const int Tag = 537;
	}

	public class buy_big_pack
	{
		public const int Tag = 272;
	}

	public class buy_car_shop
	{
		public const int Tag = 323;
	}

	public class buy_invest_pack
	{
		public const int Tag = 271;
	}

	public class buy_shop_item
	{
		public const int Tag = 144;
	}

	public class cancel_apply_join_team
	{
		public const int Tag = 573;
	}

	public class car_chase_result
	{
		public const int Tag = 193;
	}

	public class car_copy_result
	{
		public const int Tag = 608;
	}

	public class change_item_state
	{
		public const int Tag = 240;
	}

	public class change_mount_state
	{
		public const int Tag = 267;
	}

	public class change_potion
	{
		public const int Tag = 185;
	}

	public class change_scene_line
	{
		public const int Tag = 155;
	}

	public class change_show_type
	{
		public const int Tag = 223;
	}

	public class change_skill_index
	{
		public const int Tag = 316;
	}

	public class change_skill_position
	{
		public const int Tag = 192;
	}

	public class character_create
	{
		public const int Tag = 104;
	}

	public class character_list
	{
		public const int Tag = 103;
	}

	public class character_pick
	{
		public const int Tag = 105;
	}

	public class chat
	{
		public const int Tag = 120;
	}

	public class check_purchase
	{
		public const int Tag = 266;
	}

	public class comb_value_up_tip
	{
		public const int Tag = 651;
	}

	public class complete_mission
	{
		public const int Tag = 113;
	}

	public class consign_ask_items_info
	{
		public const int Tag = 189;
	}

	public class consign_ask_my_items
	{
		public const int Tag = 188;
	}

	public class consign_buy_item
	{
		public const int Tag = 190;
	}

	public class consign_cancel_sale
	{
		public const int Tag = 187;
	}

	public class consign_sale_item
	{
		public const int Tag = 186;
	}

	public class continue_tower_copy
	{
		public const int Tag = 205;
	}

	public class copy_scene_result
	{
		public const int Tag = 552;
	}

	public class copy_swipe_out
	{
		public const int Tag = 232;
	}

	public class count_down
	{
		public const int Tag = 553;
	}

	public class del_friend
	{
		public const int Tag = 125;
	}

	public class download_finish
	{
		public const int Tag = 270;
	}

	public class drop_item_info
	{
		public const int Tag = 527;
	}

	public class enter_bar_fight
	{
		public const int Tag = 207;
	}

	public class enter_copy_scene
	{
		public const int Tag = 107;
	}

	public class enter_domin_pk_scene
	{
		public const int Tag = 311;
	}

	public class enter_empty_scene
	{
		public const int Tag = 451;
	}

	public class enter_guild_battle
	{
		public const int Tag = 286;
	}

	public class enter_guild_boss_scene
	{
		public const int Tag = 194;
	}

	public class enter_guild_city_scene
	{
		public const int Tag = 322;
	}

	public class enter_map
	{
		public const int Tag = 503;
	}

	public class enter_multi_copy_scene_confirm
	{
		public const int Tag = 215;
	}

	public class enter_new_map
	{
		public const int Tag = 106;
	}

	public class enter_scuffle_batttle
	{
		public const int Tag = 273;
	}

	public class enter_single_exp_scene
	{
		public const int Tag = 276;
	}

	public class enter_survive_batttle
	{
		public const int Tag = 246;
	}

	public class enter_teleport_point
	{
		public const int Tag = 251;
	}

	public class enter_tower_copy_info
	{
		public const int Tag = 204;
	}

	public class enter_wild_boss
	{
		public const int Tag = 201;
	}

	public class equip_appraise
	{
		public const int Tag = 303;
	}

	public class equip_badge
	{
		public const int Tag = 197;
	}

	public class equip_enhance
	{
		public const int Tag = 167;
	}

	public class equip_fashion_item
	{
		public const int Tag = 221;
	}

	public class equip_inhert
	{
		public const int Tag = 184;
	}

	public class equip_inlay
	{
		public const int Tag = 304;
	}

	public class equip_item
	{
		public const int Tag = 116;
	}

	public class equip_refine
	{
		public const int Tag = 183;
	}

	public class facebook_link
	{
		public const int Tag = 5;
	}

	public class facebook_unlink
	{
		public const int Tag = 6;
	}

	public class game_check
	{
		public const int Tag = 308;
	}

	public class gather_other_player
	{
		public const int Tag = 321;
	}

	public class gather_team
	{
		public const int Tag = 177;
	}

	public class get_level_reward
	{
		public const int Tag = 675;
	}

	public class get_team_list
	{
		public const int Tag = 165;
	}

	public class grant_activity_reward
	{
		public const int Tag = 620;
	}

	public class grant_daily_mission_reward
	{
		public const int Tag = 612;
	}

	public class grant_tower_reward
	{
		public const int Tag = 203;
	}

	public class guild_approve_resverve
	{
		public const int Tag = 154;
	}

	public class guild_battle_finish_info
	{
		public const int Tag = 668;
	}

	public class guild_battle_guess
	{
		public const int Tag = 290;
	}

	public class guild_battle_start
	{
		public const int Tag = 670;
	}

	public class guild_create
	{
		public const int Tag = 146;
	}

	public class guild_donate
	{
		public const int Tag = 175;
	}

	public class guild_invite
	{
		public const int Tag = 247;
	}

	public class guild_invite_accept
	{
		public const int Tag = 639;
	}

	public class guild_job_change
	{
		public const int Tag = 150;
	}

	public class guild_join
	{
		public const int Tag = 147;
	}

	public class guild_kick
	{
		public const int Tag = 149;
	}

	public class guild_leave
	{
		public const int Tag = 148;
	}

	public class guild_log
	{
		public const int Tag = 174;
	}

	public class guild_req_info
	{
		public const int Tag = 153;
	}

	public class guild_req_list
	{
		public const int Tag = 152;
	}

	public class guild_skill_level
	{
		public const int Tag = 151;
	}

	public class heart_beat
	{
		public const int Tag = 218;
	}

	public class hit_action
	{
		public const int Tag = 514;
	}

	public class impact_npc
	{
		public const int Tag = 298;
	}

	public class invite_join_team
	{
		public const int Tag = 516;
	}

	public class leave_copy_scene
	{
		public const int Tag = 108;
	}

	public class leave_game
	{
		public const int Tag = 234;
	}

	public class leave_team
	{
		public const int Tag = 166;
	}

	public class local_character_attack
	{
		public const int Tag = 128;
	}

	public class local_npc_die
	{
		public const int Tag = 307;
	}

	public class login
	{
		public const int Tag = 4;
	}

	public class login_max_count
	{
		public const int Tag = 578;
	}

	public class mail_delete
	{
		public const int Tag = 532;
	}

	public class mail_operation
	{
		public const int Tag = 123;
	}

	public class mail_update
	{
		public const int Tag = 531;
	}

	public class main_player_create
	{
		public const int Tag = 504;
	}

	public class map_ready
	{
		public const int Tag = 100;
	}

	public class mount_equip
	{
		public const int Tag = 236;
	}

	public class mount_unequip
	{
		public const int Tag = 237;
	}

	public class mount_use_color
	{
		public const int Tag = 241;
	}

	public class move
	{
		public const int Tag = 101;
	}

	public class next_wave
	{
		public const int Tag = 515;
	}

	public class notice
	{
		public const int Tag = 529;
	}

	public class notice_add_friend
	{
		public const int Tag = 536;
	}

	public class notice_copy_scene_info
	{
		public const int Tag = 683;
	}

	public class notice_guild_battle_rank
	{
		public const int Tag = 676;
	}

	public class notice_money_copy_reward
	{
		public const int Tag = 615;
	}

	public class notice_relife_player
	{
		public const int Tag = 618;
	}

	public class notice_urge_team_leader
	{
		public const int Tag = 682;
	}

	public class notify_confirm_state
	{
		public const int Tag = 603;
	}

	public class notify_copy_start_info
	{
		public const int Tag = 629;
	}

	public class npc_create
	{
		public const int Tag = 509;
	}

	public class open_guild_boss
	{
		public const int Tag = 233;
	}

	public class open_item_package
	{
		public const int Tag = 224;
	}

	public class open_multi_tower_reward
	{
		public const int Tag = 283;
	}

	public class pause_participate_dance
	{
		public const int Tag = 228;
	}

	public class play_social_dance
	{
		public const int Tag = 275;
	}

	public class put_item_storagepack
	{
		public const int Tag = 140;
	}

	public class random_select_ok
	{
		public const int Tag = 610;
	}

	public class random_select_team
	{
		public const int Tag = 214;
	}

	public class rank_pvp_create_zombie_user
	{
		public const int Tag = 544;
	}

	public class rank_pvp_history
	{
		public const int Tag = 546;
	}

	public class rank_pvp_other_player_die
	{
		public const int Tag = 137;
	}

	public class rank_pvp_player_attack
	{
		public const int Tag = 136;
	}

	public class rank_pvp_reward
	{
		public const int Tag = 545;
	}

	public class rank_pvp_start
	{
		public const int Tag = 547;
	}

	public class re_name
	{
		public const int Tag = 301;
	}

	public class real_pvp_register
	{
		public const int Tag = 138;
	}

	public class real_pvp_start
	{
		public const int Tag = 549;
	}

	public class real_pvp_state
	{
		public const int Tag = 548;
	}

	public class receive_level_reward
	{
		public const int Tag = 297;
	}

	public class refresh_online_misison
	{
		public const int Tag = 309;
	}

	public class refresh_online_state
	{
		public const int Tag = 281;
	}

	public class relife_player
	{
		public const int Tag = 132;
	}

	public class req_buy_guild_goods
	{
		public const int Tag = 172;
	}

	public class req_change_team_goal
	{
		public const int Tag = 213;
	}

	public class req_guild_battle_guess
	{
		public const int Tag = 292;
	}

	public class req_guild_battle_info
	{
		public const int Tag = 285;
	}

	public class req_guild_battle_member
	{
		public const int Tag = 288;
	}

	public class req_guild_battle_rank
	{
		public const int Tag = 287;
	}

	public class req_guild_battle_state
	{
		public const int Tag = 295;
	}

	public class req_guild_member_info
	{
		public const int Tag = 170;
	}

	public class req_guild_notice
	{
		public const int Tag = 169;
	}

	public class req_guild_score_info
	{
		public const int Tag = 291;
	}

	public class req_guild_skill
	{
		public const int Tag = 212;
	}

	public class req_guild_star
	{
		public const int Tag = 293;
	}

	public class req_invite_team
	{
		public const int Tag = 109;
	}

	public class req_invite_team_result
	{
		public const int Tag = 517;
	}

	public class req_join_team
	{
		public const int Tag = 161;
	}

	public class req_level_reward
	{
		public const int Tag = 296;
	}

	public class req_offline_chat
	{
		public const int Tag = 168;
	}

	public class req_open_guild_shop
	{
		public const int Tag = 171;
	}

	public class req_other_team
	{
		public const int Tag = 248;
	}

	public class req_random_online_character_list
	{
		public const int Tag = 159;
	}

	public class req_seting_guild_appro
	{
		public const int Tag = 173;
	}

	public class request_activity_info
	{
		public const int Tag = 225;
	}

	public class request_bar_fight
	{
		public const int Tag = 206;
	}

	public class request_battle_info
	{
		public const int Tag = 231;
	}

	public class request_big_pack
	{
		public const int Tag = 260;
	}

	public class request_change_pk_mode
	{
		public const int Tag = 226;
	}

	public class request_daily_active
	{
		public const int Tag = 261;
	}

	public class request_daily_buy
	{
		public const int Tag = 258;
	}

	public class request_daily_mission
	{
		public const int Tag = 121;
	}

	public class request_dance_info
	{
		public const int Tag = 227;
	}

	public class request_dance_state_info
	{
		public const int Tag = 313;
	}

	public class request_domin_info
	{
		public const int Tag = 310;
	}

	public class request_first_buy
	{
		public const int Tag = 259;
	}

	public class request_guild_boss
	{
		public const int Tag = 195;
	}

	public class request_guild_map_domine_top
	{
		public const int Tag = 318;
	}

	public class request_guild_map_info
	{
		public const int Tag = 319;
	}

	public class request_guild_map_reward
	{
		public const int Tag = 320;
	}

	public class request_guild_reward
	{
		public const int Tag = 196;
	}

	public class request_invest_pack
	{
		public const int Tag = 257;
	}

	public class request_level_pack
	{
		public const int Tag = 256;
	}

	public class request_line_state
	{
		public const int Tag = 219;
	}

	public class request_mount_info
	{
		public const int Tag = 235;
	}

	public class request_random_name
	{
		public const int Tag = 118;
	}

	public class request_random_rank_pvp_opponent
	{
		public const int Tag = 133;
	}

	public class request_rank_pvp_data
	{
		public const int Tag = 210;
	}

	public class request_rank_pvp_history
	{
		public const int Tag = 211;
	}

	public class request_retrieve
	{
		public const int Tag = 279;
	}

	public class request_retrieve_info
	{
		public const int Tag = 278;
	}

	public class request_sign_30_day_info
	{
		public const int Tag = 252;
	}

	public class request_sign_week_info
	{
		public const int Tag = 253;
	}

	public class request_slot_info
	{
		public const int Tag = 242;
	}

	public class request_slot_reward
	{
		public const int Tag = 249;
	}

	public class request_slot_sum_reward
	{
		public const int Tag = 244;
	}

	public class request_special_big_pack
	{
		public const int Tag = 274;
	}

	public class request_survive_top
	{
		public const int Tag = 245;
	}

	public class request_top_rank_list
	{
		public const int Tag = 191;
	}

	public class request_top_rank_pvp_list
	{
		public const int Tag = 134;
	}

	public class request_tower_copy_info
	{
		public const int Tag = 202;
	}

	public class request_update_friend_useinfo
	{
		public const int Tag = 126;
	}

	public class request_update_storagepack
	{
		public const int Tag = 139;
	}

	public class request_wild_boss_info
	{
		public const int Tag = 200;
	}

	public class require_daily_active_reward
	{
		public const int Tag = 265;
	}

	public class require_domin_rewards
	{
		public const int Tag = 312;
	}

	public class require_first_buy_reward
	{
		public const int Tag = 264;
	}

	public class require_invest_reward
	{
		public const int Tag = 263;
	}

	public class require_level_reward
	{
		public const int Tag = 262;
	}

	public class require_vip_info
	{
		public const int Tag = 299;
	}

	public class require_vip_reward
	{
		public const int Tag = 300;
	}

	public class ret_abandon_mission
	{
		public const int Tag = 522;
	}

	public class ret_accept_mission
	{
		public const int Tag = 520;
	}

	public class ret_add_friend
	{
		public const int Tag = 533;
	}

	public class ret_ask_confirm_multi_copy_scene
	{
		public const int Tag = 217;
	}

	public class ret_ask_shop_list
	{
		public const int Tag = 554;
	}

	public class ret_battle_info
	{
		public const int Tag = 627;
	}

	public class ret_buy_car_shop
	{
		public const int Tag = 691;
	}

	public class ret_buy_guild_goods
	{
		public const int Tag = 583;
	}

	public class ret_buy_invest_pack
	{
		public const int Tag = 655;
	}

	public class ret_buy_shop_item
	{
		public const int Tag = 652;
	}

	public class ret_chat
	{
		public const int Tag = 528;
	}

	public class ret_commercail_reward
	{
		public const int Tag = 650;
	}

	public class ret_complete_mission
	{
		public const int Tag = 521;
	}

	public class ret_consign_ask_items_info
	{
		public const int Tag = 596;
	}

	public class ret_consign_ask_my_items
	{
		public const int Tag = 595;
	}

	public class ret_consign_buy_item
	{
		public const int Tag = 597;
	}

	public class ret_consign_cancel_sale
	{
		public const int Tag = 594;
	}

	public class ret_consign_sale_item
	{
		public const int Tag = 593;
	}

	public class ret_del_friend
	{
		public const int Tag = 535;
	}

	public class ret_domin_info
	{
		public const int Tag = 684;
	}

	public class ret_enter_guild_battle
	{
		public const int Tag = 677;
	}

	public class ret_get_team_list
	{
		public const int Tag = 574;
	}

	public class ret_grant_tower_reward
	{
		public const int Tag = 625;
	}

	public class ret_guild_approve_resverve
	{
		public const int Tag = 591;
	}

	public class ret_guild_battle_guess
	{
		public const int Tag = 669;
	}

	public class ret_guild_battle_info
	{
		public const int Tag = 662;
	}

	public class ret_guild_battle_member
	{
		public const int Tag = 665;
	}

	public class ret_guild_battle_rank
	{
		public const int Tag = 663;
	}

	public class ret_guild_battle_state
	{
		public const int Tag = 673;
	}

	public class ret_guild_create
	{
		public const int Tag = 567;
	}

	public class ret_guild_donate
	{
		public const int Tag = 585;
	}

	public class ret_guild_job_change
	{
		public const int Tag = 589;
	}

	public class ret_guild_join
	{
		public const int Tag = 566;
	}

	public class ret_guild_kick
	{
		public const int Tag = 590;
	}

	public class ret_guild_leave
	{
		public const int Tag = 565;
	}

	public class ret_guild_log
	{
		public const int Tag = 584;
	}

	public class ret_guild_map_domine_top
	{
		public const int Tag = 688;
	}

	public class ret_guild_map_reward
	{
		public const int Tag = 690;
	}

	public class ret_guild_member_info
	{
		public const int Tag = 581;
	}

	public class ret_guild_req_info
	{
		public const int Tag = 563;
	}

	public class ret_guild_req_list
	{
		public const int Tag = 562;
	}

	public class ret_guild_score_info
	{
		public const int Tag = 667;
	}

	public class ret_guild_skill_level
	{
		public const int Tag = 564;
	}

	public class ret_guild_star
	{
		public const int Tag = 671;
	}

	public class ret_invite_join_team
	{
		public const int Tag = 110;
	}

	public class ret_level_reward
	{
		public const int Tag = 674;
	}

	public class ret_mount_equip
	{
		public const int Tag = 631;
	}

	public class ret_mount_info
	{
		public const int Tag = 630;
	}

	public class ret_mount_use_color
	{
		public const int Tag = 632;
	}

	public class ret_offline_chat
	{
		public const int Tag = 579;
	}

	public class ret_open_guild_boss
	{
		public const int Tag = 628;
	}

	public class ret_open_guild_shop
	{
		public const int Tag = 582;
	}

	public class ret_open_item_package
	{
		public const int Tag = 617;
	}

	public class ret_random_online_character_list
	{
		public const int Tag = 570;
	}

	public class ret_re_name
	{
		public const int Tag = 680;
	}

	public class ret_req_guild_skill
	{
		public const int Tag = 609;
	}

	public class ret_request_30_day_info
	{
		public const int Tag = 640;
	}

	public class ret_request_activity_info
	{
		public const int Tag = 619;
	}

	public class ret_request_big_pack
	{
		public const int Tag = 648;
	}

	public class ret_request_daily_active
	{
		public const int Tag = 649;
	}

	public class ret_request_daily_buy
	{
		public const int Tag = 646;
	}

	public class ret_request_dance_info
	{
		public const int Tag = 623;
	}

	public class ret_request_first_buy
	{
		public const int Tag = 647;
	}

	public class ret_request_guild_boss
	{
		public const int Tag = 599;
	}

	public class ret_request_guild_map_info
	{
		public const int Tag = 689;
	}

	public class ret_request_invest_pack
	{
		public const int Tag = 645;
	}

	public class ret_request_level_pack
	{
		public const int Tag = 644;
	}

	public class ret_request_random_rank_pvp_opponent
	{
		public const int Tag = 542;
	}

	public class ret_request_retrieve_info
	{
		public const int Tag = 658;
	}

	public class ret_request_sign_week_info
	{
		public const int Tag = 641;
	}

	public class ret_request_survive_top
	{
		public const int Tag = 636;
	}

	public class ret_request_top_rank_pvp_list
	{
		public const int Tag = 543;
	}

	public class ret_request_tower_copy_info
	{
		public const int Tag = 606;
	}

	public class ret_request_update_friend_useinfo
	{
		public const int Tag = 534;
	}

	public class ret_request_update_storagepack
	{
		public const int Tag = 550;
	}

	public class ret_request_wild_boss_info
	{
		public const int Tag = 605;
	}

	public class ret_require_vip_info
	{
		public const int Tag = 678;
	}

	public class ret_require_vip_reward
	{
		public const int Tag = 679;
	}

	public class ret_search_guild
	{
		public const int Tag = 586;
	}

	public class ret_search_online_character_by_name
	{
		public const int Tag = 571;
	}

	public class ret_set_guild_battle_member
	{
		public const int Tag = 666;
	}

	public class ret_sign_30_day
	{
		public const int Tag = 642;
	}

	public class ret_sign_week
	{
		public const int Tag = 643;
	}

	public class ret_skill_use
	{
		public const int Tag = 508;
	}

	public class ret_slot_info
	{
		public const int Tag = 633;
	}

	public class ret_slot_sum_reward
	{
		public const int Tag = 635;
	}

	public class ret_special_big_pack
	{
		public const int Tag = 656;
	}

	public class ret_spin_slot
	{
		public const int Tag = 634;
	}

	public class ret_title_req_level_up
	{
		public const int Tag = 569;
	}

	public class ret_top_rank_list
	{
		public const int Tag = 598;
	}

	public class ret_tower_reset
	{
		public const int Tag = 626;
	}

	public class ret_tower_wipe_out
	{
		public const int Tag = 607;
	}

	public class ret_update_guild_star
	{
		public const int Tag = 672;
	}

	public class ret_use_item
	{
		public const int Tag = 526;
	}

	public class ret_watch_video_info
	{
		public const int Tag = 693;
	}

	public class retrieve_account
	{
		public const int Tag = 660;
	}

	public class sample_activity_result
	{
		public const int Tag = 685;
	}

	public class sample_copy_result
	{
		public const int Tag = 613;
	}

	public class search_guild
	{
		public const int Tag = 176;
	}

	public class search_online_character_by_name
	{
		public const int Tag = 160;
	}

	public class select_pk_character
	{
		public const int Tag = 135;
	}

	public class sell_item
	{
		public const int Tag = 129;
	}

	public class send_daily_mission
	{
		public const int Tag = 530;
	}

	public class send_dialog_notify
	{
		public const int Tag = 653;
	}

	public class send_escort_info
	{
		public const int Tag = 621;
	}

	public class send_mail
	{
		public const int Tag = 122;
	}

	public class send_mail_box
	{
		public const int Tag = 284;
	}

	public class set_guild_battle_member
	{
		public const int Tag = 289;
	}

	public class set_mission_param
	{
		public const int Tag = 524;
	}

	public class set_mission_state
	{
		public const int Tag = 523;
	}

	public class show_damage_board
	{
		public const int Tag = 511;
	}

	public class show_player_damage_board
	{
		public const int Tag = 687;
	}

	public class show_reward_items_tips
	{
		public const int Tag = 638;
	}

	public class sign_30_day
	{
		public const int Tag = 254;
	}

	public class sign_bar_fight
	{
		public const int Tag = 282;
	}

	public class sign_week
	{
		public const int Tag = 255;
	}

	public class single_copy_scene_npc_die
	{
		public const int Tag = 127;
	}

	public class skill_level_up
	{
		public const int Tag = 130;
	}

	public class skill_use
	{
		public const int Tag = 102;
	}

	public class spin_slot
	{
		public const int Tag = 243;
	}

	public class start_battle
	{
		public const int Tag = 220;
	}

	public class start_download
	{
		public const int Tag = 269;
	}

	public class start_enter_game
	{
		public const int Tag = 654;
	}

	public class start_participate_dance
	{
		public const int Tag = 624;
	}

	public class stop_leave_copy
	{
		public const int Tag = 250;
	}

	public class stop_random_select_team
	{
		public const int Tag = 216;
	}

	public class survive_battle_finish
	{
		public const int Tag = 637;
	}

	public class syn_friend_info
	{
		public const int Tag = 538;
	}

	public class syn_rank_pvp_data
	{
		public const int Tag = 541;
	}

	public class sync_backpack_item
	{
		public const int Tag = 592;
	}

	public class sync_badgepack_item
	{
		public const int Tag = 604;
	}

	public class sync_common_data
	{
		public const int Tag = 614;
	}

	public class sync_copyscenes_info
	{
		public const int Tag = 555;
	}

	public class sync_dance_state_info
	{
		public const int Tag = 686;
	}

	public class sync_fashion_backpack_item
	{
		public const int Tag = 616;
	}

	public class sync_guild_new_member
	{
		public const int Tag = 580;
	}

	public class sync_item_pack
	{
		public const int Tag = 611;
	}

	public class sync_mission
	{
		public const int Tag = 519;
	}

	public class sync_random_team_state
	{
		public const int Tag = 681;
	}

	public class sync_skill_info
	{
		public const int Tag = 540;
	}

	public class sync_watch_video_info
	{
		public const int Tag = 692;
	}

	public class take_item_storagepack
	{
		public const int Tag = 141;
	}

	public class team_kick
	{
		public const int Tag = 164;
	}

	public class tianti_req_win_count_rewards
	{
		public const int Tag = 157;
	}

	public class tiantti_result
	{
		public const int Tag = 551;
	}

	public class title_req_level_up
	{
		public const int Tag = 156;
	}

	public class tower_reset
	{
		public const int Tag = 230;
	}

	public class tower_wipe_out
	{
		public const int Tag = 208;
	}

	public class tutorial_finish
	{
		public const int Tag = 306;
	}

	public class unequip_badge
	{
		public const int Tag = 198;
	}

	public class unequip_fashion_item
	{
		public const int Tag = 222;
	}

	public class unequip_item
	{
		public const int Tag = 117;
	}

	public class unlock_function_complete
	{
		public const int Tag = 268;
	}

	public class unuse_mount
	{
		public const int Tag = 239;
	}

	public class update_client_state
	{
		public const int Tag = 280;
	}

	public class update_copyscene_info
	{
		public const int Tag = 561;
	}

	public class update_game_server
	{
		public const int Tag = 7;
	}

	public class update_guild_dance_time
	{
		public const int Tag = 315;
	}

	public class update_guild_star
	{
		public const int Tag = 294;
	}

	public class update_item
	{
		public const int Tag = 525;
	}

	public class update_line_state
	{
		public const int Tag = 568;
	}

	public class update_misison_complete
	{
		public const int Tag = 182;
	}

	public class update_misison_parm
	{
		public const int Tag = 178;
	}

	public class update_player_map_info
	{
		public const int Tag = 324;
	}

	public class update_queue_rank
	{
		public const int Tag = 577;
	}

	public class update_sex_mini_score
	{
		public const int Tag = 277;
	}

	public class update_team
	{
		public const int Tag = 518;
	}

	public class update_team_member
	{
		public const int Tag = 575;
	}

	public class update_team_setting
	{
		public const int Tag = 163;
	}

	public class urge_team_leader
	{
		public const int Tag = 450;
	}

	public class use_dance
	{
		public const int Tag = 229;
	}

	public class use_dance_sound_box
	{
		public const int Tag = 314;
	}

	public class use_item
	{
		public const int Tag = 115;
	}

	public class use_mount
	{
		public const int Tag = 238;
	}

	public class use_skill_buff
	{
		public const int Tag = 209;
	}

	public class verfiy
	{
		public const int Tag = 3;
	}

	public class visitor
	{
		public const int Tag = 2;
	}

	public class watch_video_info
	{
		public const int Tag = 452;
	}

	public class weapon_inhert
	{
		public const int Tag = 305;
	}

	public static Protocol Instance = new Protocol();

	private Protocol()
	{
		base.Protocol.SetProtocol<abandon_mission>(114);
		base.Protocol.SetRequest<SprotoType.abandon_mission.request>(114);
		base.Protocol.SetProtocol<accept_damge>(111);
		base.Protocol.SetRequest<SprotoType.accept_damge.request>(111);
		base.Protocol.SetProtocol<accept_mission>(112);
		base.Protocol.SetRequest<SprotoType.accept_mission.request>(112);
		base.Protocol.SetProtocol<add_friend>(124);
		base.Protocol.SetRequest<SprotoType.add_friend.request>(124);
		base.Protocol.SetProtocol<aoi_add>(505);
		base.Protocol.SetRequest<SprotoType.aoi_add.request>(505);
		base.Protocol.SetProtocol<aoi_relife_player>(512);
		base.Protocol.SetRequest<SprotoType.aoi_relife_player.request>(512);
		base.Protocol.SetProtocol<aoi_remove>(506);
		base.Protocol.SetRequest<SprotoType.aoi_remove.request>(506);
		base.Protocol.SetProtocol<aoi_social_dance>(657);
		base.Protocol.SetRequest<SprotoType.aoi_social_dance.request>(657);
		base.Protocol.SetProtocol<aoi_stop_move>(513);
		base.Protocol.SetRequest<SprotoType.aoi_stop_move.request>(513);
		base.Protocol.SetProtocol<aoi_update_attribute>(510);
		base.Protocol.SetRequest<SprotoType.aoi_update_attribute.request>(510);
		base.Protocol.SetProtocol<aoi_update_move>(507);
		base.Protocol.SetRequest<SprotoType.aoi_update_move.request>(507);
		base.Protocol.SetProtocol<apply_join_result>(162);
		base.Protocol.SetRequest<SprotoType.apply_join_result.request>(162);
		base.Protocol.SetProtocol<apply_join_state>(576);
		base.Protocol.SetRequest<SprotoType.apply_join_state.request>(576);
		base.Protocol.SetProtocol<apply_join_team>(572);
		base.Protocol.SetRequest<SprotoType.apply_join_team.request>(572);
		base.Protocol.SetProtocol<approve_resverve_friend>(158);
		base.Protocol.SetRequest<SprotoType.approve_resverve_friend.request>(158);
		base.Protocol.SetProtocol<ask_character_info>(142);
		base.Protocol.SetRequest<SprotoType.ask_character_info.request>(142);
		base.Protocol.SetResponse<SprotoType.ask_character_info.response>(142);
		base.Protocol.SetProtocol<ask_confirm>(601);
		base.Protocol.SetRequest<SprotoType.ask_confirm.request>(601);
		base.Protocol.SetResponse<SprotoType.ask_confirm.response>(601);
		base.Protocol.SetProtocol<ask_confirm_multi_copy_scene>(602);
		base.Protocol.SetRequest<SprotoType.ask_confirm_multi_copy_scene.request>(602);
		base.Protocol.SetProtocol<ask_copyscenes_info>(145);
		base.Protocol.SetRequest<SprotoType.ask_copyscenes_info.request>(145);
		base.Protocol.SetProtocol<ask_pickup_item>(119);
		base.Protocol.SetRequest<SprotoType.ask_pickup_item.request>(119);
		base.Protocol.SetProtocol<ask_shop_list>(143);
		base.Protocol.SetRequest<SprotoType.ask_shop_list.request>(143);
		base.Protocol.SetProtocol<attack_local_npc>(317);
		base.Protocol.SetRequest<SprotoType.attack_local_npc.request>(317);
		base.Protocol.SetProtocol<attribute_inhert>(302);
		base.Protocol.SetRequest<SprotoType.attribute_inhert.request>(302);
		base.Protocol.SetProtocol<badge_merge>(199);
		base.Protocol.SetRequest<SprotoType.badge_merge.request>(199);
		base.Protocol.SetResponse<SprotoType.badge_merge.response>(199);
		base.Protocol.SetProtocol<bar_fight_notify>(659);
		base.Protocol.SetRequest<SprotoType.bar_fight_notify.request>(659);
		base.Protocol.SetProtocol<be_deleted_friend>(537);
		base.Protocol.SetRequest<SprotoType.be_deleted_friend.request>(537);
		base.Protocol.SetProtocol<buy_big_pack>(272);
		base.Protocol.SetRequest<SprotoType.buy_big_pack.request>(272);
		base.Protocol.SetProtocol<buy_car_shop>(323);
		base.Protocol.SetRequest<SprotoType.buy_car_shop.request>(323);
		base.Protocol.SetProtocol<buy_invest_pack>(271);
		base.Protocol.SetRequest<SprotoType.buy_invest_pack.request>(271);
		base.Protocol.SetProtocol<buy_shop_item>(144);
		base.Protocol.SetRequest<SprotoType.buy_shop_item.request>(144);
		base.Protocol.SetProtocol<cancel_apply_join_team>(573);
		base.Protocol.SetRequest<SprotoType.cancel_apply_join_team.request>(573);
		base.Protocol.SetProtocol<car_chase_result>(193);
		base.Protocol.SetRequest<SprotoType.car_chase_result.request>(193);
		base.Protocol.SetProtocol<car_copy_result>(608);
		base.Protocol.SetRequest<SprotoType.car_copy_result.request>(608);
		base.Protocol.SetProtocol<change_item_state>(240);
		base.Protocol.SetRequest<SprotoType.change_item_state.request>(240);
		base.Protocol.SetProtocol<change_mount_state>(267);
		base.Protocol.SetRequest<SprotoType.change_mount_state.request>(267);
		base.Protocol.SetProtocol<change_potion>(185);
		base.Protocol.SetRequest<SprotoType.change_potion.request>(185);
		base.Protocol.SetProtocol<change_scene_line>(155);
		base.Protocol.SetRequest<SprotoType.change_scene_line.request>(155);
		base.Protocol.SetProtocol<change_show_type>(223);
		base.Protocol.SetRequest<SprotoType.change_show_type.request>(223);
		base.Protocol.SetProtocol<change_skill_index>(316);
		base.Protocol.SetRequest<SprotoType.change_skill_index.request>(316);
		base.Protocol.SetProtocol<change_skill_position>(192);
		base.Protocol.SetRequest<SprotoType.change_skill_position.request>(192);
		base.Protocol.SetProtocol<character_create>(104);
		base.Protocol.SetRequest<SprotoType.character_create.request>(104);
		base.Protocol.SetResponse<SprotoType.character_create.response>(104);
		base.Protocol.SetProtocol<character_list>(103);
		base.Protocol.SetResponse<SprotoType.character_list.response>(103);
		base.Protocol.SetProtocol<character_pick>(105);
		base.Protocol.SetRequest<SprotoType.character_pick.request>(105);
		base.Protocol.SetResponse<SprotoType.character_pick.response>(105);
		base.Protocol.SetProtocol<chat>(120);
		base.Protocol.SetRequest<SprotoType.chat.request>(120);
		base.Protocol.SetProtocol<check_purchase>(266);
		base.Protocol.SetRequest<SprotoType.check_purchase.request>(266);
		base.Protocol.SetProtocol<comb_value_up_tip>(651);
		base.Protocol.SetRequest<SprotoType.comb_value_up_tip.request>(651);
		base.Protocol.SetProtocol<complete_mission>(113);
		base.Protocol.SetRequest<SprotoType.complete_mission.request>(113);
		base.Protocol.SetProtocol<consign_ask_items_info>(189);
		base.Protocol.SetRequest<SprotoType.consign_ask_items_info.request>(189);
		base.Protocol.SetProtocol<consign_ask_my_items>(188);
		base.Protocol.SetRequest<SprotoType.consign_ask_my_items.request>(188);
		base.Protocol.SetProtocol<consign_buy_item>(190);
		base.Protocol.SetRequest<SprotoType.consign_buy_item.request>(190);
		base.Protocol.SetProtocol<consign_cancel_sale>(187);
		base.Protocol.SetRequest<SprotoType.consign_cancel_sale.request>(187);
		base.Protocol.SetProtocol<consign_sale_item>(186);
		base.Protocol.SetRequest<SprotoType.consign_sale_item.request>(186);
		base.Protocol.SetProtocol<continue_tower_copy>(205);
		base.Protocol.SetRequest<SprotoType.continue_tower_copy.request>(205);
		base.Protocol.SetProtocol<copy_scene_result>(552);
		base.Protocol.SetRequest<SprotoType.copy_scene_result.request>(552);
		base.Protocol.SetProtocol<copy_swipe_out>(232);
		base.Protocol.SetRequest<SprotoType.copy_swipe_out.request>(232);
		base.Protocol.SetProtocol<count_down>(553);
		base.Protocol.SetRequest<SprotoType.count_down.request>(553);
		base.Protocol.SetProtocol<del_friend>(125);
		base.Protocol.SetRequest<SprotoType.del_friend.request>(125);
		base.Protocol.SetProtocol<download_finish>(270);
		base.Protocol.SetRequest<SprotoType.download_finish.request>(270);
		base.Protocol.SetProtocol<drop_item_info>(527);
		base.Protocol.SetRequest<SprotoType.drop_item_info.request>(527);
		base.Protocol.SetProtocol<enter_bar_fight>(207);
		base.Protocol.SetRequest<SprotoType.enter_bar_fight.request>(207);
		base.Protocol.SetProtocol<enter_copy_scene>(107);
		base.Protocol.SetRequest<SprotoType.enter_copy_scene.request>(107);
		base.Protocol.SetProtocol<enter_domin_pk_scene>(311);
		base.Protocol.SetRequest<SprotoType.enter_domin_pk_scene.request>(311);
		base.Protocol.SetProtocol<enter_empty_scene>(451);
		base.Protocol.SetRequest<SprotoType.enter_empty_scene.request>(451);
		base.Protocol.SetProtocol<enter_guild_battle>(286);
		base.Protocol.SetRequest<SprotoType.enter_guild_battle.request>(286);
		base.Protocol.SetProtocol<enter_guild_boss_scene>(194);
		base.Protocol.SetRequest<SprotoType.enter_guild_boss_scene.request>(194);
		base.Protocol.SetProtocol<enter_guild_city_scene>(322);
		base.Protocol.SetRequest<SprotoType.enter_guild_city_scene.request>(322);
		base.Protocol.SetProtocol<enter_map>(503);
		base.Protocol.SetRequest<SprotoType.enter_map.request>(503);
		base.Protocol.SetProtocol<enter_multi_copy_scene_confirm>(215);
		base.Protocol.SetRequest<SprotoType.enter_multi_copy_scene_confirm.request>(215);
		base.Protocol.SetProtocol<enter_new_map>(106);
		base.Protocol.SetRequest<SprotoType.enter_new_map.request>(106);
		base.Protocol.SetProtocol<enter_scuffle_batttle>(273);
		base.Protocol.SetRequest<SprotoType.enter_scuffle_batttle.request>(273);
		base.Protocol.SetProtocol<enter_single_exp_scene>(276);
		base.Protocol.SetRequest<SprotoType.enter_single_exp_scene.request>(276);
		base.Protocol.SetProtocol<enter_survive_batttle>(246);
		base.Protocol.SetRequest<SprotoType.enter_survive_batttle.request>(246);
		base.Protocol.SetProtocol<enter_teleport_point>(251);
		base.Protocol.SetRequest<SprotoType.enter_teleport_point.request>(251);
		base.Protocol.SetProtocol<enter_tower_copy_info>(204);
		base.Protocol.SetRequest<SprotoType.enter_tower_copy_info.request>(204);
		base.Protocol.SetProtocol<enter_wild_boss>(201);
		base.Protocol.SetRequest<SprotoType.enter_wild_boss.request>(201);
		base.Protocol.SetProtocol<equip_appraise>(303);
		base.Protocol.SetRequest<SprotoType.equip_appraise.request>(303);
		base.Protocol.SetProtocol<equip_badge>(197);
		base.Protocol.SetRequest<SprotoType.equip_badge.request>(197);
		base.Protocol.SetProtocol<equip_enhance>(167);
		base.Protocol.SetRequest<SprotoType.equip_enhance.request>(167);
		base.Protocol.SetProtocol<equip_fashion_item>(221);
		base.Protocol.SetRequest<SprotoType.equip_fashion_item.request>(221);
		base.Protocol.SetProtocol<equip_inhert>(184);
		base.Protocol.SetRequest<SprotoType.equip_inhert.request>(184);
		base.Protocol.SetResponse<SprotoType.equip_inhert.response>(184);
		base.Protocol.SetProtocol<equip_inlay>(304);
		base.Protocol.SetRequest<SprotoType.equip_inlay.request>(304);
		base.Protocol.SetProtocol<equip_item>(116);
		base.Protocol.SetRequest<SprotoType.equip_item.request>(116);
		base.Protocol.SetProtocol<equip_refine>(183);
		base.Protocol.SetRequest<SprotoType.equip_refine.request>(183);
		base.Protocol.SetResponse<SprotoType.equip_refine.response>(183);
		base.Protocol.SetProtocol<facebook_link>(5);
		base.Protocol.SetRequest<SprotoType.facebook_link.request>(5);
		base.Protocol.SetResponse<SprotoType.facebook_link.response>(5);
		base.Protocol.SetProtocol<facebook_unlink>(6);
		base.Protocol.SetRequest<SprotoType.facebook_unlink.request>(6);
		base.Protocol.SetResponse<SprotoType.facebook_unlink.response>(6);
		base.Protocol.SetProtocol<game_check>(308);
		base.Protocol.SetRequest<SprotoType.game_check.request>(308);
		base.Protocol.SetProtocol<gather_other_player>(321);
		base.Protocol.SetRequest<SprotoType.gather_other_player.request>(321);
		base.Protocol.SetProtocol<gather_team>(177);
		base.Protocol.SetRequest<SprotoType.gather_team.request>(177);
		base.Protocol.SetProtocol<get_level_reward>(675);
		base.Protocol.SetRequest<SprotoType.get_level_reward.request>(675);
		base.Protocol.SetProtocol<get_team_list>(165);
		base.Protocol.SetRequest<SprotoType.get_team_list.request>(165);
		base.Protocol.SetProtocol<grant_activity_reward>(620);
		base.Protocol.SetRequest<SprotoType.grant_activity_reward.request>(620);
		base.Protocol.SetProtocol<grant_daily_mission_reward>(612);
		base.Protocol.SetRequest<SprotoType.grant_daily_mission_reward.request>(612);
		base.Protocol.SetProtocol<grant_tower_reward>(203);
		base.Protocol.SetRequest<SprotoType.grant_tower_reward.request>(203);
		base.Protocol.SetProtocol<guild_approve_resverve>(154);
		base.Protocol.SetRequest<SprotoType.guild_approve_resverve.request>(154);
		base.Protocol.SetProtocol<guild_battle_finish_info>(668);
		base.Protocol.SetRequest<SprotoType.guild_battle_finish_info.request>(668);
		base.Protocol.SetProtocol<guild_battle_guess>(290);
		base.Protocol.SetRequest<SprotoType.guild_battle_guess.request>(290);
		base.Protocol.SetProtocol<guild_battle_start>(670);
		base.Protocol.SetRequest<SprotoType.guild_battle_start.request>(670);
		base.Protocol.SetProtocol<guild_create>(146);
		base.Protocol.SetRequest<SprotoType.guild_create.request>(146);
		base.Protocol.SetProtocol<guild_donate>(175);
		base.Protocol.SetRequest<SprotoType.guild_donate.request>(175);
		base.Protocol.SetProtocol<guild_invite>(247);
		base.Protocol.SetRequest<SprotoType.guild_invite.request>(247);
		base.Protocol.SetProtocol<guild_invite_accept>(639);
		base.Protocol.SetRequest<SprotoType.guild_invite_accept.request>(639);
		base.Protocol.SetProtocol<guild_job_change>(150);
		base.Protocol.SetRequest<SprotoType.guild_job_change.request>(150);
		base.Protocol.SetProtocol<guild_join>(147);
		base.Protocol.SetRequest<SprotoType.guild_join.request>(147);
		base.Protocol.SetProtocol<guild_kick>(149);
		base.Protocol.SetRequest<SprotoType.guild_kick.request>(149);
		base.Protocol.SetProtocol<guild_leave>(148);
		base.Protocol.SetRequest<SprotoType.guild_leave.request>(148);
		base.Protocol.SetProtocol<guild_log>(174);
		base.Protocol.SetRequest<SprotoType.guild_log.request>(174);
		base.Protocol.SetProtocol<guild_req_info>(153);
		base.Protocol.SetRequest<SprotoType.guild_req_info.request>(153);
		base.Protocol.SetProtocol<guild_req_list>(152);
		base.Protocol.SetRequest<SprotoType.guild_req_list.request>(152);
		base.Protocol.SetProtocol<guild_skill_level>(151);
		base.Protocol.SetRequest<SprotoType.guild_skill_level.request>(151);
		base.Protocol.SetProtocol<heart_beat>(218);
		base.Protocol.SetRequest<SprotoType.heart_beat.request>(218);
		base.Protocol.SetResponse<SprotoType.heart_beat.response>(218);
		base.Protocol.SetProtocol<hit_action>(514);
		base.Protocol.SetRequest<SprotoType.hit_action.request>(514);
		base.Protocol.SetProtocol<impact_npc>(298);
		base.Protocol.SetRequest<SprotoType.impact_npc.request>(298);
		base.Protocol.SetProtocol<invite_join_team>(516);
		base.Protocol.SetRequest<SprotoType.invite_join_team.request>(516);
		base.Protocol.SetProtocol<leave_copy_scene>(108);
		base.Protocol.SetRequest<SprotoType.leave_copy_scene.request>(108);
		base.Protocol.SetProtocol<leave_game>(234);
		base.Protocol.SetRequest<SprotoType.leave_game.request>(234);
		base.Protocol.SetProtocol<leave_team>(166);
		base.Protocol.SetRequest<SprotoType.leave_team.request>(166);
		base.Protocol.SetProtocol<local_character_attack>(128);
		base.Protocol.SetRequest<SprotoType.local_character_attack.request>(128);
		base.Protocol.SetProtocol<local_npc_die>(307);
		base.Protocol.SetRequest<SprotoType.local_npc_die.request>(307);
		base.Protocol.SetProtocol<login>(4);
		base.Protocol.SetRequest<SprotoType.login.request>(4);
		base.Protocol.SetResponse<SprotoType.login.response>(4);
		base.Protocol.SetProtocol<login_max_count>(578);
		base.Protocol.SetRequest<SprotoType.login_max_count.request>(578);
		base.Protocol.SetProtocol<mail_delete>(532);
		base.Protocol.SetRequest<SprotoType.mail_delete.request>(532);
		base.Protocol.SetProtocol<mail_operation>(123);
		base.Protocol.SetRequest<SprotoType.mail_operation.request>(123);
		base.Protocol.SetProtocol<mail_update>(531);
		base.Protocol.SetRequest<SprotoType.mail_update.request>(531);
		base.Protocol.SetProtocol<main_player_create>(504);
		base.Protocol.SetRequest<SprotoType.main_player_create.request>(504);
		base.Protocol.SetProtocol<map_ready>(100);
		base.Protocol.SetProtocol<mount_equip>(236);
		base.Protocol.SetRequest<SprotoType.mount_equip.request>(236);
		base.Protocol.SetProtocol<mount_unequip>(237);
		base.Protocol.SetRequest<SprotoType.mount_unequip.request>(237);
		base.Protocol.SetProtocol<mount_use_color>(241);
		base.Protocol.SetRequest<SprotoType.mount_use_color.request>(241);
		base.Protocol.SetProtocol<move>(101);
		base.Protocol.SetRequest<SprotoType.move.request>(101);
		base.Protocol.SetResponse<SprotoType.move.response>(101);
		base.Protocol.SetProtocol<next_wave>(515);
		base.Protocol.SetRequest<SprotoType.next_wave.request>(515);
		base.Protocol.SetProtocol<notice>(529);
		base.Protocol.SetRequest<SprotoType.notice.request>(529);
		base.Protocol.SetProtocol<notice_add_friend>(536);
		base.Protocol.SetRequest<SprotoType.notice_add_friend.request>(536);
		base.Protocol.SetProtocol<notice_copy_scene_info>(683);
		base.Protocol.SetRequest<SprotoType.notice_copy_scene_info.request>(683);
		base.Protocol.SetProtocol<notice_guild_battle_rank>(676);
		base.Protocol.SetRequest<SprotoType.notice_guild_battle_rank.request>(676);
		base.Protocol.SetProtocol<notice_money_copy_reward>(615);
		base.Protocol.SetRequest<SprotoType.notice_money_copy_reward.request>(615);
		base.Protocol.SetProtocol<notice_relife_player>(618);
		base.Protocol.SetRequest<SprotoType.notice_relife_player.request>(618);
		base.Protocol.SetProtocol<notice_urge_team_leader>(682);
		base.Protocol.SetRequest<SprotoType.notice_urge_team_leader.request>(682);
		base.Protocol.SetProtocol<notify_confirm_state>(603);
		base.Protocol.SetRequest<SprotoType.notify_confirm_state.request>(603);
		base.Protocol.SetProtocol<notify_copy_start_info>(629);
		base.Protocol.SetRequest<SprotoType.notify_copy_start_info.request>(629);
		base.Protocol.SetProtocol<npc_create>(509);
		base.Protocol.SetRequest<SprotoType.npc_create.request>(509);
		base.Protocol.SetProtocol<open_guild_boss>(233);
		base.Protocol.SetRequest<SprotoType.open_guild_boss.request>(233);
		base.Protocol.SetProtocol<open_item_package>(224);
		base.Protocol.SetRequest<SprotoType.open_item_package.request>(224);
		base.Protocol.SetProtocol<open_multi_tower_reward>(283);
		base.Protocol.SetRequest<SprotoType.open_multi_tower_reward.request>(283);
		base.Protocol.SetProtocol<pause_participate_dance>(228);
		base.Protocol.SetRequest<SprotoType.pause_participate_dance.request>(228);
		base.Protocol.SetProtocol<play_social_dance>(275);
		base.Protocol.SetRequest<SprotoType.play_social_dance.request>(275);
		base.Protocol.SetProtocol<put_item_storagepack>(140);
		base.Protocol.SetRequest<SprotoType.put_item_storagepack.request>(140);
		base.Protocol.SetProtocol<random_select_ok>(610);
		base.Protocol.SetRequest<SprotoType.random_select_ok.request>(610);
		base.Protocol.SetProtocol<random_select_team>(214);
		base.Protocol.SetRequest<SprotoType.random_select_team.request>(214);
		base.Protocol.SetProtocol<rank_pvp_create_zombie_user>(544);
		base.Protocol.SetRequest<SprotoType.rank_pvp_create_zombie_user.request>(544);
		base.Protocol.SetProtocol<rank_pvp_history>(546);
		base.Protocol.SetRequest<SprotoType.rank_pvp_history.request>(546);
		base.Protocol.SetProtocol<rank_pvp_other_player_die>(137);
		base.Protocol.SetRequest<SprotoType.rank_pvp_other_player_die.request>(137);
		base.Protocol.SetProtocol<rank_pvp_player_attack>(136);
		base.Protocol.SetRequest<SprotoType.rank_pvp_player_attack.request>(136);
		base.Protocol.SetProtocol<rank_pvp_reward>(545);
		base.Protocol.SetRequest<SprotoType.rank_pvp_reward.request>(545);
		base.Protocol.SetProtocol<rank_pvp_start>(547);
		base.Protocol.SetRequest<SprotoType.rank_pvp_start.request>(547);
		base.Protocol.SetProtocol<re_name>(301);
		base.Protocol.SetRequest<SprotoType.re_name.request>(301);
		base.Protocol.SetProtocol<real_pvp_register>(138);
		base.Protocol.SetRequest<SprotoType.real_pvp_register.request>(138);
		base.Protocol.SetProtocol<real_pvp_start>(549);
		base.Protocol.SetRequest<SprotoType.real_pvp_start.request>(549);
		base.Protocol.SetProtocol<real_pvp_state>(548);
		base.Protocol.SetRequest<SprotoType.real_pvp_state.request>(548);
		base.Protocol.SetProtocol<receive_level_reward>(297);
		base.Protocol.SetRequest<SprotoType.receive_level_reward.request>(297);
		base.Protocol.SetProtocol<refresh_online_misison>(309);
		base.Protocol.SetRequest<SprotoType.refresh_online_misison.request>(309);
		base.Protocol.SetProtocol<refresh_online_state>(281);
		base.Protocol.SetRequest<SprotoType.refresh_online_state.request>(281);
		base.Protocol.SetProtocol<relife_player>(132);
		base.Protocol.SetRequest<SprotoType.relife_player.request>(132);
		base.Protocol.SetProtocol<req_buy_guild_goods>(172);
		base.Protocol.SetRequest<SprotoType.req_buy_guild_goods.request>(172);
		base.Protocol.SetProtocol<req_change_team_goal>(213);
		base.Protocol.SetRequest<SprotoType.req_change_team_goal.request>(213);
		base.Protocol.SetProtocol<req_guild_battle_guess>(292);
		base.Protocol.SetRequest<SprotoType.req_guild_battle_guess.request>(292);
		base.Protocol.SetProtocol<req_guild_battle_info>(285);
		base.Protocol.SetRequest<SprotoType.req_guild_battle_info.request>(285);
		base.Protocol.SetProtocol<req_guild_battle_member>(288);
		base.Protocol.SetRequest<SprotoType.req_guild_battle_member.request>(288);
		base.Protocol.SetProtocol<req_guild_battle_rank>(287);
		base.Protocol.SetRequest<SprotoType.req_guild_battle_rank.request>(287);
		base.Protocol.SetProtocol<req_guild_battle_state>(295);
		base.Protocol.SetRequest<SprotoType.req_guild_battle_state.request>(295);
		base.Protocol.SetProtocol<req_guild_member_info>(170);
		base.Protocol.SetRequest<SprotoType.req_guild_member_info.request>(170);
		base.Protocol.SetProtocol<req_guild_notice>(169);
		base.Protocol.SetRequest<SprotoType.req_guild_notice.request>(169);
		base.Protocol.SetProtocol<req_guild_score_info>(291);
		base.Protocol.SetRequest<SprotoType.req_guild_score_info.request>(291);
		base.Protocol.SetProtocol<req_guild_skill>(212);
		base.Protocol.SetRequest<SprotoType.req_guild_skill.request>(212);
		base.Protocol.SetProtocol<req_guild_star>(293);
		base.Protocol.SetProtocol<req_invite_team>(109);
		base.Protocol.SetRequest<SprotoType.req_invite_team.request>(109);
		base.Protocol.SetProtocol<req_invite_team_result>(517);
		base.Protocol.SetRequest<SprotoType.req_invite_team_result.request>(517);
		base.Protocol.SetProtocol<req_join_team>(161);
		base.Protocol.SetRequest<SprotoType.req_join_team.request>(161);
		base.Protocol.SetProtocol<req_level_reward>(296);
		base.Protocol.SetRequest<SprotoType.req_level_reward.request>(296);
		base.Protocol.SetProtocol<req_offline_chat>(168);
		base.Protocol.SetRequest<SprotoType.req_offline_chat.request>(168);
		base.Protocol.SetProtocol<req_open_guild_shop>(171);
		base.Protocol.SetRequest<SprotoType.req_open_guild_shop.request>(171);
		base.Protocol.SetProtocol<req_other_team>(248);
		base.Protocol.SetRequest<SprotoType.req_other_team.request>(248);
		base.Protocol.SetProtocol<req_random_online_character_list>(159);
		base.Protocol.SetRequest<SprotoType.req_random_online_character_list.request>(159);
		base.Protocol.SetProtocol<req_seting_guild_appro>(173);
		base.Protocol.SetRequest<SprotoType.req_seting_guild_appro.request>(173);
		base.Protocol.SetProtocol<request_activity_info>(225);
		base.Protocol.SetRequest<SprotoType.request_activity_info.request>(225);
		base.Protocol.SetProtocol<request_bar_fight>(206);
		base.Protocol.SetRequest<SprotoType.request_bar_fight.request>(206);
		base.Protocol.SetProtocol<request_battle_info>(231);
		base.Protocol.SetRequest<SprotoType.request_battle_info.request>(231);
		base.Protocol.SetProtocol<request_big_pack>(260);
		base.Protocol.SetRequest<SprotoType.request_big_pack.request>(260);
		base.Protocol.SetProtocol<request_change_pk_mode>(226);
		base.Protocol.SetRequest<SprotoType.request_change_pk_mode.request>(226);
		base.Protocol.SetProtocol<request_daily_active>(261);
		base.Protocol.SetRequest<SprotoType.request_daily_active.request>(261);
		base.Protocol.SetProtocol<request_daily_buy>(258);
		base.Protocol.SetRequest<SprotoType.request_daily_buy.request>(258);
		base.Protocol.SetProtocol<request_daily_mission>(121);
		base.Protocol.SetRequest<SprotoType.request_daily_mission.request>(121);
		base.Protocol.SetProtocol<request_dance_info>(227);
		base.Protocol.SetRequest<SprotoType.request_dance_info.request>(227);
		base.Protocol.SetProtocol<request_dance_state_info>(313);
		base.Protocol.SetRequest<SprotoType.request_dance_state_info.request>(313);
		base.Protocol.SetProtocol<request_domin_info>(310);
		base.Protocol.SetRequest<SprotoType.request_domin_info.request>(310);
		base.Protocol.SetProtocol<request_first_buy>(259);
		base.Protocol.SetRequest<SprotoType.request_first_buy.request>(259);
		base.Protocol.SetProtocol<request_guild_boss>(195);
		base.Protocol.SetRequest<SprotoType.request_guild_boss.request>(195);
		base.Protocol.SetProtocol<request_guild_map_domine_top>(318);
		base.Protocol.SetRequest<SprotoType.request_guild_map_domine_top.request>(318);
		base.Protocol.SetProtocol<request_guild_map_info>(319);
		base.Protocol.SetRequest<SprotoType.request_guild_map_info.request>(319);
		base.Protocol.SetProtocol<request_guild_map_reward>(320);
		base.Protocol.SetRequest<SprotoType.request_guild_map_reward.request>(320);
		base.Protocol.SetProtocol<request_guild_reward>(196);
		base.Protocol.SetRequest<SprotoType.request_guild_reward.request>(196);
		base.Protocol.SetProtocol<request_invest_pack>(257);
		base.Protocol.SetRequest<SprotoType.request_invest_pack.request>(257);
		base.Protocol.SetProtocol<request_level_pack>(256);
		base.Protocol.SetRequest<SprotoType.request_level_pack.request>(256);
		base.Protocol.SetProtocol<request_line_state>(219);
		base.Protocol.SetRequest<SprotoType.request_line_state.request>(219);
		base.Protocol.SetProtocol<request_mount_info>(235);
		base.Protocol.SetRequest<SprotoType.request_mount_info.request>(235);
		base.Protocol.SetProtocol<request_random_name>(118);
		base.Protocol.SetRequest<SprotoType.request_random_name.request>(118);
		base.Protocol.SetResponse<SprotoType.request_random_name.response>(118);
		base.Protocol.SetProtocol<request_random_rank_pvp_opponent>(133);
		base.Protocol.SetRequest<SprotoType.request_random_rank_pvp_opponent.request>(133);
		base.Protocol.SetProtocol<request_rank_pvp_data>(210);
		base.Protocol.SetRequest<SprotoType.request_rank_pvp_data.request>(210);
		base.Protocol.SetProtocol<request_rank_pvp_history>(211);
		base.Protocol.SetRequest<SprotoType.request_rank_pvp_history.request>(211);
		base.Protocol.SetProtocol<request_retrieve>(279);
		base.Protocol.SetRequest<SprotoType.request_retrieve.request>(279);
		base.Protocol.SetProtocol<request_retrieve_info>(278);
		base.Protocol.SetRequest<SprotoType.request_retrieve_info.request>(278);
		base.Protocol.SetProtocol<request_sign_30_day_info>(252);
		base.Protocol.SetRequest<SprotoType.request_sign_30_day_info.request>(252);
		base.Protocol.SetProtocol<request_sign_week_info>(253);
		base.Protocol.SetRequest<SprotoType.request_sign_week_info.request>(253);
		base.Protocol.SetProtocol<request_slot_info>(242);
		base.Protocol.SetRequest<SprotoType.request_slot_info.request>(242);
		base.Protocol.SetProtocol<request_slot_reward>(249);
		base.Protocol.SetRequest<SprotoType.request_slot_reward.request>(249);
		base.Protocol.SetProtocol<request_slot_sum_reward>(244);
		base.Protocol.SetRequest<SprotoType.request_slot_sum_reward.request>(244);
		base.Protocol.SetProtocol<request_special_big_pack>(274);
		base.Protocol.SetRequest<SprotoType.request_special_big_pack.request>(274);
		base.Protocol.SetProtocol<request_survive_top>(245);
		base.Protocol.SetRequest<SprotoType.request_survive_top.request>(245);
		base.Protocol.SetProtocol<request_top_rank_list>(191);
		base.Protocol.SetRequest<SprotoType.request_top_rank_list.request>(191);
		base.Protocol.SetProtocol<request_top_rank_pvp_list>(134);
		base.Protocol.SetRequest<SprotoType.request_top_rank_pvp_list.request>(134);
		base.Protocol.SetProtocol<request_tower_copy_info>(202);
		base.Protocol.SetRequest<SprotoType.request_tower_copy_info.request>(202);
		base.Protocol.SetProtocol<request_update_friend_useinfo>(126);
		base.Protocol.SetRequest<SprotoType.request_update_friend_useinfo.request>(126);
		base.Protocol.SetProtocol<request_update_storagepack>(139);
		base.Protocol.SetRequest<SprotoType.request_update_storagepack.request>(139);
		base.Protocol.SetProtocol<request_wild_boss_info>(200);
		base.Protocol.SetRequest<SprotoType.request_wild_boss_info.request>(200);
		base.Protocol.SetProtocol<require_daily_active_reward>(265);
		base.Protocol.SetRequest<SprotoType.require_daily_active_reward.request>(265);
		base.Protocol.SetProtocol<require_domin_rewards>(312);
		base.Protocol.SetRequest<SprotoType.require_domin_rewards.request>(312);
		base.Protocol.SetProtocol<require_first_buy_reward>(264);
		base.Protocol.SetRequest<SprotoType.require_first_buy_reward.request>(264);
		base.Protocol.SetProtocol<require_invest_reward>(263);
		base.Protocol.SetRequest<SprotoType.require_invest_reward.request>(263);
		base.Protocol.SetProtocol<require_level_reward>(262);
		base.Protocol.SetRequest<SprotoType.require_level_reward.request>(262);
		base.Protocol.SetProtocol<require_vip_info>(299);
		base.Protocol.SetRequest<SprotoType.require_vip_info.request>(299);
		base.Protocol.SetProtocol<require_vip_reward>(300);
		base.Protocol.SetRequest<SprotoType.require_vip_reward.request>(300);
		base.Protocol.SetProtocol<ret_abandon_mission>(522);
		base.Protocol.SetRequest<SprotoType.ret_abandon_mission.request>(522);
		base.Protocol.SetProtocol<ret_accept_mission>(520);
		base.Protocol.SetRequest<SprotoType.ret_accept_mission.request>(520);
		base.Protocol.SetProtocol<ret_add_friend>(533);
		base.Protocol.SetRequest<SprotoType.ret_add_friend.request>(533);
		base.Protocol.SetProtocol<ret_ask_confirm_multi_copy_scene>(217);
		base.Protocol.SetRequest<SprotoType.ret_ask_confirm_multi_copy_scene.request>(217);
		base.Protocol.SetProtocol<ret_ask_shop_list>(554);
		base.Protocol.SetRequest<SprotoType.ret_ask_shop_list.request>(554);
		base.Protocol.SetProtocol<ret_battle_info>(627);
		base.Protocol.SetRequest<SprotoType.ret_battle_info.request>(627);
		base.Protocol.SetProtocol<ret_buy_car_shop>(691);
		base.Protocol.SetRequest<SprotoType.ret_buy_car_shop.request>(691);
		base.Protocol.SetProtocol<ret_buy_guild_goods>(583);
		base.Protocol.SetRequest<SprotoType.ret_buy_guild_goods.request>(583);
		base.Protocol.SetProtocol<ret_buy_invest_pack>(655);
		base.Protocol.SetRequest<SprotoType.ret_buy_invest_pack.request>(655);
		base.Protocol.SetProtocol<ret_buy_shop_item>(652);
		base.Protocol.SetRequest<SprotoType.ret_buy_shop_item.request>(652);
		base.Protocol.SetProtocol<ret_chat>(528);
		base.Protocol.SetRequest<SprotoType.ret_chat.request>(528);
		base.Protocol.SetProtocol<ret_commercail_reward>(650);
		base.Protocol.SetRequest<SprotoType.ret_commercail_reward.request>(650);
		base.Protocol.SetProtocol<ret_complete_mission>(521);
		base.Protocol.SetRequest<SprotoType.ret_complete_mission.request>(521);
		base.Protocol.SetProtocol<ret_consign_ask_items_info>(596);
		base.Protocol.SetRequest<SprotoType.ret_consign_ask_items_info.request>(596);
		base.Protocol.SetProtocol<ret_consign_ask_my_items>(595);
		base.Protocol.SetRequest<SprotoType.ret_consign_ask_my_items.request>(595);
		base.Protocol.SetProtocol<ret_consign_buy_item>(597);
		base.Protocol.SetRequest<SprotoType.ret_consign_buy_item.request>(597);
		base.Protocol.SetProtocol<ret_consign_cancel_sale>(594);
		base.Protocol.SetRequest<SprotoType.ret_consign_cancel_sale.request>(594);
		base.Protocol.SetProtocol<ret_consign_sale_item>(593);
		base.Protocol.SetRequest<SprotoType.ret_consign_sale_item.request>(593);
		base.Protocol.SetProtocol<ret_del_friend>(535);
		base.Protocol.SetRequest<SprotoType.ret_del_friend.request>(535);
		base.Protocol.SetProtocol<ret_domin_info>(684);
		base.Protocol.SetRequest<SprotoType.ret_domin_info.request>(684);
		base.Protocol.SetProtocol<ret_enter_guild_battle>(677);
		base.Protocol.SetRequest<SprotoType.ret_enter_guild_battle.request>(677);
		base.Protocol.SetProtocol<ret_get_team_list>(574);
		base.Protocol.SetRequest<SprotoType.ret_get_team_list.request>(574);
		base.Protocol.SetProtocol<ret_grant_tower_reward>(625);
		base.Protocol.SetRequest<SprotoType.ret_grant_tower_reward.request>(625);
		base.Protocol.SetProtocol<ret_guild_approve_resverve>(591);
		base.Protocol.SetRequest<SprotoType.ret_guild_approve_resverve.request>(591);
		base.Protocol.SetProtocol<ret_guild_battle_guess>(669);
		base.Protocol.SetRequest<SprotoType.ret_guild_battle_guess.request>(669);
		base.Protocol.SetProtocol<ret_guild_battle_info>(662);
		base.Protocol.SetRequest<SprotoType.ret_guild_battle_info.request>(662);
		base.Protocol.SetProtocol<ret_guild_battle_member>(665);
		base.Protocol.SetRequest<SprotoType.ret_guild_battle_member.request>(665);
		base.Protocol.SetProtocol<ret_guild_battle_rank>(663);
		base.Protocol.SetRequest<SprotoType.ret_guild_battle_rank.request>(663);
		base.Protocol.SetProtocol<ret_guild_battle_state>(673);
		base.Protocol.SetRequest<SprotoType.ret_guild_battle_state.request>(673);
		base.Protocol.SetProtocol<ret_guild_create>(567);
		base.Protocol.SetRequest<SprotoType.ret_guild_create.request>(567);
		base.Protocol.SetProtocol<ret_guild_donate>(585);
		base.Protocol.SetRequest<SprotoType.ret_guild_donate.request>(585);
		base.Protocol.SetProtocol<ret_guild_job_change>(589);
		base.Protocol.SetRequest<SprotoType.ret_guild_job_change.request>(589);
		base.Protocol.SetProtocol<ret_guild_join>(566);
		base.Protocol.SetRequest<SprotoType.ret_guild_join.request>(566);
		base.Protocol.SetProtocol<ret_guild_kick>(590);
		base.Protocol.SetRequest<SprotoType.ret_guild_kick.request>(590);
		base.Protocol.SetProtocol<ret_guild_leave>(565);
		base.Protocol.SetRequest<SprotoType.ret_guild_leave.request>(565);
		base.Protocol.SetProtocol<ret_guild_log>(584);
		base.Protocol.SetRequest<SprotoType.ret_guild_log.request>(584);
		base.Protocol.SetProtocol<ret_guild_map_domine_top>(688);
		base.Protocol.SetRequest<SprotoType.ret_guild_map_domine_top.request>(688);
		base.Protocol.SetProtocol<ret_guild_map_reward>(690);
		base.Protocol.SetRequest<SprotoType.ret_guild_map_reward.request>(690);
		base.Protocol.SetProtocol<ret_guild_member_info>(581);
		base.Protocol.SetRequest<SprotoType.ret_guild_member_info.request>(581);
		base.Protocol.SetProtocol<ret_guild_req_info>(563);
		base.Protocol.SetRequest<SprotoType.ret_guild_req_info.request>(563);
		base.Protocol.SetProtocol<ret_guild_req_list>(562);
		base.Protocol.SetRequest<SprotoType.ret_guild_req_list.request>(562);
		base.Protocol.SetProtocol<ret_guild_score_info>(667);
		base.Protocol.SetRequest<SprotoType.ret_guild_score_info.request>(667);
		base.Protocol.SetProtocol<ret_guild_skill_level>(564);
		base.Protocol.SetRequest<SprotoType.ret_guild_skill_level.request>(564);
		base.Protocol.SetProtocol<ret_guild_star>(671);
		base.Protocol.SetRequest<SprotoType.ret_guild_star.request>(671);
		base.Protocol.SetProtocol<ret_invite_join_team>(110);
		base.Protocol.SetRequest<SprotoType.ret_invite_join_team.request>(110);
		base.Protocol.SetProtocol<ret_level_reward>(674);
		base.Protocol.SetRequest<SprotoType.ret_level_reward.request>(674);
		base.Protocol.SetProtocol<ret_mount_equip>(631);
		base.Protocol.SetRequest<SprotoType.ret_mount_equip.request>(631);
		base.Protocol.SetProtocol<ret_mount_info>(630);
		base.Protocol.SetRequest<SprotoType.ret_mount_info.request>(630);
		base.Protocol.SetProtocol<ret_mount_use_color>(632);
		base.Protocol.SetRequest<SprotoType.ret_mount_use_color.request>(632);
		base.Protocol.SetProtocol<ret_offline_chat>(579);
		base.Protocol.SetRequest<SprotoType.ret_offline_chat.request>(579);
		base.Protocol.SetProtocol<ret_open_guild_boss>(628);
		base.Protocol.SetRequest<SprotoType.ret_open_guild_boss.request>(628);
		base.Protocol.SetProtocol<ret_open_guild_shop>(582);
		base.Protocol.SetRequest<SprotoType.ret_open_guild_shop.request>(582);
		base.Protocol.SetProtocol<ret_open_item_package>(617);
		base.Protocol.SetRequest<SprotoType.ret_open_item_package.request>(617);
		base.Protocol.SetProtocol<ret_random_online_character_list>(570);
		base.Protocol.SetRequest<SprotoType.ret_random_online_character_list.request>(570);
		base.Protocol.SetProtocol<ret_re_name>(680);
		base.Protocol.SetRequest<SprotoType.ret_re_name.request>(680);
		base.Protocol.SetProtocol<ret_req_guild_skill>(609);
		base.Protocol.SetRequest<SprotoType.ret_req_guild_skill.request>(609);
		base.Protocol.SetProtocol<ret_request_30_day_info>(640);
		base.Protocol.SetRequest<SprotoType.ret_request_30_day_info.request>(640);
		base.Protocol.SetProtocol<ret_request_activity_info>(619);
		base.Protocol.SetRequest<SprotoType.ret_request_activity_info.request>(619);
		base.Protocol.SetProtocol<ret_request_big_pack>(648);
		base.Protocol.SetRequest<SprotoType.ret_request_big_pack.request>(648);
		base.Protocol.SetProtocol<ret_request_daily_active>(649);
		base.Protocol.SetRequest<SprotoType.ret_request_daily_active.request>(649);
		base.Protocol.SetProtocol<ret_request_daily_buy>(646);
		base.Protocol.SetRequest<SprotoType.ret_request_daily_buy.request>(646);
		base.Protocol.SetProtocol<ret_request_dance_info>(623);
		base.Protocol.SetRequest<SprotoType.ret_request_dance_info.request>(623);
		base.Protocol.SetProtocol<ret_request_first_buy>(647);
		base.Protocol.SetRequest<SprotoType.ret_request_first_buy.request>(647);
		base.Protocol.SetProtocol<ret_request_guild_boss>(599);
		base.Protocol.SetRequest<SprotoType.ret_request_guild_boss.request>(599);
		base.Protocol.SetProtocol<ret_request_guild_map_info>(689);
		base.Protocol.SetRequest<SprotoType.ret_request_guild_map_info.request>(689);
		base.Protocol.SetProtocol<ret_request_invest_pack>(645);
		base.Protocol.SetRequest<SprotoType.ret_request_invest_pack.request>(645);
		base.Protocol.SetProtocol<ret_request_level_pack>(644);
		base.Protocol.SetRequest<SprotoType.ret_request_level_pack.request>(644);
		base.Protocol.SetProtocol<ret_request_random_rank_pvp_opponent>(542);
		base.Protocol.SetRequest<SprotoType.ret_request_random_rank_pvp_opponent.request>(542);
		base.Protocol.SetProtocol<ret_request_retrieve_info>(658);
		base.Protocol.SetRequest<SprotoType.ret_request_retrieve_info.request>(658);
		base.Protocol.SetProtocol<ret_request_sign_week_info>(641);
		base.Protocol.SetRequest<SprotoType.ret_request_sign_week_info.request>(641);
		base.Protocol.SetProtocol<ret_request_survive_top>(636);
		base.Protocol.SetRequest<SprotoType.ret_request_survive_top.request>(636);
		base.Protocol.SetProtocol<ret_request_top_rank_pvp_list>(543);
		base.Protocol.SetRequest<SprotoType.ret_request_top_rank_pvp_list.request>(543);
		base.Protocol.SetProtocol<ret_request_tower_copy_info>(606);
		base.Protocol.SetRequest<SprotoType.ret_request_tower_copy_info.request>(606);
		base.Protocol.SetProtocol<ret_request_update_friend_useinfo>(534);
		base.Protocol.SetRequest<SprotoType.ret_request_update_friend_useinfo.request>(534);
		base.Protocol.SetProtocol<ret_request_update_storagepack>(550);
		base.Protocol.SetRequest<SprotoType.ret_request_update_storagepack.request>(550);
		base.Protocol.SetProtocol<ret_request_wild_boss_info>(605);
		base.Protocol.SetRequest<SprotoType.ret_request_wild_boss_info.request>(605);
		base.Protocol.SetProtocol<ret_require_vip_info>(678);
		base.Protocol.SetRequest<SprotoType.ret_require_vip_info.request>(678);
		base.Protocol.SetProtocol<ret_require_vip_reward>(679);
		base.Protocol.SetRequest<SprotoType.ret_require_vip_reward.request>(679);
		base.Protocol.SetProtocol<ret_search_guild>(586);
		base.Protocol.SetRequest<SprotoType.ret_search_guild.request>(586);
		base.Protocol.SetProtocol<ret_search_online_character_by_name>(571);
		base.Protocol.SetRequest<SprotoType.ret_search_online_character_by_name.request>(571);
		base.Protocol.SetProtocol<ret_set_guild_battle_member>(666);
		base.Protocol.SetRequest<SprotoType.ret_set_guild_battle_member.request>(666);
		base.Protocol.SetProtocol<ret_sign_30_day>(642);
		base.Protocol.SetRequest<SprotoType.ret_sign_30_day.request>(642);
		base.Protocol.SetProtocol<ret_sign_week>(643);
		base.Protocol.SetRequest<SprotoType.ret_sign_week.request>(643);
		base.Protocol.SetProtocol<ret_skill_use>(508);
		base.Protocol.SetRequest<SprotoType.ret_skill_use.request>(508);
		base.Protocol.SetProtocol<ret_slot_info>(633);
		base.Protocol.SetRequest<SprotoType.ret_slot_info.request>(633);
		base.Protocol.SetProtocol<ret_slot_sum_reward>(635);
		base.Protocol.SetRequest<SprotoType.ret_slot_sum_reward.request>(635);
		base.Protocol.SetProtocol<ret_special_big_pack>(656);
		base.Protocol.SetRequest<SprotoType.ret_special_big_pack.request>(656);
		base.Protocol.SetProtocol<ret_spin_slot>(634);
		base.Protocol.SetRequest<SprotoType.ret_spin_slot.request>(634);
		base.Protocol.SetProtocol<ret_title_req_level_up>(569);
		base.Protocol.SetRequest<SprotoType.ret_title_req_level_up.request>(569);
		base.Protocol.SetProtocol<ret_top_rank_list>(598);
		base.Protocol.SetRequest<SprotoType.ret_top_rank_list.request>(598);
		base.Protocol.SetProtocol<ret_tower_reset>(626);
		base.Protocol.SetRequest<SprotoType.ret_tower_reset.request>(626);
		base.Protocol.SetProtocol<ret_tower_wipe_out>(607);
		base.Protocol.SetRequest<SprotoType.ret_tower_wipe_out.request>(607);
		base.Protocol.SetProtocol<ret_update_guild_star>(672);
		base.Protocol.SetRequest<SprotoType.ret_update_guild_star.request>(672);
		base.Protocol.SetProtocol<ret_use_item>(526);
		base.Protocol.SetRequest<SprotoType.ret_use_item.request>(526);
		base.Protocol.SetProtocol<ret_watch_video_info>(693);
		base.Protocol.SetRequest<SprotoType.ret_watch_video_info.request>(693);
		base.Protocol.SetProtocol<retrieve_account>(660);
		base.Protocol.SetRequest<SprotoType.retrieve_account.request>(660);
		base.Protocol.SetProtocol<sample_activity_result>(685);
		base.Protocol.SetRequest<SprotoType.sample_activity_result.request>(685);
		base.Protocol.SetProtocol<sample_copy_result>(613);
		base.Protocol.SetRequest<SprotoType.sample_copy_result.request>(613);
		base.Protocol.SetProtocol<search_guild>(176);
		base.Protocol.SetRequest<SprotoType.search_guild.request>(176);
		base.Protocol.SetProtocol<search_online_character_by_name>(160);
		base.Protocol.SetRequest<SprotoType.search_online_character_by_name.request>(160);
		base.Protocol.SetProtocol<select_pk_character>(135);
		base.Protocol.SetRequest<SprotoType.select_pk_character.request>(135);
		base.Protocol.SetProtocol<sell_item>(129);
		base.Protocol.SetRequest<SprotoType.sell_item.request>(129);
		base.Protocol.SetProtocol<send_daily_mission>(530);
		base.Protocol.SetRequest<SprotoType.send_daily_mission.request>(530);
		base.Protocol.SetProtocol<send_dialog_notify>(653);
		base.Protocol.SetRequest<SprotoType.send_dialog_notify.request>(653);
		base.Protocol.SetProtocol<send_escort_info>(621);
		base.Protocol.SetRequest<SprotoType.send_escort_info.request>(621);
		base.Protocol.SetProtocol<send_mail>(122);
		base.Protocol.SetRequest<SprotoType.send_mail.request>(122);
		base.Protocol.SetProtocol<send_mail_box>(284);
		base.Protocol.SetRequest<SprotoType.send_mail_box.request>(284);
		base.Protocol.SetProtocol<set_guild_battle_member>(289);
		base.Protocol.SetRequest<SprotoType.set_guild_battle_member.request>(289);
		base.Protocol.SetProtocol<set_mission_param>(524);
		base.Protocol.SetRequest<SprotoType.set_mission_param.request>(524);
		base.Protocol.SetProtocol<set_mission_state>(523);
		base.Protocol.SetRequest<SprotoType.set_mission_state.request>(523);
		base.Protocol.SetProtocol<show_damage_board>(511);
		base.Protocol.SetRequest<SprotoType.show_damage_board.request>(511);
		base.Protocol.SetProtocol<show_player_damage_board>(687);
		base.Protocol.SetRequest<SprotoType.show_player_damage_board.request>(687);
		base.Protocol.SetProtocol<show_reward_items_tips>(638);
		base.Protocol.SetRequest<SprotoType.show_reward_items_tips.request>(638);
		base.Protocol.SetProtocol<sign_30_day>(254);
		base.Protocol.SetRequest<SprotoType.sign_30_day.request>(254);
		base.Protocol.SetProtocol<sign_bar_fight>(282);
		base.Protocol.SetRequest<SprotoType.sign_bar_fight.request>(282);
		base.Protocol.SetProtocol<sign_week>(255);
		base.Protocol.SetRequest<SprotoType.sign_week.request>(255);
		base.Protocol.SetProtocol<single_copy_scene_npc_die>(127);
		base.Protocol.SetRequest<SprotoType.single_copy_scene_npc_die.request>(127);
		base.Protocol.SetProtocol<skill_level_up>(130);
		base.Protocol.SetRequest<SprotoType.skill_level_up.request>(130);
		base.Protocol.SetProtocol<skill_use>(102);
		base.Protocol.SetRequest<SprotoType.skill_use.request>(102);
		base.Protocol.SetProtocol<spin_slot>(243);
		base.Protocol.SetRequest<SprotoType.spin_slot.request>(243);
		base.Protocol.SetProtocol<start_battle>(220);
		base.Protocol.SetRequest<SprotoType.start_battle.request>(220);
		base.Protocol.SetProtocol<start_download>(269);
		base.Protocol.SetRequest<SprotoType.start_download.request>(269);
		base.Protocol.SetProtocol<start_enter_game>(654);
		base.Protocol.SetRequest<SprotoType.start_enter_game.request>(654);
		base.Protocol.SetProtocol<start_participate_dance>(624);
		base.Protocol.SetRequest<SprotoType.start_participate_dance.request>(624);
		base.Protocol.SetProtocol<stop_leave_copy>(250);
		base.Protocol.SetRequest<SprotoType.stop_leave_copy.request>(250);
		base.Protocol.SetProtocol<stop_random_select_team>(216);
		base.Protocol.SetRequest<SprotoType.stop_random_select_team.request>(216);
		base.Protocol.SetProtocol<survive_battle_finish>(637);
		base.Protocol.SetRequest<SprotoType.survive_battle_finish.request>(637);
		base.Protocol.SetProtocol<syn_friend_info>(538);
		base.Protocol.SetRequest<SprotoType.syn_friend_info.request>(538);
		base.Protocol.SetProtocol<syn_rank_pvp_data>(541);
		base.Protocol.SetRequest<SprotoType.syn_rank_pvp_data.request>(541);
		base.Protocol.SetProtocol<sync_backpack_item>(592);
		base.Protocol.SetRequest<SprotoType.sync_backpack_item.request>(592);
		base.Protocol.SetProtocol<sync_badgepack_item>(604);
		base.Protocol.SetRequest<SprotoType.sync_badgepack_item.request>(604);
		base.Protocol.SetProtocol<sync_common_data>(614);
		base.Protocol.SetRequest<SprotoType.sync_common_data.request>(614);
		base.Protocol.SetProtocol<sync_copyscenes_info>(555);
		base.Protocol.SetRequest<SprotoType.sync_copyscenes_info.request>(555);
		base.Protocol.SetProtocol<sync_dance_state_info>(686);
		base.Protocol.SetRequest<SprotoType.sync_dance_state_info.request>(686);
		base.Protocol.SetProtocol<sync_fashion_backpack_item>(616);
		base.Protocol.SetRequest<SprotoType.sync_fashion_backpack_item.request>(616);
		base.Protocol.SetProtocol<sync_guild_new_member>(580);
		base.Protocol.SetRequest<SprotoType.sync_guild_new_member.request>(580);
		base.Protocol.SetProtocol<sync_item_pack>(611);
		base.Protocol.SetRequest<SprotoType.sync_item_pack.request>(611);
		base.Protocol.SetProtocol<sync_mission>(519);
		base.Protocol.SetRequest<SprotoType.sync_mission.request>(519);
		base.Protocol.SetProtocol<sync_random_team_state>(681);
		base.Protocol.SetRequest<SprotoType.sync_random_team_state.request>(681);
		base.Protocol.SetProtocol<sync_skill_info>(540);
		base.Protocol.SetRequest<SprotoType.sync_skill_info.request>(540);
		base.Protocol.SetResponse<SprotoType.sync_skill_info.response>(540);
		base.Protocol.SetProtocol<sync_watch_video_info>(692);
		base.Protocol.SetRequest<SprotoType.sync_watch_video_info.request>(692);
		base.Protocol.SetProtocol<take_item_storagepack>(141);
		base.Protocol.SetRequest<SprotoType.take_item_storagepack.request>(141);
		base.Protocol.SetProtocol<team_kick>(164);
		base.Protocol.SetRequest<SprotoType.team_kick.request>(164);
		base.Protocol.SetProtocol<tianti_req_win_count_rewards>(157);
		base.Protocol.SetRequest<SprotoType.tianti_req_win_count_rewards.request>(157);
		base.Protocol.SetProtocol<tiantti_result>(551);
		base.Protocol.SetRequest<SprotoType.tiantti_result.request>(551);
		base.Protocol.SetProtocol<title_req_level_up>(156);
		base.Protocol.SetRequest<SprotoType.title_req_level_up.request>(156);
		base.Protocol.SetProtocol<tower_reset>(230);
		base.Protocol.SetRequest<SprotoType.tower_reset.request>(230);
		base.Protocol.SetProtocol<tower_wipe_out>(208);
		base.Protocol.SetRequest<SprotoType.tower_wipe_out.request>(208);
		base.Protocol.SetProtocol<tutorial_finish>(306);
		base.Protocol.SetRequest<SprotoType.tutorial_finish.request>(306);
		base.Protocol.SetProtocol<unequip_badge>(198);
		base.Protocol.SetRequest<SprotoType.unequip_badge.request>(198);
		base.Protocol.SetProtocol<unequip_fashion_item>(222);
		base.Protocol.SetRequest<SprotoType.unequip_fashion_item.request>(222);
		base.Protocol.SetProtocol<unequip_item>(117);
		base.Protocol.SetRequest<SprotoType.unequip_item.request>(117);
		base.Protocol.SetProtocol<unlock_function_complete>(268);
		base.Protocol.SetRequest<SprotoType.unlock_function_complete.request>(268);
		base.Protocol.SetProtocol<unuse_mount>(239);
		base.Protocol.SetRequest<SprotoType.unuse_mount.request>(239);
		base.Protocol.SetProtocol<update_client_state>(280);
		base.Protocol.SetRequest<SprotoType.update_client_state.request>(280);
		base.Protocol.SetProtocol<update_copyscene_info>(561);
		base.Protocol.SetRequest<SprotoType.update_copyscene_info.request>(561);
		base.Protocol.SetProtocol<update_game_server>(7);
		base.Protocol.SetRequest<SprotoType.update_game_server.request>(7);
		base.Protocol.SetResponse<SprotoType.update_game_server.response>(7);
		base.Protocol.SetProtocol<update_guild_dance_time>(315);
		base.Protocol.SetRequest<SprotoType.update_guild_dance_time.request>(315);
		base.Protocol.SetProtocol<update_guild_star>(294);
		base.Protocol.SetRequest<SprotoType.update_guild_star.request>(294);
		base.Protocol.SetProtocol<update_item>(525);
		base.Protocol.SetRequest<SprotoType.update_item.request>(525);
		base.Protocol.SetProtocol<update_line_state>(568);
		base.Protocol.SetRequest<SprotoType.update_line_state.request>(568);
		base.Protocol.SetProtocol<update_misison_complete>(182);
		base.Protocol.SetRequest<SprotoType.update_misison_complete.request>(182);
		base.Protocol.SetProtocol<update_misison_parm>(178);
		base.Protocol.SetRequest<SprotoType.update_misison_parm.request>(178);
		base.Protocol.SetProtocol<update_player_map_info>(324);
		base.Protocol.SetRequest<SprotoType.update_player_map_info.request>(324);
		base.Protocol.SetResponse<SprotoType.update_player_map_info.response>(324);
		base.Protocol.SetProtocol<update_queue_rank>(577);
		base.Protocol.SetRequest<SprotoType.update_queue_rank.request>(577);
		base.Protocol.SetProtocol<update_sex_mini_score>(277);
		base.Protocol.SetRequest<SprotoType.update_sex_mini_score.request>(277);
		base.Protocol.SetProtocol<update_team>(518);
		base.Protocol.SetRequest<SprotoType.update_team.request>(518);
		base.Protocol.SetProtocol<update_team_member>(575);
		base.Protocol.SetRequest<SprotoType.update_team_member.request>(575);
		base.Protocol.SetProtocol<update_team_setting>(163);
		base.Protocol.SetRequest<SprotoType.update_team_setting.request>(163);
		base.Protocol.SetProtocol<urge_team_leader>(450);
		base.Protocol.SetRequest<SprotoType.urge_team_leader.request>(450);
		base.Protocol.SetProtocol<use_dance>(229);
		base.Protocol.SetRequest<SprotoType.use_dance.request>(229);
		base.Protocol.SetProtocol<use_dance_sound_box>(314);
		base.Protocol.SetRequest<SprotoType.use_dance_sound_box.request>(314);
		base.Protocol.SetProtocol<use_item>(115);
		base.Protocol.SetRequest<SprotoType.use_item.request>(115);
		base.Protocol.SetProtocol<use_mount>(238);
		base.Protocol.SetRequest<SprotoType.use_mount.request>(238);
		base.Protocol.SetProtocol<use_skill_buff>(209);
		base.Protocol.SetRequest<SprotoType.use_skill_buff.request>(209);
		base.Protocol.SetProtocol<verfiy>(3);
		base.Protocol.SetRequest<SprotoType.verfiy.request>(3);
		base.Protocol.SetResponse<SprotoType.verfiy.response>(3);
		base.Protocol.SetProtocol<visitor>(2);
		base.Protocol.SetRequest<SprotoType.visitor.request>(2);
		base.Protocol.SetResponse<SprotoType.visitor.response>(2);
		base.Protocol.SetProtocol<watch_video_info>(452);
		base.Protocol.SetRequest<SprotoType.watch_video_info.request>(452);
		base.Protocol.SetProtocol<weapon_inhert>(305);
		base.Protocol.SetRequest<SprotoType.weapon_inhert.request>(305);
	}
}
