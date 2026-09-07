using Sproto;
using SprotoType;

public class ret_consign_buy_item_handler
{
	public static SprotoTypeBase ret_consign_buy_item_request(SprotoTypeBase req)
	{
		if (req is ret_consign_buy_item.request request)
		{
			if (request.success == 0L)
			{
				if (SingletonUnity<ConsignRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ConsignRootLogic>.Instance.gameObject))
				{
					SingletonUnity<ConsignRootLogic>.Instance.BuySuccess(request.id);
				}
				NoticeLogic.AddNotifyData("#{101230}");
				ItemData itemDataByID = DataManager.GetItemDataByID(request.itemId);
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Tradebuy", "buytimes", "times");
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Tradebuy", $"buytype_{itemDataByID.Type}", $"buy_{itemDataByID.ID}");
			}
			else
			{
				NoticeLogic.AddNotifyData("#{101233}");
			}
		}
		return null;
	}
}
