using System.Collections.Generic;
using Sproto;
using SprotoType;

public class sync_badgepack_item_handler
{
	public static SprotoTypeBase sync_badgepack_item_request(SprotoTypeBase req)
	{
		if (req is sync_badgepack_item.request request)
		{
			ItemContainer itemContainer = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(ITEM_CONTAINER_TYPE.BADGE_BACKPACK);
			if (itemContainer != null && request.HasGameitems)
			{
				foreach (KeyValuePair<long, gameitem> gameitem in request.gameitems)
				{
					gameitem value = gameitem.Value;
					GameItem itemByIndexId = itemContainer.GetItemByIndexId(value.indexId);
					if (itemByIndexId != null)
					{
						itemByIndexId.ItemId = value.itemId;
						itemByIndexId.ContainerType = ITEM_CONTAINER_TYPE.BADGE_BACKPACK;
						if (value.HasParm)
						{
							itemByIndexId.SetParm(value.parm);
						}
						if (value.HasBindflag)
						{
							itemByIndexId.BindFlag = value.bindflag;
						}
						else
						{
							itemByIndexId.BindFlag = false;
						}
						if (value.HasStack)
						{
							itemByIndexId.StackNum = (int)value.stack;
						}
						else
						{
							itemByIndexId.StackNum = 1;
						}
						if (value.HasIndexId)
						{
							itemByIndexId.IndexId = value.indexId;
						}
						else
						{
							itemByIndexId.IndexId = -1L;
						}
						if (value.HasQuality)
						{
							itemByIndexId.Quality = (EQUIP_QUALITY)value.quality;
						}
						else
						{
							itemByIndexId.Quality = EQUIP_QUALITY.INVALID;
						}
						if (value.HasLevel)
						{
							itemByIndexId.ItemLevel = (int)value.level;
						}
						else
						{
							itemByIndexId.ItemLevel = 0;
						}
						if (value.HasAppraise)
						{
							itemByIndexId.Appraise = (int)value.appraise;
						}
						else
						{
							itemByIndexId.Appraise = 0;
						}
						if (value.HasRandom_attri)
						{
							itemByIndexId.Random_AttriDic = value.random_attri;
						}
						else
						{
							itemByIndexId.Random_AttriDic = null;
						}
						if (value.HasInlay)
						{
							itemByIndexId.InlayDic = value.inlay;
						}
						else
						{
							itemByIndexId.InlayDic = null;
						}
					}
				}
				if (UIUpdateEvent.SyncBackPackEvent != null)
				{
					UIUpdateEvent.SyncBackPackEvent();
				}
			}
		}
		return null;
	}
}
