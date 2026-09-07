using System.Collections.Generic;
using UnityEngine;

public class PlayEffInfoData
{
	public FxEffInfoData mFxEffinfoData;

	public float DelayTime;

	public EffInfoData EffinfoData;

	public Vector3 SenderPos;

	public FxControl fxControl;

	public ActionData mActionData;

	public float DurationTime;

	public Transform targetTransform;

	public float LoadStartTime;

	public string fxLoadPath => mFxEffinfoData.EffFilePath + "/" + mFxEffinfoData.EffName;

	public PlayEffInfoData(FxEffInfoData fxData, EffInfoData effData, float delayTime, Vector3 senderPos, Transform tarTransform = null)
	{
		mFxEffinfoData = fxData;
		DelayTime = mFxEffinfoData.fEffDelayTimeSeconds + delayTime;
		EffinfoData = effData;
		SenderPos = senderPos;
		targetTransform = tarTransform;
	}

	public static List<PlayEffInfoData> GetPlayEffInfoDataList(string actionName, string effInfoID, float delayTime, Vector3 senderPos, Transform targetTransform = null)
	{
		List<PlayEffInfoData> list = null;
		ActionData actionDataByName = DataManager.GetActionDataByName(actionName);
		if (actionDataByName != null)
		{
			List<FxEffInfoData> fxEffInfoDataListById = DataManager.GetFxEffInfoDataListById(actionDataByName.FxEffID);
			EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(effInfoID);
			if (fxEffInfoDataListById != null)
			{
				for (int i = 0; i < fxEffInfoDataListById.Count; i++)
				{
					PlayEffInfoData playEffInfoData = new PlayEffInfoData(fxEffInfoDataListById[i], effInfoDataById, delayTime, senderPos, targetTransform);
					playEffInfoData.DurationTime = fxEffInfoDataListById[i].EffDurationTimeSeconds;
					playEffInfoData.mActionData = actionDataByName;
					if (list == null)
					{
						list = new List<PlayEffInfoData>();
					}
					list.Add(playEffInfoData);
				}
			}
		}
		return list;
	}

	public static List<PlayEffInfoData> GetPlayBufEffInfoDataList(string mFxEffID, float delayTime, float duration, Vector3 senderPos)
	{
		List<PlayEffInfoData> list = null;
		List<FxEffInfoData> fxEffInfoDataListById = DataManager.GetFxEffInfoDataListById(mFxEffID);
		if (fxEffInfoDataListById != null)
		{
			for (int i = 0; i < fxEffInfoDataListById.Count; i++)
			{
				PlayEffInfoData playEffInfoData = new PlayEffInfoData(fxEffInfoDataListById[i], null, delayTime, senderPos);
				playEffInfoData.DurationTime = duration;
				playEffInfoData.mActionData = null;
				if (list == null)
				{
					list = new List<PlayEffInfoData>();
				}
				list.Add(playEffInfoData);
			}
		}
		return list;
	}
}
