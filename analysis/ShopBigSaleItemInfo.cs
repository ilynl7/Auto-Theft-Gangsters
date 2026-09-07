using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ShopBigSaleItemInfo : MonoBehaviour
{
	public GameObject ItemViewObj;

	public GameObject ModelViewObj;

	public UISprite StatusBtnSp;

	public UISprite ViewBtnSp;

	private bool IsShowAttInfo;

	private special_big_pack curSelectItem;

	private BigPackageData curData;

	public List<RewardItem> rewardItems;

	private List<GameItem> ItemList = new List<GameItem>();

	public UIGrid parentGrid;

	public UIEventListener RotateModelBtnListener;

	private FakeObjLogic mCurFakeObj;

	public UITexture ModelPic;

	public UITexture CarModelPic;

	private Transform CarMeshRoot;

	private Color ambientLight;

	public UILabel NameLabel;

	public void Reset()
	{
		NGUITools.SetActive(ModelViewObj.gameObject, state: false);
		NGUITools.SetActive(ItemViewObj.gameObject, state: false);
		NGUITools.SetActive(StatusBtnSp.gameObject, state: false);
		NGUITools.SetActive(ViewBtnSp.gameObject, state: false);
		IsShowAttInfo = true;
		ambientLight = RenderSettings.ambientLight;
	}

	public void RefershInfo(special_big_pack iteminfo)
	{
		if (iteminfo == null)
		{
			return;
		}
		curSelectItem = iteminfo;
		curData = DataManager.GetBigPackageDataById(curSelectItem.ID);
		UpdateItemList();
		if (curData.showmodeltype == 0)
		{
			NGUITools.SetActive(StatusBtnSp.gameObject, state: false);
			NGUITools.SetActive(ViewBtnSp.gameObject, state: false);
			IsShowAttInfo = true;
			NGUITools.SetActive(ModelViewObj.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(StatusBtnSp.gameObject, state: true);
			NGUITools.SetActive(ViewBtnSp.gameObject, state: true);
		}
		if (IsShowAttInfo)
		{
			if (!UnityVersionUtil.IsActive(ItemViewObj.gameObject))
			{
				NGUITools.SetActive(ItemViewObj.gameObject, state: true);
			}
			UpdateItemInfo();
		}
		else
		{
			if (!UnityVersionUtil.IsActive(ModelViewObj.gameObject))
			{
				NGUITools.SetActive(ModelViewObj.gameObject, state: true);
			}
			UpdateViewInfo();
		}
		SelectTable();
	}

	public void UpdateViewInfo()
	{
		if (curData.showmodeltype == 1)
		{
			ResetModelVisual(ItemList);
		}
		else if (curData.showmodeltype == 2)
		{
			for (int i = 0; i < ItemList.Count; i++)
			{
				if (ItemList[i].ItemData.Type == GameDefine.ITEM_TYPE.EXCHANGE)
				{
					MountData mountDataById = DataManager.GetMountDataById(ItemList[i].ItemData.Function.ToString());
					ResetCarModelVisual(mountDataById, DataManager.GetColorDataById(mountDataById.DefaultColorId));
					break;
				}
			}
		}
		else
		{
			ModelPic.enabled = false;
			CarModelPic.enabled = false;
		}
	}

	private void ResetCarModelVisual(MountData mountData, ColorData datacolor)
	{
		ResetFakeCarObjRoot();
		SingletonUnity<FakeCarObjRootLogic>.Instance.InitFakeCarObj(mountData, datacolor);
	}

	private void ResetModelVisual(List<GameItem> mItemList)
	{
		ResetFakeObjRoot();
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
		for (int i = 0; i < mItemList.Count; i++)
		{
			ItemData itemData = mItemList[i].ItemData;
			if (itemData.Type == GameDefine.ITEM_TYPE.EQUIP || itemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
			{
				EquipData equipDataById = DataManager.GetEquipDataById(itemData.ID);
				switch ((EQUIP_BACKPACK_TYPE)itemData.SubType)
				{
				case EQUIP_BACKPACK_TYPE.HEAD:
					empty = equipDataById.ModelId;
					break;
				case EQUIP_BACKPACK_TYPE.WEAPON:
					empty2 = equipDataById.ModelId;
					break;
				case EQUIP_BACKPACK_TYPE.BODY:
					empty3 = equipDataById.ModelId;
					break;
				case EQUIP_BACKPACK_TYPE.LEG:
					empty4 = equipDataById.ModelId;
					break;
				}
			}
		}
		if (mCurFakeObj == null || mCurFakeObj.FakeObj == null)
		{
			mCurFakeObj = new FakeObjLogic();
			mCurFakeObj.InitFakeObject(empty2, empty, empty3, empty4, playerData.Profession, SingletonUnity<FakeObjRootLogic>.Instance.MeshRoot);
		}
		else
		{
			mCurFakeObj.CheckFakeObject(empty2, empty, empty3, empty4);
			mCurFakeObj.PlayAnim("idle", playerData.CharacterModelData.ModelFirstType);
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

	public void SetCarLight()
	{
		float num = 40f / 51f;
		RenderSettings.ambientLight = new Color(num, num, num, 1f);
	}

	public void ResetNormalLight()
	{
		RenderSettings.ambientLight = ambientLight;
	}

	private void ResetFakeObjRoot()
	{
		ResetNormalLight();
		RotateModelBtnListener.onDrag = OnDragModelBtn;
		FakeObjRootLogic instance = SingletonUnity<FakeObjRootLogic>.Instance;
		if (instance == null)
		{
			ResourcesManager.LoadAndInstantiate("UIRoot/FakeObjRoot");
			instance = SingletonUnity<FakeObjRootLogic>.Instance;
		}
		SingletonUnity<FakeObjRootLogic>.Instance.SetPicValue(0.8f);
		SingletonUnity<FakeObjRootLogic>.Instance.EnableFakeObjRoot();
		ModelPic.mainTexture = instance.ModelPic;
		ModelPic.enabled = true;
		CarModelPic.enabled = false;
	}

	private void ResetFakeCarObjRoot()
	{
		SetCarLight();
		RotateModelBtnListener.onDrag = OnDragCarModelPic;
		RotateModelBtnListener.onPress = OnPressCarModelPic;
		FakeCarObjRootLogic instance = SingletonUnity<FakeCarObjRootLogic>.Instance;
		if (instance == null)
		{
			ResourcesManager.LoadAndInstantiate("UIRoot/FakeCarObjRoot");
			instance = SingletonUnity<FakeCarObjRootLogic>.Instance;
		}
		CarMeshRoot = instance.MeshRoot;
		SingletonUnity<FakeCarObjRootLogic>.Instance.EnableFakeObjRoot();
		SingletonUnity<FakeCarObjRootLogic>.Instance.PlayRotate();
		CarModelPic.mainTexture = instance.ModelPic;
		ModelPic.enabled = false;
		CarModelPic.enabled = true;
	}

	private void OnDragModelBtn(GameObject btn, Vector2 delta)
	{
		if (mCurFakeObj != null && mCurFakeObj.FakeObj != null)
		{
			mCurFakeObj.FakeObj.transform.localEulerAngles -= delta.x * Vector3.up;
		}
	}

	public void OnDragCarModelPic(GameObject btn, Vector2 delta)
	{
		if (CarMeshRoot != null)
		{
			CarMeshRoot.transform.localEulerAngles -= new Vector3(0f, delta.x, 0f);
		}
	}

	public void OnPressCarModelPic(GameObject btn, bool ispress)
	{
		if (ispress)
		{
			SingletonUnity<FakeCarObjRootLogic>.Instance.StopRotate();
		}
		else
		{
			SingletonUnity<FakeCarObjRootLogic>.Instance.PlayRotate();
		}
	}

	private void OnDisable()
	{
		UnLoadFakeObj();
	}

	public void UnLoadFakeObj()
	{
		if (mCurFakeObj != null)
		{
			mCurFakeObj.DestroyFakeObj();
			mCurFakeObj = null;
		}
		if (SingletonUnity<FakeObjRootLogic>.Instance != null)
		{
			SingletonUnity<FakeObjRootLogic>.Instance.DisableFakeObjRoot();
		}
		if (SingletonUnity<FakeCarObjRootLogic>.Instance != null)
		{
			SingletonUnity<FakeCarObjRootLogic>.Instance.DisableFakeObjRoot();
		}
	}

	public void UpdateItemList()
	{
		ItemList.Clear();
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		switch (playerData.Profession)
		{
		case PROFESSION_TYPE.XD:
			if (!string.IsNullOrEmpty(curData.XDItemID1))
			{
				ItemList.Add(new GameItem(curData.XDItemID1, (EQUIP_QUALITY)curData.XDQuality1, curData.XDItemCount1));
			}
			if (!string.IsNullOrEmpty(curData.XDItemID2))
			{
				ItemList.Add(new GameItem(curData.XDItemID2, (EQUIP_QUALITY)curData.XDQuality2, curData.XDItemCount2));
			}
			if (!string.IsNullOrEmpty(curData.XDItemID3))
			{
				ItemList.Add(new GameItem(curData.XDItemID3, (EQUIP_QUALITY)curData.XDQuality3, curData.XDItemCount3));
			}
			if (!string.IsNullOrEmpty(curData.XDItemID4))
			{
				ItemList.Add(new GameItem(curData.XDItemID4, (EQUIP_QUALITY)curData.XDQuality4, curData.XDItemCount4));
			}
			break;
		case PROFESSION_TYPE.QJ:
			if (!string.IsNullOrEmpty(curData.QJItemID1))
			{
				ItemList.Add(new GameItem(curData.QJItemID1, (EQUIP_QUALITY)curData.QJQuality1, curData.QJItemCount1));
			}
			if (!string.IsNullOrEmpty(curData.QJItemID2))
			{
				ItemList.Add(new GameItem(curData.QJItemID2, (EQUIP_QUALITY)curData.QJQuality2, curData.QJItemCount2));
			}
			if (!string.IsNullOrEmpty(curData.QJItemID3))
			{
				ItemList.Add(new GameItem(curData.QJItemID3, (EQUIP_QUALITY)curData.QJQuality3, curData.QJItemCount3));
			}
			if (!string.IsNullOrEmpty(curData.QJItemID4))
			{
				ItemList.Add(new GameItem(curData.QJItemID4, (EQUIP_QUALITY)curData.QJQuality4, curData.QJItemCount4));
			}
			break;
		case PROFESSION_TYPE.NQS:
			if (!string.IsNullOrEmpty(curData.NQItemID1))
			{
				ItemList.Add(new GameItem(curData.NQItemID1, (EQUIP_QUALITY)curData.NQQuality1, curData.NQItemCount1));
			}
			if (!string.IsNullOrEmpty(curData.NQItemID2))
			{
				ItemList.Add(new GameItem(curData.NQItemID2, (EQUIP_QUALITY)curData.NQQuality2, curData.NQItemCount2));
			}
			if (!string.IsNullOrEmpty(curData.NQItemID3))
			{
				ItemList.Add(new GameItem(curData.NQItemID3, (EQUIP_QUALITY)curData.NQQuality3, curData.NQItemCount3));
			}
			if (!string.IsNullOrEmpty(curData.NQItemID4))
			{
				ItemList.Add(new GameItem(curData.NQItemID4, (EQUIP_QUALITY)curData.NQQuality4, curData.NQItemCount4));
			}
			break;
		}
		if (!string.IsNullOrEmpty(curData.ItemID5))
		{
			ItemList.Add(new GameItem(curData.ItemID5, (EQUIP_QUALITY)curData.Quality5, curData.ItemCount5));
		}
	}

	public void UpdateItemInfo()
	{
		NameLabel.text = StrDictionary.GetDictionaryString(curData.Name);
		NameLabel.color = GameDefine.GetColorByQuality(3);
		int num = ItemList.Count - rewardItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(rewardItems[0].gameObject) as GameObject;
				RewardItem component = gameObject.GetComponent<RewardItem>();
				gameObject.name = $"reward{rewardItems.Count:D2}";
				gameObject.transform.parent = parentGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				rewardItems.Add(component);
			}
		}
		for (int j = 0; j < rewardItems.Count; j++)
		{
			if (j < ItemList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(rewardItems[j].gameObject, state: true);
				int level = 0;
				if (ItemList[j].ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(ItemList[j].ItemData.SubType);
				}
				rewardItems[j].UpdateItem(ItemList[j], level);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(rewardItems[j].gameObject, state: false);
			}
		}
		parentGrid.Reposition();
	}

	public void OnClickStatusBtn()
	{
		if (!IsShowAttInfo)
		{
			IsShowAttInfo = true;
			NGUITools.SetActive(ModelViewObj.gameObject, state: false);
			NGUITools.SetActive(ItemViewObj.gameObject, state: true);
			UpdateItemInfo();
			SelectTable();
		}
	}

	public void OnClickViewBtn()
	{
		if (IsShowAttInfo)
		{
			IsShowAttInfo = false;
			NGUITools.SetActive(ModelViewObj.gameObject, state: true);
			NGUITools.SetActive(ItemViewObj.gameObject, state: false);
			UpdateViewInfo();
			SelectTable();
		}
	}

	private void SelectTable()
	{
		if (IsShowAttInfo)
		{
			StatusBtnSp.spriteName = GameDefine.BtnIcon[0];
			ViewBtnSp.spriteName = GameDefine.BtnIcon[1];
		}
		else
		{
			StatusBtnSp.spriteName = GameDefine.BtnIcon[1];
			ViewBtnSp.spriteName = GameDefine.BtnIcon[0];
		}
	}
}
