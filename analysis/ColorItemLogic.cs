using SprotoType;
using UnityEngine;

public class ColorItemLogic : MonoBehaviour
{
	public UISprite ColorSp;

	public UISprite LockSp;

	public UISprite selectSp;

	public DelegateDefine.StringGameObjectDelegate onClickColorIcon;

	private color mCurcolorData;

	public void Reset(color curcolor, string selectid, DelegateDefine.StringGameObjectDelegate func)
	{
		if (curcolor.state == 0L)
		{
			LockSp.enabled = true;
		}
		else
		{
			LockSp.enabled = false;
		}
		ColorData colorDataById = DataManager.GetColorDataById(curcolor.ID);
		ColorSp.color = colorDataById.CUIColor;
		mCurcolorData = curcolor;
		onClickColorIcon = func;
		if (selectid.Equals(curcolor.ID))
		{
			selectSp.enabled = true;
		}
		else
		{
			selectSp.enabled = false;
		}
	}

	public void refersh(string selectid)
	{
		if (selectid.Equals(mCurcolorData.ID))
		{
			selectSp.enabled = true;
		}
		else
		{
			selectSp.enabled = false;
		}
	}

	public void OnClickColorBtn()
	{
		if (onClickColorIcon != null)
		{
			onClickColorIcon(mCurcolorData.ID, base.gameObject);
		}
	}
}
