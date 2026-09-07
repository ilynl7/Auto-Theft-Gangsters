public class CityDanceData
{
	public string ID;

	public string MapId;

	public int Type;

	public string ShowRewardId;

	public string DropId;

	public int DurationTime;

	public int RewardDuration;

	public int UnlockLevel;

	public string Name;

	public string Description;

	public string SceneObjName;

	public string Rule;

	public string Icon;

	public string Background;

	public string NpcID;

	public string MissionIcon;

	public string StartTime;

	public int CDTime;

	private long[] mStartTimes;

	public long[] StartTimes
	{
		get
		{
			if ((mStartTimes == null || mStartTimes.Length == 0) && !string.IsNullOrEmpty(StartTime))
			{
				string[] array = StartTime.Split('#');
				mStartTimes = new long[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mStartTimes[i] = long.Parse(array[i]);
				}
			}
			return mStartTimes;
		}
	}
}
