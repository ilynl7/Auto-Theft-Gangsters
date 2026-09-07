using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DailyActivityUIRootLogic : SingletonUnity<DailyActivityUIRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public int LineCount = 8;

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

	public int TargetTypeId;

	public GameObject ScoreObj;

	public UILabel scorelabel;

	public List<WildBossLineLogic> WildBossLineList = new List<WildBossLineLogic>();

	private List<activity_info> wildBossInfos = new List<activity_info>();

	public UITable RootTable;

	public List<UILabel> rankLabels;

	public List<GameObject> RankLabelObjList;

	private List<DailyActiveData> DailyActiveDataList = new List<DailyActiveData>();

	private Dictionary<string, daily_active> activeDic = new Dictionary<string, daily_active>();

	public UIScrollBar ItemRootBar;

	private WildBossData curWildBossData;

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

	public void UpdataActiveInfo()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		activeDic = playerData.welfareData.DailyActives;
		NGUITools.SetActive(ScoreObj, state: false);
		if (curActivityInfo == null)
		{
			return;
		}
		long type = curActivityInfo.Type;
		if (type >= 1 && type <= 5)
		{
			switch (type - 1)
			{
			case 4L:
				ShowScore(6);
				break;
			case 1L:
				ShowScore(4);
				break;
			case 0L:
				ShowScore(14);
				break;
			case 2L:
			case 3L:
				break;
			}
		}
	}

	private void ShowScore(int type)
	{
		for (int i = 0; i < DailyActiveDataList.Count; i++)
		{
			if (DailyActiveDataList[i].Type == type)
			{
				NGUITools.SetActive(ScoreObj, state: true);
				int num = 0;
				if (activeDic.ContainsKey(DailyActiveDataList[i].ID) && activeDic[DailyActiveDataList[i].ID].HasCount)
				{
					num = (int)activeDic[DailyActiveDataList[i].ID].count * DailyActiveDataList[i].Score;
				}
				int num2 = DailyActiveDataList[i].Score * DailyActiveDataList[i].Count;
				if (num <= num2)
				{
					scorelabel.text = $"{num}/{num2}";
				}
				else
				{
					scorelabel.text = $"{num2}/{num2}";
				}
				break;
			}
		}
	}

	public void EnableReset()
	{
		for (int i = 0; i < ActivityItems.Count; i++)
		{
			NGUITools.SetActive(ActivityItems[i].gameObject, state: false);
		}
		for (int j = 0; j < WildBossLineList.Count; j++)
		{
			NGUITools.SetActive(WildBossLineList[j].gameObject, state: false);
			NGUITools.SetActive(WildBossLineList[j].Sublineobj, state: false);
		}
		TargetTypeId = -1;
		NGUITools.SetActive(RankBtnObj, state: false);
		NGUITools.SetActive(signBtnSp.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: false);
		curActivityInfo = null;
		curRulestr = string.Empty;
		DailyActiveDataList = DataManager.GetDailyActiveDataList();
	}

	public void RefreshActivityPage(ret_request_activity_info.request request)
	{
		activityList = new List<activity_info>(request.activity_info.Values);
		activityList.Sort((activity_info x, activity_info y) => GetActivityUnlockLevel(x) - GetActivityUnlockLevel(y));
		int num = Mathf.Min(activityList.Count, LineCount) - ActivityItems.Count;
		int count = ActivityItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(ActivityItems[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{count + i}";
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
		int index = 0;
		for (int j = 0; j < ActivityItems.Count; j++)
		{
			NGUITools.SetActive(ActivityItems[j].gameObject, j < activityList.Count);
			if (j < activityList.Count)
			{
				ActivityItems[j].onClickItem = OnClickItemBtn;
				ResetItemLine(ActivityItems[j], j, mCurChoosedIndex);
				if (activityList[j].Type == TargetTypeId)
				{
					index = j;
				}
			}
		}
		RootTable.Reposition();
		uiScrollView.ResetPosition();
		mCurChoosedIndex = -1;
		ActivityItems[index].OnClikcItemBtn();
		if (wildBossInfos == null || wildBossInfos.Count == 0)
		{
			for (int k = 0; k < WildBossLineList.Count; k++)
			{
				NGUITools.SetActive(WildBossLineList[k].gameObject, state: false);
				NGUITools.SetActive(WildBossLineList[k].Sublineobj, state: false);
			}
		}
	}

	public void RefershBossInfo(ret_request_wild_boss_info.request request)
	{
		wildBossInfos = new List<activity_info>(request.activity_info.Values);
		wildBossInfos.Sort((activity_info x, activity_info y) => int.Parse(x.ID) - int.Parse(y.ID));
		int num = 1;
		int num2 = num - WildBossLineList.Count;
		int num3 = WildBossLineList.Count + ActivityItems.Count;
		WildBossLineList[0].gameObject.name = $"huaDongTiao_{ActivityItems.Count}";
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(WildBossLineList[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{num3 + i}";
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
		if (TargetTypeId == 5)
		{
			WildBossLineList[0].OnClickItemBtn();
		}
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
		if (curActivityInfo.Type != 5)
		{
			for (int i = 0; i < ActivityItems.Count; i++)
			{
				ActivityItems[i].RefreshSelect(mCurChoosedIndex);
			}
			for (int j = 0; j < WildBossLineList.Count; j++)
			{
				WildBossLineList[j].RefreshSelect(-1);
			}
		}
		else
		{
			for (int k = 0; k < ActivityItems.Count; k++)
			{
				ActivityItems[k].RefreshSelect(-1);
			}
			for (int l = 0; l < WildBossLineList.Count; l++)
			{
				WildBossLineList[l].RefreshSelect(mCurChoosedIndex);
			}
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
			else if (TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_CHOOSE_COPY && (curActivityInfo.Type == 1 || curActivityInfo.Type == 2))
			{
				CheckTutorialEvent();
			}
			return;
		}
		for (int i = 0; i < RankLabelObjList.Count; i++)
		{
			NGUITools.SetActive(RankLabelObjList[i], state: false);
		}
		unLockLevel = unlock;
		mCurChoosedIndex = realIdx;
		activity_info activity_info = activityList[realIdx];
		curActivityInfo = activity_info;
		if (TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_CHOOSE_COPY && curActivityInfo.Type == 2)
		{
			CheckTutorialEvent();
			return;
		}
		remainNum = (int)curActivityInfo.CurNum;
		ShowRewardData showRewardData = null;
		string text = string.Empty;
		curRulestr = string.Empty;
		string value = string.Empty;
		NGUITools.SetActive(RankBtnObj, state: false);
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
			string id4 = DataManager.GetAdaptDataByID(SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.ServerLevel).DorpKeyDic[surviveBattleDataById.ShowRewardID];
			showRewardData = DataManager.GetShowRewardDataByID(id4);
			text = StrDictionary.GetDictionaryString(surviveBattleDataById.Desc);
			curRulestr = surviveBattleDataById.Rule;
			value = surviveBattleDataById.Background;
		}
		else if (curActivityInfo.Type == 8)
		{
			SexMiniData sexMiniDataById = DataManager.GetSexMiniDataById(curActivityInfo.ID);
			string showRewardID = sexMiniDataById.ShowRewardID;
			showRewardData = DataManager.GetShowRewardDataByID(showRewardID);
			text = StrDictionary.GetDictionaryString(sexMiniDataById.Description);
			curRulestr = sexMiniDataById.Rule;
			value = sexMiniDataById.Background;
			NGUITools.SetActive(RankBtnObj, state: true);
		}
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
		UpdataActiveInfo();
		UpdateSelectItem();
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
			if (curActivityInfo.Type == 1 || curActivityInfo.Type == 2)
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
		if (canStartFlag == 1)
		{
			TutorialManager.LevelLimitAction();
		}
		else if (curActivityInfo.State == 1)
		{
			if (curActivityInfo.HasSign && curActivityInfo.sign == 1)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{102056}"));
				return;
			}
			sign_bar_fight.request request = new sign_bar_fight.request();
			request.ID = curActivityInfo.ID;
			NetLogic.GetInstance().Send<Protocol.sign_bar_fight>(request);
			curActivityInfo.sign = 1L;
			signBtnSp.spriteName = "CZ_anNiu_2+";
			signlabel.text = StrDictionary.GetDictionaryString("#{102055}");
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", $"activity_{curActivityInfo.Type}", "entoll");
		}
		else
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{102051}"));
		}
	}

	public void OnClickStart()
	{
		if (curActivityInfo == null)
		{
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
			SingletonUnity<ActivityUIRootLogic>.Instance.OnClickCloseBtn();
			MissionData missionDataByID = DataManager.GetMissionDataByID(curActivityInfo.ID);
			MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			missionManager.MissionFindPath(missionDataByID);
		}
		else if (curActivityInfo.Type == 2)
		{
			SingletonUnity<ActivityUIRootLogic>.Instance.OnClickCloseBtn();
			MissionData missionDataByID2 = DataManager.GetMissionDataByID(curActivityInfo.ID);
			MissionManager missionManager2 = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			missionManager2.MissionFindPath(missionDataByID2);
		}
		else if (curActivityInfo.Type == 3)
		{
			SingletonUnity<ActivityUIRootLogic>.Instance.OnClickCloseBtn();
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
			SingletonUnity<ActivityUIRootLogic>.Instance.OnClickCloseBtn();
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
		if (string.IsNullOrEmpty(curRulestr))
		{
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
		SingletonUnity<ActivityUIRootLogic>.Instance.OnClickCloseBtn();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerRankInfoRoot, delegate
		{
			SingletonUnity<PlayerRankInfoRootLogic>.Instance.ResetToSexMini();
		});
	}

	private void OnEnable()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DailyRewardNewRoot, delegate
		{
			SingletonUnity<DailyRewardNewLogic>.Instance.EnableReset();
			NetLogic.GetInstance().Send<Protocol.request_daily_active>();
		});
	}

	private void OnDisable()
	{
		if (SingletonUnity<UIManager>.Exists)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyRewardNewRoot);
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.BAR_FIGHT_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.ROBBORY_CLICK_START)
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
		for (int i = 0; i < RankLabelObjList.Count; i++)
		{
			NGUITools.SetActive(RankLabelObjList[i], state: true);
		}
		mCurChoosedIndex = index;
		curActivityInfo = wildBossInfos[index];
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
		if (showRewardData != null)
		{
			NGUITools.SetActive(ShowRewardItemsScripts.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			NGUITools.SetActive(ShowRewardItemsScripts.gameObject, state: false);
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
		UpdataActiveInfo();
		UpdateSelectItem();
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
	}
}
