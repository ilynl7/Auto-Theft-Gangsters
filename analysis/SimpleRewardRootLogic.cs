using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SimpleRewardRootLogic : SingletonUnity<SimpleRewardRootLogic>
{
	public List<SimpleRewardLine> DisableList;

	public List<SimpleRewardLine> EnableList;

	public SimpleRewardLine PrefabLine;

	private List<item> mLineReward = new List<item>();

	private List<ItemData> mLineRewardItemData = new List<ItemData>();

	private List<item> mIconReward = new List<item>();

	private List<ItemData> mIconRewardItemData = new List<ItemData>();

	private float lastLineRewardShowTime;

	private float LINE_REWARD_INTERVAL = 0.5f;

	public static void ResetSimpleReward(List<item> rewardList)
	{
		if (SingletonUnity<SimpleRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SimpleRewardRootLogic>.Instance.gameObject))
		{
			SingletonUnity<SimpleRewardRootLogic>.Instance.ResetReward(rewardList);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SimpleRewardRoot, delegate
		{
			SingletonUnity<SimpleRewardRootLogic>.Instance.ResetReward(rewardList);
		});
	}

	public static void AddRewards(List<item> list)
	{
		if (list == null || list.Count <= 0)
		{
			return;
		}
		if (SingletonUnity<SimpleRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SimpleRewardRootLogic>.Instance.gameObject))
		{
			for (int i = 0; i < list.Count; i++)
			{
				AddReward(list[i]);
			}
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SimpleRewardRoot, delegate
		{
			for (int j = 0; j < list.Count; j++)
			{
				AddReward(list[j]);
			}
		});
	}

	public static void AddReward(item rewardItem)
	{
		if (rewardItem == null)
		{
			return;
		}
		ItemData itemDataByID = DataManager.GetItemDataByID(rewardItem.itemId);
		if (itemDataByID == null)
		{
			return;
		}
		if (SingletonUnity<SimpleRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SimpleRewardRootLogic>.Instance.gameObject))
		{
			SingletonUnity<SimpleRewardRootLogic>.Instance.AddRewardItem(rewardItem);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SimpleRewardRoot, delegate
		{
			SingletonUnity<SimpleRewardRootLogic>.Instance.AddRewardItem(rewardItem);
		});
	}

	public static void AddRewards(List<GameItem> list)
	{
		if (list == null || list.Count <= 0)
		{
			return;
		}
		if (SingletonUnity<SimpleRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SimpleRewardRootLogic>.Instance.gameObject))
		{
			for (int i = 0; i < list.Count; i++)
			{
				AddReward(list[i]);
			}
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SimpleRewardRoot, delegate
		{
			for (int j = 0; j < list.Count; j++)
			{
				AddReward(list[j]);
			}
		});
	}

	public static void AddReward(GameItem gameitem)
	{
		item item = new item();
		item.itemId = gameitem.ItemId;
		item.itemCount = gameitem.StackNum;
		item.quality = (long)gameitem.Quality;
		AddReward(item);
	}

	public static void AddReward(ItemData rewardItem, int count, int quality)
	{
		item item = new item();
		item.itemId = rewardItem.ID;
		item.itemCount = count;
		item.quality = quality;
		AddReward(item);
	}

	public void AddRewardItem(item rewardItem)
	{
		mLineReward.Add(rewardItem);
		mLineRewardItemData.Add(DataManager.GetItemDataByID(rewardItem.itemId));
	}

	public void ResetReward(List<item> rewardList)
	{
		mLineReward.Clear();
		mLineRewardItemData.Clear();
		mIconReward.Clear();
		mIconRewardItemData.Clear();
		for (int i = 0; i < rewardList.Count; i++)
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(rewardList[i].itemId);
			if (IsSimpleLineReward(itemDataByID))
			{
				mLineReward.Add(rewardList[i]);
				mLineRewardItemData.Add(itemDataByID);
			}
			else
			{
				mIconReward.Add(rewardList[i]);
				mIconRewardItemData.Add(itemDataByID);
			}
		}
	}

	private bool IsSimpleLineReward(ItemData itemData)
	{
		if (itemData.Type == GameDefine.ITEM_TYPE.ADD_COIN || itemData.Type == GameDefine.ITEM_TYPE.ADD_DIAMOND || itemData.Type == GameDefine.ITEM_TYPE.ADD_EXP || itemData.Type == GameDefine.ITEM_TYPE.ADD_GOLD || itemData.Type == GameDefine.ITEM_TYPE.ADD_HONOR)
		{
			return true;
		}
		return false;
	}

	private void Update()
	{
		if (mLineReward.Count > 0 && Time.time - lastLineRewardShowTime > LINE_REWARD_INTERVAL)
		{
			lastLineRewardShowTime = Time.time;
			SimpleRewardLine simpleRewardLine = null;
			if (DisableList.Count > 0)
			{
				simpleRewardLine = DisableList[0];
				DisableList.RemoveAt(0);
			}
			else
			{
				GameObject gameObject = Object.Instantiate(PrefabLine.gameObject) as GameObject;
				gameObject.transform.parent = PrefabLine.transform.parent;
				gameObject.transform.localScale = Vector3.one;
				simpleRewardLine = gameObject.GetComponent<SimpleRewardLine>();
			}
			EnableList.Add(simpleRewardLine);
			simpleRewardLine.Reset(mLineRewardItemData[0], (int)mLineReward[0].itemCount, (EQUIP_QUALITY)mLineReward[0].quality, OnRecycleRewardLine);
			mLineReward.RemoveAt(0);
			mLineRewardItemData.RemoveAt(0);
		}
	}

	private void OnRecycleRewardLine(SimpleRewardLine line)
	{
		UnityVersionUtil.SetActiveRecursive(line.gameObject, state: false);
		EnableList.Remove(line);
		DisableList.Add(line);
		if (EnableList.Count == 0 && mLineReward.Count == 0)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SimpleRewardRoot);
		}
	}
}
