using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SellItemsRootLogic : SingletonUnity<SellItemsRootLogic>
{
	public List<RewardItem> rewardItmes = new List<RewardItem>();

	public UIScrollView parentView;

	public UIGrid ParentGrid;

	public UILabel SellMoneyLabel;

	private List<GameItem> curSellItem = new List<GameItem>();

	public UILabel infolabel;

	public void ShowRewards(List<GameItem> items)
	{
		curSellItem.Clear();
		if (GameManager.IsSupportCurDataVersion())
		{
			infolabel.text = StrDictionary.GetDictionaryString("#{100290}");
		}
		else
		{
			infolabel.text = "To get money";
		}
		if (items == null || items.Count == 0)
		{
			for (int i = 0; i < rewardItmes.Count; i++)
			{
				NGUITools.SetActive(rewardItmes[i].gameObject, state: false);
			}
			return;
		}
		int num = items.Count - rewardItmes.Count;
		if (num > 0)
		{
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = Object.Instantiate(rewardItmes[0].gameObject) as GameObject;
				gameObject.transform.parent = ParentGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				rewardItmes.Add(gameObject.GetComponent<RewardItem>());
			}
		}
		for (int k = 0; k < rewardItmes.Count; k++)
		{
			NGUITools.SetActive(rewardItmes[k].gameObject, k < items.Count);
		}
		int num2 = 0;
		for (int l = 0; l < items.Count; l++)
		{
			rewardItmes[l].UpdateItem(items[l].ItemId, (int)items[l].GetItemQuality(), items[l].StackNum);
			num2 += items[l].ItemData.GetSellPrice(items[l].GetItemQuality());
			curSellItem.Add(items[l]);
		}
		SellMoneyLabel.text = GameMoneyHelper.GetMoneyValStr(num2, GameDefine.MONEY_TYPE.CASH);
		ParentGrid.Reposition();
		parentView.ResetPosition();
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SellItemsRoot);
	}

	public void OnClickYesBtn()
	{
		for (int i = 0; i < curSellItem.Count; i++)
		{
			sell_item.request request = new sell_item.request();
			request.indexId = curSellItem[i].IndexId;
			request.itemCount = curSellItem[i].StackNum;
			request.type = (long)curSellItem[i].ContainerType;
			NetLogic.GetInstance().Send<Protocol.sell_item>(request);
		}
		OnClickCloseBtn();
	}

	public void OnClickCancelBtn()
	{
		OnClickCloseBtn();
	}
}
