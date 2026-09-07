using System;
using UnityEngine;

public class CarControl : MonoBehaviour
{
	public bool IsAutoDrive;

	public AnimationCurve torqueCurve;

	public AnimationCurve steerCurve;

	public AnimationCurve steerBtnPressCurve;

	private float steerBtnPressTime;

	public float maxSpeed = 40f;

	public float maxSteerAngle = 10f;

	public float maxAcceleration = 10f;

	public float brakeAcceleration = 40f;

	private float topAcceleration = -2f;

	private float backwardAcceleration = -2f;

	private float backwardMaxSpeed = -5f;

	private bool brakeKey;

	private bool accelKey;

	private float carSteer;

	private bool carNitro;

	private float steerAngle;

	private float speed;

	private float wheelBase = 3f;

	private Vector3 runningForward;

	private float acceleration;

	private bool mIsUsingGravity;

	private bool leftPress;

	private bool rightPress;

	private bool mOnTheGroundFlag;

	public float InputSteer => carSteer;

	public bool IsBarking => brakeKey;

	public bool OnTheGroundFlag
	{
		get
		{
			return mOnTheGroundFlag;
		}
		set
		{
			mOnTheGroundFlag = value;
		}
	}

	public float CurSpeed => speed;

	public float CurSteerAngle => steerAngle;

	public float MaxSpeed
	{
		get
		{
			return maxSpeed;
		}
		set
		{
			maxSpeed = value;
		}
	}

	public float CurMaxSteerAngle => steerCurve.Evaluate(Mathf.Abs(speed) / maxSpeed) * maxSteerAngle;

	public float SignCurSteerPercent => steerAngle / CurMaxSteerAngle;

	public float CurSteerPercent => Mathf.Abs(steerAngle) / CurMaxSteerAngle;

	public float CurSpeedPercent => speed / maxSpeed;

	private void Awake()
	{
		mIsUsingGravity = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.StreetRacingMode == 0;
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene())
		{
			mIsUsingGravity = false;
		}
	}

	private void OnDisable()
	{
		speed = 0f;
	}

	public void UpdateInput()
	{
		brakeKey = false;
		accelKey = false;
		carSteer = Input.GetAxis("Horizontal");
		accelKey = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
		brakeKey = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
		carNitro = Input.GetKey(KeyCode.Space);
	}

	private void Steering()
	{
		runningForward = base.transform.forward;
		if (carSteer < float.Epsilon && carSteer > -1E-45f)
		{
			steerAngle = 0f;
			Vector3 angularVelocity = base.rigidbody.angularVelocity;
			base.rigidbody.angularVelocity = new Vector3(angularVelocity.x, 0f, angularVelocity.z);
			return;
		}
		float num = steerCurve.Evaluate(Mathf.Abs(speed) / maxSpeed);
		steerAngle = num * maxSteerAngle * carSteer;
		float num2 = wheelBase / Mathf.Sin(steerAngle * ((float)Math.PI / 180f));
		float num3 = 57.29578f * speed / num2;
		Quaternion quaternion = Quaternion.AngleAxis(num3 * Time.deltaTime, base.transform.up);
		runningForward = quaternion * base.transform.forward;
		base.rigidbody.angularVelocity = Vector3.up * num3 * Time.deltaTime;
	}

	private void AutoSteer()
	{
		if (steerAngle < float.Epsilon && steerAngle > -1E-45f)
		{
			Vector3 angularVelocity = base.rigidbody.angularVelocity;
			base.rigidbody.angularVelocity = new Vector3(angularVelocity.x, 0f, angularVelocity.z);
			return;
		}
		float num = wheelBase / Mathf.Sin(steerAngle * ((float)Math.PI / 180f)) * 0.5f;
		float num2 = 57.29578f * speed / num;
		Quaternion quaternion = Quaternion.AngleAxis(num2 * Time.deltaTime, base.transform.up);
		runningForward = quaternion * base.transform.forward;
		base.rigidbody.angularVelocity = Vector3.up * num2 * Time.deltaTime;
	}

	private void Steering2()
	{
		runningForward = base.transform.forward;
		if (carSteer < float.Epsilon && carSteer > -1E-45f)
		{
			steerAngle = 0f;
			return;
		}
		float num = steerCurve.Evaluate(speed / maxSpeed);
		steerAngle = num * maxSteerAngle * carSteer;
		float num2 = wheelBase / Mathf.Sin(steerAngle * ((float)Math.PI / 180f));
		float num3 = 57.29578f * speed / num2;
		Quaternion quaternion = Quaternion.AngleAxis(num3 * Time.deltaTime, base.transform.up);
		base.transform.forward = quaternion * base.transform.forward;
		runningForward = base.transform.forward;
	}

	private void Driving()
	{
		if (brakeKey)
		{
			if (speed > float.Epsilon)
			{
				acceleration = 0f - brakeAcceleration;
			}
			else if (speed < backwardMaxSpeed)
			{
				acceleration = 0f - topAcceleration;
			}
			else
			{
				acceleration = backwardAcceleration;
			}
		}
		else if (accelKey)
		{
			if (speed > float.Epsilon)
			{
				if (speed > maxSpeed)
				{
					acceleration = topAcceleration;
				}
				else
				{
					acceleration = maxAcceleration * torqueCurve.Evaluate(speed / maxSpeed);
				}
			}
			else
			{
				acceleration = brakeAcceleration;
			}
		}
		else
		{
			acceleration = 0f;
		}
		float num = speed + acceleration * Time.deltaTime;
		if (num > maxSpeed * 1.05f)
		{
			num = maxSpeed * 1.05f;
		}
		Vector3 vector = runningForward * num;
		base.rigidbody.velocity = new Vector3(vector.x, base.rigidbody.velocity.y, vector.z);
	}

	private void Update()
	{
		if (!IsAutoDrive)
		{
			if (mIsUsingGravity)
			{
				carSteer = Mathf.Clamp((0f - Input.acceleration.y) * 2f, -1f, 1f);
			}
			else if (leftPress)
			{
				steerBtnPressTime += Time.deltaTime;
				carSteer = 0f - steerBtnPressCurve.Evaluate(steerBtnPressTime);
			}
			else if (rightPress)
			{
				steerBtnPressTime += Time.deltaTime;
				carSteer = steerBtnPressCurve.Evaluate(steerBtnPressTime);
			}
			else
			{
				carSteer = 0f;
			}
		}
	}

	private void FixedUpdate()
	{
		speed = base.rigidbody.velocity.magnitude;
		if (base.transform.InverseTransformDirection(base.rigidbody.velocity).z < 0f)
		{
			speed = 0f - speed;
		}
		if (OnTheGroundFlag)
		{
			if (IsAutoDrive)
			{
				AutoSteer();
			}
			else
			{
				Steering();
			}
			Driving();
		}
	}

	public void OnPressAccelBtn(bool isPress)
	{
		accelKey = isPress;
	}

	public void OnPressBrakeBtn(bool isPress)
	{
		brakeKey = isPress;
	}

	public void OnPressLeftBtn(bool isPress)
	{
		leftPress = isPress;
		steerBtnPressTime = 0f;
	}

	public void OnPressRightBtn(bool isPress)
	{
		rightPress = isPress;
		steerBtnPressTime = 0f;
	}

	public void SetCarTargetAngle(float angle)
	{
		steerAngle = angle;
		if (steerAngle > CurMaxSteerAngle)
		{
			steerAngle = CurMaxSteerAngle;
		}
		else if (steerAngle < 0f - CurMaxSteerAngle)
		{
			steerAngle = 0f - CurMaxSteerAngle;
		}
	}
}
