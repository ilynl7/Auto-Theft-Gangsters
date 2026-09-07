using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class NewDailyCopyUIRootLogic : SingletonUnity<NewDailyCopyUIRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public List<DailyCopyLineLogic> DailyCopyLineList = new List<DailyCopyLineLogic>();

	public UITable RootTable;

	public UIScrollView uiScrollView;

	private List<copyscene_info> list;

	private List<copyscene_info> equipList = new List<copyscene_info>();

	private List<copyscene_info> pklist = new List<copyscene_info>();

	private List<copyscene_info> ExpList = new List<copyscene_info>();

	private Dictionary<string, copyscene_info> DailyCopyInfoDic;

	private copyscene_info curInfo;

	private int remainNum;

	private CopySceneData mCurCopyScene;

	private string curChoosekey = string.Empty;

	private int equipcopy_index;

	private int ExpCopy_index;

	public NewActivityInfoRootLogic ActivityInfoPage;

	public GameObject ActivityLineRoot;

	public int TargetTypeId;

	public string TargetActId;

	public GameDefine.ACTIVITY_TYPE TargetActType = GameDefine.ACTIVITY_TYPE.INVALID;

	public tower_info mPlayerTowerInfo;

	public DailyCopyLineLogic TowerLineItem;

	private List<tower_special_reward> mPlayerSpecialRewardList = new List<tower_special_reward>();

	public DailyCopyLineLogic SexMiniGameItem;

	private SexMiniData CurSexMiniGameData;

	private bool IsShowActivityInfo;

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

	private new void Awake()
	{
		base.Awake();
		ActivityInfoPage.RegisterStartBtn(OnClickStart);
		ActivityInfoPage.RegisterRankBtn(OnClickRankBtn);
		ActivityInfoPage.RegisterMatchBtn(OnClickMatchBtn);
		ActivityInfoPage.RegisterCloseBtn(ShowAcitvityLine);
	}

	public void ShowAcitvityLine()
	{
		IsShowActivityInfo = false;
		NGUITools.SetActive(ActivityInfoPage.gameObject, state: false);
		NGUITools.SetActive(ActivityLineRoot.gameObject, state: true);
		RefreshCopyPage();
		ResfershTowerInfo();
	}

	public void ShowActivityInfo(copyscene_info info)
	{
		IsShowActivityInfo = true;
		NGUITools.SetActive(ActivityInfoPage.gameObject, state: true);
		NGUITools.SetActive(ActivityLineRoot.gameObject, state: false);
		ActivityInfoPage.UpdateUI(info);
	}

	public void EnableReset()
	{
		NGUITools.SetActive(ActivityLineRoot.gameObject, state: true);
		for (int i = 0; i < DailyCopyLineList.Count; i++)
		{
			NGUITools.SetActive(DailyCopyLineList[i].gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(TowerLineItem.gameObject, state: false);
		NGUITools.SetActive(ActivityInfoPage.gameObject, state: false);
		NGUITools.SetActive(SexMiniGameItem.gameObject, state: false);
		IsShowActivityInfo = false;
		TargetTypeId = -1;
		TargetActId = string.Empty;
		mCurCopyScene = null;
		mPlayerTowerInfo = null;
		curChoosekey = string.Empty;
		TargetActType = GameDefine.ACTIVITY_TYPE.INVALID;
	}

	public void RefreshCopyPage()
	{
		if (IsShowActivityInfo)
		{
			UpdateRefershCopy();
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		list = new List<copyscene_info>(playerData.CopyInfoData.DailyCopyInfoList);
		DailyCopyInfoDic = playerData.CopyInfoData.DailyCopyInfoDic;
		equipList.Clear();
		pklist.Clear();
		ExpList.Clear();
		curChoosekey = string.Empty;
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
				list.RemoveAt(num);
			}
			else if (copySceneDataById.SubType == 12)
			{
				ExpList.Add(list[num]);
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
		ExpList.Sort(delegate(copyscene_info x, copyscene_info y)
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
		ExpCopy_index = list.Count;
		equipcopy_index = list.Count + 1;
		int num4 = -1;
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
					num4 = j;
				}
			}
			else if (j == list.Count)
			{
				DailyCopyLineList[j].ResetItem(string.Empty, "#{101511}", ExpList, OnClickItemBtn, isfinal: true);
				if (TargetTypeId == 12)
				{
					num4 = ExpCopy_index;
				}
			}
			else if (j == list.Count + 1)
			{
				DailyCopyLineList[j].ResetItem(string.Empty, "#{101510}", equipList, OnClickItemBtn, isfinal: true);
				if (TargetTypeId == 16)
				{
					num4 = equipcopy_index;
				}
			}
		}
		ResetSexMinigameInfo();
		RootTable.Reposition();
		uiScrollView.ResetPosition();
		if (num4 != -1)
		{
			DailyCopyLineList[num4].OnClickSelectItemBtn(TargetActId);
			TargetTypeId = -1;
			TargetActId = string.Empty;
			TargetActType = GameDefine.ACTIVITY_TYPE.INVALID;
		}
		else if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjNormal();
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_COPY_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.EXP_COPY_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.GOLD_COPY_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.SCUFFLE_COPY_WAIT_DATA || TutorialManager.CurStep == TUTORIAL_STEP.EQUIP_COPY_WAIT_DATA)
		{
			CheckTutorialEvent();
		}
	}

	public void UpdateRefershCopy()
	{
		ActivityInfoPage.refershInfo();
	}

	public void ResetTowerInfo(ret_request_tower_copy_info.request request)
	{
		if (request.HasTower_info)
		{
			mPlayerTowerInfo = request.tower_info;
		}
		if (request.HasTower_special_reward)
		{
			mPlayerSpecialRewardList = request.tower_special_reward;
			mPlayerSpecialRewardList.Sort((tower_special_reward x, tower_special_reward y) => (int)(x.floor - y.floor));
		}
		if (UnityVersionUtil.IsActive(ActivityLineRoot.gameObject))
		{
			if (mPlayerTowerInfo != null)
			{
				NGUITools.SetActive(TowerLineItem.gameObject, state: true);
				TowerLineItem.SetTowerInfo(mPlayerTowerInfo, OnClickTowerItemBtn);
			}
			else
			{
				NGUITools.SetActive(TowerLineItem.gameObject, state: false);
			}
			RootTable.Reposition();
			uiScrollView.ResetPosition();
			if (TargetActType == GameDefine.ACTIVITY_TYPE.TOWER)
			{
				TowerLineItem.OnClikcItemBtn();
				TargetTypeId = -1;
				TargetActId = string.Empty;
				TargetActType = GameDefine.ACTIVITY_TYPE.INVALID;
			}
			if (TutorialManager.CurStep == TUTORIAL_STEP.TOWER_WAIT_DATA)
			{
				CheckTutorialEvent();
			}
		}
		if (UnityVersionUtil.IsActive(ActivityInfoPage.gameObject) && ActivityInfoPage.mPlayerTowerInfo != null)
		{
			ActivityInfoPage.UpdateTowerUI(mPlayerTowerInfo, mPlayerSpecialRewardList);
		}
	}

	public void ResetSexMinigameInfo()
	{
		CurSexMiniGameData = DataManager.GetSexMiniDataById(GameDefine.SEX_MINI_GAME_ID);
		if (CurSexMiniGameData == null)
		{
			NGUITools.SetActive(SexMiniGameItem.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(SexMiniGameItem.gameObject, state: true);
			SexMiniGameItem.SetSexGameInfo(CurSexMiniGameData, OnClickSexGameBtn);
		}
		if (TargetTypeId == 27)
		{
			TargetTypeId = -1;
			TargetActId = string.Empty;
			TargetActType = GameDefine.ACTIVITY_TYPE.INVALID;
			SexMiniGameItem.OnClikcItemBtn();
		}
	}

	public void UpdateMatchingLabel()
	{
		if (UnityVersionUtil.IsActive(ActivityInfoPage.gameObject))
		{
			ActivityInfoPage.UpdateMatchingLabel();
		}
	}

	public void ResetTowerInfo(ret_grant_tower_reward.request request)
	{
		if (request.HasTower_info)
		{
			mPlayerTowerInfo = request.tower_info;
		}
		if (request.HasTower_special_reward)
		{
			mPlayerSpecialRewardList = request.tower_special_reward;
			mPlayerSpecialRewardList.Sort((tower_special_reward x, tower_special_reward y) => (int)(x.floor - y.floor));
		}
		if (UnityVersionUtil.IsActive(ActivityLineRoot.gameObject))
		{
			if (mPlayerTowerInfo != null)
			{
				NGUITools.SetActive(TowerLineItem.gameObject, state: true);
				TowerLineItem.SetTowerInfo(mPlayerTowerInfo, OnClickTowerItemBtn);
			}
			else
			{
				NGUITools.SetActive(TowerLineItem.gameObject, state: false);
			}
			RootTable.Reposition();
			uiScrollView.ResetPosition();
		}
		if (UnityVersionUtil.IsActive(ActivityInfoPage.gameObject) && ActivityInfoPage.mPlayerTowerInfo != null)
		{
			ActivityInfoPage.UpdateTowerUI(mPlayerTowerInfo, mPlayerSpecialRewardList);
		}
	}

	public void ResfershTowerInfo()
	{
		mPlayerTowerInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TowerData.PlayerTowerInfo;
		if (mPlayerTowerInfo != null)
		{
			NGUITools.SetActive(TowerLineItem.gameObject, state: true);
			TowerLineItem.SetTowerInfo(mPlayerTowerInfo, OnClickTowerItemBtn);
		}
		else
		{
			NGUITools.SetActive(TowerLineItem.gameObject, state: false);
		}
		RootTable.Reposition();
		uiScrollView.ResetPosition();
	}

	public void ResetTowerInfo(tower_info towerInfo)
	{
		mPlayerTowerInfo = towerInfo;
		if (UnityVersionUtil.IsActive(ActivityLineRoot.gameObject))
		{
			if (mPlayerTowerInfo != null)
			{
				NGUITools.SetActive(TowerLineItem.gameObject, state: true);
				TowerLineItem.SetTowerInfo(mPlayerTowerInfo, OnClickTowerItemBtn);
			}
			else
			{
				NGUITools.SetActive(TowerLineItem.gameObject, state: false);
			}
			RootTable.Reposition();
			uiScrollView.ResetPosition();
		}
		if (UnityVersionUtil.IsActive(ActivityInfoPage.gameObject) && ActivityInfoPage.mPlayerTowerInfo != null)
		{
			ActivityInfoPage.ResetTowerInfo(mPlayerTowerInfo);
		}
	}

	public bool CheckLevel()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(mCurCopyScene.MinLevel);
	}

	public void ClickTargetDailyCopy(MAPTYPE type, string actid)
	{
		int num = -1;
		if (type == MAPTYPE.EQUIP_COPY)
		{
			num = equipcopy_index;
		}
		if (num != -1)
		{
			DailyCopyLineList[num].ClickTargetBtn(actid);
		}
	}

	public void ClickTargetDailyCopy(MAPTYPE type)
	{
		for (int i = 0; i < DailyCopyLineList.Count; i++)
		{
			if (!string.IsNullOrEmpty(DailyCopyLineList[i].Key) && DataManager.GetCopySceneDataById(DailyCopyLineList[i].Key).SubType == (int)type)
			{
				DailyCopyLineList[i].OnClikcItemBtn();
				break;
			}
		}
		if (type == MAPTYPE.SEX_GAME)
		{
			SexMiniGameItem.OnClikcItemBtn();
		}
	}

	public void ClickTargetType(GameDefine.ACTIVITY_TYPE clicktype)
	{
		if (clicktype == GameDefine.ACTIVITY_TYPE.TOWER)
		{
			TowerLineItem.OnClikcItemBtn();
		}
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
		ShowActivityInfo(curInfo);
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_COPY_CLICK_COPY)
		{
			if (mCurCopyScene.SubType == 7)
			{
				CheckTutorialEvent();
			}
		}
		else if (TutorialManager.CurStep == TUTORIAL_STEP.EXP_COPY_CLICK_COPY)
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
		else if (TutorialManager.CurStep == TUTORIAL_STEP.EQUIP_COPY_CHOOSE_COPY && mCurCopyScene.SubType == 16)
		{
			CheckTutorialEvent();
		}
	}

	public void UpdateSelectItem()
	{
		for (int i = 0; i < DailyCopyLineList.Count; i++)
		{
			DailyCopyLineList[i].RefreshLineSelect(curChoosekey);
		}
	}

	public void OnClickRankBtn()
	{
		SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerRankInfoRoot, delegate
		{
			if (mCurCopyScene != null && mCurCopyScene.IsCarChasingCopy)
			{
				SingletonUnity<PlayerRankInfoRootLogic>.Instance.ResetToCar();
			}
			else if (CurSexMiniGameData != null)
			{
				SingletonUnity<PlayerRankInfoRootLogic>.Instance.ResetToSexMini();
			}
		});
	}

	public void OnClickStart()
	{
		if (mCurCopyScene == null)
		{
			return;
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_COPY_CLICK_START && mCurCopyScene.SubType == 7)
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
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
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
			return;
		}
		if (mCurCopyScene.IsSingleDance)
		{
			SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AutoMoveDest(mCurCopyScene.MapId, Vector3.right * -5f, AUTO_SEARCH_PARTH_FINISHEVENT.CITY_DANCE);
			CheckStartFlurry();
		}
		else if (mCurCopyScene.IsPVPMap)
		{
			SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
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
				if (mCurCopyScene.IsCashCopy || mCurCopyScene.IsExpCopy || mCurCopyScene.IsCarChasingCopy)
				{
					GameMoneyHelper.ShowItemProduct(mCurCopyScene.TimeInc);
				}
			}
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101540}"));
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
			enter_copy_scene.request request = new enter_copy_scene.request();
			request.mapInfoId = mCurCopyScene.ID;
			NetLogic.GetInstance().Send<Protocol.enter_copy_scene>(request);
			CarTimesCheck();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.SyncTimeslocal(mCurCopyScene.ID);
			CheckStartFlurry();
		}
	}

	public void CheckStartFlurry()
	{
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("DailyCopy", $"copy_{mCurCopyScene.ID}", "start");
	}

	public void CarTimesCheck()
	{
		if (mCurCopyScene.IsCarChasingCopy)
		{
			if (DailyCopyInfoDic.ContainsKey(mCurCopyScene.ID) && DailyCopyInfoDic[mCurCopyScene.ID].CurNum > 0)
			{
				DailyCopyInfoDic[mCurCopyScene.ID].CurNum--;
			}
			if (remainNum > 0)
			{
				remainNum--;
			}
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.UpdateTips();
		}
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
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew);
				return;
			}
			SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
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
		ActivityInfoPage.UpdateUI(curInfo);
		DailyCopyLineList[equipcopy_index].RefershParentLine(curInfo);
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.UpdateTips();
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
		TeamData curCopyTeamData = DataManager.GetTeamDataDataByID(mCurCopyScene.ID);
		if (curCopyTeamData == null)
		{
			return;
		}
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
					MessageBoxLogic.OpenOKCancelBox("#{103204}", "#{100127}", delegate
					{
						req_change_team_goal.request rpcReq5 = new req_change_team_goal.request
						{
							goalId = mCurCopyScene.ID,
							minLevel = mCurCopyScene.MinLevel,
							maxLevel = mCurCopyScene.MaxLevel,
							isVerfiy = ((!playerData.TeamInfo.IsVertify) ? 1 : 0)
						};
						NetLogic.GetInstance().Send<Protocol.req_change_team_goal>(rpcReq5);
						random_select_team.request rpcReq6 = new random_select_team.request
						{
							id = mCurCopyScene.ID,
							type1 = curCopyTeamData.GoalType
						};
						NetLogic.GetInstance().Send<Protocol.random_select_team>(rpcReq6);
					});
				}
			}
			else if (mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
			{
				random_select_team.request request2 = new random_select_team.request();
				request2.id = mCurCopyScene.ID;
				request2.type1 = curCopyTeamData.GoalType;
				NetLogic.GetInstance().Send<Protocol.random_select_team>(request2);
			}
			else
			{
				MessageBoxLogic.OpenOKCancelBox("#{103204}", "#{100127}", delegate
				{
					req_change_team_goal.request rpcReq3 = new req_change_team_goal.request
					{
						goalId = mCurCopyScene.ID,
						minLevel = mCurCopyScene.MinLevel,
						maxLevel = mCurCopyScene.MaxLevel,
						isVerfiy = ((!playerData.TeamInfo.IsVertify) ? 1 : 0)
					};
					NetLogic.GetInstance().Send<Protocol.req_change_team_goal>(rpcReq3);
					random_select_team.request rpcReq4 = new random_select_team.request
					{
						id = mCurCopyScene.ID,
						type1 = curCopyTeamData.GoalType
					};
					NetLogic.GetInstance().Send<Protocol.random_select_team>(rpcReq4);
				});
			}
		}
		else if (!playerData.IsHaveTeam())
		{
			if (playerData.TeamInfo.IsVertify)
			{
				if (mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
				{
					stop_random_select_team.request request3 = new stop_random_select_team.request();
					request3.id = playerData.TeamInfo.TeamGoalData.ID;
					request3.type1 = playerData.TeamInfo.TeamGoalData.GoalType;
					NetLogic.GetInstance().Send<Protocol.stop_random_select_team>(request3);
				}
				else
				{
					MessageBoxLogic.OpenOKCancelBox("#{103204}", "#{100127}", delegate
					{
						stop_random_select_team.request rpcReq = new stop_random_select_team.request
						{
							id = playerData.TeamInfo.TeamGoalData.ID,
							type1 = playerData.TeamInfo.TeamGoalData.GoalType
						};
						NetLogic.GetInstance().Send<Protocol.stop_random_select_team>(rpcReq);
						random_select_team.request rpcReq2 = new random_select_team.request
						{
							id = mCurCopyScene.ID,
							type1 = curCopyTeamData.GoalType
						};
						NetLogic.GetInstance().Send<Protocol.random_select_team>(rpcReq2);
					});
				}
			}
			else
			{
				random_select_team.request request4 = new random_select_team.request();
				request4.id = mCurCopyScene.ID;
				request4.type1 = curCopyTeamData.GoalType;
				NetLogic.GetInstance().Send<Protocol.random_select_team>(request4);
			}
		}
		UpdateMatchingLabel();
	}

	private void OnDisable()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_COPY_FINISH || TutorialManager.CurStep == TUTORIAL_STEP.EXP_COPY_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.SCUFFLE_COPY_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.EQUIP_COPY_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.GOLD_COPY_CLICK_START)
		{
			CheckTutorialEvent();
		}
	}

	public void OnClickTowerItemBtn(string key, GameObject lineObj)
	{
		curChoosekey = key;
		mCurCopyScene = null;
		NGUITools.SetActive(ActivityInfoPage.gameObject, state: true);
		NGUITools.SetActive(ActivityLineRoot.gameObject, state: false);
		ActivityInfoPage.UpdateTowerUI(mPlayerTowerInfo, mPlayerSpecialRewardList, selectmapflag: true);
		if (TutorialManager.CurStep == TUTORIAL_STEP.TOWER_CLICK_COPY)
		{
			CheckTutorialEvent();
		}
	}

	public void OnClickSexGameBtn(string key, GameObject lineObj)
	{
		curChoosekey = key;
		mCurCopyScene = null;
		NGUITools.SetActive(ActivityInfoPage.gameObject, state: true);
		NGUITools.SetActive(ActivityLineRoot.gameObject, state: false);
		ActivityInfoPage.UpdateSexGameUI(CurSexMiniGameData);
	}
}
