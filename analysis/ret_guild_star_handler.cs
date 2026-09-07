using Sproto;
using SprotoType;

public class ret_guild_star_handler
{
	public static SprotoTypeBase ret_guild_star_request(SprotoTypeBase req)
	{
		if (req is ret_guild_star.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (SingletonUnity<GuildStarRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildStarRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildStarRootLogic>.Instance.Reset(request);
			}
		}
		return null;
	}
}
