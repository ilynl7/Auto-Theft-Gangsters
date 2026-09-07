using System;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
	public float mWalkSpeed = 2f;

	public float mSpeedSmoothing = 10f;

	public float mRrotateSpeed = 500f;

	private float mLockCameraTimer;

	private Vector3 mMoveDirection = Vector3.zero;

	private float mMoveSpeed;

	private bool mIsMoving;

	private bool mIsJoyStickPress;

	private float mVerticalRaw;

	private float mHorizonRaw;

	private ObjMainPlayer mainPlayer;

	private Transform cacheCameraTransform;

	private Vector3 moveDirection;

	private MissionManager missionManager;

	private bool keyboardFlag;

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

	public bool IsJoyStickPress
	{
		get
		{
			return mIsJoyStickPress;
		}
		set
		{
			mIsJoyStickPress = value;
		}
	}

	public float VerticalRaw
	{
		get
		{
			return mVerticalRaw;
		}
		set
		{
			mVerticalRaw = value;
		}
	}

	public float HorizonRaw
	{
		get
		{
			return mHorizonRaw;
		}
		set
		{
			mHorizonRaw = value;
		}
	}

	private void UpdateMove()
	{
		if (mainPlayer == null)
		{
			if (Singleton<ObjManager>.Instance.MainPlayer == null)
			{
				return;
			}
			mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (!IsJoyStickPress && !keyboardFlag)
		{
			if (IsMoving)
			{
				mIsMoving = false;
				mMoveSpeed = 0f;
				mainPlayer.DisactiveTargetArriveFinish();
				mainPlayer.StopMove();
			}
			return;
		}
		if (cacheCameraTransform == null)
		{
			cacheCameraTransform = Camera.main.transform;
			if (cacheCameraTransform == null)
			{
				return;
			}
		}
		mIsMoving = Mathf.Abs(mVerticalRaw) > 0.1f || Mathf.Abs(mHorizonRaw) > 0.1f;
		if (mIsMoving)
		{
			Vector3 vector = cacheCameraTransform.TransformDirection(Vector3.forward);
			vector.y = 0f;
			vector = vector.normalized;
			Vector3 vector2 = new Vector3(vector.z, 0f, 0f - vector.x);
			Vector3 vector3 = mVerticalRaw * vector2 + mHorizonRaw * vector;
			if (vector3 != Vector3.zero)
			{
				if (mMoveSpeed < mWalkSpeed * 0.9f)
				{
					moveDirection = vector3.normalized;
				}
				else
				{
					moveDirection = Vector3.RotateTowards(moveDirection, vector3, mRrotateSpeed * ((float)Math.PI / 180f) * Time.deltaTime, 1000f);
					moveDirection = moveDirection.normalized;
				}
			}
			float t = mSpeedSmoothing * Time.deltaTime;
			float to = mainPlayer.mMoveSpeed;
			mMoveSpeed = Mathf.Lerp(mMoveSpeed, to, t);
			Vector3 vector4 = moveDirection * mMoveSpeed;
			vector4 *= Time.deltaTime;
			Vector3 pos = mainPlayer.transform.localPosition + vector4 * 10f;
			mainPlayer.MoveTo(pos, 0.01f);
			mainPlayer.FollowServerID = -1L;
			mainPlayer.BreakAutoCombatState();
			if (missionManager == null)
			{
				missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			}
			missionManager.StopAutoMoveToMission();
			mainPlayer.IsNeedAutoMountCar = false;
		}
		else
		{
			mMoveSpeed = 0f;
			mainPlayer.DisactiveTargetArriveFinish();
			mainPlayer.StopMove();
		}
	}

	private void Start()
	{
		moveDirection = base.transform.TransformDirection(Vector3.forward);
	}

	private void Update()
	{
		UpdateMove();
	}
}
