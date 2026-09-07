using Sproto;
using SprotoType;

public class ret_buy_car_shop_handler
{
	public static SprotoTypeBase ret_buy_car_shop_request(SprotoTypeBase req)
	{
		if (req is ret_buy_car_shop.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (SingletonUnity<PlayerCarRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PlayerCarRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PlayerCarRootLogic>.Instance.UpdateCarPage(request);
			}
		}
		return null;
	}
}
