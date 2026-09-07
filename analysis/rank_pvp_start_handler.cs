using Sproto;
using SprotoType;

public class rank_pvp_start_handler
{
	public static SprotoTypeBase rank_pvp_start_request(SprotoTypeBase req)
	{
		if (req is rank_pvp_start.request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.StartGame();
		}
		return null;
	}
}
