using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GameManager : SingletonDontDestoryUnity<GameManager>
{
	public delegate void Void_Bool_Delegate(bool value);

	private SceneManager mSceneManager;

	private PlayerData mPlayerData;

	private PlayerCommonData mPlayerCommonData;

	private MissionManager mMissionManager;

	private int mRunningMapId;

	private static bool mOnlineState = false;

	private bool mFirstEnterGame;

	private bool mIsShowMainMissionTip;

	private bool mLocalFirstEnter = true;

	private static bool mIsSceneReady = false;

	private AutoSearchPathManager mAutoSearchPath = new AutoSearchPathManager();

	private SendServerCheckManager mSendServerCheckManager = new SendServerCheckManager();

	private string serverAreaName = string.Empty;

	public bool IsDayFlag;

	public bool IsAutoDownload;

	public bool IsNeedCountDownload;

	public bool IsNeedCountDownloadTime;

	public bool IsNeedCountDownloadFinish;

	private long sendServerRefreshTime = -1L;

	private float senderServerRefreshStartCheckTime = -1f;

	public static AndroidJavaClass jc;

	public static AndroidJavaObject currentActivity;

	public static bool IsInitBiling = false;

	private static List<NotifyData> NotifyList = new List<NotifyData>();

	private static bool islostfocus = false;

	public Void_Bool_Delegate OnUnityAdsFinished;

	public Void_Bool_Delegate OnUnityAdsStateChange;

	private static int supportMinVersion = 42;

	private static int supportMinVersion47 = 47;

	private static int supportMinVersion56 = 56;

	private static int supportMinVersion77 = 77;

	private static int supportMinVersion137 = 137;

	private static int supportMinVersion145 = 145;

	private static int supportMinVersion157 = 157;

	private static int supportMinVersion167 = 167;

	private static int supportMinVersion177 = 177;

	private static int supportMinVersion184 = 184;

	private static int supportMinVersion200 = 200;

	public SceneManager SceneManager
	{
		get
		{
			return mSceneManager;
		}
		private set
		{
			mSceneManager = value;
		}
	}

	public PlayerData PlayerData
	{
		get
		{
			return mPlayerData;
		}
		set
		{
			mPlayerData = value;
		}
	}

	public PlayerCommonData PlayerCommonData
	{
		get
		{
			return mPlayerCommonData;
		}
		set
		{
			mPlayerCommonData = value;
		}
	}

	public MissionManager MissionManager
	{
		get
		{
			return mMissionManager;
		}
		set
		{
			mMissionManager = value;
		}
	}

	public int RunningMapId
	{
		get
		{
			return mRunningMapId;
		}
		set
		{
			mRunningMapId = value;
		}
	}

	public string RunningMapIdStr => mRunningMapId.ToString();

	public static bool OnLineState
	{
		get
		{
			return mOnlineState;
		}
		set
		{
			mOnlineState = value;
		}
	}

	public bool FirstEnterGame
	{
		get
		{
			return mFirstEnterGame;
		}
		set
		{
			mFirstEnterGame = value;
		}
	}

	public bool IsShowMainMissionTip
	{
		get
		{
			return mIsShowMainMissionTip;
		}
		set
		{
			mIsShowMainMissionTip = value;
		}
	}

	public bool LocalFirstEnter
	{
		get
		{
			return mLocalFirstEnter;
		}
		set
		{
			mLocalFirstEnter = value;
		}
	}

	public static bool IsSceneReady
	{
		get
		{
			return mIsSceneReady;
		}
		set
		{
			mIsSceneReady = value;
		}
	}

	public AutoSearchPathManager AutoSearchPath
	{
		get
		{
			return mAutoSearchPath;
		}
		set
		{
			mAutoSearchPath = value;
		}
	}

	public SendServerCheckManager SendServerCheckManager => mSendServerCheckManager;

	public string ServerAreaName
	{
		get
		{
			return serverAreaName;
		}
		set
		{
			serverAreaName = value;
		}
	}

	public bool IsUnityAdsReady
	{
		get
		{
			if (currentActivity != null)
			{
				return currentActivity.Call<bool>("internalIsVideoAdsReady", new object[0]);
			}
			return false;
		}
	}

	private void Init()
	{
		DataManager.LoadBaseLocalization();
		mPlayerData = new PlayerData();
		mPlayerCommonData = new PlayerCommonData();
		mMissionManager = new MissionManager();
		mPlayerData.ResetPlayerData();
		mPlayerCommonData.ClearData();
		IsAutoDownload = LocalDataSaveManager.GetAutoDownloadFlag();
		IsNeedCountDownload = LocalDataSaveManager.IsNeedCountDownload();
		IsNeedCountDownloadTime = IsNeedCountDownload;
		IsNeedCountDownloadFinish = LocalDataSaveManager.IsNeedCountDownloadFinish();
		if (!GameSettingData.IsLowPhone)
		{
			AnimationManager.initStreamAnimationData(this);
		}
	}

	public void ReImportData()
	{
		DataManager.ReImportData(this);
	}

	public void ReImportAnima()
	{
		AnimationManager.ReImportDownloadAnimationData(this);
	}

	public void EnterLoginScene()
	{
		mSceneManager = null;
	}

	public void LoadScenneManager()
	{
		MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(RunningMapIdStr);
		if (mapInfoDataByID != null)
		{
			switch (mapInfoDataByID.MapType)
			{
			case MAPTYPE.SINGLE_KILL_MONSTER_COPY:
				mSceneManager = new KillBossLocalSceneManager();
				break;
			case MAPTYPE.EQUIP_COPY:
				mSceneManager = new EquipCopySceneManager();
				break;
			case MAPTYPE.RANK_PVP:
				mSceneManager = new RankPVPLocalSceneManager();
				break;
			case MAPTYPE.REAL_PVP:
				mSceneManager = new RealPVPSceneManager();
				break;
			case MAPTYPE.CAR_CHASE_COPY:
				mSceneManager = new CarChaseSceneManager();
				break;
			case MAPTYPE.SNEAKING_COPY:
				mSceneManager = new SneakingSceneManager();
				break;
			case MAPTYPE.ANIMA_EDITOR:
				mSceneManager = new AnimationEditorManager();
				break;
			case MAPTYPE.CLAMBING_TOWER:
				mSceneManager = new TowerSceneManager();
				break;
			case MAPTYPE.TUTORIAL_CAR:
				mSceneManager = new NewTutorialSceneManager();
				break;
			case MAPTYPE.MUTIPLE_KILL_MONSTER_COPY:
				mSceneManager = new MultiKillBossSceneManager();
				break;
			case MAPTYPE.CASH_DAILY_COPY:
				mSceneManager = new CashSneakingSceneManager();
				break;
			case MAPTYPE.EXP_DAILY_COPY:
			case MAPTYPE.SINGLE_EXP_DAILY_COPY:
				mSceneManager = new EXPSceneManager();
				break;
			case MAPTYPE.BAR_FIGHT_COPY:
				mSceneManager = new BarFightSceneManager();
				break;
			case MAPTYPE.WILD_BOSS_COPY:
				mSceneManager = new WildBossSceneManager();
				break;
			case MAPTYPE.GUILD_BOSS_COPY:
				mSceneManager = new GuildBossSceneManager();
				break;
			case MAPTYPE.SINGLE_RUN_POINT_COPY:
				mSceneManager = new SingleRunPointSceneManager();
				break;
			case MAPTYPE.SURVIVE_BATTLE1:
				mSceneManager = new SurvivalBattle1SceneManager();
				break;
			case MAPTYPE.SURVIVE_BATTLE2:
				mSceneManager = new SurvivalBattle2SceneManager();
				break;
			case MAPTYPE.SCUFFLE_AREA_1:
			case MAPTYPE.SCUFFLE_AREA_2:
				mSceneManager = new ScuffleAreaSceneManager();
				break;
			case MAPTYPE.LOW_PHONE:
				mSceneManager = new LowPhoneSceneManager();
				break;
			case MAPTYPE.MULTI_TOWER_COPY:
				mSceneManager = new MultiTowerCopySceneManager();
				break;
			case MAPTYPE.GUILD_BATTLE:
				mSceneManager = new GuildBattleSceneManager();
				break;
			case MAPTYPE.DOMIN_MAP:
				mSceneManager = new DominSceneManager();
				break;
			case MAPTYPE.SHOP_COPY:
				mSceneManager = new ShopCopySceneManager();
				break;
			default:
				mSceneManager = new SceneManager();
				break;
			case MAPTYPE.TUTORIAL_SCENE:
				break;
			}
			mSceneManager.Init(RunningMapIdStr);
		}
	}

	public void LoadNextLoadingTexture()
	{
		if (GameSettingData.GetPhoneClass() == 0)
		{
			return;
		}
		List<LoadingUIData> loadingUIDataList = DataManager.GetLoadingUIDataList();
		for (int num = loadingUIDataList.Count - 1; num >= 0; num--)
		{
			if (loadingUIDataList[num].ShowFlag == 0 || mPlayerData.Level < loadingUIDataList[num].MinLevel || mPlayerData.Level >= loadingUIDataList[num].MaxLevel || (!string.IsNullOrEmpty(loadingUIDataList[num].StartTime) && !TimeTools.IsTimeRange(loadingUIDataList[num].StartTimeList, loadingUIDataList[num].EndTimeList)))
			{
				loadingUIDataList.RemoveAt(num);
			}
		}
		if (loadingUIDataList.Count <= 0)
		{
			return;
		}
		string loadindex = LocalDataSaveManager.GetLoadindex();
		int num2 = 0;
		for (int i = 0; i < loadingUIDataList.Count; i++)
		{
			if (loadingUIDataList[i].ID.Equals(loadindex))
			{
				num2 = i;
				break;
			}
		}
		BundleManager.StartLoadingUItexture(this, loadingUIDataList[num2].Name);
		num2 = (num2 + 1) % loadingUIDataList.Count;
		LocalDataSaveManager.SetLoadindex(loadingUIDataList[num2].ID);
	}

	private void OnDisable()
	{
		LowPhoneSaveData();
	}

	public void LowPhoneSaveData()
	{
		if (GameSettingData.IsLowPhone && mSceneManager != null && mSceneManager is LowPhoneSceneManager lowPhoneSceneManager)
		{
			lowPhoneSceneManager.SaveData();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (SingletonDontDestoryUnity<GameManager>.Exists && base.IsInit)
		{
			Init();
			InitAndroid();
		}
	}

	private void Update()
	{
		if (mSceneManager != null)
		{
			mSceneManager.Update();
		}
	}

	private void Start()
	{
		InvokeRepeating("CheckSendServerRefresh", 10f, 60f);
	}

	public bool CanSendToServer(int eventId, float Interval = 0.5f)
	{
		return mSendServerCheckManager.CanSendToServer(eventId, Interval);
	}

	public void RegisterCheckEvent(int eventId)
	{
		mSendServerCheckManager.RegisterCheckEvent(eventId);
	}

	private void CheckSendServerRefresh()
	{
		if (sendServerRefreshTime > 0 && Time.realtimeSinceStartup - senderServerRefreshStartCheckTime > (float)sendServerRefreshTime)
		{
			SendServerRefreshInfo();
		}
	}

	public void SetNextServerRefreshTime()
	{
		sendServerRefreshTime = PlayerCommonData.GetResetDiffTime() + UnityEngine.Random.Range(0, 240);
		senderServerRefreshStartCheckTime = Time.realtimeSinceStartup;
	}

	public void SendServerRefreshInfo()
	{
		if (SceneManager != null && !Application.loadedLevelName.Equals(GameDefine.LOGIN_SCENE_NAME))
		{
			NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
			NetLogic.GetInstance().Send<Protocol.request_activity_info>();
			NetLogic.GetInstance().Send<Protocol.request_rank_pvp_data>();
			NetLogic.GetInstance().Send<Protocol.request_tower_copy_info>();
			NetLogic.GetInstance().Send<Protocol.request_slot_info>();
			NetLogic.GetInstance().Send<Protocol.request_sign_30_day_info>();
			NetLogic.GetInstance().Send<Protocol.request_daily_buy>();
			NetLogic.GetInstance().Send<Protocol.request_sign_week_info>();
			NetLogic.GetInstance().Send<Protocol.request_daily_active>();
			NetLogic.GetInstance().Send<Protocol.request_daily_mission>();
			ClearServerRefresh();
		}
	}

	public void ClearServerRefresh()
	{
		sendServerRefreshTime = -1L;
		senderServerRefreshStartCheckTime = -1f;
	}

	private void InitAndroid()
	{
		Debug.Log("**************Platform Unity Init:" + Debug.isDebugBuild);
		if (Debug.isDebugBuild)
		{
			currentActivity = null;
			return;
		}
		if (currentActivity == null)
		{
			jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		}
		Debug.Log("**************Platform Unity InitX:" + currentActivity);
		cancleAllNotification();
		serverAreaName = GetServerArea();
	}

	public bool IsFacebookEnable()
	{
		if (GetSDKVersion() < 9)
		{
			return false;
		}
		return true;
	}

	public int GetSDKVersion()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<int>("internalGetSDKVersion", new object[0]);
		}
		return -1;
	}

	public void SignInFacebook()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalFacebookSignIn");
		}
	}

	public void SignOutFacebook()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalFacebookSignOut");
		}
	}

	public void SignInGoogle()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("googleSignIn");
		}
	}

	public void SignOutGoogle()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("googleSignOut");
		}
	}

	public void SignInFacebookSuccess(string data)
	{
		if (string.IsNullOrEmpty(data) || !data.Contains("@"))
		{
			return;
		}
		string[] array = data.Split('@');
		if (array.Length == 2)
		{
			string id = array[0];
			string token = array[1];
			if (SingletonUnity<AccountVersionCheckRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AccountVersionCheckRootLogic>.Instance.gameObject))
			{
				SingletonUnity<AccountVersionCheckRootLogic>.Instance.SignInCheck(id, token);
			}
		}
	}

	public void SignInGoogleSuccess(string data)
	{
		if (string.IsNullOrEmpty(data) || !data.Contains("@"))
		{
			return;
		}
		string[] array = data.Split('@');
		if (array.Length == 2)
		{
			string id = array[0];
			string token = array[1];
			if (SingletonUnity<AccountVersionCheckRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AccountVersionCheckRootLogic>.Instance.gameObject))
			{
				SingletonUnity<AccountVersionCheckRootLogic>.Instance.SignInCheck(id, token, 1);
			}
		}
	}

	public void SignOutGoogleSuccess(string data)
	{
		if (SingletonUnity<AccountVersionCheckRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AccountVersionCheckRootLogic>.Instance.gameObject))
		{
			SingletonUnity<AccountVersionCheckRootLogic>.Instance.SignInCheck(string.Empty, string.Empty, 1);
		}
	}

	public void SignOutFacebookSuccess(string data)
	{
		if (SingletonUnity<AccountVersionCheckRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AccountVersionCheckRootLogic>.Instance.gameObject))
		{
			SingletonUnity<AccountVersionCheckRootLogic>.Instance.SignInCheck(string.Empty, string.Empty);
		}
	}

	public void CreateBilling()
	{
		Debug.Log("CreateBilling");
		if (IsInitBiling)
		{
			return;
		}
		Debug.Log("Platform creteBilling");
		string text = string.Empty;
		Dictionary<string, PurchaseData> purchaseData = DataManager.GetPurchaseData();
		foreach (KeyValuePair<string, PurchaseData> item in purchaseData)
		{
			text = text + item.Value.ProductId + "#";
		}
		if (text.Length > 0)
		{
			text = text.Remove(text.Length - 1);
			string text2 = $"{PlayerData.MainPlayerServerId}";
			if (currentActivity != null)
			{
				currentActivity.Call("internalCreateBilling", text, text2);
			}
			IsInitBiling = true;
		}
	}

	public void PurchaseInfoToServerToVerify(string str)
	{
		if (!string.IsNullOrEmpty(str))
		{
			string[] array = str.Split('#');
			if (array.Length >= 4)
			{
				string productId = array[0];
				string packageName = array[1];
				string token = array[2];
				string payload = array[3];
				check_purchase.request request = new check_purchase.request();
				request.productId = productId;
				request.packageName = packageName;
				request.token = token;
				request.payload = payload;
				NetLogic.GetInstance().Send<Protocol.check_purchase>(request);
			}
		}
	}

	public void HideFullScreenSmallFromAndroid()
	{
		if (SingletonUnity<ExitGameRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ExitGameRoot>.Instance.gameObject))
		{
			SingletonUnity<ExitGameRoot>.Instance.CloseAd();
		}
	}

	public string GetServerArea()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<string>("GetCountryServerArea", new object[0]);
		}
		return null;
	}

	public void Billing(string sku)
	{
		string text = $"{sku}#{PlayerData.MainPlayerServerId}";
		if (currentActivity != null)
		{
			currentActivity.Call("internalBilling", text);
		}
	}

	public void UpdateComsumed(ret_commercail_reward.request request)
	{
		if (request.state == 0L || request.state == 2)
		{
			switch ((GameDefine.CommercailTYPE)request.type)
			{
			case GameDefine.CommercailTYPE.INVEST:
			case GameDefine.CommercailTYPE.BIGSALE:
			case GameDefine.CommercailTYPE.DAILYBUY:
			case GameDefine.CommercailTYPE.SHOP:
			case GameDefine.CommercailTYPE.VIP:
				if (request.HasProductId && mPlayerCommonData.CheckDollorBuy(request.productId))
				{
					ComsumedPurchase(request.productId);
					mPlayerCommonData.UpdateAddFree(request.productId);
					AccountVersionCheckRootLogic.CheckFaceBookBindTips(0);
				}
				break;
			case GameDefine.CommercailTYPE.FIRSTBUY:
			case GameDefine.CommercailTYPE.DAILYACTIVE:
				break;
			}
		}
		else if (request.state == 1)
		{
			NoticeLogic.AddNotifyData("#{101233}");
		}
		else if (request.state != 3)
		{
		}
	}

	public void ComsumedPurchase(string sku)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalComsumedPurchase", sku);
		}
	}

	public void QueryInventory()
	{
		string text = $"{PlayerData.MainPlayerServerId}";
		if (currentActivity != null)
		{
			currentActivity.Call("internalQueryInventory", text);
		}
	}

	public void ShowFeatureView()
	{
		Debug.Log("Platform ShowFeatureView >_<");
		if (currentActivity != null)
		{
			currentActivity.Call("internalShowFeatureView");
		}
	}

	public void HideFeatureView()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalHideFeatureView");
		}
	}

	public bool IsFullScreenSmallReady()
	{
		Debug.Log("Platform IsFullScreenSmallReady >_<");
		if (currentActivity != null)
		{
			return currentActivity.Call<bool>("internalIsFulScreenSmallReady", new object[0]);
		}
		return false;
	}

	public bool IsFullScreenSmallShowing()
	{
		Debug.Log("Platform IsFullScreenSmallShowing >_<");
		if (currentActivity != null)
		{
			return currentActivity.Call<bool>("internalIsFullScreenSmallShowing", new object[0]);
		}
		return false;
	}

	public void ShowFullScreenExitSmall()
	{
		Debug.Log("Platform ShowFullScreenExitSmall >_<");
		if (currentActivity != null)
		{
			currentActivity.Call("internalShowFullScreenExitSmall");
		}
	}

	public void ShowFullScreenSmall()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalShowFullScreenSmall");
		}
	}

	public void HideFullScreenSmall()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalHideFullScreenSmall");
		}
	}

	public void HideFakeLoading()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalHideFakeLoading");
		}
	}

	public void ShowMoreGames()
	{
		Debug.Log("Platform ShowMoreGames >_<");
		if (currentActivity != null)
		{
			currentActivity.Call("internalMoreGames");
		}
	}

	public int GetBatteryState()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<int>("internalGetBatteryState", new object[0]);
		}
		return 50;
	}

	public void Rating()
	{
		Debug.Log("Platform Rating >_<");
		if (currentActivity != null)
		{
			currentActivity.Call("internalRating");
		}
	}

	public void FlurryLogEvent(string eventName)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryLogEvent", eventName);
		}
	}

	public void FlurryLogEventMap(string title, string name, string value)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryLogEventMap", title, name, value);
		}
	}

	public void FlurryLogEventMap(string title, string name, string value, string value2)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryLogEventMap_2", title, name, value, value2);
		}
	}

	public static bool IsEnglishLanguage()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<bool>("IsEnglishLanguage", new object[0]);
		}
		return false;
	}

	public static void setNotification(int id, int day, long delay, string message)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalSetNotification", id, day, delay, StrDictionary.GetDictionaryString(GameDefine.GameName), message);
		}
	}

	public static void cancleNotification(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalCancelNotification", id);
		}
	}

	public static void cancleAllNotification()
	{
		if (currentActivity != null)
		{
			NotifyList = DataManager.GetNotifyDataList();
			for (int i = 0; i < (NotifyList.Count + 1) * 6; i++)
			{
				cancleNotification(i);
			}
		}
	}

	public static int GetNotifyTime(int id)
	{
		string timeOffset = LocalDataSaveManager.GetTimeOffset();
		long num = long.Parse(timeOffset);
		if (id == 0)
		{
			return GameDefine.NotifyTime1;
		}
		if (id > 0)
		{
			if (num == -1)
			{
				return -1;
			}
			TimeSpan localShowTime = TimeTools.GetLocalShowTime(NotifyList[id - 1].NotifyTime, num);
			return localShowTime.Hours * 60 + localShowTime.Minutes;
		}
		return -1;
	}

	public static void SetNotify()
	{
		if (currentActivity == null)
		{
			return;
		}
		NotifyList = DataManager.GetNotifyDataList();
		int num = NotifyList.Count + 1;
		int num2 = 6;
		int num3 = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
		string empty = string.Empty;
		long num4 = -1L;
		for (int i = 0; i < num * num2; i++)
		{
			num4 = GetNotifyTime(i % num);
			empty = string.Empty;
			if ((i == 0 && LocalDataSaveManager.GetRewardFlag() == 0) || num4 == -1)
			{
				continue;
			}
			if (i < num)
			{
				if (num3 < num4)
				{
					empty = GetNotifyMessage(i % num);
					if (!string.IsNullOrEmpty(empty))
					{
						setNotification(i, i / num, num4, empty);
					}
				}
			}
			else
			{
				empty = GetNotifyMessage(i % num);
				if (!string.IsNullOrEmpty(empty))
				{
					setNotification(i, i / num, num4, empty);
				}
			}
		}
	}

	public static string GetNotifyMessage(int time)
	{
		string timeOffset = LocalDataSaveManager.GetTimeOffset();
		long offsetTime = long.Parse(timeOffset);
		TimeSpan timeSpan = default(TimeSpan);
		if (time == 0)
		{
			int num = -1;
			if (SingletonDontDestoryUnity<GameManager>.Instance != null && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData != null)
			{
				num = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.GetNextWeekRewardDay();
			}
			switch (num)
			{
			case 2:
				if (IsSupportCurDataVersion())
				{
					return StrDictionary.GetDictionaryString("#{200406}");
				}
				return "Login now, you can get a powerful weapon for free!";
			case 3:
				if (IsSupportCurDataVersion())
				{
					return StrDictionary.GetDictionaryString("#{200407}");
				}
				return "Now login, you can get brand-new vehicles for free!";
			default:
				return StrDictionary.GetDictionaryString("#{200402}");
			}
		}
		if (time > 0)
		{
			timeSpan = TimeTools.GetLocalShowTime(NotifyList[time - 1].ShowTime, offsetTime);
			return StrDictionary.GetDictionaryString(NotifyList[time - 1].InfoStr, $"{timeSpan.Hours:d2}:{timeSpan.Minutes:d2}");
		}
		return null;
	}

	private void OnApplicationQuit()
	{
		SetNotify();
	}

	private void OnApplicationFocus(bool isfocus)
	{
		if (isfocus)
		{
			if (islostfocus)
			{
				cancleAllNotification();
			}
		}
		else
		{
			SetNotify();
			islostfocus = true;
		}
		LowPhoneSaveData();
	}

	public void VideoAdsReadyCallBack()
	{
		if (OnUnityAdsStateChange != null)
		{
			OnUnityAdsStateChange(value: true);
		}
	}

	public void VideoAdsSkipCallBack(string adsVideoType)
	{
		if (OnUnityAdsFinished != null)
		{
			OnUnityAdsFinished(value: false);
			FlurryLogEventMap("VideoAds", "Skip", adsVideoType);
		}
	}

	public void VideoAdsClosedCallBack(string adsVideoType)
	{
		if (OnUnityAdsFinished != null)
		{
			OnUnityAdsFinished(value: true);
			FlurryLogEventMap("VideoAds", "Finish", adsVideoType);
		}
	}

	public void InterstitialAdCallBack(string adstr)
	{
	}

	public void ShowUnityAds()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalShowVideoAds");
		}
	}

	public static bool IsSupportCurDataVersion()
	{
		if (PlayerData.LocalDataVersion > supportMinVersion)
		{
			return true;
		}
		return false;
	}

	public static bool IsSupportCurDataVersion47()
	{
		if (PlayerData.LocalDataVersion > supportMinVersion47)
		{
			return true;
		}
		return false;
	}

	public static bool IsSupportCurDataVersion56()
	{
		if (PlayerData.LocalDataVersion >= supportMinVersion56)
		{
			return true;
		}
		return false;
	}

	public static bool IsSupportCurDataVersion77()
	{
		if (PlayerData.LocalDataVersion >= supportMinVersion77)
		{
			return true;
		}
		return false;
	}

	public static bool IsSupportCurDataVersion137()
	{
		if (PlayerData.LocalDataVersion >= supportMinVersion137)
		{
			return true;
		}
		return false;
	}

	public static bool IsSupportCurDataVersion145()
	{
		if (GameSettingData.IsLocalTestServer)
		{
			return true;
		}
		if (PlayerData.LocalDataVersion >= supportMinVersion145)
		{
			return true;
		}
		return false;
	}

	public static bool IsSupportCurDataVersion157()
	{
		if (GameSettingData.IsLocalTestServer)
		{
			return true;
		}
		if (PlayerData.LocalDataVersion >= supportMinVersion157)
		{
			return true;
		}
		return false;
	}

	public static bool IsSupportCurDataVersion167()
	{
		if (GameSettingData.IsLocalTestServer)
		{
			return true;
		}
		if (PlayerData.LocalDataVersion >= supportMinVersion167)
		{
			return true;
		}
		return false;
	}

	public static bool IsSupportCurDataVersion177()
	{
		if (GameSettingData.IsLocalTestServer)
		{
			return true;
		}
		if (PlayerData.LocalDataVersion >= supportMinVersion177)
		{
			return true;
		}
		return false;
	}

	public static bool IsSupportCurDataVersion184()
	{
		if (GameSettingData.IsLocalTestServer)
		{
			return true;
		}
		if (PlayerData.LocalDataVersion >= supportMinVersion184)
		{
			return true;
		}
		return false;
	}

	public static bool IsSupportCurDataVersion200()
	{
		if (GameSettingData.IsLocalTestServer)
		{
			return true;
		}
		if (PlayerData.LocalDataVersion >= supportMinVersion200)
		{
			return true;
		}
		return false;
	}
}
