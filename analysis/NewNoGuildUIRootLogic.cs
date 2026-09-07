using System.Collections.Generic;
using SprotoType;

public class NewNoGuildUIRootLogic : SingletonUnity<NewNoGuildUIRootLogic>
{
	public enum GuildRootType
	{
		CREATEGUILD_TYPE,
		GUILDLIST_TYPE
	}

	public void Reset()
	{
	}

	public void UpdateGuildList(Dictionary<long, guild_info> guildDic, int curPage, int maxPage)
	{
	}

	public void ShowSearchResult(List<guild_info> resultList, List<long> rankList)
	{
	}

	public void DisableSearchFlag()
	{
	}

	public void OnClickCreatBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildCreateUIRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildListInfoRoot);
	}

	public void OnClickGuildListBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildCreateUIRoot);
		Singleton<ObjManager>.Instance.MainPlayer.SearchAllGuild();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildListInfoRoot, delegate
		{
			SingletonUnity<GuildListInfoRootLogic>.Instance.PreResetGuildListInfo();
		});
		WaitResponseUIRootLogic.OpenWaitBox(152, 10f, 0f);
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildCreateUIRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildListInfoRoot);
	}

	private void OnEnable()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickGuildListBtn, isIcon: true, "CZ_tuBiao_RenWuShuXing", "List", FUNCTION_TYPE.GUILD_LIST);
			MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickCreatBtn, isIcon: true, "CZ_tuBiao_RenWuShuXing", "Create", FUNCTION_TYPE.GUILD_CREATE);
			list.Add(item);
			list.Add(item2);
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
			OnClickCreatBtn();
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(1);
		});
	}
}
