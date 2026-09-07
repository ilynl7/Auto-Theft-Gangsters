using Sproto;
using SprotoType;

public class ret_consign_ask_items_info_handler
{
	public static SprotoTypeBase ret_consign_ask_items_info_request(SprotoTypeBase req)
	{
		if (req is ret_consign_ask_items_info.request request && SingletonUnity<ConsignRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ConsignRootLogic>.Instance.gameObject))
		{
			SingletonUnity<ConsignRootLogic>.Instance.RefreshOnBuyList(request);
		}
		return null;
	}
}
