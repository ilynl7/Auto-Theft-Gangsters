using System.Collections.Generic;
using UnityEngine;

public class RankPVPLineReward : MonoBehaviour
{
	public UILabel infoLabel;

	public ShowRewardItems showrewarditem;

	public void ShowRewardInfo(LadderRewardData curdata)
	{
		ShowRewardData showRewardData = null;
		showRewardData = DataManager.GetShowRewardDataByID(curdata.ShowRewardID);
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(showrewarditem.gameObject, state: true);
			showrewarditem.ShowRewards(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(showrewarditem.gameObject, state: false);
		}
		if (curdata.Rankdown == curdata.Rankup)
		{
			infoLabel.text = $"Rank {curdata.Rankdown}";
		}
		else
		{
			infoLabel.text = $"Rank {curdata.Rankdown}-{curdata.Rankup}";
		}
	}
}
