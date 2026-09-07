using System;
using System.Collections;
using UnityEngine;

public class TestScreenBottomBtn : MonoBehaviour
{
	private UIEventListener mEventListener;

	private void Start()
	{
		if (mEventListener == null)
		{
			mEventListener = base.gameObject.AddComponent<UIEventListener>();
		}
		UIEventListener uIEventListener = mEventListener;
		uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressBottomBtn));
		UIEventListener uIEventListener2 = mEventListener;
		uIEventListener2.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener2.onDrag, new UIEventListener.VectorDelegate(OnDragBottomBtn));
	}

	public void OnPressBottomBtn(GameObject btn, bool isPress)
	{
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			if (isPress)
			{
				Singleton<ObjManager>.Instance.MainPlayer.CameraController.IsCamCanUse = isPress;
			}
			else if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(DelaySet());
			}
		}
	}

	private IEnumerator DelaySet()
	{
		yield return 1;
		Singleton<ObjManager>.Instance.MainPlayer.CameraController.IsCamCanUse = false;
	}

	public void OnDragBottomBtn(GameObject btn, Vector2 delta)
	{
	}
}
