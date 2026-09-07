using System.Collections.Generic;
using Sproto;
using SprotoType;

public class ret_buy_shop_item_handler
{
	public static SprotoTypeBase ret_buy_shop_item_request(SprotoTypeBase req)
	{
		if (req is ret_buy_shop_item.request request)
		{
			if (SingletonUnity<PopShopRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PopShopRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PopShopRootLogic>.Instance.UpdateShopItem(request.shop_item, request.type);
			}
			if (SingletonUnity<ShopTabRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ShopTabRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ShopTabRootLogic>.Instance.UpdateShopItem(request.shop_item, request.type);
			}
			if (SingletonUnity<PopTopShopRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PopTopShopRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PopTopShopRootLogic>.Instance.UpdateShopItem(request.shop_item, request.type);
			}
			ItemData itemDataByID = DataManager.GetItemDataByID(request.shop_item.ItemID);
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.BOX)
			{
				ItemContainer itemBackPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack;
				List<GameItem> itemByItemId = itemBackPack.GetItemByItemId(itemDataByID.ID);
				if (itemByItemId != null && itemByItemId.Count > 0)
				{
					open_item_package.request request2 = new open_item_package.request();
					request2.indexId = itemByItemId[0].IndexId;
					request2.count = (int)request.count;
					if (request2.count > 99)
					{
						request2.count = 99L;
					}
					NetLogic.GetInstance().Send<Protocol.open_item_package>(request2);
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OpenBoxRoot, delegate
					{
						SingletonUnity<OpenBoxRootLogic>.Instance.Reset();
						if (TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_CLICK_BUY)
						{
							TutorialManager.MoveNext();
						}
					});
				}
			}
			int num = (int)request.count;
			string text = "1_5";
			text = ((num <= 5) ? "1_5" : ((num <= 10) ? "6_10" : ((num <= 20) ? "11_20" : ((num <= 50) ? "21_50" : ((num > 100) ? "100+" : "50_100")))));
			long type = request.type;
			if (type >= 0 && type <= 7)
			{
				switch (type)
				{
				case 2L:
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_bigsale", $"shopitem_{request.shop_item.ID}", "buytimes");
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_bigsale", $"shopitem_{request.shop_item.ID}", $"buynum_{text}");
					break;
				case 1L:
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_equip", $"shopitem_{request.shop_item.ID}", "buytimes");
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_equip", $"shopitem_{request.shop_item.ID}", $"buynum_{text}");
					break;
				case 0L:
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_tool", $"shopitem_{request.shop_item.ID}", "buytimes");
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_tool", $"shopitem_{request.shop_item.ID}", $"buynum_{text}");
					break;
				case 3L:
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_guild", $"shopitem_{request.shop_item.ID}", "buytimes");
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_guild", $"shopitem_{request.shop_item.ID}", $"buynum_{text}");
					break;
				case 6L:
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_battle", $"shopitem_{request.shop_item.ID}", "buytimes");
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_battle", $"shopitem_{request.shop_item.ID}", $"buynum_{text}");
					break;
				case 7L:
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_activity", $"shopitem_{request.shop_item.ID}", "buytimes");
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_activity", $"shopitem_{request.shop_item.ID}", $"buynum_{text}");
					break;
				}
			}
		}
		return null;
	}
}
