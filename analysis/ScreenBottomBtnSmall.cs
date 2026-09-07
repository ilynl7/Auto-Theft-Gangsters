using System;
using System.Collections;
using UnityEngine;

public class ScreenBottomBtnSmall : MonoBehaviour
{
	private bool mLockBtn;

	private UIEventListener mEventListener;

	private CameraController camCtl;

	public bool LockBtn
	{
		get
		{
			return mLockBtn;
		}
		set
		{
			mLockBtn = value;
		}
	}

	private void Start()
	{
		if (mEventListener == null)
		{
			mEventListener = base.gameObject.AddComponent<UIEventListener>();
		}
		UIEventListener uIEventListener = mEventListener;
		uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressBottomBtn));
		mLockBtn = false;
	}

	public void OnPressBottomBtn(GameObject btn, bool isPress)
	{
		if (mLockBtn)
		{
			return;
		}
		if (camCtl == null && Singleton<ObjManager>.Exists && Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			camCtl = Singleton<ObjManager>.Instance.MainPlayer.CameraController;
		}
		if (camCtl != null)
		{
			if (isPress)
			{
				StopAllCoroutines();
				camCtl.IsCamCanUse = isPress;
			}
			else if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(DelaySet());
			}
		}
	}

	private IEnumerator DelaySet()
	{
		yield return null;
		if (camCtl != null)
		{
			camCtl.IsCamCanUse = false;
		}
	}

	private void OnDisable()
	{
		if (camCtl != null)
		{
			camCtl.IsCamCanUse = false;
		}
	}
}
