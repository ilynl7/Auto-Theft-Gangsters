public class ShowRewardManager
{
	public static void ShowRewardItem(ShowRewardItems showItems, string id, REWARD_TYPE type, int level, PROFESSION_TYPE profession, int showIndex = 0)
	{
		UnityVersionUtil.SetActiveRecursive(showItems.gameObject, state: true);
		switch (type)
		{
		case REWARD_TYPE.MISSION:
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(id);
			ShowRewardData showRewardData = null;
			if (missionDataByID.Class == 8)
			{
				TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(missionDataByID.TimeLimitId);
				if (timeLimitMissionDataByID == null)
				{
					UnityVersionUtil.SetActiveRecursive(showItems.gameObject, state: false);
					break;
				}
				showRewardData = DataManager.GetShowRewardDataByID(timeLimitMissionDataByID.ShowRewardId);
				if (showRewardData != null)
				{
					showItems.ShowRewards(showRewardData.ItemIdList, showRewardData.QualityList, showRewardData.CountList);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(showItems.gameObject, state: false);
				}
				break;
			}
			while (missionDataByID.IsMultiMission == 1)
			{
				missionDataByID = DataManager.GetMissionDataByID(missionDataByID.NextID);
			}
			showRewardData = profession switch
			{
				PROFESSION_TYPE.XD => DataManager.GetShowRewardDataByID(missionDataByID.XDShowID), 
				PROFESSION_TYPE.QJ => DataManager.GetShowRewardDataByID(missionDataByID.QJShowID), 
				_ => DataManager.GetShowRewardDataByID(missionDataByID.NQSShowID), 
			};
			if (showRewardData != null)
			{
				showItems.ShowRewards(showRewardData.ItemIdList, showRewardData.QualityList, showRewardData.CountList);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(showItems.gameObject, state: false);
			}
			break;
		}
		case REWARD_TYPE.COPY:
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(id);
			ShowRewardData showRewardDataByID4 = DataManager.GetShowRewardDataByID(copySceneDataById.ShowRewardId);
			if (showRewardDataByID4 != null)
			{
				showItems.ShowRewards(showRewardDataByID4.ItemIdList, showRewardDataByID4.QualityList, showRewardDataByID4.CountList);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(showItems.gameObject, state: false);
			}
			break;
		}
		case REWARD_TYPE.DAILY_MISSION:
		{
			DailyMissionData dailyMissionDataById = DataManager.GetDailyMissionDataById(id);
			if (dailyMissionDataById != null)
			{
				AdaptData adaptDataByID = DataManager.GetAdaptDataByID(level);
				ShowRewardData showRewardData2 = null;
				if (adaptDataByID != null)
				{
					showRewardData2 = DataManager.GetShowRewardDataByID(adaptDataByID.DorpKeyDic[dailyMissionDataById.ShowRewardIDList[showIndex]]);
				}
				if (showRewardData2 != null)
				{
					showItems.ShowRewards(showRewardData2.ItemIdList, showRewardData2.QualityList, showRewardData2.CountList);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(showItems.gameObject, state: false);
				}
			}
			else if (showIndex == 1)
			{
				AdaptData adaptDataByID2 = DataManager.GetAdaptDataByID(level);
				ShowRewardData showRewardDataByID2 = DataManager.GetShowRewardDataByID(adaptDataByID2.DorpKeyDic["_show_rhqh"]);
				if (showRewardDataByID2 != null)
				{
					showItems.ShowRewards(showRewardDataByID2.ItemIdList, showRewardDataByID2.QualityList, showRewardDataByID2.CountList);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(showItems.gameObject, state: false);
				}
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(showItems.gameObject, state: false);
			}
			break;
		}
		case REWARD_TYPE.ESCORT:
		{
			EscortData escortDataById = DataManager.GetEscortDataById(id);
			AdaptData adaptDataByID3 = DataManager.GetAdaptDataByID(level);
			ShowRewardData showRewardDataByID3 = DataManager.GetShowRewardDataByID(adaptDataByID3.DorpKeyDic[escortDataById.ShowRewardId]);
			if (showRewardDataByID3 != null)
			{
				showItems.ShowRewards(showRewardDataByID3.ItemIdList, showRewardDataByID3.QualityList, showRewardDataByID3.CountList);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(showItems.gameObject, state: false);
			}
			break;
		}
		case REWARD_TYPE.ONLINEMISSION:
		{
			OnlineMissionData onlineMissionDataByID = DataManager.GetOnlineMissionDataByID(id);
			ShowRewardData showRewardDataByID = DataManager.GetShowRewardDataByID(onlineMissionDataByID.ShowReward);
			if (showRewardDataByID != null)
			{
				showItems.ShowRewards(showRewardDataByID.ItemIdList, showRewardDataByID.QualityList, showRewardDataByID.CountList);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(showItems.gameObject, state: false);
			}
			break;
		}
		}
	}
}
