using Sproto;
using SprotoType;
using UnityEngine;

public class notice_urge_team_leader_handler
{
	public static SprotoTypeBase notice_urge_team_leader_request(SprotoTypeBase req)
	{
		if (req is notice_urge_team_leader.request request)
		{
			NoticeLogic.AddNotifyData("#{100294}");
			NewMessageUIRootLogic.AddteamInfo(new InviteTeamInfo(isurge: true, request.name, request.id, (long)Time.time));
		}
		return null;
	}
}
