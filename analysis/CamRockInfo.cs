using System;
using UnityEngine;

[Serializable]
public class CamRockInfo
{
	public string CamRockId = string.Empty;

	public float RockTime;

	public float NeedRockTime;

	public float DelayTime;

	public AnimationCurve XPosCurve;

	public AnimationCurve YPosCurve;

	public AnimationCurve ZPosCurve;

	public AnimationCurve XRotCurve;

	public AnimationCurve YRotCurve;

	public AnimationCurve ZRotCurve;

	public AnimationCurve WRotCurve;

	public void Init()
	{
		CamRockId = string.Empty;
		RockTime = 0f;
		NeedRockTime = 0f;
		DelayTime = 0f;
		XPosCurve = null;
		YPosCurve = null;
		ZPosCurve = null;
		XRotCurve = null;
		YRotCurve = null;
		ZRotCurve = null;
		WRotCurve = null;
	}

	public bool IsValid()
	{
		return !string.IsNullOrEmpty(CamRockId);
	}
}
