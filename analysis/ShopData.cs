public class ShopData
{
	public string ID = string.Empty;

	public int Shop;

	public string ItemID = string.Empty;

	public string Name = string.Empty;

	public int ProfessionType = -1;

	public int ItemType;

	public int Quality;

	public int PriceType;

	public int Price;

	public int Limit;

	public int SingleLimit = 30;

	public int Discount = 100;

	public int Class;

	public int Weight = int.MaxValue;

	public string StartTime = string.Empty;

	public string EndTime = string.Empty;

	public int OnlyBuyCount = -1;

	public int MinLevel;

	public int MaxLevel = int.MaxValue;

	private int[] mStartTimes;

	private int[] mEndTimes;

	public int[] StartTimes
	{
		get
		{
			if ((mStartTimes == null || mStartTimes.Length == 0) && !string.IsNullOrEmpty(StartTime))
			{
				string[] array = StartTime.Split('-');
				mStartTimes = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mStartTimes[i] = int.Parse(array[i]);
				}
			}
			return mStartTimes;
		}
	}

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
}
