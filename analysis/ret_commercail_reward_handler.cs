using Sproto;
using SprotoType;

public class ret_commercail_reward_handler
{
	public static SprotoTypeBase ret_commercail_reward_request(SprotoTypeBase req)
	{
		if (req is ret_commercail_reward.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonDontDestoryUnity<GameManager>.Instance.UpdateComsumed(request);
			if (request.state != 0L)
			{
				return null;
			}
			if (request.HasType)
			{
				GameDefine.CommercailTYPE commercailTYPE = (GameDefine.CommercailTYPE)request.type;
				switch (commercailTYPE)
				{
				case GameDefine.CommercailTYPE.LEVEL:
					if (request.HasParm2)
					{
						if (SingletonUnity<LevelPackRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<LevelPackRewardRootLogic>.Instance.gameObject))
						{
							SingletonUnity<LevelPackRewardRootLogic>.Instance.UpdateInfo(request.parm2);
						}
						SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.UpdateLevelPack(request.parm2);
					}
					break;
				case GameDefine.CommercailTYPE.INVEST:
					if (request.HasParm2)
					{
						if (SingletonUnity<InvestRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<InvestRewardRootLogic>.Instance.gameObject))
						{
							SingletonUnity<InvestRewardRootLogic>.Instance.UpdateInfo(request.parm2);
						}
						SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.UpdateInvestPack(request.parm2);
					}
					break;
				case GameDefine.CommercailTYPE.FIRSTBUY:
					if (request.HasParm2)
					{
						if (SingletonUnity<FirstBuyRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FirstBuyRootLogic>.Instance.gameObject))
						{
							SingletonUnity<FirstBuyRootLogic>.Instance.UpdateInfo(request.parm2);
						}
						if (SingletonUnity<FunctionBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionBtnRootLogic>.Instance.gameObject))
						{
							SingletonUnity<FunctionBtnRootLogic>.Instance.HideFirstSaleBtn();
						}
					}
					break;
				case GameDefine.CommercailTYPE.BIGSALE:
					if (request.HasParm2)
					{
						SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.UpdateBigPack(request.parm2);
						if (SingletonUnity<BigPackRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<BigPackRootLogic>.Instance.gameObject))
						{
							SingletonUnity<BigPackRootLogic>.Instance.UpdateInfo(request.parm2);
						}
						if (SingletonUnity<MysteryShopRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MysteryShopRootLogic>.Instance.gameObject))
						{
							SingletonUnity<MysteryShopRootLogic>.Instance.UpdateInfo(request.parm2);
						}
						if (SingletonUnity<ShopBigSaleRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ShopBigSaleRootLogic>.Instance.gameObject))
						{
							SingletonUnity<ShopBigSaleRootLogic>.Instance.UpdateInfo(request.parm2);
						}
					}
					break;
				case GameDefine.CommercailTYPE.DAILYBUY:
					if (request.HasParm2)
					{
						if (SingletonUnity<DailyBuyPackRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DailyBuyPackRootLogic>.Instance.gameObject))
						{
							SingletonUnity<DailyBuyPackRootLogic>.Instance.UpdateInfo(request.parm2);
						}
						SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.UpdateDailyBuy(request.parm2);
					}
					break;
				case GameDefine.CommercailTYPE.DAILYACTIVE:
					if (request.HasParm2)
					{
						if (SingletonUnity<DailyActiveRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DailyActiveRewardRootLogic>.Instance.gameObject))
						{
							SingletonUnity<DailyActiveRewardRootLogic>.Instance.UpdateInfo(request.parm2);
						}
						if (SingletonUnity<OpenBoxRootLogic>.Exists)
						{
							SingletonUnity<OpenBoxRootLogic>.Instance.ShowTipPage(request);
						}
						if (SingletonUnity<DailyRewardNewLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DailyRewardNewLogic>.Instance.gameObject))
						{
							SingletonUnity<DailyRewardNewLogic>.Instance.UpdateInfo(request.parm2);
						}
						SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.UpdateDailyReward(request.parm2);
					}
					break;
				case GameDefine.CommercailTYPE.SHOP:
					if (request.HasProductId)
					{
						PurchaseData purchaseDataBuyId = DataManager.GetPurchaseDataBuyId(request.productId);
						if (purchaseDataBuyId.PriceType == 2)
						{
							ItemData itemDataByID = DataManager.GetItemDataByID(GameDefine.DIAMOND_ITEM_ID);
							SimpleRewardRootLogic.AddReward(itemDataByID, purchaseDataBuyId.PriceCost, itemDataByID.Quality);
							SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_dollar", $"dollar_{request.productId}", "buytimes");
						}
						ask_shop_list.request request2 = new ask_shop_list.request();
						request2.type = 4L;
						NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request2);
						WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
					}
					break;
				case GameDefine.CommercailTYPE.VIP:
					if (request.HasParm2)
					{
						if (SingletonUnity<MonthlyCardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MonthlyCardRootLogic>.Instance.gameObject))
						{
							SingletonUnity<MonthlyCardRootLogic>.Instance.UpdateInfo(request.parm2);
						}
						SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.UpdateVipState(request.parm2);
					}
					break;
				}
				if (commercailTYPE != GameDefine.CommercailTYPE.DAILYACTIVE && request.HasItems && request.items.Count > 0)
				{
					SimpleRewardRootLogic.AddRewards(request.items);
				}
			}
		}
		return null;
	}
}
