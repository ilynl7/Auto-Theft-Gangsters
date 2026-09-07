using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class CashSneakingSceneManager : SceneManager
{
	private MovePathPoint mMovePathPoint;

	private ObjMainPlayer mMainPlayer;

	private float MISSION_TIME = 180f;

	private float mMissionTimeCount;

	private CopySceneData mCurCopySceneData;

	private bool mStartTimeCountFlag;

	private Vector3 mPlayerStartPos;

	private float mPlayerStartDir;

	private bool mFindPlayerFlag;

	private GameObject mDoorObj;

	private List<ObjPatrolNPC> mPatrolNPCList = new List<ObjPatrolNPC>();

	private bool FinishSneakingFlag;

	private bool passDoorFlag;

	private Dictionary<int, List<Vector3>> mNPCPathDic = new Dictionary<int, List<Vector3>>();

	private Dictionary<int, string> mNPCDic = new Dictionary<int, string>();

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
		GameObject gameObject = GameObject.Find("MovePathPoint");
		if (gameObject == null)
		{
			gameObject = ResourcesManager.LoadAndInstantiate("Items/MovePathPoint") as GameObject;
		}
		mMovePathPoint = gameObject.GetComponent<MovePathPoint>();
		mMovePathPoint.RegisterOnArrivePathPoint(OnArrivePathPoint);
		mMovePathPoint.ActiveRadius = 1f;
		SingletonUnity<MyEvent>.Instance.Register("OnMainPlayerCreate", this, "OnMainPlayerCreate");
		InitBlock(id);
		mFindPlayerFlag = false;
		mDoorObj = GameObject.Find("Door_50");
		FinishSneakingFlag = false;
		passDoorFlag = false;
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
	}

	private void OnArrivePathPoint(Vector3 pos)
	{
		if (mFindPlayerFlag)
		{
			UnityVersionUtil.SetActiveRecursive(mMovePathPoint.gameObject, state: true);
		}
		else
		{
			OpenDoor();
		}
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

	public void OnMainPlayerCreate()
	{
		SingletonUnity<MyEvent>.Instance.DeRegister("OnMainPlayerCreate", this, "OnMainPlayerCreate");
		ObjManager instance = Singleton<ObjManager>.Instance;
		mMainPlayer = instance.MainPlayer;
		SingletonUnity<UIManager>.Instance.ShowDefaultUI();
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.JueseJiNengQuUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PotionObjRoot);
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
		mMainPlayer.StopMove();
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
		FinishSneakingFlag = true;
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.YiDongKongZhiUI);
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		TweenPosition twDoor = mDoorObj.GetComponent<TweenPosition>();
		if (twDoor != null)
		{
			twDoor.PlayForward();
		}
		ClearPatrolNPC();
		if (SingletonUnity<JoyStickLogic>.Exists)
		{
			SingletonUnity<JoyStickLogic>.Instance.MoveOutScreen();
		}
		mMainPlayer.StopMove();
		vp_Timer.In(0.5f, delegate
		{
			Singleton<ObjManager>.Instance.MainPlayer.NavMeshAgent.walkableMask += 8;
			mMainPlayer.MoveTo(new Vector3(2f, 0f, -7f), 0.5f, delegate
			{
				OnPassDoor();
			});
		});
		vp_Timer.In(1.5f, delegate
		{
			twDoor.PlayReverse();
		});
	}

	private void OnPassDoor()
	{
		passDoorFlag = true;
		SingletonUnity<UIManager>.Instance.ReShowBaseUI();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.YiDongKongZhiUI);
		Singleton<ObjManager>.Instance.MainPlayer.NavMeshAgent.walkableMask -= 8;
		NetLogic.GetInstance().Send<Protocol.start_battle>();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.JueseJiNengQuUI);
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PotionObjRoot, delegate
			{
				SingletonUnity<PotionLogic>.Instance.Reset();
			});
		}
	}

	public void ClearPatrolNPC()
	{
		for (int num = mPatrolNPCList.Count - 1; num >= 0; num--)
		{
			mPatrolNPCList[num].RecycleSelf();
			mPatrolNPCList.RemoveAt(num);
		}
	}

	public override void OnNPCDie(object objNpc)
	{
		ObjNPC objNPC = objNpc as ObjNPC;
		if (IsCopyScene())
		{
			single_copy_scene_npc_die.request request = new single_copy_scene_npc_die.request();
			request.characterId = objNPC.ServerId;
			request.type = objNPC.NPCData.Type;
			request.npcdataid = objNPC.NPCDataID;
			request.pos_x = Mathf.FloorToInt(objNPC.Position.x * 100f);
			request.pos_z = Mathf.FloorToInt(objNPC.Position.z * 100f);
			NetLogic.GetInstance().Send<Protocol.single_copy_scene_npc_die>(request);
		}
		if (objNPC.NPCType != GameDefine.NPC_TYPE.BOSS)
		{
		}
	}

	public override void OnReconnectSuccess()
	{
		base.OnReconnectSuccess();
		if (!FinishSneakingFlag)
		{
			return;
		}
		if (!passDoorFlag)
		{
			mMainPlayer.MoveTo(new Vector3(2f, 0f, -7f), 0.5f, delegate
			{
				OnPassDoor();
			});
		}
		else
		{
			NetLogic.GetInstance().Send<Protocol.start_battle>();
		}
	}
}
