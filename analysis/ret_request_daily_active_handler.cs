using Sproto;
using SprotoType;

public class ret_request_daily_active_handler
{
	public static SprotoTypeBase ret_request_daily_active_request(SprotoTypeBase req)
	{
		if (req is ret_request_daily_active.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.InitDailyRewards(request);
			if (SingletonUnity<DailyActiveRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DailyActiveRewardRootLogic>.Instance.gameObject))
			{
				SingletonUnity<DailyActiveRewardRootLogic>.Instance.Reset(request);
			}
			if (SingletonUnity<DailyRewardNewLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DailyRewardNewLogic>.Instance.gameObject))
			{
				SingletonUnity<DailyRewardNewLogic>.Instance.Reset(request);
			}
			if (SingletonUnity<DailyCopyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DailyCopyUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<DailyCopyUIRootLogic>.Instance.UpdataActiveInfo();
			}
			if (SingletonUnity<DailyActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DailyActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<DailyActivityUIRootLogic>.Instance.UpdataActiveInfo();
			}
		}
		return null;
	}
}
