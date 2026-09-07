using System.Collections.Generic;
using SprotoType;

public class NewActivityUIRootLogic : SingletonUnity<NewActivityUIRootLogic>
{
	private UI_PAGE_TYPE mPrePage = UI_PAGE_TYPE.INVALID;

	public int CurPageIndex = -1;

	private int mMissionChangePosLevel = 10;

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

	public int GetPageIndex(FUNCTION_TYPE type)
	{
		return type switch
		{
			FUNCTION_TYPE.GIFT_ACTIVITY => 0, 
			FUNCTION_TYPE.DOMIN => 1, 
			FUNCTION_TYPE.ACTIVITY_DAILY => 2, 
			FUNCTION_TYPE.ACTIVITY_TIME => 3, 
			FUNCTION_TYPE.RANK_PVP => 4, 
			FUNCTION_TYPE.MISSION => 5, 
			_ => 0, 
		};
	}

	public void InitActivityUI()
	{
		ConfigData configDataByKey = DataManager.GetConfigDataByKey("MissionPosLevel");
		if (configDataByKey != null)
		{
			mMissionChangePosLevel = int.Parse(configDataByKey.ContentValue);
		}
		else
		{
			mMissionChangePosLevel = 10;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			WelfareData welfareData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData;
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickDailyActiveBtn, isIcon: true, "CZ_left_huoYue", StrDictionary.GetDictionaryString("#{300105}"), FUNCTION_TYPE.GIFT_ACTIVITY, welfareData.HaveDailyActivityTips);
			MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickDailyBtn, isIcon: true, "CZ_left_Daily", StrDictionary.GetDictionaryString("#{101535}"), FUNCTION_TYPE.ACTIVITY_DAILY, playerData.CopyInfoData.IsHaveDailyItemTips);
			MenuTabBtnInfo item3 = new MenuTabBtnInfo(OnClickTimeBtn, isIcon: true, "CZ_left_Activity", StrDictionary.GetDictionaryString("#{101536}"), FUNCTION_TYPE.ACTIVITY_TIME, playerData.ActivityData.IsHaveActTips);
			MenuTabBtnInfo item4 = new MenuTabBtnInfo(OnClickRankBtn, isIcon: true, "CZ_left_PVP", StrDictionary.GetDictionaryString("#{100106}"), FUNCTION_TYPE.RANK_PVP, playerData.RankPVPData.IsHavePVPTips);
			MenuTabBtnInfo item5 = new MenuTabBtnInfo(OnClickMissionBtn, isIcon: true, "CZ_left_Mission", StrDictionary.GetDictionaryString("#{100324}"), FUNCTION_TYPE.MISSION);
			MenuTabBtnInfo item6 = new MenuTabBtnInfo(OnClickDominBtn, isIcon: true, "CZ_left_Domain", StrDictionary.GetDictionaryString("#{103003}"), FUNCTION_TYPE.DOMIN);
			list.Add(item);
			list.Add(item6);
			list.Add(item2);
			list.Add(item3);
			list.Add(item4);
			list.Add(item5);
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
			CurPageIndex = -1;
		});
	}

	public void OnClickDailyActiveBtn()
	{
		if (CurPageIndex != GetPageIndex(FUNCTION_TYPE.GIFT_ACTIVITY))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMissionUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyActivityUIRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMapUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyCopyUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominInfoRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominPageRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DailyActiveRewardRoot, delegate
			{
				SingletonUnity<DailyActiveRewardRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(261, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_daily_active>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.GIFT_ACTIVITY));
			CurPageIndex = GetPageIndex(FUNCTION_TYPE.GIFT_ACTIVITY);
		}
	}

	public void OnClickActivityBtn()
	{
		OnClickActivityBtn(GameDefine.ACTIVITY_TYPE.INVALID);
	}

	public void OnClickActivityBtn(GameDefine.ACTIVITY_TYPE type)
	{
		if (CurPageIndex == GetPageIndex(FUNCTION_TYPE.GUILD_ACTIVITY))
		{
			return;
		}
		Singleton<ObjManager>.Instance.MainPlayer.ApplyUpDataGuild();
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveGuild())
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMissionUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyActivityUIRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMapUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyCopyUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActiveRewardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominPageRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominInfoRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildActivityRootLogic, delegate
			{
				SingletonUnity<GuildActivityRootLogic>.Instance.EnableReset();
				SingletonUnity<GuildActivityRootLogic>.Instance.TargetTypeId = (int)type;
				WaitResponseUIRootLogic.OpenWaitBox(195, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_guild_boss>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.GUILD_ACTIVITY));
			CurPageIndex = GetPageIndex(FUNCTION_TYPE.GUILD_ACTIVITY);
		}
		else
		{
			NoticeLogic.AddNotifyData("#{102006}");
		}
	}

	public void Reset()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_DAILY))
		{
			OnClickDailyBtn();
		}
		else
		{
			OnClickMissionBtn();
		}
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

	public void OnClickDailyBtn(MAPTYPE type, string copyid = null, string mapid = null, GameDefine.ACTIVITY_TYPE acttype = GameDefine.ACTIVITY_TYPE.INVALID)
	{
		if (CurPageIndex == GetPageIndex(FUNCTION_TYPE.ACTIVITY_DAILY))
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMissionUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyActivityUIRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActiveRewardRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominPageRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominInfoRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewMapUIRootLogic, delegate
		{
			if (string.IsNullOrEmpty(mapid))
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID);
			}
			else
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.Reset(mapid);
			}
		});
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewDailyCopyUIRootLogic, delegate
		{
			SingletonUnity<NewDailyCopyUIRootLogic>.Instance.EnableReset();
			SingletonUnity<NewDailyCopyUIRootLogic>.Instance.TargetTypeId = (int)type;
			SingletonUnity<NewDailyCopyUIRootLogic>.Instance.TargetActId = copyid;
			SingletonUnity<NewDailyCopyUIRootLogic>.Instance.TargetActType = acttype;
			WaitResponseUIRootLogic.OpenWaitBox(145, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
			NetLogic.GetInstance().Send<Protocol.request_tower_copy_info>();
		});
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.ACTIVITY_DAILY));
		CurPageIndex = GetPageIndex(FUNCTION_TYPE.ACTIVITY_DAILY);
	}

	public void OnClickTimeBtn()
	{
		OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.INVALID);
	}

	public void OnClickTimeBtn(GameDefine.ACTIVITY_TYPE type, string mapid = null, bool isMapClick = false)
	{
		if (CurPageIndex == GetPageIndex(FUNCTION_TYPE.ACTIVITY_TIME))
		{
			return;
		}
		Singleton<ObjManager>.Instance.MainPlayer.ApplyUpDataGuild();
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMissionUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyCopyUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActiveRewardRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominPageRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominInfoRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewMapUIRootLogic, delegate
		{
			if (string.IsNullOrEmpty(mapid))
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID);
			}
			else
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.Reset(mapid);
			}
		});
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewDailyActivityUIRoot, delegate
		{
			SingletonUnity<NewDailyActivityUIRootLogic>.Instance.EnableReset();
			SingletonUnity<NewDailyActivityUIRootLogic>.Instance.TargetTypeId = (int)type;
			SingletonUnity<NewDailyActivityUIRootLogic>.Instance.IsMapClick = isMapClick;
			WaitResponseUIRootLogic.OpenWaitBox(225, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.request_activity_info>();
			NetLogic.GetInstance().Send<Protocol.request_wild_boss_info>();
			WaitResponseUIRootLogic.OpenWaitBox(195, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.request_guild_boss>();
		});
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.ACTIVITY_TIME));
		CurPageIndex = GetPageIndex(FUNCTION_TYPE.ACTIVITY_TIME);
	}

	public void OnClickRankBtn()
	{
		if (CurPageIndex != GetPageIndex(FUNCTION_TYPE.RANK_PVP))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMissionUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyCopyUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyActivityUIRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMapUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActiveRewardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominPageRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominInfoRoot);
			WaitResponseUIRootLogic.OpenWaitBox(133, 10f, 0f);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.RankPVPRoot, delegate
			{
				SingletonUnity<RankPVPUIRootLogic>.Instance.EnableReset();
			});
			request_rank_pvp_data.request rpcReq = new request_rank_pvp_data.request();
			NetLogic.GetInstance().Send<Protocol.request_rank_pvp_data>(rpcReq);
			request_random_rank_pvp_opponent.request rpcReq2 = new request_random_rank_pvp_opponent.request();
			NetLogic.GetInstance().Send<Protocol.request_random_rank_pvp_opponent>(rpcReq2);
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.RANK_PVP));
			CurPageIndex = GetPageIndex(FUNCTION_TYPE.RANK_PVP);
		}
	}

	public void OnClickMissionBtn()
	{
		OnClickMissionBtn(string.Empty);
	}

	public void OnClickMissionBtn(string missionId, string mapid = null)
	{
		if (CurPageIndex == GetPageIndex(FUNCTION_TYPE.MISSION))
		{
			if (SingletonUnity<NewMissionUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMissionUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewMissionUIRootLogic>.Instance.Reset(missionId);
			}
			return;
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyActivityUIRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyCopyUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActiveRewardRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominPageRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominInfoRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewMapUIRootLogic, delegate
		{
			if (string.IsNullOrEmpty(mapid))
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID);
			}
			else
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.Reset(mapid);
			}
		});
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewMissionUIRootLogic, delegate
		{
			SingletonUnity<NewMissionUIRootLogic>.Instance.Reset(missionId);
		});
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.MISSION));
		CurPageIndex = GetPageIndex(FUNCTION_TYPE.MISSION);
	}

	public void OnClickDominBtn()
	{
		OnClickDominBtn(string.Empty);
	}

	public void OnClickDominBtn(string dominId)
	{
		if (CurPageIndex == GetPageIndex(FUNCTION_TYPE.DOMIN))
		{
			if (SingletonUnity<DominRootLogic>.Exists)
			{
				SingletonUnity<DominRootLogic>.Instance.ClickTargetLine(dominId);
				return;
			}
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominInfoRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DominPageRoot, delegate
			{
				NetLogic.GetInstance().Send<Protocol.request_domin_info>();
				SingletonUnity<DominRootLogic>.Instance.EnableReset(dominId);
			});
			return;
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyActivityUIRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyCopyUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActiveRewardRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMissionUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominInfoRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewMapUIRootLogic, delegate
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.Reset("11");
		});
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DominPageRoot, delegate
		{
			WaitResponseUIRootLogic.OpenWaitBox(310, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.request_domin_info>();
			SingletonUnity<DominRootLogic>.Instance.EnableReset(dominId);
		});
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(GetPageIndex(FUNCTION_TYPE.DOMIN));
		CurPageIndex = GetPageIndex(FUNCTION_TYPE.DOMIN);
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyCopyUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewActivityUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPVPRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMapUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMissionUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyActivityUIRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActiveRewardRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominPageRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominInfoRoot);
		CheckPrePage();
	}

	private void OnEnable()
	{
		InitActivityUI();
		mPrePage = UI_PAGE_TYPE.INVALID;
	}
}
