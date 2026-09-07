using System.Collections.Generic;
using DG.Tweening;
using SprotoType;
using UnityEngine;

public class CitySimController : SingletonUnity<CitySimController>
{
	public BlockLine[] BlockData;

	public List<CityPathPointData> PointDataList;

	public Vector2 CityLeftBottomPos = new Vector2(-800f, -800f);

	public Vector2 CitySize = new Vector2(1600f, 1600f);

	public int BlockLength = 50;

	private int NpcNumCount = 10;

	private int CarNumCount = 5;

	public float NpcCreateDis = 40f;

	public float CarCreateDis = 50f;

	private float PoliceCheckDis = 10f;

	private List<string> NpcIdList = new List<string> { "9906", "9907", "9908" };

	private List<string> PoliceIdList = new List<string> { "9909" };

	private float flashTimeCount = 5f;

	private float recycleTimeCount;

	private float posTimeCount;

	private Transform mMainPlayerTrans;

	private ObjMainPlayer mMainPlayer;

	private List<ObjRagdollNPC> mEnableRagdollList;

	private List<ObjFakeAICar> mEnableFakeAICarList;

	private List<ObjZombieRagdollPlayer> mEnableObjZombiePlayerList = new List<ObjZombieRagdollPlayer>();

	private ObjManager mObjManager;

	private float mFlashNpcSqrDis = 25f;

	private Vector3 preCreateNpcPos = Vector3.zero;

	public float mNpcRecycleDis = 50f;

	public float mCarRecycleDis = 60f;

	private Dictionary<long, ObjRagdollNPC> mEnablePoliceDic = new Dictionary<long, ObjRagdollNPC>();

	private List<ObjNPC> mEnablePoliceList = new List<ObjNPC>();

	private ObjFakeAICar mCurNearestCar;

	private SmoothFollowNew mSmoothFollowCamPos;

	private List<BlockData> mPreCheckBlockList = new List<BlockData>();

	private Dictionary<long, BlockCarData> mStaticCarDic = new Dictionary<long, BlockCarData>();

	private bool mIsHaveSpecialCar;

	public Vector3 SpecialCarPos;

	public Vector3 SpecialCarAngle;

	private string mSpecialCarId = string.Empty;

	private long mSpecialCarServerId = -1L;

	private PoliceLevelData mPoliceLevelData;

	private List<string> mBeforDownloadNpcModelIdList = new List<string>
	{
		"NPC_Nv_013", "NPC_Nv_014", "NPC_Nan_005", "NPC_Nan_013", "NPC_Nan_017", "NPC_Nan_022", "NPC_Nan_047", "NPC_Nan_036", "NPC_Nv_002", "NPC_Nv_004",
		"NPC_Nv_005", "XD_A_S", "XD_A_WQ", "XD_A_T", "XD_A_X"
	};

	private List<string> mPoliceCreateList = new List<string>();

	private List<LocationData> mPreNpcLocationData = new List<LocationData>();

	private string[] FakeAICarName = new string[17]
	{
		"Chevrolet", "chuZuChe", "daKeChe", "jiaoChe_01", "jiaoChe_02", "jingChe", "xiaoKeChe", "jiaChangChe_01", "jiaoChe_04", "jiaoChe_05",
		"jingChe_02", "jiPuChe_02", "jiPuChe_03", "kaChe_03", "kaChe_04", "qingJieChe_01", "xiaoFangChe_01"
	};

	private string[] BeforeDownloadCarId = new string[7] { "Chevrolet", "jingChe", "jiaoChe_04", "jiaoChe_05", "jingChe_02", "qingJieChe_01", "xiaoFangChe_01" };

	private List<string> BeforeDownloadStaticCar = new List<string> { "Chevrolet", "jingChe", "jiaoChe_04", "jiaoChe_05", "jingChe_02", "qingJieChe_01", "xiaoFangChe_01" };

	private float carDis1 = 2.3f;

	private float carDis2 = 5.5f;

	private float FourWayCarDis1 = 2.8f;

	private float FourWayCarDis2 = 6.7f;

	private List<LocationData> mPreCarLocationData = new List<LocationData>();

	public static ROAD_STATE CurRoadState;

	private float mRoadStateTimeCount;

	private float mRoadStateChangeTime = 15f;

	private int checkIndex;

	private ObjPlayerCar mPlayerCar;

	public vp_Timer.Handle getOnCarHandle = new vp_Timer.Handle();

	public vp_Timer.Handle getOnCarAnimaHandle = new vp_Timer.Handle();

	public FakeObjLogic RobNpcFakeObj;

	private List<MonsterData> mCurMonsterDataList = new List<MonsterData>();

	private Dictionary<long, BlockMonsterData> mStaticNpcServerIdDic = new Dictionary<long, BlockMonsterData>();

	private bool initFlag;

	private int mPoliceScores;

	private MiniMap mMiniMap;

	private float mPoliceCheckCount;

	private float mPoliceFlashCount = 15f;

	private float mTargetPoliceFlashTime;

	private float mScoresTimeCount;

	private LocationData mCurPlayerLocationData;

	private LocationData mCurTargetLocationData;

	private List<Vector3> mCurPathList = new List<Vector3>();

	private bool isDrawWalkLine;

	public ObjPlayerCar PlayerCar
	{
		get
		{
			return mPlayerCar;
		}
		set
		{
			mPlayerCar = value;
		}
	}

	public int PoliceScores
	{
		get
		{
			return mPoliceScores;
		}
		set
		{
			mPoliceScores = value;
			if (mMiniMap == null && SingletonUnity<MiniMap>.Exists)
			{
				mMiniMap = SingletonUnity<MiniMap>.Instance;
			}
			if (!(mMiniMap != null))
			{
				return;
			}
			float num = 0f;
			num = ((mPoliceScores >= mPoliceLevelData.PoliceScoresList[0]) ? Mathf.Lerp(0.2f, 0.6f, (float)(mPoliceScores - mPoliceLevelData.PoliceScoresList[0]) / (float)(mPoliceLevelData.PoliceScoresList[mPoliceLevelData.PoliceScoresList.Count - 1] - mPoliceLevelData.PoliceScoresList[0])) : ((!SingletonUnity<PoliceLevelRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<PoliceLevelRootLogic>.Instance.gameObject)) ? 0f : 0.2f));
			mMiniMap.SetPoliceAlph(num);
			if (num > 0f)
			{
				if (!mMiniMap.PoliceTweenColor.enabled)
				{
					mMiniMap.PoliceTweenColor.enabled = true;
				}
			}
			else if (mMiniMap.PoliceTweenColor.enabled)
			{
				mMiniMap.PoliceTweenColor.enabled = false;
				mMiniMap.PoliceTweenColor.ResetToBeginning();
			}
		}
	}

	public int PoliceLevel
	{
		get
		{
			for (int num = mPoliceLevelData.PoliceScoresList.Count - 1; num >= 0; num--)
			{
				if (PoliceScores >= mPoliceLevelData.PoliceScoresList[num])
				{
					return num;
				}
			}
			return -1;
		}
	}

	public void InitSaidao()
	{
		CityLeftBottomPos = new Vector2(-350f, -350f);
		CitySize = new Vector2(700f, 700f);
		BlockLength = 50;
		NpcCreateDis = 40f;
		CarCreateDis = 50f;
		mNpcRecycleDis = 50f;
		mCarRecycleDis = 60f;
	}

	public void InitGta()
	{
		CityLeftBottomPos = new Vector2(-800f, -800f);
		CitySize = new Vector2(1600f, 1600f);
		BlockLength = 100;
		NpcCreateDis = 40f;
		CarCreateDis = 50f;
		mNpcRecycleDis = 50f;
		mCarRecycleDis = 120f;
	}

	private int GetPoliceNum()
	{
		int num = 0;
		for (int i = 0; i < mEnableRagdollList.Count; i++)
		{
			if (PoliceIdList.Contains(mEnableRagdollList[i].NPCDataID))
			{
				num++;
			}
		}
		return num;
	}

	public BlockData GetBlockData(int x, int z)
	{
		if (x < 0 || x >= BlockData.Length || z < 0 || z >= BlockData[x].PointLine.Length)
		{
			return null;
		}
		return BlockData[x].PointLine[z];
	}

	private new void Awake()
	{
		base.Awake();
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.CurrentMapInofData.MapType == MAPTYPE.TUTORIAL_CAR)
		{
			NewTutorialSceneManager newTutorialSceneManager = sceneManager as NewTutorialSceneManager;
			mSmoothFollowCamPos = newTutorialSceneManager.SmoothFollowCamPos;
		}
	}

	private void Start()
	{
		InitMonsterList();
		mPoliceLevelData = DataManager.GetPoliceLevelDataById("0");
		UpdateSpecialCarState();
	}

	public void UpdateCityCheck()
	{
		if (mObjManager == null)
		{
			mObjManager = Singleton<ObjManager>.Instance;
		}
		if (mMainPlayerTrans == null)
		{
			if (!(mObjManager.MainPlayer != null))
			{
				return;
			}
			mMainPlayer = mObjManager.MainPlayer;
			mMainPlayerTrans = mObjManager.MainPlayer.CacheTransform;
			if (mMainPlayerTrans == null)
			{
				return;
			}
			mEnableRagdollList = Singleton<ObjManager>.Instance.EnableRagdollNpcList;
			mEnableFakeAICarList = Singleton<ObjManager>.Instance.EnableFakeAICarList;
		}
		flashTimeCount += Time.deltaTime;
		if (flashTimeCount > 5f)
		{
			flashTimeCount = 0f;
			CheckRecycleNpc();
			if (mEnableRagdollList.Count < NpcNumCount)
			{
				CreateNewNpc(mMainPlayerTrans, NpcCreateDis, isPolice: false);
				preCreateNpcPos = mMainPlayerTrans.position;
			}
			NormalNpcCreateCheck(mMainPlayerTrans.position);
			CheckRecycleAICar();
			if (mEnableFakeAICarList.Count < CarNumCount)
			{
				CreateNewCar(mMainPlayerTrans, CarCreateDis);
				preCreateNpcPos = mMainPlayerTrans.position;
			}
		}
		posTimeCount += Time.deltaTime;
		if (posTimeCount > 3f)
		{
			bool flag = (mMainPlayerTrans.position - preCreateNpcPos).sqrMagnitude > mFlashNpcSqrDis;
			if (flag)
			{
				CheckRecycleNpc();
				if (mEnableRagdollList.Count < NpcNumCount)
				{
					CreateNewNpc(mMainPlayerTrans, NpcCreateDis, isPolice: false);
					preCreateNpcPos = mMainPlayerTrans.position;
				}
				flashTimeCount = 0f;
				NormalNpcCreateCheck(mMainPlayerTrans.position);
			}
			if (flag)
			{
				CheckRecycleAICar();
				if (mEnableFakeAICarList.Count < CarNumCount)
				{
					CreateNewCar(mMainPlayerTrans, CarCreateDis);
					preCreateNpcPos = mMainPlayerTrans.position;
					flashTimeCount = 0f;
				}
			}
		}
		LightingSystemUpdate();
		CheckObjStop();
		if (mMainPlayer.IsDie)
		{
			if (SingletonUnity<RobCarBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RobCarBtnRootLogic>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RobCarBtnRoot);
			}
			PoliceScores = 0;
			mPoliceCreateList.Clear();
			return;
		}
		if (mPlayerCar != null)
		{
			if (mPlayerCar.CurSpeed > 5f)
			{
				if (SingletonUnity<RobCarBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RobCarBtnRootLogic>.Instance.gameObject))
				{
					SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RobCarBtnRoot);
				}
			}
			else if (!SingletonUnity<RobCarBtnRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<RobCarBtnRootLogic>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.RobCarBtnRoot);
			}
		}
		else if (mCurNearestCar == null)
		{
			if (SingletonUnity<RobCarBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RobCarBtnRootLogic>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RobCarBtnRoot);
			}
		}
		else if (mCurNearestCar.PlayerCar.IsDie)
		{
			mCurNearestCar = null;
			if (SingletonUnity<RobCarBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RobCarBtnRootLogic>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RobCarBtnRoot);
			}
		}
		else if (CheckShowDoor(mCurNearestCar.transform.InverseTransformPoint(mMainPlayerTrans.position)))
		{
			if (!SingletonUnity<RobCarBtnRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<RobCarBtnRootLogic>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.RobCarBtnRoot, delegate
				{
					SingletonUnity<RobCarBtnRootLogic>.Instance.Reset(mCurNearestCar);
				});
			}
		}
		else if (SingletonUnity<RobCarBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RobCarBtnRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RobCarBtnRoot);
		}
		PoliceUpdateCheck();
	}

	public void OnlyUpdateNpc()
	{
		if (mObjManager == null)
		{
			mObjManager = Singleton<ObjManager>.Instance;
		}
		if (mMainPlayerTrans == null)
		{
			if (!(mObjManager.MainPlayer != null))
			{
				return;
			}
			mMainPlayer = mObjManager.MainPlayer;
			mMainPlayerTrans = mObjManager.MainPlayer.CacheTransform;
			if (mMainPlayerTrans == null)
			{
				return;
			}
			mEnableRagdollList = Singleton<ObjManager>.Instance.EnableRagdollNpcList;
			mEnableFakeAICarList = Singleton<ObjManager>.Instance.EnableFakeAICarList;
		}
		flashTimeCount += Time.deltaTime;
		if (flashTimeCount > 5f)
		{
			flashTimeCount = 0f;
			CheckRecycleNpc();
			if (mEnableRagdollList.Count < NpcNumCount)
			{
				CreateNewNpc(mMainPlayerTrans, NpcCreateDis, isPolice: false);
				preCreateNpcPos = mMainPlayerTrans.position;
			}
		}
		posTimeCount += Time.deltaTime;
		if (posTimeCount > 3f && (mMainPlayerTrans.position - preCreateNpcPos).sqrMagnitude > mFlashNpcSqrDis)
		{
			CheckRecycleNpc();
			if (mEnableRagdollList.Count < NpcNumCount)
			{
				CreateNewNpc(mMainPlayerTrans, NpcCreateDis, isPolice: false);
				preCreateNpcPos = mMainPlayerTrans.position;
				flashTimeCount = 0f;
			}
		}
	}

	public void CheckRecycleNpc()
	{
		for (int num = mEnableRagdollList.Count - 1; num >= 0; num--)
		{
			if (mEnableRagdollList[num] == null)
			{
				mEnableRagdollList.RemoveAt(num);
			}
			else if (Vector3.Distance(mMainPlayerTrans.position, mEnableRagdollList[num].Position) > mNpcRecycleDis)
			{
				if (mEnablePoliceDic.ContainsKey(mEnableRagdollList[num].ServerId))
				{
					mEnablePoliceDic.Remove(mEnableRagdollList[num].ServerId);
					mEnablePoliceList.Remove(mEnableRagdollList[num]);
				}
				mEnableRagdollList[num].RecycleSelf();
			}
		}
		for (int num2 = mEnableObjZombiePlayerList.Count - 1; num2 >= 0; num2--)
		{
			if (mEnableObjZombiePlayerList[num2] == null)
			{
				mEnableObjZombiePlayerList.RemoveAt(num2);
			}
			else if (Vector3.Distance(mMainPlayerTrans.position, mEnableObjZombiePlayerList[num2].Position) > mNpcRecycleDis)
			{
				mEnableObjZombiePlayerList[num2].RecycleSelf();
			}
		}
	}

	public void CheckRecycleAICar()
	{
		for (int num = mEnableFakeAICarList.Count - 1; num >= 0; num--)
		{
			if (Vector3.Distance(mMainPlayerTrans.position, mEnableFakeAICarList[num].transform.position) > mCarRecycleDis)
			{
				mEnableFakeAICarList[num].RecycleSelf();
			}
		}
	}

	public void CreateNewNpc(Transform playerTrans, float createDis, bool isPolice)
	{
		ObjManager instance = Singleton<ObjManager>.Instance;
		List<LocationData> list = new List<LocationData>();
		if (!GetCreateLocation(playerTrans.position, createDis, list))
		{
			if (isPolice)
			{
				for (int i = 0; i < mPoliceCreateList.Count; i++)
				{
					CreateNpcNearBy(mPoliceCreateList[i]);
				}
				mPoliceCreateList.Clear();
				return;
			}
			list = mPreNpcLocationData;
		}
		mPreNpcLocationData = list;
		int level = mMainPlayer.AttributeData.Level;
		for (int j = 0; j < list.Count; j++)
		{
			CityPathPointData point = list[j].point1;
			CityPathPointData point2 = list[j].point2;
			if (!point.IsWalkable)
			{
				continue;
			}
			int num = ((Random.Range(0, 2) != 0) ? 1 : (-1));
			int num2 = ((point.LinkPointIndex[0] == point2.SelfIndex) ? 1 : (-1));
			Vector3 vector = point.PointPos + num * point.PointRight * Random.Range(point.MinWalkDis, point.MaxWalkDis) + num2 * point.PointForward * list[j].DisFromPoint1;
			if (!SceneManager.IsInNavmeshArea(vector))
			{
				continue;
			}
			if (isPolice)
			{
				if (Vector3.Distance(playerTrans.position, vector) > mNpcRecycleDis)
				{
					continue;
				}
			}
			else if (!CheckNpcPosEmpty(vector) || Vector3.Distance(playerTrans.position, vector) > mNpcRecycleDis)
			{
				continue;
			}
			if (mEnableRagdollList.Count > NpcNumCount)
			{
				continue;
			}
			NpcData npcData = null;
			if (mPoliceCreateList.Count > 0)
			{
				npcData = DataManager.GetNpcDataByID(mPoliceCreateList[0]);
				mPoliceCreateList.RemoveAt(0);
			}
			else
			{
				if (isPolice)
				{
					continue;
				}
				string id = NpcIdList[Random.Range(0, NpcIdList.Count)];
				npcData = DataManager.GetNpcDataByID(id);
			}
			CityPathPointData targetPoint = null;
			if (!point.IsWalkable && !point2.IsWalkable)
			{
				continue;
			}
			if (!point.IsWalkable)
			{
				targetPoint = point2;
			}
			else if (!point2.IsWalkable)
			{
				targetPoint = point;
			}
			else
			{
				targetPoint = ((Random.Range(0, 2) != 0) ? point2 : point);
			}
			int num3 = 1;
			num3 = (IsForward(vector - targetPoint.PointPos, targetPoint.PointRight) ? 1 : (-1));
			Vector3 vector2 = targetPoint.PointPos + num3 * targetPoint.PointRight * Random.Range(targetPoint.MinWalkDis, targetPoint.MaxWalkDis);
			ObjInitNpcData objInitNpcData = new ObjInitNpcData();
			objInitNpcData.mServerID = UUID.GenUUID();
			objInitNpcData.mPos = vector;
			objInitNpcData.mDir = (vector2 - objInitNpcData.mPos).normalized;
			objInitNpcData.npcInfoData = npcData;
			objInitNpcData.mCharacterModelId = npcData.Model;
			objInitNpcData.MaxHP = objInitNpcData.npcInfoData.Hp;
			objInitNpcData.HP = objInitNpcData.MaxHP;
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
			objInitNpcData.Level = level;
			objInitNpcData.AntiStun = objInitNpcData.npcInfoData.AntiStun;
			objInitNpcData.AntiKnockDown = objInitNpcData.npcInfoData.AntiKnockDown;
			instance.GetRagdollNPC(objInitNpcData, delegate(ObjNPC npc)
			{
				ObjRagdollNPC objRagdollNPC = npc as ObjRagdollNPC;
				if (objRagdollNPC != null)
				{
					objRagdollNPC.ResetCityMove(this, targetPoint);
					if (PoliceIdList.Contains(objRagdollNPC.NPCDataID))
					{
						mEnablePoliceDic.Add(objRagdollNPC.ServerId, objRagdollNPC);
						mEnablePoliceList.Add(objRagdollNPC);
					}
				}
			});
		}
	}

	private ObjZombieRagdollPlayer CreateZombieRagdollPlayer(BlockMonsterData blockMonsterData, character_look chaLook = null)
	{
		ObjInitPlayerData objInitPlayerData = new ObjInitPlayerData();
		objInitPlayerData.mPos = blockMonsterData.Data.GetNpcPos();
		objInitPlayerData.mDir = MathUtil.HeadingToVector3(blockMonsterData.Data.PositionO);
		objInitPlayerData.mServerID = UUID.GenUUID();
		objInitPlayerData.EXP = 0L;
		objInitPlayerData.TitleLevel = 0;
		objInitPlayerData.TitleExp = 0;
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		AdaptData adaptDataByID = DataManager.GetAdaptDataByID(level);
		NpcData npcDataByID = DataManager.GetNpcDataByID(blockMonsterData.Data.NpcID);
		objInitPlayerData.AIID = npcDataByID.AIID;
		objInitPlayerData.NpcId = npcDataByID.ID;
		objInitPlayerData.Level = level;
		objInitPlayerData.Attribute = new attribute();
		objInitPlayerData.Attribute.max_hp = npcDataByID.HpCoe * adaptDataByID.HpStd / 10000;
		objInitPlayerData.HP = (int)objInitPlayerData.Attribute.max_hp;
		objInitPlayerData.Attribute.atk = npcDataByID.AtkCoe * adaptDataByID.AtkStd / 10000;
		objInitPlayerData.Attribute.def = npcDataByID.DefCoe * adaptDataByID.DefStd / 10000;
		objInitPlayerData.Attribute.hit = npcDataByID.HITCoe * adaptDataByID.HITStd / 10000;
		objInitPlayerData.Attribute.eva = npcDataByID.DefCoe * adaptDataByID.DefStd / 10000;
		objInitPlayerData.Attribute.cri = npcDataByID.CRICoe * adaptDataByID.CRIStd / 10000;
		objInitPlayerData.Attribute.exd = npcDataByID.EXDCoe * adaptDataByID.EXDStd / 10000;
		objInitPlayerData.Attribute.exr = npcDataByID.EXRCoe * adaptDataByID.EXRStd / 10000;
		objInitPlayerData.Attribute.res = npcDataByID.RESCoe * adaptDataByID.RESStd / 10000;
		objInitPlayerData.Attribute.crd = npcDataByID.CRDCoe * adaptDataByID.CRDStd / 10000;
		objInitPlayerData.Attribute.crr = npcDataByID.CRRCoe * adaptDataByID.CRRStd / 10000;
		objInitPlayerData.Attribute.defa = npcDataByID.DEFACoe * adaptDataByID.DEFAStd / 10000;
		objInitPlayerData.Attribute.dgea = npcDataByID.DGEACoe * adaptDataByID.DGEAStd / 10000;
		objInitPlayerData.Attribute.hita = npcDataByID.HITACoe * adaptDataByID.HITAStd / 10000;
		objInitPlayerData.Attribute.resa = npcDataByID.RESACoe * adaptDataByID.RESAStd / 10000;
		objInitPlayerData.Attribute.cria = npcDataByID.CRIACoe * adaptDataByID.CRIAStd / 10000;
		objInitPlayerData.AttributeAll = objInitPlayerData.Attribute;
		objInitPlayerData.Speed = (float)npcDataByID.MoveSpeed / 10f;
		objInitPlayerData.ComboValue = 0;
		objInitPlayerData.skills = new Dictionary<string, skill_info>();
		if (!string.IsNullOrEmpty(npcDataByID.SkillGroup))
		{
			string[] array = npcDataByID.SkillGroup.Split(';');
			for (int i = 0; i < array.Length; i++)
			{
				skill_info skill_info = new skill_info();
				skill_info.skillId = array[i];
				SkillData skillDataById = DataManager.GetSkillDataById(skill_info.skillId);
				if (skillDataById.IsUpgrade == 1)
				{
					skill_info.skillLevel = level;
				}
				else
				{
					skill_info.skillLevel = 0L;
				}
				skill_info.indexPos = i;
				objInitPlayerData.skills.Add(skill_info.skillId, skill_info);
			}
		}
		if (chaLook == null)
		{
			objInitPlayerData.visual = new characterVisual();
			string[] array2 = npcDataByID.Model.Split(';');
			if (array2.Length != 4)
			{
				Debug.Log("Wrong ModelId !!!!!!!!!!!!!!");
				return null;
			}
			objInitPlayerData.visual.BodyId = array2[2];
			objInitPlayerData.visual.HeadId = array2[1];
			objInitPlayerData.visual.LegId = array2[3];
			objInitPlayerData.visual.WeaponId = array2[0];
			objInitPlayerData.visual.showType = 0L;
			objInitPlayerData.Name = npcDataByID.MName;
		}
		else
		{
			objInitPlayerData.visual = chaLook.visual;
			objInitPlayerData.Name = chaLook.visual.name;
			objInitPlayerData.TitleLevel = (int)chaLook.attribute_other.title_level;
		}
		if (objInitPlayerData.HeadId.Contains("XD"))
		{
			objInitPlayerData.mCharacterModelId = "100";
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
			{
				objInitPlayerData.visual.BodyId = GameDefine.XD_NormalModel[2];
				objInitPlayerData.visual.HeadId = GameDefine.XD_NormalModel[1];
				objInitPlayerData.visual.LegId = GameDefine.XD_NormalModel[3];
				objInitPlayerData.visual.WeaponId = GameDefine.XD_NormalModel[0];
				objInitPlayerData.visual.showType = 0L;
				objInitPlayerData.visual.WeaponItemId = null;
				objInitPlayerData.visual.FashionItemId = null;
			}
		}
		else if (objInitPlayerData.HeadId.Contains("QJ"))
		{
			objInitPlayerData.mCharacterModelId = "104";
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
			{
				objInitPlayerData.visual.BodyId = GameDefine.QJ_NormalModel[2];
				objInitPlayerData.visual.HeadId = GameDefine.QJ_NormalModel[1];
				objInitPlayerData.visual.LegId = GameDefine.QJ_NormalModel[3];
				objInitPlayerData.visual.WeaponId = GameDefine.QJ_NormalModel[0];
				objInitPlayerData.visual.showType = 0L;
				objInitPlayerData.visual.WeaponItemId = null;
				objInitPlayerData.visual.FashionItemId = null;
			}
		}
		else
		{
			objInitPlayerData.mCharacterModelId = "105";
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
			{
				objInitPlayerData.visual.BodyId = GameDefine.NQS_NormalModel[2];
				objInitPlayerData.visual.HeadId = GameDefine.NQS_NormalModel[1];
				objInitPlayerData.visual.LegId = GameDefine.NQS_NormalModel[3];
				objInitPlayerData.visual.WeaponId = GameDefine.NQS_NormalModel[0];
				objInitPlayerData.visual.showType = 0L;
				objInitPlayerData.visual.WeaponItemId = null;
				objInitPlayerData.visual.FashionItemId = null;
			}
		}
		objInitPlayerData.Camp = GameDefine.CAMP_TYPE.NORMAL_NPC;
		objInitPlayerData.PkMode = 1;
		ObjZombieRagdollPlayer objZombieRagdollPlayer = Singleton<ObjManager>.Instance.CreateZombieRagdollPlayer(objInitPlayerData);
		if (!objZombieRagdollPlayer.IsMissionNpc)
		{
			objZombieRagdollPlayer.PatrolMove(objZombieRagdollPlayer);
		}
		return objZombieRagdollPlayer;
	}

	public bool CreateNpcNearBy(string npcId)
	{
		Vector3 position = mMainPlayer.Position;
		Vector3 vector = position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * NpcCreateDis / 2f;
		if (!SceneManager.IsInNavmeshArea(vector))
		{
			return false;
		}
		NpcData npcDataByID = DataManager.GetNpcDataByID(npcId);
		ObjInitNpcData objInitNpcData = new ObjInitNpcData();
		objInitNpcData.mServerID = UUID.GenUUID();
		objInitNpcData.mPos = vector;
		objInitNpcData.mDir = (position - objInitNpcData.mPos).normalized;
		objInitNpcData.npcInfoData = npcDataByID;
		objInitNpcData.mCharacterModelId = npcDataByID.Model;
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
		Singleton<ObjManager>.Instance.GetRagdollNPC(objInitNpcData, null);
		return true;
	}

	public void CreateNewCar(Transform playerTrans, float createDis)
	{
		ObjManager instance = Singleton<ObjManager>.Instance;
		List<LocationData> list = new List<LocationData>();
		if (!GetCreateLocation(playerTrans.position, createDis, list))
		{
			list = mPreCarLocationData;
		}
		mPreCarLocationData = list;
		for (int i = 0; i < list.Count; i++)
		{
			CityPathPointData point = list[i].point1;
			CityPathPointData point2 = list[i].point2;
			int num = ((Random.Range(0, 2) != 0) ? 1 : (-1));
			int num2 = ((point.LinkPointIndex[0] == point2.SelfIndex) ? 1 : (-1));
			if (num2 == -1)
			{
				num = -num;
			}
			float num3 = ((Random.Range(0, 2) != 0) ? carDis2 : carDis1);
			num3 = ((!point.IsFourLines) ? carDis1 : ((Random.Range(0, 2) != 0) ? FourWayCarDis2 : FourWayCarDis1));
			Vector3 vector = point.PointPos + num * point.PointRight * num3 + num2 * point.PointForward * list[i].DisFromPoint1;
			if (!CheckCarPosEmpty(vector) || Vector3.Distance(playerTrans.position, vector) > mCarRecycleDis || Vector3.Distance(playerTrans.position, vector) < 5f || Mathf.Abs(SceneManager.GetHitHeight(vector) - vector.y) > 2f || mEnableFakeAICarList.Count > CarNumCount)
			{
				continue;
			}
			CityPathPointData cityPathPointData = null;
			CityPathPointData cityPathPointData2 = null;
			if (point.LinkPointIndex[0] == point2.SelfIndex)
			{
				if (num == -1)
				{
					cityPathPointData = point;
					cityPathPointData2 = point2;
				}
				else
				{
					cityPathPointData = point2;
					cityPathPointData2 = point;
				}
			}
			else if (num == 1)
			{
				cityPathPointData = point;
				cityPathPointData2 = point2;
			}
			else
			{
				cityPathPointData = point2;
				cityPathPointData2 = point;
			}
			string empty = string.Empty;
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
			{
				int num4 = Random.Range(0, FakeAICarName.Length);
				empty = FakeAICarName[num4];
			}
			else
			{
				int num5 = Random.Range(0, BeforeDownloadCarId.Length);
				empty = BeforeDownloadCarId[num5];
			}
			ObjCarInitData initData = new ObjCarInitData(vector, Quaternion.LookRotation(cityPathPointData.PointPos - cityPathPointData2.PointPos, Vector3.up).eulerAngles, UUID.GenUUID(), empty, policeFlag: false, DataManager.GetMountDataById(empty));
			if (mObjManager == null)
			{
				mObjManager = Singleton<ObjManager>.Instance;
			}
			ObjFakeAICar fakeAICar = mObjManager.GetFakeAICar(initData);
			if (fakeAICar != null)
			{
				fakeAICar.Reset(cityPathPointData, cityPathPointData2, num3, this);
			}
		}
	}

	private bool CheckNpcPosEmpty(Vector3 pos)
	{
		for (int num = mEnableRagdollList.Count - 1; num >= 0; num--)
		{
			if (mEnableRagdollList[num] == null)
			{
				mEnableRagdollList.RemoveAt(num);
			}
			else if (Vector3.SqrMagnitude(mEnableRagdollList[num].Position - pos) < 25f)
			{
				return false;
			}
		}
		return true;
	}

	private bool CheckCarPosEmpty(Vector3 pos)
	{
		for (int num = mEnableRagdollList.Count - 1; num >= 0; num--)
		{
			if (mEnableRagdollList[num] == null)
			{
				mEnableRagdollList.RemoveAt(num);
			}
			else if (Vector3.SqrMagnitude(mEnableRagdollList[num].Position - pos) < 16f)
			{
				return false;
			}
		}
		for (int i = 0; i < mEnableFakeAICarList.Count; i++)
		{
			if (Vector3.SqrMagnitude(mEnableFakeAICarList[i].transform.position - pos) < 100f)
			{
				return false;
			}
		}
		return true;
	}

	public void GetNextPoint(ObjCharacter obj)
	{
		ObjRagdollNPC objRagdollNPC = obj as ObjRagdollNPC;
		if (!(objRagdollNPC != null))
		{
			return;
		}
		CityPathPointData targetPathPoint = objRagdollNPC.TargetPathPoint;
		int num = -1;
		int num2 = 0;
		switch ((!targetPathPoint.IsCross) ? Random.Range(0, 2) : Random.Range(0, 4))
		{
		case 0:
			num = ((targetPathPoint.LinkPointIndex[0] == -1) ? targetPathPoint.LinkPointIndex[1] : targetPathPoint.LinkPointIndex[0]);
			if (PointDataList[num].IsWalkable)
			{
				break;
			}
			if (targetPathPoint.LinkPointIndex[2] != -1)
			{
				num = targetPathPoint.LinkPointIndex[2];
				break;
			}
			num = targetPathPoint.LinkPointIndex[3];
			if (num == -1)
			{
				num = targetPathPoint.LinkPointIndex[0];
			}
			break;
		case 1:
			num = ((targetPathPoint.LinkPointIndex[1] == -1) ? targetPathPoint.LinkPointIndex[0] : targetPathPoint.LinkPointIndex[1]);
			if (PointDataList[num].IsWalkable)
			{
				break;
			}
			if (targetPathPoint.LinkPointIndex[2] != -1)
			{
				num = targetPathPoint.LinkPointIndex[2];
				break;
			}
			num = targetPathPoint.LinkPointIndex[3];
			if (num == -1)
			{
				num = targetPathPoint.LinkPointIndex[1];
			}
			break;
		case 2:
			num = ((targetPathPoint.LinkPointIndex[2] == -1) ? targetPathPoint.LinkPointIndex[3] : targetPathPoint.LinkPointIndex[2]);
			if (!PointDataList[num].IsWalkable)
			{
				num = ((targetPathPoint.LinkPointIndex[1] == -1) ? targetPathPoint.LinkPointIndex[0] : targetPathPoint.LinkPointIndex[1]);
			}
			break;
		case 3:
			num = ((targetPathPoint.LinkPointIndex[3] == -1) ? targetPathPoint.LinkPointIndex[2] : targetPathPoint.LinkPointIndex[3]);
			if (!PointDataList[num].IsWalkable)
			{
				num = ((targetPathPoint.LinkPointIndex[1] == -1) ? targetPathPoint.LinkPointIndex[0] : targetPathPoint.LinkPointIndex[1]);
			}
			break;
		}
		objRagdollNPC.TargetPathPoint = PointDataList[num];
		Vector3 pos = ((!IsForward(objRagdollNPC.Position - objRagdollNPC.TargetPathPoint.PointPos, objRagdollNPC.TargetPathPoint.PointRight)) ? (objRagdollNPC.TargetPathPoint.PointPos - objRagdollNPC.TargetPathPoint.PointRight * Random.Range(objRagdollNPC.TargetPathPoint.MinWalkDis, objRagdollNPC.TargetPathPoint.MaxWalkDis)) : (objRagdollNPC.TargetPathPoint.PointPos + objRagdollNPC.TargetPathPoint.PointRight * Random.Range(objRagdollNPC.TargetPathPoint.MinWalkDis, objRagdollNPC.TargetPathPoint.MaxWalkDis)));
		objRagdollNPC.WalkMoveTo(pos, 1f, GetNextPoint);
	}

	public bool GetCreateLocation(Vector3 centerPos, float createDistance, List<LocationData> locationDataList)
	{
		LocationData playerLocation = GetPlayerLocation(centerPos, 100f);
		if (playerLocation == null)
		{
			return false;
		}
		CityPathPointData point = playerLocation.point1;
		CityPathPointData point2 = playerLocation.point2;
		if (playerLocation.DisFromPoint1 > createDistance)
		{
			LocationData locationData = new LocationData();
			locationData.point1 = point;
			locationData.point2 = point2;
			locationData.DisFromPoint1 = playerLocation.DisFromPoint1 - createDistance;
			locationDataList.Add(locationData);
			GetCreateLocationDataByRoad(point, point2, playerLocation.DisFromPoint1 - createDistance, locationDataList);
		}
		else
		{
			GetCreateLocationDataByRoad(point, point2, createDistance - playerLocation.DisFromPoint1, locationDataList);
		}
		float num = point.GetLinkDisByIndex(point2.SelfIndex) - playerLocation.DisFromPoint1;
		if (num > 0f)
		{
			if (num > createDistance)
			{
				LocationData locationData2 = new LocationData();
				locationData2.point1 = point2;
				locationData2.point2 = point;
				locationData2.DisFromPoint1 = num - createDistance;
				locationDataList.Add(locationData2);
				GetCreateLocationDataByRoad(point2, point, num - createDistance, locationDataList);
			}
			else
			{
				GetCreateLocationDataByRoad(point2, point, createDistance - num, locationDataList);
			}
		}
		return true;
	}

	public void GetCreateLocationDataByRoad(CityPathPointData curPoint, CityPathPointData prePoint, float remainDis, List<LocationData> locationDataList)
	{
		for (int i = 0; i < curPoint.LinkPointIndex.Length; i++)
		{
			if (curPoint.LinkPointIndex[i] == -1 || curPoint.LinkPointIndex[i] == prePoint.SelfIndex)
			{
				continue;
			}
			CityPathPointData cityPathPointData = PointDataList[curPoint.LinkPointIndex[i]];
			if (prePoint.IsCross && cityPathPointData.IsCross && curPoint.IsCross)
			{
				if (remainDis != -1f)
				{
					GetCreateLocationDataByRoad(cityPathPointData, curPoint, -1f, locationDataList);
				}
			}
			else if (curPoint.LinkPointDis[i] > remainDis)
			{
				LocationData locationData = new LocationData();
				locationData.point1 = cityPathPointData;
				locationData.point2 = curPoint;
				if (cityPathPointData.IsCross)
				{
					locationData.DisFromPoint1 = 0f;
				}
				else
				{
					locationData.DisFromPoint1 = curPoint.LinkPointDis[i] - remainDis;
				}
				locationDataList.Add(locationData);
				if (remainDis != -1f)
				{
					GetCreateLocationDataByRoad(cityPathPointData, curPoint, -1f, locationDataList);
				}
			}
			else
			{
				GetCreateLocationDataByRoad(cityPathPointData, curPoint, remainDis - curPoint.LinkPointDis[i], locationDataList);
			}
		}
	}

	public LocationData GetPlayerLocationBig(Vector3 pos, float checkDis = 50f)
	{
		BlockPos blockPos = GetBlockPos(pos.x, pos.z);
		int x = blockPos.x;
		int z = blockPos.z;
		BlockData blockData = GetBlockData(x, z);
		if (blockData == null)
		{
			return null;
		}
		CityPathPointData point = null;
		CityPathPointData point2 = null;
		float disFromPoint = 0f;
		if (IsInCurBlock(pos, blockData, out point, out point2, out disFromPoint, checkDis))
		{
			LocationData locationData = new LocationData();
			locationData.point1 = point;
			locationData.point2 = point2;
			locationData.DisFromPoint1 = disFromPoint;
			return locationData;
		}
		List<BlockData> nearByBlockDataBig = GetNearByBlockDataBig(pos, includeSelf: false);
		for (int i = 0; i < nearByBlockDataBig.Count; i++)
		{
			if (IsInCurBlock(pos, nearByBlockDataBig[i], out point, out point2, out disFromPoint, checkDis))
			{
				LocationData locationData2 = new LocationData();
				locationData2.point1 = point;
				locationData2.point2 = point2;
				locationData2.DisFromPoint1 = disFromPoint;
				return locationData2;
			}
		}
		return null;
	}

	public LocationData GetPlayerLocation(Vector3 pos, float checkDis = 100f)
	{
		BlockPos blockPos = GetBlockPos(pos.x, pos.z);
		int x = blockPos.x;
		int z = blockPos.z;
		BlockData blockData = GetBlockData(x, z);
		if (blockData == null)
		{
			return null;
		}
		CityPathPointData point = null;
		CityPathPointData point2 = null;
		float disFromPoint = 0f;
		if (IsInCurBlock(pos, blockData, out point, out point2, out disFromPoint, checkDis))
		{
			LocationData locationData = new LocationData();
			locationData.point1 = point;
			locationData.point2 = point2;
			locationData.DisFromPoint1 = disFromPoint;
			return locationData;
		}
		List<BlockData> nearByBlockData = GetNearByBlockData(pos, includeSelf: false);
		for (int i = 0; i < nearByBlockData.Count; i++)
		{
			if (IsInCurBlock(pos, nearByBlockData[i], out point, out point2, out disFromPoint, checkDis))
			{
				LocationData locationData2 = new LocationData();
				locationData2.point1 = point;
				locationData2.point2 = point2;
				locationData2.DisFromPoint1 = disFromPoint;
				return locationData2;
			}
		}
		return null;
	}

	public bool IsInCurBlock(Vector3 pos, BlockData curBlock, out CityPathPointData point1, out CityPathPointData point2, out float disFromPoint1, float checkDis = 100f)
	{
		float num = float.PositiveInfinity;
		point1 = null;
		point2 = null;
		disFromPoint1 = 0f;
		CityPathPointData cityPathPointData = null;
		CityPathPointData cityPathPointData2 = null;
		bool result = false;
		for (int i = 0; i < curBlock.PointIndex.Count; i++)
		{
			cityPathPointData = PointDataList[curBlock.PointIndex[i]];
			Vector2 inverseXZPos;
			Vector2 inverseXZPos2;
			if (cityPathPointData.LinkPointIndex[0] != -1)
			{
				cityPathPointData2 = PointDataList[cityPathPointData.LinkPointIndex[0]];
				if (Mathf.Abs(pos.y - cityPathPointData.PointPos.y) > 10f)
				{
					continue;
				}
				inverseXZPos = GetInverseXZPos(pos, cityPathPointData.PointPos, cityPathPointData.PointForward, cityPathPointData.PointRight);
				inverseXZPos2 = GetInverseXZPos(pos, cityPathPointData2.PointPos, cityPathPointData2.PointForward, cityPathPointData2.PointRight);
				if (!IsSameDir(cityPathPointData, cityPathPointData2))
				{
					if (inverseXZPos.y * inverseXZPos2.y > 0f)
					{
						float num2 = Mathf.Abs(inverseXZPos.x);
						if (num2 < num && num2 < checkDis)
						{
							num = num2;
							point1 = cityPathPointData;
							point2 = cityPathPointData2;
							disFromPoint1 = Mathf.Abs(inverseXZPos.y);
							result = true;
						}
					}
				}
				else if (inverseXZPos.y * inverseXZPos2.y < 0f)
				{
					float num3 = Mathf.Abs(inverseXZPos.x);
					if (num3 < num && num3 < checkDis)
					{
						num = num3;
						point1 = cityPathPointData;
						point2 = cityPathPointData2;
						disFromPoint1 = Mathf.Abs(inverseXZPos.y);
						result = true;
					}
				}
			}
			if ((cityPathPointData.LinkPointIndex[0] == -1 || IsSameDir(cityPathPointData, cityPathPointData2)) && !cityPathPointData.IsCross && (cityPathPointData.LinkPointIndex[1] == -1 || (IsSameDir(cityPathPointData, PointDataList[cityPathPointData.LinkPointIndex[1]]) && curBlock.PointIndex.Contains(cityPathPointData.LinkPointIndex[1]))))
			{
				continue;
			}
			cityPathPointData = PointDataList[curBlock.PointIndex[i]];
			if (cityPathPointData.LinkPointIndex[1] == -1)
			{
				continue;
			}
			cityPathPointData2 = PointDataList[cityPathPointData.LinkPointIndex[1]];
			if (Mathf.Abs(pos.y - cityPathPointData.PointPos.y) > 10f)
			{
				continue;
			}
			inverseXZPos = GetInverseXZPos(pos, cityPathPointData.PointPos, cityPathPointData.PointForward, cityPathPointData.PointRight);
			inverseXZPos2 = GetInverseXZPos(pos, cityPathPointData2.PointPos, cityPathPointData2.PointForward, cityPathPointData2.PointRight);
			if (!IsSameDir(cityPathPointData, cityPathPointData2))
			{
				if (inverseXZPos.y * inverseXZPos2.y > 0f)
				{
					float num4 = Mathf.Abs(inverseXZPos.x);
					if (num4 < num && num4 < checkDis)
					{
						num = num4;
						point1 = cityPathPointData;
						point2 = cityPathPointData2;
						disFromPoint1 = Mathf.Abs(inverseXZPos.y);
						result = true;
					}
				}
			}
			else if (inverseXZPos.y * inverseXZPos2.y < 0f)
			{
				float num5 = Mathf.Abs(inverseXZPos.x);
				if (num5 < num && num5 < checkDis)
				{
					num = num5;
					point1 = cityPathPointData;
					point2 = cityPathPointData2;
					disFromPoint1 = Mathf.Abs(inverseXZPos.y);
					result = true;
				}
			}
		}
		return result;
	}

	public bool IsSameDir(CityPathPointData point1, CityPathPointData point2)
	{
		if ((point1.LinkPointIndex[0] == point2.SelfIndex && point2.LinkPointIndex[0] == point1.SelfIndex) || (point1.LinkPointIndex[1] == point2.SelfIndex && point2.LinkPointIndex[1] == point1.SelfIndex))
		{
			return false;
		}
		return true;
	}

	public Vector2 GetInverseXZPos(Vector3 targetPos, Vector3 sourcePos, Vector3 sourceForward, Vector3 sourceRight)
	{
		Vector3 vector = targetPos - sourcePos;
		float num = Vector3.Project(vector, sourceRight).magnitude;
		float num2 = Vector3.Project(vector, sourceForward).magnitude;
		if (!IsForward(vector, sourceRight))
		{
			num = 0f - num;
		}
		if (!IsForward(vector, sourceForward))
		{
			num2 = 0f - num2;
		}
		return new Vector2(num, num2);
	}

	public static bool IsForward(Vector3 dir1, Vector3 dir2)
	{
		if (Vector3.Dot(dir1, dir2) > 0f)
		{
			return true;
		}
		return false;
	}

	public BlockPos GetBlockPos(float x, float z)
	{
		return new BlockPos((int)(x - CityLeftBottomPos.x) / BlockLength, (int)(z - CityLeftBottomPos.y) / BlockLength);
	}

	private void LightingSystemUpdate()
	{
		mRoadStateTimeCount += Time.deltaTime;
		if (CurRoadState == ROAD_STATE.NS_STRAIT_PASS)
		{
			if (mRoadStateTimeCount > mRoadStateChangeTime)
			{
				CurRoadState = ROAD_STATE.EW_STRAIT_PASS;
				mRoadStateTimeCount = 0f;
			}
		}
		else if (CurRoadState == ROAD_STATE.EW_STRAIT_PASS)
		{
			if (mRoadStateTimeCount > mRoadStateChangeTime)
			{
				CurRoadState = ROAD_STATE.PERSON_PASS1;
				mRoadStateTimeCount = 0f;
			}
		}
		else if (CurRoadState == ROAD_STATE.PERSON_PASS1)
		{
			if (mRoadStateTimeCount > mRoadStateChangeTime)
			{
				CurRoadState = ROAD_STATE.NS_TURN_PASS;
				mRoadStateTimeCount = 0f;
			}
		}
		else if (CurRoadState == ROAD_STATE.NS_TURN_PASS)
		{
			if (mRoadStateTimeCount > mRoadStateChangeTime)
			{
				CurRoadState = ROAD_STATE.EW_TURN_PASS;
				mRoadStateTimeCount = 0f;
			}
		}
		else if (CurRoadState == ROAD_STATE.EW_TURN_PASS)
		{
			if (mRoadStateTimeCount > mRoadStateChangeTime)
			{
				CurRoadState = ROAD_STATE.PERSON_PASS2;
				mRoadStateTimeCount = 0f;
			}
		}
		else if (CurRoadState == ROAD_STATE.PERSON_PASS2 && mRoadStateTimeCount > mRoadStateChangeTime)
		{
			CurRoadState = ROAD_STATE.NS_STRAIT_PASS;
			mRoadStateTimeCount = 0f;
		}
	}

	public void CheckObjStop()
	{
		if (mEnableRagdollList == null || mEnableFakeAICarList == null)
		{
			return;
		}
		for (int num = mEnableRagdollList.Count - 1; num >= 0; num--)
		{
			if (mEnableRagdollList[num] == null)
			{
				mEnableRagdollList.RemoveAt(num);
			}
			else if (mEnableRagdollList[num].CheckIndex != checkIndex)
			{
				mEnableRagdollList[num].CheckIndex = checkIndex;
				if (CheckObstacle(mEnableRagdollList[num].transform, 1f, 3f, isNpc: true))
				{
					if (!mEnableRagdollList[num].IsDie)
					{
						mEnableRagdollList[num].DisactiveTargetArriveFinish();
						mEnableRagdollList[num].StopMove();
					}
				}
				else if (mEnableRagdollList[num].AILogic != null && mEnableRagdollList[num].AILogic.curState == AISTATE.PATROL_STATE && !mEnableRagdollList[num].IsDie)
				{
					mEnableRagdollList[num].ContinueMove();
				}
				break;
			}
		}
		for (int i = 0; i < mEnableFakeAICarList.Count; i++)
		{
			if (mEnableFakeAICarList[i].CheckIndex == checkIndex || mEnableFakeAICarList[i].PlayerCar.IsDie)
			{
				continue;
			}
			mEnableFakeAICarList[i].CheckIndex = checkIndex;
			if (CheckObstacle(mEnableFakeAICarList[i].transform, 1.5f, 10f, isNpc: false, mEnableFakeAICarList[i]))
			{
				if (mEnableFakeAICarList[i].enabled)
				{
					mEnableFakeAICarList[i].DisactiveTargetArriveFinish();
					mEnableFakeAICarList[i].StopMove();
				}
			}
			else if (mEnableFakeAICarList[i].enabled)
			{
				mEnableFakeAICarList[i].ContinueMove();
			}
			return;
		}
		checkIndex++;
	}

	public bool CheckObstacle(Transform sourceObj, float xRange, float zRange, bool isNpc, ObjFakeAICar aiCar = null)
	{
		Vector3 zero = Vector3.zero;
		if (!isNpc)
		{
			zero = sourceObj.InverseTransformPoint(mMainPlayerTrans.position);
			if (CheckShowDoor(zero))
			{
				mCurNearestCar = aiCar;
			}
			if (Mathf.Abs(zero.x) < xRange && zero.z > 0f && zero.z < zRange)
			{
				return true;
			}
			for (int num = mEnableRagdollList.Count - 1; num >= 0; num--)
			{
				if (mEnableRagdollList[num] == null)
				{
					mEnableRagdollList.RemoveAt(num);
				}
				else
				{
					zero = sourceObj.InverseTransformPoint(mEnableRagdollList[num].Position);
					if (Mathf.Abs(zero.x) < xRange && zero.z > 0f && zero.z < zRange)
					{
						return true;
					}
				}
			}
		}
		for (int i = 0; i < mEnableFakeAICarList.Count; i++)
		{
			zero = sourceObj.InverseTransformPoint(mEnableFakeAICarList[i].transform.position);
			if (Mathf.Abs(zero.x) < xRange && zero.z > 0f && zero.z < zRange)
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckShowDoor(Vector3 inversePos)
	{
		if (mMainPlayer.SkillLogic.IsUsingSkill)
		{
			return false;
		}
		if (inversePos.x < 3f && inversePos.x > -3f && inversePos.z > -5f && inversePos.z < 5f)
		{
			return true;
		}
		return false;
	}

	public void RobCar(DelegateDefine.NoParamDelegate finishAct = null)
	{
		if (mPlayerCar == null)
		{
			if (mCurNearestCar == null)
			{
				return;
			}
			bool isNpcInCar = mCurNearestCar.enabled;
			mCurNearestCar.DisableFakeAICar();
			mPlayerCar = mCurNearestCar.PlayerCar;
			mPlayerCar.enabled = true;
			mPlayerCar.rigidbody.isKinematic = true;
			mSmoothFollowCamPos.Height = mPlayerCar.CurMountData.CamHeightMeter;
			mSmoothFollowCamPos.Distance = mPlayerCar.CurMountData.CamDisMeter;
			mSmoothFollowCamPos.SetTarget(mPlayerCar.CarControl);
			UnityVersionUtil.SetActiveRecursive(mSmoothFollowCamPos.gameObject, state: true);
			mSmoothFollowCamPos.UpdateToTargetPos();
			mMainPlayer.StopAutoAndSkill();
			mMainPlayer.LeveAutoCombat();
			mMainPlayerTrans.rigidbody.isKinematic = true;
			SingletonUnity<UIManager>.Instance.HideBaseUI();
			mMainPlayer.InvincibleFlag = true;
			mMainPlayer.IsShowInvincibleEffect = false;
			mMainPlayer.SelectTarget(null);
			bool GetOnCarFlag = true;
			mMainPlayer.CheckBeforeOnCar();
			vp_Timer.In(5f, delegate
			{
				if (GetOnCarFlag)
				{
					mMainPlayer.DisactiveTargetArriveFinish();
					mMainPlayer.DisableNavMeshAgent();
					mMainPlayer.CacheTransform.parent = mPlayerCar.DummyPlayerPoint;
					mMainPlayer.IsLocalDrivingCar = true;
					mMainPlayer.CurPlayerCar = mPlayerCar;
					mMainPlayer.CacheTransform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
					mMainPlayer.CacheTransform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
					vp_Timer.In(0.1f, delegate
					{
						if (RobNpcFakeObj != null)
						{
							RobNpcFakeObj.DestroyNpcFakeObj();
							RobNpcFakeObj = null;
						}
						if (isNpcInCar)
						{
							mPlayerCar.MeshRoot.animation.Stop();
							mPlayerCar.MeshRoot.animation.Play("RobCar");
							RobNpcFakeObj = new FakeObjLogic();
							if (mPlayerCar.CurMountData.ID.Equals("jingChe"))
							{
								RobNpcFakeObj.InitAnimaFakeNpcObj("NPC_Nan_047", mPlayerCar.DummyNPCPoint, "RobOffCar");
							}
							else
							{
								RobNpcFakeObj.InitAnimaFakeNpcObj("NPC_Nan_036", mPlayerCar.DummyNPCPoint, "RobOffCar");
							}
							mMainPlayer.AnimationLogic.ForcePlayAnimation("RobCar", null, -1f, 0f);
							if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.ROB_CAR))
							{
								local_npc_die.request rpcReq5 = new local_npc_die.request
								{
									type = 2L
								};
								NetLogic.GetInstance().Send<Protocol.local_npc_die>(rpcReq5);
								Debug.Log("ROB_CAR !!!!!!!!!!!!!!!!!!!!!!");
							}
							if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.TARGET_ROB_CAR))
							{
								local_npc_die.request rpcReq6 = new local_npc_die.request
								{
									type = 6L
								};
								NetLogic.GetInstance().Send<Protocol.local_npc_die>(rpcReq6);
							}
						}
						else
						{
							if (mCurNearestCar.IsStaticCar)
							{
								if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.ROB_CAR))
								{
									local_npc_die.request rpcReq7 = new local_npc_die.request
									{
										type = 2L
									};
									NetLogic.GetInstance().Send<Protocol.local_npc_die>(rpcReq7);
									Debug.Log("ROB_CAR !!!!!!!!!!!!!!!!!!!!!!");
								}
								if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.TARGET_ROB_CAR))
								{
									local_npc_die.request rpcReq8 = new local_npc_die.request
									{
										type = 6L
									};
									NetLogic.GetInstance().Send<Protocol.local_npc_die>(rpcReq8);
								}
							}
							mPlayerCar.MeshRoot.animation.Stop();
							if (mPlayerCar.CurMountData.IsMotorBool)
							{
								mMainPlayer.AnimationLogic.ForcePlayAnimation("GetOnCar_Motor", null, -1f, 0f);
							}
							else
							{
								mPlayerCar.MeshRoot.animation.Play("GetOnCar");
								mMainPlayer.AnimationLogic.ForcePlayAnimation("GetOnCar", null, -1f, 0f);
							}
						}
						if (mMainPlayer.SimpleShadow != null)
						{
							UnityVersionUtil.SetActiveRecursive(mMainPlayer.SimpleShadow, state: false);
						}
						vp_Timer.In(mMainPlayer.AnimationLogic.CurAnimationLength, delegate
						{
							if (mPlayerCar.CurMountData.IsShowPlayer == 0 && !mPlayerCar.CurMountData.IsMotorBool)
							{
								mMainPlayer.DisableMainPlayer();
							}
							else
							{
								mMainPlayer.GetComponent<CapsuleCollider>().enabled = false;
								mMainPlayer.RemoveXRayMat();
								mMainPlayer.enabled = false;
								if (mPlayerCar.CurMountData.IsMotorBool)
								{
									mMainPlayer.AnimationLogic.ForceSampleAnimation("GetOnCar_Motor", 1f, null, -1f, 0f);
									mMainPlayer.DisactiveHeadInfo();
								}
								else
								{
									mMainPlayer.AnimationLogic.ForceSampleAnimation("GetOnCar", 1f, null, -1f, 0f);
								}
							}
							ChangeToCarCtl(mPlayerCar);
							if (isNpcInCar)
							{
								RobNpcFakeObj.FakeObj.transform.parent.parent = null;
								vp_Timer.In(2f, delegate
								{
									if (RobNpcFakeObj != null)
									{
										RobNpcFakeObj.DestroyNpcFakeObj();
										RobNpcFakeObj = null;
									}
								});
							}
						}, getOnCarAnimaHandle);
					});
				}
			}, getOnCarHandle);
			mMainPlayer.MoveTo(mPlayerCar.DummyPlayerPoint.position, 1f, delegate
			{
				getOnCarHandle.Cancel();
				GetOnCarFlag = false;
				mMainPlayer.DisableNavMeshAgent();
				mMainPlayer.CacheTransform.parent = mPlayerCar.DummyPlayerPoint;
				mMainPlayer.IsLocalDrivingCar = true;
				mMainPlayer.CurPlayerCar = mPlayerCar;
				mMainPlayer.CacheTransform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
				mMainPlayer.CacheTransform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
				vp_Timer.In(0.1f, delegate
				{
					if (RobNpcFakeObj != null)
					{
						RobNpcFakeObj.DestroyNpcFakeObj();
						RobNpcFakeObj = null;
					}
					if (isNpcInCar)
					{
						mPlayerCar.MeshRoot.animation.Stop();
						mPlayerCar.MeshRoot.animation.Play("RobCar");
						RobNpcFakeObj = new FakeObjLogic();
						if (mPlayerCar.CurMountData.ID.Equals("jingChe"))
						{
							RobNpcFakeObj.InitAnimaFakeNpcObj("NPC_Nan_047", mPlayerCar.DummyNPCPoint, "RobOffCar");
						}
						else
						{
							RobNpcFakeObj.InitAnimaFakeNpcObj("NPC_Nan_036", mPlayerCar.DummyNPCPoint, "RobOffCar");
						}
						mMainPlayer.AnimationLogic.PlayAnimation("RobCar", null, -1f);
						if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.ROB_CAR))
						{
							local_npc_die.request rpcReq = new local_npc_die.request
							{
								type = 2L
							};
							NetLogic.GetInstance().Send<Protocol.local_npc_die>(rpcReq);
						}
						if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.TARGET_ROB_CAR))
						{
							local_npc_die.request rpcReq2 = new local_npc_die.request
							{
								type = 6L
							};
							NetLogic.GetInstance().Send<Protocol.local_npc_die>(rpcReq2);
						}
					}
					else
					{
						if (mCurNearestCar.IsStaticCar)
						{
							if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.ROB_CAR))
							{
								local_npc_die.request rpcReq3 = new local_npc_die.request
								{
									type = 2L
								};
								NetLogic.GetInstance().Send<Protocol.local_npc_die>(rpcReq3);
							}
							if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.TARGET_ROB_CAR))
							{
								local_npc_die.request rpcReq4 = new local_npc_die.request
								{
									type = 6L
								};
								NetLogic.GetInstance().Send<Protocol.local_npc_die>(rpcReq4);
							}
						}
						mPlayerCar.MeshRoot.animation.Stop();
						if (mPlayerCar.CurMountData.IsMotorBool)
						{
							mMainPlayer.AnimationLogic.ForcePlayAnimation("GetOnCar_Motor", null, -1f, 0f);
						}
						else
						{
							mPlayerCar.MeshRoot.animation.Play("GetOnCar");
							mMainPlayer.AnimationLogic.PlayAnimation("GetOnCar", null, -1f);
						}
					}
					if (mMainPlayer.SimpleShadow != null)
					{
						UnityVersionUtil.SetActiveRecursive(mMainPlayer.SimpleShadow, state: false);
					}
					vp_Timer.In(mMainPlayer.AnimationLogic.CurAnimationLength, delegate
					{
						if (mPlayerCar.CurMountData.IsShowPlayer == 0 && !mPlayerCar.CurMountData.IsMotorBool)
						{
							mMainPlayer.DisableMainPlayer();
						}
						else
						{
							mMainPlayer.GetComponent<CapsuleCollider>().enabled = false;
							mMainPlayer.RemoveXRayMat();
							mMainPlayer.enabled = false;
							if (mPlayerCar.CurMountData.IsMotorBool)
							{
								mMainPlayer.DisactiveHeadInfo();
								mMainPlayer.AnimationLogic.ForceSampleAnimation("GetOnCar_Motor", 1f, null, -1f, 0f);
							}
							else
							{
								mMainPlayer.AnimationLogic.ForceSampleAnimation("GetOnCar", 1f, null, -1f, 0f);
							}
						}
						ChangeToCarCtl(mPlayerCar);
						if (isNpcInCar)
						{
							RobNpcFakeObj.FakeObj.transform.parent.parent = null;
							vp_Timer.In(2f, delegate
							{
								if (RobNpcFakeObj != null)
								{
									RobNpcFakeObj.DestroyNpcFakeObj();
									RobNpcFakeObj = null;
								}
							});
						}
					}, getOnCarAnimaHandle);
				});
			});
			return;
		}
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		mPlayerCar.DisableCar();
		ObjPlayerCar tempCar = mPlayerCar;
		mMainPlayer.InvincibleFlag = true;
		mMainPlayer.IsShowInvincibleEffect = false;
		mMainPlayer.EnableMainPlayer();
		mMainPlayer.GetComponent<CapsuleCollider>().enabled = true;
		mMainPlayer.CameraController.IdealYaw = tempCar.transform.eulerAngles.y;
		mMainPlayer.CameraController.UpdateNow();
		if (mPlayerCar.CurMountData.IsMotorBool)
		{
			mMainPlayer.AnimationLogic.PlayAnimation("GetOffCar_Motor", delegate
			{
				ChangeToPlayerCtl();
				mMainPlayer.AddXRayMat();
				tempCar.rigidbody.useGravity = true;
				tempCar.rigidbody.isKinematic = false;
				if (finishAct != null)
				{
					finishAct();
				}
			}, -1f);
		}
		else
		{
			mPlayerCar.MeshRoot.animation.Play("GetOffCar");
			mMainPlayer.AnimationLogic.PlayAnimation("GetOffCar", delegate
			{
				ChangeToPlayerCtl();
				mMainPlayer.AddXRayMat();
				tempCar.rigidbody.useGravity = true;
				tempCar.rigidbody.isKinematic = false;
				if (finishAct != null)
				{
					finishAct();
				}
			}, -1f);
		}
		mPlayerCar.enabled = false;
		mPlayerCar = null;
	}

	public void ChangeToPlayerCtl()
	{
		mMainPlayer.InvincibleFlag = false;
		mMainPlayer.IsShowInvincibleEffect = true;
		mMainPlayer.transform.parent = null;
		mMainPlayer.EnableNavMeshAgent();
		mMainPlayer.rigidbody.isKinematic = false;
		if (mMainPlayer.SimpleShadow != null)
		{
			UnityVersionUtil.SetActiveRecursive(mMainPlayer.SimpleShadow, state: true);
		}
		CameraController camCtl = mMainPlayer.CameraController;
		vp_Timer.In(0.1f, delegate
		{
			camCtl.LerpBackToPlayer(1f, delegate
			{
				ResetPlayerUI();
				if (SingletonUnity<RealTimeShadow>.Exists)
				{
					SingletonUnity<RealTimeShadow>.Instance.EnableRealTimeShadow();
				}
				for (int i = 0; i < mMainPlayer.PartObject.Length; i++)
				{
					if (mMainPlayer.PartObject[i] != null)
					{
						mMainPlayer.PartObject[i].layer = LayerMask.NameToLayer("ShadowCaster");
					}
				}
			});
		});
	}

	public void ResetPlayerUI()
	{
		UIManager instance = SingletonUnity<UIManager>.Instance;
		instance.ReShowBaseUI();
		instance.CloseUI(UIInfo.CarControllerRoot);
		instance.ShowUI(UIInfo.YiDongKongZhiUI);
		instance.ShowUI(UIInfo.JueseJiNengQuUI, delegate
		{
			SingletonUnity<JueseJiNengQuLogic>.Instance.Reset();
		});
		instance.ShowUI(UIInfo.TouXiangKuangUI, delegate(bool isSuccess, object param)
		{
			if (isSuccess)
			{
				SingletonUnity<TouXiangKuangLogic>.Instance.Init();
			}
		});
		instance.ShowUI(UIInfo.FunctionBtnRootUI, delegate
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.Reset();
		});
		instance.ShowUI(UIInfo.ExpLineRoot);
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.IsCanUsePotion() && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
		{
			instance.ShowUI(UIInfo.PotionObjRoot, delegate
			{
				SingletonUnity<PotionLogic>.Instance.Reset();
			});
		}
	}

	private void ChangeToCarCtl(ObjPlayerCar mPlayerCar)
	{
		mMainPlayer.InvincibleFlag = false;
		mMainPlayer.IsShowInvincibleEffect = true;
		UIManager instance = SingletonUnity<UIManager>.Instance;
		instance.ReShowBaseUI();
		instance.CloseUI(UIInfo.YiDongKongZhiUI);
		instance.CloseUI(UIInfo.JueseJiNengQuUI);
		instance.ShowCarDefaultUI();
		instance.CloseUI(UIInfo.ExpLineRoot);
		instance.CloseUI(UIInfo.PotionObjRoot);
		instance.CloseUI(UIInfo.FunctionBtnRootUI);
		CameraController cameraController = mMainPlayer.CameraController;
		mPlayerCar.EnableCar(mMainPlayer);
		cameraController.LerpToTargetLocalZero(mSmoothFollowCamPos.transform, 1f, delegate
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.ROB_CAR_FINISH)
			{
				TutorialManager.MoveNext();
			}
		});
		mMainPlayer.SelectTarget(null);
		if (SingletonUnity<RealTimeShadow>.Exists)
		{
			SingletonUnity<RealTimeShadow>.Instance.DisableRealTimeShadow();
		}
	}

	public void InitMonsterList()
	{
		if (initFlag || !SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene())
		{
			return;
		}
		initFlag = true;
		mCurMonsterDataList = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.MonsterDataList;
		BlockPos blockPos = null;
		BlockData blockData = null;
		for (int i = 0; i < BlockData.Length; i++)
		{
			for (int j = 0; j < BlockData[i].PointLine.Length; j++)
			{
				BlockData[i].PointLine[j].BlockMonsterList.Clear();
			}
		}
		for (int k = 0; k < mCurMonsterDataList.Count; k++)
		{
			blockPos = GetBlockPos(mCurMonsterDataList[k].PositionX, mCurMonsterDataList[k].PositionZ);
			GetBlockData(blockPos.x, blockPos.z)?.BlockMonsterList.Add(new BlockMonsterData(mCurMonsterDataList[k]));
		}
	}

	public void NormalNpcCreateCheck(Vector3 pos)
	{
		List<BlockData> nearByBlockData = GetNearByBlockData(pos, includeSelf: true);
		List<BlockMonsterData> list = new List<BlockMonsterData>();
		for (int i = 0; i < nearByBlockData.Count; i++)
		{
			for (int j = 0; j < nearByBlockData[i].BlockMonsterList.Count; j++)
			{
				if (nearByBlockData[i].BlockMonsterList[j].ServerId == -1 && Vector3.Distance(nearByBlockData[i].BlockMonsterList[j].Data.GetNpcXZPos(), pos) < NpcCreateDis)
				{
					list.Add(nearByBlockData[i].BlockMonsterList[j]);
				}
			}
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ObjRagdollNPC objRagdollNPC = null;
		for (int k = 0; k < list.Count; k++)
		{
			NpcData npcDataByID = DataManager.GetNpcDataByID(list[k].Data.NpcID);
			if (npcDataByID == null)
			{
				continue;
			}
			if (npcDataByID.IsPlayerModel == 0)
			{
				if (!playerData.IsFinishDownload)
				{
					CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(npcDataByID.Model);
					if (!mBeforDownloadNpcModelIdList.Contains(characterModelDataByID.Name))
					{
						continue;
					}
				}
				objRagdollNPC = GetNpc(list[k]);
				if (objRagdollNPC != null)
				{
					list[k].ServerId = objRagdollNPC.ServerId;
					if (!mStaticNpcServerIdDic.ContainsKey(list[k].ServerId))
					{
						mStaticNpcServerIdDic.Add(list[k].ServerId, list[k]);
					}
				}
				continue;
			}
			if (!string.IsNullOrEmpty(npcDataByID.TalkGroup))
			{
				if (!playerData.Domin_InfoDic.ContainsKey(npcDataByID.TalkGroup))
				{
					continue;
				}
				domin_info domin_info = playerData.Domin_InfoDic[npcDataByID.TalkGroup];
				if (domin_info.state == 1 || domin_info.serverId == -1 || !playerData.Domin_CharacterDic.ContainsKey(domin_info.serverId))
				{
					continue;
				}
				ObjZombieRagdollPlayer objZombieRagdollPlayer = CreateZombieRagdollPlayer(list[k], playerData.Domin_CharacterDic[domin_info.serverId]);
				if (objZombieRagdollPlayer != null)
				{
					if (!mEnableObjZombiePlayerList.Contains(objZombieRagdollPlayer))
					{
						mEnableObjZombiePlayerList.Add(objZombieRagdollPlayer);
					}
					list[k].ServerId = objZombieRagdollPlayer.ServerId;
					if (!mStaticNpcServerIdDic.ContainsKey(list[k].ServerId))
					{
						mStaticNpcServerIdDic.Add(list[k].ServerId, list[k]);
					}
				}
				continue;
			}
			ObjZombieRagdollPlayer objZombieRagdollPlayer2 = CreateZombieRagdollPlayer(list[k]);
			if (objZombieRagdollPlayer2 != null)
			{
				if (!mEnableObjZombiePlayerList.Contains(objZombieRagdollPlayer2))
				{
					mEnableObjZombiePlayerList.Add(objZombieRagdollPlayer2);
				}
				list[k].ServerId = objZombieRagdollPlayer2.ServerId;
				if (!mStaticNpcServerIdDic.ContainsKey(list[k].ServerId))
				{
					mStaticNpcServerIdDic.Add(list[k].ServerId, list[k]);
				}
			}
		}
		List<BlockCarData> list2 = new List<BlockCarData>();
		for (int l = 0; l < nearByBlockData.Count; l++)
		{
			for (int m = 0; m < nearByBlockData[l].BlockCarDataList.Count; m++)
			{
				if (nearByBlockData[l].BlockCarDataList[m].ServerId == -1 && Vector3.Distance(nearByBlockData[l].BlockCarDataList[m].CarPos, pos) < NpcCreateDis)
				{
					list2.Add(nearByBlockData[l].BlockCarDataList[m]);
				}
			}
		}
		ObjFakeAICar objFakeAICar = null;
		for (int n = 0; n < list2.Count; n++)
		{
			objFakeAICar = GetStaticCar(list2[n]);
			if (objFakeAICar != null)
			{
				list2[n].ServerId = objFakeAICar.PlayerCar.ServerId;
				if (!mStaticCarDic.ContainsKey(list2[n].ServerId))
				{
					mStaticCarDic.Add(list2[n].ServerId, list2[n]);
				}
			}
		}
		CheckSpecialCar(pos);
	}

	private void CheckSpecialCar(Vector3 pos)
	{
		if (GameManager.IsSupportCurDataVersion167() && mIsHaveSpecialCar && mSpecialCarServerId == -1 && Vector3.Distance(SpecialCarPos, pos) < NpcCreateDis)
		{
			BlockCarData blockCarData = new BlockCarData();
			blockCarData.CarId = mSpecialCarId;
			blockCarData.CarAngle = SpecialCarAngle;
			blockCarData.CarPos = SpecialCarPos;
			ObjFakeAICar staticCar = GetStaticCar(blockCarData);
			mSpecialCarServerId = staticCar.PlayerCar.ServerId;
		}
	}

	private ObjFakeAICar GetSpecialCar(Vector3 pos)
	{
		ObjCarInitData initData = new ObjCarInitData(pos, SpecialCarAngle, UUID.GenUUID(), mSpecialCarId, policeFlag: false, DataManager.GetMountDataById(mSpecialCarId));
		ObjFakeAICar fakeAICar = mObjManager.GetFakeAICar(initData);
		if (fakeAICar != null)
		{
			fakeAICar.ResetStaticCar();
		}
		else
		{
			Debug.Log("Car == null!!!!!!!!!!!!! :: " + mEnableFakeAICarList.Count);
		}
		return fakeAICar;
	}

	public ObjFakeAICar GetStaticCar(BlockCarData data, bool isFriend = false)
	{
		Vector3 pos = new Vector3(data.CarPos.x, SceneManager.GetHitHeight(data.CarPos.x, data.CarPos.z), data.CarPos.z) + Vector3.up * 0.1f;
		string text = data.CarId;
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload && !BeforeDownloadStaticCar.Contains(text))
		{
			text = BeforeDownloadStaticCar[Random.Range(0, BeforeDownloadStaticCar.Count)];
		}
		ObjCarInitData initData = new ObjCarInitData(pos, data.CarAngle, UUID.GenUUID(), text, policeFlag: false, DataManager.GetMountDataById(text));
		if (mObjManager == null)
		{
			mObjManager = Singleton<ObjManager>.Instance;
		}
		ObjFakeAICar fakeAICar = mObjManager.GetFakeAICar(initData, isFriend);
		if (fakeAICar != null)
		{
			fakeAICar.ResetStaticCar();
		}
		return fakeAICar;
	}

	public ObjRagdollNPC GetNpc(BlockMonsterData curData)
	{
		if (!SceneManager.IsInNavmeshArea(curData.Data.GetNpcPos()))
		{
			return null;
		}
		ObjInitNpcData objInitNpcData = new ObjInitNpcData();
		objInitNpcData.mServerID = UUID.GenUUID();
		objInitNpcData.mPos = curData.Data.GetNpcPos();
		objInitNpcData.mDir = MathUtil.HeadingToVector3(curData.Data.PositionO);
		objInitNpcData.npcInfoData = DataManager.GetNpcDataByID(curData.Data.NpcID);
		objInitNpcData.mCharacterModelId = objInitNpcData.npcInfoData.Model;
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
		objInitNpcData.PathID = curData.Data.PathId;
		return Singleton<ObjManager>.Instance.GetRagdollNPC(objInitNpcData, null);
	}

	public void OnRecycleNpc(ObjRagdollNPC npc)
	{
		if (mStaticNpcServerIdDic.ContainsKey(npc.ServerId))
		{
			mStaticNpcServerIdDic[npc.ServerId].ServerId = -1L;
			mStaticNpcServerIdDic.Remove(npc.ServerId);
		}
	}

	public void OnRecycleZombiePlayer(ObjZombieRagdollPlayer zombiePlayer)
	{
		if (mStaticNpcServerIdDic.ContainsKey(zombiePlayer.ServerId))
		{
			mStaticNpcServerIdDic[zombiePlayer.ServerId].ServerId = -1L;
			mStaticNpcServerIdDic.Remove(zombiePlayer.ServerId);
		}
		if (mEnableObjZombiePlayerList.Contains(zombiePlayer))
		{
			mEnableObjZombiePlayerList.Remove(zombiePlayer);
		}
	}

	public void OnRecycleFakeAICar(ObjFakeAICar car)
	{
		if (mStaticCarDic.ContainsKey(car.PlayerCar.ServerId))
		{
			mStaticCarDic[car.PlayerCar.ServerId].ServerId = -1L;
			mStaticCarDic.Remove(car.PlayerCar.ServerId);
		}
		if (car.PlayerCar.ServerId == mSpecialCarServerId)
		{
			mSpecialCarServerId = -1L;
		}
	}

	private List<BlockData> GetNearByBlockData(Vector3 pos, bool includeSelf)
	{
		List<BlockData> list = new List<BlockData>();
		BlockPos blockPos = GetBlockPos(pos.x, pos.z);
		int x = blockPos.x;
		int z = blockPos.z;
		int num = 0;
		int num2 = 0;
		if (x < 0 || x >= BlockData.Length || z < 0 || z >= BlockData[x].PointLine.Length)
		{
			return list;
		}
		if (includeSelf)
		{
			list.Add(BlockData[x].PointLine[z]);
		}
		num = (((int)(pos.x - CityLeftBottomPos.x) % BlockLength > BlockLength / 2) ? 1 : (-1));
		num2 = (((int)(pos.z - CityLeftBottomPos.y) % BlockLength > BlockLength / 2) ? 1 : (-1));
		int num3 = x + num;
		int num4 = z + num2;
		if (num3 > 0 && num3 < BlockData.Length)
		{
			list.Add(BlockData[num3].PointLine[z]);
			if (num4 > 0 && num4 < BlockData[0].PointLine.Length)
			{
				list.Add(BlockData[num3].PointLine[num4]);
				list.Add(BlockData[x].PointLine[num4]);
			}
		}
		else if (num4 > 0 && num4 < BlockData[0].PointLine.Length)
		{
			list.Add(BlockData[x].PointLine[num4]);
		}
		return list;
	}

	private List<BlockData> GetNearByBlockDataBig(Vector3 pos, bool includeSelf)
	{
		List<BlockData> list = new List<BlockData>();
		BlockPos blockPos = GetBlockPos(pos.x, pos.z);
		int x2 = blockPos.x;
		int z = blockPos.z;
		int num = 0;
		int num2 = 0;
		if (x2 < 0 || x2 >= BlockData.Length || z < 0 || z >= BlockData[x2].PointLine.Length)
		{
			return list;
		}
		if (includeSelf)
		{
			list.Add(BlockData[x2].PointLine[z]);
		}
		List<KeyValuePair<int, int>> list2 = new List<KeyValuePair<int, int>>();
		int num3 = 0;
		int num4 = 0;
		for (int i = -2; i <= 2; i++)
		{
			for (int j = -2; j <= 2; j++)
			{
				list2.Add(new KeyValuePair<int, int>(i, j));
			}
		}
		for (int num5 = list2.Count - 1; num5 >= 0; num5--)
		{
			if (list2[num5].Key + x2 < 0 || list2[num5].Key + x2 >= BlockData.Length || list2[num5].Value + z < 0 || list2[num5].Value + z >= BlockData[0].PointLine.Length)
			{
				list2.RemoveAt(num5);
			}
		}
		list2.Sort((KeyValuePair<int, int> x, KeyValuePair<int, int> y) => x.Key * x.Key + x.Value * x.Value - y.Key * y.Key - y.Value * y.Value);
		for (int k = 0; k < list2.Count; k++)
		{
			list.Add(BlockData[x2 + list2[k].Key].PointLine[z + list2[k].Value]);
		}
		return list;
	}

	public void OnPlayerCarDie()
	{
		PoliceScores += 8;
	}

	public void OnNPCDie(ObjRagdollNPC npc)
	{
		if (mEnablePoliceDic.ContainsKey(npc.ServerId))
		{
			PoliceScores += 8;
			mEnablePoliceDic.Remove(npc.ServerId);
			mEnablePoliceList.Remove(npc);
		}
		else
		{
			MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			List<CurMission> curMissionList = missionManager.GetCurMissionList();
			for (int i = 0; i < curMissionList.Count; i++)
			{
				if (curMissionList[i].MissionState != MISSION_STATE.ACCEPTED)
				{
					continue;
				}
				MissionData missionDataByID = DataManager.GetMissionDataByID(curMissionList[i].MissionId);
				if (missionDataByID.MissionLogicType != MISSION_LOGICTYPE.KILLMONSTER && missionDataByID.MissionLogicType != MISSION_LOGICTYPE.KILLMONSTER_DROP && missionDataByID.MissionLogicType != MISSION_LOGICTYPE.LOCAL_KILL_MONSTER && missionDataByID.MissionLogicType != MISSION_LOGICTYPE.LOCAL_KILL_MONSTER_DROP && missionDataByID.MissionLogicType != MISSION_LOGICTYPE.KILL_TARGET_NPC)
				{
					continue;
				}
				if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC)
				{
					KillTargetMissionData killTargetMissionDataById = DataManager.GetKillTargetMissionDataById(missionDataByID.LogicID);
					if (killTargetMissionDataById.NpcID.Equals(npc.NPCDataID))
					{
						return;
					}
				}
				else if (missionDataByID.Target.Equals(npc.NPCDataID))
				{
					return;
				}
			}
			PoliceScores += 5;
		}
		if (PoliceScores > mPoliceLevelData.MaxScores)
		{
			PoliceScores = mPoliceLevelData.MaxScores;
		}
	}

	public void PoliceUpdateCheck()
	{
		mPoliceCheckCount += Time.deltaTime;
		int curPoliceLevel = PoliceLevel;
		if (curPoliceLevel >= 0)
		{
			if (mPoliceCheckCount >= 1f)
			{
				mPoliceCheckCount -= 1f;
				mTargetPoliceFlashTime = mPoliceLevelData.PoliceFlashTimeList[curPoliceLevel];
				if (PoliceScores > 0 && !IsPoliceAround())
				{
					PoliceScores--;
				}
				if (SingletonUnity<PoliceLevelRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PoliceLevelRootLogic>.Instance.gameObject))
				{
					SingletonUnity<PoliceLevelRootLogic>.Instance.SetPoliceLevel(curPoliceLevel);
				}
				else
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PoliceLevelRoot, delegate
					{
						SingletonUnity<PoliceLevelRootLogic>.Instance.SetPoliceLevel(curPoliceLevel);
					});
				}
			}
			mPoliceFlashCount += Time.deltaTime;
			if (mPoliceFlashCount > mTargetPoliceFlashTime)
			{
				mPoliceFlashCount = 0f;
				FlashPoliceNow(curPoliceLevel);
			}
			return;
		}
		mScoresTimeCount += Time.deltaTime;
		if (!(mScoresTimeCount > 1f))
		{
			return;
		}
		mScoresTimeCount -= 1f;
		if (PoliceScores > 0 && !IsPoliceAround())
		{
			PoliceScores--;
		}
		if (PoliceScores < 5 && SingletonUnity<PoliceLevelRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PoliceLevelRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PoliceLevelRoot);
			PoliceScores = PoliceScores;
			for (int i = 0; i < mEnablePoliceList.Count; i++)
			{
				mEnablePoliceList[i].ChangeNpcAI("1000");
			}
		}
	}

	public bool IsPoliceAround()
	{
		for (int i = 0; i < mEnablePoliceList.Count; i++)
		{
			if (Vector3.Distance(mEnablePoliceList[i].Position, mMainPlayer.Position) < PoliceCheckDis)
			{
				return true;
			}
		}
		return false;
	}

	public void FlashPoliceNow(int level)
	{
		int num = mPoliceLevelData.PoliceNumList[level] - GetPoliceNum();
		List<string> list = mPoliceLevelData.PoliceIdList[level];
		mPoliceCreateList.Clear();
		for (int i = 0; i < num; i++)
		{
			mPoliceCreateList.Add(list[Random.Range(0, list.Count)]);
		}
		CreateNewNpc(mMainPlayerTrans, NpcCreateDis, isPolice: true);
	}

	public void UpdateSpecialCarState()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (!string.IsNullOrEmpty(playerData.MountId))
		{
			mIsHaveSpecialCar = true;
			MountData mountDataById = DataManager.GetMountDataById(playerData.MountId);
			mSpecialCarId = mountDataById.GTALinkCarId;
		}
	}

	public void OnDownloadFlashNPC()
	{
		ObjManager instance = Singleton<ObjManager>.Instance;
		for (int num = mEnableRagdollList.Count - 1; num >= 0; num--)
		{
			if (mEnableRagdollList[num].MeshRoot == null)
			{
				instance.RecycleRagdollNPC(mEnableRagdollList[num]);
			}
		}
		if (mEnableRagdollList.Count < NpcNumCount)
		{
			CreateNewNpc(mMainPlayerTrans, NpcCreateDis, isPolice: false);
			preCreateNpcPos = mMainPlayerTrans.position;
		}
		NormalNpcCreateCheck(mMainPlayerTrans.position);
	}

	public void ClearMapLine()
	{
		if (mCurPlayerLocationData != null || mCurTargetLocationData != null || mCurPathList.Count > 0 || isDrawWalkLine)
		{
			mCurPlayerLocationData = null;
			mCurTargetLocationData = null;
			mCurPathList.Clear();
			isDrawWalkLine = false;
			if (SingletonUnity<NewMapUIRootLogic>.Exists)
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.ClearMapLine();
			}
			if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
			{
				SingletonUnity<MiniMap>.Instance.ClearMapLine();
			}
		}
	}

	public void UpdateMapLine(Vector3 sourcePos, Vector3 targetPos)
	{
		if (mMainPlayer == null || !mMainPlayer.IsLocalDrivingCar)
		{
			ClearMapLine();
			return;
		}
		if (Vector3.Distance(sourcePos, targetPos) < 70f)
		{
			if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.OnlyDrawWalkLine(sourcePos, targetPos);
			}
			if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
			{
				SingletonUnity<MiniMap>.Instance.OnlyDrawWalkLine(sourcePos, targetPos);
			}
			isDrawWalkLine = true;
			return;
		}
		LocationData playerLocationBig = GetPlayerLocationBig(sourcePos, 500f);
		if (playerLocationBig == null)
		{
			return;
		}
		LocationData playerLocationBig2 = GetPlayerLocationBig(targetPos, 500f);
		if (playerLocationBig2 == null)
		{
			return;
		}
		if (playerLocationBig == playerLocationBig2)
		{
			if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.OnlyDrawWalkLine(sourcePos, targetPos);
			}
			if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
			{
				SingletonUnity<MiniMap>.Instance.OnlyDrawWalkLine(sourcePos, targetPos);
			}
			isDrawWalkLine = true;
			return;
		}
		if (mCurPlayerLocationData == playerLocationBig && mCurTargetLocationData == playerLocationBig2)
		{
			if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.DrawMapLine(mCurPathList, sourcePos, targetPos);
			}
			if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
			{
				SingletonUnity<MiniMap>.Instance.DrawMapLine(mCurPathList, targetPos);
			}
			return;
		}
		mCurPlayerLocationData = playerLocationBig;
		mCurTargetLocationData = playerLocationBig2;
		mCurPathList.Clear();
		mCurPathList = GetPathList(playerLocationBig, playerLocationBig2);
		mCurPathList.Insert(0, sourcePos);
		mCurPathList.Add(targetPos);
		if (mCurPathList.Count == 0)
		{
			if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.OnlyDrawWalkLine(sourcePos, targetPos);
			}
			if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
			{
				SingletonUnity<MiniMap>.Instance.OnlyDrawWalkLine(sourcePos, targetPos);
			}
		}
		else
		{
			if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.DrawMapLine(mCurPathList, sourcePos, targetPos);
			}
			if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
			{
				SingletonUnity<MiniMap>.Instance.DrawMapLine(mCurPathList, targetPos);
			}
		}
	}

	private List<Vector3> GetPathList(LocationData sourceLoc, LocationData targetLoc)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		List<CityPathPointData> list = new List<CityPathPointData>();
		list.Add(sourceLoc.point1);
		list.Add(sourceLoc.point2);
		dictionary.Add(sourceLoc.point1.SelfIndex, -1);
		dictionary.Add(sourceLoc.point2.SelfIndex, -1);
		bool flag = false;
		int num = -1;
		while (list.Count > 0)
		{
			CityPathPointData cityPathPointData = list[0];
			list.RemoveAt(0);
			for (int i = 0; i < cityPathPointData.LinkPointIndex.Length; i++)
			{
				if (cityPathPointData.LinkPointIndex[i] != -1 && !dictionary.ContainsKey(cityPathPointData.LinkPointIndex[i]))
				{
					dictionary.Add(cityPathPointData.LinkPointIndex[i], cityPathPointData.SelfIndex);
					if (cityPathPointData.LinkPointIndex[i] == targetLoc.point1.SelfIndex || cityPathPointData.LinkPointIndex[i] == targetLoc.point2.SelfIndex)
					{
						flag = true;
						num = cityPathPointData.LinkPointIndex[i];
						break;
					}
					list.Add(PointDataList[cityPathPointData.LinkPointIndex[i]]);
				}
			}
			if (flag)
			{
				break;
			}
		}
		List<Vector3> list2 = new List<Vector3>();
		while (dictionary.ContainsKey(num) && dictionary[num] != -1)
		{
			list2.Insert(0, PointDataList[num].PointPos);
			num = dictionary[num];
		}
		return list2;
	}
}
