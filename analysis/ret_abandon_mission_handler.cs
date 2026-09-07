using Sproto;
using SprotoType;

public class ret_abandon_mission_handler
{
	public static SprotoTypeBase ret_abandon_mission_request(SprotoTypeBase req)
	{
		if (req is ret_abandon_mission.request { HasMissionId: not false } request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AbandonMissionSuccess(request.missionId);
		}
		return null;
	}
}
