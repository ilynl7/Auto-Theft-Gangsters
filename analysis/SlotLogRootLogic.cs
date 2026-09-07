using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SlotLogRootLogic : SingletonUnity<SlotLogRootLogic>
{
	public List<RewardItem> rewardItmes = new List<RewardItem>();

	public UIScrollView parentView;

	public UIGrid ParentGrid;

	public void ShowRewards(List<item> items)
	{
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
		for (int l = 0; l < items.Count; l++)
		{
			rewardItmes[l].UpdateItem(items[l].itemId, (int)items[l].quality, (int)items[l].itemCount);
		}
		ParentGrid.Reposition();
		parentView.ResetPosition();
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SlotLogRoot);
	}
}
