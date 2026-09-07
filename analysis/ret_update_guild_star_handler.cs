using Sproto;
using SprotoType;

public class ret_update_guild_star_handler
{
	public static SprotoTypeBase ret_update_guild_star_request(SprotoTypeBase req)
	{
		if (req is ret_update_guild_star.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (SingletonUnity<GuildStarRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildStarRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildStarRootLogic>.Instance.UpdateInfo(request);
			}
		}
		return null;
	}
}
