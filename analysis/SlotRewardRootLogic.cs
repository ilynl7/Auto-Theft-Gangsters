using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SlotRewardRootLogic : SingletonUnity<SlotRewardRootLogic>
{
	public ShowRewardItems ShowRewardItem;

	private bool mShowRewardFlag;

	private float mStartTime;

	private int mCurSecond;

	private float mTimeCount;

	private int mWaitCloseTime = 2;

	private DelegateDefine.NoParamDelegate onClickOK;

	public GameObject OkBtnObj;

	public UILabel TimeLabel;

	public UIPlayTween rewardAnima;

	public TweenPosition tweenPosAnima;

	public void Reset(List<item> items, DelegateDefine.NoParamDelegate okfun = null, bool showbtn = false)
	{
		mShowRewardFlag = true;
		if (showbtn)
		{
			UnityVersionUtil.SetActiveRecursive(OkBtnObj.gameObject, state: true);
			mWaitCloseTime = 10;
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(OkBtnObj.gameObject, state: false);
			tweenPosAnima.to = tweenPosAnima.transform.parent.InverseTransformPoint(SingletonUnity<SlotUIRootLogic>.Instance.totalTra.position);
			rewardAnima.resetOnPlay = true;
			rewardAnima.Play(forward: true);
			mWaitCloseTime = 2;
		}
		mStartTime = Time.time;
		mCurSecond = mWaitCloseTime;
		ShowRewardItem.ShowRewards(items);
		onClickOK = okfun;
	}

	private void Update()
	{
		if (mShowRewardFlag)
		{
			mTimeCount = Time.time - mStartTime;
			if ((int)((float)mWaitCloseTime - mTimeCount) < mCurSecond)
			{
				mCurSecond = (int)((float)mWaitCloseTime - mTimeCount);
				TimeLabel.text = StrDictionary.GetDictionaryString("{0}s", mCurSecond);
			}
			if (mTimeCount >= (float)mWaitCloseTime)
			{
				OnClickOKBtn();
			}
		}
	}

	public void OnClickOKBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SlotRewardRoot);
		if (onClickOK != null)
		{
			onClickOK();
		}
	}
}
