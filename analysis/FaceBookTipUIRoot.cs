public class FaceBookTipUIRoot : SingletonUnity<FaceBookTipUIRoot>
{
	public UISprite CanCancelBtn;

	private DelegateDefine.NoParamDelegate onClickNo;

	private DelegateDefine.NoParamDelegate onClickYes;

	public void Reset(DelegateDefine.NoParamDelegate yes = null, DelegateDefine.NoParamDelegate no = null)
	{
		onClickYes = yes;
		onClickNo = no;
	}

	private void OnEnable()
	{
		onClickNo = null;
		onClickYes = null;
	}

	public void OnClickYes()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FaceBookTipUIRoot);
		if (onClickYes != null)
		{
			onClickYes();
		}
	}

	public void OnClickNo()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FaceBookTipUIRoot);
		if (onClickNo != null)
		{
			onClickNo();
		}
	}
}
