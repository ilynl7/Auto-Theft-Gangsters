using System;
using System.Collections;
using UnityEngine;

public class ScreenBottomBtn : SingletonUnity<ScreenBottomBtn>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

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

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
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

	protected override void OnDestroy()
	{
		UIEventListener uIEventListener = mEventListener;
		uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Remove(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressBottomBtn));
		base.OnDestroy();
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
		if (!(camCtl != null))
		{
			return;
		}
		if (isPress)
		{
			StopAllCoroutines();
			camCtl.IsCamCanUse = isPress;
			return;
		}
		if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(DelaySet());
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.MOVE_SCREEN)
		{
			CheckTutorialEvent();
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
}
