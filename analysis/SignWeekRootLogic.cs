using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SignWeekRootLogic : SingletonUnity<SignWeekRootLogic>
{
	public UIEventListener RotateModelBtnListener;

	private FakeObjLogic mCurFakeObj;

	public UITexture ModelPic;

	public UISlider DaySlider;

	private int curSign;

	private bool curSignState;

	private bool completeState;

	private List<SignInWeekData> SignDataList;

	private SignInWeekData curSigninWeekData;

	private ShowModelData curshowdata;

	public UIGrid SevenDayGrid;

	public List<WeekDayRewardItem> SevenDaysItems;

	private List<GameItem> SevenShowItemList = new List<GameItem>();

	public RewardItem BestItem;

	public UIGrid DayGrid;

	public List<RewardItem> rewardItemList;

	private List<GameItem> ShowDayItemList = new List<GameItem>();

	private GameItem curbestItem;

	public UISprite BtnSp;

	public UILabel btnLabel;

	public UILabel dayinfoLabel;

	public UITexture CarModelPic;

	private Transform CarMeshRoot;

	private int curShowDay;

	private int needshowday;

	private Color ambientLight;

	private GameObject FakeItemObj;

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SignWeekRoot);
		if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
		{
			SingletonUnity<AutoPopUIRoot>.Instance.NextPop();
		}
	}

	public void Showcarday()
	{
		needshowday = 3;
	}

	public void EnableReset()
	{
		if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
		{
			SingletonUnity<AutoPopUIRoot>.Instance.RemoveUI(GameDefine.AUTOPOPTYPE.SIGNWEEK);
		}
		ModelPic.enabled = false;
		CarModelPic.enabled = false;
		needshowday = -1;
		SignDataList = DataManager.GetSignInWeekDataList();
		int num = SignDataList.Count - SevenDaysItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(SevenDaysItems[0].gameObject) as GameObject;
				WeekDayRewardItem component = gameObject.GetComponent<WeekDayRewardItem>();
				gameObject.name = $"dayitem{SevenDaysItems.Count:D2}";
				gameObject.transform.parent = SevenDayGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				SevenDaysItems.Add(component);
			}
			SevenDayGrid.Reposition();
		}
		for (int j = 0; j < SevenDaysItems.Count; j++)
		{
			if (j < SignDataList.Count)
			{
				GetDayBestItem(j);
				SevenDaysItems[j].UpdateItem(curbestItem, j + 1, OnClickDayItem);
				if (j == SignDataList.Count - 1)
				{
					SevenDaysItems[j].transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
				}
			}
			UnityVersionUtil.SetActiveRecursive(SevenDaysItems[j].gameObject, state: false);
		}
		for (int k = 0; k < rewardItemList.Count; k++)
		{
			UnityVersionUtil.SetActiveRecursive(rewardItemList[k].gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(dayinfoLabel.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(BestItem.gameObject, state: false);
		curShowDay = -1;
		curSign = -2;
		ambientLight = RenderSettings.ambientLight;
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
		if (FakeItemObj != null)
		{
			Object.DestroyImmediate(FakeItemObj, allowDestroyingAssets: true);
		}
		RotateModelBtnListener.onDrag = OnDragModelBtn;
		RotateModelBtnListener.onPress = null;
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
		if (FakeItemObj != null)
		{
			Object.DestroyImmediate(FakeItemObj, allowDestroyingAssets: true);
		}
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

	private void ShowModelVisual(int dayi)
	{
		curSigninWeekData = SignDataList[dayi];
		GetDayBestItem(dayi);
		GameItem gameItem = curbestItem;
		if (!gameItem.IsEmpty())
		{
			ItemData itemData = gameItem.ItemData;
			if (itemData.Type == GameDefine.ITEM_TYPE.EQUIP || itemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
			{
				ResetModelVisual(itemData);
			}
			else if (itemData.Type == GameDefine.ITEM_TYPE.EXCHANGE)
			{
				MountData mountDataById = DataManager.GetMountDataById(itemData.Function.ToString());
				ResetCarModelVisual(mountDataById, DataManager.GetColorDataById(mountDataById.DefaultColorId));
			}
			else
			{
				ResetItemModelVisual(itemData.Type);
			}
		}
	}

	private void ResetItemModelVisual(GameDefine.ITEM_TYPE curType)
	{
		if (mCurFakeObj != null)
		{
			mCurFakeObj.DestroyFakeObj();
			mCurFakeObj = null;
		}
		ResetFakeObjRoot();
		if (curshowdata != null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadItem(curshowdata.ModelName, ItemLoadFinish));
		}
	}

	private void ItemLoadFinish(string name, Object fakeobj, object param1 = null, object param2 = null)
	{
		if (fakeobj != null)
		{
			FakeItemObj = Object.Instantiate(fakeobj) as GameObject;
			if (FakeItemObj != null)
			{
				BundleManager.ResetAllShader(FakeItemObj.transform);
				NGUITools.SetLayer(FakeItemObj, SingletonUnity<FakeObjRootLogic>.Instance.MeshRoot.gameObject.layer);
				FakeItemObj.transform.parent = SingletonUnity<FakeObjRootLogic>.Instance.MeshRoot.transform;
				FakeItemObj.transform.localPosition = curshowdata.Position;
				FakeItemObj.transform.localRotation = Quaternion.Euler(curshowdata.Rotation);
			}
		}
	}

	private void ResetCarModelVisual(MountData mountData, ColorData datacolor)
	{
		ResetFakeCarObjRoot();
		SingletonUnity<FakeCarObjRootLogic>.Instance.InitFakeCarObj(mountData, datacolor);
	}

	private void ResetModelVisual(ItemData curItemdata)
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
		EquipData equipDataById = DataManager.GetEquipDataById(curItemdata.ID);
		switch ((EQUIP_BACKPACK_TYPE)curItemdata.SubType)
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
		SingletonUnity<FakeObjRootLogic>.Instance.MoveShowPart((int)playerData.Profession, curItemdata.SubType);
	}

	public void Reset(ret_request_sign_week_info.request request)
	{
		curSign = (int)request.cur_sign;
		curSignState = request.cur_sign_state;
		completeState = request.complete;
		RefershUI();
		if (needshowday != -1)
		{
			OnClickDayItem(needshowday);
		}
		else if (curSignState)
		{
			OnClickDayItem(curSign);
		}
		else if (curSign < 7)
		{
			OnClickDayItem(curSign + 1);
		}
		else
		{
			OnClickDayItem(7);
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Sign7", "open");
	}

	public void OnClickDayItem(int daynum)
	{
		curShowDay = daynum;
		UpdateBtnInfo();
		CalshowDayList(curShowDay - 1);
		ShowDayInfo();
		ShowModelVisual(curShowDay - 1);
	}

	public void UpdateInfo(ret_sign_week.request request)
	{
		if (curSignState)
		{
			LocalDataSaveManager.SetRewardFlag();
		}
		curSign = (int)request.cur_sign;
		curSignState = request.cur_sign_state;
		if (curSign == 7 && !curSignState)
		{
			completeState = true;
		}
		RefershUI();
		ShowRewardUI();
		if (curSignState)
		{
			OnClickDayItem(curSign);
		}
		else if (curSign < 7)
		{
			OnClickDayItem(curSign + 1);
		}
		else
		{
			OnClickDayItem(7);
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Sign7", $"require_{curSign}");
	}

	public void ShowRewardUI()
	{
		CalshowDayList(curSign - 1);
		ShowDayItemList.Insert(0, curbestItem);
		SimpleRewardRootLogic.AddRewards(ShowDayItemList);
	}

	public void RefershUI()
	{
		for (int i = 0; i < SevenDaysItems.Count; i++)
		{
			UnityVersionUtil.SetActiveRecursive(SevenDaysItems[i].gameObject, state: true);
			if (completeState)
			{
				SevenDaysItems[i].UpdataStateInfo(DayItemState.ISGET);
			}
			else if (i == curSign - 1)
			{
				if (curSignState)
				{
					SevenDaysItems[i].UpdataStateInfo(DayItemState.CURSIGN);
				}
				else
				{
					SevenDaysItems[i].UpdataStateInfo(DayItemState.ISGET);
				}
			}
			else if (i < curSign - 1)
			{
				SevenDaysItems[i].UpdataStateInfo(DayItemState.ISGET);
			}
			else
			{
				SevenDaysItems[i].UpdataStateInfo(DayItemState.NONE);
			}
		}
		SevenDayGrid.Reposition();
		DaySlider.value = (float)(curSign - 1) / 6f;
	}

	public void UpdateBtnInfo()
	{
		if (completeState)
		{
			BtnSp.spriteName = GameDefine.BtnIconNew[1];
			btnLabel.text = StrDictionary.GetDictionaryString("#{300403}");
			UnityVersionUtil.SetActiveRecursive(dayinfoLabel.gameObject, state: false);
		}
		else if (curShowDay == curSign)
		{
			if (!curSignState)
			{
				BtnSp.spriteName = GameDefine.BtnIconNew[1];
				btnLabel.text = StrDictionary.GetDictionaryString("#{300403}");
				UnityVersionUtil.SetActiveRecursive(dayinfoLabel.gameObject, state: false);
			}
			else
			{
				BtnSp.spriteName = GameDefine.BtnIconNew[0];
				btnLabel.text = StrDictionary.GetDictionaryString("#{300402}");
				UnityVersionUtil.SetActiveRecursive(dayinfoLabel.gameObject, state: true);
				dayinfoLabel.text = StrDictionary.GetDictionaryString("#{300904}");
			}
		}
		else if (curShowDay < curSign)
		{
			BtnSp.spriteName = GameDefine.BtnIconNew[1];
			btnLabel.text = StrDictionary.GetDictionaryString("#{300403}");
			UnityVersionUtil.SetActiveRecursive(dayinfoLabel.gameObject, state: false);
		}
		else
		{
			BtnSp.spriteName = GameDefine.BtnIconNew[1];
			btnLabel.text = StrDictionary.GetDictionaryString("#{300402}");
			if (curShowDay == curSign + 1)
			{
				UnityVersionUtil.SetActiveRecursive(dayinfoLabel.gameObject, state: true);
				dayinfoLabel.text = StrDictionary.GetDictionaryString("#{300902}");
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(dayinfoLabel.gameObject, state: false);
			}
		}
	}

	public void OnClickRecevieBtn()
	{
		if (curShowDay == curSign && !completeState && curSignState)
		{
			WaitResponseUIRootLogic.OpenWaitBox(255, 10f, 0f);
			sign_week.request request = new sign_week.request();
			request.day = curSign;
			NetLogic.GetInstance().Send<Protocol.sign_week>();
		}
	}

	public void ShowDayInfo()
	{
		int level = 0;
		if (curbestItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(curbestItem.ItemData.SubType);
		}
		BestItem.UpdateItem(curbestItem, level);
		UnityVersionUtil.SetActiveRecursive(BestItem.gameObject, state: true);
		int num = ShowDayItemList.Count - rewardItemList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(rewardItemList[0].gameObject) as GameObject;
				RewardItem component = gameObject.GetComponent<RewardItem>();
				gameObject.name = $"dailybuyitem{rewardItemList.Count:D2}";
				gameObject.transform.parent = DayGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				rewardItemList.Add(component);
			}
		}
		for (int j = 0; j < rewardItemList.Count; j++)
		{
			if (j < ShowDayItemList.Count)
			{
				int level2 = 0;
				if (ShowDayItemList[j].ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					level2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(ShowDayItemList[j].ItemData.SubType);
				}
				rewardItemList[j].UpdateItem(ShowDayItemList[j], level2);
				UnityVersionUtil.SetActiveRecursive(rewardItemList[j].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(rewardItemList[j].gameObject, state: false);
			}
		}
		DayGrid.Reposition();
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
		ResetNormalLight();
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
		if (FakeItemObj != null)
		{
			Object.Destroy(FakeItemObj);
		}
	}

	public void CalshowDayList(int dayi)
	{
		ShowDayItemList.Clear();
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		switch (playerData.Profession)
		{
		case PROFESSION_TYPE.XD:
			curbestItem = new GameItem(SignDataList[dayi].XDItemID1, (EQUIP_QUALITY)SignDataList[dayi].XDItemQuality1, SignDataList[dayi].XDItemCount1);
			break;
		case PROFESSION_TYPE.QJ:
			curbestItem = new GameItem(SignDataList[dayi].QJItemID1, (EQUIP_QUALITY)SignDataList[dayi].QJItemQuality1, SignDataList[dayi].QJItemCount1);
			break;
		case PROFESSION_TYPE.NQS:
			curbestItem = new GameItem(SignDataList[dayi].NQSItemID1, (EQUIP_QUALITY)SignDataList[dayi].NQSItemQuality1, SignDataList[dayi].NQSItemCount1);
			break;
		}
		if (!string.IsNullOrEmpty(SignDataList[dayi].ItemID2))
		{
			ShowDayItemList.Add(new GameItem(SignDataList[dayi].ItemID2, (EQUIP_QUALITY)SignDataList[dayi].ItemQuality2, SignDataList[dayi].ItemCount2));
		}
		if (!string.IsNullOrEmpty(SignDataList[dayi].ItemID3))
		{
			ShowDayItemList.Add(new GameItem(SignDataList[dayi].ItemID3, (EQUIP_QUALITY)SignDataList[dayi].ItemQuality3, SignDataList[dayi].ItemCount3));
		}
		if (!string.IsNullOrEmpty(SignDataList[dayi].ItemID4))
		{
			ShowDayItemList.Add(new GameItem(SignDataList[dayi].ItemID4, (EQUIP_QUALITY)SignDataList[dayi].ItemQuality4, SignDataList[dayi].ItemCount4));
		}
		if (!string.IsNullOrEmpty(SignDataList[dayi].ItemID5))
		{
			ShowDayItemList.Add(new GameItem(SignDataList[dayi].ItemID5, (EQUIP_QUALITY)SignDataList[dayi].ItemQuality5, SignDataList[dayi].ItemCount5));
		}
	}

	public void GetDayBestItem(int dayi)
	{
		curshowdata = null;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		switch (playerData.Profession)
		{
		case PROFESSION_TYPE.XD:
			curbestItem = new GameItem(SignDataList[dayi].XDItemID1, (EQUIP_QUALITY)SignDataList[dayi].XDItemQuality1, SignDataList[dayi].XDItemCount1);
			if (!string.IsNullOrEmpty(SignDataList[dayi].XDShowModelID))
			{
				curshowdata = DataManager.GetShowModelDataById(SignDataList[dayi].XDShowModelID);
			}
			break;
		case PROFESSION_TYPE.QJ:
			curbestItem = new GameItem(SignDataList[dayi].QJItemID1, (EQUIP_QUALITY)SignDataList[dayi].QJItemQuality1, SignDataList[dayi].QJItemCount1);
			if (!string.IsNullOrEmpty(SignDataList[dayi].QJShowModelID))
			{
				curshowdata = DataManager.GetShowModelDataById(SignDataList[dayi].QJShowModelID);
			}
			break;
		case PROFESSION_TYPE.NQS:
			curbestItem = new GameItem(SignDataList[dayi].NQSItemID1, (EQUIP_QUALITY)SignDataList[dayi].NQSItemQuality1, SignDataList[dayi].NQSItemCount1);
			if (!string.IsNullOrEmpty(SignDataList[dayi].NQSShowModelID))
			{
				curshowdata = DataManager.GetShowModelDataById(SignDataList[dayi].NQSShowModelID);
			}
			break;
		}
	}
}
