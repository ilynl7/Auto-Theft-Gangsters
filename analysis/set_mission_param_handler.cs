using Sproto;
using SprotoType;

public class set_mission_param_handler
{
	public static SprotoTypeBase set_mission_param_request(SprotoTypeBase req)
	{
		if (req is set_mission_param.request request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.SetMissionParam(request.missionId, (int)request.paramindex - 1, request.param);
		}
		return null;
	}
}
