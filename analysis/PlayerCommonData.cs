using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class PlayerCommonData
{
	private Dictionary<string, function_info> mTutorialFunctionStateDic = new Dictionary<string, function_info>();

	private List<string> mNeedShowUnlockIdList = new List<string>();

	private List<string> mMenuTabBtnTipIdList = new List<string>();

	private Dictionary<string, bool> mFunctionUnlockState = new Dictionary<string, bool>();

	private long mServerTime;

	private long commondSeed;

	public static long sendIndex = 0L;

	public static uint randomSeed;

	private static List<int> randomArray = new List<int>();

	private int mServerLevel;

	private long mServerLevelSealTime;

	private int mResetTime = 10800;

	private int mRankPvpResetTime = 32400;

	private float mLocalTime;

	public static XorFloat PvpScale = new XorFloat();

	public static XorFloat PvpScaleAdd = new XorFloat(0.05f);

	private long mTimeOffset;

	private long mDailyMissionRefreshTime;

	private long mCreateTime;

	private long mCreateDay = -1L;

	private long mBig_PackFlag;

	private long mFirst_PackFlag;

	private long mPush = -1L;

	private bool mPushShowFlag;

	private long mChampionGuildId;

	public Dictionary<string, function_info> TutorialFunctionStateDic
	{
		get
		{
			return mTutorialFunctionStateDic;
		}
		set
		{
			mTutorialFunctionStateDic = value;
		}
	}

	public List<string> NeedShowUnlockIdList
	{
		get
		{
			return mNeedShowUnlockIdList;
		}
		set
		{
			mNeedShowUnlockIdList = value;
		}
	}

	public List<string> MenuTabBtnTipIdList
	{
		get
		{
			return mMenuTabBtnTipIdList;
		}
		set
		{
			mMenuTabBtnTipIdList = value;
		}
	}

	public Dictionary<string, bool> FunctionUnlockState => mFunctionUnlockState;

	public long ServerTime
	{
		get
		{
			return mServerTime;
		}
		set
		{
			mServerTime = value;
			mLocalTime = Time.realtimeSinceStartup;
		}
	}

	public long CommondSeed
	{
		get
		{
			return commondSeed;
		}
		set
		{
			commondSeed = value;
		}
	}

	public int ServerLevel
	{
		get
		{
			return mServerLevel;
		}
		set
		{
			mServerLevel = value;
		}
	}

	public long ServerLevelSealTime
	{
		get
		{
			return mServerLevelSealTime;
		}
		set
		{
			mServerLevelSealTime = value;
		}
	}

	public int ResetTime
	{
		get
		{
			return mResetTime;
		}
		set
		{
			mResetTime = value;
		}
	}

	public int RankPvpResetTime
	{
		get
		{
			return mRankPvpResetTime;
		}
		set
		{
			mRankPvpResetTime = value;
		}
	}

	public long TimeOffset
	{
		get
		{
			return mTimeOffset;
		}
		set
		{
			mTimeOffset = value;
		}
	}

	public long DailyMissionRefreshTime
	{
		get
		{
			return mDailyMissionRefreshTime;
		}
		set
		{
			mDailyMissionRefreshTime = value;
			ResetTime = (int)value * 3600;
		}
	}

	public long CreateTime
	{
		get
		{
			return mCreateTime;
		}
		set
		{
			mCreateTime = value;
		}
	}

	public long CreateDay
	{
		get
		{
			if (mCreateDay == -1)
			{
				mCreateDay = (ServerTime - mCreateTime) / 86400;
			}
			return mCreateDay;
		}
	}

	public bool Big_PackFlag
	{
		get
		{
			if (mBig_PackFlag == 1)
			{
				return true;
			}
			return false;
		}
		set
		{
			mBig_PackFlag = 0L;
		}
	}

	public bool First_PackFlag
	{
		get
		{
			if (mFirst_PackFlag == 1)
			{
				return true;
			}
			return false;
		}
		set
		{
			mFirst_PackFlag = 0L;
		}
	}

	public long Push
	{
		get
		{
			return mPush;
		}
		set
		{
			mPush = value;
		}
	}

	public bool PushShowFlag
	{
		get
		{
			return mPushShowFlag;
		}
		set
		{
			mPushShowFlag = value;
		}
	}

	public long ChampionGuildId
	{
		get
		{
			return mChampionGuildId;
		}
		set
		{
			mChampionGuildId = value;
		}
	}

	public PlayerCommonData()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(CheckLevelUpUnlockFunction));
	}

	private static uint MyRandom()
	{
		randomSeed = randomSeed * 1103515245 + 12345;
		return (randomSeed << 16) | ((randomSeed >> 16) & 0xFFFFu);
	}

	public static void InitRandom(long seed)
	{
		sendIndex = 0L;
		randomSeed = (uint)seed;
		randomArray.Clear();
		for (int i = 0; i < 1000; i++)
		{
			uint num = MyRandom();
			randomArray.Add((int)(num % 100));
		}
	}

	public static int GetRandom(GameDefine.OBJ_TYPE type)
	{
		if (type == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER && !GameSettingData.IsLowPhone)
		{
			int index = (int)(sendIndex % randomArray.Count);
			sendIndex++;
			return randomArray[index];
		}
		return UnityEngine.Random.Range(0, 100);
	}

	public long GetCurServerTime()
	{
		return mServerTime + (long)(Time.realtimeSinceStartup - mLocalTime);
	}

	public static long GetServerTime()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime();
	}

	public bool IsChampionGuild(long guildId)
	{
		return guildId == mChampionGuildId;
	}

	public void ClearData()
	{
		mFunctionUnlockState.Clear();
		mNeedShowUnlockIdList.Clear();
		mMenuTabBtnTipIdList.Clear();
		mTutorialFunctionStateDic.Clear();
		ServerTime = -1L;
		TimeOffset = -1L;
		DailyMissionRefreshTime = -1L;
		mPushShowFlag = false;
		sendIndex = 0L;
	}

	public void ResetFunctionUnlockData()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		List<FunctionData> functionDataList = DataManager.GetFunctionDataList();
		int num = 0;
		functionDataList.Sort((FunctionData x, FunctionData y) => (x.Condition != y.Condition) ? (x.Condition - y.Condition) : (x.Index - y.Index));
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		for (int i = 0; i < functionDataList.Count; i++)
		{
			if (!IsTutorialCanShow(functionDataList[i].ID))
			{
				SetFunctionUnlockState(functionDataList[i].ID, isUnlock: true);
			}
			else if (functionDataList[i].Class == 0)
			{
				SetFunctionUnlockState(functionDataList[i].ID, isUnlock: true);
			}
			else if (functionDataList[i].Class == 1)
			{
				if (functionDataList[i].Condition <= playerData.Level)
				{
					SetFunctionUnlockState(functionDataList[i].ID, isUnlock: true);
				}
				else
				{
					SetFunctionUnlockState(functionDataList[i].ID, isUnlock: false);
				}
			}
			else if (functionDataList[i].Class == 2)
			{
				if (functionDataList[i].Condition <= playerCommonData.CreateDay)
				{
					SetFunctionUnlockState(functionDataList[i].ID, isUnlock: true);
				}
				else
				{
					SetFunctionUnlockState(functionDataList[i].ID, isUnlock: false);
				}
			}
			else
			{
				SetFunctionUnlockState(functionDataList[i].ID, isUnlock: false);
			}
			if (functionDataList[i].FirstOpen == 1 && mFunctionUnlockState.ContainsKey(functionDataList[i].ID) && mFunctionUnlockState[functionDataList[i].ID] && (mTutorialFunctionStateDic == null || !mTutorialFunctionStateDic.ContainsKey(functionDataList[i].ID) || mTutorialFunctionStateDic[functionDataList[i].ID].state == 0L))
			{
				if (functionDataList[i].UnlockType == 2)
				{
					mMenuTabBtnTipIdList.Add(functionDataList[i].ID);
				}
				else
				{
					mNeedShowUnlockIdList.Add(functionDataList[i].ID);
				}
			}
		}
		List<string> list = new List<string>();
		for (int j = 0; j < functionDataList.Count; j++)
		{
			if (!mFunctionUnlockState[functionDataList[j].ID] && functionDataList[j].SideMissionIdList != null && functionDataList[j].SideMissionIdList.Length > 0)
			{
				for (int k = 0; k < functionDataList[j].SideMissionIdList.Length; k++)
				{
					list.Add(functionDataList[j].SideMissionIdList[k]);
				}
			}
		}
		missionManager.SetFunctionMissionIdList(list);
		if (playerData.IsHaveGuild())
		{
			SetFunctionUnlockState(3017.ToString(), isUnlock: true);
		}
	}

	public void CheckTitleLevelFunction()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.CurTitleLevel > 0)
		{
			SetFunctionUnlockState(3011.ToString(), isUnlock: true);
			SetFunctionUnlockState(4031.ToString(), isUnlock: true);
		}
	}

	public void CheckUnlockSideMission()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		List<FunctionData> functionDataList = DataManager.GetFunctionDataList();
		int num = 0;
		functionDataList.Sort((FunctionData x, FunctionData y) => (x.Condition != y.Condition) ? (x.Condition - y.Condition) : (x.Index - y.Index));
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		for (int i = 0; i < functionDataList.Count; i++)
		{
			if (functionDataList[i].Class != 1 || functionDataList[i].Condition > playerData.Level || functionDataList[i].SideMissionIdList == null)
			{
				continue;
			}
			for (int j = 0; j < functionDataList[i].SideMissionIdList.Length; j++)
			{
				if (missionManager.IsMissionAcceptable(functionDataList[i].SideMissionIdList[j]))
				{
					missionManager.AcceptMission(functionDataList[i].SideMissionIdList[j]);
				}
			}
		}
	}

	public bool IsFunctionUnlock(FUNCTION_TYPE type)
	{
		if (type == FUNCTION_TYPE.COUNT)
		{
			return true;
		}
		int num = (int)type;
		string text = num.ToString();
		FunctionData functionDataById = DataManager.GetFunctionDataById(text);
		if (CanCheckFunction(functionDataById))
		{
			if (!mFunctionUnlockState.ContainsKey(text))
			{
				return false;
			}
			return mFunctionUnlockState[text];
		}
		return false;
	}

	public bool CanCheckFunction(FunctionData curfunctiondata)
	{
		if (curfunctiondata == null)
		{
			return false;
		}
		if (curfunctiondata.IsDownload == 1)
		{
			return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload;
		}
		return true;
	}

	public bool IsTutorialCanShow(FUNCTION_TYPE type)
	{
		int num = (int)type;
		string funciontId = num.ToString();
		return IsTutorialCanShow(funciontId);
	}

	public bool IsTutorialCanShow(string funciontId)
	{
		FunctionData functionDataById = DataManager.GetFunctionDataById(funciontId);
		if (functionDataById == null)
		{
			return false;
		}
		if (mTutorialFunctionStateDic != null && mTutorialFunctionStateDic.ContainsKey(funciontId))
		{
			if (functionDataById.Class == 3)
			{
				if (mTutorialFunctionStateDic[funciontId].state < functionDataById.Condition)
				{
					return true;
				}
				return false;
			}
			if (mTutorialFunctionStateDic[funciontId].state == 0L)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public bool CheckFirstClickState(string key)
	{
		if (mTutorialFunctionStateDic == null)
		{
			mTutorialFunctionStateDic = new Dictionary<string, function_info>();
		}
		if (mTutorialFunctionStateDic.ContainsKey(key) && mTutorialFunctionStateDic[key].state == 1)
		{
			return false;
		}
		return true;
	}

	public void SetFirstClickState(string key)
	{
		if (mTutorialFunctionStateDic == null)
		{
			mTutorialFunctionStateDic = new Dictionary<string, function_info>();
		}
		if (!mTutorialFunctionStateDic.ContainsKey(key) || mTutorialFunctionStateDic[key].state != 1)
		{
			if (!mTutorialFunctionStateDic.ContainsKey(key))
			{
				function_info function_info = new function_info();
				function_info.ID = key;
				function_info.state = 1L;
				mTutorialFunctionStateDic.Add(key, function_info);
			}
			unlock_function_complete.request request = new unlock_function_complete.request();
			request.ID = key;
			request.state = 1L;
			NetLogic.GetInstance().Send<Protocol.unlock_function_complete>(request);
		}
	}

	public void SetTutorialShowFinish(FUNCTION_TYPE type)
	{
		int num = (int)type;
		string tutorialShowFinish = num.ToString();
		SetTutorialShowFinish(tutorialShowFinish);
	}

	public void SetTutorialShowFinish(string functionId)
	{
		if (!IsTutorialCanShow(functionId))
		{
			return;
		}
		FunctionData functionDataById = DataManager.GetFunctionDataById(functionId);
		if (functionDataById == null)
		{
			return;
		}
		int num = 1;
		if (mTutorialFunctionStateDic == null)
		{
			mTutorialFunctionStateDic = new Dictionary<string, function_info>();
		}
		if (functionDataById.Class == 3)
		{
			if (mTutorialFunctionStateDic.ContainsKey(functionId))
			{
				mTutorialFunctionStateDic[functionId].state++;
				num = (int)mTutorialFunctionStateDic[functionId].state;
			}
			else
			{
				function_info function_info = new function_info();
				function_info.ID = functionId;
				function_info.state = num;
				mTutorialFunctionStateDic.Add(functionId, function_info);
			}
		}
		else if (mTutorialFunctionStateDic.ContainsKey(functionId))
		{
			mTutorialFunctionStateDic[functionId].state = 1L;
		}
		else
		{
			function_info function_info2 = new function_info();
			function_info2.ID = functionId;
			function_info2.state = 1L;
			mTutorialFunctionStateDic.Add(functionId, function_info2);
		}
		unlock_function_complete.request request = new unlock_function_complete.request();
		request.ID = functionId;
		request.state = num;
		NetLogic.GetInstance().Send<Protocol.unlock_function_complete>(request);
	}

	public void SetTimeMissionFinish(string missionId)
	{
		Debug.Log("SetTimeMissionFinish :: " + missionId);
		int num = 1;
		if (mTutorialFunctionStateDic == null)
		{
			mTutorialFunctionStateDic = new Dictionary<string, function_info>();
		}
		if (mTutorialFunctionStateDic.ContainsKey(missionId))
		{
			mTutorialFunctionStateDic[missionId].state++;
			num = (int)mTutorialFunctionStateDic[missionId].state;
		}
		else
		{
			function_info function_info = new function_info();
			function_info.ID = missionId;
			function_info.state = num;
			mTutorialFunctionStateDic.Add(missionId, function_info);
		}
		unlock_function_complete.request request = new unlock_function_complete.request();
		request.ID = missionId;
		request.state = num;
		NetLogic.GetInstance().Send<Protocol.unlock_function_complete>(request);
	}

	public bool IsTimeMissionCanAccept(string missionId)
	{
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID == null || missionDataByID.Class != 8)
		{
			return false;
		}
		TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(missionDataByID.TimeLimitId);
		if (timeLimitMissionDataByID == null)
		{
			return false;
		}
		if (mTutorialFunctionStateDic != null && mTutorialFunctionStateDic.ContainsKey(missionId))
		{
			if (mTutorialFunctionStateDic[missionId].state < timeLimitMissionDataByID.LimitNum)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public void SyncCommonData(sync_common_data.request request)
	{
		if (ServerTime == -1)
		{
			ServerTime = request.serverTime;
		}
		TimeOffset = request.time_offset;
		LocalDataSaveManager.SetTimeOffset(TimeOffset);
		DailyMissionRefreshTime = request.daily_mission_refresh_time;
		mBig_PackFlag = request.big_pack;
		mFirst_PackFlag = request.first_buy;
		if (request.HasFunc_info)
		{
			mTutorialFunctionStateDic = request.func_info;
		}
		if (request.HasAdfree && request.adfree == 1)
		{
			LocalDataSaveManager.SetADFreeFlag(1);
		}
		ResetFunctionUnlockData();
		PvpScale = (float)request.pvp_scale / 10000f;
		if (request.HasPush)
		{
			Push = request.push;
			LocalDataSaveManager.SetRewardFlag((int)Push);
		}
		else
		{
			Push = -1L;
			LocalDataSaveManager.SetRewardFlag(0);
		}
		if (request.HasGuildId)
		{
			mChampionGuildId = request.guildId;
		}
		InitRandom(request.seed);
		if (request.HasServer_level)
		{
			mServerLevel = (int)request.server_level;
		}
		if (request.HasStart_time)
		{
			mServerLevelSealTime = request.start_time;
		}
	}

	public void UpdateAddFree(string key)
	{
		PurchaseData purchaseDataBuyId = DataManager.GetPurchaseDataBuyId(key);
		if (purchaseDataBuyId != null && purchaseDataBuyId.AdFree == 1)
		{
			LocalDataSaveManager.SetADFreeFlag(1);
		}
	}

	public bool CheckDollorBuy(string key)
	{
		PurchaseData purchaseDataBuyId = DataManager.GetPurchaseDataBuyId(key);
		return purchaseDataBuyId != null;
	}

	public void CheckPopTipsUI()
	{
		if (SingletonUnity<UIManager>.Instance.ShowSpecialUI() || SingletonUnity<UIManager>.Instance.ShowSpecialRebirthUI())
		{
			return;
		}
		if (SingletonUnity<UIManager>.Instance.CheckReShowUI(UIInfo.LoadingUIRoot))
		{
			mPushShowFlag = true;
			return;
		}
		if (mPushShowFlag)
		{
			CheckShowUnlockFunction();
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.AutoPopUIRoot, delegate
		{
			SingletonUnity<AutoPopUIRoot>.Instance.Reset();
		});
	}

	public void CheckShowUnlockFunction()
	{
		if (!mPushShowFlag && SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
		{
			SingletonUnity<AutoPopUIRoot>.Instance.NextPop();
		}
		else
		{
			if (Singleton<ObjManager>.Instance.MainPlayer == null || Singleton<ObjManager>.Instance.MainPlayer.IsLocalDrivingCar || !TutorialManager.IsTutorialCanShow())
			{
				return;
			}
			if (IsTutorialCanShow(FUNCTION_TYPE.TIPBTN_TUTORIAL_TIP) && SingletonUnity<FunctionBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionBtnRootLogic>.Instance.MessageObj))
			{
				TutorialManager.ShowTutorial(TUTORIAL_STEP.FUNCTION_TIP_START);
			}
			else if (mNeedShowUnlockIdList.Count > 0)
			{
				string mCurShowId = mNeedShowUnlockIdList[0];
				mNeedShowUnlockIdList.RemoveAt(0);
				SetFunctionUnlockState(mCurShowId, isUnlock: true);
				FunctionData functionDataById = DataManager.GetFunctionDataById(mCurShowId);
				FUNCTION_TYPE fUNCTION_TYPE = (FUNCTION_TYPE)int.Parse(functionDataById.ID);
				if (functionDataById.UnlockType == 1)
				{
					if (functionDataById.FirstOpen == 1)
					{
						if (SingletonUnity<JoyStickLogic>.Exists)
						{
							SingletonUnity<JoyStickLogic>.Instance.MoveOutScreen();
						}
						Singleton<ObjManager>.Instance.MainPlayer.DisactiveTargetArriveFinish();
						Singleton<ObjManager>.Instance.MainPlayer.StopMove();
						SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath.IsAutoMovingFlag = false;
					}
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.UnlockFunctionRoot, delegate
					{
						SingletonUnity<UnlockFunctionRootLogic>.Instance.ResetUnlockFunction(mCurShowId);
					});
				}
				else if (functionDataById.UnlockType == 0)
				{
					if (functionDataById.SideMissionIdList != null)
					{
						MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
						for (int i = 0; i < functionDataById.SideMissionIdList.Length; i++)
						{
							if (missionManager.IsMissionAcceptable(functionDataById.SideMissionIdList[i]))
							{
								missionManager.AcceptMission(functionDataById.SideMissionIdList[i]);
							}
						}
					}
					if (SingletonUnity<FunctionBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionBtnRootLogic>.Instance.gameObject))
					{
						SingletonUnity<FunctionBtnRootLogic>.Instance.refershBtn();
					}
					switch (fUNCTION_TYPE)
					{
					case FUNCTION_TYPE.MAIN_MISSION:
						TutorialManager.ShowTutorial(TUTORIAL_STEP.MAIN_MISSION_PHONE_START);
						if (SingletonUnity<MissionTeamTipLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MissionTeamTipLogic>.Instance.gameObject))
						{
							SingletonUnity<MissionTeamTipLogic>.Instance.MissionTipRoot.CloseHandTip();
						}
						break;
					case FUNCTION_TYPE.MAP_TIP:
						TutorialManager.ShowTutorial(TUTORIAL_STEP.NEW_MAP_TIP_START);
						CheckShowUnlockFunction();
						break;
					default:
						CheckShowUnlockFunction();
						break;
					}
				}
				else
				{
					if (functionDataById.UnlockType != 2)
					{
						return;
					}
					if (functionDataById.SideMissionIdList != null)
					{
						MissionManager missionManager2 = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
						for (int j = 0; j < functionDataById.SideMissionIdList.Length; j++)
						{
							if (missionManager2.IsMissionAcceptable(functionDataById.SideMissionIdList[j]))
							{
								missionManager2.AcceptMission(functionDataById.SideMissionIdList[j]);
							}
						}
					}
					if (fUNCTION_TYPE == FUNCTION_TYPE.ENHANCE_EQUIP)
					{
						if (IsTutorialCanShow(FUNCTION_TYPE.ENHANCE_EQUIP))
						{
							TutorialManager.ShowTutorial(TUTORIAL_STEP.ENHANCE_START);
						}
					}
					else
					{
						CheckShowUnlockFunction();
					}
				}
			}
			else
			{
				SingletonUnity<UIManager>.Instance.ShowSpecialRebirthUI();
			}
		}
	}

	private void AddFunctionTip(FUNCTION_TYPE functionType)
	{
		switch (functionType)
		{
		case FUNCTION_TYPE.ACTIVITY_CHALLENGE:
			if (SingletonUnity<FunctionBtnRootLogic>.Exists)
			{
				FunctionTipsRootLogic.AddFunctionTips(SingletonUnity<FunctionBtnRootLogic>.Instance.ActivityBtnIcon.gameObject, Vector3.zero, -1f);
			}
			break;
		case FUNCTION_TYPE.GIFT_DAILY:
		case FUNCTION_TYPE.GIFT_INVEST:
		case FUNCTION_TYPE.GIFT_7DAY:
			if (SingletonUnity<FunctionBtnRootLogic>.Exists)
			{
				FunctionTipsRootLogic.AddFunctionTips(SingletonUnity<FunctionBtnRootLogic>.Instance.GiftFuncBtn.gameObject, Vector3.zero, -1f);
			}
			break;
		case FUNCTION_TYPE.ENHANCE_STAR:
			if (SingletonUnity<FunctionBtnRootLogic>.Exists)
			{
				FunctionTipsRootLogic.AddFunctionTips(SingletonUnity<FunctionBtnRootLogic>.Instance.EnhanceFuncBtn.gameObject, Vector3.zero, -1f);
			}
			break;
		case FUNCTION_TYPE.CHARACTER_BADGE:
			if (SingletonUnity<FunctionBtnRootLogic>.Exists)
			{
				FunctionTipsRootLogic.AddFunctionTips(SingletonUnity<FunctionBtnRootLogic>.Instance.CharacterBtnIcon.gameObject, Vector3.zero, -1f);
			}
			break;
		}
	}

	public void CheckLevelUpUnlockFunction()
	{
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		List<FunctionData> functionDataList = DataManager.GetFunctionDataList();
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		functionDataList.Sort((FunctionData x, FunctionData y) => (x.Condition != y.Condition) ? (x.Condition - y.Condition) : (x.Index - y.Index));
		for (int i = 0; i < functionDataList.Count; i++)
		{
			if (functionDataList[i].Class != 1 || functionDataList[i].Condition > level || (mFunctionUnlockState.ContainsKey(functionDataList[i].ID) && mFunctionUnlockState[functionDataList[i].ID]))
			{
				continue;
			}
			FUNCTION_TYPE fUNCTION_TYPE = (FUNCTION_TYPE)int.Parse(functionDataList[i].ID);
			if (CanCheckFunction(functionDataList[i]))
			{
				if (functionDataList[i].UnlockType == 2)
				{
					if (functionDataList[i].FirstOpen == 1 && !mMenuTabBtnTipIdList.Contains(functionDataList[i].ID))
					{
						mMenuTabBtnTipIdList.Add(functionDataList[i].ID);
					}
					if (fUNCTION_TYPE == FUNCTION_TYPE.ENHANCE_EQUIP || fUNCTION_TYPE == FUNCTION_TYPE.ACTIVITY_CHALLENGE || fUNCTION_TYPE == FUNCTION_TYPE.TEAM)
					{
						mNeedShowUnlockIdList.Add(functionDataList[i].ID);
					}
				}
				else if (!mNeedShowUnlockIdList.Contains(functionDataList[i].ID))
				{
					mNeedShowUnlockIdList.Add(functionDataList[i].ID);
				}
				if (functionDataList[i].UnlockType == 2)
				{
					SetFunctionUnlockState(functionDataList[i].ID, isUnlock: true);
					if (fUNCTION_TYPE != FUNCTION_TYPE.ENHANCE_EQUIP && fUNCTION_TYPE != FUNCTION_TYPE.ACTIVITY_CHALLENGE && fUNCTION_TYPE != FUNCTION_TYPE.TEAM && functionDataList[i].SideMissionIdList != null)
					{
						for (int j = 0; j < functionDataList[i].SideMissionIdList.Length; j++)
						{
							if (missionManager.IsMissionAcceptable(functionDataList[i].SideMissionIdList[j]))
							{
								missionManager.AcceptMission(functionDataList[i].SideMissionIdList[j]);
							}
						}
					}
				}
			}
			if (functionDataList[i].UnlockType == 0 || functionDataList[i].UnlockType == 2 || functionDataList[i].UnlockType == 1)
			{
				SetFunctionUnlockState(functionDataList[i].ID, isUnlock: true);
			}
		}
		CheckShowUnlockFunction();
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateUnlockTips();
		}
		if (SingletonUnity<MissionTeamTipLogic>.Exists)
		{
			SingletonUnity<MissionTeamTipLogic>.Instance.UpdateUnlockTips();
		}
	}

	private void SetFunctionUnlockState(string id, bool isUnlock)
	{
		if (mFunctionUnlockState.ContainsKey(id))
		{
			mFunctionUnlockState[id] = isUnlock;
		}
		else
		{
			mFunctionUnlockState.Add(id, isUnlock);
		}
		if (isUnlock)
		{
			FunctionData functionDataById = DataManager.GetFunctionDataById(id);
			if (functionDataById.SideMissionIdList != null && functionDataById.SideMissionIdList.Length > 0)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.RemoveUnLockFunctionMission(functionDataById.SideMissionIdList);
			}
			if (functionDataById.FirstOpen != 1 && functionDataById.Class != 4)
			{
				SetTutorialShowFinish(id);
			}
		}
	}

	public long GetResetDiffTime()
	{
		DateTime dateTime = new DateTime(GetCurServerTime() * TimeTools.SECONDS_TO_TICKS);
		long num = 86400L;
		return (ResetTime + TimeOffset * 3600 + 120 - (long)dateTime.TimeOfDay.TotalSeconds + num) % num;
	}

	private bool IsCurMainMissionTriggerTutorial()
	{
		CurMission curMissionByClassType = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetCurMissionByClassType(MISSION_CLASS_TYPE.MAIN);
		if (curMissionByClassType == null)
		{
			return false;
		}
		MissionData missionDataByID = DataManager.GetMissionDataByID(curMissionByClassType.MissionId);
		if (missionDataByID.TrigerType != MISSION_TRIGGER_TUTORIAL_TYPE.INVALID && IsTutorialCanShow((FUNCTION_TYPE)missionDataByID.TriggerTutorialType))
		{
			if (missionDataByID.TrigerType == MISSION_TRIGGER_TUTORIAL_TYPE.ENHANCE_WEAPON_TUTORIAL_1 || missionDataByID.TrigerType == MISSION_TRIGGER_TUTORIAL_TYPE.ENHANCE_WEAPON_TUTORIAL_2 || missionDataByID.TrigerType == MISSION_TRIGGER_TUTORIAL_TYPE.ENHANCE_WEAPON_TUTORIAL_3)
			{
				if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsCanUpgradeWeapon())
				{
					SetTutorialShowFinish((FUNCTION_TYPE)missionDataByID.TriggerTutorialType);
					return false;
				}
				return true;
			}
			return true;
		}
		return false;
	}

	public bool CheckBuyBadgeTutorial()
	{
		if (GameManager.IsSupportCurDataVersion145())
		{
			MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			if (missionManager.IsMissionAccepted("40026") && IsTutorialCanShow(FUNCTION_TYPE.BUY_BADGE_TUTORIAL))
			{
				return true;
			}
		}
		return false;
	}

	public float ServerLevelSealRatio()
	{
		long num = ServerLevelSealTime - GetCurServerTime();
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		if (num > 0)
		{
			return 1f;
		}
		if (level > ServerLevel)
		{
			LevelSealData levelSealDataByID = DataManager.GetLevelSealDataByID((level - ServerLevel).ToString());
			if (levelSealDataByID != null)
			{
				return levelSealDataByID.InhibitRatio;
			}
		}
		else
		{
			if (level == ServerLevel)
			{
				return 1f;
			}
			LevelSealData levelSealDataByID2 = DataManager.GetLevelSealDataByID((ServerLevel - level).ToString());
			if (levelSealDataByID2 != null)
			{
				return levelSealDataByID2.EncourageRatio;
			}
		}
		return 1f;
	}

	~PlayerCommonData()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(CheckLevelUpUnlockFunction));
	}
}
