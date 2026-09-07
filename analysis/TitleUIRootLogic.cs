using System.Collections.Generic;

public class TitleUIRootLogic : SingletonUnity<TitleUIRootLogic>
{
	private int curPageIndex = -1;

	public void InitTitleUI()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickPlayerTitleBtn, isIcon: true, "CZ_left_Character", StrDictionary.GetDictionaryString("#{101701}"), FUNCTION_TYPE.TITLE_TITLE, CheckTitleTips);
			list.Add(item);
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
			curPageIndex = -1;
			SingletonUnity<MenuBaseRootLogic>.Instance.mCurBtnList[0].OnClickBtn();
		});
	}

	public void OnClickPlayerTitleBtn()
	{
		if (curPageIndex != 0)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GameMenuShengWangRootUI, delegate
			{
				SingletonUnity<JSShengWangLogic>.Instance.Reset();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(0);
			curPageIndex = 0;
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TitleUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuShengWangRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
	}

	private void OnEnable()
	{
		InitTitleUI();
	}

	public bool CheckTitleTips()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		int curTitleExp = playerData.MainPlayerAttrData.CurTitleExp;
		int curTitleLevel = playerData.MainPlayerAttrData.CurTitleLevel;
		if (curTitleLevel < 10 && curTitleLevel >= 0)
		{
			TitleData titleDateById = DataManager.GetTitleDateById(curTitleLevel.ToString());
			if (curTitleExp >= titleDateById.EXP)
			{
				return true;
			}
			return false;
		}
		return false;
	}
}
