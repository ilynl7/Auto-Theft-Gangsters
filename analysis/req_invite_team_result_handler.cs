using Sproto;
using SprotoType;

public class req_invite_team_result_handler
{
	public static SprotoTypeBase req_invite_team_result_request(SprotoTypeBase req)
	{
		if (req is req_invite_team_result.request request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			playerData.TeamInfo.RemoveInvitedPerson(request.id);
		}
		return null;
	}
}
