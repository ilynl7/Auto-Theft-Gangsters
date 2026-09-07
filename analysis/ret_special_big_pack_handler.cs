using Sproto;
using SprotoType;

public class ret_special_big_pack_handler
{
	public static SprotoTypeBase ret_special_big_pack_request(SprotoTypeBase req)
	{
		if (req is ret_special_big_pack.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (SingletonUnity<MysteryShopRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MysteryShopRootLogic>.Instance.gameObject))
			{
				SingletonUnity<MysteryShopRootLogic>.Instance.Reset(request);
			}
			if (SingletonUnity<ShopBigSaleRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ShopBigSaleRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ShopBigSaleRootLogic>.Instance.Reset(request);
			}
		}
		return null;
	}
}
