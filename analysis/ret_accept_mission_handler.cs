using Sproto;
using SprotoType;

public class ret_accept_mission_handler
{
	public static SprotoTypeBase ret_accept_mission_request(SprotoTypeBase req)
	{
		if (req is ret_accept_mission.request request)
		{
			if (GameManager.IsSupportCurDataVersion184())
			{
				SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AcceptMissionSuccess(request.mission, PlayerCommonData.GetServerTime());
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AcceptMissionSuccess(request.missionId, PlayerCommonData.GetServerTime());
			}
		}
		return null;
	}
}
