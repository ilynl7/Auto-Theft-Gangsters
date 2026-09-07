using UnityEngine;

public class ConsignBuySubTabLogic : MonoBehaviour
{
	public delegate void OnClickTabDelegate(int itemType, int subType, int index);

	public UILabel TabLabel;

	private int mIndex;

	private int mItemType;

	private int mSubType;

	private OnClickTabDelegate onClickTabDel;

	public int ItemType => mItemType;

	public int SubType => mSubType;

	public void Reset(int itemType, int subType, string title, OnClickTabDelegate func, int index)
	{
		mItemType = itemType;
		mSubType = subType;
		TabLabel.text = StrDictionary.GetDictionaryString(title);
		onClickTabDel = func;
		mIndex = index;
	}

	public void OnClickTab()
	{
		if (onClickTabDel != null)
		{
			onClickTabDel(mItemType, mSubType, mIndex);
		}
	}
}
