using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ActivityData
{
	private Dictionary<string, activity_info> mCurActivityDataDic = new Dictionary<string, activity_info>();

	private Dictionary<string, activity_info> mCurWildBossDataDic = new Dictionary<string, activity_info>();

	private Dictionary<string, guild_boss> mCurGuildBossDataDic = new Dictionary<string, guild_boss>();

	private List<activity_info> mActivityInfoList = new List<activity_info>();

	private List<activity_info> mWildBossInfoList = new List<activity_info>();

	private List<guild_boss> mGuildBossInfoList = new List<guild_boss>();

	private guild_battle_info mguild_battle_info;

	public dance_state_info mguild_dance_info;

	private List<guild_member_info> mguild_member_info = new List<guild_member_info>();

	private int GuildLevel;

	private Dictionary<long, dance_state_info> mCurDanceStateDic = new Dictionary<long, dance_state_info>();

	private int mCurDanceState;

	private int mCurDanceOpen;

	private int mFirstGuildDanceOpen;

	private int mGuildDanceOpen;

	private int mGuildDonmineOpen;

	private List<string> mMissionTimeOutList = new List<string>();

	private Dictionary<string, guild_map_info> mCurGuildCityDataDic = new Dictionary<string, guild_map_info>();

	public Dictionary<string, activity_info> CurActivityDataDic => mCurActivityDataDic;

	public Dictionary<string, activity_info> CurWildBossDataDic => mCurWildBossDataDic;

	public Dictionary<string, guild_boss> CurGuildBossDataDic => mCurGuildBossDataDic;

	public List<activity_info> ActivityInfoList => mActivityInfoList;

	public List<activity_info> WildBossInfoList => mWildBossInfoList;

	public List<guild_boss> GuildBossInfoList => mGuildBossInfoList;

	public guild_battle_info GuildBattleInfo => mguild_battle_info;

	public dance_state_info GuildDanceInfo => mguild_dance_info;

	public List<guild_member_info> guild_member_info => mguild_member_info;

	public Dictionary<long, dance_state_info> CurDanceStateDic => mCurDanceStateDic;

	public int CurDanceState => mCurDanceState;

	public int CurDanceOpen => mCurDanceOpen;

	public int FirstGuildDanceOpen => mFirstGuildDanceOpen;

	public int GuildDanceOpen => mGuildDanceOpen;

	public int GuildDonmineOpen => mGuildDonmineOpen;

	public List<string> MissionTimeOutList => mMissionTimeOutList;

	public Dictionary<string, guild_map_info> CurGuildCityDataDic => mCurGuildCityDataDic;

	public void Reset()
	{
		GuildLevel = 0;
		if (mCurActivityDataDic != null)
		{
			mCurActivityDataDic.Clear();
		}
		if (mCurWildBossDataDic != null)
		{
			mCurWildBossDataDic.Clear();
		}
		if (mCurGuildBossDataDic != null)
		{
			mCurGuildBossDataDic.Clear();
		}
		if (mCurDanceStateDic != null)
		{
			mCurDanceStateDic.Clear();
		}
		if (mCurGuildCityDataDic != null)
		{
			mCurGuildCityDataDic.Clear();
		}
		mActivityInfoList.Clear();
		mWildBossInfoList.Clear();
		GuildBossInfoList.Clear();
		mguild_battle_info = null;
		mguild_dance_info = null;
		mguild_member_info.Clear();
		mCurDanceState = 0;
		mCurDanceOpen = 0;
		mFirstGuildDanceOpen = 0;
		mGuildDanceOpen = 0;
		mMissionTimeOutList.Clear();
	}

	public void SyncGuildCityInfo(ret_request_guild_map_info.request request)
	{
		if (mCurGuildCityDataDic != null)
		{
			mCurGuildCityDataDic.Clear();
		}
		if (request.HasGuild_map_info)
		{
			mCurGuildCityDataDic = request.guild_map_info;
		}
		UpdateTips();
	}

	public void SyncGuildCityRewardInfo(ret_guild_map_reward.request request)
	{
		if (mCurGuildCityDataDic != null && request.HasId && mCurGuildCityDataDic.ContainsKey(request.id))
		{
			mCurGuildCityDataDic[request.id].requireState = request.state;
		}
	}

	public bool IsOpenCityCapture(string mapid)
	{
		if (CurGuildCityDataDic == null)
		{
			return false;
		}
		foreach (guild_map_info value in CurGuildCityDataDic.Values)
		{
			GuildCaptureData guildCaptureDataByID = DataManager.GetGuildCaptureDataByID(value.id);
			if (guildCaptureDataByID.MapID.Equals(mapid) && value.state == 1)
			{
				return true;
			}
		}
		return false;
	}

	public Vector3 GetCityCaptureNpcPos(MapInfoData mapinfo)
	{
		if (CurGuildCityDataDic == null)
		{
			return mapinfo.BirthPosVector3;
		}
		foreach (guild_map_info value in CurGuildCityDataDic.Values)
		{
			GuildCaptureData guildCaptureDataByID = DataManager.GetGuildCaptureDataByID(value.id);
			if (guildCaptureDataByID.MapID.Equals(mapinfo.ID) && value.state == 1)
			{
				return guildCaptureDataByID.GetNpcPos();
			}
		}
		return mapinfo.BirthPosVector3;
	}

	public guild_map_info GetGuildMapInfo(string mapid)
	{
		if (CurGuildCityDataDic == null)
		{
			return null;
		}
		foreach (guild_map_info value in CurGuildCityDataDic.Values)
		{
			GuildCaptureData guildCaptureDataByID = DataManager.GetGuildCaptureDataByID(value.id);
			if (guildCaptureDataByID.MapID.Equals(mapid))
			{
				return value;
			}
		}
		return null;
	}

	public void SetCityCaptureFinish(string id)
	{
		if (CurGuildCityDataDic != null && CurGuildCityDataDic.ContainsKey(id) && CurGuildCityDataDic[id].state == 1)
		{
			CurGuildCityDataDic[id].state = 2L;
		}
		UpdateCityShow();
	}

	public void SetAllCityCaptureOpenState(bool isOpen)
	{
		if (CurGuildCityDataDic != null)
		{
			foreach (guild_map_info value in CurGuildCityDataDic.Values)
			{
				if (isOpen)
				{
					CurGuildCityDataDic[value.id].state = 1L;
				}
				else
				{
					CurGuildCityDataDic[value.id].state = 0L;
				}
			}
		}
		UpdateCityShow();
	}

	public dance_state_info GetDanceInfoByType(GameDefine.DANCE_TYPE dancetype)
	{
		List<dance_state_info> list = new List<dance_state_info>(mCurDanceStateDic.Values);
		if (list != null && list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(list[i].ID);
				if (cityDanceDataById.Type == (int)dancetype)
				{
					return list[i];
				}
			}
		}
		return null;
	}

	public void SyncDanceStateInfo(sync_dance_state_info.request request)
	{
		if (request.HasState)
		{
			mCurDanceState = (int)request.state;
		}
		if (request.HasOpen)
		{
			mCurDanceOpen = (int)request.open;
		}
		if (request.HasDance_state_info)
		{
			mCurDanceStateDic = request.dance_state_info;
		}
	}

	public bool IsCanDance()
	{
		List<dance_state_info> list = new List<dance_state_info>(mCurDanceStateDic.Values);
		if (list != null && list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(list[i].ID);
				if (cityDanceDataById.Type != 0)
				{
					return true;
				}
				if (cityDanceDataById.Type == 0 && list[i].HasDuration && list[i].duration > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void UpdateActivity(chat_item listdata)
	{
		List<long> intdata = listdata.intdata;
		List<string> strdata = new List<string>();
		if (listdata.HasStringdata)
		{
			strdata = listdata.stringdata;
		}
		if (intdata.Count >= 2)
		{
			ChangeActivity((int)intdata[0], (int)intdata[1], strdata);
			UpdateTips();
		}
	}

	public void ChangeActivity(int first, int secd, List<string> strdata)
	{
		switch (first)
		{
		case 1:
		case 2:
		{
			for (int n = 0; n < mActivityInfoList.Count; n++)
			{
				if (mActivityInfoList[n].Type == 1 || mActivityInfoList[n].Type == 2)
				{
					mActivityInfoList[n].State = secd;
				}
			}
			break;
		}
		case 4:
		{
			for (int k = 0; k < mActivityInfoList.Count; k++)
			{
				if (mActivityInfoList[k].Type == first)
				{
					mActivityInfoList[k].State = secd;
				}
			}
			break;
		}
		case 7:
		{
			for (int l = 0; l < mActivityInfoList.Count; l++)
			{
				if (mActivityInfoList[l].Type == first)
				{
					mActivityInfoList[l].State = secd;
				}
			}
			break;
		}
		case 5:
		{
			for (int j = 0; j < mWildBossInfoList.Count; j++)
			{
				mWildBossInfoList[j].State = secd;
			}
			break;
		}
		case 6:
		{
			for (int m = 0; m < mGuildBossInfoList.Count; m++)
			{
				mGuildBossInfoList[m].state = secd;
			}
			break;
		}
		case 9:
			if (mguild_battle_info != null)
			{
				mguild_battle_info.state = secd;
			}
			break;
		case 15:
			mFirstGuildDanceOpen = secd;
			break;
		case 16:
			mGuildDanceOpen = secd;
			break;
		case 17:
		{
			string text = strdata[0];
			for (int i = 0; i < mMissionTimeOutList.Count; i++)
			{
				if (mMissionTimeOutList[i].Equals(text))
				{
					return;
				}
			}
			mMissionTimeOutList.Add(text);
			break;
		}
		case 18:
			mGuildDonmineOpen = secd;
			SetAllCityCaptureOpenState(secd == 1);
			break;
		case 19:
			SetCityCaptureFinish(strdata[1]);
			break;
		case 3:
		case 8:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
			break;
		}
	}

	public bool IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE activitytype)
	{
		switch (activitytype)
		{
		case GameDefine.ACTIVITY_TYPE.GUILD_BOSS:
			return IsHaveGuildBossTips();
		case GameDefine.ACTIVITY_TYPE.WILD_BOSS:
			return IsHaveWildBoss();
		case GameDefine.ACTIVITY_TYPE.GUILD_BATTLE:
			return IsHaveGuildBattleTips();
		case GameDefine.ACTIVITY_TYPE.GUILD_DANCE:
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				return false;
			}
			return GuildDanceOpen == 1;
		case GameDefine.ACTIVITY_TYPE.GUILD_DONMINE:
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				return false;
			}
			return GuildDonmineOpen == 1;
		default:
			return CheckTipsByType(activitytype);
		}
	}

	public bool IsHaveMissionTimeOut()
	{
		if (MissionTimeOutList != null && MissionTimeOutList.Count > 0)
		{
			return true;
		}
		return false;
	}

	public void DecMissionLine(string missionid)
	{
		if (MissionTimeOutList == null || MissionTimeOutList.Count <= 0)
		{
			return;
		}
		for (int num = MissionTimeOutList.Count - 1; num >= 0; num--)
		{
			if (MissionTimeOutList[num].Equals(missionid))
			{
				MissionTimeOutList.RemoveAt(num);
			}
		}
	}

	public bool CheckTipsByType(GameDefine.ACTIVITY_TYPE acttype)
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_TIME))
		{
			return false;
		}
		if (mActivityInfoList.Count > 0)
		{
			for (int i = 0; i < mActivityInfoList.Count; i++)
			{
				if (mActivityInfoList[i].Type != (long)acttype)
				{
					continue;
				}
				activity_info activity_info = mActivityInfoList[i];
				if (activity_info.Type == 1 || activity_info.Type == 2)
				{
					EscortData escortDataById = DataManager.GetEscortDataById(activity_info.ID);
					if (mActivityInfoList[i].State == 1 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(escortDataById.UnlockLevel) && activity_info.CurNum > 0)
					{
						return true;
					}
				}
				else if (activity_info.Type == 3)
				{
					CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(activity_info.ID);
					if (mActivityInfoList[i].State == 1 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(cityDanceDataById.UnlockLevel))
					{
						return true;
					}
				}
				else if (activity_info.Type == 4)
				{
					BarFightCopyData barFightCopyDataByID = DataManager.GetBarFightCopyDataByID(activity_info.ID);
					if (mActivityInfoList[i].State == 2 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(barFightCopyDataByID.UnlockLevel))
					{
						return true;
					}
				}
				else if (activity_info.Type == 7)
				{
					SurviveBattleData surviveBattleDataById = DataManager.GetSurviveBattleDataById(activity_info.ID);
					if (mActivityInfoList[i].State == 1 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(surviveBattleDataById.UnlockLevel))
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	public void UpdateTips()
	{
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateMessageTips();
			SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTips(IsHaveGuildTips(), GameDefine.TIPS_TYPE.GUILD);
			SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateCityDamageFlag();
		}
		if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
		}
		if (SingletonUnity<ActivityTipsRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ActivityTipsRootLogic>.Instance.gameObject))
		{
			SingletonUnity<ActivityTipsRootLogic>.Instance.UpdateInfo();
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager != null && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.MapActivityManager != null)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.MapActivityManager.UpdateTimeActivityMapFlag();
		}
	}

	public void UpdateCityShow()
	{
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateCityDamageFlag();
		}
	}

	public void SyncGuildBattleMember(ret_guild_battle_member.request request)
	{
		if (request.HasGuild_member_info)
		{
			mguild_member_info = request.guild_member_info;
		}
		UpdateTips();
	}

	public void SyncGuildBattleInfo(ret_guild_battle_state.request request)
	{
		if (request.HasBattle_info)
		{
			mguild_battle_info = request.battle_info;
		}
		UpdateTips();
	}

	public void SyncGuildBattleInfo(ret_guild_battle_info.request request)
	{
		if (request.HasBattle_info)
		{
			mguild_battle_info = request.battle_info;
		}
		UpdateTips();
	}

	public activity_info GetActivityInfoByType(int type)
	{
		List<activity_info> list = new List<activity_info>(mCurActivityDataDic.Values);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Type == type)
			{
				return list[i];
			}
		}
		return null;
	}

	public void SyncActivityInfoData(ret_request_activity_info.request request)
	{
		mCurActivityDataDic = new Dictionary<string, activity_info>(request.activity_info);
		List<activity_info> list = new List<activity_info>(mCurActivityDataDic.Values);
		mActivityInfoList.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Type != 5 && list[i].Type != 6)
			{
				mActivityInfoList.Add(list[i]);
			}
		}
		UpdateTips();
	}

	public activity_info GetWildBossInfo()
	{
		for (int i = 0; i < mWildBossInfoList.Count; i++)
		{
			WildBossData wildBossDataByID = DataManager.GetWildBossDataByID(mWildBossInfoList[i].ID);
			if (CheckLevel(wildBossDataByID.LevelMin, wildBossDataByID.LevelMax))
			{
				return mWildBossInfoList[i];
			}
		}
		return null;
	}

	public void SyncWildBossInfoData(ret_request_wild_boss_info.request request)
	{
		mCurWildBossDataDic = new Dictionary<string, activity_info>(request.activity_info);
		List<activity_info> list = new List<activity_info>(mCurWildBossDataDic.Values);
		mWildBossInfoList.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Type != 5)
			{
				continue;
			}
			WildBossData wildBossDataByID = DataManager.GetWildBossDataByID(list[i].ID);
			if (wildBossDataByID.PVP == 1)
			{
				if (wildBossDataByID.Time == list[i].next)
				{
					mWildBossInfoList.Add(list[i]);
				}
			}
			else if (wildBossDataByID.Time != list[i].next)
			{
				mWildBossInfoList.Add(list[i]);
			}
		}
		UpdateTips();
	}

	public guild_boss GetGuildBoss()
	{
		for (int i = 0; i < mGuildBossInfoList.Count; i++)
		{
			GuildBossData guildBossDataByID = DataManager.GetGuildBossDataByID(mGuildBossInfoList[i].id);
			if (mGuildBossInfoList[i].state == 1 && GuildLevel >= guildBossDataByID.LevelMin)
			{
				return mGuildBossInfoList[i];
			}
		}
		if (mGuildBossInfoList.Count > 0)
		{
			return mGuildBossInfoList[0];
		}
		return null;
	}

	public void SyncGuildBossInfoData(ret_request_guild_boss.request request)
	{
		if (request.HasGuild_boss)
		{
			mCurGuildBossDataDic = new Dictionary<string, guild_boss>(request.guild_boss);
			mGuildBossInfoList = new List<guild_boss>(request.guild_boss.Values);
		}
		if (request.HasLevel)
		{
			GuildLevel = (int)request.level;
		}
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			GuildLevel = 0;
		}
		if (request.HasGuild_battle_info)
		{
			mguild_battle_info = request.guild_battle_info;
		}
		if (request.HasDance_state_info)
		{
			mguild_dance_info = request.dance_state_info;
		}
		if (request.HasGuild_map_info)
		{
			mCurGuildCityDataDic = request.guild_map_info;
		}
		UpdateTips();
	}

	public bool IsHaveGuildTips()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		return IsHaveGuildActTips() || playerData.PlayerGuild.IsHaveNewApply();
	}

	public bool IsHaveGuildActTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GUILD_ACTIVITY))
		{
			return false;
		}
		return IsHaveGuildBossTips() || IsHaveGuildBattleTips() || IsHaveGuildDanceTips() || IsHaveGuildCityTips();
	}

	public bool IsHaveGuildCityTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			return false;
		}
		if (mGuildDonmineOpen != 1)
		{
			return false;
		}
		if (CurGuildCityDataDic == null)
		{
			return false;
		}
		foreach (guild_map_info value in CurGuildCityDataDic.Values)
		{
			if (value.state == 1)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsHaveGuildDanceTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			return false;
		}
		if (GuildDanceInfo != null && GuildDanceInfo.HasState && GuildDanceInfo.state == 1)
		{
			return true;
		}
		return false;
	}

	public bool IsHaveGuildBattleTips()
	{
		if (mguild_battle_info == null || mguild_member_info.Count == 0)
		{
			return false;
		}
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			return false;
		}
		bool flag = false;
		for (int i = 0; i < mguild_member_info.Count; i++)
		{
			if (mguild_member_info[i].characterId == PlayerData.MainPlayerServerId)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (GuildBattleInfo.state == 1 || GuildBattleInfo.state == 3 || GuildBattleInfo.state == 5)
		{
			return true;
		}
		return false;
	}

	public bool IsHaveGuildBossTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GUILD_ACTIVITY))
		{
			return false;
		}
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			return false;
		}
		if (mGuildBossInfoList.Count > 0)
		{
			for (int i = 0; i < mGuildBossInfoList.Count; i++)
			{
				GuildBossData guildBossDataByID = DataManager.GetGuildBossDataByID(mGuildBossInfoList[i].id);
				if (mGuildBossInfoList[i].state == 1 && GuildLevel >= guildBossDataByID.LevelMin)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsHaveActTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_TIME))
		{
			return false;
		}
		return IsHaveActivity() || IsHaveWildBoss() || IsHaveGuildActTips();
	}

	public bool IsHaveActivity()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_TIME))
		{
			return false;
		}
		if (mActivityInfoList.Count > 0)
		{
			for (int i = 0; i < mActivityInfoList.Count; i++)
			{
				activity_info activity_info = mActivityInfoList[i];
				if (activity_info.Type == 1 || activity_info.Type == 2)
				{
					if (activity_info.CurNum > 0 && mActivityInfoList[i].State == 1)
					{
						return true;
					}
				}
				else if (mActivityInfoList[i].State == 1)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public bool IsHaveWildBoss()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_WORLDBOSS))
		{
			return false;
		}
		if (mWildBossInfoList.Count > 0)
		{
			for (int i = 0; i < mWildBossInfoList.Count; i++)
			{
				WildBossData wildBossDataByID = DataManager.GetWildBossDataByID(mWildBossInfoList[i].ID);
				if (wildBossDataByID != null && CheckLevel(wildBossDataByID.LevelMin, wildBossDataByID.LevelMax) && mWildBossInfoList[i].State == 1 && (mWildBossInfoList[i].CurNum > 0 || SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime() <= mWildBossInfoList[i].time + 5400))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public bool CheckLevel(int minLevel, int maxlevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minLevel, maxlevel);
	}

	public string GetCityDanceInfo()
	{
		string result = string.Empty;
		for (int i = 0; i < mActivityInfoList.Count; i++)
		{
			if (mActivityInfoList[i].Type == 3)
			{
				result = mActivityInfoList[i].ID;
				break;
			}
		}
		return result;
	}

	public int GetActivityUnlockLevel(activity_info info)
	{
		int result = 0;
		long type = info.Type;
		if (type >= 1 && type <= 7)
		{
			switch (type - 1)
			{
			case 0L:
			{
				EscortData escortDataById2 = DataManager.GetEscortDataById(info.ID);
				result = escortDataById2.UnlockLevel;
				break;
			}
			case 1L:
			{
				EscortData escortDataById = DataManager.GetEscortDataById(info.ID);
				result = escortDataById.UnlockLevel;
				break;
			}
			case 2L:
			{
				CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(info.ID);
				result = cityDanceDataById.UnlockLevel;
				break;
			}
			case 3L:
			{
				BarFightCopyData barFightCopyDataByID = DataManager.GetBarFightCopyDataByID(info.ID);
				result = barFightCopyDataByID.UnlockLevel;
				break;
			}
			case 6L:
			{
				SurviveBattleData surviveBattleDataById = DataManager.GetSurviveBattleDataById(info.ID);
				result = surviveBattleDataById.UnlockLevel;
				break;
			}
			}
		}
		return result;
	}
}
