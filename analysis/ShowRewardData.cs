using System.Collections.Generic;
using UnityEngine;

public class ShowRewardData
{
	public string ID = string.Empty;

	public string Desc = string.Empty;

	public int Exp;

	public int Money1;

	public int Money2;

	public string Item1 = string.Empty;

	public int Quality1 = -1;

	public int ItemCount1 = -1;

	public string Item2 = string.Empty;

	public int Quality2 = -1;

	public int ItemCount2 = -1;

	public string Item3 = string.Empty;

	public int Quality3 = -1;

	public int ItemCount3 = -1;

	public string Item4 = string.Empty;

	public int Quality4 = -1;

	public int ItemCount4 = -1;

	public string Item5 = string.Empty;

	public int Quality5 = -1;

	public int ItemCount5 = -1;

	public string Item6 = string.Empty;

	public int Quality6 = -1;

	public int ItemCount6 = -1;

	public string Item7 = string.Empty;

	public int Quality7 = -1;

	public int ItemCount7 = -1;

	public string Item8 = string.Empty;

	public int Quality8 = -1;

	public int ItemCount8 = -1;

	private List<string> mItemIdList;

	private List<EQUIP_QUALITY> mQualityList;

	private List<int> mCountList;

	private bool mInitFlag;

	public List<string> ItemIdList
	{
		get
		{
			if (!mInitFlag)
			{
				InitData();
			}
			return mItemIdList;
		}
	}

	public List<EQUIP_QUALITY> QualityList
	{
		get
		{
			if (!mInitFlag)
			{
				InitData();
			}
			return mQualityList;
		}
	}

	public List<int> CountList
	{
		get
		{
			if (!mInitFlag)
			{
				InitData();
			}
			return mCountList;
		}
	}

	private void InitData()
	{
		mInitFlag = true;
		mItemIdList = new List<string>();
		mQualityList = new List<EQUIP_QUALITY>();
		mCountList = new List<int>();
		AddListItem(mItemIdList, Item1, mQualityList, Quality1, mCountList, ItemCount1);
		AddListItem(mItemIdList, Item2, mQualityList, Quality2, mCountList, ItemCount2);
		AddListItem(mItemIdList, Item3, mQualityList, Quality3, mCountList, ItemCount3);
		AddListItem(mItemIdList, Item4, mQualityList, Quality4, mCountList, ItemCount4);
		AddListItem(mItemIdList, Item5, mQualityList, Quality5, mCountList, ItemCount5);
		AddListItem(mItemIdList, Item6, mQualityList, Quality6, mCountList, ItemCount6);
		AddListItem(mItemIdList, Item7, mQualityList, Quality7, mCountList, ItemCount7);
		AddListItem(mItemIdList, Item8, mQualityList, Quality8, mCountList, ItemCount8);
	}

	private void AddListItem(List<string> itemList, string itemId, List<EQUIP_QUALITY> qualityList, int quality, List<int> countList, int count)
	{
		if (!string.IsNullOrEmpty(itemId))
		{
			itemList.Add(itemId);
			qualityList.Add((EQUIP_QUALITY)quality);
			countList.Add(count);
		}
	}

	public List<int> CalCountRatio()
	{
		float num = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.ServerLevelSealRatio();
		List<int> list = new List<int>();
		int num2 = 1;
		if (CountList != null && CountList.Count > 0)
		{
			for (int i = 0; i < CountList.Count; i++)
			{
				num2 = Mathf.CeilToInt((float)CountList[i] * num);
				if (num2 == 0)
				{
					num2 = 1;
				}
				list.Add(num2);
			}
		}
		return list;
	}
}
