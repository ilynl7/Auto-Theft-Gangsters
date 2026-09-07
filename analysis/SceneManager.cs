using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SceneManager
{
	protected GameObject mUIRoot;

	protected GameObject mNameBoardRoot;

	protected GameObject mDamageBoadRoot;

	protected DamageBoardManager mDamageBoardManger;

	protected GameObject mMapActivityRoot;

	protected MapActivityManager mMapActivityManager;

	protected GameObjectPool mNameBoadPool;

	protected GameObjectPool mSimpleShadowPool;

	protected GameObject mDropItemRoot;

	protected GameObjectPool mDropItemPool;

	private bool mIsMissionStart;

	protected GameObjectPool mUIItemPool;

	protected MapInfoData mapInfoData;

	protected int mCurSceneReward;

	public bool isHaveSaftyArea;

	protected List<MapAreaInfoData> mSaftyAreaData;

	protected List<MapAreaInfoData> mGatherAreaData;

	private bool mLoadingFlag;

	protected List<MonsterData> mMonsterDataList;

	private List<ActivityMapData> mCurActivityMapDataList = new List<ActivityMapData>();

	private List<string> mCurAvailableMissionIdList;

	private Dictionary<string, List<string>> mCurMissionNpcDic;

	private List<GameObject> mExitPointList = new List<GameObject>();

	private List<MovePathPoint> mMissionPathPointList = new List<MovePathPoint>();

	private int messageCount;

	private float timeCount;

	private bool mIsHaveKillTargetMission;

	private List<CurMission> mKillTargetMissionList = new List<CurMission>();

	private List<KillTargetMissionData> mKillTargetMissionData = new List<KillTargetMissionData>();

	private float mFlashKillTargetNpcDis = 40f;

	private Dictionary<string, List<long>> mKillTargetNpcDic = new Dictionary<string, List<long>>();

	private bool mIsHaveTargetCarMission;

	private List<CurMission> mTargetCarMissionList = new List<CurMission>();

	private List<TargetCarMissionData> mTargetCarMissionDataList = new List<TargetCarMissionData>();

	private float mFlashTargetCarDis = 40f;

	private Dictionary<string, long> mTargetCarDic = new Dictionary<string, long>();

	private GameObject mLightFlare;

	private float ChangeLightMapTimeCount;

	private float ChangeLightMapTime = 10f;

	private Texture2D dayPic;

	private Texture2D nightPic;

	private LightProbes dayProbes;

	private LightProbes nightProbes;

	private Material daySkyBox;

	private Material nightSkyBox;

	private Skybox curSkyBox;

	private bool mDayState = true;

	public SimplePool<ParticleSystem> StrikePool;

	private Dictionary<string, GameObject> SceneComponentObjDic = new Dictionary<string, GameObject>();

	private List<string> SceneComponentObjList = new List<string>();

	public Dictionary<string, SceneComponentData> CurLoadActivityDic = new Dictionary<string, SceneComponentData>();

	public List<string> CurLoadActivityobjList = new List<string>();

	public List<SceneComponentData> NeedShowList = new List<SceneComponentData>();

	private GameObject mCityDanceSceneObj;

	private CityDanceData mCurCityDanceData;

	public bool IsHaveMoveTarget;

	public Vector3 CurMoveTarget = Vector3.zero;

	public GameObject UIRoot => mUIRoot;

	public GameObject NameBoardRoot
	{
		get
		{
			return mNameBoardRoot;
		}
		set
		{
			mNameBoardRoot = value;
		}
	}

	public GameObject DamageBoadRoot
	{
		get
		{
			return mDamageBoadRoot;
		}
		set
		{
			mDamageBoadRoot = value;
		}
	}

	public DamageBoardManager DamageBoardManger => mDamageBoardManger;

	public GameObject MapActivityRoot
	{
		get
		{
			return mMapActivityRoot;
		}
		set
		{
			mMapActivityRoot = value;
		}
	}

	public MapActivityManager MapActivityManager => mMapActivityManager;

	public GameObjectPool NameBoadPool
	{
		get
		{
			return mNameBoadPool;
		}
		set
		{
			mNameBoadPool = value;
		}
	}

	public GameObjectPool SimpleShadowPool
	{
		get
		{
			return mSimpleShadowPool;
		}
		set
		{
			mSimpleShadowPool = value;
		}
	}

	public GameObject DropItemRoot
	{
		get
		{
			return mDropItemRoot;
		}
		set
		{
			mDropItemRoot = value;
		}
	}

	public GameObjectPool DropItemPool
	{
		get
		{
			return mDropItemPool;
		}
		set
		{
			mDropItemPool = value;
		}
	}

	public bool IsMissionStart
	{
		get
		{
			return mIsMissionStart;
		}
		set
		{
			mIsMissionStart = value;
		}
	}

	public GameObjectPool UIItemPool
	{
		get
		{
			return mUIItemPool;
		}
		set
		{
			mUIItemPool = value;
		}
	}

	public MapInfoData CurrentMapInofData => mapInfoData;

	public int CurSceneReward
	{
		get
		{
			return mCurSceneReward;
		}
		set
		{
			mCurSceneReward = value;
		}
	}

	public List<MapAreaInfoData> SaftyAreaData => mSaftyAreaData;

	public List<MapAreaInfoData> GatherAreaData => mGatherAreaData;

	public bool LoadingFlag
	{
		get
		{
			return mLoadingFlag;
		}
		set
		{
			mLoadingFlag = value;
		}
	}

	public List<MonsterData> MonsterDataList => mMonsterDataList;

	public List<ActivityMapData> CurActivityMapDataList
	{
		get
		{
			if (mCurActivityMapDataList == null || mCurActivityMapDataList.Count == 0)
			{
				if (CurrentMapInofData == null)
				{
					return null;
				}
				mCurActivityMapDataList = DataManager.GetAcitvityMapDataByMapId(CurrentMapInofData.ID);
			}
			return mCurActivityMapDataList;
		}
	}

	public List<string> CurAvailableMissionIdList
	{
		get
		{
			if (mCurAvailableMissionIdList == null)
			{
				InitCurAvailableMissionList();
			}
			return mCurAvailableMissionIdList;
		}
	}

	public Dictionary<string, List<string>> CurMissionNpcDic
	{
		get
		{
			if (mCurMissionNpcDic == null)
			{
				InitCurAvailableMissionList();
			}
			return mCurMissionNpcDic;
		}
	}

	public List<GameObject> ExitPointList => mExitPointList;

	public CityDanceData CurCityDanceData => mCurCityDanceData;

	public int GetMonsterGroupCount()
	{
		for (int num = mMonsterDataList.Count - 1; num >= 0; num--)
		{
			if (mMonsterDataList[num].Group != GameDefine.MISSION_NPC_GROUP_VAL)
			{
				return mMonsterDataList[num].Group;
			}
		}
		return 0;
	}

	public List<int> GetMonsterGroupList()
	{
		List<int> list = new List<int>();
		if (mMonsterDataList == null)
		{
			return list;
		}
		for (int i = 0; i < mMonsterDataList.Count; i++)
		{
			if (!list.Contains(mMonsterDataList[i].Group) && mMonsterDataList[i].Group < 10000)
			{
				list.Add(mMonsterDataList[i].Group);
			}
		}
		return list;
	}

	public List<MonsterData> GetMonsterDataByGroup(int group)
	{
		List<MonsterData> list = new List<MonsterData>();
		for (int i = 0; i < mMonsterDataList.Count; i++)
		{
			if (mMonsterDataList[i].Group == group)
			{
				list.Add(mMonsterDataList[i]);
			}
		}
		return list;
	}

	public void InitCurAvailableMissionList()
	{
		if (mCurAvailableMissionIdList == null)
		{
			mCurAvailableMissionIdList = new List<string>();
			mCurMissionNpcDic = new Dictionary<string, List<string>>();
		}
		List<string> list = new List<string>(mCurMissionNpcDic.Keys);
		mCurAvailableMissionIdList.Clear();
		mCurMissionNpcDic.Clear();
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		List<MissionData> acceptMissionDataByMapId = DataManager.GetAcceptMissionDataByMapId(mapInfoData.ID);
		if (acceptMissionDataByMapId != null)
		{
			for (int i = 0; i < acceptMissionDataByMapId.Count; i++)
			{
				if (!missionManager.IsMissionAcceptable(acceptMissionDataByMapId[i].ID))
				{
					continue;
				}
				mCurAvailableMissionIdList.Add(acceptMissionDataByMapId[i].ID);
				if (!string.IsNullOrEmpty(acceptMissionDataByMapId[i].Accept))
				{
					if (mCurMissionNpcDic.ContainsKey(acceptMissionDataByMapId[i].Accept))
					{
						mCurMissionNpcDic[acceptMissionDataByMapId[i].Accept].Add(acceptMissionDataByMapId[i].ID);
						continue;
					}
					mCurMissionNpcDic.Add(acceptMissionDataByMapId[i].Accept, new List<string>());
					mCurMissionNpcDic[acceptMissionDataByMapId[i].Accept].Add(acceptMissionDataByMapId[i].ID);
				}
			}
		}
		List<string> allMissionId = missionManager.GetAllMissionId();
		MISSION_STATE mISSION_STATE = MISSION_STATE.INVALID;
		string empty = string.Empty;
		string empty2 = string.Empty;
		for (int j = 0; j < allMissionId.Count; j++)
		{
			mISSION_STATE = missionManager.GetMissionState(allMissionId[j]);
			MissionData missionDataByID = DataManager.GetMissionDataByID(allMissionId[j]);
			if (mISSION_STATE == MISSION_STATE.ACCEPTED)
			{
				if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.SURVEY)
				{
					SurveyMissionData surveyMissionDataById = DataManager.GetSurveyMissionDataById(missionDataByID.LogicID);
					empty2 = surveyMissionDataById.SceneID;
					empty = string.Empty;
				}
				else
				{
					empty2 = missionDataByID.TargetMapId;
					empty = missionDataByID.Target;
				}
			}
			else
			{
				if (mISSION_STATE != MISSION_STATE.COMPLETE)
				{
					continue;
				}
				empty2 = missionDataByID.SubmitMapId;
				empty = missionDataByID.Submit;
			}
			if (string.IsNullOrEmpty(empty2) || !empty2.Equals(mapInfoData.ID))
			{
				continue;
			}
			mCurAvailableMissionIdList.Add(missionDataByID.ID);
			if (!string.IsNullOrEmpty(empty))
			{
				if (mCurMissionNpcDic.ContainsKey(empty))
				{
					mCurMissionNpcDic[empty].Add(missionDataByID.ID);
					continue;
				}
				mCurMissionNpcDic.Add(empty, new List<string>());
				mCurMissionNpcDic[empty].Add(missionDataByID.ID);
			}
		}
		List<string> list2 = new List<string>(mCurMissionNpcDic.Keys);
		ObjNPC objNPC = null;
		ObjManager instance = Singleton<ObjManager>.Instance;
		for (int k = 0; k < list2.Count; k++)
		{
			objNPC = instance.FindMissionNpcInScene(list2[k]);
			if (objNPC != null)
			{
				objNPC.UpdateMissionNpcHead();
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			if (!mCurMissionNpcDic.ContainsKey(list[l]))
			{
				objNPC = instance.FindMissionNpcInScene(list[l]);
				if (objNPC != null)
				{
					objNPC.UpdateMissionNpcHead();
				}
			}
		}
		if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
		{
			SingletonUnity<MiniMap>.Instance.UpdateActionMapObj();
		}
		RefershMapActivity();
	}

	public void UpdateDamgeBoadScale()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData != null)
		{
			if (playerData.ViewType == CameraController.CAMERAVIEWSTATE.FREE)
			{
				DamageBoadRoot.transform.localScale = Vector3.one * 0.01f;
			}
			else
			{
				DamageBoadRoot.transform.localScale = Vector3.one * 0.02f;
			}
		}
	}

	public virtual void Init(string id)
	{
		mapInfoData = DataManager.GetMapInfoDataByID(id);
		IsHaveMoveTarget = false;
		mCurActivityMapDataList.Clear();
		if (CurrentMapInofData != null)
		{
			mCurActivityMapDataList = DataManager.GetAcitvityMapDataByMapId(CurrentMapInofData.ID);
		}
		mMapActivityRoot = GameObject.Find("MapActivityRoot");
		if (mMapActivityRoot == null)
		{
			mMapActivityRoot = ResourcesManager.LoadAndInstantiate("Items/MapActivityRoot") as GameObject;
			mMapActivityRoot.name = "MapActivityRoot";
		}
		if (mMapActivityManager == null)
		{
			mMapActivityManager = mMapActivityRoot.AddComponent<MapActivityManager>();
		}
		mMapActivityManager.Reset();
		mMonsterDataList = DataManager.GetMonsterDataListByMapId(id);
		NameBoardRoot = GameObject.Find("NameBoardRoot");
		if (NameBoardRoot == null)
		{
			NameBoardRoot = ResourcesManager.LoadAndInstantiate("UIRoot/NameBoardRoot") as GameObject;
		}
		DamageBoadRoot = GameObject.Find("DamageBoadRoot");
		if (DamageBoadRoot == null)
		{
			DamageBoadRoot = ResourcesManager.LoadAndInstantiate("UIRoot/DamageBoadRoot") as GameObject;
			DamageBoadRoot.transform.localScale = Vector3.one * 0.02f;
		}
		UpdateDamgeBoadScale();
		if (mDamageBoardManger == null)
		{
			mDamageBoardManger = DamageBoadRoot.AddComponent<DamageBoardManager>();
		}
		mNameBoadPool = new GameObjectPool("headInfo", 128);
		mSimpleShadowPool = new GameObjectPool("SimpleShadow");
		DropItemRoot = GameObject.Find("DropItemRoot");
		if (DropItemRoot == null)
		{
			DropItemRoot = ResourcesManager.LoadAndInstantiate("UIRoot/DropItemRoot") as GameObject;
		}
		mDropItemPool = new GameObjectPool("DropItem");
		if (mUIItemPool == null)
		{
			mUIItemPool = new GameObjectPool("UIItem");
		}
		Singleton<SurveyItemManager>.Instance.InitSurveyItem(id);
		if (CurrentMapInofData.ChangeLightMap == 1)
		{
			InitLightMap();
		}
		if (!string.IsNullOrEmpty(CurrentMapInofData.SaftyAreaId))
		{
			mSaftyAreaData = DataManager.GetMapAreaInfoDataListById(CurrentMapInofData.SaftyAreaId);
			isHaveSaftyArea = true;
		}
		else
		{
			isHaveSaftyArea = false;
		}
		if (!string.IsNullOrEmpty(CurrentMapInofData.GatherAreaId))
		{
			mGatherAreaData = DataManager.GetMapAreaInfoDataListById(CurrentMapInofData.GatherAreaId);
		}
		mCurCityDanceData = null;
		ClearActivityObjInfo();
		if (SingletonDontDestoryUnity<GameManager>.Instance.FirstEnterGame)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FirstEnterGame = false;
			if (!GameManager.IsInitBiling)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.CreateBilling();
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.QueryInventory();
			}
			NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
			NetLogic.GetInstance().Send<Protocol.request_activity_info>();
			NetLogic.GetInstance().Send<Protocol.request_dance_state_info>();
			NetLogic.GetInstance().Send<Protocol.request_guild_map_info>();
			if (GameManager.IsSupportCurDataVersion145())
			{
				NetLogic.GetInstance().Send<Protocol.req_guild_battle_state>();
				NetLogic.GetInstance().Send<Protocol.req_guild_battle_member>();
			}
			NetLogic.GetInstance().Send<Protocol.request_rank_pvp_data>();
			NetLogic.GetInstance().Send<Protocol.request_tower_copy_info>();
			NetLogic.GetInstance().Send<Protocol.request_wild_boss_info>();
			NetLogic.GetInstance().Send<Protocol.request_guild_boss>();
			NetLogic.GetInstance().Send<Protocol.request_slot_info>();
			NetLogic.GetInstance().Send<Protocol.request_mount_info>();
			NetLogic.GetInstance().Send<Protocol.request_sign_30_day_info>();
			NetLogic.GetInstance().Send<Protocol.request_invest_pack>();
			NetLogic.GetInstance().Send<Protocol.request_daily_active>();
			NetLogic.GetInstance().Send<Protocol.req_level_reward>();
			NetLogic.GetInstance().Send<Protocol.request_retrieve_info>();
			NetLogic.GetInstance().Send<Protocol.request_daily_buy>();
			NetLogic.GetInstance().Send<Protocol.request_sign_week_info>();
			NetLogic.GetInstance().Send<Protocol.require_vip_info>();
			NetLogic.GetInstance().Send<Protocol.request_domin_info>();
			request_dance_info.request request = new request_dance_info.request();
			request.type = 1L;
			NetLogic.GetInstance().Send<Protocol.request_dance_info>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckUnlockSideMission();
			SingletonDontDestoryUnity<GameManager>.Instance.SetNextServerRefreshTime();
		}
		else
		{
			CheckSceneActivity();
		}
		CheckSceneActivityObject();
		if (IsBigWorld() || IsTutorialScene())
		{
			GameObject gameObject = ResourcesManager.LoadAndInstantiate("Items/CityTeleport") as GameObject;
			gameObject.transform.position = new Vector3(CurrentMapInofData.TelePortPosVector3.x, GetHitHeight(CurrentMapInofData.TelePortPosVector3.x, CurrentMapInofData.TelePortPosVector3.z) + 0.1f, CurrentMapInofData.TelePortPosVector3.z);
			InitCurAvailableMissionList();
		}
		if (!string.IsNullOrEmpty(CurrentMapInofData.ExitPos))
		{
			List<Vector3> exitPosList = CurrentMapInofData.ExitPosList;
			GameObject gameObject2 = ResourcesManager.LoadAndInstantiate("Items/City_jinRu") as GameObject;
			ActivityPoint component = gameObject2.GetComponent<ActivityPoint>();
			mExitPointList.Add(gameObject2);
			component.transform.position = exitPosList[0];
			component.ResetExit(LeaveCopyScene);
			for (int i = 1; i < exitPosList.Count; i++)
			{
				GameObject gameObject3 = Object.Instantiate(gameObject2) as GameObject;
				ActivityPoint component2 = gameObject3.GetComponent<ActivityPoint>();
				gameObject3.transform.position = exitPosList[i];
				mExitPointList.Add(gameObject3);
				component2.ResetExit(LeaveCopyScene);
			}
		}
		mLoadingFlag = true;
		SingletonUnity<MyEvent>.Instance.Register("OnLoadingOver", this, "OnLoadingOver");
		GameManager.IsSceneReady = false;
		if (mapInfoData.SceneName.Equals("FB_pkTai_1"))
		{
			GameObject gameObject4 = new GameObject();
			gameObject4.transform.parent = null;
			gameObject4.transform.position = new Vector3(2.6f, 0f, 8.6f);
			gameObject4.transform.eulerAngles = new Vector3(0f, 196.5f, 0f);
			gameObject4.transform.localScale = Vector3.one;
			FakeObjLogic fakeObjLogic = new FakeObjLogic();
			fakeObjLogic.InitAnimaFakeNpcObj("NPC_Nv_004", gameObject4.transform, "tiaowu");
			GameObject gameObject5 = new GameObject();
			gameObject5.transform.parent = null;
			gameObject5.transform.position = new Vector3(1.96f, 0f, -9.45f);
			gameObject5.transform.eulerAngles = new Vector3(0f, 335.5f, 0f);
			gameObject5.transform.localScale = Vector3.one;
			FakeObjLogic fakeObjLogic2 = new FakeObjLogic();
			fakeObjLogic2.InitAnimaFakeNpcObj("NPC_Nv_004", gameObject5.transform, "tiaowu");
			GameObject gameObject6 = new GameObject();
			gameObject6.transform.parent = null;
			gameObject6.transform.position = new Vector3(-0.4f, 0f, 8.6f);
			gameObject6.transform.eulerAngles = new Vector3(0f, 171.9f, 0f);
			gameObject6.transform.localScale = Vector3.one;
			FakeObjLogic fakeObjLogic3 = new FakeObjLogic();
			fakeObjLogic3.InitAnimaFakeNpcObj("NPC_Nv_005", gameObject6.transform, "tiaowu");
			GameObject gameObject7 = new GameObject();
			gameObject7.transform.parent = null;
			gameObject7.transform.position = new Vector3(-3.91f, 0f, -8.05f);
			gameObject7.transform.eulerAngles = new Vector3(0f, 25.6f, 0f);
			gameObject7.transform.localScale = Vector3.one;
			FakeObjLogic fakeObjLogic4 = new FakeObjLogic();
			fakeObjLogic4.InitAnimaFakeNpcObj("NPC_Nv_005", gameObject7.transform, "tiaowu");
		}
		else if (mapInfoData.SceneName.Equals("FB_jingJiChang_1"))
		{
			GameObject gameObject8 = new GameObject();
			gameObject8.transform.parent = null;
			gameObject8.transform.position = new Vector3(9.01f, 0.58f, 7.93f);
			gameObject8.transform.eulerAngles = new Vector3(0f, 219.6f, 0f);
			gameObject8.transform.localScale = Vector3.one;
			FakeObjLogic fakeObjLogic5 = new FakeObjLogic();
			fakeObjLogic5.InitAnimaFakeNpcObj("NPC_Nv_004", gameObject8.transform, "tiaowu");
			GameObject gameObject9 = new GameObject();
			gameObject9.transform.parent = null;
			gameObject9.transform.position = new Vector3(-6.32f, 0.58f, 9.13f);
			gameObject9.transform.eulerAngles = new Vector3(0f, 143.5f, 0f);
			gameObject9.transform.localScale = Vector3.one;
			FakeObjLogic fakeObjLogic6 = new FakeObjLogic();
			fakeObjLogic6.InitAnimaFakeNpcObj("NPC_Nv_004", gameObject9.transform, "tiaowu");
			GameObject gameObject10 = new GameObject();
			gameObject10.transform.parent = null;
			gameObject10.transform.position = new Vector3(4.35f, 1.61f, 16.52f);
			gameObject10.transform.eulerAngles = new Vector3(0f, 184.7f, 0f);
			gameObject10.transform.localScale = Vector3.one;
			FakeObjLogic fakeObjLogic7 = new FakeObjLogic();
			fakeObjLogic7.InitAnimaFakeNpcObj("NPC_Nv_005", gameObject10.transform, "tiaowu");
			GameObject gameObject11 = new GameObject();
			gameObject11.transform.parent = null;
			gameObject11.transform.position = new Vector3(-7.82f, 0.58f, 8.26f);
			gameObject11.transform.eulerAngles = new Vector3(0f, 122.3f, 0f);
			gameObject11.transform.localScale = Vector3.one;
			FakeObjLogic fakeObjLogic8 = new FakeObjLogic();
			fakeObjLogic8.InitAnimaFakeNpcObj("NPC_Nv_005", gameObject11.transform, "tiaowu");
		}
		GameObject gameObject12 = GameObject.Find("anQuanQu");
		if (gameObject12 != null)
		{
			UnityVersionUtil.SetActiveRecursive(gameObject12, state: false);
		}
		if (isHaveSaftyArea)
		{
			GameObject gameObject13 = ResourcesManager.LoadAndInstantiate("Items/SafeAreaObj") as GameObject;
			for (int j = 0; j < mSaftyAreaData.Count; j++)
			{
				if (mSaftyAreaData[j].Type == 0)
				{
					GameObject gameObject14 = Object.Instantiate(gameObject13) as GameObject;
					float x = (mSaftyAreaData[j].PointList[0].x + mSaftyAreaData[j].PointList[2].x) / 2f;
					float z = (mSaftyAreaData[j].PointList[0].y + mSaftyAreaData[j].PointList[2].y) / 2f;
					Vector3 position = new Vector3(x, GetHitHeight(x, z) + 0.2f, z);
					Vector3 vector = new Vector3(mSaftyAreaData[j].PointList[3].x - mSaftyAreaData[j].PointList[0].x, 0f, mSaftyAreaData[j].PointList[3].y - mSaftyAreaData[j].PointList[0].y);
					Vector3 vector2 = new Vector3(mSaftyAreaData[j].PointList[1].x - mSaftyAreaData[j].PointList[0].x, 0f, mSaftyAreaData[j].PointList[1].y - mSaftyAreaData[j].PointList[0].y);
					float magnitude = vector.magnitude;
					float magnitude2 = vector2.magnitude;
					gameObject14.transform.position = position;
					gameObject14.transform.forward = vector.normalized;
					gameObject14.transform.eulerAngles = new Vector3(0f, gameObject14.transform.eulerAngles.y, 0f);
					gameObject14.transform.localScale = new Vector3(magnitude2, 10f, magnitude);
				}
			}
			Object.Destroy(gameObject13);
		}
		UpdateKillTargetMission(id);
		UpdateTargetCarMission(id);
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.AUTO_FIGHT))
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.AutoComabat = false;
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsOpenAutoCombat = false;
		}
	}

	public void UpdateArriveTargetPoint()
	{
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		if (!missionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.ARRIVE_TARGET))
		{
			for (int i = 0; i < mMissionPathPointList.Count; i++)
			{
				UnityVersionUtil.SetActiveRecursive(mMissionPathPointList[i].gameObject, state: false);
			}
			return;
		}
		List<CurMission> curMissionList = missionManager.GetCurMissionList();
		List<MissionData> list = new List<MissionData>();
		List<CurMission> list2 = new List<CurMission>();
		for (int j = 0; j < curMissionList.Count; j++)
		{
			if (curMissionList[j].MissionState != MISSION_STATE.ACCEPTED)
			{
				continue;
			}
			MissionData missionDataByID = DataManager.GetMissionDataByID(curMissionList[j].MissionId);
			if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.ARRIVE_TARGET)
			{
				MoveTargetMissionData moveTargetMissionDataById = DataManager.GetMoveTargetMissionDataById(missionDataByID.LogicID);
				if (moveTargetMissionDataById.MapId.Equals(CurrentMapInofData.ID))
				{
					list.Add(missionDataByID);
					list2.Add(curMissionList[j]);
				}
			}
		}
		int num = list2.Count - mMissionPathPointList.Count;
		if (num > 0)
		{
			for (int k = 0; k < num; k++)
			{
				GameObject gameObject = ResourcesManager.LoadAndInstantiate("Items/MovePathPoint") as GameObject;
				MovePathPoint component = gameObject.GetComponent<MovePathPoint>();
				component.RegisterOnArrivePathPoint(OnPlayerArriveMissionPoint);
				component.ActiveRadius = 8f;
				mMissionPathPointList.Add(component);
			}
		}
		for (int l = 0; l < mMissionPathPointList.Count; l++)
		{
			if (l < list2.Count)
			{
				mMissionPathPointList[l].transform.position = missionManager.GetMoveTargetMissionPos(list2[l].MissionId);
				UnityVersionUtil.SetActiveRecursive(mMissionPathPointList[l].gameObject, state: true);
				if (l == 0)
				{
					SetMoveTarget(mMissionPathPointList[l].transform.position, list2[l].MissionId);
				}
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(mMissionPathPointList[l].gameObject, state: false);
			}
		}
	}

	public void OnPlayerArriveMissionPoint(Vector3 pos)
	{
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		if (!missionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.ARRIVE_TARGET))
		{
			return;
		}
		List<CurMission> curMissionList = missionManager.GetCurMissionList();
		List<MissionData> list = new List<MissionData>();
		List<CurMission> list2 = new List<CurMission>();
		for (int i = 0; i < curMissionList.Count; i++)
		{
			if (curMissionList[i].MissionState == MISSION_STATE.ACCEPTED)
			{
				MissionData missionDataByID = DataManager.GetMissionDataByID(curMissionList[i].MissionId);
				if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.ARRIVE_TARGET)
				{
					list.Add(missionDataByID);
					list2.Add(curMissionList[i]);
				}
			}
		}
		int num = 0;
		float num2 = float.MaxValue;
		for (int j = 0; j < list2.Count; j++)
		{
			float num3 = Vector3.SqrMagnitude(missionManager.GetMoveTargetMissionPos(list2[j].MissionId) - pos);
			if (num3 < num2)
			{
				num2 = num3;
				num = j;
			}
		}
		if (list2.Count > num)
		{
			string missionId = list2[num].MissionId;
			local_npc_die.request request = new local_npc_die.request();
			request.npcid = missionId;
			request.type = 4L;
			NetLogic.GetInstance().Send<Protocol.local_npc_die>(request);
		}
	}

	public virtual void OnLoadingOver()
	{
		SingletonUnity<MyEvent>.Instance.DeRegister("OnLoadingOver", this, "OnLoadingOver");
		NetLogic.GetInstance().Send<Protocol.map_ready>();
		LoadingWindow.isSendMapReady = true;
		mLoadingFlag = false;
		if (IsCanShowCheckPopUI())
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckPopTipsUI();
		}
		else
		{
			SingletonUnity<UIManager>.Instance.ClearReshowUI();
		}
		UpdateArriveTargetPoint();
	}

	public virtual void Update()
	{
		timeCount += Time.deltaTime;
		if (timeCount > 1f)
		{
			timeCount = 0f;
			CheckKillTargetMission();
			CheckTargetCarMission();
		}
	}

	public void UpdateKillTargetMission(string sceneId)
	{
		mIsHaveKillTargetMission = false;
		mKillTargetMissionList.Clear();
		mKillTargetMissionData.Clear();
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		List<CurMission> curMissionList = missionManager.GetCurMissionList();
		for (int i = 0; i < curMissionList.Count; i++)
		{
			if (curMissionList[i].MissionState != MISSION_STATE.ACCEPTED)
			{
				continue;
			}
			MissionData missionDataByID = DataManager.GetMissionDataByID(curMissionList[i].MissionId);
			if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC)
			{
				KillTargetMissionData killTargetMissionDataById = DataManager.GetKillTargetMissionDataById(missionDataByID.LogicID);
				if (killTargetMissionDataById != null && killTargetMissionDataById.SceneID.Equals(sceneId))
				{
					mIsHaveKillTargetMission = true;
					mKillTargetMissionList.Add(curMissionList[i]);
					mKillTargetMissionData.Add(killTargetMissionDataById);
				}
			}
		}
		if (mKillTargetNpcDic.Count <= 0)
		{
			return;
		}
		List<string> list = new List<string>(mKillTargetNpcDic.Keys);
		ObjManager instance = Singleton<ObjManager>.Instance;
		for (int j = 0; j < list.Count; j++)
		{
			for (int k = 0; k < curMissionList.Count; k++)
			{
				if (list[j].Equals(curMissionList[k].MissionId))
				{
				}
			}
			List<long> list2 = mKillTargetNpcDic[list[j]];
			if (CurrentMapInofData.MapType == MAPTYPE.TUTORIAL_CAR)
			{
				for (int num = list2.Count - 1; num >= 0; num--)
				{
					ObjCharacter objCharacter = instance.FindObjInScene(list2[num]);
					if (objCharacter != null)
					{
						ObjRagdollNPC objRagdollNPC = objCharacter as ObjRagdollNPC;
						objRagdollNPC.RecycleSelf();
					}
				}
			}
			mKillTargetNpcDic.Remove(list[j]);
		}
	}

	private void CheckKillTargetMission()
	{
		if (!mIsHaveKillTargetMission)
		{
			return;
		}
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer == null)
		{
			return;
		}
		for (int i = 0; i < mKillTargetMissionData.Count; i++)
		{
			if (mKillTargetNpcDic.ContainsKey(mKillTargetMissionList[i].MissionId) || !(Vector3.Distance(mainPlayer.Position, mKillTargetMissionData[i].Pos) < mFlashKillTargetNpcDis) || CurrentMapInofData.MapType != MAPTYPE.TUTORIAL_CAR || !SingletonUnity<CitySimController>.Exists)
			{
				continue;
			}
			mKillTargetNpcDic.Add(mKillTargetMissionList[i].MissionId, new List<long>());
			for (int j = 0; j < mKillTargetMissionData[i].FlashNum; j++)
			{
				MonsterData monsterData = new MonsterData();
				monsterData.MapID = CurrentMapInofData.ID;
				monsterData.NpcID = mKillTargetMissionData[i].NpcID;
				monsterData.PosX = mKillTargetMissionData[i].PosX + Random.Range(-mKillTargetMissionData[i].Range, mKillTargetMissionData[i].Range);
				monsterData.PosZ = mKillTargetMissionData[i].PosZ + Random.Range(-mKillTargetMissionData[i].Range, mKillTargetMissionData[i].Range);
				monsterData.PosO = Random.Range(0, 36000);
				BlockMonsterData curData = new BlockMonsterData(monsterData);
				ObjRagdollNPC npc = SingletonUnity<CitySimController>.Instance.GetNpc(curData);
				if (npc != null)
				{
					mKillTargetNpcDic[mKillTargetMissionList[i].MissionId].Add(npc.ServerId);
				}
			}
			if (mKillTargetNpcDic[mKillTargetMissionList[i].MissionId].Count == 0)
			{
				mKillTargetNpcDic.Remove(mKillTargetMissionList[i].MissionId);
			}
		}
	}

	public void OnRecycleNpc(ObjCharacter objCha)
	{
		if (mKillTargetNpcDic.Count <= 0)
		{
			return;
		}
		List<string> list = new List<string>(mKillTargetNpcDic.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			if (mKillTargetNpcDic[list[i]].Contains(objCha.ServerId))
			{
				mKillTargetNpcDic[list[i]].Remove(objCha.ServerId);
				if (mKillTargetNpcDic[list[i]].Count <= 0)
				{
					mKillTargetNpcDic.Remove(list[i]);
				}
			}
		}
	}

	public void UpdateTargetCarMission(string sceneId)
	{
		mIsHaveTargetCarMission = false;
		mTargetCarMissionList.Clear();
		mTargetCarMissionDataList.Clear();
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		List<CurMission> curMissionList = missionManager.GetCurMissionList();
		for (int i = 0; i < curMissionList.Count; i++)
		{
			if (curMissionList[i].MissionState != MISSION_STATE.ACCEPTED)
			{
				continue;
			}
			MissionData missionDataByID = DataManager.GetMissionDataByID(curMissionList[i].MissionId);
			if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.TARGET_ROB_CAR)
			{
				TargetCarMissionData targetCarMissionDataById = DataManager.GetTargetCarMissionDataById(missionDataByID.LogicID);
				if (targetCarMissionDataById != null && targetCarMissionDataById.SceneID.Equals(sceneId))
				{
					mIsHaveTargetCarMission = true;
					mTargetCarMissionList.Add(curMissionList[i]);
					mTargetCarMissionDataList.Add(targetCarMissionDataById);
				}
			}
		}
		if (mTargetCarDic.Count <= 0)
		{
			return;
		}
		List<string> list = new List<string>(mTargetCarDic.Keys);
		ObjManager instance = Singleton<ObjManager>.Instance;
		for (int j = 0; j < list.Count; j++)
		{
			for (int k = 0; k < curMissionList.Count; k++)
			{
				if (list[j].Equals(curMissionList[k].MissionId))
				{
				}
			}
			long id = mTargetCarDic[list[j]];
			if (CurrentMapInofData.MapType == MAPTYPE.TUTORIAL_CAR)
			{
				Obj obj = instance.FindObj(id);
				if (obj != null)
				{
					ObjFakeAICar component = obj.GetComponent<ObjFakeAICar>();
					component.PlayerCar.AttributeData.Camp = GameDefine.CAMP_TYPE.NORMAL_NPC;
				}
			}
			mTargetCarDic.Remove(list[j]);
		}
	}

	private void CheckTargetCarMission()
	{
		if (!mIsHaveTargetCarMission)
		{
			return;
		}
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer == null)
		{
			return;
		}
		for (int i = 0; i < mTargetCarMissionDataList.Count; i++)
		{
			if (!mTargetCarDic.ContainsKey(mTargetCarMissionList[i].MissionId) && Vector3.Distance(mainPlayer.Position, mTargetCarMissionDataList[i].Pos) < mFlashTargetCarDis && CurrentMapInofData.MapType == MAPTYPE.TUTORIAL_CAR && SingletonUnity<CitySimController>.Exists)
			{
				mTargetCarDic.Add(mTargetCarMissionList[i].MissionId, 0L);
				BlockCarData blockCarData = new BlockCarData();
				blockCarData.CarId = mTargetCarMissionDataList[i].Mountid;
				blockCarData.CarAngle = mTargetCarMissionDataList[i].GetRot;
				blockCarData.CarPos = mTargetCarMissionDataList[i].Pos;
				mTargetCarDic[mTargetCarMissionList[i].MissionId] = SingletonUnity<CitySimController>.Instance.GetStaticCar(blockCarData, isFriend: true).ServerId;
			}
		}
	}

	public void OnRecycleTargetCar(ObjFakeAICar objCar)
	{
		if (mTargetCarDic.Count <= 0)
		{
			return;
		}
		List<string> list = new List<string>(mTargetCarDic.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			if (mTargetCarDic[list[i]].Equals(objCar.ServerId))
			{
				mTargetCarDic.Remove(list[i]);
			}
		}
	}

	public bool CheckCarIsMissionCar(ObjFakeAICar nearcar)
	{
		if (!mIsHaveTargetCarMission)
		{
			return false;
		}
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer == null)
		{
			return false;
		}
		if (nearcar == null)
		{
			return false;
		}
		if (isShowDoor(nearcar.transform.InverseTransformPoint(mainPlayer.transform.position)) && mTargetCarDic.ContainsValue(nearcar.ServerId) && CurrentMapInofData.MapType == MAPTYPE.TUTORIAL_CAR)
		{
			return true;
		}
		return false;
	}

	private bool isShowDoor(Vector3 inversePos)
	{
		if (inversePos.x < 3f && inversePos.x > -3f && inversePos.z > -5f && inversePos.z < 5f)
		{
			return true;
		}
		return false;
	}

	public void InitLightMap()
	{
		dayPic = LightmapSettings.lightmaps[0].lightmapFar;
		dayProbes = LightmapSettings.lightProbes;
		daySkyBox = RenderSettings.skybox;
		curSkyBox = Camera.mainCamera.GetComponent<Skybox>();
	}

	public void ChangeLightMap(bool isDay)
	{
		if (mDayState != isDay)
		{
			mDayState = isDay;
		}
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		string arg = "night";
		string text = "SkyBox_city_yeWan";
		LightmapData[] array = new LightmapData[1]
		{
			new LightmapData()
		};
		if (isDay)
		{
			array[0].lightmapFar = dayPic;
			RenderSettings.skybox = daySkyBox;
			if (curSkyBox != null)
			{
				curSkyBox.material = daySkyBox;
			}
			LightmapSettings.lightProbes = dayProbes;
			if (SingletonUnity<SceneObjectController>.Exists)
			{
				GameObject[] dayList = SingletonUnity<SceneObjectController>.Instance.DayList;
				GameObject[] nightList = SingletonUnity<SceneObjectController>.Instance.NightList;
				if (dayList != null)
				{
					for (int i = 0; i < dayList.Length; i++)
					{
						if (i == 1)
						{
							NGUITools.SetActive(dayList[i], state: false);
						}
						else
						{
							NGUITools.SetActive(dayList[i], state: true);
						}
					}
				}
				if (nightList != null)
				{
					for (int j = 0; j < nightList.Length; j++)
					{
						NGUITools.SetActive(nightList[j], state: false);
					}
				}
			}
			LightmapSettings.lightmaps = array;
			return;
		}
		if (nightPic == null)
		{
			nightPic = ResourcesManager.Load($"{CurrentMapInofData.SceneName}_LightmapFar-{arg}") as Texture2D;
			if (nightPic == null && UnityVersionUtil.IsactiveInHierarchy(SingletonDontDestoryUnity<GameManager>.Instance.gameObject))
			{
				SingletonDontDestoryUnity<GameManager>.Instance.StartCoroutine(BundleManager.LoadItem($"{CurrentMapInofData.SceneName}_LightmapFar-{arg}", LoadLightmapFinish));
			}
		}
		array[0].lightmapFar = nightPic;
		if (nightProbes == null)
		{
			nightProbes = ResourcesManager.Load($"{CurrentMapInofData.SceneName}_LightProbes-{arg}") as LightProbes;
			if (nightProbes == null && UnityVersionUtil.IsactiveInHierarchy(SingletonDontDestoryUnity<GameManager>.Instance.gameObject))
			{
				SingletonDontDestoryUnity<GameManager>.Instance.StartCoroutine(BundleManager.LoadItem($"{CurrentMapInofData.SceneName}_LightProbes-{arg}", LoadLightProbeFinish));
			}
		}
		LightmapSettings.lightProbes = nightProbes;
		if (nightSkyBox == null)
		{
			nightSkyBox = ResourcesManager.Load($"{CurrentMapInofData.SceneName}_{arg}") as Material;
			if (nightSkyBox == null && UnityVersionUtil.IsactiveInHierarchy(SingletonDontDestoryUnity<GameManager>.Instance.gameObject))
			{
				SingletonDontDestoryUnity<GameManager>.Instance.StartCoroutine(BundleManager.LoadItem($"{CurrentMapInofData.SceneName}_{arg}", LoadSkyBoxFinish));
			}
		}
		RenderSettings.skybox = nightSkyBox;
		if (curSkyBox != null)
		{
			curSkyBox.material = nightSkyBox;
		}
		if (SingletonUnity<SceneObjectController>.Exists)
		{
			GameObject[] dayList2 = SingletonUnity<SceneObjectController>.Instance.DayList;
			GameObject[] nightList2 = SingletonUnity<SceneObjectController>.Instance.NightList;
			if (dayList2 != null)
			{
				for (int k = 0; k < dayList2.Length; k++)
				{
					NGUITools.SetActive(dayList2[k], state: false);
				}
			}
			if (nightList2 != null)
			{
				for (int l = 0; l < nightList2.Length; l++)
				{
					NGUITools.SetActive(nightList2[l], state: true);
				}
			}
		}
		LightmapSettings.lightmaps = array;
	}

	public void CloseQiQiuRen()
	{
		if (SingletonUnity<SceneObjectController>.Exists)
		{
			GameObject[] dayList = SingletonUnity<SceneObjectController>.Instance.DayList;
			if (dayList.Length >= 2)
			{
				UnityVersionUtil.SetActiveRecursive(dayList[1].gameObject, state: false);
			}
		}
	}

	private void LoadLightmapFinish(string name, Object obj, object param1 = null, object param2 = null)
	{
		if (obj != null)
		{
			LightmapData[] array = new LightmapData[1]
			{
				new LightmapData()
			};
			nightPic = obj as Texture2D;
			array[0].lightmapFar = nightPic;
			LightmapSettings.lightmaps = array;
		}
	}

	private void LoadLightProbeFinish(string name, Object obj, object param1 = null, object param2 = null)
	{
		if (obj != null)
		{
			nightProbes = obj as LightProbes;
			LightmapSettings.lightProbes = nightProbes;
		}
	}

	private void LoadSkyBoxFinish(string name, Object obj, object param1 = null, object param2 = null)
	{
		if (obj != null)
		{
			nightSkyBox = obj as Material;
			RenderSettings.skybox = nightSkyBox;
			if (curSkyBox != null)
			{
				curSkyBox.material = nightSkyBox;
			}
		}
	}

	public void RefershMapActivity()
	{
		if (mMapActivityRoot != null)
		{
			mMapActivityManager.RefershMapInfo(CurActivityMapDataList, mMapActivityRoot.transform);
		}
	}

	public virtual void AutoFightAction()
	{
	}

	public bool IsCopyScene()
	{
		if (mapInfoData.MapType == MAPTYPE.BIG_WORLD)
		{
			return false;
		}
		return true;
	}

	public bool IsCopyShowMap()
	{
		if (mapInfoData.MapType == MAPTYPE.SCUFFLE_AREA_1 || mapInfoData.MapType == MAPTYPE.SCUFFLE_AREA_2 || mapInfoData.MapType == MAPTYPE.CAR_CHASE_COPY || mapInfoData.MapType == MAPTYPE.TUTORIAL_CAR || mapInfoData.MapType == MAPTYPE.SURVIVE_BATTLE1 || mapInfoData.MapType == MAPTYPE.SURVIVE_BATTLE2)
		{
			return true;
		}
		return false;
	}

	public bool IsSingleCopyScene()
	{
		if (mapInfoData.MapType == MAPTYPE.SINGLE_KILL_MONSTER_COPY || mapInfoData.MapType == MAPTYPE.TUTORIAL_SCENE || mapInfoData.MapType == MAPTYPE.SNEAKING_COPY || mapInfoData.MapType == MAPTYPE.CLAMBING_TOWER || mapInfoData.MapType == MAPTYPE.TUTORIAL_CAR || mapInfoData.MapType == MAPTYPE.RANK_PVP || mapInfoData.MapType == MAPTYPE.CASH_DAILY_COPY || mapInfoData.MapType == MAPTYPE.DOMIN_MAP || mapInfoData.MapType == MAPTYPE.ANIMA_EDITOR || mapInfoData.MapType == MAPTYPE.SINGLE_RUN_POINT_COPY || mapInfoData.MapType == MAPTYPE.CAR_CHASE_COPY)
		{
			return true;
		}
		return false;
	}

	public bool IsSingleCopySceneLocal()
	{
		if (mapInfoData.MapType == MAPTYPE.SINGLE_KILL_MONSTER_COPY || mapInfoData.MapType == MAPTYPE.TUTORIAL_SCENE || mapInfoData.MapType == MAPTYPE.SNEAKING_COPY || mapInfoData.MapType == MAPTYPE.CLAMBING_TOWER || mapInfoData.MapType == MAPTYPE.TUTORIAL_CAR || mapInfoData.MapType == MAPTYPE.RANK_PVP || mapInfoData.MapType == MAPTYPE.ANIMA_EDITOR || mapInfoData.MapType == MAPTYPE.SINGLE_RUN_POINT_COPY || mapInfoData.MapType == MAPTYPE.CAR_CHASE_COPY || mapInfoData.MapType == MAPTYPE.DOMIN_MAP)
		{
			return true;
		}
		return false;
	}

	public bool IsCanUsePotion()
	{
		if (mapInfoData.MapType == MAPTYPE.RANK_PVP || mapInfoData.MapType == MAPTYPE.DOMIN_MAP)
		{
			return false;
		}
		return true;
	}

	public bool IsRankPvPScene()
	{
		if (mapInfoData.MapType == MAPTYPE.RANK_PVP)
		{
			return true;
		}
		return false;
	}

	public bool IsSurviveBattleScene()
	{
		if (mapInfoData.MapType == MAPTYPE.SURVIVE_BATTLE1 || mapInfoData.MapType == MAPTYPE.SURVIVE_BATTLE2)
		{
			return true;
		}
		return false;
	}

	public bool IsMultiScene()
	{
		if (mapInfoData.MapType == MAPTYPE.MUTIPLE_KILL_MONSTER_COPY || mapInfoData.MapType == MAPTYPE.EXP_DAILY_COPY || mapInfoData.MapType == MAPTYPE.BAR_FIGHT_COPY || mapInfoData.MapType == MAPTYPE.WILD_BOSS_COPY || mapInfoData.MapType == MAPTYPE.GUILD_BOSS_COPY || mapInfoData.MapType == MAPTYPE.SCUFFLE_AREA_1 || mapInfoData.MapType == MAPTYPE.SCUFFLE_AREA_2 || mapInfoData.MapType == MAPTYPE.SINGLE_EXP_DAILY_COPY || mapInfoData.MapType == MAPTYPE.MULTI_TOWER_COPY || mapInfoData.MapType == MAPTYPE.EQUIP_COPY)
		{
			return true;
		}
		return false;
	}

	public bool IsShowTeamScene()
	{
		return mapInfoData.MapType == MAPTYPE.EQUIP_COPY || mapInfoData.MapType == MAPTYPE.MULTI_TOWER_COPY || mapInfoData.MapType == MAPTYPE.EXP_DAILY_COPY;
	}

	public bool IsExpDailyCopy()
	{
		if (mapInfoData.MapType == MAPTYPE.EXP_DAILY_COPY || mapInfoData.MapType == MAPTYPE.SINGLE_EXP_DAILY_COPY)
		{
			return true;
		}
		return false;
	}

	public bool IsEquipCopy()
	{
		return mapInfoData.MapType == MAPTYPE.EQUIP_COPY;
	}

	public bool IsRealPvPScene()
	{
		if (mapInfoData.MapType == MAPTYPE.REAL_PVP)
		{
			return true;
		}
		return false;
	}

	public bool IsPvPScene()
	{
		if (mapInfoData.MapType == MAPTYPE.REAL_PVP || mapInfoData.MapType == MAPTYPE.RANK_PVP)
		{
			return true;
		}
		return false;
	}

	public bool IsLowPhoneManager()
	{
		if (mapInfoData.MapType == MAPTYPE.LOW_PHONE)
		{
			return true;
		}
		return false;
	}

	public bool IsBigWorld()
	{
		if (mapInfoData.MapType == MAPTYPE.BIG_WORLD)
		{
			return true;
		}
		return false;
	}

	public bool IsWildBossScene()
	{
		if (mapInfoData.MapType == MAPTYPE.WILD_BOSS_COPY)
		{
			return true;
		}
		return false;
	}

	public bool IsShopScene()
	{
		if (mapInfoData.MapType == MAPTYPE.SHOP_COPY)
		{
			return true;
		}
		return false;
	}

	public bool IsScuffleArea()
	{
		if (mapInfoData.MapType == MAPTYPE.SCUFFLE_AREA_2)
		{
			return true;
		}
		return false;
	}

	public bool IsCanAttackOtherPlayerScene()
	{
		if (IsBigWorld() || IsScuffleArea() || IsGuildBattleScene() || IsWildBossScene())
		{
			return true;
		}
		return false;
	}

	public bool IsGuildBattleScene()
	{
		if (mapInfoData.MapType == MAPTYPE.GUILD_BATTLE)
		{
			return true;
		}
		return false;
	}

	public bool IsCanShowCheckPopUI()
	{
		return IsBigWorld() || (IsTutorialScene() && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload);
	}

	public bool IsTutorialScene()
	{
		if (mapInfoData.MapType == MAPTYPE.TUTORIAL_SCENE || mapInfoData.MapType == MAPTYPE.TUTORIAL_CAR)
		{
			return true;
		}
		return false;
	}

	public bool IsDontSynPostion()
	{
		if (IsTutorialScene())
		{
			return false;
		}
		if (IsSingleCopySceneLocal() || IsLowPhoneManager())
		{
			return true;
		}
		return false;
	}

	public bool IsCarScene()
	{
		if (mapInfoData.MapType == MAPTYPE.CAR_CHASE_COPY)
		{
			return true;
		}
		return false;
	}

	public virtual void StartGame()
	{
	}

	public virtual void SuccessMission()
	{
	}

	public virtual void FailMission()
	{
	}

	public virtual void LeaveScene()
	{
		if (!string.IsNullOrEmpty(mapInfoData.ExitPos))
		{
			Singleton<ObjManager>.Instance.MainPlayer.MoveTo(mapInfoData.ExitPosList[0]);
			return;
		}
		MessageBoxLogic.OpenOKCancelBox(mapInfoData.MExitCon, StrDictionary.GetDictionaryString("#{100127}"), delegate
		{
			NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
		});
	}

	public void LeaveCopyScene()
	{
		NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
	}

	public void CompleteMission()
	{
		Singleton<ObjManager>.Instance.MainPlayer.CompleteMissionFlag = true;
		Singleton<ObjManager>.Instance.DisableAllLocalNpcAction();
		Singleton<ObjManager>.Instance.MainPlayer.IsTalking = true;
	}

	protected ParticleSystem CreateStrike(object data)
	{
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/CarModel/FX_Strike") as GameObject;
		return gameObject.GetComponent<ParticleSystem>();
	}

	protected void DestroyStrike(ParticleSystem obj)
	{
		Object.Destroy(obj.gameObject);
	}

	public ParticleSystem GetStrike()
	{
		return StrikePool.Get();
	}

	public void RecycleStrike(ParticleSystem obj)
	{
		StrikePool.Recycle(obj);
	}

	public virtual void OnFindPlayer()
	{
	}

	public bool isPVPScene()
	{
		return isHaveSaftyArea;
	}

	public bool IsInSafeArea(Vector3 pos)
	{
		if (mSaftyAreaData != null)
		{
			for (int i = 0; i < mSaftyAreaData.Count; i++)
			{
				if (mSaftyAreaData[i].Type == 0)
				{
					if (AreaCheckTool.CheckInRectangle(new Vector2(pos.x, pos.z), mSaftyAreaData[i].PointList[0], mSaftyAreaData[i].PointList[1], mSaftyAreaData[i].PointList[2], mSaftyAreaData[i].PointList[3]))
					{
						return true;
					}
				}
				else if (AreaCheckTool.CheckInCircle(new Vector2(pos.x, pos.z), mSaftyAreaData[i].PointList[0], mSaftyAreaData[0].CircleRange))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	public bool IsInGatherArea(Vector3 pos)
	{
		if (mGatherAreaData != null)
		{
			for (int i = 0; i < mGatherAreaData.Count; i++)
			{
				if (mGatherAreaData[i].Type == 0)
				{
					if (AreaCheckTool.CheckInRectangle(new Vector2(pos.x, pos.z), mGatherAreaData[i].PointList[0], mGatherAreaData[i].PointList[1], mGatherAreaData[i].PointList[2], mGatherAreaData[i].PointList[3]))
					{
						return true;
					}
				}
				else if (AreaCheckTool.CheckInCircle(new Vector2(pos.x, pos.z), mGatherAreaData[i].PointList[0], mGatherAreaData[i].CircleRange))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	public void ClearActivityObjInfo()
	{
		SceneComponentObjDic.Clear();
		SceneComponentObjList.Clear();
		NeedShowList.Clear();
		CurLoadActivityobjList.Clear();
		NeedShowList.Clear();
	}

	public bool CheckSceneActiviyTime(SceneComponentData data)
	{
		return TimeTools.IsTimeRange(data.Starttimes, data.EndTimes);
	}

	public void CheckSceneActivityObject()
	{
		List<SceneComponentData> sceneComponentDataListById = DataManager.GetSceneComponentDataListById(CurrentMapInofData.ID);
		if (sceneComponentDataListById == null || sceneComponentDataListById.Count == 0)
		{
			CloseActivityObj();
			return;
		}
		List<SceneComponentData> list = new List<SceneComponentData>();
		for (int i = 0; i < sceneComponentDataListById.Count; i++)
		{
			list.Add(sceneComponentDataListById[i]);
		}
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (CheckSceneActiviyTime(list[num]))
			{
				if (list[num].IsDanceShow != 2)
				{
					if (mCurCityDanceData == null)
					{
						if (list[num].IsDanceShow == 1)
						{
							list.RemoveAt(num);
						}
					}
					else if (list[num].IsDanceShow == 0)
					{
						list.RemoveAt(num);
					}
				}
			}
			else
			{
				list.RemoveAt(num);
			}
		}
		NeedShowList.Clear();
		for (int j = 0; j < list.Count; j++)
		{
			NeedShowList.Add(list[j]);
		}
		for (int num2 = SceneComponentObjList.Count - 1; num2 >= 0; num2--)
		{
			bool flag = false;
			for (int num3 = list.Count - 1; num3 >= 0; num3--)
			{
				if (!string.IsNullOrEmpty(list[num3].PrefabName) && list[num3].PrefabName.Equals(SceneComponentObjList[num2]))
				{
					flag = true;
					list.RemoveAt(num3);
					break;
				}
			}
			if (!flag)
			{
				if (SceneComponentObjDic.ContainsKey(SceneComponentObjList[num2]))
				{
					if (SceneComponentObjDic[SceneComponentObjList[num2]] != null)
					{
						Object.Destroy(SceneComponentObjDic[SceneComponentObjList[num2]]);
					}
					SceneComponentObjDic.Remove(SceneComponentObjList[num2]);
				}
				SceneComponentObjList.RemoveAt(num2);
			}
		}
		if (CurLoadActivityobjList != null && CurLoadActivityobjList.Count > 0)
		{
			for (int k = 0; k < list.Count; k++)
			{
				if (!CurLoadActivityobjList.Contains(list[k].PrefabName))
				{
					CurLoadActivityDic.Add(list[k].PrefabName, list[k]);
					CurLoadActivityobjList.Add(list[k].PrefabName);
				}
			}
			return;
		}
		for (int l = 0; l < list.Count; l++)
		{
			CurLoadActivityDic.Add(list[l].PrefabName, list[l]);
			CurLoadActivityobjList.Add(list[l].PrefabName);
		}
		if (CurLoadActivityobjList != null && CurLoadActivityobjList.Count > 0)
		{
			SceneComponentObjDic.Add(CurLoadActivityobjList[0], null);
			SceneComponentObjList.Add(CurLoadActivityobjList[0]);
			if (UnityVersionUtil.IsactiveInHierarchy(SingletonDontDestoryUnity<GameManager>.Instance.gameObject))
			{
				SingletonDontDestoryUnity<GameManager>.Instance.StartCoroutine(BundleManager.LoadSceneActivityObj(CurLoadActivityobjList[0], CurLoadActivityDic[CurLoadActivityobjList[0]], SceneActivityObjectLoadFinish));
			}
		}
	}

	private void SceneActivityObjectLoadFinish(string name, SceneComponentData data, Object obj)
	{
		if (obj == null)
		{
			return;
		}
		if (CheckActivityobjList(name))
		{
			GameObject gameObject = Object.Instantiate(obj) as GameObject;
			if (data.StaticCombine)
			{
				gameObject.AddComponent<CombineStatic>();
			}
			if (gameObject == null)
			{
				return;
			}
			if (SceneComponentObjDic.ContainsKey(name))
			{
				SceneComponentObjDic[name] = gameObject;
			}
			gameObject.transform.name = name;
			gameObject.transform.position = data.FPos;
			gameObject.transform.eulerAngles = data.FAngle;
			gameObject.transform.localScale = data.FScale;
			BundleManager.ResetAllShader(gameObject.transform);
			if (data != null && data.CloseQiqiuFlag && SingletonUnity<SceneObjectController>.Exists)
			{
				GameObject[] dayList = SingletonUnity<SceneObjectController>.Instance.DayList;
				if (dayList.Length >= 2)
				{
					UnityVersionUtil.SetActiveRecursive(dayList[1].gameObject, state: false);
				}
			}
		}
		if (CurLoadActivityobjList != null && CurLoadActivityobjList.Count > 0)
		{
			CurLoadActivityDic.Remove(CurLoadActivityobjList[0]);
			CurLoadActivityobjList.RemoveAt(0);
		}
		if (CurLoadActivityobjList != null && CurLoadActivityobjList.Count > 0)
		{
			SceneComponentObjDic.Add(CurLoadActivityobjList[0], null);
			SceneComponentObjList.Add(CurLoadActivityobjList[0]);
			if (UnityVersionUtil.IsactiveInHierarchy(SingletonDontDestoryUnity<GameManager>.Instance.gameObject))
			{
				SingletonDontDestoryUnity<GameManager>.Instance.StartCoroutine(BundleManager.LoadSceneActivityObj(CurLoadActivityobjList[0], CurLoadActivityDic[CurLoadActivityobjList[0]], SceneActivityObjectLoadFinish));
			}
		}
	}

	private bool CheckActivityobjList(string name)
	{
		if (NeedShowList != null && NeedShowList.Count > 0)
		{
			for (int i = 0; i < NeedShowList.Count; i++)
			{
				if (!string.IsNullOrEmpty(NeedShowList[i].PrefabName) && NeedShowList[i].PrefabName.Equals(name))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void CloseActivityObj()
	{
		if (SceneComponentObjDic == null || SceneComponentObjDic.Count <= 0)
		{
			return;
		}
		foreach (GameObject value in SceneComponentObjDic.Values)
		{
			if (value != null)
			{
				UnityVersionUtil.SetActiveRecursive(value, state: false);
			}
		}
	}

	public void CheckSceneActivity()
	{
		CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById("101");
		if (CurrentMapInofData.ID.Equals(cityDanceDataById.MapId))
		{
			InitCityDanceScene(cityDanceDataById);
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.FirstGuildDanceOpen == 1)
			{
				ChangeLightMap(isDay: false);
			}
			else
			{
				ChangeLightMap(isDay: true);
			}
		}
	}

	public bool IsCiytDanceSceneActive()
	{
		return mCityDanceSceneObj != null;
	}

	public void InitCityDanceScene(CityDanceData data)
	{
		if (mCityDanceSceneObj == null)
		{
			mCurCityDanceData = data;
			if (UnityVersionUtil.IsactiveInHierarchy(SingletonDontDestoryUnity<GameManager>.Instance.gameObject))
			{
				SingletonDontDestoryUnity<GameManager>.Instance.StartCoroutine(BundleManager.LoadItem("CityDanceScene", DanceSceneLoadFinish));
			}
			CloseQiQiuRen();
			CheckSceneActivityObject();
		}
	}

	private void DanceSceneLoadFinish(string name, Object fakeobj, object param1 = null, object param2 = null)
	{
		if (fakeobj != null)
		{
			mCityDanceSceneObj = Object.Instantiate(fakeobj) as GameObject;
		}
		if (mCityDanceSceneObj != null)
		{
			Transform obj = mCityDanceSceneObj.transform.FindChild("DSJ_zhuCheng_LingWuXuanZhuan01/DJ_DjTai");
			Transform obj2 = mCityDanceSceneObj.transform.FindChild("DSJ_zhuCheng_LingWuXuanZhuan01/NPC_Nv_013");
			Transform obj3 = mCityDanceSceneObj.transform.FindChild("DSJ_zhuCheng_LingWuXuanZhuan01/NPC_Nv_014");
			BundleManager.ResetShader(obj);
			BundleManager.ResetShader(obj2);
			BundleManager.ResetShader(obj3);
		}
	}

	public static bool IsInNavmeshArea(Vector3 pos)
	{
		pos.y = 150f;
		if (Physics.Raycast(pos, Vector3.down, out var _, 300f, 134217728))
		{
			return true;
		}
		return false;
	}

	public static float GetHitHeight(Vector3 pos)
	{
		pos.y = 150f;
		if (Physics.Raycast(pos, Vector3.down, out var hitInfo, 300f, 134217728))
		{
			return hitInfo.point.y;
		}
		return 0f;
	}

	public static float GetHitHeight(Vector3 pos, out bool isHit)
	{
		pos.y = 150f;
		isHit = false;
		if (Physics.Raycast(pos, Vector3.down, out var hitInfo, 300f, 134217728))
		{
			isHit = true;
			return hitInfo.point.y;
		}
		return 0f;
	}

	public static float GetHitHeight(float x, float z)
	{
		Vector3 pos = new Vector3(x, 0f, z);
		return GetHitHeight(pos);
	}

	public void RemoveCityDanceScene()
	{
		if (mCityDanceSceneObj != null)
		{
			Object.Destroy(mCityDanceSceneObj);
			ChangeLightMap(isDay: true);
			Singleton<ObjManager>.Instance.MainPlayer.RemoveDance();
			mCityDanceSceneObj = null;
			mCurCityDanceData = null;
			CheckSceneActivityObject();
		}
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.DisableDanceBtn();
		}
		if (SingletonDontDestoryUnity<GameManager>.Exists && SingletonDontDestoryUnity<SoundManager>.Exists)
		{
			SoundData soundDataById = DataManager.GetSoundDataById(CurrentMapInofData.GetAudioID());
			if (soundDataById != null)
			{
				SingletonDontDestoryUnity<SoundManager>.Instance.PlayBGMusic(soundDataById.Id, soundDataById.FadeOutTime, soundDataById.FadeInTime);
			}
		}
	}

	public virtual void OnReconnectSuccess()
	{
	}

	public virtual void OnNPCDie(object objNpc)
	{
	}

	public virtual void SetMoveTarget(Vector3 pos, string missionId)
	{
	}

	public virtual void ClearMoveTarget()
	{
		IsHaveMoveTarget = false;
	}

	public virtual void MissionCheckMoveTarget(string missionId)
	{
	}
}
