using Sproto;
using SprotoType;

public class ret_guild_join_handler
{
	public static SprotoTypeBase ret_guild_join_request(SprotoTypeBase req)
	{
		if (req is ret_guild_join.request { HasGuildId: not false } request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.ServerId = request.guildId;
			if (Singleton<ObjManager>.Instance.MainPlayer != null)
			{
				Singleton<ObjManager>.Instance.MainPlayer.ApplyUpDataGuild();
			}
		}
		return null;
	}
}
