using Sproto;
using SprotoType;

public class notify_confirm_state_handler
{
	public static SprotoTypeBase notify_confirm_state_request(SprotoTypeBase req)
	{
		if (req is notify_confirm_state.request request)
		{
			Team teamInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo;
			TeamMember teamMemberByServerId = teamInfo.GetTeamMemberByServerId(request.characterId);
			if (teamMemberByServerId != null)
			{
				if (request.state == 0L)
				{
					teamMemberByServerId.IsReadyEnterCopy = true;
					teamMemberByServerId.IsRefuseEnterCopy = false;
					bool flag = true;
					for (int i = 0; i < teamInfo.TeamMembers.Length; i++)
					{
						if (teamInfo.TeamMembers[i] != null && teamInfo.TeamMembers[i].IsValid() && !teamInfo.TeamMembers[i].IsReadyEnterCopy)
						{
							flag = false;
						}
					}
					if (flag)
					{
						teamInfo.IsCheckingEnterCopy = false;
					}
				}
				else
				{
					teamMemberByServerId.IsReadyEnterCopy = false;
					teamMemberByServerId.IsRefuseEnterCopy = true;
					teamInfo.IsCheckingEnterCopy = false;
				}
			}
			else
			{
				teamInfo.IsCheckingEnterCopy = false;
			}
			if (SingletonUnity<TeamUIRootNewLogic>.Exists)
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
