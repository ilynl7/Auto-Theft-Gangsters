using System.Collections.Generic;

public class CopySceneData
{
	public string ID = string.Empty;

	[ServerExclude("ServerNoUse")]
	public string Desc = string.Empty;

	[ServerExclude("ServerNoUse")]
	public string Rule = string.Empty;

	public string MapId = string.Empty;

	public string ShowRewardId = string.Empty;

	public string DropId = string.Empty;

	public int WaitTime;

	public int Type = -1;

	public int SubType = -1;

	public int ExistTime;

	public int EndTime = 5;

	public int Reload;

	public int Parm1;

	public int Parm2;

	public int Parm3 = -1;

	public int Parm4;

	[ServerExclude("ServerNoUse")]
	public string Name = string.Empty;

	public int MaxPlayNum;

	public int MinLevel;

	public int MaxLevel = 80;

	public int MinMember;

	public int MaxMember;

	[ServerExclude("ServerNoUse")]
	public string Background = string.Empty;

	public string Icon = string.Empty;

	public string StarDescription1;

	public string StarDescription2;

	public string StarDescription3;

	public string WipeItem;

	public int CanWipeOut = 1;

	public string FinishPoint;

	public string TimeInc = string.Empty;

	public string SingleMapID = string.Empty;

	public bool IsTeamCopy => MaxMember > 1;

	public bool IsCarChasingCopy => SubType == 7;

	public bool IsScuffleCopy => SubType == 20;

	public bool IsSingleDance => SubType == 26;

	public bool IsPVPMap => SubType == 1;

	public bool IsSexGame => SubType == 27;

	public bool IsCashCopy => SubType == 11;

	public bool IsExpCopy => SubType == 12 || SubType == 22;

	public bool IsEquipCopy => SubType == 16;

	public string MName => StrDictionary.GetDictionaryString(Name);

	public string BackgroundPath => "UI/CopyMissionPic/" + Background;

	public COPY_SCENE_TYPE CopyType => (COPY_SCENE_TYPE)Type;

	public bool IsPVPFlag()
	{
		if (SubType == 1 || SubType == 20)
		{
			return true;
		}
		return false;
	}

	public void GetStarDescriptionList(int gradeFlag, List<string> resultList)
	{
		resultList.Clear();
		if (gradeFlag % 2 == 1)
		{
			resultList.Add(StarDescription1);
		}
		gradeFlag /= 2;
		if (gradeFlag % 2 == 1)
		{
			resultList.Add(StarDescription2);
		}
		gradeFlag /= 2;
		if (gradeFlag % 2 == 1)
		{
			resultList.Add(StarDescription3);
		}
	}

	public bool IsCopyDifficult()
	{
		return int.Parse(ID) % 2 == 0;
	}
}
