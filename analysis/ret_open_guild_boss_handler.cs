using Sproto;
using SprotoType;

public class ret_open_guild_boss_handler
{
	public static SprotoTypeBase ret_open_guild_boss_request(SprotoTypeBase req)
	{
		if (req is ret_open_guild_boss.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (request.ok && request.HasGuild_boss && SingletonUnity<GuildActivityRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildActivityRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildActivityRootLogic>.Instance.refershOpenBtn(request.guild_boss);
			}
		}
		return null;
	}
}
