using Sproto;
using SprotoType;

public class notice_guild_battle_rank_handler
{
	public static SprotoTypeBase notice_guild_battle_rank_request(SprotoTypeBase req)
	{
		if (req is notice_guild_battle_rank.request { HasGuildId: not false } request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.ChampionGuildId = request.guildId;
			Singleton<ObjManager>.Instance.RefreshPlayerGuildPic();
		}
		return null;
	}
}
