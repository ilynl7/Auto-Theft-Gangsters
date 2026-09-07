using System.Collections.Generic;
using UnityEngine;

public class ConsignBuyTabLogic : MonoBehaviour
{
	public delegate void OnClickTabDelegate(int itemType, int subType, int tabIndex, int subIndex);

	public delegate void OnClickTopTabDelegate(int index);

	private OnClickTabDelegate onClickTabDelegate;

	private OnClickTopTabDelegate onClickTopTabDelegate;

	public TweenScale SubTweenRoot;

	public UILabel TypeLabel;

	public UISprite RootPic;

	private int mIndex;

	public List<ConsignBuySubTabLogic> SubTabList;

	private int preClickIndex;

	private bool mIsOpen;

	public void SetMinusPic(bool isMinus)
	{
		if (isMinus)
		{
			RootPic.spriteName = $"CZ_paiMai_-";
			RootPic.MakePixelPerfect();
		}
		else
		{
			RootPic.spriteName = $"CZ_paiMai_+";
			RootPic.MakePixelPerfect();
		}
		mIsOpen = isMinus;
	}

	public void Reset(List<ConsignBuyTabData> curTabDataList, OnClickTabDelegate func, int index, OnClickTopTabDelegate clickTopTabFunc)
	{
		SetMinusPic(isMinus: false);
		TypeLabel.text = StrDictionary.GetDictionaryString(curTabDataList[0].TopTabName);
		int num = curTabDataList.Count - SubTabList.Count;
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = Object.Instantiate(SubTabList[0].gameObject) as GameObject;
			ConsignBuySubTabLogic component = gameObject.GetComponent<ConsignBuySubTabLogic>();
			gameObject.transform.parent = SubTabList[0].transform.parent;
			gameObject.transform.localPosition = new Vector3(0f, -16 - 32 * (i + 1), 0f);
			SubTabList.Add(component);
		}
		for (int j = 0; j < curTabDataList.Count; j++)
		{
			SubTabList[j].Reset(curTabDataList[j].ItemTypeId, curTabDataList[j].SubTypeId, curTabDataList[j].SubTypeName, OnClickSubTab, j);
		}
		SubTabList[0].transform.parent.localScale = new Vector3(1f, 0.001f, 1f);
		onClickTabDelegate = func;
		onClickTopTabDelegate = clickTopTabFunc;
		mIndex = index;
	}

	public void OnClickTab()
	{
		if (mIsOpen)
		{
			SetMinusPic(isMinus: false);
			return;
		}
		SetMinusPic(isMinus: true);
		if (onClickTopTabDelegate != null)
		{
			onClickTopTabDelegate(mIndex);
		}
	}

	private void OnClickSubTab(int itemType, int subType, int index)
	{
		if (onClickTabDelegate != null)
		{
			onClickTabDelegate(itemType, subType, mIndex, index);
		}
		preClickIndex = index;
	}
}
