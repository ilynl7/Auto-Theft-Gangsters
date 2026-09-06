using UnityEngine;

public class ServerLineLogic : MonoBehaviour
{
	public int lineindex;

	public UISprite btnSp;

	public UILabel LineLabel;

	private DelegateDefine.OneIntParamDelegate ClickLine;

	public void Reset(int index, DelegateDefine.OneIntParamDelegate linefun = null)
	{
		lineindex = index;
		LineLabel.text = $"{index * 10 + 1}-{(index + 1) * 10}";
		ClickLine = linefun;
	}

	public void RefershSelect(int selectid)
	{
		if (selectid == lineindex)
		{
			btnSp.spriteName = "CZ_huaDongBG_1";
		}
		else
		{
			btnSp.spriteName = "CZ_huaDongBG";
		}
	}

	public void OnClickLineItem()
	{
		if (ClickLine != null)
		{
			ClickLine(lineindex);
		}
	}
}
