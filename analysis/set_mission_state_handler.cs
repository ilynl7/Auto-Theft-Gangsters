using Sproto;
using SprotoType;

public class set_mission_state_handler
{
	public static SprotoTypeBase set_mission_state_request(SprotoTypeBase req)
	{
		if (req is set_mission_state.request request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.SetMissionState(request.missionId, (MISSION_STATE)request.missionstate, PlayerCommonData.GetServerTime());
		}
		return null;
	}
}
