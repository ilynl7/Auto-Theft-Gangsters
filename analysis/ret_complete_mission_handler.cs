using Sproto;
using SprotoType;

public class ret_complete_mission_handler
{
	public static SprotoTypeBase ret_complete_mission_request(SprotoTypeBase req)
	{
		if (req is ret_complete_mission.request request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.CompleteMissionSuccess(request.missionId);
		}
		return null;
	}
}
