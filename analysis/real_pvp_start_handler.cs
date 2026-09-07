using Sproto;
using SprotoType;

public class real_pvp_start_handler
{
	public static SprotoTypeBase real_pvp_start_request(SprotoTypeBase req)
	{
		if (req is real_pvp_start.request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.StartGame();
		}
		return null;
	}
}
