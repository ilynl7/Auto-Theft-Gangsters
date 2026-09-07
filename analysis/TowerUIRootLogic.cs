using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class TowerUIRootLogic : SingletonUnity<TowerUIRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	private int MAX_FLOOR_NUM;

	public UIWrapContentNew uiWrapContent;

	public UIWidget WrapContentBottomWidget;

	public UILabel BestFloorLabel;

	public UILabel ResetNumLabel;

	public List<TowerFloorInfoLogic> FloorBtnList;

	public ShowRewardItems curShowRewardItemScripts;

	public UIScrollView FloorLineScrollView;

	public UIScrollBar FloorLineScrollBar;

	public UISprite StartBtnPic;

	public UISprite WipeOutBtnPic;

	public UILabel WipeOutlabel;

	public UISprite ResetBtnPic;

	public UISprite WipeoutTipsPic;

	public UISprite RewardTipPic;

	public UILabel WappingRestTimeLabel;

	public UISprite SpecialRewardBtn;

	public UILabel RewardTipsLabel;

	private int mCurChoosedFloorIndex = -1;

	private int mPlayerCurFloor;

	private int mPlayerTimes;

	private int mWipeOutTimes;

	public UITexture CopyBG;

	private tower_info mPlayerTowerInfo;

	private List<tower_special_reward> mPlayerSpecialRewardList = new List<tower_special_reward>();

	private long mServerTime;

	private TowerData mCurTowerData;

	private long mRestFreshTime;

	private float mReceiveServertime;

	private int mRewardFloorIndex = -1;

	private string RemainTimeStr;

	private bool cangetSpecial;

	private int wippingRestTime;

	private float secondTimeCount;

	public tower_info PlayerTowerInfo => mPlayerTowerInfo;

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
			onClickTutorialBtn = null;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		Init();
		RemainTimeStr = StrDictionary.GetDictionaryString("#{101580}");
	}

	private void Init()
	{
		for (int i = 0; i < FloorBtnList.Count; i++)
		{
			FloorBtnList[i].Init(OnClickFloorBtn, i);
		}
		UIWrapContentNew uIWrapContentNew = uiWrapContent;
		uIWrapContentNew.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContentNew.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeFloorItem));
		TowerData towerDataByFloorID = DataManager.GetTowerDataByFloorID(0);
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBG.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if ((CopyBG.mainTexture == null || !CopyBG.mainTexture.name.Equals(towerDataByFloorID.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(towerDataByFloorID.Background, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(string name, Texture tex)
	{
		CopyBG.mainTexture = tex;
	}

	public void EnableReset()
	{
		for (int i = 0; i < FloorBtnList.Count; i++)
		{
			NGUITools.SetActive(FloorBtnList[i].gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(curShowRewardItemScripts.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(RewardTipPic.gameObject, state: false);
		mPlayerTowerInfo = null;
		WipeoutTipsPic.enabled = false;
	}

	private void OnInitializeFloorItem(GameObject obj, int index, int realIndex)
	{
		TowerFloorInfoLogic towerFloorInfoLogic = FloorBtnList[index];
		realIndex = Mathf.Abs(realIndex);
		bool isShow = false;
		bool isEnable = false;
		if (mPlayerSpecialRewardList != null && mPlayerSpecialRewardList.Count != 0)
		{
			for (int i = 0; i < mPlayerSpecialRewardList.Count; i++)
			{
				if (mPlayerSpecialRewardList[i].floor == realIndex)
				{
					isShow = mPlayerSpecialRewardList[i].state == 0L;
					isEnable = ((mPlayerSpecialRewardList[i].state == 0L && mPlayerSpecialRewardList[i].floor <= mPlayerTowerInfo.floor) ? true : false);
					break;
				}
			}
		}
		else
		{
			isShow = false;
			isEnable = false;
		}
		towerFloorInfoLogic.Reset(realIndex, mPlayerTowerInfo != null && realIndex == mCurChoosedFloorIndex, (int)((mPlayerTowerInfo == null) ? (-1) : mPlayerTowerInfo.cur_floor), isShow, isEnable);
	}

	public void UpdateTowerInfo(tower_info info)
	{
		mPlayerTowerInfo = info;
		mReceiveServertime = Time.time;
		MAX_FLOOR_NUM = (int)Mathf.Min(mPlayerTowerInfo.floor + 20, mPlayerTowerInfo.max_floor);
		uiWrapContent.maxIndex = 0;
		uiWrapContent.minIndex = -MAX_FLOOR_NUM + 1;
		WrapContentBottomWidget.height = MAX_FLOOR_NUM * uiWrapContent.itemSize;
		cangetSpecial = false;
		if (mPlayerSpecialRewardList != null && mPlayerSpecialRewardList.Count != 0)
		{
			for (int i = 0; i < mPlayerSpecialRewardList.Count; i++)
			{
				if (mPlayerSpecialRewardList[i].state == 0L)
				{
					if (i == 0 && mPlayerSpecialRewardList[i].floor <= mPlayerTowerInfo.floor)
					{
						TutorialManager.ShowTutorial(TUTORIAL_STEP.TOWER_SPECIAL_TIPS);
					}
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
		mCurChoosedFloorIndex = (int)mPlayerTowerInfo.cur_floor;
		UpdateFloorInfo(mCurChoosedFloorIndex);
		uiWrapContent.SortBasedOnScrollMovement();
		FloorLineScrollBar.value = Mathf.Clamp01(((float)mCurChoosedFloorIndex + (float)(mCurChoosedFloorIndex - (MAX_FLOOR_NUM - 1) / 2) * 2f / (float)((MAX_FLOOR_NUM - 1) / 2)) / (float)(MAX_FLOOR_NUM - 1));
		FloorLineScrollBar.ForceUpdate();
		FloorLineScrollView.UpdatePosition();
		uiWrapContent.ForceWrapContent();
		for (int j = 0; j < FloorBtnList.Count; j++)
		{
			if (FloorBtnList[j].CurFloorIndex == mCurChoosedFloorIndex)
			{
				SetSelectPic(mCurChoosedFloorIndex, j);
			}
		}
		RefershBtn();
	}

	public void RefershBtn()
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
		if (mPlayerTowerInfo.wipe_out_state == 0L)
		{
			StartBtnPic.spriteName = GameDefine.BtnIcon[1];
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

	public void UpdateTowerInfo(ret_grant_tower_reward.request request)
	{
		mPlayerTowerInfo = request.tower_info;
		mReceiveServertime = Time.time;
		MAX_FLOOR_NUM = (int)Mathf.Min(mPlayerTowerInfo.floor + 20, mPlayerTowerInfo.max_floor);
		uiWrapContent.maxIndex = 0;
		uiWrapContent.minIndex = -MAX_FLOOR_NUM + 1;
		WrapContentBottomWidget.height = MAX_FLOOR_NUM * uiWrapContent.itemSize;
		if (request.HasTower_special_reward)
		{
			mPlayerSpecialRewardList = request.tower_special_reward;
			mPlayerSpecialRewardList.Sort((tower_special_reward x, tower_special_reward y) => (int)(x.floor - y.floor));
		}
		mCurChoosedFloorIndex = (int)mPlayerTowerInfo.cur_floor;
		cangetSpecial = false;
		if (mPlayerSpecialRewardList != null && mPlayerSpecialRewardList.Count != 0)
		{
			for (int i = 0; i < mPlayerSpecialRewardList.Count; i++)
			{
				if (mPlayerSpecialRewardList[i].state == 0L)
				{
					if (i == 0 && mPlayerSpecialRewardList[i].floor <= mPlayerTowerInfo.floor)
					{
						TutorialManager.ShowTutorial(TUTORIAL_STEP.TOWER_SPECIAL_TIPS);
					}
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
		UpdateFloorInfo(mCurChoosedFloorIndex);
		uiWrapContent.SortBasedOnScrollMovement();
		FloorLineScrollBar.value = Mathf.Clamp01(((float)mCurChoosedFloorIndex + (float)(mCurChoosedFloorIndex - (MAX_FLOOR_NUM - 1) / 2) * 2f / (float)((MAX_FLOOR_NUM - 1) / 2)) / (float)(MAX_FLOOR_NUM - 1));
		FloorLineScrollBar.ForceUpdate();
		FloorLineScrollView.UpdatePosition();
		uiWrapContent.ForceWrapContent();
		for (int j = 0; j < FloorBtnList.Count; j++)
		{
			if (FloorBtnList[j].CurFloorIndex == mCurChoosedFloorIndex)
			{
				SetSelectPic(mCurChoosedFloorIndex, j);
			}
		}
		RefershBtn();
	}

	public void UpdateTowerCopyInfo(ret_request_tower_copy_info.request request)
	{
		if (request.HasTower_info)
		{
			mPlayerTowerInfo = request.tower_info;
			mReceiveServertime = Time.time;
			for (int i = 0; i < FloorBtnList.Count; i++)
			{
				NGUITools.SetActive(FloorBtnList[i].gameObject, state: true);
			}
			MAX_FLOOR_NUM = (int)Mathf.Min(mPlayerTowerInfo.floor + 20, mPlayerTowerInfo.max_floor);
			uiWrapContent.maxIndex = 0;
			uiWrapContent.minIndex = -MAX_FLOOR_NUM + 1;
			WrapContentBottomWidget.height = MAX_FLOOR_NUM * uiWrapContent.itemSize;
		}
		if (request.HasTower_special_reward)
		{
			mPlayerSpecialRewardList = request.tower_special_reward;
			mPlayerSpecialRewardList.Sort((tower_special_reward x, tower_special_reward y) => (int)(x.floor - y.floor));
		}
		cangetSpecial = false;
		if (mPlayerSpecialRewardList != null && mPlayerSpecialRewardList.Count != 0)
		{
			for (int j = 0; j < mPlayerSpecialRewardList.Count; j++)
			{
				if (mPlayerSpecialRewardList[j].state == 0L)
				{
					if (j == 0 && mPlayerSpecialRewardList[j].floor <= mPlayerTowerInfo.floor)
					{
						TutorialManager.ShowTutorial(TUTORIAL_STEP.TOWER_SPECIAL_TIPS);
					}
					mRewardFloorIndex = (int)mPlayerSpecialRewardList[j].floor;
					NGUITools.SetActive(RewardTipPic.gameObject, mPlayerSpecialRewardList[j].floor <= mPlayerTowerInfo.floor);
					cangetSpecial = true;
					break;
				}
			}
		}
		mCurChoosedFloorIndex = (int)mPlayerTowerInfo.cur_floor;
		UpdateFloorInfo(mCurChoosedFloorIndex);
		uiWrapContent.SortBasedOnScrollMovement();
		FloorLineScrollBar.value = Mathf.Clamp01(((float)mCurChoosedFloorIndex + (float)(mCurChoosedFloorIndex - (MAX_FLOOR_NUM - 1) / 2) * 2f / (float)((MAX_FLOOR_NUM - 1) / 2)) / (float)(MAX_FLOOR_NUM - 1));
		FloorLineScrollBar.ForceUpdate();
		FloorLineScrollView.UpdatePosition();
		uiWrapContent.ForceWrapContent();
		for (int k = 0; k < FloorBtnList.Count; k++)
		{
			if (FloorBtnList[k].CurFloorIndex == mCurChoosedFloorIndex)
			{
				SetSelectPic(mCurChoosedFloorIndex, k);
			}
		}
		if (!cangetSpecial)
		{
			NGUITools.SetActive(RewardTipPic.gameObject, state: false);
		}
		RefershBtn();
	}

	public void UpdateFloorInfo(int floorId)
	{
		if (floorId >= MAX_FLOOR_NUM)
		{
			RewardTipsLabel.enabled = false;
			NGUITools.SetActive(curShowRewardItemScripts.gameObject, state: false);
		}
		else
		{
			RewardTipsLabel.enabled = true;
			NGUITools.SetActive(curShowRewardItemScripts.gameObject, state: true);
			if (mRewardFloorIndex > 0)
			{
				TowerData towerDataByFloorID = DataManager.GetTowerDataByFloorID(mRewardFloorIndex);
				if (towerDataByFloorID.IsShowReward == 1)
				{
					ShowRewardData showRewardDataByID = DataManager.GetShowRewardDataByID(towerDataByFloorID.SpecialRewardId);
					curShowRewardItemScripts.ShowRewards(showRewardDataByID.ItemIdList, showRewardDataByID.QualityList, showRewardDataByID.CountList);
					if (GameManager.IsSupportCurDataVersion137())
					{
						RewardTipsLabel.text = StrDictionary.GetDictionaryString("#{102060}", towerDataByFloorID.FloorID + 1);
					}
					else
					{
						RewardTipsLabel.text = "Reward:";
					}
				}
				else
				{
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
								curShowRewardItemScripts.ShowRewards(showRewardDataByID2.ItemIdList, showRewardDataByID2.QualityList, showRewardDataByID2.CountList);
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
						curShowRewardItemScripts.ShowRewards(showRewardDataByID3.ItemIdList, showRewardDataByID3.QualityList, showRewardDataByID3.CountList);
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
			}
			else
			{
				TowerData towerDataByFloorID2 = DataManager.GetTowerDataByFloorID(floorId);
				ShowRewardData showRewardDataByID4 = DataManager.GetShowRewardDataByID(towerDataByFloorID2.ShowRewardID);
				curShowRewardItemScripts.ShowRewards(showRewardDataByID4.ItemIdList, showRewardDataByID4.QualityList, showRewardDataByID4.CountList);
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
		BestFloorLabel.text = $"{mPlayerTowerInfo.floor + 1}";
		ResetNumLabel.text = string.Format("{0}:{1}/1", StrDictionary.GetDictionaryString("#{101528}"), mPlayerTowerInfo.times);
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

	public void OnClickStartBtn()
	{
		if (mPlayerTowerInfo != null)
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.TOWER_CLICK_START)
			{
				CheckTutorialEvent();
			}
			if (mPlayerTowerInfo.wipe_out_state == 1)
			{
				NoticeLogic.AddNotifyData("#{102035}");
				return;
			}
			if (mPlayerTowerInfo.cur_floor >= MAX_FLOOR_NUM)
			{
				NoticeLogic.AddNotifyData("#{101523}");
				return;
			}
			NetLogic.GetInstance().Send<Protocol.request_tower_copy_info>();
			enter_tower_copy_info.request request = new enter_tower_copy_info.request();
			request.floorID = mCurChoosedFloorIndex;
			NetLogic.GetInstance().Send<Protocol.enter_tower_copy_info>(request);
			int num = mCurChoosedFloorIndex / 5 * 5;
			int num2 = mCurChoosedFloorIndex / 5 * 5 + 4;
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tower", $"stage{num}_{num2}", "starttimes");
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
		if (mPlayerTowerInfo == null)
		{
			return;
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.TOWER_SPECIAL_TIPS)
		{
			CheckTutorialEvent();
		}
		if (mRewardFloorIndex > 0)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TowerWipeOutRootLogic, delegate
			{
				SingletonUnity<TowerWipeOutRootLogic>.Instance.ResetGetSpecialRewardPage(mRewardFloorIndex, mRewardFloorIndex <= mPlayerTowerInfo.floor && cangetSpecial);
			});
		}
	}

	public void OnClickFloorBtn(int floorIndex, int itemIndex)
	{
	}

	private void SetSelectPic(int floorIndex, int itemIndex)
	{
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
					WappingRestTimeLabel.text = $"{RemainTimeStr}:{new TimeSpan(0, 0, wippingRestTime)}";
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
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
		{
			PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", "#{101616}", null, TimeTools.GetLocalShowTime_HM(playerCommonData.ResetTime, playerCommonData.TimeOffset));
		});
	}

	private void OnDisable()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.TOWER_CLICK_START)
		{
			CheckTutorialEvent();
		}
	}
}
