using Sproto;
using SprotoType;

public class ret_request_daily_buy_handler
{
	public static SprotoTypeBase ret_request_daily_buy_request(SprotoTypeBase req)
	{
		if (req is ret_request_daily_buy.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.welfareData.InitDailyBuy(request);
			if (SingletonUnity<DailyBuyPackRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DailyBuyPackRootLogic>.Instance.gameObject))
			{
				SingletonUnity<DailyBuyPackRootLogic>.Instance.Reset(request);
			}
		}
		return null;
	}
}
