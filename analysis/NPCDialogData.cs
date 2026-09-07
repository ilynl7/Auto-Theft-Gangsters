using System.Collections.Generic;

public class NPCDialogData
{
	public string ID = string.Empty;

	public string Dialog = string.Empty;

	public string OptionDialogID = string.Empty;

	public string MissionID = string.Empty;

	private List<string> mMissionIDList = new List<string>();

	public List<string> MissionIDList
	{
		get
		{
			if (mMissionIDList.Count == 0)
			{
				string[] array = MissionID.Split(';');
				for (int i = 0; i < array.Length; i++)
				{
					mMissionIDList.Add(array[i]);
				}
			}
			return mMissionIDList;
		}
	}

	private void AddMissionList(string missionID)
	{
		if (!string.IsNullOrEmpty(missionID))
		{
			mMissionIDList.Add(missionID);
		}
	}
}
