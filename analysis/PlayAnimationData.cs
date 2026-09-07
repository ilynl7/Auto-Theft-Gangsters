public class PlayAnimationData
{
	public string ActionName = string.Empty;

	public float DelayTime;

	public string EffInfoID;

	public PlayAnimationData(string actionName, float delayTime, string effInfoID)
	{
		ActionName = actionName;
		DelayTime = delayTime;
		EffInfoID = effInfoID;
	}
}
