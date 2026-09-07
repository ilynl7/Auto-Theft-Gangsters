public class CarRootLogic : SingletonUnity<CarRootLogic>
{
	private void OnEnable()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(null, OnClickCloseBtn, hideTab: true, StrDictionary.GetDictionaryString("#{100121}"));
		});
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
	}
}
