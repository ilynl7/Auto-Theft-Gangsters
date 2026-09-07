using System.Collections.Generic;
using SprotoType;

public class ActivityUIRootLogic : SingletonUnity<ActivityUIRootLogic>
{
	private UI_PAGE_TYPE mPrePage = UI_PAGE_TYPE.INVALID;

	public int CurPageIndex = -1;

	public void CheckPrePage()
	{
		if (mPrePage != UI_PAGE_TYPE.INVALID)
		{
			switch (mPrePage)
			{
			case UI_PAGE_TYPE.BACK_PACK_ITEM:
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
				{
					SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickItemBackPackBtn();
				});
				break;
			case UI_PAGE_TYPE.ENHANCE_EQUIP:
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
				{
					SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance();
				});
				break;
			case UI_PAGE_TYPE.REFINE:
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
				{
					SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipRefine();
				});
				break;
			}
		}
		mPrePage = UI_PAGE_TYPE.INVALID;
	}

	public void SetPrePage(UI_PAGE_TYPE prePage)
	{
		mPrePage = prePage;
	}

	public void InitActivityUI()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickDailyBtn, isIcon: true, "CZ_left_Daily", StrDictionary.GetDictionaryString("#{101535}"), FUNCTION_TYPE.ACTIVITY_DAILY, playerData.CopyInfoData.IsHaveDailyCopyTips);
			MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickTimeBtn, isIcon: true, "CZ_left_Activity", StrDictionary.GetDictionaryString("#{101536}"), FUNCTION_TYPE.ACTIVITY_TIME, playerData.ActivityData.IsHaveActivity);
			MenuTabBtnInfo item3 = new MenuTabBtnInfo(OnClickTowerBtn, isIcon: true, "CZ_left_Challenge", StrDictionary.GetDictionaryString("#{101538}"), FUNCTION_TYPE.ACTIVITY_CHALLENGE, playerData.TowerData.IsHaveTowerTips);
			MenuTabBtnInfo item4 = new MenuTabBtnInfo(OnClickActivityBtn, isIcon: true, "CZ_left_Battle", StrDictionary.GetDictionaryString("#{100114}"), FUNCTION_TYPE.GUILD_ACTIVITY, playerData.ActivityData.IsHaveGuildActTips);
			MenuTabBtnInfo item5 = new MenuTabBtnInfo(OnClickRankBtn, isIcon: true, "CZ_left_PVP", StrDictionary.GetDictionaryString("#{100106}"), FUNCTION_TYPE.RANK_PVP, playerData.RankPVPData.IsHavePVPTips);
			list.Add(item);
			list.Add(item2);
			list.Add(item5);
			list.Add(item3);
			list.Add(item4);
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
			CurPageIndex = -1;
		});
	}

	public void OnClickActivityBtn()
	{
		OnClickActivityBtn(GameDefine.ACTIVITY_TYPE.INVALID);
	}

	public void OnClickActivityBtn(GameDefine.ACTIVITY_TYPE type)
	{
		if (CurPageIndex != 4)
		{
			Singleton<ObjManager>.Instance.MainPlayer.ApplyUpDataGuild();
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActivityUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyCopyUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WildBossUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildActivityRootLogic, delegate
			{
				SingletonUnity<GuildActivityRootLogic>.Instance.EnableReset();
				SingletonUnity<GuildActivityRootLogic>.Instance.TargetTypeId = (int)type;
				WaitResponseUIRootLogic.OpenWaitBox(195, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_guild_boss>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(4);
			CurPageIndex = 4;
		}
	}

	public void Reset()
	{
		if (!SingletonUnity<MenuBaseRootLogic>.Instance.AutoClickTipsTap())
		{
			OnClickDailyBtn();
		}
	}

	public void ResetToTower()
	{
		OnClickTowerBtn();
	}

	public void ResetToPVP()
	{
		OnClickRankBtn();
	}

	public void ResetToDaily()
	{
		OnClickDailyBtn();
	}

	public void OnClickDailyBtn()
	{
		OnClickDailyBtn(MAPTYPE.INVALID);
	}

	public void OnClickDailyBtn(MAPTYPE type)
	{
		if (CurPageIndex != 0)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActivityUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WildBossUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DailyCopyUIRootLogic, delegate
			{
				SingletonUnity<DailyCopyUIRootLogic>.Instance.EnableReset();
				SingletonUnity<DailyCopyUIRootLogic>.Instance.TargetTypeId = (int)type;
				WaitResponseUIRootLogic.OpenWaitBox(145, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(0);
			CurPageIndex = 0;
		}
	}

	public void OnClickTimeBtn()
	{
		OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.INVALID);
	}

	public void OnClickTimeBtn(GameDefine.ACTIVITY_TYPE type)
	{
		if (CurPageIndex != 1)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WildBossUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyCopyUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DailyActivityUIRootLogic, delegate
			{
				SingletonUnity<DailyActivityUIRootLogic>.Instance.EnableReset();
				SingletonUnity<DailyActivityUIRootLogic>.Instance.TargetTypeId = (int)type;
				WaitResponseUIRootLogic.OpenWaitBox(225, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_activity_info>();
				NetLogic.GetInstance().Send<Protocol.request_wild_boss_info>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(1);
			CurPageIndex = 1;
		}
	}

	public void OnClickTowerBtn()
	{
		if (CurPageIndex != 3)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActivityUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyCopyUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WildBossUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TowerUIRootLogic, delegate
			{
				SingletonUnity<TowerUIRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(202, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_tower_copy_info>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(3);
			CurPageIndex = 3;
		}
	}

	public void OnClickRankBtn()
	{
		if (CurPageIndex != 2)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActivityUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyCopyUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WildBossUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			WaitResponseUIRootLogic.OpenWaitBox(133, 10f, 0f);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.RankPVPRoot, delegate
			{
				SingletonUnity<RankPVPUIRootLogic>.Instance.EnableReset();
			});
			request_rank_pvp_data.request rpcReq = new request_rank_pvp_data.request();
			NetLogic.GetInstance().Send<Protocol.request_rank_pvp_data>(rpcReq);
			request_random_rank_pvp_opponent.request rpcReq2 = new request_random_rank_pvp_opponent.request();
			NetLogic.GetInstance().Send<Protocol.request_random_rank_pvp_opponent>(rpcReq2);
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(2);
			CurPageIndex = 2;
		}
	}

	public void OnClickWildBossBtn()
	{
		if (CurPageIndex != 4)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActivityUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyCopyUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.WildBossUIRootLogic, delegate
			{
				SingletonUnity<WildBossRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(200, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_wild_boss_info>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(4);
			CurPageIndex = 4;
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActivityUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyCopyUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WildBossUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ActivityUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyRewardNewRoot);
		CheckPrePage();
	}

	public void BackTowerUI()
	{
		OnClickTowerBtn();
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(3);
	}

	private void OnEnable()
	{
		InitActivityUI();
		mPrePage = UI_PAGE_TYPE.INVALID;
	}
}
