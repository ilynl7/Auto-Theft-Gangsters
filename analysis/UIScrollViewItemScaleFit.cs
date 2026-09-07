using UnityEngine;

public class UIScrollViewItemScaleFit : MonoBehaviour
{
	public Transform CenterObj;

	private float mDisOfCenterObj;

	private Transform mTransform;

	private void Start()
	{
		if (mTransform == null)
		{
			mTransform = base.transform;
		}
	}

	private void Update()
	{
		mDisOfCenterObj = Mathf.Abs(CenterObj.InverseTransformPoint(mTransform.position).x);
		mTransform.localScale = Vector3.one * (1f - mDisOfCenterObj / 200f * 0.5f);
	}
}
