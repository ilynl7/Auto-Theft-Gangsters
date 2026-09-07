using UnityEngine;

public class NumItemLogic : MonoBehaviour
{
	public UILabel infolabel;

	public int curnum;

	private DelegateDefine.OneIntParamDelegate ClickFun;

	public void Reset(DelegateDefine.OneIntParamDelegate clickitem = null)
	{
		ClickFun = clickitem;
	}

	public void OnClickItem()
	{
		if (ClickFun != null)
		{
			ClickFun(curnum);
		}
	}
}
