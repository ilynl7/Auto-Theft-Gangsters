using UnityEngine;

public class BundleLoadYinChangData
{
	public EffInfoData effInfoData;

	public float duration;

	public Vector3 senderPos;

	public BundleLoadYinChangData(EffInfoData efData, float dur, Vector3 sdPos)
	{
		effInfoData = efData;
		duration = dur;
		senderPos = sdPos;
	}
}
