using Sproto;
using SprotoType;

public class ret_domin_info_handler
{
	public static SprotoTypeBase ret_domin_info_request(SprotoTypeBase req)
	{
		if (req is ret_domin_info.request request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			playerData.UpdateDominInfo(request);
		}
		return null;
	}
}
