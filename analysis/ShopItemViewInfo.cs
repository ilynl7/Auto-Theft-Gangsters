using SprotoType;
using UnityEngine;

public class ShopItemViewInfo : MonoBehaviour
{
	public UIEventListener RotateModelBtnListener;

	private FakeObjLogic mCurFakeObj;

	public UITexture ModelPic;

	public UITexture CarModelPic;

	private Transform CarMeshRoot;

	private Color ambientLight;

	public void Reset()
	{
		ambientLight = RenderSettings.ambientLight;
	}

	public void UpdateSelectItem(shop_item curSelectItem)
	{
		if (curSelectItem != null)
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(curSelectItem.ItemID);
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP || itemDataByID.CanShowModel)
			{
				ResetModelVisual(itemDataByID);
			}
			else if (itemDataByID.Type == GameDefine.ITEM_TYPE.EXCHANGE)
			{
				MountData mountDataById = DataManager.GetMountDataById(itemDataByID.Function.ToString());
				ResetCarModelVisual(mountDataById, DataManager.GetColorDataById(mountDataById.DefaultColorId));
			}
		}
	}

	private void ResetCarModelVisual(MountData mountData, ColorData datacolor)
	{
		ResetFakeCarObjRoot();
		SingletonUnity<FakeCarObjRootLogic>.Instance.InitFakeCarObj(mountData, datacolor);
	}

	private void ResetModelVisual(ItemData curItemdata, bool isReset = false)
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
		if (!isReset)
		{
			EquipData equipDataById = DataManager.GetEquipDataById(curItemdata.ID);
			switch ((EQUIP_BACKPACK_TYPE)curItemdata.SubType)
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
}
