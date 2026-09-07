using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class MissionPassShowRootLogic : SingletonUnity<MissionPassShowRootLogic>
{
	public GameObject RewardRoot;

	public ShowRewardItems ShowRewardItem;

	public GameObject BottomCollider;

	public UILabel TimeLabel;

	public UILabel TitleLabel;

	public GameObject SuccessRoot;

	public GameObject FailRoot;

	private bool mShowRewardFlag;

	private float mStartTime;

	private int mCurSecond;

	private float mTimeCount;

	public GameObject SucessLineObj;

	public GameObject FailLineObj;

	public GameObject StarRootObj;

	public GameObject[] StarObjList;

	private int mWaitCloseTime = 15;

	private DelegateDefine.NoParamDelegate onClickOK;

	private void OnEnable()
	{
		SingletonUnity<UIManager>.Instance.CloseOtherPlayerUI();
	}

	public void Reset(string missionId)
	{
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID.Class != 0)
		{
			mShowRewardFlag = false;
			UnityVersionUtil.SetActiveRecursive(RewardRoot.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(BottomCollider.gameObject, state: false);
			vp_Timer.In(4f, delegate
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MissionPassShowRoot);
			});
		}
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100301}");
		onClickOK = null;
		NGUITools.SetActive(SuccessRoot, state: true);
		NGUITools.SetActive(FailRoot, state: false);
		NGUITools.SetActive(StarRootObj, state: false);
	}

	public void ResetDailyMissionReward(List<item> items, DelegateDefine.NoParamDelegate onFinish = null)
	{
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100301}");
		mShowRewardFlag = true;
		mStartTime = Time.time;
		mCurSecond = mWaitCloseTime;
		ShowRewardItem.ShowRewards(items);
		onClickOK = onFinish;
		NGUITools.SetActive(SuccessRoot, state: true);
		NGUITools.SetActive(FailRoot, state: false);
		NGUITools.SetActive(FailLineObj, state: false);
		NGUITools.SetActive(StarRootObj, state: false);
	}

	public void ResetSideMissionReward(string showDropId)
	{
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100301}");
		mShowRewardFlag = true;
		mStartTime = Time.time;
		mCurSecond = mWaitCloseTime;
		ShowRewardData showRewardDataByID = DataManager.GetShowRewardDataByID(showDropId);
		if (showRewardDataByID == null)
		{
			NGUITools.SetActive(ShowRewardItem.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(ShowRewardItem.gameObject, state: true);
			ShowRewardItem.ShowRewards(showRewardDataByID.ItemIdList, showRewardDataByID.QualityList, showRewardDataByID.CountList);
		}
		NGUITools.SetActive(SuccessRoot, state: true);
		NGUITools.SetActive(FailRoot, state: false);
		NGUITools.SetActive(FailLineObj, state: false);
		NGUITools.SetActive(StarRootObj, state: false);
	}

	public void ResetDailyFinishReward(List<item> items, DelegateDefine.NoParamDelegate onFinish = null)
	{
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100311}");
		mShowRewardFlag = true;
		mStartTime = Time.time;
		mCurSecond = mWaitCloseTime;
		ShowRewardItem.ShowRewards(items);
		onClickOK = onFinish;
		NGUITools.SetActive(SuccessRoot, state: true);
		NGUITools.SetActive(FailRoot, state: false);
		NGUITools.SetActive(FailLineObj, state: false);
		NGUITools.SetActive(StarRootObj, state: false);
	}

	public void ResetNormalMissionReward(bool isSuccess, List<item> items, DelegateDefine.NoParamDelegate onFinish = null)
	{
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100301}");
		if (isSuccess)
		{
			NGUITools.SetActive(SuccessRoot, state: true);
			NGUITools.SetActive(FailRoot, state: false);
			NGUITools.SetActive(FailLineObj, state: false);
		}
		else
		{
			NGUITools.SetActive(SuccessRoot, state: false);
			NGUITools.SetActive(FailRoot, state: true);
			NGUITools.SetActive(SucessLineObj, state: false);
		}
		mShowRewardFlag = true;
		mStartTime = Time.time;
		mCurSecond = mWaitCloseTime;
		ShowRewardItem.ShowRewards(items);
		onClickOK = onFinish;
		NGUITools.SetActive(StarRootObj, state: false);
	}

	public void ResetEquipSwipe(List<item> items)
	{
		TitleLabel.text = StrDictionary.GetDictionaryString("#{101522}");
		NGUITools.SetActive(SuccessRoot, state: true);
		NGUITools.SetActive(FailRoot, state: false);
		NGUITools.SetActive(FailLineObj, state: false);
		mShowRewardFlag = true;
		mStartTime = Time.time;
		mCurSecond = mWaitCloseTime;
		ShowRewardItem.ShowRewards(items);
		NGUITools.SetActive(StarRootObj, state: false);
	}

	public void ResetTimeLimitMissionPassRoot(TimeLimitMissionData tlData, long restTime)
	{
		ShowRewardData showRewardData = null;
		if (restTime > tlData.Reward3Time)
		{
			NGUITools.SetActive(SuccessRoot, state: true);
			NGUITools.SetActive(FailRoot, state: false);
			if (!string.IsNullOrEmpty(tlData.ShowReward3))
			{
				showRewardData = DataManager.GetShowRewardDataByID(tlData.ShowReward3);
			}
		}
		else if (restTime > tlData.Reward2Time)
		{
			NGUITools.SetActive(StarObjList[2], state: false);
			NGUITools.SetActive(SuccessRoot, state: true);
			NGUITools.SetActive(FailRoot, state: false);
			if (!string.IsNullOrEmpty(tlData.ShowReward2))
			{
				showRewardData = DataManager.GetShowRewardDataByID(tlData.ShowReward2);
			}
		}
		else if (restTime > tlData.Reward1Time)
		{
			NGUITools.SetActive(StarObjList[2], state: false);
			NGUITools.SetActive(StarObjList[1], state: false);
			NGUITools.SetActive(SuccessRoot, state: true);
			NGUITools.SetActive(FailRoot, state: false);
			if (!string.IsNullOrEmpty(tlData.ShowReward1))
			{
				showRewardData = DataManager.GetShowRewardDataByID(tlData.ShowReward1);
			}
		}
		else
		{
			NGUITools.SetActive(StarRootObj, state: false);
			NGUITools.SetActive(SuccessRoot, state: false);
			NGUITools.SetActive(FailRoot, state: true);
		}
		if (showRewardData != null)
		{
			ShowRewardItem.ShowRewards(showRewardData.ItemIdList, showRewardData.QualityList, showRewardData.CountList);
		}
		mShowRewardFlag = true;
		mStartTime = Time.time;
		mCurSecond = mWaitCloseTime;
		TimeLabel.text = StrDictionary.GetDictionaryString("#{100303}", mCurSecond);
	}

	private void Update()
	{
		if (mShowRewardFlag)
		{
			mTimeCount = Time.time - mStartTime;
			if ((int)((float)mWaitCloseTime - mTimeCount) < mCurSecond)
			{
				mCurSecond = (int)((float)mWaitCloseTime - mTimeCount);
				TimeLabel.text = StrDictionary.GetDictionaryString("#{100303}", mCurSecond);
			}
			if (mTimeCount >= (float)mWaitCloseTime)
			{
				OnClickOKBtn();
			}
		}
	}

	public void OnClickOKBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MissionPassShowRoot);
		if (onClickOK != null)
		{
			onClickOK();
		}
	}

	public bool IsShowReward()
	{
		return mShowRewardFlag;
	}
}
