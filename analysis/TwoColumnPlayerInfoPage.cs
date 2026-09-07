using System.Collections.Generic;
using UnityEngine;

public class TwoColumnPlayerInfoPage : MonoBehaviour
{
	public UIGrid GrideRoot;

	public UIScrollBar ScrollBar;

	public UIScrollView ScrollView;

	public UIWrapContentNew WrapContent;

	public List<SmallPlayerInfoLine> mPlayerInfoLineList;

	public UIWidget BottomWidget;

	public int MaxLineCount = 6;

	private List<PlayerInfoItemData> mCurPageDataList;

	private DelegateDefine.OneLongParamDelegate onClickBtn;

	private string mBtnName;

	private string mDisableBtnName;

	private void Awake()
	{
		WrapContent.onInitializeItem = OnInitializeItem;
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		SmallPlayerInfoLine smallPlayerInfoLine = mPlayerInfoLineList[index];
		PlayerInfoItemData leftData = null;
		PlayerInfoItemData rightData = null;
		realIndex = Mathf.Abs(realIndex);
		if (mCurPageDataList != null)
		{
			if (mCurPageDataList.Count > realIndex * 2)
			{
				leftData = mCurPageDataList[realIndex * 2];
			}
			if (mCurPageDataList.Count > realIndex * 2 + 1)
			{
				rightData = mCurPageDataList[realIndex * 2 + 1];
			}
		}
		smallPlayerInfoLine.Reset(leftData, rightData, mBtnName, mDisableBtnName, OnClickBtn);
	}

	public void Reset(List<PlayerInfoItemData> dataList, string btnName, string disableBtnName, DelegateDefine.OneLongParamDelegate func)
	{
		if (dataList == null || dataList.Count == 0)
		{
			for (int i = 0; i < mPlayerInfoLineList.Count; i++)
			{
				NGUITools.SetActive(mPlayerInfoLineList[i].gameObject, state: false);
			}
			return;
		}
		onClickBtn = func;
		mBtnName = btnName;
		mDisableBtnName = disableBtnName;
		mCurPageDataList = dataList;
		if (mPlayerInfoLineList.Count < MaxLineCount && mPlayerInfoLineList.Count < (dataList.Count + 1) / 2)
		{
			int num = Mathf.Min(MaxLineCount - mPlayerInfoLineList.Count, (dataList.Count + 1) / 2 - mPlayerInfoLineList.Count);
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = Object.Instantiate(mPlayerInfoLineList[0].gameObject) as GameObject;
				gameObject.name = $"{mPlayerInfoLineList.Count}";
				SmallPlayerInfoLine component = gameObject.GetComponent<SmallPlayerInfoLine>();
				mPlayerInfoLineList.Add(component);
				gameObject.transform.parent = mPlayerInfoLineList[0].transform.parent;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = new Vector3(0f, -mPlayerInfoLineList.Count * WrapContent.itemSize, 0f);
			}
		}
		for (int k = 0; k < mPlayerInfoLineList.Count; k++)
		{
			NGUITools.SetActive(mPlayerInfoLineList[k].gameObject, k < MaxLineCount && k < (dataList.Count + 1) / 2);
		}
		int maxIndex = 0;
		int num2 = -((dataList.Count + 1) / 2 - 1);
		BottomWidget.height = (Mathf.Abs(num2) + 1) * WrapContent.itemSize;
		WrapContent.maxIndex = maxIndex;
		WrapContent.minIndex = num2;
		WrapContent.SortBasedOnScrollMovement();
		ScrollView.ResetPosition();
		ScrollBar.value = 0f;
	}

	private void OnClickBtn(long key)
	{
		if (onClickBtn != null)
		{
			onClickBtn(key);
		}
	}
}
