using System.Collections.Generic;
using UnityEngine;

public class ItemContainer
{
	public static int ITEM_BACKPACK_SIZE = 100;

	public static int BACKPACK_MAXSIZE = 100;

	public static int EQUIPPACK_SIZE = 6;

	public static int STORAGEPACK_MAXSIZE = 100;

	public static int STORAGEPACK_SIZE = 100;

	public static int EQUIP_BACKPACK_SIZE = 100;

	public static int BADGE_BACKPACK_SIZE = 100;

	public static int BADGE_EQUIPPACK_SIZE = 5;

	public static int FASHION_BACKPACK_SIZE = 100;

	public static int FASHION_EQUIPPACK_SIZE = 4;

	private List<GameItem> mItemList = new List<GameItem>();

	private int mContainerSize;

	private ITEM_CONTAINER_TYPE mContainerType = ITEM_CONTAINER_TYPE.INVALID;

	public List<GameItem> ItemList => mItemList;

	public int ContainerSize => mContainerSize;

	public ITEM_CONTAINER_TYPE ContainType
	{
		get
		{
			return mContainerType;
		}
		set
		{
			mContainerType = value;
		}
	}

	public ItemContainer(int size, ITEM_CONTAINER_TYPE type)
	{
		mContainerSize = size;
		mContainerType = type;
		for (int i = 0; i < mContainerSize; i++)
		{
			mItemList.Add(new GameItem());
			mItemList[i].ContainerType = mContainerType;
		}
	}

	public ItemContainer(List<GameItem> list, ITEM_CONTAINER_TYPE type)
	{
		if (list != null)
		{
			mContainerSize = list.Count;
		}
		else
		{
			mContainerSize = 0;
		}
		mContainerType = type;
		mItemList = list;
	}

	public void AddContainerSize(int addNum)
	{
		mContainerSize += addNum;
		for (int i = 0; i < addNum; i++)
		{
			mItemList.Add(new GameItem());
		}
	}

	public List<GameItem> GetItemByItemId(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		List<GameItem> list = new List<GameItem>();
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i].ItemId.Equals(id))
			{
				list.Add(mItemList[i]);
			}
		}
		return list;
	}

	public GameItem GetItemByItemId2(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i].ItemId.Equals(id))
			{
				return mItemList[i];
			}
		}
		return null;
	}

	public GameItem GetItemByIndex(int index)
	{
		if (index >= 0 && index < mItemList.Count)
		{
			return mItemList[index];
		}
		return null;
	}

	public GameItem GetItemByIndexId(long indexId)
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i].IndexId == indexId)
			{
				return mItemList[i];
			}
		}
		return GetItemByIndex(GetFirstEmptyItemIndex());
	}

	public GameItem GetItemNoEmptyByIndexId(long indexId)
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i].IndexId == indexId)
			{
				return mItemList[i];
			}
		}
		return null;
	}

	public int GetItemCount()
	{
		int num = 0;
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (!mItemList[i].IsEmpty())
			{
				num++;
			}
		}
		return num;
	}

	public int GetContainerEmptyNum()
	{
		return mContainerSize - GetItemCount();
	}

	public bool IsFull()
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i].IsEmpty())
			{
				return false;
			}
		}
		return true;
	}

	public int GetItemStackNumById(string id)
	{
		int num = 0;
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i].ItemId.Equals(id))
			{
				num += mItemList[i].StackNum;
			}
		}
		return num;
	}

	public EquipData GetEquipWeaponData(EQUIP_BACKPACK_TYPE target)
	{
		EquipData result = null;
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i] != null && !string.IsNullOrEmpty(mItemList[i].ItemId))
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(mItemList[i].ItemId);
				if (itemDataByID != null && (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP || itemDataByID.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP) && itemDataByID.SubType == (int)target)
				{
					return DataManager.GetEquipDataById(itemDataByID.ID);
				}
			}
		}
		return result;
	}

	public string GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE target, PROFESSION_TYPE profession, bool isFashion = false)
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i] != null && !string.IsNullOrEmpty(mItemList[i].ItemId))
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(mItemList[i].ItemId);
				if (itemDataByID != null && (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP || itemDataByID.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP) && itemDataByID.SubType == (int)target)
				{
					EquipData equipDataById = DataManager.GetEquipDataById(itemDataByID.ID);
					return equipDataById.ModelId;
				}
			}
		}
		if (isFashion)
		{
			return string.Empty;
		}
		return profession switch
		{
			PROFESSION_TYPE.XD => target switch
			{
				EQUIP_BACKPACK_TYPE.HEAD => GameDefine.XD_DefaultModel[1], 
				EQUIP_BACKPACK_TYPE.BODY => GameDefine.XD_DefaultModel[2], 
				EQUIP_BACKPACK_TYPE.LEG => GameDefine.XD_DefaultModel[3], 
				EQUIP_BACKPACK_TYPE.WEAPON => GameDefine.XD_DefaultModel[0], 
				_ => string.Empty, 
			}, 
			PROFESSION_TYPE.QJ => target switch
			{
				EQUIP_BACKPACK_TYPE.HEAD => GameDefine.QJ_DefaultModel[1], 
				EQUIP_BACKPACK_TYPE.BODY => GameDefine.QJ_DefaultModel[2], 
				EQUIP_BACKPACK_TYPE.LEG => GameDefine.QJ_DefaultModel[3], 
				EQUIP_BACKPACK_TYPE.WEAPON => GameDefine.QJ_DefaultModel[0], 
				_ => string.Empty, 
			}, 
			_ => target switch
			{
				EQUIP_BACKPACK_TYPE.HEAD => GameDefine.NQS_DefaultModel[1], 
				EQUIP_BACKPACK_TYPE.BODY => GameDefine.NQS_DefaultModel[2], 
				EQUIP_BACKPACK_TYPE.LEG => GameDefine.NQS_DefaultModel[3], 
				EQUIP_BACKPACK_TYPE.WEAPON => GameDefine.NQS_DefaultModel[0], 
				_ => string.Empty, 
			}, 
		};
	}

	public GameItem GetEnhanceItem()
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i] != null)
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(mItemList[i].ItemId);
				if (itemDataByID != null && itemDataByID.Type == GameDefine.ITEM_TYPE.ENHANCE_ITEM && itemDataByID.ID == "3001")
				{
					return mItemList[i];
				}
			}
		}
		return null;
	}

	public bool IsHaveWeapon()
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i] != null)
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(mItemList[i].ItemId);
				if (itemDataByID != null && itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP && itemDataByID.SubType == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public GameItem getWeapon()
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i] != null)
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(mItemList[i].ItemId);
				if (itemDataByID != null && itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP && itemDataByID.SubType == 0)
				{
					return mItemList[i];
				}
			}
		}
		return null;
	}

	public GameItem GetEquipByEquipType(EQUIP_BACKPACK_TYPE targetType)
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i] != null)
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(mItemList[i].ItemId);
				if (itemDataByID != null && itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP && itemDataByID.SubType == (int)targetType)
				{
					return mItemList[i];
				}
			}
		}
		return null;
	}

	public int GetFirstEmptyItemIndex()
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i].IsEmpty())
			{
				return i;
			}
		}
		return -1;
	}

	public GameItem GetFirstNoEmptyItem()
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			if (mItemList[i] != null)
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(mItemList[i].ItemId);
				if (itemDataByID != null && itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					return mItemList[i];
				}
			}
		}
		return null;
	}

	public bool AddItem(GameItem gameItem)
	{
		ItemData itemDataByID = DataManager.GetItemDataByID(gameItem.ItemId);
		if (itemDataByID == null)
		{
			Debug.Log("itemData == null");
			return false;
		}
		if (!IsContainerHaveEnoughSize(gameItem))
		{
			Debug.Log("No Space For Item");
			return false;
		}
		int num = -1;
		if (itemDataByID.Stack > 1)
		{
			List<GameItem> itemByItemId = GetItemByItemId(itemDataByID.ID);
			if (itemByItemId != null && itemByItemId.Count > 0)
			{
				for (int i = 0; i < itemByItemId.Count; i++)
				{
					if (!itemByItemId[i].IsFull())
					{
						if (itemByItemId[i].GetItemLeftSpace() >= gameItem.StackNum)
						{
							itemByItemId[i].StackNum += gameItem.StackNum;
							return true;
						}
						gameItem.StackNum -= itemByItemId[i].GetItemLeftSpace();
						itemByItemId[i].StackNum += itemByItemId[i].GetItemLeftSpace();
					}
				}
			}
			while (gameItem.StackNum > 0)
			{
				num = GetFirstEmptyItemIndex();
				if (num == -1)
				{
					Debug.Log("Container Is Full");
					return false;
				}
				if (gameItem.StackNum <= itemDataByID.Stack)
				{
					mItemList[num] = gameItem;
					mItemList[num].ItemId = gameItem.ItemId;
					mItemList[num].StackNum = gameItem.StackNum;
					return true;
				}
				mItemList[num].ItemId = gameItem.ItemId;
				mItemList[num].StackNum = itemDataByID.Stack;
				gameItem.StackNum -= itemDataByID.Stack;
			}
			Debug.Log("Container Is Full");
			return false;
		}
		num = GetFirstEmptyItemIndex();
		mItemList[num].SetItem(gameItem);
		return true;
	}

	public void OtherPlayerAddItem(GameItem gameItem)
	{
		ItemData itemDataByID = DataManager.GetItemDataByID(gameItem.ItemId);
		if (itemDataByID != null && IsContainerHaveEnoughSize(gameItem))
		{
			int num = -1;
			num = GetFirstEmptyItemIndex();
			mItemList[num] = gameItem;
		}
	}

	public bool IsContainerHaveEnoughSize(GameItem gameItem)
	{
		ItemData itemDataByID = DataManager.GetItemDataByID(gameItem.ItemId);
		if (itemDataByID == null)
		{
			Debug.Log("itemData == null");
			return false;
		}
		if (itemDataByID.Stack > 1)
		{
			if (gameItem.StackNum <= GetContainerEmptyNum() * itemDataByID.Stack)
			{
				return true;
			}
			int num = gameItem.StackNum - GetContainerEmptyNum() * itemDataByID.Stack;
			List<GameItem> itemByItemId = GetItemByItemId(itemDataByID.ID);
			for (int i = 0; i < itemByItemId.Count; i++)
			{
				num -= itemByItemId[i].GetItemLeftSpace();
				if (num <= 0)
				{
					return true;
				}
			}
			return false;
		}
		if (GetFirstEmptyItemIndex() == -1)
		{
			return false;
		}
		return true;
	}

	public bool RemoveItem(int index)
	{
		if (!mItemList[index].IsEmpty())
		{
			mItemList[index].Reset();
			return true;
		}
		return false;
	}

	public bool RemoveItem(GameItem item)
	{
		int index = mItemList.IndexOf(item);
		return RemoveItem(index);
	}

	public void ClearContainer()
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			mItemList[i].Reset();
		}
	}

	public void PrintContainer()
	{
		for (int i = 0; i < mItemList.Count; i++)
		{
			Debug.Log("============" + i);
			PrintItem(mItemList[i]);
		}
	}

	public void PrintItem(GameItem item)
	{
		if (item.IsEmpty())
		{
			Debug.Log("Empty");
			return;
		}
		ItemData itemDataByID = DataManager.GetItemDataByID(item.ItemId);
		Debug.Log("ItemName : " + itemDataByID.Name + "  :: ItemNum : " + item.StackNum);
	}
}
