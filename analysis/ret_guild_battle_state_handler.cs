using Sproto;
using SprotoType;

public class ret_guild_battle_state_handler
{
	public static SprotoTypeBase ret_guild_battle_state_request(SprotoTypeBase req)
	{
		if (req is ret_guild_battle_state.request request)
		{
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.ActivityData.SyncGuildBattleInfo(request);
		}
		return null;
	}
}
