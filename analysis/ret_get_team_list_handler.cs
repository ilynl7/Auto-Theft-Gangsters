using System.Collections.Generic;
using Sproto;
using SprotoType;

public class ret_get_team_list_handler
{
	public static SprotoTypeBase ret_get_team_list_request(SprotoTypeBase req)
	{
		if (req is ret_get_team_list.request { HasTeams: not false } request && SingletonUnity<SearchTeamRootLogic>.Exists)
		{
			SingletonUnity<SearchTeamRootLogic>.Instance.UpdateTeamList(new List<team>(request.teams.Values));
		}
		return null;
	}
}
