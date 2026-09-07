using System;
using System.Collections.Generic;
using Sproto;
using SprotoType;
using UnityEngine;

public class InhertUIRootLogic : SingletonUnity<InhertUIRootLogic>
{
	public int lineCount = 6;

	public int ShowLineCount = 4;

	private PlayerData playerData;

	public BackPackRootLogic BackPackRootLogicScript;

	private List<GameItem> mCurItemList;

	private GameItem levelUpItem;

	private GameItem levelDownItem;

	private bool mInitFlag;

	public UISprite LevelUpIcon;

	public UISprite LevelUpQuality;

	public UILabel LevelUpNameLabel;

	public UISprite LevelUpUnloadFlag;

	public UISprite LevelDownIcon;

	public UISprite LevelDownQuality;

	public UILabel LevelDownNameLabel;

	public UISprite LevelDownUnloadFlag;

	public GameObject LevelDownObj;

	public UILabel LevelDownCurLevel;

	public UILabel LevelDownNextLevel;

	public GameObject LevelUpObj;

	public UILabel LevelUpCurLevel;

	public UILabel LevelUpNextLevel;

	public ParticleSystem DirTipsParticle;

	public GameObject CostObj;

	public UISprite InhertBtn;

	public UILabel CoinsTxt;

	private float costCoinsN = 1f;

	private int currentCostCoin;

	public UIPlayTween leftEffect;

	public UIPlayTween rightEffect;

	private bool isCaninhert;

	private bool InhertIng;

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
		InhertIng = false;
	}

	private void OnEnable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
	}

	private void OnDisable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
		levelUpItem = null;
		levelDownItem = null;
		if (DirTipsParticle != null)
		{
			DirTipsParticle.Clear();
			DirTipsParticle.Stop();
		}
	}

	public void UpdateMoney()
	{
		if (levelUpItem != null && levelDownItem != null)
		{
			EquipData equipDataById = DataManager.GetEquipDataById(levelUpItem.ItemId);
			int exp = levelDownItem.Parm[0];
			int use;
			int upgradeLevelByExp = equipDataById.GetUpgradeLevelByExp(levelUpItem.ItemLevel, exp, out use);
			if (use > 0)
			{
				InhertBtn.spriteName = GameDefine.BtnIcon[1];
				isCaninhert = true;
				NGUITools.SetActive(CostObj, state: true);
			}
			else
			{
				InhertBtn.spriteName = GameDefine.BtnIcon[2];
				isCaninhert = false;
				NGUITools.SetActive(CostObj, state: false);
			}
			currentCostCoin = (int)(costCoinsN * (float)use);
			CoinsTxt.text = currentCostCoin.ToString();
			if (currentCostCoin <= GameMoneyHelper.GetMoneyNum(0))
			{
				CoinsTxt.color = Color.white;
			}
			else
			{
				CoinsTxt.color = Color.red;
			}
		}
	}

	public void OnClickItem(GameItem item)
	{
		if (levelDownItem == null)
		{
			ShowLevelDownItem(item);
		}
		else if (levelUpItem == null)
		{
			ShowLevelUpItem(item);
		}
		UpdateInfo();
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

	public void ClickLevelDownBtn()
	{
		if (levelDownItem != null)
		{
			HideLevelDownItem();
			levelDownItem = null;
			UpdateInfo();
		}
	}

	public void ShowLevelUpItem(GameItem item)
	{
		if (item != null)
		{
			levelUpItem = item;
			ItemData itemDataByID = DataManager.GetItemDataByID(item.ItemId);
			LevelUpIcon.spriteName = itemDataByID.BackPackIcon;
			LevelUpQuality.spriteName = item.GetItemQuality().ToString();
			LevelUpUnloadFlag.enabled = true;
			LevelUpNameLabel.text = itemDataByID.MName;
			LevelUpNameLabel.color = GameDefine.GetColorByQuality(item.GetItemQuality());
			NGUITools.SetActive(LevelUpObj, state: false);
		}
	}

	public void ShowLevelUpLevel(int nextLevel)
	{
		NGUITools.SetActive(LevelUpObj, state: true);
		LevelUpCurLevel.text = $"Lv.{levelUpItem.ItemLevel}";
		LevelUpNextLevel.text = $"Lv.{nextLevel}";
	}

	public void HideLevelUpItem()
	{
		LevelUpIcon.spriteName = "CZ_zhuangBeiCao";
		LevelUpQuality.spriteName = string.Empty;
		LevelUpUnloadFlag.enabled = false;
		LevelUpNameLabel.text = string.Empty;
		NGUITools.SetActive(LevelUpObj, state: false);
	}

	public void ShowLevelDownItem(GameItem item)
	{
		if (item != null)
		{
			levelDownItem = item;
			ItemData itemDataByID = DataManager.GetItemDataByID(item.ItemId);
			LevelDownIcon.spriteName = itemDataByID.BackPackIcon;
			LevelDownQuality.spriteName = item.GetItemQuality().ToString();
			LevelDownUnloadFlag.enabled = true;
			LevelDownNameLabel.text = itemDataByID.MName;
			LevelDownNameLabel.color = GameDefine.GetColorByQuality(item.GetItemQuality());
			NGUITools.SetActive(LevelDownObj, state: true);
			LevelDownCurLevel.text = $"Lv.{item.ItemLevel.ToString()}";
			LevelDownNextLevel.text = "Lv.0";
		}
	}

	public void HideLevelDownItem()
	{
		LevelDownIcon.spriteName = "CZ_zhuangBeiCao";
		LevelDownQuality.spriteName = string.Empty;
		LevelDownUnloadFlag.enabled = false;
		LevelDownNameLabel.text = string.Empty;
		NGUITools.SetActive(LevelDownObj, state: false);
	}

	public void Show(GameItem item = null, bool isShowUp = false)
	{
		ResetEnable();
		if (playerData == null)
		{
			playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		}
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		if (item != null)
		{
			if (isShowUp)
			{
				levelUpItem = item;
				AutoSelectDownItem();
			}
			else
			{
				levelDownItem = item;
			}
		}
		UpdateInfo();
	}

	public void AutoSelectDownItem()
	{
		List<GameItem> list = new List<GameItem>();
		list.Add(levelUpItem);
		ItemData itemData = levelUpItem.ItemData;
		List<GameItem> subItem = ItemContainerTool.GetSubItem(playerData.EquipPack, GameDefine.ITEM_TYPE.EQUIP, itemData.SubType, list);
		List<GameItem> subItem2 = ItemContainerTool.GetSubItem(playerData.EquipBackPack, GameDefine.ITEM_TYPE.EQUIP, itemData.SubType, list);
		subItem.AddRange(subItem2);
		GameItem gameItem = null;
		for (int i = 0; i < subItem.Count; i++)
		{
			if (gameItem == null || gameItem.ItemLevel < subItem[i].ItemLevel)
			{
				gameItem = subItem[i];
			}
		}
		levelDownItem = gameItem;
	}

	public void Hide()
	{
		NGUITools.SetActive(base.gameObject, state: false);
	}

	public void UpdateInfo()
	{
		if (levelUpItem == null && levelDownItem == null)
		{
			List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(playerData.EquipPack, IsAll: true, GameDefine.ITEM_TYPE.EQUIP);
			List<GameItem> targetTypeItem2 = ItemContainerTool.GetTargetTypeItem(playerData.EquipBackPack, IsAll: false, GameDefine.ITEM_TYPE.EQUIP, isUnBind: false, playerData.Profession);
			targetTypeItem.AddRange(targetTypeItem2);
			ShowItemList(targetTypeItem);
			HideLevelDownItem();
			HideLevelUpItem();
			InhertBtn.spriteName = GameDefine.BtnIcon[2];
			isCaninhert = false;
			CoinsTxt.text = string.Empty;
			NGUITools.SetActive(CostObj, state: false);
			return;
		}
		if (levelDownItem == null)
		{
			List<GameItem> list = new List<GameItem>();
			list.Add(levelUpItem);
			ItemData itemData = levelUpItem.ItemData;
			List<GameItem> subItem = ItemContainerTool.GetSubItem(playerData.EquipPack, GameDefine.ITEM_TYPE.EQUIP, itemData.SubType, list);
			List<GameItem> subItem2 = ItemContainerTool.GetSubItem(playerData.EquipBackPack, GameDefine.ITEM_TYPE.EQUIP, itemData.SubType, list, playerData.Profession);
			subItem.AddRange(subItem2);
			ShowItemList(subItem);
			ShowLevelUpItem(levelUpItem);
			HideLevelDownItem();
			InhertBtn.spriteName = GameDefine.BtnIcon[2];
			isCaninhert = false;
			CoinsTxt.text = string.Empty;
			NGUITools.SetActive(CostObj, state: false);
			return;
		}
		if (levelUpItem == null)
		{
			List<GameItem> list2 = new List<GameItem>();
			list2.Add(levelDownItem);
			ItemData itemData2 = levelDownItem.ItemData;
			List<GameItem> subItem3 = ItemContainerTool.GetSubItem(playerData.EquipPack, GameDefine.ITEM_TYPE.EQUIP, itemData2.SubType, list2);
			List<GameItem> subItem4 = ItemContainerTool.GetSubItem(playerData.EquipBackPack, GameDefine.ITEM_TYPE.EQUIP, itemData2.SubType, list2, playerData.Profession);
			subItem3.AddRange(subItem4);
			ShowItemList(subItem3);
			ShowLevelDownItem(levelDownItem);
			HideLevelUpItem();
			InhertBtn.spriteName = GameDefine.BtnIcon[2];
			isCaninhert = false;
			CoinsTxt.text = string.Empty;
			NGUITools.SetActive(CostObj, state: false);
			return;
		}
		List<GameItem> list3 = new List<GameItem>();
		list3.Add(levelDownItem);
		list3.Add(levelUpItem);
		ItemData itemData3 = levelDownItem.ItemData;
		List<GameItem> subItem5 = ItemContainerTool.GetSubItem(playerData.EquipPack, GameDefine.ITEM_TYPE.EQUIP, itemData3.SubType, list3);
		List<GameItem> subItem6 = ItemContainerTool.GetSubItem(playerData.EquipBackPack, GameDefine.ITEM_TYPE.EQUIP, itemData3.SubType, list3, playerData.Profession);
		subItem5.AddRange(subItem6);
		ShowItemList(subItem5);
		ShowLevelUpItem(levelUpItem);
		ShowLevelDownItem(levelDownItem);
		EquipData equipDataById = DataManager.GetEquipDataById(levelUpItem.ItemId);
		int exp = levelDownItem.Parm[0];
		int use;
		int upgradeLevelByExp = equipDataById.GetUpgradeLevelByExp(levelUpItem.ItemLevel, exp, out use);
		ShowLevelUpLevel(upgradeLevelByExp);
		if (use > 0)
		{
			InhertBtn.spriteName = GameDefine.BtnIcon[1];
			isCaninhert = true;
			NGUITools.SetActive(CostObj, state: true);
		}
		else
		{
			InhertBtn.spriteName = GameDefine.BtnIcon[2];
			isCaninhert = false;
			NGUITools.SetActive(CostObj, state: false);
		}
		currentCostCoin = (int)(costCoinsN * (float)use);
		CoinsTxt.text = currentCostCoin.ToString();
		if (currentCostCoin <= GameMoneyHelper.GetMoneyNum(0))
		{
			CoinsTxt.color = Color.white;
		}
		else
		{
			CoinsTxt.color = Color.red;
		}
	}

	public void ShowItemList(List<GameItem> list, bool needResetPos = true)
	{
		BackPackRootLogicScript.ShowList(list, needResetPos);
	}

	public void ClickInhertBtn()
	{
		if (!InhertIng && isCaninhert && GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.CASH, currentCostCoin))
		{
			equip_inhert.request request = new equip_inhert.request();
			request.containertype1 = (long)levelDownItem.ContainerType;
			request.indexId1 = levelDownItem.IndexId;
			request.containertype2 = (long)levelUpItem.ContainerType;
			request.indexId2 = levelUpItem.IndexId;
			InhertIng = true;
			NetLogic.GetInstance().Send<Protocol.equip_inhert>(request, EquipInhertResponse);
		}
	}

	public void EquipInhertResponse(SprotoTypeBase sp)
	{
		InhertIng = false;
		if (!(sp is equip_inhert.response response))
		{
			return;
		}
		if (response.state != 0L)
		{
			NoticeLogic.AddNotifyData("#{100922}");
			return;
		}
		ITEM_CONTAINER_TYPE type = (ITEM_CONTAINER_TYPE)response.containertype1;
		ItemContainer itemContainer = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(type);
		if (itemContainer != null)
		{
			GameItem itemByIndexId = itemContainer.GetItemByIndexId(response.item1.indexId);
			itemByIndexId.UpdateItem(response.item1);
		}
		ITEM_CONTAINER_TYPE type2 = (ITEM_CONTAINER_TYPE)response.containertype2;
		ItemContainer itemContainer2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(type2);
		if (itemContainer2 != null)
		{
			GameItem itemByIndexId2 = itemContainer2.GetItemByIndexId(response.item2.indexId);
			itemByIndexId2.UpdateItem(response.item2);
		}
		NoticeLogic.AddNotifyData("#{100921}");
		if (!(this == null) && UnityVersionUtil.IsActive(base.gameObject))
		{
			UpdateInfo();
			PlayerEffect();
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Inherit", "inherit", "inherit_times");
		}
	}

	public void PlayerEffect()
	{
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(19);
		upgradeEffect.resetOnPlay = true;
		upgradeEffect.Play(forward: true);
	}
}
