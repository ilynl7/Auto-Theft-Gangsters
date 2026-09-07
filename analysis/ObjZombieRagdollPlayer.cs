using SprotoType;
using UnityEngine;

public class ObjZombieRagdollPlayer : ObjZombiePlayer
{
	public Collider[] BodyCollider;

	public Rigidbody[] BodyRigidbody;

	public Vector3[] BodyPos;

	public Quaternion[] BodyRotation;

	public Transform[] BodyPart;

	public CityPathPointData TargetPathPoint;

	public GameObject FlagObj;

	private bool RagdollFlag;

	public int CheckIndex = -1;

	public CitySimController mCityCtl;

	private string mActivityId = string.Empty;

	private bool mIsMissionNpc;

	private AIData mAIData;

	private string npcId = string.Empty;

	private Vector3 originalPos = Vector3.zero;

	public vp_Timer.Handle recycleHandle = new vp_Timer.Handle();

	private Vector3 flyDir;

	private int mHitManSoundId = 40;

	private float mHitManSoundVolume = 1f;

	private Vector3 preTargetPoint = Vector3.zero;

	private float lastHitTime;

	private vp_Timer.Handle walkHandle = new vp_Timer.Handle();

	public bool IsRagdollEnable => RagdollFlag;

	public string ActivityId => mActivityId;

	public bool IsMissionNpc => mIsMissionNpc;

	public string NpcId => npcId;

	public ObjZombieRagdollPlayer()
	{
		mObjType = GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL;
	}

	public override void Init()
	{
		base.Init();
	}

	public void ResetObjZombieRagdollPlayer(ObjInitPlayerData initData)
	{
		ResetZombiePlayer(initData);
		RagdollFlag = false;
		CapsuleCollider component = base.gameObject.GetComponent<CapsuleCollider>();
		component.isTrigger = true;
		originalPos = initData.mPos;
		mAIData = DataManager.GetAIDataByID(initData.AIID);
		npcId = initData.NpcId;
		mActivityId = DataManager.GetNpcDataByID(npcId).TalkGroup;
		if (string.IsNullOrEmpty(mActivityId))
		{
			mIsMissionNpc = false;
			AttributeData.Camp = GameDefine.CAMP_TYPE.NORMAL_NPC;
		}
		else
		{
			mIsMissionNpc = true;
			AttributeData.Camp = GameDefine.CAMP_TYPE.FUNCTION_NPC;
		}
	}

	public void EnableRagdoll()
	{
		if (!RagdollFlag)
		{
			mAnimationLogic.DisableAnimationLogic();
			DisableNavMeshAgent();
			DisactiveTargetArriveFinish();
			DisactiveHeadInfo();
			RagdollFlag = true;
			for (int i = 0; i < BodyCollider.Length; i++)
			{
				BodyRigidbody[i].isKinematic = false;
				BodyCollider[i].enabled = true;
				BodyRigidbody[i].detectCollisions = true;
			}
			base.IsDie = true;
			recycleHandle.Cancel();
			vp_Timer.In(3f, delegate
			{
				RecycleSelf();
			}, recycleHandle);
			SendServerDieAction();
		}
	}

	public void RecycleSelf()
	{
		recycleHandle.Cancel();
		walkHandle.Cancel();
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
		Singleton<ObjManager>.Instance.RecycleZombieRagdollPlayer(this);
	}

	public override void OnDie()
	{
		base.OnDie();
		recycleHandle.Cancel();
		vp_Timer.In(2.5f, delegate
		{
			RecycleSelf();
		}, recycleHandle);
		SendServerDieAction();
	}

	private void SendServerDieAction()
	{
		local_npc_die.request request = new local_npc_die.request();
		request.npcid = npcId;
		request.x = (long)(base.Position.x * 100f);
		request.z = (long)(base.Position.z * 100f);
		request.type = 0L;
		if (IsRagdollEnable && SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.IMPACT_NPC))
		{
			request.type = 3L;
		}
		NetLogic.GetInstance().Send<Protocol.local_npc_die>(request);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!IsMissionNpc && other.gameObject.layer == LayerMask.NameToLayer("PlayerCar") && NGUITools.GetRoot(other.gameObject).rigidbody.velocity.sqrMagnitude > 4f)
		{
			EnableRagdoll();
			ObjPlayerCar objPlayerCar = null;
			objPlayerCar = Singleton<ObjManager>.Instance.MainPlayerCar;
			if (objPlayerCar != null)
			{
				mHitManSoundVolume = 0.2f + 0.8f * Mathf.Clamp01(objPlayerCar.CurSpeed / 60f);
				SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(mHitManSoundId, mHitManSoundVolume);
			}
		}
	}

	public static void InitRagdollObj(Transform root, ObjZombieRagdollPlayer npc)
	{
		GameObject gameObject = GameObject.Find("PlayerRagdollData");
		if (gameObject == null)
		{
			Debug.Log("No Player Ragdoll Data");
			return;
		}
		PlayerRagdollData component = gameObject.GetComponent<PlayerRagdollData>();
		RagdollInfoData[] ragdollData = component.RagdollData;
		PhysicMaterial ragdollMat = component.RagdollMat;
		Transform transform = null;
		GameObject gameObject2 = null;
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
				gameObject2 = transform.gameObject;
				ref Vector3 reference = ref npc.BodyPos[i];
				reference = transform.localPosition;
				ref Quaternion reference2 = ref npc.BodyRotation[i];
				reference2 = transform.localRotation;
				npc.BodyPart[i] = transform;
				rigidbody = gameObject2.AddComponent<Rigidbody>();
				rigidbody.mass = ragdollData[i].Mass;
				rigidbody.drag = ragdollData[i].Drag;
				rigidbody.angularDrag = ragdollData[i].AngularDrag;
				rigidbody.useGravity = ragdollData[i].UseGravity;
				rigidbody.isKinematic = true;
				rigidbody.detectCollisions = false;
				npc.BodyRigidbody[i] = rigidbody;
				if (ragdollData[i].ColliderType == 0)
				{
					capsuleCollider = gameObject2.AddComponent<CapsuleCollider>();
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
					boxCollider = gameObject2.AddComponent<BoxCollider>();
					boxCollider.center = ragdollData[i].ColliderCenter;
					boxCollider.size = ragdollData[i].size;
					boxCollider.material = ragdollMat;
					boxCollider.enabled = false;
					npc.BodyCollider[i] = boxCollider;
				}
				else if (ragdollData[i].ColliderType == 2)
				{
					sphereCollider = gameObject2.AddComponent<SphereCollider>();
					sphereCollider.center = ragdollData[i].ColliderCenter;
					sphereCollider.radius = ragdollData[i].ColliderRadius;
					sphereCollider.material = ragdollMat;
					sphereCollider.enabled = false;
					npc.BodyCollider[i] = sphereCollider;
				}
				if (ragdollData[i].HasJoint)
				{
					characterJoint = gameObject2.AddComponent<CharacterJoint>();
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
			else
			{
				Debug.Log(root.gameObject.name + " :: Can't find :: " + ragdollData[i].Path);
			}
		}
	}

	public void ResetCityMove(CitySimController cityCtl, CityPathPointData targetPoint)
	{
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
		WalkMoveTo(preTargetPoint, 1f, MoveToNextCityPoint);
	}

	public void MoveToNextCityPoint(ObjCharacter obj)
	{
		if (TargetPathPoint == null)
		{
			return;
		}
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
			num = ((targetPathPoint.LinkPointIndex[2] == -1) ? targetPathPoint.LinkPointIndex[3] : targetPathPoint.LinkPointIndex[2]);
			if (!mCityCtl.PointDataList[num].IsWalkable)
			{
				num = ((targetPathPoint.LinkPointIndex[1] == -1) ? targetPathPoint.LinkPointIndex[0] : targetPathPoint.LinkPointIndex[1]);
			}
			break;
		case 3:
			num = ((targetPathPoint.LinkPointIndex[3] == -1) ? targetPathPoint.LinkPointIndex[2] : targetPathPoint.LinkPointIndex[3]);
			if (!mCityCtl.PointDataList[num].IsWalkable)
			{
				num = ((targetPathPoint.LinkPointIndex[1] == -1) ? targetPathPoint.LinkPointIndex[0] : targetPathPoint.LinkPointIndex[1]);
			}
			break;
		}
		if (TargetPathPoint.IsCross && mCityCtl.PointDataList[num].IsCross && CitySimController.CurRoadState != ROAD_STATE.PERSON_PASS1 && CitySimController.CurRoadState != ROAD_STATE.PERSON_PASS2)
		{
			return;
		}
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

	private void Update()
	{
		UpdateComponent();
		UpdateMove();
		UpdateSkillCD();
		UpdateHoldTime();
		UpdateComboTime();
		base.SkillLogic.UpdateSkill();
		ZombieAICheck();
	}

	public override void OnBeaton()
	{
		base.OnBeaton();
		lastHitTime = Time.time;
		EnterAttackChase();
	}

	private void ZombieAICheck()
	{
		if (!IsMissionNpc && !base.IsDie && mIsAutoFight)
		{
			if (Time.time - lastHitTime > 3f)
			{
				LeaveAttackChase();
			}
			else
			{
				AutoFight();
			}
		}
	}

	private void EnterAttackChase()
	{
		if (!IsMissionNpc)
		{
			mIsAutoFight = true;
			walkHandle.Cancel();
		}
	}

	private void LeaveAttackChase()
	{
		if (!base.SkillLogic.IsUsingSkill)
		{
			mIsAutoFight = false;
			ReturnOriginalPos();
		}
	}

	private void ReturnOriginalPos()
	{
		WalkMoveTo(originalPos, 1f, PatrolMove);
	}

	public void PatrolMove(ObjCharacter cha)
	{
		Vector3 mTarget = Vector3.zero;
		float x = Random.Range(mAIData.PatrolDistanceMeter * -1f, mAIData.PatrolDistanceMeter);
		float z = Random.Range(mAIData.PatrolDistanceMeter * -1f, mAIData.PatrolDistanceMeter);
		Vector3 vector = originalPos + new Vector3(x, 0f, z);
		if (NavMesh.SamplePosition(vector, out var _, 0.1f, 15))
		{
			mTarget = vector;
		}
		else
		{
			NavMeshHit hit2 = default(NavMeshHit);
			NavMesh.Raycast(originalPos, vector, out hit2, base.NavMeshAgent.walkableMask);
			vector = hit2.position;
			mTarget = vector;
		}
		vp_Timer.In(Random.Range(0.5f, 3f), delegate
		{
			WalkMoveTo(mTarget, 1f, PatrolMove);
		}, walkHandle);
	}

	public override void ChangeHPEffect(long newHP, GameDefine.DAMAGEBOARD_TYPE type)
	{
		if (!base.IsDie)
		{
			OnBeaton();
			long num = AttributeData.HP - newHP;
			if (num > 0)
			{
				UpdateDamgeBoard(type, num);
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
			UpdateHeadInfo();
			if (AttributeData.HP <= 0)
			{
				OnDie();
			}
			mReceiveBiggerHpTimeCount = Time.time;
		}
	}

	public void ShowAcitvityDialog()
	{
		if (SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OptionDialogUI, OnShowOptionDialog);
		}
		FaceToPub(Singleton<ObjManager>.Instance.MainPlayer.Position);
	}

	private void OnShowOptionDialog(bool isSuccess, object param)
	{
		if (!isSuccess || !SingletonUnity<OptionDialogUILogic>.Exists)
		{
			return;
		}
		SingletonUnity<OptionDialogUILogic>.Instance.ResetOptionDialog(base.PartObjId[0], base.PartObjId[1], base.PartObjId[2], base.PartObjId[3], StrDictionary.GetDictionaryString("#{102098}"), "Yes", "No", mActivityId, delegate(string val)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (playerData.IsFinishDownload)
			{
				DominData dominDataByID = DataManager.GetDominDataByID(val);
				if (dominDataByID.IsOpen == 0)
				{
					NoticeLogic.AddNotifyData("#{103019}");
				}
				else if (playerData.Level < dominDataByID.LevelMin)
				{
					NoticeLogic.AddNotifyData("#{103009}");
				}
				else
				{
					enter_domin_pk_scene.request rpcReq = new enter_domin_pk_scene.request
					{
						id = val
					};
					NetLogic.GetInstance().Send<Protocol.enter_domin_pk_scene>(rpcReq);
				}
			}
			else
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DownLoadResRoot);
			}
		});
	}
}
