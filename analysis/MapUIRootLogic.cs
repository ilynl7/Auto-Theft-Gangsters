using System;
using System.Collections.Generic;
using System.Text;
using SprotoType;
using UnityEngine;

public class MapUIRootLogic : SingletonUnity<MapUIRootLogic>
{
	public enum SHOW_TYPE
	{
		FUNCTION,
		MONSTER
	}

	public float FlashInterval;

	public GameObject WorldMapRoot;

	public GameObject LocalMapRoot;

	public GameObject RightBtnRoot;

	public UILabel PlayerPosLabel;

	public Transform LocalMapPlayerPic;

	public UITexture LocalMapTexture;

	public UITexture WorldMapPic;

	private ObjMainPlayer Player;

	private MapInfoData curLocalMapInfo;

	public List<UISprite> MonsterPointPic;

	public List<UISprite> TeleportPointPic;

	public List<UISprite> SurviveTelPic;

	public List<UISprite> NPCPointPic;

	public List<UISprite> NPC2PointPic;

	public List<UISprite> NPC3PointPic;

	public List<WorldItemLogic> worldItemLogicList = new List<WorldItemLogic>();

	private List<MapPoint> curSceneTelePortInfo = new List<MapPoint>();

	private List<MapPoint> curSceneMonsterData = new List<MapPoint>();

	private List<MapPoint> curSceneNPCData = new List<MapPoint>();

	private List<MapPoint> curShowPointList = new List<MapPoint>();

	public List<MapBtnLine> mapRightBtnLineList = new List<MapBtnLine>();

	public List<MapBtnLine> mapLeftBtnLineList = new List<MapBtnLine>();

	public int LineCount = 5;

	public UILabel OnLineINfoLabel;

	private bool mLocalMapFlag = true;

	public GameObject[] WorldMapBtnRoot;

	public UIWrapContentNew UIWrapContent;

	public UIScrollView ScrollView;

	public UIWidget WrapContentBottomWidget;

	public UILabel FunctionLabel;

	public UILabel MonsterLabel;

	public UISprite SelectSprite;

	public GameObject WorldBtnObj;

	public GameObject LineObj;

	private SHOW_TYPE curShowType;

	private float MaxMapWidth = 350f;

	private float MaxMapHeight = 350f;

	private float intervalCount;

	private StringBuilder mTempStr = new StringBuilder();

	protected override void Awake()
	{
		base.Awake();
		Player = Singleton<ObjManager>.Instance.MainPlayer;
		InitTexture();
		UIWrapContent.enabled = false;
		UIWrapContentNew uIWrapContent = UIWrapContent;
		uIWrapContent.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContent.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
		curSceneTelePortInfo.Clear();
		curSceneMonsterData.Clear();
		curSceneNPCData.Clear();
		InitMonsterPointPic();
		InitTeleportPointPic();
		UpdateMissionPointPic();
		UpdateRightBtn();
		InitLeftBtn();
	}

	public void InitTexture()
	{
		curLocalMapInfo = DataManager.GetMapInfoDataByID(SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr);
		List<string> list = new List<string>();
		if (LocalMapTexture.mainTexture == null || !LocalMapTexture.mainTexture.name.Equals(curLocalMapInfo.MiniMapName))
		{
			list.Add(curLocalMapInfo.MiniMapName);
		}
		if (WorldMapPic.mainTexture == null)
		{
			list.Add(GameDefine.WorldMap);
		}
		if (list.Count != 0 && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(list, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(Dictionary<string, Texture> retdic)
	{
		if (retdic.Count == 0)
		{
			return;
		}
		curLocalMapInfo = DataManager.GetMapInfoDataByID(SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr);
		if (retdic.ContainsKey(curLocalMapInfo.MiniMapName))
		{
			Texture texture = retdic[curLocalMapInfo.MiniMapName];
			if (texture != null)
			{
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
				Debug.LogWarning("no mini map1 :" + curLocalMapInfo.MiniMapName);
			}
		}
		else
		{
			Debug.LogWarning("no mini map2 :" + curLocalMapInfo.MiniMapName);
		}
		if (retdic.ContainsKey(GameDefine.WorldMap))
		{
			WorldMapPic.mainTexture = retdic[GameDefine.WorldMap];
		}
		InitTeleportPointPic();
		InitMonsterPointPic();
		UpdateMissionPointPic();
	}

	public void OnClickFuncitonTab()
	{
		if (curShowType != 0)
		{
			curShowType = SHOW_TYPE.FUNCTION;
			UpdateRightBtn();
		}
	}

	public void OnClickMonsterTab()
	{
		if (curShowType != SHOW_TYPE.MONSTER)
		{
			curShowType = SHOW_TYPE.MONSTER;
			UpdateRightBtn();
		}
	}

	public void OnClickChangeLineBtn()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsCopyShowMap())
		{
			OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MapLineInfoLogic, delegate
			{
				SingletonUnity<MapLineInfoLogic>.Instance.PreReset();
				NetLogic.GetInstance().Send<Protocol.request_line_state>();
			});
		}
	}

	private void InitLeftBtn()
	{
		int curLineIndex = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CurLineIndex;
		int lineCount = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.LineCount;
		OnLineINfoLabel.text = $"line{curLineIndex}";
	}

	private int GetMapBtnIndex(int RunningMapId)
	{
		return (GameDefine.SCENE_DEFINE)RunningMapId switch
		{
			GameDefine.SCENE_DEFINE.SCENE_MAIN_CITY => 0, 
			GameDefine.SCENE_DEFINE.SCENE_SLUM_CITY => 1, 
			_ => 0, 
		};
	}

	private void UpdateCopyBtn()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsCopyShowMap())
		{
			UnityVersionUtil.SetActiveRecursive(WorldBtnObj, state: false);
			UnityVersionUtil.SetActiveRecursive(LineObj, state: false);
		}
	}

	public void Reset()
	{
		UnityVersionUtil.SetActiveRecursive(WorldMapRoot, state: false);
		UnityVersionUtil.SetActiveRecursive(LocalMapRoot, state: true);
		UnityVersionUtil.SetActiveRecursive(RightBtnRoot, state: true);
		mLocalMapFlag = true;
		UpdateMissionPointPic();
		UpdateRightBtn();
		InitLeftBtn();
		UpdateCopyBtn();
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
			if (currentMapInofData.ID == mapId && curLineIndex == num)
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
				GameObject gameObject = UnityEngine.Object.Instantiate(NPC2PointPic[0].gameObject) as GameObject;
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
				GameObject gameObject = UnityEngine.Object.Instantiate(NPC3PointPic[0].gameObject) as GameObject;
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
		List<TimerActivityData> enableTimerActivityList = DataManager.GetEnableTimerActivityList();
		List<ActivityBossData> list3 = new List<ActivityBossData>();
		ActivityBossData activityBossData = null;
		if (enableTimerActivityList != null)
		{
			for (int num2 = 0; num2 < enableTimerActivityList.Count; num2++)
			{
				activityBossData = DataManager.GetActivityBossDataByActivityIdMapId(enableTimerActivityList[num2].ID, sceneManager.CurrentMapInofData.ID);
				if (activityBossData != null)
				{
					list3.Add(activityBossData);
				}
			}
			for (int num3 = 0; num3 < list3.Count; num3++)
			{
				for (int num4 = 0; num4 < list3[num3].NpcPosList.Count; num4++)
				{
					list.Add(list3[num3].NpcPosList[num4]);
				}
			}
		}
		int num5 = list.Count - MonsterPointPic.Count;
		if (num5 > 0)
		{
			for (int num6 = 0; num6 < num5; num6++)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(MonsterPointPic[0].gameObject) as GameObject;
				gameObject2.transform.parent = MonsterPointPic[0].transform.parent;
				MonsterPointPic.Add(gameObject2.GetComponent<UISprite>());
			}
		}
		for (int num7 = 0; num7 < MonsterPointPic.Count; num7++)
		{
			MonsterPointPic[num7].enabled = num7 < list.Count;
		}
		for (int num8 = 0; num8 < list.Count; num8++)
		{
			MonsterPointPic[num8].transform.localPosition = WorldPos2MapPos(list[num8]);
		}
	}

	private void InitTeleportPointPic()
	{
		TeleportPointPic[0].enabled = false;
		SurviveTelPic[0].alpha = 0f;
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.IsCopyShowMap())
		{
			if (sceneManager.IsSurviveBattleScene())
			{
				SurviveTelPic[0].alpha = 1f;
				curSceneTelePortInfo.Clear();
				MapInfoData currentMapInofData = sceneManager.CurrentMapInofData;
				int num = currentMapInofData.TeleportPosList.Count - SurviveTelPic.Count;
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(SurviveTelPic[0].gameObject) as GameObject;
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
			MapInfoData currentMapInofData2 = sceneManager.CurrentMapInofData;
			MapPoint mapPoint2 = new MapPoint();
			mapPoint2.PointName = StrDictionary.GetDictionaryString("#{101809}");
			mapPoint2.PointPos = currentMapInofData2.TelePortPosVector3;
			mapPoint2.PointType = MAP_POINT_TYPE.TELEPORT;
			mapPoint2.NpcId = string.Empty;
			curSceneTelePortInfo.Add(mapPoint2);
			TeleportPointPic[0].transform.localPosition = WorldPos2MapPos(currentMapInofData2.TelePortPosVector3);
		}
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		MapBtnLine itemLogic = mapRightBtnLineList[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex));
	}

	private void ResetItemLine(MapBtnLine itemLogic, int idx)
	{
		if (idx < curShowPointList.Count)
		{
			itemLogic.ResetRightBtn(curShowPointList[idx]);
		}
	}

	private void UpdateRightBtn()
	{
		int num = 0;
		if (curShowType == SHOW_TYPE.FUNCTION)
		{
			SelectSprite.transform.position = FunctionLabel.transform.position;
			curShowPointList.Clear();
			curShowPointList.AddRange(curSceneNPCData);
			curShowPointList.AddRange(curSceneTelePortInfo);
		}
		else
		{
			SelectSprite.transform.position = MonsterLabel.transform.position;
			curShowPointList.Clear();
			curShowPointList.AddRange(curSceneMonsterData);
		}
		num = curShowPointList.Count;
		int num2 = Mathf.Min(num, LineCount) - mapRightBtnLineList.Count;
		for (int i = 0; i < num2; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(mapRightBtnLineList[0].gameObject) as GameObject;
			gameObject.name = $"item_{mapRightBtnLineList.Count}";
			gameObject.transform.parent = mapRightBtnLineList[0].transform.parent;
			gameObject.transform.localScale = Vector3.one;
			mapRightBtnLineList.Add(gameObject.GetComponent<MapBtnLine>());
		}
		for (int j = 0; j < mapRightBtnLineList.Count; j++)
		{
			NGUITools.SetActive(mapRightBtnLineList[j].gameObject, j < curShowPointList.Count);
			if (j < curShowPointList.Count)
			{
				ResetItemLine(mapRightBtnLineList[j], j);
			}
		}
		UIWrapContent.maxIndex = 0;
		UIWrapContent.minIndex = 1 - curShowPointList.Count;
		WrapContentBottomWidget.height = curShowPointList.Count * UIWrapContent.itemSize;
		UIWrapContent.SortBasedOnScrollMovement();
		UIWrapContent.enabled = true;
		ScrollView.ResetPosition();
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MapUIRoot);
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
		if (curLocalMapInfo != null)
		{
			LocalMapPlayerPic.localPosition = WorldPos2MapPos(Player.Position);
			LocalMapPlayerPic.localEulerAngles = new Vector3(0f, 0f, 0f - Player.transform.localEulerAngles.y);
			mTempStr.Length = 0;
			PlayerPosLabel.text = mTempStr.AppendFormat("{0},{1}", (int)Player.Position.x, (int)Player.Position.z).ToString();
			UpdateMissionPointPic();
		}
	}

	private Vector3 WorldPos2MapPos(Vector3 pos)
	{
		return new Vector3(pos.x / curLocalMapInfo.fMapLength * (float)LocalMapTexture.width, pos.z / curLocalMapInfo.fMapHeight * (float)LocalMapTexture.height, 0f);
	}

	private Vector3 WorldPos2MapPos(float posX, float posZ)
	{
		return new Vector3(posX / curLocalMapInfo.fMapLength * (float)LocalMapTexture.width, posZ / curLocalMapInfo.fMapHeight * (float)LocalMapTexture.height, 0f);
	}

	private Vector3 MapPos2WorldPos(Vector3 pos)
	{
		return new Vector3(pos.x / (float)LocalMapTexture.width * curLocalMapInfo.fMapLength, 0f, pos.y / (float)LocalMapTexture.height * curLocalMapInfo.fMapHeight);
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

	public void OnClickChangeMapBtn()
	{
		if (mLocalMapFlag)
		{
			mLocalMapFlag = false;
		}
		else
		{
			mLocalMapFlag = true;
		}
		ChangeToLocalMap(mLocalMapFlag);
	}

	public void ChangeToLocalMap(bool isTrue)
	{
		mLocalMapFlag = isTrue;
		if (isTrue)
		{
			UnityVersionUtil.SetActiveRecursive(WorldMapRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(LocalMapRoot, state: true);
			UnityVersionUtil.SetActiveRecursive(RightBtnRoot, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(WorldMapRoot, state: true);
			UnityVersionUtil.SetActiveRecursive(LocalMapRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(RightBtnRoot, state: false);
			InitWorld();
		}
	}

	public void OnClickLocalMap()
	{
		Transform cachedTransform = LocalMapTexture.cachedTransform;
		Vector3 vector = MapPos2WorldPos(cachedTransform.worldToLocalMatrix.MultiplyPoint(UICamera.lastHit.point));
		Player.StopAutoAndSkill();
		vector.y = SceneManager.GetHitHeight(vector);
		SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.StopAutoMoveToMission();
		Player.IsNeedAutoMountCar = true;
		if (Vector3.Distance(vector, Player.Position) / Player.NavMeshAgent.speed > 5f)
		{
			Player.EnterAutoMoving(Time.time);
		}
		else
		{
			Player.EnterAutoMoving(float.MaxValue);
		}
		Player.MoveTo(vector, 1f, delegate
		{
			Player.IsNeedAutoMountCar = false;
		});
	}

	private void InitWorld()
	{
		List<MapInfoData> mapInfoDataListByType = DataManager.GetMapInfoDataListByType(MAPTYPE.BIG_WORLD);
		mapInfoDataListByType.Add(DataManager.GetMapInfoDataListByType(MAPTYPE.TUTORIAL_CAR)[0]);
		int num = mapInfoDataListByType.Count - worldItemLogicList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(worldItemLogicList[0].gameObject) as GameObject;
				WorldItemLogic component = gameObject.GetComponent<WorldItemLogic>();
				gameObject.transform.parent = worldItemLogicList[0].gameObject.transform.parent;
				worldItemLogicList.Add(component);
			}
		}
		for (int j = 0; j < mapInfoDataListByType.Count; j++)
		{
			worldItemLogicList[j].ResetItem(mapInfoDataListByType[j]);
		}
	}

	public void OnClickZhuChengBtn()
	{
	}

	public void OnClickPingMingKuBtn()
	{
	}
}
