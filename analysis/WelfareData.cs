using System.Collections.Generic;
using SprotoType;

public class WelfareData
{
	private Dictionary<string, daily_buy> dailyBuy = new Dictionary<string, daily_buy>();

	private Dictionary<string, invest_pack> investPack = new Dictionary<string, invest_pack>();

	private Dictionary<string, level_pack> levelPack = new Dictionary<string, level_pack>();

	private Dictionary<string, daily_active> daily_actives = new Dictionary<string, daily_active>();

	private Dictionary<string, daily_reward> dailyRewards = new Dictionary<string, daily_reward>();

	private Dictionary<string, retrieve_info> Retrieve_Dic = new Dictionary<string, retrieve_info>();

	private Dictionary<string, special_big_pack> BigPackDic = new Dictionary<string, special_big_pack>();

	private Dictionary<string, level_reward> levelrewardDic = new Dictionary<string, level_reward>();

	private vip VipInfo;

	private bool SignMonthFlag;

	private bool SignWeekFlag;

	private int curWeekSignDay = -1;

	private bool curSignState;

	private bool completeState;

	public Dictionary<string, level_pack> LevelPackDic => levelPack;

	public Dictionary<string, daily_active> DailyActives => daily_actives;

	public void Reset()
	{
		curWeekSignDay = -1;
		SignMonthFlag = false;
		SignWeekFlag = false;
		curSignState = false;
		completeState = false;
		dailyBuy.Clear();
		investPack.Clear();
		levelPack.Clear();
		daily_actives.Clear();
		dailyRewards.Clear();
		Retrieve_Dic.Clear();
		BigPackDic.Clear();
		levelrewardDic.Clear();
		VipInfo = null;
	}

	public void SyncVipInfo(ret_require_vip_info.request request)
	{
		if (request.HasVip)
		{
			VipInfo = request.vip;
		}
		UpdateTips();
	}

	public void SyncVipInfo(ret_require_vip_reward.request request)
	{
		if (request.HasVip)
		{
			VipInfo = request.vip;
		}
		UpdateTips();
	}

	public void UpdateVipState(string id)
	{
		if (VipInfo != null && VipInfo.id.Equals(id))
		{
			VipInfo.state = 1L;
		}
		UpdateTips();
	}

	public bool HaveVipTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP_VIP))
		{
			return false;
		}
		if (VipInfo != null && VipInfo.state == 0L)
		{
			return true;
		}
		return false;
	}

	public void SyncLevelReward(ret_level_reward.request request)
	{
		if (request.HasLevel_reward)
		{
			levelrewardDic = request.level_reward;
		}
		UpdateTips();
	}

	public void SyncLevelReward(get_level_reward.request request)
	{
		if (request.HasLevel_reward)
		{
			levelrewardDic = request.level_reward;
		}
		UpdateTips();
	}

	public bool IsHaveLevelReward()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_LEVEL))
		{
			return false;
		}
		LevelRewardData levelRewardData = null;
		level_reward level_reward = null;
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		List<LevelRewardData> levelRewardDataList = DataManager.GetLevelRewardDataList();
		levelRewardDataList.Sort((LevelRewardData x, LevelRewardData y) => (x.ID.Length == y.ID.Length) ? x.ID.CompareTo(y.ID) : (x.ID.Length - y.ID.Length));
		for (int i = 0; i < levelRewardDataList.Count; i++)
		{
			if (level >= levelRewardDataList[i].StartLv && level <= levelRewardDataList[i].EndLv)
			{
				levelRewardData = levelRewardDataList[i];
				if (levelrewardDic.ContainsKey(levelRewardData.ID))
				{
					level_reward = levelrewardDic[levelRewardData.ID];
				}
			}
		}
		if (levelRewardData != null && level_reward != null)
		{
			if (level >= levelRewardData.TargetLevel1 && (level_reward.state & 1) == 0L)
			{
				return true;
			}
			if (level >= levelRewardData.TargetLevel2 && (level_reward.state & 2) == 0L)
			{
				return true;
			}
			if (level >= levelRewardData.TargetLevel3 && (level_reward.state & 4) == 0L)
			{
				return true;
			}
			if (level >= levelRewardData.TargetLevel && (level_reward.state & 8) == 0L && (level_reward.state & 1) != 0L && (level_reward.state & 2) != 0L && (level_reward.state & 4) != 0L)
			{
				return true;
			}
		}
		return false;
	}

	public void SetBigPack(ret_request_big_pack.request request)
	{
		BigPackDic = request.special_big_packs;
	}

	public void UpdateBigPack(string id)
	{
		if (BigPackDic.ContainsKey(id))
		{
			BigPackDic[id].state = 1L;
		}
	}

	public bool isHaveWeaponPack()
	{
		if (BigPackDic.ContainsKey("4") && BigPackDic["4"].state == 0L)
		{
			return true;
		}
		return false;
	}

	public void SetRetrieve(ret_request_retrieve_info.request request)
	{
		Retrieve_Dic = request.info;
		UpdateTips();
	}

	public bool HaveRetrieveTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_RETRIEVE))
		{
			return false;
		}
		foreach (retrieve_info value in Retrieve_Dic.Values)
		{
			RetrieveData retrieveDataBuyId = DataManager.GetRetrieveDataBuyId(value.ID);
			if (retrieveDataBuyId == null)
			{
				continue;
			}
			if (!retrieveDataBuyId.isGuildDance)
			{
				if (value.state == 0L)
				{
					return true;
				}
			}
			else if (value.state == 0L && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				return true;
			}
		}
		return false;
	}

	public void SetWeekFlag(ret_request_sign_week_info.request request)
	{
		SignWeekFlag = !request.complete && request.cur_sign_state;
		curWeekSignDay = (int)request.cur_sign;
		curSignState = request.cur_sign_state;
		completeState = request.complete;
		UpdateTips();
	}

	public void SetWeekFlag(ret_sign_week.request request)
	{
		SignWeekFlag = request.cur_sign_state;
		curWeekSignDay = (int)request.cur_sign;
		curSignState = request.cur_sign_state;
		if (curWeekSignDay == 7 && !curSignState)
		{
			completeState = true;
		}
		UpdateTips();
	}

	public int GetNextWeekRewardDay()
	{
		if (completeState)
		{
			return -1;
		}
		if (curWeekSignDay == 7 && !curSignState)
		{
			return -1;
		}
		if (curSignState)
		{
			return curWeekSignDay;
		}
		return curWeekSignDay + 1;
	}

	public bool HaveWeekTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_7DAY))
		{
			return false;
		}
		return SignWeekFlag;
	}

	public void SetMonthFlag(ret_request_30_day_info.request request)
	{
		SignMonthFlag = request.cur_sign_state || request.replenish_sign_state;
		UpdateTips();
	}

	public void SetMonthFlag(ret_sign_30_day.request request)
	{
		SignMonthFlag = request.cur_sign_state || request.replenish_sign_state;
		UpdateTips();
	}

	public bool HaveMonthTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_CHECK))
		{
			return false;
		}
		return SignMonthFlag;
	}

	public void InitDailyBuy(ret_request_daily_buy.request info)
	{
		dailyBuy = info.daily_buys;
		UpdateTips();
	}

	public void UpdateDailyBuy(string key)
	{
		if (dailyBuy.ContainsKey(key))
		{
			dailyBuy[key].state = 2L;
		}
		UpdateTips();
	}

	public bool HaveDailyBuyTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_DAILY))
		{
			return false;
		}
		if (!LocalDataSaveManager.ShowDailyBuyTips)
		{
			return false;
		}
		foreach (daily_buy value in dailyBuy.Values)
		{
			if (value.state == 0L)
			{
				return true;
			}
		}
		return false;
	}

	public void InitInvestPack(ret_request_invest_pack.request info)
	{
		investPack = info.invest_pack;
		UpdateTips();
	}

	public void InitInvestPack(ret_buy_invest_pack.request info)
	{
		investPack = info.invest_pack;
		UpdateTips();
	}

	public void UpdateInvestPack(string key)
	{
		if (investPack.ContainsKey(key))
		{
			investPack[key].state = 2L;
		}
		UpdateTips();
	}

	public bool HaveInvestTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_INVEST))
		{
			return false;
		}
		foreach (invest_pack value in investPack.Values)
		{
			if (value.state == -1)
			{
				return LocalDataSaveManager.ShowInvestTips;
			}
			if (value.state == 1)
			{
				return true;
			}
		}
		return false;
	}

	public void InitLevelPack(ret_request_level_pack.request info)
	{
		levelPack = info.level_pack;
		UpdateTips();
	}

	public void UpdateLevelPack(string key)
	{
		if (levelPack.ContainsKey(key))
		{
			levelPack[key].state = 2L;
		}
		UpdateTips();
	}

	public bool HaveLevelTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_LEVEL))
		{
			return false;
		}
		foreach (level_pack value in levelPack.Values)
		{
			if (value.state == 1)
			{
				return true;
			}
		}
		return false;
	}

	public List<LevelPackageData> GetUnGetLevelPack()
	{
		if (levelPack.Count > 0)
		{
			int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
			List<level_pack> list = new List<level_pack>(levelPack.Values);
			List<LevelPackageData> list2 = new List<LevelPackageData>();
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(DataManager.GetLevelPackageDataBuyId(list[i].ID));
			}
			list2.Sort((LevelPackageData x, LevelPackageData y) => x.LvTarget - y.LvTarget);
			for (int num = list2.Count - 1; num >= 0; num--)
			{
				if (list2[num].LvTarget <= level)
				{
					if (levelPack[list2[num].ID].state == 2)
					{
						list2.RemoveAt(num);
					}
					else
					{
						levelPack[list2[num].ID].state = 1L;
					}
				}
				else
				{
					list2.RemoveAt(num);
				}
			}
			return list2;
		}
		return null;
	}

	public void InitDailyRewards(ret_request_daily_active.request info)
	{
		if (info.HasDaily_actives)
		{
			daily_actives = info.daily_actives;
		}
		if (info.HasDaily_rewards)
		{
			dailyRewards = info.daily_rewards;
		}
		UpdateTips();
	}

	public void UpdateDailyReward(string key)
	{
		if (dailyRewards.ContainsKey(key))
		{
			dailyRewards[key].state = 2L;
		}
		UpdateTips();
	}

	public bool HaveDailyActivityTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_ACTIVITY))
		{
			return false;
		}
		foreach (daily_reward value in dailyRewards.Values)
		{
			if (value.state == 1)
			{
				return true;
			}
		}
		return false;
	}

	public bool isHaveWelfareTips()
	{
		return HaveWeekTips() || HaveDailyBuyTips() || HaveInvestTips() || IsHaveLevelReward() || HaveMonthTips() || HaveRetrieveTips();
	}

	public void UpdateTips()
	{
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTips(isHaveWelfareTips(), GameDefine.TIPS_TYPE.WELFARE);
			SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTips(HaveDailyActivityTips(), GameDefine.TIPS_TYPE.DAILYACT);
			SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTips(HaveVipTips(), GameDefine.TIPS_TYPE.SHOP);
		}
		if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
		}
	}

	public bool IsGetSignWeekCar()
	{
		if (curWeekSignDay > 3)
		{
			return true;
		}
		if (curWeekSignDay == 3 && !curSignState)
		{
			return true;
		}
		return false;
	}
}
