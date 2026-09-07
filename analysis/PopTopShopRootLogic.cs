using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class PopTopShopRootLogic : SingletonUnity<PopTopShopRootLogic>
{
	public List<ShopItemBtnLogic> mShopItemBtns = new List<ShopItemBtnLogic>();

	public UIGrid ItemGrid;

	public UILabel curPageLable;

	private int curPage = 1;

	private int maxPage = 1;

	public GameObject specialMoneyObj;

	public UILabel MoneyLabel;

	private GameDefine.SHOP_TYPE curType;

	private GameObject curSelectObj;

	private shop_item curSelectItem;

	public UISprite IconSprite;

	public UISprite QualitySprite;

	public GameObject DescObj;

	public UILabel DescLabel;

	public UILabel ItemNameLabel;

	private int buyCount = 1;

	public UILabel BuyCountLabel;

	public UILabel BuyCostLabel;

	private int buyCost;

	private int remainCount;

	public UILabel CountTimeLabel;

	private string NeedItemid = string.Empty;

	private List<shop_item> shopList;

	public GameObject AttributeObj;

	public UILabel[] LeftAttrLabelList;

	public UILabel[] RightAttrLabelList;

	public UISprite[] LefeAttrIconList;

	public UILabel RefershTimeLabel;

	private void OnEnable()
	{
		buyCount = 1;
		NeedItemid = string.Empty;
		curPage = -1;
		curSelectObj = null;
	}

	public void EnableReset()
	{
		ClearSelectObj();
		UnityVersionUtil.SetActiveRecursive(specialMoneyObj, state: false);
		for (int i = 0; i < mShopItemBtns.Count; i++)
		{
			NGUITools.SetActive(mShopItemBtns[i].gameObject, state: false);
		}
		for (int j = 0; j < LeftAttrLabelList.Length; j++)
		{
			UnityVersionUtil.SetActiveRecursive(LeftAttrLabelList[j].gameObject, state: false);
		}
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		RefershTimeLabel.text = StrDictionary.GetDictionaryString("#{301102}", TimeTools.GetLocalShowTime_HM(playerCommonData.ResetTime, playerCommonData.TimeOffset));
	}

	public void ShowItemProdect(string needItemid)
	{
		NeedItemid = needItemid;
	}

	private void UpdateSelectItem()
	{
		ItemData itemDataByID = DataManager.GetItemDataByID(curSelectItem.ItemID);
		int itemQuality = (int)curSelectItem.Quality;
		if (itemDataByID == null)
		{
			return;
		}
		if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP || itemDataByID.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			NGUITools.SetActive(DescObj.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(AttributeObj, state: true);
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				itemQuality = (int)curSelectItem.Quality;
			}
			else if (itemDataByID.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
			{
				itemQuality = itemDataByID.Quality;
			}
			EquipData equipDataById = DataManager.GetEquipDataById(itemDataByID.ID);
			for (int i = 0; i < LeftAttrLabelList.Length; i++)
			{
				if (i < equipDataById.GetBaseAttCount())
				{
					UnityVersionUtil.SetActiveRecursive(LeftAttrLabelList[i].gameObject, state: true);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(LeftAttrLabelList[i].gameObject, state: false);
				}
			}
			int[] array = new int[4] { -1, -1, -1, -1 };
			if (equipDataById.BaseStatusType != 0)
			{
				LeftAttrLabelList[0].text = GameDefine.GetAttributeName_S((int)equipDataById.BaseStatusType);
				array[0] = equipDataById.GetAttrValByQualityAndLevel(0, itemQuality, 0);
				RightAttrLabelList[0].text = GameDefine.GetAttributeValueStr((int)equipDataById.BaseStatusType, array[0]);
				LefeAttrIconList[0].spriteName = GameDefine.GetAttributeIcon((int)equipDataById.BaseStatusType);
			}
			if (equipDataById.Status1 != 0)
			{
				LeftAttrLabelList[1].text = GameDefine.GetAttributeName_S(equipDataById.Status1);
				array[1] = equipDataById.GetAttrValByQualityAndLevel(1, itemQuality, 0);
				RightAttrLabelList[1].text = GameDefine.GetAttributeValueStr(equipDataById.Status1, array[1]);
				LefeAttrIconList[1].spriteName = GameDefine.GetAttributeIcon(equipDataById.Status1);
			}
			if (equipDataById.Status2 != 0)
			{
				LeftAttrLabelList[2].text = GameDefine.GetAttributeName_S(equipDataById.Status2);
				array[2] = equipDataById.GetAttrValByQualityAndLevel(2, itemQuality, 0);
				RightAttrLabelList[2].text = GameDefine.GetAttributeValueStr(equipDataById.Status2, array[2]);
				LefeAttrIconList[2].spriteName = GameDefine.GetAttributeIcon(equipDataById.Status2);
			}
			if (equipDataById.ExStatus != 0)
			{
				LeftAttrLabelList[3].text = GameDefine.GetAttributeName_S(equipDataById.ExStatus);
				array[3] = equipDataById.GetAttrValByQualityAndLevel(3, itemQuality, 0);
				RightAttrLabelList[3].text = GameDefine.GetAttributeValueStr(equipDataById.ExStatus, array[3]);
				LefeAttrIconList[3].spriteName = GameDefine.GetAttributeIcon(equipDataById.ExStatus);
			}
		}
		else if (itemDataByID.Type == GameDefine.ITEM_TYPE.BADGE)
		{
			BadgeData badgeDataById = DataManager.GetBadgeDataById(itemDataByID.ID);
			for (int j = 0; j < LeftAttrLabelList.Length; j++)
			{
				NGUITools.SetActive(LeftAttrLabelList[j].gameObject, state: false);
			}
			if (badgeDataById.Status1 != -1)
			{
				NGUITools.SetActive(LeftAttrLabelList[0].gameObject, state: true);
				LeftAttrLabelList[0].text = GameDefine.GetAttributeName_S(badgeDataById.Status1);
				RightAttrLabelList[0].text = GameDefine.GetAttributeValueStr(badgeDataById.Status1, badgeDataById.Value1);
				LefeAttrIconList[0].spriteName = GameDefine.GetAttributeIcon(badgeDataById.Status1);
			}
			if (badgeDataById.Status2 != -1)
			{
				NGUITools.SetActive(LeftAttrLabelList[1].gameObject, state: true);
				LeftAttrLabelList[1].text = GameDefine.GetAttributeName_S(badgeDataById.Status2);
				RightAttrLabelList[1].text = GameDefine.GetAttributeValueStr(badgeDataById.Status2, badgeDataById.Value2);
				LefeAttrIconList[1].spriteName = GameDefine.GetAttributeIcon(badgeDataById.Status2);
			}
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(AttributeObj, state: false);
			DescLabel.text = itemDataByID.MDescription;
			NGUITools.SetActive(DescObj.gameObject, state: true);
		}
		IconSprite.spriteName = itemDataByID.BackPackIcon;
		ItemNameLabel.text = itemDataByID.MName;
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
		if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			UnityVersionUtil.SetActiveRecursive(QualitySprite.gameObject, state: true);
			QualitySprite.spriteName = ((EQUIP_QUALITY)curSelectItem.Quality).ToString();
			ItemNameLabel.color = GameDefine.GetColorByQuality((EQUIP_QUALITY)curSelectItem.Quality);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(QualitySprite.gameObject, state: true);
			QualitySprite.spriteName = itemDataByID.QualityType.ToString();
			ItemNameLabel.color = GameDefine.GetColorByQuality(itemDataByID.QualityType);
		}
		UpdateBuyCost();
	}

	private void UpdateBuyCost()
	{
		BuyCountLabel.text = $"[u]{buyCount}[/u]";
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
			return;
		}
		if (buyCount < 1)
		{
			if (remainCount == 0)
			{
				NoticeLogic.AddNotifyData("#{101244}");
			}
		}
		else
		{
			if (!GameMoneyHelper.BeforeCheckBuyTop((GameDefine.MONEY_TYPE)curSelectItem.PriceType, buyCost))
			{
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

	public void UpdateShop(ret_ask_shop_list.request request)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (curPage != (int)request.curPage)
		{
			curSelectObj = null;
		}
		curPage = (int)request.curPage;
		maxPage = (int)request.maxPage;
		curType = (GameDefine.SHOP_TYPE)request.type;
		List<shop_item> list = (shopList = request.shop_list);
		int num = list.Count - mShopItemBtns.Count;
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
			NGUITools.SetActive(mShopItemBtns[j].gameObject, j < list.Count);
		}
		curPageLable.text = $"{curPage}/{maxPage}";
		if (request.shop_list.Count == 0)
		{
			return;
		}
		for (int k = 0; k < list.Count; k++)
		{
			if (curSelectObj == null)
			{
				curSelectObj = mShopItemBtns[k].gameObject;
				curSelectItem = list[k];
			}
			mShopItemBtns[k].Reset(list[k], curType, UpdateSelect);
		}
		ItemGrid.Reposition();
		if (curType == GameDefine.SHOP_TYPE.GUILD_SHOP)
		{
			UnityVersionUtil.SetActiveRecursive(specialMoneyObj, state: true);
			MoneyLabel.text = playerData.GuildContribute.ToString();
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(specialMoneyObj, state: false);
		}
		if (!string.IsNullOrEmpty(NeedItemid))
		{
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].ItemID.Equals(NeedItemid))
				{
					curSelectObj = mShopItemBtns[l].gameObject;
					curSelectItem = list[l];
					NeedItemid = string.Empty;
					break;
				}
			}
		}
		UpdateSelect(curSelectItem, curSelectObj);
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

	public void OnClickLeft()
	{
		int num = curPage - 1;
		if (num > 0)
		{
			ask_shop_list.request request = new ask_shop_list.request();
			request.curPage = num;
			request.type = (long)curType;
			request.subType = 1L;
			NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request);
		}
	}

	public void OnClikcRight()
	{
		int num = curPage + 1;
		if (num <= maxPage)
		{
			ask_shop_list.request request = new ask_shop_list.request();
			request.curPage = num;
			request.type = (long)curType;
			request.subType = 1L;
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

	public void OnClickSelectItem()
	{
		if (curSelectItem != null)
		{
			ItemInfoRootLogicNew.ShowItemTips(curSelectItem);
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

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopTopShopRoot);
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
}
