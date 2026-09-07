using SprotoType;
using UnityEngine;

public class PlayerCarIconLogic : MonoBehaviour
{
	public UISprite Icon;

	public UISprite NewLabel;

	public DelegateDefine.StringGameObjectDelegate onClickCarIcon;

	private string mCurCarId = string.Empty;

	public string CurCarId => mCurCarId;

	public void Reset(mount curCarData, DelegateDefine.StringGameObjectDelegate func = null)
	{
		mCurCarId = curCarData.ID;
		onClickCarIcon = func;
		MountData mountDataById = DataManager.GetMountDataById(mCurCarId);
		Icon.spriteName = mountDataById.CarIcon;
		NGUITools.SetActive(NewLabel.gameObject, curCarData.state == 3);
	}

	public void OnClickBtn()
	{
		if (UnityVersionUtil.IsActive(NewLabel.gameObject))
		{
			NGUITools.SetActive(NewLabel.gameObject, state: false);
		}
		if (onClickCarIcon != null)
		{
			onClickCarIcon(mCurCarId, base.gameObject);
		}
	}
}
