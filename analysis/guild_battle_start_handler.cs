using Sproto;
using SprotoType;

public class guild_battle_start_handler
{
	public static SprotoTypeBase guild_battle_start_request(SprotoTypeBase req)
	{
		if (req is guild_battle_start.request && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager is GuildBattleSceneManager guildBattleSceneManager)
		{
			guildBattleSceneManager.OpenControl();
		}
		return null;
	}
}
