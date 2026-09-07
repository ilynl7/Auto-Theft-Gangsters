using System.Collections.Generic;
using UnityEngine;

public class TimerActivityTipsRootLogic : SingletonUnity<TimerActivityTipsRootLogic>
{
	public GameObject JumpBtnRoot;

	public UILabel JumpBtnLabel;

	public UITexture ActivityPic;

	private int mCurPageIndex;

	private List<TimerActivityTipsData> mCurActivityDataList = new List<TimerActivityTipsData>();

	private TimerActivityTipsData mCurActivityData;

	private Dictionary<string, Texture> mTextureDic = new Dictionary<string, Texture>();

	public void Reset()
	{
		mCurActivityDataList = DataManager.GetTimerActivityTipsDataList();
		InitTexture();
		ShowPage(0);
	}

	private void ShowPage(int showIndex)
	{
		mCurPageIndex = showIndex;
		mCurActivityData = mCurActivityDataList[mCurPageIndex];
		if (mCurPageIndex < mTextureDic.Count)
		{
			ActivityPic.mainTexture = mTextureDic[mCurActivityData.PicName];
			ActivityPic.SetDimensions(ActivityPic.mainTexture.width, ActivityPic.mainTexture.height);
		}
		if (mCurActivityData.JumpType == -1)
		{
			NGUITools.SetActive(JumpBtnRoot, state: false);
		}
		else
		{
			NGUITools.SetActive(JumpBtnRoot, state: true);
		}
	}

	public void InitTexture()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < mCurActivityDataList.Count; i++)
		{
			if (!string.IsNullOrEmpty(mCurActivityDataList[i].PicName))
			{
				list.Add(mCurActivityDataList[i].PicName);
			}
		}
		if (list.Count > 0)
		{
			if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(list, TextureLoadFinish));
			}
		}
		else
		{
			OnClickCloseBtn();
		}
	}

	private void TextureLoadFinish(Dictionary<string, Texture> retdic)
	{
		if (retdic.Count == 0)
		{
			OnClickCloseBtn();
			return;
		}
		mTextureDic = retdic;
		if (mCurActivityData != null)
		{
			ActivityPic.mainTexture = mTextureDic[mCurActivityData.PicName];
			ActivityPic.SetDimensions(ActivityPic.mainTexture.width, ActivityPic.mainTexture.height);
		}
		else
		{
			OnClickCloseBtn();
		}
	}

	private bool IsHaveNextPage()
	{
		if (mCurPageIndex + 1 >= mCurActivityDataList.Count)
		{
			return false;
		}
		return true;
	}

	public void OnClickCloseBtn()
	{
		if (IsHaveNextPage())
		{
			ShowPage(mCurPageIndex + 1);
			return;
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TimerActivityTipsRoot);
		if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
		{
			SingletonUnity<AutoPopUIRoot>.Instance.NextPop();
		}
	}

	public void OnClickJumpBtn()
	{
		if (mCurActivityData.JumpType == 1)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TimerActivityTipsRoot);
			if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
			{
				SingletonUnity<AutoPopUIRoot>.Instance.NextPop();
			}
			if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
			{
				SingletonUnity<AutoPopUIRoot>.Instance.NextPop(isUIJump: true);
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickToolsBtn(GameDefine.SHOP_TAB_TYPE.FASHION);
			});
		}
	}

	private void OnDisable()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < mCurActivityDataList.Count; i++)
		{
			list.Add(mCurActivityDataList[i].PicName);
		}
		BundleManager.UnloadTexture(list);
	}
}
