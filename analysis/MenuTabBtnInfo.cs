public class MenuTabBtnInfo
{
	public bool IsIconBtn;

	public string BtnName = string.Empty;

	public DelegateDefine.NoParamDelegate onClickBtn;

	public string PageName = string.Empty;

	public DelegateDefine.NoParamReturnDelegate TipsFun;

	public FUNCTION_TYPE FunctionType;

	public MenuTabBtnInfo(DelegateDefine.NoParamDelegate onClickFunc, bool isIcon, string btnName, string pageName, FUNCTION_TYPE functionType, DelegateDefine.NoParamReturnDelegate tipsfun = null)
	{
		onClickBtn = onClickFunc;
		IsIconBtn = isIcon;
		BtnName = btnName;
		PageName = pageName;
		TipsFun = tipsfun;
		FunctionType = functionType;
	}
}
