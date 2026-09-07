using Sproto;
using SprotoType;

public class update_team_member_handler
{
	public static SprotoTypeBase update_team_member_request(SprotoTypeBase req)
	{
		if (req is update_team_member.request request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.UpdateTeamMemberInfo(request);
			if (SingletonUnity<TeamUIRootNewLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamUIRootNewLogic>.Instance.gameObject))
			{
				SingletonUnity<TeamUIRootNewLogic>.Instance.Reset();
			}
			if (SingletonUnity<TeamPreparationRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamPreparationRootLogic>.Instance.gameObject))
			{
				SingletonUnity<TeamPreparationRootLogic>.Instance.Reset();
			}
		}
		return null;
	}
}
