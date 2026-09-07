using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class LowPhoneSceneManager : SceneManager
{
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

	private string mStartSceneAnima = "Tutorial/kaiChangSceneAnima";

	public Transform TalkNpcPos;

	private ObjNPC mTalkingNpc;

	private Transform TutorialCreateNPCCamLookPos;

	private CameraController mCamCtl;

	private GameObject mCarAnimaObj;

	private List<ObjNPC> npcList = new List<ObjNPC>();

	public override void Init(string id)
	{
		base.Init(id);
		mWaveCount = GetMonsterGroupCount();
		mCurPlayerPathIndex = 1;
		mCurEnemyGroup = PlayerPrefs.GetInt("mCurEnemyGroup", 1);
		if (!SingletonUnity<MyEvent>.Exists)
		{
			Debug.Log("SingletonUnity<MyEvent>.Exists == false");
		}
		Debug.Log("hahahaha");
		OnMainPlayerCreate();
		CreateNPCGroup();
	}

	private void InitBlock(string sceneId)
	{
	}

	public override void OnLoadingOver()
	{
		base.OnLoadingOver();
	}

	public override void StartGame()
	{
		Debug.Log("CarChaseSceneManager StartGame");
	}

	public override void Update()
	{
	}

	public void OnMainPlayerCreate()
	{
		ObjInitPlayerData objInitPlayerData = new ObjInitPlayerData();
		objInitPlayerData.Profession = PROFESSION_TYPE.XD;
		attribute attribute = new attribute();
		attribute.anti_knock_down = 100L;
		attribute.anti_stun = 100L;
		attribute.atk = 76L;
		attribute.crd = 15000L;
		attribute.cri = 8L;
		attribute.crr = 0L;
		attribute.def = 15L;
		attribute.eva = 8L;
		attribute.exd = 0L;
		attribute.exp = 0L;
		attribute.exr = 8L;
		attribute.hit = 8L;
		attribute.max_hp = 491L;
		attribute.mov = 500L;
		attribute.rec = 1L;
		attribute.res = 0L;
		objInitPlayerData.Attribute = attribute;
		objInitPlayerData.Name = "Player";
		objInitPlayerData.HP = 491;
		attribute attribute2 = new attribute();
		attribute2.anti_knock_down = 0L;
		attribute2.anti_stun = 0L;
		attribute2.atk = 0L;
		attribute2.crd = 0L;
		attribute2.cri = 8L;
		attribute2.crr = 0L;
		attribute2.def = 0L;
		attribute2.eva = 0L;
		attribute2.exd = 0L;
		attribute2.exp = 0L;
		attribute2.exr = 0L;
		attribute2.hit = 0L;
		attribute2.max_hp = 0L;
		attribute2.mov = 0L;
		attribute2.rec = 1L;
		attribute2.res = 0L;
		objInitPlayerData.AttributeAll = attribute;
		attribute_other attribute_other = new attribute_other();
		attribute_other.camp = 0L;
		attribute_other.combValue = 3317L;
		attribute_other.dance_state = 0L;
		attribute_other.exp = 0L;
		attribute_other.hp = 491L;
		attribute_other.level = 1L;
		attribute_other.pkMode = 0L;
		objInitPlayerData.Level = 30;
		objInitPlayerData.ComboValue = 3317;
		objInitPlayerData.mPos = new Vector3(PlayerPrefs.GetFloat("pos_x", -24f), SceneManager.GetHitHeight(0f, 0f), PlayerPrefs.GetFloat("pos_z", 159f));
		objInitPlayerData.mCharacterModelId = "100";
		characterVisual characterVisual = new characterVisual();
		characterVisual.BodyId = "XD_A_S";
		characterVisual.HeadId = "XD_A_T";
		characterVisual.LegId = "XD_A_X";
		characterVisual.WeaponId = "XD_A_WQ";
		characterVisual.showType = 1L;
		characterVisual.ModeId = "100";
		objInitPlayerData.visual = characterVisual;
		Dictionary<string, skill_info> dictionary = new Dictionary<string, skill_info>();
		skill_info skill_info = new skill_info();
		skill_info.indexPos = 0L;
		skill_info.indexPos2 = 0L;
		skill_info.skillId = "101";
		skill_info.skillLevel = 0L;
		dictionary.Add(skill_info.skillId, skill_info);
		skill_info = new skill_info();
		skill_info.indexPos = 1L;
		skill_info.indexPos2 = 1L;
		skill_info.skillId = "102";
		skill_info.skillLevel = 0L;
		dictionary.Add(skill_info.skillId, skill_info);
		skill_info = new skill_info();
		skill_info.indexPos = 2L;
		skill_info.indexPos2 = 2L;
		skill_info.skillId = "103";
		skill_info.skillLevel = 0L;
		dictionary.Add(skill_info.skillId, skill_info);
		skill_info = new skill_info();
		skill_info.indexPos = 3L;
		skill_info.indexPos2 = 3L;
		skill_info.skillId = "104";
		skill_info.skillLevel = 0L;
		dictionary.Add(skill_info.skillId, skill_info);
		skill_info = new skill_info();
		skill_info.indexPos = 4L;
		skill_info.indexPos2 = 4L;
		skill_info.skillId = "105";
		skill_info.skillLevel = 0L;
		dictionary.Add(skill_info.skillId, skill_info);
		skill_info = new skill_info();
		skill_info.indexPos = 5L;
		skill_info.indexPos2 = 5L;
		skill_info.skillId = "106";
		skill_info.skillLevel = 0L;
		dictionary.Add(skill_info.skillId, skill_info);
		skill_info = new skill_info();
		skill_info.indexPos = 6L;
		skill_info.indexPos2 = 6L;
		skill_info.skillId = "107";
		skill_info.skillLevel = 0L;
		dictionary.Add(skill_info.skillId, skill_info);
		skill_info = new skill_info();
		skill_info.indexPos = 7L;
		skill_info.indexPos2 = 7L;
		skill_info.skillId = "108";
		skill_info.skillLevel = 0L;
		dictionary.Add(skill_info.skillId, skill_info);
		skill_info = new skill_info();
		skill_info.indexPos = 8L;
		skill_info.indexPos2 = 8L;
		skill_info.skillId = "109";
		skill_info.skillLevel = 0L;
		dictionary.Add(skill_info.skillId, skill_info);
		skill_info = new skill_info();
		skill_info.indexPos = 9L;
		skill_info.indexPos2 = 9L;
		skill_info.skillId = "110";
		skill_info.skillLevel = 0L;
		dictionary.Add(skill_info.skillId, skill_info);
		objInitPlayerData.skills = dictionary;
		Singleton<ObjManager>.Instance.CreateMainPlayer(objInitPlayerData);
	}

	public void MoveNextState()
	{
		CreateNPCGroup();
	}

	private void CreateNPCGroup()
	{
		NpcCreate(GetMonsterDataByGroup(mCurEnemyGroup));
	}

	private void NpcCreate(List<MonsterData> list)
	{
		if (list == null || list.Count == 0)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			ObjInitNpcData objInitNpcData = new ObjInitNpcData();
			MonsterData monsterData = list[i];
			objInitNpcData.mServerID = UUID.GenUUID();
			objInitNpcData.mPos = new Vector3(monsterData.PositionX, 0f, monsterData.PositionZ);
			objInitNpcData.mDir = MathUtil.HeadingToVector3((float)monsterData.PosO / 100f);
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

	public void SaveData()
	{
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer != null)
		{
			PlayerPrefs.SetFloat("pos_x", mainPlayer.Position.x);
			PlayerPrefs.SetFloat("pos_z", mainPlayer.Position.z);
		}
	}

	private void OnNPCCreated(ObjNPC npc)
	{
	}

	public override void OnNPCDie(object objNpc)
	{
		ObjNPC objNPC = objNpc as ObjNPC;
		npcList.Add(objNPC);
		if (!Singleton<ObjManager>.Instance.CheckNPCClear(objNPC))
		{
			return;
		}
		mCurEnemyGroup++;
		if (mCurEnemyGroup > mWaveCount)
		{
			mCurEnemyGroup = 1;
		}
		PlayerPrefs.SetInt("mCurEnemyGroup", mCurEnemyGroup);
		MoveNextState();
		vp_Timer.In(2f, delegate
		{
			for (int i = 0; i < npcList.Count; i++)
			{
				npcList[i].DelayRecycle();
			}
			npcList.Clear();
		});
	}
}
