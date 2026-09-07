public class InviteTeamInfo
{
	public long characterId = -1L;

	public long teamId = -1L;

	public long inviteTime = -1L;

	public string inviteName = string.Empty;

	public bool IsUrgeFlag;

	public string TeamGoalId = string.Empty;

	public InviteTeamInfo(long chaid, long teamid, string name, long time, string goalid)
	{
		characterId = chaid;
		teamId = teamid;
		inviteTime = time;
		inviteName = name;
		IsUrgeFlag = false;
		TeamGoalId = goalid;
	}

	public InviteTeamInfo(bool isurge, string name, long memberid, long time)
	{
		IsUrgeFlag = isurge;
		characterId = memberid;
		inviteName = name;
		inviteTime = time;
	}
}
