using UnityEngine;

public class UISpriteKeepInPanel : MonoBehaviour
{
	public UIPanel mRootPanel;

	public Vector3 mDefaultPos;

	private Transform mCacheTransform;

	public UIWidget widget;

	private Bounds bounds;

	private void Start()
	{
		mRootPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		mCacheTransform = base.transform;
		mDefaultPos = mCacheTransform.localPosition;
		bounds = NGUIMath.CalculateRelativeWidgetBounds(mCacheTransform, mCacheTransform);
	}

	private void Update()
	{
		if (mRootPanel != null)
		{
			mCacheTransform.localPosition = mRootPanel.CalculateConstrainOffset(bounds.min, bounds.max) + mDefaultPos;
		}
	}
}
