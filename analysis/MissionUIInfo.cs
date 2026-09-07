public class MissionUIInfo
{
	public string MissionID;

	public MISSION_STATE uiType;

	public MissionUIInfo(string missionId, MISSION_STATE type)
	{
		MissionID = missionId;
		uiType = type;
	}
}
