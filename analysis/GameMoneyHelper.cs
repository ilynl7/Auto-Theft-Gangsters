using System.Collections.Generic;
using SprotoType;

public class GameMoneyHelper
{
	public const int GuildCashCount = 499;

	public const int GuildDiamondCount = 49;

	public static bool BeforeCheckBuy(int type, int cost)
	{
		return BeforeCheckBuy((GameDefine.MONEY_TYPE)type, cost);
	}

	public static bool BeforeCheckBuy(GameDefine.MONEY_TYPE type, int cost)
	{
		switch (type)
		{
		case GameDefine.MONEY_TYPE.CASH:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Cash >= cost)
			{
				return true;
			}
			break;
		case GameDefine.MONEY_TYPE.GOLD:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Gold >= cost)
			{
				return true;
			}
			break;
		case GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GuildContribute >= cost)
			{
				return true;
			}
			break;
		case GameDefine.MONEY_TYPE.DIAMOND:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Diamond >= cost)
			{
				return true;
			}
			break;
		case GameDefine.MONEY_TYPE.BATTLECOIN:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BattleCoin >= cost)
			{
				return true;
			}
			break;
		case GameDefine.MONEY_TYPE.ACTIVITYCOIN:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityCoin >= cost)
			{
				return true;
			}
			break;
		}
		switch (type)
		{
		case GameDefine.MONEY_TYPE.DIAMOND:
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopShopRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopDiamondBuyRoot, delegate
			{
				SingletonUnity<PopDiamondBuyRootLogic>.Instance.EnableReset();
				ask_shop_list.request rpcReq = new ask_shop_list.request
				{
					type = 4L,
					subType = 1L
				};
				NetLogic.GetInstance().Send<Protocol.ask_shop_list>(rpcReq);
				WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			});
			break;
		case GameDefine.MONEY_TYPE.CASH:
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopDiamondBuyRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopShopRoot, delegate
			{
				SingletonUnity<PopShopRootLogic>.Instance.EnableReset();
				ask_shop_list.request request = new ask_shop_list.request();
				string text = GetShopMoneyItemID(GameDefine.MONEY_TYPE.CASH);
				if (string.IsNullOrEmpty(text))
				{
					text = "5001";
				}
				ItemData itemDataByID = DataManager.GetItemDataByID(text);
				GameDefine.SHOP_TYPE itemShopType = itemDataByID.GetItemShopType();
				request.type = (long)itemShopType;
				request.itemId = text;
				request.subType = 1L;
				SingletonUnity<PopShopRootLogic>.Instance.ShowItemProdect(request.itemId);
				NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request);
				WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			});
			break;
		case GameDefine.MONEY_TYPE.GOLD:
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopDiamondBuyRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopShopRoot, delegate
			{
				SingletonUnity<PopShopRootLogic>.Instance.EnableReset();
				ask_shop_list.request request2 = new ask_shop_list.request();
				ItemData itemDataByID2 = DataManager.GetItemDataByID("5006");
				GameDefine.SHOP_TYPE itemShopType2 = itemDataByID2.GetItemShopType();
				request2.type = (long)itemShopType2;
				request2.itemId = "5006";
				request2.subType = 1L;
				SingletonUnity<PopShopRootLogic>.Instance.ShowItemProdect(request2.itemId);
				NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request2);
				WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			});
			break;
		case GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE:
			NoticeLogic.AddNotifyData("#{100639}");
			break;
		case GameDefine.MONEY_TYPE.BATTLECOIN:
			if (GameManager.IsSupportCurDataVersion56())
			{
				NoticeLogic.AddNotifyData("#{100655}");
			}
			else
			{
				NoticeLogic.AddNotifyData("Battle Coin is not enough!");
			}
			break;
		case GameDefine.MONEY_TYPE.ACTIVITYCOIN:
			if (GameManager.IsSupportCurDataVersion56())
			{
				MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{100658}"), StrDictionary.GetDictionaryString("#{100127}"));
			}
			else
			{
				MessageBoxLogic.OpenOKBox("The Event coin is not enough#rYou can get more coin in [ffff00]Event Enemy[-].", StrDictionary.GetDictionaryString("#{100127}"));
			}
			break;
		}
		return false;
	}

	public static void ShowItemProduct(string itemid, GameDefine.SHOP_TYPE shoptype = GameDefine.SHOP_TYPE.TOOL_SHOP)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (!playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP))
		{
			int condition = DataManager.GetFunctionDataById(3006.ToString()).Condition;
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100649}", condition));
			return;
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopDiamondBuyRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopShopRoot, delegate
		{
			SingletonUnity<PopShopRootLogic>.Instance.EnableReset();
			ask_shop_list.request request = new ask_shop_list.request();
			ItemData itemDataByID = DataManager.GetItemDataByID(itemid);
			shoptype = itemDataByID.GetItemShopType();
			request.type = (long)shoptype;
			request.itemId = itemid;
			request.subType = 1L;
			SingletonUnity<PopShopRootLogic>.Instance.ShowItemProdect(request.itemId);
			NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request);
			WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
		});
	}

	public static void ShowBuyPotion()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ItemData itemDataByID = DataManager.GetItemDataByID("9003");
		if (playerData.CheckLevel(itemDataByID.Level))
		{
			ShowItemProduct("9003");
			return;
		}
		itemDataByID = DataManager.GetItemDataByID("9002");
		if (playerData.CheckLevel(itemDataByID.Level))
		{
			ShowItemProduct("9002");
		}
		else
		{
			ShowItemProduct("9001");
		}
	}

	public static bool BeforeCheckBuyTop(string itemId, int cost)
	{
		ItemData itemDataByID = DataManager.GetItemDataByID(itemId);
		if (itemDataByID != null)
		{
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.ADD_COIN)
			{
				return BeforeCheckBuyTop(GameDefine.MONEY_TYPE.CASH, cost);
			}
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.ADD_GOLD)
			{
				return BeforeCheckBuyTop(GameDefine.MONEY_TYPE.GOLD, cost);
			}
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.ADD_DIAMOND)
			{
				return BeforeCheckBuyTop(GameDefine.MONEY_TYPE.CASH, cost);
			}
		}
		return false;
	}

	public static bool BeforeCheckBuyTop(int type, int cost)
	{
		return BeforeCheckBuyTop((GameDefine.MONEY_TYPE)type, cost);
	}

	public static bool BeforeCheckBuyTop(GameDefine.MONEY_TYPE type, int cost)
	{
		switch (type)
		{
		case GameDefine.MONEY_TYPE.CASH:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Cash >= cost)
			{
				return true;
			}
			break;
		case GameDefine.MONEY_TYPE.GOLD:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Gold >= cost)
			{
				return true;
			}
			break;
		case GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GuildContribute >= cost)
			{
				return true;
			}
			break;
		case GameDefine.MONEY_TYPE.DIAMOND:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Diamond >= cost)
			{
				return true;
			}
			break;
		case GameDefine.MONEY_TYPE.BATTLECOIN:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BattleCoin >= cost)
			{
				return true;
			}
			break;
		case GameDefine.MONEY_TYPE.ACTIVITYCOIN:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityCoin >= cost)
			{
				return true;
			}
			break;
		}
		switch (type)
		{
		case GameDefine.MONEY_TYPE.DIAMOND:
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopTopShopRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopTopDiamondBuyRoot, delegate
			{
				SingletonUnity<PopTopDiamondBuyRootLogic>.Instance.EnableReset();
				ask_shop_list.request rpcReq = new ask_shop_list.request
				{
					type = 4L,
					subType = 1L
				};
				NetLogic.GetInstance().Send<Protocol.ask_shop_list>(rpcReq);
				WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			});
			break;
		case GameDefine.MONEY_TYPE.CASH:
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopTopDiamondBuyRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopTopShopRoot, delegate
			{
				SingletonUnity<PopTopShopRootLogic>.Instance.EnableReset();
				ask_shop_list.request request = new ask_shop_list.request();
				string text = GetShopMoneyItemID(GameDefine.MONEY_TYPE.CASH);
				if (string.IsNullOrEmpty(text))
				{
					text = "5001";
				}
				ItemData itemDataByID = DataManager.GetItemDataByID(text);
				GameDefine.SHOP_TYPE itemShopType = itemDataByID.GetItemShopType();
				request.type = (long)itemShopType;
				request.itemId = text;
				request.subType = 1L;
				SingletonUnity<PopTopShopRootLogic>.Instance.ShowItemProdect(request.itemId);
				NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request);
				WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			});
			break;
		case GameDefine.MONEY_TYPE.GOLD:
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopTopDiamondBuyRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopTopShopRoot, delegate
			{
				SingletonUnity<PopTopShopRootLogic>.Instance.EnableReset();
				ask_shop_list.request request2 = new ask_shop_list.request();
				ItemData itemDataByID2 = DataManager.GetItemDataByID("5006");
				GameDefine.SHOP_TYPE itemShopType2 = itemDataByID2.GetItemShopType();
				request2.type = (long)itemShopType2;
				request2.itemId = "5006";
				request2.subType = 1L;
				SingletonUnity<PopTopShopRootLogic>.Instance.ShowItemProdect(request2.itemId);
				NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request2);
				WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			});
			break;
		}
		switch (type)
		{
		case GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE:
			NoticeLogic.AddNotifyData("#{100639}");
			break;
		case GameDefine.MONEY_TYPE.BATTLECOIN:
			if (GameManager.IsSupportCurDataVersion56())
			{
				NoticeLogic.AddNotifyData("#{100655}");
			}
			else
			{
				NoticeLogic.AddNotifyData("Battle Coin is not enough!");
			}
			break;
		}
		return false;
	}

	public static void ShowItemProductTop(string itemid, GameDefine.SHOP_TYPE shoptype = GameDefine.SHOP_TYPE.TOOL_SHOP)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (!playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP))
		{
			int condition = DataManager.GetFunctionDataById(3006.ToString()).Condition;
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100649}", condition));
			return;
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopTopDiamondBuyRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopTopShopRoot, delegate
		{
			SingletonUnity<PopTopShopRootLogic>.Instance.EnableReset();
			ask_shop_list.request request = new ask_shop_list.request();
			ItemData itemDataByID = DataManager.GetItemDataByID(itemid);
			shoptype = itemDataByID.GetItemShopType();
			request.type = (long)shoptype;
			request.itemId = itemid;
			request.subType = 1L;
			SingletonUnity<PopTopShopRootLogic>.Instance.ShowItemProdect(request.itemId);
			NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request);
			WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
		});
	}

	public static long GetMoneyNum(int curtype)
	{
		return (GameDefine.MONEY_TYPE)curtype switch
		{
			GameDefine.MONEY_TYPE.CASH => SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Cash, 
			GameDefine.MONEY_TYPE.GOLD => SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Gold, 
			GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE => SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GuildContribute, 
			GameDefine.MONEY_TYPE.DIAMOND => SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Diamond, 
			GameDefine.MONEY_TYPE.BATTLECOIN => SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BattleCoin, 
			GameDefine.MONEY_TYPE.ACTIVITYCOIN => SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityCoin, 
			_ => 0L, 
		};
	}

	public static long GetGold()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Gold;
	}

	public static long GetCash()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Cash;
	}

	public static long GetDiamond()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Diamond;
	}

	public static long GetBattleCoin()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.BattleCoin;
	}

	public static void UpdateMoney(long money1, long money2, long money3, long money4, long money5 = 0, long money6 = 0)
	{
		bool flag = false;
		if (GetCash() != money1 || GetGold() != money2 || GetDiamond() != money3)
		{
			flag = true;
		}
		SetCash(money1);
		SetGold(money2);
		SetDiamond(money3);
		SetGuildContribute(money4);
		SetBattleCoin(money5);
		SetActivityCoin(money6);
		if (flag)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.UpdateEnhanceTips();
			if (SingletonUnity<FunctionBtnRootLogic>.Exists)
			{
				SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateSkillTips();
			}
		}
	}

	public static void SetGold(long val)
	{
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SetGold(val);
	}

	public static void SetCash(long val)
	{
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SetCash(val);
	}

	public static void SetDiamond(long val)
	{
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SetDiamond(val);
	}

	public static void SetBattleCoin(long val)
	{
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SetBattleCoin(val);
		if (SingletonUnity<ShopTabRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ShopTabRootLogic>.Instance.gameObject))
		{
			SingletonUnity<ShopTabRootLogic>.Instance.UpdateMoneyLabel();
		}
	}

	public static void SetActivityCoin(long val)
	{
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SetActivityCoin(val);
		if (SingletonUnity<ShopTabRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ShopTabRootLogic>.Instance.gameObject))
		{
			SingletonUnity<ShopTabRootLogic>.Instance.UpdateMoneyLabel();
		}
	}

	public static long GetGuildContribute()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GuildContribute;
	}

	public static void SetGuildContribute(long val)
	{
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GuildContribute = val;
		if (SingletonUnity<ShopTabRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ShopTabRootLogic>.Instance.gameObject))
		{
			SingletonUnity<ShopTabRootLogic>.Instance.UpdateMoneyLabel();
		}
		if (SingletonUnity<GuildStarRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildStarRootLogic>.Instance.gameObject))
		{
			SingletonUnity<GuildStarRootLogic>.Instance.UpdateMoneyLabel();
		}
	}

	public static string GetMoneyValStr(long val, long type)
	{
		return GetMoneyValStr((int)val, (GameDefine.MONEY_TYPE)type);
	}

	public static string GetMoneyValStr(int val, int type)
	{
		return GetMoneyValStr(val, (GameDefine.MONEY_TYPE)type);
	}

	public static string GetMoneyValStr(int val, GameDefine.MONEY_TYPE type)
	{
		return type switch
		{
			GameDefine.MONEY_TYPE.CASH => $":$ {val}", 
			GameDefine.MONEY_TYPE.GOLD => $":% {val}", 
			GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE => $":& {val}", 
			GameDefine.MONEY_TYPE.DIAMOND => $":@ {val}", 
			GameDefine.MONEY_TYPE.BATTLECOIN => $":^ {val}", 
			GameDefine.MONEY_TYPE.ACTIVITYCOIN => $":~ {val}", 
			_ => $":${val}", 
		};
	}

	public static string GetMoneyValStr(int val, string itemId)
	{
		ItemData itemDataByID = DataManager.GetItemDataByID(itemId);
		if (itemDataByID != null)
		{
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.ADD_COIN)
			{
				return GetMoneyValStr(val, GameDefine.MONEY_TYPE.CASH);
			}
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.ADD_GOLD)
			{
				return GetMoneyValStr(val, GameDefine.MONEY_TYPE.GOLD);
			}
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.ADD_DIAMOND)
			{
				return GetMoneyValStr(val, GameDefine.MONEY_TYPE.DIAMOND);
			}
		}
		return string.Empty;
	}

	public static string GetMoneyIcon(GameDefine.MONEY_TYPE type)
	{
		return type switch
		{
			GameDefine.MONEY_TYPE.CASH => "CZ_tuBiao_money", 
			GameDefine.MONEY_TYPE.GOLD => "CZ_tuBiao_Gold", 
			GameDefine.MONEY_TYPE.DIAMOND => "CZ_tuBiao_zuanShi", 
			GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE => "CZ_gongHui_GongHuiHuoBi", 
			GameDefine.MONEY_TYPE.BATTLECOIN => "CZ_duiZhanHuoBi", 
			GameDefine.MONEY_TYPE.ACTIVITYCOIN => "CZ_huoDongHuoBi", 
			_ => string.Empty, 
		};
	}

	public static string GetMoneyIcon(long type)
	{
		GameDefine.MONEY_TYPE type2 = (GameDefine.MONEY_TYPE)type;
		return GetMoneyIcon(type2);
	}

	public static ItemData GetMoneyItemData(GameDefine.MONEY_TYPE type)
	{
		if (GameDefine.ITEM_ID.ContainsKey(type))
		{
			string id = GameDefine.ITEM_ID[type];
			return DataManager.GetItemDataByID(id);
		}
		return null;
	}

	public static string GetMoneyPre(GameDefine.MONEY_TYPE type)
	{
		return type switch
		{
			GameDefine.MONEY_TYPE.CASH => ":$", 
			GameDefine.MONEY_TYPE.GOLD => ":%", 
			GameDefine.MONEY_TYPE.DIAMOND => ":@", 
			GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE => ":&", 
			GameDefine.MONEY_TYPE.BATTLECOIN => ":^", 
			GameDefine.MONEY_TYPE.ACTIVITYCOIN => ":~", 
			_ => ":$", 
		};
	}

	public static string GetMoneyPicName(GameDefine.MONEY_TYPE type)
	{
		return type switch
		{
			GameDefine.MONEY_TYPE.CASH => $"CZ_qian", 
			GameDefine.MONEY_TYPE.GOLD => $"CZ_jinBi", 
			_ => $"CZ_qian", 
		};
	}

	public static string GetShopMoneyItemID(GameDefine.MONEY_TYPE type)
	{
		List<ShopData> list = new List<ShopData>();
		List<ShopData> shopDataList = DataManager.GetShopDataList();
		for (int i = 0; i < shopDataList.Count; i++)
		{
			if (shopDataList[i].Shop == 0)
			{
				list.Add(shopDataList[i]);
			}
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (type == GameDefine.MONEY_TYPE.CASH)
		{
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].ItemID.Equals("5001") && playerData.CheckLevel(list[j].MinLevel, list[j].MaxLevel))
				{
					return "5001";
				}
				if (list[j].ItemID.Equals("5002") && playerData.CheckLevel(list[j].MinLevel, list[j].MaxLevel))
				{
					return "5002";
				}
				if (list[j].ItemID.Equals("5003") && playerData.CheckLevel(list[j].MinLevel, list[j].MaxLevel))
				{
					return "5003";
				}
			}
			return "5001";
		}
		return null;
	}
}
