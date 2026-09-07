using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class PlayerModelPageRootLogic : SingletonUnity<PlayerModelPageRootLogic>
{
	public delegate void OnClickItemDelegate(GameItem item);

	public OnClickItemDelegate onClickItem;

	public bool CanClickItemFlag;

	public bool CanShowItemFlag;

	public UIEventListener RotateModelBtnListener;

	public List<ItemUILogic> ItemShowList;

	public List<UILabel> BadgeAttributeName;

	public List<UISprite> BadgeAttributeICON;

	public List<UILabel> BadgeAttributeValue;

	public List<GameObject> BadgeAttributeObj;

	private List<GameItem> mCurItemDataList;

	private PROFESSION_TYPE TargetProfession;

	private string TargetModelName;

	private FakeObjLogic mCurFakeObj;

	public UILabel TopLabel;

	public UILabel BottomLabel;

	public UITexture ModelPic;

	public GameObject ShowFashionBtn;

	public UISprite ShowFashionSprite;

	public UILabel ShowFashionLabel;

	public GameObject BadgeObj;

	public GameObject ComboObj;

	private Vector3 mModelComboObjPos = new Vector3(64.5f, -183f, 0f);

	private Vector3 mBadgeComboObjPos = new Vector3(64.5f, -69f, 0f);

	public UIGrid BadgeGrid;

	private Vector3[] mEquipPackPosList = new Vector3[6]
	{
		new Vector3(187f, 88f, 0f),
		new Vector3(-51f, 0f, 0f),
		new Vector3(187f, 0f, 0f),
		new Vector3(-51f, -88f, 0f),
		new Vector3(187f, -88f, 0f),
		new Vector3(-51f, 88f, 0f)
	};

	private Vector3[] mFashionPackPosList = new Vector3[4]
	{
		new Vector3(-44f, 44f, 0f),
		new Vector3(175f, 44f, 0f),
		new Vector3(-44f, -44f, 0f),
		new Vector3(175f, -44f, 0f)
	};

	private Vector3[] mBadgePackPosList = new Vector3[5]
	{
		new Vector3(60f, 150f, 0f),
		new Vector3(150f, 75f, 0f),
		new Vector3(120f, -14f, 0f),
		new Vector3(-3f, -14f, 0f),
		new Vector3(-29f, 75f, 0f)
	};

	private EQUIP_PACK_TYPE mCurEquipPackType;

	private bool mInitFlag;

	public GameObject[] IconEffect;

	public UISprite[] ItemsEffectBgs;

	public Transform effectParent;

	private void Start()
	{
		if (!mInitFlag)
		{
			UIEventListener rotateModelBtnListener = RotateModelBtnListener;
			rotateModelBtnListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(rotateModelBtnListener.onDrag, new UIEventListener.VectorDelegate(OnDragModelBtn));
			mInitFlag = true;
			Init();
		}
	}

	private void Init()
	{
		for (int i = 0; i < ItemShowList.Count; i++)
		{
			ItemUILogic itemUILogic = ItemShowList[i];
			itemUILogic.onClickItem = (ItemUILogic.OnClickItemDelegate)Delegate.Combine(itemUILogic.onClickItem, new ItemUILogic.OnClickItemDelegate(OnClickItem));
		}
	}

	private void UpdateFahsionFlag(bool isShow)
	{
		if (isShow)
		{
			ShowFashionSprite.spriteName = "CZ_shiZhuang_XianShi";
			ShowFashionLabel.text = StrDictionary.GetDictionaryString("close");
		}
		else
		{
			ShowFashionSprite.spriteName = "CZ_shiZhuang_BuXianShi";
			ShowFashionLabel.text = StrDictionary.GetDictionaryString("open");
		}
	}

	public void Hide()
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(mCurFakeObj.FakeObj, state: false);
	}

	public void Show(FakeObjLogic curFakeObj, List<GameItem> curItemList, string topStr, string bottomStr, bool canShowItem, bool canClickItem, EQUIP_PACK_TYPE curType)
	{
		if (!UnityVersionUtil.IsActive(base.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		}
		UnityVersionUtil.SetActiveRecursive(curFakeObj.FakeObj.gameObject, curType != EQUIP_PACK_TYPE.BADGE);
		Reset(curFakeObj, curItemList, topStr, bottomStr, canShowItem, canClickItem, curType);
	}

	public void ReShow(List<GameItem> itemList)
	{
		if (!UnityVersionUtil.IsActive(base.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		}
		if (CanShowItemFlag)
		{
			UpdateEquipPack(itemList);
		}
		else
		{
			HideEquipPack();
		}
	}

	public void ResetOnClickItem(OnClickItemDelegate func)
	{
		onClickItem = func;
	}

	public void UpdateComboValue(int value)
	{
		if (mCurEquipPackType != EQUIP_PACK_TYPE.BADGE)
		{
			BottomLabel.text = value.ToString();
		}
	}

	public static void UpdateCombo(int value)
	{
		if (SingletonUnity<PlayerModelPageRootLogic>.Exists)
		{
			SingletonUnity<PlayerModelPageRootLogic>.Instance.UpdateComboValue(value);
		}
	}

	public void Reset(FakeObjLogic curFakeObj, List<GameItem> curItemList, string topStr, string bottomStr, bool canShowItem, bool canClickItem, EQUIP_PACK_TYPE curType)
	{
		TopLabel.text = topStr;
		BottomLabel.text = bottomStr;
		mCurFakeObj = curFakeObj;
		mCurItemDataList = curItemList;
		CanShowItemFlag = canShowItem;
		CanClickItemFlag = canClickItem;
		mCurEquipPackType = curType;
		if (UnityVersionUtil.IsActive(curFakeObj.FakeObj.gameObject) != (curType != EQUIP_PACK_TYPE.BADGE))
		{
			UnityVersionUtil.SetActiveRecursive(curFakeObj.FakeObj.gameObject, curType != EQUIP_PACK_TYPE.BADGE);
		}
		if (CanShowItemFlag)
		{
			UpdateEquipPack(mCurItemDataList);
		}
		else
		{
			HideEquipPack();
		}
	}

	public GameItem GetTargetTypeEquip(EQUIP_BACKPACK_TYPE targetType)
	{
		if (mCurEquipPackType == EQUIP_PACK_TYPE.BACKPACK)
		{
			if (mCurItemDataList[ItemContainerTool.ChangeEquipTypeToIndex(targetType)].IsEmpty())
			{
				return null;
			}
			return mCurItemDataList[ItemContainerTool.ChangeEquipTypeToIndex(targetType)];
		}
		if (mCurEquipPackType == EQUIP_PACK_TYPE.FASHION)
		{
			if (mCurItemDataList[ItemContainerTool.ChangeFashionEquipTypeToIndex(targetType)].IsEmpty())
			{
				return null;
			}
			return mCurItemDataList[ItemContainerTool.ChangeFashionEquipTypeToIndex(targetType)];
		}
		return null;
	}

	public int GetTargetTypeEquipScore(EQUIP_BACKPACK_TYPE targetType)
	{
		if (mCurEquipPackType == EQUIP_PACK_TYPE.BACKPACK)
		{
			GameItem gameItem = mCurItemDataList[ItemContainerTool.ChangeEquipTypeToIndex(targetType)];
			if (gameItem == null || gameItem.IsEmpty())
			{
				return 0;
			}
			return gameItem.GetItemScore();
		}
		if (mCurEquipPackType == EQUIP_PACK_TYPE.FASHION)
		{
			GameItem gameItem2 = mCurItemDataList[ItemContainerTool.ChangeFashionEquipTypeToIndex(targetType)];
			if (gameItem2 == null || gameItem2.IsEmpty())
			{
				return 0;
			}
			return gameItem2.GetItemScore();
		}
		return 0;
	}

	public int GetTargetTypeEquipCombatVal(EQUIP_BACKPACK_TYPE targetType)
	{
		if (mCurEquipPackType == EQUIP_PACK_TYPE.BACKPACK)
		{
			GameItem gameItem = mCurItemDataList[ItemContainerTool.ChangeEquipTypeToIndex(targetType)];
			if (gameItem == null || gameItem.IsEmpty())
			{
				return 0;
			}
			return gameItem.GetItemCombatVal();
		}
		if (mCurEquipPackType == EQUIP_PACK_TYPE.FASHION)
		{
			GameItem gameItem2 = mCurItemDataList[ItemContainerTool.ChangeFashionEquipTypeToIndex(targetType)];
			if (gameItem2 == null || gameItem2.IsEmpty())
			{
				return 0;
			}
			return gameItem2.GetItemCombatVal();
		}
		return 0;
	}

	private void HideEquipPack()
	{
		for (int i = 0; i < ItemShowList.Count; i++)
		{
			UnityVersionUtil.SetActiveRecursive(ItemShowList[i].gameObject, state: false);
		}
	}

	private void UpdateBageItem()
	{
		int[] array = new int[3];
		int[] array2 = new int[3];
		int num = 1;
		int num2 = 0;
		for (int i = 0; i < mCurItemDataList.Count; i++)
		{
			GameItem gameItem = mCurItemDataList[i];
			if (gameItem != null && !gameItem.IsEmpty())
			{
				ItemData itemData = gameItem.ItemData;
				BadgeData badgeDataById = DataManager.GetBadgeDataById(gameItem.ItemId);
				array[badgeDataById.Color]++;
				array2[badgeDataById.Color] += badgeDataById.Lv;
				BadgeAttributeName[num].text = GameDefine.GetAttributeName_S(badgeDataById.Status1);
				BadgeAttributeICON[num].spriteName = GameDefine.GetAttributeIcon(badgeDataById.Status1);
				BadgeAttributeValue[num].text = GameDefine.GetAttributeValueStr(badgeDataById.Status1, badgeDataById.Value1);
				num2 += gameItem.GetItemCombatVal();
				num++;
			}
		}
		int num3 = num;
		for (int j = 0; j < BadgeAttributeObj.Count; j++)
		{
			NGUITools.SetActive(BadgeAttributeObj[j], j < num3);
		}
		int num4 = -1;
		int num5 = 0;
		for (int k = 0; k < array.Length; k++)
		{
			if (array[k] >= 3)
			{
				num4 = k;
				num5 = array2[k];
				break;
			}
		}
		List<int> list = new List<int>();
		if (num4 > -1)
		{
			for (int l = 0; l < mCurItemDataList.Count; l++)
			{
				GameItem gameItem2 = mCurItemDataList[l];
				if (gameItem2 != null && !gameItem2.IsEmpty())
				{
					ItemData itemData2 = gameItem2.ItemData;
					BadgeData badgeDataById2 = DataManager.GetBadgeDataById(gameItem2.ItemId);
					if (badgeDataById2.Color == num4)
					{
						list.Add(l);
					}
				}
			}
		}
		for (int m = 0; m < ItemShowList.Count; m++)
		{
			if (list.Contains(m))
			{
				ItemsEffectBgs[m].enabled = true;
			}
			else
			{
				ItemsEffectBgs[m].enabled = false;
			}
		}
		for (int n = 0; n < IconEffect.Length; n++)
		{
			if (n < list.Count)
			{
				IconEffect[n].transform.parent = ItemShowList[list[n]].transform;
				IconEffect[n].transform.localPosition = Vector3.zero;
				UnityVersionUtil.SetActiveRecursive(IconEffect[n], state: true);
			}
			else
			{
				IconEffect[n].transform.parent = effectParent;
				IconEffect[n].transform.localPosition = Vector3.zero;
				UnityVersionUtil.SetActiveRecursive(IconEffect[n], state: false);
			}
		}
		NGUITools.SetActive(BadgeAttributeObj[0], num4 > -1);
		switch (num4)
		{
		case 1:
		{
			ConfigData configDataByKey5 = DataManager.GetConfigDataByKey("badge_parm_2");
			ConfigData configDataByKey6 = DataManager.GetConfigDataByKey("badge_attribute_2");
			if (configDataByKey5 != null)
			{
				int value3 = Mathf.FloorToInt((float)num5 / configDataByKey5.Valuef);
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(configDataByKey6.Valuei);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(configDataByKey6.Valuei);
				BadgeAttributeValue[0].text = GameDefine.GetAttributeValueStr(configDataByKey6.Valuei, value3);
			}
			else
			{
				float num8 = Mathf.Pow(num5, 1.25f) / 3.94822f / 1000f;
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(1010);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(1010);
				BadgeAttributeValue[0].text = $"+{num8:P1}";
			}
			break;
		}
		case 0:
		{
			ConfigData configDataByKey3 = DataManager.GetConfigDataByKey("badge_parm_1");
			ConfigData configDataByKey4 = DataManager.GetConfigDataByKey("badge_attribute_1");
			if (configDataByKey3 != null)
			{
				int value2 = Mathf.FloorToInt((float)num5 / configDataByKey3.Valuef);
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(configDataByKey4.Valuei);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(configDataByKey4.Valuei);
				BadgeAttributeValue[0].text = GameDefine.GetAttributeValueStr(configDataByKey4.Valuei, value2);
			}
			else
			{
				float num7 = Mathf.Pow(num5, 0.5555f) / 1.8411f / 100f;
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(1012);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(1012);
				BadgeAttributeValue[0].text = $"+{num7:P1}";
			}
			break;
		}
		case 2:
		{
			ConfigData configDataByKey = DataManager.GetConfigDataByKey("badge_parm_3");
			ConfigData configDataByKey2 = DataManager.GetConfigDataByKey("badge_attribute_3");
			if (configDataByKey != null)
			{
				int value = Mathf.FloorToInt((float)num5 / configDataByKey.Valuef);
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(configDataByKey2.Valuei);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(configDataByKey2.Valuei);
				BadgeAttributeValue[0].text = GameDefine.GetAttributeValueStr(configDataByKey2.Valuei, value);
			}
			else
			{
				float num6 = Mathf.Pow(num5, 0.5f) / 1.73205f * 0.5f;
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(1011);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(1011);
				BadgeAttributeValue[0].text = $"+{num6:F1}";
			}
			break;
		}
		}
		BottomLabel.text = num2.ToString();
		BadgeGrid.Reposition();
	}

	public void CloseIconEffect()
	{
		for (int i = 0; i < ItemsEffectBgs.Length; i++)
		{
			ItemsEffectBgs[i].enabled = false;
			UnityVersionUtil.SetActiveRecursive(IconEffect[i], state: false);
		}
	}

	private void UpdateEquipPack(List<GameItem> itemList)
	{
		mCurItemDataList = itemList;
		Vector3[] array = null;
		if (mCurEquipPackType == EQUIP_PACK_TYPE.BACKPACK)
		{
			array = mEquipPackPosList;
		}
		else if (mCurEquipPackType == EQUIP_PACK_TYPE.FASHION)
		{
			array = mFashionPackPosList;
		}
		else if (mCurEquipPackType == EQUIP_PACK_TYPE.BADGE)
		{
			array = mBadgePackPosList;
		}
		for (int i = 0; i < ItemShowList.Count; i++)
		{
			if (i < mCurItemDataList.Count)
			{
				if (!UnityVersionUtil.IsActive(ItemShowList[i].gameObject))
				{
					UnityVersionUtil.SetActiveRecursive(ItemShowList[i].gameObject, state: true);
				}
				if (mCurItemDataList[i] != null && !mCurItemDataList[i].IsEmpty())
				{
					ItemShowList[i].UpdateItemUI(mCurItemDataList[i]);
				}
				else if (mCurEquipPackType == EQUIP_PACK_TYPE.BADGE)
				{
					ItemShowList[i].SetItemEmpty(ITEM_CONTAINER_TYPE.BADGE_EQUIPPACK);
				}
				else if (mCurEquipPackType == EQUIP_PACK_TYPE.BACKPACK)
				{
					ItemShowList[i].SetItemEmpty(ITEM_CONTAINER_TYPE.EQUIPPACK, ItemContainerTool.ChangeIndexToEquipType(i));
				}
				else if (mCurEquipPackType == EQUIP_PACK_TYPE.FASHION)
				{
					ItemShowList[i].SetItemEmpty(ITEM_CONTAINER_TYPE.EQUIPPACK, ItemContainerTool.ChangeIndexToFashionEquipType(i));
				}
				ItemShowList[i].transform.localPosition = array[i];
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(ItemShowList[i].gameObject, state: false);
			}
		}
		CloseIconEffect();
		if (mCurEquipPackType == EQUIP_PACK_TYPE.BACKPACK)
		{
			NGUITools.SetActive(ShowFashionBtn, state: false);
			NGUITools.SetActive(BadgeObj, state: false);
			NGUITools.SetActive(ComboObj, state: true);
			ComboObj.transform.localPosition = mModelComboObjPos;
			TopLabel.enabled = true;
		}
		else if (mCurEquipPackType == EQUIP_PACK_TYPE.FASHION)
		{
			NGUITools.SetActive(ShowFashionBtn, state: true);
			NGUITools.SetActive(BadgeObj, state: false);
			NGUITools.SetActive(ComboObj, state: true);
			ComboObj.transform.localPosition = mModelComboObjPos;
			TopLabel.enabled = true;
			UpdateFahsionFlag(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsShowFashion);
		}
		else if (mCurEquipPackType == EQUIP_PACK_TYPE.BADGE)
		{
			NGUITools.SetActive(ShowFashionBtn, state: false);
			NGUITools.SetActive(BadgeObj, state: true);
			NGUITools.SetActive(ComboObj, state: true);
			ComboObj.transform.localPosition = mBadgeComboObjPos;
			TopLabel.enabled = false;
			UpdateBageItem();
		}
	}

	private void OnDragModelBtn(GameObject btn, Vector2 delta)
	{
		mCurFakeObj.FakeObj.transform.localEulerAngles -= delta.x * Vector3.up;
	}

	private void OnEnable()
	{
		if (SingletonUnity<FakeObjRootLogic>.Exists)
		{
			SingletonUnity<FakeObjRootLogic>.Instance.EnableFakeObjRoot();
			SingletonUnity<FakeObjRootLogic>.Instance.SetPicValue(0.65f);
			ModelPic.mainTexture = SingletonUnity<FakeObjRootLogic>.Instance.ModelPic;
		}
		for (int i = 0; i < ItemsEffectBgs.Length; i++)
		{
			ItemsEffectBgs[i].enabled = false;
		}
	}

	private void OnDisable()
	{
		if (SingletonUnity<FakeObjRootLogic>.Exists)
		{
			SingletonUnity<FakeObjRootLogic>.Instance.DisableFakeObjRoot();
		}
	}

	public void OnClicktishiBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
		{
			SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{100631}", "#{100632}", null);
		});
	}

	public void OnClickItem(GameItem item, ItemUILogic curUIItem)
	{
		if (item != null)
		{
			if (onClickItem != null)
			{
				onClickItem(item);
			}
		}
		else if (mCurEquipPackType == EQUIP_PACK_TYPE.BACKPACK)
		{
			for (int i = 0; i < ItemShowList.Count; i++)
			{
				if (curUIItem == ItemShowList[i])
				{
					switch (ItemContainerTool.ChangeIndexToEquipType(i))
					{
					case EQUIP_BACKPACK_TYPE.HEAD:
						NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100659}"));
						break;
					case EQUIP_BACKPACK_TYPE.BODY:
						NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100659}"));
						break;
					case EQUIP_BACKPACK_TYPE.LEG:
						NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100659}"));
						break;
					case EQUIP_BACKPACK_TYPE.BELT:
						NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100659}"));
						break;
					case EQUIP_BACKPACK_TYPE.NECKLACE:
						NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100660}"));
						break;
					default:
						Debug.Log("No Item");
						break;
					case EQUIP_BACKPACK_TYPE.WEAPON:
						break;
					}
				}
			}
		}
		else if (mCurEquipPackType == EQUIP_PACK_TYPE.FASHION)
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
		else
		{
			if (mCurEquipPackType != EQUIP_PACK_TYPE.BADGE)
			{
				return;
			}
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

	public void OnClickShowFashionBtn()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.CanSendToServer(221))
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			playerData.IsShowFashion = !playerData.IsShowFashion;
			UpdateFahsionFlag(playerData.IsShowFashion);
			change_show_type.request request = new change_show_type.request();
			request.showType = (playerData.IsShowFashion ? 1 : 0);
			NetLogic.GetInstance().Send<Protocol.change_show_type>(request);
			SingletonUnity<PlayerInfoMenuRootLogic>.Instance.ResetModelVisual();
		}
		else
		{
			NoticeLogic.AddNotifyData("#{100272}");
		}
	}

	public int GetBadgeEquipCombatVal()
	{
		int num = 0;
		BadgeData badgeData = null;
		for (int i = 0; i < mCurItemDataList.Count; i++)
		{
			if (mCurItemDataList[i] != null && !mCurItemDataList[i].IsEmpty())
			{
				num += mCurItemDataList[i].GetItemCombatVal();
			}
		}
		return num;
	}
}
