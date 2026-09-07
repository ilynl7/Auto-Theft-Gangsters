using Sproto;
using SprotoType;

public class ret_consign_cancel_sale_handler
{
	public static SprotoTypeBase ret_consign_cancel_sale_request(SprotoTypeBase req)
	{
		if (req is ret_consign_cancel_sale.request request)
		{
			if (request.success == 0L)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CancelSuccess(request.id);
				if (SingletonUnity<ConsignRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ConsignRootLogic>.Instance.gameObject))
				{
					SingletonUnity<ConsignRootLogic>.Instance.RefreshOnSaleNow();
					SingletonUnity<ConsignRootLogic>.Instance.BuySuccess(request.id);
				}
				NoticeLogic.AddNotifyData("#{101231}");
			}
			else
			{
				NoticeLogic.AddNotifyData("#{101234}");
			}
		}
		return null;
	}
}
