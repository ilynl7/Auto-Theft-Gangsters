using System.Collections.Generic;

public class TimeLimitMissionData
{
	public string ID;

	public string Name;

	public string Desc;

	public string Tip;

	public string ContainsMission;

	public long LimitTime;

	public int LimitNum;

	public string Reward1;

	public string ShowReward1;

	public long Reward1Time;

	public string Reward2;

	public string ShowReward2;

	public long Reward2Time;

	public string Reward3;

	public string ShowReward3;

	public long Reward3Time;

	public string ShowRewardId;

	private List<string> mMissionList;

	public string MName => StrDictionary.GetDictionaryString(Name);

	public string MDesc => StrDictionary.GetDictionaryString(Desc);

	public string MTip => StrDictionary.GetDictionaryString(Tip);

	public List<string> MissionList
	{
		get
		{
			if (mMissionList == null)
			{
				mMissionList = new List<string>();
				string[] array = ContainsMission.Split('#');
				for (int i = 0; i < array.Length; i++)
				{
					MissionList.Add(array[i]);
				}
			}
			return mMissionList;
		}
	}
}
