using Sproto;
using SprotoType;

public class apply_join_team_handler
{
	public static SprotoTypeBase apply_join_team_request(SprotoTypeBase req)
	{
		if (req is apply_join_team.request { HasMember: not false } request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.AddApplyMember(request.member);
		}
		return null;
	}
}
