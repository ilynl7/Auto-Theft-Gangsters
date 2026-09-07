using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ConsignSellLogic : MonoBehaviour
{
	private PlayerData playerData;

	private List<GameItem> mCurItemList;

	private bool mInitFlag;

	private int currentPage;

	public SpriteState tabEdgeState1;

	public SpriteState tabEdgeState2;

	public SpriteState tabEdgeState3;

	public UISprite tabEdgeSprite1;

	public UISprite tabEdgeSprite2;

	public UISprite tabEdgeSprite3;

	private GameItem currentSelectItem;

	private int selectTime;

	public UISprite selectSpriteQuality;

	public UISprite selectSprite;

	public UILabel selectSpriteNameLabel;

	public LabelState timeLabel1;

	public LabelState timeLabel2;

	public LabelState timeLabel3;

	public UISprite timeSelectSprite;

	public BackPackRootLogic ShowBackPackLogicScript;

	public UISprite SellBtn;

	public UILabel singelPriceLabel;

	public UILabel finalPriceLabel;

	private int singelPrice;

	private int finalPrice;

	public UILabel sellCountLabel;

	private int sellCount;

	public UISlider sellCountSlider;

	public UILabel SellCommissionLabel;

	private int SellCommission = 1000;

	private int[] commisions = new int[3] { 1000, 1500, 2000 };

	private float pricePerfent = 1f;

	private float pressTime;

	public UIButton priceBtnAdd;

	public UIButton priceBtnSub;

	public UISprite ChoosedPic;

	private ItemUILogic mCurChoosedObj;

	private void Awake()
	{
		ShowBackPackLogicScript.onClickItem = OnClickItem;
		ShowBackPackLogicScript.onItemLineReset = OnItemLineReset;
	}

	public void OnItemLineReset(ItemLineLogic curLine)
	{
		if (currentSelectItem == null || currentSelectItem.IsEmpty() || !(mCurChoosedObj == null))
		{
			return;
		}
		for (int i = 0; i < curLine.ItemObjList.Count; i++)
		{
			if (curLine.ItemObjList[i].curItem != null && curLine.ItemObjList[i].curItem.IndexId == currentSelectItem.IndexId)
			{
				ChoosedPic.transform.position = curLine.ItemObjList[i].transform.position;
				NGUITools.SetActive(ChoosedPic.gameObject, state: true);
				mCurChoosedObj = curLine.ItemObjList[i];
				break;
			}
		}
	}

	private void UpdateSlider(int count)
	{
		if (currentSelectItem != null && !currentSelectItem.IsEmpty() && currentSelectItem.StackNum >= 1)
		{
			float value = (float)count / (float)currentSelectItem.StackNum;
			sellCountSlider.value = value;
		}
		else
		{
			sellCountSlider.value = 0f;
		}
	}

	public void OnSelectItem(GameItem item)
	{
		if (currentSelectItem == null || item.IndexId != currentSelectItem.IndexId)
		{
			currentSelectItem = item;
			pricePerfent = 1f;
			UpdateSlider(1);
			UpdateSelectItem();
			UpdateSelectItemPic();
		}
	}

	public void UpdateSelectItemPic()
	{
		if (currentSelectItem != null)
		{
			ItemUILogic itemObjByItemIndex = ShowBackPackLogicScript.GetItemObjByItemIndex(currentSelectItem.IndexId);
			if (itemObjByItemIndex != null)
			{
				NGUITools.SetActive(ChoosedPic.gameObject, state: true);
				ChoosedPic.transform.position = itemObjByItemIndex.transform.position;
			}
			else
			{
				NGUITools.SetActive(ChoosedPic.gameObject, state: false);
			}
			mCurChoosedObj = itemObjByItemIndex;
		}
		else
		{
			NGUITools.SetActive(ChoosedPic.gameObject, state: false);
			mCurChoosedObj = null;
		}
	}

	public void OnClickItem(GameItem item)
	{
		if (currentSelectItem == null || item.IndexId != currentSelectItem.IndexId)
		{
			currentSelectItem = item;
			ItemData itemData = item.ItemData;
			pricePerfent = 1f;
			UpdateSlider(1);
			UpdateSelectItem();
			UpdateSelectItemPic();
		}
		else if (currentSelectItem != null)
		{
			int level = 0;
			if (item.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(item.ItemData.SubType);
			}
			ItemInfoRootLogicNew.ShowItemTips(currentSelectItem, level);
		}
	}

	private void ChangeTab()
	{
		if (currentPage == 0)
		{
			tabEdgeSprite1.spriteName = "CZ_wuPinYanSe_3";
			tabEdgeSprite2.spriteName = "CZ_tongYongDi_zhuYao_4";
			tabEdgeSprite3.spriteName = "CZ_tongYongDi_zhuYao_4";
			tabEdgeState1.active = true;
			tabEdgeState2.active = false;
			tabEdgeState3.active = false;
		}
		else if (currentPage == 1)
		{
			tabEdgeSprite2.spriteName = "CZ_wuPinYanSe_3";
			tabEdgeSprite1.spriteName = "CZ_tongYongDi_zhuYao_4";
			tabEdgeSprite3.spriteName = "CZ_tongYongDi_zhuYao_4";
			tabEdgeState1.active = false;
			tabEdgeState2.active = true;
			tabEdgeState3.active = false;
		}
		else if (currentPage == 2)
		{
			tabEdgeSprite3.spriteName = "CZ_wuPinYanSe_3";
			tabEdgeSprite1.spriteName = "CZ_tongYongDi_zhuYao_4";
			tabEdgeSprite2.spriteName = "CZ_tongYongDi_zhuYao_4";
			tabEdgeState1.active = false;
			tabEdgeState2.active = false;
			tabEdgeState3.active = true;
		}
	}

	private void UpdateSelectTime()
	{
		if (selectTime == 0)
		{
			timeLabel1.active = true;
			timeLabel2.active = false;
			timeLabel3.active = false;
			timeSelectSprite.transform.position = timeLabel1.transform.position;
		}
		else if (selectTime == 1)
		{
			timeLabel1.active = false;
			timeLabel2.active = true;
			timeLabel3.active = false;
			timeSelectSprite.transform.position = timeLabel2.transform.position;
		}
		else if (selectTime == 2)
		{
			timeLabel1.active = false;
			timeLabel2.active = false;
			timeLabel3.active = true;
			timeSelectSprite.transform.position = timeLabel3.transform.position;
		}
	}

	public void SliderChange()
	{
		UpdateSelectItem();
	}

	private void UpdateSelectItem()
	{
		if (currentSelectItem != null && !currentSelectItem.IsEmpty())
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(currentSelectItem.ItemId);
			selectSpriteNameLabel.text = itemDataByID.MName;
			selectSprite.spriteName = itemDataByID.BackPackIcon;
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				selectSpriteQuality.spriteName = currentSelectItem.GetItemQuality().ToString();
			}
			else
			{
				selectSpriteQuality.spriteName = itemDataByID.QualityType.ToString();
			}
			sellCount = (int)Mathf.Round(sellCountSlider.value * (float)currentSelectItem.StackNum);
			sellCountLabel.text = sellCount.ToString();
			singelPrice = (int)((float)itemDataByID.ConsignPrice * pricePerfent + 0.5f);
			singelPriceLabel.text = singelPrice.ToString();
			finalPrice = singelPrice * sellCount;
			finalPriceLabel.text = finalPrice.ToString();
			SellCommission = commisions[selectTime];
			SellCommissionLabel.text = SellCommission.ToString();
			if (sellCount > 0)
			{
				SellBtn.spriteName = GameDefine.BtnIcon[1];
			}
			else
			{
				SellBtn.spriteName = GameDefine.BtnIcon[2];
			}
			priceBtnAdd.isEnabled = true;
			priceBtnSub.isEnabled = true;
		}
		else
		{
			sellCount = 0;
			selectSprite.spriteName = "CZ_A_beiBao_ZBKD";
			selectSpriteQuality.spriteName = string.Empty;
			selectSpriteNameLabel.text = string.Empty;
			sellCountSlider.value = 0f;
			singelPrice = 0;
			singelPriceLabel.text = singelPrice.ToString();
			finalPrice = singelPrice * sellCount;
			finalPriceLabel.text = finalPrice.ToString();
			SellCommissionLabel.text = "0";
			SellBtn.spriteName = GameDefine.BtnIcon[2];
			priceBtnAdd.isEnabled = true;
			priceBtnSub.isEnabled = true;
			sellCountLabel.text = sellCount.ToString();
		}
	}

	public void Reset(int subIndex = 0)
	{
		if (playerData == null)
		{
			playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		}
		currentPage = subIndex;
		UpdateInfo();
		ChangeTab();
	}

	public void UpdateInfo(bool isNeedResetPos = true)
	{
		if (currentPage == 0)
		{
			ItemContainer itemContainer = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(ITEM_CONTAINER_TYPE.EQUIP_BACKPACK);
			List<GameItem> consignSellItem = ItemContainerTool.GetConsignSellItem(itemContainer);
			if (consignSellItem.Count > 0)
			{
				currentSelectItem = consignSellItem[0];
			}
			else
			{
				currentSelectItem = null;
			}
			ShowBackPackLogicScript.ShowList(consignSellItem, isNeedResetPos);
		}
		else if (currentPage == 1)
		{
			ItemContainer itemContainer2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(ITEM_CONTAINER_TYPE.BADGE_BACKPACK);
			List<GameItem> consignSellItem2 = ItemContainerTool.GetConsignSellItem(itemContainer2);
			if (consignSellItem2.Count > 0)
			{
				currentSelectItem = consignSellItem2[0];
			}
			else
			{
				currentSelectItem = null;
			}
			ShowBackPackLogicScript.ShowList(consignSellItem2, isNeedResetPos);
		}
		else
		{
			ItemContainer itemContainer3 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(ITEM_CONTAINER_TYPE.ITEM_BACKPACK);
			List<GameItem> consignSellItem3 = ItemContainerTool.GetConsignSellItem(itemContainer3);
			if (consignSellItem3.Count > 0)
			{
				currentSelectItem = consignSellItem3[0];
			}
			else
			{
				currentSelectItem = null;
			}
			ShowBackPackLogicScript.ShowList(consignSellItem3, isNeedResetPos);
		}
		UpdateSlider(1);
		UpdateSelectItem();
		UpdateSelectTime();
		UpdateSelectItemPic();
	}

	public void ClickTab1()
	{
		if (currentPage != 0)
		{
			currentSelectItem = null;
			currentPage = 0;
			pricePerfent = 1f;
			UpdateInfo();
		}
		ChangeTab();
	}

	public void ClickTab2()
	{
		if (currentPage != 1)
		{
			currentSelectItem = null;
			pricePerfent = 1f;
			currentPage = 1;
			UpdateInfo();
		}
		ChangeTab();
	}

	public void ClickTab3()
	{
		if (currentPage != 2)
		{
			currentSelectItem = null;
			pricePerfent = 1f;
			currentPage = 2;
			UpdateInfo();
		}
		ChangeTab();
	}

	public void ClickTime1()
	{
		if (selectTime != 0)
		{
			selectTime = 0;
			UpdateSelectTime();
			UpdateSelectItem();
		}
	}

	public void ClickTime2()
	{
		if (selectTime != 1)
		{
			selectTime = 1;
			UpdateSelectTime();
			UpdateSelectItem();
		}
	}

	public void ClickTime3()
	{
		if (selectTime != 2)
		{
			selectTime = 2;
			UpdateSelectTime();
			UpdateSelectItem();
		}
	}

	public void ClickMax()
	{
		if (currentSelectItem != null)
		{
			sellCountSlider.value = 1f;
			UpdateSelectItem();
		}
	}

	public void ClickAddPrice()
	{
		if (currentSelectItem == null)
		{
			return;
		}
		ItemData itemDataByID = DataManager.GetItemDataByID(currentSelectItem.ItemId);
		pricePerfent += 0.02f;
		int num = (int)((float)itemDataByID.ConsignPrice * pricePerfent + 0.5f);
		if (num == singelPrice)
		{
			float num2 = (float)(num + 1) / (float)itemDataByID.ConsignPrice;
			if (num2 > 1.5f)
			{
				pricePerfent = 1.5f;
			}
			else
			{
				pricePerfent = num2;
			}
		}
		if (pricePerfent > 1.5f)
		{
			priceBtnAdd.isEnabled = false;
			pricePerfent = 1.5f;
			NoticeLogic.AddNotifyData("#{101247}");
		}
		UpdateSelectItem();
		if (!priceBtnSub.isEnabled)
		{
			priceBtnSub.isEnabled = true;
		}
	}

	public void ClickSubPrice()
	{
		if (currentSelectItem == null)
		{
			return;
		}
		ItemData itemDataByID = DataManager.GetItemDataByID(currentSelectItem.ItemId);
		pricePerfent -= 0.02f;
		int num = (int)((float)itemDataByID.ConsignPrice * pricePerfent + 0.5f);
		if (num == singelPrice)
		{
			float num2 = (float)(num - 1) / (float)itemDataByID.ConsignPrice;
			if (num2 < 0.5f)
			{
				pricePerfent = 0.5f;
			}
			else
			{
				pricePerfent = num2;
			}
		}
		if (pricePerfent < 0.5f)
		{
			priceBtnSub.isEnabled = false;
			pricePerfent = 0.5f;
			NoticeLogic.AddNotifyData("#{101248}");
		}
		UpdateSelectItem();
		if (!priceBtnAdd.isEnabled)
		{
			priceBtnAdd.isEnabled = true;
		}
	}

	public void ShowItemList(List<GameItem> list, bool isNeedResetPos = true)
	{
	}

	public void ClickSell()
	{
		if (sellCount > 0 && GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.CASH, SellCommission))
		{
			consign_sale_item.request request = new consign_sale_item.request();
			request.indexId = currentSelectItem.IndexId;
			request.itemCount = sellCount;
			request.price = singelPrice;
			request.timeType = selectTime;
			request.itemType = currentSelectItem.ItemData.ItemType;
			NetLogic.GetInstance().Send<Protocol.consign_sale_item>(request);
		}
	}
}
