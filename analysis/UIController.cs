using UnityEngine;

public class UIController : MonoBehaviour
{
	public static bool InitScreenSizeFlag;

	public static float ScreenWidth;

	public static float ScreenHeight = 480f;

	public static float ScreenWidthScale;

	public static float ScreenHeightScale;

	private void InitUI()
	{
		if (!SingletonUnity<UIManager>.Exists)
		{
			base.gameObject.AddComponent<UIManager>();
		}
	}

	private void Awake()
	{
		if (!InitScreenSizeFlag)
		{
			InitScreenSizeFlag = true;
			UIRoot component = base.gameObject.GetComponent<UIRoot>();
			ScreenHeight = component.manualHeight;
			ScreenWidth = Mathf.RoundToInt(ScreenHeight * ((float)Screen.width / (float)Screen.height));
			ScreenWidthScale = ScreenWidth / (float)Screen.width;
			ScreenHeightScale = ScreenHeight / (float)Screen.height;
		}
		InitUI();
	}
}
