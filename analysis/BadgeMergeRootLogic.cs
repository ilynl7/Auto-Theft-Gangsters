using System;
using System.Collections.Generic;
using Sproto;
using SprotoType;
using UnityEngine;

public class BadgeMergeRootLogic : SingletonUnity<BadgeMergeRootLogic>
{
	public int lineCount = 6;

	public int ShowLineCount = 4;

	private PlayerData playerData;

	public BackPackRootLogic BackPackRootLogicScript;

	private GameItem levelUpItem;

	public UISprite LevelUpIcon;

	public UISprite LevelUpQuality;

	public UILabel LevelUpNameLabel;

	public UISprite LevelUpUnloadFlag;

	public UILabel LevelUpNumLabel;

	public UISprite LevelDownIcon;

	public UISprite LevelDownQuality;

	public UILabel LevelDownNameLabel;

	public UILabel LevelDownNumLabel;

	public ParticleSystem DirTipsParticle;

	public GameObject CostObj;

	public UILabel CoinsTxt;

	public UILabel CoinsTxt2;

	public UIPlayTween leftEffect;

	public UIPlayTween rightEffect;

	private bool isCanMerge;

	private BadgeData mNextBadgeData;

	private BadgeData mLevelUpBadgeData;

	private GameItem mCurItem;

	private int mMaxMergeNum;

	private int oneCost;

	private int allCost;

	private int upCount;

	private int mergIng;

	public UIPlayTween upgradeEffect;

	protected override void Awake()
	{
		base.Awake();
		BackPackRootLogicScript.onClickItem = OnClickItem;
	}

	public void ResetEnable()
	{
		DirTipsParticle.Play();
		leftEffect.resetOnPlay = true;
		rightEffect.resetOnPlay = true;
		leftEffect.Play(forward: true);
		rightEffect.Play(forward: true);
		mergIng = 0;
	}

	private void OnEnable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
	}

	private void OnDisable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
		levelUpItem = null;
		if (DirTipsParticle != null)
		{
			DirTipsParticle.Clear();
			DirTipsParticle.Stop();
		}
	}

	public void UpdateMoney()
	{
		if (levelUpItem == null)
		{
			return;
		}
		mLevelUpBadgeData = DataManager.GetBadgeDataById(levelUpItem.ItemId);
		if (mLevelUpBadgeData != null)
		{
			mMaxMergeNum = levelUpItem.StackNum / mLevelUpBadgeData.UpgradeCount;
			CoinsTxt.text = mLevelUpBadgeData.UpgradeCost.ToString();
			CoinsTxt2.text = (mLevelUpBadgeData.UpgradeCost * Mathf.Max(1, mMaxMergeNum)).ToString();
			long moneyNum = GameMoneyHelper.GetMoneyNum(0);
			if (moneyNum >= mLevelUpBadgeData.UpgradeCost)
			{
				CoinsTxt.color = Color.white;
			}
			else
			{
				CoinsTxt.color = Color.red;
			}
			if (moneyNum >= mLevelUpBadgeData.UpgradeCost * Mathf.Max(1, mMaxMergeNum))
			{
				CoinsTxt2.color = Color.white;
			}
			else
			{
				CoinsTxt2.color = Color.red;
			}
		}
	}

	public void OnClickItem(GameItem item)
	{
		if (mergIng <= 0)
		{
			if (item != null && item == levelUpItem)
			{
				ItemInfoRootLogicNew.ShowItemTips(item, 0);
			}
			else
			{
				ShowLevelUpItem(item);
			}
		}
	}

	public void OnClickMoneyInfo()
	{
		ItemData moneyItemData = GameMoneyHelper.GetMoneyItemData(GameDefine.MONEY_TYPE.CASH);
		if (moneyItemData != null)
		{
			ItemInfoRootLogicNew.ShowItemTips(moneyItemData);
		}
	}

	public void ClickLevelUpBtn()
	{
		if (levelUpItem != null)
		{
			HideLevelUpItem();
			levelUpItem = null;
			UpdateInfo();
		}
	}

	public void ShowLevelUpItem(GameItem item)
	{
		isCanMerge = false;
		mergIng = 0;
		if (item != null)
		{
			levelUpItem = item;
			mLevelUpBadgeData = DataManager.GetBadgeDataById(item.ItemId);
			string id = (int.Parse(item.ItemId) + 1).ToString();
			mNextBadgeData = DataManager.GetBadgeDataById(id);
			if (mNextBadgeData == null)
			{
				HideLevelUpItem();
				NoticeLogic.AddNotifyData("#{100933}");
				return;
			}
			ItemData itemDataByID = DataManager.GetItemDataByID(item.ItemId);
			LevelUpIcon.spriteName = itemDataByID.BackPackIcon;
			LevelUpQuality.spriteName = itemDataByID.QualityType.ToString();
			LevelUpUnloadFlag.enabled = true;
			LevelUpNameLabel.text = itemDataByID.MName;
			LevelUpNameLabel.color = GameDefine.GetColorByQuality(itemDataByID.QualityType);
			LevelUpNumLabel.text = item.StackNum.ToString();
			upCount = item.StackNum;
			mMaxMergeNum = item.StackNum / mLevelUpBadgeData.UpgradeCount;
			ItemData itemDataByID2 = DataManager.GetItemDataByID(id);
			LevelDownIcon.spriteName = itemDataByID2.BackPackIcon;
			LevelDownQuality.spriteName = itemDataByID2.QualityType.ToString();
			LevelDownNameLabel.text = itemDataByID2.MName;
			LevelDownNameLabel.color = GameDefine.GetColorByQuality(itemDataByID2.QualityType);
			LevelDownNumLabel.text = mMaxMergeNum.ToString();
			CoinsTxt.text = mLevelUpBadgeData.UpgradeCost.ToString();
			CoinsTxt2.text = (mLevelUpBadgeData.UpgradeCost * Mathf.Max(1, mMaxMergeNum)).ToString();
			long moneyNum = GameMoneyHelper.GetMoneyNum(0);
			if (moneyNum >= mLevelUpBadgeData.UpgradeCost)
			{
				CoinsTxt.color = Color.white;
			}
			else
			{
				CoinsTxt.color = Color.red;
			}
			if (moneyNum >= mLevelUpBadgeData.UpgradeCost * Mathf.Max(1, mMaxMergeNum))
			{
				CoinsTxt2.color = Color.white;
			}
			else
			{
				CoinsTxt2.color = Color.red;
			}
			isCanMerge = mMaxMergeNum > 0;
			oneCost = mLevelUpBadgeData.UpgradeCost;
			allCost = mLevelUpBadgeData.UpgradeCost * Mathf.Max(1, mMaxMergeNum);
			UnityVersionUtil.SetActiveRecursive(CostObj, state: true);
		}
		else
		{
			HideLevelUpItem();
		}
	}

	public void HideLevelUpItem()
	{
		LevelUpIcon.spriteName = "CZ_zhuangBeiCao";
		LevelUpQuality.spriteName = string.Empty;
		LevelUpUnloadFlag.enabled = false;
		LevelUpNameLabel.text = string.Empty;
		LevelUpNumLabel.text = string.Empty;
		LevelDownIcon.spriteName = "CZ_zhuangBeiCao";
		LevelDownQuality.spriteName = string.Empty;
		LevelDownNameLabel.text = string.Empty;
		LevelDownNumLabel.text = string.Empty;
		UnityVersionUtil.SetActiveRecursive(CostObj, state: false);
		levelUpItem = null;
	}

	public void ClickShowNextItem()
	{
		if (mNextBadgeData != null)
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(mNextBadgeData.ID);
			if (itemDataByID != null)
			{
				ItemInfoRootLogicNew.ShowItemTips(itemDataByID);
			}
		}
	}

	public void Show(GameItem item = null)
	{
		ResetEnable();
		if (playerData == null)
		{
			playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		}
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		ShowLevelUpItem(item);
		UpdateInfo();
	}

	private void Refresh()
	{
		if ((bool)this && UnityVersionUtil.IsActive(base.gameObject))
		{
			UpdateInfo(needResetPos: false);
		}
	}

	public void UpdateInfo(bool needResetPos = true)
	{
		List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(playerData.BadgeBackPack, IsAll: true, GameDefine.ITEM_TYPE.BADGE);
		ShowItemList(targetTypeItem, needResetPos);
	}

	public void ShowItemList(List<GameItem> list, bool needResetPos = true)
	{
		BackPackRootLogicScript.ShowList(list, needResetPos);
	}

	public void ClickMergeOne()
	{
		if (mergIng <= 0)
		{
			if (!isCanMerge)
			{
				NoticeLogic.AddNotifyData("#{100924}");
			}
			else if (GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.CASH, oneCost))
			{
				PlayerEffect();
				badge_merge.request request = new badge_merge.request();
				request.indexId = levelUpItem.IndexId;
				request.nextItemId = mNextBadgeData.ID;
				request.count = 1L;
				NetLogic.GetInstance().Send<Protocol.badge_merge>(request, BadgeMergeResponse);
			}
		}
	}

	public void ClickMergeAll()
	{
		if (mergIng <= 0)
		{
			if (!isCanMerge)
			{
				NoticeLogic.AddNotifyData("#{100924}");
			}
			else if (GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.CASH, allCost))
			{
				PlayerEffect();
				badge_merge.request request = new badge_merge.request();
				request.indexId = levelUpItem.IndexId;
				request.nextItemId = mNextBadgeData.ID;
				request.count = mMaxMergeNum;
				NetLogic.GetInstance().Send<Protocol.badge_merge>(request, BadgeMergeResponse);
			}
		}
	}

	public void BadgeMergeResponse(SprotoTypeBase sp)
	{
		mergIng++;
		if (sp is badge_merge.response { HasState: not false, state: 0L })
		{
			if (mergIng >= 3)
			{
				if (levelUpItem.IsEmpty())
				{
					levelUpItem = null;
				}
				ShowLevelUpItem(levelUpItem);
			}
			Refresh();
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("BadgeMerger", "merger", "merger_times");
		}
		if (mergIng >= 3)
		{
			mergIng = 0;
		}
	}

	public void EffectFinish()
	{
		mergIng++;
		if ((bool)this && UnityVersionUtil.IsActive(base.gameObject) && mergIng >= 3)
		{
			if (levelUpItem.IsEmpty())
			{
				levelUpItem = null;
			}
			ShowLevelUpItem(levelUpItem);
		}
		if (mergIng >= 3)
		{
			mergIng = 0;
		}
	}

	public void PlayerEffect()
	{
		mergIng++;
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(19);
		upgradeEffect.resetOnPlay = true;
		upgradeEffect.Play(forward: true);
	}
}
