using Sproto;
using SprotoType;

public class update_team_handler
{
	public static SprotoTypeBase update_team_request(SprotoTypeBase req)
	{
		if (req is update_team.request request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			playerData.TeamInfo.UpdateTeamInfo(request);
		}
		return null;
	}
}
