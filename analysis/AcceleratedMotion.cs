using UnityEngine;

public class AcceleratedMotion : MonoBehaviour
{
	public Vector3 mOriginPosition = new Vector3(0f, 0f, 0f);

	public Vector3 mVelocity = new Vector3(0f, 0f, 0f);

	public Vector3 mAcceleration = new Vector3(0f, 0f, 0f);

	private float mStartTime;

	private float mMotionTime;

	private bool mGoing;

	private Vector3 mOriginVelocity = new Vector3(0f, 0f, 0f);

	private Transform mTransform;

	public bool Going => mGoing;

	private void Start()
	{
		mTransform = base.gameObject.transform;
	}

	private void Update()
	{
		if (mGoing)
		{
			Play();
		}
	}

	public void Init(Vector3 vecVelocity, Vector3 vecAcceleration, float fMotionTime)
	{
		mVelocity = vecVelocity;
		mOriginVelocity = vecVelocity;
		mAcceleration = vecAcceleration;
		mMotionTime = fMotionTime;
	}

	public void Go()
	{
		mGoing = true;
		mStartTime = Time.fixedTime;
	}

	private void Play()
	{
		if (Time.fixedTime - mStartTime <= mMotionTime)
		{
			base.gameObject.transform.localPosition += mVelocity * Time.deltaTime;
			mVelocity += mAcceleration * Time.deltaTime;
		}
		else
		{
			mGoing = false;
			mStartTime = 0f;
			mVelocity = mOriginVelocity;
		}
	}
}
