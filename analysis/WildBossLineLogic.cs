using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class WildBossLineLogic : MonoBehaviour
{
	public UILabel showName;

	public UILabel DurationLabel;

	public UILabel LimitLabel;

	private int idxInDic;

	public UISprite IconSprite;

	public UISprite IconLockFlag;

	public UISprite SelectBkSprite;

	public GameObject Sublineobj;

	private List<activity_info> curActivityList = new List<activity_info>();

	public TweenScale SubLineRootScale;

	public TweenRotation SubFlagAnima;

	private bool mHasSubLine;

	public UIMyCenterOnChild CenterOnChild;

	public UIWidget PVPSP;

	public UIWidget PVESP;

	public UIWidget AloneSP;

	public UIWidget TeamSp;

	public UIWidget GuildSp;

	public UISprite DailyActFlag;

	public UILabel DailyActValLabel;

	private bool isWarningflag;

	private int curBossIndex;

	public DelegateDefine.TwoIntParamDelegate onClickItem;

	public int enableLineFlag;

	public GameObject HighEffectObj;

	private bool isLevelUnlock;

	public List<activity_info> CurActivityList => curActivityList;

	public void ResetItem(List<activity_info> infoList, DelegateDefine.TwoIntParamDelegate clickFunc)
	{
		UnityVersionUtil.SetActiveRecursive(Sublineobj, state: false);
		curActivityList = infoList;
		onClickItem = clickFunc;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		isWarningflag = false;
		isLevelUnlock = false;
		if (curActivityList.Count > 0)
		{
			curBossIndex = 0;
			for (int i = 0; i < curActivityList.Count; i++)
			{
				WildBossData wildBossDataByID = DataManager.GetWildBossDataByID(curActivityList[i].ID);
				if (CheckLevel(wildBossDataByID.LevelMin, wildBossDataByID.LevelMax))
				{
					curBossIndex = i;
					break;
				}
			}
			enableLineFlag = 0;
			activity_info activity_info = infoList[curBossIndex];
			WildBossData wildBossDataByID2 = DataManager.GetWildBossDataByID(infoList[curBossIndex].ID);
			int num = (int)activity_info.CurNum;
			if (activity_info.State == 2)
			{
				enableLineFlag = 2;
			}
			if (activity_info.State == 0L && playerCommonData.GetCurServerTime() < activity_info.Parm)
			{
				enableLineFlag = 2;
			}
			if (num <= 0 && playerCommonData.GetCurServerTime() > activity_info.time + 5400)
			{
				enableLineFlag = 3;
			}
			if (!CheckLevel(wildBossDataByID2.LevelMin, wildBossDataByID2.LevelMax))
			{
				enableLineFlag = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckIsLevelHeigh(wildBossDataByID2.LevelMin, wildBossDataByID2.LevelMax);
			}
			List<TimeSpan> localShowTime = TimeTools.GetLocalShowTime(wildBossDataByID2.StartTimes, playerCommonData.TimeOffset, wildBossDataByID2.DurationTime, (int)infoList[curBossIndex].next, infoList[curBossIndex].State == 2);
			if (localShowTime != null && localShowTime.Count == 2)
			{
				TimeSpan timeSpan = localShowTime[0];
				TimeSpan timeSpan2 = localShowTime[1];
				DurationLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101509}"), $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}", $"{timeSpan2.Hours:D2}:{timeSpan2.Minutes:D2}");
			}
			else
			{
				DurationLabel.text = string.Empty;
			}
			if (infoList[curBossIndex].State != 1)
			{
				SetLabelWarining(DurationLabel, needWarning: true, isLowWarning: true);
			}
			else
			{
				SetLabelWarining(DurationLabel, needWarning: false, isLowWarning: true);
			}
			LimitLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{101508}"), num, wildBossDataByID2.Limit);
			if (num <= 0 && playerCommonData.GetCurServerTime() > infoList[curBossIndex].time + 5400)
			{
				isWarningflag = true;
			}
			WildBossData wildBossDataByID3 = DataManager.GetWildBossDataByID(curActivityList[0].ID);
			int levelMin = wildBossDataByID3.LevelMin;
			if (!CheckLevel(levelMin))
			{
				isWarningflag = true;
				LimitLabel.text = $"Lv {levelMin}";
				IconSprite.color = Color.gray;
				IconLockFlag.enabled = true;
			}
			else
			{
				IconSprite.color = Color.white;
				IconLockFlag.enabled = false;
				isLevelUnlock = true;
			}
			IconSprite.spriteName = wildBossDataByID2.Icon;
			long type = infoList[curBossIndex].Type;
			if (type == 5)
			{
				showName.text = StrDictionary.GetDictionaryString("#{101519}");
				if (wildBossDataByID2.PVP == 1)
				{
					ShowStateFlag(GameDefine.ACTIVITY_TYPE.WILD_BOSS, ispvp: true);
				}
				else
				{
					ShowStateFlag(GameDefine.ACTIVITY_TYPE.WILD_BOSS);
				}
			}
		}
		if (isWarningflag)
		{
			SetLabelWarining(LimitLabel, needWarning: true);
		}
		else
		{
			SetLabelWarining(LimitLabel, needWarning: false);
		}
		ShowFirstUnlockState(isLevelUnlock);
	}

	private void ShowFirstUnlockState(bool isunlock)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (isunlock)
		{
			GameDefine.ACTIVITY_TYPE type = GameDefine.ACTIVITY_TYPE.WILD_BOSS;
			if (playerCommonData.CheckFirstClickState(GameDefine.GetActKey_FirstClick(type)))
			{
				NGUITools.SetActive(HighEffectObj, state: true);
			}
			else
			{
				NGUITools.SetActive(HighEffectObj, state: false);
			}
		}
		else
		{
			NGUITools.SetActive(HighEffectObj, state: false);
		}
	}

	private void CheckFirstClickState()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (UnityVersionUtil.IsActive(HighEffectObj))
		{
			GameDefine.ACTIVITY_TYPE type = GameDefine.ACTIVITY_TYPE.WILD_BOSS;
			playerCommonData.SetFirstClickState(GameDefine.GetActKey_FirstClick(type));
			NGUITools.SetActive(HighEffectObj, state: false);
		}
	}

	private void ShowStateFlag(GameDefine.ACTIVITY_TYPE acttype, bool ispvp = false)
	{
		if (acttype == GameDefine.ACTIVITY_TYPE.ESCORT || acttype == GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT || acttype == GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE || ispvp)
		{
			PVPSP.alpha = 1f;
			PVESP.alpha = 0f;
		}
		else
		{
			PVPSP.alpha = 0f;
			PVESP.alpha = 1f;
		}
		GuildSp.alpha = 0f;
		if (acttype == GameDefine.ACTIVITY_TYPE.CITY_DANCE || acttype == GameDefine.ACTIVITY_TYPE.ESCORT || acttype == GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT || acttype == GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE || acttype == GameDefine.ACTIVITY_TYPE.WILD_BOSS)
		{
			TeamSp.alpha = 1f;
			AloneSP.alpha = 0f;
		}
		else
		{
			TeamSp.alpha = 0f;
			AloneSP.alpha = 1f;
		}
		DailyActFlag.alpha = 0f;
		switch (acttype)
		{
		case GameDefine.ACTIVITY_TYPE.WILD_BOSS:
			ShowScore(6);
			break;
		case GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT:
			ShowScore(4);
			break;
		case GameDefine.ACTIVITY_TYPE.ESCORT:
			ShowScore(14);
			break;
		case GameDefine.ACTIVITY_TYPE.CITY_DANCE:
		case GameDefine.ACTIVITY_TYPE.BAR_FIGHT:
			break;
		}
	}

	private void ShowScore(int type)
	{
		DailyActFlag.alpha = 0f;
		List<DailyActiveData> dailyActiveDataList = DataManager.GetDailyActiveDataList();
		for (int i = 0; i < dailyActiveDataList.Count; i++)
		{
			if (dailyActiveDataList[i].Type == type)
			{
				int score = dailyActiveDataList[i].Score;
				DailyActFlag.alpha = 1f;
				DailyActValLabel.text = $"{score}";
				break;
			}
		}
	}

	public bool IsShowNextboss()
	{
		for (int i = 0; i < curActivityList.Count; i++)
		{
			WildBossData wildBossDataByID = DataManager.GetWildBossDataByID(curActivityList[i].ID);
			if (CheckLevel(wildBossDataByID.LevelMin, wildBossDataByID.LevelMax) && curActivityList[i].State == 2)
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckLevel(int minLevel, int maxlevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minLevel, maxlevel);
	}

	public bool CheckLevel(int minLevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minLevel);
	}

	public void OpenFinish()
	{
	}

	public void OnClickItemBtn()
	{
		if (isLevelUnlock)
		{
			CheckFirstClickState();
		}
		OnClickSelectItemBtn(isSelect: false);
	}

	public void ClickTargetBtn()
	{
		int num = 0;
		for (int i = 0; i < curActivityList.Count; i++)
		{
			WildBossData wildBossDataByID = DataManager.GetWildBossDataByID(curActivityList[i].ID);
			if (CheckLevel(wildBossDataByID.LevelMin, wildBossDataByID.LevelMax))
			{
				num = i;
				break;
			}
		}
		OnClickItemBtn();
	}

	public void OnClickSelectItemBtn(bool isSelect)
	{
		if (onClickItem != null)
		{
			onClickItem(curBossIndex, enableLineFlag);
		}
	}

	private void SetLabelWarining(UILabel label, bool needWarning, bool isLowWarning = false)
	{
		if (needWarning)
		{
			if (isLowWarning)
			{
				label.color = new Color(0.537f, 0.537f, 0.537f, 1f);
			}
			else
			{
				label.color = Color.red;
			}
		}
		else
		{
			label.color = Color.white;
		}
	}

	public void RefreshSelect(int curchoose)
	{
	}

	public void OnScaleFinished()
	{
	}
}
