using System.Collections.Generic;
using Sproto;
using SprotoType;

public class ret_consign_ask_my_items_handler
{
	public static SprotoTypeBase ret_consign_ask_my_items_request(SprotoTypeBase req)
	{
		if (req is ret_consign_ask_my_items.request { success: 0L } request)
		{
			if (request.HasConsign_items && request.consign_items.Count > 0)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SaleNowList = new List<consign_item>(request.consign_items.Values);
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SaleNowList = null;
			}
			if (SingletonUnity<ConsignRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ConsignRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ConsignRootLogic>.Instance.RefreshOnSaleNow();
			}
		}
		return null;
	}
}
