using UnityEngine;

public class SurveyProgressLineLogic : SingletonUnity<SurveyProgressLineLogic>
{
	public UISlider ProgressSlider;

	public UILabel TextLabel;

	private float mSurveyTime;

	private float mUsingTime;

	private float mSurveyPercent;

	private int mCurCount;

	private int mSurveyCount;

	private int mNeedSurveyCount;

	private ObjMainPlayer mainPlayer;

	public void Reset(SurveyMissionData surveyMissionData, int needNum)
	{
		mUsingTime = 0f;
		ProgressSlider.value = 0f;
		mCurCount = 0;
		mSurveyTime = surveyMissionData.SurveyTimeSecond;
		mSurveyCount = surveyMissionData.Count;
		mNeedSurveyCount = needNum;
		TextLabel.text = StrDictionary.GetDictionaryString(surveyMissionData.Text);
		mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		mainPlayer.IsTalking = true;
		if (mainPlayer.IsDrivingMount())
		{
			mainPlayer.SendServerDisMountCar();
		}
		mainPlayer.AnimationLogic.ForcePlayAnimation("caiJi", null, -1f, 0f);
	}

	private void Update()
	{
		mUsingTime += Time.deltaTime;
		if (mUsingTime < mSurveyTime)
		{
			mSurveyPercent = mUsingTime / mSurveyTime;
			ProgressSlider.value = mSurveyPercent;
			return;
		}
		mCurCount++;
		Singleton<SurveyItemManager>.Instance.FinishSurveyItem();
		if (mCurCount >= mSurveyCount || mCurCount >= mNeedSurveyCount)
		{
			StopSurveyItem();
			return;
		}
		mUsingTime = 0f;
		ProgressSlider.value = 0f;
		mainPlayer.AnimationLogic.ForcePlayAnimation("caiJi", null, -1f, 0f);
	}

	public void StopSurveyItem()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SurveyProgressLine);
		mainPlayer.IsTalking = false;
		Singleton<SurveyItemManager>.Instance.CompleteSurveyItem();
		mainPlayer.CurAnimationState = GameDefine.ANIMATIONSTATE.IDLE;
	}
}
