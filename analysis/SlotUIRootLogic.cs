using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SlotUIRootLogic : SingletonUnity<SlotUIRootLogic>
{
	public enum SpinState
	{
		waitResult,
		getResult
	}

	private UI_PAGE_TYPE mPrePage = UI_PAGE_TYPE.INVALID;

	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public List<SlotLineLogic> slotlineslist;

	private SpinState curState;

	private SLOTTYPE curSlotType;

	public UILabel FreeTimesLabel;

	public UILabel PriceLabel;

	public UISlider sumSlider;

	public GameObject AutoObj;

	public List<ShowRewardItems> bigwinItems;

	public TweenAlpha[] LightsStartAnima;

	public TweenRotation[] DirsAnima;

	public TweenAlpha[] LightsEndAnima;

	public UIWidget[] AnimaAlphWis;

	public GameObject BigWinObj;

	public UISprite[] RotateSp;

	public UILabel[] rollTimesLabel;

	public UILabel[] PriceLabels;

	private Dictionary<string, slot_data> SlotDataDic;

	private List<slot_data> SlotDataList;

	private slot_info curSlotinfo;

	private slot_data resultSlotdata;

	private List<slot_item> SlotItemList = new List<slot_item>();

	private List<item> AutoAllGetItemList = new List<item>();

	private List<item> sunItemList = new List<item>();

	private List<item> AllGetItemList = new List<item>();

	private List<SlotIconData> icondataList = new List<SlotIconData>();

	private List<SlotAutoData> AutoDataList = new List<SlotAutoData>();

	private int SumMaxLimit = 10;

	public bool isSpinFlag;

	public string[] resultStr;

	private int linemoveStopnum;

	public GameObject UpColliderObj;

	private float MaxSpinTime = 5f;

	private float MinSpinTime = 1.5f;

	private float AutoMinSpinTime = 1f;

	private float StartTime;

	private float SpinTime;

	private float lineDeltime = 0.3f;

	public bool IsAutoFlag;

	public bool IsResultOver;

	public int AutoRollCount;

	public UILabel AutoLabel;

	public GameObject SpinSp;

	private float autoTime;

	private float autoDelTime = 1.5f;

	private bool autoIsopen;

	public UISprite spinTips;

	private bool isBigWinFlag;

	public UIPlayTween SumAnima;

	public ShowRewardItems sumShowrewards;

	private bool isautoGetSum;

	public Transform totalTra;

	public TweenScale starAnima;

	public UISprite starEffectSp;

	public void CheckPrePage()
	{
		if (mPrePage != UI_PAGE_TYPE.INVALID)
		{
			switch (mPrePage)
			{
			case UI_PAGE_TYPE.BACK_PACK_ITEM:
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
				{
					SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickItemBackPackBtn();
				});
				break;
			case UI_PAGE_TYPE.ENHANCE_EQUIP:
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
				{
					SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance();
				});
				break;
			case UI_PAGE_TYPE.REFINE:
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
				{
					SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipRefine();
				});
				break;
			}
		}
		mPrePage = UI_PAGE_TYPE.INVALID;
	}

	public void SetPrePage(UI_PAGE_TYPE prePage)
	{
		mPrePage = prePage;
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

	public void EnableReset()
	{
		curSlotinfo = null;
		PlaySlotBgm();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			List<MenuTabBtnInfo> leftBtnInfo = new List<MenuTabBtnInfo>();
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(leftBtnInfo, OnClickCloseBtn, hideTab: true);
			SingletonUnity<MenuBaseRootLogic>.Instance.SetPageLabel(StrDictionary.GetDictionaryString("#{100109}"));
		});
		UnityVersionUtil.SetActiveRecursive(BigWinObj.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(AutoObj.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(SumAnima.gameObject, state: false);
		NGUITools.SetActive(FreeTimesLabel.gameObject, state: false);
		autoIsopen = false;
		UnityVersionUtil.SetActiveRecursive(UpColliderObj.gameObject, state: false);
		icondataList = DataManager.GetSlotIconDataList();
		icondataList.Sort((SlotIconData x, SlotIconData y) => int.Parse(x.ID) - int.Parse(y.ID));
		for (int i = 0; i < slotlineslist.Count; i++)
		{
			slotlineslist[i].Reset(icondataList, i);
		}
		PlaySpinEndAnima();
		SlotItemList.Clear();
		AutoAllGetItemList.Clear();
		mPrePage = UI_PAGE_TYPE.INVALID;
		starAnima.enabled = false;
		starEffectSp.enabled = false;
		starAnima.transform.localScale = Vector3.one;
	}

	public void Reset(ret_slot_info.request request)
	{
		SlotDataDic = request.slot_datas;
		SlotDataList = new List<slot_data>(SlotDataDic.Values);
		curSlotinfo = request.slot_info;
		SlotItemList = new List<slot_item>();
		AutoAllGetItemList.Clear();
		if (request.HasSlot_items)
		{
			SlotItemList = new List<slot_item>(request.slot_items.Values);
			curSlotinfo.sumNum -= SlotItemList.Count;
			if (curSlotinfo.sumNum < 0)
			{
				Debug.LogError("slot sum num < 0");
			}
		}
		for (int i = 0; i < SlotDataList.Count; i++)
		{
			if (SlotDataList[i].Rank <= 3 && SlotDataList[i].Rank > 0)
			{
				ShowRewardData showRewardDataByID = DataManager.GetShowRewardDataByID(SlotDataList[i].ShowRewardID);
				if (showRewardDataByID != null)
				{
					UnityVersionUtil.SetActiveRecursive(bigwinItems[(int)SlotDataList[i].Rank - 1].gameObject, state: true);
					bigwinItems[(int)SlotDataList[i].Rank - 1].ShowRewards(new List<string>(showRewardDataByID.ItemIdList), new List<EQUIP_QUALITY>(showRewardDataByID.QualityList), new List<int>(showRewardDataByID.CountList));
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(bigwinItems[(int)SlotDataList[i].Rank - 1].gameObject, state: false);
				}
			}
		}
		AllGetItemList.Clear();
		isSpinFlag = false;
		IsAutoFlag = false;
		AutoRollCount = 0;
		IsResultOver = true;
		ResetAutoInfo();
		refershUI();
		CheckAutoitems();
		if (TutorialManager.CurStep == TUTORIAL_STEP.SLOT_WAIT_DATA)
		{
			CheckTutorialEvent();
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Slot", "open", "opentimes");
	}

	public void CheckAutoitems()
	{
		if (SlotItemList.Count > 0)
		{
			MessageBoxLogic.OpenOkBox_One(StrDictionary.GetDictionaryString("#{301005}", SlotItemList.Count), "#{100127}", BackYesAutoRoll);
		}
	}

	public void BackYesAutoRoll()
	{
		if (!IsAutoFlag)
		{
			IsAutoFlag = true;
			AutoRollCount = SlotItemList.Count;
			DoAutoSpin();
			UnityVersionUtil.SetActiveRecursive(UpColliderObj.gameObject, state: true);
			refershUI();
		}
	}

	public void refershUI()
	{
		if (curSlotinfo.curNum > 0)
		{
			FreeTimesLabel.text = string.Format("{0} x{1}", StrDictionary.GetDictionaryString("#{301003}"), curSlotinfo.curNum);
			NGUITools.SetActive(FreeTimesLabel.gameObject, state: true);
			NGUITools.SetActive(PriceLabel.gameObject, state: false);
			spinTips.enabled = true;
		}
		else
		{
			PriceLabel.text = GameMoneyHelper.GetMoneyValStr(AutoDataList[0].PriceCost, AutoDataList[0].PriceType);
			NGUITools.SetActive(FreeTimesLabel.gameObject, state: false);
			NGUITools.SetActive(PriceLabel.gameObject, state: true);
			spinTips.enabled = false;
		}
		sumSlider.value = (float)curSlotinfo.sumNum / (float)SumMaxLimit;
		if (curSlotinfo.sumNum >= SumMaxLimit)
		{
			starAnima.enabled = true;
			starEffectSp.enabled = true;
		}
		else
		{
			starAnima.enabled = false;
			starEffectSp.enabled = false;
			starAnima.transform.localScale = Vector3.one;
		}
		if (AutoRollCount > 0)
		{
			AutoLabel.text = string.Format("{0} x{1}", StrDictionary.GetDictionaryString("#{301004}"), AutoRollCount);
		}
		else
		{
			AutoLabel.text = StrDictionary.GetDictionaryString("#{301004}");
		}
	}

	private void Update()
	{
		if (isSpinFlag && Time.time - StartTime > SpinTime)
		{
			if (linemoveStopnum >= 3)
			{
				return;
			}
			SpinTime += lineDeltime;
			slotlineslist[linemoveStopnum].StopRolling(resultStr[linemoveStopnum]);
		}
		if (!IsAutoFlag || !IsResultOver)
		{
			return;
		}
		autoTime += Time.deltaTime;
		if (!(autoTime > autoDelTime))
		{
			return;
		}
		if (AutoRollCount > 0)
		{
			autoTime = 0f;
			DoAutoSpin();
			return;
		}
		IsAutoFlag = false;
		UnityVersionUtil.SetActiveRecursive(UpColliderObj.gameObject, state: false);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SlotRewardRoot, delegate
		{
			SingletonUnity<SlotRewardRootLogic>.Instance.Reset(AutoAllGetItemList, SetShowRewardOver, showbtn: true);
		});
	}

	public void OnClickAutoBtn()
	{
		if (curSlotinfo != null)
		{
			if (autoIsopen)
			{
				NGUITools.SetActive(AutoObj, state: false);
				autoIsopen = false;
			}
			else
			{
				NGUITools.SetActive(AutoObj, state: true);
				autoIsopen = true;
			}
		}
	}

	public void OnClickAutoBtn1()
	{
		if (GameMoneyHelper.BeforeCheckBuy(AutoDataList[1].PriceType, AutoDataList[1].PriceCost))
		{
			StartAutoSpin(2);
			OnClickAutoBtn();
		}
	}

	public void OnClickAutoBtn2()
	{
		if (GameMoneyHelper.BeforeCheckBuy(AutoDataList[2].PriceType, AutoDataList[2].PriceCost))
		{
			StartAutoSpin(3);
			OnClickAutoBtn();
		}
	}

	public void OnClickAutoBtn3()
	{
		if (GameMoneyHelper.BeforeCheckBuy(AutoDataList[3].PriceType, AutoDataList[3].PriceCost))
		{
			StartAutoSpin(4);
			OnClickAutoBtn();
		}
	}

	public void OnClickSpinBtn()
	{
		if (curSlotinfo == null)
		{
			return;
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.SLOT_CLICK)
		{
			CheckTutorialEvent();
		}
		if (isSpinFlag || !IsResultOver)
		{
			return;
		}
		if (autoIsopen)
		{
			NGUITools.SetActive(AutoObj, state: false);
			autoIsopen = false;
		}
		if (curSlotinfo.curNum <= 0)
		{
			if (GameMoneyHelper.BeforeCheckBuy(AutoDataList[0].PriceType, AutoDataList[0].PriceCost))
			{
				DoSpin();
			}
		}
		else
		{
			DoSpin();
		}
	}

	public void DoSpin()
	{
		isSpinFlag = true;
		isBigWinFlag = false;
		IsResultOver = false;
		resultSlotdata = null;
		linemoveStopnum = 0;
		StartTime = Time.time;
		SpinTime = MaxSpinTime;
		SlotItemList.Clear();
		curState = SpinState.waitResult;
		UnityVersionUtil.SetActiveRecursive(UpColliderObj.gameObject, state: true);
		setNotwinResult();
		SpinMove();
		spin_slot.request request = new spin_slot.request();
		request.ID = "1";
		NetLogic.GetInstance().Send<Protocol.spin_slot>(request);
	}

	public void StartAutoSpin(int autoid)
	{
		if (!IsAutoFlag)
		{
			IsAutoFlag = true;
			AutoRollCount = AutoDataList[autoid - 1].RollNum;
			SlotItemList.Clear();
			AutoAllGetItemList.Clear();
			DoAutoSpin();
			spin_slot.request request = new spin_slot.request();
			request.ID = autoid.ToString();
			NetLogic.GetInstance().Send<Protocol.spin_slot>(request);
			UnityVersionUtil.SetActiveRecursive(UpColliderObj.gameObject, state: true);
			refershUI();
		}
	}

	public void DoAutoSpin()
	{
		isSpinFlag = true;
		isBigWinFlag = false;
		IsResultOver = false;
		resultSlotdata = null;
		linemoveStopnum = 0;
		StartTime = Time.time;
		setNotwinResult();
		if (SlotItemList.Count == 0)
		{
			SpinTime = MaxSpinTime;
			curState = SpinState.waitResult;
		}
		else
		{
			SetRollResult();
			SpinTime = AutoMinSpinTime;
			curState = SpinState.getResult;
		}
		SpinMove();
	}

	public void SpinMove()
	{
		PlayRollSound();
		PlaySpinStartAnima();
		stopBigWinAnima();
		for (int i = 0; i < slotlineslist.Count; i++)
		{
			slotlineslist[i].StartSpin(LineStopFun);
		}
	}

	public void UpdateResult(ret_spin_slot.request request)
	{
		int num = (int)curSlotinfo.curNum;
		curSlotinfo.curNum = request.slot_info.curNum;
		SlotItemList = new List<slot_item>(request.slot_items.Values);
		SetRollResult();
		if (Time.time - StartTime < MaxSpinTime)
		{
			curState = SpinState.getResult;
			SpinTime = MinSpinTime;
		}
		if (IsAutoFlag)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Slot", "spin", $"spin_{SlotItemList.Count}");
		}
		else if (num > 0)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Slot", "spin", "spin_free");
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Slot", "spin", "spin_1");
		}
	}

	public void SetRollResult()
	{
		resultSlotdata = SlotDataDic[SlotItemList[0].ID];
		SetWinResult(resultSlotdata.RewardMap);
		if (resultSlotdata.Rank <= 3)
		{
			isBigWinFlag = true;
		}
	}

	public void ShowResult()
	{
		autoTime = 0f;
		AutoGetSumReward();
		curSlotinfo.sumNum++;
		isSpinFlag = false;
		PlaySpinEndAnima();
		playResultAnima();
		if (SlotItemList.Count > 0 && SlotItemList[0].HasItems && SlotItemList[0].items.Count > 0)
		{
			if (IsAutoFlag)
			{
				AddAutoGetItemList(SlotItemList[0].items);
				if (isBigWinFlag)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SlotBigWinRoot, delegate
					{
						SingletonUnity<SlotBigWinRootLogic>.Instance.Reset(SlotItemList[0].items, SetShowRewardOver);
					});
				}
				else
				{
					SimpleRewardRootLogic.AddRewards(SlotItemList[0].items);
					SetShowRewardOver();
				}
			}
			else if (isBigWinFlag)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SlotBigWinRoot, delegate
				{
					SingletonUnity<SlotBigWinRootLogic>.Instance.Reset(SlotItemList[0].items, SetShowRewardOver);
				});
			}
			else
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SlotRewardRoot, delegate
				{
					SingletonUnity<SlotRewardRootLogic>.Instance.Reset(SlotItemList[0].items, SetShowRewardOver);
				});
			}
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Slot", "spin_win", $"spin_{SlotItemList[0].ID}");
		}
		else
		{
			SetShowRewardOver();
		}
		if (SlotItemList.Count > 0)
		{
			request_slot_reward.request request = new request_slot_reward.request();
			request.uuid = SlotItemList[0].uuid;
			NetLogic.GetInstance().Send<Protocol.request_slot_reward>(request);
			SaveGetitems(SlotItemList[0]);
			SlotItemList.RemoveAt(0);
		}
		if (!IsAutoFlag)
		{
			UnityVersionUtil.SetActiveRecursive(UpColliderObj.gameObject, state: false);
		}
		else
		{
			AutoRollCount--;
		}
		refershUI();
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		playerData.playerSlotData.SetInfo((int)curSlotinfo.curNum, (int)curSlotinfo.sumNum, SlotItemList.Count);
	}

	public void AddAutoGetItemList(List<item> newitems)
	{
		for (int i = 0; i < newitems.Count; i++)
		{
			AutoAllGetItemList.Add(newitems[i]);
		}
	}

	public void OnClickSumRewardBtn()
	{
		if (curSlotinfo != null && curSlotinfo.HasSumNum && curSlotinfo.sumNum >= SumMaxLimit)
		{
			isautoGetSum = false;
			sunItemList.Clear();
			curSlotinfo.sumNum = 0L;
			WaitResponseUIRootLogic.OpenWaitBox(244, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.request_slot_sum_reward>();
		}
	}

	public void AutoGetSumReward()
	{
		if (curSlotinfo.HasSumNum && curSlotinfo.sumNum >= SumMaxLimit)
		{
			isautoGetSum = true;
			sunItemList.Clear();
			curSlotinfo.sumNum = 0L;
			WaitResponseUIRootLogic.OpenWaitBox(244, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.request_slot_sum_reward>();
		}
	}

	public void ShowSumReward(List<item> sumitemlist, long sumresult)
	{
		sunItemList = sumitemlist;
		SaveGetitems(sumitemlist);
		curSlotinfo.sumNum = sumresult;
		curSlotinfo.sumNum -= SlotItemList.Count;
		if (isautoGetSum)
		{
			sumShowrewards.ShowRewards(sunItemList);
			UnityVersionUtil.SetActiveRecursive(SumAnima.gameObject, state: true);
			SumAnima.resetOnPlay = true;
			SumAnima.Play(forward: true);
		}
		else
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SlotRewardRoot, delegate
			{
				SingletonUnity<SlotRewardRootLogic>.Instance.Reset(sunItemList);
			});
		}
		refershUI();
	}

	public void OnClickTotalPrizesBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SlotLogRoot, delegate
		{
			SingletonUnity<SlotLogRootLogic>.Instance.ShowRewards(AllGetItemList);
		});
	}

	public void OnClickTishiBtn()
	{
		if (curSlotinfo != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SlotRuleRoot, delegate
			{
				SingletonUnity<SlotRuleRootLogic>.Instance.Reset(SlotDataList);
			});
		}
	}

	public void OnClickCloseBtn()
	{
		IsAutoFlag = false;
		isSpinFlag = true;
		IsResultOver = false;
		StopRollSound();
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SlotUIRoot);
		PlaySceneBgmMusic();
		CheckPrePage();
	}

	private void PlaySlotBgm()
	{
		SoundData soundDataById = DataManager.GetSoundDataById(8);
		if (soundDataById != null)
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.PlayBGMusic(soundDataById.Id, soundDataById.FadeOutTime, soundDataById.FadeInTime);
		}
	}

	private void PlayRollSound()
	{
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(11);
	}

	private void StopRollSound()
	{
		SingletonDontDestoryUnity<SoundManager>.Instance.StopSoundEffect(11);
	}

	private void PlayResultSound()
	{
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(10);
	}

	private void PlaySceneBgmMusic()
	{
		MapInfoData currentMapInofData = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData;
		SoundData soundDataById = DataManager.GetSoundDataById(currentMapInofData.GetAudioID());
		if (soundDataById != null)
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.PlayBGMusic(soundDataById.Id, soundDataById.FadeOutTime, soundDataById.FadeInTime);
		}
	}

	public void LineStopFun()
	{
		linemoveStopnum++;
		if (linemoveStopnum >= 3)
		{
			ShowResult();
		}
		else if (linemoveStopnum == 1)
		{
			StopDirAnima(0);
		}
	}

	public void PlaySpinStartAnima()
	{
		for (int i = 0; i < LightsEndAnima.Length; i++)
		{
			LightsEndAnima[i].enabled = false;
		}
		SetAnimaWiAlph();
		for (int j = 0; j < LightsStartAnima.Length; j++)
		{
			LightsStartAnima[j].ResetToBeginning();
			LightsStartAnima[j].PlayForward();
		}
		for (int k = 0; k < DirsAnima.Length; k++)
		{
			DirsAnima[k].PlayForward();
		}
	}

	public void PlaySpinEndAnima()
	{
		for (int i = 0; i < LightsStartAnima.Length; i++)
		{
			LightsStartAnima[i].enabled = false;
		}
		SetAnimaWiAlph();
		for (int j = 0; j < LightsEndAnima.Length; j++)
		{
			LightsEndAnima[j].ResetToBeginning();
			LightsEndAnima[j].PlayForward();
		}
		for (int k = 0; k < DirsAnima.Length; k++)
		{
			DirsAnima[k].enabled = false;
			DirsAnima[k].transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
	}

	public void StopDirAnima(int i)
	{
		DirsAnima[i].enabled = false;
		DirsAnima[i].transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	public void SetAnimaWiAlph()
	{
		for (int i = 0; i < AnimaAlphWis.Length; i++)
		{
			AnimaAlphWis[i].alpha = 1f;
		}
	}

	public void SetShowRewardOver()
	{
		IsResultOver = true;
	}

	public void playResultAnima()
	{
		StopRollSound();
		if (IsAutoFlag)
		{
			PlayResultSound();
		}
		UnityVersionUtil.SetActiveRecursive(BigWinObj, state: true);
		switch (curSlotType)
		{
		case SLOTTYPE.XXX:
		{
			for (int i = 0; i < slotlineslist.Count; i++)
			{
				slotlineslist[i].lineitems[4].PlayeAnima();
				RotateSp[i].enabled = true;
			}
			break;
		}
		case SLOTTYPE.AXC:
			slotlineslist[1].lineitems[4].PlayeAnima();
			RotateSp[1].enabled = true;
			RotateSp[2].enabled = false;
			RotateSp[0].enabled = false;
			break;
		case SLOTTYPE.BXX:
			slotlineslist[1].lineitems[4].PlayeAnima();
			slotlineslist[2].lineitems[4].PlayeAnima();
			RotateSp[1].enabled = true;
			RotateSp[2].enabled = true;
			RotateSp[0].enabled = false;
			break;
		case SLOTTYPE.XBX:
			slotlineslist[0].lineitems[4].PlayeAnima();
			slotlineslist[2].lineitems[4].PlayeAnima();
			RotateSp[0].enabled = true;
			RotateSp[2].enabled = true;
			RotateSp[1].enabled = false;
			break;
		case SLOTTYPE.XXB:
			slotlineslist[0].lineitems[4].PlayeAnima();
			slotlineslist[1].lineitems[4].PlayeAnima();
			RotateSp[0].enabled = true;
			RotateSp[1].enabled = true;
			RotateSp[2].enabled = false;
			break;
		}
	}

	public void stopBigWinAnima()
	{
		UnityVersionUtil.SetActiveRecursive(BigWinObj.gameObject, state: false);
		for (int i = 0; i < slotlineslist.Count; i++)
		{
			slotlineslist[i].lineitems[4].StopAnima();
		}
	}

	private List<int> ListRandom(List<int> myList)
	{
		List<int> list = new List<int>();
		int num = 0;
		for (int i = 0; i < myList.Count; i++)
		{
			num = Random.Range(0, myList.Count - 1);
			if (num != i)
			{
				int value = myList[i];
				myList[i] = myList[num];
				myList[num] = value;
			}
		}
		return myList;
	}

	public void SaveGetitems(slot_item getslotitem)
	{
		List<item> list = new List<item>();
		if (getslotitem.HasItems)
		{
			for (int i = 0; i < getslotitem.items.Count; i++)
			{
				item item = new item();
				item.itemId = getslotitem.items[i].itemId;
				item.quality = getslotitem.items[i].quality;
				item.itemCount = getslotitem.items[i].itemCount;
				list.Add(item);
			}
		}
		else
		{
			list.Clear();
		}
		int num = -1;
		for (int j = 0; j < list.Count; j++)
		{
			num = -1;
			for (int k = 0; k < AllGetItemList.Count; k++)
			{
				if (AllGetItemList[k].itemId.Equals(list[j].itemId) && AllGetItemList[k].quality == list[j].quality)
				{
					num = k;
					break;
				}
			}
			item item2 = new item();
			if (num != -1)
			{
				AllGetItemList[num].itemCount += list[j].itemCount;
			}
			else
			{
				AllGetItemList.Add(list[j]);
			}
		}
		if (SingletonUnity<SlotLogRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SlotLogRootLogic>.Instance.gameObject))
		{
			SingletonUnity<SlotLogRootLogic>.Instance.ShowRewards(AllGetItemList);
		}
	}

	public void SaveGetitems(List<item> newitemsold)
	{
		List<item> list = new List<item>();
		if (newitemsold.Count > 0)
		{
			for (int i = 0; i < newitemsold.Count; i++)
			{
				item item = new item();
				item.itemId = newitemsold[i].itemId;
				item.quality = newitemsold[i].quality;
				item.itemCount = newitemsold[i].itemCount;
				list.Add(item);
			}
		}
		else
		{
			list.Clear();
		}
		int num = -1;
		for (int j = 0; j < list.Count; j++)
		{
			num = -1;
			for (int k = 0; k < AllGetItemList.Count; k++)
			{
				if (AllGetItemList[k].itemId.Equals(list[j].itemId))
				{
					num = k;
					break;
				}
			}
			if (num != -1)
			{
				AllGetItemList[num].itemCount += list[j].itemCount;
			}
			else
			{
				AllGetItemList.Add(list[j]);
			}
		}
		if (SingletonUnity<SlotLogRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SlotLogRootLogic>.Instance.gameObject))
		{
			SingletonUnity<SlotLogRootLogic>.Instance.ShowRewards(AllGetItemList);
		}
	}

	public void ResetAutoInfo()
	{
		AutoDataList = DataManager.GetSlotAutoDataList();
		AutoDataList.Sort((SlotAutoData x, SlotAutoData y) => x.ID.CompareTo(y.ID));
		for (int i = 0; i < rollTimesLabel.Length; i++)
		{
			rollTimesLabel[i].text = $"{AutoDataList[i + 1].RollNum}";
			PriceLabels[i].text = GameMoneyHelper.GetMoneyValStr(AutoDataList[i + 1].PriceCost, AutoDataList[i + 1].PriceType);
		}
	}

	public void setNotwinResult()
	{
		curSlotType = SLOTTYPE.AXC;
		List<int> list = new List<int>();
		for (int i = 0; i < 10; i++)
		{
			list.Add(i);
		}
		int item = list[Random.Range(0, list.Count)];
		list.Remove(item);
		int item2 = list[Random.Range(0, list.Count)];
		list.Remove(item2);
		int num = list[Random.Range(0, list.Count)];
		resultStr[0] = item.ToString();
		resultStr[1] = item2.ToString();
		resultStr[2] = num.ToString();
	}

	public void SetWinResult(string resultid)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < 10; i++)
		{
			list.Add(i);
		}
		curSlotType = SLOTTYPE.XXX;
		resultStr[0] = resultid.Substring(0, 1);
		resultStr[1] = resultid.Substring(1, 1);
		resultStr[2] = resultid.Substring(2, 1);
		if (resultStr[2].Equals("b"))
		{
			int item = int.Parse(resultStr[1]);
			list.Remove(item);
			switch (Random.Range(0, 3))
			{
			case 0:
			{
				curSlotType = SLOTTYPE.BXX;
				resultStr[2] = resultStr[0];
				int num2 = list[Random.Range(0, list.Count)];
				resultStr[0] = num2.ToString();
				break;
			}
			case 1:
				curSlotType = SLOTTYPE.XBX;
				resultStr[2] = resultStr[0];
				item = list[Random.Range(0, list.Count)];
				resultStr[1] = item.ToString();
				break;
			case 2:
			{
				curSlotType = SLOTTYPE.XXB;
				int num = list[Random.Range(0, list.Count)];
				resultStr[2] = num.ToString();
				break;
			}
			}
		}
		else if (resultStr[2].Equals("c"))
		{
			curSlotType = SLOTTYPE.AXC;
			int item2 = int.Parse(resultStr[1]);
			list.Remove(item2);
			int item3 = list[Random.Range(0, list.Count)];
			list.Remove(item3);
			int num3 = list[Random.Range(0, list.Count)];
			resultStr[0] = item3.ToString();
			resultStr[2] = num3.ToString();
		}
	}
}
