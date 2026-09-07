using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class FriendUIRootLogic : SingletonUnity<FriendUIRootLogic>
{
	public int LineCount = 6;

	public List<FriendItemLogic> FriendItems = new List<FriendItemLogic>();

	public UILabel Times;

	public UIWrapContentNew uiWrapContent;

	public UIScrollView uiScrollView;

	public UIWidget WrapContentBottomWidget;

	private List<friend_info> curListFriendInfo = new List<friend_info>();

	protected override void Awake()
	{
		base.Awake();
		uiWrapContent.enabled = false;
		UIWrapContentNew uIWrapContentNew = uiWrapContent;
		uIWrapContentNew.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContentNew.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
	}

	public void EnableReset()
	{
		for (int i = 0; i < FriendItems.Count; i++)
		{
			NGUITools.SetActive(FriendItems[i].gameObject, state: false);
		}
	}

	public void UpdateFriendList()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		List<friend_info> list = new List<friend_info>(playerData.FriendInfo.MainPlayerFriendDic.Values);
		list = FriendInfo.SortFrientList(list);
		int num = Mathf.Min(list.Count, LineCount) - FriendItems.Count;
		int count = FriendItems.Count;
		curListFriendInfo = list;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(FriendItems[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{count + i}";
				FriendItemLogic component = gameObject.GetComponent<FriendItemLogic>();
				if (component != null)
				{
					component.transform.parent = FriendItems[0].transform.parent;
					component.transform.localScale = Vector3.one;
					component.transform.localPosition = Vector3.zero;
					FriendItems.Add(component);
				}
			}
		}
		for (int j = 0; j < FriendItems.Count; j++)
		{
			NGUITools.SetActive(FriendItems[j].gameObject, j < list.Count);
			if (j < list.Count)
			{
				ResetItemLine(FriendItems[j], j);
			}
		}
		uiWrapContent.minIndex = 1 - list.Count;
		uiWrapContent.maxIndex = 0;
		WrapContentBottomWidget.height = list.Count * uiWrapContent.itemSize;
		uiWrapContent.SortBasedOnScrollMovement();
		uiScrollView.ResetPosition();
		uiWrapContent.enabled = true;
		Times.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100220}"), playerData.FriendInfo.FriendCount);
	}

	public void OnClikcTips()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
		{
			SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", "#{100269}", null);
		});
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		FriendItemLogic itemLogic = FriendItems[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex));
	}

	private void ResetItemLine(FriendItemLogic itemLogic, int idx)
	{
		if (idx < curListFriendInfo.Count)
		{
			itemLogic.UpdateFriendInfo(curListFriendInfo[idx]);
		}
	}

	public void OnClickAdd()
	{
		FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
		if (friendInfo.IsCanAddFriend())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FriendAddUILogic, delegate
			{
				SingletonUnity<FriendAddUILogic>.Instance.Reset();
				NetLogic.GetInstance().Send<Protocol.req_random_online_character_list>();
			});
		}
		else
		{
			NoticeLogic.AddNotifyData("#{100227}");
		}
	}
}
