using System.Collections.Generic;

public class BigPackageData
{
	public string ID;

	public string XDItemID1;

	public int XDItemCount1;

	public int XDQuality1;

	public string XDItemID2;

	public int XDItemCount2;

	public int XDQuality2;

	public string XDItemID3;

	public int XDItemCount3;

	public int XDQuality3;

	public string XDItemID4;

	public int XDItemCount4;

	public int XDQuality4;

	public string QJItemID1;

	public int QJItemCount1;

	public int QJQuality1;

	public string QJItemID2;

	public int QJItemCount2;

	public int QJQuality2;

	public string QJItemID3;

	public int QJItemCount3;

	public int QJQuality3;

	public string QJItemID4;

	public int QJItemCount4;

	public int QJQuality4;

	public string NQItemID1;

	public int NQItemCount1;

	public int NQQuality1;

	public string NQItemID2;

	public int NQItemCount2;

	public int NQQuality2;

	public string NQItemID3;

	public int NQItemCount3;

	public int NQQuality3;

	public string NQItemID4;

	public int NQItemCount4;

	public int NQQuality4;

	public string ItemID5;

	public int ItemCount5;

	public int Quality5;

	public int PriceType = -1;

	public int PriceCost;

	public string ProductId = string.Empty;

	public string Dollor;

	public int MaxCount = -1;

	public int TimeHour = -1;

	public string StartTime = string.Empty;

	public string EndTime = string.Empty;

	public int SellType;

	public int sortID = int.MaxValue;

	public int showmodeltype;

	public string TextureTitle1;

	public string TextureTitle2;

	public int LevelMin;

	public int LevelMax = 80;

	public string ServerID;

	public string FatherID;

	public int RechargeMin;

	public int RechargeMax;

	public int Discount;

	public string Name;

	public string Icon;

	public int IconQuality;

	public string TimeList = string.Empty;

	private string[] mTimeList;

	private int[] mEndTimes;

	public int[] EndTimes
	{
		get
		{
			if ((mEndTimes == null || mEndTimes.Length == 0) && !string.IsNullOrEmpty(EndTime))
			{
				string[] array = EndTime.Split('-');
				mEndTimes = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mEndTimes[i] = int.Parse(array[i]);
				}
			}
			return mEndTimes;
		}
	}

	public int[] GetCurTimeEnd()
	{
		if (!string.IsNullOrEmpty(TimeList))
		{
			if (mTimeList == null || mTimeList.Length == 0)
			{
				mTimeList = TimeList.Split('#');
			}
			if (mTimeList != null && mTimeList.Length > 0)
			{
				List<BigPackageTimeListData> bigPackageTimeListDataListById = DataManager.GetBigPackageTimeListDataListById(mTimeList);
				for (int i = 0; i < bigPackageTimeListDataListById.Count; i++)
				{
					if (TimeTools.IsTimeRange(bigPackageTimeListDataListById[i].Starttimes, bigPackageTimeListDataListById[i].EndTimes))
					{
						return bigPackageTimeListDataListById[i].EndTimes;
					}
				}
			}
		}
		return null;
	}
}
