using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnChild : MonoBehaviour
{
	public delegate void CenterOnChildDelegate(Transform obj);

	public CenterOnChildDelegate OnCenterOnEvent;

	public float springStrength = 8f;

	public float nextPageThreshold;

	public CENTERONCHILD_CENTERPOS CenterPos;

	public Vector3 CenterOffset = Vector3.zero;

	public SpringPanel.OnFinished onFinished;

	private UIScrollView mScrollView;

	private GameObject mCenteredObject;

	public GameObject centeredObject => mCenteredObject;

	public void RegisterCenterOnEvent(CenterOnChildDelegate func)
	{
		OnCenterOnEvent = (CenterOnChildDelegate)Delegate.Combine(OnCenterOnEvent, func);
	}

	public void DeregisterCenterOnEvent(CenterOnChildDelegate func)
	{
		if (OnCenterOnEvent != null)
		{
			OnCenterOnEvent = (CenterOnChildDelegate)Delegate.Remove(OnCenterOnEvent, func);
		}
	}

	private void OnEnable()
	{
		Recenter();
	}

	private void OnDragFinished()
	{
		if (base.enabled)
		{
			Recenter();
		}
	}

	private void OnValidate()
	{
		nextPageThreshold = Mathf.Abs(nextPageThreshold);
	}

	public void Recenter()
	{
		Transform transform = base.transform;
		if (transform.childCount == 0)
		{
			return;
		}
		if (mScrollView == null)
		{
			mScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
			if (mScrollView == null)
			{
				Debug.LogWarning(string.Concat(GetType(), " requires ", typeof(UIScrollView), " on a parent object in order to work"), this);
				base.enabled = false;
				return;
			}
			mScrollView.onDragFinished = OnDragFinished;
			if (mScrollView.horizontalScrollBar != null)
			{
				mScrollView.horizontalScrollBar.onDragFinished = OnDragFinished;
			}
			if (mScrollView.verticalScrollBar != null)
			{
				mScrollView.verticalScrollBar.onDragFinished = OnDragFinished;
			}
		}
		if (mScrollView.panel == null)
		{
			return;
		}
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
		Vector3 vector2 = vector + new Vector3(CenterOffset.x * base.transform.lossyScale.x, CenterOffset.y * base.transform.lossyScale.y, CenterOffset.z * base.transform.lossyScale.z);
		Vector3 vector3 = vector2 - mScrollView.currentMomentum * (mScrollView.momentumAmount * 0.1f);
		mScrollView.currentMomentum = Vector3.zero;
		float num = float.MaxValue;
		Transform target = null;
		int num2 = 0;
		int i = 0;
		for (int childCount = transform.childCount; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			float num3 = Vector3.SqrMagnitude(child.position - vector3);
			if (num3 < num)
			{
				num = num3;
				target = child;
				num2 = i;
			}
		}
		if (nextPageThreshold > 0f && UICamera.currentTouch != null && mCenteredObject != null && mCenteredObject.transform == transform.GetChild(num2))
		{
			Vector2 totalDelta = UICamera.currentTouch.totalDelta;
			float num4 = 0f;
			num4 = mScrollView.movement switch
			{
				UIScrollView.Movement.Horizontal => totalDelta.x, 
				UIScrollView.Movement.Vertical => totalDelta.y, 
				_ => totalDelta.magnitude, 
			};
			if (num4 > nextPageThreshold)
			{
				if (num2 > 0)
				{
					target = transform.GetChild(num2 - 1);
				}
			}
			else if (num4 < 0f - nextPageThreshold && num2 < transform.childCount - 1)
			{
				target = transform.GetChild(num2 + 1);
			}
		}
		CenterOn(target, vector2);
	}

	private void CenterOn(Transform target, Vector3 panelCenter)
	{
		if (target != null && mScrollView != null && mScrollView.panel != null)
		{
			Transform cachedTransform = mScrollView.panel.cachedTransform;
			mCenteredObject = target.gameObject;
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
			if (OnCenterOnEvent != null)
			{
				OnCenterOnEvent(target);
			}
		}
		else
		{
			mCenteredObject = null;
		}
	}

	public void CenterOn(Transform target)
	{
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
}
