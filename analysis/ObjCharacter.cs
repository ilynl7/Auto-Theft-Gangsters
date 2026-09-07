using System;
using System.Collections.Generic;
using DG.Tweening;
using SprotoType;
using UnityEngine;

public class ObjCharacter : Obj
{
	public delegate void TargetArriveFinsh(ObjCharacter objCha);

	public delegate void SkillUseSuccess(string id);

	public delegate void OnBeatonDelegate();

	public delegate void OnSkillFinishedDelegate();

	private float currentTime = 0.5f;

	private float checkTime = 0.5f;

	private Vector3 lastPos = Vector3.forward * float.MaxValue;

	public Dictionary<string, Transform> BonesTrsDict;

	private float mStopRange = 1f;

	private bool mIsMoving;

	private bool mIsDie;

	private bool mIsStunFlag;

	private bool mIsKnockDownFlag;

	private bool mIsSleepFlag;

	private bool mInvincibleFlag;

	public bool mIdleAttackFlag;

	private float idleAttackTimeCount;

	public static float idleAttackLastTime = 5f;

	private Vector3 mTargetPos;

	protected AnimationLogic mAnimationLogic;

	protected SkillLogic mSkillLogic;

	protected SkillMotion mSkillMotion;

	protected EffectLogic mEffectLogic;

	protected EffectMotion mEffectMotion;

	protected BuffLogic mBuffLogic;

	protected string mCurUseSkillId = string.Empty;

	protected HeadInfoLogic mHeadInfoLogic;

	protected GameObject mSimpleShadow;

	protected CharacterAttributeData mAttributeData;

	protected NavMeshAgent mNavMeshAgent;

	protected TargetArriveFinsh targetArriveFinish;

	protected TargetArriveFinsh tempTargetArriveFinish;

	protected Dictionary<string, SkillUseSuccess> mSkillUseSuccessDic = new Dictionary<string, SkillUseSuccess>();

	public OnBeatonDelegate onBeaton;

	public OnSkillFinishedDelegate onSkillFinished;

	protected float mComboTimeCount;

	protected float mComboTimeTotalCount;

	protected string mComboIndex;

	protected bool mComboNextFlag;

	private bool mIsLocalDrivingCar;

	private ObjPlayerCar mCurPlayerCar;

	private GameDefine.ANIMATIONSTATE mCurAnimationState;

	protected List<CharacterSkillData> mCharacterSkillData = new List<CharacterSkillData>();

	protected List<ObjCharacter> mEffectTargetList = new List<ObjCharacter>();

	protected float mHoldTimeCount;

	protected PLAYER_STATE mCurPlayerState;

	public string[] TargetPartObjId = new string[4];

	private string WeaponModelID = string.Empty;

	private string WeaponItemID = string.Empty;

	private string mWeaponTypeName = string.Empty;

	public bool IsShowInvincibleEffect = true;

	private float invincibleEffectSpeed = 0.2f;

	private Material[] meshMats;

	private float invincibleFlashSkinPercent;

	protected float mReceiveBiggerHpTime = 1f;

	protected float mReceiveBiggerHpTimeCount;

	public virtual float ModelRadius => base.CurrentCharacterModelData.ModelRadius;

	public float mMoveSpeed => AttributeData.CurSpeed;

	public bool IsMoving
	{
		get
		{
			return mIsMoving;
		}
		set
		{
			mIsMoving = value;
		}
	}

	public bool IsDie
	{
		get
		{
			return mIsDie;
		}
		set
		{
			mIsDie = value;
		}
	}

	public bool IsStunFlag
	{
		get
		{
			return mIsStunFlag;
		}
		set
		{
			mIsStunFlag = value;
		}
	}

	public bool IsKnockDownFlag
	{
		get
		{
			return mIsKnockDownFlag;
		}
		set
		{
			mIsKnockDownFlag = value;
		}
	}

	public bool IsSleepFlag
	{
		get
		{
			return mIsSleepFlag;
		}
		set
		{
			mIsSleepFlag = value;
		}
	}

	public bool InvincibleFlag
	{
		get
		{
			return mInvincibleFlag;
		}
		set
		{
			mInvincibleFlag = value;
			if (value)
			{
				invincibleFlashSkinPercent = 0f;
				invincibleEffectSpeed = Mathf.Abs(invincibleEffectSpeed);
			}
			else
			{
				ResetSkinColor();
			}
		}
	}

	public bool IdleAttackFlag
	{
		get
		{
			return mIdleAttackFlag;
		}
		set
		{
			mIdleAttackFlag = value;
		}
	}

	protected Vector3 TargetPos => mTargetPos;

	public AnimationLogic AnimationLogic
	{
		get
		{
			return mAnimationLogic;
		}
		set
		{
			mAnimationLogic = value;
		}
	}

	public SkillLogic SkillLogic => mSkillLogic;

	public SkillMotion SkillMotion => mSkillMotion;

	public EffectLogic EffectLogic => mEffectLogic;

	public EffectMotion EffectMotion => mEffectMotion;

	public BuffLogic BuffLogic => mBuffLogic;

	public string CurUseSkillId
	{
		get
		{
			return mCurUseSkillId;
		}
		set
		{
			mCurUseSkillId = value;
		}
	}

	public HeadInfoLogic HeadInfoLogic
	{
		get
		{
			return mHeadInfoLogic;
		}
		set
		{
			mHeadInfoLogic = value;
		}
	}

	public GameObject SimpleShadow
	{
		get
		{
			return mSimpleShadow;
		}
		set
		{
			mSimpleShadow = value;
		}
	}

	public virtual CharacterAttributeData AttributeData
	{
		get
		{
			return mAttributeData;
		}
		set
		{
			mAttributeData = value;
		}
	}

	public NavMeshAgent NavMeshAgent
	{
		get
		{
			return mNavMeshAgent;
		}
		set
		{
			mNavMeshAgent = value;
		}
	}

	public float ComboValidTime => mComboTimeCount;

	public bool IsLocalDrivingCar
	{
		get
		{
			return mIsLocalDrivingCar;
		}
		set
		{
			mIsLocalDrivingCar = value;
		}
	}

	public ObjPlayerCar CurPlayerCar
	{
		get
		{
			return mCurPlayerCar;
		}
		set
		{
			mCurPlayerCar = value;
		}
	}

	public GameDefine.ANIMATIONSTATE CurAnimationState
	{
		get
		{
			return mCurAnimationState;
		}
		set
		{
			OnSwithAnimState(value);
		}
	}

	public virtual List<CharacterSkillData> CharacterSkillData
	{
		get
		{
			return mCharacterSkillData;
		}
		set
		{
			mCharacterSkillData = value;
		}
	}

	public float HoldTimeCount
	{
		get
		{
			return mHoldTimeCount;
		}
		set
		{
			mHoldTimeCount = value;
		}
	}

	public PLAYER_STATE CurPlayerState
	{
		get
		{
			return mCurPlayerState;
		}
		set
		{
			mCurPlayerState = value;
		}
	}

	public string WeaponTypeName
	{
		get
		{
			if (string.IsNullOrEmpty(mWeaponTypeName))
			{
				mWeaponTypeName = GetWeaponName();
			}
			return mWeaponTypeName;
		}
		set
		{
			mWeaponTypeName = value;
		}
	}

	public void DisactiveTargetArriveFinish()
	{
		targetArriveFinish = null;
	}

	public void UpdatePlayerMeshId(string weaponId, string headId, string bodyId, string legId)
	{
		SetTargetPartObjId(MODEL_TYPE.WEAPON, weaponId);
		SetTargetPartObjId(MODEL_TYPE.HEAD, headId);
		SetTargetPartObjId(MODEL_TYPE.BODY, bodyId);
		SetTargetPartObjId(MODEL_TYPE.LEG, legId);
	}

	public virtual void SetTargetPartObjId(MODEL_TYPE type, string id)
	{
		TargetPartObjId[(int)type] = id;
	}

	public string GetWeaponName()
	{
		if (!string.IsNullOrEmpty(WeaponItemID))
		{
			EquipData equipDataById = DataManager.GetEquipDataById(WeaponItemID);
			if (equipDataById != null && GameDefine.WeaponTypeName.ContainsKey(equipDataById.WeaponType))
			{
				return GameDefine.WeaponTypeName[equipDataById.WeaponType];
			}
		}
		return GameDefine.GetWeaponName(WeaponModelID);
	}

	public int GetWeaponType()
	{
		if (GameDefine.NameWeaponType.ContainsKey(WeaponTypeName))
		{
			return GameDefine.NameWeaponType[WeaponTypeName];
		}
		return 0;
	}

	public void UpdateWeaponModeID(string weaponmodeid)
	{
		if (!string.IsNullOrEmpty(weaponmodeid) && (string.IsNullOrEmpty(WeaponModelID) || !WeaponModelID.Equals(weaponmodeid)))
		{
			WeaponModelID = weaponmodeid;
			WeaponTypeName = GetWeaponName();
			if (base.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER && mAnimationLogic != null)
			{
				mAnimationLogic.PlayAnimationDataList.Clear();
			}
		}
	}

	public void UpdateWeaponItemID(string weaponitemid)
	{
		if (!string.IsNullOrEmpty(weaponitemid) && (string.IsNullOrEmpty(WeaponItemID) || !WeaponItemID.Equals(weaponitemid)))
		{
			WeaponItemID = weaponitemid;
			WeaponTypeName = GetWeaponName();
			if (base.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER && mAnimationLogic != null)
			{
				mAnimationLogic.PlayAnimationDataList.Clear();
			}
		}
	}

	public string GetActionName(string actname)
	{
		if (base.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || base.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER || base.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER || base.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL)
		{
			if (!string.IsNullOrEmpty(WeaponTypeName))
			{
				return $"{base.IndexName}_{WeaponTypeName}_{actname}";
			}
			return null;
		}
		return $"{base.IndexName}_{actname}";
	}

	public string GetActionNameNoName(string actname)
	{
		if (base.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || base.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER || base.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER || base.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL)
		{
			if (!string.IsNullOrEmpty(WeaponTypeName))
			{
				return $"{base.IndexName}_{WeaponTypeName}_{actname}";
			}
			return null;
		}
		return $"{actname}";
	}

	public override void Init()
	{
		base.Init();
		if (mAnimationLogic == null)
		{
			mAnimationLogic = base.gameObject.AddComponent<AnimationLogic>();
		}
		if (mSkillLogic == null)
		{
			mSkillLogic = new SkillLogic();
		}
		if (mEffectLogic == null)
		{
			mEffectLogic = base.gameObject.AddComponent<EffectLogic>();
		}
		if (mSkillMotion == null)
		{
			mSkillMotion = base.gameObject.AddComponent<SkillMotion>();
		}
		if (mEffectMotion == null)
		{
			mEffectMotion = base.gameObject.AddComponent<EffectMotion>();
		}
		if (mBuffLogic == null)
		{
			mBuffLogic = base.gameObject.AddComponent<BuffLogic>();
		}
		if (AttributeData == null)
		{
			AttributeData = new CharacterAttributeData();
		}
		RegisterEvent();
		mAnimationLogic.Init(this);
		mCurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
		mSkillMotion.Init(this);
		mEffectLogic.Init(this);
		mEffectMotion.Init(this);
		mBuffLogic.Init(this);
		mTransform = base.transform;
	}

	public void InitNavMeshAgent()
	{
		if (mNavMeshAgent == null)
		{
			mNavMeshAgent = base.gameObject.AddComponent<NavMeshAgent>();
		}
		ResetNavMeshAgent();
	}

	public virtual void UpdatePlayerSpeed()
	{
	}

	public virtual void UpdateMountSpeed()
	{
	}

	public virtual void Reset()
	{
		IdleAttackFlag = false;
		IsDie = false;
		IsStunFlag = false;
		IsKnockDownFlag = false;
		IsSleepFlag = false;
		InvincibleFlag = false;
		IsShowInvincibleEffect = true;
		BuffLogic.ClearBuff();
		AnimationLogic.ResetAnimationFlag();
		CurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
		if (mNavMeshAgent != null && mNavMeshAgent.enabled)
		{
			mNavMeshAgent.Stop();
			mNavMeshAgent.ResetPath();
			mNavMeshAgent.enabled = false;
		}
		mSkillLogic.ResetSkillLogic();
		if (mObjType != GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			InitSimpleShadow();
		}
		else if (!GameSettingData.IsShowPlayerShadow[GameSettingData.GetPhoneClass()])
		{
			InitSimpleShadow();
		}
		targetArriveFinish = null;
		mIsLocalDrivingCar = false;
		mCurPlayerCar = null;
	}

	public void InitSimpleShadow()
	{
		if (mSimpleShadow == null)
		{
			ResourcesManager.LoadSimpleShadowPrefab(UIInfo.SimpleShadowUI, "SimpleShadow", LoadSimpleShadow);
		}
	}

	private void LoadSimpleShadow(GameObject obj)
	{
		if (obj != null)
		{
			SimpleShadowFollow simpleShadowFollow = obj.GetComponent<SimpleShadowFollow>();
			if (simpleShadowFollow == null)
			{
				simpleShadowFollow = obj.AddComponent<SimpleShadowFollow>();
			}
			simpleShadowFollow.enabled = true;
			simpleShadowFollow.BindObj = base.gameObject;
			simpleShadowFollow.DeltaHeight = 0f;
			mSimpleShadow = obj;
			UnityVersionUtil.SetActiveRecursive(mSimpleShadow.gameObject, state: true);
		}
	}

	public void ResetNavMeshAgent()
	{
		if (mNavMeshAgent != null)
		{
			mNavMeshAgent.enabled = true;
			mNavMeshAgent.radius = ModelRadius;
			mNavMeshAgent.stoppingDistance = 0.8f;
			mNavMeshAgent.speed = mMoveSpeed;
			mNavMeshAgent.acceleration = 10000f;
			mNavMeshAgent.angularSpeed = 30000f;
			mNavMeshAgent.walkableMask = 1;
			mNavMeshAgent.autoBraking = false;
			mNavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
			if (mObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
			{
				mNavMeshAgent.walkableMask += 65536;
			}
			UpdateMountSpeed();
		}
	}

	public void InitInfo(CharacterModelData characterModelData)
	{
		if (characterModelData != null)
		{
			mCharacterModelData = characterModelData;
		}
	}

	public void UpdateMove()
	{
		if (IsMoving)
		{
			float num = VectorXZ.Distance(new VectorXZ(mTargetPos.x, mTargetPos.z), new VectorXZ(base.Position.x, base.Position.z));
			if (num - mStopRange <= 0f)
			{
				StopMove();
			}
			else if (num - mMoveSpeed * Time.deltaTime <= 0f)
			{
				base.Position = mTargetPos;
				StopMove();
			}
		}
	}

	public void UpdateSkillCD()
	{
		for (int i = 0; i < CharacterSkillData.Count; i++)
		{
			if (CharacterSkillData[i].CDTimeCount > 0f)
			{
				CharacterSkillData[i].CDTimeCount -= Time.deltaTime;
				if (CharacterSkillData[i].CDTimeCount <= 0f)
				{
					OnSkillEnable(CharacterSkillData[i].ID);
				}
			}
		}
	}

	public void UpdateHoldTime()
	{
		if (mHoldTimeCount > 0f)
		{
			mHoldTimeCount -= Time.deltaTime;
		}
	}

	public void UpdateComponent()
	{
		mAnimationLogic.AnimationLogicUpdate();
		mEffectLogic.UpdatePlayEffInfoData();
		mEffectMotion.UpdateEffectMotion();
		mSkillMotion.UpdateSkillMotion();
		mBuffLogic.UpdateBuffLogic();
		UpdateIdleAttackLastTime();
		UpdateInvincibleEffect();
	}

	public void UpdateInvincibleEffect()
	{
		if (InvincibleFlag && IsShowInvincibleEffect)
		{
			invincibleFlashSkinPercent += invincibleEffectSpeed;
			InvincibleFlashSkin();
			if (invincibleFlashSkinPercent >= 1f || invincibleFlashSkinPercent <= 0f)
			{
				invincibleEffectSpeed = 0f - invincibleEffectSpeed;
			}
		}
	}

	public void UpdateIdleAttackLastTime()
	{
		if (IdleAttackFlag && !IsStun() && !mSkillLogic.IsUsingSkill)
		{
			idleAttackTimeCount -= Time.deltaTime;
			if (idleAttackTimeCount <= 0f)
			{
				DisactiveIdleAttack();
			}
		}
	}

	public void AddPlayAnimationData(string actionName, float delayTime, string effInfoID)
	{
		if (mAnimationLogic != null)
		{
			mAnimationLogic.AddPlayAnimationData(actionName, delayTime, effInfoID);
		}
	}

	public void AddPlayEffInfoData(string actionName, string effInfoID, float delayTime, Vector3 senderPos, Transform targetTransform = null)
	{
		if (mEffectLogic != null)
		{
			mEffectLogic.AddPlayEffInfoData(actionName, effInfoID, delayTime, senderPos, targetTransform);
		}
	}

	public void AddPlayeBufEffInfoData(string fxeffectInfoId, float delayTime, float duration, Vector3 senderPosition)
	{
		if (mEffectLogic != null)
		{
			mEffectLogic.AddPlayeBufEffInfoData(fxeffectInfoId, delayTime, duration, senderPosition);
		}
	}

	public void AddPlayerEffMotionData(string effInfoId, float delayTime, Vector3 senderPos)
	{
		if (mEffectMotion != null)
		{
			mEffectMotion.AddPlayEffInfoMotionData(effInfoId, delayTime, senderPos);
		}
	}

	public void PlayYinChangeEffInfo(string effInfoId, float duration, Vector3 senderPos)
	{
		if (mEffectLogic != null)
		{
			mEffectLogic.PlayYinChangeEffInfo(effInfoId, duration, senderPos);
		}
	}

	public void AddBuffInfoData(string buffID, float delayTime, float duration, ObjCharacter sender)
	{
		if (mBuffLogic != null)
		{
			mBuffLogic.AddBuffInfoData(buffID, delayTime, duration, sender);
		}
	}

	public void OnSwithAnimState(GameDefine.ANIMATIONSTATE newState)
	{
		if (mCurPlayerState == PLAYER_STATE.NORMAL)
		{
			if (newState == GameDefine.ANIMATIONSTATE.IDLE && mIdleAttackFlag)
			{
				newState = GameDefine.ANIMATIONSTATE.IDLE_ATTACK;
			}
			mCurAnimationState = newState;
			switch (CurAnimationState)
			{
			case GameDefine.ANIMATIONSTATE.IDLE:
				ChangeIdleState();
				break;
			case GameDefine.ANIMATIONSTATE.RUN:
				ChangeRunState();
				break;
			case GameDefine.ANIMATIONSTATE.WALK:
				ChangeWalkState();
				break;
			case GameDefine.ANIMATIONSTATE.DIE:
				ChangeDieState();
				break;
			case GameDefine.ANIMATIONSTATE.IDLE_ATTACK:
				ChangeIdleAttackState();
				break;
			case GameDefine.ANIMATIONSTATE.DRIVING:
				ChangeDrivingState();
				break;
			}
		}
	}

	protected virtual void ChangeIdleState()
	{
		mAnimationLogic.PlayAnimation(0);
	}

	protected virtual void ChangeRunState()
	{
		mAnimationLogic.PlayAnimation(1);
	}

	private void ChangeWalkState()
	{
		mAnimationLogic.PlayAnimation(2);
	}

	private void ChangeDieState()
	{
		mAnimationLogic.ForcePlayAnimation(mAnimationLogic.animationName[3], null, -1f, 0f);
	}

	private void ChangeIdleAttackState()
	{
		mAnimationLogic.PlayAnimation(4);
	}

	public void ChangeDrivingState()
	{
		mAnimationLogic.PlayAnimation(5);
	}

	public virtual void ChangeWeaponAnimaCheck()
	{
		if (!IsDie)
		{
			if (IsMoving)
			{
				ChangeRunState();
			}
			else if (IdleAttackFlag)
			{
				CurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
			}
			else
			{
				CurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
			}
		}
	}

	private void StartMove()
	{
		if (mNavMeshAgent != null)
		{
			mNavMeshAgent.speed = AttributeData.CurSpeed;
			UpdateMountSpeed();
		}
		if (CurAnimationState != GameDefine.ANIMATIONSTATE.RUN)
		{
			CurAnimationState = GameDefine.ANIMATIONSTATE.RUN;
		}
	}

	public virtual void StopMove()
	{
		if (IsMoving)
		{
			IsMoving = false;
			currentTime = checkTime;
			lastPos = Vector3.forward * float.MaxValue;
			CurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
			if (mNavMeshAgent != null && mNavMeshAgent.enabled)
			{
				mNavMeshAgent.Stop();
			}
			if (targetArriveFinish != null)
			{
				tempTargetArriveFinish = targetArriveFinish;
				targetArriveFinish = null;
				tempTargetArriveFinish(this);
			}
		}
		else if (targetArriveFinish != null)
		{
			tempTargetArriveFinish = targetArriveFinish;
			targetArriveFinish = null;
			tempTargetArriveFinish(this);
		}
	}

	private bool CheckArrive(Vector3 target)
	{
		float num = Vector3.SqrMagnitude(target - base.Position);
		if (num <= mStopRange * mStopRange)
		{
			return true;
		}
		return false;
	}

	public bool BeforeMoveCheck()
	{
		if (IsDie)
		{
			return true;
		}
		if (IsStun())
		{
			return true;
		}
		if (mSkillLogic.IsUsingSkill)
		{
			return true;
		}
		if (mEffectMotion.NeedMoveFlag)
		{
			return true;
		}
		if (mSkillMotion.NeedMoveFlag)
		{
			return true;
		}
		if (mCurPlayerState == PLAYER_STATE.SOCIAL_DANCE)
		{
			return true;
		}
		if (IsLocalDrivingCar)
		{
			return true;
		}
		return false;
	}

	public bool BeforeSkillCheck()
	{
		if (IsDie)
		{
			return true;
		}
		if (IsStun())
		{
			return true;
		}
		if (!CheckHoldTime())
		{
			return true;
		}
		if (mEffectMotion.NeedMoveFlag)
		{
			return true;
		}
		if (mSkillMotion.NeedMoveFlag)
		{
			return true;
		}
		return false;
	}

	public void UpdateStopDistance(float distance)
	{
		mStopRange = distance;
		if (mNavMeshAgent != null)
		{
			mNavMeshAgent.stoppingDistance = distance;
		}
	}

	public void MoveTo(float posX, float posY, float posZ, float stopRange = 1f, TargetArriveFinsh arriveFinsh = null)
	{
		MoveTo(new Vector3(posX, posY, posZ), stopRange, arriveFinsh);
	}

	public void MoveTo(float posX, float posZ, float stopRange = 1f, TargetArriveFinsh arriveFinsh = null)
	{
		MoveTo(new Vector3(posX, SceneManager.GetHitHeight(new Vector3(posX, 0f, posZ)), posZ), stopRange, arriveFinsh);
	}

	public virtual void MoveTo(Vector3 pos, float stopRange = 1f, TargetArriveFinsh arriveFinsh = null)
	{
		if (BeforeMoveCheck())
		{
			return;
		}
		targetArriveFinish = arriveFinsh;
		mStopRange = stopRange;
		mTargetPos = pos;
		if (CheckArrive(pos))
		{
			StopMove();
			return;
		}
		StartMove();
		IsMoving = true;
		if (mNavMeshAgent != null && mNavMeshAgent.enabled)
		{
			mNavMeshAgent.stoppingDistance = stopRange;
			mNavMeshAgent.SetDestination(mTargetPos);
		}
		else
		{
			Debug.Log("mNavMeshAgent.enabled == false");
		}
	}

	private void FaceTo(Vector3 pos)
	{
		Vector3 vector = pos - base.Position;
		vector.y = 0f;
		if (vector != Vector3.zero)
		{
			base.CacheTransform.rotation = Quaternion.Slerp(base.CacheTransform.rotation, Quaternion.LookRotation(vector), 10f * Time.deltaTime);
		}
	}

	public void WalkMoveTo(Vector3 pos, float stopRange = 1f, TargetArriveFinsh arriveFinsh = null)
	{
		if (BeforeMoveCheck())
		{
			return;
		}
		targetArriveFinish = arriveFinsh;
		mStopRange = stopRange;
		mTargetPos = pos;
		if (CheckArrive(pos))
		{
			StopMove();
			return;
		}
		StartWalk();
		IsMoving = true;
		if (mNavMeshAgent != null && mNavMeshAgent.enabled)
		{
			mNavMeshAgent.stoppingDistance = stopRange;
			mNavMeshAgent.SetDestination(mTargetPos);
		}
	}

	private void StartWalk()
	{
		if (mNavMeshAgent != null)
		{
			mNavMeshAgent.speed = AttributeData.WalkSpeed;
			UpdateMountSpeed();
		}
		CurAnimationState = GameDefine.ANIMATIONSTATE.WALK;
	}

	public void FaceToPub(Vector3 pos)
	{
		Vector3 vector = pos - base.Position;
		vector.y = 0f;
		if (vector != Vector3.zero)
		{
			base.CacheTransform.rotation = Quaternion.LookRotation(vector);
		}
	}

	public CharacterSkillData GetCharacterSkillDataByID(string skillID)
	{
		if (CharacterSkillData.Count == 0)
		{
			return null;
		}
		for (int i = 0; i < CharacterSkillData.Count; i++)
		{
			if (CharacterSkillData[i].ID == skillID)
			{
				return CharacterSkillData[i];
			}
		}
		return null;
	}

	public virtual void UpdateSkillList(Dictionary<string, skill_info> skills)
	{
	}

	public bool CheckHoldTime()
	{
		if (mHoldTimeCount > 0f)
		{
			return false;
		}
		return true;
	}

	public bool CheckSkillCD(SkillData skillData)
	{
		CharacterSkillData characterSkillDataByID = GetCharacterSkillDataByID(skillData.ID);
		if (characterSkillDataByID != null)
		{
			if (characterSkillDataByID.CDTimeCount <= 0f)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public bool CheckSkillDistance(SkillData skillData, ObjCharacter target)
	{
		if (skillData.TraceDistance <= 0)
		{
			return true;
		}
		float traceDistanceMeter = skillData.TraceDistanceMeter;
		float num = VectorXZ.Distance(base.Position, target.Position);
		float num2 = num - traceDistanceMeter - ModelRadius - target.ModelRadius;
		if (num2 > 0f)
		{
			return false;
		}
		return true;
	}

	public float GetPathDistance(Vector3 pos)
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		if (!NavMesh.CalculatePath(base.Position, pos, -1, navMeshPath))
		{
			return 9999f;
		}
		float num = 0f;
		for (int i = 1; i < navMeshPath.corners.Length; i++)
		{
			num += VectorXZ.Distance(navMeshPath.corners[i], navMeshPath.corners[i - 1]);
		}
		return num;
	}

	public virtual void OnDie()
	{
		if (!IsDie)
		{
			IsDie = true;
			DisableNavMeshAgent();
			if (mSkillLogic.IsUsingSkill)
			{
				mSkillLogic.BreakCurSkill();
			}
			if (IsMoving)
			{
				StopMove();
			}
			CurAnimationState = GameDefine.ANIMATIONSTATE.DIE;
			DisactiveHeadInfo();
			BuffLogic.ClearBuff();
		}
	}

	public virtual void OnRelife(long hp, Vector3 pos)
	{
		Reset();
		if (mAnimationLogic != null && mAnimationLogic.AnimaObj != null)
		{
			mAnimationLogic.AnimaObj.Stop();
			CurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
		}
		AttributeData.HP = hp;
		EnableNavMeshAgent();
		ActiveHeadInfo();
		UpdateHeadInfo();
		base.Position = pos;
	}

	public bool IsStun()
	{
		return IsStunFlag || IsSleepFlag || IsKnockDownFlag;
	}

	public virtual void OnStun(BuffInfoData buffInfoData)
	{
		mIsStunFlag = true;
		if (IsMoving)
		{
			StopMove();
		}
		if (mSkillLogic.IsUsingSkill)
		{
			mSkillLogic.BreakCurSkill();
		}
	}

	public void OnStunDone(BuffInfoData buffInfoData)
	{
		mIsStunFlag = false;
		if (!mNavMeshAgent.enabled)
		{
			EnableNavMeshAgent();
		}
	}

	public virtual void OnKnockDown(BuffInfoData buffInfoData, ObjCharacter sender)
	{
		mIsKnockDownFlag = true;
		if (IsMoving)
		{
			StopMove();
		}
		FaceToPub(sender.Position);
		if (mSkillLogic.IsUsingSkill)
		{
			mSkillLogic.BreakCurSkill();
		}
	}

	public void OnKnockDownDone(BuffInfoData buffInfoData)
	{
		mIsKnockDownFlag = false;
		if (!mNavMeshAgent.enabled)
		{
			EnableNavMeshAgent();
		}
	}

	public void OnSleep(BuffInfoData buffInfoData)
	{
		mIsSleepFlag = true;
		if (IsMoving)
		{
			StopMove();
		}
	}

	public void OnSleepDone(BuffInfoData buffInfoData)
	{
		mIsSleepFlag = false;
	}

	public void OnChangeAttr(BuffInfoData buffInfoData)
	{
		switch ((ATTRIBUTE_TYPE)buffInfoData.AttrID)
		{
		case ATTRIBUTE_TYPE.ATK:
			AttributeData.CurATK = CalAttrValue(AttributeData.CurATK, AttributeData.ATK, buffInfoData);
			break;
		case ATTRIBUTE_TYPE.HIT:
			AttributeData.CurHIT = CalAttrValue(AttributeData.CurHIT, AttributeData.HIT, buffInfoData);
			break;
		case ATTRIBUTE_TYPE.CRI:
			AttributeData.CurCRI = CalAttrValue(AttributeData.CurCRI, AttributeData.CRI, buffInfoData);
			break;
		case ATTRIBUTE_TYPE.DEF:
			AttributeData.CurDEF = CalAttrValue(AttributeData.CurDEF, AttributeData.DEF, buffInfoData);
			break;
		case ATTRIBUTE_TYPE.DGE:
			AttributeData.CurDGE = CalAttrValue(AttributeData.CurDGE, AttributeData.DGE, buffInfoData);
			break;
		case ATTRIBUTE_TYPE.HP:
			break;
		}
	}

	public float CalAttrValue(float curNum, float defaultNum, BuffInfoData buffInfoData)
	{
		return buffInfoData.AttrType switch
		{
			0 => curNum + (float)buffInfoData.AttrValue, 
			1 => curNum + defaultNum * (float)buffInfoData.AttrValue, 
			2 => curNum, 
			_ => curNum, 
		};
	}

	public void OnChangeAttrDone(BuffInfoData buffInfoData)
	{
		switch ((ATTRIBUTE_TYPE)buffInfoData.AttrID)
		{
		case ATTRIBUTE_TYPE.ATK:
			AttributeData.CurATK = CalRemoveAttrValue(AttributeData.CurATK, AttributeData.ATK, buffInfoData);
			break;
		case ATTRIBUTE_TYPE.HIT:
			AttributeData.CurHIT = CalRemoveAttrValue(AttributeData.CurHIT, AttributeData.HIT, buffInfoData);
			break;
		case ATTRIBUTE_TYPE.CRI:
			AttributeData.CurCRI = CalRemoveAttrValue(AttributeData.CurCRI, AttributeData.CRI, buffInfoData);
			break;
		case ATTRIBUTE_TYPE.DEF:
			AttributeData.CurDEF = CalRemoveAttrValue(AttributeData.CurDEF, AttributeData.DEF, buffInfoData);
			break;
		case ATTRIBUTE_TYPE.DGE:
			AttributeData.CurDGE = CalRemoveAttrValue(AttributeData.CurDGE, AttributeData.DGE, buffInfoData);
			break;
		case ATTRIBUTE_TYPE.HP:
			break;
		}
	}

	public float CalRemoveAttrValue(float curNum, float defaultNum, BuffInfoData buffInfoData)
	{
		return buffInfoData.AttrType switch
		{
			0 => curNum - (float)buffInfoData.AttrValue, 
			1 => curNum - defaultNum * (float)buffInfoData.AttrValue, 
			2 => curNum, 
			_ => curNum, 
		};
	}

	public void FlashSkin()
	{
		if (mObjType == GameDefine.OBJ_TYPE.OBJ_PLAYER_CAR)
		{
			return;
		}
		if (meshMats == null)
		{
			SkinnedMeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
			if (componentsInChildren.Length == 0)
			{
				return;
			}
			meshMats = new Material[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				meshMats[i] = componentsInChildren[i].material;
			}
		}
		if (meshMats == null)
		{
			return;
		}
		vp_Timer.In(0.1f, delegate
		{
			for (int j = 0; j < meshMats.Length; j++)
			{
				meshMats[j].SetColor("_RimColor", Color.black);
				meshMats[j].SetFloat("_RimPower", 0.05f);
				meshMats[j].DOColor(new Color(0.5f, 0.5f, 0.5f, 1f), "_RimColor", 0.1f);
				meshMats[j].DOFloat(0f, "_RimPower", 0.1f).OnComplete(delegate
				{
					for (int k = 0; k < meshMats.Length; k++)
					{
						meshMats[k].DOFloat(1f, "_RimPower", 0.05f);
						meshMats[k].DOColor(Color.black, "_RimColor", 0.05f);
					}
				});
			}
		});
	}

	public void InvincibleFlashSkin()
	{
		if (meshMats == null)
		{
			SkinnedMeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
			if (componentsInChildren.Length == 0)
			{
				return;
			}
			meshMats = new Material[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				meshMats[i] = componentsInChildren[i].material;
			}
		}
		if (meshMats != null)
		{
			Color color = Color.Lerp(Color.black, Color.gray, invincibleFlashSkinPercent);
			float value = Mathf.Lerp(1f, 0f, invincibleFlashSkinPercent);
			for (int j = 0; j < meshMats.Length; j++)
			{
				meshMats[j].SetColor("_RimColor", color);
				meshMats[j].SetFloat("_RimPower", value);
			}
		}
	}

	public void ResetSkinColor()
	{
		if (meshMats == null)
		{
			SkinnedMeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
			if (componentsInChildren.Length == 0)
			{
				return;
			}
			meshMats = new Material[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				meshMats[i] = componentsInChildren[i].material;
			}
		}
		if (meshMats != null)
		{
			for (int j = 0; j < meshMats.Length; j++)
			{
				meshMats[j].SetColor("_RimColor", Color.black);
				meshMats[j].SetFloat("_RimPower", 1f);
			}
		}
	}

	public virtual void ChangeHPVal(long newHP)
	{
	}

	public virtual void ChangeHPEffect(long newHP, GameDefine.DAMAGEBOARD_TYPE damageType)
	{
	}

	public virtual void ChangeLevel(int level, int combovalue = 0)
	{
	}

	public void UpdateDamgeBoard(GameDefine.DAMAGEBOARD_TYPE type, long cHP)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		string empty = string.Empty;
		switch (type)
		{
		case GameDefine.DAMAGEBOARD_TYPE.PLAYER_ATTACK_MISS:
		case GameDefine.DAMAGEBOARD_TYPE.TARGET_ATTACK_MISS:
			empty = StrDictionary.GetDictionaryString("#{100140}");
			break;
		case GameDefine.DAMAGEBOARD_TYPE.PLAYER_ATTACK_CRITICAL:
		case GameDefine.DAMAGEBOARD_TYPE.TARGET_ATTACK_CRITICAL:
			empty = string.Format("{0}-{1}", StrDictionary.GetDictionaryString("#{100141}"), cHP);
			break;
		case GameDefine.DAMAGEBOARD_TYPE.PLAYER_HP_UP:
			empty = $"+{cHP}";
			break;
		default:
			empty = $"-{cHP}";
			break;
		}
		if (sceneManager.DamageBoardManger != null)
		{
			Vector3 position = base.Position;
			position.y += Mathf.Max(ModelHeight - 2f, 0f);
			sceneManager.DamageBoardManger.ShowDamgaeBoard((int)type, empty, position);
		}
		else
		{
			Debug.Log("sceneManger.DamageBoardManger==null");
		}
	}

	public void UpdateSkillName(GameDefine.DAMAGEBOARD_TYPE type, string name)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.DamageBoardManger != null)
		{
			sceneManager.DamageBoardManger.ShowDamgaeBoard((int)type, name, base.Position);
		}
	}

	public virtual void OnBeaton()
	{
		if (onBeaton != null)
		{
			onBeaton();
		}
		ActiveIdleAttack();
	}

	public virtual void OnSkillEnable(string skillID)
	{
	}

	public virtual void OnSkillDisable(string skillID)
	{
	}

	public virtual void OnSkillFinished()
	{
		EnableNavMeshAgent();
		if (onSkillFinished != null)
		{
			onSkillFinished();
		}
	}

	public virtual void OnSkillUseSuccess(string skillId)
	{
		DisableNavMeshAgent();
		if (mSkillUseSuccessDic.Count != 0 && mSkillUseSuccessDic.ContainsKey(skillId))
		{
			mSkillUseSuccessDic[skillId](skillId);
			mSkillUseSuccessDic.Remove(skillId);
		}
		ActiveIdleAttack();
	}

	public virtual void OnSkillUseFail(string skillId)
	{
	}

	public void ActiveIdleAttack()
	{
		mIdleAttackFlag = true;
		idleAttackTimeCount = idleAttackLastTime;
		mCurAnimationState = GameDefine.ANIMATIONSTATE.IDLE_ATTACK;
	}

	public void DisactiveIdleAttack()
	{
		mIdleAttackFlag = false;
		if (!IsDie && mCurAnimationState == GameDefine.ANIMATIONSTATE.IDLE_ATTACK)
		{
			OnSwithAnimState(GameDefine.ANIMATIONSTATE.IDLE);
		}
	}

	public virtual void UpdateHeadInfo()
	{
		if (mHeadInfoLogic != null)
		{
			mHeadInfoLogic.SetHpVal((float)AttributeData.HP / (float)AttributeData.MaxHP);
		}
	}

	public virtual void RefreshHeadInfo()
	{
	}

	public void DisactiveHeadInfo()
	{
		if (mHeadInfoLogic != null)
		{
			UnityVersionUtil.SetActiveRecursive(mHeadInfoLogic.gameObject, state: false);
		}
	}

	public void ActiveHeadInfo()
	{
		if (CameraController.CurrentViewState != CameraController.CAMERAVIEWSTATE.FIXED_3D && CameraController.CurrentViewState != CameraController.CAMERAVIEWSTATE.FEXED_2_FIXED_3D && mHeadInfoLogic != null)
		{
			UnityVersionUtil.SetActiveRecursive(mHeadInfoLogic.gameObject, state: true);
			mHeadInfoLogic.Init();
		}
	}

	public virtual void Recyle()
	{
		if (mHeadInfoLogic != null)
		{
			ResourcesManager.UnLoadHeadInfoPrefab(mHeadInfoLogic.gameObject);
			mHeadInfoLogic = null;
		}
		if (mSimpleShadow != null)
		{
			ResourcesManager.UnLoadSimpleShadowPrefab(mSimpleShadow.gameObject);
			mSimpleShadow = null;
		}
	}

	public virtual void EnterCombat(ObjCharacter target, SkillUseSuccess onSkillUseSuccess = null)
	{
	}

	public void RegisterOnBeaton(OnBeatonDelegate func)
	{
		onBeaton = (OnBeatonDelegate)Delegate.Combine(onBeaton, func);
	}

	public void DeRegisterOnBeaton(OnBeatonDelegate func)
	{
		if (onBeaton != null)
		{
			onBeaton = (OnBeatonDelegate)Delegate.Remove(onBeaton, func);
		}
	}

	public void RegisterOnSkillFinished(OnSkillFinishedDelegate func)
	{
		onSkillFinished = (OnSkillFinishedDelegate)Delegate.Combine(onSkillFinished, func);
	}

	public void DeRegisterOnSkillFinished(OnSkillFinishedDelegate func)
	{
		if (onSkillFinished != null)
		{
			onSkillFinished = (OnSkillFinishedDelegate)Delegate.Remove(onSkillFinished, func);
		}
	}

	private void Destroy()
	{
		DeRegisterEvent();
	}

	private void RegisterEvent()
	{
		mBuffLogic.RegisterOnSleep(OnSleep);
		mBuffLogic.RegisterOnSleepDone(OnSleepDone);
		mBuffLogic.RegisterOnStun(OnStun);
		mBuffLogic.RegisterOnStunDone(OnStunDone);
		mBuffLogic.RegisterOnKnockDown(OnKnockDown);
		mBuffLogic.RegisterOnKnockDownDone(OnKnockDownDone);
	}

	private void DeRegisterEvent()
	{
		mBuffLogic.DeRegisterOnSleep(OnSleep);
		mBuffLogic.DeRegisterOnSleepDone(OnSleepDone);
		mBuffLogic.DeRegisterOnStun(OnStun);
		mBuffLogic.DeRegisterOnStunDone(OnStunDone);
		mBuffLogic.DeRegisterOnKnockDown(OnKnockDown);
		mBuffLogic.DeRegisterOnKnockDownDone(OnKnockDownDone);
	}

	public void EnableNavMeshAgent()
	{
		mNavMeshAgent.enabled = true;
	}

	public void DisableNavMeshAgent()
	{
		mNavMeshAgent.enabled = false;
	}

	public float MainPlayerDistance()
	{
		if (mObjType != GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			if (mainPlayer != null)
			{
				return VectorXZ.Distance(base.Position, Singleton<ObjManager>.Instance.MainPlayer.Position);
			}
			return float.MaxValue;
		}
		return 0f;
	}
}
