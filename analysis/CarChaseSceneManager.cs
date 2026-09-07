using System.Collections.Generic;
using DG.Tweening;
using SprotoType;
using UnityEngine;

public class CarChaseSceneManager : SceneManager
{
	private SmoothFollowNew mSmoothFollowCamPos;

	private Vector3 mMainPlayerRunTargetPos;

	private Vector3 mPlayerCarCreatePos;

	private Vector3 mPlayerCarCreateAngle;

	private ObjMainPlayer mMainPlayer;

	private ObjPlayerCar mPlayerCar;

	private MovePathPoint mMovePathPoint;

	private List<Vector3> mPlayerPathList = new List<Vector3>();

	private int mCurPlayerPathIndex;

	private float MISSION_TIME = 180f;

	private float mMissionTimeCount;

	private CopySceneData mCurCopySceneData;

	private bool mStartTimeCountFlag;

	private int mBestTime;

	private int[] mPathPointBestTime;

	private int[] mCurPathPointTime;

	private float mMissionStartTime;

	private Dictionary<string, GameObject> mStaticBlockDic = new Dictionary<string, GameObject>();

	private Dictionary<int, MoveCarBlock> mMoveCarBlockDic = new Dictionary<int, MoveCarBlock>();

	private float FreshNPCTimeCount;

	private float FreshNPCTime = 2f;

	private float PlayerBolckCheckTime = 5f;

	private float playerBlockTimeCount;

	private Vector3 prePos;

	private bool LerpToCarFlag;

	private Vector3 startPosition;

	private Quaternion startRotation;

	private float lerpTime = 1f;

	private float lerpCountTime;

	private bool mEndMissionFlag;

	public override void Init(string id)
	{
		base.Init(id);
		mCurCopySceneData = DataManager.GetCopySceneDataById(mapInfoData.ID);
		if (mCurCopySceneData != null)
		{
			MISSION_TIME = mCurCopySceneData.ExistTime;
			mMissionTimeCount = MISSION_TIME;
		}
		mStartTimeCountFlag = false;
		mEndMissionFlag = false;
		GameObject gameObject = GameObject.Find("SmoothFollowCamPos");
		if (gameObject != null)
		{
			mSmoothFollowCamPos = gameObject.GetComponent<SmoothFollowNew>();
		}
		gameObject = ResourcesManager.LoadAndInstantiate("Items/MovePathPoint") as GameObject;
		mMovePathPoint = gameObject.GetComponent<MovePathPoint>();
		mMovePathPoint.ActiveRadius = 15f;
		mMovePathPoint.RegisterOnArrivePathPoint(OnArrivePathPoint);
		SingletonUnity<MyEvent>.Instance.Register("OnMainPlayerCreate", this, "OnMainPlayerCreate");
		InitBlock(id);
		StrikePool = new SimplePool<ParticleSystem>();
		StrikePool.Reset(base.CreateStrike, base.DestroyStrike, 10);
		base.IsMissionStart = false;
		copyscene_info copyscene_info = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.DailyCopyInfoDic[mCurCopySceneData.ID];
		if (copyscene_info.HasBestGrade)
		{
			mBestTime = (int)copyscene_info.BestGrade;
		}
		else
		{
			mBestTime = -1;
		}
		if (copyscene_info.HasStr)
		{
			string[] array = copyscene_info.str.Split('#');
			mPathPointBestTime = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				mPathPointBestTime[i] = int.Parse(array[i]);
			}
		}
		mCurPathPointTime = new int[mPlayerPathList.Count];
		GameObject go = GameObject.Find("FB_saiDao_1/dibiao_100+/FB_saiDao_dangBan_GTA");
		UnityVersionUtil.SetActiveRecursive(go, state: false);
		ResourcesManager.LoadAndInstantiate("Tutorial/CitySimController");
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
					gameObject7 = Object.Instantiate(mStaticBlockDic[carMissionBlockDataListBySceneId[i].ObstacleName]) as GameObject;
				}
				else
				{
					gameObject7 = ResourcesManager.LoadAndInstantiate($"CarMissionBlock/{carMissionBlockDataListBySceneId[i].ObstacleName}") as GameObject;
					if (gameObject7 == null)
					{
						continue;
					}
					mStaticBlockDic.Add(carMissionBlockDataListBySceneId[i].ObstacleName, gameObject7);
				}
				if (!(gameObject7 == null))
				{
					gameObject7.transform.parent = gameObject2.transform;
					gameObject7.transform.position = new Vector3(carMissionBlockDataListBySceneId[i].PosX, carMissionBlockDataListBySceneId[i].PosY, carMissionBlockDataListBySceneId[i].PosZ);
					gameObject7.transform.eulerAngles = new Vector3(carMissionBlockDataListBySceneId[i].AngleX, carMissionBlockDataListBySceneId[i].AngleY, carMissionBlockDataListBySceneId[i].AngleZ);
				}
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
		mCurPlayerPathIndex = 0;
		for (int j = 0; j < list.Count; j++)
		{
			mPlayerPathList.Add(list[j].Value);
		}
		mMovePathPoint.transform.position = mPlayerPathList[0];
	}

	public override void StartGame()
	{
		base.IsMissionStart = true;
		ChangeToCarCtl();
		NetLogic.GetInstance().Send<Protocol.start_battle>();
		mMissionStartTime = Time.time;
		SingletonUnity<CarBestTimeCountRoot>.Instance.EnableTimeCount(mMissionStartTime);
	}

	private void OnArrivePathPoint(Vector3 pos)
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CarPointBestTimeRoot, delegate
		{
			int num = -1;
			num = ((mPathPointBestTime != null && mPathPointBestTime.Length > mCurPlayerPathIndex) ? mPathPointBestTime[mCurPlayerPathIndex] : (-1));
			SingletonUnity<CarPointBestTimeRoot>.Instance.Reset(GetCurMissionTime(), num);
		});
		mCurPathPointTime[mCurPlayerPathIndex] = GetCurMissionTime();
		mCurPlayerPathIndex++;
		if (mCurPlayerPathIndex < mPlayerPathList.Count)
		{
			mMovePathPoint.transform.position = mPlayerPathList[mCurPlayerPathIndex];
			UnityVersionUtil.SetActiveRecursive(mMovePathPoint.gameObject, state: true);
			SingletonUnity<MiniMap>.Instance.SetTarget(mPlayerPathList[mCurPlayerPathIndex]);
			SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mMovePathPoint.gameObject, mPlayerCar.gameObject);
		}
		else
		{
			SuccessMission();
		}
	}

	public override void OnLoadingOver()
	{
		base.OnLoadingOver();
		vp_Timer.In(0.5f, delegate
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CarMissionStartTimeCountRoot, delegate
			{
				SingletonUnity<CarMissionStartTimeCountRoot>.Instance.Reset(Time.time);
			});
		});
		CameraController cameraController = mMainPlayer.CameraController;
		cameraController.enabled = false;
		cameraController.transform.position = mPlayerCar.StartCamView.transform.position;
		cameraController.transform.rotation = mPlayerCar.StartCamView.transform.rotation;
		cameraController.LerpToTargetLocalZero(mSmoothFollowCamPos.transform, 1f);
		mPlayerCar.EnableCar(mMainPlayer);
		mPlayerCar.FreezeCar();
	}

	public override void SuccessMission()
	{
		base.SuccessMission();
		EndMission(isSuccess: true);
	}

	public override void FailMission()
	{
		base.FailMission();
		EndMission(isSuccess: false);
	}

	public override void Update()
	{
		if (SingletonUnity<CitySimController>.Exists)
		{
			SingletonUnity<CitySimController>.Instance.OnlyUpdateNpc();
		}
	}

	private void CheckPlayerBlock()
	{
		if (Vector3.SqrMagnitude(prePos - mPlayerCar.Position) < 4f)
		{
			EndMission(isSuccess: false);
		}
		prePos = mPlayerCar.Position;
	}

	private void FreshNPC()
	{
		if (SingletonUnity<EasyCitySimController>.Exists)
		{
			SingletonUnity<EasyCitySimController>.Instance.FreshNPC(mMainPlayer.CacheTransform);
		}
	}

	private void RobCar()
	{
		mMainPlayer.rigidbody.isKinematic = true;
		mMainPlayer.MoveTo(mPlayerCar.DummyPlayerPoint.position, 1f, delegate
		{
			SingletonUnity<UIManager>.Instance.HideBaseUI();
			mMainPlayer.CameraController.LerpToTargetLocalZero(mPlayerCar.CarDoorView, 1f);
			mMainPlayer.DisableNavMeshAgent();
			mMainPlayer.CacheTransform.parent = mPlayerCar.DummyPlayerPoint;
			mMainPlayer.CacheTransform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
			mMainPlayer.CacheTransform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
			vp_Timer.In(0.1f, delegate
			{
				mPlayerCar.MeshRoot.animation.Play("RobCar");
				mMainPlayer.AnimationLogic.PlayAnimation("RobCar", delegate
				{
					mMainPlayer.DisableMainPlayer();
					ChangeToCarCtl();
				}, -1f);
				ObjInitNpcData objInitNpcData = new ObjInitNpcData
				{
					mServerID = UUID.GenUUID(),
					mPos = mPlayerCar.DummyNPCPoint.position
				};
				NpcData npcDataByID = DataManager.GetNpcDataByID("201");
				objInitNpcData.HP = npcDataByID.Hp;
				objInitNpcData.MaxHP = npcDataByID.Hp;
				objInitNpcData.npcInfoData = npcDataByID;
				Singleton<ObjManager>.Instance.CreateNPC(objInitNpcData, delegate(ObjNPC npc)
				{
					npc.CacheTransform.parent = mPlayerCar.DummyNPCPoint;
					npc.CacheTransform.localPosition = Vector3.zero;
					npc.CacheTransform.localRotation = Quaternion.identity;
				}, delegate(ObjNPC npc)
				{
					npc.AnimationLogic.PlayAnimation("RobOffCar", delegate
					{
						npc.CacheTransform.parent = null;
					}, -1f);
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
			ChangeToPlayerCtl();
		}, -1f);
	}

	private void ChangeToPlayerCtl()
	{
		mMainPlayer.transform.parent = null;
		mMainPlayer.EnableNavMeshAgent();
		mMainPlayer.rigidbody.isKinematic = false;
		UIManager uiManager = SingletonUnity<UIManager>.Instance;
		CameraController camCtl = mMainPlayer.CameraController;
		vp_Timer.In(0.1f, delegate
		{
			camCtl.LerpBackToPlayer(1f, delegate
			{
				uiManager.ReShowBaseUI();
				uiManager.CloseUI(UIInfo.CarControllerRoot);
				uiManager.ShowUI(UIInfo.YiDongKongZhiUI);
				uiManager.ShowUI(UIInfo.JueseJiNengQuUI);
				uiManager.ShowUI(UIInfo.TouXiangKuangUI, delegate(bool isSuccess, object param)
				{
					if (isSuccess)
					{
						SingletonUnity<TouXiangKuangLogic>.Instance.Init();
					}
				});
			});
		});
	}

	private void ChangeToCarCtl()
	{
		UIManager instance = SingletonUnity<UIManager>.Instance;
		instance.ReShowBaseUI();
		instance.CloseUI(UIInfo.YiDongKongZhiUI);
		instance.CloseUI(UIInfo.JueseJiNengQuUI);
		instance.CloseUI(UIInfo.TouXiangKuangUI);
		instance.ShowCarDefaultUI();
		instance.ShowUI(UIInfo.CarBestTimeCountRoot, delegate
		{
			SingletonUnity<CarBestTimeCountRoot>.Instance.Reset(mBestTime);
		});
		instance.ShowUI(UIInfo.CarTargetRoot, delegate
		{
			SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mMovePathPoint.gameObject, mPlayerCar.gameObject);
		});
		SingletonUnity<MiniMap>.Instance.SetTarget(mPlayerPathList[0]);
		mPlayerCar.DisFreezeCar();
		mStartTimeCountFlag = true;
	}

	private void CreatePlayerCar()
	{
		MountData mountDataById = DataManager.GetMountDataById(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MountId);
		ObjCarInitData initData = new ObjCarInitData(mPlayerCarCreatePos, mPlayerCarCreateAngle, UUID.GenUUID(), string.Empty, policeFlag: false, mountDataById);
		Singleton<ObjManager>.Instance.CreateMainPlayerCar(initData);
		mPlayerCar = Singleton<ObjManager>.Instance.MainPlayerCar;
		mSmoothFollowCamPos.SetTarget(mPlayerCar.CarControl);
	}

	public void OnPlayerCarMeshLoadDone()
	{
		GameManager.IsSceneReady = true;
	}

	public void OnMainPlayerCreate()
	{
		SingletonUnity<MyEvent>.Instance.DeRegister("OnMainPlayerCreate", this, "OnMainPlayerCreate");
		ObjManager instance = Singleton<ObjManager>.Instance;
		mMainPlayer = instance.MainPlayer;
		mMainPlayer.CurAnimationState = GameDefine.ANIMATIONSTATE.DRIVING;
		mMainPlayer.CurPlayerState = PLAYER_STATE.DRIVING;
		mMainPlayer.rigidbody.isKinematic = true;
		GameManager.IsSceneReady = false;
		CreatePlayerCar();
	}

	private void EndMission(bool isSuccess)
	{
		if (mEndMissionFlag)
		{
			return;
		}
		Singleton<ObjManager>.Instance.StopAllPoliceSound();
		mEndMissionFlag = true;
		int curMissionTime = GetCurMissionTime();
		copyscene_info copyscene_info = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.DailyCopyInfoDic[mCurCopySceneData.ID];
		string text = $"{mCurPathPointTime[0]}";
		for (int i = 1; i < mCurPathPointTime.Length; i++)
		{
			text = $"{text}#{mCurPathPointTime[i]}";
		}
		if (copyscene_info.HasBestGrade)
		{
			if (copyscene_info.BestGrade > curMissionTime)
			{
				copyscene_info.BestGrade = curMissionTime;
				copyscene_info.str = text;
			}
		}
		else
		{
			copyscene_info.BestGrade = curMissionTime;
			copyscene_info.str = text;
		}
		car_chase_result.request request = new car_chase_result.request();
		request.state = isSuccess;
		request.param1 = curMissionTime;
		request.param2 = text;
		NetLogic.GetInstance().Send<Protocol.car_chase_result>(request);
		mPlayerCar.StopCar();
		CameraController cameraController = mMainPlayer.CameraController;
		cameraController.LerpToTargetLocalZero(mPlayerCar.WinCamView, 1f, delegate
		{
			mPlayerCar.WinCamView.animation.Play();
		});
		SingletonUnity<UIManager>.Instance.HideBaseUI();
	}

	public int GetCurMissionTime()
	{
		return (int)((Time.time - mMissionStartTime) * 100f);
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
		if (mCurPlayerPathIndex == 0)
		{
			mPlayerCar.transform.position = mPlayerCarCreatePos;
			mPlayerCar.transform.eulerAngles = mPlayerCarCreateAngle;
		}
		else
		{
			mPlayerCar.transform.position = mPlayerPathList[mCurPlayerPathIndex - 1];
			mPlayerCar.transform.forward = (mPlayerPathList[mCurPlayerPathIndex] - mPlayerPathList[mCurPlayerPathIndex - 1]).normalized;
		}
	}
}
