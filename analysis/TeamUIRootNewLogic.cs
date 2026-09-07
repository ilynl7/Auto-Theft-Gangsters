using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class TeamUIRootNewLogic : SingletonUnity<TeamUIRootNewLogic>
{
	public List<TeamPlayerPicLogic> PlayerPicLogicList;

	public GameObject ApplyBtnRoot;

	public GameObject StartBtnRoot;

	public GameObject ApplyTipPic;

	public UILabel TeamTargetLabel;

	public UILabel TeamLimitLevelLabel;

	public TweenAlpha AutoMatchAnima;

	public UILabel AutoMatchLabel;

	public UISprite ShoutRootPic;

	public UISprite LeaveBtnPic;

	private PlayerData mPlayerData;

	public GameObject UrgeBtnRoot;

	private int mPreCount;

	private bool mIsMatching;

	private float lastClickUrgeTime;

	public bool IsMatching
	{
		get
		{
			return mIsMatching;
		}
		set
		{
			mIsMatching = value;
		}
	}

	private void OnEnable()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(null, OnClickCloseBtn, hideTab: true, StrDictionary.GetDictionaryString("#{100251}"));
		});
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		Reset();
		mPreCount = 0;
	}

	private void OnDisable()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.JOIN_TEAM_START || TutorialManager.CurStep == TUTORIAL_STEP.JOIN_TEAM_SHOW_1 || TutorialManager.CurStep == TUTORIAL_STEP.JOIN_TEAM_CLICK_URGE || TutorialManager.CurStep == TUTORIAL_STEP.JOIN_TEAM_FINISH)
		{
			TutorialManager.CloseTutorial();
		}
	}

	public void Reset()
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			if (mPlayerData.IsHaveTeam())
			{
				ResetTeamPage();
				return;
			}
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TeamRootNew);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		}
	}

	private void ResetTeamPage()
	{
		CopySceneData copySceneData = null;
		if (mPlayerData.TeamInfo.TeamGoalData.GoalType == 1)
		{
			copySceneData = DataManager.GetCopySceneDataById(mPlayerData.TeamInfo.TeamGoalData.CopyId);
		}
		PlayerPicLogicList[0].Reset(mPlayerData.TeamInfo.TeamLeader);
		for (int i = 1; i < PlayerPicLogicList.Count; i++)
		{
			if (mPlayerData.TeamInfo.TeamMembers[i - 1].IsValid())
			{
				PlayerPicLogicList[i].Reset(mPlayerData.TeamInfo.TeamMembers[i - 1]);
			}
			else if (copySceneData != null && i >= copySceneData.MaxMember)
			{
				PlayerPicLogicList[i].Reset(null, isLock: true);
			}
			else
			{
				PlayerPicLogicList[i].Reset(null);
			}
		}
		if (mPlayerData.IsTeamLeader())
		{
			UnityVersionUtil.SetActiveRecursive(ApplyBtnRoot, state: true);
			UnityVersionUtil.SetActiveRecursive(UrgeBtnRoot, state: false);
			if (mPlayerData.TeamInfo.TeamGoalData.GoalType == 0)
			{
				UnityVersionUtil.SetActiveRecursive(StartBtnRoot, state: false);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(StartBtnRoot, state: true);
				mPreCount = mPlayerData.TeamInfo.GetTeamberCount();
			}
			if (mPlayerData.TeamInfo.ApplyMemberDic.Count > 0)
			{
				UnityVersionUtil.SetActiveRecursive(ApplyTipPic, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(ApplyTipPic, state: false);
			}
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ApplyBtnRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(StartBtnRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(ApplyTipPic, state: false);
			if (mPlayerData.TeamInfo.TeamGoalData.GoalType == 0)
			{
				UnityVersionUtil.SetActiveRecursive(UrgeBtnRoot, state: false);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(UrgeBtnRoot, state: true);
			}
		}
		if (mPlayerData.TeamInfo.IsVertify)
		{
			AutoMatchAnima.enabled = true;
			AutoMatchAnima.PlayForward();
			AutoMatchLabel.text = StrDictionary.GetDictionaryString("#{102007}");
			IsMatching = true;
		}
		else
		{
			AutoMatchAnima.ResetToBeginning();
			AutoMatchAnima.enabled = false;
			AutoMatchLabel.text = StrDictionary.GetDictionaryString("#{100826}");
			IsMatching = false;
		}
		if (!string.IsNullOrEmpty(mPlayerData.TeamInfo.TeamGoalData.TitleName))
		{
			TeamTargetLabel.text = mPlayerData.TeamInfo.TeamGoalData.MTitleName;
		}
		else
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(mPlayerData.TeamInfo.TeamGoalData.CopyId);
			TeamTargetLabel.text = copySceneDataById.MName;
		}
		TeamLimitLevelLabel.text = $"Lv.{mPlayerData.TeamInfo.MinLimitLevel}-{mPlayerData.TeamInfo.MaxLimitLevel}";
		if (mPlayerData.IsTeamLeader() && (TutorialManager.CurStep == TUTORIAL_STEP.JOIN_TEAM_START || TutorialManager.CurStep == TUTORIAL_STEP.JOIN_TEAM_SHOW_1 || TutorialManager.CurStep == TUTORIAL_STEP.JOIN_TEAM_CLICK_URGE || TutorialManager.CurStep == TUTORIAL_STEP.JOIN_TEAM_FINISH))
		{
			TutorialManager.CloseTutorial();
		}
	}

	public void OnApplyTeam()
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			NGUITools.SetActive(ApplyTipPic.gameObject, state: true);
		}
	}

	public void OnClearApplyTeam()
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			NGUITools.SetActive(ApplyTipPic.gameObject, state: false);
		}
	}

	public void OnClickListingBtn()
	{
		WaitResponseUIRootLogic.OpenWaitBox(145, 10f, 0f);
		NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SearchTeamRoot, delegate
		{
			SingletonUnity<SearchTeamRootLogic>.Instance.Reset(string.Empty);
		});
	}

	public void OnClickApplyBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamApplyListRoot);
	}

	public void OnClickStartBtn()
	{
		mPlayerData.TeamInfo.EnterMultiCopyAction();
	}

	public void OnClickAutoMatchBtn()
	{
		if (mPlayerData.IsTeamLeader())
		{
			req_change_team_goal.request request = new req_change_team_goal.request();
			request.goalId = mPlayerData.TeamInfo.TeamGoalData.ID;
			request.minLevel = mPlayerData.TeamInfo.MinLimitLevel;
			request.maxLevel = mPlayerData.TeamInfo.MaxLimitLevel;
			request.isVerfiy = (mPlayerData.TeamInfo.IsVertify ? 1 : 0);
			NetLogic.GetInstance().Send<Protocol.req_change_team_goal>(request);
		}
		else
		{
			NoticeLogic.AddNotifyData("#{102008}");
		}
	}

	public void OnClickLeaveBtn()
	{
		leave_team.request request = new leave_team.request();
		request.teamid = mPlayerData.TeamInfo.TeamID;
		request.characterId = PlayerData.MainPlayerServerId;
		NetLogic.GetInstance().Send<Protocol.leave_team>(request);
	}

	public void OnClickChangeTargetBtn()
	{
		if (mPlayerData.IsTeamLeader())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CreateTeamRoot, delegate
			{
				WaitResponseUIRootLogic.OpenWaitBox(145, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
				SingletonUnity<CreateTeamRootLogic>.Instance.Reset(isCreate: false, string.Empty);
			});
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TeamRootNew);
	}

	public void OnClickNormalChatBtn()
	{
		OnClickCloseBtn();
		ChatUIRootLogic.ResetLinkChat(GameDefine.CHAT_LINK_TYPE.TEAM, GameDefine.CHAT_CHANNEL_TYPE.NORMAL, mPlayerData.TeamInfo);
	}

	public void OnClickWorldChatBtn()
	{
		OnClickCloseBtn();
		ChatUIRootLogic.ResetLinkChat(GameDefine.CHAT_LINK_TYPE.TEAM, GameDefine.CHAT_CHANNEL_TYPE.WORLD, mPlayerData.TeamInfo);
	}

	public void OnClickGuildChatBtn()
	{
		if (mPlayerData.IsHaveGuild())
		{
			OnClickCloseBtn();
			ChatUIRootLogic.ResetLinkChat(GameDefine.CHAT_LINK_TYPE.TEAM, GameDefine.CHAT_CHANNEL_TYPE.GUILD, mPlayerData.TeamInfo);
		}
		else
		{
			NoticeLogic.AddNotifyData("#{102006}");
		}
	}

	public void OnClickUrgeBtn()
	{
		if (Time.time - lastClickUrgeTime >= 5f)
		{
			lastClickUrgeTime = Time.time;
			NetLogic.GetInstance().Send<Protocol.urge_team_leader>();
		}
	}

	public void OnClickRecruitBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamBroadCastRoot);
	}
}
