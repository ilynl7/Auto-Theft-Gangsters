using UnityEngine;

public class ObjAICar : ObjCar
{
	private ObjPlayerCar mPlayerCar;

	private Vector3 mCurTargetPos;

	private Transform mCurTarget;

	private float mTargetSpeed;

	private bool mFollowPathFlag = true;

	private float mCurrentAngle;

	private bool ChaseDoneFlag;

	private bool mBackWardDriving;

	private float mRayDetectDis = 20f;

	private float mSideDetectDis = 10f;

	private bool mFrontRayDetect;

	private float mFrontDistance;

	private float mLeftDistance;

	private float mRightDistance;

	private Transform mLeftFrontRayPos;

	private Transform mRightFrontRayPos;

	private Transform mFrontLeftRayPos;

	private Transform mFrontRightRayPos;

	public LayerMask RayDetectLayer;

	private RaycastHit hitInfo;

	private bool mBackDoneFlag = true;

	private float mCurSpeed => mCarControl.CurSpeed;

	private float mMaxSpeed => mCarControl.MaxSpeed;

	private float mCurMaxSteerAngle => mCarControl.CurMaxSteerAngle;

	private ObjAICar()
	{
		mObjType = GameDefine.OBJ_TYPE.OBJ_NPC_CAR;
	}

	protected new void Init()
	{
		base.Init();
		mLeftFrontRayPos = base.transform.Find("ModelRoot/LeftFrontRayPos");
		mRightFrontRayPos = base.transform.Find("ModelRoot/RightFrontRayPos");
		mFrontLeftRayPos = base.transform.Find("ModelRoot/FrontLeftRayPos");
		mFrontRightRayPos = base.transform.Find("ModelRoot/FrontRightRayPos");
		mCarControl.IsAutoDrive = true;
		RayDetectLayer = 67108864;
		mCarControl.MaxSpeed = 60f;
		mCarControl.maxAcceleration = 20f;
		mCarControl.maxSteerAngle = 25f;
	}

	private void Awake()
	{
		Init();
	}

	public void Reset(ObjCarInitData initData)
	{
		Reset();
		base.CacheTransform.position = initData.Pos;
		base.CacheTransform.eulerAngles = initData.Angle;
		DisableCar();
	}

	public void SetPath(CarPath path, int curIndex)
	{
		mPath = path;
		mCurPathIndex = curIndex;
		mCurTargetPos = mPath.PathPointList[mCurPathIndex].transform.position;
		mTargetSpeed = mPath.PathPointList[mCurPathIndex].Speed;
		mPlayerCar = Singleton<ObjManager>.Instance.MainPlayerCar;
	}

	private bool CheckArriveTarget()
	{
		if ((mCurTargetPos - base.transform.position).sqrMagnitude < 100f)
		{
			return true;
		}
		return false;
	}

	private void OnArriveTraget()
	{
		int curPathIndex = mPlayerCar.GetCurPathIndex();
		if (mCurPathIndex == curPathIndex)
		{
			mFollowPathFlag = false;
			mCurTargetPos = mPlayerCar.Position + mPlayerCar.CacheTransform.forward * 20f;
			mTargetSpeed = 2.1474836E+09f;
			mCurTarget = mPlayerCar.CacheTransform;
		}
		else if (mCurPathIndex < mPath.PathPointList.Count - 1)
		{
			mCurPathIndex++;
			if (mCurPathIndex == curPathIndex)
			{
				mFollowPathFlag = false;
				mCurTargetPos = mPlayerCar.Position + mPlayerCar.CacheTransform.forward * 20f;
				mTargetSpeed = 2.1474836E+09f;
				mCurTarget = mPlayerCar.CacheTransform;
				return;
			}
			mFollowPathFlag = true;
			mCurTargetPos = mPath.PathPointList[mCurPathIndex].transform.position;
			if (mCurPathIndex - 1 >= 0)
			{
				mTargetSpeed = mPath.PathPointList[mCurPathIndex - 1].Speed;
			}
			mCurTarget = mPath.PathPointList[mCurPathIndex].transform;
		}
		else
		{
			Debug.Log("No Path Point Left");
		}
	}

	protected new void FixedUpdate()
	{
		UpdateMove();
	}

	private new void UpdateMove()
	{
		if (ChaseDoneFlag)
		{
			return;
		}
		if (!mFollowPathFlag)
		{
			mCurTargetPos = mPlayerCar.Position + mPlayerCar.CacheTransform.forward * mPlayerCar.CurSpeed;
			mTargetSpeed = 2.1474836E+09f;
			mCurTarget = mPlayerCar.CacheTransform;
			if ((mCurTargetPos - mTransform.position).sqrMagnitude <= 64f)
			{
				ChaseDoneFlag = true;
				mTargetSpeed = 0f;
			}
		}
		else if (CheckArriveTarget())
		{
			OnArriveTraget();
		}
		Vector3 vector = mCurTargetPos - base.CacheTransform.position;
		Vector3 vector2 = base.CacheTransform.InverseTransformPoint(mCurTargetPos);
		float num = ObstacleAvoidanceSteering();
		mRayDetectDis = 5f;
		if (!mFrontRayDetect)
		{
			num = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
		}
		if (mCurrentAngle * num < 0f)
		{
			mCurrentAngle = 0f;
		}
		if (mCurrentAngle < num)
		{
			mCurrentAngle += Time.fixedDeltaTime * 40f;
			if (mCurrentAngle > num)
			{
				mCurrentAngle = num;
			}
		}
		else if (mCurrentAngle > num)
		{
			mCurrentAngle -= Time.fixedDeltaTime * 40f;
			if (mCurrentAngle < num)
			{
				mCurrentAngle = num;
			}
		}
		mCarControl.SetCarTargetAngle(mCurrentAngle);
		if (mBackWardDriving)
		{
			mCarControl.OnPressAccelBtn(isPress: false);
			mCarControl.OnPressBrakeBtn(isPress: true);
		}
		else if (mFrontDistance < mRayDetectDis * 0.5f && mCurSpeed > 10f)
		{
			mCarControl.OnPressAccelBtn(isPress: false);
			mCarControl.OnPressBrakeBtn(isPress: true);
		}
		else if (mCurSpeed < mTargetSpeed)
		{
			mCarControl.OnPressBrakeBtn(isPress: false);
			mCarControl.OnPressAccelBtn(isPress: true);
		}
		else
		{
			mCarControl.OnPressAccelBtn(isPress: false);
			mCarControl.OnPressBrakeBtn(isPress: true);
		}
	}

	private float ObstacleAvoidanceSteering()
	{
		mFrontRayDetect = false;
		mFrontDistance = mRayDetectDis;
		mLeftDistance = mSideDetectDis;
		mRightDistance = mSideDetectDis;
		if (Physics.Raycast(mFrontLeftRayPos.position, mFrontLeftRayPos.forward, out hitInfo, mRayDetectDis, RayDetectLayer))
		{
			mFrontRayDetect = true;
			if (mFrontDistance > hitInfo.distance)
			{
				mFrontDistance = hitInfo.distance;
			}
			if (hitInfo.distance < mLeftDistance)
			{
				mLeftDistance = hitInfo.distance;
			}
		}
		if (Physics.Raycast(mFrontRightRayPos.position, mFrontRightRayPos.forward, out hitInfo, mRayDetectDis, RayDetectLayer))
		{
			mFrontRayDetect = true;
			if (mFrontDistance > hitInfo.distance)
			{
				mFrontDistance = hitInfo.distance;
			}
			if (hitInfo.distance < mRightDistance)
			{
				mRightDistance = hitInfo.distance;
			}
		}
		if (Physics.Raycast(mLeftFrontRayPos.position, mLeftFrontRayPos.forward, out hitInfo, mSideDetectDis, RayDetectLayer))
		{
			if (mLeftDistance > hitInfo.distance)
			{
				mLeftDistance = hitInfo.distance;
			}
			if (mLeftDistance < mSideDetectDis / 2f)
			{
				mFrontRayDetect = true;
			}
		}
		if (Physics.Raycast(mRightFrontRayPos.position, mRightFrontRayPos.forward, out hitInfo, mSideDetectDis, RayDetectLayer))
		{
			if (mRightDistance > hitInfo.distance)
			{
				mRightDistance = hitInfo.distance;
			}
			if (mRightDistance < mSideDetectDis / 2f)
			{
				mFrontRayDetect = true;
			}
		}
		float num = SteerDecision(mLeftDistance, mRightDistance, mFrontDistance, mFrontRayDetect, mCurTargetPos);
		if (mBackWardDriving)
		{
			if (mCurSpeed < 0f)
			{
				if (num > 0f)
				{
					num = 0f - mCurMaxSteerAngle;
				}
				else if (num < 0f)
				{
					num = mCurMaxSteerAngle;
				}
			}
			if (mFrontDistance > 4f || mRayDetectDis - mFrontDistance < float.Epsilon)
			{
				mBackWardDriving = false;
			}
		}
		return num;
	}

	private float SteerDecision(float leftDistance, float rightDistance, float frontDistance, bool frontContact, Vector3 targetPos)
	{
		float num = mCurMaxSteerAngle;
		float result = 0f;
		float num2 = 1f;
		float num3 = 1f;
		if (frontContact && frontDistance < 1f)
		{
			mBackWardDriving = true;
		}
		if (!frontContact)
		{
			mBackDoneFlag = true;
		}
		if (!mBackWardDriving)
		{
			if (!mBackDoneFlag)
			{
				if (base.transform.InverseTransformPoint(targetPos).x < 0f)
				{
					return -1f * num;
				}
				return num;
			}
			if (leftDistance > rightDistance || (frontContact && mSideDetectDis - leftDistance < float.Epsilon))
			{
				if (frontContact)
				{
					result = -1f * num;
				}
				else
				{
					if (rightDistance < mSideDetectDis)
					{
						num3 = rightDistance / mSideDetectDis;
					}
					result = -1f * num * (1f - num3);
				}
			}
			else if (leftDistance < rightDistance || (frontContact && mSideDetectDis - rightDistance < float.Epsilon))
			{
				if (frontContact)
				{
					result = num;
				}
				else
				{
					if (leftDistance < mSideDetectDis)
					{
						num2 = leftDistance / mSideDetectDis;
					}
					result = num * (1f - num2);
				}
			}
		}
		else
		{
			mBackDoneFlag = false;
			result = ((!(base.transform.InverseTransformPoint(targetPos).x < 0f)) ? num : (-1f * num));
		}
		return result;
	}
}
