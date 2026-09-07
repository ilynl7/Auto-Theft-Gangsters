using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ActivityItemLogic : MonoBehaviour
{
	public delegate void ActivityItemDelegate(int a, int b, int c, bool d);

	public UILabel NameLabel;

	public UISprite IconSprite;

	public UISprite IconLockFlag;

	public UILabel LimitLabel;

	public UILabel DurationLabel;

	public UISprite SelectBkSprite;

	private int enableLineFlag;

	private int UnlockLevel;

	public int curIndex = -1;

	public ActivityItemDelegate onClickItem;

	public activity_info curActivityInfo;

	public UIWidget PVPSP;

	public UIWidget PVESP;

	public UIWidget AloneSP;

	public UIWidget TeamSp;

	public UIWidget GuildSp;

	public UISprite DailyActFlag;

	public UILabel DailyActValLabel;

	private bool isWarningflag;

	public GameObject HighEffectObj;

	private bool isLevelUnlock;

	public bool CheckLevel(int minLevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minLevel);
	}

	public void UpdateItem(int index, activity_info info, int curChoose)
	{
		enableLineFlag = 0;
		curIndex = index;
		curActivityInfo = info;
		int num = (int)curActivityInfo.CurNum;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		isWarningflag = false;
		isLevelUnlock = false;
		if (curActivityInfo.Type == 1 || curActivityInfo.Type == 2)
		{
			EscortData escortDataById = DataManager.GetEscortDataById(info.ID);
			IconSprite.spriteName = escortDataById.Icon;
			TimeSpan localShowTime = TimeTools.GetLocalShowTime(escortDataById.StartTime, playerCommonData.TimeOffset);
			TimeSpan localShowTime2 = TimeTools.GetLocalShowTime(escortDataById.StartTime + escortDataById.DurationTime, playerCommonData.TimeOffset);
			DurationLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101558}"), $"{localShowTime.Hours:D2}:{localShowTime.Minutes:D2}", $"{localShowTime2.Hours:D2}:{localShowTime2.Minutes:D2}");
			if (curActivityInfo.State == 0L && playerCommonData.GetCurServerTime() < curActivityInfo.Parm)
			{
				SetLabelWarining(DurationLabel, needWarning: true, isLowWarningLimit: true);
				enableLineFlag = 2;
			}
			else
			{
				SetLabelWarining(DurationLabel, needWarning: false, isLowWarningLimit: true);
			}
			LimitLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{101508}"), num, escortDataById.MaxPlayNum);
			if (num <= 0)
			{
				isWarningflag = true;
				enableLineFlag = 3;
			}
			NameLabel.text = StrDictionary.GetDictionaryString(escortDataById.Name);
			UnlockLevel = escortDataById.UnlockLevel;
			if (!CheckLevel(escortDataById.UnlockLevel))
			{
				LimitLabel.text = $"Lv {UnlockLevel}";
				enableLineFlag = 1;
				isWarningflag = true;
				IconSprite.color = Color.gray;
				IconLockFlag.enabled = true;
			}
			else
			{
				IconSprite.color = Color.white;
				IconLockFlag.enabled = false;
				isLevelUnlock = true;
			}
		}
		else if (curActivityInfo.Type == 3)
		{
			CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(info.ID);
			NameLabel.text = StrDictionary.GetDictionaryString(cityDanceDataById.Name);
			TimeSpan localShowTime3 = TimeTools.GetLocalShowTime((int)cityDanceDataById.StartTime[0], playerCommonData.TimeOffset);
			TimeSpan localShowTime4 = TimeTools.GetLocalShowTime(cityDanceDataById.StartTime[0] + cityDanceDataById.DurationTime, playerCommonData.TimeOffset);
			DurationLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101509}"), $"{localShowTime3.Hours:D2}:{localShowTime3.Minutes:D2}", $"{localShowTime4.Hours:D2}:{localShowTime4.Minutes:D2}");
			if (curActivityInfo.State == 0L && playerCommonData.GetCurServerTime() < curActivityInfo.Parm)
			{
				SetLabelWarining(DurationLabel, needWarning: true, isLowWarningLimit: true);
				enableLineFlag = 2;
			}
			else
			{
				SetLabelWarining(DurationLabel, needWarning: false, isLowWarningLimit: true);
			}
			IconSprite.spriteName = cityDanceDataById.Icon;
			UnlockLevel = cityDanceDataById.UnlockLevel;
			LimitLabel.text = $"Lv {UnlockLevel}";
			if (!CheckLevel(cityDanceDataById.UnlockLevel))
			{
				isWarningflag = true;
				enableLineFlag = 1;
				IconSprite.color = Color.gray;
				IconLockFlag.enabled = true;
			}
			else
			{
				IconSprite.color = Color.white;
				IconLockFlag.enabled = false;
				isLevelUnlock = true;
			}
		}
		else if (curActivityInfo.Type == 4)
		{
			BarFightCopyData barFightCopyDataByID = DataManager.GetBarFightCopyDataByID(info.ID);
			NameLabel.text = StrDictionary.GetDictionaryString(barFightCopyDataByID.Name);
			TimeSpan localShowTime5 = TimeTools.GetLocalShowTime(barFightCopyDataByID.StartTime + barFightCopyDataByID.DurationTime, playerCommonData.TimeOffset);
			TimeSpan localShowTime6 = TimeTools.GetLocalShowTime(barFightCopyDataByID.StartTime + barFightCopyDataByID.DurationTime + barFightCopyDataByID.WaitTime, playerCommonData.TimeOffset);
			DurationLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101509}"), $"{localShowTime5.Hours:D2}:{localShowTime5.Minutes:D2}", $"{localShowTime6.Hours:D2}:{localShowTime6.Minutes:D2}");
			TimeSpan localShowTime7 = TimeTools.GetLocalShowTime(barFightCopyDataByID.StartTime, playerCommonData.TimeOffset);
			TimeSpan localShowTime8 = TimeTools.GetLocalShowTime(barFightCopyDataByID.StartTime + barFightCopyDataByID.DurationTime, playerCommonData.TimeOffset);
			if (curActivityInfo.State == 0L && playerCommonData.GetCurServerTime() < curActivityInfo.Parm)
			{
				SetLabelWarining(DurationLabel, needWarning: true, isLowWarningLimit: true);
				enableLineFlag = 2;
			}
			else if (curActivityInfo.State == 1)
			{
				SetLabelWarining(DurationLabel, needWarning: true, isLowWarningLimit: true);
				enableLineFlag = 5;
			}
			else if (curActivityInfo.State == 2)
			{
				SetLabelWarining(DurationLabel, needWarning: false, isLowWarningLimit: true);
			}
			IconSprite.spriteName = barFightCopyDataByID.Icon;
			UnlockLevel = barFightCopyDataByID.UnlockLevel;
			LimitLabel.text = $"Lv {UnlockLevel}";
			if (!CheckLevel(barFightCopyDataByID.UnlockLevel))
			{
				isWarningflag = true;
				enableLineFlag = 1;
				IconSprite.color = Color.gray;
				IconLockFlag.enabled = true;
			}
			else
			{
				IconSprite.color = Color.white;
				IconLockFlag.enabled = false;
				isLevelUnlock = true;
			}
		}
		else if (curActivityInfo.Type == 7)
		{
			SurviveBattleData surviveBattleDataById = DataManager.GetSurviveBattleDataById(info.ID);
			NameLabel.text = StrDictionary.GetDictionaryString(surviveBattleDataById.Name);
			List<TimeSpan> localShowTime9 = TimeTools.GetLocalShowTime(surviveBattleDataById.StartTimes, playerCommonData.TimeOffset, surviveBattleDataById.DurationTime, (int)info.next);
			if (localShowTime9 != null && localShowTime9.Count == 2)
			{
				TimeSpan timeSpan = localShowTime9[0];
				TimeSpan timeSpan2 = localShowTime9[1];
				DurationLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101509}"), $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}", $"{timeSpan2.Hours:D2}:{timeSpan2.Minutes:D2}");
			}
			else
			{
				DurationLabel.text = string.Empty;
			}
			if (curActivityInfo.State == 0L && playerCommonData.GetCurServerTime() < curActivityInfo.Parm)
			{
				SetLabelWarining(DurationLabel, needWarning: true, isLowWarningLimit: true);
				enableLineFlag = 2;
			}
			else
			{
				SetLabelWarining(DurationLabel, needWarning: false, isLowWarningLimit: true);
			}
			IconSprite.spriteName = surviveBattleDataById.Icon;
			UnlockLevel = surviveBattleDataById.UnlockLevel;
			LimitLabel.text = $"Lv {UnlockLevel}";
			if (!CheckLevel(surviveBattleDataById.UnlockLevel))
			{
				isWarningflag = true;
				enableLineFlag = 1;
				IconSprite.color = Color.gray;
				IconLockFlag.enabled = true;
			}
			else
			{
				IconSprite.color = Color.white;
				IconLockFlag.enabled = false;
				isLevelUnlock = true;
			}
		}
		else if (curActivityInfo.Type == 8)
		{
			SexMiniData sexMiniDataById = DataManager.GetSexMiniDataById(info.ID);
			NameLabel.text = StrDictionary.GetDictionaryString(sexMiniDataById.Name);
			DurationLabel.text = StrDictionary.GetDictionaryString("#{100830}");
			SetLabelWarining(DurationLabel, needWarning: false, isLowWarningLimit: true);
			IconSprite.spriteName = sexMiniDataById.Icon;
			UnlockLevel = sexMiniDataById.UnlockLevel;
			LimitLabel.text = $"Lv {UnlockLevel}";
			if (!CheckLevel(sexMiniDataById.UnlockLevel))
			{
				isWarningflag = true;
				enableLineFlag = 1;
				IconSprite.color = Color.gray;
				IconLockFlag.enabled = true;
			}
			else
			{
				IconSprite.color = Color.white;
				IconLockFlag.enabled = false;
				isLevelUnlock = true;
			}
		}
		ShowStateFlag((GameDefine.ACTIVITY_TYPE)curActivityInfo.Type);
		ShowFirstUnlockState(isLevelUnlock);
		if (isWarningflag)
		{
			SetLabelWarining(LimitLabel, needWarning: true);
		}
		else
		{
			SetLabelWarining(LimitLabel, needWarning: false);
		}
		RefreshSelect(curChoose);
	}

	private void ShowFirstUnlockState(bool isunlock)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (isunlock)
		{
			GameDefine.ACTIVITY_TYPE type = (GameDefine.ACTIVITY_TYPE)curActivityInfo.Type;
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
			GameDefine.ACTIVITY_TYPE type = (GameDefine.ACTIVITY_TYPE)curActivityInfo.Type;
			playerCommonData.SetFirstClickState(GameDefine.GetActKey_FirstClick(type));
			NGUITools.SetActive(HighEffectObj, state: false);
		}
	}

	private void ShowStateFlag(GameDefine.ACTIVITY_TYPE acttype)
	{
		if (acttype == GameDefine.ACTIVITY_TYPE.ESCORT || acttype == GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT || acttype == GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE)
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
		case GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE:
			ShowScore(18);
			break;
		case GameDefine.ACTIVITY_TYPE.CITY_DANCE:
		case GameDefine.ACTIVITY_TYPE.BAR_FIGHT:
		case GameDefine.ACTIVITY_TYPE.GUILD_BOSS:
			break;
		}
	}

	public void OnClikcItemBtn()
	{
		if (isLevelUnlock)
		{
			CheckFirstClickState();
		}
		if (onClickItem != null)
		{
			onClickItem(curIndex, enableLineFlag, UnlockLevel, d: false);
		}
	}

	public void MapClickItemBtn()
	{
		if (isLevelUnlock)
		{
			CheckFirstClickState();
		}
		if (onClickItem != null)
		{
			onClickItem(curIndex, enableLineFlag, UnlockLevel, d: true);
		}
	}

	public void RefreshSelect(int index)
	{
	}

	private void SetLabelWarining(UILabel label, bool needWarning, bool isLowWarningLimit = false)
	{
		if (needWarning)
		{
			if (isLowWarningLimit)
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
}
