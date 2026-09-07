using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ChangeTimeRootLogic : SingletonUnity<ChangeTimeRootLogic>
{
	public List<TimeItem> TimeList;

	public UIGrid ParentGrid;

	public UIScrollView panelView;

	private int ProSelect;

	private long[] CurTimes;

	private int CurSelect;

	public UILabel infoLabel;

	private int DuringTime;

	public void ResetGuildDance(long[] timelist, int select, string name, int duringtime)
	{
		ProSelect = select;
		CurTimes = timelist;
		CurSelect = select;
		DuringTime = duringtime;
		infoLabel.text = StrDictionary.GetDictionaryString("#{105099}", name);
		if (timelist != null)
		{
			int num = timelist.Length - TimeList.Count;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = Object.Instantiate(TimeList[0].gameObject) as GameObject;
					gameObject.transform.parent = ParentGrid.transform;
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.name = $"{TimeList.Count:d2}";
					TimeList.Add(gameObject.GetComponent<TimeItem>());
				}
			}
		}
		for (int j = 0; j < TimeList.Count; j++)
		{
			if (j < CurTimes.Length)
			{
				NGUITools.SetActive(TimeList[j].gameObject, state: true);
				TimeList[j].Reset(CurTimes[j], j, ProSelect, OnClickItem);
			}
			else
			{
				NGUITools.SetActive(TimeList[j].gameObject, state: false);
			}
		}
		panelView.ResetPosition();
		ParentGrid.Reposition();
	}

	public void OnClickConfirmBtn()
	{
		if (CurSelect != ProSelect)
		{
			if (TimeTools.IsTimeRange(CurTimes[CurSelect], CurTimes[CurSelect] + DuringTime))
			{
				NoticeLogic.AddNotifyData("#{105101}");
				return;
			}
			update_guild_dance_time.request request = new update_guild_dance_time.request();
			request.index = CurSelect + 1;
			NetLogic.GetInstance().Send<Protocol.update_guild_dance_time>(request);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ChangeTimeRoot);
		}
		else
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ChangeTimeRoot);
		}
	}

	public void OnClickCancelBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ChangeTimeRoot);
	}

	public void OnClickItem(int select)
	{
		if (CurSelect != select)
		{
			CurSelect = select;
			for (int i = 0; i < TimeList.Count; i++)
			{
				TimeList[i].UpdateSelect(select);
			}
		}
	}
}
