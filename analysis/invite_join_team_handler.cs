using Sproto;
using SprotoType;
using UnityEngine;

public class invite_join_team_handler
{
	private static long characterId = -1L;

	private static long teamId = -1L;

	public static SprotoTypeBase invite_join_team_request(SprotoTypeBase req)
	{
		if (req is invite_join_team.request request)
		{
			characterId = request.member.id;
			teamId = request.teamid;
			if (UIManager.IsUnlockTutorialEnable())
			{
				DisagreeInvite();
				return null;
			}
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsCanInviteTeam(teamId))
			{
				string goalid = string.Empty;
				if (request.HasGoalId)
				{
					goalid = request.goalId;
				}
				NewMessageUIRootLogic.AddteamInfo(new InviteTeamInfo(characterId, teamId, request.member.name, (long)Time.time, goalid));
			}
			else
			{
				DisagreeInvite();
			}
		}
		return null;
	}

	public static void AgreeInvite()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.TEAM))
		{
			ret_invite_join_team.request request = new ret_invite_join_team.request();
			request.ok = 1L;
			request.id = characterId;
			NetLogic.GetInstance().Send<Protocol.ret_invite_join_team>(request);
			if (teamId != -1)
			{
				req_join_team.request request2 = new req_join_team.request();
				request2.teamid = teamId;
				request2.isapply = true;
				NetLogic.GetInstance().Send<Protocol.req_join_team>(request2);
			}
		}
		else
		{
			NoticeLogic.AddNotifyData("#{100276}");
		}
	}

	public static void DisagreeInvite()
	{
		ret_invite_join_team.request request = new ret_invite_join_team.request();
		request.ok = 0L;
		request.id = characterId;
		NetLogic.GetInstance().Send<Protocol.ret_invite_join_team>(request);
	}
}
