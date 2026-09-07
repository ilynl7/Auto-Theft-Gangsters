using Sproto;
using SprotoType;

public class ret_guild_map_reward_handler
{
	public static SprotoTypeBase ret_guild_map_reward_request(SprotoTypeBase req)
	{
		if (req is ret_guild_map_reward.request request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.SyncGuildCityRewardInfo(request);
			if (SingletonUnity<GuildCityRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildCityRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildCityRootLogic>.Instance.UpdateCityItemInfo(request);
			}
		}
		return null;
	}
}
