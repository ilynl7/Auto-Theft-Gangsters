using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuBaseTabBtnLogic : MonoBehaviour
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UISprite BtnIconSprite;

	public UILabel BtnNameLabel;

	public UISprite BtnToggle;

	public UIWidget BtnWidget;

	public UISprite TipsSp;

	public bool isTips;

	private MenuTabBtnInfo mCurBtnInfo;

	private bool mIsBtnEnable;

	private int unlockLevel;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	public void Reset(MenuTabBtnInfo btnInfo)
	{
		mCurBtnInfo = btnInfo;
		if (mCurBtnInfo.IsIconBtn)
		{
			BtnIconSprite.spriteName = mCurBtnInfo.BtnName;
			BtnNameLabel.text = mCurBtnInfo.PageName;
			UISpriteData atlasSprite = BtnIconSprite.GetAtlasSprite();
			if (atlasSprite != null)
			{
				if (mCurBtnInfo.BtnName.Equals("CZ_left_Domain"))
				{
					BtnIconSprite.width = 32;
					BtnIconSprite.height = 32;
				}
				else
				{
					BtnIconSprite.width = atlasSprite.width;
					BtnIconSprite.height = atlasSprite.height;
				}
			}
			else
			{
				Debug.Log("No Pic In Atlas :: " + mCurBtnInfo.BtnName);
			}
		}
		else
		{
			if (BtnIconSprite != null)
			{
				UnityVersionUtil.SetActiveRecursive(BtnIconSprite.gameObject, state: false);
			}
			UnityVersionUtil.SetActiveRecursive(BtnNameLabel.gameObject, state: true);
			BtnNameLabel.text = mCurBtnInfo.PageName;
		}
		if (mCurBtnInfo.FunctionType == FUNCTION_TYPE.COUNT)
		{
			mIsBtnEnable = true;
		}
		else if (GameManager.IsSupportCurDataVersion145())
		{
			mIsBtnEnable = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(mCurBtnInfo.FunctionType);
		}
		else if (mCurBtnInfo.FunctionType == FUNCTION_TYPE.GUILD_ACTIVITY)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				mIsBtnEnable = true;
			}
			else
			{
				mIsBtnEnable = false;
			}
		}
		else
		{
			mIsBtnEnable = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(mCurBtnInfo.FunctionType);
		}
		if (mIsBtnEnable)
		{
			if (BtnIconSprite != null)
			{
				BtnIconSprite.color = Color.white;
			}
			BtnNameLabel.color = Color.white;
		}
		else
		{
			if (BtnIconSprite != null)
			{
				BtnIconSprite.color = GameDefine.GrayColor;
			}
			BtnNameLabel.color = GameDefine.GrayColor;
			int functionType = (int)mCurBtnInfo.FunctionType;
			FunctionData functionDataById = DataManager.GetFunctionDataById(functionType.ToString());
			if (functionDataById != null)
			{
				unlockLevel = functionDataById.Condition;
			}
			else
			{
				unlockLevel = 1;
			}
		}
		List<string> menuTabBtnTipIdList = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.MenuTabBtnTipIdList;
		int functionType2 = (int)mCurBtnInfo.FunctionType;
		if (menuTabBtnTipIdList.Contains(functionType2.ToString()))
		{
			FunctionTipsRootLogic.AddFunctionTips(base.gameObject, new Vector3(BtnWidget.width / 2, -BtnWidget.height / 2, 0f), -1f);
		}
		UpdateTips();
	}

	public void UpdateTips()
	{
		isTips = false;
		if (TipsSp != null)
		{
			if (mCurBtnInfo.TipsFun != null)
			{
				isTips = mCurBtnInfo.TipsFun();
				TipsSp.enabled = isTips;
			}
			else
			{
				TipsSp.enabled = false;
			}
		}
	}

	public void OnClickBtn()
	{
		List<string> menuTabBtnTipIdList = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.MenuTabBtnTipIdList;
		int functionType = (int)mCurBtnInfo.FunctionType;
		if (menuTabBtnTipIdList.Contains(functionType.ToString()))
		{
			FunctionTipsRootLogic.RemoveFunctionTips(base.gameObject);
			List<string> menuTabBtnTipIdList2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.MenuTabBtnTipIdList;
			int functionType2 = (int)mCurBtnInfo.FunctionType;
			menuTabBtnTipIdList2.Remove(functionType2.ToString());
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.SetTutorialShowFinish(mCurBtnInfo.FunctionType);
			if (SingletonUnity<FunctionBtnRootLogic>.Exists)
			{
				SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateUnlockTips();
			}
		}
		if (mIsBtnEnable)
		{
			if (mCurBtnInfo.onClickBtn != null)
			{
				mCurBtnInfo.onClickBtn();
			}
		}
		else
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", unlockLevel));
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.BADGE_CLICK_TAB || TutorialManager.CurStep == TUTORIAL_STEP.WORLD_BOSS_CLICK_TAB || TutorialManager.CurStep == TUTORIAL_STEP.RANK_PVP_CLICK_TAB || TutorialManager.CurStep == TUTORIAL_STEP.STRENGTH_STAR_CLICK_TAB || TutorialManager.CurStep == TUTORIAL_STEP.GUILD_BOSS_CLICK_TAB || TutorialManager.CurStep == TUTORIAL_STEP.BAR_FIGHT_CLICK_TAB || TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_CLICK_TAB || TutorialManager.CurStep == TUTORIAL_STEP.ROBBORY_CLICK_TAB || TutorialManager.CurStep == TUTORIAL_STEP.SURVIVAL_BATTLE_CLICK_TAB || TutorialManager.CurStep == TUTORIAL_STEP.SKILL_UPGRADE_CLICK_SKILL || TutorialManager.CurStep == TUTORIAL_STEP.TITLE_CLICK_TAP || TutorialManager.CurStep == TUTORIAL_STEP.ENHANCE_CLICK_TAB || TutorialManager.CurStep == TUTORIAL_STEP.CAR_COPY_CLICK_DAILY || TutorialManager.CurStep == TUTORIAL_STEP.EXP_COPY_CLICK_DAILY || TutorialManager.CurStep == TUTORIAL_STEP.GOLD_COPY_CLICK_DAILY || TutorialManager.CurStep == TUTORIAL_STEP.TOWER_CLICK_DAILY || TutorialManager.CurStep == TUTORIAL_STEP.EQUIP_COPY_CLICK_TAB || TutorialManager.CurStep == TUTORIAL_STEP.SCUFFLE_COPY_CLICK_DAILY || TutorialManager.CurStep == TUTORIAL_STEP.CAPTURE_CLICK_TAB)
		{
			CheckTutorialEvent();
		}
	}

	private void OnEnable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(CheckBtnColor));
	}

	private void OnDisable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(CheckBtnColor));
		FunctionTipsRootLogic.RemoveFunctionTips(base.gameObject);
		ClearTutorialEvent();
	}

	private void CheckBtnColor()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(mCurBtnInfo.FunctionType))
		{
			if (BtnIconSprite != null)
			{
				BtnIconSprite.color = Color.white;
			}
			BtnNameLabel.color = Color.white;
		}
		else
		{
			if (BtnIconSprite != null)
			{
				BtnIconSprite.color = GameDefine.GrayColor;
			}
			BtnNameLabel.color = GameDefine.GrayColor;
		}
	}
}
