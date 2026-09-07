using System;
using Sproto;
using SprotoType;
using UnityEngine;

public class RefineUIRootLogic : SingletonUnity<RefineUIRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UISprite[] RefinePartIconList;

	public UISprite SelctIcon;

	public UISprite CurPartIcon;

	public UISprite CurParColorIcon;

	private Color[] parColor = new Color[5]
	{
		Color.black,
		new Color(1f, 40f / 51f, 0.29803923f, 1f),
		new Color(0.81960785f, 0.23921569f, 1f, 1f),
		new Color(1f, 16f / 51f, 16f / 51f, 1f),
		new Color(16f / 51f, 1f, 0.8f, 1f)
	};

	public UILabel CurPartNameLabel;

	public GameObject[] CurAttrLineList;

	public UILabel[] CurAttrTypeLabelList;

	public UILabel[] CurAttrValLabelList;

	public UISprite[] AttrIconPicList;

	public UILabel[] NextAttrValLabelList;

	public UISprite[] StarList;

	public GameObject[] NextLevelArrowPic;

	public UILabel costnamelabel;

	public UISprite Cost1Icon;

	public UISprite Cost1QualityIcon;

	public UISprite Cost2Icon;

	public UISprite Cost2QualityIcon;

	public UILabel Cost1NeedNumLabel;

	public UILabel Cost2NeedNumLabel;

	public UISprite CostMoneyIcon;

	public UILabel CostMoneyLabel;

	public UISprite SaftyItemIcon;

	public UILabel SaftyItemNeedNumLabel;

	public UISprite SaftyUsePic;

	public GameObject SaftyItemRoot;

	public GameObject pageRightRoot;

	public bool UseSaftyFlag;

	public UILabel CombatValueLabel;

	public GameObject[] LeftStarObj;

	public UILabel[] LeftStarlabel;

	public UIPlayTween upgradeEffect;

	public UIPlayTween[] LineEffect;

	public UIPlayTween[] starEffect;

	public UIPlayTween leftEffect;

	public GameObject UpgradeBtn;

	private RefineData[] mCurRefineData;

	private RefineData curRefineData;

	private string[] RefinePartName = new string[5]
	{
		string.Empty,
		"#{100908}",
		"#{100909}",
		"#{100910}",
		"#{100911}"
	};

	private PlayerData mPlayerData;

	private REFINE_PART mCurPagePart = REFINE_PART.NECK;

	private bool mUpgradeBtnUseFlag;

	private bool isPlayEffect;

	public REFINE_PART CurPagePart
	{
		get
		{
			return mCurPagePart;
		}
		set
		{
			mCurPagePart = value;
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

	public void ResetEnable()
	{
		leftEffect.resetOnPlay = true;
		leftEffect.Play(forward: true);
	}

	private void OnEnable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
	}

	private void OnDisable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
		isPlayEffect = false;
		if (TutorialManager.CurStep == TUTORIAL_STEP.STRENGTH_STAR_CLICK_BTN)
		{
			CheckTutorialEvent();
		}
	}

	public void UpdateMoney()
	{
		if (mPlayerData == null)
		{
			mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		}
		if (curRefineData == null)
		{
			return;
		}
		int part = (int)mCurPagePart;
		int targetRefinePartLevel = mPlayerData.MainPlayerAttrData.GetTargetRefinePartLevel(mCurPagePart);
		RefineData refineData = null;
		if (targetRefinePartLevel < GameDefine.MAX_REFINE_LEVEL)
		{
			refineData = DataManager.GetRefineDataByPartLevelPRO(part, targetRefinePartLevel + 1, (int)mPlayerData.Profession);
		}
		if (refineData != null)
		{
			CostMoneyLabel.text = $"{curRefineData.MoneyCost}";
			if (curRefineData.MoneyCost <= GameMoneyHelper.GetMoneyNum(0))
			{
				CostMoneyLabel.color = Color.white;
			}
			else
			{
				CostMoneyLabel.color = Color.red;
			}
		}
	}

	public void Show()
	{
		ResetEnable();
		if (mPlayerData == null)
		{
			mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		}
		if (mCurRefineData == null)
		{
			mCurRefineData = new RefineData[5];
		}
		for (int i = 1; i < 5; i++)
		{
			UpdateCurRefineData((REFINE_PART)i);
		}
		ResetPate();
	}

	private void UpdateCurRefineData(REFINE_PART targetPart)
	{
		mCurRefineData[(int)targetPart] = DataManager.GetRefineDataByPartLevelPRO((int)targetPart, mPlayerData.MainPlayerAttrData.GetTargetRefinePartLevel(targetPart), (int)mPlayerData.Profession);
	}

	public void OnClickTips()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
		{
			SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{100907}", "#{100906}", null);
		});
	}

	public void Hide()
	{
	}

	public void ResetPate()
	{
		mUpgradeBtnUseFlag = false;
		UpdateLeftPage();
		UpdateRightPage(mCurPagePart);
	}

	public void UpdateLeftPage()
	{
		UpdateLeftStar();
		for (int i = 1; i < RefinePartIconList.Length; i++)
		{
			RefinePartIconList[i].spriteName = mCurRefineData[i].ICON;
		}
	}

	private void UpdateLeftStar()
	{
		int num = 0;
		int targetRefinePartLevel = mPlayerData.MainPlayerAttrData.GetTargetRefinePartLevel(REFINE_PART.NECK);
		if (targetRefinePartLevel < 1)
		{
			UnityVersionUtil.SetActiveRecursive(LeftStarObj[0], state: false);
		}
		else
		{
			curRefineData = mCurRefineData[1];
			num += curRefineData.GetCombatValue();
			UnityVersionUtil.SetActiveRecursive(LeftStarObj[0], state: true);
			LeftStarlabel[0].text = targetRefinePartLevel.ToString();
		}
		targetRefinePartLevel = mPlayerData.MainPlayerAttrData.GetTargetRefinePartLevel(REFINE_PART.RING1);
		if (targetRefinePartLevel < 1)
		{
			UnityVersionUtil.SetActiveRecursive(LeftStarObj[1], state: false);
		}
		else
		{
			curRefineData = mCurRefineData[2];
			num += curRefineData.GetCombatValue();
			UnityVersionUtil.SetActiveRecursive(LeftStarObj[1], state: true);
			LeftStarlabel[1].text = targetRefinePartLevel.ToString();
		}
		targetRefinePartLevel = mPlayerData.MainPlayerAttrData.GetTargetRefinePartLevel(REFINE_PART.RING2);
		if (targetRefinePartLevel < 1)
		{
			UnityVersionUtil.SetActiveRecursive(LeftStarObj[2], state: false);
		}
		else
		{
			curRefineData = mCurRefineData[3];
			num += curRefineData.GetCombatValue();
			UnityVersionUtil.SetActiveRecursive(LeftStarObj[2], state: true);
			LeftStarlabel[2].text = targetRefinePartLevel.ToString();
		}
		targetRefinePartLevel = mPlayerData.MainPlayerAttrData.GetTargetRefinePartLevel(REFINE_PART.BELT);
		if (targetRefinePartLevel < 1)
		{
			UnityVersionUtil.SetActiveRecursive(LeftStarObj[3], state: false);
		}
		else
		{
			curRefineData = mCurRefineData[4];
			num += curRefineData.GetCombatValue();
			UnityVersionUtil.SetActiveRecursive(LeftStarObj[3], state: true);
			LeftStarlabel[3].text = targetRefinePartLevel.ToString();
		}
		CombatValueLabel.text = num.ToString();
	}

	public void UpdateRightPage(REFINE_PART updatePart)
	{
		UnityVersionUtil.SetActiveRecursive(pageRightRoot, state: true);
		mCurPagePart = updatePart;
		int num = (int)mCurPagePart;
		int targetRefinePartLevel = mPlayerData.MainPlayerAttrData.GetTargetRefinePartLevel(mCurPagePart);
		CurPartNameLabel.text = StrDictionary.GetDictionaryString(RefinePartName[num]);
		curRefineData = mCurRefineData[num];
		RefineData refineData = null;
		SelctIcon.transform.position = RefinePartIconList[num].transform.position;
		if (targetRefinePartLevel < GameDefine.MAX_REFINE_LEVEL)
		{
			refineData = DataManager.GetRefineDataByPartLevelPRO(num, targetRefinePartLevel + 1, (int)mPlayerData.Profession);
		}
		CurPartIcon.spriteName = curRefineData.ICON;
		CurParColorIcon.color = parColor[(int)updatePart];
		CurAttrTypeLabelList[0].text = GameDefine.GetAttributeName_S(curRefineData.Stat1);
		AttrIconPicList[0].spriteName = GameDefine.GetAttributeIcon(curRefineData.Stat1);
		CurAttrValLabelList[0].text = GameDefine.GetAttributeValueStr2(curRefineData.Stat1, curRefineData.Value1);
		if (refineData != null)
		{
			NextAttrValLabelList[0].text = GameDefine.GetAttributeValueStr2(refineData.Stat1, refineData.Value1);
			UnityVersionUtil.SetActiveRecursive(NextLevelArrowPic[0], state: true);
			UnityVersionUtil.SetActiveRecursive(NextAttrValLabelList[0].gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(NextLevelArrowPic[0], state: false);
			UnityVersionUtil.SetActiveRecursive(NextAttrValLabelList[0].gameObject, state: false);
		}
		if (curRefineData.Stat2 == 0)
		{
			if (refineData == null || refineData.Stat2 == 0)
			{
				UnityVersionUtil.SetActiveRecursive(CurAttrLineList[1], state: false);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(CurAttrLineList[1], state: true);
				CurAttrTypeLabelList[1].text = GameDefine.GetAttributeName_S(curRefineData.Stat2);
				AttrIconPicList[1].spriteName = GameDefine.GetAttributeIcon(curRefineData.Stat2);
				CurAttrValLabelList[1].text = GameDefine.GetAttributeValueStr2(refineData.Stat2, 0);
				NextAttrValLabelList[1].text = GameDefine.GetAttributeValueStr2(refineData.Stat2, refineData.Value2);
				UnityVersionUtil.SetActiveRecursive(NextLevelArrowPic[1], state: true);
			}
		}
		else
		{
			CurAttrTypeLabelList[1].text = GameDefine.GetAttributeName_S(curRefineData.Stat2);
			AttrIconPicList[1].spriteName = GameDefine.GetAttributeIcon(curRefineData.Stat2);
			CurAttrValLabelList[1].text = GameDefine.GetAttributeValueStr2(curRefineData.Stat2, curRefineData.Value2);
			if (refineData != null)
			{
				NextAttrValLabelList[1].text = GameDefine.GetAttributeValueStr2(refineData.Stat2, refineData.Value2);
				UnityVersionUtil.SetActiveRecursive(NextLevelArrowPic[1], state: true);
				UnityVersionUtil.SetActiveRecursive(NextAttrValLabelList[1].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(NextLevelArrowPic[1], state: false);
				UnityVersionUtil.SetActiveRecursive(NextAttrValLabelList[1].gameObject, state: false);
			}
		}
		if (curRefineData.Stat3 == 0)
		{
			if (refineData == null || refineData.Stat3 == 0)
			{
				UnityVersionUtil.SetActiveRecursive(CurAttrLineList[2], state: false);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(CurAttrLineList[2], state: true);
				CurAttrTypeLabelList[2].text = GameDefine.GetAttributeName_S(curRefineData.Stat3);
				AttrIconPicList[2].spriteName = GameDefine.GetAttributeIcon(curRefineData.Stat3);
				CurAttrValLabelList[2].text = GameDefine.GetAttributeValueStr2(refineData.Stat3, 0);
				NextAttrValLabelList[2].text = GameDefine.GetAttributeValueStr2(refineData.Stat3, refineData.Value3);
				UnityVersionUtil.SetActiveRecursive(NextLevelArrowPic[2], state: true);
			}
		}
		else
		{
			CurAttrTypeLabelList[2].text = GameDefine.GetAttributeName_S(curRefineData.Stat3);
			AttrIconPicList[2].spriteName = GameDefine.GetAttributeIcon(curRefineData.Stat3);
			CurAttrValLabelList[2].text = GameDefine.GetAttributeValueStr2(curRefineData.Stat3, curRefineData.Value3);
			if (refineData != null)
			{
				NextAttrValLabelList[2].text = GameDefine.GetAttributeValueStr2(refineData.Stat3, refineData.Value3);
				UnityVersionUtil.SetActiveRecursive(NextLevelArrowPic[2], state: true);
				UnityVersionUtil.SetActiveRecursive(NextAttrValLabelList[2].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(NextLevelArrowPic[2], state: false);
				UnityVersionUtil.SetActiveRecursive(NextAttrValLabelList[2].gameObject, state: false);
			}
		}
		if (curRefineData.Stat4 == 0)
		{
			if (refineData == null || refineData.Stat4 == 0)
			{
				UnityVersionUtil.SetActiveRecursive(CurAttrLineList[3], state: false);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(CurAttrLineList[3], state: true);
				CurAttrTypeLabelList[3].text = GameDefine.GetAttributeName_S(curRefineData.Stat4);
				AttrIconPicList[3].spriteName = GameDefine.GetAttributeIcon(curRefineData.Stat4);
				CurAttrValLabelList[3].text = GameDefine.GetAttributeValueStr2(refineData.Stat4, 0);
				NextAttrValLabelList[3].text = GameDefine.GetAttributeValueStr2(refineData.Stat4, refineData.Value4);
				UnityVersionUtil.SetActiveRecursive(NextLevelArrowPic[3], state: true);
			}
		}
		else
		{
			CurAttrTypeLabelList[3].text = GameDefine.GetAttributeName_S(curRefineData.Stat4);
			AttrIconPicList[3].spriteName = GameDefine.GetAttributeIcon(curRefineData.Stat4);
			CurAttrValLabelList[3].text = GameDefine.GetAttributeValueStr2(curRefineData.Stat4, curRefineData.Value4);
			if (refineData != null)
			{
				NextAttrValLabelList[3].text = GameDefine.GetAttributeValueStr2(refineData.Stat4, refineData.Value4);
				UnityVersionUtil.SetActiveRecursive(NextLevelArrowPic[3], state: true);
				UnityVersionUtil.SetActiveRecursive(NextAttrValLabelList[3].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(NextLevelArrowPic[3], state: false);
				UnityVersionUtil.SetActiveRecursive(NextAttrValLabelList[3].gameObject, state: false);
			}
		}
		for (int i = 0; i < StarList.Length; i++)
		{
			if (i < targetRefinePartLevel)
			{
				UnityVersionUtil.SetActiveRecursive(StarList[i].gameObject, state: true);
				StarList[i].color = Color.white;
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(StarList[i].gameObject, state: false);
			}
		}
		if (isPlayEffect && targetRefinePartLevel < GameDefine.MAX_REFINE_LEVEL && targetRefinePartLevel >= 1)
		{
			starEffect[targetRefinePartLevel - 1].resetOnPlay = true;
			starEffect[targetRefinePartLevel - 1].Play(forward: true);
			isPlayEffect = false;
		}
		if (refineData == null)
		{
			UnityVersionUtil.SetActiveRecursive(Cost1Icon.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(Cost1NeedNumLabel.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(Cost2Icon.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(Cost2NeedNumLabel.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(CostMoneyIcon.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(CostMoneyLabel.gameObject, state: false);
			costnamelabel.enabled = false;
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(Cost1Icon.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(Cost1NeedNumLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(Cost2Icon.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(Cost2NeedNumLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(CostMoneyIcon.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(CostMoneyLabel.gameObject, state: true);
			costnamelabel.enabled = true;
			RefershItemUI();
			CostMoneyLabel.text = $"{curRefineData.MoneyCost}";
			if (curRefineData.MoneyCost <= GameMoneyHelper.GetMoneyNum(0))
			{
				CostMoneyLabel.color = Color.white;
			}
			else
			{
				CostMoneyLabel.color = Color.red;
			}
		}
		if (targetRefinePartLevel < 3)
		{
			UnityVersionUtil.SetActiveRecursive(SaftyItemRoot, state: false);
			UseSaftyFlag = false;
		}
		else if (!string.IsNullOrEmpty(curRefineData.SaftyId) && curRefineData.Chance < 100)
		{
			UnityVersionUtil.SetActiveRecursive(SaftyItemRoot, state: true);
			ItemData itemDataByID = DataManager.GetItemDataByID(curRefineData.SaftyId);
			SaftyItemIcon.spriteName = itemDataByID.BackPackIcon;
			int itemStackNumById = mPlayerData.ItemBackPack.GetItemStackNumById(curRefineData.SaftyId);
			SaftyItemNeedNumLabel.text = $"{curRefineData.SaftyCost}/{itemStackNumById}";
			if (itemStackNumById >= curRefineData.SaftyCost)
			{
				SaftyItemNeedNumLabel.color = Color.white;
				UseSaftyItem();
			}
			else
			{
				SaftyItemNeedNumLabel.color = Color.red;
				NotUseSaftyItem();
			}
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(SaftyItemRoot, state: false);
			UseSaftyFlag = false;
		}
	}

	public void RefershItemUI()
	{
		ItemData itemDataByID = DataManager.GetItemDataByID(curRefineData.CostId1);
		if (itemDataByID == null)
		{
			return;
		}
		Cost1Icon.spriteName = itemDataByID.BackPackIcon;
		Cost1QualityIcon.spriteName = itemDataByID.QualityType.ToString();
		int itemStackNumById = mPlayerData.ItemBackPack.GetItemStackNumById(curRefineData.CostId1);
		Cost1NeedNumLabel.text = $"{curRefineData.Cost1}/{itemStackNumById}";
		if (curRefineData.Cost1 > itemStackNumById)
		{
			Cost1NeedNumLabel.color = Color.red;
		}
		else
		{
			Cost1NeedNumLabel.color = Color.white;
		}
		if (string.IsNullOrEmpty(curRefineData.CostId2))
		{
			UnityVersionUtil.SetActiveRecursive(Cost2Icon.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(Cost2NeedNumLabel.gameObject, state: false);
		}
		else
		{
			ItemData itemDataByID2 = DataManager.GetItemDataByID(curRefineData.CostId2);
			Cost2Icon.spriteName = itemDataByID2.BackPackIcon;
			int itemStackNumById2 = mPlayerData.ItemBackPack.GetItemStackNumById(curRefineData.CostId2);
			Cost2NeedNumLabel.text = $"{curRefineData.Cost2}/{itemStackNumById2}";
			Cost2QualityIcon.spriteName = itemDataByID2.QualityType.ToString();
			if (curRefineData.Cost2 > itemStackNumById2)
			{
				Cost2NeedNumLabel.color = Color.red;
			}
			else
			{
				Cost2NeedNumLabel.color = Color.white;
			}
		}
		if (!string.IsNullOrEmpty(curRefineData.SaftyId) && curRefineData.Chance < 100)
		{
			ItemData itemDataByID3 = DataManager.GetItemDataByID(curRefineData.SaftyId);
			SaftyItemIcon.spriteName = itemDataByID3.BackPackIcon;
			int itemStackNumById3 = mPlayerData.ItemBackPack.GetItemStackNumById(curRefineData.SaftyId);
			SaftyItemNeedNumLabel.text = $"{curRefineData.SaftyCost}/{itemStackNumById3}";
			if (itemStackNumById3 >= curRefineData.SaftyCost)
			{
				SaftyItemNeedNumLabel.color = Color.white;
				UseSaftyItem();
			}
			else
			{
				SaftyItemNeedNumLabel.color = Color.red;
				NotUseSaftyItem();
			}
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(SaftyItemRoot, state: false);
			UseSaftyFlag = false;
		}
	}

	private void UseSaftyItem()
	{
		UseSaftyFlag = true;
		SaftyUsePic.enabled = true;
	}

	private void NotUseSaftyItem()
	{
		UseSaftyFlag = false;
		SaftyUsePic.enabled = false;
	}

	public void OnClickUseSaftyItemBtn()
	{
		if (UseSaftyFlag)
		{
			NotUseSaftyItem();
			return;
		}
		RefineData refineData = mCurRefineData[(int)mCurPagePart];
		int itemStackNumById = mPlayerData.ItemBackPack.GetItemStackNumById(refineData.SaftyId);
		if (!string.IsNullOrEmpty(refineData.SaftyId) && refineData.SaftyCost <= itemStackNumById)
		{
			UseSaftyItem();
			return;
		}
		ItemData itemDataByID = DataManager.GetItemDataByID(refineData.SaftyId);
		GameMoneyHelper.ShowItemProduct(refineData.SaftyId);
	}

	public void OnClickRefineCost1()
	{
		RefineData refineData = mCurRefineData[(int)mCurPagePart];
		if (refineData != null)
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(refineData.CostId1);
			if (itemDataByID != null)
			{
				ItemInfoRootLogicNew.ShowItemTips(itemDataByID, isNeedShowJumpPath: true, UI_PAGE_TYPE.REFINE);
			}
		}
	}

	public void OnClickRefineCost2()
	{
		RefineData refineData = mCurRefineData[(int)mCurPagePart];
		if (refineData != null && !string.IsNullOrEmpty(refineData.CostId2))
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(refineData.CostId2);
			if (itemDataByID != null)
			{
				ItemInfoRootLogicNew.ShowItemTips(itemDataByID, isNeedShowJumpPath: true, UI_PAGE_TYPE.REFINE);
			}
		}
	}

	public void OnClickRefineMoeny()
	{
		ItemData moneyItemData = GameMoneyHelper.GetMoneyItemData(GameDefine.MONEY_TYPE.CASH);
		if (moneyItemData != null)
		{
			ItemInfoRootLogicNew.ShowItemTips(moneyItemData, isNeedShowJumpPath: true, UI_PAGE_TYPE.REFINE);
		}
	}

	public void OnClickSafeInfo()
	{
		RefineData refineData = mCurRefineData[(int)mCurPagePart];
		if (refineData != null)
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(refineData.SaftyId);
			if (itemDataByID != null)
			{
				ItemInfoRootLogicNew.ShowItemTips(itemDataByID, isNeedShowJumpPath: true, UI_PAGE_TYPE.REFINE);
			}
		}
	}

	public void OnClickRefineUpgradeBtn()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.STRENGTH_STAR_CLICK_BTN)
		{
			CheckTutorialEvent();
		}
		if (mUpgradeBtnUseFlag)
		{
			return;
		}
		RefineData refineData = mCurRefineData[(int)mCurPagePart];
		RefineData refineDataByPartLevelPRO = DataManager.GetRefineDataByPartLevelPRO(refineData.Part, refineData.Lv + 1, (int)mPlayerData.Profession);
		if (refineData.Lv < GameDefine.MAX_REFINE_LEVEL)
		{
			if (!GameMoneyHelper.BeforeCheckBuy(refineData.MoneyType, refineData.MoneyCost))
			{
				return;
			}
			int itemStackNumById = mPlayerData.ItemBackPack.GetItemStackNumById(refineData.CostId1);
			if (itemStackNumById < refineData.Cost1)
			{
				GameMoneyHelper.ShowItemProduct(refineData.CostId1);
				return;
			}
			if (!string.IsNullOrEmpty(refineData.CostId2))
			{
				int itemStackNumById2 = mPlayerData.ItemBackPack.GetItemStackNumById(refineData.CostId2);
				if (itemStackNumById2 < refineData.Cost2)
				{
					GameMoneyHelper.ShowItemProduct(refineData.CostId2);
					return;
				}
			}
			RefineData refineData2 = null;
			if (refineData.Lv > 0)
			{
				refineData2 = DataManager.GetRefineDataByPartLevelPRO(refineData.Part, refineData.Lv - 1, (int)mPlayerData.Profession);
			}
			equip_refine.request request = new equip_refine.request();
			request.Id = refineDataByPartLevelPRO.ID;
			request.curId = refineData.ID;
			request.partId = refineDataByPartLevelPRO.Part;
			request.level = refineDataByPartLevelPRO.Lv;
			request.safe = UseSaftyFlag;
			if (refineData2 != null)
			{
				request.preId = refineData2.ID;
			}
			NetLogic.GetInstance().Send<Protocol.equip_refine>(request, OnRefineUpgradeResponse);
			mUpgradeBtnUseFlag = true;
		}
		else
		{
			Debug.Log("Max Level");
		}
	}

	public void PlayEffect()
	{
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(18);
		upgradeEffect.resetOnPlay = true;
		upgradeEffect.Play(forward: true);
		for (int i = 0; i < LineEffect.Length; i++)
		{
			LineEffect[i].resetOnPlay = true;
			LineEffect[i].Play(forward: true);
		}
		isPlayEffect = true;
	}

	private void OnRefineUpgradeResponse(SprotoTypeBase rpcRsp)
	{
		mUpgradeBtnUseFlag = false;
		if (!(rpcRsp is equip_refine.response response))
		{
			return;
		}
		if (response.state == 0L)
		{
			NoticeLogic.AddNotifyData("#{100919}");
			if (this != null && UnityVersionUtil.IsActive(base.gameObject))
			{
				PlayEffect();
			}
		}
		else if (response.state == 2)
		{
			NoticeLogic.AddNotifyData("#{100920}");
		}
		else
		{
			NoticeLogic.AddNotifyData("#{100920}");
		}
		mPlayerData.MainPlayerAttrData.SetTargetRefinePartLevel((REFINE_PART)response.partId, (int)response.level);
		if (response.HasAllstar)
		{
			mPlayerData.MainPlayerAttrData.RefineLevel = (int)response.allstar;
		}
		if (this != null && UnityVersionUtil.IsActive(base.gameObject))
		{
			UpdateCurRefineData((REFINE_PART)response.partId);
			UpdateLeftPage();
			if ((long)mCurPagePart == response.partId)
			{
				UpdateRightPage(mCurPagePart);
			}
		}
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.UpdateEnhanceTips();
		if (response.state == 0L)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Refine", $"part_{response.partId}", "success");
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Refine", $"part_{response.partId}", $"level_{response.level}");
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Refine", $"part_{response.partId}", "failure");
		}
	}

	public void OnClickRefineLevelInfoBtn()
	{
	}

	public void OnClickRefineNackBtn()
	{
		UpdateRightPage(REFINE_PART.NECK);
	}

	public void OnClickRefineRing1Btn()
	{
		UpdateRightPage(REFINE_PART.RING1);
	}

	public void OnClickRefineRing2Btn()
	{
		UpdateRightPage(REFINE_PART.RING2);
	}

	public void OnClickRefineBeltBtn()
	{
		UpdateRightPage(REFINE_PART.BELT);
	}

	public void OnClickTipBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
		{
			SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{100906}", "#{100907}", null);
		});
	}
}
