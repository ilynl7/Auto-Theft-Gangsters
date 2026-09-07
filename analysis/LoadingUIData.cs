public class LoadingUIData
{
	public string ID;

	public string Name;

	public int ShowFlag;

	public int MinLevel;

	public int MaxLevel;

	public string StartTime = string.Empty;

	public string EndTime = string.Empty;

	private int[] mStartTimeList;

	private int[] mEndTimeList;

	public int[] StartTimeList
	{
		get
		{
			if ((mStartTimeList == null || mStartTimeList.Length == 0) && !string.IsNullOrEmpty(StartTime))
			{
				string[] array = StartTime.Split('-');
				mStartTimeList = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mStartTimeList[i] = int.Parse(array[i]);
				}
			}
			return mStartTimeList;
		}
	}

	public int[] EndTimeList
	{
		get
		{
			if ((mEndTimeList == null || mEndTimeList.Length == 0) && !string.IsNullOrEmpty(EndTime))
			{
				string[] array = EndTime.Split('-');
				mEndTimeList = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mEndTimeList[i] = int.Parse(array[i]);
				}
			}
			return mEndTimeList;
		}
	}
}
