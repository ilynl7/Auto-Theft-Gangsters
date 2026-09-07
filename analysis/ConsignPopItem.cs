using UnityEngine;

public class ConsignPopItem : MonoBehaviour
{
	public delegate void OnClickItemDelegate(int index, string val);

	private int mItemIndex;

	public UILabel ItemLabel;

	private OnClickItemDelegate onClickItem;

	public void Reset(int index, string str, OnClickItemDelegate func)
	{
		string text = StrDictionary.GetDictionaryString(str);
		int num = text.LastIndexOf(' ');
		if (num >= 0)
		{
			text = text.Substring(0, num);
		}
		ItemLabel.text = text;
		mItemIndex = index;
		onClickItem = func;
	}

	public void OnClickItem()
	{
		if (onClickItem != null)
		{
			onClickItem(mItemIndex, ItemLabel.text);
		}
	}
}
