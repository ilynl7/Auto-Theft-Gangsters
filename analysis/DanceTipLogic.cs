using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DanceTipLogic : MonoBehaviour
{
	public List<DanceTipLineLogic> DanceList;

	private PlayerData mPlayerdata;

	private List<dance_state_info> CurDanceList;

	public UIWrapContentNew uiWrapContent;

	private int lineMinCount = 6;

	public UIWidget WrapContentBottomWidget;

	public UIScrollView uiScrollView;

	private void Awake()
	{
		uiWrapContent.enabled = false;
		UIWrapContentNew uIWrapContentNew = uiWrapContent;
		uIWrapContentNew.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContentNew.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
	}

	public void Reset()
	{
		mPlayerdata = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		CurDanceList = new List<dance_state_info>(mPlayerdata.ActivityData.CurDanceStateDic.Values);
		if (CurDanceList != null || CurDanceList.Count > 0)
		{
			int num = Mathf.Min(CurDanceList.Count, lineMinCount) - DanceList.Count;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(DanceList[0].gameObject) as GameObject;
					DanceTipLineLogic component = gameObject.GetComponent<DanceTipLineLogic>();
					gameObject.name = $"{DanceList.Count:D2}";
					gameObject.transform.parent = uiWrapContent.transform;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					DanceList.Add(component);
				}
			}
			for (int j = 0; j < DanceList.Count; j++)
			{
				if (j < CurDanceList.Count)
				{
					NGUITools.SetActive(DanceList[j].gameObject, state: true);
					DanceList[j].Reset(CurDanceList[j]);
				}
				else
				{
					NGUITools.SetActive(DanceList[j].gameObject, state: false);
				}
			}
		}
		else
		{
			for (int k = 0; k < DanceList.Count; k++)
			{
				NGUITools.SetActive(DanceList[k].gameObject, state: false);
			}
		}
		uiWrapContent.minIndex = 1 - CurDanceList.Count;
		WrapContentBottomWidget.height = CurDanceList.Count * uiWrapContent.itemSize;
		uiWrapContent.SortBasedOnScrollMovement();
		uiScrollView.ResetPosition();
		uiWrapContent.enabled = true;
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		DanceTipLineLogic itemLogic = DanceList[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex));
	}

	private void ResetItemLine(DanceTipLineLogic itemLogic, int idx)
	{
		if (idx < CurDanceList.Count)
		{
			itemLogic.Reset(CurDanceList[idx]);
		}
	}
}
