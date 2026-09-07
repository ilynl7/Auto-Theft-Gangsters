using UnityEngine;

public class ObjFakeAICar : MonoBehaviour
{
	public delegate void AICarArriveFinsh(ObjFakeAICar aiCar);

	protected long mServerId;

	public bool IsMoving;

	public string ModelId = string.Empty;

	private NavMeshAgent mNavMeshAgent;

	private float mSpeed = 10f;

	private float mAccSpeed = 5f;

	private CityPathPointData mCurTargetPoint;

	private CityPathPointData mPreTargetPoint;

	private AICarArriveFinsh onArrivePoint;

	private AICarArriveFinsh tempTargetArriveFinish;

	private float mStopRange;

	private Vector3 mTargetPos;

	private Transform mCacheTransform;

	private float mWayDis;

	private ObjPlayerCar mPlayerCar;

	private CitySimController mCityCtl;

	public int CheckIndex = -1;

	private bool stopFlag;

	private Vector3 FLPos;

	private Vector3 FRPos;

	private Vector3 BLPos;

	private Vector3 BRPos;

	public bool IsStaticCar;

	private float carDis1 = 2.3f;

	private float carDis2 = 5.5f;

	private float FourWayCarDis1 = 2.8f;

	private float FourWayCarDis2 = 6.7f;

	private float preAngleY;

	private float steerAngle;

	private float wheelAngle;

	private float[] steerList = new float[6];

	private int curIndex;

	private float mAngleSpeed;

	private vp_Timer.Handle timerHandle = new vp_Timer.Handle();

	public long ServerId
	{
		get
		{
			return mServerId;
		}
		set
		{
			mServerId = value;
		}
	}

	public ObjPlayerCar PlayerCar => mPlayerCar;

	private void Awake()
	{
		mCacheTransform = base.transform;
		mNavMeshAgent = base.gameObject.GetComponent<NavMeshAgent>();
		if (mNavMeshAgent == null)
		{
			mNavMeshAgent = base.gameObject.AddComponent<NavMeshAgent>();
		}
		mNavMeshAgent.speed = mSpeed;
		mNavMeshAgent.acceleration = mAccSpeed;
		mNavMeshAgent.radius = 3f;
		mNavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		mPlayerCar = base.gameObject.GetComponent<ObjPlayerCar>();
		InitCar();
	}

	public void InitCar()
	{
		if (mPlayerCar.MeshRoot != null)
		{
			FLPos = mPlayerCar.FLWheel.wheelTrs.localPosition;
			FRPos = mPlayerCar.FRWheel.wheelTrs.localPosition;
			BLPos = mPlayerCar.BLWheel.wheelTrs.localPosition;
			BRPos = mPlayerCar.BRWheel.wheelTrs.localPosition;
		}
	}

	public void EnableFackAICar()
	{
		mNavMeshAgent.enabled = true;
		base.enabled = true;
	}

	public void DisableFakeAICar()
	{
		StopMove();
		mNavMeshAgent.enabled = false;
		base.enabled = false;
		IsMoving = false;
	}

	public void Reset(CityPathPointData curTargetPoint, CityPathPointData prePoint, float wayDis, CitySimController cityCtl)
	{
		IsStaticCar = false;
		stopFlag = false;
		base.enabled = true;
		mNavMeshAgent.enabled = true;
		mPlayerCar.enabled = false;
		mPlayerCar.rigidbody.isKinematic = false;
		mPlayerCar.IsDie = false;
		mCityCtl = cityCtl;
		UnityVersionUtil.SetActiveRecursive(mPlayerCar.ExplosionParticle.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(mPlayerCar.SmokeParticle.gameObject, state: false);
		mCurTargetPoint = curTargetPoint;
		mPreTargetPoint = prePoint;
		mWayDis = wayDis;
		if (mPlayerCar.DummyNPCPoint != null)
		{
			mPlayerCar.DummyNPCPoint.transform.parent = mPlayerCar.MeshRoot.transform;
			mPlayerCar.DummyNPCPoint.transform.localPosition = mPlayerCar.DefaultNpcPos;
			mPlayerCar.DummyNPCPoint.transform.localRotation = mPlayerCar.DefaultNpcRotation;
		}
		if (mPlayerCar.LLight != null)
		{
			UnityVersionUtil.SetActiveRecursive(mPlayerCar.LLight.gameObject, state: false);
		}
		if (mPlayerCar.RLight != null)
		{
			UnityVersionUtil.SetActiveRecursive(mPlayerCar.RLight.gameObject, state: false);
		}
		mAngleSpeed = mSpeed * 57.29578f / mPlayerCar.FLWheel.WheelRadius / 2f;
		mPlayerCar.MeshRoot.SampleAnimation(mPlayerCar.MeshRoot.animation.GetClip("GTACheBaoZa_Animation"), 0f);
		mPlayerCar.FLWheel.wheelTrs.localPosition = FLPos;
		mPlayerCar.FRWheel.wheelTrs.localPosition = FRPos;
		mPlayerCar.BLWheel.wheelTrs.localPosition = BLPos;
		mPlayerCar.BRWheel.wheelTrs.localPosition = BRPos;
		MoveTo(GetTargetPos(mCurTargetPoint, mPreTargetPoint), 5f, MoveNextPoint);
	}

	public void ResetStaticCar()
	{
		IsStaticCar = true;
		stopFlag = false;
		base.enabled = false;
		mNavMeshAgent.enabled = false;
		mPlayerCar.enabled = false;
		mPlayerCar.rigidbody.isKinematic = false;
		mPlayerCar.IsDie = false;
		mCityCtl = null;
		if (!(mPlayerCar.MeshRoot == null))
		{
			mPlayerCar.ExplosionParticle.Stop();
			mPlayerCar.SmokeParticle.Stop();
			mCurTargetPoint = null;
			mPreTargetPoint = null;
			mWayDis = 0f;
			if (mPlayerCar.DummyNPCPoint != null)
			{
				mPlayerCar.DummyNPCPoint.transform.parent = mPlayerCar.MeshRoot.transform;
				mPlayerCar.DummyNPCPoint.transform.localPosition = mPlayerCar.DefaultNpcPos;
				mPlayerCar.DummyNPCPoint.transform.localRotation = mPlayerCar.DefaultNpcRotation;
			}
			if (mPlayerCar.LLight != null)
			{
				UnityVersionUtil.SetActiveRecursive(mPlayerCar.LLight.gameObject, state: false);
			}
			if (mPlayerCar.RLight != null)
			{
				UnityVersionUtil.SetActiveRecursive(mPlayerCar.RLight.gameObject, state: false);
			}
			mAngleSpeed = mSpeed * 57.29578f / mPlayerCar.FLWheel.WheelRadius / 2f;
			if (mPlayerCar.CurMountData.IsShowPlayer == 0)
			{
				mPlayerCar.MeshRoot.SampleAnimation(mPlayerCar.MeshRoot.animation.GetClip("GTACheBaoZa_Animation"), 0f);
			}
			else
			{
				mPlayerCar.MeshRoot.SampleAnimation(mPlayerCar.MeshRoot.animation.GetClip("GTACheBaoZa_Animation"), 0f);
				mPlayerCar.UpdateColor();
			}
			mPlayerCar.FLWheel.wheelTrs.localPosition = FLPos;
			mPlayerCar.FRWheel.wheelTrs.localPosition = FRPos;
			mPlayerCar.BLWheel.wheelTrs.localPosition = BLPos;
			mPlayerCar.BRWheel.wheelTrs.localPosition = BRPos;
			mPlayerCar.BaseDisableCar();
			mPlayerCar.rigidbody.useGravity = true;
			mPlayerCar.rigidbody.isKinematic = false;
			mPlayerCar.rigidbody.velocity = Vector3.zero;
			mPlayerCar.rigidbody.angularVelocity = Vector3.zero;
			BoxCollider component = mPlayerCar.CarBodyRoot.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.size = new Vector3(mPlayerCar.defaultColliderSize.x, mPlayerCar.defaultColliderSize.y + mPlayerCar.FLWheel.WheelRadius, mPlayerCar.defaultColliderSize.z);
			}
			onArrivePoint = null;
		}
	}

	public void MoveNextPoint(ObjFakeAICar aiCar)
	{
		IsMoving = false;
		if (mPlayerCar.IsDie || !base.enabled || mCurTargetPoint == null || mPreTargetPoint == null)
		{
			return;
		}
		if (mCurTargetPoint.IsCross && !mPreTargetPoint.IsCross)
		{
			if ((mCurTargetPoint.IsNsCross && CitySimController.CurRoadState != 0 && CitySimController.CurRoadState != ROAD_STATE.NS_TURN_PASS) || (!mCurTargetPoint.IsNsCross && CitySimController.CurRoadState != ROAD_STATE.EW_STRAIT_PASS && CitySimController.CurRoadState != ROAD_STATE.EW_TURN_PASS))
			{
				return;
			}
			bool flag = false;
			if ((CitySimController.CurRoadState == ROAD_STATE.EW_STRAIT_PASS || CitySimController.CurRoadState == ROAD_STATE.NS_STRAIT_PASS) ? true : false)
			{
				if (mCurTargetPoint.LinkPointIndex[0] == mPreTargetPoint.SelfIndex)
				{
					mPreTargetPoint = mCurTargetPoint;
					if (mCurTargetPoint.LinkPointIndex[1] != -1)
					{
						mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[1]];
					}
					else if (mWayDis > 3f)
					{
						if (mCurTargetPoint.LinkPointIndex[2] != -1)
						{
							mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[2]];
						}
						else
						{
							mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[3]];
						}
					}
					else if (mCurTargetPoint.LinkPointIndex[3] != -1)
					{
						mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[3]];
					}
					else
					{
						mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[2]];
					}
				}
				else
				{
					mPreTargetPoint = mCurTargetPoint;
					if (mCurTargetPoint.LinkPointIndex[0] != -1)
					{
						mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[0]];
					}
					else if (mWayDis > 3f)
					{
						if (mCurTargetPoint.LinkPointIndex[2] != -1)
						{
							mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[2]];
						}
						else
						{
							mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[3]];
						}
					}
					else if (mCurTargetPoint.LinkPointIndex[3] != -1)
					{
						mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[3]];
					}
					else
					{
						mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[2]];
					}
				}
			}
			else
			{
				if (mWayDis > 3f)
				{
					if (mCurTargetPoint.LinkPointIndex[2] == -1)
					{
						return;
					}
					mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[2]];
				}
				else
				{
					if (mCurTargetPoint.LinkPointIndex[3] == -1)
					{
						return;
					}
					mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[3]];
				}
				mPreTargetPoint = mCurTargetPoint;
			}
		}
		else if (mCurTargetPoint.LinkPointIndex[0] == mPreTargetPoint.SelfIndex)
		{
			mPreTargetPoint = mCurTargetPoint;
			if (mCurTargetPoint.LinkPointIndex[1] != -1)
			{
				mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[1]];
			}
			else if (mWayDis > 3f)
			{
				if (mCurTargetPoint.LinkPointIndex[2] != -1)
				{
					mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[2]];
				}
				else
				{
					mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[3]];
				}
			}
			else if (mCurTargetPoint.LinkPointIndex[3] != -1)
			{
				mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[3]];
			}
			else
			{
				mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[2]];
			}
		}
		else
		{
			mPreTargetPoint = mCurTargetPoint;
			if (mCurTargetPoint.LinkPointIndex[0] == -1)
			{
				return;
			}
			mCurTargetPoint = mCityCtl.PointDataList[mCurTargetPoint.LinkPointIndex[0]];
		}
		MoveTo(GetTargetPos(mCurTargetPoint, mPreTargetPoint), 5f, MoveNextPoint);
	}

	private Vector3 GetTargetPos(CityPathPointData targetPoint, CityPathPointData prePoint)
	{
		if (targetPoint.IsFourLines)
		{
			if (!prePoint.IsFourLines)
			{
				mWayDis = FourWayCarDis2;
			}
		}
		else if (mWayDis > 3f)
		{
			mWayDis = carDis1;
		}
		if (Vector3.Angle(base.transform.forward, targetPoint.PointForward) > 90f)
		{
			return targetPoint.PointPos - targetPoint.PointRight * mWayDis;
		}
		return targetPoint.PointPos + targetPoint.PointRight * mWayDis;
	}

	public void ContinueMove()
	{
		if (!mPlayerCar.IsDie && (!mCurTargetPoint.IsCross || mPreTargetPoint.IsCross || ((!mCurTargetPoint.IsNsCross || CitySimController.CurRoadState == ROAD_STATE.NS_STRAIT_PASS || CitySimController.CurRoadState == ROAD_STATE.NS_TURN_PASS) && (mCurTargetPoint.IsNsCross || CitySimController.CurRoadState == ROAD_STATE.EW_STRAIT_PASS || CitySimController.CurRoadState == ROAD_STATE.EW_TURN_PASS))))
		{
			MoveTo(GetTargetPos(mCurTargetPoint, mPreTargetPoint), 5f, MoveNextPoint);
		}
	}

	public void MoveTo(Vector3 pos, float stopRange = 1f, AICarArriveFinsh arriveFinsh = null)
	{
		if (mPlayerCar.IsDie || stopFlag)
		{
			return;
		}
		onArrivePoint = arriveFinsh;
		mStopRange = stopRange;
		mTargetPos = pos;
		if (CheckArrive(pos))
		{
			StopMove();
			return;
		}
		if (mNavMeshAgent != null && mNavMeshAgent.enabled)
		{
			mNavMeshAgent.acceleration = mAccSpeed;
			mNavMeshAgent.stoppingDistance = 0.1f;
			mNavMeshAgent.SetDestination(mTargetPos);
		}
		IsMoving = true;
	}

	public void DisactiveTargetArriveFinish()
	{
		onArrivePoint = null;
	}

	public void StopMove()
	{
		IsMoving = false;
		if (mNavMeshAgent != null && mNavMeshAgent.enabled)
		{
			mNavMeshAgent.acceleration = mAccSpeed * 6f;
			mNavMeshAgent.Stop();
		}
		if (onArrivePoint != null)
		{
			tempTargetArriveFinish = onArrivePoint;
			onArrivePoint = null;
			tempTargetArriveFinish(this);
		}
	}

	private void Update()
	{
		if (mPlayerCar.IsDie)
		{
			if (base.enabled)
			{
				DisableFakeAICar();
			}
		}
		else if (!(Time.timeScale < float.Epsilon))
		{
			UpdateMove();
		}
	}

	public void UpdateMove()
	{
		if (Time.deltaTime < float.Epsilon)
		{
			return;
		}
		if (IsMoving)
		{
			float num = VectorXZ.Distance(new VectorXZ(mTargetPos.x, mTargetPos.z), new VectorXZ(mCacheTransform.position.x, mCacheTransform.position.z));
			if (num - mStopRange <= 0f)
			{
				StopMove();
				return;
			}
			if (num - mSpeed * Time.deltaTime <= 0f)
			{
				mCacheTransform.position = mTargetPos;
				StopMove();
				return;
			}
			steerAngle = Mathf.Lerp(steerAngle, Mathf.Clamp((0f - Mathf.DeltaAngle(mCacheTransform.eulerAngles.y, preAngleY)) / Time.deltaTime / 4f, -50f, 50f), 0.8f);
			steerList[curIndex] = steerAngle;
			steerAngle = GetAVE();
			curIndex = (curIndex + 1) % steerList.Length;
			wheelAngle += mAngleSpeed * Time.deltaTime;
			mPlayerCar.FLWheel.wheelTrs.localRotation = Quaternion.Euler(new Vector3(wheelAngle, steerAngle, 0f));
			mPlayerCar.FRWheel.wheelTrs.localRotation = Quaternion.Euler(new Vector3(0f - wheelAngle, steerAngle + 180f, 0f));
			mPlayerCar.BLWheel.wheelTrs.localRotation = Quaternion.Euler(new Vector3(wheelAngle, 0f, 0f));
			mPlayerCar.BRWheel.wheelTrs.localRotation = Quaternion.Euler(new Vector3(0f - wheelAngle, 180f, 0f));
			preAngleY = mCacheTransform.eulerAngles.y;
		}
		else
		{
			steerList[curIndex] = 0f;
			steerAngle = GetAVE();
			curIndex = (curIndex + 1) % steerList.Length;
			mPlayerCar.FLWheel.wheelTrs.localRotation = Quaternion.Euler(new Vector3(wheelAngle, steerAngle, 0f));
			mPlayerCar.FRWheel.wheelTrs.localRotation = Quaternion.Euler(new Vector3(wheelAngle, steerAngle + 180f, 0f));
		}
	}

	private float GetAVE()
	{
		float num = 0f;
		for (int i = 0; i < steerList.Length; i++)
		{
			num += steerList[i];
		}
		return num / (float)steerList.Length;
	}

	private bool CheckArrive(Vector3 target)
	{
		float num = Vector3.SqrMagnitude(target - mCacheTransform.position);
		if (num <= mStopRange * mStopRange)
		{
			return true;
		}
		return false;
	}

	public void RecycleSelf()
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
		Singleton<ObjManager>.Instance.RecycleFakeAICar(this);
	}

	private void OnCollisionEnter(Collision other)
	{
		if (base.enabled && other.gameObject.CompareTag("PlayerCar"))
		{
			DisactiveTargetArriveFinish();
			StopMove();
			stopFlag = true;
		}
	}

	private void OnCollisionExit(Collision other)
	{
		if (!base.enabled || !other.gameObject.CompareTag("PlayerCar"))
		{
			return;
		}
		if (timerHandle != null)
		{
			timerHandle.Cancel();
		}
		vp_Timer.In(5f, delegate
		{
			if (stopFlag)
			{
				stopFlag = false;
				if (base.enabled)
				{
					ContinueMove();
				}
			}
		}, timerHandle);
	}

	private void OnDisable()
	{
		timerHandle.Cancel();
	}
}
