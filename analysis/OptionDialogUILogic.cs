using SprotoType;
using UnityEngine;

public class OptionDialogUILogic : SingletonUnity<OptionDialogUILogic>
{
	public UILabel TitleLabel;

	public UILabel TextLabel;

	public UILabel Option1Label;

	public UILabel Option2Label;

	public NpcOptionDialogData optionData;

	public TeamFakeObjPicRootLogic NpcFakeObjRoot;

	public FakeObjLogic NpcFakeObj = new FakeObjLogic();

	public UITexture NpcPic;

	public GameObject CashRoot;

	public UILabel CashLabel;

	public UISprite CashPic;

	public GameObject NoBtnRoot;

	private DelegateDefine.OneStringParamDelegate mOnClickOk;

	private string mParam;

	private bool mCloseOptionFlag;

	private NpcData mCurNpcData;

	private void Reset()
	{
		TitleLabel.text = string.Empty;
		TextLabel.text = string.Empty;
		Option1Label.text = string.Empty;
		Option2Label.text = string.Empty;
		mOnClickOk = null;
		mParam = string.Empty;
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.OptionDialogUI);
		Singleton<DialogManager>.Instance.OnCloseDialog();
		SingletonUnity<UIManager>.Instance.CheckReShowUI(UIInfo.OptionDialogUI);
	}

	public void OnClickOkBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.OptionDialogUI);
		Singleton<DialogManager>.Instance.OnCloseDialog();
		if (mOnClickOk != null)
		{
			mOnClickOk(mParam);
		}
		else if (!mCloseOptionFlag)
		{
			switch (optionData.Type)
			{
			case OPTION_TYPE.ENTERCOPY:
				EnterCopy();
				break;
			case OPTION_TYPE.WEAPONSHOP:
				OpenWeaponShop();
				break;
			case OPTION_TYPE.STORAGE:
				OpenStorage();
				break;
			case OPTION_TYPE.SEX_NPC_MAN:
				OpenSexManDialog();
				break;
			case OPTION_TYPE.SEX_NPC_WOMEN:
				OpenSexWomenDialog();
				break;
			case OPTION_TYPE.OPEN_SHOP:
				OpenShop();
				break;
			}
		}
	}

	private void OpenShop()
	{
		switch ((GameDefine.NPC_FUNCTION_TYPE)mCurNpcData.FunctionType)
		{
		case GameDefine.NPC_FUNCTION_TYPE.RECHARGE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickBuyDiamondBtn();
			});
			break;
		case GameDefine.NPC_FUNCTION_TYPE.TOOL_SHOP:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickToolsBtn();
			});
			break;
		case GameDefine.NPC_FUNCTION_TYPE.EQUIP_SHOP:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickEquipBtn();
			});
			break;
		case GameDefine.NPC_FUNCTION_TYPE.BIGSALE_SHOP:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickBigSaleBtn();
			});
			break;
		case GameDefine.NPC_FUNCTION_TYPE.GANG_SHOP:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.Reset();
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickGuildBtn();
			});
			break;
		case GameDefine.NPC_FUNCTION_TYPE.MONTH_CARD:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickMonthlyCardBtn();
			});
			break;
		}
	}

	private void OpenSexManDialog()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SexGameUIRoot, delegate
		{
			SingletonUnity<SexGameUIRootLogic>.Instance.Reset(isMan: true, optionData.OptionParam, mCurNpcData);
		});
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "activity_8", "accept_man");
	}

	private void OpenSexWomenDialog()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SexGameUIRoot, delegate
		{
			SingletonUnity<SexGameUIRootLogic>.Instance.Reset(isMan: false, optionData.OptionParam, mCurNpcData);
		});
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "activity_8", "accept_women");
	}

	private void OpenStorage()
	{
	}

	private void OnOpenStorageRoot(bool isSuccess, object param)
	{
	}

	private void OpenWeaponShop()
	{
	}

	private void OnOpenWeaponShopPage(bool isSuccess, object param)
	{
	}

	private void EnterCopy()
	{
		Debug.Log("EnterCopy");
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
		enter_copy_scene.request request = new enter_copy_scene.request();
		request.mapInfoId = "1";
		NetLogic.GetInstance().Send<Protocol.enter_copy_scene>(request);
	}

	public void ResetOptionDialog(string optDiaID, NpcData npcData, bool showCash = false)
	{
		Reset();
		if (showCash)
		{
			NGUITools.SetActive(CashRoot, state: true);
			CashLabel.text = $"{GameMoneyHelper.GetCash():N0}";
		}
		else
		{
			NGUITools.SetActive(CashRoot, state: false);
		}
		optionData = DataManager.GetNpcOptionDialogDataByID(optDiaID);
		if (optionData != null)
		{
			if (optionData.Type == OPTION_TYPE.SEX_NPC_MAN || optionData.Type == OPTION_TYPE.SEX_NPC_WOMEN)
			{
				activity_info activity_info = null;
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.CurActivityDataDic.ContainsKey(optionData.OptionParam))
				{
					activity_info = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.CurActivityDataDic[optionData.OptionParam];
				}
				if (activity_info != null)
				{
					int num = (int)activity_info.CurNum;
					SexMiniData sexMiniDataById = DataManager.GetSexMiniDataById(optionData.OptionParam);
					if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.Level < sexMiniDataById.UnlockLevel)
					{
						UnityVersionUtil.SetActiveRecursive(NoBtnRoot, state: false);
						TextLabel.text = optionData.MOptionFailDialog;
						Option1Label.text = optionData.MFailOption;
						mCloseOptionFlag = true;
					}
					else
					{
						int num2 = sexMiniDataById.CostStart + sexMiniDataById.CostAdd * num;
						if (num2 > sexMiniDataById.CostMax)
						{
							num2 = sexMiniDataById.CostMax;
						}
						CashPic.spriteName = GameMoneyHelper.GetMoneyPicName((GameDefine.MONEY_TYPE)sexMiniDataById.CostType);
						CashLabel.text = $"{GameMoneyHelper.GetMoneyNum(sexMiniDataById.CostType):N0}";
						if (GameMoneyHelper.GetMoneyNum(sexMiniDataById.CostType) < num2)
						{
							UnityVersionUtil.SetActiveRecursive(NoBtnRoot, state: false);
							TextLabel.text = StrDictionary.GetDictionaryString(optionData.OptionFailDialog);
							Option1Label.text = StrDictionary.GetDictionaryString(optionData.MFailOption);
							mCloseOptionFlag = true;
						}
						else
						{
							TextLabel.text = StrDictionary.GetDictionaryString(optionData.CenterDialog, GameMoneyHelper.GetMoneyValStr(num2, (GameDefine.MONEY_TYPE)sexMiniDataById.CostType));
							Option1Label.text = StrDictionary.GetDictionaryString(optionData.MOption1);
							Option2Label.text = StrDictionary.GetDictionaryString(optionData.MOption2);
						}
					}
				}
				else
				{
					Debug.Log("No Activity Data Error!!!!!!!!!!!!!!!!!!!!!!!!!!");
				}
			}
			else if (optionData.Type == OPTION_TYPE.OPEN_SHOP)
			{
				bool flag = false;
				switch ((GameDefine.NPC_FUNCTION_TYPE)npcData.FunctionType)
				{
				case GameDefine.NPC_FUNCTION_TYPE.RECHARGE:
					if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP_BUY))
					{
						flag = true;
					}
					break;
				case GameDefine.NPC_FUNCTION_TYPE.TOOL_SHOP:
					if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP_TOOL))
					{
						flag = true;
					}
					break;
				case GameDefine.NPC_FUNCTION_TYPE.EQUIP_SHOP:
					if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP_EQUIP))
					{
						flag = true;
					}
					break;
				case GameDefine.NPC_FUNCTION_TYPE.BIGSALE_SHOP:
					if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP_BIGSALE))
					{
						flag = true;
					}
					break;
				case GameDefine.NPC_FUNCTION_TYPE.GANG_SHOP:
					if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP_GUILD))
					{
						flag = true;
					}
					break;
				case GameDefine.NPC_FUNCTION_TYPE.MONTH_CARD:
					if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP_VIP))
					{
						flag = true;
					}
					break;
				}
				if (!flag)
				{
					UnityVersionUtil.SetActiveRecursive(NoBtnRoot, state: false);
					TextLabel.text = optionData.MOptionFailDialog;
					Option1Label.text = optionData.MFailOption;
					mCloseOptionFlag = true;
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(NoBtnRoot, state: true);
					TextLabel.text = StrDictionary.GetDictionaryString(optionData.CenterDialog);
					Option1Label.text = StrDictionary.GetDictionaryString(optionData.Option1);
					Option2Label.text = StrDictionary.GetDictionaryString(optionData.Option2);
					mCloseOptionFlag = false;
				}
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(NoBtnRoot, state: true);
				TextLabel.text = StrDictionary.GetDictionaryString(optionData.CenterDialog);
				Option1Label.text = StrDictionary.GetDictionaryString(optionData.Option1);
				Option2Label.text = StrDictionary.GetDictionaryString(optionData.Option2);
				mCloseOptionFlag = false;
			}
		}
		NpcFakeObjRoot.EnableFakeObjRoot();
		NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
		NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
		mCurNpcData = npcData;
	}

	public void ResetOptionDialog(NpcData npcData, string textStr, string option1Str, string option2Str, string param, DelegateDefine.OneStringParamDelegate func, bool showCash = false)
	{
		Reset();
		TextLabel.text = StrDictionary.GetDictionaryString(textStr);
		Option1Label.text = StrDictionary.GetDictionaryString(option1Str);
		Option2Label.text = StrDictionary.GetDictionaryString(option2Str);
		mOnClickOk = func;
		mParam = param;
		NpcFakeObjRoot.EnableFakeObjRoot();
		NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
		NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
		if (showCash)
		{
			NGUITools.SetActive(CashRoot, state: true);
			CashLabel.text = $"{GameMoneyHelper.GetCash():N0}";
		}
		else
		{
			NGUITools.SetActive(CashRoot, state: false);
		}
	}

	public void ResetOptionDialog(string weaponId, string headId, string bodyId, string legId, string textStr, string option1Str, string option2Str, string param, DelegateDefine.OneStringParamDelegate func, bool showCash = false)
	{
		Reset();
		TextLabel.text = StrDictionary.GetDictionaryString(textStr);
		Option1Label.text = StrDictionary.GetDictionaryString(option1Str);
		Option2Label.text = StrDictionary.GetDictionaryString(option2Str);
		mOnClickOk = func;
		mParam = param;
		NpcFakeObjRoot.EnableFakeObjRoot();
		NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
		NpcFakeObj.InitFakeObject(weaponId, headId, bodyId, legId, NpcFakeObjRoot.MeshRoot, null, "FakeObj");
		if (showCash)
		{
			NGUITools.SetActive(CashRoot, state: true);
			CashLabel.text = $"{GameMoneyHelper.GetCash():N0}";
		}
		else
		{
			NGUITools.SetActive(CashRoot, state: false);
		}
	}

	private void OnEnable()
	{
		Singleton<ObjManager>.Instance.MainPlayer.IsTalking = true;
	}

	private void OnDisable()
	{
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.IsTalking = false;
		}
		NpcFakeObj.DisableNpcAnimaHandle();
	}

	protected new virtual void OnDestroy()
	{
		NpcFakeObj.DestroyNpcFakeObj();
		base.OnDestroy();
	}
}
