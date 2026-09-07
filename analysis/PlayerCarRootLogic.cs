using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class PlayerCarRootLogic : SingletonUnity<PlayerCarRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UILabel CarNameLabel;

	public UILabel CarDescriptionLabel;

	public UISprite[] AttFlag;

	public UILabel[] AttrNameLabel;

	public UILabel[] AttrValLabel;

	public List<PlayerCarIconLogic> CarIconList;

	public UICenterOnChild CenterOnChild;

	public UIGrid CarIconRootGride;

	public UISprite ChoosedTargetPic;

	public GameObject HaveCarTipObj;

	public GameObject NoCarTipObj;

	public GameObject EquipedCarTipObj;

	public UIWidget EquipBtn;

	public GameObject buycolorBtn;

	public UISprite ColorPriceSp;

	public UILabel ColorPriceLabel;

	public UIWidget GetBtn;

	public UITexture CarModelPic;

	public Transform MeshRoot;

	public UIEventListener CarPicDragListener;

	public GameObject BtnHaveTips;

	private List<mount> mCurMountInfoList;

	private Dictionary<string, mount> mCurMountInfoDic;

	private Dictionary<string, color> mCurColorInfoDic;

	private MountData mCurMountData;

	public UIGrid ColorGride;

	public List<ColorItemLogic> ColorIconList;

	private List<color> curColorlist = new List<color>();

	private string curColorSelectId = string.Empty;

	private mount curMountInfo;

	private ColorData curColorData;

	public UILabel SteerAngleLabel;

	public UISlider SteerAngleSlider;

	public UILabel MaxSpeedLabel;

	public UISlider MaxSpeedSlider;

	public UILabel HpLabel;

	public UISlider HpSlider;

	public UILabel AccLabel;

	public UISlider AccSlider;

	private int MaxHPValue = 50000;

	private int MaxAtkValue = 50000;

	private int MaxSpeedValue = 6000;

	private int MaxSteerValue = 30;

	private int MaxAccValue = 3000;

	public UILabel BuyPriceLabel;

	public UILabel GetBtnLabel;

	private Color ambientLight;

	public Dictionary<string, mount> CurMountInfoDic
	{
		get
		{
			return mCurMountInfoDic;
		}
		set
		{
			mCurMountInfoDic = value;
		}
	}

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			TutorialManager.OnClickTutorialBtn onClickTutorialBtn = mOnClickTutorialBtn;
			mOnClickTutorialBtn = null;
			onClickTutorialBtn();
		}
	}

	private void BreakTutorial()
	{
		if (mOnClickTutorialBtn != null)
		{
			TutorialManager.CloseTutorial();
			mOnClickTutorialBtn = null;
		}
	}

	public void EnableReset()
	{
		NGUITools.SetActive(NoCarTipObj, state: false);
		NGUITools.SetActive(HaveCarTipObj, state: false);
		NGUITools.SetActive(EquipedCarTipObj, state: false);
		NGUITools.SetActive(GetBtn.gameObject, state: false);
		NGUITools.SetActive(EquipBtn.gameObject, state: false);
	}

	public void Reset(ret_mount_info.request request)
	{
		mCurMountInfoDic = request.mount_info;
		mCurMountInfoList = new List<mount>(mCurMountInfoDic.Values);
		mCurMountInfoList.Sort((mount pre, mount next) => pre.ID.CompareTo(next.ID));
		bool flag = false;
		ItemContainer itemBackPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack;
		MountData mountData = null;
		for (int num = mCurMountInfoList.Count - 1; num >= 0; num--)
		{
			if (mCurMountInfoList[num].state == 0L)
			{
				mountData = DataManager.GetMountDataById(mCurMountInfoList[num].ID);
				if (mountData.NeedShow == 0)
				{
					mCurMountInfoList.RemoveAt(num);
				}
				else if (!string.IsNullOrEmpty(mountData.StartTime) && !TimeTools.IsTimeRange(mountData.StartTimeList))
				{
					List<GameItem> itemByItemId = itemBackPack.GetItemByItemId(mountData.ItemID);
					if (itemByItemId == null || itemByItemId.Count == 0)
					{
						mCurMountInfoList.RemoveAt(num);
					}
				}
			}
		}
		int num2 = mCurMountInfoList.Count - CarIconList.Count;
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = Object.Instantiate(CarIconList[0].gameObject) as GameObject;
				gameObject.transform.parent = CarIconList[0].transform.parent;
				gameObject.gameObject.name = $"{CarIconList.Count:D2}";
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				CarIconList.Add(gameObject.GetComponent<PlayerCarIconLogic>());
			}
		}
		CarIconRootGride.Reposition();
		for (int j = 0; j < CarIconList.Count; j++)
		{
			if (j < mCurMountInfoList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(CarIconList[j].gameObject, state: true);
				CarIconList[j].Reset(mCurMountInfoList[j], OnClickCarIcon);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(CarIconList[j].gameObject, state: false);
			}
		}
		vp_Timer.In(Time.deltaTime * 2f, delegate
		{
			setbegin();
		});
	}

	public void setbegin()
	{
		bool flag = false;
		for (int i = 0; i < mCurMountInfoList.Count; i++)
		{
			if (mCurMountInfoList[i].state == 2)
			{
				CarIconList[i].OnClickBtn();
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			CarIconList[0].OnClickBtn();
		}
	}

	private void ResetFakeCarObjRoot()
	{
		FakeCarObjRootLogic instance = SingletonUnity<FakeCarObjRootLogic>.Instance;
		if (instance == null)
		{
			ResourcesManager.LoadAndInstantiate("UIRoot/FakeCarObjRoot");
			instance = SingletonUnity<FakeCarObjRootLogic>.Instance;
		}
		MeshRoot = instance.MeshRoot;
		SingletonUnity<FakeCarObjRootLogic>.Instance.EnableFakeObjRoot();
		SingletonUnity<FakeCarObjRootLogic>.Instance.PlayRotate();
		CarModelPic.mainTexture = instance.ModelPic;
	}

	public void UpdateCarPage(ret_mount_equip.request request)
	{
		mCurMountInfoDic = request.mount_info;
		mCurMountInfoList = new List<mount>(mCurMountInfoDic.Values);
		mCurMountInfoList.Sort((mount pre, mount next) => pre.ID.CompareTo(next.ID));
		for (int i = 0; i < CarIconList.Count; i++)
		{
			if (CarIconList[i].CurCarId.Equals(request.ID))
			{
				CarIconList[i].OnClickBtn();
				break;
			}
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Vehicle", $"Car_{request.ID}", "equip");
	}

	public void UpdateCarPage(ret_buy_car_shop.request request)
	{
		if (!request.HasMountId)
		{
			return;
		}
		if (mCurMountInfoDic.ContainsKey(request.mountId))
		{
			mCurMountInfoDic[request.mountId].state = request.state;
			mCurMountInfoList = new List<mount>(mCurMountInfoDic.Values);
			mCurMountInfoList.Sort((mount pre, mount next) => pre.ID.CompareTo(next.ID));
		}
		for (int i = 0; i < CarIconList.Count; i++)
		{
			if (CarIconList[i].CurCarId.Equals(request.mountId))
			{
				CarIconList[i].OnClickBtn();
				break;
			}
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Vehicle", $"Car_{request.mountId}", "Buy");
	}

	public void UpdateCarPage(string carId)
	{
		for (int i = 0; i < CarIconList.Count; i++)
		{
			if (CarIconList[i].CurCarId.Equals(carId))
			{
				CarIconList[i].OnClickBtn();
			}
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Vehicle", $"Car_{carId}", "get");
	}

	public void OnClickCarIcon(string id, GameObject iconObj)
	{
		if (string.IsNullOrEmpty(id))
		{
			return;
		}
		ChoosedTargetPic.transform.parent = iconObj.transform;
		ChoosedTargetPic.transform.localPosition = Vector3.zero;
		CenterOnChild.CenterOn(iconObj.transform);
		MountData mountDataById = DataManager.GetMountDataById(id);
		CarNameLabel.text = StrDictionary.GetDictionaryString(mountDataById.CarName);
		CarDescriptionLabel.text = StrDictionary.GetDictionaryString(mountDataById.Desc);
		AttFlag[0].spriteName = GameDefine.GetAttributeIcon(mountDataById.Status1);
		AttFlag[1].spriteName = GameDefine.GetAttributeIcon(mountDataById.Status2);
		AttFlag[2].spriteName = GameDefine.GetAttributeIcon(mountDataById.Status3);
		AttFlag[3].spriteName = GameDefine.GetAttributeIcon(mountDataById.Status4);
		AttrNameLabel[0].text = GameDefine.GetAttributeName_S(mountDataById.Status1);
		AttrNameLabel[1].text = GameDefine.GetAttributeName_S(mountDataById.Status2);
		AttrNameLabel[2].text = GameDefine.GetAttributeName_S(mountDataById.Status3);
		AttrNameLabel[3].text = GameDefine.GetAttributeName_S(mountDataById.Status4);
		AttrValLabel[0].text = GameDefine.GetAttributeValueStr(mountDataById.Status1, mountDataById.Value1);
		AttrValLabel[1].text = GameDefine.GetAttributeValueStr(mountDataById.Status2, mountDataById.Value2);
		AttrValLabel[2].text = GameDefine.GetAttributeValueStr(mountDataById.Status3, mountDataById.Value3);
		AttrValLabel[3].text = GameDefine.GetAttributeValueStr(mountDataById.Status4, mountDataById.Value4);
		curMountInfo = mCurMountInfoDic[id];
		mCurMountData = mountDataById;
		SteerAngleLabel.text = $"{mCurMountData.MaxSteerAngle}";
		SteerAngleSlider.value = ((float)MaxSteerValue - mCurMountData.MaxSteerAngle) / (float)MaxSteerValue;
		MaxSpeedLabel.text = $"{mCurMountData.MaxSp}";
		MaxSpeedSlider.value = (float)mCurMountData.MaxSpeed / (float)MaxSpeedValue;
		HpLabel.text = $"{mCurMountData.MaxHP}";
		HpSlider.value = (float)mCurMountData.MaxHP / (float)MaxHPValue;
		AccLabel.text = $"{mCurMountData.MaxAcce}";
		AccSlider.value = (float)mCurMountData.MaxAcceleration / (float)MaxAccValue;
		long state = curMountInfo.state;
		if (state >= 0 && state <= 3)
		{
			switch (state)
			{
			case 0L:
				NGUITools.SetActive(NoCarTipObj, state: true);
				NGUITools.SetActive(HaveCarTipObj, state: false);
				NGUITools.SetActive(EquipedCarTipObj, state: false);
				NGUITools.SetActive(GetBtn.gameObject, state: true);
				NGUITools.SetActive(EquipBtn.gameObject, state: false);
				UpdateBtnTips();
				break;
			case 1L:
				NGUITools.SetActive(NoCarTipObj, state: false);
				NGUITools.SetActive(HaveCarTipObj, state: true);
				NGUITools.SetActive(EquipedCarTipObj, state: false);
				NGUITools.SetActive(GetBtn.gameObject, state: false);
				NGUITools.SetActive(EquipBtn.gameObject, state: true);
				break;
			case 2L:
				NGUITools.SetActive(NoCarTipObj, state: false);
				NGUITools.SetActive(HaveCarTipObj, state: false);
				NGUITools.SetActive(EquipedCarTipObj, state: true);
				NGUITools.SetActive(GetBtn.gameObject, state: false);
				NGUITools.SetActive(EquipBtn.gameObject, state: false);
				break;
			case 3L:
			{
				NGUITools.SetActive(NoCarTipObj, state: false);
				NGUITools.SetActive(HaveCarTipObj, state: true);
				NGUITools.SetActive(EquipedCarTipObj, state: false);
				NGUITools.SetActive(GetBtn.gameObject, state: false);
				NGUITools.SetActive(EquipBtn.gameObject, state: true);
				change_mount_state.request request = new change_mount_state.request();
				request.ID = curMountInfo.ID;
				NetLogic.GetInstance().Send<Protocol.change_mount_state>(request);
				break;
			}
			}
		}
		curColorSelectId = curMountInfo.select;
		curColorData = DataManager.GetColorDataById(curMountInfo.select);
		ResetCarModelVisual(mountDataById, curColorData);
		ShowColorInfo();
	}

	public void ShowColorInfo()
	{
		curColorlist.Clear();
		mCurColorInfoDic = curMountInfo.colors;
		curColorlist = new List<color>(mCurColorInfoDic.Values);
		curColorlist.Sort((color pre, color next) => (pre.ID.Length != next.ID.Length) ? (pre.ID.Length - next.ID.Length) : pre.ID.CompareTo(next.ID));
		int num = curColorlist.Count - ColorIconList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(ColorIconList[0].gameObject) as GameObject;
				gameObject.transform.parent = ColorIconList[0].transform.parent;
				gameObject.gameObject.name = $"Color{ColorIconList.Count:D2}";
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				ColorIconList.Add(gameObject.GetComponent<ColorItemLogic>());
			}
			ColorGride.Reposition();
		}
		for (int j = 0; j < ColorIconList.Count; j++)
		{
			if (j < curColorlist.Count)
			{
				UnityVersionUtil.SetActiveRecursive(ColorIconList[j].gameObject, state: true);
				ColorIconList[j].Reset(curColorlist[j], curColorSelectId, OnClickColorItem);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(ColorIconList[j].gameObject, state: false);
			}
		}
		NGUITools.SetActive(buycolorBtn, state: false);
		ChangeCarColor();
	}

	public void OnClickColorItem(string id, GameObject colorObj)
	{
		if (curColorSelectId.Equals(id))
		{
			Debug.Log("id ==:" + id);
			return;
		}
		curColorSelectId = id;
		color color = curMountInfo.colors[id];
		curColorData = DataManager.GetColorDataById(color.ID);
		if (curMountInfo.state == 2)
		{
			if (color.state == 0L)
			{
				NGUITools.SetActive(buycolorBtn, state: true);
				ColorPriceSp.spriteName = GameMoneyHelper.GetMoneyIcon(curColorData.PriceType);
				ColorPriceLabel.text = curColorData.PriceCost.ToString();
			}
			else
			{
				NGUITools.SetActive(buycolorBtn, state: false);
				if (!curMountInfo.select.Equals(id))
				{
					ChangeColor();
				}
			}
		}
		else
		{
			NGUITools.SetActive(buycolorBtn, state: false);
		}
		for (int i = 0; i < ColorIconList.Count; i++)
		{
			ColorIconList[i].refersh(curColorSelectId);
		}
		ChangeCarColor();
	}

	public void OnClickColorbuyBtn()
	{
		OkBuyColor();
	}

	public void OkBuyColor()
	{
		if (GameMoneyHelper.BeforeCheckBuy(curColorData.PriceType, curColorData.PriceCost))
		{
			WaitResponseUIRootLogic.OpenWaitBox(241, 10f, 0f);
			mount_use_color.request request = new mount_use_color.request();
			request.mountId = curMountInfo.ID;
			request.colorId = curColorSelectId;
			NetLogic.GetInstance().Send<Protocol.mount_use_color>(request);
		}
	}

	public void ChangeColor()
	{
		WaitResponseUIRootLogic.OpenWaitBox(241, 10f, 0f);
		mount_use_color.request request = new mount_use_color.request();
		request.mountId = curMountInfo.ID;
		request.colorId = curColorSelectId;
		NetLogic.GetInstance().Send<Protocol.mount_use_color>(request);
	}

	public void buyColorSuccess(ret_mount_use_color.request request)
	{
		mCurMountInfoDic = request.mount_info;
		mCurMountInfoList = new List<mount>(mCurMountInfoDic.Values);
		mCurMountInfoList.Sort((mount pre, mount next) => pre.ID.CompareTo(next.ID));
		if (curMountInfo.ID.Equals(request.mountId))
		{
			curMountInfo = mCurMountInfoDic[request.mountId];
		}
		ShowColorInfo();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Vehicle", $"Car_{request.mountId}", $"color_{request.colorId}");
	}

	public void ChangeCarColor()
	{
		ColorData colorDataById = DataManager.GetColorDataById(curColorSelectId);
		if (SingletonUnity<FakeCarObjRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FakeCarObjRootLogic>.Instance.gameObject))
		{
			SingletonUnity<FakeCarObjRootLogic>.Instance.ChangeColor(colorDataById);
		}
	}

	private void ResetCarModelVisual(MountData mountData, ColorData colorda)
	{
		SingletonUnity<FakeCarObjRootLogic>.Instance.InitFakeCarObj(mountData, colorda);
	}

	public void OnDragPic(GameObject btn, Vector2 delta)
	{
		if (MeshRoot != null)
		{
			MeshRoot.transform.localEulerAngles -= new Vector3(0f, delta.x, 0f);
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

	public void OnClickEquipBtn()
	{
		mount_equip.request request = new mount_equip.request();
		request.ID = mCurMountData.ID;
		NetLogic.GetInstance().Send<Protocol.mount_equip>(request);
		WaitResponseUIRootLogic.OpenWaitBox(236, 10f, 0f);
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.ChangeMountCar(mCurMountData.ID, curMountInfo.select);
			Singleton<ObjManager>.Instance.MainPlayer.MountId = mCurMountData.ID;
		}
	}

	private void UpdateBtnTips()
	{
		bool flag = false;
		ItemContainer itemBackPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack;
		List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(itemBackPack, IsAll: false, GameDefine.ITEM_TYPE.EXCHANGE);
		bool flag2 = false;
		GameItem gameItem = null;
		if (targetTypeItem == null || targetTypeItem.Count == 0)
		{
			flag = false;
		}
		for (int i = 0; i < targetTypeItem.Count; i++)
		{
			if (targetTypeItem[i].ItemData.Function.ToString().Equals(mCurMountData.ID))
			{
				flag = true;
				break;
			}
		}
		NGUITools.SetActive(BtnHaveTips, flag);
		if (flag)
		{
			GetBtnLabel.text = StrDictionary.GetDictionaryString("#{101405}");
			BuyPriceLabel.text = string.Empty;
			GetBtn.transform.localPosition = new Vector3(246f, -111.1f, 0f);
		}
		else if (mCurMountData.PriceType != -1)
		{
			GetBtnLabel.text = StrDictionary.GetDictionaryString("#{300601}");
			BuyPriceLabel.text = GameMoneyHelper.GetMoneyValStr(mCurMountData.Price, mCurMountData.PriceType);
			GetBtn.transform.localPosition = new Vector3(305f, -111.1f, 0f);
		}
		else
		{
			GetBtnLabel.text = StrDictionary.GetDictionaryString("#{101405}");
			BuyPriceLabel.text = string.Empty;
			GetBtn.transform.localPosition = new Vector3(246f, -111.1f, 0f);
		}
	}

	public void OnClickGetBtn()
	{
		ItemContainer itemBackPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack;
		List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(itemBackPack, IsAll: false, GameDefine.ITEM_TYPE.EXCHANGE);
		bool flag = false;
		GameItem exchangeItem = null;
		if (targetTypeItem == null || targetTypeItem.Count == 0)
		{
			flag = false;
		}
		else
		{
			for (int i = 0; i < targetTypeItem.Count; i++)
			{
				if (targetTypeItem[i].ItemData.Function.ToString().Equals(mCurMountData.ID))
				{
					flag = true;
					exchangeItem = targetTypeItem[i];
					break;
				}
			}
		}
		if (flag)
		{
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{101408}", exchangeItem.ItemData.MName), StrDictionary.GetDictionaryString("#{101405}"), delegate
			{
				use_item.request rpcReq = new use_item.request
				{
					indexId = exchangeItem.IndexId
				};
				NetLogic.GetInstance().Send<Protocol.use_item>(rpcReq);
				WaitResponseUIRootLogic.OpenWaitBox(115, 10f, 0f);
			});
			if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_CLICK_ACQUIRE)
			{
				CheckTutorialEvent();
			}
		}
		else
		{
			if (mCurMountData.PriceType == -1)
			{
				ShowGetPath();
			}
			else if (GameMoneyHelper.BeforeCheckBuy(mCurMountData.PriceType, mCurMountData.Price))
			{
				WaitResponseUIRootLogic.OpenWaitBox(323, 10f, 0f);
				buy_car_shop.request request = new buy_car_shop.request();
				request.mountId = mCurMountData.ID;
				NetLogic.GetInstance().Send<Protocol.buy_car_shop>(request);
			}
			BreakTutorial();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.SetTutorialShowFinish(FUNCTION_TYPE.CAR);
		}
	}

	public void ShowGetPath()
	{
		switch (mCurMountData.GetPath)
		{
		case 1:
			break;
		case 2:
			if (CheckPathOpen(FUNCTION_TYPE.GIFT_7DAY))
			{
				NoticeLogic.AddNotifyData(mCurMountData.GetDesc);
				OnClickCloseBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CommercialUIRoot, delegate
				{
					SingletonUnity<CommercialUIRootLogic>.Instance.OnClickWeekBtn();
				});
			}
			break;
		case 3:
			if (CheckPathOpen(FUNCTION_TYPE.SHOP))
			{
				GameMoneyHelper.ShowItemProduct(mCurMountData.ItemID, GameDefine.SHOP_TYPE.EQUIP_SHOP);
			}
			break;
		case 4:
			if (CheckPathOpen(FUNCTION_TYPE.LOTTO))
			{
				OnClickCloseBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SlotUIRoot, delegate
				{
					SingletonUnity<SlotUIRootLogic>.Instance.EnableReset();
					WaitResponseUIRootLogic.OpenWaitBox(242, 10f, 0f);
					NetLogic.GetInstance().Send<Protocol.request_slot_info>();
				});
			}
			break;
		case 5:
			if (CheckPathOpen(FUNCTION_TYPE.MYSTERYSHOP))
			{
				OnClickCloseBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MysteryShopRoot, delegate
				{
					SingletonUnity<MysteryShopRootLogic>.Instance.EnableReset();
					WaitResponseUIRootLogic.OpenWaitBox(274, 10f, 0f);
					NetLogic.GetInstance().Send<Protocol.request_special_big_pack>();
					SingletonUnity<MysteryShopRootLogic>.Instance.TargetID = "2";
				});
			}
			break;
		case 6:
			if (string.IsNullOrEmpty(mCurMountData.StartTime))
			{
				if (CheckPathOpen(FUNCTION_TYPE.BIGSALES) && SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.Big_PackFlag)
				{
					OnClickCloseBtn();
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BigPackRoot, delegate
					{
						SingletonUnity<BigPackRootLogic>.Instance.EnableReset();
						WaitResponseUIRootLogic.OpenWaitBox(260, 10f, 0f);
						NetLogic.GetInstance().Send<Protocol.request_big_pack>();
					});
				}
			}
			else if (TimeTools.IsTimeRange(mCurMountData.StartTimeList, mCurMountData.EndTimeList))
			{
				if (CheckPathOpen(FUNCTION_TYPE.BIGSALES) && SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.Big_PackFlag)
				{
					OnClickCloseBtn();
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BigPackRoot, delegate
					{
						SingletonUnity<BigPackRootLogic>.Instance.EnableReset();
						WaitResponseUIRootLogic.OpenWaitBox(260, 10f, 0f);
						NetLogic.GetInstance().Send<Protocol.request_big_pack>();
					});
				}
			}
			else
			{
				NoticeLogic.AddNotifyData("#{202001}");
			}
			break;
		}
	}

	private bool CheckPathOpen(FUNCTION_TYPE curtype)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (!playerCommonData.IsFunctionUnlock(curtype))
		{
			int num = (int)curtype;
			int condition = DataManager.GetFunctionDataById(num.ToString()).Condition;
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", condition));
			return false;
		}
		return true;
	}

	public bool IsBackPackHaveCarTicket()
	{
		ItemContainer itemBackPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack;
		List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(itemBackPack, IsAll: false, GameDefine.ITEM_TYPE.EXCHANGE);
		bool result = false;
		GameItem gameItem = null;
		if (targetTypeItem == null || targetTypeItem.Count == 0)
		{
			result = false;
		}
		else
		{
			for (int i = 0; i < targetTypeItem.Count; i++)
			{
				if (targetTypeItem[i].ItemData.Function.ToString().Equals(mCurMountData.ID))
				{
					result = true;
					gameItem = targetTypeItem[i];
					break;
				}
			}
		}
		return result;
	}

	private void OnEnable()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(null, OnClickCloseBtn, hideTab: true, StrDictionary.GetDictionaryString("#{100121}"));
		});
		CarPicDragListener.onDrag = OnDragPic;
		CarPicDragListener.onPress = OnPressCarModelPic;
		ResetFakeCarObjRoot();
		ambientLight = RenderSettings.ambientLight;
		RenderSettings.ambientLight = new Color(0f, 0f, 0f, 1f);
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerCarRoot);
		if (SingletonUnity<FakeCarObjRootLogic>.Instance != null)
		{
			SingletonUnity<FakeCarObjRootLogic>.Instance.DisableFakeObjRoot();
		}
		RenderSettings.ambientLight = ambientLight;
	}

	public void OnClickLeftBtn()
	{
		int num = 0;
		for (int i = 0; i < CarIconList.Count; i++)
		{
			if (CarIconList[i].CurCarId.Equals(curMountInfo.ID))
			{
				num = i;
			}
		}
		if (num > 0)
		{
			OnClickCarIcon(CarIconList[num - 1].CurCarId, CarIconList[num - 1].gameObject);
		}
	}

	public void OnClickRightBtn()
	{
		int num = 0;
		for (int i = 0; i < CarIconList.Count; i++)
		{
			if (CarIconList[i].CurCarId.Equals(curMountInfo.ID))
			{
				num = i;
			}
		}
		if (num < CarIconList.Count - 1)
		{
			OnClickCarIcon(CarIconList[num + 1].CurCarId, CarIconList[num + 1].gameObject);
		}
	}
}
