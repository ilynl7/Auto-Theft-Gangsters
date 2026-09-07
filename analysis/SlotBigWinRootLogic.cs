using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SlotBigWinRootLogic : SingletonUnity<SlotBigWinRootLogic>
{
	public ShowRewardItems ShowRewardItem;

	public GameObject uiObj;

	public UITexture BigWinTexture;

	public UITexture BigWinLineTexture;

	private bool mShowRewardFlag;

	private float mStartTime;

	private float mTimeCount;

	private int mWaitCloseTime = 1;

	private DelegateDefine.NoParamDelegate onClickOK;

	public void Reset(List<item> items, DelegateDefine.NoParamDelegate okfun = null)
	{
		mShowRewardFlag = true;
		mStartTime = Time.time;
		ShowRewardItem.ShowRewards(items);
		onClickOK = okfun;
		UnityVersionUtil.SetActiveRecursive(uiObj, state: false);
		InitTexture();
		vp_Timer.In(1f, delegate
		{
			setbegin();
		});
	}

	public void setbegin()
	{
		UnityVersionUtil.SetActiveRecursive(uiObj, state: true);
	}

	public void InitTexture()
	{
		List<string> list = new List<string>();
		if (BigWinTexture.mainTexture == null)
		{
			list.Add(GameDefine.SlotBigWin);
		}
		if (BigWinLineTexture.mainTexture == null)
		{
			list.Add(GameDefine.SlotBigWinBian);
		}
		if (list.Count != 0 && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(list, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(Dictionary<string, Texture> retdic)
	{
		if (retdic.Count != 0)
		{
			if (retdic.ContainsKey(GameDefine.SlotBigWin))
			{
				BigWinTexture.mainTexture = retdic[GameDefine.SlotBigWin];
			}
			if (retdic.ContainsKey(GameDefine.SlotBigWinBian))
			{
				BigWinLineTexture.mainTexture = retdic[GameDefine.SlotBigWinBian];
			}
		}
	}

	private void Update()
	{
		if (mShowRewardFlag)
		{
			mTimeCount = Time.time - mStartTime;
			if (mTimeCount >= (float)mWaitCloseTime)
			{
				mShowRewardFlag = false;
			}
		}
	}

	public void OnClickOKBtn()
	{
		if (!mShowRewardFlag)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SlotBigWinRoot);
			if (onClickOK != null)
			{
				onClickOK();
			}
		}
	}
}
