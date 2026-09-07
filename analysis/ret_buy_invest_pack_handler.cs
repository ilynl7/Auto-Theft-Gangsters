using Sproto;
using SprotoType;

public class ret_buy_invest_pack_handler
{
	public static SprotoTypeBase ret_buy_invest_pack_request(SprotoTypeBase req)
	{
		if (req is ret_buy_invest_pack.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.InitInvestPack(request);
			if (SingletonUnity<InvestRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<InvestRewardRootLogic>.Instance.gameObject))
			{
				SingletonUnity<InvestRewardRootLogic>.Instance.UpdateInfo(request);
			}
		}
		return null;
	}
}
