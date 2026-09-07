public class TimerActivityData
{
	public string ID;

	public string EventName;

	public string EventDialog;

	public int EventType;

	public string StartTime;

	public string EndTime;

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
