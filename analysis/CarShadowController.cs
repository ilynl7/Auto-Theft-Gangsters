using UnityEngine;

public class CarShadowController : MonoBehaviour
{
	private Projector mProjecter;

	private Transform mTargetCar;

	private Transform mTransform;

	private float zAngle;

	public void Reset(Transform car)
	{
		mTargetCar = car;
		mTransform = base.transform;
		mProjecter = base.gameObject.GetComponent<Projector>();
	}

	private void Update()
	{
		if (!(mTargetCar != null))
		{
			return;
		}
		mTransform.position = mTargetCar.transform.position + Vector3.up * 2f;
		mTransform.eulerAngles = new Vector3(90f, 0f, 0f - mTargetCar.eulerAngles.y);
		zAngle = Mathf.Clamp(Mathf.Abs((!(mTargetCar.transform.eulerAngles.z > 180f)) ? mTargetCar.transform.eulerAngles.z : (mTargetCar.transform.eulerAngles.z - 360f)), 0f, 90f);
		mProjecter.aspectRatio = Mathf.Lerp(1.3f, 0.5f, zAngle / 90f);
		if (!UnityVersionUtil.IsActive(mTargetCar.gameObject))
		{
			if (mProjecter.enabled)
			{
				mProjecter.enabled = false;
			}
		}
		else if (!mProjecter.enabled)
		{
			mProjecter.enabled = true;
		}
	}
}
