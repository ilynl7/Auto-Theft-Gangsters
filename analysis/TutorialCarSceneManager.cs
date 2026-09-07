using System;
using System.Collections.Generic;
using DG.Tweening;
using SprotoType;
using UnityEngine;

public class TutorialCarSceneManager : SceneManager
{
	private string[] PageStr = new string[3] { "My brother is a detective at FBI. At the age of 13, our parents were involved in the terrorist attacks, my brother vowed to punish the wicked. In my eyes, becoming FBI's brother is no doubt a great superhero.\n10 years later, as usual, I was preparing for work, when suddenly the black man who knocked on the door told me the news that had changed my life......", "\"Your brother, the mob was in the act of killing, has been confirmed.\"\nThe irony of life, my last relatives, my super hero, also died in the hands of the terrorist.After that, I found a hidden note from brother's room, and I knew the case that my brother had been tracking was a shooting and murder in Vice City. Even more so, brother shows that the case is likely to belong to the terrorist that killed its parents 10 years ago.", "In order to find out the truth, I quit my job as a journalist. Bought some light equipment through the familiar arms dealer. Pack up, came to Vice City." };

	private SmoothFollowNew mSmoothFollowCamPos;

	private Vector3 mMainPlayerRunTargetPos;

	private Vector3 mPlayerCarCreatePos;

	private Vector3 mPlayerCarCreateAngle;

	private ObjMainPlayer mMainPlayer;

	private ObjPlayerCar mPlayerCar;

	private MovePathPoint mMovePathPoint;

	private List<Vector3> mPlayerCarPathList = new List<Vector3>();

	private int mCurPlayerCarPathIndex;

	private CopySceneData mCurCopySceneData;

	private CarHPRootLogic carHPRoot;

	private Transform mPlayerPathPointRoot;

	private int mWaveCount;

	private int mStepIndex;

	private int mCurEnemyGroup;

	private int mCurPlayerPathIndex;

	private GameObject mRandomWalkPosRoot;

	private List<Vector3> mRandomWalkPosList;

	private List<ObjRagdollNPC> mWalkNpcList = new List<ObjRagdollNPC>();

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
		GameObject gameObject = GameObject.Find("SmoothFollowCamPos");
		if (gameObject != null)
		{
			mSmoothFollowCamPos = gameObject.GetComponent<SmoothFollowNew>();
		}
		UnityVersionUtil.SetActiveRecursive(mSmoothFollowCamPos.gameObject, state: false);
		gameObject = GameObject.Find("MovePathPoint");
		mMovePathPoint = gameObject.GetComponent<MovePathPoint>();
		mMovePathPoint.RegisterOnArrivePathPoint(OnPlayerArrivePathPoint);
		mWaveCount = GetMonsterGroupCount();
		mStepIndex = 0;
		mCurEnemyGroup = 0;
		mCurPlayerPathIndex = 1;
		if (SingletonUnity<MyEvent>.Exists)
		{
			SingletonUnity<MyEvent>.Instance.Register("OnMainPlayerCreate", this, "OnMainPlayerCreate");
		}
		else
		{
			Debug.Log("SingletonUnity<MyEvent>.Exists == false");
		}
		UnityVersionUtil.SetActiveRecursive(mMovePathPoint.gameObject, state: false);
		mMovePathPoint.ActiveRadius = 1f;
		ResourcesManager.LoadAndInstantiate("Tutorial/TutorialStaticMesh");
		mEndMissionFlag = false;
		InitBlock(id);
		StrikePool = new SimplePool<ParticleSystem>();
		StrikePool.Reset(base.CreateStrike, base.DestroyStrike, 10);
		mRandomWalkPosRoot = GameObject.Find("TutorialWalkNPCPos");
		mRandomWalkPosList = new List<Vector3>();
		for (int i = 0; i < mRandomWalkPosRoot.transform.childCount; i++)
		{
			mRandomWalkPosList.Add(mRandomWalkPosRoot.transform.GetChild(i).position);
		}
		for (int j = 0; j < mRandomWalkPosList.Count; j++)
		{
			ObjInitNpcData objInitNpcData = new ObjInitNpcData();
			objInitNpcData.mServerID = UUID.GenUUID();
			objInitNpcData.mPos = mRandomWalkPosList[j];
			NpcData npcDataByID = DataManager.GetNpcDataByID("301");
			objInitNpcData.HP = npcDataByID.Hp;
			objInitNpcData.MaxHP = npcDataByID.Hp;
			objInitNpcData.npcInfoData = npcDataByID;
			objInitNpcData.mCharacterModelId = npcDataByID.Model;
			Singleton<ObjManager>.Instance.GetRagdollNPC(objInitNpcData, delegate(ObjNPC npc)
			{
				if (npc != null)
				{
					mWalkNpcList.Add(npc as ObjRagdollNPC);
					npc.WalkMoveTo(mRandomWalkPosList[UnityEngine.Random.Range(0, mRandomWalkPosList.Count)], UnityEngine.Random.Range(1, 4), OnNPCArriveMoveTarget);
				}
			});
		}
		GameObject gameObject2 = GameObject.Find("TutorialPlayerPathRoot");
		mPlayerPathPointRoot = gameObject2.transform;
	}

	private void OnNPCArriveMoveTarget(ObjCharacter objCha)
	{
		objCha.WalkMoveTo(mRandomWalkPosList[UnityEngine.Random.Range(0, mRandomWalkPosList.Count)], UnityEngine.Random.Range(1, 4), OnNPCArriveMoveTarget);
	}

	private void ClearWalkNpc()
	{
		for (int num = mWalkNpcList.Count - 1; num >= 0; num--)
		{
			mWalkNpcList[num].RecycleSelf();
		}
		mWalkNpcList.Clear();
	}

	private void InitBlock(string sceneId)
	{
		Debug.Log("sceneId :: " + sceneId);
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
					boxCollider.size = new Vector3(20f, 5f, 1f);
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
			MoveNextState();
		}
	}

	public override void SuccessMission()
	{
		EndMission(isSuccess: true);
	}

	public override void FailMission()
	{
		EndMission(isSuccess: false);
	}

	public override void Update()
	{
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
					Debug.Log("PlayAnimation Finish");
					mMainPlayer.DisableMainPlayer();
					MoveNextState();
					ChangeToCarCtl();
				}, -1f);
				ObjInitNpcData objInitNpcData = new ObjInitNpcData
				{
					mServerID = UUID.GenUUID(),
					mPos = mPlayerCar.DummyNPCPoint.position
				};
				NpcData npcDataByID = DataManager.GetNpcDataByID("1001");
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
			for (int i = 0; i < mMainPlayer.PartObject.Length; i++)
			{
				if (mMainPlayer.PartObject[i] != null)
				{
					mMainPlayer.PartObject[i].layer = LayerMask.NameToLayer("ShadowCaster");
				}
			}
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
		instance.ShowUI(UIInfo.CarTargetRoot, delegate
		{
			SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mMovePathPoint.gameObject, mPlayerCar.gameObject);
		});
		MapInfoData mapInfo = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData;
		if (!string.IsNullOrEmpty(mapInfo.MiniMapName))
		{
			instance.ShowUI(UIInfo.MiniMapRoot, delegate
			{
				SingletonUnity<MiniMap>.Instance.Reset(mapInfo.fMapLength, mapInfo.fMapHeight, 200f, mapInfo.MiniMapName, mapInfo.Name);
				SingletonUnity<MiniMap>.Instance.SetTarget(mPlayerCarPathList[0]);
			});
		}
		CameraController cameraController = mMainPlayer.CameraController;
		cameraController.LerpToTargetLocalZero(mSmoothFollowCamPos.transform, 1f, delegate
		{
			mPlayerCar.EnableCar(mMainPlayer);
		});
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
		mMainPlayer.Position = mPlayerPathPointRoot.GetChild(0).position;
		mMainPlayer.FaceToPub(mMainPlayer.Position + mPlayerPathPointRoot.GetChild(0).forward);
		GameObject gameObject = GameObject.Find("TutorialStartCamMove");
		CameraController cam = mMainPlayer.CameraController;
		cam.Init(CameraController.CAMERAVIEWSTATE.FIXED);
		cam.enabled = false;
		cam.transform.parent = gameObject.transform;
		cam.transform.localPosition = Vector3.zero;
		cam.transform.localRotation = Quaternion.identity;
		gameObject.animation.Play();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PrinterPageRoot, delegate
		{
			SingletonUnity<PrinterPageRootLogic>.Instance.Reset(PageStr, delegate
			{
				cam.LerpBackToPlayer(0f);
				StoryDialogRootLogic.ShowStory("101", null);
				UIUpdateEvent.OnStoryShowOver = (UIUpdateEvent.OnStoryShowOverDelegate)Delegate.Combine(UIUpdateEvent.OnStoryShowOver, new UIUpdateEvent.OnStoryShowOverDelegate(OnStoryShowOver));
			});
		});
	}

	private void OnStoryShowOver(string storyId)
	{
		UIUpdateEvent.OnStoryShowOver = (UIUpdateEvent.OnStoryShowOverDelegate)Delegate.Remove(UIUpdateEvent.OnStoryShowOver, new UIUpdateEvent.OnStoryShowOverDelegate(OnStoryShowOver));
		MoveNextState();
	}

	private void EndMission(bool isSuccess)
	{
		if (!mEndMissionFlag)
		{
			mEndMissionFlag = true;
			GetOffCar();
		}
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
		UnityVersionUtil.SetActiveRecursive(mMovePathPoint.gameObject, state: false);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CarTargetRoot);
		MoveNextState();
		mCurPlayerPathIndex++;
	}

	public void MoveNextState()
	{
		mStepIndex++;
		switch (mStepIndex)
		{
		case 1:
			SingletonUnity<UIManager>.Instance.ShowTutorialDefaultUI();
			TutorialManager.ShowTutorial(TUTORIAL_STEP.SWITCH_VIEW);
			break;
		case 2:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SelectTargetUI);
			ClearWalkNpc();
			SingletonUnity<JueseJiNengQuLogic>.Instance.ShowSkillBtn(0);
			TutorialManager.MoveNext();
			CreateNPCGroup();
			break;
		case 3:
			SingletonUnity<JueseJiNengQuLogic>.Instance.ShowAllSkillBtn();
			TutorialManager.ShowTutorial(TUTORIAL_STEP.SKILL1_BUTTON);
			CreateNPCGroup();
			break;
		case 4:
			SetPlayerMoveTarget();
			break;
		case 5:
			StartRobCarState();
			break;
		case 6:
			mMovePathPoint.DeRegisterOnArrivePathPoint(OnPlayerArrivePathPoint);
			mMovePathPoint.RegisterOnArrivePathPoint(OnArriveCarPathPoint);
			mMovePathPoint.ActiveRadius = 15f;
			SetPlayerCarMoveTarget();
			break;
		case 7:
			StartCrashCar();
			break;
		}
	}

	private void StartCrashCar()
	{
		UnityVersionUtil.SetActiveRecursive(mPlayerCar.gameObject, state: false);
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		string path = $"Tutorial/CrashCarAnima";
		GameObject gameObject = ResourcesManager.LoadAndInstantiate(path) as GameObject;
		SceneAnimationCtl component = gameObject.GetComponent<SceneAnimationCtl>();
		component.RegisterOnFinished(FinishTutorial);
	}

	private void FinishTutorial()
	{
		vp_Timer.In(5f, delegate
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
			enter_new_map.request rpcReq = new enter_new_map.request
			{
				mapInfoId = 101.ToString()
			};
			NetLogic.GetInstance().Send<Protocol.enter_new_map>(rpcReq);
		});
	}

	private void StartRobCarState()
	{
		mMainPlayer.DisactiveIdleAttack();
		mMainPlayer.DisableMainPlayer();
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		string empty = string.Empty;
		empty = ((SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession == PROFESSION_TYPE.XD) ? $"Tutorial/XD_RobCarSceneAnima" : ((SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession != PROFESSION_TYPE.QJ) ? $"Tutorial/XD_RobCarSceneAnima" : $"Tutorial/XD_RobCarSceneAnima"));
		GameObject gameObject = ResourcesManager.LoadAndInstantiate(empty) as GameObject;
		SceneAnimationCtl component = gameObject.GetComponent<SceneAnimationCtl>();
		component.RegisterOnFinished(onFinishedRobCarState);
	}

	private void onFinishedRobCarState()
	{
		UnityVersionUtil.SetActiveRecursive(mSmoothFollowCamPos.gameObject, state: true);
		CreatePlayerCar();
		mPlayerCar.transform.position = new Vector3(-36f, 0f, 174f);
		mSmoothFollowCamPos.UpdateToTargetPos();
		mMainPlayer.Position = new Vector3(-33f, 0f, 170f);
		mMainPlayer.EnableMainPlayer();
		mMainPlayer.EnableNavMeshAgent();
		mMainPlayer.MoveTo(mPlayerCar.DummyPlayerPoint.position, 0.1f, delegate
		{
			RobCar();
		});
	}

	public void SetPlayerMoveTarget()
	{
		mMovePathPoint.transform.position = mPlayerPathPointRoot.GetChild(mCurPlayerPathIndex).position;
		UnityVersionUtil.SetActiveRecursive(mMovePathPoint.gameObject, state: true);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CarTargetRoot, delegate
		{
			SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mMovePathPoint.gameObject, mMainPlayer.gameObject);
		});
	}

	public void SetPlayerCarMoveTarget()
	{
		mMovePathPoint.transform.position = mPlayerCarPathList[mCurPlayerCarPathIndex];
		UnityVersionUtil.SetActiveRecursive(mMovePathPoint.gameObject, state: true);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CarTargetRoot, delegate
		{
			SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mMovePathPoint.gameObject, mPlayerCar.gameObject);
		});
	}

	private void CreateNPCGroup()
	{
		mCurEnemyGroup++;
		NpcCreate(GetMonsterDataByGroup(mCurEnemyGroup));
	}

	private void NpcCreate(List<MonsterData> list)
	{
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			ObjInitNpcData objInitNpcData = new ObjInitNpcData();
			MonsterData monsterData = list[i];
			objInitNpcData.mServerID = UUID.GenUUID();
			objInitNpcData.mPos = new Vector3(monsterData.PositionX, 0f, monsterData.PositionZ);
			NpcData npcData = (objInitNpcData.npcInfoData = DataManager.GetNpcDataByID(monsterData.NpcID));
			objInitNpcData.HP = npcData.Hp;
			objInitNpcData.MaxHP = npcData.Hp;
			if (npcData.AI.Equals("BlockAI"))
			{
				Singleton<ObjManager>.Instance.CreateNPC(objInitNpcData);
			}
			else
			{
				Singleton<ObjManager>.Instance.CreateNPC(objInitNpcData, OnNPCCreated);
			}
		}
	}

	private void OnNPCCreated(ObjNPC npc)
	{
		npc.ChangeBornPos(Singleton<ObjManager>.Instance.MainPlayer.Position);
	}

	public override void OnNPCDie(object objNpc)
	{
		ObjNPC objNpc2 = objNpc as ObjNPC;
		if (Singleton<ObjManager>.Instance.CheckNPCClear(objNpc2))
		{
			MoveNextState();
		}
	}
}
