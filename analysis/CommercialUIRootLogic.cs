using System.Collections.Generic;

public class CommercialUIRootLogic : SingletonUnity<CommercialUIRootLogic>
{
	private int curPageIndex = -1;

	private PlayerCommonData commdata;

	private void OnEnable()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			ActivityData activityData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData;
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			WelfareData welfareData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData;
			commdata = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickMonthBtn, isIcon: true, "CZ_left_yueQian", StrDictionary.GetDictionaryString("#{300101}"), FUNCTION_TYPE.GIFT_CHECK, welfareData.HaveMonthTips);
			MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickDailyBuyBtn, isIcon: true, "CZ_left_1yuan", StrDictionary.GetDictionaryString("#{300102}"), FUNCTION_TYPE.GIFT_DAILY, welfareData.HaveDailyBuyTips);
			MenuTabBtnInfo item3 = new MenuTabBtnInfo(OnClickInvestBtn, isIcon: true, "CZ_left_TouZi", StrDictionary.GetDictionaryString("#{300103}"), FUNCTION_TYPE.GIFT_INVEST, welfareData.HaveInvestTips);
			MenuTabBtnInfo item4 = new MenuTabBtnInfo(OnClickWeekBtn, isIcon: true, "CZ_left_7Days", StrDictionary.GetDictionaryString("#{300104}"), FUNCTION_TYPE.GIFT_7DAY, welfareData.HaveWeekTips);
			MenuTabBtnInfo item5 = new MenuTabBtnInfo(OnClickLevelBtn, isIcon: true, "CZ_left_DengJiLiBao", StrDictionary.GetDictionaryString("#{300106}"), FUNCTION_TYPE.GIFT_LEVEL, welfareData.IsHaveLevelReward);
			MenuTabBtnInfo item6 = new MenuTabBtnInfo(OnClickRetrieveBtn, isIcon: true, "CZ_left_zhaoHui", StrDictionary.GetDictionaryString("#{301104}"), FUNCTION_TYPE.GIFT_RETRIEVE, welfareData.HaveRetrieveTips);
			if ((commdata.Push & 8) != 0L)
			{
				list.Add(item);
				list.Add(item2);
				list.Add(item3);
				list.Add(item5);
				list.Add(item6);
			}
			else
			{
				list.Add(item);
				list.Add(item2);
				list.Add(item3);
				list.Add(item4);
				list.Add(item5);
				list.Add(item6);
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
			curPageIndex = -1;
		});
	}

	public void Reset()
	{
		if (!SingletonUnity<MenuBaseRootLogic>.Instance.AutoClickTipsTap())
		{
			OnClickMonthBtn();
		}
	}

	public void OnClickMonthBtn()
	{
		if (curPageIndex != GetPageIndex(FUNCTION_TYPE.GIFT_CHECK))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignWeekRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LevelRewardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.InvestRewardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyBuyPackRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RetrieveRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SignMonthRoot, delegate
			{
				SingletonUnity<SignMonthRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(252, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_sign_30_day_info>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.GIFT_CHECK));
			curPageIndex = GetPageIndex(FUNCTION_TYPE.GIFT_CHECK);
		}
	}

	public void OnClickDailyBuyBtn()
	{
		if (curPageIndex == GetPageIndex(FUNCTION_TYPE.GIFT_DAILY))
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignMonthRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignWeekRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LevelRewardRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.InvestRewardRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RetrieveRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DailyBuyPackRoot, delegate
		{
			SingletonUnity<DailyBuyPackRootLogic>.Instance.EnableReset();
			WaitResponseUIRootLogic.OpenWaitBox(258, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.request_daily_buy>();
		});
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.GIFT_DAILY));
		curPageIndex = GetPageIndex(FUNCTION_TYPE.GIFT_DAILY);
		if (LocalDataSaveManager.ShowDailyBuyTips)
		{
			LocalDataSaveManager.ShowDailyBuyTips = false;
			if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
			}
		}
	}

	public void OnClickInvestBtn()
	{
		if (curPageIndex == GetPageIndex(FUNCTION_TYPE.GIFT_INVEST))
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignMonthRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignWeekRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LevelRewardRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyBuyPackRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RetrieveRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.InvestRewardRoot, delegate
		{
			SingletonUnity<InvestRewardRootLogic>.Instance.EnableReset();
			WaitResponseUIRootLogic.OpenWaitBox(257, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.request_invest_pack>();
		});
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.GIFT_INVEST));
		curPageIndex = GetPageIndex(FUNCTION_TYPE.GIFT_INVEST);
		if (LocalDataSaveManager.ShowInvestTips)
		{
			LocalDataSaveManager.ShowInvestTips = false;
			if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
			}
		}
	}

	public void OnClickWeekBtn()
	{
		if (curPageIndex != GetPageIndex(FUNCTION_TYPE.GIFT_7DAY))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignMonthRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LevelRewardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.InvestRewardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyBuyPackRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RetrieveRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SignWeekRoot, delegate
			{
				SingletonUnity<SignWeekRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(253, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_sign_week_info>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.GIFT_7DAY));
			curPageIndex = GetPageIndex(FUNCTION_TYPE.GIFT_7DAY);
		}
	}

	public void OnClickLevelBtn()
	{
		if (curPageIndex != GetPageIndex(FUNCTION_TYPE.GIFT_LEVEL))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignMonthRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignWeekRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.InvestRewardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyBuyPackRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RetrieveRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.LevelRewardRoot, delegate
			{
				SingletonUnity<LevelRewardRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(296, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.req_level_reward>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.GIFT_LEVEL));
			curPageIndex = GetPageIndex(FUNCTION_TYPE.GIFT_LEVEL);
		}
	}

	public void OnClickRetrieveBtn()
	{
		if (curPageIndex != GetPageIndex(FUNCTION_TYPE.GIFT_RETRIEVE))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignMonthRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignWeekRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.InvestRewardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyBuyPackRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LevelRewardRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.RetrieveRoot, delegate
			{
				SingletonUnity<RetrieveRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(278, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_retrieve_info>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.GIFT_RETRIEVE));
			curPageIndex = GetPageIndex(FUNCTION_TYPE.GIFT_RETRIEVE);
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CommercialUIRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignMonthRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignWeekRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LevelRewardRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.InvestRewardRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyBuyPackRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RetrieveRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
		{
			SingletonUnity<AutoPopUIRoot>.Instance.NextPop();
		}
	}

	public int GetPageIndex(FUNCTION_TYPE type)
	{
		if ((commdata.Push & 8) != 0L)
		{
			switch (type)
			{
			case FUNCTION_TYPE.GIFT_CHECK:
				return 0;
			case FUNCTION_TYPE.GIFT_DAILY:
				return 1;
			case FUNCTION_TYPE.GIFT_INVEST:
				return 2;
			case FUNCTION_TYPE.GIFT_LEVEL:
				return 3;
			case FUNCTION_TYPE.GIFT_RETRIEVE:
				return 4;
			}
		}
		else
		{
			switch (type)
			{
			case FUNCTION_TYPE.GIFT_CHECK:
				return 0;
			case FUNCTION_TYPE.GIFT_DAILY:
				return 1;
			case FUNCTION_TYPE.GIFT_INVEST:
				return 2;
			case FUNCTION_TYPE.GIFT_7DAY:
				return 3;
			case FUNCTION_TYPE.GIFT_LEVEL:
				return 4;
			case FUNCTION_TYPE.GIFT_RETRIEVE:
				return 5;
			}
		}
		return 0;
	}
}
