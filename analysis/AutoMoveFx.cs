using UnityEngine;

public class AutoMoveFx : MonoBehaviour
{
	private Vector3 mStartPos;

	private Transform mTarget;

	private float mDuartion;

	private float mUseTime;

	private Vector3 mTargetPos;

	private Transform moveObj;

	private void Start()
	{
	}

	private void Update()
	{
		mUseTime += Time.deltaTime;
		float value = mUseTime / mDuartion;
		value = Mathf.Clamp01(value);
		float num = 3f - 12f * (value - 0.5f) * (value - 0.5f);
		if (mTarget != null)
		{
			moveObj.position = Vector3.Lerp(mStartPos, mTarget.position + Vector3.up * 0.5f, value) + Vector3.up * num;
		}
		else
		{
			moveObj.position = Vector3.Lerp(mStartPos, mTargetPos + Vector3.up * 0.5f, value) + Vector3.up * num;
		}
	}

	public void Reset(float duration, Transform target)
	{
		mTarget = target;
		mDuartion = duration * 0.5f;
		mUseTime = 0f;
		moveObj = base.transform.FindChild("shouLei");
		moveObj.transform.localPosition = Vector3.zero;
		mStartPos = moveObj.position;
	}

	public void Reset(float duration, Vector3 targetPos)
	{
		mTarget = null;
		mTargetPos = targetPos;
		mDuartion = duration * 0.5f;
		mUseTime = 0f;
		moveObj = base.transform.FindChild("shouLei");
		moveObj.transform.localPosition = Vector3.zero;
		mStartPos = moveObj.position;
	}
}
