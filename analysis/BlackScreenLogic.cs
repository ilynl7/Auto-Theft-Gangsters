public class BlackScreenLogic : SingletonUnity<BlackScreenLogic>
{
	public TweenAlpha tweenAlph;

	private bool isOpen;

	private DelegateDefine.NoParamDelegate onTweenFinished;

	public static void Close(float duration = 0.5f, DelegateDefine.NoParamDelegate func = null)
	{
		if (SingletonUnity<BlackScreenLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<BlackScreenLogic>.Instance.gameObject))
		{
			SingletonUnity<BlackScreenLogic>.Instance.CloseScreen(duration, func);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BlackScreenRoot, delegate
		{
			SingletonUnity<BlackScreenLogic>.Instance.CloseScreen(duration, func);
		});
	}

	public static void Open(float duration = 0.5f, DelegateDefine.NoParamDelegate func = null)
	{
		if (SingletonUnity<BlackScreenLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<BlackScreenLogic>.Instance.gameObject))
		{
			SingletonUnity<BlackScreenLogic>.Instance.OpenScreen(duration, func);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BlackScreenRoot, delegate
		{
			SingletonUnity<BlackScreenLogic>.Instance.OpenScreen(duration, func);
		});
	}

	public void CloseScreen(float duration = 0.5f, DelegateDefine.NoParamDelegate func = null)
	{
		if (!UnityVersionUtil.IsActive(tweenAlph.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(tweenAlph.gameObject, state: true);
		}
		tweenAlph.duration = duration;
		tweenAlph.PlayForward();
		isOpen = true;
		onTweenFinished = func;
	}

	public void OpenScreen(float duration = 0.5f, DelegateDefine.NoParamDelegate func = null)
	{
		tweenAlph.duration = duration;
		tweenAlph.PlayReverse();
		isOpen = false;
		onTweenFinished = func;
	}

	public void OnTweenFinished()
	{
		if (!isOpen)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BlackScreenRoot);
		}
		if (onTweenFinished != null)
		{
			onTweenFinished();
			onTweenFinished = null;
		}
	}
}
