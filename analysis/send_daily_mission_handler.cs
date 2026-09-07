using Sproto;
using SprotoType;

public class send_daily_mission_handler
{
	public static SprotoTypeBase send_daily_mission_request(SprotoTypeBase req)
	{
		if (req is send_daily_mission.request request)
		{
			MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			missionManager.AcceptMissionSucess(request);
		}
		return null;
	}
}
