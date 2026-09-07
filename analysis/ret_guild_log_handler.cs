using Sproto;
using SprotoType;

public class ret_guild_log_handler
{
	public static SprotoTypeBase ret_guild_log_request(SprotoTypeBase req)
	{
		if (req is ret_guild_log.request request)
		{
			if (request.HasLogs)
			{
				if (SingletonUnity<GuildMemberRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildMemberRootLogic>.Instance.gameObject))
				{
					SingletonUnity<GuildMemberRootLogic>.Instance.ShowGuildLogs(request.logs);
				}
			}
			else if (SingletonUnity<GuildMemberRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildMemberRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildMemberRootLogic>.Instance.ShowGuildLogs(null);
			}
		}
		return null;
	}
}
