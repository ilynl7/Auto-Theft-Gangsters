using System.Collections.Generic;
using SprotoType;

public class NewGuildUIRootLogic : SingletonUnity<NewGuildUIRootLogic>
{
	public enum SHOW_GUILD_TYPE
	{
		NOTHING = -1,
		NO_GUILD,
		HAS_GUILD
	}

	public enum GUILD_TAB_TYPE
	{
		GUILD_NOTING = -1,
		GUILD_INFO,
		GUILD_MEMBER,
		GUILD_SKILL,
		GUILD_SHOP,
		GUILD_ACTIVITY,
		GUILD_LIST,
		GUILD_CREATE,
		GUILD_STAR,
		GUILD_CITY
	}

	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	private bool isShowActivityTab;

	public bool isInitTab;

	public SHOW_GUILD_TYPE curShowType = SHOW_GUILD_TYPE.NOTHING;

	public GUILD_TAB_TYPE curTabType = GUILD_TAB_TYPE.GUILD_NOTING;

	public int targetTabType = -1;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	public void RequestGuildInfo()
	{
		Singleton<ObjManager>.Instance.MainPlayer.OpenGuild();
		WaitResponseUIRootLogic.OpenWaitBox(153, 10f, 0f);
	}

	public void RequestGuildList()
	{
		Singleton<ObjManager>.Instance.MainPlayer.SearchAllGuild();
		WaitResponseUIRootLogic.OpenWaitBox(153, 10f, 0f);
	}

	private void ShowBlankTabBase()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			List<MenuTabBtnInfo> leftBtnInfo = new List<MenuTabBtnInfo>();
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(leftBtnInfo, OnClickCloseBtn);
			RequestGuildInfo();
		});
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildCreateUIRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildListInfoRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildSkillRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildMemberRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildInfoRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStarRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewGuildUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStrengthenRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildCityRoot);
	}

	public void CloseHasGuildUI()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildInfoRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildMemberRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildSkillRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
		curShowType = SHOW_GUILD_TYPE.NOTHING;
	}

	public void CloseNoGuildUI()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildCreateUIRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildListInfoRoot);
		curShowType = SHOW_GUILD_TYPE.NOTHING;
	}

	private void OnEnable()
	{
		curShowType = SHOW_GUILD_TYPE.NOTHING;
		curTabType = GUILD_TAB_TYPE.GUILD_NOTING;
		ShowBlankTabBase();
		targetTabType = -1;
	}

	public void OnClickCreatBtn()
	{
		if (curTabType != GUILD_TAB_TYPE.GUILD_CREATE)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildListInfoRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildCreateUIRoot);
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(1);
			curTabType = GUILD_TAB_TYPE.GUILD_CREATE;
		}
	}

	public void OnClickGuildListBtn()
	{
		if (curTabType != GUILD_TAB_TYPE.GUILD_LIST)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildCreateUIRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildListInfoRoot, delegate
			{
				SingletonUnity<GuildListInfoRootLogic>.Instance.PreResetGuildListInfo();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(0);
			Singleton<ObjManager>.Instance.MainPlayer.SearchAllGuild();
			curTabType = GUILD_TAB_TYPE.GUILD_LIST;
		}
	}

	public void ShowHasNoGuildInfo(ret_guild_req_list.request request)
	{
		if (curShowType != 0)
		{
			CloseHasGuildUI();
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickGuildListBtn, isIcon: true, "CZ_left_List", StrDictionary.GetDictionaryString("#{100730}"), FUNCTION_TYPE.GUILD_LIST);
			MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickCreatBtn, isIcon: true, "CZ_left_Create", StrDictionary.GetDictionaryString("#{100731}"), FUNCTION_TYPE.GUILD_CREATE);
			list.Add(item);
			list.Add(item2);
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
			curTabType = GUILD_TAB_TYPE.GUILD_NOTING;
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildListInfoRoot, delegate
			{
				if (request != null && request.HasGuild_info)
				{
					SingletonUnity<GuildListInfoRootLogic>.Instance.UpdateGuildListInfo(request.guild_info, (int)request.curPage, (int)request.maxPage);
				}
				else
				{
					SingletonUnity<GuildListInfoRootLogic>.Instance.UpdateGuildListInfo(null, 1, 1);
				}
				SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(0);
				curTabType = GUILD_TAB_TYPE.GUILD_LIST;
			});
			isInitTab = true;
			curShowType = SHOW_GUILD_TYPE.NO_GUILD;
		}
		else if (SingletonUnity<GuildListInfoRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildListInfoRootLogic>.Instance.gameObject))
		{
			if (request != null && request.HasGuild_info)
			{
				SingletonUnity<GuildListInfoRootLogic>.Instance.UpdateGuildListInfo(request.guild_info, (int)request.curPage, (int)request.maxPage);
			}
			else
			{
				SingletonUnity<GuildListInfoRootLogic>.Instance.UpdateGuildListInfo(null, 1, 1);
			}
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.GUILD_WAIT_DATA)
		{
			CheckTutorialEvent();
		}
	}

	public void OnClickGuildInfoBtn()
	{
		if (curTabType != 0)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildMemberRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildSkillRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildInfoRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStarRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStrengthenRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildCityRoot);
			RequestGuildInfo();
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(0);
			curTabType = GUILD_TAB_TYPE.GUILD_INFO;
		}
	}

	public void OnClickMemberBtn()
	{
		if (curTabType != GUILD_TAB_TYPE.GUILD_MEMBER)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildInfoRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildSkillRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStarRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStrengthenRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildCityRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildMemberRootLogic, delegate
			{
				SingletonUnity<GuildMemberRootLogic>.Instance.EnableReset();
				Singleton<ObjManager>.Instance.MainPlayer.ApplyUpdataGuildMemberList();
				WaitResponseUIRootLogic.OpenWaitBox(170, 10f, 0f);
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(1);
			curTabType = GUILD_TAB_TYPE.GUILD_MEMBER;
		}
	}

	public void OnClickShopBtn()
	{
		if (curTabType != GUILD_TAB_TYPE.GUILD_SHOP)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildInfoRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildSkillRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildMemberRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStarRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStrengthenRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildCityRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopTabRootLogic, delegate
			{
				SingletonUnity<ShopTabRootLogic>.Instance.EnableReset(GameDefine.SHOP_TYPE.GUILD_SHOP);
				SingletonUnity<ShopTabRootLogic>.Instance.ClearSelectObj();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(3);
			curTabType = GUILD_TAB_TYPE.GUILD_SHOP;
		}
	}

	public void OnClickActivityBtn()
	{
		if (curTabType != GUILD_TAB_TYPE.GUILD_ACTIVITY)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildInfoRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildSkillRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildMemberRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStarRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStrengthenRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildCityRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildActivityRootLogic, delegate
			{
				SingletonUnity<GuildActivityRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(195, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_guild_boss>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(4);
			curTabType = GUILD_TAB_TYPE.GUILD_ACTIVITY;
		}
	}

	public void ShowHasGuildInfo()
	{
		PlayerData playeData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (curShowType != SHOW_GUILD_TYPE.HAS_GUILD)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			CloseNoGuildUI();
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickGuildInfoBtn, isIcon: true, "CZ_left_Information", StrDictionary.GetDictionaryString("#{100702}"), FUNCTION_TYPE.GUILD_INFO);
			MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickMemberBtn, isIcon: true, "CZ_left_Member", StrDictionary.GetDictionaryString("#{100703}"), FUNCTION_TYPE.GUILD_MEMBER, playerData.PlayerGuild.IsHaveNewApply);
			MenuTabBtnInfo item3 = new MenuTabBtnInfo(OnClickStrengthenBtn, isIcon: true, "CZ_left_Skill", StrDictionary.GetDictionaryString("#{100619}"), FUNCTION_TYPE.GUILD_SKILL);
			MenuTabBtnInfo item4 = new MenuTabBtnInfo(OnClickShopBtn, isIcon: true, "CZ_left_Shop", StrDictionary.GetDictionaryString("#{100705}"), FUNCTION_TYPE.GUILD_SHOP);
			MenuTabBtnInfo item5 = new MenuTabBtnInfo(OnClickActivityBtn, isIcon: true, "CZ_left_Activity", StrDictionary.GetDictionaryString("#{101536}"), FUNCTION_TYPE.GUILD_ACTIVITY, playerData.ActivityData.IsHaveGuildActTips);
			MenuTabBtnInfo item6 = new MenuTabBtnInfo(OnClickGuildCityBtn, isIcon: true, "CZ_left_GangDomain", StrDictionary.GetDictionaryString("#{106033}"), FUNCTION_TYPE.GUILD_CITY);
			list.Add(item);
			list.Add(item2);
			list.Add(item3);
			list.Add(item4);
			list.Add(item5);
			list.Add(item6);
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
			curShowType = SHOW_GUILD_TYPE.HAS_GUILD;
			if (targetTabType == 2)
			{
				targetTabType = -1;
				OnClickStrengthenBtn();
				return;
			}
			if (targetTabType == 0)
			{
				targetTabType = -1;
				OnClickGuildInfoBtn();
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildInfoRootLogic, delegate
			{
				SingletonUnity<GuildInfoRootLogic>.Instance.UpDateGuildInfo(playeData.PlayerGuild);
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(0);
			curTabType = GUILD_TAB_TYPE.GUILD_INFO;
			if (UIUpdateEvent.OnUIPageLoadFinished != null)
			{
				UIUpdateEvent.OnUIPageLoadFinished();
			}
			else
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.AutoClickTipsTap();
			}
		}
		else if (SingletonUnity<GuildInfoRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildInfoRootLogic>.Instance.gameObject))
		{
			SingletonUnity<GuildInfoRootLogic>.Instance.UpDateGuildInfo(playeData.PlayerGuild);
		}
	}

	public void OnClickStrengthenBtn()
	{
		if (curTabType != GUILD_TAB_TYPE.GUILD_SKILL)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildInfoRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildMemberRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStarRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildCityRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildStrengthenRoot, delegate
			{
				SingletonUnity<GuildStrengthenRootLogic>.Instance.Reset();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(2);
			curTabType = GUILD_TAB_TYPE.GUILD_SKILL;
		}
	}

	public void OnClickGuildCityBtn()
	{
		if (curTabType != GUILD_TAB_TYPE.GUILD_CITY)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildInfoRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildSkillRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStarRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStrengthenRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildMemberRootLogic);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildCityRoot, delegate
			{
				WaitResponseUIRootLogic.OpenWaitBox(319, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_guild_map_info>();
				SingletonUnity<GuildCityRootLogic>.Instance.EnableReset();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(5);
			curTabType = GUILD_TAB_TYPE.GUILD_CITY;
		}
	}
}
