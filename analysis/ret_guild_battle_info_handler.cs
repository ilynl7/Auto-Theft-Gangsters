using Sproto;
using SprotoType;

public class ret_guild_battle_info_handler
{
	public static SprotoTypeBase ret_guild_battle_info_request(SprotoTypeBase req)
	{
		if (req is ret_guild_battle_info.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.ActivityData.SyncGuildBattleInfo(request);
			if (SingletonUnity<GuildBattleRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildBattleRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildBattleRootLogic>.Instance.Reset(request);
			}
		}
		return null;
	}
}
