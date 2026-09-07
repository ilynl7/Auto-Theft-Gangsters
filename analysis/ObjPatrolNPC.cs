using System.Collections.Generic;
using UnityEngine;

public class ObjPatrolNPC : ObjNPC
{
	private List<Vector3> mPatrolPointList = new List<Vector3>();

	private int mCurPointIndex;

	private float mSearchAngle;

	private float mSearchDis;

	private ObjMainPlayer mMainPlayer;

	private GameObject mCheckAreaObj;

	private SceneManager mSceneManager;

	private float mCheckTimeCount;

	public List<Vector3> PatrolPointList
	{
		get
		{
			return mPatrolPointList;
		}
		set
		{
			mPatrolPointList = value;
		}
	}

	public int CurPointIndex
	{
		get
		{
			return mCurPointIndex;
		}
		set
		{
			mCurPointIndex = value;
		}
	}

	public float SearchAngle
	{
		get
		{
			return mSearchAngle;
		}
		set
		{
			mSearchAngle = value;
		}
	}

	public float SearchDis
	{
		get
		{
			return mSearchDis;
		}
		set
		{
			mSearchDis = value;
		}
	}

	public override void ResetNpc(ObjInitNpcData initData)
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		base.Reset();
		ServerId = initData.mServerID;
		base.Position = initData.mPos;
		mTransform.forward = initData.mDir;
		mNpcData = initData.npcInfoData;
		base.BornPos = initData.mPos;
		AttributeData.Camp = (GameDefine.CAMP_TYPE)initData.npcInfoData.Group;
		mNPCFunctionType = (GameDefine.NPC_FUNCTION_TYPE)initData.npcInfoData.FunctionType;
		mNPCType = (GameDefine.NPC_TYPE)initData.npcInfoData.Type;
		AttributeData.HP = initData.HP;
		AttributeData.MaxHP = initData.MaxHP;
		AttributeData.CurATK = initData.npcInfoData.Atk;
		AttributeData.CurDEF = initData.npcInfoData.Def;
		AttributeData.Name = initData.npcInfoData.Name;
		AttributeData.CurEXD = (float)mNpcData.EXD / 10000f;
		AttributeData.CurEXR = (float)mNpcData.EXR / 10000f;
		AttributeData.CurHIT = mNpcData.HIT;
		AttributeData.CurDGE = mNpcData.DGE;
		AttributeData.CurCRI = mNpcData.CRI;
		AttributeData.CurRES = mNpcData.RES;
		AttributeData.CurCRD = (float)mNpcData.CRD / 10000f;
		AttributeData.CurCRR = (float)mNpcData.CRR / 10000f;
		AttributeData.CurDEFA = mNpcData.DEFA;
		AttributeData.CurDGEA = initData.DGEA;
		AttributeData.CurRESA = initData.RESA;
		AttributeData.CurHITA = initData.HITA;
		AttributeData.CurCRIA = initData.CRIA;
		AttributeData.CurAntiKnockDown = (float)initData.AntiKnockDown / 10000f;
		AttributeData.CurAntiStun = (float)initData.AntiStun / 10000f;
		AttributeData.Level = initData.Level;
		AttributeData.CurSpeed = initData.npcInfoData.MoveSpeedMeter;
		AttributeData.WalkSpeed = initData.npcInfoData.WalkSpeedMeter;
		InitNavMeshAgent();
		if (mTransform.childCount > 0)
		{
			Transform child = mTransform.GetChild(0);
			child.localScale = Vector3.one * initData.npcInfoData.ModelScale;
		}
		mCurPointIndex = 1;
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		mSearchDis = 5f;
		mSearchAngle = 60f;
		if (mCheckAreaObj == null)
		{
			mCheckAreaObj = new GameObject("CheckArea");
			mCheckAreaObj.transform.parent = base.CacheTransform;
			mCheckAreaObj.transform.localPosition = Vector3.up * 0.2f;
			mCheckAreaObj.transform.localRotation = Quaternion.identity;
			Light light = mCheckAreaObj.AddComponent<Light>();
			light.type = LightType.Spot;
			light.range = mSearchDis;
			light.spotAngle = mSearchAngle;
			light.color = Color.white;
			light.cullingMask = 8388608;
			light.intensity = 8f;
			light.renderMode = LightRenderMode.ForcePixel;
		}
		mSceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
	}

	public void SetPathPoint(List<Vector3> pointList)
	{
		for (int i = 0; i < pointList.Count; i++)
		{
			mPatrolPointList.Add(pointList[i]);
		}
		WalkMoveTo(mPatrolPointList[mCurPointIndex], 1f, OnArrivePoint);
	}

	private void OnArrivePoint(ObjCharacter objCha)
	{
		mCurPointIndex = (mCurPointIndex + 1) % mPatrolPointList.Count;
		WalkMoveTo(mPatrolPointList[mCurPointIndex], 1f, OnArrivePoint);
	}

	public void ResetToPathBegin()
	{
		base.CacheTransform.position = mPatrolPointList[0];
		mCurPointIndex = 0;
		OnArrivePoint(this);
	}

	private void Update()
	{
		UpdateComponent();
		UpdateMove();
		mCheckTimeCount += Time.deltaTime;
		if (mCheckTimeCount > 0.1f)
		{
			mCheckTimeCount = 0f;
			CheckFindPlayer();
		}
	}

	private void CheckFindPlayer()
	{
		if (mMainPlayer != null)
		{
			if (AreaCheckTool.CheckInSector(mMainPlayer.Position, base.CacheTransform, mSearchDis, mSearchAngle))
			{
				mSceneManager.OnFindPlayer();
			}
		}
		else
		{
			mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
	}

	public void RecycleSelf()
	{
		Recyle();
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
		if (mAnimationLogic.AnimaObj != null)
		{
			if (mAnimationLogic.AnimaObj["idle"] == null)
			{
				mAnimationLogic.LoadAnim(DataManager.GetActionDataByName("idle"));
			}
			mAnimationLogic.AnimaObj.Play("idle");
			mAnimationLogic.AnimaObj["idle"].time = 0f;
			mAnimationLogic.AnimaObj.Sample();
		}
		Singleton<ObjManager>.Instance.RecyclePatrolNPC(this);
	}
}
