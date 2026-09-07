using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonUnity<UIManager>
{
	public enum SHOW_TYPE
	{
		DEFAULT,
		RANK_PVP,
		BIGSALE,
		SHOP,
		SLOT,
		WELFARE,
		MYSTERY,
		SKILL,
		ENHANCE_EQUIP,
		ENHANCE_STAR,
		INHERT
	}

	public delegate void OnOpenUIDelegate(bool bSuccess, object param);

	public delegate void OnLoadUIDelegate(GameObject resObject, object param);

	public static SHOW_TYPE NextShowType = SHOW_TYPE.DEFAULT;

	private Dictionary<string, GameObject> mDicBaseUI = new Dictionary<string, GameObject>();

	private Dictionary<string, GameObject> mDicPopUI = new Dictionary<string, GameObject>();

	private Dictionary<string, GameObject> mDicCacheUI = new Dictionary<string, GameObject>();

	private Dictionary<string, GameObject> mDicMenuPopUI = new Dictionary<string, GameObject>();

	private Dictionary<string, GameObject> mDicMessageUI = new Dictionary<string, GameObject>();

	private Dictionary<string, GameObject> mDicMenuTopUI = new Dictionary<string, GameObject>();

	private Dictionary<string, GameObject> mDicMenuTop2UI = new Dictionary<string, GameObject>();

	private Dictionary<string, GameObject> mDicMenuTop3UI = new Dictionary<string, GameObject>();

	private UIPanel BaseUIRoot;

	private UIPanel PopUIRoot;

	private UIPanel TipUIRoot;

	private UIPanel MenuPopUIRoot;

	private UIPanel MenuTop2UIRoot;

	private UIPanel MenuTopUIRoot;

	private UIPanel MenuTop3UIRoot;

	private UIPanel MessageUIRoot;

	private UIPanel DeathUIRoot;

	public List<UIPathData> mHideObjList = new List<UIPathData>();

	public List<UIPathData> CurShowUIList = new List<UIPathData>();

	public static List<string> CanReShowUIList = new List<string>();

	public bool IsHideBaseUI;

	private List<UIPanel> mBaseUIPanelList = new List<UIPanel>();

	protected override void Awake()
	{
		base.Awake();
		Init();
		GameObject res = ResourcesManager.LoadAndInstantiate("UIRoot/EmptyPanel") as GameObject;
		if (BaseUIRoot == null)
		{
			BaseUIRoot = CreateRootObj(res, "BaseUIRoot", 10);
		}
		if (PopUIRoot == null)
		{
			PopUIRoot = CreateRootObj(res, "PopUIRoot", 20);
		}
		if (MenuPopUIRoot == null)
		{
			MenuPopUIRoot = CreateRootObj(res, "MenuPopUIRoot", 30);
		}
		if (MenuTopUIRoot == null)
		{
			MenuTopUIRoot = CreateRootObj(res, "MenuTopUIRoot", 40);
		}
		if (MenuTop2UIRoot == null)
		{
			MenuTop2UIRoot = CreateRootObj(res, "MenuTop2UIRoot", 50);
		}
		if (MenuTop3UIRoot == null)
		{
			MenuTop3UIRoot = CreateRootObj(res, "MenuTop3UIRoot", 60);
		}
		if (MessageUIRoot == null)
		{
			MessageUIRoot = CreateRootObj(res, "MessageUIRoot", 70);
		}
		mHideObjList.Clear();
		CurShowUIList.Clear();
	}

	public static void Reset()
	{
		NextShowType = SHOW_TYPE.DEFAULT;
	}

	private void Start()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager != null)
		{
			ShowUI(UIInfo.LoadingUIRoot);
		}
	}

	private UIPanel CreateRootObj(GameObject res, string objName, int depth)
	{
		GameObject gameObject = Object.Instantiate(res) as GameObject;
		gameObject.gameObject.name = objName;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		UIPanel component = gameObject.GetComponent<UIPanel>();
		if (component != null)
		{
			component.depth = depth;
		}
		return component;
	}

	private void Init()
	{
		mDicBaseUI.Clear();
		mDicPopUI.Clear();
		mDicCacheUI.Clear();
		CurShowUIList.Clear();
	}

	public void ShowUI(UIPathData pathData, OnOpenUIDelegate delOpenUI = null, object param = null)
	{
		Dictionary<string, GameObject> dictionary = null;
		switch (pathData.uiType)
		{
		case UIPathData.UIType.TYPE_BASE:
			dictionary = mDicBaseUI;
			break;
		case UIPathData.UIType.TYPE_POP:
			dictionary = mDicPopUI;
			break;
		case UIPathData.UIType.TYPE_MENU_POP:
			dictionary = mDicMenuPopUI;
			break;
		case UIPathData.UIType.TYPE_MESSAGE:
			dictionary = mDicMessageUI;
			break;
		case UIPathData.UIType.TYPE_MENU_TOP:
			dictionary = mDicMenuTopUI;
			break;
		case UIPathData.UIType.TYPE_MEMU_TOP_2:
			dictionary = mDicMenuTop2UI;
			break;
		case UIPathData.UIType.TYPE_MEMU_TOP_3:
			dictionary = mDicMenuTop3UI;
			break;
		}
		if (dictionary == null)
		{
			return;
		}
		if (pathData.hideBase)
		{
			if (!mHideObjList.Contains(pathData))
			{
				mHideObjList.Add(pathData);
			}
			HideBaseUI();
		}
		if (mDicCacheUI.ContainsKey(pathData.name))
		{
			if (!dictionary.ContainsKey(pathData.name))
			{
				dictionary.Add(pathData.name, mDicCacheUI[pathData.name]);
			}
			mDicCacheUI.Remove(pathData.name);
		}
		if (dictionary.ContainsKey(pathData.name))
		{
			DoShowUI(pathData, dictionary[pathData.name], delOpenUI, param);
		}
		else
		{
			LoadUI(pathData, delOpenUI, param);
		}
	}

	public void LoadUI(UIPathData pathData, OnOpenUIDelegate delOpenUI = null, object param = null)
	{
		string text = "UI/" + pathData.path;
		text = text.Insert(text.LastIndexOf("/"), "/New");
		GameObject gameObject = ResourcesManager.Load(text) as GameObject;
		if (gameObject == null)
		{
			gameObject = ResourcesManager.Load("UI/" + pathData.path) as GameObject;
		}
		if (gameObject != null)
		{
			DoShowUI(pathData, gameObject, delOpenUI, param);
		}
	}

	public void LoadUIItem(UIPathData pathData, OnLoadUIDelegate delOpenUI = null, object param = null)
	{
		GameObject gameObject = ResourcesManager.Load("UI/" + pathData.path) as GameObject;
		if (gameObject != null)
		{
			delOpenUI?.Invoke(gameObject, param);
		}
	}

	private void DoShowUI(UIPathData pathData, GameObject curWindow, OnOpenUIDelegate delOpenUI, object param = null)
	{
		if (curWindow == null)
		{
			return;
		}
		Dictionary<string, GameObject> dictionary = null;
		Transform parent;
		switch (pathData.uiType)
		{
		case UIPathData.UIType.TYPE_BASE:
			dictionary = mDicBaseUI;
			parent = BaseUIRoot.transform;
			break;
		case UIPathData.UIType.TYPE_POP:
			dictionary = mDicPopUI;
			parent = PopUIRoot.transform;
			break;
		case UIPathData.UIType.TYPE_MENU_POP:
			dictionary = mDicMenuPopUI;
			parent = MenuPopUIRoot.transform;
			break;
		case UIPathData.UIType.TYPE_MESSAGE:
			dictionary = mDicMessageUI;
			parent = MessageUIRoot.transform;
			break;
		case UIPathData.UIType.TYPE_MENU_TOP:
			dictionary = mDicMenuTopUI;
			parent = MenuTopUIRoot.transform;
			break;
		case UIPathData.UIType.TYPE_MEMU_TOP_2:
			dictionary = mDicMenuTop2UI;
			parent = MenuTop2UIRoot.transform;
			break;
		case UIPathData.UIType.TYPE_MEMU_TOP_3:
			dictionary = mDicMenuTop3UI;
			parent = MenuTop3UIRoot.transform;
			break;
		default:
			parent = UIRoot.list[0].transform;
			break;
		}
		if (dictionary != null && dictionary.ContainsKey(pathData.name))
		{
			if (!UnityVersionUtil.IsActive(dictionary[pathData.name]))
			{
				dictionary[pathData.name].transform.parent = parent;
				dictionary[pathData.name].transform.localScale = Vector3.one;
				dictionary[pathData.name].transform.localPosition = Vector3.zero;
				NGUITools.SetActive(dictionary[pathData.name], state: true);
			}
		}
		else if (dictionary != null)
		{
			GameObject gameObject = Object.Instantiate(curWindow) as GameObject;
			gameObject.transform.parent = parent;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			if (gameObject != null)
			{
				dictionary.Add(pathData.name, gameObject);
			}
		}
		delOpenUI?.Invoke(curWindow != null, param);
		if (pathData.backFlag)
		{
			AddShowUIList(pathData);
		}
	}

	private void AddShowUIList(UIPathData newpathdata)
	{
		if (CurShowUIList.Contains(newpathdata))
		{
			CurShowUIList.Remove(newpathdata);
		}
		CurShowUIList.Insert(0, newpathdata);
	}

	private IEnumerator DelayCheckUnlockFunction(UIPathData pathData)
	{
		yield return null;
		if (!IsHideBaseUI && !SingletonUnity<LoadingUIRoot>.Exists)
		{
			if (pathData == null)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckShowUnlockFunction();
			}
			else if (TutorialManager.IsNeedCheckTutorial(pathData))
			{
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckShowUnlockFunction();
			}
		}
	}

	public void CloseUI(UIPathData pathData)
	{
		if (pathData.hideBase)
		{
			if (mHideObjList.Contains(pathData))
			{
				mHideObjList.Remove(pathData);
			}
			if (mHideObjList.Count == 0)
			{
				ReShowBaseUI();
			}
		}
		else if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(DelayCheckUnlockFunction(pathData));
		}
		switch (pathData.uiType)
		{
		case UIPathData.UIType.TYPE_BASE:
			CloseBaseUI(pathData);
			break;
		case UIPathData.UIType.TYPE_POP:
			ClosePopUI(pathData);
			break;
		case UIPathData.UIType.TYPE_MENU_POP:
			CloseMenuPopUI(pathData);
			break;
		case UIPathData.UIType.TYPE_MESSAGE:
			CloseMessageUI(pathData);
			break;
		case UIPathData.UIType.TYPE_MENU_TOP:
			CloseMenuTopUI(pathData);
			break;
		case UIPathData.UIType.TYPE_MEMU_TOP_2:
			CloseMenuTop2UI(pathData);
			break;
		case UIPathData.UIType.TYPE_MEMU_TOP_3:
			CloseMenuTop3UI(pathData);
			break;
		}
		if (pathData.backFlag)
		{
			RemoveShowList(pathData);
		}
	}

	private void RemoveShowList(UIPathData pathData)
	{
		if (CurShowUIList.Contains(pathData))
		{
			CurShowUIList.Remove(pathData);
		}
	}

	private void DestroyUI(UIPathData pathData, GameObject obj)
	{
		Object.Destroy(obj);
	}

	private void CloseBaseUI(UIPathData pathData)
	{
		if (mDicBaseUI.ContainsKey(pathData.name))
		{
			NGUITools.SetActive(mDicBaseUI[pathData.name], state: false);
		}
	}

	private void CloseMessageUI(UIPathData pathData)
	{
		TryDestroyUI(mDicMessageUI, pathData);
	}

	private void CloseMenuPopUI(UIPathData pathData)
	{
		TryDestroyUI(mDicMenuPopUI, pathData);
	}

	private void ClosePopUI(UIPathData pathData)
	{
		TryDestroyUI(mDicPopUI, pathData);
	}

	private void CloseMenuTopUI(UIPathData pathData)
	{
		TryDestroyUI(mDicMenuTopUI, pathData);
	}

	private void CloseMenuTop2UI(UIPathData pathData)
	{
		TryDestroyUI(mDicMenuTop2UI, pathData);
	}

	private void CloseMenuTop3UI(UIPathData pathData)
	{
		TryDestroyUI(mDicMenuTop3UI, pathData);
	}

	private void TryDestroyUI(Dictionary<string, GameObject> dict, UIPathData pathData)
	{
		if (dict == null)
		{
			return;
		}
		string key = pathData.name;
		if (dict.ContainsKey(key))
		{
			if (!pathData.isDestoryOnUnload)
			{
				NGUITools.SetActive(dict[key], state: false);
				mDicCacheUI.Add(key, dict[key]);
			}
			else
			{
				DestroyUI(pathData, dict[key]);
			}
			dict.Remove(pathData.name);
		}
	}

	public bool CheckUIExit(UIPathData pathData)
	{
		switch (pathData.uiType)
		{
		case UIPathData.UIType.TYPE_BASE:
			if (mDicBaseUI.ContainsKey(pathData.name))
			{
				return true;
			}
			break;
		case UIPathData.UIType.TYPE_POP:
			if (mDicPopUI.ContainsKey(pathData.name))
			{
				return true;
			}
			break;
		case UIPathData.UIType.TYPE_MENU_POP:
			if (mDicMenuPopUI.ContainsKey(pathData.name))
			{
				return true;
			}
			break;
		case UIPathData.UIType.TYPE_MESSAGE:
			if (mDicMessageUI.ContainsKey(pathData.name))
			{
				return true;
			}
			break;
		case UIPathData.UIType.TYPE_MENU_TOP:
			if (mDicMenuTopUI.ContainsKey(pathData.name))
			{
				return true;
			}
			break;
		case UIPathData.UIType.TYPE_MEMU_TOP_2:
			if (mDicMenuTop2UI.ContainsKey(pathData.name))
			{
				return true;
			}
			break;
		case UIPathData.UIType.TYPE_MEMU_TOP_3:
			if (mDicMenuTop3UI.ContainsKey(pathData.name))
			{
				return true;
			}
			break;
		}
		return false;
	}

	public void CloseOtherPlayerUI()
	{
		if (SingletonUnity<OtherPlayerInfoUILogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<OtherPlayerInfoUILogic>.Instance.gameObject))
		{
			CloseUI(UIInfo.OtherPlayerInfoUILogicRoot);
		}
		if (SingletonUnity<HitOtherPLayerLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<HitOtherPLayerLogic>.Instance.gameObject))
		{
			CloseUI(UIInfo.HitOtherPlayerRoot);
		}
	}

	public bool CloseAllPOPUI()
	{
		if (IsUnlockTutorialEnable())
		{
			Debug.Log("Tutorial Is On, Can't Close All Pop UI!!!!!!!!!!!!!!!!!");
			return false;
		}
		List<string> list = new List<string>(mDicPopUI.Keys);
		AddReShowUI(list);
		for (int i = 0; i < list.Count; i++)
		{
			UIPathData pathData = UIPathData.UINameDic[list[i]];
			CloseUI(pathData);
		}
		if (SingletonUnity<SocialUIRootLogic>.Exists)
		{
			SingletonUnity<SocialUIRootLogic>.Instance.CloseSocialUI();
		}
		list = new List<string>(mDicMenuPopUI.Keys);
		AddReShowUI(list);
		for (int j = 0; j < list.Count; j++)
		{
			UIPathData pathData2 = UIPathData.UINameDic[list[j]];
			CloseUI(pathData2);
		}
		list = new List<string>(mDicMenuTopUI.Keys);
		AddReShowUI(list);
		for (int k = 0; k < list.Count; k++)
		{
			UIPathData pathData3 = UIPathData.UINameDic[list[k]];
			CloseUI(pathData3);
		}
		list = new List<string>(mDicMenuTop2UI.Keys);
		AddReShowUI(list);
		for (int l = 0; l < list.Count; l++)
		{
			UIPathData pathData4 = UIPathData.UINameDic[list[l]];
			CloseUI(pathData4);
		}
		CloseUI(UIInfo.ItemInfoRoot);
		CloseUI(UIInfo.ItemInfoRootNew);
		CloseUI(UIInfo.NumRoot);
		CloseUI(UIInfo.OpenBoxRoot);
		CloseUI(UIInfo.ChatRoot);
		return true;
	}

	public void UICheckChangeScene()
	{
		CanReShowUIList.Clear();
		List<string> uinamelist = new List<string>(mDicPopUI.Keys);
		AddReShowUI(uinamelist);
		uinamelist = new List<string>(mDicMenuPopUI.Keys);
		AddReShowUI(uinamelist);
		uinamelist = new List<string>(mDicMenuTopUI.Keys);
		AddReShowUI(uinamelist);
		uinamelist = new List<string>(mDicMenuTop2UI.Keys);
		AddReShowUI(uinamelist);
	}

	public void AddReShowUI(List<string> uinamelist)
	{
		for (int i = 0; i < uinamelist.Count; i++)
		{
			UIPathData uIPathData = UIPathData.UINameDic[uinamelist[i]];
			if (uIPathData.ReShowFlag && !CanReShowUIList.Contains(uinamelist[i]))
			{
				CanReShowUIList.Add(uinamelist[i]);
			}
		}
	}

	public void HideBaseUI()
	{
		IsHideBaseUI = true;
		BaseUIRoot.transform.localPosition = new Vector3(0f, 2000f, 0f);
		if (SingletonUnity<JoyStickLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<JoyStickLogic>.Instance.gameObject))
		{
			SingletonUnity<JoyStickLogic>.Instance.MoveOutScreen();
		}
		if (SingletonUnity<ChatUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ChatUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ChatRoot);
		}
	}

	public void ReShowBaseUI()
	{
		IsHideBaseUI = false;
		BaseUIRoot.transform.localPosition = Vector3.zero;
		if ((!SingletonUnity<TutorialUIRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(DelayCheckUnlockFunction(null));
		}
		if (UIUpdateEvent.OnReshowBase != null)
		{
			UIUpdateEvent.OnReshowBase();
		}
	}

	public void ShowBaseUI()
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (!sceneManager.IsTutorialScene() && !sceneManager.IsCarScene() && !sceneManager.IsPvPScene() && sceneManager.CurrentMapInofData.MapType != MAPTYPE.ANIMA_EDITOR)
		{
			ShowDefaultUI();
		}
	}

	public void ShowTutorialDefaultUI()
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		ShowUI(UIInfo.LevelUpUIRoot, delegate
		{
			CloseUI(UIInfo.LevelUpUIRoot);
		});
		ShowUI(UIInfo.YiDongKongZhiUI);
		ShowUI(UIInfo.JueseJiNengQuUI, delegate
		{
			SingletonUnity<JueseJiNengQuLogic>.Instance.Reset(hideAllBtn: true);
		});
		ShowUI(UIInfo.ExpLineRoot);
		ShowUI(UIInfo.SelectTargetUI);
		ShowUI(UIInfo.NotifyRootUI);
		ShowUI(UIInfo.TouXiangKuangUI, delegate(bool isSuccess, object param)
		{
			if (isSuccess)
			{
				SingletonUnity<TouXiangKuangLogic>.Instance.Init();
			}
		});
	}

	public void ShowCarDefaultUI()
	{
		ShowUI(UIInfo.ExpLineRoot);
		ShowUI(UIInfo.NotifyRootUI);
		ShowUI(UIInfo.CarControllerRoot, delegate
		{
			SingletonUnity<CarControllerRootLogic>.Instance.Reset();
		});
		MapInfoData mapInfo = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData;
		if (!SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene())
		{
			ShowUI(UIInfo.CopyFunctionBtnRoot, delegate
			{
				SingletonUnity<CopyFunctionRootLogic>.Instance.Reset(mapInfo.MapType, mapInfo.Name);
			});
		}
		ShowUI(UIInfo.MiniMapRoot, delegate
		{
			SingletonUnity<MiniMap>.Instance.Reset(mapInfo.fMapLength, mapInfo.fMapHeight, (mapInfo.fMapLength + mapInfo.fMapHeight) / 6f, mapInfo.MiniMapName, mapInfo.Name);
		});
		ShowUI(UIInfo.BroadCastRoot);
	}

	public void ShowLowPhoneUI()
	{
		ShowUI(UIInfo.YiDongKongZhiUI);
		ShowUI(UIInfo.JueseJiNengQuUI);
	}

	public static void SetSpecialType(SHOW_TYPE type)
	{
		NextShowType = type;
	}

	public bool ShowSpecialUI()
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if ((sceneManager.IsBigWorld() || sceneManager.IsTutorialScene()) && NextShowType == SHOW_TYPE.RANK_PVP)
		{
			NextShowType = SHOW_TYPE.DEFAULT;
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.ResetToPVP();
			});
			return true;
		}
		return false;
	}

	public bool ShowSpecialRebirthUI()
	{
		if (NextShowType == SHOW_TYPE.DEFAULT)
		{
			return false;
		}
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.IsBigWorld() || sceneManager.IsTutorialScene())
		{
			if (NextShowType == SHOW_TYPE.BIGSALE)
			{
				NextShowType = SHOW_TYPE.DEFAULT;
				ShowUI(UIInfo.BigPackRoot, delegate
				{
					SingletonUnity<BigPackRootLogic>.Instance.EnableReset();
					WaitResponseUIRootLogic.OpenWaitBox(260, 10f, 0f);
					NetLogic.GetInstance().Send<Protocol.request_big_pack>();
				});
				return true;
			}
			if (NextShowType == SHOW_TYPE.SHOP)
			{
				NextShowType = SHOW_TYPE.DEFAULT;
				ShowUI(UIInfo.ShopRoot, delegate
				{
					SingletonUnity<ShopUIRootLogic>.Instance.OnClickEquipBtn(GameDefine.SHOP_TAB_TYPE.TICKET);
				});
				return true;
			}
			if (NextShowType == SHOW_TYPE.SLOT)
			{
				NextShowType = SHOW_TYPE.DEFAULT;
				ShowUI(UIInfo.SlotUIRoot, delegate
				{
					SingletonUnity<SlotUIRootLogic>.Instance.EnableReset();
					WaitResponseUIRootLogic.OpenWaitBox(242, 10f, 0f);
					NetLogic.GetInstance().Send<Protocol.request_slot_info>();
				});
				return true;
			}
			if (NextShowType == SHOW_TYPE.WELFARE)
			{
				NextShowType = SHOW_TYPE.DEFAULT;
				ShowUI(UIInfo.CommercialUIRoot, delegate
				{
					SingletonUnity<CommercialUIRootLogic>.Instance.Reset();
				});
				return true;
			}
			if (NextShowType == SHOW_TYPE.MYSTERY)
			{
				NextShowType = SHOW_TYPE.DEFAULT;
				ShowUI(UIInfo.MysteryShopRoot, delegate
				{
					SingletonUnity<MysteryShopRootLogic>.Instance.EnableReset();
					WaitResponseUIRootLogic.OpenWaitBox(274, 10f, 0f);
					NetLogic.GetInstance().Send<Protocol.request_special_big_pack>();
				});
				return true;
			}
			if (NextShowType == SHOW_TYPE.SKILL)
			{
				NextShowType = SHOW_TYPE.DEFAULT;
				ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
				{
					SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowSkillInfo();
				});
				return true;
			}
			if (NextShowType == SHOW_TYPE.ENHANCE_EQUIP)
			{
				NextShowType = SHOW_TYPE.DEFAULT;
				ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
				{
					SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance();
				});
				return true;
			}
			if (NextShowType == SHOW_TYPE.ENHANCE_STAR)
			{
				NextShowType = SHOW_TYPE.DEFAULT;
				ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
				{
					SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipRefine();
				});
				return true;
			}
			if (NextShowType == SHOW_TYPE.INHERT)
			{
				NextShowType = SHOW_TYPE.DEFAULT;
				ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
				{
					SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickEquipBackPackBtn();
				});
				return true;
			}
		}
		return false;
	}

	public void ShowDefaultUI()
	{
		SceneManager curSceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		ShowUI(UIInfo.YiDongKongZhiUI);
		ShowUI(UIInfo.JueseJiNengQuUI);
		if (!GameSettingData.IsLowPhone)
		{
			ShowUI(UIInfo.ExpLineRoot);
		}
		ShowUI(UIInfo.SelectTargetUI);
		if (curSceneManager.IsLowPhoneManager())
		{
			MapInfoData currentMapInofData = curSceneManager.CurrentMapInofData;
			ShowUI(UIInfo.TouXiangKuangUI, delegate(bool isSuccess, object param)
			{
				if (isSuccess)
				{
					SingletonUnity<TouXiangKuangLogic>.Instance.Init();
				}
			});
			return;
		}
		ShowUI(UIInfo.NotifyRootUI);
		ShowUI(UIInfo.SocialUIRootLogic);
		ShowUI(UIInfo.LevelUpUIRoot, delegate
		{
			CloseUI(UIInfo.LevelUpUIRoot);
		});
		if (curSceneManager.IsBigWorld() || curSceneManager.IsTutorialScene())
		{
			ShowUI(UIInfo.MissionTeamTipRootUI, delegate
			{
				SingletonUnity<MissionTeamTipLogic>.Instance.EnableReset();
			});
			ShowUI(UIInfo.FunctionBtnRootUI, delegate
			{
				SingletonUnity<FunctionBtnRootLogic>.Instance.Reset();
			});
			MapInfoData mapInfo2 = curSceneManager.CurrentMapInofData;
			ShowUI(UIInfo.MiniMapRoot, delegate
			{
				SingletonUnity<MiniMap>.Instance.Reset(mapInfo2.fMapLength, mapInfo2.fMapHeight, (mapInfo2.fMapLength + mapInfo2.fMapHeight) / 6f, mapInfo2.MiniMapName, mapInfo2.Name);
			});
			ShowUI(UIInfo.CountTimeRoot);
		}
		else
		{
			if (curSceneManager.IsCopyShowMap())
			{
				MapInfoData mapInfo = curSceneManager.CurrentMapInofData;
				ShowUI(UIInfo.MiniMapRoot, delegate
				{
					SingletonUnity<MiniMap>.Instance.Reset(mapInfo.fMapLength, mapInfo.fMapHeight, (mapInfo.fMapLength + mapInfo.fMapHeight) / 6f, mapInfo.MiniMapName, mapInfo.Name);
				});
				ShowUI(UIInfo.CopyFunctionBtnRoot, delegate
				{
					SingletonUnity<CopyFunctionRootLogic>.Instance.Reset(curSceneManager.CurrentMapInofData.MapType, curSceneManager.CurrentMapInofData.Name);
				});
			}
			else
			{
				ShowUI(UIInfo.CopyFunctionBtnRoot, delegate
				{
					SingletonUnity<CopyFunctionRootLogic>.Instance.Reset(curSceneManager.CurrentMapInofData.MapType, curSceneManager.CurrentMapInofData.Name);
				});
			}
			ShowUI(UIInfo.CountTimeRoot);
			if (!curSceneManager.IsRankPvPScene())
			{
				ShowUI(UIInfo.CopyDrugUseUIRoot, delegate
				{
					SingletonUnity<CopyDrugUseUIRoot>.Instance.Reset();
				});
			}
			ShowUI(UIInfo.ChatBaseRoot, delegate
			{
				SingletonUnity<ChatBaseRootLogic>.Instance.UpdateMessage();
			});
		}
		if (curSceneManager.IsCanUsePotion() && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
		{
			ShowUI(UIInfo.PotionObjRoot, delegate
			{
				SingletonUnity<PotionLogic>.Instance.Reset();
			});
		}
		ShowUI(UIInfo.TouXiangKuangUI, delegate(bool isSuccess, object param)
		{
			if (isSuccess)
			{
				SingletonUnity<TouXiangKuangLogic>.Instance.Init();
			}
		});
		ShowUI(UIInfo.BroadCastRoot);
	}

	private void CheckTipUI()
	{
	}

	public static bool IsUnlockTutorialEnable()
	{
		if ((SingletonUnity<UnlockFunctionRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<UnlockFunctionRootLogic>.Instance.gameObject)) || (SingletonUnity<TutorialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject)) || (SingletonUnity<TeamPreparationRootLogic>.Exists && UnityVersionUtil.IsactiveInHierarchy(SingletonUnity<TeamPreparationRootLogic>.Instance.gameObject)))
		{
			return true;
		}
		return false;
	}

	public static bool IsPopMessageCanShow()
	{
		if (SingletonUnity<SexGameUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SexGameUIRootLogic>.Instance.gameObject))
		{
			return false;
		}
		return true;
	}

	public void CheckFunctionTop(bool isshowbossline)
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager == null || !SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsBigWorld() || !SingletonUnity<FunctionBtnRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<FunctionBtnRootLogic>.Instance.gameObject))
		{
			return;
		}
		if (isshowbossline)
		{
			if (SingletonUnity<FunctionBtnRootLogic>.Instance.IsOpenLeftBtn)
			{
				SingletonUnity<FunctionBtnRootLogic>.Instance.OnClickLeftArrowBtn();
			}
		}
		else if (!SingletonUnity<FunctionBtnRootLogic>.Instance.IsOpenLeftBtn)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.OnClickLeftArrowBtn();
		}
	}

	public bool BackUIFun()
	{
		if (IsUnlockTutorialEnable())
		{
			return true;
		}
		if (CurShowUIList.Count <= 0)
		{
			return false;
		}
		UIPathData uIPathData = CurShowUIList[0];
		if (uIPathData.needSelfClose)
		{
			return true;
		}
		CurShowUIList.RemoveAt(0);
		if (uIPathData.name.Equals("ExitGameRoot"))
		{
			return false;
		}
		if (uIPathData.name.Equals("ChooseRoleRoot"))
		{
			if (SingletonUnity<ChooseRoleRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ChooseRoleRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ChooseRoleRootLogic>.Instance.OnClickBackBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("MenuBaseRoot"))
		{
			if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("OptionUIRootLogic"))
		{
			if (SingletonUnity<OptionUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<OptionUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<OptionUIRootLogic>.Instance.OnClickClose();
				return true;
			}
		}
		else if (uIPathData.name.Equals("OtherPlayerInfoUILogicRoot"))
		{
			if (SingletonUnity<OtherPlayerInfoUILogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<OtherPlayerInfoUILogic>.Instance.gameObject))
			{
				SingletonUnity<OtherPlayerInfoUILogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("CreateTeamRoot"))
		{
			if (SingletonUnity<CreateTeamRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CreateTeamRootLogic>.Instance.gameObject))
			{
				SingletonUnity<CreateTeamRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("SearchTeamRoot"))
		{
			if (SingletonUnity<SearchTeamRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SearchTeamRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SearchTeamRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("TeamApplyListRoot"))
		{
			if (SingletonUnity<TeamApplyListLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamApplyListLogic>.Instance.gameObject))
			{
				SingletonUnity<TeamApplyListLogic>.Instance.OnClickClose();
				return true;
			}
		}
		else if (uIPathData.name.Equals("TeamInviteRoot"))
		{
			if (SingletonUnity<TeamInviteRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamInviteRootLogic>.Instance.gameObject))
			{
				SingletonUnity<TeamInviteRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("ItemInfoRoot"))
		{
			if (SingletonUnity<ItemInfoRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ItemInfoRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ItemInfoRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("ItemInfoRootNew"))
		{
			if (SingletonUnity<ItemInfoRootLogicNew>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ItemInfoRootLogicNew>.Instance.gameObject))
			{
				SingletonUnity<ItemInfoRootLogicNew>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("MissionPageRoot"))
		{
			if (SingletonUnity<MissionPageRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MissionPageRootLogic>.Instance.gameObject))
			{
				SingletonUnity<MissionPageRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("CopyFunctionBtnRoot"))
		{
			if (SingletonUnity<CopyFunctionRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CopyFunctionRootLogic>.Instance.gameObject))
			{
				SingletonUnity<CopyFunctionRootLogic>.Instance.OnClickLeaveCopyBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("HitOtherPlayerUIRoot"))
		{
			if (SingletonUnity<HitOtherPLayerLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<HitOtherPLayerLogic>.Instance.gameObject))
			{
				SingletonUnity<HitOtherPLayerLogic>.Instance.Close();
				return true;
			}
		}
		else if (uIPathData.name.Equals("ExcInfoRoot"))
		{
			if (SingletonUnity<ExcInfoRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ExcInfoRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ExcInfoRootLogic>.Instance.Close();
				return true;
			}
		}
		else if (uIPathData.name.Equals("ReportRoot"))
		{
			if (SingletonUnity<ReportRootUILogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ReportRootUILogic>.Instance.gameObject))
			{
				SingletonUnity<ReportRootUILogic>.Instance.OnClickClose();
				return true;
			}
		}
		else if (uIPathData.name.Equals("ChooseServerRoot"))
		{
			if (SingletonUnity<ChooseServerRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ChooseServerRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ChooseServerRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("LoginNoticeRoot"))
		{
			if (SingletonUnity<LoginNoticeRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<LoginNoticeRootLogic>.Instance.gameObject))
			{
				SingletonUnity<LoginNoticeRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("MapUIRoot"))
		{
			if (SingletonUnity<MapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MapUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<MapUIRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("PVPLogUIRoot"))
		{
			if (SingletonUnity<PVPLogUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PVPLogUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PVPLogUIRootLogic>.Instance.OnlClickClose();
				return true;
			}
		}
		else if (uIPathData.name.Equals("GuildLogUIRootLogic"))
		{
			if (SingletonUnity<GuildLogUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildLogUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildLogUIRootLogic>.Instance.OnlClickClose();
				return true;
			}
		}
		else if (uIPathData.name.Equals("GuildApplyListLogicRoot"))
		{
			if (SingletonUnity<GuildApplyListLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildApplyListLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildApplyListLogic>.Instance.OnClickClose();
				return true;
			}
		}
		else if (uIPathData.name.Equals("PopShopRoot"))
		{
			if (SingletonUnity<PopShopRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PopShopRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PopShopRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("PopDiamondBuyRoot"))
		{
			if (SingletonUnity<PopDiamondBuyRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PopDiamondBuyRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PopDiamondBuyRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("PopTopShopRoot"))
		{
			if (SingletonUnity<PopTopShopRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PopTopShopRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PopTopShopRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("PopTopDiamondBuyRoot"))
		{
			if (SingletonUnity<PopTopDiamondBuyRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PopTopDiamondBuyRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PopTopDiamondBuyRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("NumRoot"))
		{
			if (SingletonUnity<NumRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NumRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NumRootLogic>.Instance.OnClickClose();
				return true;
			}
		}
		else if (uIPathData.name.Equals("FriendAddUILogic"))
		{
			if (SingletonUnity<FriendAddUILogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FriendAddUILogic>.Instance.gameObject))
			{
				SingletonUnity<FriendAddUILogic>.Instance.OnClickClose();
				return true;
			}
		}
		else if (uIPathData.name.Equals("MapLineInfoLogic"))
		{
			if (SingletonUnity<MapLineInfoLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MapLineInfoLogic>.Instance.gameObject))
			{
				SingletonUnity<MapLineInfoLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("SlotLogRoot"))
		{
			if (SingletonUnity<SlotLogRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SlotLogRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SlotLogRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("SlotRuleRoot"))
		{
			if (SingletonUnity<SlotRuleRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SlotRuleRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SlotRuleRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("FirstBuyRoot"))
		{
			if (SingletonUnity<FirstBuyRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FirstBuyRootLogic>.Instance.gameObject))
			{
				SingletonUnity<FirstBuyRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("BigPackRoot"))
		{
			if (SingletonUnity<BigPackRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<BigPackRootLogic>.Instance.gameObject))
			{
				SingletonUnity<BigPackRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("RateRoot"))
		{
			if (SingletonUnity<RateRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RateRootLogic>.Instance.gameObject))
			{
				SingletonUnity<RateRootLogic>.Instance.CloseRate();
				return true;
			}
		}
		else if (uIPathData.name.Equals("TimerActivityTipsRoot"))
		{
			if (SingletonUnity<TimerActivityTipsRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TimerActivityTipsRootLogic>.Instance.gameObject))
			{
				SingletonUnity<TimerActivityTipsRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("RankPvpShowRewardRoot"))
		{
			if (SingletonUnity<RankPVPRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RankPVPRewardRootLogic>.Instance.gameObject))
			{
				SingletonUnity<RankPVPRewardRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("GuildBattleRankRoot"))
		{
			if (SingletonUnity<GuildBattleRankRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildBattleRankRoot>.Instance.gameObject))
			{
				SingletonUnity<GuildBattleRankRoot>.Instance.OnClickCloseBtn();
			}
		}
		else if (uIPathData.name.Equals("LevelRewardRoot"))
		{
			if (SingletonUnity<LevelRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<LevelRewardRootLogic>.Instance.gameObject))
			{
				SingletonUnity<LevelRewardRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("GuildBattleResultRoot"))
		{
			if (SingletonUnity<GuildBattleResultRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildBattleResultRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildBattleResultRootLogic>.Instance.OnClickExitBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("WorldMapRoot"))
		{
			if (SingletonUnity<WorldMapRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<WorldMapRoot>.Instance.gameObject))
			{
				SingletonUnity<WorldMapRoot>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("EquipInhertRoot"))
		{
			if (SingletonUnity<EquipInhertRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<EquipInhertRootLogic>.Instance.gameObject))
			{
				SingletonUnity<EquipInhertRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("NewMessageUIRoot"))
		{
			if (SingletonUnity<NewMessageUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMessageUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewMessageUIRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("TeamBroadCastRoot"))
		{
			if (SingletonUnity<TeamBroadCastRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamBroadCastRootLogic>.Instance.gameObject))
			{
				SingletonUnity<TeamBroadCastRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("ChangeTimeRoot"))
		{
			if (SingletonUnity<ChangeTimeRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ChangeTimeRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ChangeTimeRootLogic>.Instance.OnClickCancelBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("EnemyRevengeRoot"))
		{
			if (SingletonUnity<EnemyRevengeRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<EnemyRevengeRootLogic>.Instance.gameObject))
			{
				SingletonUnity<EnemyRevengeRootLogic>.Instance.OnClickCloseBtn();
				return true;
			}
		}
		else if (uIPathData.name.Equals("CityDamageRoot") && SingletonUnity<CityDamageRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CityDamageRootLogic>.Instance.gameObject))
		{
			SingletonUnity<CityDamageRootLogic>.Instance.OnClickCloseBtn();
			return true;
		}
		return true;
	}

	public void ClearReshowUI()
	{
		if (CanReShowUIList == null)
		{
			CanReShowUIList.Clear();
		}
	}

	public bool CheckReShowUI(UIPathData pathData)
	{
		if (IsUnlockTutorialEnable())
		{
			return false;
		}
		if (CanReShowUIList == null || CanReShowUIList.Count == 0)
		{
			return false;
		}
		if (pathData.name.Equals("StoryDialogUIRoot") || pathData.name.Equals("DialogMissionUIRoot") || pathData.name.Equals("DialogUIRoot") || pathData.name.Equals("OptionDialogUIRoot") || pathData.name.Equals("LoadingUIRoot"))
		{
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			string text = CanReShowUIList[CanReShowUIList.Count - 1];
			if (sceneManager.IsBigWorld())
			{
				if (text.Equals("NewMissionUIRootLogic"))
				{
					ShowUI(UIInfo.NewActivityUIRootLogic, delegate
					{
						SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickMissionBtn();
					});
				}
				else if (text.Equals("NewDailyCopyUIRootLogic"))
				{
					ShowUI(UIInfo.NewActivityUIRootLogic, delegate
					{
						SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn();
					});
				}
				else if (text.Equals("NewDailyActivityUIRoot"))
				{
					ShowUI(UIInfo.NewActivityUIRootLogic, delegate
					{
						SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn();
					});
				}
				else if (text.Equals("RankPVPRoot"))
				{
					ShowUI(UIInfo.NewActivityUIRootLogic, delegate
					{
						SingletonUnity<NewActivityUIRootLogic>.Instance.ResetToPVP();
					});
				}
				else if (text.Equals("SlotUIRoot"))
				{
					ShowUI(UIInfo.SlotUIRoot, delegate
					{
						SingletonUnity<SlotUIRootLogic>.Instance.EnableReset();
						WaitResponseUIRootLogic.OpenWaitBox(242, 10f, 0f);
						NetLogic.GetInstance().Send<Protocol.request_slot_info>();
					});
				}
				else if (text.Equals("DailyActiveRewardRoot"))
				{
					ShowUI(UIInfo.NewActivityUIRootLogic, delegate
					{
						SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyActiveBtn();
					});
				}
				else if (text.Equals("EnhanceUIRootLogic"))
				{
					ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
					{
						SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance();
					});
				}
				else if (text.Equals("RefineUIRootLogic"))
				{
					ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
					{
						SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipRefine();
					});
				}
				else if (text.Equals("BadgeMergeRoot"))
				{
					ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
					{
						SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowBadgeMerge();
					});
				}
				else if (text.Equals("SkillInfoRoot"))
				{
					ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
					{
						SingletonUnity<EquipStrengthenUIRootLogic>.Instance.OnClickSkillBtn();
					});
				}
				else if (text.Equals("ShopTabRootLogic"))
				{
					ShowUI(UIInfo.ShopRoot, delegate
					{
						SingletonUnity<ShopUIRootLogic>.Instance.OnClickToolsBtn();
					});
				}
				else if (text.Equals("BuyDiamondRoot"))
				{
					ShowUI(UIInfo.ShopRoot, delegate
					{
						SingletonUnity<ShopUIRootLogic>.Instance.OnClickBuyDiamondBtn();
					});
				}
				else if (text.Equals("PlayerCarRoot"))
				{
					ShowUI(UIInfo.PlayerCarRoot, delegate
					{
						SingletonUnity<PlayerCarRootLogic>.Instance.EnableReset();
						WaitResponseUIRootLogic.OpenWaitBox(235, 10f, 0f);
						NetLogic.GetInstance().Send<Protocol.request_mount_info>();
					});
				}
			}
		}
		CanReShowUIList.Clear();
		return true;
	}
}
