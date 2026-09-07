public class PlayingEffectData
{
	public string EffectDataId;

	public long OwnerId;

	public PlayingEffectData(long ownerId, string effectId)
	{
		EffectDataId = effectId;
		OwnerId = ownerId;
	}
}
