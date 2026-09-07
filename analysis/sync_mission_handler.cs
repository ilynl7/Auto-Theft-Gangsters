using Sproto;
using SprotoType;

public class sync_mission_handler
{
	public static SprotoTypeBase sync_mission_request(SprotoTypeBase req)
	{
		if (req is sync_mission.request request && SingletonDontDestoryUnity<GameManager>.Instance.MissionManager != null)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.SyncMissionList(request);
		}
		return null;
	}
}
