using System;
using UnityEngine;

public class ScaleUI : MonoBehaviour
{
	private void Awake()
	{
	}

	private void OnEnable()
	{
		ChangeScale();
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(ChangeScale));
	}

	private void ChangeScale()
	{
		base.transform.localScale = Vector3.one * Mathf.Clamp01((float)Screen.width / (float)Screen.height * 0.6f);
	}

	private void OnDisable()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(ChangeScale));
	}
}
