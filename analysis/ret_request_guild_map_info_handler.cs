using Sproto;
using SprotoType;

public class ret_request_guild_map_info_handler
{
	public static SprotoTypeBase ret_request_guild_map_info_request(SprotoTypeBase req)
	{
		if (req is ret_request_guild_map_info.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.SyncGuildCityInfo(request);
			if (SingletonUnity<GuildCityRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildCityRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildCityRootLogic>.Instance.UpdateCityInfo();
			}
			if (SingletonUnity<WorldMapRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<WorldMapRoot>.Instance.gameObject))
			{
				SingletonUnity<WorldMapRoot>.Instance.Reset();
			}
		}
		return null;
	}
}
