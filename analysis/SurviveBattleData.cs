public class SurviveBattleData
{
	public string ID;

	public string MapID;

	public string MapID2;

	public int Type;

	public string StartTime;

	public int DurationTime;

	public int WaitTime;

	public int ExistTime;

	public int MaxPlayer;

	public int UnlockLevel;

	public int FirstMaxScore;

	public int SecondMinScore;

	public string FirstDropID;

	public int FirstRank;

	public string SecondDropID;

	public int SecondRank;

	public string ThirdDropID;

	public int ThirdRank;

	public string FourthDropID;

	public int FourthRank;

	public string FifthDropID;

	public string ShowRewardID;

	public string Name;

	public string Desc;

	public string Rule;

	public string Icon;

	public string Background;

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
