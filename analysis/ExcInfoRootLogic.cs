public class ExcInfoRootLogic : SingletonUnity<ExcInfoRootLogic>
{
	public UIScrollView CurScrollView;

	public UILabel InfoLabel;

	public UILabel titleLabel;

	private DelegateDefine.NoParamDelegate onClosed;

	public void Reset(string titlestr, string infostr, DelegateDefine.NoParamDelegate closefun = null, params object[] args)
	{
		titleLabel.text = StrDictionary.GetDictionaryString(titlestr);
		InfoLabel.text = StrDictionary.GetDictionaryString(infostr, args);
		CurScrollView.ResetPosition();
		onClosed = closefun;
	}

	public void Close()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ExcInfoRoot);
		if (onClosed != null)
		{
			onClosed();
		}
	}
}
