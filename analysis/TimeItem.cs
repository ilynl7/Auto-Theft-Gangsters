using System;
using UnityEngine;

public class TimeItem : MonoBehaviour
{
	public UISprite btnSp;

	public UILabel TimeLabel;

	public UISprite SelectSp;

	private long CurTime;

	private int CurIndex;

	private DelegateDefine.OneIntParamDelegate OnClickItem;

	public void Reset(long curtime, int index, int select, DelegateDefine.OneIntParamDelegate onclickitem)
	{
		CurTime = curtime;
		CurIndex = index;
		OnClickItem = onclickitem;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		TimeSpan localShowTime = TimeTools.GetLocalShowTime(CurTime, playerCommonData.TimeOffset);
		TimeLabel.text = $"{localShowTime.Hours:D2}:{localShowTime.Minutes:D2}";
		UpdateSelect(select);
	}

	public void OnClickBtn()
	{
		if (OnClickItem != null)
		{
			OnClickItem(CurIndex);
		}
	}

	public void UpdateSelect(int select)
	{
		if (CurIndex == select)
		{
			SelectSp.enabled = true;
			btnSp.spriteName = "CZ_huaDongBG_1";
		}
		else
		{
			SelectSp.enabled = false;
			btnSp.spriteName = "CZ_huaDongBG_2_XuanDing";
		}
	}
}
