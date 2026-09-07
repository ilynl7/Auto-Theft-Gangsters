using UnityEngine;

public class AutoComboInfoLogic : SingletonUnity<AutoComboInfoLogic>
{
	private bool isShow = true;

	public UIWidget CurWidget;

	public Transform Offset;

	private void OnEnable()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsCopyScene() && !SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene())
		{
			Vector3 localPosition = Offset.localPosition;
			localPosition.y = 60f;
			Offset.localPosition = localPosition;
			CurWidget.alpha = 0f;
		}
	}

	public void Show(bool show)
	{
		if (isShow != show)
		{
			isShow = show;
			if (SingletonUnity<FunctionBtnRootLogic>.Exists)
			{
				SingletonUnity<FunctionBtnRootLogic>.Instance.ShowAutoCombo(show);
				CurWidget.alpha = 0f;
			}
			else if (isShow)
			{
				CurWidget.alpha = 1f;
			}
			else
			{
				CurWidget.alpha = 0f;
			}
		}
	}
}
