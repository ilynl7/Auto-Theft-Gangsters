public class TipUIRootLogic : SingletonUnity<TipUIRootLogic>
{
	public UILabel TipLabel;

	private TIP_EVENT mCurEvent;

	public void Reset(TIP_EVENT tipEvent)
	{
		mCurEvent = tipEvent;
		if (tipEvent == TIP_EVENT.TEAM)
		{
			TipLabel.text = tipEvent.ToString();
		}
	}

	public void OnClickBtn()
	{
		if (mCurEvent != 0)
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew, delegate(bool isSuccess, object param)
		{
			if (isSuccess)
			{
				SingletonUnity<TeamUIRootNewLogic>.Instance.Reset();
				SingletonUnity<TeamUIRootNewLogic>.Instance.OnClickApplyBtn();
			}
		});
	}
}
