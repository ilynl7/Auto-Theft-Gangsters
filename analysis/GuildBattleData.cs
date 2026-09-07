public class GuildBattleData
{
	public string ID = string.Empty;

	public int LevelMin;

	public int LevelMax = int.MaxValue;

	public string ShowRewardID = string.Empty;

	public string Name = string.Empty;

	public string Desc = string.Empty;

	public string Rule = string.Empty;

	public string Background = string.Empty;

	public string Icon = string.Empty;

	public int Week = -1;

	public int StartTime = -1;

	public int Week1 = -1;

	public int StartTime1 = -1;

	public int MaxPlayNum;

	public int Week2 = -1;

	public int StartTime2 = -1;

	public int Week3 = -1;

	public int StartTime3 = -1;

	public int DurationTime = 1800;

	public int WaitTime = 10;

	public string FirstDropID = string.Empty;

	public int FirstRank;

	public string SecondDropID = string.Empty;

	public int SecondRank;

	public string ThirdDropID = string.Empty;

	public int ThirdRank;

	public string ParticipateDropID = string.Empty;

	public string RankDropID1 = string.Empty;

	public string RankDropID2 = string.Empty;

	public string RankDropID3 = string.Empty;

	public string RankDropID4 = string.Empty;

	public string RankDropID5 = string.Empty;

	public string RankDropID6 = string.Empty;

	public string RankDropID7 = string.Empty;

	public string RankDropID8 = string.Empty;

	public string WinDropID = string.Empty;

	public string FailureDropID = string.Empty;

	public string GuessID = string.Empty;

	public int GuessCost;

	public int GuessSucess = 100;

	public string MName => StrDictionary.GetDictionaryString(Name);

	public string MDesc => StrDictionary.GetDictionaryString(Desc);

	public string MRule
	{
		get
		{
			PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			return StrDictionary.GetDictionaryString(Rule, TimeTools.GetLocalShowTime_HM(StartTime1, playerCommonData.TimeOffset), TimeTools.GetLocalShowTime_HM(StartTime, playerCommonData.TimeOffset));
		}
	}
}
