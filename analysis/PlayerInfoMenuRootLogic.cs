using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoMenuRootLogic : SingletonUnity<PlayerInfoMenuRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public GameObject AutoEquipBtn;

	private GAME_MENU_TAP_TYPE mCurTapType = GAME_MENU_TAP_TYPE.INVILAD;

	private bool isGoToEquipUp;

	private GameItem curitem;

	private FakeObjLogic mPlayerModelVisual;

	private MenuTopRootLogic mMenuTopRoot;

	public GAME_MENU_TAP_TYPE CurTapType => mCurTapType;

	public FakeObjLogic PlayerModelVisual
	{
		get
		{
			return mPlayerModelVisual;
		}
		set
		{
			mPlayerModelVisual = value;
		}
	}

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private new void Awake()
	{
		base.Awake();
		ResetFakeObjRoot();
	}

	private void ResetFakeObjRoot()
	{
		FakeObjRootLogic instance = SingletonUnity<FakeObjRootLogic>.Instance;
		if (instance == null)
		{
			ResourcesManager.LoadAndInstantiate("UIRoot/FakeObjRoot");
			instance = SingletonUnity<FakeObjRootLogic>.Instance;
		}
	}

	private void OnDisable()
	{
		UnLoadFakeObj();
	}

	public void ResetModelVisual()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ItemContainer fashionEquipPack = playerData.FashionEquipPack;
		ItemContainer equipPack = playerData.EquipPack;
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		if (playerData.IsShowFashion)
		{
			empty = fashionEquipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.HEAD, playerData.Profession, isFashion: true);
			if (string.IsNullOrEmpty(empty))
			{
				empty = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.HEAD, playerData.Profession);
			}
			if (CheckWeaponIsSame())
			{
				empty2 = fashionEquipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.WEAPON, playerData.Profession, isFashion: true);
				if (string.IsNullOrEmpty(empty2))
				{
					empty2 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.WEAPON, playerData.Profession);
				}
			}
			else
			{
				empty2 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.WEAPON, playerData.Profession);
			}
			empty3 = fashionEquipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.BODY, playerData.Profession, isFashion: true);
			if (string.IsNullOrEmpty(empty3))
			{
				empty3 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.BODY, playerData.Profession);
			}
			empty4 = fashionEquipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.LEG, playerData.Profession, isFashion: true);
			if (string.IsNullOrEmpty(empty4))
			{
				empty4 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.LEG, playerData.Profession);
			}
		}
		else
		{
			empty = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.HEAD, playerData.Profession);
			empty2 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.WEAPON, playerData.Profession);
			empty3 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.BODY, playerData.Profession);
			empty4 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.LEG, playerData.Profession);
		}
		if (mPlayerModelVisual == null)
		{
			mPlayerModelVisual = new FakeObjLogic();
			mPlayerModelVisual.InitFakeObject(empty2, empty, empty3, empty4, playerData.Profession, SingletonUnity<FakeObjRootLogic>.Instance.MeshRoot);
		}
		else
		{
			mPlayerModelVisual.CheckFakeObject(empty2, empty, empty3, empty4);
			mPlayerModelVisual.PlayAnim("idle", playerData.CharacterModelData.ModelFirstType);
		}
	}

	public bool CheckWeaponIsSame()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ItemContainer fashionEquipPack = playerData.FashionEquipPack;
		ItemContainer equipPack = playerData.EquipPack;
		EquipData equipWeaponData = equipPack.GetEquipWeaponData(EQUIP_BACKPACK_TYPE.WEAPON);
		EquipData equipWeaponData2 = fashionEquipPack.GetEquipWeaponData(EQUIP_BACKPACK_TYPE.WEAPON);
		if (equipWeaponData != null && equipWeaponData2 != null)
		{
			return equipWeaponData.WeaponType == equipWeaponData2.WeaponType;
		}
		return true;
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerInfoMenuRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerInfoMenuBackPackRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerInfoMenuPlayerInfoRootUI);
		if (SingletonUnity<PlayerModelPageRootLogic>.Exists)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerInfoMenuPlayerModelPageRoot);
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		UnLoadFakeObj();
	}

	public void UnLoadFakeObj()
	{
		if (mPlayerModelVisual != null)
		{
			mPlayerModelVisual.DestroyFakeObj();
			mPlayerModelVisual = null;
		}
		if (SingletonUnity<FakeObjRootLogic>.Instance != null)
		{
			SingletonUnity<FakeObjRootLogic>.Instance.DisableFakeObjRoot();
		}
	}

	public void Reset()
	{
		UnityVersionUtil.SetActiveRecursive(AutoEquipBtn, state: false);
		if (!SingletonUnity<MenuBaseRootLogic>.Instance.AutoClickTipsTap())
		{
			OnClickPlayerInfoBtn();
		}
	}

	public void OnClickPlayerInfoBtn()
	{
		if (mCurTapType != 0)
		{
			mCurTapType = GAME_MENU_TAP_TYPE.PLAYERINFO_TAP;
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerInfoMenuBackPackRootUI);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuPlayerModelPageRoot, OnOpenPlayerModelPageRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuPlayerInfoRootUI, OnOpenPlayerInfoRoot);
		}
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(0);
		UnityVersionUtil.SetActiveRecursive(AutoEquipBtn, state: false);
	}

	public void OnClickItemBackPackBtn()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
		if (mCurTapType != GAME_MENU_TAP_TYPE.ITEM_BACKPACK_TAP)
		{
			mCurTapType = GAME_MENU_TAP_TYPE.ITEM_BACKPACK_TAP;
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerInfoMenuPlayerInfoRootUI);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuPlayerModelPageRoot, OnOpenPlayerModelPageRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuBackPackRootUI, OnOpenBackPackRoot);
		}
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(4);
		UnityVersionUtil.SetActiveRecursive(AutoEquipBtn, state: false);
	}

	public void OnClickEquipBackPackBtn()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
		if (mCurTapType != GAME_MENU_TAP_TYPE.EQUIP_BACKPACK_TAB)
		{
			mCurTapType = GAME_MENU_TAP_TYPE.EQUIP_BACKPACK_TAB;
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerInfoMenuPlayerInfoRootUI);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuPlayerModelPageRoot, OnOpenPlayerModelPageRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuBackPackRootUI, OnOpenBackPackRoot);
		}
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(1);
		UnityVersionUtil.SetActiveRecursive(AutoEquipBtn, state: false);
	}

	public void OnClickFashionBackPackBtn()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
		if (mCurTapType != GAME_MENU_TAP_TYPE.FASHION_TAB)
		{
			mCurTapType = GAME_MENU_TAP_TYPE.FASHION_TAB;
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerInfoMenuPlayerInfoRootUI);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuPlayerModelPageRoot, OnOpenPlayerModelPageRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuBackPackRootUI, OnOpenBackPackRoot);
		}
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(2);
		UnityVersionUtil.SetActiveRecursive(AutoEquipBtn, state: false);
	}

	public void OnClickBadgeBtn()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
		if (mCurTapType != GAME_MENU_TAP_TYPE.BADGE_BACKPACK_TAB)
		{
			mCurTapType = GAME_MENU_TAP_TYPE.BADGE_BACKPACK_TAB;
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerInfoMenuPlayerInfoRootUI);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuPlayerModelPageRoot, OnOpenPlayerModelPageRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuBackPackRootUI, OnOpenBackPackRoot);
		}
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(3);
		UnityVersionUtil.SetActiveRecursive(AutoEquipBtn, state: false);
	}

	public void GoToZhuangBeiQiangHua(GameItem item)
	{
		curitem = item;
		isGoToEquipUp = true;
	}

	private void OnOpenBackPackRoot(bool success, object param)
	{
		if (mCurTapType == GAME_MENU_TAP_TYPE.ITEM_BACKPACK_TAP)
		{
			SingletonUnity<BackPackPageRootLogic>.Instance.Reset(needResetPackPos: true, ITEM_CONTAINER_TYPE.ITEM_BACKPACK);
		}
		else if (mCurTapType == GAME_MENU_TAP_TYPE.EQUIP_BACKPACK_TAB)
		{
			SingletonUnity<BackPackPageRootLogic>.Instance.Reset(needResetPackPos: true, ITEM_CONTAINER_TYPE.EQUIP_BACKPACK);
		}
		else if (mCurTapType == GAME_MENU_TAP_TYPE.BADGE_BACKPACK_TAB)
		{
			SingletonUnity<BackPackPageRootLogic>.Instance.Reset(needResetPackPos: true, ITEM_CONTAINER_TYPE.BADGE_BACKPACK);
		}
		else if (mCurTapType == GAME_MENU_TAP_TYPE.FASHION_TAB)
		{
			SingletonUnity<BackPackPageRootLogic>.Instance.Reset(needResetPackPos: true, ITEM_CONTAINER_TYPE.FASHION_BACKPACK);
		}
	}

	private void OnOpenPlayerInfoRoot(bool success, object param)
	{
		if (success)
		{
			SingletonUnity<JSSXKuangRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData);
		}
	}

	private void OnOpenSkillInfoRoot(bool success, object param)
	{
		if (success)
		{
			SingletonUnity<SkillInfoRootLogic>.Instance.Reset();
		}
	}

	private void OnOpenShengWangRoot(bool success, object param)
	{
		if (success)
		{
			SingletonUnity<JSShengWangLogic>.Instance.Reset();
		}
	}

	private void OnOpenPlayerModelPageRoot(bool success, object param)
	{
		if (!success)
		{
			return;
		}
		PlayerModelPageRootLogic instance = SingletonUnity<PlayerModelPageRootLogic>.Instance;
		bool flag = false;
		bool flag2 = false;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		List<GameItem> curItemList = null;
		EQUIP_PACK_TYPE curType = EQUIP_PACK_TYPE.BACKPACK;
		if (mCurTapType == GAME_MENU_TAP_TYPE.PLAYERINFO_TAP)
		{
			flag = true;
			flag2 = true;
			instance.ResetOnClickItem(OnClickEquipItem);
			curItemList = ItemContainerTool.GetEquipItemList(playerData.EquipPack);
			curType = EQUIP_PACK_TYPE.BACKPACK;
		}
		else
		{
			flag = true;
			flag2 = true;
			if (mCurTapType == GAME_MENU_TAP_TYPE.EQUIP_BACKPACK_TAB)
			{
				instance.ResetOnClickItem(OnClickEquipItem);
				curItemList = ItemContainerTool.GetEquipItemList(playerData.EquipPack);
				curType = EQUIP_PACK_TYPE.BACKPACK;
			}
			else if (mCurTapType == GAME_MENU_TAP_TYPE.BADGE_BACKPACK_TAB)
			{
				instance.ResetOnClickItem(OnClickEquipItem);
				curItemList = playerData.BadgeEquipPack.ItemList;
				curType = EQUIP_PACK_TYPE.BADGE;
			}
			else if (mCurTapType == GAME_MENU_TAP_TYPE.ITEM_BACKPACK_TAP)
			{
				instance.ResetOnClickItem(OnClickEquipItem);
				curItemList = ItemContainerTool.GetEquipItemList(playerData.EquipPack);
				curType = EQUIP_PACK_TYPE.BACKPACK;
			}
			else if (mCurTapType == GAME_MENU_TAP_TYPE.FASHION_TAB)
			{
				instance.ResetOnClickItem(OnClickEquipItem);
				curItemList = ItemContainerTool.GetFashionEquipItemList(playerData.FashionEquipPack);
				curType = EQUIP_PACK_TYPE.FASHION;
			}
		}
		ResetModelVisual();
		instance.Reset(mPlayerModelVisual, curItemList, string.Empty, playerData.MainPlayerAttrData.ComboValue.ToString(), flag, flag2, curType);
	}

	public void OnClickEquipItem(GameItem item)
	{
		if (item == null)
		{
			return;
		}
		if (item.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP || item.ItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			if (item.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				item.ItemLevel = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(item.ItemData.SubType);
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRootNew, delegate
			{
				SingletonUnity<ItemInfoRootLogicNew>.Instance.Reset(item, ITEM_SHOW_TYPE.EQUIPPACK);
			});
		}
		else
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ItemInfoRootNew, delegate
			{
				SingletonUnity<ItemInfoRootLogicNew>.Instance.Reset(item, ITEM_SHOW_TYPE.EQUIPPACK);
			});
		}
	}

	private void OnEnable()
	{
		mCurTapType = GAME_MENU_TAP_TYPE.INVILAD;
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickPlayerInfoBtn, isIcon: true, "CZ_left_Character", StrDictionary.GetDictionaryString("#{100601}"), FUNCTION_TYPE.CHARACTER_INFO);
			MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickEquipBackPackBtn, isIcon: true, "CZ_left_Equipment", StrDictionary.GetDictionaryString("#{100602}"), FUNCTION_TYPE.CHARACTER_EQUIP, playerData.IsHaveEquipTips);
			MenuTabBtnInfo item3 = new MenuTabBtnInfo(OnClickFashionBackPackBtn, isIcon: true, "CZ_left_Fashion", StrDictionary.GetDictionaryString("#{100603}"), FUNCTION_TYPE.CHARACTER_FASHION, playerData.IsHaveFashionEquipTips);
			MenuTabBtnInfo item4 = new MenuTabBtnInfo(OnClickBadgeBtn, isIcon: true, "CZ_left_Badge", StrDictionary.GetDictionaryString("#{100604}"), FUNCTION_TYPE.CHARACTER_BADGE, playerData.IsHaveBadgeTips);
			MenuTabBtnInfo item5 = new MenuTabBtnInfo(OnClickItemBackPackBtn, isIcon: true, "CZ_left_Item", StrDictionary.GetDictionaryString("#{100605}"), FUNCTION_TYPE.CHARACTER_ITEM, playerData.IsHaveItemTips);
			list.Add(item);
			list.Add(item2);
			list.Add(item3);
			list.Add(item4);
			list.Add(item5);
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
		});
	}

	public void UpdateEquipPack()
	{
		PlayerModelPageRootLogic instance = SingletonUnity<PlayerModelPageRootLogic>.Instance;
		if (mCurTapType == GAME_MENU_TAP_TYPE.EQUIP_BACKPACK_TAB || mCurTapType == GAME_MENU_TAP_TYPE.ITEM_BACKPACK_TAP || mCurTapType == GAME_MENU_TAP_TYPE.PLAYERINFO_TAP)
		{
			instance.ReShow(ItemContainerTool.GetEquipItemList(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipPack));
		}
		else if (mCurTapType == GAME_MENU_TAP_TYPE.BADGE_BACKPACK_TAB)
		{
			instance.ReShow(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BadgeEquipPack.ItemList);
		}
		else if (mCurTapType == GAME_MENU_TAP_TYPE.FASHION_TAB)
		{
			instance.ReShow(ItemContainerTool.GetFashionEquipItemList(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FashionEquipPack));
		}
		ResetModelVisual();
	}

	public void OnClickAutoEquipBtn()
	{
		ItemContainer equipBackPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipBackPack;
		ItemContainer equipPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipPack;
		PROFESSION_TYPE profession = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession;
		List<GameItem> equipItemList = ItemContainerTool.GetEquipItemList(equipPack);
		GameItem gameItem = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.WEAPON)];
		GameItem gameItem2 = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.HEAD)];
		GameItem gameItem3 = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.BODY)];
		GameItem gameItem4 = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.LEG)];
		GameItem gameItem5 = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.BELT)];
		GameItem gameItem6 = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.NECKLACE)];
		GameItem bestTargetEquipItem = ItemContainerTool.GetBestTargetEquipItem(equipBackPack, 0, profession);
		GameItem bestTargetEquipItem2 = ItemContainerTool.GetBestTargetEquipItem(equipBackPack, 1, profession);
		GameItem bestTargetEquipItem3 = ItemContainerTool.GetBestTargetEquipItem(equipBackPack, 2, profession);
		GameItem bestTargetEquipItem4 = ItemContainerTool.GetBestTargetEquipItem(equipBackPack, 3, profession);
		GameItem bestTargetEquipItem5 = ItemContainerTool.GetBestTargetEquipItem(equipBackPack, 4, profession);
		GameItem bestTargetEquipItem6 = ItemContainerTool.GetBestTargetEquipItem(equipBackPack, 5, profession);
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (!(mainPlayer != null))
		{
			return;
		}
		if (bestTargetEquipItem != null && !bestTargetEquipItem.IsEmpty())
		{
			if (gameItem == null || gameItem.IsEmpty())
			{
				mainPlayer.EquipItem(bestTargetEquipItem);
			}
			else if (bestTargetEquipItem.GetItemCombatVal() > gameItem.GetItemCombatVal())
			{
				mainPlayer.EquipItem(bestTargetEquipItem);
			}
		}
		if (bestTargetEquipItem2 != null && !bestTargetEquipItem2.IsEmpty())
		{
			if (gameItem2 == null || gameItem2.IsEmpty())
			{
				mainPlayer.EquipItem(bestTargetEquipItem2);
			}
			else if (bestTargetEquipItem2.GetItemCombatVal() > gameItem2.GetItemCombatVal())
			{
				mainPlayer.EquipItem(bestTargetEquipItem2);
			}
		}
		if (bestTargetEquipItem3 != null && !bestTargetEquipItem3.IsEmpty())
		{
			if (gameItem3 == null || gameItem3.IsEmpty())
			{
				mainPlayer.EquipItem(bestTargetEquipItem3);
			}
			else if (bestTargetEquipItem3.GetItemCombatVal() > gameItem3.GetItemCombatVal())
			{
				mainPlayer.EquipItem(bestTargetEquipItem3);
			}
		}
		if (bestTargetEquipItem4 != null && !bestTargetEquipItem4.IsEmpty())
		{
			if (gameItem4 == null || gameItem4.IsEmpty())
			{
				mainPlayer.EquipItem(bestTargetEquipItem4);
			}
			else if (bestTargetEquipItem4.GetItemCombatVal() > gameItem4.GetItemCombatVal())
			{
				mainPlayer.EquipItem(bestTargetEquipItem4);
			}
		}
		if (bestTargetEquipItem5 != null && !bestTargetEquipItem5.IsEmpty())
		{
			if (gameItem5 == null || gameItem5.IsEmpty())
			{
				mainPlayer.EquipItem(bestTargetEquipItem5);
			}
			else if (bestTargetEquipItem5.GetItemCombatVal() > gameItem5.GetItemCombatVal())
			{
				mainPlayer.EquipItem(bestTargetEquipItem5);
			}
		}
		if (bestTargetEquipItem6 != null && !bestTargetEquipItem6.IsEmpty())
		{
			if (gameItem6 == null || gameItem6.IsEmpty())
			{
				mainPlayer.EquipItem(bestTargetEquipItem6);
			}
			else if (bestTargetEquipItem6.GetItemCombatVal() > gameItem6.GetItemCombatVal())
			{
				mainPlayer.EquipItem(bestTargetEquipItem6);
			}
		}
	}

	private bool CheckCanEquip(GameItem item)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (!playerData.CheckLevel(item.ItemData.Level))
		{
			return false;
		}
		EquipData equipDataById = DataManager.GetEquipDataById(item.ItemId);
		if (equipDataById != null && playerData.Profession != equipDataById.profession)
		{
			return false;
		}
		return true;
	}
}
