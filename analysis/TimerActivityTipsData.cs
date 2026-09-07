public class TimerActivityTipsData
{
	public string ID = string.Empty;

	public string EventName = string.Empty;

	public string StartTime = string.Empty;

	public string EndTime = string.Empty;

	public string PicName = string.Empty;

	public int Weight = int.MaxValue;

	public int JumpType = -1;

	public string JumpLabel = string.Empty;

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
