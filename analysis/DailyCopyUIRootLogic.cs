using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DailyCopyUIRootLogic : SingletonUnity<DailyCopyUIRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public List<DailyCopyLineLogic> DailyCopyLineList = new List<DailyCopyLineLogic>();

	public UITexture CopyBG;

	public ShowRewardItems ShowRewardItemsScripts;

	public UILabel DescLabel;

	public UITable RootTable;

	public UIScrollView uiScrollView;

	public UISprite WipeOutSp;

	public UISprite StartSp;

	public UISprite MatchSp;

	public GameObject WipeOutBtn;

	public GameObject MatchBtn;

	public UILabel WipeOutLabel;

	public UILabel MatchBtnLabel;

	public GameObject BtnRankObj;

	private List<copyscene_info> list;

	private List<copyscene_info> equipList = new List<copyscene_info>();

	private List<copyscene_info> pklist = new List<copyscene_info>();

	private Dictionary<string, copyscene_info> DailyCopyInfoDic;

	private copyscene_info curInfo;

	private int remainNum;

	private CopySceneData mCurCopyScene;

	private string curChoosekey = string.Empty;

	private int equipcopy_index;

	public GameObject ScoreObj;

	public UILabel ScoreLabel;

	private List<DailyActiveData> DailyActiveDataList = new List<DailyActiveData>();

	private Dictionary<string, daily_active> activeDic = new Dictionary<string, daily_active>();

	public int TargetTypeId;

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

	private void OnEnable()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DailyRewardNewRoot, delegate
		{
			SingletonUnity<DailyRewardNewLogic>.Instance.EnableReset();
			NetLogic.GetInstance().Send<Protocol.request_daily_active>();
		});
	}

	public void UpdataActiveInfo()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		activeDic = playerData.welfareData.DailyActives;
		NGUITools.SetActive(ScoreObj, state: false);
		if (mCurCopyScene != null)
		{
			switch (mCurCopyScene.SubType)
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
					ScoreLabel.text = $"{num}/{num2}";
				}
				else
				{
					ScoreLabel.text = $"{num2}/{num2}";
				}
				break;
			}
		}
	}

	public void EnableReset()
	{
		for (int i = 0; i < DailyCopyLineList.Count; i++)
		{
			NGUITools.SetActive(DailyCopyLineList[i].gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(WipeOutBtn, state: false);
		UnityVersionUtil.SetActiveRecursive(MatchBtn, state: false);
		UnityVersionUtil.SetActiveRecursive(BtnRankObj, state: false);
		TargetTypeId = -1;
		UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: false);
		mCurCopyScene = null;
		DailyActiveDataList = DataManager.GetDailyActiveDataList();
	}

	public void RefreshCopyPage()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		list = new List<copyscene_info>(playerData.CopyInfoData.DailyCopyInfoList);
		DailyCopyInfoDic = playerData.CopyInfoData.DailyCopyInfoDic;
		equipList.Clear();
		pklist.Clear();
		for (int num = list.Count - 1; num >= 0; num--)
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(list[num].ID);
			if (copySceneDataById == null)
			{
				list.RemoveAt(num);
			}
			else if (copySceneDataById.SubType == 16)
			{
				equipList.Add(list[num]);
				list.RemoveAt(num);
			}
			else if (copySceneDataById.SubType == 1)
			{
				pklist.Add(list[num]);
				list.RemoveAt(num);
			}
		}
		list.Sort(delegate(copyscene_info x, copyscene_info y)
		{
			CopySceneData copySceneDataById6 = DataManager.GetCopySceneDataById(x.ID);
			CopySceneData copySceneDataById7 = DataManager.GetCopySceneDataById(y.ID);
			return copySceneDataById6.MinLevel - copySceneDataById7.MinLevel;
		});
		equipList.Sort(delegate(copyscene_info x, copyscene_info y)
		{
			CopySceneData copySceneDataById4 = DataManager.GetCopySceneDataById(x.ID);
			CopySceneData copySceneDataById5 = DataManager.GetCopySceneDataById(y.ID);
			return copySceneDataById4.MinLevel - copySceneDataById5.MinLevel;
		});
		pklist.Sort(delegate(copyscene_info x, copyscene_info y)
		{
			CopySceneData copySceneDataById2 = DataManager.GetCopySceneDataById(x.ID);
			CopySceneData copySceneDataById3 = DataManager.GetCopySceneDataById(y.ID);
			return copySceneDataById2.MinLevel - copySceneDataById3.MinLevel;
		});
		int num2 = list.Count + 2;
		int num3 = num2 - DailyCopyLineList.Count;
		int count = DailyCopyLineList.Count;
		if (num3 > 0)
		{
			for (int i = 0; i < num3; i++)
			{
				GameObject gameObject = Object.Instantiate(DailyCopyLineList[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{count + i:D3}";
				DailyCopyLineLogic component = gameObject.GetComponent<DailyCopyLineLogic>();
				if (component != null)
				{
					component.transform.parent = DailyCopyLineList[0].transform.parent;
					component.transform.localScale = Vector3.one;
					component.transform.localPosition = Vector3.zero;
					DailyCopyLineList.Add(component);
				}
			}
		}
		equipcopy_index = list.Count + 1;
		int index = 0;
		for (int j = 0; j < DailyCopyLineList.Count; j++)
		{
			NGUITools.SetActive(DailyCopyLineList[j].gameObject, j < num2);
			if (j >= num2)
			{
				continue;
			}
			if (j < list.Count)
			{
				DailyCopyLineList[j].ResetItem(list[j], OnClickItemBtn, isfinal: false);
				if (DataManager.GetCopySceneDataById(list[j].ID).SubType == TargetTypeId)
				{
					index = j;
				}
			}
			else if (j == list.Count)
			{
				DailyCopyLineList[j].ResetItem(string.Empty, "#{101633}", pklist, OnClickItemBtn, isfinal: false);
				if (TargetTypeId == 1)
				{
					index = list.Count;
				}
			}
			else
			{
				DailyCopyLineList[j].ResetItem(string.Empty, "#{101510}", equipList, OnClickItemBtn, isfinal: true);
				if (TargetTypeId == 16)
				{
					index = equipcopy_index;
				}
			}
		}
		curChoosekey = string.Empty;
		RootTable.Reposition();
		uiScrollView.ResetPosition();
		DailyCopyLineList[index].OnClikcItemBtn();
	}

	public bool CheckLevel()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(mCurCopyScene.MinLevel);
	}

	public void OnClickCarRank()
	{
		SingletonUnity<ActivityUIRootLogic>.Instance.OnClickCloseBtn();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerRankInfoRoot, delegate
		{
			SingletonUnity<PlayerRankInfoRootLogic>.Instance.ResetToCar();
		});
	}

	public void OnClickItemBtn(string key, GameObject lineObj)
	{
		if (curChoosekey.Equals(key))
		{
			return;
		}
		curChoosekey = key;
		copyscene_info copyscene_info = DailyCopyInfoDic[key];
		mCurCopyScene = DataManager.GetCopySceneDataById(copyscene_info.ID);
		curInfo = copyscene_info;
		remainNum = (int)curInfo.CurNum;
		ShowRewardData showRewardData = null;
		if (mCurCopyScene.SubType == 12 || mCurCopyScene.SubType == 11 || mCurCopyScene.SubType == 23)
		{
			string id = DataManager.GetAdaptDataByID(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.Level).DorpKeyDic[mCurCopyScene.ShowRewardId];
			showRewardData = DataManager.GetShowRewardDataByID(id);
		}
		else if (mCurCopyScene.SubType == 20)
		{
			string id2 = DataManager.GetAdaptDataByID(SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.ServerLevel).DorpKeyDic[mCurCopyScene.ShowRewardId];
			showRewardData = DataManager.GetShowRewardDataByID(id2);
		}
		else
		{
			showRewardData = DataManager.GetShowRewardDataByID(mCurCopyScene.ShowRewardId);
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
		DescLabel.text = StrDictionary.GetDictionaryString(mCurCopyScene.Desc);
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBG.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if ((CopyBG.mainTexture == null || !CopyBG.mainTexture.name.Equals(mCurCopyScene.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(mCurCopyScene.Background, TextureLoadFinish));
		}
		refershUI();
		if (TutorialManager.CurStep == TUTORIAL_STEP.EXP_COPY_CLICK_COPY)
		{
			if (mCurCopyScene.SubType == 12)
			{
				CheckTutorialEvent();
			}
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.GOLD_COPY_CLICK_COPY)
		{
			if (mCurCopyScene.SubType == 11)
			{
				CheckTutorialEvent();
			}
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.SCUFFLE_COPY_CLICK_COPY)
		{
			if (mCurCopyScene.SubType == 20)
			{
				CheckTutorialEvent();
			}
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.EQUIP_COPY_CHOOSE_COPY)
		{
			if (mCurCopyScene.SubType == 16)
			{
				CheckTutorialEvent();
			}
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_COPY_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.CAR_COPY_FINISH || TutorialManager.CurStep == TUTORIAL_STEP.EXP_COPY_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.EXP_COPY_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.GOLD_COPY_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.GOLD_COPY_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.SCUFFLE_COPY_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.SCUFFLE_COPY_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.EQUIP_COPY_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.EQUIP_COPY_CLICK_START)
		{
			CheckTutorialEvent();
		}
	}

	private void TextureLoadFinish(string name, Texture tex)
	{
		CopyBG.mainTexture = tex;
	}

	public void refershUI()
	{
		if (mCurCopyScene.SubType == 7)
		{
			UnityVersionUtil.SetActiveRecursive(BtnRankObj, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(BtnRankObj, state: false);
		}
		if (mCurCopyScene.CanWipeOut == 1)
		{
			if (curInfo.HasBestGrade)
			{
				if (curInfo.BestGrade >= 3)
				{
					WipeOutSp.spriteName = "CZ_anNiu_1";
				}
				else
				{
					WipeOutSp.spriteName = "CZ_anNiu_2+";
				}
			}
			else
			{
				WipeOutSp.spriteName = "CZ_anNiu_2+";
			}
			if (CheckLevel())
			{
				StartSp.spriteName = "CZ_anNiu_2";
			}
			else
			{
				StartSp.spriteName = "CZ_anNiu_2+";
				WipeOutSp.spriteName = "CZ_anNiu_2+";
			}
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			int itemStackNumById = playerData.ItemBackPack.GetItemStackNumById(mCurCopyScene.WipeItem);
			WipeOutLabel.text = $"1/{itemStackNumById}";
			NGUITools.SetActive(MatchBtn.gameObject, state: false);
		}
		else if (mCurCopyScene.IsTeamCopy)
		{
			PlayerData playerData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (!CheckLevel())
			{
				StartSp.spriteName = "CZ_anNiu_2+";
			}
			else
			{
				StartSp.spriteName = "CZ_anNiu_2";
			}
			NGUITools.SetActive(MatchBtn.gameObject, state: false);
			UpdateMatchingLabel();
		}
		else
		{
			NGUITools.SetActive(MatchBtn.gameObject, state: false);
			if (CheckLevel())
			{
				StartSp.spriteName = "CZ_anNiu_2";
			}
			else
			{
				StartSp.spriteName = "CZ_anNiu_2+";
			}
		}
		UpdataActiveInfo();
		UpdateSelectItem();
	}

	public void UpdateSelectItem()
	{
		for (int i = 0; i < DailyCopyLineList.Count; i++)
		{
			DailyCopyLineList[i].RefreshLineSelect(curChoosekey);
		}
	}

	private void SetRewardItem(List<string> itemIds, List<EQUIP_QUALITY> qualitys, List<int> counts)
	{
		ShowRewardItemsScripts.ShowRewards(itemIds, qualitys, counts);
	}

	public void OnClickStart()
	{
		if (mCurCopyScene == null)
		{
			return;
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_COPY_FINISH && mCurCopyScene.SubType == 7)
		{
			CheckTutorialEvent();
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.EXP_COPY_CLICK_START && mCurCopyScene.SubType == 12)
		{
			CheckTutorialEvent();
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.GOLD_COPY_CLICK_START && mCurCopyScene.SubType == 11)
		{
			CheckTutorialEvent();
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.SCUFFLE_COPY_CLICK_START && mCurCopyScene.SubType == 20)
		{
			CheckTutorialEvent();
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.EQUIP_COPY_CHOOSE_COPY && mCurCopyScene.SubType == 16)
		{
			CheckTutorialEvent();
		}
		if (mCurCopyScene.SubType == 7 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsTutorialCanShow(FUNCTION_TYPE.CAR_COPY))
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.SetTutorialShowFinish(FUNCTION_TYPE.CAR_COPY);
		}
		if (!CheckLevel())
		{
			TutorialManager.LevelLimitAction();
			return;
		}
		if (mCurCopyScene.IsTeamCopy)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (playerData.IsHaveTeam() && !playerData.IsTeamLeader())
			{
				SingletonUnity<ActivityUIRootLogic>.Instance.OnClickCloseBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew);
			}
			else if (remainNum <= 0)
			{
				if (!string.IsNullOrEmpty(mCurCopyScene.TimeInc))
				{
					PlayerData playerData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
					int itemStackNumById = playerData2.ItemBackPack.GetItemStackNumById(mCurCopyScene.TimeInc);
					if (itemStackNumById > 0)
					{
						ItemData itemDataByID = DataManager.GetItemDataByID(mCurCopyScene.TimeInc);
						MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{101598}", itemDataByID.MName), "#{100127}", TeamYesStartfun);
						return;
					}
				}
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101540}"));
			}
			else
			{
				TeamStartFun();
			}
			return;
		}
		if (mCurCopyScene.IsCarChasingCopy)
		{
			ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			if (string.IsNullOrEmpty(mainPlayer.MountId))
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101557}"));
				return;
			}
		}
		if (mCurCopyScene.IsScuffleCopy)
		{
			if (remainNum <= 0)
			{
				NoticeLogic.AddNotifyData("#{102046}");
				return;
			}
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
			enter_scuffle_batttle.request request = new enter_scuffle_batttle.request();
			request.ID = mCurCopyScene.ID;
			request.floor = 0L;
			NetLogic.GetInstance().Send<Protocol.enter_scuffle_batttle>(request);
			CheckStartFlurry();
		}
		else if (mCurCopyScene.IsPVPMap)
		{
			SingletonUnity<ActivityUIRootLogic>.Instance.OnClickCloseBtn();
			MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(mCurCopyScene.MapId);
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AutoMoveDest(mCurCopyScene.MapId, mapInfoDataByID.BirthPosVector3, AUTO_SEARCH_PARTH_FINISHEVENT.CHANGE_MAP);
			CheckStartFlurry();
		}
		else if (remainNum <= 0)
		{
			if (!string.IsNullOrEmpty(mCurCopyScene.TimeInc))
			{
				PlayerData playerData3 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				int itemStackNumById2 = playerData3.ItemBackPack.GetItemStackNumById(mCurCopyScene.TimeInc);
				if (itemStackNumById2 > 0)
				{
					ItemData itemDataByID2 = DataManager.GetItemDataByID(mCurCopyScene.TimeInc);
					MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{101598}", itemDataByID2.MName), "#{100127}", delegate
					{
						WaitResponseUIRootLogic.OpenWaitBox(107, 10f, 0f);
						SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
						enter_copy_scene.request rpcReq = new enter_copy_scene.request
						{
							mapInfoId = mCurCopyScene.ID,
							reaminItem = true
						};
						NetLogic.GetInstance().Send<Protocol.enter_copy_scene>(rpcReq);
						CheckStartFlurry();
					});
					return;
				}
			}
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101540}"));
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
			enter_copy_scene.request request2 = new enter_copy_scene.request();
			request2.mapInfoId = mCurCopyScene.ID;
			NetLogic.GetInstance().Send<Protocol.enter_copy_scene>(request2);
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.SyncTimeslocal(mCurCopyScene.ID);
			CheckStartFlurry();
		}
	}

	public void CheckStartFlurry()
	{
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("DailyCopy", $"copy_{mCurCopyScene.ID}", "start");
	}

	public void TeamYesStartfun()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ItemContainer itemBackPack = playerData.ItemBackPack;
		List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(itemBackPack, IsAll: false, GameDefine.ITEM_TYPE.REMAIN);
		bool flag = false;
		GameItem gameItem = null;
		if (targetTypeItem == null || targetTypeItem.Count == 0)
		{
			flag = false;
		}
		else
		{
			for (int i = 0; i < targetTypeItem.Count; i++)
			{
				if (targetTypeItem[i].ItemId.Equals(mCurCopyScene.TimeInc))
				{
					flag = true;
					gameItem = targetTypeItem[i];
					break;
				}
			}
		}
		if (flag)
		{
			use_item.request request = new use_item.request();
			request.indexId = gameItem.IndexId;
			NetLogic.GetInstance().Send<Protocol.use_item>(request);
			WaitResponseUIRootLogic.OpenWaitBox(115, 10f, 0f);
			remainNum++;
			if (DailyCopyInfoDic.ContainsKey(curInfo.ID))
			{
				DailyCopyInfoDic[curInfo.ID].CurNum++;
			}
			for (int j = 0; j < DailyCopyLineList.Count; j++)
			{
				DailyCopyLineList[j].UpdateLineInfo(curInfo);
			}
			TeamStartFun();
		}
	}

	public void TeamStartFun()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveTeam())
		{
			if (mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
			{
				SingletonUnity<ActivityUIRootLogic>.Instance.OnClickCloseBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew);
				return;
			}
			SingletonUnity<ActivityUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CreateTeamRoot, delegate
			{
				WaitResponseUIRootLogic.OpenWaitBox(145, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
				SingletonUnity<CreateTeamRootLogic>.Instance.Reset(isCreate: false, mCurCopyScene.ID);
			});
		}
		else
		{
			OnClickMatchBtn();
		}
	}

	public void OnClicktishiBtn()
	{
		if (mCurCopyScene != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
			{
				PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
				SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", mCurCopyScene.Rule, null, TimeTools.GetLocalShowTime_HM(playerCommonData.ResetTime, playerCommonData.TimeOffset));
			});
		}
	}

	public void OnClickWipeOutBtn()
	{
		copyscene_info copyscene_info = curInfo;
		if (copyscene_info == null)
		{
			return;
		}
		if (!CheckLevel())
		{
			TutorialManager.LevelLimitAction();
			return;
		}
		if (copyscene_info.BestGrade < 3)
		{
			NoticeLogic.AddNotifyData("#{101574}");
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		int itemStackNumById = playerData.ItemBackPack.GetItemStackNumById(mCurCopyScene.WipeItem);
		if (itemStackNumById <= 0)
		{
			NoticeLogic.AddNotifyData("#{101543}");
			GameMoneyHelper.ShowItemProduct(mCurCopyScene.WipeItem);
		}
		else if (remainNum <= 0)
		{
			if (!string.IsNullOrEmpty(mCurCopyScene.TimeInc))
			{
				PlayerData playerData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				int itemStackNumById2 = playerData2.ItemBackPack.GetItemStackNumById(mCurCopyScene.TimeInc);
				if (itemStackNumById2 > 0)
				{
					ItemData itemDataByID = DataManager.GetItemDataByID(mCurCopyScene.TimeInc);
					MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{101598}", itemDataByID.MName), "#{100127}", delegate
					{
						WaitResponseUIRootLogic.OpenWaitBox(232, 10f, 0f);
						copy_swipe_out.request rpcReq = new copy_swipe_out.request
						{
							copyInfoId = mCurCopyScene.ID,
							reaminItem = true
						};
						NetLogic.GetInstance().Send<Protocol.copy_swipe_out>(rpcReq);
						SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("DailyCopy", $"copy_{mCurCopyScene.ID}", "wipe_out");
					});
					return;
				}
			}
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101540}"));
		}
		else
		{
			WaitResponseUIRootLogic.OpenWaitBox(232, 10f, 0f);
			copy_swipe_out.request request = new copy_swipe_out.request();
			request.copyInfoId = mCurCopyScene.ID;
			NetLogic.GetInstance().Send<Protocol.copy_swipe_out>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("DailyCopy", $"copy_{mCurCopyScene.ID}", "wipe_out");
			wipeoutUpdate();
		}
	}

	public void wipeoutUpdate()
	{
		if (mCurCopyScene.SubType != 16)
		{
			return;
		}
		for (int i = 0; i < equipList.Count; i++)
		{
			if (DailyCopyInfoDic.ContainsKey(equipList[i].ID))
			{
				DailyCopyInfoDic[equipList[i].ID].CurNum--;
			}
		}
		remainNum--;
		refershUI();
		DailyCopyLineList[equipcopy_index].RefershParentLine(curInfo);
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.UpdateTips();
	}

	public void UpdateWipeoutItem(string item_id)
	{
		if (mCurCopyScene.CanWipeOut == 1 && item_id.Equals(mCurCopyScene.WipeItem))
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			int itemStackNumById = playerData.ItemBackPack.GetItemStackNumById(mCurCopyScene.WipeItem);
			WipeOutLabel.text = $"1/{itemStackNumById}";
		}
	}

	public void OnClickMatchBtn()
	{
		if (!CheckLevel())
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveTeam() && !playerData.IsTeamLeader())
		{
			NoticeLogic.AddNotifyData("#{102008}");
		}
		else if (remainNum <= 0)
		{
			if (!string.IsNullOrEmpty(mCurCopyScene.TimeInc))
			{
				PlayerData playerData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				int itemStackNumById = playerData2.ItemBackPack.GetItemStackNumById(mCurCopyScene.TimeInc);
				if (itemStackNumById > 0)
				{
					ItemData itemDataByID = DataManager.GetItemDataByID(mCurCopyScene.TimeInc);
					MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{101598}", itemDataByID.MName), "#{100127}", OnClickYesUseitemMatch);
					return;
				}
			}
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101540}"));
		}
		else
		{
			MatchFun();
		}
	}

	public void OnClickYesUseitemMatch()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ItemContainer itemBackPack = playerData.ItemBackPack;
		List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(itemBackPack, IsAll: false, GameDefine.ITEM_TYPE.REMAIN);
		bool flag = false;
		GameItem gameItem = null;
		if (targetTypeItem == null || targetTypeItem.Count == 0)
		{
			flag = false;
		}
		else
		{
			for (int i = 0; i < targetTypeItem.Count; i++)
			{
				if (targetTypeItem[i].ItemId.Equals(mCurCopyScene.TimeInc))
				{
					flag = true;
					gameItem = targetTypeItem[i];
					break;
				}
			}
		}
		if (flag)
		{
			use_item.request request = new use_item.request();
			request.indexId = gameItem.IndexId;
			NetLogic.GetInstance().Send<Protocol.use_item>(request);
			WaitResponseUIRootLogic.OpenWaitBox(115, 10f, 0f);
			remainNum++;
			if (DailyCopyInfoDic.ContainsKey(curInfo.ID))
			{
				DailyCopyInfoDic[curInfo.ID].CurNum++;
			}
			for (int j = 0; j < DailyCopyLineList.Count; j++)
			{
				DailyCopyLineList[j].UpdateLineInfo(curInfo);
			}
			MatchFun();
		}
	}

	public void MatchFun()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		TeamData teamDataDataByID = DataManager.GetTeamDataDataByID(mCurCopyScene.ID);
		if (playerData.IsTeamLeader())
		{
			if (playerData.TeamInfo.IsVertify)
			{
				if (mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
				{
					stop_random_select_team.request request = new stop_random_select_team.request();
					request.id = playerData.TeamInfo.TeamGoalData.ID;
					request.type1 = playerData.TeamInfo.TeamGoalData.GoalType;
					NetLogic.GetInstance().Send<Protocol.stop_random_select_team>(request);
				}
				else
				{
					WaitResponseUIRootLogic.OpenWaitBox(145, 10f, 0f);
					NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SearchTeamRoot, delegate
					{
						SingletonUnity<SearchTeamRootLogic>.Instance.Reset(mCurCopyScene.ID);
					});
				}
			}
			else if (mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
			{
				random_select_team.request request2 = new random_select_team.request();
				request2.id = mCurCopyScene.ID;
				request2.type1 = teamDataDataByID.GoalType;
				NetLogic.GetInstance().Send<Protocol.random_select_team>(request2);
			}
			else
			{
				WaitResponseUIRootLogic.OpenWaitBox(145, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SearchTeamRoot, delegate
				{
					SingletonUnity<SearchTeamRootLogic>.Instance.Reset(mCurCopyScene.ID);
				});
			}
		}
		else if (!playerData.IsHaveTeam())
		{
			random_select_team.request request3 = new random_select_team.request();
			request3.id = mCurCopyScene.ID;
			request3.type1 = teamDataDataByID.GoalType;
			NetLogic.GetInstance().Send<Protocol.random_select_team>(request3);
		}
	}

	public void UpdateMatchingLabel()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsTeamLeader())
		{
			if (playerData.TeamInfo.IsVertify)
			{
				if (mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
				{
					MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{102007}");
				}
				else
				{
					MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{101503}");
				}
			}
			else
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{101503}");
			}
		}
		else if (playerData.IsHaveTeam())
		{
			if (playerData.TeamInfo.IsVertify && mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{102007}");
			}
			else
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{101503}");
			}
		}
		else
		{
			MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{101503}");
		}
	}

	private void OnDisable()
	{
		if (SingletonUnity<UIManager>.Exists)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyRewardNewRoot);
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_COPY_FINISH || TutorialManager.CurStep == TUTORIAL_STEP.EXP_COPY_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.SCUFFLE_COPY_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.EQUIP_COPY_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.GOLD_COPY_CLICK_START)
		{
			CheckTutorialEvent();
		}
	}
}
