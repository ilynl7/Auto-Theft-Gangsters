using System.Collections.Generic;
using SprotoType;
using UnityEngine;

[ExecuteInEditMode]
public class ShowRewardItems : MonoBehaviour
{
	public UIGrid grid;

	public List<RewardItem> rewardItmes = new List<RewardItem>();

	private void Awake()
	{
	}

	public void ShowRewards(Dictionary<string, item> items)
	{
		if (items == null || items.Count == 0)
		{
			for (int i = 0; i < rewardItmes.Count; i++)
			{
				NGUITools.SetActive(rewardItmes[i].gameObject, state: false);
			}
		}
		else
		{
			ShowRewards(new List<item>(items.Values));
		}
	}

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
		items.Sort((item x, item y) => (x.itemId.Length != y.itemId.Length) ? (-x.itemId.Length + y.itemId.Length) : y.itemId.CompareTo(x.itemId));
		int num = items.Count - rewardItmes.Count;
		if (num > 0)
		{
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = Object.Instantiate(rewardItmes[0].gameObject) as GameObject;
				grid.AddChild(gameObject.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.name = $"zhuangBei{rewardItmes.Count:d2}";
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
			rewardItmes[num2++].UpdateItem(items[l].itemId, (int)items[l].quality, (int)items[l].itemCount, (int)(items[l].HasCount2 ? items[l].count2 : 0));
		}
		grid.Reposition();
	}

	public void ShowRewards(List<string> itemIds, List<int> qualitys, List<int> counts)
	{
		if (itemIds == null)
		{
			for (int i = 0; i < rewardItmes.Count; i++)
			{
				NGUITools.SetActive(rewardItmes[i].gameObject, state: false);
			}
			return;
		}
		int num = itemIds.Count - rewardItmes.Count;
		if (num > 0)
		{
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = Object.Instantiate(rewardItmes[0].gameObject) as GameObject;
				grid.AddChild(gameObject.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.name = $"zhuangBei{rewardItmes.Count:d2}";
				rewardItmes.Add(gameObject.GetComponent<RewardItem>());
			}
		}
		for (int k = 0; k < rewardItmes.Count; k++)
		{
			NGUITools.SetActive(rewardItmes[k].gameObject, k < itemIds.Count);
		}
		for (int l = 0; l < itemIds.Count; l++)
		{
			rewardItmes[l].UpdateItem(itemIds[l], qualitys[l], counts[l]);
		}
		grid.Reposition();
	}

	public void ShowRewards(ItemData itemData, int itemCount = 1)
	{
		for (int i = 0; i < rewardItmes.Count; i++)
		{
			NGUITools.SetActive(rewardItmes[i].gameObject, i < 1);
		}
		rewardItmes[0].UpdateItem(itemData.ID, itemData.QualityType, itemCount);
		grid.Reposition();
	}

	public void ShowRewards(List<string> itemIds, List<EQUIP_QUALITY> qualitys, List<int> counts)
	{
		if (itemIds == null)
		{
			for (int i = 0; i < rewardItmes.Count; i++)
			{
				NGUITools.SetActive(rewardItmes[i].gameObject, state: false);
			}
			return;
		}
		int num = itemIds.Count - rewardItmes.Count;
		if (num > 0)
		{
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = Object.Instantiate(rewardItmes[0].gameObject) as GameObject;
				grid.AddChild(gameObject.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.name = $"zhuangBei{rewardItmes.Count:d2}";
				rewardItmes.Add(gameObject.GetComponent<RewardItem>());
			}
		}
		for (int k = 0; k < rewardItmes.Count; k++)
		{
			NGUITools.SetActive(rewardItmes[k].gameObject, k < itemIds.Count);
		}
		for (int l = 0; l < itemIds.Count; l++)
		{
			rewardItmes[l].UpdateItem(itemIds[l], qualitys[l], counts[l]);
		}
		grid.Reposition();
	}
}
