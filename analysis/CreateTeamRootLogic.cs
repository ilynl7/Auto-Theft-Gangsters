using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class CreateTeamRootLogic : SingletonUnity<CreateTeamRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public TeamTargetTab TeamTargetTabRoot;

	public GameObject LimitObject;

	public UILabel titleLabel;

	public UILabel LimitLevel;

	public UILabel LimitTime;

	public UILabel LimitTimes;

	public GameObject CreateBtnRoot;

	public GameObject ChangeBtnRoot;

	public GameObject AutoMatchPic;

	private string mCurChooseKey;

	private int mCurLeftLevel;

	private int mCurRightLevel;

	private bool mIsCreatePage;

	private bool mIsAutoMatch;

	private bool IsWorldFlag;

	private bool IsNearbyFlag;

	private bool IsGangFlag;

	public UISprite WorldFlag;

	public UISprite NearbyFlag;

	public UISprite GangFlag;

	private string targetGoalId = string.Empty;

	private int remainNum;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	public void Init()
	{
		List<TeamData> teamDataList = DataManager.GetTeamDataList();
		Dictionary<string, TeamTargetTabData> dictionary = new Dictionary<string, TeamTargetTabData>();
		CopyData copyInfoData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData;
		for (int i = 0; i < teamDataList.Count; i++)
		{
			if (!string.IsNullOrEmpty(teamDataList[i].CopyId) && !copyInfoData.DailyCopyInfoDic.ContainsKey(teamDataList[i].CopyId))
			{
				continue;
			}
			string text = string.Empty;
			if (string.IsNullOrEmpty(teamDataList[i].TitleName))
			{
				if (teamDataList[i].GoalType == 1)
				{
					CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(teamDataList[i].CopyId);
					text = copySceneDataById.MName;
				}
			}
			else
			{
				text = teamDataList[i].MTitleName;
			}
			if (string.IsNullOrEmpty(teamDataList[i].ParentId))
			{
				TeamTargetTabData teamTargetTabData = new TeamTargetTabData();
				teamTargetTabData.Reset(text, new List<string>(), teamDataList[i].ID, new List<string>());
				dictionary.Add(teamDataList[i].ID, teamTargetTabData);
			}
			else
			{
				TeamTargetTabData teamTargetTabData2 = dictionary[teamDataList[i].ParentId];
				teamTargetTabData2.SubTitle.Add(text);
				teamTargetTabData2.SubKey.Add(teamDataList[i].ID);
			}
		}
		TeamTargetTabRoot.Reset(new List<TeamTargetTabData>(dictionary.Values), OnClickLeftTab);
	}

	public void Reset(bool isCreate, string goalId)
	{
		mIsCreatePage = isCreate;
		if (mIsCreatePage)
		{
			titleLabel.text = StrDictionary.GetDictionaryString("#{100801}");
		}
		else
		{
			titleLabel.text = StrDictionary.GetDictionaryString("#{100835}");
		}
		UnityVersionUtil.SetActiveRecursive(TeamTargetTabRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(CreateBtnRoot, state: false);
		UnityVersionUtil.SetActiveRecursive(ChangeBtnRoot, state: false);
		targetGoalId = goalId;
		IsWorldFlag = true;
		WorldFlag.enabled = true;
		IsNearbyFlag = true;
		NearbyFlag.enabled = true;
		IsGangFlag = true;
		GangFlag.enabled = true;
	}

	public void RefreshPage()
	{
		UnityVersionUtil.SetActiveRecursive(TeamTargetTabRoot.gameObject, state: true);
		Init();
		if (mIsCreatePage)
		{
			NGUITools.SetActive(CreateBtnRoot, state: true);
			NGUITools.SetActive(ChangeBtnRoot, state: false);
			mIsAutoMatch = true;
			NGUITools.SetActive(AutoMatchPic, state: true);
			TeamTargetTabRoot.TargetTabLineList[0].OnClickTab();
			return;
		}
		NGUITools.SetActive(CreateBtnRoot, state: false);
		NGUITools.SetActive(ChangeBtnRoot, state: true);
		Team teamInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo;
		if (teamInfo.IsVertify)
		{
			mIsAutoMatch = true;
			NGUITools.SetActive(AutoMatchPic, state: true);
		}
		else
		{
			mIsAutoMatch = false;
			NGUITools.SetActive(AutoMatchPic, state: false);
		}
		bool flag = false;
		flag = !string.IsNullOrEmpty(targetGoalId);
		bool flag2 = false;
		for (int i = 0; i < TeamTargetTabRoot.TargetTabLineList.Count; i++)
		{
			if (flag)
			{
				if (targetGoalId.Equals(TeamTargetTabRoot.TargetTabLineList[i].Key))
				{
					TeamTargetTabRoot.TargetTabLineList[i].OnClickTab();
					break;
				}
			}
			else if (teamInfo.TeamGoalData.ID.Equals(TeamTargetTabRoot.TargetTabLineList[i].Key))
			{
				TeamTargetTabRoot.TargetTabLineList[i].OnClickTab();
				break;
			}
			if (TeamTargetTabRoot.TargetTabLineList[i].HasSubLine)
			{
				for (int j = 0; j < TeamTargetTabRoot.TargetTabLineList[i].SubLineList.Count; j++)
				{
					if (flag)
					{
						if (TeamTargetTabRoot.TargetTabLineList[i].SubLineList[j].Key.Equals(targetGoalId))
						{
							TeamTargetTabRoot.TargetTabLineList[i].ArrowPicTw.PlayForward();
							TeamTargetTabRoot.TargetTabLineList[i].SubTabRootTw.PlayForward();
							TeamTargetTabRoot.TargetTabLineList[i].SubLineList[j].OnClickTab();
							flag2 = true;
							break;
						}
					}
					else if (TeamTargetTabRoot.TargetTabLineList[i].SubLineList[j].Key.Equals(teamInfo.TeamGoalData.ID))
					{
						TeamTargetTabRoot.TargetTabLineList[i].ArrowPicTw.PlayForward();
						TeamTargetTabRoot.TargetTabLineList[i].SubTabRootTw.PlayForward();
						TeamTargetTabRoot.TargetTabLineList[i].SubLineList[j].OnClickTab();
						flag2 = true;
						break;
					}
				}
			}
			if (flag2)
			{
				break;
			}
		}
	}

	private void OnInitializeFloorItem(GameObject obj, int index, int realIndex)
	{
		UILabel component = obj.GetComponent<UILabel>();
		component.text = Mathf.Abs(realIndex).ToString();
	}

	private void LeftLevelCenterOn(Transform centerObj)
	{
		mCurLeftLevel = int.Parse(centerObj.gameObject.GetComponent<UILabel>().text);
	}

	private void RightLevelCenterOn(Transform centerObj)
	{
		mCurRightLevel = int.Parse(centerObj.gameObject.GetComponent<UILabel>().text);
	}

	private void OnClickLeftTab(string key)
	{
		ResetPage(key);
	}

	public void OnClickCreateBtn()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.CREATE_TEAM_CREATE_BTN)
		{
			CheckTutorialEvent();
		}
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(Mathf.Min(mCurLeftLevel, mCurRightLevel)))
		{
			NoticeLogic.AddNotifyData("#{101539}");
			return;
		}
		req_invite_team.request request = new req_invite_team.request();
		request.characterid = -1L;
		request.goalId = mCurChooseKey;
		request.minLevel = Mathf.Min(mCurLeftLevel, mCurRightLevel);
		request.maxLevel = Mathf.Max(mCurLeftLevel, mCurRightLevel);
		request.isVerfiy = ((!mIsAutoMatch) ? 1 : 0);
		request.recruit = GetRecruitNum();
		NetLogic.GetInstance().Send<Protocol.req_invite_team>(request);
		WaitResponseUIRootLogic.OpenWaitBox(109, 10f, 0f);
	}

	public void OnClickChangeBtn()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(Mathf.Min(mCurLeftLevel, mCurRightLevel)))
		{
			NoticeLogic.AddNotifyData("#{101539}");
			return;
		}
		req_change_team_goal.request request = new req_change_team_goal.request();
		request.goalId = mCurChooseKey;
		request.minLevel = Mathf.Min(mCurLeftLevel, mCurRightLevel);
		request.maxLevel = Mathf.Max(mCurLeftLevel, mCurRightLevel);
		request.isVerfiy = ((!mIsAutoMatch) ? 1 : 0);
		request.recruit = GetRecruitNum();
		NetLogic.GetInstance().Send<Protocol.req_change_team_goal>(request);
		OnClickCloseBtn();
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CreateTeamRoot);
		if (TutorialManager.CurStep == TUTORIAL_STEP.CREATE_TEAM_CREATE_BTN)
		{
			CheckTutorialEvent();
		}
	}

	public void OnClickAutoMatchBtn()
	{
		if (mIsAutoMatch)
		{
			mIsAutoMatch = false;
			NGUITools.SetActive(AutoMatchPic, state: false);
		}
		else
		{
			mIsAutoMatch = true;
			NGUITools.SetActive(AutoMatchPic, state: true);
		}
	}

	public void ResetPage(string key)
	{
		int num = 1;
		int pLAYER_MAX_LEVEL = GameDefine.PLAYER_MAX_LEVEL;
		mCurChooseKey = key;
		TeamData teamDataDataByID = DataManager.GetTeamDataDataByID(mCurChooseKey);
		if (teamDataDataByID.GoalType == 1)
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(teamDataDataByID.CopyId);
			NGUITools.SetActive(LimitObject, state: true);
			num = copySceneDataById.MinLevel;
			pLAYER_MAX_LEVEL = copySceneDataById.MaxLevel;
			mCurLeftLevel = num;
			mCurRightLevel = pLAYER_MAX_LEVEL;
			LimitLevel.text = $"Lv.{num} - Lv.{pLAYER_MAX_LEVEL} ";
			LimitTime.text = StrDictionary.GetDictionaryString("#{100830}");
			CopyData copyInfoData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData;
			remainNum = (int)copyInfoData.DailyCopyInfoDic[copySceneDataById.ID].CurNum;
			LimitTimes.text = $"{remainNum}/{copySceneDataById.MaxPlayNum}";
		}
		else
		{
			remainNum = 1;
			mCurLeftLevel = 1;
			mCurRightLevel = GameDefine.PLAYER_MAX_LEVEL;
			NGUITools.SetActive(LimitObject, state: false);
		}
	}

	public void OnClickRecruitBtn()
	{
		if (IsWorldFlag)
		{
			WorldFlag.enabled = false;
		}
		else
		{
			WorldFlag.enabled = true;
		}
		IsWorldFlag = !IsWorldFlag;
	}

	public void OnClickFriendsBtn()
	{
		if (IsNearbyFlag)
		{
			NearbyFlag.enabled = false;
		}
		else
		{
			NearbyFlag.enabled = true;
		}
		IsNearbyFlag = !IsNearbyFlag;
	}

	public void OnClickGangBtn()
	{
		if (IsGangFlag)
		{
			GangFlag.enabled = false;
		}
		else
		{
			GangFlag.enabled = true;
		}
		IsGangFlag = !IsGangFlag;
	}

	public int GetRecruitNum()
	{
		int num = 0;
		if (IsWorldFlag)
		{
			num++;
		}
		if (IsNearbyFlag)
		{
			num += 2;
		}
		if (IsGangFlag)
		{
			num += 4;
		}
		return num;
	}
}
