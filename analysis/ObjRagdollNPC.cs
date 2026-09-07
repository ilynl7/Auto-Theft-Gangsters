using SprotoType;
using UnityEngine;

public class ObjRagdollNPC : ObjNPC
{
	public Collider[] BodyCollider;

	public Rigidbody[] BodyRigidbody;

	public Vector3[] BodyPos;

	public Quaternion[] BodyRotation;

	public Transform[] BodyPart;

	public CityPathPointData TargetPathPoint;

	private CityPathPointData PreTargetPoint;

	private bool RagdollFlag;

	private bool EnterTriggerFlag;

	public int CheckIndex = -1;

	public CitySimController mCityCtl;

	private bool mIsNormalNpc = true;

	public vp_Timer.Handle recycleHandle = new vp_Timer.Handle();

	private Vector3 flyDir;

	private int mHitManSoundId = 40;

	private float mHitManSoundVolume = 1f;

	private Vector3 preTargetPoint = Vector3.zero;

	public bool IsRagdollEnable => RagdollFlag;

	public bool IsEnterTrigger
	{
		get
		{
			return EnterTriggerFlag;
		}
		set
		{
			EnterTriggerFlag = value;
		}
	}

	public override void Init()
	{
		base.Init();
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
		AttributeData.CurSpeed = initData.npcInfoData.MoveSpeedMeter;
		AttributeData.WalkSpeed = initData.npcInfoData.WalkSpeedMeter;
		mDefaultDialogID = initData.npcInfoData.TalkGroup;
		AddDialogMission();
		InitNavMeshAgent();
		InitNPCHeadInfo();
		InitSkill(mNpcData.SkillList);
		if (mTransform.childCount > 0)
		{
			Transform child = mTransform.GetChild(0);
			child.localScale = Vector3.one * initData.npcInfoData.ModelScale;
		}
		CapsuleCollider component = base.gameObject.GetComponent<CapsuleCollider>();
		if (component != null)
		{
			component.height = base.CurrentCharacterModelData.ModelHeight;
			component.center = Vector3.up * base.CurrentCharacterModelData.ModelHeight / 2f;
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
			else
			{
				if (mAILogic != null)
				{
					Object.Destroy(mAILogic);
					mAILogic = null;
				}
				if (IsMissionNpc())
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
				Object.Destroy(mAILogic);
				mAILogic = null;
			}
			if (IsMissionNpc())
			{
				mNavMeshAgent.enabled = false;
				NavMesh.SamplePosition(initData.mPos, out var hit2, 1f, -1);
				base.Position = hit2.position;
			}
			else
			{
				mNavMeshAgent.enabled = true;
			}
		}
		mIsNormalNpc = true;
		ShowMesh();
		RagdollFlag = false;
	}

	public void RecycleSelf()
	{
		recycleHandle.Cancel();
		mAnimationLogic.EnableAnimationLogic();
		RagdollFlag = false;
		if (BodyCollider != null)
		{
			for (int i = 0; i < BodyCollider.Length; i++)
			{
				BodyCollider[i].enabled = false;
				BodyRigidbody[i].isKinematic = true;
				BodyRigidbody[i].detectCollisions = false;
				BodyPart[i].localPosition = BodyPos[i];
				BodyPart[i].localRotation = BodyRotation[i];
			}
		}
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
		Singleton<ObjManager>.Instance.RecycleRagdollNPC(this);
	}

	public override void OnDie()
	{
		base.OnDie();
		recycleHandle.Cancel();
		vp_Timer.In(2.5f, delegate
		{
			RecycleSelf();
		}, recycleHandle);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (IsMissionNpc() || other.gameObject.layer != LayerMask.NameToLayer("PlayerCar"))
		{
			return;
		}
		IsEnterTrigger = true;
		if (NGUITools.GetRoot(other.gameObject).rigidbody.velocity.sqrMagnitude > 4f)
		{
			ObjPlayerCar objPlayerCar = null;
			objPlayerCar = Singleton<ObjManager>.Instance.MainPlayerCar;
			if (objPlayerCar != null)
			{
				mHitManSoundVolume = 0.2f + 0.8f * Mathf.Clamp01(objPlayerCar.CurSpeed / 60f);
				SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(mHitManSoundId, mHitManSoundVolume);
			}
			else
			{
				objPlayerCar = Singleton<ObjManager>.Instance.MainPlayer.CurPlayerCar;
			}
			int damageByCar = CharacterAttributeData.GetDamageByCar(objPlayerCar, Singleton<ObjManager>.Instance.MainPlayer, this);
			if (damageByCar > 0)
			{
				ChangeHPVal(AttributeData.HP - damageByCar);
			}
			EnableRagdoll();
		}
	}

	public void EnableRagdoll()
	{
		if (RagdollFlag)
		{
			return;
		}
		mAnimationLogic.DisableAnimationLogic();
		DisableNavMeshAgent();
		DisactiveTargetArriveFinish();
		if (base.SimpleShadow != null)
		{
			UnityVersionUtil.SetActiveRecursive(base.SimpleShadow, state: false);
		}
		RagdollFlag = true;
		for (int i = 0; i < BodyCollider.Length; i++)
		{
			BodyRigidbody[i].isKinematic = false;
			BodyCollider[i].enabled = true;
			BodyRigidbody[i].detectCollisions = true;
		}
		if (mHeadInfoLogic != null)
		{
			(mHeadInfoLogic as NPCHeadInfoLogic).HideHpLine();
		}
		base.IsDie = true;
		if (AttributeData.HP <= 0 && GameManager.OnLineState && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsCarScene())
		{
			if (GameManager.IsSupportCurDataVersion145())
			{
				impact_npc.request rpcReq = new impact_npc.request();
				NetLogic.GetInstance().Send<Protocol.impact_npc>(rpcReq);
			}
			recycleHandle.Cancel();
			vp_Timer.In(3f, delegate
			{
				RecycleSelf();
			}, recycleHandle);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!IsMissionNpc() && other.gameObject.layer == LayerMask.NameToLayer("PlayerCar"))
		{
			IsEnterTrigger = false;
			vp_Timer.In(3f, delegate
			{
				ResetGetUpSelf();
			});
		}
	}

	public void ResetGetUpSelf()
	{
		if (!RagdollFlag || IsEnterTrigger || !base.IsDie || AttributeData.HP <= 0)
		{
			return;
		}
		mAnimationLogic.EnableAnimationLogic();
		if (base.SimpleShadow != null)
		{
			UnityVersionUtil.SetActiveRecursive(base.SimpleShadow, state: true);
		}
		RagdollFlag = false;
		base.IsDie = false;
		if (BodyCollider != null)
		{
			if (BodyCollider.Length > 0)
			{
				base.transform.position = BodyCollider[0].transform.position;
			}
			for (int i = 0; i < BodyCollider.Length; i++)
			{
				BodyCollider[i].enabled = false;
				BodyRigidbody[i].isKinematic = true;
				BodyRigidbody[i].detectCollisions = false;
				BodyPart[i].localPosition = BodyPos[i];
				BodyPart[i].localRotation = BodyRotation[i];
			}
		}
		EnableNavMeshAgent();
		WalkMoveTo(preTargetPoint, 1f, MoveToNextCityPoint);
		if (mHeadInfoLogic != null)
		{
			(mHeadInfoLogic as NPCHeadInfoLogic).ShowHpLine();
		}
	}

	public override void ChangeHPVal(long newHP)
	{
		if (IsRagdollEnable)
		{
			return;
		}
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
		else if (newHP <= 0 && !IsCityCaptureNpc())
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

	public new virtual void UpdateHeadInfo()
	{
		if (mHeadInfoLogic != null)
		{
			mHeadInfoLogic.SetHpVal((float)AttributeData.HP / (float)AttributeData.MaxHP);
		}
	}

	public static void InitRagdollObj(Transform root, ObjRagdollNPC npc)
	{
		if (!SingletonUnity<NPCRagdollData>.Exists)
		{
			Debug.Log("No Ragdoll Data");
			return;
		}
		RagdollInfoData[] ragdollData = SingletonUnity<NPCRagdollData>.Instance.RagdollData;
		PhysicMaterial ragdollMat = SingletonUnity<NPCRagdollData>.Instance.RagdollMat;
		Transform transform = null;
		GameObject gameObject = null;
		Rigidbody rigidbody = null;
		CapsuleCollider capsuleCollider = null;
		BoxCollider boxCollider = null;
		SphereCollider sphereCollider = null;
		CharacterJoint characterJoint = null;
		npc.BodyRigidbody = new Rigidbody[ragdollData.Length];
		npc.BodyCollider = new Collider[ragdollData.Length];
		npc.BodyPos = new Vector3[ragdollData.Length];
		npc.BodyRotation = new Quaternion[ragdollData.Length];
		npc.BodyPart = new Transform[ragdollData.Length];
		for (int i = 0; i < ragdollData.Length; i++)
		{
			transform = root.FindChild(ragdollData[i].Path);
			if (transform != null)
			{
				gameObject = transform.gameObject;
				ref Vector3 reference = ref npc.BodyPos[i];
				reference = transform.localPosition;
				ref Quaternion reference2 = ref npc.BodyRotation[i];
				reference2 = transform.localRotation;
				npc.BodyPart[i] = transform;
				rigidbody = gameObject.AddComponent<Rigidbody>();
				rigidbody.mass = ragdollData[i].Mass;
				rigidbody.drag = ragdollData[i].Drag;
				rigidbody.angularDrag = ragdollData[i].AngularDrag;
				rigidbody.useGravity = ragdollData[i].UseGravity;
				rigidbody.isKinematic = true;
				rigidbody.detectCollisions = false;
				npc.BodyRigidbody[i] = rigidbody;
				if (ragdollData[i].ColliderType == 0)
				{
					capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
					capsuleCollider.center = ragdollData[i].ColliderCenter;
					capsuleCollider.radius = ragdollData[i].ColliderRadius;
					capsuleCollider.height = ragdollData[i].Height;
					capsuleCollider.direction = ragdollData[i].Direction;
					capsuleCollider.material = ragdollMat;
					capsuleCollider.enabled = false;
					npc.BodyCollider[i] = capsuleCollider;
				}
				else if (ragdollData[i].ColliderType == 1)
				{
					boxCollider = gameObject.AddComponent<BoxCollider>();
					boxCollider.center = ragdollData[i].ColliderCenter;
					boxCollider.size = ragdollData[i].size;
					boxCollider.material = ragdollMat;
					boxCollider.enabled = false;
					npc.BodyCollider[i] = boxCollider;
				}
				else if (ragdollData[i].ColliderType == 2)
				{
					sphereCollider = gameObject.AddComponent<SphereCollider>();
					sphereCollider.center = ragdollData[i].ColliderCenter;
					sphereCollider.radius = ragdollData[i].ColliderRadius;
					sphereCollider.material = ragdollMat;
					sphereCollider.enabled = false;
					npc.BodyCollider[i] = sphereCollider;
				}
				if (ragdollData[i].HasJoint)
				{
					characterJoint = gameObject.AddComponent<CharacterJoint>();
					characterJoint.connectedBody = root.FindChild(ragdollData[i].ConnectBodyPath).gameObject.rigidbody;
					characterJoint.anchor = ragdollData[i].Anchor;
					characterJoint.axis = ragdollData[i].Axis;
					characterJoint.swingAxis = ragdollData[i].SwingAxis;
					SoftJointLimit lowTwistLimit = default(SoftJointLimit);
					lowTwistLimit.limit = ragdollData[i].LowTwistLimit.Limit;
					lowTwistLimit.bounciness = ragdollData[i].LowTwistLimit.Bounciness;
					lowTwistLimit.spring = ragdollData[i].LowTwistLimit.Spring;
					lowTwistLimit.damper = ragdollData[i].LowTwistLimit.Damper;
					characterJoint.lowTwistLimit = lowTwistLimit;
					SoftJointLimit highTwistLimit = default(SoftJointLimit);
					highTwistLimit.limit = ragdollData[i].HighTwistLimit.Limit;
					highTwistLimit.bounciness = ragdollData[i].HighTwistLimit.Bounciness;
					highTwistLimit.spring = ragdollData[i].HighTwistLimit.Spring;
					highTwistLimit.damper = ragdollData[i].HighTwistLimit.Damper;
					characterJoint.highTwistLimit = highTwistLimit;
					SoftJointLimit swing1Limit = default(SoftJointLimit);
					swing1Limit.limit = ragdollData[i].Swing1Limit.Limit;
					swing1Limit.bounciness = ragdollData[i].Swing1Limit.Bounciness;
					swing1Limit.spring = ragdollData[i].Swing1Limit.Spring;
					swing1Limit.damper = ragdollData[i].Swing1Limit.Damper;
					characterJoint.swing1Limit = swing1Limit;
					SoftJointLimit swing2Limit = default(SoftJointLimit);
					swing2Limit.limit = ragdollData[i].Swing2Limit.Limit;
					swing2Limit.bounciness = ragdollData[i].Swing2Limit.Bounciness;
					swing2Limit.spring = ragdollData[i].Swing2Limit.Spring;
					swing2Limit.damper = ragdollData[i].Swing2Limit.Damper;
					characterJoint.swing2Limit = swing2Limit;
				}
			}
		}
	}

	public void ResetCityMove(CitySimController cityCtl, CityPathPointData targetPoint)
	{
		mIsNormalNpc = false;
		mCityCtl = cityCtl;
		TargetPathPoint = targetPoint;
		int num = 1;
		num = (CitySimController.IsForward(base.Position - targetPoint.PointPos, targetPoint.PointRight) ? 1 : (-1));
		Vector3 pos = targetPoint.PointPos + num * targetPoint.PointRight * Random.Range(targetPoint.MinWalkDis, targetPoint.MaxWalkDis);
		bool flag = false;
		flag = SceneManager.IsInNavmeshArea(pos);
		int num2 = 0;
		while (!flag)
		{
			num2++;
			pos = targetPoint.PointPos + num * targetPoint.PointRight * Random.Range(targetPoint.MinWalkDis - (float)num2, targetPoint.MaxWalkDis - (float)num2);
			flag = SceneManager.IsInNavmeshArea(pos);
		}
		preTargetPoint = pos;
		WalkMoveTo(pos, 5f, MoveToNextCityPoint);
	}

	public void ContinueMove()
	{
		if (!mIsNormalNpc)
		{
			WalkMoveTo(preTargetPoint, 1f, MoveToNextCityPoint);
		}
	}

	public void MoveToNextCityPoint(ObjCharacter obj)
	{
		CityPathPointData targetPathPoint = TargetPathPoint;
		int num = -1;
		int num2 = 0;
		switch ((!targetPathPoint.IsCross) ? Random.Range(0, 2) : Random.Range(0, 4))
		{
		case 0:
			num = ((targetPathPoint.LinkPointIndex[0] == -1) ? targetPathPoint.LinkPointIndex[1] : targetPathPoint.LinkPointIndex[0]);
			if (mCityCtl.PointDataList[num].IsWalkable)
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
			if (mCityCtl.PointDataList[num].IsWalkable)
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
			num = ((targetPathPoint.LinkPointIndex[2] != -1) ? targetPathPoint.LinkPointIndex[2] : ((targetPathPoint.LinkPointIndex[3] == -1) ? targetPathPoint.LinkPointIndex[0] : targetPathPoint.LinkPointIndex[3]));
			if (!mCityCtl.PointDataList[num].IsWalkable)
			{
				num = ((targetPathPoint.LinkPointIndex[1] == -1) ? targetPathPoint.LinkPointIndex[0] : targetPathPoint.LinkPointIndex[1]);
			}
			break;
		case 3:
			num = ((targetPathPoint.LinkPointIndex[3] != -1) ? targetPathPoint.LinkPointIndex[3] : ((targetPathPoint.LinkPointIndex[2] == -1) ? targetPathPoint.LinkPointIndex[0] : targetPathPoint.LinkPointIndex[2]));
			if (!mCityCtl.PointDataList[num].IsWalkable)
			{
				num = ((targetPathPoint.LinkPointIndex[1] == -1) ? targetPathPoint.LinkPointIndex[0] : targetPathPoint.LinkPointIndex[1]);
			}
			break;
		}
		if (PreTargetPoint != null && PreTargetPoint.IsCross && targetPathPoint.IsCross)
		{
			num = targetPathPoint.LinkPointIndex[0];
		}
		if (TargetPathPoint.IsCross && mCityCtl.PointDataList[num].IsCross && CitySimController.CurRoadState != ROAD_STATE.PERSON_PASS1 && CitySimController.CurRoadState != ROAD_STATE.PERSON_PASS2)
		{
			return;
		}
		PreTargetPoint = TargetPathPoint;
		TargetPathPoint = mCityCtl.PointDataList[num];
		Vector3 pos;
		if (CitySimController.IsForward(base.Position - TargetPathPoint.PointPos, TargetPathPoint.PointRight))
		{
			pos = TargetPathPoint.PointPos + TargetPathPoint.PointRight * Random.Range(TargetPathPoint.MinWalkDis, TargetPathPoint.MaxWalkDis);
			bool flag = false;
			flag = SceneManager.IsInNavmeshArea(pos);
			int num3 = 0;
			while (!flag)
			{
				num3++;
				pos = TargetPathPoint.PointPos + TargetPathPoint.PointRight * Random.Range(TargetPathPoint.MinWalkDis - (float)num3, TargetPathPoint.MaxWalkDis - (float)num3);
				flag = SceneManager.IsInNavmeshArea(pos);
			}
		}
		else
		{
			pos = TargetPathPoint.PointPos - TargetPathPoint.PointRight * Random.Range(TargetPathPoint.MinWalkDis, TargetPathPoint.MaxWalkDis);
			bool flag2 = false;
			flag2 = SceneManager.IsInNavmeshArea(pos);
			int num4 = 0;
			while (!flag2)
			{
				num4++;
				pos = TargetPathPoint.PointPos + TargetPathPoint.PointRight * Random.Range(TargetPathPoint.MinWalkDis - (float)num4, TargetPathPoint.MaxWalkDis - (float)num4);
				flag2 = SceneManager.IsInNavmeshArea(pos);
			}
		}
		preTargetPoint = pos;
		WalkMoveTo(pos, 5f, MoveToNextCityPoint);
	}

	public override void StopMove()
	{
		if (!base.IsDie)
		{
			base.StopMove();
		}
	}
}
