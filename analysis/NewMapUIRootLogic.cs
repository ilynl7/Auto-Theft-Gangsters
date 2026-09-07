using System.Collections.Generic;
using System.Text;
using SprotoType;
using UnityEngine;

public class NewMapUIRootLogic : SingletonUnity<NewMapUIRootLogic>
{
	public enum SHOW_TYPE
	{
		FUNCTION,
		MONSTER
	}

	public float FlashInterval;

	public UILabel MapNameLabel;

	public Transform LocalMapPlayerPic;

	public UITexture LocalMapTexture;

	private ObjMainPlayer Player;

	private MapInfoData curMapInfo;

	public List<UISprite> MonsterPointPic;

	public List<UISprite> TeleportPointPic;

	public List<UISprite> SurviveTelPic;

	public List<UISprite> NPCPointPic;

	public List<UISprite> NPC2PointPic;

	public List<UISprite> NPC3PointPic;

	public List<UISprite> NpcPointPicList = new List<UISprite>();

	public List<NewMapActivityObj> MapActivityObjList = new List<NewMapActivityObj>();

	public List<NewMapActivityObj> MissionActivityObjList = new List<NewMapActivityObj>();

	private List<MapPoint> curSceneTelePortInfo = new List<MapPoint>();

	private List<MapPoint> curSceneMonsterData = new List<MapPoint>();

	private List<MapPoint> curSceneNPCData = new List<MapPoint>();

	private List<ActivityMapData> curActivityMapData = new List<ActivityMapData>();

	private List<ActivityMapData> curVisibleActivityMapData = new List<ActivityMapData>();

	private List<MissionData> curMapAcceptMissionDataList = new List<MissionData>();

	private List<MissionData> curAcceptableMissionDataList = new List<MissionData>();

	private List<MissionData> curAcceptedMissionDataList = new List<MissionData>();

	private List<MapPoint> curShowPointList = new List<MapPoint>();

	public int LineCount = 5;

	public UILabel OnLineINfoLabel;

	private bool mLocalMapFlag = true;

	public GameObject LineObj;

	public UISprite MoveTargetPic;

	public List<UILabel> MapLockLabel;

	public LineRenderer PathLineRender;

	public LineRenderer WalkLineRender1;

	public LineRenderer WalkLineRender2;

	public UILabel MapcapturLabel;

	private SHOW_TYPE curShowType;

	private bool mLoadingMapFlag;

	private bool mFindTargetFlag;

	private int mTargetType = -99;

	private int mTargetSubtype = -99;

	private bool mTargetIsMission;

	private string mTargetActId = string.Empty;

	private Texture tutorialLockPic;

	private float MaxMapWidth = 300f;

	private float MaxMapHeight = 300f;

	private float intervalCount;

	private StringBuilder mTempStr = new StringBuilder();

	public MapInfoData CurMapInfo => curMapInfo;

	protected override void Awake()
	{
		base.Awake();
		Player = Singleton<ObjManager>.Instance.MainPlayer;
		curSceneTelePortInfo.Clear();
		curSceneMonsterData.Clear();
		curSceneNPCData.Clear();
		curActivityMapData.Clear();
		for (int i = 0; i < MonsterPointPic.Count; i++)
		{
			MonsterPointPic[i].enabled = false;
		}
		for (int j = 0; j < TeleportPointPic.Count; j++)
		{
			TeleportPointPic[j].enabled = false;
		}
		for (int k = 0; k < SurviveTelPic.Count; k++)
		{
			SurviveTelPic[k].enabled = false;
		}
		for (int l = 0; l < NPCPointPic.Count; l++)
		{
			NPCPointPic[l].enabled = false;
		}
		for (int m = 0; m < NPC2PointPic.Count; m++)
		{
			NPC2PointPic[m].enabled = false;
		}
		for (int n = 0; n < NPC3PointPic.Count; n++)
		{
			NPC3PointPic[n].enabled = false;
		}
		for (int num = 0; num < NpcPointPicList.Count; num++)
		{
			NpcPointPicList[num].enabled = false;
		}
		MoveTargetPic.enabled = false;
	}

	public void InitTexture()
	{
		List<string> list = new List<string>();
		if (LocalMapTexture.mainTexture == null || !LocalMapTexture.mainTexture.name.Equals(curMapInfo.MiniMapName))
		{
			list.Add(curMapInfo.MiniMapName);
		}
		if (list.Count == 0)
		{
			UpdateMapLockIcon();
			return;
		}
		mLoadingMapFlag = true;
		if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(list, TextureLoadFinish));
		}
	}

	public void UpdateMapLockIcon()
	{
		Material material = LocalMapTexture.material;
		for (int i = 0; i < MapLockLabel.Count; i++)
		{
			NGUITools.SetActive(MapLockLabel[i].gameObject, state: false);
		}
		material.SetTexture("_MapLockTex", null);
		material.SetFloat("_LockShowRange", 1.1f);
	}

	private void TextureLoadFinish(Dictionary<string, Texture> retdic)
	{
		mLoadingMapFlag = false;
		if (retdic.Count == 0)
		{
			return;
		}
		if (retdic.ContainsKey(curMapInfo.MiniMapName))
		{
			Texture texture = retdic[curMapInfo.MiniMapName];
			if (texture != null)
			{
				Material material = LocalMapTexture.material;
				material.SetTexture("_MainTex", texture);
				UpdateMapLockIcon();
				LocalMapTexture.mainTexture = texture;
				if (texture.width > texture.height)
				{
					if ((float)texture.width > MaxMapWidth)
					{
						LocalMapTexture.width = (int)MaxMapWidth;
						LocalMapTexture.height = (int)((float)texture.height * (MaxMapWidth / (float)texture.width));
					}
					else
					{
						LocalMapTexture.width = texture.width;
						LocalMapTexture.height = texture.height;
					}
				}
				else if ((float)texture.height > MaxMapHeight)
				{
					LocalMapTexture.width = (int)((float)texture.width * (MaxMapHeight / (float)texture.height));
					LocalMapTexture.height = (int)MaxMapHeight;
				}
				else
				{
					LocalMapTexture.width = texture.width;
					LocalMapTexture.height = texture.height;
				}
				LocalMapTexture.SetDirty();
			}
			else
			{
				Debug.LogWarning("no mini map1 :" + curMapInfo.MiniMapName);
			}
		}
		else
		{
			Debug.LogWarning("no mini map2 :" + curMapInfo.MiniMapName);
		}
		InitTeleportPointPic();
		InitMapActivityPic();
		InitMissionActivityPic();
		UpdateMissionPointPic();
		InitLeftBtn();
		InitActivityBossPointPic();
		UpdateMoveTargetPic();
		UpdateNpcPos();
		if (mFindTargetFlag)
		{
			if (mTargetType != -99)
			{
				ChooseActivityObj(mTargetType, mTargetSubtype, mTargetActId);
			}
			else
			{
				ChooseActivityObj(mTargetActId, mTargetIsMission);
			}
		}
	}

	public void OnClickChangeLineBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MapLineInfoLogic, delegate
		{
			SingletonUnity<MapLineInfoLogic>.Instance.PreReset();
			NetLogic.GetInstance().Send<Protocol.request_line_state>();
		});
	}

	public void OnClickWorldMapBtn()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.WorldMapRoot, delegate
			{
				WaitResponseUIRootLogic.OpenWaitBox(319, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_guild_map_info>();
				SingletonUnity<WorldMapRoot>.Instance.EnableReset();
			});
		}
		else if (SingletonUnity<DownloadTipRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DownloadTipRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.CloseAllPOPUI();
			SingletonUnity<DownloadTipRootLogic>.Instance.OnClickDownloadTipBtn();
		}
	}

	private void InitLeftBtn()
	{
		int curLineIndex = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CurLineIndex;
		int lineCount = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.LineCount;
		OnLineINfoLabel.text = $"line{curLineIndex}";
	}

	public void Reset(string mapId)
	{
		if (curMapInfo != null && mapId.Equals(curMapInfo.ID))
		{
			UpdateMapLockIcon();
			return;
		}
		curMapInfo = DataManager.GetMapInfoDataByID(mapId);
		if (curMapInfo == null)
		{
			Debug.Log("No Map Data!!!!!!!!!!!!!!!!  " + mapId);
			return;
		}
		EnableReset();
		MapNameLabel.text = StrDictionary.GetDictionaryString(curMapInfo.Name);
		curActivityMapData = DataManager.GetAcitvityMapDataByMapId(mapId);
		guild_map_info guildMapInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.GetGuildMapInfo(mapId);
		if (guildMapInfo != null && guildMapInfo.HasGuildName)
		{
			MapcapturLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{106040}"), guildMapInfo.guildName);
		}
		else
		{
			MapcapturLabel.text = string.Empty;
		}
		InitTexture();
		if (!mLoadingMapFlag)
		{
			InitMapActivityPic();
			InitMissionActivityPic();
			UpdateMissionPointPic();
			InitLeftBtn();
			InitActivityBossPointPic();
			UpdateMoveTargetPic();
			UpdateMapLockIcon();
			UpdateNpcPos();
		}
	}

	public void EnableReset()
	{
		mLoadingMapFlag = false;
		for (int i = 0; i < MapActivityObjList.Count; i++)
		{
			NGUITools.SetActive(MapActivityObjList[i].gameObject, state: false);
		}
		for (int j = 0; j < MissionActivityObjList.Count; j++)
		{
			NGUITools.SetActive(MissionActivityObjList[j].gameObject, state: false);
		}
		for (int k = 0; k < MapLockLabel.Count; k++)
		{
			NGUITools.SetActive(MapLockLabel[k].gameObject, state: false);
		}
		MoveTargetPic.enabled = false;
	}

	public void InitMapActivityPic()
	{
		curVisibleActivityMapData.Clear();
		for (int i = 0; i < curActivityMapData.Count; i++)
		{
			if (curActivityMapData[i].IsVisible)
			{
				curVisibleActivityMapData.Add(curActivityMapData[i]);
			}
		}
		int num = curVisibleActivityMapData.Count - MapActivityObjList.Count;
		if (num > 0)
		{
			GameObject gameObject = null;
			for (int j = 0; j < num; j++)
			{
				gameObject = Object.Instantiate(MapActivityObjList[0].gameObject) as GameObject;
				gameObject.transform.parent = MapActivityObjList[0].transform.parent;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				MapActivityObjList.Add(gameObject.GetComponent<NewMapActivityObj>());
			}
		}
		for (int k = 0; k < MapActivityObjList.Count; k++)
		{
			if (k < curVisibleActivityMapData.Count)
			{
				NGUITools.SetActive(MapActivityObjList[k].gameObject, state: true);
				MapActivityObjList[k].Reset(curVisibleActivityMapData[k]);
				MapActivityObjList[k].transform.localPosition = WorldPos2MapPos(curVisibleActivityMapData[k].Position);
			}
			else
			{
				NGUITools.SetActive(MapActivityObjList[k].gameObject, state: false);
			}
		}
	}

	public void InitMissionActivityPic()
	{
		curAcceptableMissionDataList.Clear();
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		curMapAcceptMissionDataList = DataManager.GetAcceptMissionDataByMapId(curMapInfo.ID);
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		if (curMapAcceptMissionDataList != null)
		{
			for (int i = 0; i < curMapAcceptMissionDataList.Count; i++)
			{
				if (curMapAcceptMissionDataList[i].Class != 4 && curMapAcceptMissionDataList[i].Class != 5 && missionManager.IsMissionAcceptable(curMapAcceptMissionDataList[i].ID) && !string.IsNullOrEmpty(curMapAcceptMissionDataList[i].Accept))
				{
					if (dictionary.ContainsKey(curMapAcceptMissionDataList[i].Accept))
					{
						dictionary[curMapAcceptMissionDataList[i].Accept].Add(curMapAcceptMissionDataList[i].ID);
						continue;
					}
					dictionary.Add(curMapAcceptMissionDataList[i].Accept, new List<string>());
					dictionary[curMapAcceptMissionDataList[i].Accept].Add(curMapAcceptMissionDataList[i].ID);
					curAcceptableMissionDataList.Add(curMapAcceptMissionDataList[i]);
				}
			}
		}
		curAcceptedMissionDataList.Clear();
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
			if (string.IsNullOrEmpty(empty2) || !empty2.Equals(curMapInfo.ID))
			{
				continue;
			}
			if (!string.IsNullOrEmpty(empty))
			{
				if (dictionary.ContainsKey(empty))
				{
					dictionary[empty].Add(allMissionId[j]);
					continue;
				}
				dictionary.Add(empty, new List<string>());
				dictionary[empty].Add(allMissionId[j]);
			}
			curAcceptedMissionDataList.Add(missionDataByID);
		}
		int num = curAcceptableMissionDataList.Count + curAcceptedMissionDataList.Count - MissionActivityObjList.Count;
		if (num > 0)
		{
			GameObject gameObject = null;
			for (int k = 0; k < num; k++)
			{
				gameObject = Object.Instantiate(MissionActivityObjList[0].gameObject) as GameObject;
				gameObject.transform.parent = MissionActivityObjList[0].transform.parent;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				MissionActivityObjList.Add(gameObject.GetComponent<NewMapActivityObj>());
			}
		}
		int num2 = curAcceptedMissionDataList.Count + curAcceptableMissionDataList.Count;
		for (int l = 0; l < MissionActivityObjList.Count; l++)
		{
			if (l < num2)
			{
				if (l < curAcceptableMissionDataList.Count)
				{
					if (string.IsNullOrEmpty(curAcceptableMissionDataList[l].Accept))
					{
						continue;
					}
					NGUITools.SetActive(MissionActivityObjList[l].gameObject, state: true);
					if (dictionary.ContainsKey(curAcceptableMissionDataList[l].Accept))
					{
						MissionActivityObjList[l].Reset(curAcceptableMissionDataList[l], dictionary[curAcceptableMissionDataList[l].Accept]);
					}
					else
					{
						MissionActivityObjList[l].Reset(curAcceptableMissionDataList[l], null);
					}
					Vector3 pos = DataManager.GetNPCPosInMonsterData(curMapInfo.ID, curAcceptableMissionDataList[l].Accept);
					if (curAcceptableMissionDataList[l].Class == 1 && curAcceptableMissionDataList[l].MissionLogicType == MISSION_LOGICTYPE.CAPTURE_SUCCESS)
					{
						DominData dominDataByID = DataManager.GetDominDataByID(curAcceptableMissionDataList[l].LogicID);
						if (dominDataByID != null)
						{
							pos = dominDataByID.GetPos();
						}
					}
					MissionActivityObjList[l].transform.localPosition = WorldPos2MapPos(pos);
					continue;
				}
				int index = l - curAcceptableMissionDataList.Count;
				NGUITools.SetActive(MissionActivityObjList[l].gameObject, state: true);
				mISSION_STATE = missionManager.GetMissionState(curAcceptedMissionDataList[index].ID);
				Vector3 zero = Vector3.zero;
				if (mISSION_STATE == MISSION_STATE.ACCEPTED)
				{
					if (curAcceptedMissionDataList[index].MissionLogicType == MISSION_LOGICTYPE.SURVEY)
					{
						SurveyMissionData surveyMissionDataById2 = DataManager.GetSurveyMissionDataById(curAcceptedMissionDataList[index].LogicID);
						zero = new Vector3(surveyMissionDataById2.PosX, surveyMissionDataById2.PosY, surveyMissionDataById2.PosZ);
						MissionActivityObjList[l].Reset(curAcceptedMissionDataList[index], null);
					}
					else
					{
						zero = DataManager.GetNPCPosInMonsterData(curMapInfo.ID, curAcceptedMissionDataList[index].Target);
						if (dictionary.ContainsKey(curAcceptedMissionDataList[index].Target))
						{
							MissionActivityObjList[l].Reset(curAcceptedMissionDataList[index], dictionary[curAcceptedMissionDataList[index].Target]);
						}
						else
						{
							MissionActivityObjList[l].Reset(curAcceptedMissionDataList[index], null);
						}
					}
				}
				else
				{
					zero = DataManager.GetNPCPosInMonsterData(curMapInfo.ID, curAcceptedMissionDataList[index].Submit);
					if (dictionary.ContainsKey(curAcceptedMissionDataList[index].Submit))
					{
						MissionActivityObjList[l].Reset(curAcceptedMissionDataList[index], dictionary[curAcceptedMissionDataList[index].Submit]);
					}
					else
					{
						MissionActivityObjList[l].Reset(curAcceptedMissionDataList[index], null);
					}
				}
				if (curAcceptedMissionDataList[index].Class == 1 && curAcceptedMissionDataList[index].MissionLogicType == MISSION_LOGICTYPE.CAPTURE_SUCCESS)
				{
					DominData dominDataByID2 = DataManager.GetDominDataByID(curAcceptedMissionDataList[index].LogicID);
					if (dominDataByID2 != null)
					{
						zero = dominDataByID2.GetPos();
					}
				}
				MissionActivityObjList[l].transform.localPosition = WorldPos2MapPos(zero);
			}
			else
			{
				NGUITools.SetActive(MissionActivityObjList[l].gameObject, state: false);
			}
		}
	}

	private void UpdateMissionPointPic()
	{
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		MapInfoData currentMapInofData = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData;
		int curLineIndex = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CurLineIndex;
		CurMission escortMission = missionManager.GetEscortMission();
		string mapId = string.Empty;
		if (escortMission != null)
		{
			Vector3 escortNpcPos = missionManager.GetEscortNpcPos(out mapId);
			int num = (int)escortMission.GetParam(2);
			if (currentMapInofData.ID.Equals(mapId) && curLineIndex == num)
			{
				NPCPointPic[0].enabled = true;
				NPCPointPic[0].transform.localPosition = WorldPos2MapPos(escortNpcPos);
			}
			else
			{
				NPCPointPic[0].enabled = false;
			}
		}
		else
		{
			NPCPointPic[0].enabled = false;
		}
		List<ObjNPC> otherPlayerEscortNPCList = Singleton<ObjManager>.Instance.OtherPlayerEscortNPCList;
		if (otherPlayerEscortNPCList.Count > 0)
		{
			int num2 = otherPlayerEscortNPCList.Count - NPC2PointPic.Count;
			if (num2 > 0)
			{
				GameObject gameObject = Object.Instantiate(NPC2PointPic[0].gameObject) as GameObject;
				gameObject.transform.parent = NPC2PointPic[0].transform.parent;
				gameObject.transform.localScale = Vector3.one;
				NPC2PointPic.Add(gameObject.GetComponent<UISprite>());
			}
		}
		for (int i = 0; i < NPC2PointPic.Count; i++)
		{
			NPC2PointPic[i].enabled = i < otherPlayerEscortNPCList.Count;
			if (i < otherPlayerEscortNPCList.Count)
			{
				NPC2PointPic[i].transform.localPosition = WorldPos2MapPos(otherPlayerEscortNPCList[i].Position);
			}
		}
	}

	private void InitMonsterPointPic()
	{
		curSceneMonsterData.Clear();
		curSceneNPCData.Clear();
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		List<int> monsterGroupList = sceneManager.GetMonsterGroupList();
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		for (int i = 0; i < monsterGroupList.Count; i++)
		{
			List<MonsterData> monsterDataByGroup = sceneManager.GetMonsterDataByGroup(monsterGroupList[i]);
			if (monsterGroupList[i] != GameDefine.MISSION_NPC_GROUP_VAL && !sceneManager.IsSurviveBattleScene())
			{
				Vector3 zero = Vector3.zero;
				for (int j = 0; j < monsterDataByGroup.Count; j++)
				{
					zero += monsterDataByGroup[j].GetNpcXZPos();
				}
				NpcData npcDataByID = DataManager.GetNpcDataByID(monsterDataByGroup[0].NpcID);
				zero /= (float)monsterDataByGroup.Count;
				if (npcDataByID.Type == 2)
				{
					list.Add(monsterDataByGroup[0].GetNpcXZPos());
				}
				else if (string.IsNullOrEmpty(monsterDataByGroup[0].PathId))
				{
					list2.Add(monsterDataByGroup[0].GetNpcXZPos());
				}
				if (npcDataByID.FunctionType != 2 && npcDataByID.FunctionType != 3)
				{
					MapPoint mapPoint = new MapPoint();
					if (string.IsNullOrEmpty(monsterDataByGroup[0].PathId))
					{
						mapPoint.PointName = npcDataByID.MName;
					}
					else
					{
						mapPoint.PointName = "[Patrol] " + npcDataByID.MName;
					}
					mapPoint.PointPos = zero;
					mapPoint.PointType = MAP_POINT_TYPE.MONSTER;
					mapPoint.NpcId = monsterDataByGroup[0].NpcID;
					curSceneMonsterData.Add(mapPoint);
				}
				continue;
			}
			for (int k = 0; k < monsterDataByGroup.Count; k++)
			{
				NpcData npcDataByID2 = DataManager.GetNpcDataByID(monsterDataByGroup[k].NpcID);
				if (npcDataByID2.Group == 6 || npcDataByID2.FunctionType == 2 || npcDataByID2.FunctionType == 3)
				{
					continue;
				}
				MapPoint mapPoint2 = new MapPoint();
				mapPoint2.PointName = DataManager.GetNpcDataByID(monsterDataByGroup[k].NpcID).MName;
				mapPoint2.PointPos = monsterDataByGroup[k].GetNpcXZPos();
				mapPoint2.NpcId = monsterDataByGroup[k].NpcID;
				if (sceneManager.IsSurviveBattleScene())
				{
					NpcData npcDataByID3 = DataManager.GetNpcDataByID(monsterDataByGroup[0].NpcID);
					if (npcDataByID3.Type == 2)
					{
						list.Add(monsterDataByGroup[k].GetNpcXZPos());
					}
					else
					{
						list2.Add(monsterDataByGroup[k].GetNpcXZPos());
					}
					mapPoint2.PointType = MAP_POINT_TYPE.MONSTER;
					curSceneMonsterData.Add(mapPoint2);
				}
				else
				{
					mapPoint2.PointType = MAP_POINT_TYPE.NPC;
					curSceneNPCData.Add(mapPoint2);
				}
			}
		}
		int num = list2.Count - NPC3PointPic.Count;
		if (num > 0)
		{
			for (int l = 0; l < num; l++)
			{
				GameObject gameObject = Object.Instantiate(NPC3PointPic[0].gameObject) as GameObject;
				gameObject.transform.parent = NPC3PointPic[0].transform.parent;
				NPC3PointPic.Add(gameObject.GetComponent<UISprite>());
			}
		}
		for (int m = 0; m < NPC3PointPic.Count; m++)
		{
			NPC3PointPic[m].enabled = m < list2.Count;
		}
		for (int n = 0; n < list2.Count; n++)
		{
			NPC3PointPic[n].transform.localPosition = WorldPos2MapPos(list2[n]);
		}
	}

	public void UpdateMoveTargetPic()
	{
		if (curMapInfo.ID.Equals(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID) && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsHaveMoveTarget)
		{
			MoveTargetPic.transform.localPosition = WorldPos2MapPos(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurMoveTarget);
			MoveTargetPic.enabled = true;
		}
		else
		{
			MoveTargetPic.enabled = false;
		}
	}

	private void InitActivityBossPointPic()
	{
		List<Vector3> list = new List<Vector3>();
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		List<TimerActivityData> enableTimerActivityList = DataManager.GetEnableTimerActivityList();
		List<ActivityBossData> list2 = new List<ActivityBossData>();
		ActivityBossData activityBossData = null;
		if (enableTimerActivityList != null)
		{
			for (int i = 0; i < enableTimerActivityList.Count; i++)
			{
				activityBossData = DataManager.GetActivityBossDataByActivityIdMapId(enableTimerActivityList[i].ID, CurMapInfo.ID);
				if (activityBossData != null)
				{
					list2.Add(activityBossData);
				}
			}
			for (int j = 0; j < list2.Count; j++)
			{
				for (int k = 0; k < list2[j].NpcPosList.Count; k++)
				{
					list.Add(list2[j].NpcPosList[k]);
				}
			}
		}
		int num = list.Count - MonsterPointPic.Count;
		if (num > 0)
		{
			for (int l = 0; l < num; l++)
			{
				GameObject gameObject = Object.Instantiate(MonsterPointPic[0].gameObject) as GameObject;
				gameObject.transform.parent = MonsterPointPic[0].transform.parent;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				MonsterPointPic.Add(gameObject.GetComponent<UISprite>());
			}
		}
		for (int m = 0; m < MonsterPointPic.Count; m++)
		{
			MonsterPointPic[m].enabled = m < list.Count;
		}
		for (int n = 0; n < list.Count; n++)
		{
			MonsterPointPic[n].transform.localPosition = WorldPos2MapPos(list[n]);
		}
	}

	private void InitTeleportPointPic()
	{
		TeleportPointPic[0].enabled = false;
		SurviveTelPic[0].alpha = 0f;
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.IsCopyShowMap() && !sceneManager.IsTutorialScene())
		{
			if (sceneManager.IsSurviveBattleScene())
			{
				SurviveTelPic[0].alpha = 1f;
				curSceneTelePortInfo.Clear();
				MapInfoData currentMapInofData = sceneManager.CurrentMapInofData;
				int num = currentMapInofData.TeleportPosList.Count - SurviveTelPic.Count;
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = Object.Instantiate(SurviveTelPic[0].gameObject) as GameObject;
					UISprite component = gameObject.GetComponent<UISprite>();
					gameObject.transform.parent = SurviveTelPic[0].transform.parent;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					SurviveTelPic.Add(component);
				}
				for (int j = 0; j < currentMapInofData.TeleportPosList.Count; j++)
				{
					MapPoint mapPoint = new MapPoint();
					if (GameManager.IsSupportCurDataVersion56())
					{
						mapPoint.PointName = StrDictionary.GetDictionaryString("#{101812}");
					}
					else
					{
						mapPoint.PointName = StrDictionary.GetDictionaryString("#{101809}");
					}
					mapPoint.PointPos = currentMapInofData.TeleportPosList[j];
					mapPoint.PointType = MAP_POINT_TYPE.TELEPORT;
					mapPoint.NpcId = string.Empty;
					curSceneTelePortInfo.Add(mapPoint);
					SurviveTelPic[j].transform.localPosition = WorldPos2MapPos(mapPoint.PointPos);
				}
			}
			else
			{
				SurviveTelPic[0].alpha = 0f;
			}
		}
		else
		{
			TeleportPointPic[0].enabled = true;
			curSceneTelePortInfo.Clear();
			MapInfoData mapInfoData = curMapInfo;
			MapPoint mapPoint2 = new MapPoint();
			mapPoint2.PointName = StrDictionary.GetDictionaryString("#{101809}");
			mapPoint2.PointPos = mapInfoData.TelePortPosVector3;
			mapPoint2.PointType = MAP_POINT_TYPE.TELEPORT;
			mapPoint2.NpcId = string.Empty;
			curSceneTelePortInfo.Add(mapPoint2);
			TeleportPointPic[0].transform.localPosition = WorldPos2MapPos(mapInfoData.TelePortPosVector3);
		}
	}

	private void Update()
	{
		intervalCount += Time.deltaTime;
		if (intervalCount > FlashInterval)
		{
			intervalCount = 0f;
			FlashPlayerPos();
		}
	}

	private void FlashPlayerPos()
	{
		if (curMapInfo == null)
		{
			return;
		}
		if (!curMapInfo.ID.Equals(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID))
		{
			if (UnityVersionUtil.IsActive(LocalMapPlayerPic.gameObject))
			{
				NGUITools.SetActive(LocalMapPlayerPic.gameObject, state: false);
			}
			return;
		}
		if (!UnityVersionUtil.IsActive(LocalMapPlayerPic.gameObject))
		{
			NGUITools.SetActive(LocalMapPlayerPic.gameObject, state: true);
		}
		LocalMapPlayerPic.localPosition = WorldPos2MapPos(Player.Position);
		if (Player.IsLocalDrivingCar)
		{
			LocalMapPlayerPic.localEulerAngles = new Vector3(0f, 0f, 0f - Player.CurPlayerCar.transform.localEulerAngles.y);
		}
		else
		{
			LocalMapPlayerPic.localEulerAngles = new Vector3(0f, 0f, 0f - Player.transform.localEulerAngles.y);
		}
		mTempStr.Length = 0;
		UpdateMissionPointPic();
	}

	private Vector3 WorldPos2MapPos(Vector3 pos)
	{
		return new Vector3(pos.x / curMapInfo.fMapLength * (float)LocalMapTexture.width, pos.z / curMapInfo.fMapHeight * (float)LocalMapTexture.height, 0f);
	}

	private Vector3 WorldPos2MapPos(float posX, float posZ)
	{
		return new Vector3(posX / curMapInfo.fMapLength * (float)LocalMapTexture.width, posZ / curMapInfo.fMapHeight * (float)LocalMapTexture.height, 0f);
	}

	private Vector3 MapPos2WorldPos(Vector3 pos)
	{
		return new Vector3(pos.x / (float)LocalMapTexture.width * curMapInfo.fMapLength, 0f, pos.y / (float)LocalMapTexture.height * curMapInfo.fMapHeight);
	}

	public void OnClickLeftMapBtn(int index)
	{
		change_scene_line.request request = new change_scene_line.request();
		request.line_index = index + 1;
		NetLogic.GetInstance().Send<Protocol.change_scene_line>(request);
	}

	public void OnClickRightNPCBtn(MapPoint clickPoint)
	{
		Player.BreakAutoCombatState();
		Player.SkillLogic.BreakCurSkill();
		Vector3 pointPos = clickPoint.PointPos;
		pointPos.y = SceneManager.GetHitHeight(clickPoint.PointPos);
		SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.StopAutoMoveToMission();
		Player.IsNeedAutoMountCar = true;
		if (Vector3.Distance(pointPos, Player.Position) / Player.NavMeshAgent.speed > 5f)
		{
			Player.EnterAutoMoving(Time.time);
		}
		else
		{
			Player.EnterAutoMoving(float.MaxValue);
		}
		Player.MoveTo(pointPos, 1f, delegate
		{
			Player.IsNeedAutoMountCar = false;
			if (clickPoint.PointType == MAP_POINT_TYPE.NPC)
			{
				ObjNPC objNPC = FindNPC(clickPoint.NpcId);
				if (objNPC != null && objNPC.CheckInDialogRange())
				{
					Singleton<DialogManager>.Instance.ShowDialog(objNPC, string.Empty);
				}
			}
			else if (clickPoint.PointType == MAP_POINT_TYPE.MONSTER)
			{
				SingletonUnity<JueseJiNengQuLogic>.Instance.UseSkill_1_Onclick();
			}
		});
	}

	private ObjNPC FindNPC(string npcId)
	{
		List<Obj> list = new List<Obj>(Singleton<ObjManager>.Instance.ObjDict.Values);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
			{
				ObjNPC objNPC = list[i] as ObjNPC;
				if (objNPC.NPCDataID.Equals(npcId))
				{
					return objNPC;
				}
			}
		}
		return null;
	}

	public void OnClickZhuChengBtn()
	{
	}

	public void OnClickPingMingKuBtn()
	{
	}

	public void ChooseActivityObj(int actType, int subType, string actId)
	{
		if (mLoadingMapFlag)
		{
			mFindTargetFlag = true;
			mTargetType = actType;
			mTargetSubtype = subType;
			mTargetActId = actId;
			mTargetIsMission = false;
			return;
		}
		mFindTargetFlag = false;
		for (int i = 0; i < MapActivityObjList.Count; i++)
		{
			if (UnityVersionUtil.IsActive(MapActivityObjList[i].gameObject))
			{
				MapActivityObjList[i].UpdateSelection(actType, subType, actId);
			}
		}
		for (int j = 0; j < MissionActivityObjList.Count; j++)
		{
			if (UnityVersionUtil.IsActive(MissionActivityObjList[j].gameObject))
			{
				MissionActivityObjList[j].UpdateSelection(actType, subType, actId);
			}
		}
	}

	public void ChooseActivityObj(string actId, bool isMission)
	{
		if (mLoadingMapFlag)
		{
			mFindTargetFlag = true;
			mTargetType = -99;
			mTargetSubtype = -99;
			mTargetActId = actId;
			mTargetIsMission = isMission;
			return;
		}
		mFindTargetFlag = false;
		for (int i = 0; i < MapActivityObjList.Count; i++)
		{
			if (UnityVersionUtil.IsActive(MapActivityObjList[i].gameObject))
			{
				MapActivityObjList[i].UpdateSelection(actId, isMission);
			}
		}
		for (int j = 0; j < MissionActivityObjList.Count; j++)
		{
			if (UnityVersionUtil.IsActive(MissionActivityObjList[j].gameObject))
			{
				MissionActivityObjList[j].UpdateSelection(actId, isMission);
			}
		}
	}

	public void SetActivityObjNormal()
	{
		for (int i = 0; i < MapActivityObjList.Count; i++)
		{
			if (UnityVersionUtil.IsActive(MapActivityObjList[i].gameObject))
			{
				MapActivityObjList[i].SetNormalState();
			}
		}
		for (int j = 0; j < MissionActivityObjList.Count; j++)
		{
			if (UnityVersionUtil.IsActive(MissionActivityObjList[j].gameObject))
			{
				MissionActivityObjList[j].SetNormalState();
			}
		}
	}

	public void SetActivityObjDisable()
	{
		for (int i = 0; i < MapActivityObjList.Count; i++)
		{
			if (UnityVersionUtil.IsActive(MapActivityObjList[i].gameObject))
			{
				MapActivityObjList[i].SetDisActiveState();
			}
		}
		for (int j = 0; j < MissionActivityObjList.Count; j++)
		{
			if (UnityVersionUtil.IsActive(MissionActivityObjList[j].gameObject))
			{
				MissionActivityObjList[j].SetDisActiveState();
			}
		}
	}

	public void OnClickLocalMap()
	{
		if (mLoadingMapFlag || !CurMapInfo.ID.Equals(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID) || Player.AttributeData.Level <= 5)
		{
			return;
		}
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		if (instance.SceneManager.IsHaveMoveTarget)
		{
			instance.SceneManager.ClearMoveTarget();
			return;
		}
		Transform cachedTransform = LocalMapTexture.cachedTransform;
		Vector3 vector = MapPos2WorldPos(cachedTransform.worldToLocalMatrix.MultiplyPoint(UICamera.lastHit.point));
		if (SceneManager.IsInNavmeshArea(vector))
		{
			Player.StopAutoAndSkill();
			vector.y = SceneManager.GetHitHeight(vector);
			instance.MissionManager.StopAutoMoveToMission();
			Player.IsNeedAutoMountCar = true;
			if (Vector3.Distance(vector, Player.Position) / Player.NavMeshAgent.speed > 5f)
			{
				Player.EnterAutoMoving(Time.time);
			}
			else
			{
				Player.EnterAutoMoving(float.MaxValue);
			}
			instance.SceneManager.SetMoveTarget(vector, string.Empty);
			Player.MoveTo(vector, 1f, delegate
			{
				Player.IsNeedAutoMountCar = false;
			});
		}
	}

	private void OnDisable()
	{
		curMapInfo = null;
	}

	public void DrawMapLine(List<Vector3> posList, Vector3 playerPos, Vector3 targetPos)
	{
		PathLineRender.SetVertexCount(posList.Count);
		for (int i = 0; i < posList.Count; i++)
		{
			PathLineRender.SetPosition(i, WorldPos2MapPos(posList[i]));
		}
		WalkLineRender1.SetVertexCount(0);
		WalkLineRender2.SetVertexCount(0);
	}

	public void ClearMapLine()
	{
		PathLineRender.SetVertexCount(0);
		WalkLineRender1.SetVertexCount(0);
		WalkLineRender2.SetVertexCount(0);
	}

	public void OnlyDrawWalkLine(Vector3 playerPos, Vector3 targetPos)
	{
		WalkLineRender1.SetVertexCount(0);
		WalkLineRender2.SetVertexCount(0);
		PathLineRender.SetVertexCount(2);
		PathLineRender.SetPosition(0, WorldPos2MapPos(playerPos));
		PathLineRender.SetPosition(1, WorldPos2MapPos(targetPos));
	}

	public void UpdateNpcPos()
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (CurMapInfo == null || sceneManager == null || !sceneManager.CurrentMapInofData.ID.Equals(CurMapInfo.ID))
		{
			for (int i = 0; i < NpcPointPicList.Count; i++)
			{
				NpcPointPicList[i].enabled = false;
			}
			return;
		}
		List<ObjNPC> mapShowObjList = Singleton<ObjManager>.Instance.MapShowObjList;
		List<ObjNPC> list = new List<ObjNPC>();
		for (int j = 0; j < mapShowObjList.Count; j++)
		{
			if (mapShowObjList[j].IsCityCaptureNpc())
			{
				list.Add(mapShowObjList[j]);
			}
		}
		int num = list.Count - NpcPointPicList.Count;
		if (num > 0)
		{
			for (int k = 0; k < num; k++)
			{
				GameObject gameObject = Object.Instantiate(NpcPointPicList[0].gameObject) as GameObject;
				gameObject.transform.parent = NpcPointPicList[0].transform.parent;
				gameObject.transform.localScale = Vector3.one;
				NpcPointPicList.Add(gameObject.GetComponent<UISprite>());
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			NpcPointPicList[l].enabled = true;
			NpcPointPicList[l].spriteName = GameDefine.GetObjTypeSpriteName(list[l]);
			if (list[l].IsCityCaptureNpc())
			{
				NpcPointPicList[l].SetDimensions(20, 20);
			}
			else
			{
				NpcPointPicList[l].MakePixelPerfect();
			}
			NpcPointPicList[l].transform.localPosition = WorldPos2MapPos(list[l].transform.position);
		}
		for (int m = list.Count; m < NpcPointPicList.Count; m++)
		{
			NpcPointPicList[m].enabled = false;
		}
	}
}
