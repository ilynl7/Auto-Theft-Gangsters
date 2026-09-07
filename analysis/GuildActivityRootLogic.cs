using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildActivityRootLogic : SingletonUnity<GuildActivityRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UIScrollView uiScrollView;

	public List<GuildBossLineLogic> guildBossLines = new List<GuildBossLineLogic>();

	private List<guild_boss> guildBossInfos = new List<guild_boss>();

	private guild_boss curBossInfo;

	public ShowRewardItems ShowRewardItemScripts;

	private GuildBattleData mCurGuildBattleData;

	private CityDanceData mCurGuildDanceData;

	public UILabel DescLabel;

	public UILabel NameLabel;

	public UITable RootTable;

	private int mGuildBattleIndex = -1;

	private int GUILD_BATTLE_INDEX;

	private int GUILD_DANCE_INDEX = 2;

	private int mCurChoosedIndex = -1;

	private int mCurChoosedSubIndex = -1;

	public UISprite startBtnSp;

	public UISprite ChangeTimeBtn;

	private long ReamainTime;

	public UILabel NextTimeLabel;

	public UILabel NextTimeName;

	private int canStartFlag;

	private int canOpenFlag;

	public List<UILabel> rankLabels;

	public GameObject RankRootObj;

	public GameObject RewardObj;

	private int sublineisEnable;

	private GuildBossData curBossData;

	public UITexture CopyBG;

	public int TargetTypeId;

	private guild_battle_info GuildBattleInfo;

	private dance_state_info GuildDanceInfo;

	private List<guild_map_info> GuildCityList;

	private GuildCaptureData CurGuildCityData;

	private bool isOpenGuildCity;

	private int GuildCityIndex;

	private float tempCountTime;

	public int CurChoosedIndex => mCurChoosedIndex;

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

	public void EnableReset()
	{
		for (int i = 0; i < guildBossLines.Count; i++)
		{
			NGUITools.SetActive(guildBossLines[i].gameObject, state: false);
		}
		NGUITools.SetActive(ShowRewardItemScripts.gameObject, state: false);
		curBossInfo = null;
		curBossData = null;
		GuildBattleInfo = null;
		TargetTypeId = -1;
		mCurChoosedIndex = -1;
	}

	public void RefershBossInfo(ret_request_guild_boss.request request)
	{
		if (request.HasGuild_boss)
		{
			guildBossInfos = new List<guild_boss>(request.guild_boss.Values);
			guildBossInfos.Sort((guild_boss x, guild_boss y) => int.Parse(x.id) - int.Parse(y.id));
		}
		int num = Mathf.Min(guildBossInfos.Count, 1);
		if (request.HasGuild_battle_info)
		{
			num++;
			mGuildBattleIndex = GUILD_BATTLE_INDEX;
		}
		else
		{
			mGuildBattleIndex = -1;
		}
		if (request.HasDance_state_info)
		{
			num++;
		}
		if (request.HasGuild_map_info)
		{
			num++;
		}
		if (!request.HasGuild_boss)
		{
			for (int i = 0; i < guildBossLines.Count; i++)
			{
				NGUITools.SetActive(guildBossLines[i].gameObject, state: false);
			}
		}
		int num2 = num - guildBossLines.Count;
		int count = guildBossLines.Count;
		if (num2 > 0)
		{
			for (int j = 0; j < num2; j++)
			{
				GameObject gameObject = Object.Instantiate(guildBossLines[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{count + j:D2}";
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
		for (int k = 0; k < guildBossLines.Count; k++)
		{
			if (k < num)
			{
				NGUITools.SetActive(guildBossLines[k].gameObject, state: true);
			}
			else
			{
				NGUITools.SetActive(guildBossLines[k].gameObject, state: false);
			}
		}
		int index = 0;
		if (request.HasGuild_battle_info)
		{
			for (int l = 1; l < num; l++)
			{
				guildBossLines[l].ResetItem(guildBossInfos, OnClickItemBtn, l);
			}
			GuildBattleInfo = request.guild_battle_info;
			mCurGuildBattleData = DataManager.GetGuildBattleDataById(GuildBattleInfo.ID);
			guildBossLines[mGuildBattleIndex].ResetItem(mCurGuildBattleData.ID, OnClickGuildBattle, mGuildBattleIndex, GuildBattleInfo);
			if (TargetTypeId == 9)
			{
				index = mGuildBattleIndex;
				TargetTypeId = 0;
			}
			else if (TargetTypeId == 6 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				index = 1;
				TargetTypeId = 0;
			}
		}
		else
		{
			for (int m = 0; m < num; m++)
			{
				guildBossLines[m].ResetItem(guildBossInfos, OnClickItemBtn, m);
			}
		}
		if (request.HasDance_state_info)
		{
			GuildDanceInfo = request.dance_state_info;
			mCurGuildDanceData = DataManager.GetCityDanceDataById(GuildDanceInfo.ID);
			guildBossLines[GUILD_DANCE_INDEX].ResetItem(OnClickGuildDance, GUILD_DANCE_INDEX, GuildDanceInfo);
			if (TargetTypeId == 16)
			{
				index = GUILD_DANCE_INDEX;
				TargetTypeId = 0;
			}
		}
		if (request.HasGuild_map_info)
		{
			GuildCityList = new List<guild_map_info>(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.CurGuildCityDataDic.Values);
			isOpenGuildCity = false;
			if (GuildCityList != null && GuildCityList.Count > 0)
			{
				for (int n = 0; n < GuildCityList.Count; n++)
				{
					if (GuildCityList[n].state == 1)
					{
						isOpenGuildCity = true;
						break;
					}
				}
				GuildCityIndex = num - 1;
				guildBossLines[GuildCityIndex].ResetItem(OnClickGuildCity, GuildCityIndex, GuildCityList);
				if (TargetTypeId == 18)
				{
					index = GuildCityIndex;
					TargetTypeId = 0;
				}
			}
		}
		mCurChoosedIndex = -1;
		RootTable.Reposition();
		uiScrollView.ResetPosition();
		if (num > 0)
		{
			guildBossLines[index].OnClickItemBtn();
		}
	}

	public void OnClickChangeTimeBtn()
	{
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

	public void OnClickGuildDance(int index, int subIndex, int isenable)
	{
		mCurChoosedIndex = index;
		mCurChoosedSubIndex = subIndex;
		ShowRewardData showRewardData = null;
		string empty = string.Empty;
		string empty2 = string.Empty;
		NGUITools.SetActive(RankRootObj, state: false);
		NGUITools.SetActive(RewardObj, state: true);
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
		empty2 = StrDictionary.GetDictionaryString(mCurGuildDanceData.Name);
		DescLabel.text = empty;
		NameLabel.text = empty2;
		for (int i = 0; i < rankLabels.Count; i++)
		{
			rankLabels[i].text = string.Empty;
		}
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemScripts.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemScripts.gameObject, state: false);
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

	public void OnClickGuildCity(int index, int subIndex, int isenable)
	{
		mCurChoosedIndex = index;
		mCurChoosedSubIndex = subIndex;
		ShowRewardData showRewardData = null;
		string empty = string.Empty;
		string empty2 = string.Empty;
		NGUITools.SetActive(RankRootObj, state: false);
		NGUITools.SetActive(RewardObj, state: false);
		NGUITools.SetActive(startBtnSp.gameObject, state: true);
		NGUITools.SetActive(ChangeTimeBtn.gameObject, state: false);
		CurGuildCityData = DataManager.GetGuildCaptureDataByID(GuildCityList[0].id);
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
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			startBtnSp.spriteName = GameDefine.BtnIcon[2];
		}
		empty = StrDictionary.GetDictionaryString(CurGuildCityData.Description);
		empty2 = StrDictionary.GetDictionaryString(CurGuildCityData.Name);
		DescLabel.text = empty;
		NameLabel.text = empty2;
		for (int i = 0; i < rankLabels.Count; i++)
		{
			rankLabels[i].text = string.Empty;
		}
		UnityVersionUtil.SetActiveRecursive(ShowRewardItemScripts.gameObject, state: false);
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

	public void UpdateGuildBossInfo(ret_request_guild_boss.request request)
	{
		if (request.HasGuild_boss)
		{
			guildBossInfos = new List<guild_boss>(request.guild_boss.Values);
			guildBossInfos.Sort((guild_boss x, guild_boss y) => int.Parse(x.id) - int.Parse(y.id));
		}
		if (request.HasGuild_battle_info)
		{
			GuildBattleInfo = request.guild_battle_info;
		}
		if (request.HasDance_state_info)
		{
			GuildDanceInfo = request.dance_state_info;
			mCurGuildDanceData = DataManager.GetCityDanceDataById(GuildDanceInfo.ID);
			guildBossLines[GUILD_DANCE_INDEX].ResetItem(OnClickGuildDance, GUILD_DANCE_INDEX, GuildDanceInfo);
		}
		if (mCurChoosedIndex == GUILD_DANCE_INDEX)
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

	public void OnClickGuildBattle(int index, int subIndex, int isenable)
	{
		mCurChoosedIndex = index;
		mCurChoosedSubIndex = subIndex;
		ShowRewardData showRewardData = null;
		string empty = string.Empty;
		string empty2 = string.Empty;
		NGUITools.SetActive(RankRootObj, state: false);
		NGUITools.SetActive(ChangeTimeBtn.gameObject, state: false);
		NGUITools.SetActive(startBtnSp.gameObject, state: true);
		NGUITools.SetActive(RewardObj, state: true);
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
		empty2 = mCurGuildBattleData.MName;
		DescLabel.text = empty;
		NameLabel.text = empty2;
		for (int i = 0; i < rankLabels.Count; i++)
		{
			rankLabels[i].text = string.Empty;
		}
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemScripts.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemScripts.gameObject, state: false);
		}
		UpdateSelectItem();
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

	public void OnClickItemBtn(int index, int subIndex, int isenable)
	{
		if (mCurChoosedIndex == index && mCurChoosedSubIndex == subIndex)
		{
			return;
		}
		mCurChoosedIndex = index;
		mCurChoosedSubIndex = subIndex;
		sublineisEnable = isenable;
		NGUITools.SetActive(RankRootObj, state: true);
		NGUITools.SetActive(ChangeTimeBtn.gameObject, state: false);
		NGUITools.SetActive(RewardObj, state: true);
		curBossInfo = guildBossInfos[mCurChoosedSubIndex];
		ShowRewardData showRewardData = null;
		string empty = string.Empty;
		string empty2 = string.Empty;
		curBossData = DataManager.GetGuildBossDataByID(curBossInfo.id);
		int id = 2;
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			id = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.GuilLevel;
		}
		if (DataManager.GetAdaptDataByID(id).DorpKeyDic.ContainsKey(curBossData.ShowRewardID))
		{
			string id2 = DataManager.GetAdaptDataByID(id).DorpKeyDic[curBossData.ShowRewardID];
			showRewardData = DataManager.GetShowRewardDataByID(id2);
		}
		empty = StrDictionary.GetDictionaryString(curBossData.Desc);
		empty2 = StrDictionary.GetDictionaryString("#{101534}");
		DescLabel.text = empty;
		NameLabel.text = empty2;
		for (int i = 0; i < rankLabels.Count; i++)
		{
			if (curBossInfo.HasSort_item && i < curBossInfo.sort_item.Count)
			{
				rankLabels[i].text = $"{curBossInfo.sort_item[i].name}";
			}
			else
			{
				rankLabels[i].text = string.Format("{0}", "- - - -");
			}
		}
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemScripts.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemScripts.gameObject, state: false);
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
		UpdateSelectItem();
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBG.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if (curBossData != null && (CopyBG.mainTexture == null || !CopyBG.mainTexture.name.Equals(curBossData.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(curBossData.Background, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(string name, Texture tex)
	{
		CopyBG.mainTexture = tex;
	}

	public void UpdateSelectItem()
	{
		for (int i = 0; i < guildBossLines.Count; i++)
		{
			guildBossLines[i].RefreshSelect(mCurChoosedIndex, mCurChoosedSubIndex);
		}
	}

	private void SetRewardItem(List<string> itemIds, List<EQUIP_QUALITY> qualitys, List<int> counts)
	{
		ShowRewardItemScripts.ShowRewards(itemIds, qualitys, counts);
	}

	public void OnClickOpenBtn()
	{
		if (curBossInfo == null)
		{
			return;
		}
		if (canOpenFlag == 0)
		{
			WaitResponseUIRootLogic.OpenWaitBox(233, 10f, 0f);
			open_guild_boss.request request = new open_guild_boss.request();
			request.id = curBossInfo.id;
			NetLogic.GetInstance().Send<Protocol.open_guild_boss>(request);
			return;
		}
		switch (canOpenFlag)
		{
		case 1:
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
			break;
		case 3:
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101540}"));
			break;
		case 2:
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101541}"));
			break;
		}
	}

	public void refershOpenBtn(guild_boss openboss)
	{
	}

	public void OnClickStartBtn()
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
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildActivityRootLogic);
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
				SingletonUnity<NewGuildUIRootLogic>.Instance.OnClickCloseBtn();
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

	public void OnClicktishiBtn()
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
					PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
					SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", CurGuildCityData.Rule, null);
				});
			}
		}
		else if (curBossData != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
			{
				SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", curBossData.Rule, null);
			});
		}
	}

	private void OnDisable()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.GUILD_BOSS_CLICK_START)
		{
			CheckTutorialEvent();
		}
	}
}
