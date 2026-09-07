public class DailyMissionData
{
	public string ID = string.Empty;

	public int LevelMax;

	public int LevelMin;

	public string ShowRewardID = string.Empty;

	public string DropID = string.Empty;

	[ServerExclude("ServerNoUse")]
	public string[] mShowRewardIDList;

	[ServerExclude("ServerNoUse")]
	public string[] mDropIDList;

	public string[] ShowRewardIDList
	{
		get
		{
			if (mShowRewardIDList == null)
			{
				mShowRewardIDList = ShowRewardID.Split('#');
			}
			return mShowRewardIDList;
		}
	}

	public string[] DropIDList
	{
		get
		{
			if (mDropIDList == null)
			{
				mDropIDList = DropID.Split('#');
			}
			return mDropIDList;
		}
	}
}
