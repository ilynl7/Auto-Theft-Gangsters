using System;
using System.Collections.Generic;
using UnityEngine;

public class BackPackRootLogic : MonoBehaviour
{
	public enum BACKPACK_TAP_PAGE
	{
		BACKPACK_ALL_PAGE,
		BACKPACK_EQUIP_PAGE,
		BACKPACK_ITEM_PAGE,
		BACKPACK_Part_PAGE
	}

	public delegate void OnClickItemDelegate(GameItem item);

	public delegate void OnItemLineResetDelegate(ItemLineLogic itemObj);

	public OnClickItemDelegate onClickItem;

	public OnItemLineResetDelegate onItemLineReset;

	public UISprite[] TopTabBtnPic;

	public List<ItemLineLogic> ItemLineList = new List<ItemLineLogic>();

	public Transform OffsetRoot;

	private BACKPACK_TAP_PAGE mCurTapPage;

	private List<GameItem> mCurItemList;

	private bool mInitFlag;

	public UIWrapContentNew uiWrapContent;

	public UIScrollView scrollView;

	public UIWidget BottomWidget;

	public GameObject RecycleBtnRoot;

	public UILabel RecycleLabel;

	public GameObject SellBtnRoot;

	public UISprite SellBtnPic;

	private bool mIsInSellMode;

	private bool mCanSellItem;

	private int MAX_SELL_NUM = 20;

	private List<GameItem> mChoosedSellItemList = new List<GameItem>();

	private ItemContainer mCurContainner;

	private string disableBtnPic = "CZ_anNiu_2+";

	private string enableBtnPic = "CZ_anNiu_2";

	public bool IsInSellMode => mIsInSellMode;

	public bool CanSellItem => mCanSellItem;

	public ItemContainer CurContainner => mCurContainner;

	private void Awake()
	{
		if (!mInitFlag)
		{
			mInitFlag = true;
			Init();
		}
	}

	private void Init()
	{
		UIWrapContentNew uIWrapContentNew = uiWrapContent;
		uIWrapContentNew.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContentNew.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
		for (int i = 0; i < ItemLineList.Count; i++)
		{
			ItemLineLogic itemLineLogic = ItemLineList[i];
			itemLineLogic.onClickItem = (ItemUILogic.OnClickItemDelegate)Delegate.Combine(itemLineLogic.onClickItem, new ItemUILogic.OnClickItemDelegate(OnClickItem));
		}
	}

	public void InitCanSell(bool canSell)
	{
		mCanSellItem = canSell;
	}

	public void Hide()
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
	}

	public void ShowList(List<GameItem> list, bool needResetPos)
	{
		mCurTapPage = BACKPACK_TAP_PAGE.BACKPACK_Part_PAGE;
		ShowItem(list, needResetPos);
		if (needResetPos)
		{
			scrollView.ResetPosition();
		}
	}

	public void Show(bool needResetPackPos, ITEM_CONTAINER_TYPE containerType)
	{
		mCurContainner = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(containerType);
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		NGUITools.SetActive(SellBtnRoot, state: false);
		if (containerType != 0 || !mCanSellItem)
		{
			NGUITools.SetActive(RecycleBtnRoot, state: false);
		}
		else
		{
			NGUITools.SetActive(RecycleBtnRoot, state: true);
		}
		mIsInSellMode = false;
		if (needResetPackPos)
		{
			Reset();
		}
		else
		{
			UpdateBackPack();
		}
	}

	public ItemUILogic GetItemObjByItemIndex(long indexId)
	{
		for (int i = 0; i < ItemLineList.Count; i++)
		{
			for (int j = 0; j < ItemLineList[i].ItemObjList.Count; j++)
			{
				if (ItemLineList[i].ItemObjList[j].curItem != null && ItemLineList[i].ItemObjList[j].curItem.IndexId == indexId)
				{
					return ItemLineList[i].ItemObjList[j];
				}
			}
		}
		return null;
	}

	private void OnEnable()
	{
		UIUpdateEvent.UpdateBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateBackPack));
	}

	private void OnDisable()
	{
		UIUpdateEvent.UpdateBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateBackPack));
	}

	public void UpdateBackPack()
	{
		switch (mCurTapPage)
		{
		case BACKPACK_TAP_PAGE.BACKPACK_ALL_PAGE:
			ShowAllItem(needResetPos: false);
			break;
		case BACKPACK_TAP_PAGE.BACKPACK_EQUIP_PAGE:
			ShowEquipItem(needResetPos: false);
			break;
		case BACKPACK_TAP_PAGE.BACKPACK_ITEM_PAGE:
			ShowItemItem(needResetPos: false);
			break;
		}
		ResetTopTapBtn();
	}

	private void Reset()
	{
		mCurTapPage = BACKPACK_TAP_PAGE.BACKPACK_ALL_PAGE;
		OnClickShowAllItem();
	}

	public void ResetTopTapBtn()
	{
		for (int i = 0; i < TopTabBtnPic.Length; i++)
		{
			if (i == (int)mCurTapPage)
			{
				TopTabBtnPic[i].color = Color.yellow;
			}
			else
			{
				TopTabBtnPic[i].color = Color.white;
			}
		}
	}

	public void OnClickShowAllItem()
	{
		mCurTapPage = BACKPACK_TAP_PAGE.BACKPACK_ALL_PAGE;
		ShowAllItem(needResetPos: true);
		ResetTopTapBtn();
		scrollView.ResetPosition();
	}

	public void OnClickShowEquipItem()
	{
		mCurTapPage = BACKPACK_TAP_PAGE.BACKPACK_EQUIP_PAGE;
		ShowEquipItem(needResetPos: true);
		ResetTopTapBtn();
		scrollView.ResetPosition();
	}

	public void OnClickShowItemBtn()
	{
		mCurTapPage = BACKPACK_TAP_PAGE.BACKPACK_ITEM_PAGE;
		ShowItemItem(needResetPos: true);
		ResetTopTapBtn();
		scrollView.ResetPosition();
	}

	private void ShowAllItem(bool needResetPos)
	{
		List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(mCurContainner, IsAll: true);
		if (GameManager.IsSupportCurDataVersion77() && (mCurContainner.ContainType == ITEM_CONTAINER_TYPE.BADGE_BACKPACK || mCurContainner.ContainType == ITEM_CONTAINER_TYPE.EQUIP_BACKPACK || mCurContainner.ContainType == ITEM_CONTAINER_TYPE.FASHION_BACKPACK || mCurContainner.ContainType == ITEM_CONTAINER_TYPE.ITEM_BACKPACK) && !mCurContainner.IsFull())
		{
			GameItem gameItem = new GameItem();
			gameItem.SetAddItem();
			targetTypeItem.Add(gameItem);
		}
		if (targetTypeItem != null)
		{
			ShowItem(targetTypeItem, needResetPos);
		}
	}

	private void ShowEquipItem(bool needResetPos)
	{
		List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(mCurContainner, IsAll: false, GameDefine.ITEM_TYPE.EQUIP);
		if (targetTypeItem != null)
		{
			ShowItem(targetTypeItem, needResetPos);
		}
	}

	private void ShowItemItem(bool needResetPos)
	{
		List<GameItem> otherTypeItem = ItemContainerTool.GetOtherTypeItem(mCurContainner, GameDefine.ITEM_TYPE.EQUIP);
		if (otherTypeItem != null)
		{
			ShowItem(otherTypeItem, needResetPos);
		}
	}

	public void ShowItemPart(List<GameItem> itemList, BACKPACK_TAP_PAGE type = BACKPACK_TAP_PAGE.BACKPACK_Part_PAGE, bool needResetPos = true)
	{
		mCurTapPage = type;
		ShowItem(itemList, needResetPos);
	}

	public void ShowItem(List<GameItem> itemList, bool needResetPos)
	{
		mCurItemList = itemList;
		if (mCurTapPage == BACKPACK_TAP_PAGE.BACKPACK_ALL_PAGE)
		{
			uiWrapContent.minIndex = -19;
			BottomWidget.height = uiWrapContent.itemSize * ItemContainer.BACKPACK_MAXSIZE / ItemLineList[0].ItemObjList.Count;
		}
		else
		{
			if (mCurItemList.Count > 0)
			{
				uiWrapContent.minIndex = -((mCurItemList.Count + ItemLineList[0].ItemObjList.Count - 1) / ItemLineList[0].ItemObjList.Count - 1);
			}
			else
			{
				uiWrapContent.minIndex = 0;
			}
			BottomWidget.height = uiWrapContent.itemSize * ((mCurItemList.Count + ItemLineList[0].ItemObjList.Count - 1) / ItemLineList[0].ItemObjList.Count);
		}
		if (needResetPos)
		{
			uiWrapContent.SortBasedOnScrollMovement();
		}
		else
		{
			ReShowItemList();
		}
		mIsInSellMode = false;
		UpdateSellMode();
	}

	private void UpdateSellMode()
	{
		if (mChoosedSellItemList.Count > 0)
		{
			for (int i = 0; i < ItemLineList.Count; i++)
			{
				for (int j = 0; j < ItemLineList[i].ItemObjList.Count; j++)
				{
					ItemLineList[i].ItemObjList[j].SetSellChoose(active: false);
				}
			}
		}
		mChoosedSellItemList.Clear();
		if (!CanSellItem)
		{
			return;
		}
		if (!mIsInSellMode)
		{
			NGUITools.SetActive(SellBtnRoot, state: false);
			RecycleLabel.text = StrDictionary.GetDictionaryString("#{100621}");
			return;
		}
		NGUITools.SetActive(SellBtnRoot, state: true);
		if (GameManager.IsSupportCurDataVersion())
		{
			RecycleLabel.text = StrDictionary.GetDictionaryString("#{100289}");
		}
		else
		{
			RecycleLabel.text = "Back";
		}
		SellBtnPic.spriteName = disableBtnPic;
	}

	public void OnClickRecycleBtn()
	{
		mIsInSellMode = !mIsInSellMode;
		UpdateSellMode();
	}

	public void OnClickSellBtn()
	{
		if (mChoosedSellItemList.Count > 0)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SellItemsRoot, delegate
			{
				SingletonUnity<SellItemsRootLogic>.Instance.ShowRewards(mChoosedSellItemList);
			});
		}
	}

	public void ReShowItemList()
	{
		for (int i = 0; i < ItemLineList.Count; i++)
		{
			if (mCurContainner != null)
			{
				ResetItemLine(ItemLineList[i], ItemLineList[i].CurIndex, mCurContainner.ContainerSize);
			}
			else
			{
				ResetItemLine(ItemLineList[i], ItemLineList[i].CurIndex, mCurItemList.Count);
			}
		}
	}

	public void OnClickClearUpBtn()
	{
		switch (mCurTapPage)
		{
		case BACKPACK_TAP_PAGE.BACKPACK_ALL_PAGE:
			ShowAllItem(needResetPos: true);
			break;
		case BACKPACK_TAP_PAGE.BACKPACK_EQUIP_PAGE:
			ShowEquipItem(needResetPos: true);
			break;
		case BACKPACK_TAP_PAGE.BACKPACK_ITEM_PAGE:
			ShowItemItem(needResetPos: true);
			break;
		}
		scrollView.ResetPosition();
	}

	public ItemUILogic GetFirstEquipItem()
	{
		for (int i = 0; i < ItemLineList.Count; i++)
		{
			for (int j = 0; j < ItemLineList[i].ItemObjList.Count; j++)
			{
				if (ItemLineList[i].ItemObjList[j].curItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					return ItemLineList[i].ItemObjList[j];
				}
			}
		}
		return null;
	}

	public void OnClickItem(GameItem item, ItemUILogic curUIItem)
	{
		if (item == null)
		{
			return;
		}
		if (CanSellItem && mIsInSellMode)
		{
			if (item.ItemId.Equals(GameDefine.EmptyAddItemID))
			{
				return;
			}
			if (mChoosedSellItemList.Contains(item))
			{
				mChoosedSellItemList.Remove(item);
				curUIItem.SetSellChoose(active: false);
			}
			else if (mChoosedSellItemList.Count < MAX_SELL_NUM)
			{
				mChoosedSellItemList.Add(item);
				curUIItem.SetSellChoose(active: true);
			}
			else if (GameManager.IsSupportCurDataVersion47())
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100654}", MAX_SELL_NUM));
			}
			if (mChoosedSellItemList.Count > 0)
			{
				if (!SellBtnPic.spriteName.Equals(enableBtnPic))
				{
					SellBtnPic.spriteName = enableBtnPic;
				}
			}
			else if (!SellBtnPic.spriteName.Equals(disableBtnPic))
			{
				SellBtnPic.spriteName = disableBtnPic;
			}
		}
		else if (onClickItem != null)
		{
			onClickItem(item);
		}
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		ItemLineLogic curLine = ItemLineList[index];
		if (mCurContainner == null)
		{
			ResetItemLine(curLine, Mathf.Abs(realIndex), mCurItemList.Count);
		}
		else
		{
			ResetItemLine(curLine, Mathf.Abs(realIndex), mCurContainner.ContainerSize);
		}
	}

	private void ResetItemLine(ItemLineLogic curLine, int realIndex, int maxIndex)
	{
		int num = realIndex * ItemLineList[0].ItemObjList.Count;
		List<GameItem> list = new List<GameItem>();
		list.Clear();
		for (int i = 0; i < curLine.ItemObjList.Count; i++)
		{
			if (i + num < mCurItemList.Count)
			{
				list.Add(mCurItemList[i + num]);
			}
			else
			{
				list.Add(null);
			}
		}
		if (mCurTapPage == BACKPACK_TAP_PAGE.BACKPACK_ALL_PAGE)
		{
			curLine.Reset(list, needShowEmpty: true, realIndex, num, maxIndex);
		}
		else
		{
			curLine.Reset(list, needShowEmpty: false, realIndex, num, maxIndex);
		}
		if (onItemLineReset != null)
		{
			onItemLineReset(curLine);
		}
		if (mIsInSellMode)
		{
			for (int j = 0; j < curLine.ItemObjList.Count; j++)
			{
				if (mChoosedSellItemList.Contains(curLine.ItemObjList[j].curItem))
				{
					curLine.ItemObjList[j].SetSellChoose(active: true);
				}
				else
				{
					curLine.ItemObjList[j].SetSellChoose(active: false);
				}
			}
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.BADGE_CLICK_ITEM)
		{
			FunctionTipsRootLogic.ClearHandTip();
		}
	}

	private int GetMinCurLine()
	{
		int num = int.MaxValue;
		for (int i = 0; i < ItemLineList.Count; i++)
		{
			if (num > ItemLineList[i].CurIndex)
			{
				num = ItemLineList[i].CurIndex;
			}
		}
		return num;
	}
}
