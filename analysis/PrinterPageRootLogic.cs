public class PrinterPageRootLogic : SingletonUnity<PrinterPageRootLogic>
{
	public MyTypewriterEffect TypeWriter;

	private string[] mPageStr;

	private DelegateDefine.NoParamDelegate onPrintFinish;

	private int mCurPage;

	private bool isFinishePage;

	public void Reset(string[] pages, DelegateDefine.NoParamDelegate onFinished)
	{
		mPageStr = pages;
		onPrintFinish = onFinished;
		TypeWriter.Reset(mPageStr[mCurPage], OnPageShowFinished);
		isFinishePage = false;
	}

	private void OnPageShowFinished()
	{
		isFinishePage = true;
	}

	public void OnClickShowAllBtn()
	{
		if (!isFinishePage)
		{
			TypeWriter.ShowAll();
			isFinishePage = true;
			return;
		}
		mCurPage++;
		if (mCurPage < mPageStr.Length)
		{
			TypeWriter.Reset(mPageStr[mCurPage], OnPageShowFinished);
			isFinishePage = false;
		}
		else
		{
			OnClickSkipBtn();
		}
	}

	public void OnClickSkipBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PrinterPageRoot);
		if (onPrintFinish != null)
		{
			onPrintFinish();
		}
	}
}
