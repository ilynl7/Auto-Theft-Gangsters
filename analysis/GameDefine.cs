using System.Collections.Generic;
using UnityEngine;

public class GameDefine
{
	public enum OBJ_TYPE
	{
		OBJ_NPC,
		OBJ_MAIN_PLAYER,
		OBJ_OTHER_PLAYER,
		OBJ_ZOMBIE_PLAYER,
		OBJ_DROP_ITEM,
		OBJ_COLLECT_ITEM,
		OBJ_PLAYER_CAR,
		OBJ_NPC_CAR,
		OBJ_ZOMBIE_RAGDOLL
	}

	public enum ITEM_TYPE
	{
		INVALID = -1,
		ITEM = 0,
		EQUIP = 2,
		POTION = 3,
		ENHANCE_ITEM = 4,
		MISSION_ITEM = 5,
		BADGE = 6,
		ADD_COIN = 7,
		ADD_GOLD = 8,
		ADD_DIAMOND = 9,
		ADD_HONOR = 10,
		ADD_EXP = 11,
		DKP = 12,
		SHOW = 13,
		RES = 14,
		BOX = 15,
		FASHION_EQUIP = 16,
		SWIPE = 17,
		REMAIN = 18,
		EXCHANGE = 19,
		LOCK1 = 20,
		LOCK2 = 21,
		BUFF = 22,
		SCORE = 23,
		RENAME = 26,
		POTION_2 = 27,
		DANCE_TOOL = 28,
		ENEMYWARP_TOOL = 29,
		WORLDSPEAK = 30
	}

	public enum MONEY_TYPE
	{
		CASH,
		GOLD,
		DIAMOND,
		GUILD_CONTRIBUTE,
		BATTLECOIN,
		ACTIVITYCOIN
	}

	public enum SHOP_TYPE
	{
		TOOL_SHOP,
		EQUIP_SHOP,
		BIGSALE_SHOP,
		GUILD_SHOP,
		DOLLAR_SHOP,
		VIP_SHOP,
		BATTLECOIN_SHOP,
		ACTIVITY_SHOP
	}

	public enum ANIMATIONSTATE
	{
		IDLE,
		RUN,
		WALK,
		DIE,
		IDLE_ATTACK,
		DRIVING
	}

	public enum ACTIVITY_TYPE
	{
		INVALID = -1,
		ALL,
		ESCORT,
		ATTACK_ESCORT,
		CITY_DANCE,
		BAR_FIGHT,
		WILD_BOSS,
		GUILD_BOSS,
		SURVIVE_BATTLE,
		SEX_MINI,
		GUILD_BATTLE,
		DAILY_COPY,
		MISSION,
		TOWER,
		RANKPVP,
		DOMIN,
		FIRST_GUILD_DANCE,
		GUILD_DANCE,
		MISSION_TIMEOUT,
		GUILD_DONMINE,
		GUILD_DONMINE_RES,
		KILL_PLAYER,
		SHOP_GATE,
		COUNT
	}

	public enum DANCE_TYPE
	{
		SINGLE,
		SINGLE_TOOL,
		GANG,
		GANG_TOOL
	}

	public enum ACTIVITY_STATE
	{
		NORMAL,
		LEVEL_LIMIT,
		TIME_LIMIT,
		TIMESNUM_LIMIT,
		BARFIGHT_NOSIGN,
		TIME_SIGN_LIMIT,
		LEVEL_BELOW_LIMIT,
		LEVEL_HEIGH_LIMIT
	}

	public enum CAMP_TYPE
	{
		PLAYER_1,
		PLAYER_2,
		NORMAL_NPC,
		NPC_ATTACK_NPC,
		STATIC_NPC,
		FUNCTION_NPC,
		PLAYER_FRIEND_NPC,
		COUNT
	}

	public enum NPC_FUNCTION_TYPE
	{
		NORMAL,
		SHIFT,
		CITIZEN_NPC,
		SOUND_BOX,
		RECHARGE,
		TOOL_SHOP,
		EQUIP_SHOP,
		BIGSALE_SHOP,
		GANG_SHOP,
		MONTH_CARD,
		GANGCITY_NPC
	}

	public enum NPC_TYPE
	{
		NORMAL,
		ELITE,
		BOSS,
		MISSION,
		ESCORT
	}

	public enum Mail_Type
	{
		OPEN,
		DELETE,
		GET,
		GET_ALL,
		DELETE_ALL
	}

	public enum SCENE_DEFINE
	{
		SCENE_LOGIN = 0,
		SCENE_LODING = 1,
		SCENE_ANIMAEDITOR = 9,
		SCENE_TUTORIAL_CAR = 11,
		SCENE_TUTORIAL_GAMBLING = 12,
		SCENE_MAIN_CITY = 101,
		SCENE_SLUM_CITY = 102,
		SCENE_TUTORIAL = 103,
		SCENE_GAMBLING_CITY = 104,
		SCENE_BUSINESS_CITY = 105,
		SCENE_BEACH_CITY = 106,
		SCENE_CHINA_TOWN = 107,
		SCENE_RICH_CITY = 108,
		SCENE_ESCORT_TEST = 109,
		SCENE_PVP_1 = 110,
		SCENE_PVP_2 = 111,
		SCENE_GARAGE = 201,
		SCENE_CAR_CHASE = 401,
		SCENE_RANK_PVP_1 = 501,
		SCENE_REAL_PVP_1 = 502,
		GUILD_GARAGE = 601,
		CLAMBING_TOWER = 701,
		LOW_PHONE_SCENE = 3001,
		COUNT = 3002
	}

	public enum UIBACKTYPE
	{
		NOTHINTG,
		FASHION,
		EQUIP,
		BADGE,
		ITEM,
		ENHANCE
	}

	public enum TIPS_TYPE
	{
		ACTIVITY = 1,
		ACTIVITY_WILDBOSS,
		SOCIAL,
		GUILD,
		SLOT,
		VEHICLE,
		ACHIEVEMNT,
		CHARACTER,
		ITEMS,
		ENHANCE,
		SKILL,
		WELFARE,
		DAILYACT,
		SEVENDAY,
		MAIL,
		SHOP
	}

	public enum CommercailTYPE
	{
		LEVEL,
		INVEST,
		FIRSTBUY,
		BIGSALE,
		DAILYBUY,
		DAILYACTIVE,
		SHOP,
		VIP
	}

	public enum AUTOPOPTYPE
	{
		NOTHING,
		RATE,
		SIGNMONTH,
		SIGNWEEK,
		BIGSALE,
		FIRSTBUY,
		MYSTERYSHOP,
		RETRIEVE,
		TIME_ACTIVITY_TIPS
	}

	public enum STRONGER_ACTIVITY
	{
		SKILL,
		ENHANCE_CUS,
		HONOR,
		ENHANCE_STR,
		ENHANCE_FUSE,
		MAIN_LINE,
		DAILY_LINE,
		EQUIP_COPY,
		EXP_COPY,
		ESCORT,
		DANCE,
		BARFIGHT,
		SCUFFLE_AREA,
		CASH_COPY,
		CAR_COPY,
		TOWER,
		WILD_BOSS,
		PVP,
		DIAMOND_BUY,
		SURVIVE_BATTLE,
		GUILD_SHOP,
		DIAMOND_SHOP,
		GOLD_SHOP,
		SLOT,
		GUILD_DONATE,
		GUILD_BOSS,
		CASH_SHOP,
		SEX_GAME,
		TRADE,
		BIGSALE,
		FIRSTBUY,
		GUILDSKILL,
		EQUIPMORE,
		EQUIPBEST,
		PKMAP
	}

	public enum CHAT_CHANNEL_TYPE
	{
		INVALID = -1,
		SYSTEM,
		NORMAL,
		WORLD,
		TEAM,
		GUILD,
		PRIVATE,
		NOTIFY,
		BROAT_CAST,
		COUNT
	}

	public enum CHAT_LINK_TYPE
	{
		INVALID = -1,
		ITEM,
		EQUIP,
		TEAM,
		GUILD,
		GUILD_BOSS,
		ESCORT,
		DANCE,
		BAR_FIGHT,
		WILD_BOSS,
		SURVIVE,
		ATTACK_ESCORT,
		GUILD_DANCE,
		FIRST_GUILD_DANCE,
		GUILD_DONMINE,
		GUILD_DONMINE_RES
	}

	public enum DAILY_ACTIVE_TYPE
	{
		EQUIP_COPY,
		EXP_COPY,
		CASH_COPY,
		CAR_COPY,
		ATTACK_ESCORT,
		TOWER_COPY,
		WILD_BOSS,
		RANK_PVP,
		EQUIP_UPGRADE,
		REFINE_UPGRADE,
		SKILL_UPGRADE,
		GOLD_BUY,
		DIAMOND_BUY,
		DAILY_BUY,
		ESCORT,
		DOMIN,
		SCUFFLE_AREA,
		SINGLE_DANCE,
		SURVIVE,
		GUILD_BOSS,
		GUILD_DANCE
	}

	public enum DAMAGEBOARD_TYPE
	{
		PLAYER_HP_DOWN = 1,
		TARGET_HPDOWN_PARTNER,
		TARGET_HPDOWN_PLAYER,
		PLAYER_ATTACK_MISS,
		TARGET_ATTACK_MISS,
		SKILL_NAME,
		PLAYER_ATTACK_CRITICAL,
		TARGET_ATTACK_CRITICAL,
		SKILL_NAME_NPC,
		PLAYER_HP_UP,
		COUNT
	}

	public enum BROAD_CAST_TYPE
	{
		SLOT,
		WILD_BOSS,
		PACK_ITEM,
		RANK_PVP,
		REFINE,
		BADGE,
		SURVIVE,
		TITLE,
		GUILD_LEVEL,
		FUNC_LEVEL,
		REFINE_2,
		GUILD_BOSS,
		CAR_RANK,
		BADGE_2,
		BAR_FIGHT,
		SURVIVE_2,
		WILD_BOSS_2,
		DANCE,
		RAID_BOSS,
		GUILD_BOSS_2,
		GUILD_BATTLE,
		FIRST_GUILD_DANCE,
		RARE_ITEM
	}

	public enum SHOP_TAB_TYPE
	{
		INVALID,
		FASHION,
		ITEM,
		EXCHANGE,
		TICKET,
		CASE,
		EQUIP,
		PETANIMAL,
		PIECE
	}

	public const int SKILL_DRAG_LEVEL = 25;

	public const int MAX_TEAM_MEMBER = 4;

	public const float SELL_ITEM_MUTI_NUM = 1f;

	public const string POINTLOCKNAME = "Activity_Point_6";

	public const string LOCKFLAGNAME = "CZ_effect_suo";

	public const string DOORITEMNAME = "City_jinRu";

	public const string ICON_EMPTY_NAME = "CZ_zhuangBeiCao";

	public const float CONSIGN_SELL_LOW = 0.5f;

	public const float CONSIGN_SELL_HEIGHT = 1.5f;

	public const string NORMAL_BUTTON_SPRITE_NAME = "CZ_anNiu_2";

	public const string GRAY_BUTTON_SPRITE_NAME = "CZ_anNiu_2+";

	public const string CASH_GET_ITEMID = "5001";

	public const string CASH_GET_ITEMID1 = "5002";

	public const string CASH_GET_ITEMID2 = "5003";

	public const string GOLD_GET_ITEMID = "5006";

	public const string PVP_NAME = "#{100125}";

	public const string PVE_NAME = "#{100124}";

	public const string GANG_NAME = "#{100114}";

	public const string ENHANCE_ID = "3001";

	public const string REFINE_ID1 = "4001";

	public const string REFINE_ID2 = "4002";

	public const string REFINE_Safty = "4003";

	public const string HOUNR_ID = "2002";

	public const string POTION_ITEMID1 = "9001";

	public const string POTION_ITEMID2 = "9002";

	public const string POTION_ITEMID3 = "9003";

	public const string EXP_ITEM3 = "5013";

	public const string SecondCarId = "1002";

	public const string WeaponPackID = "4";

	public const string Badge_Pack_ITEM = "9602";

	public const string Speaker_ITEM = "5026";

	public const string Buy_Badge_Tutorial_Id = "9999";

	public const string CashCopyTicket = "9203";

	public const string ExpCopuTicket = "9202";

	public const string EQUIPCOPYID = "901";

	public const string EXPCOPYID = "223";

	public const string CASHCOPYID = "201";

	public const string CARCOPYID = "211";

	public const string SCUFFLE_AREA_ID = "1201";

	public const string ATTACKESCORTID = "30002";

	public const string ESCORTID = "30001";

	public const string WILDBOSSID = "801";

	public const string SURVIVE = "1101";

	public const string BigSaleCarID = "2";

	public const string ReNameCardID = "5024";

	public static int BUF_USE_SUCCESS = -2;

	public static int BUF_USE_FAIL = -1;

	public static int AutoReDownloadTimes = 3;

	public static int DayCentiSecond = 8640000;

	public static int NotifyTime1 = 600;

	public static string GameName = "#{200405}";

	public static string[] MAP_ACTIVITY_ICON = new string[12]
	{
		string.Empty,
		"CZ_xianShiRenWu",
		"CZ_xianShiRenWu",
		"CZ_xianShiRenWu",
		"CZ_xianShiRenWu",
		"CZ_xianShiRenWu",
		"CZ_xianShiRenWu",
		"CZ_xianShiRenWu",
		"CZ_riChangRenWu",
		"CZ_riChangRenWu",
		"CZ_riChangRenWu",
		"CZ_riChangRenWu"
	};

	public static string[] MAP_ACTIVITY_MISSION_ICON = new string[9] { "CZ_riHuanRenWu", "CZ_zhuXianRenWu", "CZ_zhiXianRenWu", "CZ_riHuanRenWu", "CZ_xianShiRenWu", "CZ_xianShiRenWu", "CZ_riHuanRenWu", "CZ_riHuanRenWu", "CZ_lianXianRenWu" };

	public static string LOGIN_SCENE_NAME = "Login";

	public static Dictionary<int, string> STRONGER_TYPE_NAME = new Dictionary<int, string>
	{
		{ 1, "#{800001}" },
		{ 2, "#{800002}" },
		{ 3, "#{800003}" },
		{ 4, "#{800004}" },
		{ 5, "#{800005}" },
		{ 6, "#{800006}" },
		{ 7, "#{800007}" },
		{ 8, "#{800008}" },
		{ 9, "#{800009}" },
		{ 10, "#{800010}" },
		{ 11, "#{800011}" },
		{ 12, "#{800012}" }
	};

	public static Dictionary<int, string> ACTIVITYPOINTNAME = new Dictionary<int, string>
	{
		{ 1, "Activity_Point_1" },
		{ 2, "Activity_Point_2" },
		{ 3, "Activity_Point_3" },
		{ 4, "Activity_Point_4" },
		{ 5, "Activity_Point_5" }
	};

	public static string SingleDanceToolItem = "6001";

	public static string GangDanceToolItem = "6002";

	public static string EnemyWarpToolItem = "5025";

	public static Dictionary<string, string> DanceToolName = new Dictionary<string, string>
	{
		{ "6001", "CZ_gaWuDaoJu_geRen" },
		{ "6002", "CZ_gaWuDaoJu_gongHui" }
	};

	public static string AtkBuffItemId = "9501";

	public static string DefBuffItemId = "9503";

	public static string MovBuffItemId = "9502";

	public static string[] XD_DefaultModel = new string[4] { "XD_A_WQ", "XD_0_T", "XD_0_S", "XD_0_X" };

	public static string[] QJ_DefaultModel = new string[4] { "QJ_A_WQ", "QJ_0_T", "QJ_0_S", "QJ_0_X" };

	public static string[] NQS_DefaultModel = new string[4] { "NQS_A_WQ", "NQS_0_T", "NQS_0_S", "NQS_0_X" };

	public static string[] XD_NormalModel = new string[4] { "XD_A_WQ", "XD_A_T", "XD_A_S", "XD_A_X" };

	public static string[] QJ_NormalModel = new string[4] { "QJ_A_WQ", "QJ_A_T", "QJ_A_S", "QJ_A_X" };

	public static string[] NQS_NormalModel = new string[4] { "NQS_A_WQ", "NQS_A_T", "NQS_A_S", "NQS_A_X" };

	public static string[] Player_Icon_Pic = new string[3] { "CZ_touXiang_1", "CZ_touXiang_2", "CZ_touXiang_3" };

	public static string[] Game_Player_Icon_pic = new string[3] { "CZ_xieDouTouXiang", "CZ_quanJiTouXiang", "CZ_nvQiangTouXiang" };

	public static string[] Player_Icon_Small_Pic = new string[4] { "CZ_touXiang_yuan1", "CZ_touXiang_yuan2", "CZ_touXiang_yuan3", "CZ_touXiang_XiTong" };

	public static string[] Player_Profession_Pic = new string[3] { "CZ_xieDouTouXiang", "CZ_quanJiTouXiang", "CZ_nvQiangTouXiang" };

	public static string[] TianTi_TuBiao = new string[10] { "CZ_A_tianTi_tuBiao_1", "CZ_A_tianTi_tuBiao_2", "CZ_A_tianTi_tuBiao_3", "CZ_A_tianTi_tuBiao_4", "CZ_A_tianTi_tuBiao_5", "CZ_A_tianTi_tuBiao_6", "CZ_A_tianTi_tuBiao_7", "CZ_A_tianTi_tuBiao_8", "CZ_A_tianTi_tuBiao_9", "CZ_A_tianTi_tuBiao_10" };

	public static string[] MailIcon = new string[2] { "CZ_sheJiao_youJianWeiDuTB", "CZ_sheJiao_youJianTB" };

	public static string[] TianTi_AnNiu = new string[2] { "CZ_A_tianTi_sehngJiAnNiuLiang", "CZ_A_tianTi_sehngJiAnNiu" };

	public static string[] GuildJobStr = new string[4] { "#{100720}", "#{100721}", "#{100722}", "#{100703}" };

	public static string[] GuildJOb = new string[4] { "BOSS", "VICEBOSS", "ELDER", "MEMBER" };

	public static string[] GuildIcon = new string[8] { "CZ_gongHui_TuBiao1", "CZ_gongHui_TuBiao2", "CZ_gongHui_TuBiao3", "CZ_gongHui_TuBiao4", "CZ_gongHui_TuBiao5", "CZ_gongHui_TuBiao6", "CZ_gongHui_TuBiao7", "CZ_gongHui_TuBiao8" };

	public static string[] DonateTipsTB = new string[2] { "CZ_A_HSXZG", "CZ_A_jinZhi" };

	public static string[] RefinePartName = new string[5] { "1", "#{100908}", "#{100909}", "#{100910}", "#{100911}" };

	public static string[] BtnIcon = new string[3] { "CZ_anNiu_1", "CZ_anNiu_2", "CZ_anNiu_2+" };

	public static string[] BtnIconNew = new string[2] { "CZ_SY_AnNiu_1", "CZ_SY_AnNiu_2" };

	public static string[] RoundName = new string[3] { "#{105002}", "#{105003}", "#{105004}" };

	public static int MISSION_NPC_GROUP_VAL = 9999;

	public static int CopyMissionBestGrade = 2;

	public static string[] CHANNEL_PRE_WORD = new string[6] { "#{100282}", "#{100283}", "#{100284}", "#{100285}", "#{100286}", "#{100287}" };

	public static Dictionary<int, string> EquipQualityValAddName = new Dictionary<int, string>
	{
		{ 0, "EquipQuality_White" },
		{ 1, "EquipQuality_Green" },
		{ 2, "EquipQuality_Blue" },
		{ 3, "EquipQuality_Purple" },
		{ 4, "EquipQuality_Orange" },
		{ 5, "EquipQuality_Red" }
	};

	public static Dictionary<int, string> WeaponTypeName = new Dictionary<int, string>
	{
		{ 0, "XD" },
		{ 1, "QJ" },
		{ 2, "QS" }
	};

	public static Dictionary<string, int> NameWeaponType = new Dictionary<string, int>
	{
		{ "XD", 0 },
		{ "QJ", 1 },
		{ "QS", 2 }
	};

	public static int MAX_REFINE_LEVEL = 10;

	public static int MAX_BADGE_LEVEL = 8;

	public static int PLAYER_MAX_LEVEL = 80;

	public static int MAX_FRIENT_COUNT = 100;

	public static int MAX_APPLY_FRIEND_COUNT = 100;

	public static int MAX_ENEMY_COUNT = 30;

	public static string IdleSelectAnimaName = "idle_select";

	public static string ShowSelectAnimaName = "show_select";

	public static string[] RoleIdleSelectAnimaName = new string[3] { "idle_select", "idle", "idle_attack" };

	public static string[] RoleShowSelectAnimaName = new string[3] { "show_select", "show_select", "show_select" };

	public static string DailyMissionBoardMapId = "101";

	public static Vector3 DailyMissionBoardPos = new Vector3(-25f, 0f, -34.73339f);

	public static int SKILL_MAX_LEVEL = 80;

	public static string[] RANK_PICNAME = new string[3] { "CZ_A_paiHang_diYiMing", "CZ_A_paiHang_diErMing", "CZ_A_paiHang_diSanMing" };

	public static string[] BATTLE_WIN_PIC = new string[3] { "CZ_gongHui_HuoShengYiCi", "CZ_gongHui_HuoShengErCi", "CZ_gongHui_GuanJun" };

	public static string[] Profession_PicName = new string[3] { "CZ_zhiYeTuBiao_1", "CZ_zhiYeTuBiao_2", "CZ_zhiYeTuBiao_3" };

	public static Color GrayColor = new Color(0.11764706f, 0.11764706f, 0.11764706f, 1f);

	public static Dictionary<MONEY_TYPE, string> ITEM_ID = new Dictionary<MONEY_TYPE, string>
	{
		{
			MONEY_TYPE.CASH,
			"1001"
		},
		{
			MONEY_TYPE.GOLD,
			"1002"
		},
		{
			MONEY_TYPE.DIAMOND,
			"1003"
		},
		{
			MONEY_TYPE.GUILD_CONTRIBUTE,
			"1004"
		}
	};

	public static Dictionary<string, MONEY_TYPE> ITEM_ID_MONEYTYPR = new Dictionary<string, MONEY_TYPE>
	{
		{
			"1001",
			MONEY_TYPE.CASH
		},
		{
			"1002",
			MONEY_TYPE.GOLD
		},
		{
			"1003",
			MONEY_TYPE.DIAMOND
		},
		{
			"1004",
			MONEY_TYPE.GUILD_CONTRIBUTE
		}
	};

	public static Dictionary<int, string> ATTRIBUTE_ICON = new Dictionary<int, string>
	{
		{ 1001, "CZ_renWuShuXingTuBiao_1" },
		{ 1002, "CZ_renWuShuXingTuBiao_2" },
		{ 1003, "CZ_renWuShuXingTuBiao_3" },
		{ 1004, "CZ_renWuShuXingTuBiao_4" },
		{ 1005, "CZ_renWuShuXingTuBiao_5" },
		{ 1006, "CZ_renWuShuXingTuBiao_6" },
		{ 1007, "CZ_renWuShuXingTuBiao_7" },
		{ 1008, "CZ_renWuShuXingTuBiao_8" },
		{ 1009, "CZ_renWuShuXingTuBiao_9" },
		{ 1010, "CZ_renWuShuXingTuBiao_10" },
		{ 1011, "CZ_renWuShuXingTuBiao_11" },
		{ 1012, "CZ_renWuShuXingTuBiao_12" },
		{ 1013, "CZ_renWuShuXingTuBiao_13" }
	};

	public static Dictionary<int, Color> QUALITY_COLOR = new Dictionary<int, Color>
	{
		{
			0,
			new Color(0.5882353f, 0.5882353f, 0.5882353f, 1f)
		},
		{
			1,
			new Color(0.007843138f, 2f / 3f, 0f, 1f)
		},
		{
			2,
			new Color(0f, 44f / 85f, 1f, 1f)
		},
		{
			3,
			new Color(0.6784314f, 0f, 1f, 1f)
		},
		{
			4,
			new Color(1f, 0.7058824f, 0f, 1f)
		},
		{
			5,
			new Color(1f, 0f, 0f, 1f)
		}
	};

	public static Dictionary<int, string> StarAttIntegral = new Dictionary<int, string>
	{
		{ 0, "StarAttIntegral_White" },
		{ 1, "StarAttIntegral_Green" },
		{ 2, "StarAttIntegral_Blue" },
		{ 3, "StarAttIntegral_Purple" },
		{ 4, "StarAttIntegral_Orange" },
		{ 5, "StarAttIntegral_Red" }
	};

	public static string[] EquipQualityScoreName = new string[6] { "EquipQuality0", "EquipQuality1", "EquipQuality2", "EquipQuality3", "EquipQuality4", "EquipQuality5" };

	public static string[] EquipStarScoreName = new string[7] { "EquipStar1", "EquipStar2", "EquipStar3", "EquipStar4", "EquipStar5", "EquipStar6", "EquipStar7" };

	public static Dictionary<int, string> EquipInherited = new Dictionary<int, string>
	{
		{ 0, "EquipInherited_White" },
		{ 1, "EquipInherited_Green" },
		{ 2, "EquipInherited_Blue" },
		{ 3, "EquipInherited_Purple" },
		{ 4, "EquipInherited_Orange" },
		{ 5, "EquipInherited_Red" }
	};

	public static Dictionary<int, string> WeaponInherited = new Dictionary<int, string>
	{
		{ 0, "WeaponInherited_White" },
		{ 1, "WeaponInherited_Green" },
		{ 2, "WeaponInherited_Blue" },
		{ 3, "WeaponInherited_Purple" },
		{ 4, "WeaponInherited_Orange" },
		{ 5, "WeaponInherited_Red" }
	};

	public static Dictionary<int, string> QUALITY_NAME = new Dictionary<int, string>
	{
		{ 0, "#{100625}" },
		{ 1, "#{100626}" },
		{ 2, "#{100627}" },
		{ 3, "#{100628}" },
		{ 4, "#{100628}" }
	};

	public static Dictionary<int, string> WEEK_NAME = new Dictionary<int, string>
	{
		{ 0, "#{106019}" },
		{ 1, "#{106020}" },
		{ 2, "#{106021}" },
		{ 3, "#{106022}" },
		{ 4, "#{106023}" },
		{ 5, "#{106024}" },
		{ 6, "#{106025}" }
	};

	public static Dictionary<int, string> ATTRIBUTE_NAME = new Dictionary<int, string>
	{
		{ 1001, "#{100402}" },
		{ 1002, "#{100403}" },
		{ 1003, "#{100404}" },
		{ 1004, "#{100405}" },
		{ 1005, "#{100406}" },
		{ 1006, "#{100407}" },
		{ 1007, "#{100408}" },
		{ 1008, "#{100409}" },
		{ 1009, "#{100410}" },
		{ 1010, "#{100422}" },
		{ 1011, "#{100420}" },
		{ 1012, "#{100423}" },
		{ 1013, "#{100424}" }
	};

	public static Dictionary<int, string> ATTRIBUTE_NAME_S = new Dictionary<int, string>
	{
		{ 2001, "#{100411}" },
		{ 2002, "#{100412}" },
		{ 2003, "#{100413}" },
		{ 2004, "#{100414}" },
		{ 2005, "#{100415}" },
		{ 2006, "#{100416}" },
		{ 2007, "#{100417}" },
		{ 2008, "#{100418}" },
		{ 2009, "#{100419}" },
		{ 2010, "#{100422}" },
		{ 2011, "#{100420}" },
		{ 2012, "#{100437}" },
		{ 2013, "#{100438}" }
	};

	public static Dictionary<int, float> ATTRIBUTE_COMBAT_VAL_DIC_XD = new Dictionary<int, float>
	{
		{ 1001, 16f },
		{ 1002, 1f },
		{ 1003, 11f },
		{ 1004, 2f },
		{ 1005, 5.5f },
		{ 1006, 10f },
		{ 1007, 10f },
		{ 1008, 5f },
		{ 1009, 5f },
		{ 1012, 5f },
		{ 1013, 5f },
		{ 2001, 5f },
		{ 2002, 5f },
		{ 2003, 5f },
		{ 2004, 5f },
		{ 2005, 5f },
		{ 2006, 5f },
		{ 2007, 5f },
		{ 2008, 5f },
		{ 2009, 5f },
		{ 2012, 5f },
		{ 2013, 5f }
	};

	public static Dictionary<int, float> ATTRIBUTE_COMBAT_VAL_DIC_QJ = new Dictionary<int, float>
	{
		{ 1001, 20f },
		{ 1002, 1f },
		{ 1003, 12f },
		{ 1004, 1f },
		{ 1005, 6f },
		{ 1006, 5f },
		{ 1007, 10f },
		{ 1008, 5f },
		{ 1009, 5f },
		{ 1012, 5f },
		{ 1013, 5f },
		{ 2001, 5f },
		{ 2002, 5f },
		{ 2003, 5f },
		{ 2004, 5f },
		{ 2005, 5f },
		{ 2006, 5f },
		{ 2007, 5f },
		{ 2008, 5f },
		{ 2009, 5f },
		{ 2012, 5f },
		{ 2013, 5f }
	};

	public static Dictionary<int, float> ATTRIBUTE_COMBAT_VAL_DIC_NQS = new Dictionary<int, float>
	{
		{ 1001, 7f },
		{ 1002, 1f },
		{ 1003, 7.4f },
		{ 1004, 3f },
		{ 1005, 3.7f },
		{ 1006, 15f },
		{ 1007, 10f },
		{ 1008, 5f },
		{ 1009, 5f },
		{ 1012, 5f },
		{ 1013, 5f },
		{ 2001, 5f },
		{ 2002, 5f },
		{ 2003, 5f },
		{ 2004, 5f },
		{ 2005, 5f },
		{ 2006, 5f },
		{ 2007, 5f },
		{ 2008, 5f },
		{ 2009, 5f },
		{ 2012, 5f },
		{ 2013, 5f }
	};

	public static string[] ProfessionName = new string[3] { "#{100128}", "#{100129}", "#{100130}" };

	public static string[] WeaponName = new string[3] { "#{100676}", "#{100678}", "#{100677}" };

	public static Dictionary<int, string> SERVER_AREA_NAME = new Dictionary<int, string>
	{
		{ -1, "#{200084}" },
		{ 0, "#{200079}" },
		{ 1, "#{200081}" },
		{ 2, "#{200082}" }
	};

	public static Dictionary<int, Color> SERVER_STATE_COLOR = new Dictionary<int, Color>
	{
		{
			0,
			Color.green
		},
		{
			1,
			Color.yellow
		},
		{
			2,
			Color.red
		},
		{
			3,
			Color.white
		}
	};

	public static Dictionary<int, string> SHOP_TAB_NAME = new Dictionary<int, string>
	{
		{ 1, "#{301107}" },
		{ 2, "#{301108}" },
		{ 3, "#{301109}" },
		{ 4, "#{301110}" },
		{ 5, "#{301111}" },
		{ 6, "#{301121}" },
		{ 7, "#{301122}" },
		{ 8, "#{301123}" }
	};

	public static int[] SHOP_TAB_SORT = new int[8] { 3, 1, 6, 7, 8, 2, 4, 5 };

	public static string TowerIconName = "CZ_paTa_PVE";

	public static string EmptyBadgeIconName = "CZ_zhuangBeiCao_jia";

	public static string EmptyItemIconName = "CZ_zhuangBeiCao";

	public static string EmptyEquipIconName = "CZ_zhuangBeiCao_jia";

	public static string EmptyAddItemID = "9900";

	public static string GuildStarBG = "CZ_xingPan_BeiJing";

	public static string GuildXingpanDi = "CZ_xingPanDi";

	public static string TextureBannerDaily = "banner_Daily";

	public static string TextureBannerLevel = "banner_Level";

	public static string TextrueBannerInvest = "banner_TouZiJiHua";

	public static string TextureBannerRetrieve = "banner_zhaoHui";

	public static string TestureBannerStronger = "banner_bianQiang";

	public static string TextureBigSale = "CZ_SY_BIG_SALE";

	public static string TextureBigSaleBG = "CZ_SY_BIG_SALE_BG";

	public static string TextureFirstBuy = "CZ_SY_First_RR";

	public static string TextureFirstBuyBG = "CZ_SY_First_RR_BG";

	public static string WorldMap = "CZ_diTu_ShiJie";

	public static string SlotBigWin = "CZ_solt_bigWin";

	public static string SlotBigWinBian = "CZ_solt_bigWinCaiDai";

	public static string LevelRewardBg = "FuBenTuBiao_DANCE";

	public static string CopyBGNameDefault = "DENGLUTU";

	public static string[] LoadingTips = new string[18]
	{
		"#{700001}", "#{700002}", "#{700003}", "#{700004}", "#{700005}", "#{700006}", "#{700007}", "#{700008}", "#{700009}", "#{700010}",
		"#{700011}", "#{700012}", "#{700001}", "#{700006}", "#{700015}", "#{700016}", "#{700017}", "#{700018}"
	};

	public static string CASH_ITEM_ID = "1001";

	public static string GOLD_ITEM_ID = "1002";

	public static string DIAMOND_ITEM_ID = "1003";

	public static string XD_INVINCIBLE_SKILL_ID = "199";

	public static string QJ_INVINCIBLE_SKILL_ID = "299";

	public static string NQS_INVINCIBLE_SKILL_ID = "399";

	public static int NPC_SERVER_WAIT_RELIFE_NUM = 5;

	public static float NPC_HP_LINE_SHOW_TIME = 10f;

	public static string SEX_MINI_GAME_ID = "1301";

	public static string GetMissionStateIcon(MISSION_STATE state)
	{
		return state switch
		{
			MISSION_STATE.INVALID => "CZ_renWu_tanHao", 
			MISSION_STATE.ACCEPTED => "CZ_renWu_wenHao_hui", 
			MISSION_STATE.COMPLETE => "CZ_renWu_wenHao", 
			_ => string.Empty, 
		};
	}

	public static string GetObjTypeSpriteName(ObjNPC obj)
	{
		string result = "CZ_diTu_TuBiao_hongDian";
		switch (obj.NPCType)
		{
		case NPC_TYPE.BOSS:
			if (obj.IsCityCaptureNpc())
			{
				result = "CZ_GangDomain_paiMing";
			}
			break;
		case NPC_TYPE.MISSION:
			result = "CZ_diTu_TuBiao_lvDian";
			break;
		case NPC_TYPE.ESCORT:
			if (Singleton<ObjManager>.Instance.IsMyEscortNpc(obj))
			{
				result = "CZ_diTu_TuBiao_lvDian";
			}
			break;
		}
		return result;
	}

	public static string GetYellowColor(string str)
	{
		return $"[[ffff00]{str}[-]]";
	}

	public static string GetYellowColor(int value)
	{
		return $"[[ffff00]{value}[-]]";
	}

	public static string GetBoldStr(string str)
	{
		return $"[b]{str}[-]";
	}

	public static string GetWeaponName(string WeaponModelName)
	{
		if (!string.IsNullOrEmpty(WeaponModelName))
		{
			string[] array = WeaponModelName.Split('_');
			if (array != null && array.Length > 0)
			{
				if (array[0].Equals("XD"))
				{
					return "XD";
				}
				if (array[0].Equals("QJ"))
				{
					return "QJ";
				}
				if (array[0].Equals("NQS"))
				{
					return "QS";
				}
			}
		}
		return null;
	}

	public static int GetWeaponType(string WeaponModelName)
	{
		if (!string.IsNullOrEmpty(WeaponModelName))
		{
			string[] array = WeaponModelName.Split('_');
			if (array != null && array.Length > 0)
			{
				if (array[0].Equals("XD"))
				{
					return 0;
				}
				if (array[0].Equals("QJ"))
				{
					return 1;
				}
				if (array[0].Equals("NQS"))
				{
					return 2;
				}
			}
		}
		return -1;
	}

	public static string GetRoleIdelSelectName(int per, int weapon)
	{
		switch (weapon)
		{
		case 0:
			return RoleIdleSelectAnimaName[0];
		case 1:
			return RoleIdleSelectAnimaName[2];
		case 2:
			if (per == 2)
			{
				return RoleIdleSelectAnimaName[1];
			}
			return RoleIdleSelectAnimaName[2];
		default:
			return RoleIdleSelectAnimaName[1];
		}
	}

	public static string GetAttributeName(int type)
	{
		if (type > 2000)
		{
			type -= 1000;
		}
		if (ATTRIBUTE_NAME.ContainsKey(type))
		{
			return StrDictionary.GetDictionaryString(ATTRIBUTE_NAME[type]);
		}
		return string.Empty;
	}

	public static string GetAttributeName_S(int type)
	{
		if (type > 3000)
		{
			type -= 1000;
		}
		if (type < 2000)
		{
			type += 1000;
		}
		if (ATTRIBUTE_NAME_S.ContainsKey(type))
		{
			return StrDictionary.GetDictionaryString(ATTRIBUTE_NAME_S[type]);
		}
		return string.Empty;
	}

	public static string GetAttributeIcon(int type)
	{
		if (type > 2000)
		{
			type -= 1000;
		}
		if (ATTRIBUTE_ICON.ContainsKey(type))
		{
			return ATTRIBUTE_ICON[type];
		}
		return string.Empty;
	}

	public static string GetAttributeValueStr(int type, int value)
	{
		int num = type;
		if (num > 2000)
		{
			num -= 1000;
		}
		if (ATTRIBUTE_NAME.ContainsKey(num))
		{
			if (type > 2000 || type == 1008 || type == 1009 || type == 1012 || type == 1013)
			{
				return $"+{(float)value / 10000f:P1}";
			}
			return $"+{value}";
		}
		return string.Empty;
	}

	public static string GetAttributeValueStr2(int type, int value)
	{
		int num = type;
		if (num > 2000)
		{
			num -= 1000;
		}
		if (ATTRIBUTE_NAME.ContainsKey(num))
		{
			if (type > 2000 || type == 1008 || type == 1009 || type == 1012 || type == 1013)
			{
				return $"{(float)value / 10000f:P1}";
			}
			return $"{value}";
		}
		return string.Empty;
	}

	public static Color GetColorByQuality(EQUIP_QUALITY quality)
	{
		return GetColorByQuality((int)quality);
	}

	public static string GetStrByQuality(EQUIP_QUALITY quality)
	{
		return GetStrByQuality((int)quality);
	}

	public static string GetStrByQuality(int quality)
	{
		if (QUALITY_NAME.ContainsKey(quality))
		{
			return StrDictionary.GetDictionaryString(QUALITY_NAME[quality]);
		}
		return string.Empty;
	}

	public static Color GetColorByQuality(int quality)
	{
		if (QUALITY_COLOR.ContainsKey(quality))
		{
			return QUALITY_COLOR[quality];
		}
		return Color.white;
	}

	public static float GET_ATTRIBUTE_COMBAT_VAL(int attid)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		switch (playerData.Profession)
		{
		case PROFESSION_TYPE.XD:
			if (ATTRIBUTE_COMBAT_VAL_DIC_XD.ContainsKey(attid))
			{
				return ATTRIBUTE_COMBAT_VAL_DIC_XD[attid];
			}
			break;
		case PROFESSION_TYPE.QJ:
			if (ATTRIBUTE_COMBAT_VAL_DIC_QJ.ContainsKey(attid))
			{
				return ATTRIBUTE_COMBAT_VAL_DIC_QJ[attid];
			}
			break;
		case PROFESSION_TYPE.NQS:
			if (ATTRIBUTE_COMBAT_VAL_DIC_NQS.ContainsKey(attid))
			{
				return ATTRIBUTE_COMBAT_VAL_DIC_NQS[attid];
			}
			break;
		}
		return 0f;
	}

	public static string GetActKey_FirstClick(ACTIVITY_TYPE type)
	{
		return $"ACTIVITY{(int)type}";
	}

	public static string GetCopyKey_FirstClick(MAPTYPE type)
	{
		return $"COPY{(int)type}";
	}

	public static float GetSkillTypeValue(EffInfoData data, OBJ_TYPE objType, SKILL_ADD_TYPE type)
	{
		if (objType == OBJ_TYPE.OBJ_NPC)
		{
			return 0f;
		}
		float num = 0f;
		if (data.AddType1 == (int)type)
		{
			num += (float)data.AddValue1;
		}
		if (data.AddType2 == (int)type)
		{
			num += (float)data.AddValue2;
		}
		if (data.AddType3 == (int)type)
		{
			num += (float)data.AddValue3;
		}
		if (data.AddType4 == (int)type)
		{
			num += (float)data.AddValue4;
		}
		return num / 10000f;
	}
}
