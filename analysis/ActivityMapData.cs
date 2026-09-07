using SprotoType;
using UnityEngine;

public class ActivityMapData
{
	public static int DefaultSubType = -1;

	public string ID = string.Empty;

	public int Type = -1;

	public int SubType = DefaultSubType;

	public string ActivityID = string.Empty;

	public string MapId = string.Empty;

	public int PosX;

	public int PosZ;

	public string Icon = string.Empty;

	public int Color;

	public int MinLevel = int.MinValue;

	public int MaxLevel = int.MaxValue;

	public string TargetMapID;

	private int isCheckUnlock = -1;

	private int isCheckVisible = -1;

	public Vector3 Position
	{
		get
		{
			float x = (float)PosX / 100f;
			float z = (float)PosZ / 100f;
			return new Vector3(x, SceneManager.GetHitHeight(x, z), z);
		}
	}

	public GameDefine.ACTIVITY_TYPE ActivityType => (GameDefine.ACTIVITY_TYPE)Type;

	public bool IsUnlock
	{
		get
		{
			if (ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
			{
				isCheckVisible = -1;
				isCheckUnlock = -1;
				Init();
				return isCheckUnlock == 1;
			}
			if (isCheckUnlock == 1)
			{
				return true;
			}
			Init();
			return isCheckUnlock == 1;
		}
	}

	public bool IsVisible
	{
		get
		{
			if (ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
			{
				isCheckVisible = -1;
				isCheckUnlock = -1;
				Init();
				return isCheckVisible == 1;
			}
			if (isCheckVisible == 1)
			{
				return true;
			}
			Init();
			return isCheckVisible == 1;
		}
	}

	public bool IsShowDoorFlag()
	{
		return ActivityType == GameDefine.ACTIVITY_TYPE.SHOP_GATE;
	}

	private void Init()
	{
		if (isCheckVisible != -1 && isCheckUnlock != -1)
		{
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerData == null)
		{
			return;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT || ActivityType == GameDefine.ACTIVITY_TYPE.ESCORT)
		{
			EscortData escortDataById = DataManager.GetEscortDataById(ActivityID);
			if (playerData.CheckLevel(escortDataById.UnlockLevel))
			{
				isCheckUnlock = 1;
			}
			isCheckVisible = 1;
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.BAR_FIGHT)
		{
			BarFightCopyData barFightCopyDataByID = DataManager.GetBarFightCopyDataByID(ActivityID);
			if (playerData.CheckLevel(barFightCopyDataByID.UnlockLevel))
			{
				isCheckUnlock = 1;
			}
			isCheckVisible = 1;
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.CITY_DANCE)
		{
			CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(ActivityID);
			if (playerData.CheckLevel(cityDanceDataById.UnlockLevel))
			{
				isCheckUnlock = 1;
			}
			isCheckVisible = 1;
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.DAILY_COPY)
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(ActivityID);
			if (copySceneDataById != null)
			{
				if (playerData.CheckLevel(copySceneDataById.MinLevel))
				{
					isCheckUnlock = 1;
				}
				isCheckVisible = 1;
			}
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BATTLE)
		{
			GuildBattleData guildBattleDataById = DataManager.GetGuildBattleDataById(ActivityID);
			if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GUILD_ACTIVITY))
			{
				isCheckUnlock = 1;
			}
			isCheckVisible = 1;
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BOSS)
		{
			GuildBossData guildBossDataByID = DataManager.GetGuildBossDataByID(ActivityID);
			if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GUILD_ACTIVITY) && playerData.IsHaveGuild())
			{
				isCheckUnlock = 1;
			}
			isCheckVisible = 1;
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.SEX_MINI)
		{
			SexMiniData sexMiniDataById = DataManager.GetSexMiniDataById(ActivityID);
			if (playerData.CheckLevel(sexMiniDataById.UnlockLevel))
			{
				isCheckUnlock = 1;
			}
			isCheckVisible = 1;
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE)
		{
			SurviveBattleData surviveBattleDataById = DataManager.GetSurviveBattleDataById(ActivityID);
			if (playerData.CheckLevel(surviveBattleDataById.UnlockLevel))
			{
				isCheckUnlock = 1;
			}
			isCheckVisible = 1;
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.WILD_BOSS)
		{
			WildBossData wildBossDataByID = DataManager.GetWildBossDataByID(ActivityID);
			if (playerData.CheckLevel(wildBossDataByID.LevelMin))
			{
				isCheckUnlock = 1;
			}
			isCheckVisible = 1;
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.IsMissionAcceptable(ActivityID))
			{
				isCheckUnlock = 1;
				isCheckVisible = 1;
			}
			else
			{
				isCheckUnlock = -1;
				isCheckVisible = -1;
			}
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.TOWER)
		{
			if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_CHALLENGE))
			{
				isCheckUnlock = 1;
			}
			isCheckVisible = 1;
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.RANKPVP)
		{
			if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.RANK_PVP))
			{
				isCheckUnlock = 1;
			}
			isCheckVisible = 1;
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.DOMIN)
		{
			if (playerData.Domin_InfoDic.ContainsKey(ActivityID))
			{
				DominData dominDataByID = DataManager.GetDominDataByID(ActivityID);
				if (playerData.Level >= dominDataByID.LevelMin)
				{
					isCheckUnlock = 1;
				}
				isCheckVisible = 1;
			}
		}
		else if (ActivityType == GameDefine.ACTIVITY_TYPE.SHOP_GATE)
		{
			isCheckVisible = 1;
			isCheckUnlock = 1;
		}
		if (MinLevel != int.MinValue && MaxLevel != int.MaxValue)
		{
			if (playerData.CheckLevel(MinLevel, MaxLevel))
			{
				isCheckVisible = 1;
			}
			else
			{
				isCheckVisible = -1;
			}
		}
	}

	public copyscene_info GetDailyCopyInfo()
	{
		if (ActivityType == GameDefine.ACTIVITY_TYPE.DAILY_COPY)
		{
			if (!IsNeedDailyActid())
			{
				PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				return playerData.CopyInfoData.GetCopyinfoByType(SubType);
			}
			PlayerData playerData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			return playerData2.CopyInfoData.GetCopyinfoByID(ActivityID);
		}
		return null;
	}

	public bool IsNeedDailyActid()
	{
		if (SubType == 12 || SubType == 7 || SubType == 11 || SubType == 20 || SubType == 16 || SubType == 26 || SubType == 27)
		{
			return false;
		}
		return true;
	}

	public bool IsTimeActivity()
	{
		if (ActivityType == GameDefine.ACTIVITY_TYPE.ESCORT || ActivityType == GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT || ActivityType == GameDefine.ACTIVITY_TYPE.CITY_DANCE || ActivityType == GameDefine.ACTIVITY_TYPE.BAR_FIGHT || ActivityType == GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE || ActivityType == GameDefine.ACTIVITY_TYPE.WILD_BOSS || ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BATTLE || ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BOSS)
		{
			return true;
		}
		return false;
	}

	public bool IsInTimeActivityUI()
	{
		if (ActivityType == GameDefine.ACTIVITY_TYPE.ESCORT || ActivityType == GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT || ActivityType == GameDefine.ACTIVITY_TYPE.CITY_DANCE || ActivityType == GameDefine.ACTIVITY_TYPE.BAR_FIGHT || ActivityType == GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE || ActivityType == GameDefine.ACTIVITY_TYPE.WILD_BOSS)
		{
			return true;
		}
		return false;
	}

	public activity_info GetActivityInfo()
	{
		if (ActivityType == GameDefine.ACTIVITY_TYPE.ESCORT || ActivityType == GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT || ActivityType == GameDefine.ACTIVITY_TYPE.CITY_DANCE || ActivityType == GameDefine.ACTIVITY_TYPE.BAR_FIGHT || ActivityType == GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			return playerData.ActivityData.GetActivityInfoByType(Type);
		}
		return null;
	}

	public guild_battle_info GetGuildBattleInfo()
	{
		if (ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BATTLE)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			return playerData.ActivityData.GuildBattleInfo;
		}
		return null;
	}

	public guild_boss GetGuildBossInfo()
	{
		if (ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BOSS)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			return playerData.ActivityData.GetGuildBoss();
		}
		return null;
	}

	public activity_info GetWildBossInfo()
	{
		if (ActivityType == GameDefine.ACTIVITY_TYPE.WILD_BOSS)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			return playerData.ActivityData.GetWildBossInfo();
		}
		return null;
	}

	public bool CheckCanGoTo()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
		{
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.DAILY_COPY)
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(ActivityID);
			if (!IsUnlock)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", copySceneDataById.MinLevel));
				return false;
			}
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT || ActivityType == GameDefine.ACTIVITY_TYPE.ESCORT)
		{
			EscortData escortDataById = DataManager.GetEscortDataById(ActivityID);
			if (!IsUnlock)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", escortDataById.UnlockLevel));
				return false;
			}
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.BAR_FIGHT)
		{
			BarFightCopyData barFightCopyDataByID = DataManager.GetBarFightCopyDataByID(ActivityID);
			if (!IsUnlock)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", barFightCopyDataByID.UnlockLevel));
				return false;
			}
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.CITY_DANCE)
		{
			CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(ActivityID);
			if (!IsUnlock)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", cityDanceDataById.UnlockLevel));
				return false;
			}
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.SEX_MINI)
		{
			SexMiniData sexMiniDataById = DataManager.GetSexMiniDataById(ActivityID);
			if (!IsUnlock)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", sexMiniDataById.UnlockLevel));
				return false;
			}
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE)
		{
			SurviveBattleData surviveBattleDataById = DataManager.GetSurviveBattleDataById(ActivityID);
			if (!IsUnlock)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", surviveBattleDataById.UnlockLevel));
				return false;
			}
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BATTLE)
		{
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BOSS)
		{
			GuildBossData guildBossDataByID = DataManager.GetGuildBossDataByID(ActivityID);
			if (!IsUnlock)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{102006}"));
				return false;
			}
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.WILD_BOSS)
		{
			WildBossData wildBossDataByID = DataManager.GetWildBossDataByID(ActivityID);
			if (!IsUnlock)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", wildBossDataByID.LevelMin));
				return false;
			}
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.TOWER)
		{
			int condition = DataManager.GetFunctionDataById(4003.ToString()).Condition;
			if (!IsUnlock)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", condition));
				return false;
			}
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.RANKPVP)
		{
			int condition2 = DataManager.GetFunctionDataById(3002.ToString()).Condition;
			if (!IsUnlock)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", condition2));
				return false;
			}
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.DOMIN)
		{
			if (!IsUnlock)
			{
				NoticeLogic.AddNotifyData("#{103009}");
				return false;
			}
			return true;
		}
		if (ActivityType == GameDefine.ACTIVITY_TYPE.SHOP_GATE)
		{
			if (!IsUnlock)
			{
				return false;
			}
			return true;
		}
		return false;
	}
}
