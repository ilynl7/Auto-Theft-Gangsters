using Sproto;
using SprotoType;

public class start_participate_dance_handler
{
	public static SprotoTypeBase start_participate_dance_request(SprotoTypeBase req)
	{
		if (req is start_participate_dance.request request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerDanceData.SyncPlayerDanceInfo(request);
			ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			mainPlayer.StartDance(request.curUse);
		}
		return null;
	}
}
