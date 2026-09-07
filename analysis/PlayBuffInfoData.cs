public class PlayBuffInfoData
{
	public BuffInfoData BuffInfoData;

	public ObjCharacter Sender;

	public float DelayTime;

	public float Duration;

	public string FxEffectId = string.Empty;

	public PlayBuffInfoData(string buffID, float delayTime, float duration, ObjCharacter sender = null)
	{
		BuffInfoData = DataManager.GetBuffInfoDataByID(buffID);
		DelayTime = delayTime;
		Duration = duration;
		Sender = sender;
		FxEffectId = string.Empty;
	}
}
