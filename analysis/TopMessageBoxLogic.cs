public class TopMessageBoxLogic : SingletonUnity<TopMessageBoxLogic>
{
	public UILabel titlelabel;

	public UILabel infoLabel;

	public UILabel yesLabel;

	public UILabel noLabel;

	private DelegateDefine.NoParamDelegate onClickNo;

	private DelegateDefine.NoParamDelegate onClickYes;

	public void Clear()
	{
		onClickNo = null;
		onClickYes = null;
	}

	public void OpenOKCancelBox(string text, string title, DelegateDefine.NoParamDelegate delOnYesClick = null, DelegateDefine.NoParamDelegate delOnCancelClick = null, string yesStr = null, string noStr = null)
	{
		titlelabel.text = StrDictionary.GetDictionaryString(title);
		infoLabel.text = StrDictionary.GetDictionaryString(text);
		yesLabel.text = StrDictionary.GetDictionaryString(yesStr);
		noLabel.text = StrDictionary.GetDictionaryString(noStr);
		onClickYes = delOnYesClick;
		onClickNo = delOnCancelClick;
	}

	public void OnClickYesBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TopMessageBoxUI);
		if (onClickYes != null)
		{
			onClickYes();
		}
	}

	public void OnClickNoBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TopMessageBoxUI);
		if (onClickNo != null)
		{
			onClickNo();
		}
	}
}
