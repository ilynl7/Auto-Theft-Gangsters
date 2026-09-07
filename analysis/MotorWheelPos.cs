using UnityEngine;

public class MotorWheelPos : MonoBehaviour
{
	public Transform LeftWheelTrans;

	public Transform RightWheelTrans;

	private Transform mCacheTrans;

	private Vector3 defaultPos = Vector3.zero;

	private void Start()
	{
		mCacheTrans = base.transform;
		defaultPos = mCacheTrans.localPosition;
	}

	private void Update()
	{
		mCacheTrans.localPosition = new Vector3(defaultPos.x, (LeftWheelTrans.localPosition.y + RightWheelTrans.localPosition.y) / 2f, defaultPos.z);
		mCacheTrans.rotation = LeftWheelTrans.rotation;
	}
}
