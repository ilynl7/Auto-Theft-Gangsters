using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ObjMainPlayer : ObjOtherPlayer
{
	private ThirdPersonController mThirdPersonController;

	private CameraController mCameraController;

	private PlayerData mCachePlayerData;

	private Material mXRayMat;

	private bool mIsTalking;

	private bool mIsNeedAutoMountCar;

	private float mStartAutoMoveTime;

	private float AutoMountTime = 3f;

	private bool mCompleteMissionFlag;

	private GameManager mGameManager;

	private float ftime;

	private Vector3 mLastPosition = Vector3.zero;

	private float timeWait = 0.2f;

	private move.request request = new move.request();

	private position pos = new position();

	private int relifeCount;

	public List<string> mWaitForSkillRetList = new List<string>();

	private List<ObjCharacter> mCurSearchTargetList;

	private float switchCD = 30f;

	private float lastChangeTime = float.MinValue;

	private List<string> mPlayerSkillIDList;

	private bool mIsNeedUpdateCheck;

	private SceneManager mCurSceneManager;

	private List<string> skillList = new List<string>();

	protected bool mAutoInviteTeamAccept;

	protected bool mAutoJoinTeamAccept;

	private List<GameItem> dragList;

	private float lastUseTime;

	private AutoComboInfoLogic mAutoComboInfoLogic;

	private ObjCharacter currentCharacter;

	public List<long> ApplyGuildIDList = new List<long>();

	private float CurTime = -30f;

	private float RuquestTime = 30f;

	public long LeaveGuildTime = -1L;

	private long mFollowServerID = -1L;

	private ObjCharacter mFollowCharacter;

	private float Speed = -1f;

	private bool isWalk;

	public override long ServerId
	{
		get
		{
			return PlayerData.MainPlayerServerId;
		}
		set
		{
			PlayerData.MainPlayerServerId = value;
		}
	}

	public override long GuildId => GameManager.PlayerData.PlayerGuild.ServerId;

	public override long TeamId => GameManager.PlayerData.TeamInfo.TeamID;

	public ThirdPersonController ThirdPersonController => mThirdPersonController;

	public CameraController CameraController => mCameraController;

	public override CharacterAttributeData AttributeData
	{
		get
		{
			if (mAttributeData == null)
			{
				mAttributeData = GameManager.PlayerData.MainPlayerAttrData;
			}
			return mAttributeData;
		}
		set
		{
			mAttributeData = value;
		}
	}

	public override PROFESSION_TYPE Profession
	{
		get
		{
			return GameManager.PlayerData.Profession;
		}
		set
		{
			GameManager.PlayerData.Profession = value;
		}
	}

	public override List<CharacterSkillData> CharacterSkillData
	{
		get
		{
			if (mCharacterSkillData == null)
			{
				mCharacterSkillData = GameManager.PlayerData.MainPlayerSkillDataList;
			}
			return mCharacterSkillData;
		}
		set
		{
			mCharacterSkillData = value;
		}
	}

	public int SkillIndex
	{
		get
		{
			return GameManager.PlayerData.SkillIndex;
		}
		set
		{
			GameManager.PlayerData.SkillIndex = value;
		}
	}

	private PlayerData mPlayerData
	{
		get
		{
			if (mCachePlayerData == null)
			{
				mCachePlayerData = GameManager.PlayerData;
			}
			return mCachePlayerData;
		}
	}

	public override string MountId
	{
		get
		{
			return mPlayerData.MountId;
		}
		set
		{
			mPlayerData.MountId = value;
		}
	}

	public override string MountColor
	{
		get
		{
			return mPlayerData.MountColor;
		}
		set
		{
			mPlayerData.MountColor = value;
		}
	}

	public override bool IsServerRidingMount
	{
		get
		{
			return mPlayerData.IsServerRidingMount;
		}
		set
		{
			mPlayerData.IsServerRidingMount = value;
		}
	}

	public Material XRayMat
	{
		get
		{
			if (mXRayMat == null)
			{
				mXRayMat = ResourcesManager.Load("Material/XRay") as Material;
			}
			return mXRayMat;
		}
		set
		{
			mXRayMat = value;
		}
	}

	public bool IsTalking
	{
		get
		{
			return mIsTalking;
		}
		set
		{
			mIsTalking = value;
			if (value)
			{
				AutoComabat = false;
			}
		}
	}

	public bool IsAutoMovingFlag => GameManager.AutoSearchPath.IsAutoMovingFlag;

	public bool IsNeedAutoMountCar
	{
		get
		{
			return mIsNeedAutoMountCar;
		}
		set
		{
			mIsNeedAutoMountCar = value;
		}
	}

	public bool CompleteMissionFlag
	{
		get
		{
			return mCompleteMissionFlag;
		}
		set
		{
			mCompleteMissionFlag = value;
		}
	}

	private GameManager GameManager
	{
		get
		{
			if (mGameManager == null)
			{
				mGameManager = SingletonDontDestoryUnity<GameManager>.Instance;
			}
			return mGameManager;
		}
	}

	public List<string> PlayerSkillIDList
	{
		get
		{
			if (mPlayerSkillIDList == null)
			{
				mPlayerSkillIDList = GameManager.PlayerData.MainPlayerSkillIDList;
			}
			return mPlayerSkillIDList;
		}
		set
		{
			mPlayerSkillIDList = value;
		}
	}

	public bool IsOpenAutoCombat
	{
		get
		{
			return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsOpenAutoCombat;
		}
		set
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsOpenAutoCombat = value;
		}
	}

	public float BreakAutoCombatTime
	{
		get
		{
			return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BreakAutoCombatTime;
		}
		set
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BreakAutoCombatTime = value;
		}
	}

	public bool AutoComabat
	{
		get
		{
			return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.AutoComabat && !SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.LoadingFlag;
		}
		set
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.AutoComabat = value;
		}
	}

	public bool AutoUseDrag
	{
		get
		{
			return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.AutoUseDrag;
		}
		set
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.AutoUseDrag = value;
		}
	}

	public float AutoUseDragThreshold
	{
		get
		{
			return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.AutoUseDragThreshold;
		}
		set
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.AutoUseDragThreshold = value;
		}
	}

	public bool AutoUseSort
	{
		get
		{
			return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.AutoUseSort;
		}
		set
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.AutoUseSort = value;
		}
	}

	public bool AutoInviteTeamAccept
	{
		get
		{
			return mAutoInviteTeamAccept;
		}
		set
		{
			mAutoInviteTeamAccept = value;
		}
	}

	public bool AutoJoinTeamAccept
	{
		get
		{
			return mAutoJoinTeamAccept;
		}
		set
		{
			mAutoJoinTeamAccept = value;
		}
	}

	public long FollowServerID
	{
		get
		{
			return mFollowServerID;
		}
		set
		{
			Speed = -1f;
			mFollowServerID = value;
			mFollowCharacter = Singleton<ObjManager>.Instance.FindObjInScene(value);
			UpdateFollowSpeed();
		}
	}

	public ObjMainPlayer()
	{
		mObjType = GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER;
	}

	public float GetStopDistance()
	{
		if (base.CurPlayerState == PLAYER_STATE.DRIVING)
		{
			return 3f;
		}
		return 1.5f;
	}

	public void EnterAutoMoving(float time)
	{
		mStartAutoMoveTime = time;
	}

	public override void SetTargetPartObjId(MODEL_TYPE type, string id)
	{
		TargetPartObjId[(int)type] = id;
		mPlayerData.UpdateMainPlayerPartBundleIdList(TargetPartObjId);
	}

	private void Awake()
	{
	}

	public override void Init()
	{
		base.Init();
		InitMainPlayer();
		BonesTrsDict = base.gameObject.GetComponentInChildren<TransDicts>().TransDict;
	}

	private new void InitHeadInfo()
	{
		ResourcesManager.LoadHeadInfoPrefab(UIInfo.PlayerHeadInfoUI, "PlayerHeadInfoRoot", LoadPlayerHeadInfo);
	}

	private void LoadPlayerHeadInfo(GameObject newObj)
	{
		if (newObj != null)
		{
			BillBoard billBoard = newObj.GetComponent<BillBoard>();
			if (billBoard == null)
			{
				billBoard = newObj.AddComponent<BillBoard>();
			}
			billBoard.BindObj = base.gameObject;
			billBoard.DeltaHeight = base.CurrentCharacterModelData.ModelHeight + PLAYER_NAME_DELTA_HIGHT;
		}
		PlayerHeadInfoLogic playerHeadInfoLogic = (PlayerHeadInfoLogic)(mHeadInfoLogic = newObj.GetComponent<PlayerHeadInfoLogic>());
		if (mGameManager.SceneManager.IsTutorialScene())
		{
			playerHeadInfoLogic.Reset(isMainPlayer: true, AttributeData.CurTitleLevel, AttributeData.Name, AttributeData.GuildName, GameDefine.CAMP_TYPE.PLAYER_1, ShowHpLine: false, AttributeData.IsChampionGuild());
		}
		else if (mGameManager.SceneManager.IsSurviveBattleScene())
		{
			playerHeadInfoLogic.Reset(isMainPlayer: true, AttributeData.CurTitleLevel, AttributeData.Name, AttributeData.GuildName, AttributeData.Camp, ShowHpLine: false, AttributeData.IsChampionGuild());
		}
		else
		{
			playerHeadInfoLogic.Reset(isMainPlayer: true, AttributeData.CurTitleLevel, AttributeData.Name, AttributeData.GuildName, GameDefine.CAMP_TYPE.PLAYER_1, ShowHpLine: false, AttributeData.IsChampionGuild());
		}
	}

	public void HideHeadInfo()
	{
		if (mHeadInfoLogic != null)
		{
			UnityVersionUtil.SetActiveRecursive(mHeadInfoLogic.gameObject, state: false);
		}
	}

	public void ShowHeadInfo()
	{
		if (mHeadInfoLogic != null)
		{
			UnityVersionUtil.SetActiveRecursive(mHeadInfoLogic.gameObject, state: true);
			if (mGameManager.SceneManager.IsSurviveBattleScene())
			{
				(mHeadInfoLogic as PlayerHeadInfoLogic).Reset(isMainPlayer: true, AttributeData.CurTitleLevel, AttributeData.Name, AttributeData.GuildName, AttributeData.Camp, ShowHpLine: false, AttributeData.IsChampionGuild());
			}
			else
			{
				(mHeadInfoLogic as PlayerHeadInfoLogic).Reset(isMainPlayer: true, AttributeData.CurTitleLevel, AttributeData.Name, AttributeData.GuildName, GameDefine.CAMP_TYPE.PLAYER_1, ShowHpLine: false, AttributeData.IsChampionGuild());
			}
		}
	}

	public void reName(string newname)
	{
		if (mHeadInfoLogic != null)
		{
			(mHeadInfoLogic as PlayerHeadInfoLogic).UpdateName(newname);
		}
	}

	public void InitMainPlayer()
	{
		if (mThirdPersonController == null)
		{
			mThirdPersonController = base.gameObject.AddComponent<ThirdPersonController>();
		}
		if (mCameraController == null)
		{
			mCameraController = Camera.main.transform.parent.gameObject.AddComponent<CameraController>();
		}
		if (mGameManager == null)
		{
			mGameManager = SingletonDontDestoryUnity<GameManager>.Instance;
		}
	}

	public void UpdateMainPlayerVisual(characterVisual visual)
	{
		mPlayerData.CharacterModelId = visual.ModeId;
		mPlayerData.CharacterModelData = DataManager.GetCharacterModelDataByID(visual.ModeId);
		mPlayerData.PartHeadId = visual.HeadId;
		mPlayerData.PartBodyId = visual.BodyId;
		mPlayerData.PartLegId = visual.LegId;
		mPlayerData.PartWeaponId = visual.WeaponId;
		if (visual.HasWeaponItemId)
		{
			mPlayerData.WeaponItemId = visual.WeaponItemId;
		}
		if (visual.HasFashionItemId)
		{
			mPlayerData.FashionItemId = visual.FashionItemId;
		}
		mPlayerData.FashionWeaponId = ((!visual.HasFashion_WeaponId) ? visual.WeaponId : visual.Fashion_WeaponId);
		mPlayerData.FashionHeadId = ((!visual.HasFashion_HeadId) ? visual.HeadId : visual.Fashion_HeadId);
		mPlayerData.FashionBodyId = ((!visual.HasFashion_BodyId) ? visual.BodyId : visual.Fashion_BodyId);
		mPlayerData.FashionLegId = ((!visual.HasFashion_LegId) ? visual.LegId : visual.Fashion_LegId);
		mPlayerData.IsShowFashion = visual.showType == 1;
		if (mPlayerData.IsHaveTeam())
		{
			TeamMember selfMember = mPlayerData.TeamInfo.SelfMember;
			selfMember.Visual = visual;
		}
	}

	public void ResetMainPlayer(ObjInitPlayerData playerInitData)
	{
		base.Reset();
		base.Position = playerInitData.mPos;
		base.CacheTransform.forward = playerInitData.mDir;
		PlayerData.MainPlayerServerId = playerInitData.mServerID;
		mPlayerData.MainPlayerStartPos = playerInitData.mPos;
		mPlayerData.MainPlayerStartDir = playerInitData.mDir;
		mPlayerData.CharacterModelId = playerInitData.mCharacterModelId;
		mPlayerData.CharacterModelData = DataManager.GetCharacterModelDataByID(mPlayerData.CharacterModelId);
		mPlayerData.PartHeadId = playerInitData.visual.HeadId;
		mPlayerData.PartBodyId = playerInitData.visual.BodyId;
		mPlayerData.PartLegId = playerInitData.visual.LegId;
		mPlayerData.PartWeaponId = playerInitData.visual.WeaponId;
		if (playerInitData.visual.HasWeaponItemId)
		{
			mPlayerData.WeaponItemId = playerInitData.visual.WeaponItemId;
		}
		if (playerInitData.visual.HasFashionItemId)
		{
			mPlayerData.FashionItemId = playerInitData.visual.FashionItemId;
		}
		mPlayerData.FashionHeadId = ((!playerInitData.visual.HasFashion_HeadId) ? playerInitData.visual.HeadId : playerInitData.visual.Fashion_HeadId);
		mPlayerData.FashionBodyId = ((!playerInitData.visual.HasFashion_BodyId) ? playerInitData.visual.BodyId : playerInitData.visual.Fashion_BodyId);
		mPlayerData.FashionLegId = ((!playerInitData.visual.HasFashion_LegId) ? playerInitData.visual.LegId : playerInitData.visual.Fashion_LegId);
		mPlayerData.FashionWeaponId = ((!playerInitData.visual.HasFashion_WeaponId) ? playerInitData.visual.WeaponId : playerInitData.visual.Fashion_WeaponId);
		mPlayerData.IsShowFashion = playerInitData.visual.showType == 1;
		mPlayerData.MountColor = playerInitData.visual.mount_color;
		mPlayerData.MountId = playerInitData.visual.MountId;
		mPlayerData.IsServerRidingMount = playerInitData.visual.mount_state == 1;
		AttributeData.InitData(playerInitData.Attribute, playerInitData.AttributeAll);
		Profession = playerInitData.Profession;
		AttributeData.HP = playerInitData.HP;
		AttributeData.Name = playerInitData.Name;
		AttributeData.GuildName = playerInitData.GuildName;
		AttributeData.CurEXP = playerInitData.EXP;
		AttributeData.Level = playerInitData.Level;
		AttributeData.CurTitleExp = playerInitData.TitleExp;
		AttributeData.CurTitleLevel = playerInitData.TitleLevel;
		AttributeData.ComboValue = playerInitData.ComboValue;
		AttributeData.CurSpeed = playerInitData.Speed;
		AttributeData.WalkSpeed = playerInitData.WalkSpeed;
		AttributeData.CurRec = playerInitData.Rec;
		AttributeData.RefineLevel = playerInitData.RefineLevel;
		AttributeData.RefineNeckLevel = playerInitData.RefineNeckLevel;
		AttributeData.RefineRing1Level = playerInitData.RefineRing1Level;
		AttributeData.RefineRing2Level = playerInitData.RefineRing2Level;
		AttributeData.RefineBeltLevel = playerInitData.RefineBeltLevel;
		for (int i = 0; i < playerInitData.EnhanceLevelList.Length; i++)
		{
			AttributeData.EquipEnhanceList[i] = playerInitData.EnhanceLevelList[i];
		}
		AttributeData.Camp = playerInitData.Camp;
		AttributeData.PkMode = playerInitData.PkMode;
		AttributeData.DanceState = playerInitData.DanceState;
		AttributeData.DanceId = playerInitData.DanceId;
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SkillIndex = playerInitData.SkillIndex;
		UpdateSkillList(playerInitData.skills);
		ResetCombo();
		InitHeadInfo();
		UpdatePlayerHp(AttributeData.HP);
		InitNavMeshAgent();
		mCompleteMissionFlag = false;
		if (IsServerRidingMount && GameManager.SceneManager.IsBigWorld())
		{
			MountCar(MountId, MountColor);
		}
	}

	public void ResetMainPlayer(movement posInfo)
	{
		base.Reset();
		base.Position = new Vector3((float)posInfo.pos.x / 100f, SceneManager.GetHitHeight((float)posInfo.pos.x / 100f, (float)posInfo.pos.z / 100f), (float)posInfo.pos.z / 100f);
		mPlayerData.MainPlayerStartDir = MathUtil.HeadingToVector3((float)posInfo.pos.o / 100f);
		base.CacheTransform.forward = mPlayerData.MainPlayerStartDir;
		mPlayerData.MainPlayerStartPos = base.Position;
		ResetCombo();
		InitHeadInfo();
		UpdatePlayerHp(AttributeData.HP);
		InitNavMeshAgent();
		mCompleteMissionFlag = false;
		if (IsServerRidingMount && GameManager.SceneManager.IsBigWorld())
		{
			MountCar(MountId, MountColor);
		}
	}

	public void SyncPlayerData(PlayerData data)
	{
	}

	private void SynPlayerPosition()
	{
		if (!mGameManager.SceneManager.IsDontSynPostion() && !base.IsDie && !(mNavMeshAgent == null) && mNavMeshAgent.enabled && Time.time > ftime && Vector3.SqrMagnitude(mLastPosition - base.Position) > 0.010000001f)
		{
			mLastPosition = base.CacheTransform.position;
			ftime = Time.time + timeWait;
			request.clear();
			pos.clear();
			pos.x = Mathf.CeilToInt(base.CacheTransform.position.x * 100f);
			pos.y = Mathf.CeilToInt(base.CacheTransform.position.y * 100f);
			pos.z = Mathf.CeilToInt(base.CacheTransform.position.z * 100f);
			pos.o = Mathf.CeilToInt(MathUtil.Heading(base.CacheTransform.forward) * 100f);
			request.pos = pos;
			request.moving = base.IsMoving;
			request.index = 1L;
			request.parm = (long)(AttributeData.CurSpeed * 100f);
			NetLogic.GetInstance().Send<Protocol.move>(request);
		}
	}

	public override void ChangeHPVal(long newHP)
	{
		if (!base.IsDie)
		{
			AttributeData.HP = newHP;
			UpdatePlayerHp(newHP);
			if (AttributeData.HP <= 0)
			{
				OnDie();
			}
		}
		else if (newHP > 0)
		{
			relifeCount++;
			if (relifeCount > 5)
			{
				relifeCount = 0;
				OnRelife(newHP, base.Position);
			}
		}
	}

	public override void ChangeHPEffect(long newHP, GameDefine.DAMAGEBOARD_TYPE type)
	{
		if (!base.IsDie)
		{
			long cHP = AttributeData.HP - newHP;
			UpdateDamgeBoard(type, cHP);
			if (newHP < 0)
			{
				newHP = 0L;
			}
			mReceiveBiggerHpTimeCount = Time.time;
		}
	}

	public override void ChangeLevel(int level, int combovalue = 0)
	{
		if (level > AttributeData.Level)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.LevelUpUIRoot, delegate
			{
				SingletonUnity<LevelUpUIRoot>.Instance.Reset(AttributeData.Level, level, AttributeData.ComboValue, combovalue);
			});
			if (SingletonUnity<TouXiangKuangLogic>.Exists)
			{
				SingletonUnity<TouXiangKuangLogic>.Instance.ChangeLevel(level);
			}
			GameManager.FlurryLogEventMap("LevelEvent", "LevelUp", level.ToString());
		}
	}

	private void UpdatePlayerHp(long newHp)
	{
		if (!base.IsLocalDrivingCar && SingletonUnity<TouXiangKuangLogic>.Exists)
		{
			SingletonUnity<TouXiangKuangLogic>.Instance.ChangeHP(newHp, AttributeData.MaxHP);
		}
	}

	public void UpdateComboTime()
	{
		if (mComboTimeCount > 0f)
		{
			mComboTimeCount -= Time.deltaTime;
			if (mComboTimeCount <= 0f)
			{
				ResetCombo();
			}
		}
	}

	public void UpdateStep()
	{
		if (base.IsMoving && mCurPlayerState != PLAYER_STATE.DRIVING)
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(0);
		}
	}

	private void UpdateAutoMountCar()
	{
	}

	private void Update()
	{
		UpdateComponent();
		UpdateMove();
		UpdateSkillCD();
		UpdateHoldTime();
		UpdateComboTime();
		UpdateAutoCombatBreakState();
		UpdateAuto();
		UpdateTeamFollow();
		base.SkillLogic.UpdateSkill();
		SynPlayerPosition();
		UpdateStep();
		UpdateAutoMountCar();
		UpdateMoveCheck();
	}

	public void AddSkillWaitRet(string skillId)
	{
	}

	public void RemoveSkillWaitRet(string skillId)
	{
		if (mWaitForSkillRetList.Contains(skillId))
		{
			mWaitForSkillRetList.Remove(skillId);
		}
	}

	public bool CheckInSkillWaitRet(string skillId)
	{
		if (mWaitForSkillRetList.Contains(skillId) || mWaitForSkillRetList.Count > 3)
		{
			return true;
		}
		return false;
	}

	public void ClearSkillWaitRet()
	{
		mWaitForSkillRetList.Clear();
	}

	public bool IsCanAttackTarget(ObjCharacter obj)
	{
		if (obj.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
		{
			return CampTool.ISPlayerCanAttack(this, obj as ObjOtherPlayer);
		}
		return true;
	}

	public void UseComboSkill()
	{
		if (mCurPlayerState == PLAYER_STATE.DANCE || CharacterSkillData.Count <= 0)
		{
			return;
		}
		if (!CheckHoldTime())
		{
			CheckComboDelay();
		}
		else
		{
			if (BeforeSkillCheck())
			{
				return;
			}
			ObjCharacter objCharacter = null;
			if (mSelectedTarget != null && mSelectedTarget.ServerId != ServerId && IsCanAttackTarget(mSelectedTarget))
			{
				objCharacter = mSelectedTarget;
			}
			if (objCharacter == null || objCharacter.IsDie)
			{
				objCharacter = ChooseTarget(mComboIndex);
				SelectTarget(objCharacter);
			}
			if (objCharacter == null)
			{
				return;
			}
			base.CurUseSkillId = mComboIndex;
			if (mComboIndex == CharacterSkillData[0].ID)
			{
				EnterCombat(objCharacter, OnComboSuccess);
			}
			else
			{
				if (base.SkillLogic.IsUsingSkill && !base.SkillLogic.CheckSkillCanBeBreak(base.CurUseSkillId))
				{
					return;
				}
				SkillData skillDataById = DataManager.GetSkillDataById(base.CurUseSkillId);
				if (!CheckSkillDistance(skillDataById, objCharacter))
				{
					MoveTo(objCharacter.Position, skillDataById.TraceDistanceMeter + objCharacter.ModelRadius + ModelRadius - 0.5f, TargetArriveUseSkill, isNeedUpdateCheck: true);
					return;
				}
				if (IsDrivingMount())
				{
					SendServerDisMountCar();
				}
				if (!mGameManager.SceneManager.IsSingleCopyScene())
				{
					List<attack_list> list = new List<attack_list>();
					attack_list attack_list = new attack_list();
					if (!objCharacter.InvincibleFlag)
					{
						attack_list.id = objCharacter.ServerId;
						list.Add(attack_list);
					}
					skill_use.request request = new skill_use.request();
					request.skillId = base.CurUseSkillId;
					request.targetId = objCharacter.ServerId;
					request.combo = true;
					request.attack_list = list;
					request.parm = (int)(Time.realtimeSinceStartup * 100f);
					if (base.SkillLogic.ServerUseSkill(base.CurUseSkillId, ServerId, objCharacter.ServerId, list))
					{
						NetLogic.GetInstance().Send<Protocol.skill_use>(request);
						OnComboSuccess(base.CurUseSkillId);
					}
				}
				else if (base.SkillLogic.UseSkill(base.CurUseSkillId, ServerId, objCharacter.ServerId))
				{
					OnComboSuccess(base.CurUseSkillId);
				}
				mGameManager.MissionManager.StopAutoMoveToMission();
				mIsNeedAutoMountCar = false;
			}
		}
	}

	public void UseSkill(string skillId, SkillUseSuccess onSkillUseSuccess = null)
	{
		if (mCurPlayerState != PLAYER_STATE.DANCE && !BeforeSkillCheck())
		{
			ObjCharacter objCharacter = null;
			if (mSelectedTarget != null && mSelectedTarget.ServerId != ServerId && IsCanAttackTarget(mSelectedTarget))
			{
				objCharacter = mSelectedTarget;
			}
			if (objCharacter == null || objCharacter.IsDie)
			{
				objCharacter = ChooseTarget(skillId);
				SelectTarget(objCharacter);
			}
			base.CurUseSkillId = skillId;
			EnterCombat(objCharacter, onSkillUseSuccess);
		}
	}

	public override void EnterCombat(ObjCharacter target, SkillUseSuccess onSkillUseSuccess = null)
	{
		if (base.SkillLogic.IsUsingSkill && !base.SkillLogic.CheckSkillCanBeBreak(base.CurUseSkillId))
		{
			return;
		}
		SkillData skillDataById = DataManager.GetSkillDataById(base.CurUseSkillId);
		if (skillDataById == null)
		{
			return;
		}
		if (target != null)
		{
			if (!CheckSkillDistance(skillDataById, target))
			{
				MoveTo(target.Position, skillDataById.TraceDistanceMeter + target.ModelRadius + ModelRadius - 0.5f, TargetArriveUseSkill, isNeedUpdateCheck: true);
				return;
			}
		}
		else
		{
			EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(skillDataById.EffId_0);
			if (effInfoDataById != null && effInfoDataById.Target == 0 && (effInfoDataById.AreaType == 0 || effInfoDataById.AreaType == 1))
			{
				return;
			}
		}
		if (!CheckSkillCD(skillDataById))
		{
			return;
		}
		if (onSkillUseSuccess != null && !mSkillUseSuccessDic.ContainsKey(base.CurUseSkillId))
		{
			mSkillUseSuccessDic.Add(base.CurUseSkillId, onSkillUseSuccess);
		}
		if (IsDrivingMount())
		{
			SendServerDisMountCar();
		}
		if (!mGameManager.SceneManager.IsSingleCopyScene())
		{
			skill_use.request request = new skill_use.request();
			request.skillId = base.CurUseSkillId;
			long targetId = -1L;
			if (target != null)
			{
				targetId = target.ServerId;
			}
			request.targetId = targetId;
			request.combo = true;
			List<attack_list> list = new List<attack_list>();
			if (!string.IsNullOrEmpty(skillDataById.EffId_0))
			{
				EffInfoData effInfoDataById2 = DataManager.GetEffInfoDataById(skillDataById.EffId_0);
				if (target != null && effInfoDataById2 != null && mSkillLogic.IsNeedFaceTarget(effInfoDataById2))
				{
					FaceToPub(target.Position);
				}
				BuffInfoData buffInfoData = null;
				if (!string.IsNullOrEmpty(effInfoDataById2.BuffID))
				{
					buffInfoData = DataManager.GetBuffInfoDataByID(effInfoDataById2.BuffID);
				}
				int bUF_USE_FAIL = GameDefine.BUF_USE_FAIL;
				List<ObjCharacter> effectTargetList = mSkillLogic.GetEffectTargetList(this, skillDataById, effInfoDataById2, target, mSkillLogic.GetCandidateList(AttributeData.Camp));
				for (int i = 0; i < effectTargetList.Count; i++)
				{
					attack_list attack_list = new attack_list();
					attack_list.id = effectTargetList[i].ServerId;
					if (buffInfoData != null)
					{
						bUF_USE_FAIL = CharacterAttributeData.BuffUseState(this, effectTargetList[i].AttributeData, skillDataById.ID, effInfoDataById2, UnityEngine.Random.Range(0, 100), buffInfoData.BufType);
						attack_list.value = bUF_USE_FAIL;
					}
					else
					{
						attack_list.value = GameDefine.BUF_USE_FAIL;
					}
					list.Add(attack_list);
				}
				request.attack_list = list;
			}
			request.parm = (int)(Time.realtimeSinceStartup * 100f);
			if (mSkillLogic.ServerUseSkill(base.CurUseSkillId, ServerId, targetId, list))
			{
				NetLogic.GetInstance().Send<Protocol.skill_use>(request);
			}
		}
		else if (target != null)
		{
			base.SkillLogic.UseSkill(base.CurUseSkillId, ServerId, target.ServerId);
		}
		else
		{
			base.SkillLogic.UseSkill(base.CurUseSkillId, ServerId, -1L);
		}
		mGameManager.MissionManager.StopAutoMoveToMission();
		mIsNeedAutoMountCar = false;
	}

	public void OnComboSuccess(string skillId)
	{
		SkillData skillDataById = DataManager.GetSkillDataById(skillId);
		mComboIndex = skillDataById.NextSkill;
		mComboTimeCount = skillDataById.ComboValidTimeSecond;
		mComboTimeTotalCount = mComboTimeCount;
	}

	public override void OnSkillUseSuccess(string skillId)
	{
		base.OnSkillUseSuccess(skillId);
		RemoveSkillWaitRet(skillId);
	}

	public override void OnSkillUseFail(string skillId)
	{
		if (mSkillUseSuccessDic.ContainsKey(skillId))
		{
			mSkillUseSuccessDic.Remove(skillId);
		}
		RemoveSkillWaitRet(skillId);
	}

	public override void OnSkillFinished()
	{
		base.OnSkillFinished();
		if (mComboNextFlag)
		{
			UseComboSkill();
			mComboNextFlag = false;
		}
	}

	public void TargetArriveUseSkill(ObjCharacter objCha)
	{
		SkillData skillDataById = DataManager.GetSkillDataById(mCurUseSkillId);
		if (skillDataById != null)
		{
			if (!string.IsNullOrEmpty(skillDataById.NextSkill))
			{
				UseComboSkill();
			}
			else
			{
				UseSkill(mCurUseSkillId);
			}
		}
	}

	public ObjCharacter ChooseTarget(string skillId)
	{
		ObjCharacter result = null;
		float num = 9999f;
		bool flag = false;
		bool flag2 = false;
		mCurSearchTargetList = Singleton<ObjManager>.Instance.CampTargetList[(int)AttributeData.Camp];
		for (int i = 0; i < mCurSearchTargetList.Count; i++)
		{
			if (mCurSearchTargetList[i].ServerId == ServerId || mCurSearchTargetList[i].IsDie || (AutoComabat && mCurSearchTargetList[i].ObjType == GameDefine.OBJ_TYPE.OBJ_NPC && (mCurSearchTargetList[i] as ObjNPC).NPCFunctionType == GameDefine.NPC_FUNCTION_TYPE.CITIZEN_NPC) || (mCurSearchTargetList[i].ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER && !CampTool.ISPlayerCanAttack(this, mCurSearchTargetList[i] as ObjOtherPlayer)))
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
					num = pathDistance;
				}
				else if (num > pathDistance)
				{
					result = mCurSearchTargetList[i];
					num = pathDistance;
				}
			}
			else if (flag2)
			{
				float pathDistance = GetPathDistance(mCurSearchTargetList[i].Position);
				if (num > pathDistance)
				{
					result = mCurSearchTargetList[i];
					num = pathDistance;
				}
			}
		}
		return result;
	}

	public void CheckComboDelay()
	{
		if (base.SkillLogic.UsingSkillData != null && base.SkillLogic.UsingSkillData.NextSkill == mComboIndex)
		{
			mComboNextFlag = true;
		}
	}

	public bool CheckSkillEnable(SkillData skillData, ObjCharacter target)
	{
		if (!CheckSkillCD(skillData))
		{
			return false;
		}
		if (!CheckSkillDistance(skillData, target))
		{
			return false;
		}
		return true;
	}

	private void ResetCombo()
	{
		if (CharacterSkillData.Count != 0)
		{
			mComboIndex = CharacterSkillData[0].ID;
		}
		SkillData skillDataById = DataManager.GetSkillDataById(mComboIndex);
		mComboTimeCount = skillDataById.ComboValidTimeSecond;
	}

	public bool IsComboFirstSkill()
	{
		return mComboIndex == CharacterSkillData[0].ID;
	}

	public float GetComboSKillTimePercent()
	{
		CharacterSkillData characterSkillDataByID = GetCharacterSkillDataByID(mComboIndex);
		if (characterSkillDataByID != null)
		{
			if (characterSkillDataByID.CDTimeCount > 0f)
			{
				return Mathf.Clamp01(characterSkillDataByID.CDTimeCount / DataManager.GetSkillDataById(mComboIndex).CDSecond);
			}
			return 0f;
		}
		return 0f;
	}

	public float GetSkillTimePercent(string skillId)
	{
		CharacterSkillData characterSkillDataByID = GetCharacterSkillDataByID(skillId);
		if (characterSkillDataByID != null)
		{
			if (characterSkillDataByID.CDTimeCount > 0f)
			{
				return Mathf.Clamp01(characterSkillDataByID.CDTimeCount / DataManager.GetSkillDataById(skillId).CDSecond);
			}
			return 0f;
		}
		return 0f;
	}

	public float GetSwitchSkillCDPercent()
	{
		float num = switchCD - (Time.time - lastChangeTime);
		return Mathf.Clamp01(num / switchCD);
	}

	public bool SwitchSkillGroup()
	{
		if (Time.time - lastChangeTime > switchCD)
		{
			lastChangeTime = Time.time;
			if (SkillIndex != 0)
			{
				change_skill_index.request request = new change_skill_index.request();
				request.index = 0L;
				NetLogic.GetInstance().Send<Protocol.change_skill_index>(request);
				SkillIndex = 0;
			}
			else
			{
				change_skill_index.request request2 = new change_skill_index.request();
				request2.index = 1L;
				NetLogic.GetInstance().Send<Protocol.change_skill_index>(request2);
				SkillIndex = 1;
			}
			UpdateSkillIdList();
			return true;
		}
		return false;
	}

	public void RemoveSkillPosition(string skillId)
	{
		for (int i = 0; i < CharacterSkillData.Count; i++)
		{
			if (skillId.Equals(CharacterSkillData[i].ID))
			{
				CharacterSkillData[i].Index = 10;
				change_skill_position.request request = new change_skill_position.request();
				request.skillId = skillId;
				request.indexPos = 10L;
				NetLogic.GetInstance().Send<Protocol.change_skill_position>(request);
			}
		}
		UpdateSkillIdList();
	}

	public void ChangeSkillPosition(string skillId, int newindex)
	{
		string text = string.Empty;
		int num = 10;
		int num2 = 0;
		for (int i = 0; i < CharacterSkillData.Count; i++)
		{
			int index = CharacterSkillData[i].Index;
			if (index == newindex && CharacterSkillData[i].ID == skillId)
			{
				return;
			}
			if (CharacterSkillData[i].ID == skillId)
			{
				num = CharacterSkillData[i].Index;
				CharacterSkillData[i].Index = 10;
			}
			else if (index == newindex)
			{
				text = CharacterSkillData[i].ID;
				CharacterSkillData[i].Index = 10;
			}
		}
		for (int j = 0; j < CharacterSkillData.Count; j++)
		{
			if (skillId == CharacterSkillData[j].ID)
			{
				CharacterSkillData[j].Index = newindex;
				change_skill_position.request request = new change_skill_position.request();
				request.skillId = skillId;
				request.indexPos = newindex;
				NetLogic.GetInstance().Send<Protocol.change_skill_position>(request);
			}
			if (text == CharacterSkillData[j].ID)
			{
				CharacterSkillData[j].Index = num;
				change_skill_position.request request2 = new change_skill_position.request();
				request2.skillId = text;
				request2.indexPos = num;
				NetLogic.GetInstance().Send<Protocol.change_skill_position>(request2);
			}
		}
		UpdateSkillIdList();
	}

	public override void UpdateSkillList(Dictionary<string, skill_info> skills)
	{
		CharacterSkillData.Clear();
		if (skills == null)
		{
			return;
		}
		int count = skills.Count;
		foreach (KeyValuePair<string, skill_info> skill in skills)
		{
			CharacterSkillData.Add(new CharacterSkillData(skill.Value.skillId, (int)skill.Value.skillLevel, (int)skill.Value.indexPos, (int)skill.Value.indexPos2, skill.Value.disable));
		}
		CharacterSkillData.Sort((CharacterSkillData temp1, CharacterSkillData temp2) => temp1.Index2 - temp2.Index2);
		UpdateSkillIdList();
		ResetCombo();
	}

	public void UpdateSkillIdList()
	{
		PlayerSkillIDList.Clear();
		for (int i = 0; i < 4; i++)
		{
			PlayerSkillIDList.Add(string.Empty);
		}
		for (int j = 0; j < CharacterSkillData.Count; j++)
		{
			int index = CharacterSkillData[j].Index;
			if (index == 3)
			{
				PlayerSkillIDList[index - 3] = CharacterSkillData[j].ID;
			}
			if (SkillIndex == 0)
			{
				if (index > 3 && index < 7)
				{
					PlayerSkillIDList[index - 3] = CharacterSkillData[j].ID;
				}
			}
			else if (index > 6 && index < 10)
			{
				PlayerSkillIDList[index - 6] = CharacterSkillData[j].ID;
			}
		}
	}

	public int GetPlayerSkillLevelByPos(int skillpos)
	{
		for (int i = 0; i < CharacterSkillData.Count; i++)
		{
			if (CharacterSkillData[i].Index2 == skillpos)
			{
				return CharacterSkillData[i].Level;
			}
		}
		return 0;
	}

	public bool CheckSkillCanUpdate()
	{
		for (int i = 0; i < CharacterSkillData.Count; i++)
		{
			CharacterSkillData characterSkillData = CharacterSkillData[i];
			if (characterSkillData == null || !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(characterSkillData.UnlockLevel))
			{
				continue;
			}
			int index = characterSkillData.Index;
			if (index >= 4 && index <= 6)
			{
				SkillData skillDataById = DataManager.GetSkillDataById(characterSkillData.ID);
				if (skillDataById != null && skillDataById.IsUpgrade != 0 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level > characterSkillData.Level + 1)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void SelectTarget(ObjCharacter target)
	{
		if (target != null)
		{
			if (target.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
			{
				ObjNPC targetNPC = target as ObjNPC;
				if (targetNPC.IsMissionNpc())
				{
					if (targetNPC.CheckInDialogRange())
					{
						Singleton<DialogManager>.Instance.ShowDialog(targetNPC, string.Empty);
						return;
					}
					float stopRange = GetStopDistance();
					Vector3 vector = target.Position;
					if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsShopScene())
					{
						stopRange = 1f;
						vector = target.Position + target.transform.forward * 2.5f;
					}
					MoveTo(vector, stopRange, delegate
					{
						Singleton<DialogManager>.Instance.ShowDialog(targetNPC, string.Empty);
					});
					return;
				}
				if (!CampTool.CanAttack(AttributeData.Camp, targetNPC.AttributeData.Camp))
				{
					return;
				}
			}
			else if (target.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
			{
				ObjOtherPlayer target2 = target as ObjOtherPlayer;
				if (!CampTool.ISPlayerCanAttack(this, target2))
				{
					return;
				}
			}
			else if (target.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL)
			{
				ObjZombieRagdollPlayer objZombieRagdollPlayer = target as ObjZombieRagdollPlayer;
				if (objZombieRagdollPlayer.IsMissionNpc)
				{
					if (objZombieRagdollPlayer.CheckInDialogRange())
					{
						objZombieRagdollPlayer.ShowAcitvityDialog();
					}
					return;
				}
			}
			else if ((target.ObjType == GameDefine.OBJ_TYPE.OBJ_PLAYER_CAR || target.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC_CAR) && !CampTool.CanAttack(AttributeData.Camp, target.AttributeData.Camp))
			{
				return;
			}
		}
		if (base.CurPlayerCar != null && base.CurPlayerCar == target)
		{
			target = null;
		}
		base.SelectedTarget = target;
	}

	public void SelectTargetNPC(ObjNPC target)
	{
		if (!(target != null))
		{
			return;
		}
		ObjNPC targetNPC = target;
		if (!targetNPC.IsMissionNpc())
		{
			return;
		}
		if (targetNPC.CheckInDialogRange())
		{
			Singleton<DialogManager>.Instance.ShowDialog(targetNPC, string.Empty);
			return;
		}
		MoveTo(target.Position, GetStopDistance(), delegate
		{
			Singleton<DialogManager>.Instance.ShowDialog(targetNPC, string.Empty);
		});
	}

	public void UpdateSelectTarget()
	{
		if (base.SelectedTarget == null)
		{
			return;
		}
		if (base.SelectedTarget.IsDie)
		{
			SelectTarget(null);
			return;
		}
		if (base.IsDie)
		{
			SelectTarget(null);
			return;
		}
		float num = 144f;
		if (num < Vector3.SqrMagnitude(base.SelectedTarget.Position - base.Position))
		{
			SelectTarget(null);
		}
	}

	public override void OnDie()
	{
		if (IsDrivingMount())
		{
			SendServerDisMountCar();
		}
		base.OnDie();
		ClearSkillWaitRet();
		mSkillUseSuccessDic.Clear();
	}

	public override void OnRelife(long hp, Vector3 pos)
	{
		mCurPlayerState = PLAYER_STATE.NORMAL;
		base.OnRelife(AttributeData.MaxHP, pos);
		UpdatePlayerHp(AttributeData.HP);
		UseInvincibleSkill();
		LeveAutoCombat();
		base.IsLocalDrivingCar = false;
		base.CurPlayerCar = null;
	}

	public void SendNotify(bool isFilterRepeat, string msg, params object[] args)
	{
		NoticeLogic.AddNotifyData2Client(isFilterRepeat, msg, isWarning: false, args);
	}

	public void EquipItem(GameItem item, bool isinhert = false)
	{
		equip_item.request request = new equip_item.request();
		request.indexId = item.IndexId;
		request.inhert = isinhert;
		NetLogic.GetInstance().Send<Protocol.equip_item>(request);
	}

	public void UnEquipItem(GameItem item)
	{
		unequip_item.request request = new unequip_item.request();
		request.indexId = item.IndexId;
		NetLogic.GetInstance().Send<Protocol.unequip_item>(request);
	}

	public void EquipBadge(GameItem item, int pos)
	{
		equip_badge.request request = new equip_badge.request();
		request.indexId = item.IndexId;
		request.pos = pos;
		NetLogic.GetInstance().Send<Protocol.equip_badge>(request);
	}

	public void UnEquipBadge(GameItem item)
	{
		unequip_badge.request request = new unequip_badge.request();
		request.indexId = item.IndexId;
		NetLogic.GetInstance().Send<Protocol.unequip_badge>(request);
	}

	public bool UseDrag(GameItem item)
	{
		if (mGameManager.PlayerData.CanUseDrag() && mGameManager.SceneManager.IsCanUsePotion() && !base.IsDie)
		{
			use_item.request request = new use_item.request();
			request.indexId = item.IndexId;
			NetLogic.GetInstance().Send<Protocol.use_item>(request);
			mGameManager.PlayerData.UpdateDragCD();
			return true;
		}
		return false;
	}

	public void UseBuffDrag(GameItem item)
	{
		use_item.request request = new use_item.request();
		request.indexId = item.IndexId;
		NetLogic.GetInstance().Send<Protocol.use_item>(request);
		EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(item.ItemData.Function.ToString());
		AddBuffInfoData(effInfoDataById.BuffID, 0f, effInfoDataById.BuffDurationSecond, this);
		if (item.ItemId == GameDefine.AtkBuffItemId)
		{
			NoticeLogic.AddNotifyData("#{100650}");
		}
		else if (item.ItemId == GameDefine.DefBuffItemId)
		{
			NoticeLogic.AddNotifyData("#{100651}");
		}
		else if (item.ItemId == GameDefine.MovBuffItemId)
		{
			NoticeLogic.AddNotifyData("#{100652}");
		}
	}

	public void UseDanceItem(string itemid)
	{
		List<GameItem> itemByItemId = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack.GetItemByItemId(itemid);
		if (itemByItemId == null || itemByItemId.Count <= 0)
		{
			return;
		}
		use_item.request request = new use_item.request();
		request.indexId = itemByItemId[0].IndexId;
		List<Vector3> circlePoint = GetCirclePoint(base.transform.position, 0.5f);
		circlePoint.Add(base.transform.position);
		Vector3 vector = circlePoint[0];
		bool flag = false;
		for (int i = 0; i < circlePoint.Count; i++)
		{
			vector = circlePoint[i];
			if (SceneManager.IsInNavmeshArea(vector) && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsInGatherArea(vector))
			{
				break;
			}
		}
		if (SceneManager.IsInNavmeshArea(vector))
		{
			request.x = (long)vector.x * 100;
			request.z = (long)vector.z * 100;
			NetLogic.GetInstance().Send<Protocol.use_item>(request);
		}
	}

	public List<Vector3> GetCirclePoint(Vector3 orign, float radius)
	{
		List<Vector3> list = new List<Vector3>();
		for (float num = 0f; num < (float)Math.PI * 2f; num += (float)Math.PI / 5f)
		{
			list.Add(new Vector3(orign.x + radius * Mathf.Cos(num), 0f, orign.z + radius * Mathf.Sin(num)));
		}
		List<Vector3> list2 = new List<Vector3>();
		int count = list.Count;
		int num2 = 0;
		for (int i = 0; i < count; i++)
		{
			num2 = UnityEngine.Random.Range(0, list.Count);
			list2.Add(list[num2]);
			list.RemoveAt(num2);
		}
		return list2;
	}

	public void EquipFashionItem(GameItem item)
	{
		equip_fashion_item.request request = new equip_fashion_item.request();
		request.indexId = item.IndexId;
		NetLogic.GetInstance().Send<Protocol.equip_fashion_item>(request);
	}

	public void UnEquipFashionItem(GameItem item)
	{
		unequip_fashion_item.request request = new unequip_fashion_item.request();
		request.indexId = item.IndexId;
		NetLogic.GetInstance().Send<Protocol.unequip_fashion_item>(request);
	}

	public bool IsHaveWeapon()
	{
		return true;
	}

	public void DisableMainPlayer()
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
		DisactiveHeadInfo();
	}

	public void EnableMainPlayer()
	{
		base.enabled = true;
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		ActiveHeadInfo();
		(mHeadInfoLogic as PlayerHeadInfoLogic).Reset(isMainPlayer: true, AttributeData.CurTitleLevel, AttributeData.Name, AttributeData.GuildName, GameDefine.CAMP_TYPE.PLAYER_1, ShowHpLine: false, AttributeData.IsChampionGuild());
	}

	public void ChangeMountColor(string mountId, string newColorId)
	{
		if (GameManager.SceneManager.IsBigWorld() && IsDrivingMount() && MountId.Equals(mountId))
		{
			MountCar(mountId, newColorId);
		}
	}

	public void ChangeMountCar(string mountId, string mountColor)
	{
		if (GameManager.SceneManager.IsBigWorld() && IsDrivingMount() && !MountId.Equals(mountId))
		{
			MountCar(mountId, mountColor);
		}
	}

	public override void MountCar(string mountId, string mountColor)
	{
		base.MountCar(mountId, mountColor);
		RemoveXRayMat();
		if (SingletonUnity<RealTimeShadow>.Exists)
		{
			SingletonUnity<RealTimeShadow>.Instance.DisableRealTimeShadow();
		}
		if (mCameraController.InitFlag)
		{
			mCameraController.ChangeToCarView();
		}
		UpdateStopDistance(GetStopDistance());
	}

	public override void DisMountCar()
	{
		base.DisMountCar();
		AddXRayMat();
		if (SingletonUnity<RealTimeShadow>.Exists)
		{
			SingletonUnity<RealTimeShadow>.Instance.EnableRealTimeShadow();
		}
		mCameraController.BackToNormalView();
		UpdateStopDistance(GetStopDistance());
	}

	public void RemoveXRayMat()
	{
		Material[] array = null;
		SkinnedMeshRenderer skinnedMeshRenderer = null;
		for (int i = 0; i < base.PartObject.Length; i++)
		{
			if (!(base.PartObject[i] != null))
			{
				continue;
			}
			skinnedMeshRenderer = base.PartObject[i].GetComponent<SkinnedMeshRenderer>();
			if (skinnedMeshRenderer.materials.Length > 1)
			{
				array = new Material[skinnedMeshRenderer.materials.Length - 1];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = skinnedMeshRenderer.materials[j];
				}
				skinnedMeshRenderer.materials = array;
			}
		}
	}

	public void AddXRayMat()
	{
		Material[] array = null;
		SkinnedMeshRenderer skinnedMeshRenderer = null;
		for (int i = 0; i < base.PartObject.Length; i++)
		{
			if (!(base.PartObject[i] != null))
			{
				continue;
			}
			skinnedMeshRenderer = base.PartObject[i].GetComponent<SkinnedMeshRenderer>();
			array = new Material[skinnedMeshRenderer.materials.Length + 1];
			bool flag = false;
			for (int j = 0; j < skinnedMeshRenderer.materials.Length; j++)
			{
				if (skinnedMeshRenderer.materials[j].name.Contains("XRay"))
				{
					flag = true;
					break;
				}
				array[j] = skinnedMeshRenderer.materials[j];
			}
			if (!flag)
			{
				array[skinnedMeshRenderer.materials.Length] = XRayMat;
				skinnedMeshRenderer.materials = array;
			}
		}
	}

	public void SendServerMountCar()
	{
		if (!mSkillLogic.IsUsingSkill && !base.IsDie && !IsStun())
		{
			if (mCurPlayerState == PLAYER_STATE.DANCE)
			{
				StopDance();
			}
			NetLogic.GetInstance().Send<Protocol.use_mount>();
			MountCar(MountId, MountColor);
		}
	}

	public void SendServerDisMountCar()
	{
		NetLogic.GetInstance().Send<Protocol.unuse_mount>();
		DisMountCar();
		mStartAutoMoveTime = Time.time;
	}

	public override void StartDance(string danceId)
	{
		if (IsDrivingMount())
		{
			SendServerDisMountCar();
		}
		if (mCurPlayerState != PLAYER_STATE.DANCE)
		{
			base.StartDance(danceId);
			mPlayerData.PlayerDanceData.CurDanceId = danceId;
			mPlayerData.PlayerDanceData.CurDanceData = DataManager.GetDanceDataById(danceId);
		}
		if (SingletonUnity<DanceBtnRootLogic>.Exists)
		{
			SingletonUnity<DanceBtnRootLogic>.Instance.UpdateDanceBtn();
		}
	}

	public override bool PlayeSocialDance(string danceId)
	{
		if (mCurPlayerState == PLAYER_STATE.SOCIAL_DANCE)
		{
			return false;
		}
		if (mCurPlayerState == PLAYER_STATE.DANCE)
		{
			return false;
		}
		if (IsOpenAutoCombat)
		{
			return false;
		}
		if (base.SkillLogic.IsUsingSkill)
		{
			return false;
		}
		if (base.IsMoving)
		{
			DisactiveTargetArriveFinish();
			StopMove();
		}
		if (IsDrivingMount())
		{
			SendServerDisMountCar();
		}
		SocialDanceData socialDanceDataById = DataManager.GetSocialDanceDataById(danceId);
		if (socialDanceDataById == null)
		{
			return false;
		}
		if (socialDanceDataById.Level > AttributeData.Level)
		{
			return false;
		}
		mCurPlayerState = PLAYER_STATE.SOCIAL_DANCE;
		base.AnimationLogic.PlayAnimation(socialDanceDataById.ActionID, SocialDanceFinish, -1f);
		play_social_dance.request request = new play_social_dance.request();
		request.id = ServerId;
		request.danceId = danceId;
		NetLogic.GetInstance().Send<Protocol.play_social_dance>(request);
		return true;
	}

	public override void StopDance()
	{
		if (mCurPlayerState == PLAYER_STATE.DANCE)
		{
			base.StopDance();
			NetLogic.GetInstance().Send<Protocol.pause_participate_dance>();
			if (SingletonUnity<DanceBtnRootLogic>.Exists)
			{
				SingletonUnity<DanceBtnRootLogic>.Instance.UpdateDanceBtn();
			}
		}
	}

	public override void RemoveDance()
	{
		base.RemoveDance();
		mPlayerData.PlayerDanceData.Reset();
	}

	public override void MoveTo(Vector3 pos, float stopRange = 1f, TargetArriveFinsh arriveFinsh = null)
	{
		mIsNeedUpdateCheck = false;
		if (mCurPlayerState == PLAYER_STATE.DANCE)
		{
			StopDance();
		}
		base.MoveTo(pos, stopRange, arriveFinsh);
	}

	public void UseInvincibleSkill()
	{
		switch (GetWeaponType())
		{
		case 0:
			UseSkill(GameDefine.XD_INVINCIBLE_SKILL_ID);
			break;
		case 1:
			UseSkill(GameDefine.QJ_INVINCIBLE_SKILL_ID);
			break;
		case 2:
			UseSkill(GameDefine.NQS_INVINCIBLE_SKILL_ID);
			break;
		}
	}

	public void MoveTo(Vector3 pos, float stopRange, TargetArriveFinsh arriveFinsh, bool isNeedUpdateCheck)
	{
		MoveTo(pos, stopRange, arriveFinsh);
		mIsNeedUpdateCheck = isNeedUpdateCheck;
	}

	public new void StopMove()
	{
		base.StopMove();
		mIsNeedUpdateCheck = false;
	}

	private void UpdateMoveCheck()
	{
		if (mIsNeedUpdateCheck && targetArriveFinish != null)
		{
			tempTargetArriveFinish = targetArriveFinish;
			targetArriveFinish = null;
			tempTargetArriveFinish(this);
		}
	}

	public override void OnStun(BuffInfoData buffInfoData)
	{
		if (IsDrivingMount())
		{
			SendServerDisMountCar();
		}
		base.OnStun(buffInfoData);
	}

	public override void OnKnockDown(BuffInfoData buffInfoData, ObjCharacter sender)
	{
		if (IsDrivingMount())
		{
			SendServerDisMountCar();
		}
		base.OnKnockDown(buffInfoData, sender);
	}

	public void EnterAutoCombat()
	{
		IsOpenAutoCombat = true;
		if (base.IsMoving)
		{
			AutoComabat = false;
			BreakAutoCombatTime = Time.time;
		}
		else
		{
			AutoComabat = true;
		}
		UpdateShowAuotState();
	}

	public void LeveAutoCombat()
	{
		AutoComabat = false;
		IsOpenAutoCombat = false;
		UpdateShowAuotState();
		if (SingletonUnity<FunctionBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionBtnRootLogic>.Instance.gameObject))
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateAutoBtn();
		}
		if (SingletonUnity<CopyFunctionRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CopyFunctionRootLogic>.Instance.gameObject))
		{
			SingletonUnity<CopyFunctionRootLogic>.Instance.UpdateAutoFightBtn();
		}
	}

	public void ReturnAutoCombatState()
	{
		if (IsOpenAutoCombat && !AutoComabat)
		{
			AutoComabat = true;
			BreakAutoCombatTime = 0f;
		}
	}

	private void InitAutoInfo()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.AutoComboInfoUI, delegate
		{
			mAutoComboInfoLogic = SingletonUnity<AutoComboInfoLogic>.Instance;
		});
	}

	public GameItem SelectDragItem()
	{
		int index = 0;
		if (dragList == null || dragList.Count <= 0)
		{
			dragList = ItemContainerTool.GetTargetPotionItemLevel(mGameManager.PlayerData.ItemBackPack, AttributeData.Level);
		}
		while (dragList.Count > 0)
		{
			if (!AutoUseSort)
			{
				index = dragList.Count - 1;
			}
			GameItem gameItem = dragList[index];
			if (gameItem != null && !gameItem.IsEmpty())
			{
				return gameItem;
			}
			dragList.RemoveAt(index);
		}
		dragList = ItemContainerTool.GetTargetPotionItemLevel(mGameManager.PlayerData.ItemBackPack, AttributeData.Level);
		while (dragList.Count > 0)
		{
			if (!AutoUseSort)
			{
				index = dragList.Count - 1;
			}
			GameItem gameItem2 = dragList[index];
			if (gameItem2 != null && !gameItem2.IsEmpty())
			{
				return gameItem2;
			}
			dragList.RemoveAt(index);
		}
		return null;
	}

	private void UpdateUseDrag()
	{
		if (IsOpenAutoCombat && AutoUseDrag && (float)AttributeData.HP / (float)AttributeData.MaxHP <= AutoUseDragThreshold)
		{
			GameItem gameItem = SelectDragItem();
			if (gameItem != null && UseDrag(gameItem))
			{
				PotionLogic.AutoUpdateSelectItem(gameItem);
			}
		}
	}

	private void UpdateShowAuotState()
	{
		if (mAutoComboInfoLogic == null)
		{
			InitAutoInfo();
		}
		else
		{
			mAutoComboInfoLogic.Show(AutoComabat && currentCharacter != null);
		}
	}

	public bool GetAutoCombatState()
	{
		return AutoComabat && currentCharacter != null;
	}

	public void ClearAutoSelectCharacter()
	{
		currentCharacter = null;
	}

	public void BreakAutoCombatState()
	{
		AutoComabat = false;
		BreakAutoCombatTime = Time.time;
		currentCharacter = null;
	}

	public void StopAutoAndSkill()
	{
		BreakAutoCombatState();
		base.SkillLogic.BreakCurSkill();
	}

	public void UpdateAutoCombatBreakState()
	{
		if (IsOpenAutoCombat && !AutoComabat)
		{
			if (base.IsMoving || IsTalking || mCurPlayerState == PLAYER_STATE.DANCE)
			{
				BreakAutoCombatTime = Time.time;
			}
			if (Time.time - BreakAutoCombatTime >= 2f)
			{
				AutoComabat = true;
			}
		}
		else
		{
			BreakAutoCombatTime = 0f;
		}
	}

	public string PickSkill(List<string> list)
	{
		if (list == null || list.Count == 0)
		{
			return string.Empty;
		}
		string result = string.Empty;
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			SkillData skillDataById = DataManager.GetSkillDataById(list[i]);
			if (skillDataById != null && skillDataById.PriorityAutoCombat > num && skillDataById.PriorityAutoCombat > 0)
			{
				num = skillDataById.PriorityAutoCombat;
				result = list[i];
			}
		}
		return result;
	}

	public string SeleSkill()
	{
		skillList.Clear();
		for (int i = 1; i < PlayerSkillIDList.Count; i++)
		{
			CharacterSkillData characterSkillDataByID = GetCharacterSkillDataByID(PlayerSkillIDList[i]);
			if (characterSkillDataByID != null && characterSkillDataByID.CDTimeCount <= 0f && characterSkillDataByID.UnlockLevel <= AttributeData.Level)
			{
				skillList.Add(PlayerSkillIDList[i]);
			}
		}
		if (skillList.Count > 0)
		{
			return PickSkill(skillList);
		}
		return string.Empty;
	}

	private void UpdateAuto()
	{
		UpdateShowAuotState();
		if (base.IsDie)
		{
			return;
		}
		UpdateUseDrag();
		if (!AutoComabat || !GameManager.OnLineState)
		{
			return;
		}
		if (mSkillLogic.IsUsingSkill)
		{
			lastUseTime = Time.time;
		}
		else
		{
			if ((double)(Time.time - lastUseTime) < 0.1)
			{
				return;
			}
			lastUseTime = Time.time;
			string text = SeleSkill();
			if (currentCharacter != mSelectedTarget && mSelectedTarget != null && Singleton<ObjManager>.Instance.IsCanAttack(mSelectedTarget))
			{
				currentCharacter = mSelectedTarget;
			}
			if (currentCharacter == null || currentCharacter.IsDie || !UnityVersionUtil.IsActive(currentCharacter.gameObject) || Vector3.SqrMagnitude(currentCharacter.Position - base.Position) > 64f)
			{
				currentCharacter = Singleton<ObjManager>.Instance.FindCanAttackCharacter((int)AttributeData.Camp, base.Position);
				if (currentCharacter == null)
				{
					if (mCurSceneManager == null)
					{
						mCurSceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
					}
					mCurSceneManager.AutoFightAction();
					return;
				}
			}
			SelectTarget(currentCharacter);
			if (string.IsNullOrEmpty(text))
			{
				UseComboSkill();
				return;
			}
			SkillData skillDataById = DataManager.GetSkillDataById(text);
			float num = skillDataById.AutoAttackDistanceMeter + currentCharacter.ModelRadius;
			float num2 = Vector3.Distance(base.Position, currentCharacter.Position);
			if (num2 > num)
			{
				MoveTo(currentCharacter.Position, num2 - num - 0.5f);
			}
			else
			{
				UseSkill(text);
			}
		}
	}

	public bool IsInLeaveGuildTime()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime() - LeaveGuildTime < 86400;
	}

	public void CreatGuild(string GuildName, string notice, long icon, GameDefine.MONEY_TYPE type)
	{
		if (string.IsNullOrEmpty(GuildName))
		{
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveGuild())
		{
			return;
		}
		switch (type)
		{
		case GameDefine.MONEY_TYPE.GOLD:
			if (!GameMoneyHelper.BeforeCheckBuy(type, 499))
			{
				return;
			}
			break;
		case GameDefine.MONEY_TYPE.DIAMOND:
			if (!GameMoneyHelper.BeforeCheckBuy(type, 49))
			{
				return;
			}
			break;
		}
		guild_create.request request = new guild_create.request();
		request.guildName = GuildName;
		request.Icon = icon;
		if (!string.IsNullOrEmpty(notice))
		{
			request.notice = notice;
		}
		request.costType = (long)type;
		NetLogic.GetInstance().Send<Protocol.guild_create>(request);
		switch (type)
		{
		case GameDefine.MONEY_TYPE.GOLD:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Guild", "Create", "type_cash");
			break;
		case GameDefine.MONEY_TYPE.DIAMOND:
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Guild", "Create", "type_diamond");
			break;
		}
	}

	public void OpenGuild()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveGuild())
		{
			guild_req_info.request request = new guild_req_info.request();
			request.characterId = ServerId;
			NetLogic.GetInstance().Send<Protocol.guild_req_info>(request);
		}
		else
		{
			guild_req_list.request request2 = new guild_req_list.request();
			request2.characterId = ServerId;
			request2.curPage = 1L;
			NetLogic.GetInstance().Send<Protocol.guild_req_list>(request2);
		}
	}

	public void ApplyUpDataGuild()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveGuild())
		{
			guild_req_info.request request = new guild_req_info.request();
			request.characterId = ServerId;
			NetLogic.GetInstance().Send<Protocol.guild_req_info>(request);
		}
	}

	public void ApplyUpdataGuildMemberList()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			req_guild_member_info.request request = new req_guild_member_info.request();
			request.guildId = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.ServerId;
			NetLogic.GetInstance().Send<Protocol.req_guild_member_info>(request);
		}
	}

	public void ReqGuildSkill()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			NetLogic.GetInstance().Send<Protocol.req_guild_skill>();
		}
	}

	public bool JoinGuild(long GuildId)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveGuild())
		{
			return false;
		}
		if (ApplyGuildIDList.Contains(GuildId))
		{
			return false;
		}
		if (Singleton<ObjManager>.Instance.MainPlayer != null && Singleton<ObjManager>.Instance.MainPlayer.IsInLeaveGuildTime())
		{
			NoticeLogic.AddNotifyData("#{100777}");
			return false;
		}
		if (ApplyGuildIDList.Count <= 30)
		{
			ApplyGuildIDList.Add(GuildId);
			guild_join.request request = new guild_join.request();
			request.guildId = GuildId;
			NetLogic.GetInstance().Send<Protocol.guild_join>(request);
			return true;
		}
		NoticeLogic.AddNotifyData("#{100779}*30");
		return false;
	}

	public void LeaveGuild()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerGuild.CanLeaveGuild())
		{
			guild_leave.request request = new guild_leave.request();
			request.characterId = ServerId;
			NetLogic.GetInstance().Send<Protocol.guild_leave>(request);
		}
	}

	public void KickGuildMember(long ID)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerGuild.CanKickedMember(ID))
		{
			guild_kick.request request = new guild_kick.request();
			request.characterId = ID;
			NetLogic.GetInstance().Send<Protocol.guild_kick>(request);
		}
	}

	public void ChangeMemberJob(long ID, Guild_JOB job)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerGuild.CanChangMemmberJob(job))
		{
			guild_job_change.request request = new guild_job_change.request();
			request.characterId = ID;
			request.jobId = (long)job;
			NetLogic.GetInstance().Send<Protocol.guild_job_change>(request);
		}
	}

	public void UpGuildLevel()
	{
	}

	public void SearchAllGuild()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			guild_req_list.request request = new guild_req_list.request();
			request.characterId = ServerId;
			NetLogic.GetInstance().Send<Protocol.guild_req_list>(request);
		}
	}

	public void AgreeJoinGuild(long ID)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerGuild.CanApprove())
		{
			guild_approve_resverve.request request = new guild_approve_resverve.request();
			request.characterId = ID;
			request.isAgree = 1L;
			NetLogic.GetInstance().Send<Protocol.guild_approve_resverve>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Guild", "Member", "agree_times");
		}
	}

	public void DisAgreeJoinGuild(long ID)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerGuild.CanApprove())
		{
			guild_approve_resverve.request request = new guild_approve_resverve.request();
			request.characterId = ID;
			request.isAgree = 0L;
			NetLogic.GetInstance().Send<Protocol.guild_approve_resverve>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Guild", "Member", "disagree_times");
		}
	}

	public void LookAtLog()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveGuild())
		{
			guild_log.request rpcReq = new guild_log.request();
			NetLogic.GetInstance().Send<Protocol.guild_log>(rpcReq);
		}
	}

	public void ChangeGuildNotice(string notice)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerGuild.CanEditNotice())
		{
			req_guild_notice.request request = new req_guild_notice.request();
			request.notice = notice;
			NetLogic.GetInstance().Send<Protocol.req_guild_notice>(request);
		}
	}

	public void GuildDonate(GuildDonateData curData)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (!playerData.IsHaveGuild())
		{
			return;
		}
		if (GameDefine.ITEM_ID_MONEYTYPR.ContainsKey(curData.ItemID))
		{
			GameDefine.MONEY_TYPE type = GameDefine.ITEM_ID_MONEYTYPR[curData.ItemID];
			if (!GameMoneyHelper.BeforeCheckBuy(type, curData.ItemCount))
			{
				return;
			}
		}
		guild_donate.request request = new guild_donate.request();
		request.id = curData.ID;
		NetLogic.GetInstance().Send<Protocol.guild_donate>(request);
	}

	public void SetGuildNeedAppro(bool needAppro)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerGuild.CanSetApprove())
		{
			req_seting_guild_appro.request request = new req_seting_guild_appro.request();
			request.guildId = playerData.PlayerGuild.ServerId;
			request.isNeedAppro = needAppro;
			NetLogic.GetInstance().Send<Protocol.req_seting_guild_appro>(request);
		}
	}

	public void ReqInviteTeam(long teamId)
	{
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		if (instance.PlayerData.IsHaveTeam() && instance.PlayerData.TeamInfo.IsFull())
		{
			SendNotify(false, "team is full");
			return;
		}
		if (teamId != -1)
		{
			SendNotify(false, "invite is send");
		}
		req_invite_team.request request = new req_invite_team.request();
		request.characterid = teamId;
		NetLogic.GetInstance().Send<Protocol.req_invite_team>(request);
	}

	public void ReqJoinTeam(long memberId)
	{
	}

	public void ReqLeaveTeam()
	{
	}

	public void ReqKickTeamMenber(long memberId)
	{
	}

	public void ReqChangeTeamLeader(long memberId)
	{
	}

	public void LeaveTeam()
	{
	}

	public bool IsTeamLeader(long id)
	{
		if (id == -1 || SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.TeamID == -1)
		{
			return false;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.GetTeamber(0).ServerId == id)
		{
			return true;
		}
		return false;
	}

	public bool IsTeamLeader()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.TeamID == -1)
		{
			return false;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.GetTeamber(0).ServerId == ServerId)
		{
			return true;
		}
		return false;
	}

	private void UpdateFollowSpeed()
	{
		if (mFollowCharacter == null)
		{
			Speed = AttributeData.CurSpeed;
			if (Speed != mNavMeshAgent.speed)
			{
				mNavMeshAgent.speed = Speed;
				if (base.MountRoot != null)
				{
					base.MountRoot.UpdateSetSpeed(Speed);
				}
			}
			return;
		}
		if (Vector3.SqrMagnitude(mFollowCharacter.Position - base.Position) <= 16f)
		{
			Speed = mFollowCharacter.AttributeData.WalkSpeed;
			isWalk = true;
		}
		else
		{
			isWalk = false;
			Speed = AttributeData.CurSpeed;
		}
		if (Speed != mNavMeshAgent.speed)
		{
			mNavMeshAgent.speed = Speed;
			if (base.MountRoot != null)
			{
				base.MountRoot.UpdateSetSpeed(Speed);
			}
		}
	}

	public void EnterTeamFollow()
	{
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		if (instance.SceneManager.IsCopyScene() || instance.PlayerData.TeamInfo.TeamID == -1 || IsTeamLeader())
		{
			return;
		}
		TeamMember teamber = instance.PlayerData.TeamInfo.GetTeamber(0);
		if (!teamber.IsValid())
		{
			return;
		}
		foreach (KeyValuePair<long, Obj> item in Singleton<ObjManager>.Instance.ObjDict)
		{
			if (item.Value.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
			{
				ObjOtherPlayer objOtherPlayer = item.Value as ObjOtherPlayer;
				if (objOtherPlayer != null && objOtherPlayer.ServerId == teamber.ServerId)
				{
					FollowServerID = objOtherPlayer.ServerId;
					break;
				}
			}
		}
	}

	public void EnterFollowTarget(long targetId)
	{
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		if (!instance.SceneManager.IsCopyScene())
		{
			ObjManager instance2 = Singleton<ObjManager>.Instance;
			if (instance2.ObjDict.ContainsKey(targetId))
			{
				Obj obj = instance2.ObjDict[targetId];
				FollowServerID = obj.ServerId;
			}
		}
	}

	public void LeaveTeamFollow()
	{
		mFollowServerID = -1L;
		mFollowCharacter = null;
	}

	public bool IsTeamFollowState()
	{
		return mFollowServerID != -1;
	}

	public void UpdateTeamFollow()
	{
		if (IsTeamFollowState() && mFollowCharacter != null)
		{
			if (isWalk)
			{
				WalkMoveTo(mFollowCharacter.CacheTransform.position, 3f);
			}
			else
			{
				MoveTo(mFollowCharacter.CacheTransform.position, 3f);
			}
			UpdateFollowSpeed();
		}
	}
}
