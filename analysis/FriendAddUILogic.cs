using System.Collections.Generic;
using System.Text.RegularExpressions;
using SprotoType;
using UnityEngine;

public class FriendAddUILogic : SingletonUnity<FriendAddUILogic>
{
	public enum OPEN_TYPE
	{
		RANDOM_TYPE,
		SEARCH_TYPE
	}

	public List<FriendAddItemLogic> FriendItems = new List<FriendAddItemLogic>();

	private List<friend_info> curListFriendInfo = new List<friend_info>();

	public UIGrid grid;

	public UIInput uiInput;

	public GameObject refreshObj;

	private OPEN_TYPE curOpenType;

	protected override void Awake()
	{
		base.Awake();
		InitList();
	}

	public void Reset()
	{
		InitList();
	}

	private void OnEnable()
	{
		curOpenType = OPEN_TYPE.RANDOM_TYPE;
		InitList();
	}

	private void InitList()
	{
		for (int i = 0; i < FriendItems.Count; i++)
		{
			NGUITools.SetActive(FriendItems[i].gameObject, state: false);
		}
	}

	public void SetOpenType(OPEN_TYPE type)
	{
		curOpenType = type;
	}

	public void UpdateFriendList()
	{
		List<friend_info> list = null;
		NGUITools.SetActive(refreshObj, curOpenType == OPEN_TYPE.RANDOM_TYPE);
		FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
		list = ((curOpenType != 0) ? new List<friend_info>(friendInfo.MainPlayerSearchFriendDic.Values) : new List<friend_info>(friendInfo.MainPlayerRandomFriendDic.Values));
		int num = list.Count - FriendItems.Count;
		int count = FriendItems.Count;
		curListFriendInfo = list;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(FriendItems[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{count + i}";
				FriendAddItemLogic component = gameObject.GetComponent<FriendAddItemLogic>();
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
		grid.Reposition();
	}

	private void ResetItemLine(FriendAddItemLogic itemLogic, int idx)
	{
		if (idx < curListFriendInfo.Count)
		{
			itemLogic.UpdateFriendInfo(curListFriendInfo[idx]);
		}
	}

	public void OnClickRefresh()
	{
		NetLogic.GetInstance().Send<Protocol.req_random_online_character_list>();
	}

	public void OnClickSearch()
	{
		string value = uiInput.value;
		value = value.Trim();
		if (string.IsNullOrEmpty(value))
		{
			NoticeLogic.AddNotifyData("#{100274}");
			return;
		}
		if (!isRightName(value))
		{
			NoticeLogic.AddNotifyData("#{100222}");
			return;
		}
		search_online_character_by_name.request request = new search_online_character_by_name.request();
		request.name = value;
		NetLogic.GetInstance().Send<Protocol.search_online_character_by_name>(request);
		curOpenType = OPEN_TYPE.SEARCH_TYPE;
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Social", "Friend", "search_times");
	}

	private bool isRightName(string namestr)
	{
		string pattern = "^[a-zA-Z0-9]{1}([a-zA-Z0-9]|[ _]){4,15}$";
		if (Regex.IsMatch(namestr, pattern))
		{
			return true;
		}
		return false;
	}

	public void OnClickClose()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FriendAddUILogic);
	}
}
