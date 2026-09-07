using UnityEngine;

public class PlayEffInfoMotionData
{
	public EffInfoData EffInfoData;

	public float DelayTime;

	public Vector3 SenderPos;

	public PlayEffInfoMotionData(string id, float delayTime, Vector3 senderPos)
	{
		EffInfoData = DataManager.GetEffInfoDataById(id);
		DelayTime = delayTime;
		SenderPos = senderPos;
	}
}
