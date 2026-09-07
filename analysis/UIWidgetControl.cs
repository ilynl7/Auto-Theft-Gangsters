using System;
using UnityEngine;

[ExecuteInEditMode]
public class UIWidgetControl : MonoBehaviour
{
	private UIWidget uiWidget;

	public int width = 800;

	public int height = 480;

	private static float originWidth = 800f;

	private void Awake()
	{
		if (width == 800)
		{
			width = 804;
		}
		if (height == 480)
		{
			height = 484;
		}
		uiWidget = GetComponent<UIWidget>();
		if (uiWidget != null)
		{
			float num = (float)Screen.width / (float)Screen.height * 480f;
			int num2 = Mathf.FloorToInt(num * ((float)width / originWidth));
			if (num2 != uiWidget.width)
			{
				uiWidget.width = num2;
				uiWidget.height = height;
			}
			BoxCollider component = GetComponent<BoxCollider>();
			if (component != null)
			{
				component.size = new Vector3(num2, height, 1f);
			}
		}
	}

	public static int GetFitWidth(int sourceWidth)
	{
		float num = (float)Screen.width / (float)Screen.height * 480f;
		return Mathf.FloorToInt(num * ((float)sourceWidth / originWidth));
	}

	private void Start()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(ScreenSizeChanged));
	}

	private void Update()
	{
	}

	private void ScreenSizeChanged()
	{
		Awake();
	}

	private void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(ScreenSizeChanged));
	}
}
