using System;
using UnityEngine;

public class BackPackPageRootLogic : SingletonUnity<BackPackPageRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public Transform ScaleUIRoot;

	private BackPackRootLogic mBackPackRoot;

	private int mItemInfoLeftOffsetPos = -180;

	private int mItemInfoRightOffsetPos = 136;

	private bool mInitFlag;

	private ITEM_CONTAINER_TYPE mCurContainerType;

	private ITEM_SHOW_TYPE mCurItemShowType;

	public BackPackRootLogic BackPackRoot
	{
		get
		{
			if (mBackPackRoot == null)
			{
				SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.BackPackPageRootItem, OnLoadBackPackPageRoot);
			}
			return mBackPackRoot;
		}
	}

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	private void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	protected override void Awake()
	{
		base.Awake();
		Init();
	}

	private void Init()
	{
		if (!mInitFlag)
		{
			mInitFlag = true;
			if (mBackPackRoot == null)
			{
				SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.BackPackPageRootItem, OnLoadBackPackPageRoot);
			}
		}
	}

	private void OnLoadBackPackPageRoot(GameObject newObj, object param)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(newObj) as GameObject;
		gameObject.transform.parent = ScaleUIRoot;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		mBackPackRoot = gameObject.GetComponent<BackPackRootLogic>();
		BackPackRootLogic backPackRootLogic = mBackPackRoot;
		backPackRootLogic.onClickItem = (BackPackRootLogic.OnClickItemDelegate)Delegate.Combine(backPackRootLogic.onClickItem, new BackPackRootLogic.OnClickItemDelegate(OnClickBackPackItem));
		mBackPackRoot.OffsetRoot.localPosition = new Vector3(200f, 0f, 0f);
		mBackPackRoot.InitCanSell(canSell: true);
	}

	public void Reset(bool needResetPackPos, ITEM_CONTAINER_TYPE containerType)
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.BADGE_CLICK_ITEM)
		{
			FunctionTipsRootLogic.ClearHandTip();
		}
		mCurContainerType = containerType;
		if (BackPackRoot != null)
		{
			BackPackRoot.Show(needResetPackPos, mCurContainerType);
		}
	}

	private bool IsRootActive()
	{
		return UnityVersionUtil.IsActive(base.gameObject);
	}

	private bool IsBackPackRootActive()
	{
		if (BackPackRoot == null)
		{
			return false;
		}
		return UnityVersionUtil.IsActive(BackPackRoot.gameObject);
	}

	private bool IsEquipPackRootActive()
	{
		if (SingletonUnity<PlayerModelPageRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PlayerModelPageRootLogic>.Instance.gameObject))
		{
			return true;
		}
		return false;
	}

	public void ResetBackPack()
	{
		if (IsRootActive() && IsBackPackRootActive())
		{
			BackPackRoot.UpdateBackPack();
		}
	}

	public void OnClickBackPackItem(GameItem item)
	{
		ShowItemInfo(item, ITEM_SHOW_TYPE.BACKPACK, mItemInfoLeftOffsetPos, 0f);
	}

	public void ShowItemInfo(GameItem item, ITEM_SHOW_TYPE showType, float xOffset, float yOffset)
	{
		if (item.ItemId.Equals(GameDefine.EmptyAddItemID))
		{
			if (mCurContainerType == ITEM_CONTAINER_TYPE.EQUIP_BACKPACK)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100659}"));
			}
			else if (mCurContainerType == ITEM_CONTAINER_TYPE.FASHION_BACKPACK)
			{
				if (!CheckUnlockFunction(FUNCTION_TYPE.SHOP_TOOL))
				{
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100662}"));
					return;
				}
				SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
				{
					SingletonUnity<ShopUIRootLogic>.Instance.OnClickBigSaleBtn(GameDefine.SHOP_TAB_TYPE.FASHION, GameDefine.UIBACKTYPE.FASHION);
				});
			}
			else if (mCurContainerType == ITEM_CONTAINER_TYPE.BADGE_BACKPACK)
			{
				if (!CheckUnlockFunction(FUNCTION_TYPE.SHOP_TOOL))
				{
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100661}"));
					return;
				}
				SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
				{
					SingletonUnity<ShopUIRootLogic>.Instance.OnClickToolsBtn(GameDefine.SHOP_TAB_TYPE.ITEM, GameDefine.UIBACKTYPE.BADGE, "9602");
				});
			}
			else if (mCurContainerType == ITEM_CONTAINER_TYPE.ITEM_BACKPACK)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100426}"));
			}
			return;
		}
		if (item.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			EquipData equipDataById = DataManager.GetEquipDataById(item.ItemData.ID);
			if (!item.IsAppraise)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRootNew, delegate
				{
					SingletonUnity<ItemInfoRootLogicNew>.Instance.ResetAppraise(item);
				});
				return;
			}
			if (item.ItemData.SubType == 0 || equipDataById.profession == SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession)
			{
				PlayerModelPageRootLogic curModelPage = SingletonUnity<PlayerModelPageRootLogic>.Instance;
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRootNew, delegate
				{
					int equipEnhanceLevel3 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(item.ItemData.SubType);
					if (item != null)
					{
						item.ItemLevel = equipEnhanceLevel3;
					}
					GameItem targetTypeEquip = curModelPage.GetTargetTypeEquip((EQUIP_BACKPACK_TYPE)item.ItemData.SubType);
					if (targetTypeEquip != null && !targetTypeEquip.IsEmpty())
					{
						targetTypeEquip.ItemLevel = equipEnhanceLevel3;
					}
					SingletonUnity<ItemInfoRootLogicNew>.Instance.ResetCompareEquip(item, targetTypeEquip);
					if (showType == ITEM_SHOW_TYPE.BACKPACK)
					{
						CheckTutorialEvent();
					}
				});
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRootNew, delegate
			{
				int equipEnhanceLevel2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(item.ItemData.SubType);
				if (item != null)
				{
					item.ItemLevel = equipEnhanceLevel2;
				}
				SingletonUnity<ItemInfoRootLogicNew>.Instance.ResetCompareEquip(item, null);
			});
			return;
		}
		if (item.ItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRootNew, delegate
			{
				SingletonUnity<ItemInfoRootLogicNew>.Instance.Reset(item, ITEM_SHOW_TYPE.BACKPACK);
			});
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRootNew, delegate
		{
			if (item.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				int equipEnhanceLevel = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(item.ItemData.SubType);
				item.ItemLevel = equipEnhanceLevel;
			}
			if (!string.IsNullOrEmpty(item.ItemData.JumpPath))
			{
				SingletonUnity<ItemInfoRootLogicNew>.Instance.Reset(item, showType, isNeedShowJumpPath: true, UI_PAGE_TYPE.BACK_PACK_ITEM);
			}
			else
			{
				SingletonUnity<ItemInfoRootLogicNew>.Instance.Reset(item, showType);
			}
			if (showType == ITEM_SHOW_TYPE.BACKPACK)
			{
				CheckTutorialEvent();
			}
		});
	}

	public bool CheckUnlockFunction(FUNCTION_TYPE curFunction)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData.IsFunctionUnlock(curFunction))
		{
			return true;
		}
		return false;
	}

	public void OnCloseItemInfo()
	{
		Reset(needResetPackPos: false, mCurContainerType);
		if (SingletonUnity<PlayerModelPageRootLogic>.Exists && !UnityVersionUtil.IsActive(SingletonUnity<PlayerModelPageRootLogic>.Instance.gameObject))
		{
			if (mCurContainerType == ITEM_CONTAINER_TYPE.ITEM_BACKPACK || mCurContainerType == ITEM_CONTAINER_TYPE.EQUIP_BACKPACK)
			{
				SingletonUnity<PlayerModelPageRootLogic>.Instance.ReShow(ItemContainerTool.GetEquipItemList(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipPack));
			}
			else if (mCurContainerType == ITEM_CONTAINER_TYPE.BADGE_BACKPACK)
			{
				SingletonUnity<PlayerModelPageRootLogic>.Instance.ReShow(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BadgeEquipPack.ItemList);
			}
			SingletonUnity<PlayerInfoMenuRootLogic>.Instance.ResetModelVisual();
		}
	}
}
