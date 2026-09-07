using System.Collections.Generic;
using UnityEngine;

public class AnimationFit : MonoBehaviour
{
	public bool setFlag = true;

	public float defaultHight;

	public List<string> AnimNameList = new List<string>();

	private Dictionary<string, int> AnimNameDict = new Dictionary<string, int>();

	private float mCrossTime;

	private float mCountTime;

	private Vector3 targetPos;

	private Vector3 startPos;

	private void Awake()
	{
		for (int i = 0; i < AnimNameList.Count; i++)
		{
			AnimNameDict.Add(AnimNameList[i], 1);
		}
		AnimNameDict.Add("die", 1);
	}

	public void UpdateAnim(string name, float crossTime)
	{
		if (AnimNameDict.ContainsKey(name))
		{
			targetPos = Vector3.zero;
		}
		else
		{
			targetPos = Vector3.up * defaultHight;
		}
		startPos = base.transform.localPosition;
		mCrossTime = crossTime;
		mCountTime = 0f;
	}

	private void Update()
	{
		if (mCrossTime > 0f)
		{
			mCountTime += Time.deltaTime;
			base.transform.localPosition = Vector3.Lerp(startPos, targetPos, mCountTime / mCrossTime);
			if (mCountTime >= mCrossTime)
			{
				mCrossTime = -1f;
			}
		}
	}
}
