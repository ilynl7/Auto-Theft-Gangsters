using System.Collections.Generic;

public class CopyTeamTipRoot : SingletonUnity<CopyTeamTipRoot>
{
	public List<TeamTipMemberLineLogic> TeamMemberLineList;

	public void Reset()
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (playerData.IsHaveTeam())
			{
				ResetTeamMember();
			}
			else
			{
				NGUITools.SetActive(base.gameObject, state: false);
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
}
