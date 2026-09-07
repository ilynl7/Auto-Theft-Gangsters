using SprotoType;
using UnityEngine;

public class TeamPreparationRootLogic : SingletonUnity<TeamPreparationRootLogic>
{
	public UILabel[] NameLabelList;

	public UISprite[] IconPicList;

	public UILabel[] LevelLabelList;

	public UISprite[] ReadyPicList;

	public UILabel[] RemainTimesList;

	public UILabel CancelBtnLabel;

	public GameObject CancelBtnRoot;

	public GameObject ReadyBtnRoot;

	public UILabel WaitInfoLabel;

	public UILabel ReadyInfoLabel;

	private TeamMember[] mTeamMemberList;

	private TeamMember mTeamLeader;

	private Team mTeamInfo;

	private float mWaitTime = 20f;

	private float mTimeCount;

	private long mSession;

	private bool mIsReady;

	private int RemainNum;

	private float flashInterval = 1f;

	public long Session => mSession;

	public void Reset()
	{
		Reset(mSession, refreshTime: false);
	}

	public void Reset(long session, bool refreshTime)
	{
		mSession = session;
		mTeamInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo;
		if (mTeamInfo == null || mTeamInfo.TeamID == -1)
		{
			return;
		}
		mTeamMemberList = mTeamInfo.TeamMembers;
		mTeamLeader = mTeamInfo.TeamLeader;
		ResetPlayerIcon(mTeamLeader, 0);
		for (int i = 0; i < mTeamMemberList.Length; i++)
		{
			if (mTeamMemberList[i] != null && mTeamMemberList[i].IsValid())
			{
				if (mTeamMemberList[i].IsRefuseEnterCopy)
				{
					SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TeamPreparationRoot);
					return;
				}
				ResetPlayerIcon(mTeamMemberList[i], i + 1);
			}
			else
			{
				ResetPlayerIconEmpty(i + 1);
			}
		}
		mIsReady = false;
		if (mTeamLeader.ServerId == PlayerData.MainPlayerServerId)
		{
			mIsReady = mTeamLeader.IsReadyEnterCopy;
		}
		else
		{
			for (int j = 0; j < mTeamMemberList.Length; j++)
			{
				if (mTeamMemberList[j] != null && mTeamMemberList[j].IsValid() && mTeamMemberList[j].ServerId == PlayerData.MainPlayerServerId)
				{
					if (mTeamMemberList[j].IsReadyEnterCopy)
					{
						mIsReady = true;
					}
					else
					{
						mIsReady = false;
					}
				}
			}
		}
		if (refreshTime)
		{
			mTimeCount = mWaitTime;
		}
		ShowReady(mIsReady);
	}

	public void ShowReady(bool isReady)
	{
		if (isReady)
		{
			WaitInfoLabel.enabled = false;
			ReadyInfoLabel.enabled = true;
			NGUITools.SetActive(ReadyBtnRoot, state: false);
			NGUITools.SetActive(CancelBtnRoot, state: false);
		}
		else
		{
			WaitInfoLabel.enabled = true;
			ReadyInfoLabel.enabled = false;
			NGUITools.SetActive(ReadyBtnRoot, state: true);
			NGUITools.SetActive(CancelBtnRoot, state: true);
			CancelBtnLabel.text = string.Format("{0}({1})", StrDictionary.GetDictionaryString("#{102069}"), (int)mTimeCount);
		}
	}

	public void ResetPlayerIcon(TeamMember memberInfo, int index)
	{
		IconPicList[index].spriteName = GameDefine.Game_Player_Icon_pic[(int)memberInfo.Profession];
		IconPicList[index].width = 54;
		IconPicList[index].height = 63;
		NameLabelList[index].enabled = true;
		NameLabelList[index].text = memberInfo.Name;
		LevelLabelList[index].enabled = true;
		LevelLabelList[index].text = $"Lv.{memberInfo.Level}";
		RemainTimesList[index].enabled = true;
		CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(mTeamInfo.TeamGoalData.CopyId);
		RemainTimesList[index].text = string.Format("{0}:{1}/{2}", StrDictionary.GetDictionaryString("#{100749}"), memberInfo.CopyRestNum, copySceneDataById.MaxPlayNum);
		if (memberInfo.ServerId == PlayerData.MainPlayerServerId)
		{
			RemainNum = memberInfo.CopyRestNum;
		}
		if (memberInfo.IsReadyEnterCopy)
		{
			ReadyPicList[index].enabled = true;
		}
		else
		{
			ReadyPicList[index].enabled = false;
		}
	}

	public void ResetPlayerIconEmpty(int index)
	{
		IconPicList[index].spriteName = "CZ_zuDui_wuRen";
		IconPicList[index].width = 60;
		IconPicList[index].height = 60;
		NameLabelList[index].enabled = false;
		LevelLabelList[index].enabled = false;
		ReadyPicList[index].enabled = false;
		RemainTimesList[index].enabled = false;
	}

	public void OnClickCancelBtn()
	{
		ret_ask_confirm_multi_copy_scene.request request = new ret_ask_confirm_multi_copy_scene.request();
		request.state = 1L;
		request.session = mSession;
		NetLogic.GetInstance().Send<Protocol.ret_ask_confirm_multi_copy_scene>(request);
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveTeam())
		{
			leave_team.request request2 = new leave_team.request();
			request2.teamid = mTeamInfo.TeamID;
			request2.characterId = PlayerData.MainPlayerServerId;
			NetLogic.GetInstance().Send<Protocol.leave_team>(request2);
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TeamPreparationRoot);
	}

	public void OnClickReadyBtn()
	{
		if (RemainNum <= 0)
		{
			MessageBoxLogic.OpenOKCancelBox("#{103206}", "#{100127}", delegate
			{
				AutoReady();
			});
		}
		else
		{
			AutoReady();
		}
	}

	public void AutoReady()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveTeam())
		{
			ret_ask_confirm_multi_copy_scene.request request = new ret_ask_confirm_multi_copy_scene.request();
			request.state = 1L;
			request.session = mSession;
			NetLogic.GetInstance().Send<Protocol.ret_ask_confirm_multi_copy_scene>(request);
		}
		else
		{
			ret_ask_confirm_multi_copy_scene.request request2 = new ret_ask_confirm_multi_copy_scene.request();
			request2.state = 0L;
			request2.session = mSession;
			NetLogic.GetInstance().Send<Protocol.ret_ask_confirm_multi_copy_scene>(request2);
			Team teamInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo;
			teamInfo.SelfMember.IsReadyEnterCopy = true;
		}
		Reset(mSession, refreshTime: false);
	}

	private void Update()
	{
		if (mIsReady)
		{
			return;
		}
		mTimeCount -= Time.deltaTime;
		flashInterval -= Time.deltaTime;
		if (!(flashInterval < 0f))
		{
			return;
		}
		flashInterval += 1f;
		if (GameManager.OnLineState)
		{
			CancelBtnLabel.text = string.Format("{0}({1})", StrDictionary.GetDictionaryString("#{102069}"), (int)mTimeCount);
			if (mTimeCount < 0f)
			{
				AutoReady();
			}
		}
	}

	private void OnDisable()
	{
		Team teamInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo;
		if (teamInfo != null)
		{
			teamInfo.IsCheckingEnterCopy = false;
		}
	}
}
