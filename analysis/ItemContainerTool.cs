using System.Collections.Generic;
using SprotoType;

public class ItemContainerTool
{
	public static List<GameItem> GetConsignSellItem(ItemContainer Container)
	{
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < Container.ContainerSize; i++)
		{
			GameItem itemByIndex = Container.GetItemByIndex(i);
			if (!itemByIndex.IsEmpty())
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(itemByIndex.ItemId);
				if (!itemByIndex.BindFlag && itemDataByID.ConsignPrice > 0)
				{
					list.Add(itemByIndex);
				}
			}
		}
		return SortItemList(list);
	}

	public static List<GameItem> GetTargetTypeItem(ItemContainer Container, bool IsAll, GameDefine.ITEM_TYPE TargetType = GameDefine.ITEM_TYPE.INVALID, bool isUnBind = false, PROFESSION_TYPE prof = PROFESSION_TYPE.INVALID)
	{
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < Container.ContainerSize; i++)
		{
			GameItem itemByIndex = Container.GetItemByIndex(i);
			if (itemByIndex.IsEmpty() || (isUnBind && itemByIndex.BindFlag))
			{
				continue;
			}
			if (IsAll)
			{
				list.Add(itemByIndex);
				continue;
			}
			ItemData itemDataByID = DataManager.GetItemDataByID(itemByIndex.ItemId);
			if (itemDataByID.Type != TargetType)
			{
				continue;
			}
			if (prof == PROFESSION_TYPE.INVALID)
			{
				list.Add(itemByIndex);
			}
			else if (TargetType == GameDefine.ITEM_TYPE.EQUIP)
			{
				EquipData equipDataById = DataManager.GetEquipDataById(itemByIndex.ItemId);
				if (equipDataById.profession == prof)
				{
					list.Add(itemByIndex);
				}
			}
		}
		return SortItemList(list);
	}

	public static List<GameItem> GetTargetItemByID(ItemContainer Container, string needid)
	{
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < Container.ContainerSize; i++)
		{
			GameItem itemByIndex = Container.GetItemByIndex(i);
			if (!itemByIndex.IsEmpty() && itemByIndex.ItemId.Equals(needid))
			{
				list.Add(itemByIndex);
			}
		}
		return SortItemList(list);
	}

	public static List<GameItem> GetTargetTypeItemLevel(ItemContainer Container, GameDefine.ITEM_TYPE TargetType = GameDefine.ITEM_TYPE.INVALID, int level = 0)
	{
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < Container.ContainerSize; i++)
		{
			GameItem itemByIndex = Container.GetItemByIndex(i);
			if (!itemByIndex.IsEmpty())
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(itemByIndex.ItemId);
				if (itemDataByID.Type == TargetType && itemDataByID.Level <= level)
				{
					list.Add(itemByIndex);
				}
			}
		}
		return SortItemList(list);
	}

	public static List<GameItem> GetTargetPotionItemLevel(ItemContainer Container, int level = 0)
	{
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < Container.ContainerSize; i++)
		{
			GameItem itemByIndex = Container.GetItemByIndex(i);
			if (!itemByIndex.IsEmpty())
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(itemByIndex.ItemId);
				if ((itemDataByID.Type == GameDefine.ITEM_TYPE.POTION || itemDataByID.Type == GameDefine.ITEM_TYPE.POTION_2) && itemDataByID.Level <= level)
				{
					list.Add(itemByIndex);
				}
			}
		}
		return SortItemList(list);
	}

	public static bool isContainItem(List<GameItem> exclude, GameItem item)
	{
		if (exclude != null && exclude.Count > 0)
		{
			for (int i = 0; i < exclude.Count; i++)
			{
				if (exclude[i].IndexId == item.IndexId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static List<GameItem> GetSubItem(ItemContainer Container, GameDefine.ITEM_TYPE TargetType, int SubType, List<GameItem> exclude, PROFESSION_TYPE prof = PROFESSION_TYPE.INVALID)
	{
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < Container.ContainerSize; i++)
		{
			GameItem itemByIndex = Container.GetItemByIndex(i);
			if (itemByIndex.IsEmpty())
			{
				continue;
			}
			ItemData itemDataByID = DataManager.GetItemDataByID(itemByIndex.ItemId);
			if (itemDataByID.Type != TargetType || itemDataByID.SubType != SubType || isContainItem(exclude, itemByIndex))
			{
				continue;
			}
			if (prof == PROFESSION_TYPE.INVALID)
			{
				list.Add(itemByIndex);
			}
			else if (TargetType == GameDefine.ITEM_TYPE.EQUIP)
			{
				EquipData equipDataById = DataManager.GetEquipDataById(itemByIndex.ItemId);
				if (equipDataById.profession == prof)
				{
					list.Add(itemByIndex);
				}
			}
		}
		return SortItemList(list);
	}

	public static GameItem GetBestTargetEquipItem(ItemContainer Container, int SubType, PROFESSION_TYPE prof)
	{
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		GameItem gameItem = null;
		int num = int.MinValue;
		GameItem gameItem2 = null;
		ItemData itemData = null;
		EquipData equipData = null;
		for (int i = 0; i < Container.ContainerSize; i++)
		{
			gameItem2 = Container.GetItemByIndex(i);
			if (gameItem2.IsEmpty())
			{
				continue;
			}
			itemData = gameItem2.ItemData;
			if (itemData.Level <= level && itemData.Type == GameDefine.ITEM_TYPE.EQUIP && itemData.SubType == SubType)
			{
				equipData = DataManager.GetEquipDataById(itemData.ID);
				if (equipData.profession == prof && gameItem2.GetItemCombatVal() > num)
				{
					num = gameItem2.GetItemCombatVal();
					gameItem = gameItem2;
				}
			}
		}
		if (gameItem != null && !gameItem.IsEmpty())
		{
			return gameItem;
		}
		return null;
	}

	public static List<GameItem> GetTargetTypeItem(ItemContainer Container, int SubType, PROFESSION_TYPE prof, GameItem curequip = null)
	{
		List<GameItem> list = new List<GameItem>();
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		GameItem gameItem = null;
		ItemData itemData = null;
		EquipData equipData = null;
		for (int i = 0; i < Container.ContainerSize; i++)
		{
			gameItem = Container.GetItemByIndex(i);
			if (gameItem == null || gameItem.IsEmpty())
			{
				continue;
			}
			itemData = gameItem.ItemData;
			if (itemData.Level > level || itemData.Type != GameDefine.ITEM_TYPE.EQUIP || itemData.SubType != SubType)
			{
				continue;
			}
			equipData = DataManager.GetEquipDataById(itemData.ID);
			if (SubType == 0)
			{
				EquipData equipDataById = DataManager.GetEquipDataById(curequip.ItemId);
				if (equipDataById.WeaponType == equipData.WeaponType)
				{
					list.Add(gameItem);
				}
			}
			else if (equipData.profession == prof)
			{
				list.Add(gameItem);
			}
		}
		return list;
	}

	public static List<GameItem> GetOtherTypeItem(ItemContainer Container, GameDefine.ITEM_TYPE TargetType = GameDefine.ITEM_TYPE.INVALID, bool isUnBind = false)
	{
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < Container.ContainerSize; i++)
		{
			GameItem itemByIndex = Container.GetItemByIndex(i);
			if (!itemByIndex.IsEmpty() && (!isUnBind || !itemByIndex.BindFlag))
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(itemByIndex.ItemId);
				if (itemDataByID.Type != TargetType)
				{
					list.Add(itemByIndex);
				}
			}
		}
		return SortItemList(list);
	}

	public static List<GameItem> SortItemList(List<GameItem> itemList)
	{
		itemList.Sort(delegate(GameItem block1, GameItem block2)
		{
			if (block1.IsEmpty() && !block2.IsEmpty())
			{
				return 1;
			}
			if (!block1.IsEmpty() && block2.IsEmpty())
			{
				return -1;
			}
			if (block1.IsEmpty() && block2.IsEmpty())
			{
				return 0;
			}
			if (block1.ItemData.ItemType > block2.ItemData.ItemType)
			{
				return 1;
			}
			if (block1.ItemData.ItemType < block2.ItemData.ItemType)
			{
				return -1;
			}
			if (block1.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				if (block1.ItemData.SubType > block2.ItemData.SubType)
				{
					return 1;
				}
				if (block1.ItemData.SubType < block2.ItemData.SubType)
				{
					return -1;
				}
				if (block1.ItemData.Level > block2.ItemData.Level)
				{
					return -1;
				}
				if (block1.ItemData.Level < block2.ItemData.Level)
				{
					return 1;
				}
				if (block1.GetItemQuality() > block2.GetItemQuality())
				{
					return -1;
				}
				if (block1.GetItemQuality() < block2.GetItemQuality())
				{
					return 1;
				}
				return block1.ItemId.CompareTo(block2.ItemId);
			}
			if (block1.ItemData.Level > block2.ItemData.Level)
			{
				return -1;
			}
			return (block1.ItemData.Level < block2.ItemData.Level) ? 1 : block1.ItemId.CompareTo(block2.ItemId);
		});
		return itemList;
	}

	public static List<GameItem> SortBadgeItemList(List<GameItem> itemList)
	{
		itemList.Sort(delegate(GameItem block1, GameItem block2)
		{
			if (block1.IsEmpty() && !block2.IsEmpty())
			{
				return 1;
			}
			if (!block1.IsEmpty() && block2.IsEmpty())
			{
				return -1;
			}
			if (block1.IsEmpty() && block2.IsEmpty())
			{
				return 0;
			}
			BadgeData badgeDataById = DataManager.GetBadgeDataById(block1.ItemId);
			BadgeData badgeDataById2 = DataManager.GetBadgeDataById(block2.ItemId);
			if (badgeDataById.Color < badgeDataById2.Color)
			{
				return -1;
			}
			if (badgeDataById.Color > badgeDataById2.Color)
			{
				return 1;
			}
			if (badgeDataById.BadgeType < badgeDataById2.BadgeType)
			{
				return -1;
			}
			if (badgeDataById.BadgeType > badgeDataById2.BadgeType)
			{
				return 1;
			}
			if (badgeDataById.Lv < badgeDataById2.Lv)
			{
				return -1;
			}
			return (badgeDataById.Lv > badgeDataById2.Lv) ? 1 : 0;
		});
		return itemList;
	}

	public static List<GameItem> GetBadgeEquipItemList(List<GameItem> oldlist)
	{
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < ItemContainer.BADGE_EQUIPPACK_SIZE; i++)
		{
			list.Add(null);
		}
		for (int j = 0; j < oldlist.Count; j++)
		{
			if (!oldlist[j].IsEmpty())
			{
				int num = oldlist[j].Parm[0];
				if (num < list.Count)
				{
					list[num] = oldlist[j];
				}
			}
		}
		return list;
	}

	public static List<GameItem> GetFashionEquipItemList(List<GameItem> oldlist)
	{
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < ItemContainer.FASHION_EQUIPPACK_SIZE; i++)
		{
			list.Add(null);
		}
		for (int j = 0; j < oldlist.Count; j++)
		{
			if (!oldlist[j].IsEmpty())
			{
				ItemData itemData = oldlist[j].ItemData;
				int num = ChangeFashionEquipTypeToIndex((EQUIP_BACKPACK_TYPE)itemData.SubType);
				if (num < list.Count)
				{
					list[num] = oldlist[j];
				}
			}
		}
		return list;
	}

	public static List<GameItem> GetEquipItemList(List<GameItem> oldlist)
	{
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < ItemContainer.EQUIPPACK_SIZE; i++)
		{
			list.Add(new GameItem());
			list[i].EquipType = ChangeIndexToEquipType(i);
		}
		for (int j = 0; j < oldlist.Count; j++)
		{
			if (!oldlist[j].IsEmpty())
			{
				ItemData itemData = oldlist[j].ItemData;
				int num = ChangeEquipTypeToIndex((EQUIP_BACKPACK_TYPE)itemData.SubType);
				if (num < list.Count)
				{
					list[num] = oldlist[j];
				}
			}
		}
		return list;
	}

	public static List<GameItem> GetEquipItemList(ItemContainer container)
	{
		List<GameItem> list = new List<GameItem>();
		List<ItemData> list2 = new List<ItemData>();
		for (int i = 0; i < container.ContainerSize; i++)
		{
			if (!container.GetItemByIndex(i).IsEmpty())
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(container.GetItemByIndex(i).ItemId);
				list2.Add(itemDataByID);
			}
			else
			{
				list2.Add(null);
			}
		}
		List<int> list3 = new List<int>();
		for (int j = 0; j < container.ContainerSize; j++)
		{
			list3.Clear();
			for (int k = 0; k < list2.Count; k++)
			{
				if (list2[k] != null && list2[k].SubType == (int)ChangeIndexToEquipType(j))
				{
					list3.Add(k);
				}
			}
			if (list3.Count != 0)
			{
				for (int l = 0; l < list3.Count; l++)
				{
					list.Add(container.GetItemByIndex(list3[l]));
					list2[list3[l]] = null;
				}
			}
			else
			{
				list.Add(new GameItem());
				list[j].EquipType = ChangeIndexToEquipType(j);
			}
		}
		return list;
	}

	public static List<GameItem> GetFashionEquipItemList(ItemContainer container)
	{
		List<GameItem> list = new List<GameItem>();
		List<ItemData> list2 = new List<ItemData>();
		for (int i = 0; i < container.ContainerSize; i++)
		{
			if (!container.GetItemByIndex(i).IsEmpty())
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(container.GetItemByIndex(i).ItemId);
				list2.Add(itemDataByID);
			}
			else
			{
				list2.Add(null);
			}
		}
		List<int> list3 = new List<int>();
		for (int j = 0; j < container.ContainerSize; j++)
		{
			list3.Clear();
			for (int k = 0; k < list2.Count; k++)
			{
				if (list2[k] != null && list2[k].SubType == (int)ChangeIndexToFashionEquipType(j))
				{
					list3.Add(k);
				}
			}
			if (list3.Count != 0)
			{
				for (int l = 0; l < list3.Count; l++)
				{
					list.Add(container.GetItemByIndex(list3[l]));
					list2[list3[l]] = null;
				}
			}
			else
			{
				list.Add(new GameItem());
				list[j].EquipType = ChangeIndexToFashionEquipType(j);
			}
		}
		return list;
	}

	public static EQUIP_BACKPACK_TYPE ChangeIndexToFashionEquipType(int index)
	{
		return index switch
		{
			0 => EQUIP_BACKPACK_TYPE.HEAD, 
			1 => EQUIP_BACKPACK_TYPE.BODY, 
			2 => EQUIP_BACKPACK_TYPE.WEAPON, 
			3 => EQUIP_BACKPACK_TYPE.LEG, 
			_ => EQUIP_BACKPACK_TYPE.COUNT, 
		};
	}

	public static int ChangeFashionEquipTypeToIndex(EQUIP_BACKPACK_TYPE index)
	{
		return index switch
		{
			EQUIP_BACKPACK_TYPE.HEAD => 0, 
			EQUIP_BACKPACK_TYPE.BODY => 1, 
			EQUIP_BACKPACK_TYPE.WEAPON => 2, 
			EQUIP_BACKPACK_TYPE.LEG => 3, 
			_ => 0, 
		};
	}

	public static EQUIP_BACKPACK_TYPE ChangeIndexToEquipType(int index)
	{
		return index switch
		{
			0 => EQUIP_BACKPACK_TYPE.HEAD, 
			1 => EQUIP_BACKPACK_TYPE.BODY, 
			2 => EQUIP_BACKPACK_TYPE.BELT, 
			3 => EQUIP_BACKPACK_TYPE.LEG, 
			4 => EQUIP_BACKPACK_TYPE.NECKLACE, 
			5 => EQUIP_BACKPACK_TYPE.WEAPON, 
			_ => EQUIP_BACKPACK_TYPE.COUNT, 
		};
	}

	public static int ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE index)
	{
		return index switch
		{
			EQUIP_BACKPACK_TYPE.HEAD => 0, 
			EQUIP_BACKPACK_TYPE.BODY => 1, 
			EQUIP_BACKPACK_TYPE.BELT => 2, 
			EQUIP_BACKPACK_TYPE.LEG => 3, 
			EQUIP_BACKPACK_TYPE.NECKLACE => 4, 
			EQUIP_BACKPACK_TYPE.WEAPON => 5, 
			_ => 0, 
		};
	}

	public static GameItem ChangeNetItemToGameItem(gameitem netItem)
	{
		GameItem gameItem = new GameItem();
		gameItem.ItemId = netItem.itemId;
		if (netItem.HasParm)
		{
			gameItem.SetParm(netItem.parm);
		}
		if (netItem.HasBindflag)
		{
			gameItem.BindFlag = netItem.bindflag;
		}
		else
		{
			gameItem.BindFlag = false;
		}
		if (netItem.HasStack)
		{
			gameItem.StackNum = (int)netItem.stack;
		}
		else
		{
			gameItem.StackNum = 1;
		}
		if (netItem.HasIndexId)
		{
			gameItem.IndexId = netItem.indexId;
		}
		else
		{
			gameItem.IndexId = -1L;
		}
		if (netItem.HasQuality)
		{
			gameItem.Quality = (EQUIP_QUALITY)netItem.quality;
		}
		else
		{
			gameItem.Quality = EQUIP_QUALITY.INVALID;
		}
		if (netItem.HasLevel)
		{
			gameItem.ItemLevel = (int)netItem.level;
		}
		else
		{
			gameItem.ItemLevel = 0;
		}
		if (netItem.HasAppraise)
		{
			gameItem.Appraise = (int)netItem.appraise;
		}
		else
		{
			gameItem.Appraise = 0;
		}
		if (netItem.HasRandom_attri)
		{
			gameItem.Random_AttriDic = netItem.random_attri;
		}
		else
		{
			gameItem.Random_AttriDic = null;
		}
		if (netItem.HasInlay)
		{
			gameItem.InlayDic = netItem.inlay;
		}
		else
		{
			gameItem.InlayDic = null;
		}
		return gameItem;
	}
}
