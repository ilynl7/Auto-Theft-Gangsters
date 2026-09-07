using UnityEngine;

public class CarMissionStartTimeCountRoot : SingletonUnity<CarMissionStartTimeCountRoot>
{
	public UISprite[] NumLabelPic;

	private float mStartTime;

	private int mCurPicIndex;

	private DelegateDefine.NoParamDelegate onCountFinish;

	private int tempNum;

	private int mCountSoundId = 42;

	public void Reset(float startTime, DelegateDefine.NoParamDelegate onFinish = null)
	{
		mStartTime = startTime;
		mCurPicIndex = 0;
		for (int i = 0; i < NumLabelPic.Length; i++)
		{
			NGUITools.SetActive(NumLabelPic[i].gameObject, state: false);
		}
		NGUITools.SetActive(NumLabelPic[0].gameObject, state: true);
		onCountFinish = onFinish;
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(mCountSoundId);
	}

	private void Update()
	{
		if (mCurPicIndex >= NumLabelPic.Length)
		{
			return;
		}
		tempNum = (int)(Time.time - mStartTime);
		if (tempNum <= mCurPicIndex)
		{
			return;
		}
		mCurPicIndex = tempNum;
		if (mCurPicIndex < NumLabelPic.Length)
		{
			NGUITools.SetActive(NumLabelPic[mCurPicIndex].gameObject, state: true);
			if (mCurPicIndex == NumLabelPic.Length - 1)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.StartGame();
				if (onCountFinish != null)
				{
					onCountFinish();
				}
			}
			SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(mCountSoundId);
		}
		else
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CarMissionStartTimeCountRoot);
		}
	}
}
