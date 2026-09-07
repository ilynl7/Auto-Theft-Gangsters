using System.Collections.Generic;
using UnityEngine;

public class MissionPageRootLogic : SingletonUnity<MissionPageRootLogic>
{
	public UILabel MissionTitleLabel;

	public UILabel MissionDiscribeLabel;

	public UILabel MissionStateLabel;

	public GameObject MissionLineChoosePic;

	public Transform[] MissionTabArray;

	public Transform[] MissionLineRootArray;

	public bool[] lineisOpenFlag;

	public ShowRewardItems ShowRewardItem;

	private List<MissionLineLogic> mMainMissionLineList = new List<MissionLineLogic>();

	private List<MissionLineLogic> mSideMissionLineList = new List<MissionLineLogic>();

	private List<MissionLineLogic> mDailyMissionLineList = new List<MissionLineLogic>();

	public GameObject missionLinePrefab;

	public GameObject MissionPageRoot;

	public GameObject AbandonBtn;

	public UITable Table;

	public UILabel StaticMissionTitle2Label;

	public ShowRewardItems DailyFullShowReward;

	private string curMissionId;

	public TweenRotation[] subLineFlagAnim;

	public UISprite[] lineSps;

	public TweenScale[] subLineScaleAnim;

	private int curSelectLines = -1;

	private bool isRefershFlag;

	private bool isOpening;

	private List<MissionData> curMainMissionList = new List<MissionData>();

	private List<MissionData> curSideMissionList = new List<MissionData>();

	private List<MissionData> curDailyMissionList = new List<MissionData>();

	public void Reset(bool isRefersh = false)
	{
		if (!UnityVersionUtil.IsActive(base.gameObject))
		{
			return;
		}
		isRefershFlag = isRefersh;
		UnityVersionUtil.SetActiveRecursive(missionLinePrefab, state: false);
		UnityVersionUtil.SetActiveRecursive(MissionLineChoosePic.gameObject, state: false);
		MissionLineChoosePic.transform.parent = MissionTabArray[0];
		for (int i = 0; i < lineisOpenFlag.Length; i++)
		{
			lineisOpenFlag[i] = false;
		}
		List<string> allMissionId = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetAllMissionId();
		curMainMissionList.Clear();
		curSideMissionList.Clear();
		curDailyMissionList.Clear();
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		for (int j = 0; j < allMissionId.Count; j++)
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(allMissionId[j]);
			if (level >= missionDataByID.DisplayLv && missionDataByID != null)
			{
				switch (missionDataByID.Class)
				{
				case 0:
				case 3:
				case 6:
					curDailyMissionList.Add(missionDataByID);
					break;
				case 1:
					curMainMissionList.Add(missionDataByID);
					break;
				case 2:
				case 4:
				case 5:
					curSideMissionList.Add(missionDataByID);
					break;
				}
			}
		}
		ResetMissionTab(MissionLineRootArray[0], curMainMissionList, mMainMissionLineList);
		ResetMissionTab(MissionLineRootArray[1], curDailyMissionList, mDailyMissionLineList);
		ResetMissionTab(MissionLineRootArray[2], curSideMissionList, mSideMissionLineList);
		if (!isRefershFlag)
		{
			AutoChoiceMission();
		}
		else
		{
			SelectLine();
		}
		Table.Reposition();
	}

	public void AutoChoiceMission()
	{
		isRefershFlag = false;
		if (curMainMissionList.Count != 0)
		{
			UnityVersionUtil.SetActiveRecursive(MissionLineChoosePic.gameObject, state: true);
			MissionLineChoosePic.transform.parent = mMainMissionLineList[0].transform;
			MissionLineChoosePic.transform.localPosition = Vector3.zero;
			OnClickTabRootBtn(MissionTabArray[0].gameObject);
		}
		else if (mDailyMissionLineList.Count != 0)
		{
			UnityVersionUtil.SetActiveRecursive(MissionLineChoosePic.gameObject, state: true);
			MissionLineChoosePic.transform.parent = mDailyMissionLineList[0].transform;
			MissionLineChoosePic.transform.localPosition = Vector3.zero;
			OnClickTabRootBtn(MissionTabArray[1].gameObject);
		}
		else if (curSideMissionList.Count != 0)
		{
			UnityVersionUtil.SetActiveRecursive(MissionLineChoosePic.gameObject, state: true);
			MissionLineChoosePic.transform.parent = mSideMissionLineList[0].transform;
			MissionLineChoosePic.transform.localPosition = Vector3.zero;
			OnClickTabRootBtn(MissionTabArray[2].gameObject);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(MissionLineChoosePic.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(MissionPageRoot, state: false);
		}
	}

	public void SelectLine()
	{
		lineisOpenFlag[curSelectLines] = isOpening;
		switch (curSelectLines)
		{
		case 0:
			if (curMainMissionList.Count != 0)
			{
				UnityVersionUtil.SetActiveRecursive(MissionLineChoosePic.gameObject, state: true);
				MissionLineChoosePic.transform.parent = mMainMissionLineList[0].transform;
				MissionLineChoosePic.transform.localPosition = Vector3.zero;
				OnClickTabRootBtn(MissionTabArray[0].gameObject);
			}
			else
			{
				AutoChoiceMission();
			}
			break;
		case 1:
			if (mDailyMissionLineList.Count != 0)
			{
				UnityVersionUtil.SetActiveRecursive(MissionLineChoosePic.gameObject, state: true);
				MissionLineChoosePic.transform.parent = mDailyMissionLineList[0].transform;
				MissionLineChoosePic.transform.localPosition = Vector3.zero;
				OnClickTabRootBtn(MissionTabArray[1].gameObject);
			}
			else
			{
				AutoChoiceMission();
			}
			break;
		case 2:
			if (curSideMissionList.Count != 0)
			{
				UnityVersionUtil.SetActiveRecursive(MissionLineChoosePic.gameObject, state: true);
				MissionLineChoosePic.transform.parent = mSideMissionLineList[0].transform;
				MissionLineChoosePic.transform.localPosition = Vector3.zero;
				OnClickTabRootBtn(MissionTabArray[2].gameObject);
			}
			else
			{
				AutoChoiceMission();
			}
			break;
		}
	}

	private void ResetMissionTab(Transform missionLineRoot, List<MissionData> dataList, List<MissionLineLogic> curMisLine)
	{
		int num = dataList.Count - curMisLine.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(missionLinePrefab) as GameObject;
				gameObject.transform.parent = missionLineRoot;
				gameObject.transform.localScale = Vector3.one;
				MissionLineLogic component = gameObject.GetComponent<MissionLineLogic>();
				curMisLine.Add(component);
			}
		}
		else if (num < 0)
		{
			for (int num2 = 0; num2 > num; num2--)
			{
				int index = curMisLine.Count - 1 + num2;
				MissionLineLogic missionLineLogic = curMisLine[index];
				curMisLine.RemoveAt(index);
				Object.Destroy(missionLineLogic.gameObject);
			}
		}
		for (int j = 0; j < curMisLine.Count; j++)
		{
			if (j < dataList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(curMisLine[j].gameObject, state: true);
				curMisLine[j].Reset(dataList[j]);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(curMisLine[j].gameObject, state: false);
			}
		}
	}

	public void ResetMissionDataPage(MissionLineLogic missionLine)
	{
		if (!UnityVersionUtil.IsActive(MissionPageRoot))
		{
			UnityVersionUtil.SetActiveRecursive(MissionPageRoot, state: true);
		}
		MissionData curMissionData = missionLine.CurMissionData;
		MissionTitleLabel.text = curMissionData.MTipDescribeID;
		MissionDiscribeLabel.text = curMissionData.MDescribeID;
		MISSION_STATE missionState = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetMissionState(curMissionData.ID);
		MissionManager.GetMissionStateLabel(curMissionData, missionState, MissionStateLabel);
		PROFESSION_TYPE profession = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession;
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		if (curMissionData.Class == 0)
		{
			ShowRewardManager.ShowRewardItem(ShowRewardItem, curMissionData.ID, REWARD_TYPE.DAILY_MISSION, level, profession);
			NGUITools.SetActive(AbandonBtn, state: true);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100311}");
			NGUITools.SetActive(MissionDiscribeLabel.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: true);
			int num = (int)SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetMissionParam(curMissionData.ID, 1);
			if (num < 0)
			{
				num = level;
			}
			ShowRewardManager.ShowRewardItem(DailyFullShowReward, curMissionData.ID, REWARD_TYPE.DAILY_MISSION, num, profession, 1);
		}
		else if (curMissionData.Class == 1)
		{
			ShowRewardManager.ShowRewardItem(ShowRewardItem, curMissionData.ID, REWARD_TYPE.MISSION, level, profession);
			NGUITools.SetActive(AbandonBtn, state: false);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDiscribeLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
		else if (curMissionData.Class == 4 || curMissionData.Class == 5)
		{
			ShowRewardManager.ShowRewardItem(ShowRewardItem, curMissionData.ID, REWARD_TYPE.ESCORT, level, profession);
			NGUITools.SetActive(AbandonBtn, state: true);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDiscribeLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
		else if (curMissionData.Class == 3)
		{
			ShowRewardManager.ShowRewardItem(ShowRewardItem, curMissionData.ID, REWARD_TYPE.DAILY_MISSION, level, profession, 1);
			NGUITools.SetActive(AbandonBtn, state: false);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDiscribeLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
		else if (curMissionData.Class == 2)
		{
			ShowRewardManager.ShowRewardItem(ShowRewardItem, curMissionData.ID, REWARD_TYPE.MISSION, level, profession);
			NGUITools.SetActive(AbandonBtn, state: false);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDiscribeLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
		else if (curMissionData.Class == 6)
		{
			int num2 = (int)SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetMissionParam(curMissionData.ID, 1);
			if (num2 < 0)
			{
				num2 = level;
			}
			ShowRewardManager.ShowRewardItem(ShowRewardItem, curMissionData.ID, REWARD_TYPE.DAILY_MISSION, num2, profession, 1);
			NGUITools.SetActive(AbandonBtn, state: false);
			StaticMissionTitle2Label.text = StrDictionary.GetDictionaryString("#{100307}");
			NGUITools.SetActive(MissionDiscribeLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(DailyFullShowReward.gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(MissionLineChoosePic.gameObject, state: true);
		MissionLineChoosePic.transform.parent = missionLine.transform;
		MissionLineChoosePic.transform.localPosition = Vector3.zero;
		MissionLineChoosePic.transform.localScale = Vector3.one;
		curMissionId = missionLine.CurMissionData.ID;
	}

	public void OnClickAbandonBtn()
	{
		SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AbandonMission(curMissionId);
	}

	public void OnClickGoBtn()
	{
		OnClickCloseBtn();
		MissionManager.ClickMissionAction(curMissionId);
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MissionPageRoot);
	}

	public void CheckTabState(int tabIndex)
	{
		for (int i = 0; i < lineSps.Length; i++)
		{
			if (i == curSelectLines)
			{
				lineSps[i].spriteName = "CZ_huaDongBG_1";
			}
			else
			{
				lineSps[i].spriteName = "CZ_huaDongBG";
			}
		}
	}

	public void OnClickTabRootBtn(GameObject obj)
	{
		if (obj.name.Equals("0"))
		{
			if (mMainMissionLineList.Count != 0)
			{
				OpenSubline(0);
				ResetMissionDataPage(mMainMissionLineList[0]);
			}
		}
		else if (obj.name.Equals("1"))
		{
			if (mDailyMissionLineList.Count != 0)
			{
				OpenSubline(1);
				ResetMissionDataPage(mDailyMissionLineList[0]);
			}
		}
		else if (obj.name.Equals("2") && mSideMissionLineList.Count != 0)
		{
			OpenSubline(2);
			ResetMissionDataPage(mSideMissionLineList[0]);
		}
	}

	public void OpenSubline(int tabindex)
	{
		if (isRefershFlag)
		{
			curSelectLines = tabindex;
			if (!lineisOpenFlag[tabindex])
			{
				subLineFlagAnim[tabindex].PlayForward();
				subLineScaleAnim[tabindex].PlayForward();
				lineisOpenFlag[tabindex] = true;
			}
		}
		else
		{
			curSelectLines = tabindex;
			if (lineisOpenFlag[tabindex])
			{
				subLineFlagAnim[tabindex].PlayReverse();
				subLineScaleAnim[tabindex].PlayReverse();
				lineisOpenFlag[tabindex] = false;
				isOpening = false;
			}
			else
			{
				subLineFlagAnim[tabindex].PlayForward();
				subLineScaleAnim[tabindex].PlayForward();
				lineisOpenFlag[tabindex] = true;
				isOpening = true;
			}
		}
		isRefershFlag = false;
		for (int i = 0; i < lineisOpenFlag.Length; i++)
		{
			if (i != tabindex && lineisOpenFlag[i])
			{
				subLineFlagAnim[i].PlayReverse();
				subLineScaleAnim[i].PlayReverse();
				lineisOpenFlag[i] = false;
			}
		}
		CheckTabState(tabindex);
	}
}
