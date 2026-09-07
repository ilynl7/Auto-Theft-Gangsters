using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ShopTabRootLogic : SingletonUnity<ShopTabRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public ShopItemInfoNew ShowItemInfoRoot;

	public List<ShopItemBtnLogic> mShopItemBtns = new List<ShopItemBtnLogic>();

	public UIGrid ItemGrid;

	public UILabel curPageLable;

	private int curPage = 1;

	private int maxPage = 1;

	public GameObject specialMoneyObj;

	public UILabel MoneyLabel;

	public UISprite specialMoneyFlag;

	private GameDefine.SHOP_TYPE curType;

	private GameObject curSelectObj;

	private shop_item curSelectItem;

	private int buyCount = 1;

	public UILabel BuyCountLabel;

	public UILabel BuyCostLabel;

	private int buyCost;

	private int remainCount;

	private List<shop_item> shopList;

	public UILabel RefershTimeLabel;

	private Color ambientLight;

	private string NeedItemid = string.Empty;

	public UIWidget BuyBtnRoot;

	public GameObject CurSelectObj => curSelectObj;

	public shop_item CurSelectItem => curSelectItem;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (SingletonUnity<TutorialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.CloseCheck();
		}
		if (mOnClickTutorialBtn != null)
		{
			TutorialManager.OnClickTutorialBtn onClickTutorialBtn = mOnClickTutorialBtn;
			mOnClickTutorialBtn = null;
			onClickTutorialBtn();
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	public void EnableReset(GameDefine.SHOP_TYPE shoptype, string needItemid = null)
	{
		ShowItemInfoRoot.Reset();
		NeedItemid = needItemid;
		ClearSelectObj();
		curType = shoptype;
		ambientLight = RenderSettings.ambientLight;
		for (int i = 0; i < mShopItemBtns.Count; i++)
		{
			NGUITools.SetActive(mShopItemBtns[i].gameObject, state: false);
		}
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		buyCount = 1;
		curPage = -1;
		RefershTimeLabel.text = StrDictionary.GetDictionaryString("#{301102}", TimeTools.GetLocalShowTime_HM(playerCommonData.ResetTime, playerCommonData.TimeOffset));
		curSelectObj = null;
		curSelectItem = null;
		WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
		ask_shop_list.request request = new ask_shop_list.request();
		request.type = (long)curType;
		if (!string.IsNullOrEmpty(NeedItemid))
		{
			request.itemId = NeedItemid;
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_START)
		{
			request.special = 1L;
		}
		NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request);
		if (specialMoneyObj != null)
		{
			if (curType == GameDefine.SHOP_TYPE.GUILD_SHOP)
			{
				specialMoneyFlag.spriteName = GameMoneyHelper.GetMoneyIcon(GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE);
				UnityVersionUtil.SetActiveRecursive(specialMoneyObj, state: true);
				specialMoneyFlag.MakePixelPerfect();
				MoneyLabel.text = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GuildContribute.ToString();
			}
			else if (curType == GameDefine.SHOP_TYPE.BATTLECOIN_SHOP)
			{
				specialMoneyFlag.spriteName = GameMoneyHelper.GetMoneyIcon(GameDefine.MONEY_TYPE.BATTLECOIN);
				UnityVersionUtil.SetActiveRecursive(specialMoneyObj, state: true);
				specialMoneyFlag.MakePixelPerfect();
				MoneyLabel.text = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BattleCoin.ToString();
			}
			else if (curType == GameDefine.SHOP_TYPE.ACTIVITY_SHOP)
			{
				specialMoneyFlag.spriteName = GameMoneyHelper.GetMoneyIcon(GameDefine.MONEY_TYPE.ACTIVITYCOIN);
				UnityVersionUtil.SetActiveRecursive(specialMoneyObj, state: true);
				specialMoneyFlag.MakePixelPerfect();
				MoneyLabel.text = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityCoin.ToString();
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(specialMoneyObj, state: false);
			}
		}
		if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			if (curType == GameDefine.SHOP_TYPE.GUILD_SHOP)
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.ShowGangMoney();
			}
			else
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.hideGangMoney();
			}
		}
		flurryShopOpen();
	}

	private void flurryShopOpen()
	{
		switch (curType)
		{
		case GameDefine.SHOP_TYPE.BIGSALE_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_bigsale", "open", "times");
			break;
		case GameDefine.SHOP_TYPE.EQUIP_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_equip", "open", "times");
			break;
		case GameDefine.SHOP_TYPE.TOOL_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_tool", "open", "times");
			break;
		case GameDefine.SHOP_TYPE.GUILD_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_guild", "open", "times");
			break;
		case GameDefine.SHOP_TYPE.BATTLECOIN_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_battle", "open", "times");
			break;
		case GameDefine.SHOP_TYPE.ACTIVITY_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_activity", "open", "times");
			break;
		case GameDefine.SHOP_TYPE.DOLLAR_SHOP:
		case GameDefine.SHOP_TYPE.VIP_SHOP:
			break;
		}
	}

	public void UpdateShop(ret_ask_shop_list.request request)
	{
		if (curType != (GameDefine.SHOP_TYPE)request.type)
		{
			Debug.Log(string.Concat("Shop Ttype error! + curtype:", curType, "***return Type:", (GameDefine.SHOP_TYPE)request.type));
			return;
		}
		if (curType == GameDefine.SHOP_TYPE.GUILD_SHOP && !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			NoticeLogic.AddNotifyData("#{102006}");
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (curPage != (int)request.curPage)
		{
			curSelectObj = null;
		}
		curPage = (int)request.curPage;
		maxPage = (int)request.maxPage;
		List<shop_item> shop_list = request.shop_list;
		shopList = shop_list;
		int num = shopList.Count - mShopItemBtns.Count;
		int count = mShopItemBtns.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(mShopItemBtns[0].gameObject) as GameObject;
				gameObject.name = $"shangPing_{count + i}";
				ItemGrid.AddChild(gameObject.transform);
				gameObject.transform.localScale = Vector3.one;
				mShopItemBtns.Add(gameObject.GetComponent<ShopItemBtnLogic>());
			}
		}
		for (int j = 0; j < mShopItemBtns.Count; j++)
		{
			NGUITools.SetActive(mShopItemBtns[j].gameObject, j < shopList.Count);
		}
		curPageLable.text = $"{curPage}/{maxPage}";
		if (shopList.Count == 0)
		{
			return;
		}
		for (int k = 0; k < shopList.Count; k++)
		{
			if (curSelectObj == null)
			{
				curSelectObj = mShopItemBtns[k].gameObject;
				curSelectItem = shopList[k];
			}
			mShopItemBtns[k].Reset(shopList[k], curType, UpdateSelect);
		}
		ItemGrid.Reposition();
		if (!string.IsNullOrEmpty(NeedItemid))
		{
			for (int l = 0; l < shopList.Count; l++)
			{
				if (shopList[l].ItemID.Equals(NeedItemid))
				{
					curSelectObj = mShopItemBtns[l].gameObject;
					curSelectItem = shopList[l];
					NeedItemid = string.Empty;
					break;
				}
			}
		}
		NeedItemid = string.Empty;
		UpdateSelect(curSelectItem, curSelectObj);
		if (TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_WAIT)
		{
			CheckTutorialEvent();
		}
	}

	private void UpdateSelectItem()
	{
		ShowItemInfoRoot.RefershInfo(curSelectItem);
		if (curSelectItem.Limit > 0)
		{
			remainCount = (int)(curSelectItem.Limit - curSelectItem.curNum);
		}
		else
		{
			remainCount = 999999;
		}
		if (buyCount > remainCount)
		{
			buyCount = remainCount;
		}
		if (remainCount < 1)
		{
			buyCount = 0;
		}
		UpdateBuyCost();
	}

	private void UpdateBuyCost()
	{
		BuyCountLabel.text = $"{buyCount}";
		ItemData itemDataByID = DataManager.GetItemDataByID(curSelectItem.ItemID);
		buyCost = (int)(curSelectItem.Price * buyCount);
		BuyCostLabel.text = GameMoneyHelper.GetMoneyValStr(buyCost, (GameDefine.MONEY_TYPE)curSelectItem.PriceType);
	}

	public void OnClickBuyLeft()
	{
		if (buyCount > 1)
		{
			buyCount--;
			UpdateBuyCost();
		}
		else
		{
			NoticeLogic.AddNotifyData("#{101245}");
		}
	}

	public void OnClickBuyRight()
	{
		if (buyCount < remainCount)
		{
			buyCount++;
			ItemData itemDataByID = DataManager.GetItemDataByID(curSelectItem.ItemID);
			if (itemDataByID != null && itemDataByID.Type == GameDefine.ITEM_TYPE.BOX)
			{
				ShopData shopDataByID = DataManager.GetShopDataByID(curSelectItem.ID);
				if (buyCount > shopDataByID.SingleLimit)
				{
					buyCount = shopDataByID.SingleLimit;
					NoticeLogic.AddNotifyData("#{301124}");
				}
			}
			UpdateBuyCost();
		}
		else
		{
			NoticeLogic.AddNotifyData("#{101246}");
		}
	}

	public void OnClickBuyItem()
	{
		if (curSelectItem == null)
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_CLICK_BUY)
			{
				TutorialManager.CloseTutorial();
			}
			return;
		}
		if (buyCount < 1)
		{
			if (remainCount == 0)
			{
				NoticeLogic.AddNotifyData("#{101244}");
			}
			if (TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_CLICK_BUY)
			{
				TutorialManager.CloseTutorial();
			}
			return;
		}
		if (!GameMoneyHelper.BeforeCheckBuy((GameDefine.MONEY_TYPE)curSelectItem.PriceType, buyCost))
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_CLICK_BUY)
			{
				TutorialManager.CloseTutorial();
			}
			return;
		}
		ShopData shopDataByID = DataManager.GetShopDataByID(curSelectItem.ID);
		if (shopDataByID != null && !string.IsNullOrEmpty(shopDataByID.EndTime) && TimeTools.GetShopItemTime(shopDataByID.EndTimes).TotalSeconds <= 0.0)
		{
			if (GameManager.IsSupportCurDataVersion56())
			{
				NoticeLogic.AddNotifyData("#{301119}");
			}
			else
			{
				NoticeLogic.AddNotifyData("Sales time is over. The item cannot be purchased.");
			}
			if (TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_CLICK_BUY)
			{
				TutorialManager.CloseTutorial();
			}
		}
		else
		{
			buy_shop_item.request request = new buy_shop_item.request();
			request.ID = curSelectItem.ID;
			request.itemCount = buyCount;
			request.type = (long)curType;
			NetLogic.GetInstance().Send<Protocol.buy_shop_item>(request);
		}
	}

	public void UpdateShopItem(shop_item item, long type)
	{
		if (type != (long)curType || item == null)
		{
			return;
		}
		if (curSelectItem != null && item.ID == curSelectItem.ID)
		{
			curSelectItem.curNum = item.curNum;
			UpdateSelect(curSelectItem, curSelectObj);
		}
		if (shopList != null)
		{
			for (int i = 0; i < shopList.Count; i++)
			{
				if (shopList[i].ID == item.ID)
				{
					shopList[i].curNum = item.curNum;
				}
			}
		}
		for (int j = 0; j < mShopItemBtns.Count; j++)
		{
			mShopItemBtns[j].UpdateInfo(item);
		}
	}

	public void UpdateMoneyLabel()
	{
		if (specialMoneyObj != null)
		{
			if (curType == GameDefine.SHOP_TYPE.GUILD_SHOP)
			{
				specialMoneyFlag.spriteName = GameMoneyHelper.GetMoneyIcon(GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE);
				UnityVersionUtil.SetActiveRecursive(specialMoneyObj, state: true);
				specialMoneyFlag.MakePixelPerfect();
				MoneyLabel.text = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GuildContribute.ToString();
			}
			else if (curType == GameDefine.SHOP_TYPE.BATTLECOIN_SHOP)
			{
				specialMoneyFlag.spriteName = GameMoneyHelper.GetMoneyIcon(GameDefine.MONEY_TYPE.BATTLECOIN);
				UnityVersionUtil.SetActiveRecursive(specialMoneyObj, state: true);
				specialMoneyFlag.MakePixelPerfect();
				MoneyLabel.text = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BattleCoin.ToString();
			}
			else if (curType == GameDefine.SHOP_TYPE.ACTIVITY_SHOP)
			{
				specialMoneyFlag.spriteName = GameMoneyHelper.GetMoneyIcon(GameDefine.MONEY_TYPE.ACTIVITYCOIN);
				UnityVersionUtil.SetActiveRecursive(specialMoneyObj, state: true);
				specialMoneyFlag.MakePixelPerfect();
				MoneyLabel.text = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityCoin.ToString();
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(specialMoneyObj, state: false);
			}
		}
	}

	public void OnClickLeft()
	{
		int num = curPage - 1;
		if (num > 0)
		{
			WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			ask_shop_list.request request = new ask_shop_list.request();
			request.curPage = num;
			request.type = (long)curType;
			NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request);
		}
	}

	public void OnClikcRight()
	{
		int num = curPage + 1;
		if (num <= maxPage)
		{
			WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			ask_shop_list.request request = new ask_shop_list.request();
			request.curPage = num;
			request.type = (long)curType;
			NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request);
		}
	}

	private void UpdateSelect(shop_item item)
	{
		for (int i = 0; i < mShopItemBtns.Count; i++)
		{
			mShopItemBtns[i].UpdateSelect(item);
		}
	}

	public void UpdateSelect(shop_item item, GameObject obj)
	{
		curSelectObj = obj;
		if (curSelectItem == null || !curSelectItem.ID.Equals(item.ID))
		{
			buyCount = 1;
		}
		curSelectItem = item;
		UpdateSelect(item);
		UpdateSelectItem();
		flurryShopItemclick(item);
	}

	private void flurryShopItemclick(shop_item clickitem)
	{
		switch (curType)
		{
		case GameDefine.SHOP_TYPE.BIGSALE_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_bigsale", $"shopitem_{clickitem.ID}", "clicktimes");
			break;
		case GameDefine.SHOP_TYPE.EQUIP_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_equip", $"shopitem_{clickitem.ID}", "clicktimes");
			break;
		case GameDefine.SHOP_TYPE.TOOL_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_tool", $"shopitem_{clickitem.ID}", "clicktimes");
			break;
		case GameDefine.SHOP_TYPE.GUILD_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_guild", $"shopitem_{clickitem.ID}", "clicktimes");
			break;
		case GameDefine.SHOP_TYPE.BATTLECOIN_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_battle", $"shopitem_{clickitem.ID}", "clicktimes");
			break;
		case GameDefine.SHOP_TYPE.ACTIVITY_SHOP:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_activity", $"shopitem_{clickitem.ID}", "clicktimes");
			break;
		case GameDefine.SHOP_TYPE.DOLLAR_SHOP:
		case GameDefine.SHOP_TYPE.VIP_SHOP:
			break;
		}
	}

	public void ClearSelectObj()
	{
		curSelectItem = null;
		curSelectObj = null;
	}

	public void OnClickNumLabel()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NumRoot, delegate
		{
			SingletonUnity<NumRootLogic>.Instance.Reset(CalNumFun);
		});
	}

	private void CalNumFun(int num)
	{
		if (curSelectItem == null)
		{
			return;
		}
		int num2 = buyCount;
		int num3 = 0;
		num3 = (int)(GameMoneyHelper.GetMoneyNum((int)curSelectItem.PriceType) / curSelectItem.Price);
		num2 = num switch
		{
			-1 => num2 / 10, 
			-2 => num3, 
			_ => num2 * 10 + num, 
		};
		ItemData itemDataByID = DataManager.GetItemDataByID(curSelectItem.ItemID);
		if (itemDataByID != null && itemDataByID.Type == GameDefine.ITEM_TYPE.BOX)
		{
			ShopData shopDataByID = DataManager.GetShopDataByID(curSelectItem.ID);
			if (num2 > shopDataByID.SingleLimit)
			{
				num2 = shopDataByID.SingleLimit;
			}
		}
		if (num2 > remainCount)
		{
			num2 = remainCount;
		}
		if (remainCount < 1)
		{
			num2 = 0;
		}
		buyCount = num2;
		UpdateBuyCost();
	}

	public void OnClickMaxBtn()
	{
		CalNumFun(-2);
	}

	public void ResetNormalLight()
	{
		RenderSettings.ambientLight = ambientLight;
	}

	private void OnDisable()
	{
		ResetNormalLight();
		ShowItemInfoRoot.ModelViewObj.UnLoadFakeObj();
		if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.hideGangMoney();
		}
	}

	public void OnClickMoneyGetBtn()
	{
		if (curType == GameDefine.SHOP_TYPE.GUILD_SHOP)
		{
			if (!CheckUnlockFunction(FUNCTION_TYPE.GUILD_ACTIVITY))
			{
				return;
			}
			if (GameManager.IsSupportCurDataVersion56())
			{
				MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{100795}"), StrDictionary.GetDictionaryString("#{100127}"), delegate
				{
					if (SingletonUnity<NewGuildUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewGuildUIRootLogic>.Instance.gameObject))
					{
						SingletonUnity<NewGuildUIRootLogic>.Instance.OnClickGuildInfoBtn();
					}
					else
					{
						SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
						SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewGuildUIRootLogic, delegate
						{
							SingletonUnity<NewGuildUIRootLogic>.Instance.targetTabType = 0;
						});
					}
				});
				return;
			}
			MessageBoxLogic.OpenOKCancelBox("Your contribution points is insufficient.#rYou can contribute points by challenging [ffff00]Guild BOSS[-] or [ffff00]Donation[-].#rDo you want to go to the guild for contribution points?", StrDictionary.GetDictionaryString("#{100127}"), delegate
			{
				if (SingletonUnity<NewGuildUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewGuildUIRootLogic>.Instance.gameObject))
				{
					SingletonUnity<NewGuildUIRootLogic>.Instance.OnClickGuildInfoBtn();
				}
				else
				{
					SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewGuildUIRootLogic, delegate
					{
						SingletonUnity<NewGuildUIRootLogic>.Instance.targetTabType = 0;
					});
				}
			});
		}
		else if (curType == GameDefine.SHOP_TYPE.BATTLECOIN_SHOP)
		{
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				return;
			}
			if (GameManager.IsSupportCurDataVersion56())
			{
				MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{100656}"), StrDictionary.GetDictionaryString("#{100127}"), delegate
				{
					SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
					{
						SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE);
					});
				});
				return;
			}
			MessageBoxLogic.OpenOKCancelBox("The battle coin is not enough#rYou can get more coin in [ffff00]survival field[-]", StrDictionary.GetDictionaryString("#{100127}"), delegate
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE);
				});
			});
		}
		else if (curType == GameDefine.SHOP_TYPE.ACTIVITY_SHOP)
		{
			if (GameManager.IsSupportCurDataVersion56())
			{
				MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{502004}"), StrDictionary.GetDictionaryString("#{100127}"));
			}
			else
			{
				MessageBoxLogic.OpenOKBox("Special coin are obtained at the event and can be exchanged for items in the event shop.", StrDictionary.GetDictionaryString("#{100127}"));
			}
		}
	}

	public bool CheckUnlockFunction(FUNCTION_TYPE curFunction)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData.IsFunctionUnlock(curFunction))
		{
			return true;
		}
		NoticeLogic.AddNotifyData("#{101539}");
		return false;
	}
}
