public class TeamData
{
	public string ID;

	public int GoalType;

	public string TitleName;

	public string CopyId;

	public string ParentId;

	public string MTitleName => StrDictionary.GetDictionaryString(TitleName);

	public string GetTeamTitle()
	{
		if (string.IsNullOrEmpty(TitleName))
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(CopyId);
			return copySceneDataById.Name;
		}
		return TitleName;
	}
}
