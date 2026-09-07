using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class EnhanceUIRootLogic : SingletonUnity<EnhanceUIRootLogic>
{
	private int mClickUpgradeBtnCount;

	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	private vp_Timer.Handle clickHandle;

	private GameItem mCurItem;

	private EquipData mCurEquipData;

	private GameItem mCurCaiLiao;

	public QiangHuaListLogic QiangHuaEquipList;

	public UISprite CaiLiaoSP;

	public UISprite CaiLiaoQualitySprite;

	public UISprite CurItemIcon;

	public UILabel EquipNameLab;

	public UILabel CurLevelLab;

	public UILabel NextLevelLab;

	public UISprite ItemQualityIcon;

	public UIGrid ItemGrid;

	public List<UILabel> EquipDataLab;

	public List<UILabel> NextLevelEquipDataLab;

	public List<GameObject> EquipLabs;

	public List<UILabel> EquipDataNameLab;

	public List<UISprite> EquipIconSprite;

	public GameObject Cost;

	public UIWidget UpLevelBtn;

	public UIWidget UpLevelAllBtn;

	public UILabel NeedStackLab;

	private int CurStack = -1;

	private int NeedStack;

	public UILabel NeedMoneyLab;

	public UISprite LevelProgress;

	private int NeedMoney;

	private bool MaxLevelFlag;

	private PlayerData mplayerdata;

	public ParticleSystem EquipEffect;

	public UIPlayTween[] lineEffect;

	private List<GameItem> mEnhancePartList = new List<GameItem>();

	public GameItem CurItem
	{
		get
		{
			return mCurItem;
		}
		set
		{
			mCurItem = value;
		}
	}

	public EquipData CurEquipData
	{
		get
		{
			return mCurEquipData;
		}
		set
		{
			mCurEquipData = value;
		}
	}

	public GameItem CurCaiLiao
	{
		get
		{
			return mCurCaiLiao;
		}
		set
		{
			mCurCaiLiao = value;
		}
	}

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mClickUpgradeBtnCount = 0;
		mOnClickTutorialBtn = tutorialEvent;
		if (TutorialManager.CurStep == TUTORIAL_STEP.ENHANCE_CLICK)
		{
			clickHandle = new vp_Timer.Handle();
		}
	}

	private void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			TutorialManager.OnClickTutorialBtn onClickTutorialBtn = mOnClickTutorialBtn;
			mOnClickTutorialBtn = null;
			onClickTutorialBtn();
			onClickTutorialBtn = null;
		}
	}

	private void UpgradeBtnTutorialCheck()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.ENHANCE_CLICK)
		{
			mClickUpgradeBtnCount++;
			NGUITools.SetActive(SingletonUnity<TutorialUIRootLogic>.Instance.HandTipTweenS.gameObject, state: false);
			NGUITools.SetActive(SingletonUnity<TutorialUIRootLogic>.Instance.TipCircleSprite.gameObject, state: false);
			if (clickHandle != null)
			{
				clickHandle.Cancel();
			}
			vp_Timer.In(0.5f, delegate
			{
				NGUITools.SetActive(SingletonUnity<TutorialUIRootLogic>.Instance.HandTipTweenS.gameObject, state: true);
				NGUITools.SetActive(SingletonUnity<TutorialUIRootLogic>.Instance.TipCircleSprite.gameObject, state: true);
			}, clickHandle);
			if (mClickUpgradeBtnCount >= 3)
			{
				CheckTutorialEvent();
			}
		}
	}

	private void CloseTutorial()
	{
		if (mOnClickTutorialBtn != null)
		{
			TutorialManager.CloseTutorial();
			if (TutorialManager.CurStep == TUTORIAL_STEP.ENHANCE_CLICK)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.SetTutorialShowFinish(FUNCTION_TYPE.ENHANCE);
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		Init();
	}

	public void Init()
	{
		QiangHuaEquipList.Init();
		QiangHuaListLogic qiangHuaEquipList = QiangHuaEquipList;
		qiangHuaEquipList.onClickEquipItem = (EquipItemLogic.OnClickItem)Delegate.Combine(qiangHuaEquipList.onClickEquipItem, new EquipItemLogic.OnClickItem(OnClickEquipItem));
	}

	private void OnEnable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
	}

	private void OnDisable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
	}

	public void UpdateMoney()
	{
		if (CurItem != null)
		{
			mCurEquipData = DataManager.GetEquipDataById(CurItem.ItemId);
			NeedMoney = CurEquipData.GetUpgradeMoneyByQualityAndLevel((int)CurItem.GetItemQuality(), GetCurItemLevel());
			NeedMoneyLab.text = NeedMoney.ToString();
			if (NeedMoney <= GameMoneyHelper.GetMoneyNum(0))
			{
				NeedMoneyLab.color = Color.white;
			}
			else
			{
				NeedMoneyLab.color = Color.red;
			}
		}
	}

	private int GetCurItemLevel()
	{
		if (CurItem == null || CurItem.IsEmpty())
		{
			return 0;
		}
		if (GameManager.IsSupportCurDataVersion137())
		{
			if (mplayerdata == null)
			{
				mplayerdata = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			}
			return mplayerdata.MainPlayerAttrData.EquipEnhanceList[CurItem.ItemData.SubType];
		}
		return CurItem.ItemLevel;
	}

	private void OnClickEquipItem(GameItem item)
	{
		if (item == null || item.IndexId == -1)
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100659}"));
			return;
		}
		if (item != null && mCurItem != null && mCurItem.IndexId == item.IndexId)
		{
			int itemLevel = 0;
			if (item.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				itemLevel = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(item.ItemData.SubType);
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRootNew, delegate
			{
				item.ItemLevel = itemLevel;
				SingletonUnity<ItemInfoRootLogicNew>.Instance.Reset(item, ITEM_SHOW_TYPE.REWARD_TIPS);
			});
		}
		QiangHuaEquipList.UpdateEquipInfo(item);
		UpdateEquipUpdateInfoPage(item);
	}

	public void Show(GameItem item)
	{
		Reset(item);
	}

	public void OnClickSelctItem()
	{
		if (mCurItem != null)
		{
			int itemLevel = 0;
			if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				itemLevel = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(mCurItem.ItemData.SubType);
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRootNew, delegate
			{
				mCurItem.ItemLevel = itemLevel;
				SingletonUnity<ItemInfoRootLogicNew>.Instance.Reset(mCurItem, ITEM_SHOW_TYPE.REWARD_TIPS);
			});
		}
	}

	public void OnClickCaoLiao()
	{
		if (mCurCaiLiao != null)
		{
			ItemInfoRootLogicNew.ShowItemTips(mCurCaiLiao, 0, isNeedShowJumpPath: true, UI_PAGE_TYPE.ENHANCE_EQUIP);
		}
	}

	public void OnClikcHuoBi()
	{
		ItemData moneyItemData = GameMoneyHelper.GetMoneyItemData(GameDefine.MONEY_TYPE.CASH);
		if (moneyItemData != null)
		{
			ItemInfoRootLogicNew.ShowItemTips(moneyItemData, isNeedShowJumpPath: true, UI_PAGE_TYPE.ENHANCE_EQUIP);
		}
	}

	public void RefershUI()
	{
		Reset(CurItem, resetpos: false);
	}

	public void Reset(GameItem item, bool resetpos = true)
	{
		mplayerdata = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		List<GameItem> itemList = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipPack.ItemList;
		itemList = ItemContainerTool.SortItemList(itemList);
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < 6; i++)
		{
			list.Add(new GameItem());
			list[i].IndexId = -1L;
		}
		for (int j = 0; j < itemList.Count; j++)
		{
			if (itemList[j] != null && !itemList[j].IsEmpty())
			{
				list[itemList[j].ItemData.SubType] = new GameItem(itemList[j].IndexId, itemList[j].ContainerType, itemList[j].ItemId, itemList[j].BindFlag, itemList[j].StackNum, itemList[j].Quality);
				list[itemList[j].ItemData.SubType].SetAttInfo(itemList[j].Appraise, itemList[j].Random_AttriDic, itemList[j].InlayDic);
			}
		}
		if (GameManager.IsSupportCurDataVersion137())
		{
			for (int k = 0; k < list.Count; k++)
			{
				if (list[k].IndexId == -1)
				{
					list[k].ItemId = $"{60001 + k}";
					list[k].Quality = EQUIP_QUALITY.KUANG_PURPLE;
				}
				list[k].ItemLevel = mplayerdata.MainPlayerAttrData.GetEquipEnhanceLevel(list[k].ItemData.SubType);
			}
		}
		mEnhancePartList = list;
		if (GameManager.IsSupportCurDataVersion137())
		{
			QiangHuaEquipList.ResetEuipListInfo(list, list[item.ItemData.SubType], resetpos);
			UpdateEquipUpdateInfoPage(list[item.ItemData.SubType]);
		}
		else
		{
			QiangHuaEquipList.ResetEuipListInfo(itemList, item, resetpos);
			UpdateEquipUpdateInfoPage(item);
		}
	}

	public void UpdateEquipUpdateInfoPage(GameItem item)
	{
		if (mEnhancePartList == null || mEnhancePartList.Count == 0)
		{
			return;
		}
		mplayerdata = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		CurItem = item;
		CurCaiLiao = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack.GetEnhanceItem();
		if (CurCaiLiao == null)
		{
			CurStack = 0;
		}
		else
		{
			CurStack = CurCaiLiao.StackNum;
		}
		ItemData itemDataByID = DataManager.GetItemDataByID("3001");
		CurCaiLiao = new GameItem(itemDataByID.ID, itemDataByID.QualityType, 1);
		CaiLiaoSP.spriteName = itemDataByID.BackPackIcon;
		CaiLiaoQualitySprite.spriteName = itemDataByID.QualityType.ToString();
		if (CurItem != null)
		{
			UnityVersionUtil.SetActiveRecursive(Cost, state: true);
			NGUITools.SetActive(UpLevelBtn.gameObject, state: true);
			NGUITools.SetActive(UpLevelAllBtn.gameObject, state: true);
			mCurEquipData = DataManager.GetEquipDataById(CurItem.ItemId);
			ItemData itemDataByID2 = DataManager.GetItemDataByID(CurItem.ItemId);
			EquipNameLab.text = itemDataByID2.MName;
			EquipNameLab.color = GameDefine.GetColorByQuality(CurItem.GetItemQuality());
			CurItemIcon.spriteName = itemDataByID2.BackPackIcon;
			float num = (float)GetCurItemLevel() / (float)mplayerdata.Level;
			if (num > 1f)
			{
				num = 1f;
			}
			LevelProgress.fillAmount = 0.09f + num * 0.9f;
			if (GetCurItemLevel() < mplayerdata.Level)
			{
				MaxLevelFlag = false;
				if (GameManager.IsSupportCurDataVersion137())
				{
					CurLevelLab.text = $"LV.{mplayerdata.MainPlayerAttrData.EquipEnhanceList[CurItem.ItemData.SubType]}";
				}
				else
				{
					CurLevelLab.text = $"LV.{GetCurItemLevel()}";
				}
				NextLevelLab.text = $"LV.{GetCurItemLevel() + 1}";
			}
			else
			{
				MaxLevelFlag = true;
				if (GameManager.IsSupportCurDataVersion137())
				{
					CurLevelLab.text = $"LV.{mplayerdata.MainPlayerAttrData.EquipEnhanceList[CurItem.ItemData.SubType]}";
				}
				else
				{
					CurLevelLab.text = $"LV.{GetCurItemLevel()}";
				}
				NextLevelLab.text = "Lv.Max";
			}
			if (itemDataByID2.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				UnityVersionUtil.SetActiveRecursive(ItemQualityIcon.gameObject, state: true);
				ItemQualityIcon.spriteName = CurItem.GetItemQuality().ToString();
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(ItemQualityIcon.gameObject, state: true);
				ItemQualityIcon.spriteName = itemDataByID2.QualityType.ToString();
			}
			for (int i = 0; i < EquipDataLab.Count; i++)
			{
				if (CurItem.IndexId != -1 && i < mCurEquipData.GetBaseAttCount())
				{
					NGUITools.SetActive(EquipLabs[i].gameObject, state: true);
					EquipDataNameLab[i].text = GameDefine.GetAttributeName_S(mCurEquipData.GetAttrIDByQuality(i));
					EquipIconSprite[i].spriteName = GameDefine.GetAttributeIcon(mCurEquipData.GetAttrIDByQuality(i));
					EquipDataLab[i].text = mCurEquipData.GetAttrValByQualityAndLevel(i, (int)CurItem.GetItemQuality(), GetCurItemLevel()).ToString();
					if (MaxLevelFlag)
					{
						if (mplayerdata.Level < GameDefine.PLAYER_MAX_LEVEL)
						{
							NextLevelEquipDataLab[i].text = mCurEquipData.GetAttrValByQualityAndLevel(i, (int)CurItem.GetItemQuality(), GetCurItemLevel() + 1).ToString();
						}
						else
						{
							NextLevelEquipDataLab[i].text = mCurEquipData.GetAttrValByQualityAndLevel(i, (int)CurItem.GetItemQuality(), GetCurItemLevel()).ToString();
						}
					}
					else
					{
						NextLevelEquipDataLab[i].text = mCurEquipData.GetAttrValByQualityAndLevel(i, (int)CurItem.GetItemQuality(), GetCurItemLevel() + 1).ToString();
					}
				}
				else
				{
					NGUITools.SetActive(EquipLabs[i].gameObject, state: false);
				}
			}
			NeedStack = CurEquipData.GetUpgradeExpValByLevel(GetCurItemLevel());
			NeedStackLab.text = $"{NeedStack}/{CurStack}";
			if (NeedStack > CurStack)
			{
				NeedStackLab.color = Color.red;
			}
			else
			{
				NeedStackLab.color = Color.white;
			}
			NeedMoney = CurEquipData.GetUpgradeMoneyByQualityAndLevel((int)CurItem.GetItemQuality(), GetCurItemLevel());
			NeedMoneyLab.text = NeedMoney.ToString();
			if (NeedMoney <= GameMoneyHelper.GetMoneyNum(0))
			{
				NeedMoneyLab.color = Color.white;
			}
			else
			{
				NeedMoneyLab.color = Color.red;
			}
			ItemGrid.Reposition();
			ItemGrid.repositionNow = true;
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(Cost, state: false);
			NGUITools.SetActive(UpLevelBtn.gameObject, state: false);
			NGUITools.SetActive(UpLevelAllBtn.gameObject, state: false);
			EquipNameLab.text = string.Empty;
			CurItemIcon.spriteName = string.Empty;
			CurLevelLab.text = "--";
			NextLevelLab.text = "--";
			UnityVersionUtil.SetActiveRecursive(ItemGrid.gameObject, state: false);
			ItemGrid.Reposition();
			LevelProgress.fillAmount = 0f;
			UnityVersionUtil.SetActiveRecursive(ItemQualityIcon.gameObject, state: false);
		}
	}

	public void UpdateQHInfo(GameItem item)
	{
		if (CurCaiLiao != null && item.IndexId == CurCaiLiao.IndexId)
		{
			UpdataCaiLiaoStack(item);
		}
		else if (item.IndexId == CurItem.IndexId)
		{
			UpdateEquipUpdateInfoPage(item);
			QiangHuaEquipList.UpdateEquipInfo(item);
			NoticeLogic.AddNotifyData("#{100917}");
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Enhance", $"part_{item.ItemData.SubType}", $"level_{item.ItemLevel}");
		}
	}

	public void UpdataCaiLiaoStack(GameItem item)
	{
		CurStack = item.StackNum;
	}

	public void OnClickAllLevelBtn()
	{
		if (CurItem == null || CurEquipData == null || CurItem.IndexId == -1)
		{
			NoticeLogic.AddNotifyData("#{100923}");
			CloseTutorial();
			return;
		}
		if (GetCurItemLevel() >= mplayerdata.Level)
		{
			NoticeLogic.AddNotifyData("#{100918}");
			CloseTutorial();
			return;
		}
		if (!GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.CASH, NeedMoney))
		{
			CloseTutorial();
			return;
		}
		if (CurStack < NeedStack)
		{
			GameMoneyHelper.ShowItemProduct("3001");
			CloseTutorial();
			return;
		}
		int i = GetCurItemLevel();
		int num = 0;
		int num2 = 0;
		long cash = GameMoneyHelper.GetCash();
		for (; i < mplayerdata.Level; i++)
		{
			num += CurEquipData.GetUpgradeExpValByLevel(i);
			num2 += CurEquipData.GetUpgradeMoneyByQualityAndLevel((int)CurItem.GetItemQuality(), i);
			if (num > CurStack || num2 > cash)
			{
				break;
			}
		}
		if (i > GetCurItemLevel())
		{
			PlayEffect();
			equip_enhance.request request = new equip_enhance.request();
			request.indexId = CurItem.IndexId;
			request.level = i;
			NetLogic.GetInstance().Send<Protocol.equip_enhance>(request);
			if (TutorialManager.CurStep == TUTORIAL_STEP.ENHANCE_ALL_CLICK || TutorialManager.CurStep == TUTORIAL_STEP.ENHANCE_CLICK_ALL)
			{
				CheckTutorialEvent();
			}
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Enhance", "click_enhanceall", "times");
		}
	}

	public void PlayEffect()
	{
		EquipEffect.Clear();
		EquipEffect.Play(withChildren: true);
		for (int i = 0; i < EquipDataLab.Count; i++)
		{
			if (UnityVersionUtil.IsActive(EquipDataLab[i].gameObject))
			{
				lineEffect[i].resetOnPlay = true;
				lineEffect[i].Play(forward: true);
			}
		}
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(19);
	}

	public void OnClickUpLevelBtn()
	{
		if (CurItem == null || CurEquipData == null || CurItem.IndexId == -1)
		{
			NoticeLogic.AddNotifyData("#{100923}");
			CloseTutorial();
		}
		else if (GetCurItemLevel() >= mplayerdata.Level)
		{
			NoticeLogic.AddNotifyData("#{100918}");
			CloseTutorial();
		}
		else if (!GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.CASH, NeedMoney))
		{
			CloseTutorial();
		}
		else if (CurStack < NeedStack)
		{
			GameMoneyHelper.ShowItemProduct("3001");
			CloseTutorial();
		}
		else if (CurItem != null)
		{
			PlayEffect();
			equip_enhance.request request = new equip_enhance.request();
			request.indexId = CurItem.IndexId;
			NetLogic.GetInstance().Send<Protocol.equip_enhance>(request);
			if (TutorialManager.CurStep == TUTORIAL_STEP.ENHANCE_CLICK)
			{
				CheckTutorialEvent();
			}
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Enhance", "click_enhance", "times");
		}
	}
}
