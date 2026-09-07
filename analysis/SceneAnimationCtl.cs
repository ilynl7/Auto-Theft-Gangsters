using System.Collections.Generic;
using UnityEngine;

public class SceneAnimationCtl : MonoBehaviour
{
	public List<SceneSubAnimationCtl> SubAnimaList;

	private int mCurSceneIndex;

	private int mSceneCount;

	private DelegateDefine.NoParamDelegate onFinished;

	private void Start()
	{
		Init();
		PlaySceneAnima();
	}

	public void RegisterOnFinished(DelegateDefine.NoParamDelegate func = null)
	{
		onFinished = func;
	}

	private void Init()
	{
		mCurSceneIndex = 0;
		mSceneCount = SubAnimaList.Count;
		for (int i = 0; i < SubAnimaList.Count; i++)
		{
			SubAnimaList[i].Init(OnSceneAnimaFinished);
			UnityVersionUtil.SetActiveRecursive(SubAnimaList[i].gameObject, state: false);
		}
	}

	private void OnSceneAnimaFinished()
	{
		mCurSceneIndex++;
		if (mCurSceneIndex < mSceneCount)
		{
			PlaySceneAnima();
			return;
		}
		if (onFinished != null)
		{
			onFinished();
		}
		Object.Destroy(base.gameObject);
	}

	private void PlaySceneAnima()
	{
		SubAnimaList[mCurSceneIndex].StartScene();
	}

	public void SkipAnima()
	{
		if (onFinished != null)
		{
			onFinished();
		}
		Object.Destroy(base.gameObject);
	}
}
