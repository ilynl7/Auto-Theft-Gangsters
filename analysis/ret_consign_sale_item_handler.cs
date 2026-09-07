using Sproto;
using SprotoType;

public class ret_consign_sale_item_handler
{
	public static SprotoTypeBase ret_consign_sale_item_request(SprotoTypeBase req)
	{
		if (req is ret_consign_sale_item.request request)
		{
			if (request.success == 0L)
			{
				GameDefine.ITEM_TYPE iTEM_TYPE = (GameDefine.ITEM_TYPE)request.itemType;
				GameItem gameItem = null;
				switch (iTEM_TYPE)
				{
				case GameDefine.ITEM_TYPE.BADGE:
				{
					ItemContainer itemContainer3 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(ITEM_CONTAINER_TYPE.BADGE_BACKPACK);
					if (itemContainer3 == null)
					{
						break;
					}
					gameItem = itemContainer3.GetItemByIndexId(request.indexId);
					if (gameItem != null)
					{
						if (request.HasGameitem)
						{
							gameItem.UpdateItem(request.gameitem);
						}
						else
						{
							gameItem.Reset();
						}
					}
					if (SingletonUnity<ConsignRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ConsignRootLogic>.Instance.gameObject))
					{
						SingletonUnity<ConsignRootLogic>.Instance.SellSuccess();
					}
					break;
				}
				case GameDefine.ITEM_TYPE.EQUIP:
				{
					ItemContainer itemContainer2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(ITEM_CONTAINER_TYPE.EQUIP_BACKPACK);
					if (itemContainer2 == null)
					{
						break;
					}
					gameItem = itemContainer2.GetItemByIndexId(request.indexId);
					if (gameItem != null)
					{
						if (request.HasGameitem)
						{
							gameItem.UpdateItem(request.gameitem);
						}
						else
						{
							gameItem.Reset();
						}
					}
					if (SingletonUnity<ConsignRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ConsignRootLogic>.Instance.gameObject))
					{
						SingletonUnity<ConsignRootLogic>.Instance.SellSuccess();
					}
					break;
				}
				default:
				{
					ItemContainer itemContainer = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(ITEM_CONTAINER_TYPE.ITEM_BACKPACK);
					if (itemContainer == null)
					{
						break;
					}
					gameItem = itemContainer.GetItemByIndexId(request.indexId);
					if (gameItem != null)
					{
						if (request.HasGameitem)
						{
							gameItem.UpdateItem(request.gameitem);
						}
						else
						{
							gameItem.Reset();
						}
					}
					if (SingletonUnity<ConsignRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ConsignRootLogic>.Instance.gameObject))
					{
						SingletonUnity<ConsignRootLogic>.Instance.SellSuccess();
					}
					break;
				}
				}
				NoticeLogic.AddNotifyData("#{101232}");
				if (gameItem != null)
				{
					ItemData itemData = gameItem.ItemData;
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Tradesell", "selltimes", "times");
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Tradesell", $"selltype_{itemData.Type}", $"sell_{itemData.ID}");
				}
			}
			else if (request.success == 3)
			{
				NoticeLogic.AddNotifyData("#{101243}");
			}
			else
			{
				NoticeLogic.AddNotifyData("#{101235}");
			}
		}
		return null;
	}
}
