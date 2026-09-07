using Sproto;
using SprotoType;

public class ret_require_vip_reward_handler
{
	public static SprotoTypeBase ret_require_vip_reward_request(SprotoTypeBase req)
	{
		if (req is ret_require_vip_reward.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.SyncVipInfo(request);
			if (SingletonUnity<MonthlyCardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MonthlyCardRootLogic>.Instance.gameObject))
			{
				SingletonUnity<MonthlyCardRootLogic>.Instance.RefershInfo(request);
			}
		}
		return null;
	}
}
