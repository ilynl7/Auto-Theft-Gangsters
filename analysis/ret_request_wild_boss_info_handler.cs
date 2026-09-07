using Sproto;
using SprotoType;

public class ret_request_wild_boss_info_handler
{
	public static SprotoTypeBase ret_request_wild_boss_info_request(SprotoTypeBase req)
	{
		if (req is ret_request_wild_boss_info.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.ActivityData.SyncWildBossInfoData(request);
			if (SingletonUnity<NewDailyActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewDailyActivityUIRootLogic>.Instance.RefershBossInfo();
			}
		}
		return null;
	}
}
