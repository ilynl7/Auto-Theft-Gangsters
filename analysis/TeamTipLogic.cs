using System.Collections.Generic;
using UnityEngine;

public class TeamTipLogic : MonoBehaviour
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public GameObject NoTeamRoot;

	public GameObject HasTeamRoot;

	public List<TeamTipMemberLineLogic> TeamMemberLineList;

	public UISprite CreateTeamBtn;

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

	public void Reset()
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (playerData.IsHaveTeam())
			{
				NGUITools.SetActive(NoTeamRoot, state: false);
				NGUITools.SetActive(HasTeamRoot, state: true);
				ResetTeamMember();
			}
			else
			{
				NGUITools.SetActive(NoTeamRoot, state: true);
				NGUITools.SetActive(HasTeamRoot, state: false);
			}
		}
	}

	public void ResetTeamMember()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsTeamLeader())
		{
			for (int i = 0; i < playerData.TeamInfo.TeamMembers.Length; i++)
			{
				TeamMemberLineList[i].Reset(playerData.TeamInfo.TeamMembers[i]);
			}
			return;
		}
		int num = 0;
		TeamMemberLineList[num].Reset(playerData.TeamInfo.TeamLeader);
		num++;
		for (int j = 0; j < playerData.TeamInfo.TeamMembers.Length; j++)
		{
			if (playerData.TeamInfo.TeamMembers[j].ServerId != PlayerData.MainPlayerServerId)
			{
				TeamMemberLineList[num].Reset(playerData.TeamInfo.TeamMembers[j]);
				num++;
			}
		}
	}

	public void UpdateMemberInfo(TeamMember member)
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			TeamTipMemberLineLogic memberLineById = GetMemberLineById(member.ServerId);
			if (memberLineById != null)
			{
				memberLineById.Reset(member);
			}
		}
	}

	private TeamTipMemberLineLogic GetMemberLineById(long serverId)
	{
		for (int i = 0; i < TeamMemberLineList.Count; i++)
		{
			if (TeamMemberLineList[i].CurMember != null && TeamMemberLineList[i].CurMember.IsValid() && TeamMemberLineList[i].CurMember.ServerId == serverId)
			{
				return TeamMemberLineList[i];
			}
		}
		return null;
	}

	public void OnClickCreateBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CreateTeamRoot, delegate
		{
			WaitResponseUIRootLogic.OpenWaitBox(145, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
			SingletonUnity<CreateTeamRootLogic>.Instance.Reset(isCreate: true, string.Empty);
			SingletonUnity<CreateTeamRootLogic>.Instance.RefreshPage();
			if (TutorialManager.CurStep == TUTORIAL_STEP.CREATE_TEAM_CREATE)
			{
				CheckTutorialEvent();
			}
		});
	}

	public void OnClickSearchBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SearchTeamRoot, delegate
		{
			SingletonUnity<SearchTeamRootLogic>.Instance.Reset(string.Empty);
			WaitResponseUIRootLogic.OpenWaitBox(145, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
		});
	}

	public void OnClickHasTeamBottomPic()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew);
	}
}
