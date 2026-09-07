using Sproto;
using SprotoType;

public class ret_request_guild_boss_handler
{
	public static SprotoTypeBase ret_request_guild_boss_request(SprotoTypeBase req)
	{
		if (req is ret_request_guild_boss.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.ActivityData.SyncGuildBossInfoData(request);
			if (SingletonUnity<GuildActivityRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildActivityRootLogic>.Instance.gameObject))
			{
				if (SingletonUnity<GuildActivityRootLogic>.Instance.CurChoosedIndex == -1)
				{
					SingletonUnity<GuildActivityRootLogic>.Instance.RefershBossInfo(request);
				}
				else
				{
					SingletonUnity<GuildActivityRootLogic>.Instance.UpdateGuildBossInfo(request);
				}
			}
			if (SingletonUnity<NewDailyActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewDailyActivityUIRootLogic>.Instance.RefershGuildBossInfo();
			}
		}
		return null;
	}
}
