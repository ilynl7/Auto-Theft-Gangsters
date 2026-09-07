using Sproto;
using SprotoType;

public class sync_random_team_state_handler
{
	public static SprotoTypeBase sync_random_team_state_request(SprotoTypeBase req)
	{
		if (req is sync_random_team_state.request req2)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			playerData.TeamInfo.UpdateMatchState(req2);
		}
		return null;
	}
}
