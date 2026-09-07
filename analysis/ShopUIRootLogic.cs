using System.Collections.Generic;
using SprotoType;

public class ShopUIRootLogic : SingletonUnity<ShopUIRootLogic>
{
	private int curPageIndex = -1;

	private GameDefine.UIBACKTYPE curBackType;

	public void InitShopUI()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			WelfareData welfareData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData;
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickBuyDiamondBtn, isIcon: true, "CZ_left_ShangDian_ChongZhi", StrDictionary.GetDictionaryString("#{300005}"), FUNCTION_TYPE.SHOP_BUY);
			list.Add(item);
			MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickToolsBtn, isIcon: true, "CZ_left_BadgeUp", StrDictionary.GetDictionaryString("#{300001}"), FUNCTION_TYPE.SHOP_TOOL);
			list.Add(item2);
			MenuTabBtnInfo item3 = new MenuTabBtnInfo(OnClickEquipBtn, isIcon: true, "CZ_left_Equipment", StrDictionary.GetDictionaryString("#{300002}"), FUNCTION_TYPE.SHOP_EQUIP);
			list.Add(item3);
			MenuTabBtnInfo item4 = new MenuTabBtnInfo(OnClickBigSaleBtn, isIcon: true, "CZ_left_1yuan", StrDictionary.GetDictionaryString("#{300003}"), FUNCTION_TYPE.SHOP_BIGSALE);
			list.Add(item4);
			MenuTabBtnInfo item5 = new MenuTabBtnInfo(OnClickGuildBtn, isIcon: true, "CZ_left_ShangDian_GongHui", StrDictionary.GetDictionaryString("#{300004}"), FUNCTION_TYPE.SHOP_GUILD);
			list.Add(item5);
			MenuTabBtnInfo item6 = new MenuTabBtnInfo(OnClickMonthlyCardBtn, isIcon: true, "CZ_left_yueKa", StrDictionary.GetDictionaryString("#{300707}"), FUNCTION_TYPE.SHOP_VIP, welfareData.HaveVipTips);
			list.Add(item6);
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
			curPageIndex = -1;
		});
	}

	public void Reset()
	{
		OnClickBuyDiamondBtn();
		SingletonUnity<MenuBaseRootLogic>.Instance.AutoClickTipsTap();
	}

	public void OnClickMonthlyCardBtn()
	{
		if (curPageIndex != 5)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BuyDiamondRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopBigSaleRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MonthlyCardRoot, delegate
			{
				SingletonUnity<MonthlyCardRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(299, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.require_vip_info>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(5);
			curPageIndex = 5;
		}
	}

	public void OnClickToolsBtn()
	{
		OnClickToolsBtn(GameDefine.SHOP_TAB_TYPE.INVALID);
	}

	public void OnClickToolsBtn(GameDefine.SHOP_TAB_TYPE tarclass, GameDefine.UIBACKTYPE needback = GameDefine.UIBACKTYPE.NOTHINTG, string needitemid = null)
	{
		if (curPageIndex != 1)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BuyDiamondRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MonthlyCardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopBigSaleRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopTabRootLogic, delegate
			{
				SingletonUnity<ShopTabRootLogic>.Instance.EnableReset(GameDefine.SHOP_TYPE.TOOL_SHOP, needitemid);
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(1);
			curPageIndex = 1;
			curBackType = needback;
		}
	}

	public void OnClickEquipBtn()
	{
		OnClickEquipBtn(GameDefine.SHOP_TAB_TYPE.INVALID);
	}

	public void OnClickEquipBtn(GameDefine.SHOP_TAB_TYPE tarclass, GameDefine.UIBACKTYPE needback = GameDefine.UIBACKTYPE.NOTHINTG, string needitemid = null)
	{
		if (curPageIndex != 2)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BuyDiamondRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MonthlyCardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopBigSaleRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopTabRootLogic, delegate
			{
				SingletonUnity<ShopTabRootLogic>.Instance.EnableReset(GameDefine.SHOP_TYPE.EQUIP_SHOP, needitemid);
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(2);
			curPageIndex = 2;
			curBackType = needback;
		}
	}

	public void OnClickBigSaleBtn()
	{
		OnClickBigSaleBtn(GameDefine.SHOP_TAB_TYPE.INVALID);
	}

	public void OnClickBigSaleBtn(GameDefine.SHOP_TAB_TYPE tarclass, GameDefine.UIBACKTYPE needback = GameDefine.UIBACKTYPE.NOTHINTG, string needitemid = null)
	{
		if (curPageIndex != 3)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BuyDiamondRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MonthlyCardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopBigSaleRoot, delegate
			{
				SingletonUnity<ShopBigSaleRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(274, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_special_big_pack>();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(3);
			curPageIndex = 3;
			curBackType = needback;
		}
	}

	public void OnClickGuildBtn()
	{
		OnClickGuildBtn(GameDefine.SHOP_TAB_TYPE.INVALID);
	}

	public void OnClickGuildBtn(GameDefine.SHOP_TAB_TYPE tarclass, GameDefine.UIBACKTYPE needback = GameDefine.UIBACKTYPE.NOTHINTG, string needitemid = null)
	{
		if (curPageIndex == 4)
		{
			return;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BuyDiamondRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MonthlyCardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopBigSaleRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopTabRootLogic, delegate
			{
				SingletonUnity<ShopTabRootLogic>.Instance.EnableReset(GameDefine.SHOP_TYPE.GUILD_SHOP, needitemid);
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(4);
			curPageIndex = 4;
			curBackType = needback;
		}
		else
		{
			NoticeLogic.AddNotifyData("#{102006}");
		}
	}

	public void OnClickBuyDiamondBtn()
	{
		OnClickBuyDiamondBtn(GameDefine.UIBACKTYPE.NOTHINTG);
	}

	public void OnClickBuyDiamondBtn(GameDefine.UIBACKTYPE needback)
	{
		if (curPageIndex != 0)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MonthlyCardRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopBigSaleRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BuyDiamondRoot, delegate
			{
				SingletonUnity<BuyDiamondRootLogic>.Instance.EnableReset();
				ask_shop_list.request rpcReq = new ask_shop_list.request
				{
					type = 4L
				};
				NetLogic.GetInstance().Send<Protocol.ask_shop_list>(rpcReq);
				WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(0);
			curPageIndex = 0;
			curBackType = needback;
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BuyDiamondRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopTabRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MonthlyCardRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShopBigSaleRoot);
		if (curBackType == GameDefine.UIBACKTYPE.FASHION)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
			{
				SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickFashionBackPackBtn();
			});
		}
		else if (curBackType == GameDefine.UIBACKTYPE.EQUIP)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
			{
				SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickEquipBackPackBtn();
			});
		}
		else if (curBackType == GameDefine.UIBACKTYPE.BADGE)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
			{
				SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickBadgeBtn();
			});
		}
		else if (curBackType == GameDefine.UIBACKTYPE.ENHANCE)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
			{
				SingletonUnity<EquipStrengthenUIRootLogic>.Instance.Reset();
			});
		}
	}

	private void OnEnable()
	{
		InitShopUI();
	}
}
