using System;
using System.Collections.Generic;
using DG.Tweening;
using SprotoType;
using UnityEngine;

public class NewTutorialSceneManager : SceneManager
{
	private SmoothFollowNew mSmoothFollowCamPos;

	private Vector3 mMainPlayerRunTargetPos;

	private Vector3 mPlayerCarCreatePos;

	private Vector3 mPlayerCarCreateAngle;

	private ObjMainPlayer mMainPlayer;

	private ObjPlayerCar mPlayerCar;

	private MovePathPoint mTutorialMovePathPoint;

	private MovePathPoint mMoveTargetPathPoint;

	private List<Vector3> mPlayerCarPathList = new List<Vector3>();

	private int mCurPlayerCarPathIndex;

	private CopySceneData mCurCopySceneData;

	private CarHPRootLogic carHPRoot;

	private Transform mPlayerPathPointRoot;

	private int mWaveCount;

	private int mStepIndex;

	private int mCurEnemyGroup;

	private int mCurPlayerPathIndex;

	public Transform TalkNpcPos;

	private ObjNPC mTalkingNpc;

	private CameraController mCamCtl;

	private GameObject mCarAnimaObj;

	private int DRIVING_CAR_TUTORIAL_STEP = 8;

	private GameObject mTutorialStaticMeshObj;

	private bool mIsTutorialFinish;

	public ObjFakeAICar mTutorialCar;

	private GameObject mRandomWalkPosRoot;

	private List<Vector3> mRandomWalkPosList = new List<Vector3>();

	private string[] NpcIdList = new string[5] { "1072", "1073", "1074", "1075", "1076" };

	private List<ObjNPC> mCurRandomWalkNpcList = new List<ObjNPC>();

	private string[] NpcIdList2 = new string[5] { "1082", "1083", "1084", "204", "204" };

	private Dictionary<long, vp_Timer.Handle> walkNpcHandle = new Dictionary<long, vp_Timer.Handle>();

	private GameObject mStartSceneAnimaObj;

	private GameObject mRobCarSceneAnimaObj;

	private GameObject mCrashCarSceneAnimaObj;

	private Dictionary<string, GameObject> mStaticBlockDic = new Dictionary<string, GameObject>();

	private Dictionary<int, MoveCarBlock> mMoveCarBlockDic = new Dictionary<int, MoveCarBlock>();

	public SceneAnimationCtl curAnimaCtl;

	private bool loadingOverFlag;

	private float FreshNPCTimeCount;

	private float FreshNPCTime = 2f;

	private float PlayerBolckCheckTime = 5f;

	private float playerBlockTimeCount;

	private bool mIsCarCtlTutorialShow;

	private float checkHpTimeCount;

	private float waitDownloadStartTime;

	private int waitTime = 30;

	private float flashTimeCount;

	private bool LerpToCarFlag;

	private Vector3 startPosition;

	private Quaternion startRotation;

	private float lerpTime = 1f;

	private float lerpCountTime;

	private bool isFinishRobCarAnima;

	private GameObject staticNpc;

	private int ROBBING_CAR_STATE = 7;

	private string curMoveTargetMissionId = string.Empty;

	public SmoothFollowNew SmoothFollowCamPos => mSmoothFollowCamPos;

	public override void Init(string id)
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.FirstEnterGame)
		{
			NetLogic.GetInstance().Send<Protocol.request_domin_info>();
		}
		base.Init(id);
		GameObject gameObject = GameObject.Find("SmoothFollowCamPos");
		if (gameObject != null)
		{
			mSmoothFollowCamPos = gameObject.GetComponent<SmoothFollowNew>();
		}
		UnityVersionUtil.SetActiveRecursive(mSmoothFollowCamPos.gameObject, state: false);
		if (SingletonUnity<MyEvent>.Exists)
		{
			SingletonUnity<MyEvent>.Instance.Register("OnMainPlayerCreate", this, "OnMainPlayerCreate");
		}
		else
		{
			Debug.Log("SingletonUnity<MyEvent>.Exists == false");
		}
		StrikePool = new SimplePool<ParticleSystem>();
		StrikePool.Reset(base.CreateStrike, base.DestroyStrike, 10);
		mIsTutorialFinish = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsTutorialFinish;
		if (!mIsTutorialFinish)
		{
			gameObject = ResourcesManager.LoadAndInstantiate("Items/MovePathPoint") as GameObject;
			mTutorialMovePathPoint = gameObject.GetComponent<MovePathPoint>();
			mTutorialMovePathPoint.RegisterOnArrivePathPoint(OnPlayerArrivePathPoint);
			mWaveCount = GetMonsterGroupCount();
			mStepIndex = 0;
			mCurEnemyGroup = 0;
			mCurPlayerPathIndex = 1;
			UnityVersionUtil.SetActiveRecursive(mTutorialMovePathPoint.gameObject, state: false);
			mTutorialMovePathPoint.ActiveRadius = 2f;
			GameObject gameObject2 = ResourcesManager.LoadAndInstantiate("Tutorial/TutorialPlayerPathRoot") as GameObject;
			mPlayerPathPointRoot = gameObject2.transform;
			mStartSceneAnimaObj = ResourcesManager.LoadAndInstantiate("StartSceneAnima/kaiChangSceneAnima") as GameObject;
			UnityVersionUtil.SetActiveRecursive(mStartSceneAnimaObj, state: false);
			if (GameSettingData.GetPhoneClass() == 0)
			{
				waitTime = 5;
			}
			else
			{
				waitTime = 30;
			}
		}
		if (base.CurrentMapInofData.SceneName.Equals("FB_saiDao_1"))
		{
			ResourcesManager.LoadAndInstantiate("Tutorial/CitySimController");
		}
		else
		{
			ResourcesManager.LoadAndInstantiate("Tutorial/CitySimController_GTA");
		}
		if (SingletonUnity<CitySimController>.Exists)
		{
			SingletonUnity<CitySimController>.Instance.InitMonsterList();
		}
		base.IsMissionStart = true;
		mTutorialCar = null;
	}

	~NewTutorialSceneManager()
	{
	}

	public void CheckLockArea()
	{
	}

	private void CreateTutorialCar()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsTutorialCanShow(FUNCTION_TYPE.ROB_CAR_TIP))
		{
			TalkNpcPos = GameObject.Find("TalkNpcPos").transform;
			ObjCarInitData initData = new ObjCarInitData(TalkNpcPos.position, TalkNpcPos.eulerAngles, UUID.GenUUID(), "Chevrolet", policeFlag: false, DataManager.GetMountDataById("Chevrolet"));
			ObjFakeAICar fakeAICar = Singleton<ObjManager>.Instance.GetFakeAICar(initData);
			if (fakeAICar != null)
			{
				fakeAICar.ResetStaticCar();
				mTutorialCar = fakeAICar;
			}
		}
	}

	private void CreateStartNPC()
	{
		TalkNpcPos = GameObject.Find("TalkNpcPos").transform;
		List<MonsterData> list = new List<MonsterData>();
		MonsterData monsterData = new MonsterData();
		monsterData.MapID = "11";
		monsterData.Group = 1;
		monsterData.NpcID = "100";
		monsterData.PosX = -13100;
		monsterData.PosZ = 8700;
		monsterData.PosO = 0;
		list.Add(monsterData);
		NpcCreate(list);
		CreatWalkNpc();
	}

	private void CreatWalkNpc()
	{
		mRandomWalkPosRoot = GameObject.Find("TutorialRandomWalkNpcPos");
		mRandomWalkPosList.Clear();
		for (int i = 0; i < mRandomWalkPosRoot.transform.childCount; i++)
		{
			mRandomWalkPosList.Add(mRandomWalkPosRoot.transform.GetChild(i).position);
		}
		ObjManager instance = Singleton<ObjManager>.Instance;
		for (int j = 0; j < 5; j++)
		{
			ObjInitNpcData objInitNpcData = new ObjInitNpcData();
			objInitNpcData.mServerID = UUID.GenUUID();
			objInitNpcData.mPos = mRandomWalkPosList[j];
			objInitNpcData.mDir = MathUtil.HeadingToVector3(UnityEngine.Random.Range(0, 360));
			objInitNpcData.npcInfoData = DataManager.GetNpcDataByID(NpcIdList[j]);
			instance.CreateNPC(objInitNpcData, OnCreateNpc);
		}
	}

	private void CreatWalkNpc2()
	{
	}

	private void CreateWalkNpc2(string npcId)
	{
		ObjManager instance = Singleton<ObjManager>.Instance;
		ObjInitNpcData objInitNpcData = new ObjInitNpcData();
		objInitNpcData.mServerID = UUID.GenUUID();
		objInitNpcData.mPos = mRandomWalkPosList[UnityEngine.Random.Range(0, mRandomWalkPosList.Count)];
		objInitNpcData.mDir = MathUtil.HeadingToVector3(UnityEngine.Random.Range(0, 360));
		objInitNpcData.npcInfoData = DataManager.GetNpcDataByID(npcId);
		objInitNpcData.MaxHP = objInitNpcData.npcInfoData.Hp;
		objInitNpcData.HP = objInitNpcData.npcInfoData.Hp;
		objInitNpcData.ATK = objInitNpcData.npcInfoData.Atk;
		objInitNpcData.DEF = objInitNpcData.npcInfoData.Def;
		objInitNpcData.HIT = objInitNpcData.npcInfoData.HIT;
		objInitNpcData.EVA = objInitNpcData.npcInfoData.DGE;
		objInitNpcData.CRI = objInitNpcData.npcInfoData.CRI;
		objInitNpcData.EXD = objInitNpcData.npcInfoData.EXD;
		objInitNpcData.EXR = objInitNpcData.npcInfoData.EXR;
		objInitNpcData.RES = objInitNpcData.npcInfoData.RES;
		objInitNpcData.CRD = objInitNpcData.npcInfoData.CRD;
		objInitNpcData.CRR = objInitNpcData.npcInfoData.CRR;
		objInitNpcData.DEFA = objInitNpcData.npcInfoData.DEFA;
		objInitNpcData.DGEA = objInitNpcData.npcInfoData.DGEA;
		objInitNpcData.HITA = objInitNpcData.npcInfoData.HITA;
		objInitNpcData.RESA = objInitNpcData.npcInfoData.RESA;
		objInitNpcData.CRIA = objInitNpcData.npcInfoData.CRIA;
		objInitNpcData.Level = objInitNpcData.npcInfoData.Lv;
		objInitNpcData.AntiStun = objInitNpcData.npcInfoData.AntiStun;
		objInitNpcData.AntiKnockDown = objInitNpcData.npcInfoData.AntiKnockDown;
		instance.CreateNPC(objInitNpcData, OnCreateNpc);
	}

	private void OnCreateNpc(ObjNPC objNpc)
	{
		mCurRandomWalkNpcList.Add(objNpc);
		objNpc.WalkMoveTo(mRandomWalkPosList[UnityEngine.Random.Range(0, mRandomWalkPosList.Count)], 1f, OnArrivePoint);
	}

	private void OnArrivePoint(ObjCharacter objCha)
	{
		if (walkNpcHandle.ContainsKey(objCha.ServerId))
		{
			if (walkNpcHandle[objCha.ServerId] != null)
			{
				walkNpcHandle[objCha.ServerId].Cancel();
			}
			else
			{
				walkNpcHandle[objCha.ServerId] = new vp_Timer.Handle();
			}
		}
		else
		{
			walkNpcHandle.Add(objCha.ServerId, new vp_Timer.Handle());
		}
		vp_Timer.In(UnityEngine.Random.Range(0, 4), delegate
		{
			if (objCha != null && UnityVersionUtil.IsActive(objCha.gameObject) && objCha.NavMeshAgent.enabled)
			{
				objCha.WalkMoveTo(mRandomWalkPosList[UnityEngine.Random.Range(0, mRandomWalkPosList.Count)], 1f, OnArrivePoint);
			}
		}, walkNpcHandle[objCha.ServerId]);
	}

	private void ClearWalkNpc()
	{
		GameObject gameObject = GameObject.Find("TutorialEscapeNpcPos");
		for (int i = 0; i < mCurRandomWalkNpcList.Count; i++)
		{
			if (walkNpcHandle.ContainsKey(mCurRandomWalkNpcList[i].ServerId) && walkNpcHandle[mCurRandomWalkNpcList[i].ServerId] != null)
			{
				walkNpcHandle[mCurRandomWalkNpcList[i].ServerId].Cancel();
			}
			mCurRandomWalkNpcList[i].DisactiveTargetArriveFinish();
			mCurRandomWalkNpcList[i].MoveTo(gameObject.transform.GetChild(i).position, 1f, delegate(ObjCharacter npc)
			{
				Singleton<ObjManager>.Instance.RecycleNpc(npc as ObjNPC);
			});
		}
	}

	private void InitBlock(string sceneId)
	{
		List<CarMissionBlockData> carMissionBlockDataListBySceneId = DataManager.GetCarMissionBlockDataListBySceneId(sceneId);
		GameObject gameObject = new GameObject("BlockRoot");
		GameObject gameObject2 = new GameObject("StaticObstacleRoot");
		GameObject gameObject3 = new GameObject("MoveCarBlockRoot");
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.rotation = Quaternion.identity;
		gameObject2.transform.parent = gameObject.transform;
		gameObject3.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject3.transform.localPosition = Vector3.zero;
		gameObject3.transform.localRotation = Quaternion.identity;
		List<KeyValuePair<int, Vector3>> list = new List<KeyValuePair<int, Vector3>>();
		for (int i = 0; i < carMissionBlockDataListBySceneId.Count; i++)
		{
			if (carMissionBlockDataListBySceneId[i].PointType == 0 || carMissionBlockDataListBySceneId[i].PointType == 3 || carMissionBlockDataListBySceneId[i].PointType == 4)
			{
				MoveCarBlock moveCarBlock = null;
				CarPath carPath = null;
				if (mMoveCarBlockDic.ContainsKey(carMissionBlockDataListBySceneId[i].BlockGroup))
				{
					moveCarBlock = mMoveCarBlockDic[carMissionBlockDataListBySceneId[i].BlockGroup];
					carPath = moveCarBlock.Path;
				}
				else
				{
					GameObject gameObject4 = new GameObject($"Block{carMissionBlockDataListBySceneId[i].BlockGroup}");
					gameObject4.transform.parent = gameObject3.transform;
					gameObject4.transform.position = Vector3.zero;
					gameObject4.transform.rotation = Quaternion.identity;
					BoxCollider boxCollider = gameObject4.AddComponent<BoxCollider>();
					boxCollider.size = new Vector3(120f, 5f, 1f);
					boxCollider.isTrigger = true;
					moveCarBlock = gameObject4.AddComponent<MoveCarBlock>();
					mMoveCarBlockDic.Add(carMissionBlockDataListBySceneId[i].BlockGroup, moveCarBlock);
					GameObject gameObject5 = new GameObject($"Path");
					gameObject5.transform.parent = gameObject4.transform;
					gameObject5.transform.localPosition = Vector3.zero;
					gameObject5.transform.localRotation = Quaternion.identity;
					carPath = (moveCarBlock.Path = gameObject5.AddComponent<CarPath>());
				}
				if (carMissionBlockDataListBySceneId[i].BlockIndex == -1)
				{
					moveCarBlock.transform.position = new Vector3(carMissionBlockDataListBySceneId[i].PosX, carMissionBlockDataListBySceneId[i].PosY, carMissionBlockDataListBySceneId[i].PosZ);
					moveCarBlock.transform.eulerAngles = new Vector3(carMissionBlockDataListBySceneId[i].AngleX, carMissionBlockDataListBySceneId[i].AngleY, carMissionBlockDataListBySceneId[i].AngleZ);
					moveCarBlock.PoliceFlag = carMissionBlockDataListBySceneId[i].PointType;
					continue;
				}
				GameObject gameObject6 = new GameObject($"Point{carMissionBlockDataListBySceneId[i].BlockIndex}");
				gameObject6.transform.parent = carPath.transform;
				gameObject6.transform.position = new Vector3(carMissionBlockDataListBySceneId[i].PosX, carMissionBlockDataListBySceneId[i].PosY, carMissionBlockDataListBySceneId[i].PosZ);
				gameObject6.transform.eulerAngles = new Vector3(carMissionBlockDataListBySceneId[i].AngleX, carMissionBlockDataListBySceneId[i].AngleY, carMissionBlockDataListBySceneId[i].AngleZ);
				CarPathPoint carPathPoint = gameObject6.AddComponent<CarPathPoint>();
				carPathPoint.Speed = carMissionBlockDataListBySceneId[i].MaxSpeed;
				if (carPath.PathPointList.Count <= carMissionBlockDataListBySceneId[i].BlockIndex)
				{
					carPath.PathPointList.Add(carPathPoint);
				}
				else if (carPath.PathPointList.IndexOf(carPathPoint) != carMissionBlockDataListBySceneId[i].BlockIndex)
				{
					carPath.PathPointList[carMissionBlockDataListBySceneId[i].BlockIndex] = carPathPoint;
				}
			}
			else if (carMissionBlockDataListBySceneId[i].PointType == 1)
			{
				GameObject gameObject7 = null;
				if (mStaticBlockDic.ContainsKey(carMissionBlockDataListBySceneId[i].ObstacleName))
				{
					gameObject7 = UnityEngine.Object.Instantiate(mStaticBlockDic[carMissionBlockDataListBySceneId[i].ObstacleName]) as GameObject;
				}
				else
				{
					gameObject7 = ResourcesManager.LoadAndInstantiate($"CarMissionBlock/{carMissionBlockDataListBySceneId[i].ObstacleName}") as GameObject;
					mStaticBlockDic.Add(carMissionBlockDataListBySceneId[i].ObstacleName, gameObject7);
				}
				if (gameObject7 == null)
				{
					Debug.Log("curBlockDataList[i].ObstacleName :: " + carMissionBlockDataListBySceneId[i].ObstacleName);
				}
				gameObject7.transform.parent = gameObject2.transform;
				gameObject7.transform.position = new Vector3(carMissionBlockDataListBySceneId[i].PosX, carMissionBlockDataListBySceneId[i].PosY, carMissionBlockDataListBySceneId[i].PosZ);
				gameObject7.transform.eulerAngles = new Vector3(carMissionBlockDataListBySceneId[i].AngleX, carMissionBlockDataListBySceneId[i].AngleY, carMissionBlockDataListBySceneId[i].AngleZ);
			}
			else if (carMissionBlockDataListBySceneId[i].PointType == 2)
			{
				if (carMissionBlockDataListBySceneId[i].BlockIndex == 0)
				{
					mPlayerCarCreatePos = new Vector3(carMissionBlockDataListBySceneId[i].PosX, carMissionBlockDataListBySceneId[i].PosY, carMissionBlockDataListBySceneId[i].PosZ);
					mPlayerCarCreateAngle = new Vector3(carMissionBlockDataListBySceneId[i].AngleX, carMissionBlockDataListBySceneId[i].AngleY, carMissionBlockDataListBySceneId[i].AngleZ);
				}
				else
				{
					list.Add(new KeyValuePair<int, Vector3>(carMissionBlockDataListBySceneId[i].BlockIndex, new Vector3(carMissionBlockDataListBySceneId[i].PosX, carMissionBlockDataListBySceneId[i].PosY, carMissionBlockDataListBySceneId[i].PosZ)));
				}
			}
		}
		list.Sort((KeyValuePair<int, Vector3> pre, KeyValuePair<int, Vector3> next) => pre.Key - next.Key);
		mCurPlayerCarPathIndex = 0;
		for (int j = 0; j < list.Count; j++)
		{
			mPlayerCarPathList.Add(list[j].Value);
		}
	}

	public override void OnLoadingOver()
	{
		base.OnLoadingOver();
		loadingOverFlag = true;
		if (mStartSceneAnimaObj != null)
		{
			UnityVersionUtil.SetActiveRecursive(mStartSceneAnimaObj, state: true);
			curAnimaCtl = mStartSceneAnimaObj.GetComponent<SceneAnimationCtl>();
			curAnimaCtl.RegisterOnFinished(OnFinishedStartSceneAnima);
			mCamCtl.CurCamera.enabled = false;
			if (SingletonUnity<ScreenBottomBtn>.Exists)
			{
				SingletonUnity<ScreenBottomBtn>.Instance.LockBtn = true;
			}
			UICamera.mainCamera.depth = 2f;
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SkipBtnRoot);
		}
		else if (!mIsTutorialFinish)
		{
			OnFinishedStartSceneAnima();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckShowUnlockFunction();
		}
	}

	public void OnFinishedStartSceneAnima()
	{
		mCamCtl.CurCamera.enabled = true;
		mCamCtl.CurCamera.farClipPlane = 500f;
		if (SingletonUnity<ScreenBottomBtn>.Exists)
		{
			SingletonUnity<ScreenBottomBtn>.Instance.LockBtn = false;
		}
		UICamera.mainCamera.depth = 0f;
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SkipBtnRoot);
		MoveNextState();
	}

	public override void StartGame()
	{
		Debug.Log("CarChaseSceneManager StartGame");
	}

	private void OnArriveCarPathPoint(Vector3 pos)
	{
		mCurPlayerCarPathIndex++;
		if (mCurPlayerCarPathIndex < mPlayerCarPathList.Count)
		{
			SetPlayerCarMoveTarget();
		}
		else
		{
			if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
			{
				SingletonUnity<MiniMap>.Instance.HideTarget();
			}
			MoveNextState();
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "step", "14CarArriveAtPoint_" + (mCurPlayerCarPathIndex - 1));
	}

	public override void SuccessMission()
	{
	}

	public override void FailMission()
	{
	}

	public override void Update()
	{
		base.Update();
		if (mIsTutorialFinish && mMainPlayer != null && SingletonUnity<CitySimController>.Exists)
		{
			SingletonUnity<CitySimController>.Instance.UpdateCityCheck();
			flashTimeCount += Time.deltaTime;
			if (flashTimeCount >= 0.5f)
			{
				UpdateMapLine();
			}
		}
	}

	private void FreshNPC()
	{
		if (SingletonUnity<EasyCitySimController>.Exists)
		{
			SingletonUnity<EasyCitySimController>.Instance.FreshNPC(mMainPlayer.CacheTransform);
		}
	}

	public void MissionMoveToPoint()
	{
		mMainPlayer.MoveTo(mTutorialMovePathPoint.transform.position);
	}

	public void RobCar()
	{
		mMainPlayer.rigidbody.isKinematic = true;
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		mMainPlayer.MoveTo(mPlayerCar.DummyPlayerPoint.position, 1f, delegate
		{
			mMainPlayer.CameraController.LerpToTargetLocalZero(mPlayerCar.CarDoorView, 1f);
			mMainPlayer.DisableNavMeshAgent();
			mMainPlayer.CacheTransform.parent = mPlayerCar.DummyPlayerPoint;
			mMainPlayer.CacheTransform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
			mMainPlayer.CacheTransform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
			vp_Timer.In(0.1f, delegate
			{
				mPlayerCar.MeshRoot.animation.Play("RobCar");
				FakeObjLogic fakeObj = new FakeObjLogic();
				fakeObj.InitAnimaFakeNpcObj("NPC_Nan_036", mPlayerCar.DummyNPCPoint, "RobOffCar");
				mMainPlayer.AnimationLogic.PlayAnimation("RobCar", null, -1f);
				vp_Timer.In(mMainPlayer.AnimationLogic.CurAnimationLength, delegate
				{
					mMainPlayer.DisableMainPlayer();
					MoveNextState();
					ChangeToCarCtl();
					UnityEngine.Object.Destroy(mCarAnimaObj);
					UnityVersionUtil.SetActiveRecursive(mTutorialStaticMeshObj, state: false);
					fakeObj.FakeObj.transform.parent.parent = null;
					vp_Timer.In(2f, delegate
					{
						fakeObj.DestroyNpcFakeObj();
						fakeObj = null;
					});
				});
			});
		});
	}

	private void GetOffCar()
	{
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		mPlayerCar.DisableCar();
		mMainPlayer.EnableMainPlayer();
		mPlayerCar.MeshRoot.animation.Play("GetOffCar");
		mMainPlayer.CameraController.LerpToTargetLocalZero(mPlayerCar.CarDoorView, 1f);
		mMainPlayer.AnimationLogic.PlayAnimation("GetOffCar", delegate
		{
		}, -1f);
	}

	private void ChangeToCarCtl()
	{
		UIManager instance = SingletonUnity<UIManager>.Instance;
		instance.ReShowBaseUI();
		instance.CloseUI(UIInfo.YiDongKongZhiUI);
		instance.CloseUI(UIInfo.JueseJiNengQuUI);
		instance.CloseUI(UIInfo.TouXiangKuangUI);
		instance.ShowCarDefaultUI();
		instance.CloseUI(UIInfo.CopyFunctionBtnRoot);
		instance.CloseUI(UIInfo.ExpLineRoot);
		instance.ShowUI(UIInfo.CarTargetRoot, delegate
		{
			SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mTutorialMovePathPoint.gameObject, mPlayerCar.gameObject);
		});
		if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
		{
			SingletonUnity<MiniMap>.Instance.SetTarget(mTutorialMovePathPoint.transform.position);
		}
		CameraController cameraController = mMainPlayer.CameraController;
		cameraController.LerpToTargetLocalZero(mSmoothFollowCamPos.transform, 1f, delegate
		{
			base.IsMissionStart = true;
			mPlayerCar.EnableCar(mMainPlayer);
		});
		TutorialManager.ShowTutorial(TUTORIAL_STEP.CAR_ACCBTN_START);
	}

	private void CreatePlayerCar()
	{
		ObjCarInitData initData = new ObjCarInitData(mPlayerCarCreatePos, mPlayerCarCreateAngle, UUID.GenUUID(), "Chevrolet", policeFlag: false);
		Singleton<ObjManager>.Instance.CreateTutorialPlayerCar(initData);
		mPlayerCar = Singleton<ObjManager>.Instance.MainPlayerCar;
		mSmoothFollowCamPos.SetTarget(mPlayerCar.CarControl);
	}

	public void OnMainPlayerCreate()
	{
		SingletonUnity<MyEvent>.Instance.DeRegister("OnMainPlayerCreate", this, "OnMainPlayerCreate");
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		mMainPlayer.gameObject.rigidbody.isKinematic = false;
		if (!mIsTutorialFinish)
		{
			mMainPlayer.DisableNavMeshAgent();
			mMainPlayer.Position = mPlayerPathPointRoot.GetChild(0).position;
			mMainPlayer.FaceToPub(mMainPlayer.Position + mPlayerPathPointRoot.GetChild(0).forward);
			mMainPlayer.EnableNavMeshAgent();
			mCamCtl = mMainPlayer.CameraController;
			mCamCtl.Init();
			mCamCtl.mScale = 0.7f;
			SingletonUnity<UIManager>.Instance.ShowDefaultUI();
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.SetFirstSceneMissionTarget();
		}
		else
		{
			SingletonUnity<UIManager>.Instance.ShowDefaultUI();
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DownLoadTipRoot, delegate
				{
					SingletonUnity<DownloadTipRootLogic>.Instance.Reset();
				});
			}
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.SetFirstSceneMissionTarget();
		}
		if (SingletonUnity<CitySimController>.Exists)
		{
			SingletonUnity<CitySimController>.Instance.UpdateSpecialCarState();
		}
	}

	private void OnStoryShowOver(string storyId)
	{
		UIUpdateEvent.OnStoryShowOver = (UIUpdateEvent.OnStoryShowOverDelegate)Delegate.Remove(UIUpdateEvent.OnStoryShowOver, new UIUpdateEvent.OnStoryShowOverDelegate(OnStoryShowOver));
		MoveNextState();
	}

	private void MoveToPlayerCar()
	{
		mMainPlayerRunTargetPos = mPlayerCar.DummyPlayerPoint.position;
		mMainPlayer.MoveTo(mPlayerCar.DummyPlayerPoint.position, 1f, delegate
		{
			Time.timeScale = 1f;
			mMainPlayer.CameraController.LerpToTargetLocalZero(mPlayerCar.CarDoorView, 1f);
			mMainPlayer.DisableNavMeshAgent();
			mMainPlayer.CacheTransform.parent = mPlayerCar.DummyPlayerPoint;
			mMainPlayer.CacheTransform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
			mMainPlayer.CacheTransform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
			vp_Timer.In(0.1f, delegate
			{
				mPlayerCar.MeshRoot.animation.Play("GetOnCar");
				mMainPlayer.AnimationLogic.PlayAnimation("GetOnCar", delegate
				{
					mMainPlayer.DisableMainPlayer();
					ChangeToCarCtl();
				}, -1f);
			});
		});
	}

	private void OnPlayerArrivePathPoint(Vector3 pos)
	{
		UnityVersionUtil.SetActiveRecursive(mTutorialMovePathPoint.gameObject, state: false);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CarTargetRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DragScreenHelpRoot);
		MoveNextState();
		mCurPlayerPathIndex++;
	}

	public void OnClickTalkNpc(TapGesture gesture)
	{
		Ray ray = Camera.main.ScreenPointToRay(gesture.Position);
		CastScreenRay(ray, gesture);
	}

	private void CastScreenRay(Ray ray, TapGesture gesture)
	{
		RaycastHit hitInfo = default(RaycastHit);
		if (!Physics.Raycast(ray, out hitInfo, 200f))
		{
			return;
		}
		ObjCharacter component = hitInfo.collider.gameObject.GetComponent<ObjCharacter>();
		if (!(component != null))
		{
			return;
		}
		if (component.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			Ray ray2 = new Ray(hitInfo.point + ray.direction * 0.1f, ray.direction);
			CastScreenRay(ray2, gesture);
		}
		else
		{
			if (component.ObjType != 0)
			{
				return;
			}
			ObjNPC objNPC = component as ObjNPC;
			if (objNPC.AttributeData.Camp == GameDefine.CAMP_TYPE.NORMAL_NPC)
			{
				TutorialManager.MoveNext();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.YiDongKongZhiUI, delegate
				{
					SingletonUnity<JoyStickLogic>.Instance.HidePic();
				});
				SingletonUnity<InputController>.Instance.TapRecognizer.OnGesture -= OnClickTalkNpc;
				TutorialManager.Show_CLICK_NPC_FINISH();
				if (mStepIndex == 4)
				{
					MoveNextState();
				}
			}
		}
	}

	public void MoveNextState()
	{
		mStepIndex++;
		switch (mStepIndex)
		{
		case 1:
			SendServerFinishTutorial();
			break;
		case 2:
			TutorialManager.CloseTutorial();
			SendServerFinishTutorial();
			SingletonUnity<JueseJiNengQuLogic>.Instance.ShowAllSkillBtn();
			break;
		case 3:
			ClearWalkNpc();
			SingletonUnity<JueseJiNengQuLogic>.Instance.ShowAllSkillBtn();
			TutorialManager.ShowTutorial(TUTORIAL_STEP.SKILL1_BUTTON);
			SingletonUnity<DownloadResTipRootLogic>.Instance.UpdateTutorialStep(0, 0f);
			break;
		case 4:
			CreateNPCGroup();
			FunctionTipsRootLogic.ClearHandTip();
			break;
		case 5:
			ClearWalkNpc();
			SingletonUnity<JueseJiNengQuLogic>.Instance.ShowAllSkillBtn();
			TutorialManager.ShowTutorial(TUTORIAL_STEP.SKILL1_BUTTON);
			SendServerFinishTutorial();
			break;
		case 6:
			SetPlayerMoveTarget();
			SingletonUnity<DownloadResTipRootLogic>.Instance.UpdateTutorialStep(3, 1f);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FakeMissioonTutorial);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "step", "7MOVETO_CAR");
			break;
		case 7:
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DownLoadResTipRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FakeMissioonTutorial);
			Singleton<ObjManager>.Instance.RecycleAllNPC();
			TutorialManager.CloseTutorial();
			StartRobCarState();
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "step", "8ArriveAtPoint");
			break;
		case 8:
			mIsCarCtlTutorialShow = false;
			mTutorialMovePathPoint.DeRegisterOnArrivePathPoint(OnPlayerArrivePathPoint);
			mTutorialMovePathPoint.RegisterOnArrivePathPoint(OnArriveCarPathPoint);
			mTutorialMovePathPoint.ActiveRadius = 20f;
			SetPlayerCarMoveTarget();
			break;
		case 9:
			Singleton<ObjManager>.Instance.ClearAICar();
			StartCrashCar();
			break;
		}
	}

	private void SendServerFinishTutorial()
	{
		mIsTutorialFinish = true;
		NetLogic.GetInstance().Send<Protocol.tutorial_finish>();
		vp_Timer.In(2f, delegate
		{
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DownLoadTipRoot, delegate
				{
					SingletonUnity<DownloadTipRootLogic>.Instance.Reset();
				});
			}
		});
	}

	private void CreateShowNpc()
	{
		Transform transform = GameObject.Find("ShowNpcPos").transform;
		SoundManager instance = SingletonDontDestoryUnity<SoundManager>.Instance;
		Transform child = transform.GetChild(0);
		ObjInitNpcData objInitNpcData = new ObjInitNpcData();
		objInitNpcData.mServerID = UUID.GenUUID();
		objInitNpcData.mPos = child.position;
		objInitNpcData.mDir = child.forward;
		objInitNpcData.mCharacterModelId = (objInitNpcData.npcInfoData = DataManager.GetNpcDataByID("204")).Model;
		Singleton<ObjManager>.Instance.GetRagdollNPC(objInitNpcData, delegate(ObjNPC npc)
		{
			if (npc.AnimationLogic.AnimaObj != null)
			{
				vp_Timer.In(1f, delegate
				{
					npc.AnimationLogic.PlayAnimation("attack_1", delegate
					{
						npc.MoveTo(-17f, 0f, 134f);
						vp_Timer.In(1.5f, delegate
						{
							(npc as ObjRagdollNPC).RecycleSelf();
						});
					}, -1f);
				});
			}
			else
			{
				npc.onLoadMeshFinished = delegate(ObjNPC ObjNpc)
				{
					vp_Timer.In(1f, delegate
					{
						ObjNpc.AnimationLogic.PlayAnimation("attack_1", delegate
						{
							ObjNpc.MoveTo(-17f, 0f, 134f);
							vp_Timer.In(1.5f, delegate
							{
								(ObjNpc as ObjRagdollNPC).RecycleSelf();
							});
						}, -1f);
					});
				};
			}
		});
		Transform child2 = transform.GetChild(1);
		ObjInitNpcData objInitNpcData2 = new ObjInitNpcData();
		objInitNpcData2.mServerID = UUID.GenUUID();
		objInitNpcData2.mPos = child2.position;
		objInitNpcData2.mDir = child2.forward;
		objInitNpcData2.mCharacterModelId = (objInitNpcData2.npcInfoData = DataManager.GetNpcDataByID("204")).Model;
		Singleton<ObjManager>.Instance.GetRagdollNPC(objInitNpcData2, delegate(ObjNPC npc)
		{
			if (npc.AnimationLogic.AnimaObj != null)
			{
				vp_Timer.In(1.5f, delegate
				{
					npc.AnimationLogic.PlayAnimation("attack_1", delegate
					{
						npc.MoveTo(-17f, 0f, 134f);
						vp_Timer.In(1.5f, delegate
						{
							(npc as ObjRagdollNPC).RecycleSelf();
						});
					}, -1f);
				});
			}
			else
			{
				npc.onLoadMeshFinished = delegate(ObjNPC ObjNpc)
				{
					vp_Timer.In(1f, delegate
					{
						ObjNpc.AnimationLogic.PlayAnimation("attack_1", delegate
						{
							ObjNpc.MoveTo(-17f, 0f, 134f);
							vp_Timer.In(1.5f, delegate
							{
								(ObjNpc as ObjRagdollNPC).RecycleSelf();
							});
						}, -1f);
					});
				};
			}
		});
	}

	private void StartCrashCar()
	{
		UnityVersionUtil.SetActiveRecursive(mPlayerCar.gameObject, state: false);
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		if (mCrashCarSceneAnimaObj != null)
		{
			UnityVersionUtil.SetActiveRecursive(mCrashCarSceneAnimaObj, state: true);
			curAnimaCtl = mCrashCarSceneAnimaObj.GetComponent<SceneAnimationCtl>();
			curAnimaCtl.RegisterOnFinished(FinishTutorial);
			mCamCtl.CurCamera.enabled = false;
			UICamera.mainCamera.depth = 2f;
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SkipBtnRoot);
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "step", "15StartCrashCarAnima");
	}

	private void FinishTutorial()
	{
		UICamera.mainCamera.depth = 0f;
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SkipBtnRoot);
		string[] PageStr = new string[3]
		{
			StrDictionary.GetDictionaryString("#{90101}"),
			StrDictionary.GetDictionaryString("#{90102}"),
			StrDictionary.GetDictionaryString("#{90103}")
		};
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PrinterPageRoot, delegate
		{
			SingletonUnity<PrinterPageRootLogic>.Instance.Reset(PageStr, delegate
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DownLoadResRoot);
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "step", "17ShowDownloadPage");
			});
		});
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "step", "16ShowPrintPage");
	}

	private void StartRobCarState()
	{
		mMainPlayer.CurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
		mMainPlayer.DisactiveIdleAttack();
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		UnityVersionUtil.SetActiveRecursive(mCarAnimaObj.gameObject, state: false);
		if (mRobCarSceneAnimaObj != null)
		{
			UnityVersionUtil.SetActiveRecursive(mRobCarSceneAnimaObj, state: true);
			curAnimaCtl = mRobCarSceneAnimaObj.GetComponent<SceneAnimationCtl>();
			curAnimaCtl.RegisterOnFinished(onFinishedRobCarState);
			mCamCtl.CurCamera.enabled = false;
			UICamera.mainCamera.depth = 2f;
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SkipBtnRoot);
		}
		isFinishRobCarAnima = false;
	}

	private void onFinishedRobCarState()
	{
		UnityVersionUtil.SetActiveRecursive(mCarAnimaObj.gameObject, state: true);
		mCamCtl.CurCamera.enabled = true;
		UICamera.mainCamera.depth = 0f;
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SkipBtnRoot);
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		UnityVersionUtil.SetActiveRecursive(mSmoothFollowCamPos.gameObject, state: true);
		CreatePlayerCar();
		mPlayerCar.transform.position = mPlayerCarCreatePos;
		mSmoothFollowCamPos.UpdateToTargetPos();
		mMainPlayer.rigidbody.isKinematic = false;
		mMainPlayer.MoveTo(mPlayerCar.DummyPlayerPoint.position, 0.1f, delegate
		{
			RobCar();
		});
		isFinishRobCarAnima = true;
	}

	public void SetPlayerMoveTarget()
	{
		mTutorialMovePathPoint.transform.position = mPlayerPathPointRoot.GetChild(mCurPlayerPathIndex).position;
		mTutorialMovePathPoint.transform.localScale = new Vector3(1f, 0.5f, 1f);
		UnityVersionUtil.SetActiveRecursive(mTutorialMovePathPoint.gameObject, state: true);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CarTargetRoot, delegate
		{
			SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mTutorialMovePathPoint.gameObject, mMainPlayer.gameObject);
		});
		if (!IsNeedCheckMainPlayerActiveRange())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DragScreenHelpRoot, delegate
			{
				SingletonUnity<DragScreenHelpRoot>.Instance.SetTargetObj(mTutorialMovePathPoint.gameObject);
			});
		}
	}

	public void SetPlayerCarMoveTarget()
	{
		mTutorialMovePathPoint.transform.position = mPlayerCarPathList[mCurPlayerCarPathIndex];
		UnityVersionUtil.SetActiveRecursive(mTutorialMovePathPoint.gameObject, state: true);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CarTargetRoot, delegate
		{
			SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mTutorialMovePathPoint.gameObject, mPlayerCar.gameObject);
			if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
			{
				SingletonUnity<MiniMap>.Instance.SetTarget(mTutorialMovePathPoint.transform.position);
			}
		});
	}

	private void CreateNPCGroup()
	{
		mCurEnemyGroup++;
		NpcCreate(GetMonsterDataByGroup(mCurEnemyGroup));
	}

	private void NpcCreate(List<MonsterData> list)
	{
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				ObjInitNpcData objInitNpcData = new ObjInitNpcData();
				MonsterData monsterData = list[i];
				objInitNpcData.mServerID = UUID.GenUUID();
				objInitNpcData.mPos = new Vector3(monsterData.PositionX, 0f, monsterData.PositionZ);
				objInitNpcData.mDir = MathUtil.HeadingToVector3((float)monsterData.PosO / 100f);
				NpcData npcDataByID = DataManager.GetNpcDataByID(monsterData.NpcID);
				objInitNpcData.npcInfoData = npcDataByID;
				objInitNpcData.MaxHP = objInitNpcData.npcInfoData.Hp;
				objInitNpcData.HP = objInitNpcData.npcInfoData.Hp;
				objInitNpcData.ATK = objInitNpcData.npcInfoData.Atk;
				objInitNpcData.DEF = objInitNpcData.npcInfoData.Def;
				objInitNpcData.HIT = objInitNpcData.npcInfoData.HIT;
				objInitNpcData.EVA = objInitNpcData.npcInfoData.DGE;
				objInitNpcData.CRI = objInitNpcData.npcInfoData.CRI;
				objInitNpcData.EXD = objInitNpcData.npcInfoData.EXD;
				objInitNpcData.EXR = objInitNpcData.npcInfoData.EXR;
				objInitNpcData.RES = objInitNpcData.npcInfoData.RES;
				objInitNpcData.CRD = objInitNpcData.npcInfoData.CRD;
				objInitNpcData.CRR = objInitNpcData.npcInfoData.CRR;
				objInitNpcData.DEFA = objInitNpcData.npcInfoData.DEFA;
				objInitNpcData.Level = objInitNpcData.npcInfoData.Lv;
				objInitNpcData.AntiStun = objInitNpcData.npcInfoData.AntiStun;
				objInitNpcData.AntiKnockDown = objInitNpcData.npcInfoData.AntiKnockDown;
				Singleton<ObjManager>.Instance.CreateNPC(objInitNpcData, OnNPCCreated);
			}
		}
	}

	private void OnNPCCreated(ObjNPC npc)
	{
		if (npc.NPCData.AI.Equals("BlockAI"))
		{
			staticNpc = npc.gameObject;
			CapsuleCollider component = staticNpc.GetComponent<CapsuleCollider>();
			if (component != null)
			{
				component.radius = 0.8f;
				component.height = 2.5f;
			}
		}
		else
		{
			npc.ChangeBornPos(Singleton<ObjManager>.Instance.MainPlayer.Position);
			npc.MoveTo(Singleton<ObjManager>.Instance.MainPlayer.Position);
		}
	}

	public override void OnNPCDie(object objNpc)
	{
		ObjRagdollNPC npc = objNpc as ObjRagdollNPC;
		npc.recycleHandle.Cancel();
		vp_Timer.In(2f, delegate
		{
			npc.RecycleSelf();
		}, npc.recycleHandle);
		local_npc_die.request request = new local_npc_die.request();
		request.npcid = npc.NPCDataID;
		request.x = (long)(npc.Position.x * 100f);
		request.z = (long)(npc.Position.z * 100f);
		request.type = 0L;
		if (npc.IsRagdollEnable && SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.IMPACT_NPC))
		{
			request.type = 3L;
		}
		NetLogic.GetInstance().Send<Protocol.local_npc_die>(request);
		if (SingletonUnity<CitySimController>.Exists)
		{
			SingletonUnity<CitySimController>.Instance.OnNPCDie(npc);
		}
	}

	public override void LeaveScene()
	{
		MessageBoxLogic.OpenOKCancelBox(mapInfoData.MExitCon, StrDictionary.GetDictionaryString("#{100127}"), delegate
		{
			NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
			Singleton<ObjManager>.Instance.StopAllPoliceSound();
		});
	}

	public void ResetPlayerCarToLastPoint()
	{
		mPlayerCar.rigidbody.velocity = Vector3.zero;
		mPlayerCar.rigidbody.angularVelocity = Vector3.zero;
		if (mCurPlayerCarPathIndex == 0)
		{
			mPlayerCar.transform.position = mPlayerCarCreatePos;
			mPlayerCar.transform.eulerAngles = mPlayerCarCreateAngle;
		}
		else
		{
			mPlayerCar.transform.position = mPlayerCarPathList[mCurPlayerCarPathIndex - 1];
			mPlayerCar.transform.forward = (mPlayerCarPathList[mCurPlayerCarPathIndex] - mPlayerCarPathList[mCurPlayerCarPathIndex - 1]).normalized;
		}
	}

	public bool IsNeedCheckMainPlayerActiveRange()
	{
		return mStepIndex >= 6;
	}

	public override void OnReconnectSuccess()
	{
		base.OnReconnectSuccess();
		if (mStepIndex == ROBBING_CAR_STATE && isFinishRobCarAnima)
		{
			mMainPlayer.MoveTo(mPlayerCar.DummyPlayerPoint.position, 0.1f, delegate
			{
				RobCar();
			});
		}
	}

	private bool IsNeedUpdateProgressLine()
	{
		if (mStepIndex == 3 || mStepIndex == 4)
		{
			return true;
		}
		return false;
	}

	private bool IsNeedCheckDownloadProgress()
	{
		if (mStepIndex == 5)
		{
			return true;
		}
		return false;
	}

	public override void SetMoveTarget(Vector3 pos, string missionId)
	{
		if (mMainPlayer == null)
		{
			mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			if (mMainPlayer == null)
			{
				return;
			}
		}
		if (Vector3.Distance(mMainPlayer.Position, pos) < 10f)
		{
			return;
		}
		base.SetMoveTarget(pos, missionId);
		curMoveTargetMissionId = missionId;
		IsHaveMoveTarget = true;
		CurMoveTarget = pos;
		if (mMoveTargetPathPoint == null)
		{
			GameObject gameObject = ResourcesManager.LoadAndInstantiate("Items/MovePathPoint") as GameObject;
			mMoveTargetPathPoint = gameObject.GetComponent<MovePathPoint>();
			mMoveTargetPathPoint.RegisterOnArrivePathPoint(OnPlayerArriveTargetPoint);
			mMoveTargetPathPoint.ActiveRadius = 8f;
		}
		mMoveTargetPathPoint.transform.position = new Vector3(pos.x, SceneManager.GetHitHeight(pos), pos.z);
		UnityVersionUtil.SetActiveRecursive(mMoveTargetPathPoint.gameObject, state: true);
		UpdateMapLine();
		if (!SingletonUnity<CarTargetUIRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<CarTargetUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CarTargetRoot, delegate
			{
				SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mMoveTargetPathPoint.gameObject, mMainPlayer.gameObject);
			});
		}
		else
		{
			SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mMoveTargetPathPoint.gameObject, mMainPlayer.gameObject);
		}
		if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
		{
			SingletonUnity<MiniMap>.Instance.SetTarget(pos);
		}
		if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.UpdateMoveTargetPic();
		}
	}

	private void UpdateMapLine()
	{
		if (IsHaveMoveTarget && SingletonUnity<CitySimController>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CitySimController>.Instance.gameObject))
		{
			SingletonUnity<CitySimController>.Instance.UpdateMapLine(mMainPlayer.Position, CurMoveTarget);
		}
	}

	private void ClearMapLine()
	{
		if (SingletonUnity<CitySimController>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CitySimController>.Instance.gameObject))
		{
			SingletonUnity<CitySimController>.Instance.ClearMapLine();
		}
	}

	public override void ClearMoveTarget()
	{
		OnPlayerArriveTargetPoint(Vector3.zero);
	}

	public void OnPlayerArriveTargetPoint(Vector3 pos)
	{
		base.ClearMoveTarget();
		ClearMapLine();
		if (mMoveTargetPathPoint != null)
		{
			UnityVersionUtil.SetActiveRecursive(mMoveTargetPathPoint.gameObject, state: false);
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CarTargetRoot);
		if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
		{
			SingletonUnity<MiniMap>.Instance.HideTarget();
		}
		if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.UpdateMoveTargetPic();
		}
	}

	public override void MissionCheckMoveTarget(string missionId)
	{
		if (!string.IsNullOrEmpty(curMoveTargetMissionId) && curMoveTargetMissionId.Equals(missionId))
		{
			OnPlayerArriveTargetPoint(Vector3.zero);
		}
	}
}
