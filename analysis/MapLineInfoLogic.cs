using System.Collections.Generic;
using UnityEngine;

public class MapLineInfoLogic : SingletonUnity<MapLineInfoLogic>
{
	public UIWrapContentNew UIWrapContent;

	public UIScrollView ScrollView;

	public UIWidget WrapContentBottomWidget;

	public List<MapLineItemLogic> MapLineItems = new List<MapLineItemLogic>();

	public List<long> curLineStates = new List<long>();

	public int LineCount = 7;

	private int curLineCount;

	private int curRealCount;

	private new void Awake()
	{
		base.Awake();
		UIWrapContent.onInitializeItem = OnInitializeItem;
	}

	public void PreReset()
	{
		UpdateItems();
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		MapLineItemLogic itemLogic = MapLineItems[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex));
	}

	private void ResetItemLine(MapLineItemLogic itemLogic, int idx)
	{
		if (idx < curLineCount)
		{
			int num = idx * 2;
			int num2 = idx * 2 + 1;
			if (num2 >= curRealCount)
			{
				num2 = -2;
			}
			int state = -2;
			if (curLineStates.Count > num)
			{
				state = (int)curLineStates[num];
			}
			int state2 = -2;
			if (curLineStates.Count > num2 && num2 >= 0)
			{
				state2 = (int)curLineStates[num2];
			}
			itemLogic.UpdateLineState(num, state, num2, state2, idx);
		}
	}

	public void UpdateItems()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		curRealCount = playerData.LineCount;
		if (playerData.LineStates != null)
		{
			curLineStates = playerData.LineStates;
		}
		int num = (curLineCount = (curRealCount + 1) / 2);
		int num2 = Mathf.Min(num, LineCount) - MapLineItems.Count;
		for (int i = 0; i < num2; i++)
		{
			GameObject gameObject = Object.Instantiate(MapLineItems[0].gameObject) as GameObject;
			gameObject.name = $"LineItem_{MapLineItems.Count}";
			gameObject.transform.parent = MapLineItems[0].transform.parent;
			gameObject.transform.localScale = Vector3.one;
			MapLineItems.Add(gameObject.GetComponent<MapLineItemLogic>());
		}
		for (int j = 0; j < MapLineItems.Count; j++)
		{
			NGUITools.SetActive(MapLineItems[j].gameObject, j < num);
			if (j < num)
			{
				ResetItemLine(MapLineItems[j], j);
			}
		}
		UIWrapContent.maxIndex = 0;
		UIWrapContent.minIndex = 1 - num;
		WrapContentBottomWidget.height = num * UIWrapContent.itemSize;
		ScrollView.ResetPosition();
		UIWrapContent.SortBasedOnScrollMovement();
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MapLineInfoLogic);
	}
}
