using System.Collections.Generic;

public class ItemData
{
	public string ID;

	public string Name;

	[ServerExclude("ServerNoUse")]
	public string Description;

	public int Level;

	public int Flags;

	public int Quality;

	public int ItemType;

	public int SubType;

	public int PriceType;

	public string Price = string.Empty;

	public int ConsignPrice;

	public int Stack;

	public int Function;

	public string FunLock;

	[ServerExclude("ServerNoUse")]
	public string DropIcon;

	[ServerExclude("ServerNoUse")]
	public string BackPackIcon;

	public string Score;

	public string Tvshow;

	public int UseHour = -1;

	public string JumpPath = string.Empty;

	private int[] mScore;

	private int[] mPrice;

	public string MName => StrDictionary.GetDictionaryString(Name);

	public string MDescription => StrDictionary.GetDictionaryString(Description);

	public GameDefine.ITEM_TYPE Type => (GameDefine.ITEM_TYPE)ItemType;

	public bool CanShowModel => Type == GameDefine.ITEM_TYPE.FASHION_EQUIP || (Type == GameDefine.ITEM_TYPE.EQUIP && SubType < 4);

	public GameDefine.MONEY_TYPE SellType => (GameDefine.MONEY_TYPE)PriceType;

	public EQUIP_QUALITY QualityType => (EQUIP_QUALITY)Quality;

	public int GetScore(EQUIP_QUALITY quality = EQUIP_QUALITY.INVALID)
	{
		if (mScore == null)
		{
			string[] array = Score.Split('#');
			mScore = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				mScore[i] = int.Parse(array[i]);
			}
		}
		if (Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			if ((int)quality < mScore.Length)
			{
				return mScore[(int)quality];
			}
		}
		if (mScore.Length > 0)
		{
			return mScore[0];
		}
		return -1;
	}

	public int GetSellPrice(EQUIP_QUALITY quality = EQUIP_QUALITY.INVALID)
	{
		if (Type == GameDefine.ITEM_TYPE.ADD_COIN || Type == GameDefine.ITEM_TYPE.ADD_DIAMOND || Type == GameDefine.ITEM_TYPE.ADD_GOLD)
		{
			return 0;
		}
		if (mPrice == null && !string.IsNullOrEmpty(Price))
		{
			string[] array = Price.Split('#');
			mPrice = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				mPrice[i] = int.Parse(array[i]);
			}
		}
		if (Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			if ((int)quality < mPrice.Length)
			{
				return mPrice[(int)quality];
			}
		}
		if (mPrice.Length > 0)
		{
			return mPrice[0];
		}
		return -1;
	}

	public bool CanSell()
	{
		if (Type == GameDefine.ITEM_TYPE.EQUIP || Type == GameDefine.ITEM_TYPE.ENHANCE_ITEM || Type == GameDefine.ITEM_TYPE.POTION || Type == GameDefine.ITEM_TYPE.BOX || Type == GameDefine.ITEM_TYPE.LOCK1 || Type == GameDefine.ITEM_TYPE.LOCK2)
		{
			return true;
		}
		return false;
	}

	public bool CanConsign()
	{
		if (ConsignPrice > 0)
		{
			return true;
		}
		return false;
	}

	public GameDefine.SHOP_TYPE GetItemShopType()
	{
		List<ShopData> shpTabClassList = GetShpTabClassList(0);
		for (int i = 0; i < shpTabClassList.Count; i++)
		{
			if (shpTabClassList[i].ItemID.Equals(ID))
			{
				return GameDefine.SHOP_TYPE.TOOL_SHOP;
			}
		}
		shpTabClassList = GetShpTabClassList(1);
		for (int j = 0; j < shpTabClassList.Count; j++)
		{
			if (shpTabClassList[j].ItemID.Equals(ID))
			{
				return GameDefine.SHOP_TYPE.EQUIP_SHOP;
			}
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			shpTabClassList = GetShpTabClassList(3);
			for (int k = 0; k < shpTabClassList.Count; k++)
			{
				if (shpTabClassList[k].ItemID.Equals(ID))
				{
					return GameDefine.SHOP_TYPE.GUILD_SHOP;
				}
			}
		}
		return GameDefine.SHOP_TYPE.TOOL_SHOP;
	}

	public int GetItemClass(GameDefine.SHOP_TYPE shoptype)
	{
		List<ShopData> shpTabClassList = GetShpTabClassList((int)shoptype);
		for (int i = 0; i < shpTabClassList.Count; i++)
		{
			if (shpTabClassList[i].ItemID.Equals(ID))
			{
				return shpTabClassList[i].Class;
			}
		}
		return -1;
	}

	public List<ShopData> GetShpTabClassList(int curType)
	{
		List<ShopData> list = new List<ShopData>();
		List<ShopData> shopDataList = DataManager.GetShopDataList();
		for (int i = 0; i < shopDataList.Count; i++)
		{
			if (curType == shopDataList[i].Shop)
			{
				list.Add(shopDataList[i]);
			}
		}
		return list;
	}

	public ITEM_CONTAINER_TYPE GetContainerType()
	{
		if (Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			return ITEM_CONTAINER_TYPE.EQUIP_BACKPACK;
		}
		if (Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			return ITEM_CONTAINER_TYPE.FASHION_BACKPACK;
		}
		if (Type == GameDefine.ITEM_TYPE.BADGE)
		{
			return ITEM_CONTAINER_TYPE.BADGE_BACKPACK;
		}
		return ITEM_CONTAINER_TYPE.ITEM_BACKPACK;
	}
}
