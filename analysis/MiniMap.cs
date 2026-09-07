using System.Collections.Generic;
using UnityEngine;

public class MiniMap : SingletonUnity<MiniMap>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UILabel MapNameLabel;

	public GameObject MapPicObj;

	public GameObject MapNameObj;

	private float MapSize = 130f;

	private float mMapLength;

	private float mMapHeight;

	private float mMapWHRatio;

	private float mRadius;

	private float mMapShowRange;

	private Mesh mMapMesh;

	private Material mMapMat;

	private Vector2[] mTempUV;

	private Vector2[] mMapUV;

	public Transform PlayerPic;

	public Transform TargetPic;

	public Vector2 TargetPos;

	public Vector2 playerPos;

	private Transform mPlayerTransform;

	private ObjMainPlayer mMainPlayer;

	private Transform mCamTrans;

	public List<UISprite> NpcPointPicList = new List<UISprite>();

	public List<NewMiniMapActivityObj> ActionMapObjList = new List<NewMiniMapActivityObj>();

	private List<ActivityMapData> curActivityMapDataList;

	private List<string> curShowMissionList;

	private List<ActivityObjData> curMapObjDataList = new List<ActivityObjData>();

	public UILabel OnlineLabel;

	public UISprite ClickTipsSprite;

	public TweenColor PoliceTweenColor;

	public LineRenderer PathLineRender;

	public LineRenderer WalkLineRender1;

	public LineRenderer WalkLineRender2;

	private string curMapName = string.Empty;

	private string curMapId = string.Empty;

	public UIWidget TutorialClickSp;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	private new void Awake()
	{
		base.Awake();
		MeshFilter component = MapPicObj.GetComponent<MeshFilter>();
		if (component.mesh == null)
		{
			component.mesh = new Mesh();
		}
		mMapMesh = component.mesh;
		mMapMat = MapPicObj.renderer.sharedMaterial;
		GenerateMesh();
		UpdateNpcPos();
	}

	private void InitOnline()
	{
		int curLineIndex = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CurLineIndex;
		if (curLineIndex >= 0)
		{
			OnlineLabel.text = $"{curLineIndex}";
		}
		else
		{
			OnlineLabel.text = "-";
		}
	}

	public void Reset(float mapLength, float mapHeight, float showLength, string minimapName, string mapName)
	{
		curMapId = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID;
		if (curMapId.Equals("11"))
		{
			showLength = 100f;
		}
		mMapLength = mapLength;
		mMapHeight = mapHeight;
		mMapShowRange = showLength / mapLength / 2f * 1.5f;
		mRadius = mMapShowRange;
		mMapWHRatio = mMapLength / mMapHeight;
		mMapMat.SetFloat("_Radius", mRadius);
		mMapMat.SetFloat("_WHRatio", mMapWHRatio);
		mMapMat.SetFloat("_OutLine", mRadius * 0.01f);
		float value = showLength / mapLength / 3f * 1.5f;
		mMapMat.SetFloat("_PoliceRadius", value);
		SetPoliceAlph(0f);
		MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(curMapId);
		mMapMat.SetFloat("_LockShowRange", 1.1f);
		if (mMapMat.mainTexture == null || !mMapMat.mainTexture.name.Equals(minimapName))
		{
			curMapName = minimapName;
			if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(minimapName, TextureLoadFinish));
				mMapMat.SetTexture("_AreaTipTex", null);
			}
		}
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		mPlayerTransform = mMainPlayer.CacheTransform;
		mCamTrans = Singleton<ObjManager>.Instance.MainPlayer.CameraController.transform;
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsHaveMoveTarget)
		{
			UnityVersionUtil.SetActiveRecursive(TargetPic.gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(TargetPic.gameObject, state: false);
		}
		MapNameLabel.text = StrDictionary.GetDictionaryString(mapName);
		InitOnline();
		UpdateMapName();
		UpdateActionMapObj();
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsCopyScene())
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene())
			{
				ClickTipsSprite.enabled = true;
			}
			else
			{
				ClickTipsSprite.enabled = false;
			}
		}
		else
		{
			ClickTipsSprite.enabled = true;
		}
	}

	private void TextureLoadFinish(string name, Texture curtex)
	{
		if (string.IsNullOrEmpty(curMapName) || curMapName.Equals(name))
		{
			mMapMat.SetTexture("_MainTex", curtex);
		}
	}

	private void UpdateMapName()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsCopyShowMap())
		{
			UnityVersionUtil.SetActiveRecursive(MapNameObj, state: false);
		}
	}

	public void OnClickMapBtn()
	{
		if (GameSettingData.IsLowPhone)
		{
			return;
		}
		if (SingletonUnity<TutorialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.CloseCheck();
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.MapType == MAPTYPE.CAR_CHASE_COPY)
		{
			return;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsCopyScene() && !SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MapUIRoot, delegate
			{
				SingletonUnity<MapUIRootLogic>.Instance.Reset();
			});
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
		{
			SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
			if (TutorialManager.CurStep == TUTORIAL_STEP.NEW_MAP_TIP_START || TutorialManager.CurStep == TUTORIAL_STEP.CAR_COPY_START || TutorialManager.CurStep == TUTORIAL_STEP.EXP_COPY_START || TutorialManager.CurStep == TUTORIAL_STEP.GOLD_COPY_START || TutorialManager.CurStep == TUTORIAL_STEP.TOWER_START || TutorialManager.CurStep == TUTORIAL_STEP.RANK_PVP_START || TutorialManager.CurStep == TUTORIAL_STEP.EQUIP_COPY_START || TutorialManager.CurStep == TUTORIAL_STEP.SCUFFLE_COPY_START || TutorialManager.CurStep == TUTORIAL_STEP.CAPTURE_START || TutorialManager.CurStep == TUTORIAL_STEP.WORLD_BOSS_START || TutorialManager.CurStep == TUTORIAL_STEP.SURVIVAL_BATTLE_START || TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_START || TutorialManager.CurStep == TUTORIAL_STEP.ROBBORY_START)
			{
				CheckTutorialEvent();
			}
		});
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("MainUI", "BtnClick", "minimapBtn");
	}

	private void UpdateNpcPos()
	{
		List<ObjNPC> mapShowObjList = Singleton<ObjManager>.Instance.MapShowObjList;
		int num = mapShowObjList.Count - NpcPointPicList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(NpcPointPicList[0].gameObject) as GameObject;
				gameObject.transform.parent = NpcPointPicList[0].transform.parent;
				gameObject.transform.localScale = Vector3.one;
				NpcPointPicList.Add(gameObject.GetComponent<UISprite>());
			}
		}
		for (int j = 0; j < mapShowObjList.Count; j++)
		{
			Vector3 position = mapShowObjList[j].Position;
			Vector2 vector = new Vector2((position.x + mMapLength / 2f) / mMapLength, (position.z + mMapHeight / 2f) / mMapHeight);
			Vector2 vector2 = vector - playerPos;
			float num2 = mRadius * 0.92f;
			if (vector2.sqrMagnitude < num2 * num2)
			{
				NpcPointPicList[j].enabled = true;
				NpcPointPicList[j].spriteName = GameDefine.GetObjTypeSpriteName(mapShowObjList[j]);
				if (mapShowObjList[j].IsCityCaptureNpc())
				{
					NpcPointPicList[j].SetDimensions(20, 20);
				}
				else
				{
					NpcPointPicList[j].MakePixelPerfect();
				}
				NpcPointPicList[j].transform.localPosition = vector2 / mRadius * MapSize / 2f;
				if (mapShowObjList[j].IsCityCaptureNpc())
				{
					NpcPointPicList[j].transform.localEulerAngles = new Vector3(0f, 0f, 0f - NpcPointPicList[0].transform.parent.localEulerAngles.z);
				}
			}
			else
			{
				NpcPointPicList[j].enabled = false;
			}
		}
		for (int k = mapShowObjList.Count; k < NpcPointPicList.Count; k++)
		{
			NpcPointPicList[k].enabled = false;
		}
	}

	private Vector2 WorldPos2MapPos(Vector3 pos)
	{
		Vector2 vector = new Vector2((pos.x + mMapLength / 2f) / mMapLength, (pos.z + mMapHeight / 2f) / mMapHeight);
		Vector2 vector2 = vector - playerPos;
		return vector2 / mRadius * MapSize / 2f;
	}

	public void UpdateActionMapObj()
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		curActivityMapDataList = sceneManager.CurActivityMapDataList;
		List<string> curAvailableMissionIdList = sceneManager.CurAvailableMissionIdList;
		Dictionary<string, List<string>> curMissionNpcDic = sceneManager.CurMissionNpcDic;
		curShowMissionList = new List<string>();
		List<string> list = new List<string>(curMissionNpcDic.Keys);
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		curMapObjDataList.Clear();
		string empty = string.Empty;
		List<string> list2 = null;
		string empty2 = string.Empty;
		MISSION_STATE mISSION_STATE = MISSION_STATE.INVALID;
		for (int i = 0; i < list.Count; i++)
		{
			empty = list[i];
			list2 = curMissionNpcDic[empty];
			empty2 = string.Empty;
			if (list2 == null || list2.Count <= 0)
			{
				continue;
			}
			mISSION_STATE = MISSION_STATE.INVALID;
			empty2 = list2[0];
			for (int j = 0; j < list2.Count; j++)
			{
				mISSION_STATE = missionManager.GetMissionState(list2[j]);
				switch (mISSION_STATE)
				{
				case MISSION_STATE.COMPLETE:
					break;
				case MISSION_STATE.ACCEPTED:
					empty2 = list2[j];
					continue;
				default:
					continue;
				}
				empty2 = list2[j];
				break;
			}
			curShowMissionList.Add(empty2);
			curMapObjDataList.Add(new ActivityObjData(empty2, mISSION_STATE, DataManager.GetNPCPosInMonsterData(curMapId, empty)));
		}
		MissionData missionData = null;
		for (int k = 0; k < curAvailableMissionIdList.Count; k++)
		{
			if (curShowMissionList.Contains(curAvailableMissionIdList[k]))
			{
				continue;
			}
			mISSION_STATE = missionManager.GetMissionState(curAvailableMissionIdList[k]);
			if (mISSION_STATE == MISSION_STATE.ACCEPTED)
			{
				missionData = DataManager.GetMissionDataByID(curAvailableMissionIdList[k]);
				if (missionData.MissionLogicType == MISSION_LOGICTYPE.SURVEY)
				{
					SurveyMissionData surveyMissionDataById = DataManager.GetSurveyMissionDataById(missionData.LogicID);
					curMapObjDataList.Add(new ActivityObjData(curAvailableMissionIdList[k], mISSION_STATE, new Vector3(surveyMissionDataById.PosX, 0f, surveyMissionDataById.PosZ)));
					curShowMissionList.Add(curAvailableMissionIdList[k]);
				}
			}
		}
		for (int l = 0; l < curActivityMapDataList.Count; l++)
		{
			if (curActivityMapDataList[l].IsVisible)
			{
				curMapObjDataList.Add(new ActivityObjData(curActivityMapDataList[l]));
			}
		}
		int num = curMapObjDataList.Count - ActionMapObjList.Count;
		if (num > 0)
		{
			for (int m = 0; m < num; m++)
			{
				GameObject gameObject = Object.Instantiate(ActionMapObjList[0].gameObject) as GameObject;
				gameObject.transform.parent = ActionMapObjList[0].transform.parent;
				gameObject.transform.localScale = Vector3.one;
				ActionMapObjList.Add(gameObject.GetComponent<NewMiniMapActivityObj>());
			}
		}
		UpdateActionMapObjPos();
	}

	private void UpdateActionMapObjPos()
	{
		for (int i = 0; i < ActionMapObjList.Count; i++)
		{
			if (i < curMapObjDataList.Count)
			{
				Vector3 position = curMapObjDataList[i].Position;
				Vector2 vector = new Vector2((position.x + mMapLength / 2f) / mMapLength, (position.z + mMapHeight / 2f) / mMapHeight);
				Vector2 vector2 = vector - playerPos;
				float num = mRadius * 0.85f;
				if (vector2.sqrMagnitude < num * num)
				{
					NGUITools.SetActive(ActionMapObjList[i].gameObject, state: true);
					ActionMapObjList[i].Reset(curMapObjDataList[i].GetIcon(), curMapObjDataList[i].GetStateIcon(), curMapObjDataList[i].IsMission());
					ActionMapObjList[i].transform.localPosition = vector2 / mRadius * MapSize / 2f;
				}
				else
				{
					NGUITools.SetActive(ActionMapObjList[i].gameObject, state: false);
				}
			}
			else
			{
				NGUITools.SetActive(ActionMapObjList[i].gameObject, state: false);
			}
		}
	}

	private void Update()
	{
		UpdatePlayerPos();
	}

	public void SetTarget(Vector3 pos)
	{
		Vector2 target = new Vector2((pos.x + mMapLength / 2f) / mMapLength, (pos.z + mMapHeight / 2f) / mMapHeight);
		SetTarget(target);
	}

	public void SetTarget(Vector2 pos)
	{
		if (!UnityVersionUtil.IsActive(TargetPic.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(TargetPic.gameObject, state: true);
		}
		TargetPos = pos;
	}

	public void HideTarget()
	{
		UnityVersionUtil.SetActiveRecursive(TargetPic.gameObject, state: false);
	}

	private void UpdateTargetPos()
	{
		if (UnityVersionUtil.IsActive(TargetPic.gameObject))
		{
			Vector2 vector = TargetPos - playerPos;
			if (vector.sqrMagnitude > mRadius * mRadius)
			{
				TargetPic.transform.localPosition = vector.normalized * MapSize / 2f;
			}
			else
			{
				TargetPic.transform.localPosition = vector / mRadius * MapSize / 2f;
			}
			TargetPic.transform.localPosition -= Vector3.forward * 5f;
			TargetPic.transform.localEulerAngles = new Vector3(0f, 0f, 0f - mCamTrans.eulerAngles.y);
		}
	}

	public void UpdatePlayerPos()
	{
		playerPos = new Vector2((mPlayerTransform.position.x + mMapLength / 2f) / mMapLength, (mPlayerTransform.position.z + mMapHeight / 2f) / mMapHeight);
		SetMapCenterPos(playerPos);
		if (mMainPlayer.IsLocalDrivingCar)
		{
			PlayerPic.localEulerAngles = new Vector3(0f, 0f, 0f - mMainPlayer.CurPlayerCar.transform.eulerAngles.y);
		}
		else
		{
			PlayerPic.localEulerAngles = new Vector3(0f, 0f, 0f - mPlayerTransform.eulerAngles.y);
		}
		MapPicObj.transform.localEulerAngles = new Vector3(0f, 0f, mCamTrans.eulerAngles.y);
		UpdateTargetPos();
		UpdateNpcPos();
		UpdateActionMapObjPos();
	}

	public void SetMapUnlockAlph(float unlockAlph)
	{
		mMapMat.SetFloat("_LockShowRange", unlockAlph);
	}

	public void SetMapCenterPos(Vector2 pos)
	{
		Vector2 vector = new Vector2(pos.x - mMapShowRange, pos.y - mMapShowRange * mMapWHRatio);
		Vector2 zero = Vector2.zero;
		for (int i = 0; i < mMapUV.Length; i++)
		{
			zero = mMapUV[i];
			zero.x = zero.x * mMapShowRange * 2f + vector.x;
			zero.y = zero.y * mMapShowRange * 2f * mMapWHRatio + vector.y;
			mTempUV[i] = zero;
		}
		mMapMesh.uv = mTempUV;
		mMapMat.SetFloat("_CenterX", pos.x);
		mMapMat.SetFloat("_CenterY", pos.y);
		mMapMat.SetFloat("_PoliceCenterX", pos.x);
		mMapMat.SetFloat("_PoliceCenterY", pos.y);
	}

	public void SetPoliceAlph(float alph)
	{
		mMapMat.SetColor("_PoliceColor", new Color(1f, 0f, 0f, alph));
	}

	private void GenerateMesh()
	{
		mMapMesh.Clear();
		Vector3[] array = new Vector3[4];
		int[] array2 = new int[6];
		Vector3[] array3 = new Vector3[4];
		mMapUV = new Vector2[4];
		float num = MapSize / 2f;
		ref Vector3 reference = ref array[0];
		reference = new Vector3(base.transform.position.x - num, base.transform.position.y - num, 0f);
		ref Vector3 reference2 = ref array[1];
		reference2 = new Vector3(base.transform.position.x + num, base.transform.position.y - num, 0f);
		ref Vector3 reference3 = ref array[2];
		reference3 = new Vector3(base.transform.position.x + num, base.transform.position.y + num, 0f);
		ref Vector3 reference4 = ref array[3];
		reference4 = new Vector3(base.transform.position.x - num, base.transform.position.y + num, 0f);
		mMapMesh.vertices = array;
		array2[0] = 0;
		array2[1] = 2;
		array2[2] = 1;
		array2[3] = 0;
		array2[4] = 3;
		array2[5] = 2;
		mMapMesh.triangles = array2;
		ref Vector3 reference5 = ref array3[0];
		reference5 = -Vector3.forward;
		ref Vector3 reference6 = ref array3[1];
		reference6 = -Vector3.forward;
		ref Vector3 reference7 = ref array3[2];
		reference7 = -Vector3.forward;
		ref Vector3 reference8 = ref array3[3];
		reference8 = -Vector3.forward;
		mMapMesh.normals = array3;
		ref Vector2 reference9 = ref mMapUV[0];
		reference9 = new Vector2(0f, 0f);
		ref Vector2 reference10 = ref mMapUV[1];
		reference10 = new Vector2(1f, 0f);
		ref Vector2 reference11 = ref mMapUV[2];
		reference11 = new Vector2(1f, 1f);
		ref Vector2 reference12 = ref mMapUV[3];
		reference12 = new Vector2(0f, 1f);
		mMapMesh.uv = mMapUV;
		mTempUV = new Vector2[mMapUV.Length];
	}

	public Vector2 GetLineCirclePos(Vector2 pos1, Vector2 pos2, float radius_2)
	{
		float num = (pos2.y - pos1.y) / (pos2.x - pos1.x);
		float num2 = pos1.y - num * pos1.x;
		float num3 = num * num;
		float num4 = num2 * num2;
		float num5 = Mathf.Sqrt(num3 * num4 - (1f + num3) * (num4 - radius_2));
		float num6 = ((0f - num) * num2 + num5) / (1f + num3);
		float num7 = 0f;
		num7 = ((!((num6 - pos1.x) * (num6 - pos2.x) < 0f)) ? (((0f - num) * num2 - num5) / (1f + num3)) : num6);
		float y = num * num7 + num2;
		return new Vector2(num7, y);
	}

	public void DrawMapLine(List<Vector3> posList, Vector3 targetPos)
	{
		List<Vector3> list = new List<Vector3>();
		float num = MapSize / 2f - 5f;
		num *= num;
		bool flag = true;
		for (int i = 0; i < posList.Count; i++)
		{
			Vector2 vector = WorldPos2MapPos(posList[i]);
			if (vector.sqrMagnitude < num)
			{
				list.Add(vector);
			}
			else if (list.Count > 0 && flag)
			{
				flag = false;
				list.Add(GetLineCirclePos(list[list.Count - 1], vector, num));
			}
		}
		PathLineRender.SetVertexCount(list.Count);
		for (int j = 0; j < list.Count; j++)
		{
			PathLineRender.SetPosition(j, list[j]);
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
}
