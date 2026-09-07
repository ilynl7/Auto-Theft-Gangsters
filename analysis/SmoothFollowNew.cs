using UnityEngine;

public class SmoothFollowNew : MonoBehaviour
{
	public bool RotateWithSteer;

	public float MaxFOV;

	public float MinFOV;

	public float TargetDeltaHeight = 0.5f;

	public float Height = 0.5f;

	public float RotationDamping = 3f;

	public float HeightDamping = 3f;

	public float Distance = 5f;

	public Camera MainCam;

	private CarControl mCarCtl;

	public Transform mCarTransform;

	public float DiffWantedAngle;

	private Rigidbody mCarRigidbody;

	private float preSpeedPercent;

	private Vector3 mTargetPos;

	private float mWantedAngle;

	private float mWantedHeight;

	private float mCurrentAngle;

	private float mCurrentHeight;

	private Quaternion mCurRotation;

	private Vector3 mCamTempPos;

	private float mCamAngleZ;

	private float mWantedAngleZ;

	private float mCurrentAngleZ;

	private float mAngleZVelocity;

	public void SetTarget(CarControl targetCar)
	{
		mCarCtl = targetCar;
		mCarTransform = targetCar.transform;
		mCarRigidbody = targetCar.rigidbody;
		UpdatePos();
		Update();
	}

	private void Update()
	{
		if (!(mCarTransform == null) && !(mCarRigidbody == null))
		{
			MainCam.fieldOfView = Mathf.Lerp(MinFOV, MaxFOV, Mathf.Lerp(preSpeedPercent, mCarCtl.CurSpeedPercent, Time.deltaTime * 2f));
			preSpeedPercent = mCarCtl.CurSpeedPercent;
		}
	}

	private void LateUpdate()
	{
		UpdatePos();
	}

	private void UpdatePos()
	{
		if (mCarTransform == null)
		{
			return;
		}
		mTargetPos = mCarTransform.position + Vector3.up * TargetDeltaHeight;
		mWantedAngle = mCarTransform.eulerAngles.y + DiffWantedAngle;
		mWantedHeight = mTargetPos.y + Height;
		mCurrentAngle = base.transform.eulerAngles.y;
		mCurrentHeight = base.transform.position.y;
		mCurrentAngle = Mathf.LerpAngle(mCurrentAngle, mWantedAngle, RotationDamping * Time.deltaTime);
		mCurrentHeight = Mathf.Lerp(mCurrentHeight, mWantedHeight, HeightDamping * Time.deltaTime);
		mCurRotation = Quaternion.Euler(0f, mCurrentAngle, 0f);
		mCamTempPos = mTargetPos - mCurRotation * Vector3.forward * Distance;
		base.transform.position = new Vector3(mCamTempPos.x, mCurrentHeight, mCamTempPos.z);
		base.transform.LookAt(mTargetPos);
		if (RotateWithSteer)
		{
			mWantedAngleZ = 50f * mCarCtl.CurSteerPercent * mCarCtl.CurSpeedPercent;
			mCurrentAngleZ = Mathf.SmoothDampAngle(base.transform.eulerAngles.z, mWantedAngleZ, ref mAngleZVelocity, 0.3f);
			if (!float.IsNaN(mCurrentAngleZ))
			{
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, mCurrentAngleZ);
			}
		}
	}

	public void UpdateToTargetPos()
	{
		mTargetPos = mCarTransform.position + Vector3.up * TargetDeltaHeight;
		mWantedAngle = mCarTransform.eulerAngles.y + DiffWantedAngle;
		mWantedHeight = mTargetPos.y + Height;
		mCurrentAngle = base.transform.eulerAngles.y;
		mCurrentHeight = base.transform.position.y;
		mCurrentAngle = mWantedAngle;
		mCurrentHeight = mWantedHeight;
		mCurRotation = Quaternion.Euler(0f, mCurrentAngle, 0f);
		mCamTempPos = mTargetPos - mCurRotation * Vector3.forward * Distance;
		base.transform.position = new Vector3(mCamTempPos.x, mCurrentHeight, mCamTempPos.z);
		base.transform.LookAt(mTargetPos);
		if (RotateWithSteer)
		{
			mWantedAngleZ = 50f * mCarCtl.CurSteerPercent * mCarCtl.CurSpeedPercent;
			mCurrentAngleZ = Mathf.SmoothDampAngle(base.transform.eulerAngles.z, mWantedAngleZ, ref mAngleZVelocity, 0.3f);
			if (!float.IsNaN(mCurrentAngleZ))
			{
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, mCurrentAngleZ);
			}
		}
	}
}
