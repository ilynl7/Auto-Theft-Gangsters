using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DailyCopyLineLogic : MonoBehaviour
{
	public UILabel NameLabel;

	public UISprite IconSprite;

	public UISprite IconLockFlag;

	public UILabel LimitLabel;

	public UISprite SelectBkSprite;

	public UIButtonColor LineBtnColor;

	public int curIndex = -1;

	private DelegateDefine.StringGameObjectDelegate onClickItem;

	public UIGrid SubLineRootGride;

	public UIMyCenterOnChild CenterOnChild;

	private string mKey = string.Empty;

	private bool isFinal;

	private List<copyscene_info> CurInfoList = new List<copyscene_info>();

	public UIWidget PVPSP;

	public UIWidget PVESP;

	public UIWidget AloneSP;

	public UIWidget MultiSp;

	public UIWidget TeamSp;

	public UIWidget GuildSp;

	public UISprite DailyActFlag;

	public UILabel DailyActValLabel;

	private bool isWarningflag;

	public GameObject HighEffectObj;

	private bool isLevelUnlock;

	private CopySceneData mCurCopyScene;

	private MAPTYPE CurMapType = MAPTYPE.INVALID;

	public string Key => mKey;

	public void ResetItem(copyscene_info info, DelegateDefine.StringGameObjectDelegate clickFunc, bool isfinal)
	{
		isFinal = isfinal;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		mKey = info.ID;
		onClickItem = clickFunc;
		mCurCopyScene = DataManager.GetCopySceneDataById(info.ID);
		int num = (int)info.CurNum;
		isWarningflag = false;
		LimitLabel.enabled = true;
		isLevelUnlock = false;
		CurMapType = (MAPTYPE)mCurCopyScene.SubType;
		if (mCurCopyScene.IsSingleDance)
		{
			LimitLabel.text = string.Format("{0}  {1}", StrDictionary.GetDictionaryString("#{102018}"), TimeTools.GetMinuteSecondStr(num));
		}
		else
		{
			LimitLabel.text = string.Format("{0}  {1}/{2}", StrDictionary.GetDictionaryString("#{101508}"), num, mCurCopyScene.MaxPlayNum);
		}
		if (num <= 0)
		{
			isWarningflag = true;
		}
		if (!CheckLevel(mCurCopyScene.MinLevel))
		{
			LimitLabel.text = $"Lv {mCurCopyScene.MinLevel}";
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
		if (isWarningflag)
		{
			SetLabelWarining(LimitLabel, needWarning: true);
		}
		else
		{
			SetLabelWarining(LimitLabel, needWarning: false);
		}
		ShowDailyActInfo(mCurCopyScene);
		GuildSp.alpha = 0f;
		if (mCurCopyScene.IsTeamCopy)
		{
			MultiSp.alpha = 0f;
			AloneSP.alpha = 0f;
			TeamSp.alpha = 1f;
		}
		else if (mCurCopyScene.IsScuffleCopy)
		{
			MultiSp.alpha = 1f;
			AloneSP.alpha = 0f;
			TeamSp.alpha = 0f;
		}
		else
		{
			MultiSp.alpha = 0f;
			AloneSP.alpha = 1f;
			TeamSp.alpha = 0f;
		}
		if (mCurCopyScene.IsPVPFlag())
		{
			PVPSP.alpha = 1f;
			PVESP.alpha = 0f;
		}
		else
		{
			PVPSP.alpha = 0f;
			PVESP.alpha = 1f;
		}
		NameLabel.text = mCurCopyScene.MName;
		IconSprite.spriteName = mCurCopyScene.Icon;
		ShowFirstUnlockState(isLevelUnlock);
	}

	public void ResetItem(string key, string name, List<copyscene_info> infoList, DelegateDefine.StringGameObjectDelegate clickFunc, bool isfinal)
	{
		isFinal = isfinal;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		onClickItem = clickFunc;
		CurInfoList = infoList;
		isLevelUnlock = false;
		if (infoList.Count > 0)
		{
			int index = 0;
			for (int i = 0; i < infoList.Count; i++)
			{
				CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(infoList[i].ID);
				if (!CheckLevel(copySceneDataById.MinLevel))
				{
					break;
				}
				index = i;
			}
			copyscene_info copyscene_info = infoList[index];
			mKey = copyscene_info.ID;
			mCurCopyScene = DataManager.GetCopySceneDataById(copyscene_info.ID);
			CurMapType = (MAPTYPE)mCurCopyScene.SubType;
			GuildSp.alpha = 0f;
			if (mCurCopyScene.IsTeamCopy)
			{
				MultiSp.alpha = 0f;
				AloneSP.alpha = 0f;
				TeamSp.alpha = 1f;
			}
			else
			{
				MultiSp.alpha = 0f;
				AloneSP.alpha = 1f;
				TeamSp.alpha = 0f;
			}
			if (mCurCopyScene.IsPVPFlag())
			{
				PVPSP.alpha = 1f;
				PVESP.alpha = 0f;
			}
			else
			{
				PVPSP.alpha = 0f;
				PVESP.alpha = 1f;
			}
			ShowDailyActInfo(mCurCopyScene);
			if (mCurCopyScene.SubType == 16 || mCurCopyScene.SubType == 12)
			{
				int num = (int)copyscene_info.CurNum;
				IconSprite.spriteName = mCurCopyScene.Icon;
				isWarningflag = false;
				LimitLabel.enabled = true;
				LimitLabel.text = string.Format("{0}  {1}/{2}", StrDictionary.GetDictionaryString("#{101508}"), num, mCurCopyScene.MaxPlayNum);
				if (num <= 0)
				{
					isWarningflag = true;
				}
				if (!CheckLevel(mCurCopyScene.MinLevel))
				{
					isWarningflag = true;
					LimitLabel.text = $"Lv {mCurCopyScene.MinLevel}";
					IconSprite.color = Color.gray;
					IconLockFlag.enabled = true;
				}
				else
				{
					IconSprite.color = Color.white;
					IconLockFlag.enabled = false;
					isLevelUnlock = true;
				}
				if (isWarningflag)
				{
					SetLabelWarining(LimitLabel, needWarning: true);
				}
				else
				{
					SetLabelWarining(LimitLabel, needWarning: false);
				}
			}
			else if (mCurCopyScene.SubType == 1)
			{
				IconSprite.spriteName = mCurCopyScene.Icon;
				LimitLabel.enabled = false;
			}
		}
		else
		{
			LimitLabel.enabled = false;
		}
		NameLabel.text = StrDictionary.GetDictionaryString(name);
		SelectBkSprite.spriteName = "CZ_huaDongBG";
		ShowFirstUnlockState(isLevelUnlock);
	}

	private void ShowFirstUnlockState(bool isunlock)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (isunlock)
		{
			MAPTYPE curMapType = CurMapType;
			if (playerCommonData.CheckFirstClickState(GameDefine.GetCopyKey_FirstClick(curMapType)))
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
			MAPTYPE curMapType = CurMapType;
			playerCommonData.SetFirstClickState(GameDefine.GetCopyKey_FirstClick(curMapType));
			NGUITools.SetActive(HighEffectObj, state: false);
		}
	}

	public void ShowDailyActInfo(CopySceneData curdata)
	{
		DailyActFlag.alpha = 0f;
		if (curdata != null)
		{
			switch (curdata.SubType)
			{
			case 12:
				ShowScore(1);
				break;
			case 16:
				ShowScore(0);
				break;
			case 11:
				ShowScore(2);
				break;
			case 7:
				ShowScore(3);
				break;
			case 20:
				ShowScore(16);
				break;
			case 26:
				ShowScore(17);
				break;
			}
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

	public bool CheckLevel(int minLevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minLevel);
	}

	public void RefershParentLine(copyscene_info datainfo)
	{
	}

	public void UpdateLineInfo(copyscene_info datainfo)
	{
		if (datainfo.ID.Equals(mKey))
		{
			int num = (int)datainfo.CurNum;
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(datainfo.ID);
			LimitLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{101508}"), num, copySceneDataById.MaxPlayNum);
			if (num <= 0)
			{
				SetLabelWarining(LimitLabel, needWarning: true, isLowWarning: true);
			}
			else
			{
				SetLabelWarining(LimitLabel, needWarning: false, isLowWarning: true);
			}
		}
	}

	public void ClickTargetBtn(string actid)
	{
	}

	public void OnClikcItemBtn()
	{
		OnClickSelectItemBtn(null);
	}

	public void OnClickSelectItemBtn(string actid)
	{
		if (isLevelUnlock)
		{
			CheckFirstClickState();
		}
		if (onClickItem != null)
		{
			onClickItem(mKey, base.gameObject);
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

	public void RefreshLineSelect(string key)
	{
	}

	public void SetTowerInfo(tower_info curtowerinfo, DelegateDefine.StringGameObjectDelegate clickFunc)
	{
		onClickItem = clickFunc;
		mKey = "tower";
		GuildSp.alpha = 0f;
		MultiSp.alpha = 0f;
		TeamSp.alpha = 0f;
		AloneSP.alpha = 1f;
		PVESP.alpha = 1f;
		PVPSP.alpha = 0f;
		ShowScore(5);
		NameLabel.text = StrDictionary.GetDictionaryString("#{101538}");
		IconSprite.spriteName = GameDefine.TowerIconName;
		int condition = DataManager.GetFunctionDataById(4003.ToString()).Condition;
		isWarningflag = false;
		LimitLabel.enabled = true;
		LimitLabel.text = string.Format("{0}:{1}/1", StrDictionary.GetDictionaryString("#{101528}"), curtowerinfo.times);
		isLevelUnlock = false;
		CurMapType = MAPTYPE.SEX_GAME;
		if (!CheckLevel(condition))
		{
			LimitLabel.text = $"Lv {condition}";
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

	public void SetSexGameInfo(SexMiniData CurSexInfo, DelegateDefine.StringGameObjectDelegate clickFunc)
	{
		onClickItem = clickFunc;
		mKey = "Sex";
		GuildSp.alpha = 0f;
		MultiSp.alpha = 0f;
		TeamSp.alpha = 0f;
		AloneSP.alpha = 1f;
		PVESP.alpha = 1f;
		PVPSP.alpha = 0f;
		ShowDailyActInfo(null);
		NameLabel.text = StrDictionary.GetDictionaryString(CurSexInfo.Name);
		IconSprite.spriteName = CurSexInfo.Icon;
		int unlockLevel = CurSexInfo.UnlockLevel;
		isWarningflag = false;
		LimitLabel.enabled = true;
		LimitLabel.text = string.Empty;
		isLevelUnlock = false;
		CurMapType = MAPTYPE.CLAMBING_TOWER;
		if (!CheckLevel(unlockLevel))
		{
			LimitLabel.text = $"Lv {unlockLevel}";
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
}
