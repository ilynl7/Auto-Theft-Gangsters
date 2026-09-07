using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ItemInfoRootLogic : SingletonUnity<ItemInfoRootLogic>
{
	public const string equip_btn_str = "#{100614}";

	public const string un_equip_btn_str = "#{100615}";

	public const string use_str = "#{100623}";

	public const string useall_str = "#{300407}";

	public const string open_str = "#{300405}";

	public const string openall_str = "#{300406}";

	public const string recyle_str = "#{100621}";

	public const string trade_str = "#{100622}";

	public const string enhance_str = "#{100618}";

	public const string merge_str = "#{100616}";

	public const string split_str = "#{100617}";

	public const string extra_str = "#{100623}";

	public DelegateDefine.NoParamDelegate onClose;

	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public float LeftPos;

	public float RightPos;

	public float CenterPos;

	public GameObject[] BtnList;

	public UILabel[] BtnLabelList;

	public UISprite[] BtnSpList;

	public UIGrid BtnGride;

	public ItemInfoSubRootLogic ItemInfoRoot;

	public GameObject ItemInfoObjRoot;

	public ItemInfoSubRootLogic EquipInfoRoot;

	public ItemInfoSubRootLogic ConsumeItemInfoRoot;

	private GameItem mCurItem;

	private DelegateDefine.OneGameItemParamDelegate[] mOnClickBtnList = new DelegateDefine.OneGameItemParamDelegate[4];

	private UI_PAGE_TYPE mPrePage = UI_PAGE_TYPE.INVALID;

	private JUMP_PATH[] mJumpPath = new JUMP_PATH[4];

	public GameItem CurItem => mCurItem;

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

	public void OnClickBtn1()
	{
		if (mOnClickBtnList[0] != null)
		{
			mOnClickBtnList[0](mCurItem);
		}
	}

	public void OnClickBtn2()
	{
		if (mOnClickBtnList[1] != null)
		{
			mOnClickBtnList[1](mCurItem);
		}
	}

	public void OnClickBtn3()
	{
		if (mOnClickBtnList[2] != null)
		{
			mOnClickBtnList[2](mCurItem);
		}
	}

	public void OnClickBtn4()
	{
		if (mOnClickBtnList[3] != null)
		{
			mOnClickBtnList[3](mCurItem);
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRoot);
		if (onClose != null)
		{
			onClose();
		}
	}

	private void UpdateRootPos(ITEM_SHOW_TYPE showType)
	{
		switch (showType)
		{
		case ITEM_SHOW_TYPE.BACKPACK:
		{
			Vector3 localPosition3 = ItemInfoRoot.transform.localPosition;
			localPosition3.x = LeftPos;
			ItemInfoRoot.transform.localPosition = localPosition3;
			localPosition3 = ConsumeItemInfoRoot.transform.localPosition;
			localPosition3.x = LeftPos;
			ConsumeItemInfoRoot.transform.localPosition = localPosition3;
			break;
		}
		case ITEM_SHOW_TYPE.EQUIPPACK:
		{
			Vector3 localPosition2 = ItemInfoRoot.transform.localPosition;
			localPosition2.x = RightPos;
			ItemInfoRoot.transform.localPosition = localPosition2;
			localPosition2 = ConsumeItemInfoRoot.transform.localPosition;
			localPosition2.x = RightPos;
			ConsumeItemInfoRoot.transform.localPosition = localPosition2;
			break;
		}
		default:
		{
			Vector3 localPosition = ItemInfoRoot.transform.localPosition;
			localPosition.x = CenterPos;
			ItemInfoRoot.transform.localPosition = localPosition;
			localPosition = ConsumeItemInfoRoot.transform.localPosition;
			localPosition.x = CenterPos;
			ConsumeItemInfoRoot.transform.localPosition = localPosition;
			break;
		}
		}
	}

	public static void ShowItemTips(shop_item item)
	{
		GameItem item2 = new GameItem(item.ItemID, (EQUIP_QUALITY)item.Quality, (int)item.curNum);
		if (item2.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			item2.ItemLevel = 80;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRoot, delegate
		{
			SingletonUnity<ItemInfoRootLogic>.Instance.Reset(item2, ITEM_SHOW_TYPE.REWARD_TIPS, isNeedShowJumpPath: false, UI_PAGE_TYPE.INVALID, isShopItem: true);
		});
	}

	public static void ShowItemTips(consign_item item)
	{
		GameItem item2 = new GameItem(item.itemId, (EQUIP_QUALITY)item.quality, (int)item.stack);
		if (item2.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			item2.ItemLevel = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(item2.ItemData.SubType);
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRoot, delegate
		{
			SingletonUnity<ItemInfoRootLogic>.Instance.Reset(item2, ITEM_SHOW_TYPE.REWARD_TIPS);
		});
	}

	public static void ShowItemTips(ItemData itemData, bool isNeedShowJumpPath = false, UI_PAGE_TYPE prePage = UI_PAGE_TYPE.INVALID)
	{
		GameItem item1 = new GameItem(itemData.ID, itemData.QualityType, 1);
		if (item1.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			item1.ItemLevel = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(item1.ItemData.SubType);
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRoot, delegate
		{
			SingletonUnity<ItemInfoRootLogic>.Instance.Reset(item1, ITEM_SHOW_TYPE.REWARD_TIPS, isNeedShowJumpPath, prePage);
		});
	}

	public static void ShowItemTips(gameitem item)
	{
		GameItem item2 = new GameItem(item.itemId, (EQUIP_QUALITY)item.quality, (int)item.stack);
		if (item2.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			item2.ItemLevel = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(item2.ItemData.SubType);
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRoot, delegate
		{
			SingletonUnity<ItemInfoRootLogic>.Instance.Reset(item2, ITEM_SHOW_TYPE.REWARD_TIPS);
		});
	}

	public static void ShowItemTips(GameItem item, int level, bool isNeedShowJumpPath = false, UI_PAGE_TYPE prePage = UI_PAGE_TYPE.INVALID)
	{
		if (item.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			item.ItemLevel = level;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRoot, delegate
		{
			SingletonUnity<ItemInfoRootLogic>.Instance.Reset(item, ITEM_SHOW_TYPE.REWARD_TIPS, isNeedShowJumpPath, prePage);
		});
	}

	public void ResetCompareEquip(GameItem gameItem, GameItem equipedItem)
	{
		NGUITools.SetActive(ConsumeItemInfoRoot.gameObject, state: false);
		mCurItem = gameItem;
		ItemData itemData = mCurItem.ItemData;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ItemInfoRoot.ItemNameLabel.text = itemData.MName;
		ItemInfoRoot.ItemIcon.spriteName = itemData.BackPackIcon;
		if (itemData.CanSell())
		{
			ItemInfoRoot.PriceLabel.text = GameMoneyHelper.GetMoneyValStr(itemData.GetSellPrice(mCurItem.GetItemQuality()), itemData.PriceType);
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.PriceLabel.gameObject, state: true);
		}
		else
		{
			ItemInfoRoot.PriceLabel.text = string.Empty;
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.PriceLabel.gameObject, state: false);
		}
		ItemInfoRoot.LevelLabel.text = itemData.Level.ToString();
		SetLabelWarning(ItemInfoRoot.LevelLabel, playerData.Level < itemData.Level);
		ItemInfoRoot.BindingLabel.text = ((!mCurItem.BindFlag) ? StrDictionary.GetDictionaryString("#{100630}") : StrDictionary.GetDictionaryString("#{100629}"));
		ItemInfoRoot.LegendSprite.enabled = false;
		UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.ItemQualityIcon.gameObject, state: true);
		ItemInfoRoot.ItemQualityIcon.spriteName = gameItem.GetItemQuality().ToString();
		UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.ItemEnhanceLabel.gameObject, state: true);
		int itemCombatVal = mCurItem.GetItemCombatVal();
		ItemInfoRoot.ItemEnhanceLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100664}"), itemCombatVal);
		ItemInfoRoot.ItemNameLabel.color = GameDefine.GetColorByQuality(gameItem.GetItemQuality());
		ItemInfoRoot.QualityLabel.text = GameDefine.GetStrByQuality(gameItem.GetItemQuality());
		UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.ScoreLabel.gameObject, state: true);
		EquipData equipDataById = DataManager.GetEquipDataById(mCurItem.ItemId);
		ItemInfoRoot.ProfessionLabel.text = StrDictionary.GetDictionaryString(GameDefine.ProfessionName[equipDataById.Job]);
		SetLabelWarning(ItemInfoRoot.ProfessionLabel, playerData.Profession != equipDataById.profession);
		ItemInfoRoot.DescLabel.text = string.Empty;
		ItemInfoRoot.TitleLabel.text = StrDictionary.GetDictionaryString("#{100612}");
		ItemInfoRoot.IsEquipedSprite.enabled = false;
		for (int i = 0; i < ItemInfoRoot.LeftAttrLabelList.Length; i++)
		{
			if (i <= (int)mCurItem.Quality)
			{
				UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.LeftAttrLabelList[i].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.LeftAttrLabelList[i].gameObject, state: false);
			}
		}
		int[] array = new int[4] { -1, -1, -1, -1 };
		if (mCurItem.Quality >= EQUIP_QUALITY.KUANG_WHITE)
		{
			ItemInfoRoot.LeftAttrLabelList[0].text = GameDefine.GetAttributeName_S((int)equipDataById.BaseStatusType);
			array[0] = equipDataById.GetAttrValByQualityAndLevel(0, (int)mCurItem.Quality, mCurItem.ItemLevel);
			ItemInfoRoot.RightAttrLabelList[0].text = GameDefine.GetAttributeValueStr((int)equipDataById.BaseStatusType, array[0]);
			ItemInfoRoot.LefeAttrIconList[0].spriteName = GameDefine.GetAttributeIcon((int)equipDataById.BaseStatusType);
		}
		if (mCurItem.Quality >= EQUIP_QUALITY.KUANG_GREEN)
		{
			ItemInfoRoot.LeftAttrLabelList[1].text = GameDefine.GetAttributeName_S(equipDataById.Status1);
			array[1] = equipDataById.GetAttrValByQualityAndLevel(1, (int)mCurItem.Quality, mCurItem.ItemLevel);
			ItemInfoRoot.RightAttrLabelList[1].text = GameDefine.GetAttributeValueStr(equipDataById.Status1, array[1]);
			ItemInfoRoot.LefeAttrIconList[1].spriteName = GameDefine.GetAttributeIcon(equipDataById.Status1);
		}
		if (mCurItem.Quality >= EQUIP_QUALITY.KUANG_BLUE)
		{
			ItemInfoRoot.LeftAttrLabelList[2].text = GameDefine.GetAttributeName_S(equipDataById.Status2);
			array[2] = equipDataById.GetAttrValByQualityAndLevel(2, (int)mCurItem.Quality, mCurItem.ItemLevel);
			ItemInfoRoot.RightAttrLabelList[2].text = GameDefine.GetAttributeValueStr(equipDataById.Status2, array[2]);
			ItemInfoRoot.LefeAttrIconList[2].spriteName = GameDefine.GetAttributeIcon(equipDataById.Status2);
		}
		if (mCurItem.Quality >= EQUIP_QUALITY.KUANG_PURPLE)
		{
			ItemInfoRoot.LegendSprite.enabled = true;
			ItemInfoRoot.LeftAttrLabelList[3].text = GameDefine.GetAttributeName_S(equipDataById.ExStatus);
			array[3] = equipDataById.GetAttrValByQualityAndLevel(3, (int)mCurItem.Quality, mCurItem.ItemLevel);
			ItemInfoRoot.RightAttrLabelList[3].text = GameDefine.GetAttributeValueStr(equipDataById.ExStatus, array[3]);
			ItemInfoRoot.LefeAttrIconList[3].spriteName = GameDefine.GetAttributeIcon(equipDataById.ExStatus);
		}
		if (equipedItem != null && !equipedItem.IsEmpty())
		{
			ItemData itemData2 = equipedItem.ItemData;
			EquipInfoRoot.ItemNameLabel.text = itemData2.MName;
			EquipInfoRoot.ItemIcon.spriteName = itemData2.BackPackIcon;
			if (itemData.CanSell())
			{
				EquipInfoRoot.PriceLabel.text = GameMoneyHelper.GetMoneyValStr(itemData2.GetSellPrice(equipedItem.Quality), itemData2.PriceType);
				UnityVersionUtil.SetActiveRecursive(EquipInfoRoot.PriceLabel.gameObject, state: true);
			}
			else
			{
				EquipInfoRoot.PriceLabel.text = string.Empty;
				UnityVersionUtil.SetActiveRecursive(EquipInfoRoot.PriceLabel.gameObject, state: false);
			}
			EquipInfoRoot.LevelLabel.text = itemData2.Level.ToString();
			SetLabelWarning(EquipInfoRoot.LevelLabel, istrue: false);
			EquipInfoRoot.BindingLabel.text = ((!equipedItem.BindFlag) ? StrDictionary.GetDictionaryString("#{100630}") : StrDictionary.GetDictionaryString("#{100629}"));
			EquipInfoRoot.LegendSprite.enabled = false;
			UnityVersionUtil.SetActiveRecursive(EquipInfoRoot.ItemQualityIcon.gameObject, state: true);
			EquipInfoRoot.ItemQualityIcon.spriteName = equipedItem.Quality.ToString();
			UnityVersionUtil.SetActiveRecursive(EquipInfoRoot.ItemEnhanceLabel.gameObject, state: true);
			int itemCombatVal2 = equipedItem.GetItemCombatVal();
			EquipInfoRoot.ItemEnhanceLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100664}"), itemCombatVal2);
			EquipInfoRoot.QualityLabel.text = GameDefine.GetStrByQuality(equipedItem.Quality);
			UnityVersionUtil.SetActiveRecursive(EquipInfoRoot.ScoreLabel.gameObject, state: true);
			EquipData equipDataById2 = DataManager.GetEquipDataById(equipedItem.ItemId);
			EquipInfoRoot.ProfessionLabel.text = StrDictionary.GetDictionaryString(GameDefine.ProfessionName[equipDataById2.Job]);
			SetLabelWarning(EquipInfoRoot.ProfessionLabel, playerData.Profession != equipDataById2.profession);
			EquipInfoRoot.DescLabel.text = string.Empty;
			EquipInfoRoot.TitleLabel.text = StrDictionary.GetDictionaryString("#{100612}");
			EquipInfoRoot.IsEquipedSprite.enabled = true;
			for (int j = 0; j < EquipInfoRoot.LeftAttrLabelList.Length; j++)
			{
				if (j <= (int)equipedItem.Quality)
				{
					UnityVersionUtil.SetActiveRecursive(EquipInfoRoot.LeftAttrLabelList[j].gameObject, state: true);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(EquipInfoRoot.LeftAttrLabelList[j].gameObject, state: false);
				}
			}
			int[] array2 = new int[4] { -1, -1, -1, -1 };
			if (equipedItem.Quality >= EQUIP_QUALITY.KUANG_WHITE)
			{
				EquipInfoRoot.LeftAttrLabelList[0].text = GameDefine.GetAttributeName_S((int)equipDataById2.BaseStatusType);
				array2[0] = equipDataById2.GetAttrValByQualityAndLevel(0, (int)equipedItem.Quality, equipedItem.ItemLevel);
				EquipInfoRoot.RightAttrLabelList[0].text = GameDefine.GetAttributeValueStr((int)equipDataById2.BaseStatusType, array2[0]);
				EquipInfoRoot.LefeAttrIconList[0].spriteName = GameDefine.GetAttributeIcon((int)equipDataById2.BaseStatusType);
			}
			if (equipedItem.Quality >= EQUIP_QUALITY.KUANG_GREEN)
			{
				EquipInfoRoot.LeftAttrLabelList[1].text = GameDefine.GetAttributeName_S(equipDataById2.Status1);
				array2[1] = equipDataById2.GetAttrValByQualityAndLevel(1, (int)equipedItem.Quality, equipedItem.ItemLevel);
				EquipInfoRoot.RightAttrLabelList[1].text = GameDefine.GetAttributeValueStr((int)equipDataById2.BaseStatusType, array2[1]);
				EquipInfoRoot.LefeAttrIconList[1].spriteName = GameDefine.GetAttributeIcon(equipDataById2.Status1);
			}
			if (equipedItem.Quality >= EQUIP_QUALITY.KUANG_BLUE)
			{
				EquipInfoRoot.LeftAttrLabelList[2].text = GameDefine.GetAttributeName_S(equipDataById2.Status2);
				array2[2] = equipDataById2.GetAttrValByQualityAndLevel(2, (int)equipedItem.Quality, equipedItem.ItemLevel);
				EquipInfoRoot.RightAttrLabelList[2].text = GameDefine.GetAttributeValueStr((int)equipDataById2.BaseStatusType, array2[2]);
				EquipInfoRoot.LefeAttrIconList[2].spriteName = GameDefine.GetAttributeIcon(equipDataById2.Status2);
			}
			if (equipedItem.Quality >= EQUIP_QUALITY.KUANG_PURPLE)
			{
				EquipInfoRoot.LegendSprite.enabled = true;
				EquipInfoRoot.LeftAttrLabelList[3].text = GameDefine.GetAttributeName_S(equipDataById2.ExStatus);
				array2[3] = equipDataById2.GetAttrValByQualityAndLevel(3, (int)equipedItem.Quality, equipedItem.ItemLevel);
				EquipInfoRoot.RightAttrLabelList[3].text = GameDefine.GetAttributeValueStr((int)equipDataById2.BaseStatusType, array2[3]);
				EquipInfoRoot.LefeAttrIconList[3].spriteName = GameDefine.GetAttributeIcon(equipDataById2.ExStatus);
			}
			EquipInfoRoot.AttributeGrid.Reposition();
			EquipInfoRoot.InfoGrid.Reposition();
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k] > array2[k])
				{
					NGUITools.SetActive(ItemInfoRoot.UpArrowList[k], state: true);
					NGUITools.SetActive(ItemInfoRoot.DownArrowList[k], state: false);
				}
				else if (array[k] < array2[k] && array[k] > 0)
				{
					NGUITools.SetActive(ItemInfoRoot.UpArrowList[k], state: false);
					NGUITools.SetActive(ItemInfoRoot.DownArrowList[k], state: true);
				}
				else
				{
					NGUITools.SetActive(ItemInfoRoot.UpArrowList[k], state: false);
					NGUITools.SetActive(ItemInfoRoot.DownArrowList[k], state: false);
				}
				NGUITools.SetActive(EquipInfoRoot.UpArrowList[k], state: false);
				NGUITools.SetActive(EquipInfoRoot.DownArrowList[k], state: false);
			}
			if (itemCombatVal > itemCombatVal2)
			{
				NGUITools.SetActive(ItemInfoRoot.PowerUpArrow, state: true);
				NGUITools.SetActive(ItemInfoRoot.PowerDownArrow, state: false);
			}
			else if (itemCombatVal < itemCombatVal2)
			{
				NGUITools.SetActive(ItemInfoRoot.PowerUpArrow, state: false);
				NGUITools.SetActive(ItemInfoRoot.PowerDownArrow, state: true);
			}
			else
			{
				NGUITools.SetActive(ItemInfoRoot.PowerUpArrow, state: false);
				NGUITools.SetActive(ItemInfoRoot.PowerDownArrow, state: false);
			}
			NGUITools.SetActive(EquipInfoRoot.PowerUpArrow, state: false);
			NGUITools.SetActive(EquipInfoRoot.PowerDownArrow, state: false);
			UpdateRootPos(ITEM_SHOW_TYPE.EQUIPPACK);
		}
		else
		{
			NGUITools.SetActive(EquipInfoRoot.gameObject, state: false);
			for (int l = 0; l < array.Length; l++)
			{
				if (array[l] > 0)
				{
					NGUITools.SetActive(ItemInfoRoot.UpArrowList[l], state: true);
					NGUITools.SetActive(ItemInfoRoot.DownArrowList[l], state: false);
				}
				else
				{
					NGUITools.SetActive(ItemInfoRoot.UpArrowList[l], state: false);
					NGUITools.SetActive(ItemInfoRoot.DownArrowList[l], state: false);
				}
			}
			NGUITools.SetActive(ItemInfoRoot.PowerUpArrow, state: true);
			NGUITools.SetActive(ItemInfoRoot.PowerDownArrow, state: false);
			NGUITools.SetActive(EquipInfoRoot.PowerUpArrow, state: false);
			NGUITools.SetActive(EquipInfoRoot.PowerDownArrow, state: false);
			UpdateRootPos(ITEM_SHOW_TYPE.BACKPACK);
		}
		ItemInfoRoot.AttributeGrid.Reposition();
		ItemInfoRoot.InfoGrid.Reposition();
		if (!mCurItem.BindFlag && mCurItem.ItemData.CanConsign())
		{
			SetBtn(2, "#{100614}", "#{100621}");
			mOnClickBtnList[0] = OnClickEquipBtn;
			mOnClickBtnList[1] = OnClickSellBtn;
		}
		else
		{
			SetBtn(2, "#{100614}", "#{100621}");
			mOnClickBtnList[0] = OnClickEquipBtn;
			mOnClickBtnList[1] = OnClickSellBtn;
		}
		if (CheckCanEquip())
		{
			SetBtnSp(BtnSpList[0], isenable: true);
		}
		else
		{
			SetBtnSp(BtnSpList[0], isenable: false);
		}
	}

	public void Reset(GameItem gameItem, ITEM_SHOW_TYPE showType, bool isNeedShowJumpPath = false, UI_PAGE_TYPE prePage = UI_PAGE_TYPE.INVALID, bool isShopItem = false)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		mCurItem = gameItem;
		ItemData itemData = mCurItem.ItemData;
		mPrePage = prePage;
		if (isNeedShowJumpPath && !string.IsNullOrEmpty(itemData.JumpPath) && GameManager.IsSupportCurDataVersion47())
		{
			NGUITools.SetActive(ConsumeItemInfoRoot.gameObject, state: true);
			NGUITools.SetActive(EquipInfoRoot.gameObject, state: false);
			NGUITools.SetActive(ItemInfoObjRoot.gameObject, state: false);
			UpdateRootPos(showType);
			ConsumeItemInfoRoot.ItemNameLabel.text = itemData.MName;
			ConsumeItemInfoRoot.ItemIcon.spriteName = itemData.BackPackIcon;
			if (itemData.CanSell())
			{
				ConsumeItemInfoRoot.PriceLabel.text = GameMoneyHelper.GetMoneyValStr(itemData.GetSellPrice(gameItem.Quality), itemData.PriceType);
				UnityVersionUtil.SetActiveRecursive(ConsumeItemInfoRoot.PriceLabel.gameObject, state: true);
			}
			else
			{
				ConsumeItemInfoRoot.PriceLabel.text = string.Empty;
				UnityVersionUtil.SetActiveRecursive(ConsumeItemInfoRoot.PriceLabel.gameObject, state: false);
			}
			ConsumeItemInfoRoot.LevelLabel.text = itemData.Level.ToString();
			SetLabelWarning(ConsumeItemInfoRoot.LevelLabel, playerData.Level < itemData.Level);
			ConsumeItemInfoRoot.TitleLabel.text = StrDictionary.GetDictionaryString("#{100611}");
			ConsumeItemInfoRoot.ItemQualityIcon.spriteName = itemData.QualityType.ToString();
			ConsumeItemInfoRoot.ItemNameLabel.color = GameDefine.GetColorByQuality(itemData.QualityType);
			ConsumeItemInfoRoot.DescLabel.text = itemData.MDescription;
			string[] array = itemData.JumpPath.Split(';');
			for (int i = 0; i < ConsumeItemInfoRoot.JumpBtnRoot.Length; i++)
			{
				if (i < array.Length)
				{
					NGUITools.SetActive(ConsumeItemInfoRoot.JumpBtnRoot[i].gameObject, state: true);
					mJumpPath[i] = (JUMP_PATH)int.Parse(array[i]);
					switch (mJumpPath[i])
					{
					case JUMP_PATH.SHOP:
						ConsumeItemInfoRoot.JumpBtnLabel[i].text = StrDictionary.GetDictionaryString("#{100110}");
						break;
					case JUMP_PATH.SLOT:
						ConsumeItemInfoRoot.JumpBtnLabel[i].text = StrDictionary.GetDictionaryString("#{100109}");
						break;
					case JUMP_PATH.RACE:
						ConsumeItemInfoRoot.JumpBtnLabel[i].text = StrDictionary.GetDictionaryString("#{101569}");
						break;
					case JUMP_PATH.SCUFFLE:
						ConsumeItemInfoRoot.JumpBtnLabel[i].text = StrDictionary.GetDictionaryString("#{102017}");
						break;
					case JUMP_PATH.EQUIP_COPY:
						ConsumeItemInfoRoot.JumpBtnLabel[i].text = StrDictionary.GetDictionaryString("#{101510}");
						break;
					default:
						ConsumeItemInfoRoot.JumpBtnLabel[i].text = StrDictionary.GetDictionaryString("#{100110}");
						break;
					}
				}
				else
				{
					NGUITools.SetActive(ConsumeItemInfoRoot.JumpBtnRoot[i].gameObject, state: false);
				}
			}
			if (showType != ITEM_SHOW_TYPE.REWARD_TIPS)
			{
				switch (itemData.Type)
				{
				case GameDefine.ITEM_TYPE.POTION:
				case GameDefine.ITEM_TYPE.POTION_2:
					if (mCurItem.ItemData.CanConsign())
					{
						SetBtn(2, "#{100623}", "#{100621}");
						mOnClickBtnList[0] = OnClickUseDragBtn;
						mOnClickBtnList[1] = OnClickSellBtn;
					}
					else
					{
						SetBtn(2, "#{100623}", "#{100621}");
						mOnClickBtnList[0] = OnClickUseDragBtn;
						mOnClickBtnList[1] = OnClickSellBtn;
					}
					if (CheckLevel())
					{
						SetBtnSp(BtnSpList[0], isenable: true);
					}
					else
					{
						SetBtnSp(BtnSpList[0], isenable: false);
					}
					break;
				case GameDefine.ITEM_TYPE.ENHANCE_ITEM:
					if (mCurItem.ItemData.CanConsign())
					{
						SetBtn(1, "#{100623}");
						mOnClickBtnList[0] = OnClickUseEnhanceBtn;
					}
					else
					{
						SetBtn(1, "#{100623}");
						mOnClickBtnList[0] = OnClickUseEnhanceBtn;
					}
					if (CheckLevel())
					{
						SetBtnSp(BtnSpList[0], isenable: true);
					}
					else
					{
						SetBtnSp(BtnSpList[0], isenable: false);
					}
					break;
				default:
					if (mCurItem.ItemData.CanConsign())
					{
						SetBtn(0);
					}
					else
					{
						SetBtn(0);
					}
					break;
				}
			}
			else
			{
				SetBtn(0);
			}
			return;
		}
		NGUITools.SetActive(ConsumeItemInfoRoot.gameObject, state: false);
		NGUITools.SetActive(EquipInfoRoot.gameObject, state: false);
		UpdateRootPos(showType);
		ItemInfoRoot.ItemNameLabel.text = itemData.MName;
		ItemInfoRoot.ItemIcon.spriteName = itemData.BackPackIcon;
		if (itemData.CanSell())
		{
			ItemInfoRoot.PriceLabel.text = GameMoneyHelper.GetMoneyValStr(itemData.GetSellPrice(gameItem.Quality), itemData.PriceType);
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.PriceLabel.gameObject, state: true);
		}
		else
		{
			ItemInfoRoot.PriceLabel.text = string.Empty;
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.PriceLabel.gameObject, state: false);
		}
		ItemInfoRoot.LevelLabel.text = itemData.Level.ToString();
		SetLabelWarning(ItemInfoRoot.LevelLabel, playerData.Level < itemData.Level);
		ItemInfoRoot.BindingLabel.text = ((!mCurItem.BindFlag) ? StrDictionary.GetDictionaryString("#{100630}") : StrDictionary.GetDictionaryString("#{100629}"));
		ItemInfoRoot.LegendSprite.enabled = false;
		if (itemData.Type == GameDefine.ITEM_TYPE.EQUIP || itemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.ItemQualityIcon.gameObject, state: true);
			ItemInfoRoot.ItemQualityIcon.spriteName = gameItem.Quality.ToString();
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.ItemEnhanceLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.PowerUpArrow, state: false);
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.PowerDownArrow, state: false);
			if (itemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				if (isShopItem)
				{
					ItemInfoRoot.ItemEnhanceLabel.text = string.Format("{0}:{1}[FFFF00] (Max)[-]", StrDictionary.GetDictionaryString("#{100664}"), mCurItem.GetItemCombatVal());
				}
				else
				{
					ItemInfoRoot.ItemEnhanceLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100664}"), mCurItem.GetItemCombatVal());
				}
			}
			else if (gameItem.Parm[4] > 0)
			{
				ItemInfoRoot.ItemEnhanceLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100643}"), TimeTools.GetFormateTime(gameItem.Parm[4] - playerCommonData.GetCurServerTime()));
			}
			else if (gameItem.ItemData.UseHour > 0)
			{
				ItemInfoRoot.ItemEnhanceLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100643}"), TimeTools.GetFormateTime(gameItem.ItemData.UseHour * 3600));
			}
			else
			{
				ItemInfoRoot.ItemEnhanceLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100643}"), StrDictionary.GetDictionaryString("#{100646}"));
			}
			ItemInfoRoot.ItemNameLabel.color = GameDefine.GetColorByQuality(gameItem.Quality);
			ItemInfoRoot.QualityLabel.text = GameDefine.GetStrByQuality(gameItem.Quality);
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.ScoreLabel.gameObject, state: true);
			EquipData equipDataById = DataManager.GetEquipDataById(mCurItem.ItemId);
			ItemInfoRoot.ProfessionLabel.text = StrDictionary.GetDictionaryString(GameDefine.ProfessionName[equipDataById.Job]);
			SetLabelWarning(ItemInfoRoot.ProfessionLabel, playerData.Profession != equipDataById.profession);
			ItemInfoRoot.DescLabel.text = string.Empty;
			ItemInfoRoot.TitleLabel.text = StrDictionary.GetDictionaryString("#{100612}");
			switch (showType)
			{
			case ITEM_SHOW_TYPE.BACKPACK:
			case ITEM_SHOW_TYPE.REWARD_TIPS:
				ItemInfoRoot.IsEquipedSprite.enabled = false;
				break;
			case ITEM_SHOW_TYPE.EQUIPPACK:
				ItemInfoRoot.IsEquipedSprite.enabled = true;
				break;
			}
			for (int j = 0; j < ItemInfoRoot.LeftAttrLabelList.Length; j++)
			{
				if (j <= (int)mCurItem.Quality)
				{
					UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.LeftAttrLabelList[j].gameObject, state: true);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.LeftAttrLabelList[j].gameObject, state: false);
				}
			}
			if (mCurItem.Quality >= EQUIP_QUALITY.KUANG_WHITE)
			{
				if (isShopItem)
				{
					ItemInfoRoot.LeftAttrLabelList[0].text = $"{GameDefine.GetAttributeName_S((int)equipDataById.BaseStatusType)}[FFFF00] (Max)[-]";
				}
				else
				{
					ItemInfoRoot.LeftAttrLabelList[0].text = GameDefine.GetAttributeName_S((int)equipDataById.BaseStatusType);
				}
				ItemInfoRoot.RightAttrLabelList[0].text = GameDefine.GetAttributeValueStr((int)equipDataById.BaseStatusType, equipDataById.GetAttrValByQualityAndLevel(0, (int)mCurItem.Quality, mCurItem.ItemLevel));
				ItemInfoRoot.LefeAttrIconList[0].spriteName = GameDefine.GetAttributeIcon((int)equipDataById.BaseStatusType);
			}
			if (mCurItem.Quality >= EQUIP_QUALITY.KUANG_GREEN)
			{
				if (isShopItem)
				{
					ItemInfoRoot.LeftAttrLabelList[1].text = $"{GameDefine.GetAttributeName_S(equipDataById.Status1)}[FFFF00] (Max)[-]";
				}
				else
				{
					ItemInfoRoot.LeftAttrLabelList[1].text = GameDefine.GetAttributeName_S(equipDataById.Status1);
				}
				ItemInfoRoot.RightAttrLabelList[1].text = GameDefine.GetAttributeValueStr(equipDataById.Status1, equipDataById.GetAttrValByQualityAndLevel(1, (int)mCurItem.Quality, mCurItem.ItemLevel));
				ItemInfoRoot.LefeAttrIconList[1].spriteName = GameDefine.GetAttributeIcon(equipDataById.Status1);
			}
			if (mCurItem.Quality >= EQUIP_QUALITY.KUANG_BLUE)
			{
				if (isShopItem)
				{
					ItemInfoRoot.LeftAttrLabelList[2].text = $"{GameDefine.GetAttributeName_S(equipDataById.Status2)}[FFFF00] (Max)[-]";
				}
				else
				{
					ItemInfoRoot.LeftAttrLabelList[2].text = GameDefine.GetAttributeName_S(equipDataById.Status2);
				}
				ItemInfoRoot.RightAttrLabelList[2].text = GameDefine.GetAttributeValueStr(equipDataById.Status2, equipDataById.GetAttrValByQualityAndLevel(2, (int)mCurItem.Quality, mCurItem.ItemLevel));
				ItemInfoRoot.LefeAttrIconList[2].spriteName = GameDefine.GetAttributeIcon(equipDataById.Status2);
			}
			if (mCurItem.Quality >= EQUIP_QUALITY.KUANG_PURPLE)
			{
				if (isShopItem)
				{
					ItemInfoRoot.LeftAttrLabelList[3].text = $"{GameDefine.GetAttributeName_S(equipDataById.ExStatus)}[FFFF00] (Max)[-]";
				}
				else
				{
					ItemInfoRoot.LeftAttrLabelList[3].text = GameDefine.GetAttributeName_S(equipDataById.ExStatus);
				}
				ItemInfoRoot.LegendSprite.enabled = true;
				ItemInfoRoot.RightAttrLabelList[3].text = GameDefine.GetAttributeValueStr(equipDataById.ExStatus, equipDataById.GetAttrValByQualityAndLevel(3, (int)mCurItem.Quality, mCurItem.ItemLevel));
				ItemInfoRoot.LefeAttrIconList[3].spriteName = GameDefine.GetAttributeIcon(equipDataById.ExStatus);
			}
		}
		else if (itemData.Type == GameDefine.ITEM_TYPE.BADGE)
		{
			BadgeData badgeDataById = DataManager.GetBadgeDataById(mCurItem.ItemId);
			ItemInfoRoot.ItemQualityIcon.spriteName = itemData.QualityType.ToString();
			ItemInfoRoot.ItemNameLabel.color = GameDefine.GetColorByQuality(itemData.QualityType);
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.ItemEnhanceLabel.gameObject, state: false);
			ItemInfoRoot.ItemEnhanceLabel.text = badgeDataById.Lv.ToString();
			ItemInfoRoot.ProfessionLabel.text = StrDictionary.GetDictionaryString("#{100830}");
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.ScoreLabel.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.QualityLabel.gameObject, state: false);
			ItemInfoRoot.DescLabel.text = string.Empty;
			ItemInfoRoot.TitleLabel.text = StrDictionary.GetDictionaryString("#{100612}");
			switch (showType)
			{
			case ITEM_SHOW_TYPE.BACKPACK:
			case ITEM_SHOW_TYPE.REWARD_TIPS:
				ItemInfoRoot.IsEquipedSprite.enabled = false;
				break;
			case ITEM_SHOW_TYPE.EQUIPPACK:
				ItemInfoRoot.IsEquipedSprite.enabled = true;
				break;
			}
			for (int k = 0; k < ItemInfoRoot.LeftAttrLabelList.Length; k++)
			{
				NGUITools.SetActive(ItemInfoRoot.LeftAttrLabelList[k].gameObject, state: false);
			}
			if (badgeDataById.Status1 != -1)
			{
				NGUITools.SetActive(ItemInfoRoot.LeftAttrLabelList[0].gameObject, state: true);
				ItemInfoRoot.LeftAttrLabelList[0].text = GameDefine.GetAttributeName_S(badgeDataById.Status1);
				ItemInfoRoot.RightAttrLabelList[0].text = GameDefine.GetAttributeValueStr(badgeDataById.Status1, badgeDataById.Value1);
				ItemInfoRoot.LefeAttrIconList[0].spriteName = GameDefine.GetAttributeIcon(badgeDataById.Status1);
			}
			if (badgeDataById.Status2 != -1)
			{
				NGUITools.SetActive(ItemInfoRoot.LeftAttrLabelList[1].gameObject, state: true);
				ItemInfoRoot.LeftAttrLabelList[1].text = GameDefine.GetAttributeName_S(badgeDataById.Status2);
				ItemInfoRoot.RightAttrLabelList[1].text = GameDefine.GetAttributeValueStr(badgeDataById.Status2, badgeDataById.Value2);
				ItemInfoRoot.LefeAttrIconList[1].spriteName = GameDefine.GetAttributeIcon(badgeDataById.Status2);
			}
		}
		else
		{
			ItemInfoRoot.TitleLabel.text = StrDictionary.GetDictionaryString("#{100611}");
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.ItemEnhanceLabel.gameObject, state: false);
			ItemInfoRoot.ItemQualityIcon.spriteName = itemData.QualityType.ToString();
			ItemInfoRoot.ItemNameLabel.color = GameDefine.GetColorByQuality(itemData.QualityType);
			ItemInfoRoot.IsEquipedSprite.enabled = false;
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.ScoreLabel.gameObject, state: false);
			ItemInfoRoot.ProfessionLabel.text = StrDictionary.GetDictionaryString("#{100830}");
			UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.QualityLabel.gameObject, state: false);
			ItemInfoRoot.DescLabel.text = itemData.MDescription;
			for (int l = 0; l < ItemInfoRoot.LeftAttrLabelList.Length; l++)
			{
				UnityVersionUtil.SetActiveRecursive(ItemInfoRoot.LeftAttrLabelList[l].gameObject, state: false);
			}
		}
		ItemInfoRoot.AttributeGrid.Reposition();
		ItemInfoRoot.InfoGrid.Reposition();
		if (showType != ITEM_SHOW_TYPE.REWARD_TIPS)
		{
			switch (itemData.Type)
			{
			case GameDefine.ITEM_TYPE.EQUIP:
				switch (showType)
				{
				case ITEM_SHOW_TYPE.BACKPACK:
					if (mCurItem.ItemData.CanConsign() && !mCurItem.BindFlag)
					{
						SetBtn(2, "#{100614}", "#{100621}");
						mOnClickBtnList[0] = OnClickEquipBtn;
						mOnClickBtnList[1] = OnClickSellBtn;
					}
					else
					{
						SetBtn(2, "#{100614}", "#{100621}");
						mOnClickBtnList[0] = OnClickEquipBtn;
						mOnClickBtnList[1] = OnClickSellBtn;
					}
					if (CheckCanEquip())
					{
						SetBtnSp(BtnSpList[0], isenable: true);
					}
					else
					{
						SetBtnSp(BtnSpList[0], isenable: false);
					}
					break;
				case ITEM_SHOW_TYPE.EQUIPPACK:
				{
					EquipData equipDataById2 = DataManager.GetEquipDataById(mCurItem.ItemId);
					if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP && equipDataById2.EquipType == EQUIP_BACKPACK_TYPE.WEAPON)
					{
						SetBtn(1, "#{100618}");
						mOnClickBtnList[0] = OnClickEnhanceBtn;
						if (CheckFunctionUnlock(FUNCTION_TYPE.ENHANCE_EQUIP))
						{
							SetBtnSp(BtnSpList[0], isenable: true);
						}
						else
						{
							SetBtnSp(BtnSpList[0], isenable: false);
						}
						break;
					}
					SetBtn(2, "#{100615}", "#{100618}");
					mOnClickBtnList[0] = OnClickTakeOffBtn;
					mOnClickBtnList[1] = OnClickEnhanceBtn;
					if (CheckFunctionUnlock(FUNCTION_TYPE.ENHANCE_EQUIP))
					{
						SetBtnSp(BtnSpList[1], isenable: true);
					}
					else
					{
						SetBtnSp(BtnSpList[1], isenable: false);
					}
					break;
				}
				}
				break;
			case GameDefine.ITEM_TYPE.POTION:
			case GameDefine.ITEM_TYPE.POTION_2:
				if (mCurItem.ItemData.CanConsign())
				{
					SetBtn(2, "#{100623}", "#{100621}");
					mOnClickBtnList[0] = OnClickUseDragBtn;
					mOnClickBtnList[1] = OnClickSellBtn;
				}
				else
				{
					SetBtn(2, "#{100623}", "#{100621}");
					mOnClickBtnList[0] = OnClickUseDragBtn;
					mOnClickBtnList[1] = OnClickSellBtn;
				}
				if (CheckLevel())
				{
					SetBtnSp(BtnSpList[0], isenable: true);
				}
				else
				{
					SetBtnSp(BtnSpList[0], isenable: false);
				}
				break;
			case GameDefine.ITEM_TYPE.ENHANCE_ITEM:
				if (mCurItem.ItemData.CanConsign())
				{
					SetBtn(1, "#{100623}");
					mOnClickBtnList[0] = OnClickUseEnhanceBtn;
				}
				else
				{
					SetBtn(1, "#{100623}");
					mOnClickBtnList[0] = OnClickUseEnhanceBtn;
				}
				if (CheckLevel())
				{
					SetBtnSp(BtnSpList[0], isenable: true);
				}
				else
				{
					SetBtnSp(BtnSpList[0], isenable: false);
				}
				break;
			case GameDefine.ITEM_TYPE.BADGE:
				switch (showType)
				{
				case ITEM_SHOW_TYPE.BACKPACK:
				{
					BadgeData badgeDataById2 = DataManager.GetBadgeDataById(mCurItem.ItemId);
					if (badgeDataById2.Lv < GameDefine.MAX_BADGE_LEVEL)
					{
						if (mCurItem.ItemData.CanConsign())
						{
							SetBtn(2, "#{100614}", "#{100616}");
							mOnClickBtnList[0] = OnClickEquipBtn;
							mOnClickBtnList[1] = OnClickMergeBtn;
						}
						else
						{
							SetBtn(2, "#{100614}", "#{100616}");
							mOnClickBtnList[0] = OnClickEquipBtn;
							mOnClickBtnList[1] = OnClickMergeBtn;
						}
						if (CheckFunctionUnlock(FUNCTION_TYPE.ENHANCE_BADGE))
						{
							SetBtnSp(BtnSpList[1], isenable: true);
						}
						else
						{
							SetBtnSp(BtnSpList[1], isenable: false);
						}
					}
					else if (mCurItem.ItemData.CanConsign())
					{
						SetBtn(1, "#{100614}");
						mOnClickBtnList[0] = OnClickEquipBtn;
					}
					else
					{
						SetBtn(1, "#{100614}");
						mOnClickBtnList[0] = OnClickEquipBtn;
					}
					break;
				}
				case ITEM_SHOW_TYPE.EQUIPPACK:
					SetBtn(1, "#{100615}");
					mOnClickBtnList[0] = OnClickTakeOffBtn;
					break;
				}
				break;
			case GameDefine.ITEM_TYPE.FASHION_EQUIP:
				switch (showType)
				{
				case ITEM_SHOW_TYPE.BACKPACK:
					SetBtn(2, "#{100614}", "#{100621}");
					mOnClickBtnList[0] = OnClickEquipBtn;
					mOnClickBtnList[1] = OnClickSellBtn;
					break;
				case ITEM_SHOW_TYPE.EQUIPPACK:
					SetBtn(1, "#{100615}");
					mOnClickBtnList[0] = OnClickTakeOffBtn;
					break;
				}
				if (CheckCanEquip())
				{
					SetBtnSp(BtnSpList[0], isenable: true);
				}
				else
				{
					SetBtnSp(BtnSpList[0], isenable: false);
				}
				break;
			case GameDefine.ITEM_TYPE.BOX:
				if (mCurItem.ItemData.CanConsign())
				{
					SetBtn(2, "#{300405}", "#{300406}");
					mOnClickBtnList[0] = OnClickOpenBoxBtn;
					mOnClickBtnList[1] = OnClickOpenAllBoxBtn;
				}
				else
				{
					SetBtn(2, "#{300405}", "#{300406}");
					mOnClickBtnList[0] = OnClickOpenBoxBtn;
					mOnClickBtnList[1] = OnClickOpenAllBoxBtn;
				}
				if (CheckLevel())
				{
					SetBtnSp(BtnSpList[0], isenable: true);
					SetBtnSp(BtnSpList[1], isenable: true);
				}
				else
				{
					SetBtnSp(BtnSpList[0], isenable: false);
					SetBtnSp(BtnSpList[1], isenable: false);
				}
				break;
			case GameDefine.ITEM_TYPE.LOCK1:
				if (mCurItem.ItemData.CanConsign())
				{
					SetBtn(2, "#{300405}", "#{300406}");
					mOnClickBtnList[0] = OnClickOpenBoxBtn;
					mOnClickBtnList[1] = OnClickOpenAllBoxBtn;
				}
				else
				{
					SetBtn(2, "#{300405}", "#{300406}");
					mOnClickBtnList[0] = OnClickOpenBoxBtn;
					mOnClickBtnList[1] = OnClickOpenAllBoxBtn;
				}
				if (CheckLevel())
				{
					SetBtnSp(BtnSpList[0], isenable: true);
					SetBtnSp(BtnSpList[1], isenable: true);
				}
				else
				{
					SetBtnSp(BtnSpList[0], isenable: false);
					SetBtnSp(BtnSpList[1], isenable: false);
				}
				break;
			case GameDefine.ITEM_TYPE.LOCK2:
				if (mCurItem.ItemData.CanConsign())
				{
					SetBtn(2, "#{100623}", "#{300407}");
					mOnClickBtnList[0] = OnClickOpenBoxBtn;
					mOnClickBtnList[1] = OnClickOpenAllBoxBtn;
				}
				else
				{
					SetBtn(2, "#{100623}", "#{300407}");
					mOnClickBtnList[0] = OnClickOpenBoxBtn;
					mOnClickBtnList[1] = OnClickOpenAllBoxBtn;
				}
				if (CheckLevel())
				{
					SetBtnSp(BtnSpList[0], isenable: true);
					SetBtnSp(BtnSpList[1], isenable: true);
				}
				else
				{
					SetBtnSp(BtnSpList[0], isenable: false);
					SetBtnSp(BtnSpList[1], isenable: false);
				}
				break;
			case GameDefine.ITEM_TYPE.EXCHANGE:
				if (mCurItem.ItemData.CanConsign())
				{
					SetBtn(1, "#{100623}");
					mOnClickBtnList[0] = OnClickUseExChanegBtn;
				}
				else
				{
					SetBtn(1, "#{100623}");
					mOnClickBtnList[0] = OnClickUseExChanegBtn;
				}
				if (CheckLevel())
				{
					SetBtnSp(BtnSpList[0], isenable: true);
				}
				else
				{
					SetBtnSp(BtnSpList[0], isenable: false);
				}
				break;
			case GameDefine.ITEM_TYPE.REMAIN:
				if (mCurItem.ItemData.CanConsign())
				{
					SetBtn(1, "#{100623}");
					mOnClickBtnList[0] = OnClickUsItemBtn;
				}
				else
				{
					SetBtn(1, "#{100623}");
					mOnClickBtnList[0] = OnClickUsItemBtn;
				}
				if (CheckLevel())
				{
					SetBtnSp(BtnSpList[0], isenable: true);
				}
				else
				{
					SetBtnSp(BtnSpList[0], isenable: false);
				}
				break;
			case GameDefine.ITEM_TYPE.RENAME:
				SetBtn(1, "#{100623}");
				mOnClickBtnList[0] = OnClickUsItemBtn;
				if (CheckLevel())
				{
					SetBtnSp(BtnSpList[0], isenable: true);
				}
				else
				{
					SetBtnSp(BtnSpList[0], isenable: false);
				}
				break;
			default:
				if (mCurItem.ItemData.CanConsign())
				{
					SetBtn(0);
				}
				else
				{
					SetBtn(0);
				}
				break;
			}
		}
		else
		{
			SetBtn(0);
		}
		for (int m = 0; m < ItemInfoRoot.UpArrowList.Length; m++)
		{
			NGUITools.SetActive(ItemInfoRoot.UpArrowList[m], state: false);
			NGUITools.SetActive(ItemInfoRoot.DownArrowList[m], state: false);
		}
	}

	private void SetBtn(int count, string btn0Name = null, string btn1Name = null, string btn2Name = null, string btn3Name = null)
	{
		for (int i = 0; i < BtnList.Length; i++)
		{
			NGUITools.SetActive(BtnList[i].gameObject, i < count);
		}
		BtnLabelList[0].text = ((!string.IsNullOrEmpty(btn0Name)) ? StrDictionary.GetDictionaryString(btn0Name) : string.Empty);
		BtnLabelList[1].text = ((!string.IsNullOrEmpty(btn1Name)) ? StrDictionary.GetDictionaryString(btn1Name) : string.Empty);
		BtnLabelList[2].text = ((!string.IsNullOrEmpty(btn2Name)) ? StrDictionary.GetDictionaryString(btn2Name) : string.Empty);
		BtnLabelList[3].text = ((!string.IsNullOrEmpty(btn3Name)) ? StrDictionary.GetDictionaryString(btn3Name) : string.Empty);
		SetBtnSp(BtnSpList[0], isenable: true);
		BtnGride.Reposition();
	}

	public void SetBtnSp(UISprite curBtn, bool isenable)
	{
		if (isenable)
		{
			curBtn.spriteName = GameDefine.BtnIcon[1];
		}
		else
		{
			curBtn.spriteName = GameDefine.BtnIcon[2];
		}
	}

	private bool CheckCanEquip(bool isShow = false)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (!playerData.CheckLevel(mCurItem.ItemData.Level))
		{
			if (isShow)
			{
				TutorialManager.LevelLimitAction();
			}
			return false;
		}
		EquipData equipDataById = DataManager.GetEquipDataById(mCurItem.ItemId);
		if (equipDataById != null && playerData.Profession != equipDataById.profession)
		{
			if (isShow)
			{
				NoticeLogic.AddNotifyData("#{100641}");
			}
			return false;
		}
		return true;
	}

	private bool CheckProfession(bool isShow = false)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		EquipData equipDataById = DataManager.GetEquipDataById(mCurItem.ItemId);
		if (equipDataById != null && playerData.Profession != equipDataById.profession)
		{
			if (isShow)
			{
				NoticeLogic.AddNotifyData("#{100641}");
			}
			return false;
		}
		return true;
	}

	private bool CheckLevel(bool isShow = false)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (!playerData.CheckLevel(mCurItem.ItemData.Level))
		{
			if (isShow)
			{
				NoticeLogic.AddNotifyData("#{100642}");
			}
			return false;
		}
		return true;
	}

	private bool CanTakeOff()
	{
		if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			ItemContainer equipBackPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipBackPack;
			if (equipBackPack.GetContainerEmptyNum() < 1)
			{
				return false;
			}
		}
		else if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.BADGE)
		{
			ItemContainer badgeBackPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BadgeBackPack;
			if (badgeBackPack.GetContainerEmptyNum() < 1)
			{
				return false;
			}
		}
		return true;
	}

	private void OnClickUseBuff(GameItem item)
	{
		OnClickCloseBtn();
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer != null)
		{
			mainPlayer.UseBuffDrag(item);
		}
	}

	private void OnClickUseDragBtn(GameItem item)
	{
		if (CheckLevel(isShow: true))
		{
			OnClickCloseBtn();
			ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			if (mainPlayer != null)
			{
				mainPlayer.UseDrag(item);
			}
		}
	}

	private void OnClickUseEnhanceBtn(GameItem item)
	{
		if (!CheckLevel(isShow: true))
		{
			return;
		}
		OnClickCloseBtn();
		SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickCloseBtn();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
		{
			if (item.ItemId.Equals("3001"))
			{
				SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance();
			}
			else if (item.ItemId.Equals("4001") || item.ItemId.Equals("4002") || item.ItemId.Equals("4003"))
			{
				SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipRefine();
			}
		});
	}

	private void OnClickConsignBtn(GameItem item)
	{
	}

	private void OnClickSellBtn(GameItem item)
	{
		int num = (int)((float)(item.ItemData.GetSellPrice(item.GetItemQuality()) * item.StackNum) * 1f);
		MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{100636}", num), StrDictionary.GetDictionaryString("#{100127}"), delegate
		{
			if (mCurItem.ItemData.Type != GameDefine.ITEM_TYPE.BADGE)
			{
				sell_item.request rpcReq = new sell_item.request
				{
					indexId = mCurItem.IndexId,
					itemCount = mCurItem.StackNum,
					type = (long)item.ContainerType
				};
				NetLogic.GetInstance().Send<Protocol.sell_item>(rpcReq);
			}
			OnClickCloseBtn();
		});
	}

	private void OnClickTakeOffBtn(GameItem item)
	{
		if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.CanSendToServer(117))
			{
				if (CanTakeOff())
				{
					Singleton<ObjManager>.Instance.MainPlayer.UnEquipItem(mCurItem);
					OnClickCloseBtn();
				}
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100272}");
			}
		}
		else if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.BADGE)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.CanSendToServer(198))
			{
				if (CanTakeOff())
				{
					Singleton<ObjManager>.Instance.MainPlayer.UnEquipBadge(mCurItem);
					OnClickCloseBtn();
				}
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100272}");
			}
		}
		else if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.CanSendToServer(221))
			{
				ItemContainer fashionEquipPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FashionEquipPack;
				Singleton<ObjManager>.Instance.MainPlayer.UnEquipFashionItem(mCurItem);
				OnClickCloseBtn();
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100272}");
			}
		}
	}

	private void OnClickEquipBtn(GameItem item)
	{
		if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			if (CheckCanEquip(isShow: true))
			{
				if (SingletonDontDestoryUnity<GameManager>.Instance.CanSendToServer(116))
				{
					Singleton<ObjManager>.Instance.MainPlayer.EquipItem(mCurItem);
					OnClickCloseBtn();
				}
				else
				{
					NoticeLogic.AddNotifyData("#{100272}");
				}
			}
		}
		else if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.BADGE)
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.BADGE_CLICK_EQUIP)
			{
				CheckTutorialEvent();
			}
			if (SingletonDontDestoryUnity<GameManager>.Instance.CanSendToServer(197))
			{
				ItemContainer badgeEquipPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BadgeEquipPack;
				int firstEmptyItemIndex = badgeEquipPack.GetFirstEmptyItemIndex();
				if (firstEmptyItemIndex > -1)
				{
					Singleton<ObjManager>.Instance.MainPlayer.EquipBadge(mCurItem, firstEmptyItemIndex);
				}
				else
				{
					NoticeLogic.AddNotifyData("#{100638}");
				}
				OnClickCloseBtn();
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100272}");
			}
		}
		else if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP && CheckCanEquip(isShow: true))
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.CanSendToServer(221))
			{
				ItemContainer fashionEquipPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FashionEquipPack;
				Singleton<ObjManager>.Instance.MainPlayer.EquipFashionItem(mCurItem);
				OnClickCloseBtn();
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100272}");
			}
		}
	}

	private void OnClickMergeBtn(GameItem item)
	{
		if (CheckFunctionUnlock(FUNCTION_TYPE.ENHANCE_BADGE))
		{
			OnClickCloseBtn();
			SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
			{
				SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowBadgeMerge(item, EquipStrengthenUIRootLogic.OPENTYPE.BADGE);
			});
		}
		else
		{
			NoticeLogic.AddNotifyData("#{100642}");
		}
	}

	private void OnClickEnhanceBtn(GameItem item)
	{
		if (!CheckFunctionUnlock(FUNCTION_TYPE.ENHANCE_EQUIP))
		{
			NoticeLogic.AddNotifyData("#{100642}");
			return;
		}
		OnClickCloseBtn();
		SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickCloseBtn();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
		{
			SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance(item, EquipStrengthenUIRootLogic.OPENTYPE.EQUIP);
		});
	}

	private void OnClickOpenBoxBtn(GameItem item)
	{
		if (!CheckLevel(isShow: true))
		{
			return;
		}
		OnClickCloseBtn();
		ItemData itemData = item.ItemData;
		open_item_package.request request = new open_item_package.request();
		if (itemData.Type == GameDefine.ITEM_TYPE.BOX)
		{
			request.indexId = item.IndexId;
			request.count = 1L;
			NetLogic.GetInstance().Send<Protocol.open_item_package>(request);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OpenBoxRoot, delegate
			{
				SingletonUnity<OpenBoxRootLogic>.Instance.Reset();
			});
		}
		else if (itemData.Type == GameDefine.ITEM_TYPE.LOCK1)
		{
			string funLock = itemData.FunLock;
			ItemContainer itemBackPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack;
			List<GameItem> itemByItemId = itemBackPack.GetItemByItemId(funLock);
			if (itemByItemId != null && itemByItemId.Count > 0)
			{
				request.indexId = item.IndexId;
				request.indexId2 = itemByItemId[0].IndexId;
				request.count = 1L;
				NetLogic.GetInstance().Send<Protocol.open_item_package>(request);
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OpenBoxRoot, delegate
				{
					SingletonUnity<OpenBoxRootLogic>.Instance.Reset();
				});
			}
			else
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(funLock);
				ShowItemsRootLogic.ShowYestOrNoBtn(itemDataByID, 1, "#{100127}", "#{100647}", LockYesBtnFun, null);
			}
		}
		else
		{
			if (itemData.Type != GameDefine.ITEM_TYPE.LOCK2)
			{
				return;
			}
			string funLock2 = itemData.FunLock;
			ItemContainer itemBackPack2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack;
			List<GameItem> itemByItemId2 = itemBackPack2.GetItemByItemId(funLock2);
			if (itemByItemId2 != null && itemByItemId2.Count > 0)
			{
				request.indexId = itemByItemId2[0].IndexId;
				request.indexId2 = item.IndexId;
				request.count = 1L;
				NetLogic.GetInstance().Send<Protocol.open_item_package>(request);
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OpenBoxRoot, delegate
				{
					SingletonUnity<OpenBoxRootLogic>.Instance.Reset();
				});
			}
			else
			{
				ItemData itemDataByID2 = DataManager.GetItemDataByID(funLock2);
				ShowItemsRootLogic.ShowYesBtn(itemDataByID2, "#{100127}", "#{100648}", null);
			}
		}
	}

	private void OnClickOpenAllBoxBtn(GameItem item)
	{
		if (!CheckLevel(isShow: true))
		{
			return;
		}
		OnClickCloseBtn();
		ItemData itemData = item.ItemData;
		open_item_package.request request = new open_item_package.request();
		if (itemData.Type == GameDefine.ITEM_TYPE.BOX)
		{
			request.indexId = item.IndexId;
			request.count = item.StackNum;
			if (request.count > 99)
			{
				request.count = 99L;
			}
			NetLogic.GetInstance().Send<Protocol.open_item_package>(request);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OpenBoxRoot, delegate
			{
				SingletonUnity<OpenBoxRootLogic>.Instance.Reset();
			});
		}
		else if (itemData.Type == GameDefine.ITEM_TYPE.LOCK1)
		{
			string funLock = itemData.FunLock;
			ItemContainer itemBackPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack;
			List<GameItem> itemByItemId = itemBackPack.GetItemByItemId(funLock);
			if (itemByItemId != null && itemByItemId.Count > 0)
			{
				request.indexId = item.IndexId;
				request.indexId2 = itemByItemId[0].IndexId;
				request.count = Mathf.Min(item.StackNum, itemBackPack.GetItemStackNumById(funLock));
				if (request.count > 99)
				{
					request.count = 99L;
				}
				NetLogic.GetInstance().Send<Protocol.open_item_package>(request);
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OpenBoxRoot, delegate
				{
					SingletonUnity<OpenBoxRootLogic>.Instance.Reset();
				});
			}
			else
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(funLock);
				ShowItemsRootLogic.ShowYestOrNoBtn(itemDataByID, 1, "#{100127}", "#{100647}", LockYesBtnFun, null);
			}
		}
		else
		{
			if (itemData.Type != GameDefine.ITEM_TYPE.LOCK2)
			{
				return;
			}
			string funLock2 = itemData.FunLock;
			ItemContainer itemBackPack2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack;
			List<GameItem> itemByItemId2 = itemBackPack2.GetItemByItemId(funLock2);
			if (itemByItemId2 != null && itemByItemId2.Count > 0)
			{
				request.indexId = itemByItemId2[0].IndexId;
				request.indexId2 = item.IndexId;
				request.count = Mathf.Min(item.StackNum, itemBackPack2.GetItemStackNumById(funLock2));
				if (request.count > 99)
				{
					request.count = 99L;
				}
				NetLogic.GetInstance().Send<Protocol.open_item_package>(request);
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OpenBoxRoot, delegate
				{
					SingletonUnity<OpenBoxRootLogic>.Instance.Reset();
				});
			}
			else
			{
				ItemData itemDataByID2 = DataManager.GetItemDataByID(funLock2);
				ShowItemsRootLogic.ShowYesBtn(itemDataByID2, "#{100127}", "#{100648}", null);
			}
		}
	}

	private void LockYesBtnFun()
	{
		GameMoneyHelper.ShowItemProduct(CurItem.ItemData.FunLock);
	}

	private void OnClickUseExChanegBtn(GameItem item)
	{
		if (CheckLevel(isShow: true))
		{
			OnClickCloseBtn();
			use_item.request request = new use_item.request();
			request.indexId = item.IndexId;
			NetLogic.GetInstance().Send<Protocol.use_item>(request);
		}
	}

	private void OnClickUsItemBtn(GameItem item)
	{
		if (CheckLevel(isShow: true))
		{
			OnClickCloseBtn();
			if (item.ItemData.Type == GameDefine.ITEM_TYPE.RENAME)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.RenameRoot);
				return;
			}
			use_item.request request = new use_item.request();
			request.indexId = item.IndexId;
			NetLogic.GetInstance().Send<Protocol.use_item>(request);
		}
	}

	public void SetLabelWarning(UILabel curlabel, bool istrue)
	{
		if (istrue)
		{
			curlabel.color = Color.red;
		}
	}

	public bool CheckFunctionUnlock(FUNCTION_TYPE type)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		return playerCommonData.IsFunctionUnlock(type);
	}

	private void OnDisable()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.BADGE_CLICK_EQUIP)
		{
			CheckTutorialEvent();
		}
	}

	public void OnClickJumpBtn1()
	{
		JumpPage(mJumpPath[0]);
	}

	public void OnClickJumpBtn2()
	{
		JumpPage(mJumpPath[1]);
	}

	public void OnClickJumpBtn3()
	{
		JumpPage(mJumpPath[2]);
	}

	public void OnClickJumpBtn4()
	{
		JumpPage(mJumpPath[3]);
	}

	private void JumpPage(JUMP_PATH jumpPath)
	{
		switch (jumpPath)
		{
		case JUMP_PATH.SHOP:
			OnClickCloseBtn();
			GameMoneyHelper.ShowItemProduct(mCurItem.ItemId);
			break;
		case JUMP_PATH.SLOT:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.LOTTO))
			{
				ClosePrePage();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SlotUIRoot, delegate
				{
					SingletonUnity<SlotUIRootLogic>.Instance.EnableReset();
					WaitResponseUIRootLogic.OpenWaitBox(242, 10f, 0f);
					NetLogic.GetInstance().Send<Protocol.request_slot_info>();
					SingletonUnity<SlotUIRootLogic>.Instance.SetPrePage(mPrePage);
				});
				OnClickCloseBtn();
			}
			else
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100834}"));
			}
			break;
		case JUMP_PATH.RACE:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				OnClickCloseBtn();
				ClosePrePage();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.CAR_CHASE_COPY);
					SingletonUnity<NewActivityUIRootLogic>.Instance.SetPrePage(mPrePage);
				});
			}
			else
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
			}
			break;
		case JUMP_PATH.SCUFFLE:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				OnClickCloseBtn();
				ClosePrePage();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.SCUFFLE_AREA_1);
					SingletonUnity<NewActivityUIRootLogic>.Instance.SetPrePage(mPrePage);
				});
			}
			else
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
			}
			break;
		case JUMP_PATH.EQUIP_COPY:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				OnClickCloseBtn();
				ClosePrePage();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.EQUIP_COPY);
					SingletonUnity<NewActivityUIRootLogic>.Instance.SetPrePage(mPrePage);
				});
			}
			else
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
			}
			break;
		}
	}

	private void ClosePrePage()
	{
		switch (mPrePage)
		{
		case UI_PAGE_TYPE.BACK_PACK_ITEM:
			if (SingletonUnity<PlayerInfoMenuRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PlayerInfoMenuRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickCloseBtn();
			}
			break;
		case UI_PAGE_TYPE.ENHANCE_EQUIP:
			if (SingletonUnity<EquipStrengthenUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<EquipStrengthenUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<EquipStrengthenUIRootLogic>.Instance.OnClickCloseBtn();
			}
			break;
		case UI_PAGE_TYPE.REFINE:
			if (SingletonUnity<EquipStrengthenUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<EquipStrengthenUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<EquipStrengthenUIRootLogic>.Instance.OnClickCloseBtn();
			}
			break;
		}
	}
}
