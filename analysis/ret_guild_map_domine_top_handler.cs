using Sproto;
using SprotoType;

public class ret_guild_map_domine_top_handler
{
	public static SprotoTypeBase ret_guild_map_domine_top_request(SprotoTypeBase req)
	{
		if (req is ret_guild_map_domine_top.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (SingletonUnity<CityDamageRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CityDamageRootLogic>.Instance.gameObject))
			{
				SingletonUnity<CityDamageRootLogic>.Instance.UpdateInfo(request);
			}
		}
		return null;
	}
}
