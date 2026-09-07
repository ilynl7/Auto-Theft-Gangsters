using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class NewDailyActivityUIRootLogic : SingletonUnity<NewDailyActivityUIRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public List<ActivityItemLogic> ActivityItems = new List<ActivityItemLogic>();

	public ShowRewardItems ShowRewardItemsScripts;

	public UILabel DescLabel;

	public UIScrollView uiScrollView;

	public UITexture ActivityPic;

	private int remainNum;

	private int mCurChoosedIndex = -1;

	private List<activity_info> activityList = new List<activity_info>();

	private activity_info curActivityInfo;

	public UISprite startBtnSp;

	public GameObject RankBtnObj;

	public UISprite signBtnSp;

	public UILabel signlabel;

	private int canStartFlag;

	private string curRulestr;

	private int unLockLevel;

	public UITexture CopyBG;

	public bool IsMapClick;

	public int TargetTypeId;

	public List<WildBossLineLogic> WildBossLineList = new List<WildBossLineLogic>();

	private List<activity_info> wildBossInfos = new List<activity_info>();

	public UITable RootTable;

	public List<UILabel> rankLabels;

	public List<GameObject> RankLabelObjList;

	private Dictionary<string, daily_active> activeDic = new Dictionary<string, daily_active>();

	public UIScrollBar ItemRootBar;

	public GameObject ActivityInfoPage;

	public GameObject ActivityLineRoot;

	private bool isShowInfoPage;

	private List<guild_map_info> GuildCityList;

	private GuildCaptureData CurGuildCityData;

	private bool isOpenGuildCity;

	public GameObject RewardObj;

	private List<guild_boss> guildBossInfos = new List<guild_boss>();

	private guild_boss curBossInfo;

	private GuildBattleData mCurGuildBattleData;

	private int mGuildBattleIndex = -1;

	private int GUILD_BATTLE_INDEX;

	private int mCurChoosedSubIndex = -1;

	private GuildBossData curGuildBossData;

	private guild_battle_info GuildBattleInfo;

	public List<GuildBossLineLogic> guildBossLines = new List<GuildBossLineLogic>();

	private CityDanceData mCurGuildDanceData;

	private int GUILD_DANCE_INDEX = 2;

	private dance_state_info GuildDanceInfo;

	public UISprite ChangeTimeBtn;

	private long ReamainTime;

	public UILabel NextTimeLabel;

	public UILabel NextTimeName;

	private int GuildCityIndex;

	private WildBossData curWildBossData;

	private bool IsShowGuildDanceFlag;

	private float tempCountTime;

	public List<activity_info> ActivityList => activityList;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			TutorialManager.OnClickTutorialBtn onClickTutorialBtn = mOnClickTutorialBtn;
			mOnClickTutorialBtn = null;
			onClickTutorialBtn();
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public void EnableReset()
	{
		NGUITools.SetActive(ActivityInfoPage.gameObject, state: false);
		NGUITools.SetActive(ActivityLineRoot.gameObject, state: true);
		isShowInfoPage = false;
		for (int i = 0; i < ActivityItems.Count; i++)
		{
			NGUITools.SetActive(ActivityItems[i].gameObject, state: false);
		}
		for (int j = 0; j < WildBossLineList.Count; j++)
		{
			NGUITools.SetActive(WildBossLineList[j].gameObject, state: false);
			NGUITools.SetActive(WildBossLineList[j].Sublineobj, state: false);
		}
		for (int k = 0; k < guildBossLines.Count; k++)
		{
			NGUITools.SetActive(guildBossLines[k].gameObject, state: false);
		}
		TargetTypeId = -1;
	}

	public void ShowAcitvityLine()
	{
		SingletonUnity<NewMapUIRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID);
		SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjNormal();
		NGUITools.SetActive(ActivityInfoPage.gameObject, state: false);
		NGUITools.SetActive(ActivityLineRoot.gameObject, state: true);
		isShowInfoPage = false;
		RefreshActivityPage();
		RefershBossInfo();
		RefershGuildBossInfo();
	}

	public void ShowActivityInfo()
	{
		NGUITools.SetActive(ActivityInfoPage.gameObject, state: true);
		NGUITools.SetActive(ActivityLineRoot.gameObject, state: false);
		isShowInfoPage = true;
		ReamainTime = 0L;
	}

	public void RefreshActivityPage()
	{
		if (isShowInfoPage)
		{
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		activityList = new List<activity_info>(playerData.ActivityData.CurActivityDataDic.Values);
		for (int num = activityList.Count - 1; num >= 0; num--)
		{
			if (activityList[num].Type == 3 || activityList[num].Type == 8)
			{
				activityList.RemoveAt(num);
			}
		}
		activityList.Sort((activity_info x, activity_info y) => GetActivityUnlockLevel(x) - GetActivityUnlockLevel(y));
		int num2 = activityList.Count - ActivityItems.Count;
		int count = ActivityItems.Count;
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(ActivityItems[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{count + i:D2}";
				ActivityItemLogic component = gameObject.GetComponent<ActivityItemLogic>();
				if (component != null)
				{
					component.transform.parent = ActivityItems[0].transform.parent;
					component.transform.localScale = Vector3.one;
					component.transform.localPosition = Vector3.zero;
					ActivityItems.Add(component);
				}
			}
		}
		int num3 = -1;
		for (int j = 0; j < ActivityItems.Count; j++)
		{
			NGUITools.SetActive(ActivityItems[j].gameObject, j < activityList.Count);
			if (j < activityList.Count)
			{
				ActivityItems[j].onClickItem = OnClickItemBtn;
				ResetItemLine(ActivityItems[j], j, mCurChoosedIndex);
				if (activityList[j].Type == TargetTypeId)
				{
					num3 = j;
					TargetTypeId = -1;
				}
			}
		}
		RootTable.Reposition();
		uiScrollView.ResetPosition();
		mCurChoosedIndex = -1;
		if (TutorialManager.CurStep == TUTORIAL_STEP.SURVIVAL_BATTLE_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.ROBBORY_WAIT_DATA)
		{
			CheckTutorialEvent();
		}
		else if (num3 != -1)
		{
			if (IsMapClick)
			{
				ActivityItems[num3].MapClickItemBtn();
				IsMapClick = false;
			}
			else
			{
				ActivityItems[num3].OnClikcItemBtn();
			}
		}
		if (wildBossInfos == null || wildBossInfos.Count == 0)
		{
			for (int k = 0; k < WildBossLineList.Count; k++)
			{
				NGUITools.SetActive(WildBossLineList[k].gameObject, state: false);
				NGUITools.SetActive(WildBossLineList[k].Sublineobj, state: false);
			}
		}
	}

	public void ClickTargetType(GameDefine.ACTIVITY_TYPE type)
	{
		for (int i = 0; i < ActivityItems.Count; i++)
		{
			if (ActivityItems[i].curActivityInfo.Type == (long)type)
			{
				ActivityItems[i].MapClickItemBtn();
				break;
			}
		}
	}

	public void RefershBossInfo()
	{
		if (isShowInfoPage)
		{
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		wildBossInfos = playerData.ActivityData.WildBossInfoList;
		wildBossInfos.Sort((activity_info x, activity_info y) => int.Parse(x.ID) - int.Parse(y.ID));
		int num = 1;
		int num2 = num - WildBossLineList.Count;
		int num3 = 50;
		WildBossLineList[0].gameObject.name = $"huaDongTiao_{num3:D2}";
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(WildBossLineList[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{num3 + i + 1:D2}";
				WildBossLineLogic component = gameObject.GetComponent<WildBossLineLogic>();
				if (component != null)
				{
					component.transform.parent = WildBossLineList[0].transform.parent;
					component.transform.localScale = Vector3.one;
					component.transform.localPosition = Vector3.zero;
					WildBossLineList.Add(component);
				}
			}
		}
		for (int j = 0; j < WildBossLineList.Count; j++)
		{
			NGUITools.SetActive(WildBossLineList[j].gameObject, state: true);
			WildBossLineList[j].ResetItem(wildBossInfos, OnClickBossBtn);
		}
		mCurChoosedIndex = -1;
		RootTable.Reposition();
		uiScrollView.ResetPosition();
		if (TutorialManager.CurStep == TUTORIAL_STEP.WORLD_BOSS_WAIT_DATA)
		{
			CheckTutorialEvent();
		}
		else if (TargetTypeId == 5)
		{
			WildBossLineList[0].ClickTargetBtn();
			TargetTypeId = -1;
			IsMapClick = false;
		}
	}

	public void RefershGuildBossInfo()
	{
		if (isShowInfoPage)
		{
			UpdateGuildBossInfo();
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.ActivityData.CurGuildBossDataDic.Count > 0)
		{
			guildBossInfos = new List<guild_boss>(playerData.ActivityData.CurGuildBossDataDic.Values);
			guildBossInfos.Sort((guild_boss x, guild_boss y) => int.Parse(x.id) - int.Parse(y.id));
		}
		int num = Mathf.Min(guildBossInfos.Count, 1);
		if (playerData.ActivityData.GuildBattleInfo != null)
		{
			num++;
			mGuildBattleIndex = GUILD_BATTLE_INDEX;
		}
		else
		{
			mGuildBattleIndex = -1;
		}
		if (playerData.ActivityData.GuildDanceInfo != null)
		{
			num++;
		}
		if (playerData.ActivityData.CurGuildCityDataDic != null)
		{
			num++;
		}
		int num2 = num - guildBossLines.Count;
		int num3 = 80;
		guildBossLines[0].gameObject.name = $"huaDongTiao_{num3:D2}";
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(guildBossLines[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{num3 + i + 1:D2}";
				GuildBossLineLogic component = gameObject.GetComponent<GuildBossLineLogic>();
				if (component != null)
				{
					component.transform.parent = guildBossLines[0].transform.parent;
					component.transform.localScale = Vector3.one;
					component.transform.localPosition = Vector3.zero;
					guildBossLines.Add(component);
				}
			}
		}
		for (int j = 0; j < guildBossLines.Count; j++)
		{
			NGUITools.SetActive(guildBossLines[j].gameObject, j < num);
		}
		int num4 = -1;
		if (playerData.ActivityData.GuildBattleInfo != null)
		{
			for (int k = 1; k < num; k++)
			{
				guildBossLines[k].ResetItem(guildBossInfos, OnClickGuildBossBtn, k);
			}
			GuildBattleInfo = playerData.ActivityData.GuildBattleInfo;
			mCurGuildBattleData = DataManager.GetGuildBattleDataById(GuildBattleInfo.ID);
			guildBossLines[mGuildBattleIndex].ResetItem(mCurGuildBattleData.ID, OnClickGuildBattle, mGuildBattleIndex, GuildBattleInfo);
			if (TargetTypeId == 9)
			{
				num4 = mGuildBattleIndex;
				TargetTypeId = -1;
			}
			else if (TargetTypeId == 6 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				num4 = 1;
				TargetTypeId = -1;
			}
		}
		else
		{
			for (int l = 0; l < num; l++)
			{
				guildBossLines[l].ResetItem(guildBossInfos, OnClickGuildBossBtn, l);
			}
		}
		if (playerData.ActivityData.GuildDanceInfo != null)
		{
			GuildDanceInfo = playerData.ActivityData.GuildDanceInfo;
			mCurGuildDanceData = DataManager.GetCityDanceDataById(GuildDanceInfo.ID);
			guildBossLines[GUILD_DANCE_INDEX].ResetItem(OnClickGuildDance, GUILD_DANCE_INDEX, GuildDanceInfo);
			if (TargetTypeId == 16)
			{
				num4 = GUILD_DANCE_INDEX;
				TargetTypeId = -1;
			}
		}
		if (playerData.ActivityData.CurGuildCityDataDic != null)
		{
			GuildCityList = new List<guild_map_info>(playerData.ActivityData.CurGuildCityDataDic.Values);
			isOpenGuildCity = false;
			if (GuildCityList != null && GuildCityList.Count > 0)
			{
				for (int m = 0; m < GuildCityList.Count; m++)
				{
					if (GuildCityList[m].state == 1)
					{
						isOpenGuildCity = true;
						break;
					}
				}
				GuildCityIndex = num - 1;
				guildBossLines[GuildCityIndex].ResetItem(OnClickGuildCity, GuildCityIndex, GuildCityList);
				if (TargetTypeId == 18)
				{
					num4 = GuildCityIndex;
					TargetTypeId = -1;
				}
			}
		}
		mCurChoosedIndex = -1;
		RootTable.Reposition();
		uiScrollView.ResetPosition();
		if (num <= 0)
		{
			return;
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.GUILD_BOSS_WAIT_DATA)
		{
			for (int n = 0; n < num; n++)
			{
				if (guildBossLines[n].IsGuildBossLine())
				{
					guildBossLines[n].OnClickItemBtn();
					break;
				}
			}
			CheckTutorialEvent();
		}
		else if (num4 != -1)
		{
			guildBossLines[num4].OnClickItemBtn();
			IsMapClick = false;
		}
	}

	public void OnClickTargetType(GameDefine.ACTIVITY_TYPE targettype)
	{
		if (targettype == GameDefine.ACTIVITY_TYPE.GUILD_BATTLE)
		{
			if (guildBossLines.Count > 0)
			{
				guildBossLines[0].OnClickItemBtn();
			}
		}
		else if (TargetTypeId == 6 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild() && guildBossLines.Count > 1)
		{
			guildBossLines[1].OnClickItemBtn();
		}
	}

	public bool CheckLevel(int minLevel, int maxlevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minLevel, maxlevel);
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		ActivityItemLogic itemLogic = ActivityItems[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex), mCurChoosedIndex);
	}

	private void ResetItemLine(ActivityItemLogic itemLogic, int idx, int curChoose)
	{
		if (idx < activityList.Count)
		{
			itemLogic.UpdateItem(idx, activityList[idx], curChoose);
		}
	}

	private void UpdateSelectItem()
	{
	}

	public void OnClickGuildCity(int index, int subIndex, int isenable)
	{
		if (mCurChoosedIndex == index && mCurChoosedSubIndex == subIndex)
		{
			return;
		}
		curActivityInfo = null;
		mCurChoosedIndex = index;
		mCurChoosedSubIndex = subIndex;
		IsShowGuildDanceFlag = false;
		ShowActivityInfo();
		for (int i = 0; i < RankLabelObjList.Count; i++)
		{
			NGUITools.SetActive(RankLabelObjList[i], state: false);
		}
		SelectShowINMap(GameDefine.ACTIVITY_TYPE.GUILD_DONMINE, isMapSelect: false);
		NGUITools.SetActive(RankBtnObj, state: false);
		NGUITools.SetActive(signBtnSp.gameObject, state: false);
		NGUITools.SetActive(startBtnSp.gameObject, state: true);
		NGUITools.SetActive(ChangeTimeBtn.gameObject, state: false);
		NGUITools.SetActive(RewardObj, state: false);
		if (!isOpenGuildCity)
		{
			canStartFlag = 2;
			startBtnSp.spriteName = GameDefine.BtnIcon[2];
		}
		else
		{
			canStartFlag = 0;
			startBtnSp.spriteName = GameDefine.BtnIcon[1];
		}
		UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: false);
		string empty = string.Empty;
		string empty2 = string.Empty;
		CurGuildCityData = DataManager.GetGuildCaptureDataByID(GuildCityList[0].id);
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			startBtnSp.spriteName = GameDefine.BtnIcon[2];
		}
		empty = StrDictionary.GetDictionaryString(CurGuildCityData.Description);
		DescLabel.text = empty;
		for (int j = 0; j < rankLabels.Count; j++)
		{
			rankLabels[j].text = string.Empty;
		}
		UpdateSelectItem();
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBG.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if (CurGuildCityData != null && (CopyBG.mainTexture == null || !CopyBG.mainTexture.name.Equals(CurGuildCityData.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(CurGuildCityData.Background, TextureLoadFinish));
		}
	}

	public void OnClickItemBtn(int realIdx, int enableflag, int unlock, bool isMapClick)
	{
		if (mCurChoosedIndex == realIdx && curActivityInfo != null && curActivityInfo.Type != 5)
		{
			if (curActivityInfo == null)
			{
				return;
			}
			if (TutorialManager.CurStep == TUTORIAL_STEP.SURVIVAL_BATTLE_CHOOSE_COPY)
			{
				if (curActivityInfo.Type == 7)
				{
					CheckTutorialEvent();
				}
			}
			else if (TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_CHOOSE_COPY)
			{
				if (curActivityInfo.Type == 1)
				{
					CheckTutorialEvent();
				}
			}
			else if (TutorialManager.CurStep == TUTORIAL_STEP.ROBBORY_CHOOSE_COPY && curActivityInfo.Type == 2)
			{
				CheckTutorialEvent();
			}
			return;
		}
		ShowActivityInfo();
		for (int i = 0; i < RankLabelObjList.Count; i++)
		{
			NGUITools.SetActive(RankLabelObjList[i], state: false);
		}
		unLockLevel = unlock;
		mCurChoosedIndex = realIdx;
		mCurChoosedSubIndex = -1;
		activity_info activity_info = activityList[realIdx];
		curActivityInfo = activity_info;
		SelectShowINMap(curActivityInfo, isMapClick);
		remainNum = (int)curActivityInfo.CurNum;
		ShowRewardData showRewardData = null;
		string text = string.Empty;
		curRulestr = string.Empty;
		string value = string.Empty;
		NGUITools.SetActive(RankBtnObj, state: false);
		NGUITools.SetActive(ChangeTimeBtn.gameObject, state: false);
		NGUITools.SetActive(signBtnSp.gameObject, state: false);
		if (curActivityInfo.Type == 1 || curActivityInfo.Type == 2)
		{
			EscortData escortDataById = DataManager.GetEscortDataById(curActivityInfo.ID);
			string id = DataManager.GetAdaptDataByID(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.Level).DorpKeyDic[escortDataById.ShowRewardId];
			showRewardData = DataManager.GetShowRewardDataByID(id);
			text = StrDictionary.GetDictionaryString(escortDataById.Description);
			curRulestr = escortDataById.Rule;
			if (enableflag == 2)
			{
				enableflag = 0;
			}
			value = escortDataById.Background;
		}
		else if (curActivityInfo.Type == 3)
		{
			CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(curActivityInfo.ID);
			string id2 = DataManager.GetAdaptDataByID(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.Level).DorpKeyDic[cityDanceDataById.ShowRewardId];
			showRewardData = DataManager.GetShowRewardDataByID(id2);
			text = StrDictionary.GetDictionaryString(cityDanceDataById.Description);
			curRulestr = cityDanceDataById.Rule;
			value = cityDanceDataById.Background;
		}
		else if (curActivityInfo.Type == 4)
		{
			if (curActivityInfo.State == 1)
			{
				if (curActivityInfo.HasSign && curActivityInfo.sign == 1)
				{
					signBtnSp.spriteName = "CZ_anNiu_2+";
					signlabel.text = StrDictionary.GetDictionaryString("#{102055}");
				}
				else
				{
					signBtnSp.spriteName = "CZ_anNiu_2";
					signlabel.text = StrDictionary.GetDictionaryString("#{102048}");
				}
			}
			else
			{
				signBtnSp.spriteName = "CZ_anNiu_2+";
				signlabel.text = StrDictionary.GetDictionaryString("#{102048}");
			}
			NGUITools.SetActive(signBtnSp.gameObject, state: true);
			BarFightCopyData barFightCopyDataByID = DataManager.GetBarFightCopyDataByID(curActivityInfo.ID);
			string id3 = DataManager.GetAdaptDataByID(SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.ServerLevel).DorpKeyDic[barFightCopyDataByID.RewardID];
			showRewardData = DataManager.GetShowRewardDataByID(id3);
			text = StrDictionary.GetDictionaryString(barFightCopyDataByID.Description);
			curRulestr = barFightCopyDataByID.Rule;
			value = barFightCopyDataByID.Background;
		}
		else if (curActivityInfo.Type == 7)
		{
			SurviveBattleData surviveBattleDataById = DataManager.GetSurviveBattleDataById(curActivityInfo.ID);
			string showRewardID = surviveBattleDataById.ShowRewardID;
			showRewardData = DataManager.GetShowRewardDataByID(showRewardID);
			text = StrDictionary.GetDictionaryString(surviveBattleDataById.Desc);
			curRulestr = surviveBattleDataById.Rule;
			value = surviveBattleDataById.Background;
		}
		else if (curActivityInfo.Type == 8)
		{
			SexMiniData sexMiniDataById = DataManager.GetSexMiniDataById(curActivityInfo.ID);
			string showRewardID2 = sexMiniDataById.ShowRewardID;
			showRewardData = DataManager.GetShowRewardDataByID(showRewardID2);
			text = StrDictionary.GetDictionaryString(sexMiniDataById.Description);
			curRulestr = sexMiniDataById.Rule;
			value = sexMiniDataById.Background;
			NGUITools.SetActive(RankBtnObj, state: true);
		}
		NGUITools.SetActive(RewardObj.gameObject, state: true);
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: false);
		}
		DescLabel.text = text;
		if (enableflag == 0)
		{
			startBtnSp.spriteName = "CZ_anNiu_2";
		}
		else
		{
			startBtnSp.spriteName = "CZ_anNiu_2+";
		}
		canStartFlag = enableflag;
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBG.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if (!string.IsNullOrEmpty(value) && (CopyBG.mainTexture == null || !CopyBG.mainTexture.name.Equals(value)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(value, TextureLoadFinish));
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.BAR_FIGHT_CHOOSE_COPY)
		{
			if (curActivityInfo.Type == 4)
			{
				CheckTutorialEvent();
			}
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_CHOOSE_COPY)
		{
			if (curActivityInfo.Type == 1)
			{
				CheckTutorialEvent();
			}
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.ROBBORY_CHOOSE_COPY)
		{
			if (curActivityInfo.Type == 2)
			{
				CheckTutorialEvent();
			}
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.SURVIVAL_BATTLE_CHOOSE_COPY)
		{
			if (curActivityInfo.Type == 7)
			{
				CheckTutorialEvent();
			}
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.BAR_FIGHT_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.BAR_FIGHT_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.ROBBORY_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.ROBBORY_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.SURVIVAL_BATTLE_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.SURVIVAL_BATTLE_CLICK_START)
		{
			CheckTutorialEvent();
		}
	}

	private void TextureLoadFinish(string name, Texture tex)
	{
		CopyBG.mainTexture = tex;
	}

	private void SetRewardItem(List<string> itemIds, List<EQUIP_QUALITY> qualitys, List<int> counts)
	{
		ShowRewardItemsScripts.ShowRewards(itemIds, qualitys, counts);
	}

	public void OnClickSignBarFight()
	{
	}

	public void OnClickStart()
	{
		if (curActivityInfo == null)
		{
			OnClickGuildActivityStartBtn();
			return;
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.BAR_FIGHT_CLICK_START && curActivityInfo.Type == 4)
		{
			CheckTutorialEvent();
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_CLICK_START && curActivityInfo.Type == 1)
		{
			CheckTutorialEvent();
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.ROBBORY_CLICK_START && curActivityInfo.Type == 2)
		{
			CheckTutorialEvent();
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.SURVIVAL_BATTLE_CLICK_START && curActivityInfo.Type == 7)
		{
			CheckTutorialEvent();
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.WORLD_BOSS_CLICK_START && curActivityInfo.Type == 5)
		{
			CheckTutorialEvent();
		}
		if (canStartFlag != 0)
		{
			switch (canStartFlag)
			{
			case 5:
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101541}"));
				break;
			case 1:
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
				TutorialManager.LevelLimitAction();
				break;
			case 6:
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
				break;
			case 7:
				TutorialManager.LevelLimitAction();
				break;
			case 3:
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{102057}"));
				break;
			case 2:
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101541}"));
				break;
			case 4:
				break;
			}
			return;
		}
		if (curActivityInfo.Type == 1)
		{
			SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
			MissionData missionDataByID = DataManager.GetMissionDataByID(curActivityInfo.ID);
			MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			missionManager.MissionFindPath(missionDataByID);
		}
		else if (curActivityInfo.Type == 2)
		{
			SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
			MissionData missionDataByID2 = DataManager.GetMissionDataByID(curActivityInfo.ID);
			MissionManager missionManager2 = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			missionManager2.MissionFindPath(missionDataByID2);
		}
		else if (curActivityInfo.Type == 3)
		{
			SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AutoMoveDest(curActivityInfo.ID, Vector3.right * -5f, AUTO_SEARCH_PARTH_FINISHEVENT.CITY_DANCE);
		}
		else if (curActivityInfo.Type == 4)
		{
			if (curActivityInfo.State == 2)
			{
				enter_bar_fight.request request = new enter_bar_fight.request();
				request.ID = curActivityInfo.ID;
				WaitResponseUIRootLogic.OpenWaitBox(207, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.enter_bar_fight>(request);
			}
		}
		else if (curActivityInfo.Type == 7)
		{
			enter_survive_batttle.request request2 = new enter_survive_batttle.request();
			request2.id = curActivityInfo.ID;
			request2.floor = 0L;
			WaitResponseUIRootLogic.OpenWaitBox(246, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.enter_survive_batttle>(request2);
		}
		else if (curActivityInfo.Type == 8)
		{
			SexMiniData sexMiniDataById = DataManager.GetSexMiniDataById(curActivityInfo.ID);
			string empty = string.Empty;
			empty = ((SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession != PROFESSION_TYPE.NQS) ? sexMiniDataById.WomenNpcId : sexMiniDataById.ManNpcId);
			SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
			MissionManager missionManager3 = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			missionManager3.AutoMoveDest(sexMiniDataById.MapId, DataManager.GetNPCPosInMonsterData(sexMiniDataById.MapId, empty), AUTO_SEARCH_PARTH_FINISHEVENT.FIND_NPC, empty);
		}
		else if (curActivityInfo.Type == 5)
		{
			enter_wild_boss.request request3 = new enter_wild_boss.request();
			request3.ID = curActivityInfo.ID;
			WaitResponseUIRootLogic.OpenWaitBox(201, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.enter_wild_boss>(request3);
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", $"activity_{curActivityInfo.Type}", "start");
	}

	public void OnClicktishiBtn()
	{
		if (string.IsNullOrEmpty(curRulestr) || curActivityInfo == null)
		{
			OnClickGuildtishiBtn();
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
		{
			PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			if (curActivityInfo.Type == 7)
			{
				SurviveBattleData surviveBattleDataById = DataManager.GetSurviveBattleDataById(curActivityInfo.ID);
				TimeSpan localShowTime = TimeTools.GetLocalShowTime(surviveBattleDataById.StartTimes, playerCommonData.TimeOffset);
				SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", curRulestr, null);
			}
			else
			{
				SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", curRulestr, null, TimeTools.GetLocalShowTime_HM(playerCommonData.ResetTime, playerCommonData.TimeOffset));
			}
		});
	}

	public void OnClickGuildtishiBtn()
	{
		if (mCurChoosedIndex == mGuildBattleIndex)
		{
			if (mCurGuildBattleData != null)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
				{
					PlayerCommonData playerCommonData3 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
					SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", mCurGuildBattleData.MRule, null);
				});
			}
		}
		else if (mCurChoosedIndex == GUILD_DANCE_INDEX)
		{
			if (mCurGuildDanceData != null)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
				{
					PlayerCommonData playerCommonData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
					SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", mCurGuildDanceData.Rule, null);
				});
			}
		}
		else if (mCurChoosedIndex == GuildCityIndex)
		{
			if (CurGuildCityData != null)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
				{
					Debug.Log(CurGuildCityData.Rule);
					PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
					SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", CurGuildCityData.Rule, null);
				});
			}
		}
		else if (curGuildBossData != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
			{
				SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", curGuildBossData.Rule, null);
			});
		}
	}

	public int GetActivityUnlockLevel(activity_info info)
	{
		int result = 0;
		long type = info.Type;
		if (type >= 1 && type <= 7)
		{
			switch (type - 1)
			{
			case 0L:
			{
				EscortData escortDataById2 = DataManager.GetEscortDataById(info.ID);
				result = escortDataById2.UnlockLevel;
				break;
			}
			case 1L:
			{
				EscortData escortDataById = DataManager.GetEscortDataById(info.ID);
				result = escortDataById.UnlockLevel;
				break;
			}
			case 2L:
			{
				CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(info.ID);
				result = cityDanceDataById.UnlockLevel;
				break;
			}
			case 3L:
			{
				BarFightCopyData barFightCopyDataByID = DataManager.GetBarFightCopyDataByID(info.ID);
				result = barFightCopyDataByID.UnlockLevel;
				break;
			}
			case 6L:
			{
				SurviveBattleData surviveBattleDataById = DataManager.GetSurviveBattleDataById(info.ID);
				result = surviveBattleDataById.UnlockLevel;
				break;
			}
			}
		}
		return result;
	}

	public void OnClickSexMiniRank()
	{
		SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerRankInfoRoot, delegate
		{
			SingletonUnity<PlayerRankInfoRootLogic>.Instance.ResetToSexMini();
		});
	}

	private void OnDisable()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.BAR_FIGHT_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.ROBBORY_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.WORLD_BOSS_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.SURVIVAL_BATTLE_CLICK_START)
		{
			CheckTutorialEvent();
		}
	}

	public void OnClickBossBtn(int index, int isenable)
	{
		if (mCurChoosedIndex == index && curActivityInfo != null && curActivityInfo.Type != 5)
		{
			return;
		}
		ShowActivityInfo();
		for (int i = 0; i < RankLabelObjList.Count; i++)
		{
			NGUITools.SetActive(RankLabelObjList[i], state: true);
		}
		mCurChoosedIndex = index;
		mCurChoosedSubIndex = -1;
		curActivityInfo = wildBossInfos[index];
		SelectShowINMap(curActivityInfo, isMapSelect: false);
		ShowRewardData showRewardData = null;
		string text = string.Empty;
		if (curActivityInfo.Type == 5)
		{
			curWildBossData = DataManager.GetWildBossDataByID(curActivityInfo.ID);
			showRewardData = DataManager.GetShowRewardDataByID(curWildBossData.ShowRewardID);
			text = StrDictionary.GetDictionaryString(curWildBossData.Desc);
			curRulestr = curWildBossData.Rule;
		}
		NGUITools.SetActive(RankBtnObj, state: false);
		NGUITools.SetActive(ChangeTimeBtn.gameObject, state: false);
		NGUITools.SetActive(signBtnSp.gameObject, state: false);
		DescLabel.text = text;
		if (curActivityInfo.Parmstr != null)
		{
			string[] array = curActivityInfo.Parmstr.Split('#');
			for (int j = 0; j < rankLabels.Count; j++)
			{
				if (j < array.Length)
				{
					if (!array[j].Equals(string.Empty))
					{
						string[] array2 = array[j].Split('@');
						rankLabels[j].text = $"{array2[1]}";
					}
					else
					{
						rankLabels[j].text = string.Format("{0}", "- - - -");
					}
				}
				else
				{
					rankLabels[j].text = string.Format("{0}", "- - - -");
				}
			}
		}
		else
		{
			for (int k = 0; k < rankLabels.Count; k++)
			{
				rankLabels[k].text = string.Format("{0}", "- - - -");
			}
		}
		NGUITools.SetActive(RewardObj.gameObject, state: true);
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: false);
		}
		if (isenable == 0)
		{
			startBtnSp.spriteName = "CZ_anNiu_2";
		}
		else
		{
			startBtnSp.spriteName = "CZ_anNiu_2+";
		}
		canStartFlag = isenable;
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBG.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if (curWildBossData != null && (CopyBG.mainTexture == null || !CopyBG.mainTexture.name.Equals(curWildBossData.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(curWildBossData.Background, TextureLoadFinish));
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.WORLD_BOSS_CHOOSE_COPY)
		{
			CheckTutorialEvent();
		}
	}

	public void SelectShowINMap(activity_info datainfo, bool isMapSelect)
	{
		if (isMapSelect)
		{
			return;
		}
		if (datainfo.Type == 1 || datainfo.Type == 2)
		{
			EscortData escortDataById = DataManager.GetEscortDataById(datainfo.ID);
			if (escortDataById != null)
			{
				bool flag = false;
				ActivityMapData activityMapDataById = DataManager.GetActivityMapDataById(escortDataById.ActivityMapId);
				if (activityMapDataById == null)
				{
					return;
				}
				if (activityMapDataById.MapId.Equals(SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID))
				{
					flag = true;
				}
				bool flag2 = false;
				flag2 = !flag;
				if (activityMapDataById != null)
				{
					if (flag2)
					{
						SingletonUnity<NewMapUIRootLogic>.Instance.Reset(activityMapDataById.MapId);
					}
					if (!activityMapDataById.IsNeedDailyActid())
					{
						SingletonUnity<NewMapUIRootLogic>.Instance.ChooseActivityObj(activityMapDataById.Type, activityMapDataById.SubType, string.Empty);
					}
					else
					{
						SingletonUnity<NewMapUIRootLogic>.Instance.ChooseActivityObj(activityMapDataById.Type, activityMapDataById.SubType, activityMapDataById.ActivityID);
					}
				}
			}
			else if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjNormal();
			}
			return;
		}
		List<ActivityMapData> activityMapDataByType = DataManager.GetActivityMapDataByType((int)datainfo.Type, ActivityMapData.DefaultSubType);
		if (activityMapDataByType == null)
		{
			return;
		}
		List<ActivityMapData> list = new List<ActivityMapData>();
		for (int i = 0; i < activityMapDataByType.Count; i++)
		{
			if (activityMapDataByType[i].IsVisible)
			{
				list.Add(activityMapDataByType[i]);
			}
		}
		if (list != null && list.Count > 0)
		{
			if (!SingletonUnity<NewMapUIRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
			{
				return;
			}
			bool flag3 = false;
			ActivityMapData activityMapData = null;
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].MapId.Equals(SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID))
				{
					flag3 = true;
					activityMapData = list[j];
					break;
				}
			}
			bool flag4 = false;
			if (flag3)
			{
				flag4 = false;
			}
			else
			{
				flag4 = true;
				if (!SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID.Equals(SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID))
				{
					bool flag5 = false;
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].MapId.Equals(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID))
						{
							flag5 = true;
							activityMapData = list[k];
							break;
						}
					}
					if (!flag5)
					{
						activityMapData = list[0];
					}
				}
				else
				{
					activityMapData = list[0];
				}
			}
			if (activityMapData != null)
			{
				if (flag4)
				{
					SingletonUnity<NewMapUIRootLogic>.Instance.Reset(activityMapData.MapId);
				}
				if (!activityMapData.IsNeedDailyActid())
				{
					SingletonUnity<NewMapUIRootLogic>.Instance.ChooseActivityObj(activityMapData.Type, activityMapData.SubType, string.Empty);
				}
				else
				{
					SingletonUnity<NewMapUIRootLogic>.Instance.ChooseActivityObj(activityMapData.Type, activityMapData.SubType, activityMapData.ActivityID);
				}
			}
		}
		else if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjNormal();
		}
	}

	public void SelectShowINMap(GameDefine.ACTIVITY_TYPE ActType, bool isMapSelect)
	{
		if (isMapSelect)
		{
			return;
		}
		List<ActivityMapData> activityMapDataByType = DataManager.GetActivityMapDataByType((int)ActType, ActivityMapData.DefaultSubType);
		if (activityMapDataByType == null)
		{
			return;
		}
		List<ActivityMapData> list = new List<ActivityMapData>();
		for (int i = 0; i < activityMapDataByType.Count; i++)
		{
			if (activityMapDataByType[i].IsVisible)
			{
				list.Add(activityMapDataByType[i]);
			}
		}
		if (list != null && list.Count > 0)
		{
			if (!SingletonUnity<NewMapUIRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
			{
				return;
			}
			bool flag = false;
			ActivityMapData activityMapData = null;
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].MapId.Equals(SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID))
				{
					flag = true;
					activityMapData = list[j];
					break;
				}
			}
			bool flag2 = false;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				flag2 = true;
				if (!SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID.Equals(SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID))
				{
					bool flag3 = false;
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].MapId.Equals(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID))
						{
							flag3 = true;
							activityMapData = list[k];
							break;
						}
					}
					if (!flag3)
					{
						activityMapData = list[0];
					}
				}
				else
				{
					activityMapData = list[0];
				}
			}
			if (activityMapData != null)
			{
				if (flag2)
				{
					SingletonUnity<NewMapUIRootLogic>.Instance.Reset(activityMapData.MapId);
				}
				if (!activityMapData.IsNeedDailyActid())
				{
					SingletonUnity<NewMapUIRootLogic>.Instance.ChooseActivityObj(activityMapData.Type, activityMapData.SubType, string.Empty);
				}
				else
				{
					SingletonUnity<NewMapUIRootLogic>.Instance.ChooseActivityObj(activityMapData.Type, activityMapData.SubType, activityMapData.ActivityID);
				}
			}
		}
		else if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjNormal();
		}
	}

	public void OnClickGuildBossBtn(int index, int subIndex, int isenable)
	{
		if (mCurChoosedIndex == index && mCurChoosedSubIndex == subIndex)
		{
			return;
		}
		curActivityInfo = null;
		mCurChoosedIndex = index;
		mCurChoosedSubIndex = subIndex;
		ShowActivityInfo();
		for (int i = 0; i < RankLabelObjList.Count; i++)
		{
			NGUITools.SetActive(RankLabelObjList[i], state: true);
		}
		NGUITools.SetActive(RankBtnObj, state: false);
		NGUITools.SetActive(ChangeTimeBtn.gameObject, state: false);
		NGUITools.SetActive(signBtnSp.gameObject, state: false);
		curBossInfo = guildBossInfos[mCurChoosedSubIndex];
		SelectShowINMap(GameDefine.ACTIVITY_TYPE.GUILD_BOSS, isMapSelect: false);
		ShowRewardData showRewardData = null;
		string empty = string.Empty;
		curGuildBossData = DataManager.GetGuildBossDataByID(curBossInfo.id);
		int id = 2;
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			id = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.GuilLevel;
		}
		if (DataManager.GetAdaptDataByID(id).DorpKeyDic.ContainsKey(curGuildBossData.ShowRewardID))
		{
			string id2 = DataManager.GetAdaptDataByID(id).DorpKeyDic[curGuildBossData.ShowRewardID];
			showRewardData = DataManager.GetShowRewardDataByID(id2);
		}
		empty = StrDictionary.GetDictionaryString(curGuildBossData.Desc);
		DescLabel.text = empty;
		for (int j = 0; j < rankLabels.Count; j++)
		{
			if (curBossInfo.HasSort_item && j < curBossInfo.sort_item.Count)
			{
				rankLabels[j].text = $"{curBossInfo.sort_item[j].name}";
			}
			else
			{
				rankLabels[j].text = string.Format("{0}", "- - - -");
			}
		}
		NGUITools.SetActive(RewardObj.gameObject, state: true);
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: false);
		}
		if (curBossInfo.state == 0L)
		{
			startBtnSp.spriteName = "CZ_anNiu_2+";
			canStartFlag = 2;
		}
		else if (curBossInfo.state == 1)
		{
			startBtnSp.spriteName = "CZ_anNiu_2";
			canStartFlag = 0;
		}
		else if (curBossInfo.state == 2)
		{
			startBtnSp.spriteName = "CZ_anNiu_2+";
			canStartFlag = 3;
		}
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			startBtnSp.spriteName = "CZ_anNiu_2+";
		}
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBG.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if (curGuildBossData != null && (CopyBG.mainTexture == null || !CopyBG.mainTexture.name.Equals(curGuildBossData.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(curGuildBossData.Background, TextureLoadFinish));
		}
	}

	public void OnClickGuildBattle(int index, int subIndex, int isenable)
	{
		if (mCurChoosedIndex == index && mCurChoosedSubIndex == subIndex)
		{
			return;
		}
		curActivityInfo = null;
		mCurChoosedIndex = index;
		mCurChoosedSubIndex = subIndex;
		ShowActivityInfo();
		for (int i = 0; i < RankLabelObjList.Count; i++)
		{
			NGUITools.SetActive(RankLabelObjList[i], state: false);
		}
		SelectShowINMap(GameDefine.ACTIVITY_TYPE.GUILD_BATTLE, isMapSelect: false);
		NGUITools.SetActive(RankBtnObj, state: false);
		NGUITools.SetActive(ChangeTimeBtn.gameObject, state: false);
		NGUITools.SetActive(signBtnSp.gameObject, state: false);
		ShowRewardData showRewardData = null;
		string empty = string.Empty;
		NGUITools.SetActive(startBtnSp.gameObject, state: true);
		if (GuildBattleInfo != null && GuildBattleInfo.state == -2)
		{
			startBtnSp.spriteName = GameDefine.BtnIcon[2];
		}
		else
		{
			startBtnSp.spriteName = GameDefine.BtnIcon[1];
		}
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			startBtnSp.spriteName = GameDefine.BtnIcon[2];
		}
		showRewardData = DataManager.GetShowRewardDataByID(mCurGuildBattleData.ShowRewardID);
		empty = mCurGuildBattleData.MDesc;
		DescLabel.text = empty;
		for (int j = 0; j < rankLabels.Count; j++)
		{
			rankLabels[j].text = string.Empty;
		}
		NGUITools.SetActive(RewardObj.gameObject, state: true);
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: false);
		}
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBG.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if (mCurGuildBattleData != null && (CopyBG.mainTexture == null || !CopyBG.mainTexture.name.Equals(mCurGuildBattleData.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(mCurGuildBattleData.Background, TextureLoadFinish));
		}
		canStartFlag = 0;
		if (TutorialManager.CurStep == TUTORIAL_STEP.GUILD_BOSS_CLICK_START)
		{
			FunctionTipsRootLogic.ClearHandTip();
			TutorialManager.CloseTutorial();
		}
	}

	public void OnClickGuildActivityStartBtn()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			NoticeLogic.AddNotifyData("#{102006}");
		}
		else if (mCurChoosedIndex == mGuildBattleIndex)
		{
			if (GuildBattleInfo != null && GuildBattleInfo.state == -2)
			{
				NoticeLogic.AddNotifyData("#{105077}");
				return;
			}
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewDailyActivityUIRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMapUIRootLogic);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildBattleRoot, delegate
			{
				SingletonUnity<GuildBattleRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(285, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.req_guild_battle_info>();
			});
		}
		else if (mCurChoosedIndex == GUILD_DANCE_INDEX)
		{
			if (canStartFlag != 0)
			{
				switch (canStartFlag)
				{
				case 1:
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
					break;
				case 3:
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{102061}"));
					break;
				case 2:
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101541}"));
					break;
				}
			}
			else
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
				SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AutoMoveDest(mCurGuildDanceData.MapId, Vector3.right * -5f, AUTO_SEARCH_PARTH_FINISHEVENT.CITY_DANCE);
			}
		}
		else if (mCurChoosedIndex == GuildCityIndex)
		{
			if (canStartFlag != 0)
			{
				switch (canStartFlag)
				{
				case 1:
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
					break;
				case 3:
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{102061}"));
					break;
				case 2:
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101541}"));
					break;
				}
			}
			else
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.WorldMapRoot, delegate
				{
					WaitResponseUIRootLogic.OpenWaitBox(319, 10f, 0f);
					NetLogic.GetInstance().Send<Protocol.request_guild_map_info>();
					SingletonUnity<WorldMapRoot>.Instance.EnableReset(isact: true);
				});
			}
		}
		else
		{
			if (curBossInfo == null)
			{
				return;
			}
			if (TutorialManager.CurStep == TUTORIAL_STEP.GUILD_BOSS_CLICK_START)
			{
				CheckTutorialEvent();
			}
			if (canStartFlag != 0)
			{
				switch (canStartFlag)
				{
				case 1:
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
					break;
				case 3:
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{102061}"));
					break;
				case 2:
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101541}"));
					break;
				}
			}
			else
			{
				enter_guild_boss_scene.request request = new enter_guild_boss_scene.request();
				request.id = curBossInfo.id;
				WaitResponseUIRootLogic.OpenWaitBox(194, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.enter_guild_boss_scene>(request);
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("guildboss", $"guildboss_{curBossInfo.id}", "starttimes");
			}
		}
	}

	public void OnClickGuildDance(int index, int subIndex, int isenable)
	{
		if (mCurChoosedIndex == index && mCurChoosedSubIndex == subIndex)
		{
			return;
		}
		curActivityInfo = null;
		mCurChoosedIndex = index;
		mCurChoosedSubIndex = subIndex;
		IsShowGuildDanceFlag = true;
		ShowActivityInfo();
		for (int i = 0; i < RankLabelObjList.Count; i++)
		{
			NGUITools.SetActive(RankLabelObjList[i], state: false);
		}
		SelectShowINMap(GameDefine.ACTIVITY_TYPE.GUILD_DANCE, isMapSelect: false);
		NGUITools.SetActive(RankBtnObj, state: false);
		NGUITools.SetActive(signBtnSp.gameObject, state: false);
		ShowRewardData showRewardData = null;
		string empty = string.Empty;
		if (GuildDanceInfo.state != 1)
		{
			canStartFlag = 2;
			NGUITools.SetActive(startBtnSp.gameObject, state: false);
			NGUITools.SetActive(ChangeTimeBtn.gameObject, state: true);
			startBtnSp.spriteName = GameDefine.BtnIcon[2];
		}
		else
		{
			canStartFlag = 0;
			NGUITools.SetActive(startBtnSp.gameObject, state: true);
			NGUITools.SetActive(ChangeTimeBtn.gameObject, state: false);
			startBtnSp.spriteName = GameDefine.BtnIcon[1];
		}
		if (GuildDanceInfo.HasReset_time)
		{
			ReamainTime = GuildDanceInfo.reset_time - SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime();
		}
		else
		{
			ReamainTime = -1L;
		}
		if (ReamainTime > 0)
		{
			NextTimeLabel.text = StrDictionary.GetDictionaryString("#{105095}", TimeTools.GetFullTime(ReamainTime));
			ChangeTimeBtn.spriteName = GameDefine.BtnIcon[2];
			NextTimeName.enabled = true;
		}
		else
		{
			NextTimeLabel.text = string.Empty;
			ChangeTimeBtn.spriteName = GameDefine.BtnIcon[1];
			NextTimeName.enabled = false;
		}
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			startBtnSp.spriteName = GameDefine.BtnIcon[2];
		}
		showRewardData = DataManager.GetShowRewardDataByID(mCurGuildDanceData.ShowRewardId);
		empty = StrDictionary.GetDictionaryString(mCurGuildDanceData.Description);
		DescLabel.text = empty;
		for (int j = 0; j < rankLabels.Count; j++)
		{
			rankLabels[j].text = string.Empty;
		}
		NGUITools.SetActive(RewardObj.gameObject, state: true);
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: false);
		}
		UpdateSelectItem();
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBG.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if (mCurGuildDanceData != null && (CopyBG.mainTexture == null || !CopyBG.mainTexture.name.Equals(mCurGuildDanceData.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(mCurGuildDanceData.Background, TextureLoadFinish));
		}
	}

	public void OnClickChangeTimeBtn()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			NoticeLogic.AddNotifyData("#{102006}");
			return;
		}
		if (GuildDanceInfo.state == 1)
		{
			NoticeLogic.AddNotifyData("#{105098}");
			return;
		}
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.isGuildChief())
		{
			NoticeLogic.AddNotifyData("#{105097}");
			return;
		}
		if (ReamainTime > 0)
		{
			NoticeLogic.AddNotifyData("#{105096}");
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ChangeTimeRoot, delegate
		{
			SingletonUnity<ChangeTimeRootLogic>.Instance.ResetGuildDance(mCurGuildDanceData.StartTimes, (int)GuildDanceInfo.parm - 1, StrDictionary.GetDictionaryString(mCurGuildDanceData.Name), mCurGuildDanceData.DurationTime);
		});
	}

	private void Update()
	{
		if (ReamainTime > 0)
		{
			tempCountTime += Time.deltaTime;
			if (tempCountTime >= 1f)
			{
				ReamainTime--;
				tempCountTime -= 1f;
				NextTimeLabel.text = StrDictionary.GetDictionaryString("#{105095}", TimeTools.GetFullTime(ReamainTime));
			}
			if (ReamainTime <= 0)
			{
				tempCountTime = 0f;
				NextTimeLabel.text = string.Empty;
				ChangeTimeBtn.spriteName = GameDefine.BtnIcon[1];
				NextTimeName.enabled = false;
			}
		}
	}

	public void UpdateGuildBossInfo()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.ActivityData.CurGuildBossDataDic.Count > 0)
		{
			guildBossInfos = new List<guild_boss>(playerData.ActivityData.CurGuildBossDataDic.Values);
			guildBossInfos.Sort((guild_boss x, guild_boss y) => int.Parse(x.id) - int.Parse(y.id));
		}
		if (playerData.ActivityData.GuildBattleInfo != null)
		{
			GuildBattleInfo = playerData.ActivityData.GuildBattleInfo;
			mCurGuildBattleData = DataManager.GetGuildBattleDataById(GuildBattleInfo.ID);
		}
		if (playerData.ActivityData.GuildDanceInfo != null)
		{
			GuildDanceInfo = playerData.ActivityData.GuildDanceInfo;
			mCurGuildDanceData = DataManager.GetCityDanceDataById(GuildDanceInfo.ID);
		}
		if (playerData.ActivityData.CurGuildCityDataDic != null)
		{
			GuildCityList = new List<guild_map_info>(playerData.ActivityData.CurGuildCityDataDic.Values);
			isOpenGuildCity = false;
			if (GuildCityList != null && GuildCityList.Count > 0)
			{
				for (int i = 0; i < GuildCityList.Count; i++)
				{
					if (GuildCityList[i].state == 1)
					{
						isOpenGuildCity = true;
						break;
					}
				}
			}
			CurGuildCityData = DataManager.GetGuildCaptureDataByID(GuildCityList[0].id);
		}
		if (curActivityInfo == null && mCurChoosedIndex == GUILD_DANCE_INDEX)
		{
			if (GuildDanceInfo.state != 1)
			{
				canStartFlag = 2;
				NGUITools.SetActive(startBtnSp.gameObject, state: false);
				NGUITools.SetActive(ChangeTimeBtn.gameObject, state: true);
				startBtnSp.spriteName = GameDefine.BtnIcon[2];
			}
			else
			{
				canStartFlag = 0;
				NGUITools.SetActive(startBtnSp.gameObject, state: true);
				NGUITools.SetActive(ChangeTimeBtn.gameObject, state: false);
				startBtnSp.spriteName = GameDefine.BtnIcon[1];
			}
			if (GuildDanceInfo.HasReset_time)
			{
				ReamainTime = GuildDanceInfo.reset_time - SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime();
			}
			else
			{
				ReamainTime = -1L;
			}
			if (ReamainTime > 0)
			{
				NextTimeLabel.text = StrDictionary.GetDictionaryString("#{105095}", TimeTools.GetFullTime(ReamainTime));
				ChangeTimeBtn.spriteName = GameDefine.BtnIcon[2];
				NextTimeName.enabled = true;
			}
			else
			{
				NextTimeLabel.text = string.Empty;
				ChangeTimeBtn.spriteName = GameDefine.BtnIcon[1];
				NextTimeName.enabled = false;
			}
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				startBtnSp.spriteName = GameDefine.BtnIcon[2];
			}
		}
	}
}
