using Sproto;
using SprotoType;

public class ret_request_first_buy_handler
{
	public static SprotoTypeBase ret_request_first_buy_request(SprotoTypeBase req)
	{
		if (req is ret_request_first_buy.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (SingletonUnity<FirstBuyRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FirstBuyRootLogic>.Instance.gameObject))
			{
				SingletonUnity<FirstBuyRootLogic>.Instance.Reset(request);
			}
		}
		return null;
	}
}
