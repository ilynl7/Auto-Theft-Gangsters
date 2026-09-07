public class LoginNoticeRootLogic : SingletonUnity<LoginNoticeRootLogic>
{
	public UILabel InfoLabel;

	public void Reset(string infoStr)
	{
		InfoLabel.text = StrDictionary.GetDictionaryString(infoStr).Replace("#r", "\n");
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LoginNoticeRootUI);
	}
}
