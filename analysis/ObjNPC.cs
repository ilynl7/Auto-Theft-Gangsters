using System;
using System.Collections.Generic;
using DG.Tweening;
using SprotoType;
using UnityEngine;

public class ObjNPC : ObjCharacter
{
	private const float mDialogRange = 5f;

	private Material mMeshMat;

	protected NpcData mNpcData;

	protected AutoMoveLogic mAutoMoveLogic;

	protected AILogic mAILogic;

	protected BundleManager.LoadModelData mLoadingModelData;

	protected long mLoadingModelDataId;

	protected GameObject mMeshRoot;

	protected Vector3 mBornPos;

	protected float mPatrolRange = 5f;

	protected float mSearchRange = 7f;

	private Transform mFollowTarget;

	public vp_Timer.Handle dieDelayHandle;

	protected GameDefine.NPC_FUNCTION_TYPE mNPCFunctionType;

	protected GameDefine.NPC_TYPE mNPCType;

	private ObjCharacter mSelectedTarget;

	protected string mPathID;

	private List<SkillData> mEnableSkillIDList = new List<SkillData>();

	private string mPlayerName = string.Empty;

	private long mGuildId = -1L;

	private long mTeamId = -1L;

	private ParticleSystem SmokeParticle;

	private ParticleSystem ExplosionParticle;

	private Transform FLWheel;

	private Transform FRWheel;

	private Transform BLWheel;

	private Transform BRWheel;

	private Vector3 FLPos;

	private Vector3 FRPos;

	private Vector3 BLPos;

	private Vector3 BRPos;

	protected string mDefaultDialogID = string.Empty;

	private List<string> mMissionIdList = new List<string>();

	private NPCHeadInfoLogic mNpcHeadInfoLogic;

	public ObjManager.OnGetNPC onLoadMeshFinished;

	private Transform mMainPlayerTransform;

	private bool mInCircleFlag;

	private float SqrActiveRadius = 16f;

	private float ExitSqrActiveRadius = 20.25f;

	private float sqrDis;

	private List<ObjCharacter> mCurSearchTargetList;

	private int mServerNotDieNumCount;

	public string NPCDataID
	{
		get
		{
			if (mNpcData != null)
			{
				return mNpcData.ID;
			}
			return "-1";
		}
	}

	public Material MeshMat
	{
		get
		{
			return mMeshMat;
		}
		set
		{
			mMeshMat = value;
		}
	}

	public NpcData NPCData
	{
		get
		{
			return mNpcData;
		}
		set
		{
			mNpcData = value;
		}
	}

	public AutoMoveLogic AutoMoveLogic
	{
		get
		{
			return mAutoMoveLogic;
		}
		set
		{
			mAutoMoveLogic = value;
		}
	}

	public AILogic AILogic
	{
		get
		{
			return mAILogic;
		}
		set
		{
			mAILogic = value;
		}
	}

	public BundleManager.LoadModelData LoadingModelData
	{
		get
		{
			return mLoadingModelData;
		}
		set
		{
			mLoadingModelData = value;
		}
	}

	public long LoadingModelDataId
	{
		get
		{
			return mLoadingModelDataId;
		}
		set
		{
			mLoadingModelDataId = value;
		}
	}

	public GameObject MeshRoot
	{
		get
		{
			return mMeshRoot;
		}
		set
		{
			mMeshRoot = value;
		}
	}

	public Vector3 BornPos
	{
		get
		{
			return mBornPos;
		}
		set
		{
			mBornPos = value;
		}
	}

	public float PatrolRange
	{
		get
		{
			return mPatrolRange;
		}
		set
		{
			mPatrolRange = value;
		}
	}

	public float SearchRange
	{
		get
		{
			return mSearchRange;
		}
		set
		{
			mSearchRange = value;
		}
	}

	public Transform FollowTarget
	{
		get
		{
			return mFollowTarget;
		}
		set
		{
			mFollowTarget = value;
		}
	}

	public GameDefine.NPC_FUNCTION_TYPE NPCFunctionType
	{
		get
		{
			return mNPCFunctionType;
		}
		set
		{
			mNPCFunctionType = value;
		}
	}

	public GameDefine.NPC_TYPE NPCType
	{
		get
		{
			return mNPCType;
		}
		set
		{
			mNPCType = value;
		}
	}

	public ObjCharacter SelectedTarget
	{
		get
		{
			return mSelectedTarget;
		}
		set
		{
			mSelectedTarget = value;
		}
	}

	public string PathID
	{
		get
		{
			return mPathID;
		}
		set
		{
			mPathID = value;
		}
	}

	public List<SkillData> EnableSkillIDList => mEnableSkillIDList;

	public string PlayerName => mPlayerName;

	public long GuildId
	{
		get
		{
			return mGuildId;
		}
		set
		{
			mGuildId = value;
		}
	}

	public long TeamId
	{
		get
		{
			return mTeamId;
		}
		set
		{
			mTeamId = value;
		}
	}

	public string DefaultDialogID
	{
		get
		{
			return mDefaultDialogID;
		}
		set
		{
			mDefaultDialogID = value;
		}
	}

	public List<string> MissionIdList
	{
		get
		{
			return mMissionIdList;
		}
		set
		{
			mMissionIdList = value;
		}
	}

	public NPCHeadInfoLogic NpcHeadInfoLogic
	{
		get
		{
			return mNpcHeadInfoLogic;
		}
		set
		{
			mNpcHeadInfoLogic = value;
		}
	}

	public ObjNPC()
	{
		mObjType = GameDefine.OBJ_TYPE.OBJ_NPC;
	}

	protected void ShowMesh()
	{
		if (!(mMeshMat == null) && mMeshMat.HasProperty("_DefaultColor"))
		{
			Color color = mMeshMat.GetColor("_DefaultColor");
			mMeshMat.SetColor("_Color", new Color(color.r, color.g, color.b, 0f));
			mMeshMat.DOColor(color, 1f);
		}
	}

	public static Vector3 HeadingToVector3(float heading)
	{
		float x = Mathf.Sin(heading * ((float)Math.PI / 180f));
		float z = Mathf.Cos(heading * ((float)Math.PI / 180f));
		return new Vector3(x, 0f, z);
	}

	private void AddMissionToList(string missionId)
	{
		if (!string.IsNullOrEmpty(missionId))
		{
			mMissionIdList.Add(missionId);
		}
	}

	private void ClearMissionList()
	{
		mMissionIdList.Clear();
	}

	public bool IsContainsMission(string missionId)
	{
		if (mMissionIdList.Contains(missionId))
		{
			return true;
		}
		return false;
	}

	protected void AddDialogMission()
	{
		if (!IsMissionNpc())
		{
			return;
		}
		NPCDialogData nPCDialogDataByID = DataManager.GetNPCDialogDataByID(mDefaultDialogID);
		if (nPCDialogDataByID == null)
		{
			Debug.LogWarning("mission npc no dialogData");
		}
		else if (nPCDialogDataByID.MissionIDList != null && nPCDialogDataByID.MissionIDList.Count != 0)
		{
			for (int i = 0; i < nPCDialogDataByID.MissionIDList.Count; i++)
			{
				AddMissionToList(nPCDialogDataByID.MissionIDList[i]);
			}
		}
	}

	public bool CheckInDialogRange()
	{
		float num = ((!Singleton<ObjManager>.Instance.MainPlayer.IsLocalDrivingCar) ? 5 : 10);
		if (VectorXZ.Distance(base.Position, Singleton<ObjManager>.Instance.MainPlayer.Position) < num)
		{
			return true;
		}
		return false;
	}

	public bool IsMissionNpc()
	{
		return mNPCType == GameDefine.NPC_TYPE.MISSION;
	}

	public bool IsSoundBoxNpc()
	{
		return mNpcData != null && mNpcData.FunctionType == 3;
	}

	public bool IsShopFunctionNpc()
	{
		return mNpcData != null && mNpcData.FunctionType >= 4 && mNpcData.FunctionType <= 9;
	}

	public bool IsCityCaptureNpc()
	{
		return mNpcData != null && mNpcData.FunctionType == 10 && mNpcData.Group == 4;
	}

	public void ChangeToAttackNPC()
	{
		mDefaultDialogID = string.Empty;
		if (mAILogic == null)
		{
			mAILogic = base.gameObject.AddComponent<AILogic>();
			mAILogic.ResetAI(mNpcData.AI, mNpcData.AIID, mPathID);
		}
		mNavMeshAgent.enabled = true;
		AttributeData.HP = AttributeData.MaxHP;
	}

	public void ChangeNpcAI(string aiId)
	{
		if (mAILogic == null)
		{
			mAILogic = base.gameObject.AddComponent<AILogic>();
			mAILogic.ResetAI(mNpcData.AI, mNpcData.AIID, mPathID);
		}
		else
		{
			mAILogic.ResetAI(string.Empty, aiId, mPathID);
		}
	}

	public override void Init()
	{
		base.Init();
		if (mAutoMoveLogic == null)
		{
			mAutoMoveLogic = base.gameObject.AddComponent<AutoMoveLogic>();
		}
		mAutoMoveLogic.Init(this);
	}

	public void InitNPCHeadInfo()
	{
		if (mNPCType != GameDefine.NPC_TYPE.BOSS)
		{
			ResourcesManager.LoadHeadInfoPrefab(UIInfo.NPCHeadInfoUI, "NPCHeadInfoRoot", LoadNPCHeadInfo);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BossXueTiaoUI, delegate
		{
			SingletonUnity<BossXueTiaoLogicNew>.Instance.RegisterBoss(this);
			SingletonUnity<BossXueTiaoLogicNew>.Instance.resetHPinfo(AttributeData);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BossXueTiaoUI);
			SingletonUnity<UIManager>.Instance.CheckFunctionTop(isshowbossline: false);
		});
	}

	public void UpdateNpcHeadInfo(HEAD_PIC_TYPE type)
	{
		if (mNPCType != GameDefine.NPC_TYPE.BOSS)
		{
			mNpcHeadInfoLogic.SetNameLabel(AttributeData.Name, AttributeData.Camp, needShowHPLine: false, type);
		}
	}

	private void LoadNPCHeadInfo(GameObject obj)
	{
		if (!(obj != null))
		{
			return;
		}
		BillBoard billBoard = obj.GetComponent<BillBoard>();
		if (billBoard == null)
		{
			billBoard = obj.AddComponent<BillBoard>();
		}
		billBoard.enabled = true;
		billBoard.BindObj = base.gameObject;
		billBoard.DeltaHeight = base.CurrentCharacterModelData.ModelHeight + 0.3f;
		mNpcHeadInfoLogic = obj.GetComponent<NPCHeadInfoLogic>();
		mHeadInfoLogic = mNpcHeadInfoLogic;
		if (mNPCType == GameDefine.NPC_TYPE.BOSS)
		{
			return;
		}
		if (mNpcData.Type == 4)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if ((playerData.IsHaveTeam() && playerData.TeamInfo.TeamID == mTeamId) || (playerData.IsHaveGuild() && playerData.PlayerGuild.ServerId == mGuildId))
			{
				mNpcHeadInfoLogic.SetNameLabel($"{mPlayerName}'s {AttributeData.Name}", AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.SELF_ESCORT_NPC, needShowName: true);
			}
			else
			{
				CurMission escortMission = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetEscortMission();
				if (escortMission != null && escortMission.GetParam(1) == ServerId)
				{
					mNpcHeadInfoLogic.SetNameLabel($"{mPlayerName}'s {AttributeData.Name}", AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.SELF_ESCORT_NPC, needShowName: true);
				}
				else
				{
					mNpcHeadInfoLogic.SetNameLabel($"{mPlayerName}'s {AttributeData.Name}", AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.OTHER_ESCORT_NPC, needShowName: true);
				}
			}
		}
		else if (AttributeData.Camp == GameDefine.CAMP_TYPE.FUNCTION_NPC)
		{
			UpdateMissionNpcHead();
		}
		else
		{
			mNpcHeadInfoLogic.SetNameLabel(AttributeData.Name, AttributeData.Camp, needShowHPLine: false);
		}
		mNpcHeadInfoLogic.ForceSetHpVal((float)AttributeData.HP / (float)AttributeData.MaxHP);
	}

	public void UpdateMissionNpcHead()
	{
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		Dictionary<string, List<string>> curMissionNpcDic = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurMissionNpcDic;
		if (curMissionNpcDic.ContainsKey(NPCDataID) && curMissionNpcDic[NPCDataID] != null && curMissionNpcDic[NPCDataID].Count > 0)
		{
			MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			MISSION_STATE mISSION_STATE = MISSION_STATE.INVALID;
			MISSION_STATE mISSION_STATE2 = MISSION_STATE.INVALID;
			string text = string.Empty;
			for (int i = 0; i < curMissionNpcDic[NPCDataID].Count; i++)
			{
				switch (missionManager.GetMissionState(curMissionNpcDic[NPCDataID][i]))
				{
				case MISSION_STATE.COMPLETE:
				{
					MissionData missionDataByID = DataManager.GetMissionDataByID(curMissionNpcDic[NPCDataID][i]);
					if (level < missionDataByID.MinLv)
					{
						text = curMissionNpcDic[NPCDataID][i];
						continue;
					}
					break;
				}
				case MISSION_STATE.ACCEPTED:
					mISSION_STATE = MISSION_STATE.ACCEPTED;
					text = curMissionNpcDic[NPCDataID][i];
					continue;
				default:
					continue;
				}
				mISSION_STATE = MISSION_STATE.COMPLETE;
				text = curMissionNpcDic[NPCDataID][i];
				break;
			}
			switch (mISSION_STATE)
			{
			case MISSION_STATE.ACCEPTED:
				mNpcHeadInfoLogic.SetNameLabel(AttributeData.Name, AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.MISSION_ACCEPT_NPC, GameSettingData.IsShowNormalNpcName);
				return;
			case MISSION_STATE.COMPLETE:
				mNpcHeadInfoLogic.SetNameLabel(AttributeData.Name, AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.MISSION_COMPLETE_NPC, GameSettingData.IsShowNormalNpcName);
				return;
			}
			if (!string.IsNullOrEmpty(text) && missionManager.IsMissionAccepted(text))
			{
				mNpcHeadInfoLogic.SetNameLabel(AttributeData.Name, AttributeData.Camp, needShowHPLine: false);
			}
			else
			{
				mNpcHeadInfoLogic.SetNameLabel(AttributeData.Name, AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.MISSION_TARGET_NPC, GameSettingData.IsShowNormalNpcName);
			}
		}
		else
		{
			mNpcHeadInfoLogic.SetNameLabel(AttributeData.Name, AttributeData.Camp, needShowHPLine: false);
		}
	}

	public void UpdateEscortNpcCamp(long guildId, long teamId)
	{
		if (NPCData.Type == 4)
		{
			GuildId = guildId;
			TeamId = teamId;
			UpdateEscortNpcCamp();
		}
	}

	public void UpdateEscortNpcCamp()
	{
		if (NPCData.Type != 4 || base.IsDie)
		{
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if ((playerData.IsHaveGuild() && playerData.PlayerGuild.ServerId == GuildId) || (playerData.IsHaveTeam() && playerData.TeamInfo.TeamID == TeamId))
		{
			if (AttributeData.Camp == GameDefine.CAMP_TYPE.NORMAL_NPC)
			{
				Singleton<ObjManager>.Instance.ChangeCamp(this, AttributeData.Camp, (GameDefine.CAMP_TYPE)NPCData.Group);
				AttributeData.Camp = (GameDefine.CAMP_TYPE)NPCData.Group;
				NpcHeadInfoLogic.SetNameLabel($"{PlayerName}'s {AttributeData.Name}", AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.SELF_ESCORT_NPC, needShowName: true);
				if (Singleton<ObjManager>.Instance.MainPlayer.SelectedTarget != null && Singleton<ObjManager>.Instance.MainPlayer.SelectedTarget.ServerId == ServerId)
				{
					Singleton<ObjManager>.Instance.MainPlayer.SelectTarget(null);
				}
			}
			return;
		}
		CurMission escortMission = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetEscortMission();
		if (escortMission != null && escortMission.GetParam(1) == ServerId)
		{
			if (AttributeData.Camp == GameDefine.CAMP_TYPE.NORMAL_NPC)
			{
				Singleton<ObjManager>.Instance.ChangeCamp(this, AttributeData.Camp, (GameDefine.CAMP_TYPE)NPCData.Group);
				AttributeData.Camp = (GameDefine.CAMP_TYPE)NPCData.Group;
				NpcHeadInfoLogic.SetNameLabel($"{PlayerName}'s {AttributeData.Name}", AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.SELF_ESCORT_NPC, needShowName: true);
				if (Singleton<ObjManager>.Instance.MainPlayer.SelectedTarget != null && Singleton<ObjManager>.Instance.MainPlayer.SelectedTarget.ServerId == ServerId)
				{
					Singleton<ObjManager>.Instance.MainPlayer.SelectTarget(null);
				}
			}
		}
		else if (AttributeData.Camp != GameDefine.CAMP_TYPE.NORMAL_NPC)
		{
			Singleton<ObjManager>.Instance.ChangeCamp(this, AttributeData.Camp, GameDefine.CAMP_TYPE.NORMAL_NPC);
			AttributeData.Camp = GameDefine.CAMP_TYPE.NORMAL_NPC;
			NpcHeadInfoLogic.SetNameLabel($"{PlayerName}'s {AttributeData.Name}", AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.OTHER_ESCORT_NPC, needShowName: true);
		}
	}

	public void LoadModelFinishInit()
	{
		if (IsCityCaptureNpc())
		{
			Transform transform = base.transform.FindChild("MeshRoot/effect_Smoke");
			Transform transform2 = base.transform.FindChild("MeshRoot/effect_baoZha");
			if (transform != null)
			{
				SmokeParticle = transform.gameObject.GetComponent<ParticleSystem>();
				SmokeParticle.Stop();
				UnityVersionUtil.SetActiveRecursive(SmokeParticle.gameObject, state: false);
			}
			if (transform2 != null)
			{
				ExplosionParticle = transform2.gameObject.GetComponent<ParticleSystem>();
				ExplosionParticle.Stop();
				UnityVersionUtil.SetActiveRecursive(ExplosionParticle.gameObject, state: false);
			}
			if (FLWheel == null)
			{
				FLWheel = base.transform.FindChild("MeshRoot/qianlun-L").transform;
				FLPos = FLWheel.transform.localPosition;
			}
			if (FRWheel == null)
			{
				FRWheel = base.transform.FindChild("MeshRoot/qianlun-R").transform;
				FRPos = FRWheel.transform.localPosition;
			}
			if (BLWheel == null)
			{
				BLWheel = base.transform.FindChild("MeshRoot/houlun-L").transform;
				BLPos = BLWheel.transform.localPosition;
			}
			if (BRWheel == null)
			{
				BRWheel = base.transform.FindChild("MeshRoot/houlun-R").transform;
				BRPos = BRWheel.transform.localPosition;
			}
		}
		if (!IsSoundBoxNpc() && !IsCityCaptureNpc())
		{
			mAnimationLogic.Init(this);
		}
		if (onLoadMeshFinished != null)
		{
			onLoadMeshFinished(this);
		}
		ShowMesh();
		if (IsCityCaptureNpc() && AttributeData.HP <= 0)
		{
			base.IsDie = false;
			ChangeHPVal(0L);
		}
	}

	private void OnDisable()
	{
		if (mNavMeshAgent != null && mNavMeshAgent.enabled)
		{
			mNavMeshAgent.Stop();
			mNavMeshAgent.ResetPath();
			mNavMeshAgent.enabled = false;
		}
	}

	public virtual void ResetNpc(ObjInitNpcData initData)
	{
		mNPCType = (GameDefine.NPC_TYPE)initData.npcInfoData.Type;
		base.Reset();
		ServerId = initData.mServerID;
		base.Position = initData.mPos;
		mTransform.forward = initData.mDir;
		mNpcData = initData.npcInfoData;
		BornPos = initData.mPos;
		mPlayerName = initData.PlayerName;
		mTeamId = initData.TeamId;
		mGuildId = initData.GuildId;
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		if (mNpcData.Type == 4)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if ((playerData.IsHaveTeam() && playerData.TeamInfo.TeamID == mTeamId) || (playerData.IsHaveGuild() && playerData.PlayerGuild.ServerId == mGuildId))
			{
				AttributeData.Camp = (GameDefine.CAMP_TYPE)initData.npcInfoData.Group;
			}
			else if (missionManager.IsInEscortMission() && ServerId == missionManager.GetEscortMission().GetParam(1))
			{
				AttributeData.Camp = (GameDefine.CAMP_TYPE)initData.npcInfoData.Group;
			}
			else
			{
				AttributeData.Camp = GameDefine.CAMP_TYPE.NORMAL_NPC;
			}
			UIUpdateEvent.OnChangeTeam = (DelegateDefine.NoParamDelegate)Delegate.Combine(UIUpdateEvent.OnChangeTeam, new DelegateDefine.NoParamDelegate(UpdateEscortNpcCamp));
			UIUpdateEvent.OnChangeGuild = (DelegateDefine.NoParamDelegate)Delegate.Combine(UIUpdateEvent.OnChangeGuild, new DelegateDefine.NoParamDelegate(UpdateEscortNpcCamp));
		}
		else
		{
			AttributeData.Camp = (GameDefine.CAMP_TYPE)initData.npcInfoData.Group;
		}
		mNPCFunctionType = (GameDefine.NPC_FUNCTION_TYPE)initData.npcInfoData.FunctionType;
		AttributeData.MaxHP = initData.MaxHP;
		AttributeData.HP = initData.HP;
		AttributeData.Name = initData.npcInfoData.Name;
		AttributeData.CurATK = initData.ATK;
		AttributeData.CurDEF = initData.DEF;
		AttributeData.CurEXD = (float)initData.EXD / 10000f;
		AttributeData.CurEXR = (float)initData.EXR / 10000f;
		AttributeData.CurHIT = initData.HIT;
		AttributeData.CurDGE = initData.EVA;
		AttributeData.CurCRI = initData.CRI;
		AttributeData.CurRES = initData.RES;
		AttributeData.CurCRD = (float)initData.CRD / 10000f;
		AttributeData.CurCRR = (float)initData.CRR / 10000f;
		AttributeData.CurDEFA = initData.DEFA;
		AttributeData.CurDGEA = initData.DGEA;
		AttributeData.CurRESA = initData.RESA;
		AttributeData.CurHITA = initData.HITA;
		AttributeData.CurCRIA = initData.CRIA;
		AttributeData.CurAntiKnockDown = (float)initData.AntiKnockDown / 10000f;
		AttributeData.CurAntiStun = (float)initData.AntiStun / 10000f;
		AttributeData.Level = initData.Level;
		AttributeData.CurSpeed = initData.npcInfoData.MoveSpeedMeter;
		AttributeData.WalkSpeed = initData.npcInfoData.WalkSpeedMeter;
		mPatrolRange = initData.npcInfoData.PatrolRadius;
		mSearchRange = initData.npcInfoData.SearchRadius;
		mPathID = initData.PathID;
		if (dieDelayHandle != null)
		{
			dieDelayHandle.Cancel();
		}
		mDefaultDialogID = initData.npcInfoData.TalkGroup;
		InitSkill(mNpcData.SkillList);
		InitNavMeshAgent();
		mNavMeshAgent.walkableMask += 56;
		InitNPCHeadInfo();
		AddDialogMission();
		if (mAutoMoveLogic != null)
		{
			mAutoMoveLogic.Reset();
		}
		if (mMeshRoot != null)
		{
			mMeshRoot.transform.localScale = Vector3.one * initData.npcInfoData.ModelScale;
		}
		CapsuleCollider component = base.gameObject.GetComponent<CapsuleCollider>();
		if (component != null)
		{
			component.height = base.CurrentCharacterModelData.ModelHeight * 1.2f;
			component.center = Vector3.up * base.CurrentCharacterModelData.ModelHeight / 2f * 1.2f;
			component.radius = base.CurrentCharacterModelData.ModelRadius;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsSingleCopyScene() || SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsLowPhoneManager())
		{
			if (!IsMissionNpc() && AttributeData.Camp != GameDefine.CAMP_TYPE.FUNCTION_NPC)
			{
				if (mAILogic == null)
				{
					mAILogic = base.gameObject.AddComponent<AILogic>();
				}
				if (mAILogic != null)
				{
					mAILogic.ResetAI(mNpcData.AI, mNpcData.AIID, initData.PathID);
				}
			}
		}
		else if (mNpcData.AI.Equals("FollowAI"))
		{
			if (mAILogic == null)
			{
				mAILogic = base.gameObject.AddComponent<AILogic>();
			}
			if (mAILogic != null)
			{
				mAILogic.ResetAI(mNpcData.AI, mNpcData.AIID, initData.PathID);
			}
		}
		else
		{
			if (mAILogic != null)
			{
				UnityEngine.Object.Destroy(mAILogic);
				mAILogic = null;
			}
			if (IsMissionNpc() || IsSoundBoxNpc() || IsCityCaptureNpc())
			{
				mNavMeshAgent.enabled = false;
				NavMesh.SamplePosition(initData.mPos, out var hit, 1f, -1);
				base.Position = hit.position;
			}
			else
			{
				mNavMeshAgent.enabled = true;
			}
		}
		mServerNotDieNumCount = 0;
		ShowMesh();
		if (!IsCityCaptureNpc())
		{
			return;
		}
		if (AttributeData.HP <= 0)
		{
			ChangeHPVal(0L);
			return;
		}
		if (SmokeParticle != null)
		{
			SmokeParticle.Stop();
			UnityVersionUtil.SetActiveRecursive(SmokeParticle.gameObject, state: false);
		}
		if (ExplosionParticle != null)
		{
			ExplosionParticle.Stop();
			UnityVersionUtil.SetActiveRecursive(ExplosionParticle.gameObject, state: false);
		}
		if (MeshRoot != null)
		{
			MeshRoot.SampleAnimation(MeshRoot.animation.GetClip("GTACheBaoZa_Animation"), 0f);
			if (FLWheel != null)
			{
				FLWheel.localPosition = FLPos;
			}
			if (FRWheel != null)
			{
				FRWheel.localPosition = FRPos;
			}
			if (BLWheel != null)
			{
				BLWheel.localPosition = BLPos;
			}
			if (BRWheel != null)
			{
				BRWheel.localPosition = BRPos;
			}
		}
	}

	public void OnRecycle()
	{
		if (NPCType == GameDefine.NPC_TYPE.ESCORT)
		{
			UIUpdateEvent.OnChangeTeam = (DelegateDefine.NoParamDelegate)Delegate.Remove(UIUpdateEvent.OnChangeTeam, new DelegateDefine.NoParamDelegate(UpdateEscortNpcCamp));
			UIUpdateEvent.OnChangeGuild = (DelegateDefine.NoParamDelegate)Delegate.Remove(UIUpdateEvent.OnChangeGuild, new DelegateDefine.NoParamDelegate(UpdateEscortNpcCamp));
		}
	}

	public void ChangeBornPos(Vector3 pos)
	{
		BornPos = pos;
	}

	public void InitSkill(string[] skillList)
	{
		CharacterSkillData.Clear();
		mEnableSkillIDList.Clear();
		if (skillList != null)
		{
			List<SkillData> list = new List<SkillData>();
			for (int i = 0; i < skillList.Length; i++)
			{
				CharacterSkillData.Add(new CharacterSkillData(skillList[i]));
				list.Add(DataManager.GetSkillDataById(skillList[i]));
			}
			list.Sort((SkillData x, SkillData y) => y.PriorityAutoCombat - x.PriorityAutoCombat);
			for (int j = 0; j < list.Count; j++)
			{
				mEnableSkillIDList.Add(list[j]);
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!IsSoundBoxNpc())
		{
			if (IsCityCaptureNpc())
			{
				updateBossHPState();
				return;
			}
			UpdateComponent();
			UpdateMove();
			UpdateSkillCD();
			UpdateHoldTime();
			base.SkillLogic.UpdateSkill();
			updateBossHPState();
		}
	}

	private void FixedUpdate()
	{
		if (!IsShopFunctionNpc())
		{
			return;
		}
		if (null == mMainPlayerTransform)
		{
			if (null != Singleton<ObjManager>.Instance.MainPlayer)
			{
				mMainPlayerTransform = Singleton<ObjManager>.Instance.MainPlayer.transform;
			}
			if (null == mMainPlayerTransform)
			{
				return;
			}
		}
		if (!(null != Singleton<ObjManager>.Instance.MainPlayer))
		{
			return;
		}
		sqrDis = (mMainPlayerTransform.position - base.CacheTransform.position).sqrMagnitude;
		if (sqrDis <= SqrActiveRadius)
		{
			if (!mInCircleFlag)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ActivityTipstRoot, delegate
				{
					SingletonUnity<ActivityTipsRootLogic>.Instance.ShowShopNpcInfo(this);
				});
				mInCircleFlag = true;
			}
		}
		else if (sqrDis > ExitSqrActiveRadius)
		{
			if (mInCircleFlag && SingletonUnity<ActivityTipsRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ActivityTipsRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ActivityTipsRootLogic>.Instance.CloseUI();
			}
			mInCircleFlag = false;
		}
	}

	public void UseSkill(string skillId, SkillUseSuccess onSkillUseSuccess = null)
	{
		if (BeforeSkillCheck() || mMeshRoot == null)
		{
			return;
		}
		ObjCharacter objCharacter = null;
		if (mSelectedTarget != null && mSelectedTarget.ServerId != ServerId)
		{
			if (mSelectedTarget.IsDie)
			{
				mSelectedTarget = null;
			}
			else
			{
				objCharacter = mSelectedTarget;
			}
		}
		if (objCharacter == null)
		{
			objCharacter = (mSelectedTarget = ChooseTarget(9999f));
		}
		mCurUseSkillId = skillId;
		EnterCombat(objCharacter, onSkillUseSuccess);
	}

	public ObjCharacter ChooseTarget(float minSqrDis = 9999f)
	{
		ObjCharacter result = null;
		bool flag = false;
		bool flag2 = false;
		mCurSearchTargetList = Singleton<ObjManager>.Instance.CampTargetList[(int)AttributeData.Camp];
		for (int i = 0; i < mCurSearchTargetList.Count; i++)
		{
			if (mCurSearchTargetList[i].ServerId == ServerId || mCurSearchTargetList[i].IsDie || (mCurSearchTargetList[i].AttributeData.Camp == GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC && mCurSearchTargetList[i].ObjType == GameDefine.OBJ_TYPE.OBJ_PLAYER_CAR))
			{
				continue;
			}
			if (CampTool.IsFirstClass(AttributeData.Camp, mCurSearchTargetList[i].AttributeData.Camp))
			{
				flag2 = true;
			}
			if (!flag)
			{
				float pathDistance = GetPathDistance(mCurSearchTargetList[i].Position);
				if (flag2)
				{
					flag = true;
					result = mCurSearchTargetList[i];
					minSqrDis = pathDistance;
				}
				else if (minSqrDis > pathDistance)
				{
					result = mCurSearchTargetList[i];
					minSqrDis = pathDistance;
				}
			}
			else if (flag2)
			{
				float pathDistance = GetPathDistance(mCurSearchTargetList[i].Position);
				if (minSqrDis > pathDistance)
				{
					result = mCurSearchTargetList[i];
					minSqrDis = pathDistance;
				}
			}
		}
		return result;
	}

	public override void EnterCombat(ObjCharacter target, SkillUseSuccess onSkillUseSuccess = null)
	{
		SkillData skillDataById = DataManager.GetSkillDataById(base.CurUseSkillId);
		if (skillDataById == null || !(target != null))
		{
			return;
		}
		if (target.IsLocalDrivingCar)
		{
			target = target.CurPlayerCar;
		}
		if (!CheckSkillDistance(skillDataById, target))
		{
			MoveTo(target.Position, skillDataById.TraceDistanceMeter + target.ModelRadius + ModelRadius - 0.5f);
		}
		else if (CheckSkillCD(skillDataById))
		{
			if (onSkillUseSuccess != null)
			{
				mSkillUseSuccessDic.Add(base.CurUseSkillId, onSkillUseSuccess);
			}
			if (target != null)
			{
				base.SkillLogic.UseSkill(base.CurUseSkillId, ServerId, target.ServerId);
			}
			else
			{
				base.SkillLogic.UseSkill(base.CurUseSkillId, ServerId, -1L);
			}
		}
	}

	public override void OnSkillUseSuccess(string skillId)
	{
		base.OnSkillUseSuccess(skillId);
		OnSkillDisable(base.CurUseSkillId);
	}

	public override void ChangeHPVal(long newHP)
	{
		if (!base.IsDie)
		{
			if (newHP > AttributeData.HP && Time.time - mReceiveBiggerHpTimeCount < mReceiveBiggerHpTime)
			{
				return;
			}
			if (AttributeData.HP != newHP)
			{
				AttributeData.HP = newHP;
				UpdateHeadInfo();
			}
			if (mNPCType == GameDefine.NPC_TYPE.BOSS && SingletonUnity<BossXueTiaoLogicNew>.Exists)
			{
				SingletonUnity<BossXueTiaoLogicNew>.Instance.ChangeHP(newHP, this);
			}
			if (AttributeData.HP > 0)
			{
				return;
			}
			OnDie();
			if (!IsCityCaptureNpc())
			{
				float delay = 4f;
				if (dieDelayHandle == null)
				{
					dieDelayHandle = new vp_Timer.Handle();
				}
				vp_Timer.In(delay, delegate
				{
					DelayRecycle();
				}, dieDelayHandle);
			}
		}
		else if (newHP <= 0)
		{
			if (!IsCityCaptureNpc())
			{
				float delay2 = 10f;
				if (dieDelayHandle == null)
				{
					dieDelayHandle = new vp_Timer.Handle();
				}
				vp_Timer.In(delay2, delegate
				{
					DelayRecycle();
				}, dieDelayHandle);
			}
		}
		else
		{
			mServerNotDieNumCount++;
			if (mServerNotDieNumCount > GameDefine.NPC_SERVER_WAIT_RELIFE_NUM)
			{
				OnRelife(newHP, base.Position);
				mServerNotDieNumCount = 0;
			}
		}
	}

	public override void OnRelife(long hp, Vector3 pos)
	{
		base.OnRelife(hp, pos);
		if (mNPCType == GameDefine.NPC_TYPE.BOSS)
		{
			if (SingletonUnity<BossXueTiaoLogicNew>.Exists)
			{
				SingletonUnity<BossXueTiaoLogicNew>.Instance.ChangeHP(hp, this);
			}
			return;
		}
		if (mNpcData.Type == 4)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if ((playerData.IsHaveTeam() && playerData.TeamInfo.TeamID == mTeamId) || (playerData.IsHaveGuild() && playerData.PlayerGuild.ServerId == mGuildId))
			{
				mNpcHeadInfoLogic.SetNameLabel($"{mPlayerName}'s {AttributeData.Name}", AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.SELF_ESCORT_NPC, needShowName: true);
			}
			else
			{
				CurMission escortMission = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetEscortMission();
				if (escortMission != null && escortMission.GetParam(1) == ServerId)
				{
					mNpcHeadInfoLogic.SetNameLabel($"{mPlayerName}'s {AttributeData.Name}", AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.SELF_ESCORT_NPC, needShowName: true);
				}
				else
				{
					mNpcHeadInfoLogic.SetNameLabel($"{mPlayerName}'s {AttributeData.Name}", AttributeData.Camp, needShowHPLine: false, HEAD_PIC_TYPE.OTHER_ESCORT_NPC, needShowName: true);
				}
			}
		}
		else
		{
			mNpcHeadInfoLogic.SetNameLabel(AttributeData.Name, AttributeData.Camp, needShowHPLine: false);
		}
		mNpcHeadInfoLogic.ForceSetHpVal((float)AttributeData.HP / (float)AttributeData.MaxHP);
	}

	public override void ChangeHPEffect(long newHP, GameDefine.DAMAGEBOARD_TYPE type)
	{
		if (base.IsDie)
		{
			return;
		}
		long num = AttributeData.HP - newHP;
		if (num > 0)
		{
			UpdateDamgeBoard(type, num);
			OnBeaton();
		}
		else
		{
			UpdateDamgeBoard(type, num);
		}
		if (newHP < 0)
		{
			newHP = 0L;
		}
		AttributeData.HP = newHP;
		if (AttributeData.HP < AttributeData.MaxHP / 2 && AttributeData.Camp == GameDefine.CAMP_TYPE.STATIC_NPC && NPCFunctionType == GameDefine.NPC_FUNCTION_TYPE.GANGCITY_NPC && SmokeParticle != null && (!UnityVersionUtil.IsActive(SmokeParticle.gameObject) || !SmokeParticle.isPlaying))
		{
			UnityVersionUtil.SetActiveRecursive(SmokeParticle.gameObject, state: true);
			SmokeParticle.Play();
		}
		UpdateHeadInfo();
		if (mNPCType == GameDefine.NPC_TYPE.BOSS && SingletonUnity<BossXueTiaoLogicNew>.Exists)
		{
			SingletonUnity<BossXueTiaoLogicNew>.Instance.ChangeHP(newHP, this);
		}
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.CurrentMapInofData.MapType == MAPTYPE.CASH_DAILY_COPY && mNPCType == GameDefine.NPC_TYPE.BOSS)
		{
			int num2 = (int)((float)num / (float)AttributeData.MaxHP * (float)SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurSceneReward);
			if (num2 <= 0)
			{
				return;
			}
			ObjInitDropItemData objInitDropItemData = new ObjInitDropItemData();
			item item = new item();
			item.itemId = GameDefine.CASH_ITEM_ID;
			item.itemCount = num2;
			objInitDropItemData.item = item;
			objInitDropItemData.ownerServerId = PlayerData.MainPlayerServerId;
			objInitDropItemData.ServerID = UUID.GenUUID();
			float num3 = UnityEngine.Random.Range(0.5f, 3f);
			objInitDropItemData.Pos = base.Position + new Vector3(num3, 0f, num3);
			Singleton<ObjManager>.Instance.CreateDropItem(objInitDropItemData);
		}
		if (AttributeData.HP <= 0)
		{
			OnDie();
		}
		mReceiveBiggerHpTimeCount = Time.time;
	}

	public override void OnDie()
	{
		if (base.IsDie)
		{
			return;
		}
		base.OnDie();
		if (NPCType == GameDefine.NPC_TYPE.BOSS)
		{
			if (SingletonUnity<BossXueTiaoLogicNew>.Exists)
			{
				SingletonUnity<BossXueTiaoLogicNew>.Instance.RemoveBoss(this);
			}
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BossXueTiaoUI);
			SingletonUnity<UIManager>.Instance.CheckFunctionTop(isshowbossline: false);
			Singleton<ObjManager>.Instance.ClearSceneBomb();
		}
		if (IsCityCaptureNpc())
		{
			if (ExplosionParticle != null)
			{
				UnityVersionUtil.SetActiveRecursive(ExplosionParticle.gameObject, state: true);
				ExplosionParticle.Play();
			}
			if (MeshRoot != null)
			{
				MeshRoot.animation.Play("GTACheBaoZa_Animation");
			}
		}
		if (!GameManager.OnLineState || SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsSingleCopyScene())
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.OnNPCDie(this);
		}
	}

	public override void Recyle()
	{
		base.Recyle();
		if (NPCType == GameDefine.NPC_TYPE.BOSS)
		{
			if (SingletonUnity<BossXueTiaoLogicNew>.Exists)
			{
				SingletonUnity<BossXueTiaoLogicNew>.Instance.RemoveBoss(this);
			}
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BossXueTiaoUI);
			SingletonUnity<UIManager>.Instance.CheckFunctionTop(isshowbossline: false);
		}
	}

	private void CalDropItem()
	{
	}

	public void DelayRecycle()
	{
		Singleton<ObjManager>.Instance.RecycleNpc(this);
	}

	public override void OnSkillEnable(string skillID)
	{
		if (mAILogic == null || !mAILogic.enabled)
		{
			return;
		}
		SkillData skillDataById = DataManager.GetSkillDataById(skillID);
		if (mEnableSkillIDList.Contains(skillDataById))
		{
			return;
		}
		if (mEnableSkillIDList.Count > 0)
		{
			for (int i = 0; i < mEnableSkillIDList.Count; i++)
			{
				if (skillDataById.PriorityAutoCombat > mEnableSkillIDList[i].PriorityAutoCombat)
				{
					mEnableSkillIDList.Insert(i, skillDataById);
					return;
				}
			}
		}
		mEnableSkillIDList.Add(skillDataById);
	}

	public override void OnSkillDisable(string skillID)
	{
		mEnableSkillIDList.Remove(DataManager.GetSkillDataById(skillID));
	}

	public override void OnSkillFinished()
	{
		base.OnSkillFinished();
	}

	public void PrintSkill()
	{
		MonoBehaviour.print(base.gameObject.name + "====================");
		for (int i = 0; i < mEnableSkillIDList.Count; i++)
		{
			MonoBehaviour.print(mEnableSkillIDList[i]);
		}
	}

	public void DisableAllComponent()
	{
		base.enabled = false;
		mAnimationLogic.enabled = false;
		mNavMeshAgent.enabled = false;
		mEffectLogic.enabled = false;
		mSkillMotion.enabled = false;
		mEffectMotion.enabled = false;
		mBuffLogic.enabled = false;
		mAutoMoveLogic.enabled = false;
		if (mAILogic != null)
		{
			mAILogic.enabled = false;
		}
	}

	public void EnableAllComponent()
	{
		base.enabled = true;
		mAnimationLogic.enabled = true;
		mNavMeshAgent.enabled = true;
		mEffectLogic.enabled = true;
		mSkillMotion.enabled = true;
		mEffectMotion.enabled = true;
		mBuffLogic.enabled = true;
		mAutoMoveLogic.enabled = true;
		if (mAILogic != null)
		{
			mAILogic.enabled = true;
		}
	}

	public void updateBossHPState()
	{
		if (!base.IsDie && mNPCType == GameDefine.NPC_TYPE.BOSS && SingletonUnity<BossXueTiaoLogicNew>.Exists)
		{
			SingletonUnity<BossXueTiaoLogicNew>.Instance.UpdateBossHp(this, AttributeData);
		}
	}

	protected override void ChangeIdleState()
	{
		if (mNPCType == GameDefine.NPC_TYPE.BOSS)
		{
			mAnimationLogic.PlayAnimation(4);
		}
		else
		{
			mAnimationLogic.PlayAnimation(0);
		}
	}
}
