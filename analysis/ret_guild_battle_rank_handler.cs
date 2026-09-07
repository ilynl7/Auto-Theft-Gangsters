using Sproto;
using SprotoType;

public class ret_guild_battle_rank_handler
{
	public static SprotoTypeBase ret_guild_battle_rank_request(SprotoTypeBase req)
	{
		if (req is ret_guild_battle_rank.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (SingletonUnity<GuildBattleRankRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildBattleRankRoot>.Instance.gameObject))
			{
				SingletonUnity<GuildBattleRankRoot>.Instance.ResershInfo(request);
			}
		}
		return null;
	}
}
