using UnityEngine;

public class UIMyCenterOnChild : MonoBehaviour
{
	public CENTERONCHILD_CENTERPOS CenterPos;

	public Vector3 CenterOffset = Vector3.zero;

	private UIScrollView mScrollView;

	public float springStrength = 8f;

	public SpringPanel.OnFinished onFinished;

	private void CenterOnNow(Transform target, Vector3 panelCenter)
	{
		if (target != null && mScrollView != null && mScrollView.panel != null)
		{
			Transform cachedTransform = mScrollView.panel.cachedTransform;
			Vector3 vector = cachedTransform.InverseTransformPoint(target.position);
			Vector3 vector2 = cachedTransform.InverseTransformPoint(panelCenter);
			Vector3 vector3 = vector - vector2;
			if (!mScrollView.canMoveHorizontally)
			{
				vector3.x = 0f;
			}
			if (!mScrollView.canMoveVertically)
			{
				vector3.y = 0f;
			}
			vector3.z = 0f;
			Vector3 localPosition = mScrollView.panel.cachedGameObject.transform.localPosition;
			Vector3 vector4 = cachedTransform.localPosition - vector3;
			mScrollView.panel.cachedGameObject.transform.localPosition = vector4;
			Vector3 vector5 = vector4 - localPosition;
			Vector2 clipOffset = mScrollView.panel.clipOffset;
			clipOffset.x -= vector5.x;
			clipOffset.y -= vector5.y;
			mScrollView.panel.clipOffset = clipOffset;
			if (mScrollView != null)
			{
				mScrollView.UpdateScrollbars(recalculateBounds: false);
			}
		}
	}

	public void CenterOn(Transform target)
	{
		if (mScrollView == null)
		{
			mScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		}
		if (mScrollView != null && mScrollView.panel != null)
		{
			Vector3[] worldCorners = mScrollView.panel.worldCorners;
			Vector3 vector = Vector3.zero;
			switch (CenterPos)
			{
			case CENTERONCHILD_CENTERPOS.CENTER:
				vector = (worldCorners[2] + worldCorners[0]) * 0.5f;
				break;
			case CENTERONCHILD_CENTERPOS.LEFT:
				vector = (worldCorners[1] + worldCorners[0]) * 0.5f;
				break;
			case CENTERONCHILD_CENTERPOS.RIGHT:
				vector = (worldCorners[2] + worldCorners[3]) * 0.5f;
				break;
			case CENTERONCHILD_CENTERPOS.TOP:
				vector = (worldCorners[2] + worldCorners[1]) * 0.5f;
				break;
			case CENTERONCHILD_CENTERPOS.BOTTOM:
				vector = (worldCorners[3] + worldCorners[0]) * 0.5f;
				break;
			}
			Vector3 panelCenter = vector + new Vector3(CenterOffset.x * base.transform.lossyScale.x, CenterOffset.y * base.transform.lossyScale.y, CenterOffset.z * base.transform.lossyScale.z);
			CenterOn(target, panelCenter);
		}
	}

	private void CenterOn(Transform target, Vector3 panelCenter)
	{
		if (target != null && mScrollView != null && mScrollView.panel != null)
		{
			Transform cachedTransform = mScrollView.panel.cachedTransform;
			Vector3 vector = cachedTransform.InverseTransformPoint(target.position);
			Vector3 vector2 = cachedTransform.InverseTransformPoint(panelCenter);
			Vector3 vector3 = vector - vector2;
			if (!mScrollView.canMoveHorizontally)
			{
				vector3.x = 0f;
			}
			if (!mScrollView.canMoveVertically)
			{
				vector3.y = 0f;
			}
			vector3.z = 0f;
			SpringPanel.Begin(mScrollView.panel.cachedGameObject, cachedTransform.localPosition - vector3, springStrength).onFinished = onFinished;
		}
	}
}
