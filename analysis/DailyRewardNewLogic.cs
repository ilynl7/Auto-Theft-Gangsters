using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DailyRewardNewLogic : SingletonUnity<DailyRewardNewLogic>
{
	public List<DailyRewardNewItem> rewardItemsList;

	public GameObject rewardObj;

	public UISlider ActiveSlider;

	public UILabel ActiveLabel;

	private int MaxActive = 100;

	private List<daily_reward> rewardList;

	private List<DailyActiveRewardData> RewardDataList = new List<DailyActiveRewardData>();

	private int curScore;

	public GameObject AnimaObj;

	public UILabel infolabel;

	public UILabel totalNameLabel;

	private float[] sliderValue = new float[4] { 0.27f, 0.517f, 0.75f, 1f };

	public void EnableReset()
	{
		UnityVersionUtil.SetActiveRecursive(rewardObj, state: false);
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (GameManager.IsSupportCurDataVersion145())
		{
			totalNameLabel.text = StrDictionary.GetDictionaryString("#{301125}");
			infolabel.text = StrDictionary.GetDictionaryString("#{300817}", TimeTools.GetLocalShowTime_HM(playerCommonData.ResetTime, playerCommonData.TimeOffset));
		}
		else
		{
			totalNameLabel.text = "Today Score";
			infolabel.text = $"The score will be reset at {TimeTools.GetLocalShowTime_HM(playerCommonData.ResetTime, playerCommonData.TimeOffset)} everyday.";
		}
	}

	public void Reset(ret_request_daily_active.request request)
	{
		rewardList = new List<daily_reward>(request.daily_rewards.Values);
		rewardList.Sort((daily_reward x, daily_reward y) => (x.ID.Length == y.ID.Length) ? x.ID.CompareTo(y.ID) : (x.ID.Length - y.ID.Length));
		RewardDataList.Clear();
		for (int i = 0; i < rewardList.Count; i++)
		{
			RewardDataList.Add(DataManager.GetDailyActiveRewardDataById(rewardList[i].ID));
		}
		curScore = (int)request.score;
		UnityVersionUtil.SetActiveRecursive(rewardObj, state: true);
		for (int j = 0; j < rewardItemsList.Count; j++)
		{
			if (j < rewardList.Count)
			{
				NGUITools.SetActive(rewardItemsList[j].gameObject, state: true);
				rewardItemsList[j].UpdateInfo(rewardList[j]);
			}
			else
			{
				NGUITools.SetActive(rewardItemsList[j].gameObject, state: false);
			}
		}
		if (RewardDataList.Count > 0)
		{
			MaxActive = RewardDataList[RewardDataList.Count - 1].Score;
		}
		SetActiveSlider((int)request.score);
		ActiveLabel.text = $"{(int)request.score}/{MaxActive}";
		if (request.score >= 100)
		{
			NGUITools.SetActive(AnimaObj, state: false);
		}
		else
		{
			NGUITools.SetActive(AnimaObj, state: true);
		}
	}

	private void SetActiveSlider(int score)
	{
		int num = -1;
		for (int i = 0; i < RewardDataList.Count; i++)
		{
			if (score <= RewardDataList[i].Score)
			{
				num = i;
				break;
			}
		}
		if (score > RewardDataList[RewardDataList.Count - 1].Score)
		{
			ActiveSlider.value = 1f;
		}
		else if (num > 0)
		{
			ActiveSlider.value = sliderValue[num - 1] + (sliderValue[num] - sliderValue[num - 1]) * ((float)(score - RewardDataList[num - 1].Score) / (float)(RewardDataList[num].Score - RewardDataList[num - 1].Score));
		}
		else
		{
			ActiveSlider.value = sliderValue[0] * ((float)score / (float)RewardDataList[0].Score);
		}
	}

	public void UpdateInfo(string id)
	{
		for (int i = 0; i < rewardList.Count; i++)
		{
			if (rewardList[i].ID.Equals(id))
			{
				rewardList[i].state = 2L;
			}
		}
		for (int j = 0; j < rewardItemsList.Count; j++)
		{
			rewardItemsList[j].UpdateInfo(rewardList[j]);
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "DailyActive", $"require_{id}");
	}
}
