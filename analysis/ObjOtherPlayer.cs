using System.Collections.Generic;
using DG.Tweening;
using SprotoType;
using UnityEngine;

public class ObjOtherPlayer : ObjCharacter
{
	public float PLAYER_NAME_DELTA_HIGHT = 0.25f;

	protected ObjCharacter mSelectedTarget;

	private AutoMoveLogic mAutoMoveLogic;

	private GameObject mVisibleRootObj;

	private PROFESSION_TYPE mProfession;

	private string[] mPartObjId = new string[4];

	private BundleManager.LoadModelData[] mLoadingModelData = new BundleManager.LoadModelData[4];

	private long[] mLoadingModelDataId = new long[4];

	private GameObject[] mPartObject = new GameObject[4];

	private Material[] mPartMatList = new Material[4];

	private List<GameObject>[] mPartEffectList = new List<GameObject>[4];

	private long mGuildId = -1L;

	private long mTeamId = -1L;

	private static string NQS_run_naQiang_ActName = "run_naQiang";

	private string mMountId = string.Empty;

	private string mMountColor = string.Empty;

	private ObjPlayerMountCar mMountRoot;

	private bool mIsServerRidingMount;

	private bool mLoadingMountFlag;

	private DanceLogic mDanceLogic;

	private int mServerNotDieNumCount;

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

	public GameObject VisibleRootObj
	{
		get
		{
			return mVisibleRootObj;
		}
		set
		{
			mVisibleRootObj = value;
		}
	}

	public virtual PROFESSION_TYPE Profession
	{
		get
		{
			return mProfession;
		}
		set
		{
			mProfession = value;
		}
	}

	public string[] PartObjId => mPartObjId;

	public BundleManager.LoadModelData[] LoadingModelData => mLoadingModelData;

	public long[] LoadingModelDataId => mLoadingModelDataId;

	public GameObject[] PartObject
	{
		get
		{
			return mPartObject;
		}
		set
		{
			mPartObject = value;
		}
	}

	public Material[] PartMatList => mPartMatList;

	public List<GameObject>[] PartEffectList => mPartEffectList;

	public virtual long GuildId
	{
		get
		{
			return AttributeData.GuildId;
		}
		set
		{
			AttributeData.GuildId = value;
		}
	}

	public virtual long TeamId
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

	public virtual string MountId
	{
		get
		{
			return mMountId;
		}
		set
		{
			mMountId = value;
		}
	}

	public virtual string MountColor
	{
		get
		{
			return mMountColor;
		}
		set
		{
			mMountColor = value;
		}
	}

	public ObjPlayerMountCar MountRoot => mMountRoot;

	public virtual bool IsServerRidingMount
	{
		get
		{
			return mIsServerRidingMount;
		}
		set
		{
			mIsServerRidingMount = value;
		}
	}

	public bool LoadingMountFlag
	{
		get
		{
			return mLoadingMountFlag;
		}
		set
		{
			mLoadingMountFlag = value;
		}
	}

	public ObjOtherPlayer()
	{
		mObjType = GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER;
	}

	public void ClearPartEffect()
	{
		for (int i = 0; i < mPartEffectList.Length; i++)
		{
			if (mPartEffectList[i] != null)
			{
				for (int j = 0; j < mPartEffectList[i].Count; j++)
				{
					Object.Destroy(mPartEffectList[i][j].gameObject);
				}
			}
			mPartEffectList[i] = null;
		}
	}

	public void ReshowPartEffect()
	{
		if (base.ObjType != GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			for (int i = 0; i < mPartObjId.Length; i++)
			{
				if (!string.IsNullOrEmpty(mPartObjId[i]))
				{
					Singleton<ObjManager>.Instance.CheckModelEffect(DataManager.GetModeDataByID(mPartObjId[i]), this);
				}
			}
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsShowFashion)
		{
			if (playerData.CheckWeaponIsSame())
			{
				if (!string.IsNullOrEmpty(playerData.FashionWeaponId))
				{
					Singleton<ObjManager>.Instance.CheckModelEffect(DataManager.GetModeDataByID(playerData.FashionWeaponId), this);
				}
			}
			else if (!string.IsNullOrEmpty(playerData.PartWeaponId))
			{
				Singleton<ObjManager>.Instance.CheckModelEffect(DataManager.GetModeDataByID(playerData.PartWeaponId), this);
			}
			Singleton<ObjManager>.Instance.CheckModelEffect(DataManager.GetModeDataByID(playerData.FashionHeadId), this);
			Singleton<ObjManager>.Instance.CheckModelEffect(DataManager.GetModeDataByID(playerData.FashionLegId), this);
			Singleton<ObjManager>.Instance.CheckModelEffect(DataManager.GetModeDataByID(playerData.FashionBodyId), this);
		}
		else
		{
			if (!string.IsNullOrEmpty(playerData.PartWeaponId))
			{
				Singleton<ObjManager>.Instance.CheckModelEffect(DataManager.GetModeDataByID(playerData.PartWeaponId), this);
			}
			Singleton<ObjManager>.Instance.CheckModelEffect(DataManager.GetModeDataByID(playerData.PartHeadId), this);
			Singleton<ObjManager>.Instance.CheckModelEffect(DataManager.GetModeDataByID(playerData.PartLegId), this);
			Singleton<ObjManager>.Instance.CheckModelEffect(DataManager.GetModeDataByID(playerData.PartBodyId), this);
		}
	}

	private void InitOtherPlayer()
	{
		if (mAutoMoveLogic == null)
		{
			mAutoMoveLogic = base.gameObject.AddComponent<AutoMoveLogic>();
		}
		mAutoMoveLogic.Init(this);
		BonesTrsDict = base.gameObject.GetComponentInChildren<TransDicts>().TransDict;
	}

	public override void Init()
	{
		base.Init();
		mVisibleRootObj = base.CacheTransform.FindChild("ModelRoot").gameObject;
		InitOtherPlayer();
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

	public void ResetOtherPlayer(ObjInitPlayerData playerInitData)
	{
		base.Reset();
		base.Position = playerInitData.mPos;
		base.CacheTransform.forward = playerInitData.mDir;
		ServerId = playerInitData.mServerID;
		Profession = playerInitData.Profession;
		AttributeData.InitData(playerInitData.Attribute, playerInitData.AttributeAll);
		AttributeData.HP = playerInitData.HP;
		AttributeData.Name = playerInitData.Name;
		AttributeData.GuildName = playerInitData.GuildName;
		AttributeData.GuildId = playerInitData.GuildId;
		AttributeData.CurEXP = playerInitData.EXP;
		AttributeData.Level = playerInitData.Level;
		AttributeData.CurSpeed = playerInitData.Speed;
		AttributeData.WalkSpeed = playerInitData.WalkSpeed;
		AttributeData.CurRec = playerInitData.Rec;
		AttributeData.ComboValue = playerInitData.ComboValue;
		AttributeData.Camp = playerInitData.Camp;
		AttributeData.PkMode = playerInitData.PkMode;
		AttributeData.DanceState = playerInitData.DanceState;
		AttributeData.DanceId = playerInitData.DanceId;
		AttributeData.CurTitleLevel = playerInitData.TitleLevel;
		GuildId = playerInitData.GuildId;
		TeamId = playerInitData.TeamId;
		mIsServerRidingMount = playerInitData.visual.mount_state == 1;
		mMountId = playerInitData.visual.MountId;
		mMountColor = playerInitData.visual.mount_color;
		UpdateSkillList(playerInitData.skills);
		InitHeadInfo();
		InitNavMeshAgent();
		if (mAutoMoveLogic != null)
		{
			mAutoMoveLogic.Reset();
		}
		mServerNotDieNumCount = 0;
		mLoadingMountFlag = false;
		if (AttributeData.HP <= 0)
		{
			OnDie();
			mAnimationLogic.ForcePlayAnimation(mAnimationLogic.animationName[3], null, -1f, 0.9f);
		}
		SetVisible(playerInitData.IsVisible);
	}

	public virtual void ReLoadPlayerVisual(characterVisual visual)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		string weaponTypeName = base.WeaponTypeName;
		UpdateWeaponModeID(visual.WeaponId);
		if (visual.HasWeaponItemId)
		{
			UpdateWeaponItemID(visual.WeaponItemId);
		}
		string weaponTypeName2 = base.WeaponTypeName;
		if (visual.showType == 0L)
		{
			empty = visual.WeaponId;
			empty2 = visual.HeadId;
			empty3 = visual.BodyId;
			empty4 = visual.LegId;
		}
		else
		{
			empty = ((!CheckWeaponIsSame(visual)) ? visual.WeaponId : ((!visual.HasFashion_WeaponId) ? visual.WeaponId : visual.Fashion_WeaponId));
			empty2 = ((!visual.HasFashion_HeadId) ? visual.HeadId : visual.Fashion_HeadId);
			empty3 = ((!visual.HasFashion_BodyId) ? visual.BodyId : visual.Fashion_BodyId);
			empty4 = ((!visual.HasFashion_LegId) ? visual.LegId : visual.Fashion_LegId);
		}
		if (UnityVersionUtil.IsActive(VisibleRootObj))
		{
			Singleton<ObjManager>.Instance.LoadPlayerVisual(this, empty, empty2, empty3, empty4);
		}
		else
		{
			TargetPartObjId[1] = empty2;
			TargetPartObjId[2] = empty3;
			TargetPartObjId[3] = empty4;
			TargetPartObjId[0] = empty;
		}
		if (string.IsNullOrEmpty(weaponTypeName) || !weaponTypeName.Equals(weaponTypeName2))
		{
			ChangeWeaponAnimaCheck();
		}
	}

	public bool CheckWeaponIsSame(characterVisual visual)
	{
		if (visual.HasWeaponItemId && visual.HasFashionItemId)
		{
			EquipData equipDataById = DataManager.GetEquipDataById(visual.WeaponItemId);
			EquipData equipDataById2 = DataManager.GetEquipDataById(visual.FashionItemId);
			return equipDataById.WeaponType == equipDataById2.WeaponType;
		}
		if (visual.HasWeaponId && visual.HasFashion_WeaponId)
		{
			if (GameDefine.GetWeaponName(visual.WeaponId).Equals(GameDefine.GetWeaponName(visual.Fashion_WeaponId)))
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public override void UpdatePlayerSpeed()
	{
		if (AttributeData.CurSpeed != mNavMeshAgent.speed)
		{
			mNavMeshAgent.speed = AttributeData.CurSpeed;
			if (MountRoot != null)
			{
				MountRoot.UpdateSpeed();
			}
		}
	}

	public override void UpdateMountSpeed()
	{
		if (MountRoot != null)
		{
			MountRoot.UpdateSpeed();
		}
	}

	public void InitHeadInfo()
	{
		ResourcesManager.LoadHeadInfoPrefab(UIInfo.PlayerHeadInfoUI, "PlayerHeadInfoRoot", LoadOtherPlayerHeadInfo);
	}

	private void LoadOtherPlayerHeadInfo(GameObject obj)
	{
		if (obj != null)
		{
			BillBoard billBoard = obj.GetComponent<BillBoard>();
			if (billBoard == null)
			{
				billBoard = obj.AddComponent<BillBoard>();
			}
			billBoard.enabled = true;
			billBoard.BindObj = base.gameObject;
			billBoard.DeltaHeight = base.CurrentCharacterModelData.ModelHeight + PLAYER_NAME_DELTA_HIGHT;
			PlayerHeadInfoLogic playerHeadInfoLogic = (PlayerHeadInfoLogic)(mHeadInfoLogic = obj.GetComponent<PlayerHeadInfoLogic>());
			bool flag = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerCamp == AttributeData.Camp;
			playerHeadInfoLogic.Reset(isMainPlayer: false, AttributeData.CurTitleLevel, AttributeData.Name, AttributeData.GuildName, AttributeData.Camp, ShowHpLine: true, AttributeData.IsChampionGuild());
		}
	}

	public void SetNormalNameHeight()
	{
		BillBoard component = mHeadInfoLogic.gameObject.GetComponent<BillBoard>();
		component.DeltaHeight = base.CurrentCharacterModelData.ModelHeight + PLAYER_NAME_DELTA_HIGHT;
	}

	public void SetDrivingNameHeight(MountData mountData)
	{
		BillBoard component = mHeadInfoLogic.gameObject.GetComponent<BillBoard>();
		component.DeltaHeight = mountData.NameHeight;
	}

	public override void RefreshHeadInfo()
	{
		if (!base.IsDie && mHeadInfoLogic != null)
		{
			PlayerHeadInfoLogic playerHeadInfoLogic = mHeadInfoLogic as PlayerHeadInfoLogic;
			if (playerHeadInfoLogic != null)
			{
				playerHeadInfoLogic.Refresh(AttributeData.CurTitleLevel, AttributeData.Name, AttributeData.GuildName, AttributeData.IsChampionGuild());
			}
		}
	}

	private void Update()
	{
		UpdateComponent();
		UpdateMove();
		base.SkillLogic.UpdateSkill();
	}

	public override void OnDie()
	{
		if (mObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER && IsDrivingMount())
		{
			DisMountCar();
		}
		base.OnDie();
	}

	public void SetVisible(bool visible)
	{
		UnityVersionUtil.SetActiveRecursive(mVisibleRootObj.gameObject, visible);
		if (visible)
		{
			for (int i = 0; i < mPartMatList.Length; i++)
			{
				if (mPartMatList[i] != null)
				{
					Color color = mPartMatList[i].GetColor("_DefaultColor");
					mPartMatList[i].SetColor("_Color", new Color(color.r, color.g, color.b, 0f));
					mPartMatList[i].DOColor(color, 1.5f);
				}
			}
		}
		update_client_state.request request = new update_client_state.request();
		request.id = mServerId;
		request.state = 1L;
		NetLogic.GetInstance().Send<Protocol.update_client_state>(request);
		if (IsServerRidingMount && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsBigWorld())
		{
			MountCar(MountId, MountColor);
		}
		if (AttributeData.DanceState == 1)
		{
			StartDance(AttributeData.DanceId);
		}
	}

	public bool IsVisible()
	{
		return mVisibleRootObj != null && UnityVersionUtil.IsActive(mVisibleRootObj.gameObject);
	}

	protected override void ChangeRunState()
	{
		if (GetWeaponType() == 2 && mIdleAttackFlag)
		{
			mAnimationLogic.PlayAnimation(NQS_run_naQiang_ActName, null, -1f);
		}
		else
		{
			mAnimationLogic.PlayAnimation(1);
		}
	}

	public virtual bool PlayeSocialDance(string danceId)
	{
		if (mCurPlayerState == PLAYER_STATE.SOCIAL_DANCE)
		{
			return false;
		}
		if (IsDrivingMount())
		{
			DisMountCar();
		}
		else if (mCurPlayerState == PLAYER_STATE.DANCE)
		{
			StopDance();
		}
		base.SkillLogic.BreakCurSkill();
		if (base.IsMoving)
		{
			StopMove();
		}
		mCurPlayerState = PLAYER_STATE.SOCIAL_DANCE;
		SocialDanceData socialDanceDataById = DataManager.GetSocialDanceDataById(danceId);
		if (socialDanceDataById == null)
		{
			return false;
		}
		base.AnimationLogic.PlayAnimation(socialDanceDataById.ActionID, SocialDanceFinish, -1f);
		return true;
	}

	public virtual void SocialDanceFinish()
	{
		if (mCurPlayerState == PLAYER_STATE.SOCIAL_DANCE)
		{
			mCurPlayerState = PLAYER_STATE.NORMAL;
		}
	}

	public void StopSocialDance()
	{
		if (mCurPlayerState == PLAYER_STATE.SOCIAL_DANCE)
		{
			mCurPlayerState = PLAYER_STATE.NORMAL;
			base.CurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
		}
	}

	public bool IsDrivingMount()
	{
		if (base.CurPlayerState == PLAYER_STATE.DRIVING || mLoadingMountFlag)
		{
			return true;
		}
		return false;
	}

	public virtual void MountCar(string mountId, string mountColor)
	{
		if (!IsVisible() || mLoadingMountFlag)
		{
			return;
		}
		if (mCurPlayerState == PLAYER_STATE.DRIVING)
		{
			if (!mountId.Equals(MountId))
			{
				ChangeMount(mountId, mountColor);
			}
			else if (!mountColor.Equals(MountColor))
			{
				MountColor = mountColor;
				MountRoot.ChangeColor(DataManager.GetColorDataById(mountColor));
			}
			return;
		}
		if (mCurPlayerState == PLAYER_STATE.DANCE)
		{
			StopDance();
		}
		else if (mCurPlayerState == PLAYER_STATE.SOCIAL_DANCE)
		{
			StopSocialDance();
		}
		MountId = mountId;
		MountColor = mountColor;
		MountData mountDataById = DataManager.GetMountDataById(MountId);
		RideMountData rideMountData = new RideMountData();
		rideMountData.MountData = mountDataById;
		rideMountData.mColorData = DataManager.GetColorDataById(MountColor);
		rideMountData.Player = this;
		mLoadingMountFlag = true;
		mMountRoot = Singleton<ObjManager>.Instance.GetMountCar(rideMountData);
		mMountRoot.transform.parent = base.CacheTransform;
		mMountRoot.transform.localPosition = Vector3.zero;
		mMountRoot.transform.localRotation = Quaternion.identity;
		UnityVersionUtil.SetActiveRecursive(mMountRoot.gameObject, state: true);
		SetDrivingNameHeight(mountDataById);
		CapsuleCollider component = base.gameObject.GetComponent<CapsuleCollider>();
		component.radius = 1.5f;
	}

	public virtual void DisMountCar()
	{
		if (IsDrivingMount())
		{
			mCurPlayerState = PLAYER_STATE.NORMAL;
			Transform transform = mAnimationLogic.AnimaObj.transform;
			transform.transform.parent = base.CacheTransform;
			transform.transform.localPosition = Vector3.zero;
			transform.transform.localRotation = Quaternion.identity;
			if (base.IsMoving)
			{
				base.CurAnimationState = GameDefine.ANIMATIONSTATE.RUN;
			}
			else
			{
				base.CurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
			}
			mMountRoot.transform.parent = null;
			UnityVersionUtil.SetActiveRecursive(mMountRoot.gameObject, state: false);
			Singleton<ObjManager>.Instance.RecycleMountCar(mMountRoot);
			CapsuleCollider component = base.gameObject.GetComponent<CapsuleCollider>();
			component.radius = 0.35f;
			SetNormalNameHeight();
			mLoadingMountFlag = false;
		}
	}

	public void ChangeMount(string newMountId, string colorId)
	{
		DisMountCar();
		MountCar(newMountId, colorId);
	}

	public virtual void StartDance(string danceId)
	{
		if (!IsVisible() || mCurPlayerState == PLAYER_STATE.DANCE)
		{
			return;
		}
		if (IsDrivingMount())
		{
			DisMountCar();
		}
		base.SkillLogic.BreakCurSkill();
		if (base.IsMoving)
		{
			StopMove();
		}
		mCurPlayerState = PLAYER_STATE.DANCE;
		if (mDanceLogic == null)
		{
			mDanceLogic = base.gameObject.GetComponent<DanceLogic>();
			if (mDanceLogic == null)
			{
				mDanceLogic = base.gameObject.AddComponent<DanceLogic>();
			}
		}
		mDanceLogic.Reset(this);
		mDanceLogic.StartDance(danceId);
	}

	public virtual void StopDance()
	{
		if (mCurPlayerState == PLAYER_STATE.DANCE)
		{
			mDanceLogic.StopDance();
			mCurPlayerState = PLAYER_STATE.NORMAL;
			base.CurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
		}
	}

	public virtual void RemoveDance()
	{
		StopDance();
		Object.Destroy(mDanceLogic);
		mDanceLogic = null;
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
			AttributeData.HP = newHP;
			mReceiveBiggerHpTimeCount = Time.time;
		}
	}

	public override void ChangeHPVal(long newHP)
	{
		if (!base.IsDie)
		{
			AttributeData.HP = newHP;
			UpdateHeadInfo();
			if (AttributeData.HP <= 0)
			{
				OnDie();
			}
		}
		else if (newHP > 0)
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
		RefreshHeadInfo();
		base.IsLocalDrivingCar = false;
		base.CurPlayerCar = null;
	}

	public void RecycleUnloadModelBundle()
	{
		for (int i = 0; i < LoadingModelData.Length; i++)
		{
			if (LoadingModelData[i] != null)
			{
				LoadingModelData[i].OnLoadFinished = null;
				LoadingModelData[i] = null;
				BundleManager.RemoveFromLoadModelList(LoadingModelDataId[i]);
				LoadingModelDataId[i] = -1L;
			}
		}
		if (PartObject[1] != null)
		{
			BundleManager.UnloadModel(PartObjId[1], -1L, isMainPlayerUnload: false);
		}
		if (PartObject[0] != null && !string.IsNullOrEmpty(PartObjId[0]))
		{
			BundleManager.UnloadModel(PartObjId[0], -1L, isMainPlayerUnload: false);
		}
		if (PartObject[3] != null)
		{
			BundleManager.UnloadModel(PartObjId[3], -1L, isMainPlayerUnload: false);
		}
		if (PartObject[2] != null)
		{
			BundleManager.UnloadModel(PartObjId[2], -1L, isMainPlayerUnload: false);
		}
	}

	public override void OnStun(BuffInfoData buffInfoData)
	{
		if (mObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER && IsDrivingMount())
		{
			DisMountCar();
		}
		base.OnStun(buffInfoData);
	}

	public override void OnKnockDown(BuffInfoData buffInfoData, ObjCharacter sender)
	{
		if (mObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER && IsDrivingMount())
		{
			DisMountCar();
		}
		base.OnKnockDown(buffInfoData, sender);
	}

	public override void ChangeWeaponAnimaCheck()
	{
		if (base.IsDie)
		{
			base.CurAnimationState = GameDefine.ANIMATIONSTATE.DIE;
		}
		else if (IsDrivingMount())
		{
			ChangeDrivingState();
		}
		else if (mCurPlayerState == PLAYER_STATE.DANCE)
		{
			StopDance();
		}
		else if (mCurPlayerState == PLAYER_STATE.SOCIAL_DANCE)
		{
			StopSocialDance();
		}
		else if (base.IsMoving)
		{
			ChangeRunState();
		}
		else
		{
			base.CurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
		}
	}

	public void CheckBeforeOnCar()
	{
		if (mCurPlayerState == PLAYER_STATE.DANCE)
		{
			StopDance();
		}
		else if (mCurPlayerState == PLAYER_STATE.SOCIAL_DANCE)
		{
			StopSocialDance();
		}
	}
}
