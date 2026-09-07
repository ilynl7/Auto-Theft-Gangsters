using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class OpenBoxRootLogic : SingletonUnity<OpenBoxRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public GameObject BackRoot;

	public GameObject TipRoot;

	public GameObject OpenBoxEffect;

	public ShowRewardItems ShowRewardItem;

	private float startTime;

	private bool OpenFlag;

	private List<item> mCurItems;

	public UIWidget OkBtnRoot;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (SingletonUnity<TutorialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.CloseCheck();
		}
		if (mOnClickTutorialBtn != null)
		{
			TutorialManager.OnClickTutorialBtn onClickTutorialBtn = mOnClickTutorialBtn;
			mOnClickTutorialBtn = null;
			onClickTutorialBtn();
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	public void Reset()
	{
		NGUITools.SetActive(BackRoot, state: false);
		NGUITools.SetActive(TipRoot, state: false);
		OpenFlag = false;
		startTime = Time.time;
	}

	public void ShowTipPage(ret_open_item_package.request request)
	{
		OpenFlag = true;
		mCurItems = request.items;
	}

	public void ShowTipPage(ret_commercail_reward.request request)
	{
		OpenFlag = true;
		mCurItems = request.items;
	}

	private void Update()
	{
		if (OpenFlag && Time.time - startTime > 2f && !UnityVersionUtil.IsActive(BackRoot.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(OpenBoxEffect.gameObject, state: false);
			NGUITools.SetActive(BackRoot, state: true);
			NGUITools.SetActive(TipRoot, state: true);
			ShowRewardItem.ShowRewards(mCurItems);
		}
	}

	public void OnClickOkBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.OpenBoxRoot);
		if (TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_CLICK_CONFIRM)
		{
			CheckTutorialEvent();
		}
	}

	public void OnTweenScaleFinish()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_WAIT_OPEN_BOX)
		{
			CheckTutorialEvent();
		}
	}
}
