using UnityEngine;

public class NewMissionInfoRootLogic : MonoBehaviour
{
	public UILabel MissionDescLabel;

	public UILabel StaticMissionTitle2Label;

	public ShowRewardItems ShowRewardRoot;

	public ShowRewardItems DailyFullShowReward;

	public UISprite StartBtnSp;

	public UISprite AbandonBtnSp;

	private MissionData curMissionData;

	public GameObject StarObj;

	public UISprite[] Star;

	public UILabel TimeLabel;

	private MissionManager missionManager;

	private float timeCount;

	public void Reset(string missionId)
	{
		if (curMissionData != null && curMissionData.ID.Equals(missionId))
		{
			return;
		}
		NGUITools.SetActive(StarObj, state: false);
		curMissionData = DataManager.GetMissionDataByID(missionId);
		MissionDescLabel.text = curMissionData.MDescribeID;
		missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		MISSION_STATE missionState = missionManager.GetMissionState(curMissionData.ID);
		PROFESSION_TYPE profession = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession;
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		TimeLabel.enabled = false;
		if (curMissionData.Class == 0)
		{
			ShowRewardManager.ShowRewardItem(ShowRewardRoot, curMissionData.ID, REWARD_TYPE.DAILY_MISSION, level, profession);
			NGUITools.SetActive(AbandonBtnSp.gameObject, state: false);
			StartBtnSp.transform.localPosition = new Vector3(0f, StartBtnSp.transform.localPosition.y, 0f);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100311}");
			NGUITools.SetActive(MissionDescLabel.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: true);
			int num = (int)missionManager.GetMissionParam(curMissionData.ID, 1);
			if (num < 0)
			{
				num = level;
			}
			ShowRewardManager.ShowRewardItem(DailyFullShowReward, curMissionData.ID, REWARD_TYPE.DAILY_MISSION, num, profession, 1);
		}
		else if (curMissionData.Class == 1)
		{
			ShowRewardManager.ShowRewardItem(ShowRewardRoot, curMissionData.ID, REWARD_TYPE.MISSION, level, profession);
			NGUITools.SetActive(AbandonBtnSp.gameObject, state: false);
			StartBtnSp.transform.localPosition = new Vector3(0f, StartBtnSp.transform.localPosition.y, 0f);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDescLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
		else if (curMissionData.Class == 4 || curMissionData.Class == 5)
		{
			ShowRewardManager.ShowRewardItem(ShowRewardRoot, curMissionData.ID, REWARD_TYPE.ESCORT, level, profession);
			NGUITools.SetActive(AbandonBtnSp.gameObject, state: true);
			StartBtnSp.transform.localPosition = new Vector3(60f, StartBtnSp.transform.localPosition.y, 0f);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDescLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
		else if (curMissionData.Class == 3)
		{
			ShowRewardManager.ShowRewardItem(ShowRewardRoot, curMissionData.ID, REWARD_TYPE.DAILY_MISSION, level, profession, 1);
			NGUITools.SetActive(AbandonBtnSp.gameObject, state: false);
			StartBtnSp.transform.localPosition = new Vector3(0f, StartBtnSp.transform.localPosition.y, 0f);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDescLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
		else if (curMissionData.Class == 2)
		{
			ShowRewardManager.ShowRewardItem(ShowRewardRoot, curMissionData.ID, REWARD_TYPE.MISSION, level, profession);
			NGUITools.SetActive(AbandonBtnSp.gameObject, state: false);
			StartBtnSp.transform.localPosition = new Vector3(0f, StartBtnSp.transform.localPosition.y, 0f);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDescLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
		else if (curMissionData.Class == 6)
		{
			int num2 = (int)missionManager.GetMissionParam(curMissionData.ID, 1);
			if (num2 < 0)
			{
				num2 = level;
			}
			ShowRewardManager.ShowRewardItem(ShowRewardRoot, curMissionData.ID, REWARD_TYPE.DAILY_MISSION, num2, profession, 1);
			NGUITools.SetActive(AbandonBtnSp.gameObject, state: false);
			StartBtnSp.transform.localPosition = new Vector3(0f, StartBtnSp.transform.localPosition.y, 0f);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDescLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
		else if (curMissionData.Class == 7)
		{
			OnlineMissionData onlineMissionDataByID = DataManager.GetOnlineMissionDataByID(curMissionData.ID);
			string empty = string.Empty;
			empty = missionManager.GetMissionParam(curMissionData.ID, 1).ToString();
			onlineMissionDataByID = DataManager.GetOnlineMissionDataByID(empty);
			if (onlineMissionDataByID != null)
			{
				ShowRewardManager.ShowRewardItem(ShowRewardRoot, empty, REWARD_TYPE.ONLINEMISSION, level, profession);
				NGUITools.SetActive(StarObj, state: true);
				for (int i = 0; i < Star.Length; i++)
				{
					if (i < onlineMissionDataByID.MissionStar)
					{
						Star[i].color = Color.white;
						Star[i].alpha = 1f;
					}
					else
					{
						Star[i].color = Color.black;
						Star[i].alpha = 0.5f;
					}
				}
			}
			NGUITools.SetActive(AbandonBtnSp.gameObject, state: true);
			StartBtnSp.transform.localPosition = new Vector3(60f, StartBtnSp.transform.localPosition.y, 0f);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDescLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
		else if (curMissionData.Class == 8)
		{
			TimeLabel.enabled = true;
			long num3 = 0L;
			if (missionState == MISSION_STATE.INVALID)
			{
				TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(curMissionData.TimeLimitId);
				MissionDescLabel.text = timeLimitMissionDataByID.MDesc;
				num3 = timeLimitMissionDataByID.LimitTime;
			}
			else
			{
				num3 = missionManager.GetMissionRestTime(curMissionData.ID);
			}
			TimeLabel.text = StrDictionary.GetDictionaryString("#{100002}", TimeTools.GetHourMinSecStr(num3));
			ShowRewardManager.ShowRewardItem(ShowRewardRoot, curMissionData.ID, REWARD_TYPE.MISSION, level, profession);
			NGUITools.SetActive(AbandonBtnSp.gameObject, state: false);
			StartBtnSp.transform.localPosition = new Vector3(0f, StartBtnSp.transform.localPosition.y, 0f);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDescLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
	}

	public void OnClickStartBtn()
	{
		MissionData missionData = curMissionData;
		SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
		MissionManager.ClickMissionAction(missionData.ID);
	}

	public void OnClickAbandonBtn()
	{
		missionManager.AbandonMission(curMissionData.ID);
	}

	private void OnDisable()
	{
		curMissionData = null;
	}

	private void Update()
	{
		if (curMissionData == null || curMissionData.Class != 8)
		{
			return;
		}
		timeCount += Time.deltaTime;
		if (timeCount >= 1f)
		{
			timeCount -= 1f;
			if (missionManager.IsMissionAccepted(curMissionData.ID))
			{
				TimeLabel.text = StrDictionary.GetDictionaryString("#{100002}", TimeTools.GetHourMinSecStr(missionManager.GetMissionRestTime(curMissionData.ID)));
			}
		}
	}
}
