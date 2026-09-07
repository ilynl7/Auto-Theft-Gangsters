using System.Collections.Generic;
using UnityEngine;

public class SmallPlayerInfoLine : MonoBehaviour
{
	public List<SmallPlayerInfoItem> ItemList;

	public void Reset(PlayerInfoItemData leftData, PlayerInfoItemData rightData, string btnName, string disableBtnName, DelegateDefine.OneLongParamDelegate onClickBtn)
	{
		if (leftData == null)
		{
			NGUITools.SetActive(ItemList[0].gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(ItemList[0].gameObject, state: true);
			ItemList[0].Reset(leftData.Profession, leftData.Name, leftData.ComboVal, leftData.Level, btnName, disableBtnName, leftData.IsEnable, leftData.Key, leftData.GuildId, leftData.GuildName, onClickBtn);
		}
		if (rightData == null)
		{
			NGUITools.SetActive(ItemList[1].gameObject, state: false);
			return;
		}
		NGUITools.SetActive(ItemList[1].gameObject, state: true);
		ItemList[1].Reset(rightData.Profession, rightData.Name, rightData.ComboVal, rightData.Level, btnName, disableBtnName, rightData.IsEnable, rightData.Key, rightData.GuildId, rightData.GuildName, onClickBtn);
	}
}
