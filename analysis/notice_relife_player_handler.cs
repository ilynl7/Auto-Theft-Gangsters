using Sproto;
using SprotoType;

public class notice_relife_player_handler
{
	public static SprotoTypeBase notice_relife_player_request(SprotoTypeBase req)
	{
		notice_relife_player.request request = req as notice_relife_player.request;
		if (request != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.RebirthUIRoot, delegate
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRoot);
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRootNew);
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopTopDiamondBuyRoot);
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopTopShopRoot);
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NumRoot);
				SingletonUnity<RebirthUIRootLogic>.Instance.Reset(request);
			});
		}
		return null;
	}
}
