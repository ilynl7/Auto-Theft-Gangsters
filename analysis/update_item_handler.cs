using Sproto;
using SprotoType;

public class update_item_handler
{
	public static SprotoTypeBase update_item_request(SprotoTypeBase req)
	{
		if (req is update_item.request request)
		{
			int num = (int)request.containertype;
			long indexId = request.indexId;
			ITEM_CONTAINER_TYPE iTEM_CONTAINER_TYPE = (ITEM_CONTAINER_TYPE)num;
			ItemContainer itemContainer = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(iTEM_CONTAINER_TYPE);
			if (itemContainer != null)
			{
				GameItem itemByIndexId = itemContainer.GetItemByIndexId(indexId);
				if (itemByIndexId != null)
				{
					if (request.HasGameitem)
					{
						gameitem gameitem = request.gameitem;
						itemByIndexId.ItemId = gameitem.itemId;
						itemByIndexId.ContainerType = iTEM_CONTAINER_TYPE;
						if (gameitem.HasParm)
						{
							itemByIndexId.SetParm(gameitem.parm);
						}
						if (gameitem.HasBindflag)
						{
							itemByIndexId.BindFlag = gameitem.bindflag;
						}
						else
						{
							itemByIndexId.BindFlag = false;
						}
						if (gameitem.HasStack)
						{
							itemByIndexId.StackNum = (int)gameitem.stack;
						}
						else
						{
							itemByIndexId.StackNum = 1;
						}
						if (gameitem.HasIndexId)
						{
							itemByIndexId.IndexId = gameitem.indexId;
						}
						else
						{
							itemByIndexId.IndexId = -1L;
						}
						if (gameitem.HasQuality)
						{
							itemByIndexId.Quality = (EQUIP_QUALITY)gameitem.quality;
						}
						else
						{
							itemByIndexId.Quality = EQUIP_QUALITY.INVALID;
						}
						if (gameitem.HasLevel)
						{
							itemByIndexId.ItemLevel = (int)gameitem.level;
							if (itemByIndexId.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
							{
								SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.SetEquipEnhanceLevel(itemByIndexId.ItemData.SubType, itemByIndexId.ItemLevel);
							}
						}
						else
						{
							itemByIndexId.ItemLevel = 0;
						}
						if (gameitem.HasAppraise)
						{
							itemByIndexId.Appraise = (int)gameitem.appraise;
						}
						else
						{
							itemByIndexId.Appraise = 0;
						}
						if (gameitem.HasRandom_attri)
						{
							itemByIndexId.Random_AttriDic = gameitem.random_attri;
						}
						else
						{
							itemByIndexId.Random_AttriDic = null;
						}
						if (gameitem.HasInlay)
						{
							itemByIndexId.InlayDic = gameitem.inlay;
						}
						else
						{
							itemByIndexId.InlayDic = null;
						}
						if (SingletonUnity<EnhanceUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<EnhanceUIRootLogic>.Instance.gameObject))
						{
							SingletonUnity<EnhanceUIRootLogic>.Instance.UpdateQHInfo(itemByIndexId);
						}
						if (iTEM_CONTAINER_TYPE == ITEM_CONTAINER_TYPE.EQUIP_BACKPACK || iTEM_CONTAINER_TYPE == ITEM_CONTAINER_TYPE.FASHION_BACKPACK)
						{
							NewItemTipsRootLogic.AddNewItem(itemByIndexId);
						}
					}
					else
					{
						itemByIndexId.Reset();
					}
					switch (iTEM_CONTAINER_TYPE)
					{
					case ITEM_CONTAINER_TYPE.EQUIP_BACKPACK:
					case ITEM_CONTAINER_TYPE.ITEM_BACKPACK:
					case ITEM_CONTAINER_TYPE.FASHION_BACKPACK:
						if (UIUpdateEvent.UpdateBackPackEvent != null)
						{
							UIUpdateEvent.UpdateBackPackEvent();
						}
						break;
					case ITEM_CONTAINER_TYPE.EQUIPPACK:
						if (SingletonUnity<PlayerInfoMenuRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PlayerInfoMenuRootLogic>.Instance.gameObject))
						{
							SingletonUnity<PlayerInfoMenuRootLogic>.Instance.UpdateEquipPack();
						}
						break;
					case ITEM_CONTAINER_TYPE.BADGE_BACKPACK:
						if (UIUpdateEvent.UpdateBackPackEvent != null)
						{
							UIUpdateEvent.UpdateBackPackEvent();
						}
						break;
					case ITEM_CONTAINER_TYPE.BADGE_EQUIPPACK:
						if (SingletonUnity<PlayerInfoMenuRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PlayerInfoMenuRootLogic>.Instance.gameObject))
						{
							SingletonUnity<PlayerInfoMenuRootLogic>.Instance.UpdateEquipPack();
						}
						break;
					case ITEM_CONTAINER_TYPE.FASHION_EQUIPPACK:
						if (SingletonUnity<PlayerInfoMenuRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PlayerInfoMenuRootLogic>.Instance.gameObject))
						{
							SingletonUnity<PlayerInfoMenuRootLogic>.Instance.UpdateEquipPack();
						}
						break;
					}
					SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.UpdateEquipsTips();
					SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.UpdateEnhanceTips();
					if (SingletonUnity<EnhanceUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<EnhanceUIRootLogic>.Instance.gameObject))
					{
						SingletonUnity<EnhanceUIRootLogic>.Instance.RefershUI();
					}
					if (SingletonUnity<RefineUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RefineUIRootLogic>.Instance.gameObject))
					{
						SingletonUnity<RefineUIRootLogic>.Instance.RefershItemUI();
					}
					if (SingletonUnity<RebirthUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RebirthUIRootLogic>.Instance.gameObject))
					{
						SingletonUnity<RebirthUIRootLogic>.Instance.refershItemInfo();
					}
					if (SingletonUnity<DailyCopyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DailyCopyUIRootLogic>.Instance.gameObject))
					{
						SingletonUnity<DailyCopyUIRootLogic>.Instance.UpdateWipeoutItem(itemByIndexId.ItemId);
					}
					if (SingletonUnity<EquipInhertRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<EquipInhertRootLogic>.Instance.gameObject))
					{
						SingletonUnity<EquipInhertRootLogic>.Instance.UpdateInhertItem(itemByIndexId);
					}
				}
			}
		}
		return null;
	}
}
