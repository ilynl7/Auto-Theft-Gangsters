using UnityEngine;

public class ObjSimpleAICar : ObjCar
{
	private bool PoliceFlag;

	private float mNearDamageTimeCount;

	private float mNearDamageTimeInterval;

	private float mCheckDis = 100f;

	private MountCarMeshRoot mCurMeshData;

	private int mPoliceCarSoundId = 35;

	private Vector3 mCurTargetPos;

	private Transform mCurTarget;

	private float mTargetSpeed;

	private ObjPlayerCar mTargetCar;

	private float mCurrentAngle;

	private bool ChaseDoneFlag;

	private float mRecycleTimeCount;

	private float RECYCLE_TIME = 10f;

	private int mCollideTimeCount;

	private float mCurSpeed => mCarControl.CurSpeed;

	private float mMaxSpeed => mCarControl.MaxSpeed;

	private float mCurMaxSteerAngle => mCarControl.CurMaxSteerAngle;

	public bool IsChaseDone => ChaseDoneFlag;

	private ObjSimpleAICar()
	{
		mObjType = GameDefine.OBJ_TYPE.OBJ_NPC_CAR;
	}

	public void InitMeshData(MountCarMeshRoot meshRootData)
	{
		mCurMeshData = meshRootData;
		FLWheel.wheelTrs = mCurMeshData.QLWheel;
		FLWheel.Init(mCurMeshData.WheelRadius);
		FRWheel.wheelTrs = mCurMeshData.QRWheel;
		FRWheel.Init(mCurMeshData.WheelRadius);
		BLWheel.wheelTrs = mCurMeshData.HLWheel;
		BLWheel.Init(mCurMeshData.WheelRadius);
		BRWheel.wheelTrs = mCurMeshData.HRWheel;
		BRWheel.Init(mCurMeshData.WheelRadius);
		CarBodyRoot = mCurMeshData.CarBodyRoot;
		GameObject gameObject = MeshRoot.transform.FindChild("cheshen").gameObject;
		BoxCollider component = gameObject.GetComponent<BoxCollider>();
		component.size = mCurMeshData.ColliderSize;
		component.center = mCurMeshData.ColliderCenter;
		GameObject gameObject2 = MeshRoot.transform.FindChild("FrontCollision").gameObject;
		gameObject2.transform.localPosition = component.center + Vector3.forward * (component.size.z / 2f + 0.18f);
		GameObject gameObject3 = new GameObject("PlayerCarCollider");
		gameObject3.transform.parent = gameObject.transform.parent;
		gameObject3.transform.localPosition = gameObject.transform.localPosition;
		gameObject3.transform.localRotation = gameObject.transform.localRotation;
		BoxCollider boxCollider = gameObject3.AddComponent<BoxCollider>();
		boxCollider.center = component.center + Vector3.forward * 1.5f / 2f;
		boxCollider.size = component.size + Vector3.forward * 1.5f;
		boxCollider.isTrigger = true;
	}

	protected new void Init()
	{
		base.Init();
		MeshRoot = base.transform.FindChild("MeshRoot").gameObject;
		mCarControl.IsAutoDrive = true;
		mCarControl.MaxSpeed = 50f;
		mCarControl.maxAcceleration = 15f;
		mCarControl.maxSteerAngle = 30f;
		mNearDamageTimeInterval = 1f / (float)ObjPlayerCar.POLICENEAR_DAMAGE;
	}

	private void Awake()
	{
		Init();
	}

	public void Reset(ObjCarInitData initData)
	{
		Reset();
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		base.CacheTransform.position = initData.Pos;
		base.CacheTransform.eulerAngles = initData.Angle;
		base.rigidbody.drag = 0f;
		base.rigidbody.angularDrag = 0f;
		DisableCar();
		ChaseDoneFlag = false;
		mRecycleTimeCount = 0f;
		base.ModelId = initData.CharacterModelId;
		mTargetCar = Singleton<ObjManager>.Instance.MainPlayerCar;
		mCollideTimeCount = 0;
		PoliceFlag = initData.PoliceFlag;
		mNearDamageTimeCount = 0f;
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(mPoliceCarSoundId);
	}

	public void SetPath(CarPath path, int curIndex)
	{
		mPath = path;
		mCurPathIndex = curIndex;
		mCurTargetPos = mPath.PathPointList[mCurPathIndex].transform.position;
		mTargetSpeed = mPath.PathPointList[mCurPathIndex].Speed;
		mCurTarget = mPath.PathPointList[mCurPathIndex].transform;
	}

	private bool CheckArriveTarget()
	{
		if ((mCurTargetPos - base.transform.position).sqrMagnitude < 16f)
		{
			return true;
		}
		return false;
	}

	private void ChaseDone()
	{
		if (!ChaseDoneFlag)
		{
			ChaseDoneFlag = true;
			base.rigidbody.drag = 10f;
			base.rigidbody.angularDrag = 10f;
			Singleton<ObjManager>.Instance.StopPoliceSound();
		}
	}

	private void OnArriveTraget()
	{
		if (mCurPathIndex < mPath.PathPointList.Count - 1)
		{
			mCurPathIndex++;
			mCurTargetPos = mPath.PathPointList[mCurPathIndex].transform.position;
			if (mCurPathIndex - 1 >= 0)
			{
				mTargetSpeed = mPath.PathPointList[mCurPathIndex - 1].Speed;
			}
			mCurTarget = mPath.PathPointList[mCurPathIndex].transform;
		}
		else
		{
			ChaseDone();
		}
	}

	private new void FixedUpdate()
	{
		base.FixedUpdate();
		UpdateMove();
	}

	private new void Update()
	{
		base.Update();
	}

	private new void UpdateMove()
	{
		if (ChaseDoneFlag)
		{
			if (base.CurSpeed > 0f)
			{
				mCarControl.OnPressBrakeBtn(isPress: true);
				mCarControl.OnPressAccelBtn(isPress: false);
			}
			mRecycleTimeCount += Time.fixedDeltaTime;
			if (mRecycleTimeCount > RECYCLE_TIME)
			{
				UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
				Singleton<ObjManager>.Instance.RecycleSimpleAICar(this);
			}
			return;
		}
		if (PoliceFlag && Vector3.SqrMagnitude(mTargetCar.Position - base.Position) < 400f)
		{
			mCurTargetPos = mTargetCar.Position + mTargetCar.CacheTransform.forward * mTargetCar.CurSpeed / 4f;
		}
		else
		{
			if (CheckArriveTarget())
			{
				OnArriveTraget();
			}
			mCurTargetPos = mCurTarget.position;
		}
		Vector3 vector = mCurTargetPos - base.CacheTransform.position;
		Vector3 vector2 = base.CacheTransform.InverseTransformPoint(mCurTargetPos);
		float num = 0f;
		num = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
		if (mCurrentAngle * num < 0f)
		{
			mCurrentAngle = 0f;
		}
		if (mCurrentAngle < num)
		{
			mCurrentAngle += Time.fixedDeltaTime * 15f;
			if (mCurrentAngle > num)
			{
				mCurrentAngle = num;
			}
		}
		else if (mCurrentAngle > num)
		{
			mCurrentAngle -= Time.fixedDeltaTime * 15f;
			if (mCurrentAngle < num)
			{
				mCurrentAngle = num;
			}
		}
		mCarControl.SetCarTargetAngle(mCurrentAngle);
		if (Mathf.Abs(mCurrentAngle) <= mCarControl.CurMaxSteerAngle)
		{
			if (mCurSpeed < mTargetSpeed)
			{
				mCarControl.OnPressBrakeBtn(isPress: false);
				mCarControl.OnPressAccelBtn(isPress: true);
			}
			else if (mCarControl.CurSpeed > 0f)
			{
				mCarControl.OnPressAccelBtn(isPress: false);
				mCarControl.OnPressBrakeBtn(isPress: true);
			}
			else
			{
				mCarControl.OnPressAccelBtn(isPress: false);
				mCarControl.OnPressBrakeBtn(isPress: false);
			}
		}
		else
		{
			mCarControl.OnPressAccelBtn(isPress: false);
			mCarControl.OnPressBrakeBtn(isPress: true);
		}
	}

	private void CheckPlayerCarDistance()
	{
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("PlayerCar"))
		{
			mCollideTimeCount++;
			if (mCollideTimeCount >= 1)
			{
				ChaseDone();
			}
		}
	}

	private void OnDisable()
	{
		ChaseDone();
	}
}
