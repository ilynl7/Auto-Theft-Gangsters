using Sproto;
using SprotoType;

public class apply_join_state_handler
{
	public static SprotoTypeBase apply_join_state_request(SprotoTypeBase req)
	{
		if (req is apply_join_state.request { HasTeamid: not false, isAgree: 0L } request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.RemoveApplyTeam(request.teamid);
		}
		return null;
	}
}
