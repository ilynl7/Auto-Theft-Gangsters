using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class BigPackPageLogic : MonoBehaviour
{
	private special_big_pack CurInfo;

	public UILabel btnLabel;

	public UILabel timeNamelabel;

	public UILabel TimeLabel;

	public List<RewardItem> rewardItems;

	private List<GameItem> ItemList = new List<GameItem>();

	public UIGrid parentGrid1;

	public UIGrid parentGrid2;

	private BigPackageData curData;

	private int CurState;

	private string CurID;

	private long reamainTime;

	public UIEventListener RotateModelBtnListener;

	private FakeObjLogic mCurFakeObj;

	public UITexture ModelPic;

	public UITexture CarModelPic;

	private Transform CarMeshRoot;

	private Color ambientLight;

	public UITexture BgTexture;

	public UITexture fontTexture;

	private List<string> textureList = new List<string>();

	private TimeSpan LimitTimeSpan;

	private string dayName;

	private bool IsDollorBuy;

	private float tempCountTime;

	private void Update()
	{
		if (reamainTime > 0)
		{
			tempCountTime += Time.deltaTime;
			if (tempCountTime >= 1f)
			{
				reamainTime--;
				tempCountTime -= 1f;
				TimeLabel.text = TimeTools.GetFullTime(reamainTime);
			}
			if (reamainTime <= 0)
			{
				tempCountTime = 0f;
				TimeLabel.enabled = false;
				timeNamelabel.enabled = false;
			}
		}
	}

	public void InitTexture()
	{
		List<string> list = new List<string>();
		if (!string.IsNullOrEmpty(curData.TextureTitle1))
		{
			list.Add(curData.TextureTitle1);
		}
		if (!string.IsNullOrEmpty(curData.TextureTitle2))
		{
			list.Add(curData.TextureTitle2);
		}
		if (list.Count != 0 && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(list, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(Dictionary<string, Texture> retdic)
	{
		if (retdic != null && retdic.Count != 0)
		{
			if (!string.IsNullOrEmpty(curData.TextureTitle1) && retdic.ContainsKey(curData.TextureTitle1))
			{
				fontTexture.mainTexture = retdic[curData.TextureTitle1];
				fontTexture.SetDimensions(retdic[curData.TextureTitle1].width, retdic[curData.TextureTitle1].height);
			}
			else
			{
				fontTexture.mainTexture = null;
			}
			if (!string.IsNullOrEmpty(curData.TextureTitle2) && retdic.ContainsKey(curData.TextureTitle2))
			{
				BgTexture.mainTexture = retdic[curData.TextureTitle2];
				BgTexture.SetDimensions(retdic[curData.TextureTitle2].width, retdic[curData.TextureTitle2].height);
			}
			else
			{
				BgTexture.mainTexture = null;
			}
		}
	}

	public void RefershInfo(special_big_pack curPackinfo, Color oriColor)
	{
		ambientLight = oriColor;
		CurInfo = curPackinfo;
		CurID = CurInfo.ID;
		curData = DataManager.GetBigPackageDataById(CurInfo.ID);
		InitTexture();
		CurState = (int)CurInfo.state;
		reamainTime = 0L;
		tempCountTime = 0f;
		TimeLabel.enabled = false;
		timeNamelabel.enabled = false;
		if (!string.IsNullOrEmpty(curData.TimeList))
		{
			dayName = StrDictionary.GetDictionaryString("#{100241}");
			reamainTime = (long)TimeTools.GetShopItemTime(curData.GetCurTimeEnd()).TotalSeconds;
			if (reamainTime > 0)
			{
				TimeLabel.text = TimeTools.GetFullTime(reamainTime);
				TimeLabel.enabled = true;
				timeNamelabel.enabled = true;
			}
		}
		if (string.IsNullOrEmpty(curData.ProductId))
		{
			IsDollorBuy = false;
		}
		else
		{
			IsDollorBuy = true;
		}
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
		int num = ItemList.Count - rewardItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(rewardItems[0].gameObject) as GameObject;
				RewardItem component = gameObject.GetComponent<RewardItem>();
				gameObject.name = $"reward{rewardItems.Count:D2}";
				gameObject.transform.parent = parentGrid1.transform;
				gameObject.transform.localScale = Vector3.one;
				rewardItems.Add(component);
			}
		}
		for (int j = 0; j < rewardItems.Count; j++)
		{
			if (j < ItemList.Count)
			{
				if (j < 2)
				{
					rewardItems[j].transform.parent = parentGrid1.transform;
					rewardItems[j].transform.localScale = Vector3.one;
				}
				else
				{
					rewardItems[j].transform.parent = parentGrid2.transform;
					rewardItems[j].transform.localScale = Vector3.one;
				}
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
		parentGrid1.Reposition();
		parentGrid2.Reposition();
		if (CurState == 0)
		{
			btnLabel.text = StrDictionary.GetDictionaryString("#{300601}");
		}
		else
		{
			btnLabel.text = StrDictionary.GetDictionaryString("#{301116}");
		}
		if (curData.showmodeltype == 1)
		{
			ResetModelVisual(ItemList);
		}
		else if (curData.showmodeltype == 2)
		{
			for (int k = 0; k < ItemList.Count; k++)
			{
				if (ItemList[k].ItemData.Type == GameDefine.ITEM_TYPE.EXCHANGE)
				{
					MountData mountDataById = DataManager.GetMountDataById(ItemList[k].ItemData.Function.ToString());
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
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "BigSales", "open");
	}

	public void OnClickBuyBtn()
	{
		if (CurState != 0)
		{
			return;
		}
		if (IsDollorBuy)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.Billing(curData.ProductId);
			if (GameSettingData.IsTestBilling)
			{
				WaitResponseUIRootLogic.OpenWaitBox(266, 10f, 0f);
				check_purchase.request request = new check_purchase.request();
				request.productId = curData.ProductId;
				NetLogic.GetInstance().Send<Protocol.check_purchase>(request);
			}
		}
		else if (GameMoneyHelper.BeforeCheckBuy(curData.PriceType, curData.PriceCost))
		{
			WaitResponseUIRootLogic.OpenWaitBox(272, 10f, 0f);
			buy_big_pack.request request2 = new buy_big_pack.request();
			request2.ID = curData.ID;
			NetLogic.GetInstance().Send<Protocol.buy_big_pack>(request2);
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BigPackRoot);
		if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
		{
			SingletonUnity<AutoPopUIRoot>.Instance.NextPop();
		}
	}

	public void UpdateInfo(string id)
	{
		if (CurID.Equals(id))
		{
			CurState = 1;
			btnLabel.text = StrDictionary.GetDictionaryString("#{301116}");
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
		ItemContainer equipPack = playerData.EquipPack;
		string headId = string.Empty;
		string weaponId = string.Empty;
		string bodyId = string.Empty;
		string legId = string.Empty;
		switch (playerData.Profession)
		{
		case PROFESSION_TYPE.XD:
			weaponId = GameDefine.XD_DefaultModel[0];
			headId = GameDefine.XD_DefaultModel[1];
			bodyId = GameDefine.XD_DefaultModel[2];
			legId = GameDefine.XD_DefaultModel[3];
			break;
		case PROFESSION_TYPE.QJ:
			weaponId = GameDefine.QJ_DefaultModel[0];
			headId = GameDefine.QJ_DefaultModel[1];
			bodyId = GameDefine.QJ_DefaultModel[2];
			legId = GameDefine.QJ_DefaultModel[3];
			break;
		case PROFESSION_TYPE.NQS:
			weaponId = GameDefine.NQS_DefaultModel[0];
			headId = GameDefine.NQS_DefaultModel[1];
			bodyId = GameDefine.NQS_DefaultModel[2];
			legId = GameDefine.NQS_DefaultModel[3];
			break;
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
					headId = equipDataById.ModelId;
					break;
				case EQUIP_BACKPACK_TYPE.WEAPON:
					weaponId = equipDataById.ModelId;
					break;
				case EQUIP_BACKPACK_TYPE.BODY:
					bodyId = equipDataById.ModelId;
					break;
				case EQUIP_BACKPACK_TYPE.LEG:
					legId = equipDataById.ModelId;
					break;
				}
			}
		}
		if (mCurFakeObj != null)
		{
			mCurFakeObj.DestroyFakeObj();
		}
		if (mCurFakeObj == null || mCurFakeObj.FakeObj == null)
		{
			mCurFakeObj = new FakeObjLogic();
			mCurFakeObj.InitFakeObject(weaponId, headId, bodyId, legId, playerData.Profession, SingletonUnity<FakeObjRootLogic>.Instance.MeshRoot);
		}
		else
		{
			mCurFakeObj.CheckFakeObject(weaponId, headId, bodyId, legId);
			mCurFakeObj.PlayAnim("idle", playerData.CharacterModelData.ModelFirstType);
		}
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
		SingletonUnity<FakeObjRootLogic>.Instance.SetPicValue(0.9f);
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
