using System.Collections.Generic;
using UnityEngine;

public class NewItemTipsRootLogic : SingletonUnity<NewItemTipsRootLogic>
{
	private static bool ListisEmpty = true;

	public static List<GameItem> NewItemList = new List<GameItem>();

	public UILabel nameLabel;

	public UILabel scoreLabel;

	public ItemUILogic itemLogic;

	public UILabel BtnLabel;

	public GameObject ShowObj;

	private bool isShowitemFlag;

	private float temptime;

	private float deltime_showitem = 0.5f;

	private GameItem curGameItem;

	private ItemData curitemData;

	private float showtime = 60f;

	private float tempshowtime;

	private int AppraisePrice;

	public static void AddNewItem(GameItem newitem)
	{
		ItemData itemData = newitem.ItemData;
		ItemContainer itemContainer = null;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (itemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			itemContainer = playerData.GetItemContainer(ITEM_CONTAINER_TYPE.EQUIPPACK);
		}
		else if (itemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			itemContainer = playerData.GetItemContainer(ITEM_CONTAINER_TYPE.FASHION_EQUIPPACK);
		}
		if (newitem.Parm[5] != 0 || itemContainer == null)
		{
			return;
		}
		EquipData equipDataById = DataManager.GetEquipDataById(newitem.ItemId);
		if ((equipDataById.profession != playerData.Profession && newitem.ItemData.SubType != 0) || !playerData.CheckLevel(itemData.Level))
		{
			return;
		}
		bool flag = false;
		int @class = equipDataById.Class;
		if (itemContainer != null)
		{
			List<GameItem> itemList = itemContainer.ItemList;
			int num = -3;
			int num2 = -1;
			int num3 = -1;
			for (int i = 0; i < itemList.Count; i++)
			{
				GameItem gameItem = itemList[i];
				if (!gameItem.IsEmpty() && gameItem.ItemData.SubType == newitem.ItemData.SubType)
				{
					num = (int)gameItem.GetItemQuality();
					num2 = gameItem.ItemData.Level;
					EquipData equipDataById2 = DataManager.GetEquipDataById(gameItem.ItemId);
					if (equipDataById2 != null)
					{
						num3 = equipDataById2.Class;
					}
					break;
				}
			}
			if (num3 < @class)
			{
				flag = true;
			}
		}
		if (flag)
		{
			bool flag2 = true;
			for (int num4 = NewItemList.Count - 1; num4 > -1; num4--)
			{
				GameItem gameItem2 = NewItemList[num4];
				if (gameItem2.ContainerType == newitem.ContainerType && gameItem2.ItemData.SubType == newitem.ItemData.SubType)
				{
					int num5 = -1;
					EquipData equipDataById3 = DataManager.GetEquipDataById(gameItem2.ItemId);
					if (equipDataById3 != null)
					{
						num5 = equipDataById3.Class;
					}
					if (@class > num5)
					{
						NewItemList.RemoveAt(num4);
						flag2 = true;
					}
					else
					{
						flag2 = false;
					}
				}
			}
			if (flag2)
			{
				NewItemList.Add(newitem);
			}
		}
		ListisEmpty = NewItemList.Count <= 0;
		if (!ListisEmpty && (!SingletonUnity<NewItemTipsRootLogic>.Exists || UnityVersionUtil.IsActive(SingletonUnity<NewItemTipsRootLogic>.Instance.gameObject)))
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewItemTipsRoot);
		}
	}

	public static void CloseNewItemTips()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewItemTipsRoot);
	}

	private void OnEnable()
	{
		tempshowtime = 0f;
		temptime = 0f;
		ListisEmpty = NewItemList.Count <= 0;
		isShowitemFlag = false;
		UnityVersionUtil.SetActiveRecursive(ShowObj, state: false);
	}

	private bool CheckNeedShow()
	{
		if (SingletonUnity<DialogMissionUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DialogMissionUIRoot>.Instance.gameObject))
		{
			return false;
		}
		return true;
	}

	private void Update()
	{
		if (!ListisEmpty && !isShowitemFlag)
		{
			temptime += Time.deltaTime;
			if (temptime > deltime_showitem)
			{
				temptime = 0f;
				ShowItemInfo();
			}
		}
		if (isShowitemFlag)
		{
			tempshowtime += Time.deltaTime;
			if (tempshowtime >= showtime)
			{
				NextItemShow();
			}
		}
	}

	public void ShowItemInfo()
	{
		isShowitemFlag = true;
		curGameItem = NewItemList[0];
		NewItemList.RemoveAt(0);
		ListisEmpty = NewItemList.Count <= 0;
		itemLogic.UpdateNewItemUI(curGameItem);
		curitemData = curGameItem.ItemData;
		nameLabel.text = curitemData.MName;
		nameLabel.color = GameDefine.GetColorByQuality(curGameItem.GetItemQuality());
		UnityVersionUtil.SetActiveRecursive(ShowObj, state: true);
		if (curitemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			BtnLabel.text = StrDictionary.GetDictionaryString("#{100637}");
			EquipData equipDataById = DataManager.GetEquipDataById(curGameItem.ItemId);
			AppraisePrice = equipDataById.GetAppraisePrice(curGameItem.GetItemQuality());
			if (AppraisePrice != 0)
			{
				scoreLabel.text = GameMoneyHelper.GetMoneyValStr(AppraisePrice, GameDefine.MONEY_TYPE.GOLD);
			}
			else
			{
				scoreLabel.text = string.Empty;
			}
		}
		else if (curitemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			BtnLabel.text = StrDictionary.GetDictionaryString("#{100614}");
			scoreLabel.text = string.Empty;
		}
	}

	public void OnClickPutonBtn()
	{
		OnClickEquipBtn();
		NextItemShow();
	}

	public void OnClickEquipBtn()
	{
		if (curGameItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			EquipData equipDataById = DataManager.GetEquipDataById(curGameItem.ItemId);
			if (GameMoneyHelper.GetMoneyNum(1) < AppraisePrice)
			{
				NoticeLogic.AddNotifyData("#{100640}");
			}
			else if (SingletonDontDestoryUnity<GameManager>.Instance.CanSendToServer(116))
			{
				Singleton<ObjManager>.Instance.MainPlayer.EquipItem(curGameItem, isinhert: true);
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100272}");
			}
		}
		else if (curGameItem.ItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.CanSendToServer(221))
			{
				ItemContainer fashionEquipPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FashionEquipPack;
				Singleton<ObjManager>.Instance.MainPlayer.EquipFashionItem(curGameItem);
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100272}");
			}
		}
	}

	private bool CheckCanEquip()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (!playerData.CheckLevel(curGameItem.ItemData.Level))
		{
			NoticeLogic.AddNotifyData("#{100642}");
			return false;
		}
		return true;
	}

	public void NextItemShow()
	{
		tempshowtime = 0f;
		UnityVersionUtil.SetActiveRecursive(ShowObj, state: false);
		isShowitemFlag = false;
		if (ListisEmpty)
		{
			CloseNewItemTips();
		}
	}

	public void OnClickCloseBtn()
	{
		NextItemShow();
	}
}
