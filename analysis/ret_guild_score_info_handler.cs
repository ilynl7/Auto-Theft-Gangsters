using Sproto;
using SprotoType;

public class ret_guild_score_info_handler
{
	public static SprotoTypeBase ret_guild_score_info_request(SprotoTypeBase req)
	{
		if (req is ret_guild_score_info.request request)
		{
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			if (sceneManager.IsBigWorld())
			{
				return null;
			}
			if (request.HasGuild_battle_score_info && SingletonUnity<GuildBattleInfoRoot>.Exists)
			{
				SingletonUnity<GuildBattleInfoRoot>.Instance.UpdateInfo(request.guild_battle_score_info);
			}
		}
		return null;
	}
}
