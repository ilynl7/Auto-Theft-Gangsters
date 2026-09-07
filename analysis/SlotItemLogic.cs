using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SlotItemLogic : MonoBehaviour
{
	public UISprite[] SpList;

	public ShowRewardItems rewardshow;

	public string[] resultStr;

	public void Reset(slot_data curinfo)
	{
		ShowRewardData showRewardDataByID = DataManager.GetShowRewardDataByID(curinfo.ShowRewardID);
		if (showRewardDataByID != null)
		{
			rewardshow.ShowRewards(new List<string>(showRewardDataByID.ItemIdList), new List<EQUIP_QUALITY>(showRewardDataByID.QualityList), new List<int>(showRewardDataByID.CountList));
		}
		string rewardMap = curinfo.RewardMap;
		if (string.IsNullOrEmpty(rewardMap) && curinfo.ID.Equals("9999"))
		{
			SpList[0].spriteName = "CZ_slot_Star";
			SpList[0].SetDimensions(30, 30);
			SpList[0].enabled = true;
			SpList[1].enabled = false;
			SpList[2].enabled = false;
			return;
		}
		resultStr[0] = rewardMap.Substring(0, 1);
		resultStr[1] = rewardMap.Substring(1, 1);
		resultStr[2] = rewardMap.Substring(2, 1);
		List<string> list = new List<string>();
		for (int i = 0; i < resultStr.Length; i++)
		{
			if (resultStr[i].Equals("a") || resultStr[i].Equals("b") || resultStr[i].Equals("c"))
			{
				list.Add("a");
			}
			else
			{
				list.Add(resultStr[i]);
			}
		}
		for (int j = 0; j < SpList.Length; j++)
		{
			if (j < list.Count)
			{
				if (list[j].Equals("a"))
				{
					SpList[j].spriteName = "CZ_renWu_wenHao";
				}
				else
				{
					SlotIconData slotIconDataById = DataManager.GetSlotIconDataById(list[j]);
					SpList[j].spriteName = slotIconDataById.IconName;
				}
				if (list[j].Equals("1") || list[j].Equals("2"))
				{
					SpList[j].SetDimensions(30, 25);
				}
				else if (list[j].Equals("a"))
				{
					SpList[j].SetDimensions(20, 28);
				}
				else
				{
					SpList[j].SetDimensions(24, 26);
				}
				SpList[j].enabled = true;
			}
			else
			{
				SpList[j].enabled = false;
			}
		}
	}
}
