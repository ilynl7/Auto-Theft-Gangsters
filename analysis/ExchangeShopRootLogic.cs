using System.Collections.Generic;

public class ExchangeShopRootLogic : SingletonUnity<ExchangeShopRootLogic>
{
	private int curPageIndex = -1;

	public List<ShopData> mShopDataList = new List<ShopData>();

	public void InitShopUI()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickBattleBtn, isIcon: true, "CZ_left_duiZhanHuoBi", StrDictionary.GetDictionaryString("#{300012}"), FUNCTION_TYPE.SHOP_BATTLE);
			list.Add(item);
			if (isHaveActivityShop())
			{
				MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickActivityBtn, isIcon: true, "CZ_left_huoDong", StrDictionary.GetDictionaryString("#{300013}"), FUNCTION_TYPE.SHOP);
				list.Add(item2);
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
			curPageIndex = -1;
		});
	}

	public bool isHaveActivityShop()
	{
		mShopDataList = DataManager.GetShopDataList();
		for (int i = 0; i < mShopDataList.Count; i++)
		{
			if (mShopDataList[i].Shop == 7 && TimeTools.IsTimeRange(mShopDataList[i].StartTimes, mShopDataList[i].EndTimes))
			{
				return true;
			}
		}
		return false;
	}

	public void Reset()
	{
		OnClickBattleBtn();
		SingletonUnity<MenuBaseRootLogic>.Instance.AutoClickTipsTap();
	}

	public void ResetToEventShop()
	{
		if (isHaveActivityShop())
		{
			OnClickActivityBtn();
		}
		else
		{
			OnClickBattleBtn();
		}
	}

	public void OnClickBattleBtn()
	{
		OnClickBattleBtn(GameDefine.SHOP_TAB_TYPE.INVALID);
	}

	public void OnClickBattleBtn(GameDefine.SHOP_TAB_TYPE tarclass)
	{
		if (curPageIndex != 0)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopTabRootLogic, delegate
			{
				SingletonUnity<ShopTabRootLogic>.Instance.EnableReset(GameDefine.SHOP_TYPE.BATTLECOIN_SHOP);
				SingletonUnity<ShopTabRootLogic>.Instance.ClearSelectObj();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(0);
			curPageIndex = 0;
		}
	}

	public void OnClickActivityBtn()
	{
		OnClickActivityBtn(GameDefine.SHOP_TAB_TYPE.INVALID);
	}

	public void OnClickActivityBtn(GameDefine.SHOP_TAB_TYPE tarclass)
	{
		if (curPageIndex != 1)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopTabRootLogic, delegate
			{
				SingletonUnity<ShopTabRootLogic>.Instance.EnableReset(GameDefine.SHOP_TYPE.ACTIVITY_SHOP);
				SingletonUnity<ShopTabRootLogic>.Instance.ClearSelectObj();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(1);
			curPageIndex = 1;
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ExchangeShopRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
	}

	private void OnEnable()
	{
		InitShopUI();
	}
}
