public class MissionData
{
	public string ID = string.Empty;

	[ServerExclude("ServerNoUse")]
	public string Name = string.Empty;

	public string DescribeID = string.Empty;

	[ServerExclude("ServerNoUse")]
	public string TipDescribeID = string.Empty;

	[ServerExclude("ServerNoUse")]
	public string TagDescribeID = string.Empty;

	public int Class = -1;

	public int LogicType = -1;

	public string LogicID = string.Empty;

	public string PreID = string.Empty;

	public string NextID = string.Empty;

	public string AcceptDialog = string.Empty;

	public string TargetDialog = string.Empty;

	public string CompleteDialog = string.Empty;

	public string Target = string.Empty;

	public string TargetMapId = string.Empty;

	public string Accept = string.Empty;

	public string AcceptMapId = string.Empty;

	public string Submit = string.Empty;

	public string SubmitMapId = string.Empty;

	public int MinLv;

	public int DisplayLv;

	public int AutoAcceptLv;

	public int Repeat;

	public string XDShowID = string.Empty;

	public string XDDropID = string.Empty;

	public string QJShowID = string.Empty;

	public string QJDropID = string.Empty;

	public string NQSShowID = string.Empty;

	public string NQSDropID = string.Empty;

	public int ShowStoryState = -1;

	public string StoryID = string.Empty;

	public int IsMultiMission;

	public string TimeLimitId = string.Empty;

	public int TriggerTutorialType = -1;

	public string NextSideID = string.Empty;

	private string[] mNextSideIDList;

	public int ShowRank = -1;

	public string TipStr = string.Empty;

	public MISSION_TRIGGER_TUTORIAL_TYPE TrigerType => (MISSION_TRIGGER_TUTORIAL_TYPE)TriggerTutorialType;

	public MISSION_LOGICTYPE MissionLogicType => (MISSION_LOGICTYPE)LogicType;

	public string MName => StrDictionary.GetDictionaryString(Name);

	public string MDescribeID => StrDictionary.GetDictionaryString(DescribeID);

	public string MTipDescribeID => StrDictionary.GetDictionaryString(TipDescribeID);

	public string[] NextSideIDList
	{
		get
		{
			if (mNextSideIDList == null)
			{
				if (NextSideID.Contains(";"))
				{
					mNextSideIDList = NextSideID.Split(';');
				}
				else
				{
					mNextSideIDList = new string[1];
					mNextSideIDList[0] = NextSideID;
				}
			}
			return mNextSideIDList;
		}
	}

	public int GetTimeLimitMissionStar()
	{
		if (Class == 8)
		{
			TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(TimeLimitId);
			long missionRestTime = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetMissionRestTime(ID);
			if (missionRestTime >= timeLimitMissionDataByID.Reward3Time)
			{
				return 3;
			}
			if (missionRestTime >= timeLimitMissionDataByID.Reward2Time)
			{
				return 2;
			}
			if (missionRestTime >= timeLimitMissionDataByID.Reward1Time)
			{
				return 1;
			}
			return 0;
		}
		return 0;
	}
}
