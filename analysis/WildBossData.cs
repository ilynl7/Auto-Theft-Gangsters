public class WildBossData
{
	public string ID;

	public string MapID;

	public int Type;

	public string Desc;

	public string Rule;

	public int SubType;

	public string StartTime;

	public int DurationTime;

	public int WaitTime;

	public int ExistTime;

	public int MaxPlayer;

	public int LevelMin;

	public int LevelMax;

	public string LastKillDropID;

	public string FirstDropID;

	public int FirstRank;

	public string SecondDropID;

	public int SecondRank;

	public string ThirdDropID;

	public int ThirdRank;

	public string FourthDropID;

	public int FourthRank;

	public string FifthDropID;

	public int FifthRank;

	public string ParticipateDropID;

	public string ShowRewardID = "1";

	public string BossID;

	public string Icon;

	public string Background;

	public long MinHp = 100L;

	public long MaxHp = 2147483647L;

	public long SingleHp = 100L;

	public int PVP;

	public int Time;

	public int Limit;

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
