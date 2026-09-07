public class BigPackageTimeListData
{
	public string ID;

	public string StartTime;

	public string EndTime;

	private int[] mEndTimes;

	private int[] mStarttimes;

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

	public int[] Starttimes
	{
		get
		{
			if ((mStarttimes == null || mStarttimes.Length == 0) && !string.IsNullOrEmpty(StartTime))
			{
				string[] array = StartTime.Split('-');
				mStarttimes = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mStarttimes[i] = int.Parse(array[i]);
				}
			}
			return mStarttimes;
		}
	}
}
