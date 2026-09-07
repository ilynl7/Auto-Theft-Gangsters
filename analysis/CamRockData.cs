using System.Collections.Generic;
using UnityEngine;

public class CamRockData
{
	public string ID = string.Empty;

	public string CurveName = string.Empty;

	public int NeedRockTime;

	public int DelayTime;

	public int RockRate = 100;

	private bool mInitFlag;

	private AnimationCurve mXPosCurve;

	private AnimationCurve mYPosCurve;

	private AnimationCurve mZPosCurve;

	private AnimationCurve mXRotCurve;

	private AnimationCurve mYRotCurve;

	private AnimationCurve mZRotCurve;

	private AnimationCurve mWRotCurve;

	public float NeedRockTimeSecond => (float)NeedRockTime / 1000f;

	public float DelayTimeSecond => (float)DelayTime / 1000f;

	public AnimationCurve XPosCurve => mXPosCurve;

	public AnimationCurve YPosCurve => mYPosCurve;

	public AnimationCurve ZPosCurve => mZPosCurve;

	public AnimationCurve XRotCurve => mXRotCurve;

	public AnimationCurve YRotCurve => mYRotCurve;

	public AnimationCurve ZRotCurve => mZRotCurve;

	public AnimationCurve WRotCurve => mWRotCurve;

	public void Init()
	{
		if (!mInitFlag)
		{
			List<CamRockCurveData> camRockCurveDataListByName = DataManager.GetCamRockCurveDataListByName(CurveName);
			mXPosCurve = GetAnimaCurve(GetTargetTypeKey(camRockCurveDataListByName, 0));
			mYPosCurve = GetAnimaCurve(GetTargetTypeKey(camRockCurveDataListByName, 1));
			mZPosCurve = GetAnimaCurve(GetTargetTypeKey(camRockCurveDataListByName, 2));
			mXRotCurve = GetAnimaCurve(GetTargetTypeKey(camRockCurveDataListByName, 3));
			mYRotCurve = GetAnimaCurve(GetTargetTypeKey(camRockCurveDataListByName, 4));
			mZRotCurve = GetAnimaCurve(GetTargetTypeKey(camRockCurveDataListByName, 5));
			mWRotCurve = GetAnimaCurve(GetTargetTypeKey(camRockCurveDataListByName, 6));
			if (NeedRockTime == 0)
			{
				NeedRockTime = (int)(camRockCurveDataListByName[0].ClipLength * 1000f);
			}
			mInitFlag = true;
		}
	}

	private List<CamRockCurveData> GetTargetTypeKey(List<CamRockCurveData> sumList, int targetType)
	{
		List<CamRockCurveData> list = new List<CamRockCurveData>();
		for (int i = 0; i < sumList.Count; i++)
		{
			if (sumList[i].CurveType == targetType)
			{
				list.Add(sumList[i]);
			}
		}
		return list;
	}

	private AnimationCurve GetAnimaCurve(List<CamRockCurveData> keyList)
	{
		if (keyList != null && keyList.Count > 0)
		{
			keyList.Sort((CamRockCurveData preData, CamRockCurveData nextData) => preData.KeyFrameIndex - nextData.KeyFrameIndex);
			Keyframe[] array = new Keyframe[keyList.Count];
			for (int i = 0; i < keyList.Count; i++)
			{
				array[i].value = keyList[i].KeyValue;
				array[i].time = keyList[i].KeyTime;
				array[i].inTangent = keyList[i].KeyInTangent;
				array[i].outTangent = keyList[i].KeyOutTangent;
				array[i].tangentMode = keyList[i].KeyTangentMode;
			}
			AnimationCurve animationCurve = new AnimationCurve(array);
			animationCurve.preWrapMode = (WrapMode)keyList[0].PreWrapMode;
			animationCurve.postWrapMode = (WrapMode)keyList[0].PostWrapMode;
			return animationCurve;
		}
		return null;
	}
}
