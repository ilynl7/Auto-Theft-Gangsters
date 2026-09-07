using System.Collections.Generic;
using UnityEngine;

public class SneakingSceneManager : SceneManager
{
	private MovePathPoint mMovePathPoint;

	private ObjMainPlayer mMainPlayer;

	private float MISSION_TIME = 180f;

	private float mMissionTimeCount;

	private CopySceneData mCurCopySceneData;

	private bool mStartTimeCountFlag;

	private bool mFindPlayerFlag;

	private GameObject mDoorObj;

	private List<ObjPatrolNPC> mPatrolNPCList = new List<ObjPatrolNPC>();

	private Vector3 BossPoint = new Vector3(2f, 0f, -7f);

	private Dictionary<int, List<Vector3>> mNPCPathDic = new Dictionary<int, List<Vector3>>();

	private Dictionary<int, string> mNPCDic = new Dictionary<int, string>();

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
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("Items/MovePathPoint") as GameObject;
		mMovePathPoint = gameObject.GetComponent<MovePathPoint>();
		mMovePathPoint.RegisterOnArrivePathPoint(OnArrivePathPoint);
		mMovePathPoint.ActiveRadius = 1f;
		SingletonUnity<MyEvent>.Instance.Register("OnMainPlayerCreate", this, "OnMainPlayerCreate");
		InitBlock(id);
		mFindPlayerFlag = false;
		mDoorObj = GameObject.Find("Door_50");
	}

	private void InitBlock(string mapId)
	{
		List<SneakingMissionData> sneakingMissionDataListByMapId = DataManager.GetSneakingMissionDataListByMapId(mapId);
		List<Vector3> list = null;
		for (int i = 0; i < sneakingMissionDataListByMapId.Count; i++)
		{
			if (sneakingMissionDataListByMapId[i].PointType == 1)
			{
				if (!mNPCPathDic.ContainsKey(sneakingMissionDataListByMapId[i].PathGroup))
				{
					mNPCPathDic.Add(sneakingMissionDataListByMapId[i].PathGroup, new List<Vector3>());
					mNPCDic.Add(sneakingMissionDataListByMapId[i].PathGroup, sneakingMissionDataListByMapId[i].NPCID);
				}
				list = mNPCPathDic[sneakingMissionDataListByMapId[i].PathGroup];
				list.Add(new Vector3(sneakingMissionDataListByMapId[i].PosX, sneakingMissionDataListByMapId[i].PosY, sneakingMissionDataListByMapId[i].PosZ));
			}
			else if (sneakingMissionDataListByMapId[i].PointType == 0)
			{
				mMovePathPoint.transform.position = new Vector3(sneakingMissionDataListByMapId[i].PosX, sneakingMissionDataListByMapId[i].PosY, sneakingMissionDataListByMapId[i].PosZ);
			}
			else if (sneakingMissionDataListByMapId[i].PointType == 2)
			{
				BossPoint = new Vector3(sneakingMissionDataListByMapId[i].PosX, sneakingMissionDataListByMapId[i].PosY, sneakingMissionDataListByMapId[i].PosZ);
			}
		}
		foreach (int key in mNPCPathDic.Keys)
		{
			NpcData npcDataByID = DataManager.GetNpcDataByID(mNPCDic[key]);
			List<Vector3> path = mNPCPathDic[key];
			ObjInitNpcData objInitNpcData = new ObjInitNpcData();
			objInitNpcData.mServerID = UUID.GenUUID();
			objInitNpcData.mPos = path[0];
			objInitNpcData.mDir = (path[1] - path[0]).normalized;
			objInitNpcData.HP = npcDataByID.Hp;
			objInitNpcData.MaxHP = npcDataByID.Hp;
			objInitNpcData.npcInfoData = npcDataByID;
			objInitNpcData.mCharacterModelId = npcDataByID.Model;
			Singleton<ObjManager>.Instance.GetPatrolNPC(objInitNpcData, delegate(ObjNPC npc)
			{
				if (npc != null)
				{
					ObjPatrolNPC objPatrolNPC = npc as ObjPatrolNPC;
					if (objPatrolNPC != null)
					{
						objPatrolNPC.SetPathPoint(path);
					}
					mPatrolNPCList.Add(objPatrolNPC);
				}
			});
		}
	}

	public override void StartGame()
	{
		Debug.Log("CarChaseSceneManager StartGame");
	}

	private void OnArrivePathPoint(Vector3 pos)
	{
		if (mFindPlayerFlag)
		{
			UnityVersionUtil.SetActiveRecursive(mMovePathPoint.gameObject, state: true);
			return;
		}
		OpenDoor();
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CarTargetRoot);
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
	}

	public void OnMainPlayerCreate()
	{
		SingletonUnity<MyEvent>.Instance.DeRegister("OnMainPlayerCreate", this, "OnMainPlayerCreate");
		ObjManager instance = Singleton<ObjManager>.Instance;
		mMainPlayer = instance.MainPlayer;
		SingletonUnity<UIManager>.Instance.ShowDefaultUI();
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.JueseJiNengQuUI);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CarTargetRoot, delegate
		{
			SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mMovePathPoint.gameObject, mMainPlayer.gameObject);
		});
	}

	private void EndMission(bool isSuccess)
	{
		if (!mEndMissionFlag)
		{
			mEndMissionFlag = true;
		}
	}

	public override void OnFindPlayer()
	{
		if (mFindPlayerFlag)
		{
			return;
		}
		mFindPlayerFlag = true;
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BlackScreenRoot, delegate
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.WaringUIRoot, delegate
			{
				SingletonUnity<WaringUIRoot>.Instance.Reset(StrDictionary.GetDictionaryString("#{101545}"), 2f);
			});
			SingletonUnity<BlackScreenLogic>.Instance.CloseScreen();
		});
		vp_Timer.In(1f, ResetToBeginPos);
	}

	public void ResetToBeginPos()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		mMainPlayer.DisableNavMeshAgent();
		mMainPlayer.CacheTransform.position = playerData.MainPlayerStartPos;
		mMainPlayer.FaceToPub(playerData.MainPlayerStartDir);
		mMainPlayer.EnableNavMeshAgent();
		SingletonUnity<BlackScreenLogic>.Instance.OpenScreen(0.5f, delegate
		{
			mFindPlayerFlag = false;
		});
	}

	public void OpenDoor()
	{
		if (mFindPlayerFlag)
		{
			return;
		}
		ClearPatrolNPC();
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.YiDongKongZhiUI);
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		TweenPosition twDoor = null;
		if (mDoorObj != null)
		{
			twDoor = mDoorObj.GetComponent<TweenPosition>();
		}
		if (twDoor != null)
		{
			twDoor.PlayForward();
		}
		if (SingletonUnity<JoyStickLogic>.Exists)
		{
			SingletonUnity<JoyStickLogic>.Instance.MoveOutScreen();
		}
		mMainPlayer.StopMove();
		Vector3 targetPoint = new Vector3(2f, 0f, -7f);
		if (GameManager.IsSupportCurDataVersion145())
		{
			targetPoint = BossPoint;
		}
		vp_Timer.In(0.5f, delegate
		{
			Singleton<ObjManager>.Instance.MainPlayer.NavMeshAgent.walkableMask += 8;
			mMainPlayer.MoveTo(targetPoint, 0.5f, delegate
			{
				SingletonUnity<UIManager>.Instance.ReShowBaseUI();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.YiDongKongZhiUI);
				if (twDoor != null)
				{
					twDoor.PlayReverse();
				}
				Singleton<ObjManager>.Instance.MainPlayer.NavMeshAgent.walkableMask -= 8;
				NetLogic.GetInstance().Send<Protocol.start_battle>();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.JueseJiNengQuUI);
			});
		});
	}

	public void ClearPatrolNPC()
	{
		for (int num = mPatrolNPCList.Count - 1; num >= 0; num--)
		{
			UnityVersionUtil.SetActiveRecursive(mPatrolNPCList[num].gameObject, state: false);
			Singleton<ObjManager>.Instance.RecyclePatrolNPC(mPatrolNPCList[num]);
			mPatrolNPCList.RemoveAt(num);
		}
	}

	public override void OnNPCDie(object objNpc)
	{
	}
}
