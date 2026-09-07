using System;
using UnityEngine;

public class TowerFloorInfoLogic : MonoBehaviour
{
	private int mCurFloorIndex;

	private int mSelfIndex;

	private int mCurCompleteFloor;

	public UISprite BtnPic;

	public UILabel FloorLabel;

	public UISprite CompletePic;

	private bool mIsEnable;

	private DelegateDefine.TwoIntParamDelegate onClickItem;

	public UISprite IconSprite;

	public GameObject RewardRoot;

	public UITweener[] RewardTweenerList;

	private bool isShowReward;

	private bool isShowRewardEnable;

	public int CurFloorIndex => mCurFloorIndex;

	public void Init(DelegateDefine.TwoIntParamDelegate func, int selfIndex)
	{
		RegisterClickEvent(func);
		mSelfIndex = selfIndex;
	}

	public void RegisterClickEvent(DelegateDefine.TwoIntParamDelegate func)
	{
		onClickItem = (DelegateDefine.TwoIntParamDelegate)Delegate.Combine(onClickItem, func);
	}

	public void DeRegisterClickEvent(DelegateDefine.TwoIntParamDelegate func)
	{
		if (onClickItem != null)
		{
			onClickItem = (DelegateDefine.TwoIntParamDelegate)Delegate.Remove(onClickItem, func);
		}
	}

	public void Refresh(int selectFloor, int completetFloor)
	{
		if (mCurFloorIndex == selectFloor)
		{
			BtnPic.spriteName = "CZ_huaDongBG_1";
		}
		else if (mCurFloorIndex > completetFloor)
		{
			BtnPic.spriteName = "CZ_huaDongBG_2";
		}
		else
		{
			BtnPic.spriteName = "CZ_huaDongBG";
		}
	}

	public void Reset(int floorIndex, bool isSelect, int completetFloor, bool isShow, bool isEnable)
	{
		mCurFloorIndex = floorIndex;
		mCurCompleteFloor = completetFloor;
		FloorLabel.text = string.Format("{0} {1}", StrDictionary.GetDictionaryString("#{101526}"), mCurFloorIndex + 1);
		isShowReward = isShow;
		isShowRewardEnable = isEnable;
		if ((mCurFloorIndex + 1) % 5 == 0 && mCurFloorIndex > 0)
		{
			IconSprite.spriteName = "CZ_paTa_PVE";
		}
		else
		{
			IconSprite.spriteName = "CZ_jingJiChang_PVP";
			isShowReward = false;
			isShowRewardEnable = false;
		}
		NGUITools.SetActive(CompletePic.gameObject, state: false);
		if (isSelect)
		{
			BtnPic.spriteName = "CZ_huaDongBG_1";
		}
		else if (floorIndex > completetFloor)
		{
			BtnPic.spriteName = "CZ_huaDongBG_2";
		}
		else
		{
			BtnPic.spriteName = "CZ_huaDongBG";
			if (isShowReward)
			{
				NGUITools.SetActive(CompletePic.gameObject, state: false);
			}
			else
			{
				NGUITools.SetActive(CompletePic.gameObject, state: true);
			}
		}
		if (isShowReward)
		{
			ShowReward();
			if (isShowRewardEnable)
			{
				EnableReward();
			}
			else
			{
				DisableReward();
			}
		}
		else
		{
			HideReward();
		}
	}

	public void OnClickBtn()
	{
		if (onClickItem != null)
		{
			onClickItem(mCurFloorIndex, mSelfIndex);
		}
	}

	public void EnableReward()
	{
		for (int i = 0; i < RewardTweenerList.Length; i++)
		{
			RewardTweenerList[i].enabled = true;
		}
	}

	public void DisableReward()
	{
		for (int i = 0; i < RewardTweenerList.Length; i++)
		{
			RewardTweenerList[i].enabled = false;
		}
	}

	public void ShowReward()
	{
		NGUITools.SetActive(RewardRoot, state: true);
		for (int i = 0; i < RewardTweenerList.Length; i++)
		{
			RewardTweenerList[i].ResetToBeginning();
		}
	}

	public void HideReward()
	{
		NGUITools.SetActive(RewardRoot, state: false);
	}

	public void OnClickReward()
	{
		if (isShowReward)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TowerWipeOutRootLogic, delegate
			{
				SingletonUnity<TowerWipeOutRootLogic>.Instance.ResetGetSpecialRewardPage(CurFloorIndex, isShowRewardEnable);
			});
		}
	}
}
