using System.Collections.Generic;

public class NumRootLogic : SingletonUnity<NumRootLogic>
{
	public UIGrid ItemGrid;

	public List<NumItemLogic> numItems;

	private int curNum;

	private DelegateDefine.OneIntParamDelegate backFun;

	public void Reset(DelegateDefine.OneIntParamDelegate clicknumfun)
	{
		backFun = clicknumfun;
		for (int i = 0; i < numItems.Count; i++)
		{
			numItems[i].Reset(OnClickItem);
		}
	}

	public void OnClickItem(int num)
	{
		if (num < 10)
		{
			if (backFun != null)
			{
				backFun(num);
			}
		}
		else if (num == 99)
		{
			OnClickClose();
		}
	}

	public void OnClickClose()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NumRoot);
	}
}
