using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class EnemyUIRootLogic : SingletonUnity<EnemyUIRootLogic>
{
	public int LineCount = 6;

	public List<EnemyItemLogic> EnemyItems = new List<EnemyItemLogic>();

	public UIWrapContentNew uiWrapContent;

	public UIScrollView uiScrollView;

	public UIWidget WrapContentBottomWidget;

	private List<friend_info> curListEnemyInfo = new List<friend_info>();

	public UILabel NumLabel;

	protected override void Awake()
	{
		base.Awake();
		uiWrapContent.enabled = false;
		UIWrapContentNew uIWrapContentNew = uiWrapContent;
		uIWrapContentNew.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContentNew.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
	}

	public void EnableReset()
	{
		for (int i = 0; i < EnemyItems.Count; i++)
		{
			NGUITools.SetActive(EnemyItems[i].gameObject, state: false);
		}
	}

	public void UpdateEnemyList()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		List<friend_info> list = new List<friend_info>(playerData.FriendInfo.MainPlayerEnemyDic.Values);
		list = FriendInfo.SortEnemyList(list);
		int num = Mathf.Min(list.Count, LineCount) - EnemyItems.Count;
		int count = EnemyItems.Count;
		curListEnemyInfo = list;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(EnemyItems[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{count + i}";
				EnemyItemLogic component = gameObject.GetComponent<EnemyItemLogic>();
				if (component != null)
				{
					component.transform.parent = EnemyItems[0].transform.parent;
					component.transform.localScale = Vector3.one;
					component.transform.localPosition = Vector3.zero;
					EnemyItems.Add(component);
				}
			}
		}
		for (int j = 0; j < EnemyItems.Count; j++)
		{
			NGUITools.SetActive(EnemyItems[j].gameObject, j < list.Count);
			if (j < list.Count)
			{
				ResetItemLine(EnemyItems[j], j);
			}
		}
		uiWrapContent.minIndex = 1 - list.Count;
		uiWrapContent.maxIndex = 0;
		WrapContentBottomWidget.height = list.Count * uiWrapContent.itemSize;
		uiWrapContent.SortBasedOnScrollMovement();
		uiScrollView.ResetPosition();
		uiWrapContent.enabled = true;
		NumLabel.text = string.Format("{0}:{1}/{2}", StrDictionary.GetDictionaryString("#{100220}"), playerData.FriendInfo.EnemyCount, GameDefine.MAX_ENEMY_COUNT);
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		EnemyItemLogic itemLogic = EnemyItems[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex));
	}

	private void ResetItemLine(EnemyItemLogic itemLogic, int idx)
	{
		if (idx < curListEnemyInfo.Count)
		{
			itemLogic.UpdateFriendInfo(curListEnemyInfo[idx]);
		}
	}
}
