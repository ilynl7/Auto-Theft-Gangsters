using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildBossLineLogic : MonoBehaviour
{
	public UILabel showName;

	public UILabel DurationLabel;

	public UILabel LimitLabel;

	private int mCurIndex;

	private int mCurSubIndex;

	public UISprite selectbtnbg;

	public UISprite IconSprite;

	public UISprite IconLockFlag;

	private GuildBossData curBossData;

	private List<guild_boss> curGuildBossList;

	private GuildBattleData curGuildBattleData;

	private CityDanceData CurGuildDanceData;

	private bool mHasSubLine;

	private bool isClose;

	private DelegateDefine.ThirdIntParamDelegate OnClickLine;

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

	private List<guild_map_info> GuildCityList;

	private GameDefine.ACTIVITY_TYPE curActType = GameDefine.ACTIVITY_TYPE.INVALID;

	public bool IsGuildBossLine()
	{
		if (curGuildBossList != null && curGuildBossList.Count > 0)
		{
			return true;
		}
		return false;
	}

	public void ResetItem(List<guild_boss> infoList, DelegateDefine.ThirdIntParamDelegate clickFunc, int index)
	{
		mCurIndex = index;
		curGuildBossList = infoList;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		OnClickLine = clickFunc;
		isLevelUnlock = false;
		curActType = GameDefine.ACTIVITY_TYPE.GUILD_BOSS;
		if (curGuildBossList.Count > 0)
		{
			bool flag = false;
			mCurSubIndex = 0;
			for (int i = 0; i < curGuildBossList.Count; i++)
			{
				if (curGuildBossList[i].state == 1)
				{
					flag = true;
					mCurSubIndex = i;
					break;
				}
			}
			curBossData = DataManager.GetGuildBossDataByID(infoList[mCurSubIndex].id);
			TimeSpan localShowTime = TimeTools.GetLocalShowTime(curBossData.StartTime, playerCommonData.TimeOffset);
			TimeSpan localShowTime2 = TimeTools.GetLocalShowTime(curBossData.StartTime + curBossData.DurationTime, playerCommonData.TimeOffset);
			DurationLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101509}"), $"{localShowTime.Hours:D2}:{localShowTime.Minutes:D2}", $"{localShowTime2.Hours:D2}:{localShowTime2.Minutes:D2}");
			if (!flag)
			{
				SetLabelWarining(DurationLabel, needWarning: true, isLowWarning: true);
			}
			else
			{
				SetLabelWarining(DurationLabel, needWarning: false, isLowWarning: true);
			}
			isWarningflag = false;
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				LimitLabel.text = string.Format("{0} {1}", StrDictionary.GetDictionaryString("#{100735}"), curBossData.LevelMin);
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.GuilLevel >= curBossData.LevelMin)
				{
					NGUITools.SetActive(LimitLabel.gameObject, state: false);
					SetLabelWarining(LimitLabel, needWarning: false);
					IconSprite.color = Color.white;
					IconLockFlag.enabled = false;
					isLevelUnlock = true;
				}
				else
				{
					NGUITools.SetActive(LimitLabel.gameObject, state: true);
					SetLabelWarining(LimitLabel, needWarning: true);
					IconSprite.color = Color.gray;
					IconLockFlag.enabled = true;
				}
			}
			else
			{
				LimitLabel.text = StrDictionary.GetDictionaryString("#{102087}");
				NGUITools.SetActive(LimitLabel.gameObject, state: true);
				SetLabelWarining(LimitLabel, needWarning: true);
				IconSprite.color = Color.gray;
				IconLockFlag.enabled = true;
			}
			IconSprite.spriteName = curBossData.Icon;
			showName.text = StrDictionary.GetDictionaryString("#{100748}");
			ShowStateFlag(GameDefine.ACTIVITY_TYPE.GUILD_BOSS);
		}
		ShowFirstUnlockState(isLevelUnlock);
	}

	private void ShowStateFlag(GameDefine.ACTIVITY_TYPE acttype)
	{
		if (acttype == GameDefine.ACTIVITY_TYPE.GUILD_BATTLE || acttype == GameDefine.ACTIVITY_TYPE.GUILD_DONMINE)
		{
			PVPSP.alpha = 1f;
			PVESP.alpha = 0f;
		}
		else
		{
			PVPSP.alpha = 0f;
			PVESP.alpha = 1f;
		}
		GuildSp.alpha = 1f;
		TeamSp.alpha = 0f;
		AloneSP.alpha = 0f;
		DailyActFlag.alpha = 0f;
		switch (acttype)
		{
		case GameDefine.ACTIVITY_TYPE.GUILD_BOSS:
			ShowScore(19);
			break;
		case GameDefine.ACTIVITY_TYPE.GUILD_DANCE:
			ShowScore(20);
			break;
		}
	}

	public void ResetItem(string id, DelegateDefine.ThirdIntParamDelegate clickFunc, int index, guild_battle_info curinfo)
	{
		curGuildBattleData = DataManager.GetGuildBattleDataById(id);
		isClose = true;
		mHasSubLine = false;
		OnClickLine = clickFunc;
		curActType = GameDefine.ACTIVITY_TYPE.GUILD_BATTLE;
		DurationLabel.text = string.Empty;
		LimitLabel.text = string.Empty;
		string text = string.Empty;
		int num = 0;
		int num2 = 0;
		isLevelUnlock = false;
		if (curinfo.state < 0 || curinfo.state >= 6)
		{
			num = curGuildBattleData.Week;
			num2 = curGuildBattleData.StartTime;
			text = StrDictionary.GetDictionaryString("#{105076}");
		}
		else if (curinfo.state <= 1)
		{
			num = curGuildBattleData.Week1;
			num2 = curGuildBattleData.StartTime1;
			text = StrDictionary.GetDictionaryString("#{105002}");
		}
		else if (curinfo.state <= 3)
		{
			num = curGuildBattleData.Week2;
			num2 = curGuildBattleData.StartTime2;
			text = StrDictionary.GetDictionaryString("#{105003}");
		}
		else if (curinfo.state <= 5)
		{
			num = curGuildBattleData.Week3;
			num2 = curGuildBattleData.StartTime3;
			text = StrDictionary.GetDictionaryString("#{105004}");
		}
		if (curinfo.state == -2)
		{
			DurationLabel.text = StrDictionary.GetDictionaryString("#{101406}");
		}
		else
		{
			PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			TimeSpan localShowTime = TimeTools.GetLocalShowTime(num2, playerCommonData.TimeOffset);
			int key = (num - 1 + TimeTools.GetOffsetDay(num2, playerCommonData.TimeOffset) + GameDefine.WEEK_NAME.Count) % GameDefine.WEEK_NAME.Count;
			DurationLabel.text = $"{text}:{StrDictionary.GetDictionaryString(GameDefine.WEEK_NAME[key])} {localShowTime.Hours:d2}:{localShowTime.Minutes:d2}";
		}
		if (curinfo.state == 0L || curinfo.state == 1 || curinfo.state == 3 || curinfo.state == 5)
		{
			SetLabelWarining(DurationLabel, needWarning: false, isLowWarning: true);
		}
		else
		{
			SetLabelWarining(DurationLabel, needWarning: true, isLowWarning: true);
		}
		IconSprite.spriteName = curGuildBattleData.Icon;
		showName.text = curGuildBattleData.MName;
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			NGUITools.SetActive(LimitLabel.gameObject, state: false);
			IconSprite.color = Color.white;
			IconLockFlag.enabled = false;
			isLevelUnlock = true;
		}
		else
		{
			LimitLabel.text = StrDictionary.GetDictionaryString("#{102087}");
			NGUITools.SetActive(LimitLabel.gameObject, state: true);
			SetLabelWarining(LimitLabel, needWarning: true);
			IconSprite.color = Color.gray;
			IconLockFlag.enabled = true;
		}
		mCurIndex = index;
		mCurSubIndex = 0;
		ShowStateFlag(GameDefine.ACTIVITY_TYPE.GUILD_BATTLE);
		ShowFirstUnlockState(isLevelUnlock);
	}

	public void ResetItem(DelegateDefine.ThirdIntParamDelegate clickFunc, int index, dance_state_info curinfo)
	{
		CurGuildDanceData = DataManager.GetCityDanceDataById(curinfo.ID);
		isClose = true;
		mHasSubLine = false;
		OnClickLine = clickFunc;
		curActType = GameDefine.ACTIVITY_TYPE.GUILD_DANCE;
		DurationLabel.text = string.Empty;
		LimitLabel.text = string.Empty;
		isLevelUnlock = false;
		int num = 0;
		if (curinfo.HasParm)
		{
			num = (int)curinfo.parm - 1;
		}
		if (num < 0)
		{
			num = 0;
		}
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		TimeSpan localShowTime = TimeTools.GetLocalShowTime(CurGuildDanceData.StartTimes[num], playerCommonData.TimeOffset);
		TimeSpan localShowTime2 = TimeTools.GetLocalShowTime(CurGuildDanceData.StartTimes[num] + CurGuildDanceData.DurationTime, playerCommonData.TimeOffset);
		DurationLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101509}"), $"{localShowTime.Hours:D2}:{localShowTime.Minutes:D2}", $"{localShowTime2.Hours:D2}:{localShowTime2.Minutes:D2}");
		if (curinfo.state == 1)
		{
			SetLabelWarining(DurationLabel, needWarning: false, isLowWarning: true);
		}
		else
		{
			SetLabelWarining(DurationLabel, needWarning: true, isLowWarning: true);
		}
		IconSprite.spriteName = CurGuildDanceData.Icon;
		showName.text = StrDictionary.GetDictionaryString(CurGuildDanceData.Name);
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			NGUITools.SetActive(LimitLabel.gameObject, state: false);
			IconSprite.color = Color.white;
			IconLockFlag.enabled = false;
			isLevelUnlock = true;
		}
		else
		{
			LimitLabel.text = StrDictionary.GetDictionaryString("#{102087}");
			NGUITools.SetActive(LimitLabel.gameObject, state: true);
			SetLabelWarining(LimitLabel, needWarning: true);
			IconSprite.color = Color.gray;
			IconLockFlag.enabled = true;
		}
		mCurIndex = index;
		mCurSubIndex = 0;
		ShowStateFlag(GameDefine.ACTIVITY_TYPE.GUILD_DANCE);
		ShowFirstUnlockState(isLevelUnlock);
	}

	public void ResetItem(DelegateDefine.ThirdIntParamDelegate clickFunc, int index, List<guild_map_info> curinfolist)
	{
		GuildCityList = curinfolist;
		guild_map_info guild_map_info = GuildCityList[0];
		GuildCaptureData guildCaptureDataByID = DataManager.GetGuildCaptureDataByID(GuildCityList[0].id);
		isClose = true;
		mHasSubLine = false;
		OnClickLine = clickFunc;
		curActType = GameDefine.ACTIVITY_TYPE.GUILD_DONMINE;
		DurationLabel.text = string.Empty;
		LimitLabel.text = string.Empty;
		isLevelUnlock = false;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		TimeSpan localShowTime = TimeTools.GetLocalShowTime(guildCaptureDataByID.StartTime, playerCommonData.TimeOffset);
		int key = (guildCaptureDataByID.Week - 1 + TimeTools.GetOffsetDay(guildCaptureDataByID.StartTime, playerCommonData.TimeOffset) + GameDefine.WEEK_NAME.Count) % GameDefine.WEEK_NAME.Count;
		TimeSpan localShowTime2 = TimeTools.GetLocalShowTime(guildCaptureDataByID.StartTime + guildCaptureDataByID.DurationTime, playerCommonData.TimeOffset);
		DurationLabel.text = $"{StrDictionary.GetDictionaryString(GameDefine.WEEK_NAME[key])} {localShowTime.Hours:d2}:{localShowTime.Minutes:d2}-{localShowTime2.Hours:d2}:{localShowTime2.Minutes:d2}";
		if (guild_map_info.state == 1)
		{
			SetLabelWarining(DurationLabel, needWarning: false, isLowWarning: true);
		}
		else
		{
			SetLabelWarining(DurationLabel, needWarning: true, isLowWarning: true);
		}
		IconSprite.spriteName = guildCaptureDataByID.Icon;
		showName.text = StrDictionary.GetDictionaryString(guildCaptureDataByID.Name);
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			NGUITools.SetActive(LimitLabel.gameObject, state: false);
			IconSprite.color = Color.white;
			IconLockFlag.enabled = false;
			isLevelUnlock = true;
		}
		else
		{
			LimitLabel.text = StrDictionary.GetDictionaryString("#{102087}");
			NGUITools.SetActive(LimitLabel.gameObject, state: true);
			SetLabelWarining(LimitLabel, needWarning: true);
			IconSprite.color = Color.gray;
			IconLockFlag.enabled = true;
		}
		mCurIndex = index;
		mCurSubIndex = 0;
		ShowStateFlag(GameDefine.ACTIVITY_TYPE.GUILD_DONMINE);
		ShowFirstUnlockState(isLevelUnlock);
	}

	private void ShowFirstUnlockState(bool isunlock)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (isunlock)
		{
			GameDefine.ACTIVITY_TYPE type = curActType;
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
			GameDefine.ACTIVITY_TYPE type = curActType;
			playerCommonData.SetFirstClickState(GameDefine.GetActKey_FirstClick(type));
			NGUITools.SetActive(HighEffectObj, state: false);
		}
	}

	public void OnClickItemBtn()
	{
		if (isLevelUnlock)
		{
			CheckFirstClickState();
		}
		if (OnClickLine != null)
		{
			OnClickLine(mCurIndex, mCurSubIndex, 0);
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

	public void RefreshSelect(int curchoose, int curSubChoose)
	{
		if (mCurIndex != curchoose)
		{
			selectbtnbg.spriteName = "CZ_huaDongBG";
		}
		else
		{
			selectbtnbg.spriteName = "CZ_huaDongBG_1";
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
