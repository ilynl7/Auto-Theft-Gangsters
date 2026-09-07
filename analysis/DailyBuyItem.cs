using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DailyBuyItem : MonoBehaviour
{
	public UIGrid ParentGrid;

	public List<RewardItem> RewardItems;

	public UISprite UIbtnSp;

	public UILabel BtnLabel;

	private daily_buy curInfo;

	private DailyBuyData curBuyData;

	public UILabel infoLabel;

	private List<GameItem> ItemList = new List<GameItem>();

	public void UpdateInfo(daily_buy curinfo)
	{
		curBuyData = DataManager.GetDailyBuyDataBuyId(curinfo.ID);
		curInfo = curinfo;
		if (curInfo.state == 0L)
		{
			UIbtnSp.spriteName = GameDefine.BtnIconNew[0];
		}
		else
		{
			UIbtnSp.spriteName = GameDefine.BtnIconNew[1];
		}
		BtnLabel.text = $"$ {curBuyData.Dollor}";
		infoLabel.text = StrDictionary.GetDictionaryString("#{300701}", curBuyData.Value);
		ItemList.Clear();
		if (!string.IsNullOrEmpty(curBuyData.ItemID1))
		{
			ItemList.Add(new GameItem(curBuyData.ItemID1, (EQUIP_QUALITY)curBuyData.Quality1, curBuyData.ItemCount1));
		}
		if (!string.IsNullOrEmpty(curBuyData.ItemID2))
		{
			ItemList.Add(new GameItem(curBuyData.ItemID2, (EQUIP_QUALITY)curBuyData.Quality2, curBuyData.ItemCount2));
		}
		if (!string.IsNullOrEmpty(curBuyData.ItemID3))
		{
			ItemList.Add(new GameItem(curBuyData.ItemID3, (EQUIP_QUALITY)curBuyData.Quality3, curBuyData.ItemCount3));
		}
		if (!string.IsNullOrEmpty(curBuyData.ItemID4))
		{
			ItemList.Add(new GameItem(curBuyData.ItemID4, (EQUIP_QUALITY)curBuyData.Quality4, curBuyData.ItemCount4));
		}
		if (!string.IsNullOrEmpty(curBuyData.ItemID5))
		{
			ItemList.Add(new GameItem(curBuyData.ItemID5, (EQUIP_QUALITY)curBuyData.Quality5, curBuyData.ItemCount5));
		}
		if (ItemList.Count <= 4)
		{
			ParentGrid.cellWidth = 72f;
			ParentGrid.maxPerLine = 2;
		}
		else
		{
			ParentGrid.cellWidth = 62f;
			ParentGrid.maxPerLine = 3;
		}
		int num = ItemList.Count - RewardItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(RewardItems[0].gameObject) as GameObject;
				RewardItem component = gameObject.GetComponent<RewardItem>();
				gameObject.name = $"reward{RewardItems.Count:D2}";
				gameObject.transform.parent = ParentGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				RewardItems.Add(component);
			}
		}
		ParentGrid.Reposition();
		for (int j = 0; j < RewardItems.Count; j++)
		{
			if (j < ItemList.Count)
			{
				int level = 0;
				if (ItemList[j].ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(ItemList[j].ItemData.SubType);
				}
				RewardItems[j].UpdateItem(ItemList[j], level);
				UnityVersionUtil.SetActiveRecursive(RewardItems[j].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(RewardItems[j].gameObject, state: false);
			}
		}
	}

	public void OnClickBuyBtn()
	{
		if (curInfo.state == 0L)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.Billing(curBuyData.ProductId);
			if (GameSettingData.IsTestBilling)
			{
				WaitResponseUIRootLogic.OpenWaitBox(266, 10f, 0f);
				check_purchase.request request = new check_purchase.request();
				request.productId = curBuyData.ProductId;
				NetLogic.GetInstance().Send<Protocol.check_purchase>(request);
			}
		}
	}
}
