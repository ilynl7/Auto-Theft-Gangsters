using System;
using UnityEngine;

public class SpecialScaleUI : MonoBehaviour
{
	private void Awake()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(ScreenSizeChanged));
	}

	private void OnEnable()
	{
		ScreenSizeChanged();
	}

	private void ScreenSizeChanged()
	{
		base.transform.localScale = new Vector3((float)Screen.width / (float)Screen.height * 0.6f, Mathf.Clamp01((float)Screen.width / (float)Screen.height * 0.6f), 1f);
	}

	private void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(ScreenSizeChanged));
	}
}
