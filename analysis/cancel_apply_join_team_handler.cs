using Sproto;
using SprotoType;

public class cancel_apply_join_team_handler
{
	public static SprotoTypeBase cancel_apply_join_team_request(SprotoTypeBase req)
	{
		if (req is cancel_apply_join_team.request { HasId: not false } request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.RemoveApplyMember(request.id);
		}
		return null;
	}
}
