using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class NewActivityInfoRootLogic : MonoBehaviour
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public GameObject DescObj;

	public GameObject TowerInfoObj;

	public GameObject ScuffleObj;

	public UILabel BestFloorLabel;

	public UILabel CurFloorLabel;

	public UILabel ResetNumLabel;

	public ShowRewardItems ShowRewardRoot;

	public UILabel DescLabel;

	public UISprite StartBtnPic;

	public UISprite WipeOutBtnPic;

	public UILabel WipeOutlabel;

	public UISprite ResetBtnPic;

	public UISprite WipeoutTipsPic;

	public UISprite RewardTipPic;

	public UILabel WappingRestTimeLabel;

	public UILabel RewardTipsLabel;

	public UILabel TowerLimitLevelLabel;

	public UISprite RankBtnSp;

	public UITexture CopyBGTexture;

	private copyscene_info mCurCopyInfo;

	private CopySceneData mCurCopyScene;

	public UISprite MatchBtn;

	public UILabel MatchBtnLabel;

	public TweenAlpha MatchAnima;

	public UISprite SingleBtn;

	private DelegateDefine.NoParamDelegate onClickStartBtn;

	private DelegateDefine.NoParamDelegate onClickRankBtn;

	private DelegateDefine.NoParamDelegate onClickCloseBtn;

	private DelegateDefine.NoParamDelegate onClickMatchBtn;

	public tower_info mPlayerTowerInfo;

	private List<tower_special_reward> mPlayerSpecialRewardList = new List<tower_special_reward>();

	private int mRewardFloorIndex = -1;

	private bool cangetSpecial;

	private int MAX_FLOOR_NUM;

	private int remainNum;

	public UISprite AcceptBtn;

	public UISprite RefershBtn;

	public UISprite GiveUpBtn;

	public UILabel MissionLabel;

	public UILabel TimesLabel;

	public UISprite[] Stars;

	public UILabel priceLabel;

	private OnlineMissionData CurOnLineMissiondata;

	private MissionData CurMissiondata;

	private int costprice;

	private int costpricetype = 1;

	private SexMiniData CurSexMiniData;

	private int mCurPlayerLimitFloor;

	private int wippingRestTime;

	private float secondTimeCount;

	private bool mIsMatching;

	public bool IsMatching
	{
		get
		{
			return mIsMatching;
		}
		set
		{
			mIsMatching = value;
		}
	}

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

	public void RegisterStartBtn(DelegateDefine.NoParamDelegate func)
	{
		onClickStartBtn = func;
	}

	public void RegisterRankBtn(DelegateDefine.NoParamDelegate func)
	{
		onClickRankBtn = func;
	}

	public void RegisterCloseBtn(DelegateDefine.NoParamDelegate func)
	{
		onClickCloseBtn = func;
	}

	public void RegisterMatchBtn(DelegateDefine.NoParamDelegate func)
	{
		onClickMatchBtn = func;
	}

	public void refershInfo()
	{
		if (mCurCopyInfo != null)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			mCurCopyInfo = playerData.CopyInfoData.GetCopyinfoByID(mCurCopyInfo.ID);
			UpdateUI(mCurCopyInfo);
		}
	}

	public void UpdateUI(copyscene_info info)
	{
		mCurCopyInfo = info;
		mPlayerTowerInfo = null;
		CurSexMiniData = null;
		mCurCopyScene = DataManager.GetCopySceneDataById(mCurCopyInfo.ID);
		if (mCurCopyScene.IsScuffleCopy)
		{
			UpdateScuffleInfo();
			return;
		}
		NGUITools.SetActive(DescObj, state: true);
		NGUITools.SetActive(TowerInfoObj, state: false);
		NGUITools.SetActive(ScuffleObj, state: false);
		ShowRewardData showRewardData = null;
		if (mCurCopyScene.SubType == 11 || mCurCopyScene.SubType == 23 || mCurCopyScene.SubType == 7)
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
		if (mCurCopyScene.SubType == 16 || mCurCopyScene.SubType == 12)
		{
			NGUITools.SetActive(SingleBtn.gameObject, state: true);
			NGUITools.SetActive(MatchBtn.gameObject, state: true);
			NGUITools.SetActive(StartBtnPic.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(SingleBtn.gameObject, state: false);
			NGUITools.SetActive(MatchBtn.gameObject, state: false);
			NGUITools.SetActive(StartBtnPic.gameObject, state: true);
		}
		UpdateMatchingLabel();
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardRoot.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardRoot.gameObject, state: false);
		}
		DescLabel.text = StrDictionary.GetDictionaryString(mCurCopyScene.Desc);
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBGTexture.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if ((CopyBGTexture.mainTexture == null || !CopyBGTexture.mainTexture.name.Equals(mCurCopyScene.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(mCurCopyScene.Background, TextureLoadFinish));
		}
		if (mCurCopyScene.SubType == 7)
		{
			UnityVersionUtil.SetActiveRecursive(RankBtnSp.gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(RankBtnSp.gameObject, state: false);
		}
		if (CheckLevel())
		{
			StartBtnPic.spriteName = "CZ_anNiu_2";
		}
		else
		{
			StartBtnPic.spriteName = "CZ_anNiu_2+";
		}
		SelectDailyCopyMapItem();
	}

	public void UpdateScuffleInfo()
	{
		NGUITools.SetActive(DescObj, state: true);
		NGUITools.SetActive(TowerInfoObj, state: false);
		NGUITools.SetActive(ScuffleObj, state: true);
		DescLabel.text = string.Empty;
		NGUITools.SetActive(SingleBtn.gameObject, state: false);
		NGUITools.SetActive(MatchBtn.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(RankBtnSp.gameObject, state: false);
		ConfigData configDataByKey = DataManager.GetConfigDataByKey("HangupMissionCost");
		costprice = 0;
		costpricetype = 1;
		if (configDataByKey != null)
		{
			costprice = configDataByKey.Valuei;
		}
		priceLabel.text = GameMoneyHelper.GetMoneyValStr(costprice, costpricetype);
		remainNum = (int)mCurCopyInfo.CurNum;
		TimesLabel.text = string.Format("{0}  {1}/{2}", StrDictionary.GetDictionaryString("#{101508}"), remainNum, mCurCopyScene.MaxPlayNum);
		if (mCurCopyInfo.HasStr)
		{
			CurOnLineMissiondata = DataManager.GetOnlineMissionDataByID(mCurCopyInfo.str);
		}
		if (CurOnLineMissiondata != null)
		{
			CurMissiondata = DataManager.GetMissionDataByID(CurOnLineMissiondata.MissionId);
			if (CurMissiondata != null)
			{
				MissionLabel.text = CurMissiondata.MDescribeID;
			}
			else
			{
				MissionLabel.text = string.Empty;
			}
			for (int i = 0; i < Stars.Length; i++)
			{
				if (i < CurOnLineMissiondata.MissionStar)
				{
					Stars[i].color = Color.white;
					Stars[i].alpha = 1f;
				}
				else
				{
					Stars[i].color = Color.black;
					Stars[i].alpha = 0.5f;
				}
			}
			ShowRewardData showRewardData = null;
			showRewardData = DataManager.GetShowRewardDataByID(CurOnLineMissiondata.ShowReward);
			if (showRewardData != null)
			{
				UnityVersionUtil.SetActiveRecursive(ShowRewardRoot.gameObject, state: true);
				SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(ShowRewardRoot.gameObject, state: false);
			}
		}
		else
		{
			for (int j = 0; j < Stars.Length; j++)
			{
				Stars[j].color = Color.black;
				Stars[j].alpha = 0.5f;
			}
			UnityVersionUtil.SetActiveRecursive(ShowRewardRoot.gameObject, state: false);
		}
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBGTexture.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if ((CopyBGTexture.mainTexture == null || !CopyBGTexture.mainTexture.name.Equals(mCurCopyScene.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(mCurCopyScene.Background, TextureLoadFinish));
		}
		if (CheckLevel() && mCurCopyInfo.state != 2)
		{
			StartBtnPic.spriteName = "CZ_anNiu_1";
			AcceptBtn.spriteName = "CZ_anNiu_1";
			RefershBtn.spriteName = "CZ_anNiu_2";
		}
		else
		{
			StartBtnPic.spriteName = "CZ_anNiu_2+";
			AcceptBtn.spriteName = "CZ_anNiu_2+";
			RefershBtn.spriteName = "CZ_anNiu_2+";
		}
		if (mCurCopyInfo.state == 0L)
		{
			NGUITools.SetActive(AcceptBtn.gameObject, state: true);
			NGUITools.SetActive(StartBtnPic.gameObject, state: false);
			NGUITools.SetActive(RefershBtn.gameObject, state: true);
			NGUITools.SetActive(GiveUpBtn.gameObject, state: false);
		}
		else if (mCurCopyInfo.state == 1)
		{
			NGUITools.SetActive(AcceptBtn.gameObject, state: false);
			NGUITools.SetActive(StartBtnPic.gameObject, state: true);
			NGUITools.SetActive(RefershBtn.gameObject, state: false);
			NGUITools.SetActive(GiveUpBtn.gameObject, state: true);
		}
		else if (mCurCopyInfo.state == 2)
		{
			NGUITools.SetActive(AcceptBtn.gameObject, state: true);
			NGUITools.SetActive(StartBtnPic.gameObject, state: false);
			NGUITools.SetActive(RefershBtn.gameObject, state: true);
			NGUITools.SetActive(GiveUpBtn.gameObject, state: false);
		}
		SelectDailyCopyMapItem();
	}

	public void UpdateSexGameUI(SexMiniData info)
	{
		CurSexMiniData = info;
		mPlayerTowerInfo = null;
		mCurCopyInfo = null;
		mCurCopyScene = null;
		NGUITools.SetActive(DescObj, state: true);
		NGUITools.SetActive(TowerInfoObj, state: false);
		NGUITools.SetActive(ScuffleObj, state: false);
		ShowRewardData showRewardData = null;
		showRewardData = DataManager.GetShowRewardDataByID(CurSexMiniData.ShowRewardID);
		NGUITools.SetActive(SingleBtn.gameObject, state: false);
		NGUITools.SetActive(MatchBtn.gameObject, state: false);
		NGUITools.SetActive(StartBtnPic.gameObject, state: true);
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardRoot.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardRoot.gameObject, state: false);
		}
		DescLabel.text = StrDictionary.GetDictionaryString(CurSexMiniData.Description);
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBGTexture.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if ((CopyBGTexture.mainTexture == null || !CopyBGTexture.mainTexture.name.Equals(CurSexMiniData.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(CurSexMiniData.Background, TextureLoadFinish));
		}
		UnityVersionUtil.SetActiveRecursive(RankBtnSp.gameObject, state: true);
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(CurSexMiniData.UnlockLevel))
		{
			StartBtnPic.spriteName = "CZ_anNiu_2";
		}
		else
		{
			StartBtnPic.spriteName = "CZ_anNiu_2+";
		}
		SelectShowINMapByType(GameDefine.ACTIVITY_TYPE.SEX_MINI);
	}

	public int GetCurPlayerLimitFloor()
	{
		Dictionary<int, TowerData> towerDataDic = DataManager.TowerDataDic;
		int count = towerDataDic.Count;
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		for (int num = count - 1; num >= 0; num--)
		{
			if (towerDataDic[num].LimitLevel <= level)
			{
				return num;
			}
		}
		return 0;
	}

	public void UpdateTowerUI(tower_info info, List<tower_special_reward> SpecialRewardList = null, bool selectmapflag = false)
	{
		mPlayerTowerInfo = info;
		mCurCopyInfo = null;
		mCurCopyScene = null;
		CurSexMiniData = null;
		NGUITools.SetActive(DescObj, state: false);
		NGUITools.SetActive(TowerInfoObj, state: true);
		NGUITools.SetActive(ScuffleObj, state: false);
		NGUITools.SetActive(RankBtnSp.gameObject, state: false);
		NGUITools.SetActive(SingleBtn.gameObject, state: false);
		NGUITools.SetActive(MatchBtn.gameObject, state: false);
		NGUITools.SetActive(StartBtnPic.gameObject, state: true);
		cangetSpecial = false;
		mRewardFloorIndex = -1;
		mPlayerSpecialRewardList = SpecialRewardList;
		if (mPlayerSpecialRewardList != null && mPlayerSpecialRewardList.Count != 0)
		{
			for (int i = 0; i < mPlayerSpecialRewardList.Count; i++)
			{
				if (mPlayerSpecialRewardList[i].state == 0L)
				{
					mRewardFloorIndex = (int)mPlayerSpecialRewardList[i].floor;
					NGUITools.SetActive(RewardTipPic.gameObject, mPlayerSpecialRewardList[i].floor <= mPlayerTowerInfo.floor);
					cangetSpecial = true;
					break;
				}
			}
		}
		if (!cangetSpecial)
		{
			NGUITools.SetActive(RewardTipPic.gameObject, state: false);
		}
		BestFloorLabel.text = $"{mPlayerTowerInfo.floor + 1}";
		if (mPlayerTowerInfo.cur_floor < 80)
		{
			CurFloorLabel.text = $"{mPlayerTowerInfo.cur_floor + 1}";
		}
		else
		{
			CurFloorLabel.text = $"{mPlayerTowerInfo.cur_floor}";
		}
		ResetNumLabel.text = string.Format("{0}:{1}/1", StrDictionary.GetDictionaryString("#{101528}"), mPlayerTowerInfo.times);
		MAX_FLOOR_NUM = (int)mPlayerTowerInfo.max_floor;
		UpdateFloorInfo((int)mPlayerTowerInfo.cur_floor);
		mCurPlayerLimitFloor = GetCurPlayerLimitFloor();
		TowerLimitLevelLabel.text = StrDictionary.GetDictionaryString("#{102082}", mCurPlayerLimitFloor + 1);
		RefershTowerBtn();
		TowerData towerDataByFloorID = DataManager.GetTowerDataByFloorID(0);
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBGTexture.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if ((CopyBGTexture.mainTexture == null || !CopyBGTexture.mainTexture.name.Equals(towerDataByFloorID.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(towerDataByFloorID.Background, TextureLoadFinish));
		}
		if (selectmapflag)
		{
			SelectShowINMapByType(GameDefine.ACTIVITY_TYPE.TOWER);
		}
	}

	public void ResetTowerInfo(tower_info info)
	{
		if (mPlayerTowerInfo == null)
		{
			return;
		}
		mPlayerTowerInfo = info;
		mCurCopyInfo = null;
		mCurCopyScene = null;
		CurSexMiniData = null;
		NGUITools.SetActive(DescObj, state: false);
		NGUITools.SetActive(TowerInfoObj, state: true);
		NGUITools.SetActive(ScuffleObj, state: false);
		NGUITools.SetActive(RankBtnSp.gameObject, state: false);
		cangetSpecial = false;
		mRewardFloorIndex = -1;
		if (mPlayerSpecialRewardList != null && mPlayerSpecialRewardList.Count != 0)
		{
			for (int i = 0; i < mPlayerSpecialRewardList.Count; i++)
			{
				if (mPlayerSpecialRewardList[i].state == 0L)
				{
					mRewardFloorIndex = (int)mPlayerSpecialRewardList[i].floor;
					NGUITools.SetActive(RewardTipPic.gameObject, mPlayerSpecialRewardList[i].floor <= mPlayerTowerInfo.floor);
					cangetSpecial = true;
					break;
				}
			}
		}
		if (!cangetSpecial)
		{
			NGUITools.SetActive(RewardTipPic.gameObject, state: false);
		}
		BestFloorLabel.text = $"{mPlayerTowerInfo.floor + 1}";
		if (mPlayerTowerInfo.cur_floor < 80)
		{
			CurFloorLabel.text = $"{mPlayerTowerInfo.cur_floor + 1}";
		}
		else
		{
			CurFloorLabel.text = $"{mPlayerTowerInfo.cur_floor}";
		}
		ResetNumLabel.text = string.Format("{0}:{1}/1", StrDictionary.GetDictionaryString("#{101528}"), mPlayerTowerInfo.times);
		MAX_FLOOR_NUM = (int)mPlayerTowerInfo.max_floor;
		UpdateFloorInfo((int)mPlayerTowerInfo.cur_floor);
		mCurPlayerLimitFloor = GetCurPlayerLimitFloor();
		TowerLimitLevelLabel.text = StrDictionary.GetDictionaryString("#{102082}", mCurPlayerLimitFloor + 1);
		RefershTowerBtn();
	}

	public void UpdateFloorInfo(int floorId)
	{
		if (floorId >= MAX_FLOOR_NUM)
		{
			RewardTipsLabel.enabled = false;
			NGUITools.SetActive(ShowRewardRoot.gameObject, state: false);
			return;
		}
		RewardTipsLabel.enabled = true;
		NGUITools.SetActive(ShowRewardRoot.gameObject, state: true);
		if (mRewardFloorIndex > 0)
		{
			TowerData towerDataByFloorID = DataManager.GetTowerDataByFloorID(mRewardFloorIndex);
			if (towerDataByFloorID.IsShowReward == 1)
			{
				ShowRewardData showRewardDataByID = DataManager.GetShowRewardDataByID(towerDataByFloorID.SpecialRewardId);
				ShowRewardRoot.ShowRewards(showRewardDataByID.ItemIdList, showRewardDataByID.QualityList, showRewardDataByID.CountList);
				if (GameManager.IsSupportCurDataVersion137())
				{
					RewardTipsLabel.text = StrDictionary.GetDictionaryString("#{102060}", towerDataByFloorID.FloorID + 1);
				}
				else
				{
					RewardTipsLabel.text = "Reward:";
				}
				return;
			}
			if (mPlayerSpecialRewardList != null && mPlayerSpecialRewardList.Count != 0)
			{
				for (int i = 0; i < mPlayerSpecialRewardList.Count; i++)
				{
					if (mPlayerSpecialRewardList[i].floor < mRewardFloorIndex)
					{
						continue;
					}
					towerDataByFloorID = DataManager.GetTowerDataByFloorID((int)mPlayerSpecialRewardList[i].floor);
					if (towerDataByFloorID.IsShowReward == 1)
					{
						ShowRewardData showRewardDataByID2 = DataManager.GetShowRewardDataByID(towerDataByFloorID.SpecialRewardId);
						ShowRewardRoot.ShowRewards(showRewardDataByID2.ItemIdList, showRewardDataByID2.QualityList, showRewardDataByID2.CountList);
						if (GameManager.IsSupportCurDataVersion137())
						{
							RewardTipsLabel.text = StrDictionary.GetDictionaryString("#{102060}", towerDataByFloorID.FloorID + 1);
						}
						else
						{
							RewardTipsLabel.text = "Reward:";
						}
						break;
					}
				}
			}
			if (towerDataByFloorID.IsShowReward == 0)
			{
				towerDataByFloorID = DataManager.GetTowerDataByFloorID(floorId);
				ShowRewardData showRewardDataByID3 = DataManager.GetShowRewardDataByID(towerDataByFloorID.ShowRewardID);
				ShowRewardRoot.ShowRewards(showRewardDataByID3.ItemIdList, showRewardDataByID3.QualityList, showRewardDataByID3.CountList);
				if (GameManager.IsSupportCurDataVersion137())
				{
					RewardTipsLabel.text = StrDictionary.GetDictionaryString("#{102060}", towerDataByFloorID.FloorID + 1);
				}
				else
				{
					RewardTipsLabel.text = "Reward:";
				}
			}
		}
		else
		{
			TowerData towerDataByFloorID2 = DataManager.GetTowerDataByFloorID(floorId);
			ShowRewardData showRewardDataByID4 = DataManager.GetShowRewardDataByID(towerDataByFloorID2.ShowRewardID);
			ShowRewardRoot.ShowRewards(showRewardDataByID4.ItemIdList, showRewardDataByID4.QualityList, showRewardDataByID4.CountList);
			if (GameManager.IsSupportCurDataVersion137())
			{
				RewardTipsLabel.text = StrDictionary.GetDictionaryString("#{102060}", towerDataByFloorID2.FloorID + 1);
			}
			else
			{
				RewardTipsLabel.text = "Reward:";
			}
		}
	}

	public void OnClickAcceptBtn()
	{
		if (!CheckLevel())
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
		}
		else if (remainNum <= 0)
		{
			NoticeLogic.AddNotifyData("#{102046}");
		}
		else if (CurMissiondata != null && mCurCopyInfo.state == 0L && CurMissiondata.Class == 7 && CurOnLineMissiondata != null)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AcceptMission(CurMissiondata.ID, CurOnLineMissiondata.ID);
		}
	}

	public void OnClickGiveUpBtn()
	{
		if (CurMissiondata != null && mCurCopyInfo.state == 1 && CurMissiondata.Class == 7)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AbandonMission(CurMissiondata.ID);
		}
	}

	public void OnClickRefershBtn()
	{
		if (!CheckLevel())
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
		}
		else if (remainNum <= 0)
		{
			NoticeLogic.AddNotifyData("#{102046}");
		}
		else if (GameMoneyHelper.BeforeCheckBuy(costpricetype, costprice))
		{
			NetLogic.GetInstance().Send<Protocol.refresh_online_misison>();
		}
	}

	public void OnClickResetBtn()
	{
		if (mPlayerTowerInfo == null)
		{
			return;
		}
		if (mPlayerTowerInfo.wipe_out_state == 1)
		{
			NoticeLogic.AddNotifyData("#{102035}");
		}
		else if (mPlayerTowerInfo.cur_floor != 0L)
		{
			if (mPlayerTowerInfo.times > 0)
			{
				NetLogic.GetInstance().Send<Protocol.tower_reset>();
			}
			else
			{
				NoticeLogic.AddNotifyData("#{102003}");
			}
		}
	}

	public void OnClickWipeOutBtn()
	{
		if (mPlayerTowerInfo == null)
		{
			return;
		}
		if (mPlayerTowerInfo.wipe_out_state == 0L)
		{
			if (mPlayerTowerInfo.cur_floor <= mPlayerTowerInfo.floor)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TowerWipeOutRootLogic, delegate
				{
					SingletonUnity<TowerWipeOutRootLogic>.Instance.Reset((int)mPlayerTowerInfo.cur_floor, (int)mPlayerTowerInfo.floor);
				});
			}
			else
			{
				NoticeLogic.AddNotifyData("#{102002}");
			}
		}
		else if (mPlayerTowerInfo.wipe_out_state == 1)
		{
			int restTime = (int)(mPlayerTowerInfo.wipe_time - SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime());
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TowerWipeOutRootLogic, delegate
			{
				SingletonUnity<TowerWipeOutRootLogic>.Instance.ResetWipingPage((int)mPlayerTowerInfo.cur_floor, (int)mPlayerTowerInfo.floor, restTime);
			});
		}
		else if (mPlayerTowerInfo.wipe_out_state == 2)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TowerWipeOutRootLogic, delegate
			{
				SingletonUnity<TowerWipeOutRootLogic>.Instance.ResetWipeOutRewardPage((int)mPlayerTowerInfo.cur_floor, (int)mPlayerTowerInfo.floor);
			});
			NoticeLogic.AddNotifyData("#{101522}");
		}
	}

	public void OnClickRankingBtn()
	{
		SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerRankInfoRoot, delegate
		{
			SingletonUnity<PlayerRankInfoRootLogic>.Instance.ResetToTower();
		});
	}

	public void OnClickRewardBtn()
	{
		if (mPlayerTowerInfo != null && mRewardFloorIndex > 0)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TowerWipeOutRootLogic, delegate
			{
				SingletonUnity<TowerWipeOutRootLogic>.Instance.ResetGetSpecialRewardPage(mRewardFloorIndex, mRewardFloorIndex <= mPlayerTowerInfo.floor && cangetSpecial);
			});
		}
	}

	private void SetRewardItem(List<string> itemIds, List<EQUIP_QUALITY> qualitys, List<int> counts)
	{
		ShowRewardRoot.ShowRewards(itemIds, qualitys, counts);
	}

	private void TextureLoadFinish(string name, Texture tex)
	{
		CopyBGTexture.mainTexture = tex;
	}

	public bool CheckLevel()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(mCurCopyScene.MinLevel);
	}

	public bool CheckLevel(int minLevel, int maxLevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minLevel, maxLevel);
	}

	public void UpdateWipeoutItem(string item_id)
	{
		if (mCurCopyScene.CanWipeOut == 1 && item_id.Equals(mCurCopyScene.WipeItem))
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			int itemStackNumById = playerData.ItemBackPack.GetItemStackNumById(mCurCopyScene.WipeItem);
		}
	}

	public void OnClickStartBtn()
	{
		if (CurSexMiniData != null)
		{
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(CurSexMiniData.UnlockLevel))
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100313}", CurSexMiniData.UnlockLevel));
				return;
			}
			string empty = string.Empty;
			empty = ((SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession != PROFESSION_TYPE.NQS) ? CurSexMiniData.WomenNpcId : CurSexMiniData.ManNpcId);
			SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
			MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			missionManager.AutoMoveDest(CurSexMiniData.MapId, DataManager.GetNPCPosInMonsterData(CurSexMiniData.MapId, empty), AUTO_SEARCH_PARTH_FINISHEVENT.FIND_NPC, empty);
		}
		else if (mPlayerTowerInfo != null)
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.TOWER_CLICK_START)
			{
				CheckTutorialEvent();
			}
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			int condition = DataManager.GetFunctionDataById(4003.ToString()).Condition;
			if (!playerData.CheckLevel(condition))
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", condition));
				return;
			}
			int num = (int)mPlayerTowerInfo.cur_floor;
			if (num >= MAX_FLOOR_NUM)
			{
				num = MAX_FLOOR_NUM - 1;
			}
			int limitLevel = DataManager.GetTowerDataByFloorID(num).LimitLevel;
			if (!playerData.CheckLevel(limitLevel))
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{102083}", limitLevel));
				return;
			}
			if (mPlayerTowerInfo.wipe_out_state == 1)
			{
				NoticeLogic.AddNotifyData("#{102035}");
				return;
			}
			if (mPlayerTowerInfo.cur_floor >= mPlayerTowerInfo.max_floor)
			{
				NoticeLogic.AddNotifyData("#{101523}");
				return;
			}
			NetLogic.GetInstance().Send<Protocol.request_tower_copy_info>();
			enter_tower_copy_info.request request = new enter_tower_copy_info.request();
			request.floorID = (int)mPlayerTowerInfo.cur_floor;
			NetLogic.GetInstance().Send<Protocol.enter_tower_copy_info>(request);
			int num2 = (int)mPlayerTowerInfo.cur_floor / 5 * 5;
			int num3 = (int)mPlayerTowerInfo.cur_floor / 5 * 5 + 4;
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tower", $"stage{num2}_{num3}", "starttimes");
		}
		else if (mCurCopyScene != null && mCurCopyScene.IsScuffleCopy)
		{
			if (!CheckLevel())
			{
				TutorialManager.LevelLimitAction();
			}
			else if (CurMissiondata != null)
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
				MissionManager.ClickMissionAction(CurMissiondata.ID);
			}
		}
		else if (onClickStartBtn != null)
		{
			onClickStartBtn();
		}
	}

	public void OnClickRankBtn()
	{
		if (onClickRankBtn != null)
		{
			onClickRankBtn();
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<NewMapUIRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID);
		SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjNormal();
		if (onClickCloseBtn != null)
		{
			onClickCloseBtn();
		}
	}

	public void RefershTowerBtn()
	{
		if (mPlayerTowerInfo.cur_floor <= mPlayerTowerInfo.floor)
		{
			WipeOutBtnPic.spriteName = GameDefine.BtnIcon[0];
			WipeoutTipsPic.enabled = true;
		}
		else
		{
			WipeOutBtnPic.spriteName = GameDefine.BtnIcon[2];
			WipeoutTipsPic.enabled = false;
		}
		if (mPlayerTowerInfo.wipe_out_state == 1)
		{
			NGUITools.SetActive(WappingRestTimeLabel.gameObject, state: true);
			wippingRestTime = (int)(mPlayerTowerInfo.wipe_time - SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime());
			secondTimeCount = wippingRestTime;
			WipeoutTipsPic.enabled = false;
			WipeOutlabel.text = StrDictionary.GetDictionaryString("#{102034}");
		}
		else if (mPlayerTowerInfo.wipe_out_state == 2)
		{
			WipeoutTipsPic.enabled = true;
			NGUITools.SetActive(WappingRestTimeLabel.gameObject, state: false);
			WipeOutlabel.text = StrDictionary.GetDictionaryString("#{300402}");
		}
		else
		{
			NGUITools.SetActive(WappingRestTimeLabel.gameObject, state: false);
			WipeOutlabel.text = StrDictionary.GetDictionaryString("#{101502}");
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		int condition = DataManager.GetFunctionDataById(4003.ToString()).Condition;
		int num = (int)mPlayerTowerInfo.cur_floor;
		if (num >= MAX_FLOOR_NUM)
		{
			num = MAX_FLOOR_NUM - 1;
		}
		int limitLevel = DataManager.GetTowerDataByFloorID(num).LimitLevel;
		if (!playerData.CheckLevel(condition))
		{
			StartBtnPic.spriteName = GameDefine.BtnIcon[2];
		}
		else if (mPlayerTowerInfo.wipe_out_state == 0L)
		{
			if (playerData.CheckLevel(limitLevel))
			{
				StartBtnPic.spriteName = GameDefine.BtnIcon[1];
			}
			else
			{
				StartBtnPic.spriteName = GameDefine.BtnIcon[2];
			}
		}
		else
		{
			StartBtnPic.spriteName = GameDefine.BtnIcon[2];
		}
		if (mPlayerTowerInfo.cur_floor != 0L && mPlayerTowerInfo.times > 0 && mPlayerTowerInfo.wipe_out_state != 1)
		{
			ResetBtnPic.spriteName = GameDefine.BtnIcon[1];
		}
		else
		{
			ResetBtnPic.spriteName = GameDefine.BtnIcon[2];
		}
	}

	private void Update()
	{
		if (mPlayerTowerInfo == null)
		{
			return;
		}
		if (mPlayerTowerInfo.wipe_out_state == 1)
		{
			if (!UnityVersionUtil.IsActive(WappingRestTimeLabel.gameObject))
			{
				NGUITools.SetActive(WappingRestTimeLabel.gameObject, state: true);
			}
			secondTimeCount -= Time.deltaTime;
			if ((int)secondTimeCount != wippingRestTime)
			{
				wippingRestTime = (int)secondTimeCount;
				if (wippingRestTime >= 0)
				{
					WappingRestTimeLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{101580}"), new TimeSpan(0, 0, wippingRestTime));
				}
				else
				{
					WappingRestTimeLabel.text = string.Empty;
				}
				if (wippingRestTime <= -2)
				{
					mPlayerTowerInfo.wipe_out_state = 2L;
					NetLogic.GetInstance().Send<Protocol.request_tower_copy_info>();
				}
			}
		}
		else if (UnityVersionUtil.IsActive(WappingRestTimeLabel.gameObject))
		{
			NGUITools.SetActive(WappingRestTimeLabel.gameObject, state: false);
		}
	}

	public void OnClicktishiBtn()
	{
		if (mCurCopyScene != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
			{
				PlayerCommonData playerCommonData3 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
				SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", mCurCopyScene.Rule, null, TimeTools.GetLocalShowTime_HM(playerCommonData3.ResetTime, playerCommonData3.TimeOffset));
			});
		}
		else if (mPlayerTowerInfo != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
			{
				PlayerCommonData playerCommonData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
				SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", "#{101616}", null, TimeTools.GetLocalShowTime_HM(playerCommonData2.ResetTime, playerCommonData2.TimeOffset));
			});
		}
		else if (CurSexMiniData != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
			{
				PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
				SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", CurSexMiniData.Rule, null, TimeTools.GetLocalShowTime_HM(playerCommonData.ResetTime, playerCommonData.TimeOffset));
			});
		}
	}

	public void SelectDailyCopyMapItem()
	{
		List<ActivityMapData> activityMapDataByType = DataManager.GetActivityMapDataByType(10, mCurCopyScene.SubType);
		if (activityMapDataByType == null)
		{
			return;
		}
		List<ActivityMapData> list = new List<ActivityMapData>();
		for (int i = 0; i < activityMapDataByType.Count; i++)
		{
			if (activityMapDataByType[i].IsNeedDailyActid())
			{
				if (activityMapDataByType[i].IsVisible && activityMapDataByType[i].ActivityID.Equals(mCurCopyScene.ID))
				{
					list.Add(activityMapDataByType[i]);
				}
			}
			else if (activityMapDataByType[i].IsVisible)
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

	public void SelectShowINMapByType(GameDefine.ACTIVITY_TYPE needtype)
	{
		List<ActivityMapData> activityMapDataByType = DataManager.GetActivityMapDataByType((int)needtype, ActivityMapData.DefaultSubType);
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

	public void OnClickMatchBtn()
	{
		if (onClickMatchBtn != null)
		{
			onClickMatchBtn();
		}
	}

	public void OnClickSingleBtn()
	{
		if (mCurCopyScene == null)
		{
			return;
		}
		if (!CheckLevel())
		{
			TutorialManager.LevelLimitAction();
			return;
		}
		remainNum = (int)mCurCopyInfo.CurNum;
		if (remainNum <= 0)
		{
			if (!string.IsNullOrEmpty(mCurCopyScene.TimeInc))
			{
				PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				int itemStackNumById = playerData.ItemBackPack.GetItemStackNumById(mCurCopyScene.TimeInc);
				if (itemStackNumById > 0)
				{
					ItemData itemDataByID = DataManager.GetItemDataByID(mCurCopyScene.TimeInc);
					MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{101598}", itemDataByID.MName), "#{100127}", delegate
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
			enter_copy_scene.request request = new enter_copy_scene.request();
			request.mapInfoId = mCurCopyScene.ID;
			NetLogic.GetInstance().Send<Protocol.enter_copy_scene>(request);
			CheckStartFlurry();
		}
	}

	public void CheckStartFlurry()
	{
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("DailyCopy", $"copy_{mCurCopyScene.ID}", "start");
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
					SetMatchAnima(isshow: true);
				}
				else
				{
					MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{101501}");
					SetMatchAnima(isshow: false);
				}
			}
			else
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{101501}");
				SetMatchAnima(isshow: false);
			}
		}
		else if (playerData.IsHaveTeam())
		{
			if (playerData.TeamInfo.IsVertify && mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{102007}");
				SetMatchAnima(isshow: true);
			}
			else
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{101501}");
				SetMatchAnima(isshow: false);
			}
		}
		else if (playerData.TeamInfo.IsVertify)
		{
			if (mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{102007}");
				SetMatchAnima(isshow: true);
			}
			else
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{101501}");
				SetMatchAnima(isshow: false);
			}
		}
		else
		{
			MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{101501}");
			SetMatchAnima(isshow: false);
		}
	}

	public void SetMatchAnima(bool isshow)
	{
		if (isshow)
		{
			MatchAnima.enabled = true;
			MatchAnima.PlayForward();
			IsMatching = true;
		}
		else
		{
			MatchAnima.ResetToBeginning();
			MatchAnima.enabled = false;
			IsMatching = false;
		}
	}
}
