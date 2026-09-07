using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class CopyFailShowRootLogic : SingletonUnity<CopyFailShowRootLogic>
{
	public UIGrid BtnGrid;

	public GameObject BtnRetryBtn;

	public GameObject RewardRoot;

	public GameObject NoRewardRoot;

	public ShowRewardItems ShowRewardItem;

	public UILabel TimeLabel;

	public UILabel TitleLabel;

	public UILabel ContentLabel;

	private bool mShowRewardFlag;

	private float mStartTime;

	private int mCurSecond;

	private float mTimeCount;

	private int mWaitCloseTime = 15;

	private string Id;

	private bool isMission;

	private int type;

	private void OnEnable()
	{
		SingletonUnity<UIManager>.Instance.CloseOtherPlayerUI();
	}

	public void ResetNormalCopy()
	{
		isMission = false;
		mShowRewardFlag = false;
		mStartTime = Time.time;
		mCurSecond = mWaitCloseTime;
		TimeLabel.enabled = false;
		UnityVersionUtil.SetActiveRecursive(RewardRoot, state: false);
		UnityVersionUtil.SetActiveRecursive(NoRewardRoot, state: true);
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100757}");
		ContentLabel.text = StrDictionary.GetDictionaryString("#{101578}");
		UnityVersionUtil.SetActiveRecursive(BtnRetryBtn, state: false);
		BtnGrid.Reposition();
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer != null)
		{
			mainPlayer.StopAutoAndSkill();
		}
		LocalDataSaveManager.SetDiedFlag(1);
	}

	public void ResetTowerCopy(string id)
	{
		isMission = false;
		Id = id;
		mShowRewardFlag = true;
		mStartTime = Time.time;
		mCurSecond = mWaitCloseTime;
		TimeLabel.enabled = true;
		UnityVersionUtil.SetActiveRecursive(RewardRoot, state: false);
		UnityVersionUtil.SetActiveRecursive(NoRewardRoot, state: true);
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100757}");
		ContentLabel.text = StrDictionary.GetDictionaryString("#{101578}");
		UnityVersionUtil.SetActiveRecursive(BtnRetryBtn, state: true);
		BtnGrid.Reposition();
		type = 0;
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer != null)
		{
			mainPlayer.StopAutoAndSkill();
		}
		LocalDataSaveManager.SetDiedFlag(1);
	}

	public void ResetMission(List<item> items)
	{
		isMission = true;
		mShowRewardFlag = true;
		mStartTime = Time.time;
		mCurSecond = mWaitCloseTime;
		TimeLabel.enabled = true;
		UnityVersionUtil.SetActiveRecursive(RewardRoot, state: true);
		UnityVersionUtil.SetActiveRecursive(NoRewardRoot, state: false);
		ShowRewardItem.ShowRewards(items);
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100312}");
		ContentLabel.text = StrDictionary.GetDictionaryString("#{101578}");
		UnityVersionUtil.SetActiveRecursive(BtnRetryBtn, state: false);
		BtnGrid.Reposition();
	}

	public void ResetRankPvp(List<item> items)
	{
		isMission = false;
		mShowRewardFlag = false;
		mStartTime = Time.time;
		mCurSecond = mWaitCloseTime;
		TimeLabel.enabled = false;
		UnityVersionUtil.SetActiveRecursive(RewardRoot, state: true);
		UnityVersionUtil.SetActiveRecursive(NoRewardRoot, state: false);
		ShowRewardItem.ShowRewards(items);
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100312}");
		ContentLabel.text = StrDictionary.GetDictionaryString("#{101578}");
		UnityVersionUtil.SetActiveRecursive(BtnRetryBtn, state: false);
		BtnGrid.Reposition();
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer != null)
		{
			mainPlayer.StopAutoAndSkill();
		}
		LocalDataSaveManager.SetDiedFlag(1);
	}

	public void OnRetry()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CopyFailShowRoot);
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager is TowerSceneManager towerSceneManager)
		{
			towerSceneManager.Retry();
		}
	}

	public void OnClickLeave()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CopyFailShowRoot);
		if (!isMission)
		{
			NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
		}
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
				OnClickLeave();
				TimeLabel.enabled = false;
			}
		}
	}
}
