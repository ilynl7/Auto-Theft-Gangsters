using System.Collections.Generic;
using System.Text;
using SprotoType;
using UnityEngine;

public class FunctionBtnRootLogic : SingletonUnity<FunctionBtnRootLogic>
{
	private const float MAX_MOUNT_CD = 3f;

	public TweenRotation LeftBtnRotationTween;

	public UISprite LeftBtnSp;

	public UIGridNew LeftBtnGride1;

	public UISprite ActivityBtnIcon;

	public UISprite StrongerIcon;

	public UIGridNew LeftBtnGride2;

	public UIGrid bottomGrid;

	public UIGrid ChatGrid;

	public UISprite CharacterBtnIcon;

	public UISprite ActionBtnIcon;

	public UISprite AutoBtnSprite;

	public UITweener AutoBtnScale;

	public UISprite FollowEscortBtnSprite;

	public UITweener FollowEscortBtnScale;

	public UIGrid DynamicBtnGride;

	public UISprite FollowEffect;

	public UISprite DanceEffect;

	public UISprite AutoEffect;

	private bool IsOpenChatPage;

	public Transform ChatOffsetTra;

	public TweenPosition ChatOffsetAnima;

	public UIWidget AutoComboWi;

	public MainFuncBtnLogic CarFuncBtn;

	public MainFuncBtnLogic ShopFuncBtn;

	public MainFuncBtnLogic SkillFuncBtn;

	public MainFuncBtnLogic EnhanceFuncBtn;

	public MainFuncBtnLogic BagFuncBtn;

	public MainFuncBtnLogic GuildFuncBtn;

	public MainFuncBtnLogic TitleFuncBtn;

	public MainFuncBtnLogic SocialFuncBtn;

	public MainFuncBtnLogic ChatFuncBtn;

	public MainFuncBtnLogic RankFuncBtn;

	public MainFuncBtnLogic SlotFuncBtn;

	public MainFuncBtnLogic GiftFuncBtn;

	public MainFuncBtnLogic BigSaleFuncBtn;

	public MainFuncBtnLogic FirstSaleFuncBtn;

	public MainFuncBtnLogic MysteryFuncBtn;

	public UISprite MountCarBtn;

	public UISprite MountCDSprite;

	public UISprite activityIconTips;

	public GameObject MessageObj;

	public UISprite CityDamageFlag;

	private GameDefine.ACTIVITY_TYPE TipsActivityType = GameDefine.ACTIVITY_TYPE.INVALID;

	private bool isOpenRightBtn;

	private bool isOpenLeftBtn = true;

	private float mEnterDanceAreaTime;

	private int DANCE_WAIT_TIME = 20;

	private SceneManager mCurSceneManager;

	private ObjMainPlayer mMainPlayer;

	private PlayerData mPlayerData;

	private MissionManager missionManager;

	private bool isInsafeArea;

	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	private float mMountTime = -1f;

	public int MaxLineCount = 2;

	private bool[] AcceptChannelType = new bool[8] { true, true, true, true, true, true, true, true };

	public UILabel textLabel;

	private List<PlayerChatHistoryInfo> mHistoryListCache;

	private StringBuilder outPutStr = new StringBuilder();

	private StringBuilder tempStr = new StringBuilder();

	private bool isShowAutoCombo = true;

	private float mLastSendServerTimeCheck;

	private float mSendSerTimeInterval = 5f;

	private float nearDistance = 4f;

	private float farDistance = 10f;

	public List<UIButtonColor> RightBtnColorList;

	public List<UIButtonColor> TopBtnColorList;

	public bool IsOpenRightBtn => isOpenRightBtn;

	public bool IsOpenLeftBtn => isOpenLeftBtn;

	private List<PlayerChatHistoryInfo> mHistoryList
	{
		get
		{
			if (mHistoryListCache == null)
			{
				mHistoryListCache = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ChatHistory.ChatHistoryList;
			}
			return mHistoryListCache;
		}
	}

	public float LastSendServerTimeCheck
	{
		set
		{
			mLastSendServerTimeCheck = value;
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
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	private void Start()
	{
		UpdateAutoBtn();
		mCurSceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ChatHistory.InitOfflineChat();
		missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
	}

	public void Reset()
	{
		refershBtn();
		ResetRightBtn();
		isOpenRightBtn = true;
		MountCDSprite.fillAmount = 0f;
		UpdateMessage();
		ResetChatState();
		ShowAutoCombo(mMainPlayer.GetAutoCombatState());
	}

	public void OnReceiveMessage()
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			UpdateMessage();
		}
	}

	public void UpdateMessage()
	{
		if (mHistoryList.Count == 0)
		{
			return;
		}
		textLabel.UpdateNGUIText();
		outPutStr.Length = 0;
		int num = 0;
		string finalText = null;
		for (int num2 = mHistoryList.Count - 1; num2 >= 0; num2--)
		{
			if (IsAcceptChannel(mHistoryList[num2].ChannelType))
			{
				NGUIText.WrapText(FormatStr(mHistoryList[num2]), out finalText);
				if (finalText[finalText.Length - 1] != '\n')
				{
					finalText = $"{finalText}\n";
				}
				num += GetLineCount(finalText);
				if (num > MaxLineCount)
				{
					int num3 = num - MaxLineCount;
					if (num3 > 0)
					{
						finalText = finalText.Substring(GetIndexOfCount(finalText, '\n', num3) + 1);
						outPutStr.Insert(0, finalText);
					}
					break;
				}
				outPutStr.Insert(0, finalText);
				if (num == MaxLineCount)
				{
					break;
				}
			}
		}
		if (outPutStr.Length > 0)
		{
			outPutStr.Length -= 1;
		}
		textLabel.text = outPutStr.ToString();
	}

	private int GetIndexOfCount(string str, char val, int count)
	{
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == val)
			{
				count--;
				if (count <= 0)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private int GetLineCount(string str)
	{
		int num = 0;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == '\n')
			{
				num++;
			}
		}
		return num;
	}

	public string FormatStr(PlayerChatHistoryInfo info)
	{
		tempStr.Length = 0;
		if (mPlayerData.IsNeedTranslation)
		{
			tempStr.AppendFormat("{0} [8FF4F3]{1}[-]: {2}\n", StrDictionary.GetDictionaryString(GameDefine.CHANNEL_PRE_WORD[(int)info.ChannelType]), info.SenderName, info.ChatInfo2);
		}
		else
		{
			tempStr.AppendFormat("{0} [8FF4F3]{1}[-]: {2}\n", StrDictionary.GetDictionaryString(GameDefine.CHANNEL_PRE_WORD[(int)info.ChannelType]), info.SenderName, info.ChatInfo);
		}
		return tempStr.ToString();
	}

	private bool IsAcceptChannel(GameDefine.CHAT_CHANNEL_TYPE type)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData?.IsAcceptChannel(type) ?? true;
	}

	public void SetChannelAccept(GameDefine.CHAT_CHANNEL_TYPE type, bool IsAccept)
	{
		AcceptChannelType[(int)type] = IsAccept;
	}

	public void OnClickOpenChatBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ChatRoot, OnChatRootShow);
	}

	public void HideShowCarUI()
	{
		NGUITools.SetActive(AutoBtnSprite.gameObject, state: false);
		NGUITools.SetActive(ActionBtnIcon.gameObject, state: false);
	}

	private void OnChatRootShow(bool isSuccess, object param)
	{
		if (isSuccess)
		{
			SingletonUnity<ChatUIRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ChoosedChannelType);
		}
	}

	private void CheckFunctionBtn(UISprite btnIcon, FUNCTION_TYPE funcType)
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(funcType))
		{
			NGUITools.SetActive(btnIcon.gameObject, state: true);
			btnIcon.alpha = 1f;
		}
		else
		{
			NGUITools.SetActive(btnIcon.gameObject, state: false);
		}
	}

	public void OnClickOpenActivity()
	{
		switch (TipsActivityType)
		{
		case GameDefine.ACTIVITY_TYPE.WILD_BOSS:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.WILD_BOSS);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.CITY_DANCE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.CITY_DANCE);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.BAR_FIGHT:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.BAR_FIGHT);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.GUILD_BOSS:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_BOSS);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.GUILD_BATTLE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_BATTLE);
			});
			break;
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("MainUI", "BtnClick", "PopActivityBtn");
	}

	public void refershBtn()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene())
		{
			NGUITools.SetActive(MountCarBtn.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(MountCarBtn.gameObject, playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.CAR));
		}
		ResetLeftBtn();
		isOpenLeftBtn = true;
		LeftBtnRotationTween.ResetToBeginning();
		ShowAutoBtn();
		NGUITools.SetActive(StrongerIcon.gameObject, state: false);
		UpdateAutoBtn();
		DisableEscortBtn();
		UpdateTips();
		UpdateUnlockTips();
		EnableBtnColor(TopBtnColorList);
		EnableBtnColor(RightBtnColorList);
	}

	public void UpdateUnlockTips()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData.MenuTabBtnTipIdList.Contains(4012.ToString()) || playerCommonData.MenuTabBtnTipIdList.Contains(4013.ToString()) || playerCommonData.MenuTabBtnTipIdList.Contains(3022.ToString()))
		{
			FunctionTipsRootLogic.AddFunctionTips(GiftFuncBtn.gameObject, Vector3.zero, -1f);
		}
		else
		{
			FunctionTipsRootLogic.RemoveFunctionTips(GiftFuncBtn.gameObject);
		}
		if (playerCommonData.MenuTabBtnTipIdList.Contains(4043.ToString()))
		{
			FunctionTipsRootLogic.AddFunctionTips(EnhanceFuncBtn.gameObject, Vector3.zero, -1f);
		}
		else
		{
			FunctionTipsRootLogic.RemoveFunctionTips(EnhanceFuncBtn.gameObject);
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
		{
			if (playerCommonData.MenuTabBtnTipIdList.Contains(4061.ToString()) || playerCommonData.MenuTabBtnTipIdList.Contains(4062.ToString()))
			{
				FunctionTipsRootLogic.AddFunctionTips(SocialFuncBtn.gameObject, Vector3.zero, -1f);
			}
			else
			{
				FunctionTipsRootLogic.RemoveFunctionTips(SocialFuncBtn.gameObject);
			}
		}
		if (playerCommonData.MenuTabBtnTipIdList.Contains(3021.ToString()))
		{
			FunctionTipsRootLogic.AddFunctionTips(AutoBtnSprite.gameObject, Vector3.zero, -1f);
		}
		else
		{
			FunctionTipsRootLogic.RemoveFunctionTips(AutoBtnSprite.gameObject);
		}
	}

	public void HideBigSaleBtn()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		playerCommonData.Big_PackFlag = false;
		NGUITools.SetActive(BigSaleFuncBtn.gameObject, state: false);
		LeftBtnGride2.Reposition();
	}

	public void HideMysterySaleBtn()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData.Push != -1 && (playerCommonData.Push & 0x10) != 0L)
		{
			playerCommonData.Push -= 16L;
		}
		NGUITools.SetActive(MysteryFuncBtn.gameObject, state: false);
		LeftBtnGride2.Reposition();
	}

	public void HideFirstSaleBtn()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		playerCommonData.First_PackFlag = false;
		NGUITools.SetActive(FirstSaleFuncBtn.gameObject, state: false);
		LeftBtnGride2.Reposition();
	}

	public void UpdateMenu(bool resetNow = false)
	{
		ResetRightBtn();
		EnableBtnColor(RightBtnColorList);
	}

	public void ResetRightBtn()
	{
		CarFuncBtn.SetState(FUNCTION_TYPE.CAR, Islockshow: true);
		ShopFuncBtn.SetState(FUNCTION_TYPE.SHOP);
		EnhanceFuncBtn.SetState(FUNCTION_TYPE.ENHANCE, Islockshow: true);
		BagFuncBtn.SetState(FUNCTION_TYPE.BAG, Islockshow: true);
		GuildFuncBtn.SetState(FUNCTION_TYPE.GUILD, Islockshow: true);
		ShowDownLoadLaterBtn();
		bottomGrid.Reposition();
		ChatGrid.Reposition();
	}

	public void ResetLeftBtn()
	{
		RankFuncBtn.SetState(FUNCTION_TYPE.RANK, Islockshow: true);
		SlotFuncBtn.SetState(FUNCTION_TYPE.LOTTO);
		GiftFuncBtn.SetState(FUNCTION_TYPE.GIFT);
		BigSaleFuncBtn.SetState(FUNCTION_TYPE.BIGSALES);
		FirstSaleFuncBtn.SetState(FUNCTION_TYPE.FIRST_PAY);
		MysteryFuncBtn.SetState(FUNCTION_TYPE.MYSTERYSHOP);
		CheckLeftBtn();
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (!playerCommonData.Big_PackFlag)
		{
			NGUITools.SetActive(BigSaleFuncBtn.gameObject, state: false);
		}
		if (!playerCommonData.First_PackFlag)
		{
			NGUITools.SetActive(FirstSaleFuncBtn.gameObject, state: false);
		}
		if (playerCommonData.Push != -1 && (playerCommonData.Push & 0x10) == 0L)
		{
			NGUITools.SetActive(MysteryFuncBtn.gameObject, state: false);
		}
		LeftBtnGride1.Reposition();
		LeftBtnGride2.Reposition();
	}

	public void ShowDownLoadLaterBtn()
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			SocialFuncBtn.SetState(FUNCTION_TYPE.SOCIAL);
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
			{
				NGUITools.SetActive(ActionBtnIcon.gameObject, state: true);
			}
			else
			{
				NGUITools.SetActive(ActionBtnIcon.gameObject, state: false);
			}
			bottomGrid.Reposition();
			ChatGrid.Reposition();
		}
	}

	public void CheckLeftBtn()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_ACTIVITY) || playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT) || playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.FIRST_PAY) || playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.BIGSALES) || playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.MYSTERYSHOP) || playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.LOTTO) || playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP))
		{
			LeftBtnSp.alpha = 1f;
		}
		else
		{
			LeftBtnSp.alpha = 0f;
		}
	}

	public void ShowAutoCombo(bool isshow)
	{
		if (isShowAutoCombo != isshow)
		{
			isShowAutoCombo = isshow;
			if (isShowAutoCombo)
			{
				AutoComboWi.alpha = 1f;
			}
			else
			{
				AutoComboWi.alpha = 0f;
			}
		}
	}

	public void ResetChatState()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetChatShow == 1)
		{
			ChatOffsetAnima.ResetToBeginning();
			ChatOffsetAnima.enabled = false;
			IsOpenChatPage = true;
		}
		else
		{
			ChatOffsetAnima.PlayForward();
			ChatOffsetAnima.transform.localPosition = ChatOffsetAnima.to;
			ChatOffsetAnima.enabled = false;
			IsOpenChatPage = false;
		}
		ChatFuncBtn.SetDirState(IsOpenChatPage);
	}

	public void OnClickChatBtn()
	{
		if (IsOpenChatPage)
		{
			ChatOffsetAnima.PlayForward();
			ChatFuncBtn.ShowDirAnima(isopen: true);
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SetChatShow(0);
		}
		else
		{
			ChatOffsetAnima.PlayReverse();
			ChatFuncBtn.ShowDirAnima(isopen: false);
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SetChatShow(1);
		}
		IsOpenChatPage = !IsOpenChatPage;
	}

	public void OnClickSocialDance()
	{
		if (SingletonUnity<SocialDanceUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SocialDanceUIRoot>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SocialDanceRoot);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SocialDanceRoot, delegate(bool bSuccess, object param)
		{
			if (bSuccess)
			{
				SingletonUnity<SocialDanceUIRoot>.Instance.Reset();
			}
		});
	}

	public void EnableDanceBtn()
	{
		if (!SingletonUnity<DanceBtnRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<DanceBtnRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DanceBtnRoot, delegate
			{
				SingletonUnity<DanceBtnRootLogic>.Instance.Reset();
				mEnterDanceAreaTime = Time.time;
				ShowAutoBtn();
				if (SingletonUnity<MissionTeamTipLogic>.Exists)
				{
					SingletonUnity<MissionTeamTipLogic>.Instance.ChangeToDanceMission(IsEnterDance: true);
				}
			});
		}
		if (SingletonUnity<PotionLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PotionLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PotionObjRoot);
		}
	}

	public void DisableDanceBtn()
	{
		if (SingletonUnity<DanceBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DanceBtnRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DanceBtnRoot);
			ShowAutoBtn();
			if (SingletonUnity<MissionTeamTipLogic>.Exists)
			{
				SingletonUnity<MissionTeamTipLogic>.Instance.ChangeToDanceMission(IsEnterDance: false);
			}
		}
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.IsCanUsePotion() && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PotionObjRoot, delegate
			{
				SingletonUnity<PotionLogic>.Instance.Reset();
			});
		}
	}

	public void EnableEscortBtn()
	{
		if (!UnityVersionUtil.IsActive(FollowEscortBtnScale.gameObject))
		{
			NGUITools.SetActive(FollowEscortBtnScale.gameObject, state: true);
			DynamicBtnGride.Reposition();
			ShowAutoBtn();
			UpdateFollowEscortBtn();
			if (mMainPlayer.IsOpenAutoCombat)
			{
				mMainPlayer.LeveAutoCombat();
			}
		}
	}

	public void DisableEscortBtn()
	{
		if (UnityVersionUtil.IsActive(FollowEscortBtnScale.gameObject))
		{
			NGUITools.SetActive(FollowEscortBtnScale.gameObject, state: false);
			DynamicBtnGride.Reposition();
			ShowAutoBtn();
		}
	}

	public void OnClickMenuBtn()
	{
		UpdateMenu();
	}

	public void OnClickActivityBtn()
	{
		if (SingletonUnity<TutorialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.CloseCheck();
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ActivityUIRootLogic, delegate
		{
			SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
			if (TutorialManager.CurStep == TUTORIAL_STEP.EXP_COPY_START || TutorialManager.CurStep == TUTORIAL_STEP.CAR_COPY_START || TutorialManager.CurStep == TUTORIAL_STEP.GOLD_COPY_START || TutorialManager.CurStep == TUTORIAL_STEP.TOWER_START || TutorialManager.CurStep == TUTORIAL_STEP.SCUFFLE_COPY_START || TutorialManager.CurStep == TUTORIAL_STEP.WORLD_BOSS_START || TutorialManager.CurStep == TUTORIAL_STEP.EQUIP_COPY_START || TutorialManager.CurStep == TUTORIAL_STEP.BAR_FIGHT_START || TutorialManager.CurStep == TUTORIAL_STEP.ESCORT_START || TutorialManager.CurStep == TUTORIAL_STEP.ROBBORY_START || TutorialManager.CurStep == TUTORIAL_STEP.SURVIVAL_BATTLE_START || TutorialManager.CurStep == TUTORIAL_STEP.GUILD_BOSS_START || TutorialManager.CurStep == TUTORIAL_STEP.RANK_PVP_START)
			{
				CheckTutorialEvent();
			}
		});
	}

	public void OnClickAuctionBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ConsignUIRoot, OnShowConsignRoot);
	}

	private void OnShowConsignRoot(bool isSuccess, object param)
	{
		if (isSuccess)
		{
			SingletonUnity<ConsignRootLogic>.Instance.Reset();
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.SELL_ITEM_START)
		{
			CheckTutorialEvent();
		}
	}

	public void OnClickGiftBtn()
	{
		if (isUnlockFun(FUNCTION_TYPE.GIFT))
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.MenuTabBtnTipIdList.Contains(3022.ToString()))
			{
				FunctionTipsRootLogic.RemoveFunctionTips(AutoBtnSprite.gameObject);
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.MenuTabBtnTipIdList.Remove(3022.ToString());
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.SetTutorialShowFinish(FUNCTION_TYPE.GIFT_TIP);
				UpdateUnlockTips();
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CommercialUIRoot, delegate
			{
				SingletonUnity<CommercialUIRootLogic>.Instance.Reset();
			});
		}
	}

	public void OnClickDailyActivity()
	{
	}

	public void OnClickStrongerBtn()
	{
	}

	public void OnClickLottoBtn()
	{
		if (!isUnlockFun(FUNCTION_TYPE.LOTTO))
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SlotUIRoot, delegate
		{
			SingletonUnity<SlotUIRootLogic>.Instance.EnableReset();
			WaitResponseUIRootLogic.OpenWaitBox(242, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.request_slot_info>();
			if (TutorialManager.CurStep == TUTORIAL_STEP.SLOT_START)
			{
				CheckTutorialEvent();
			}
		});
	}

	public void OnClickRankBnt()
	{
		if (isUnlockFun(FUNCTION_TYPE.RANK))
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerRankInfoRoot, delegate
			{
				SingletonUnity<PlayerRankInfoRootLogic>.Instance.Reset();
			});
		}
	}

	public void OnClickSalesBtn()
	{
		if (isUnlockFun(FUNCTION_TYPE.FIRST_PAY))
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FirstBuyRoot, delegate
			{
				SingletonUnity<FirstBuyRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(259, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_first_buy>();
			});
		}
	}

	public void OnClickBigSalesBtn()
	{
		if (isUnlockFun(FUNCTION_TYPE.BIGSALES))
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BigPackRoot, delegate
			{
				SingletonUnity<BigPackRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(260, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_big_pack>();
			});
		}
	}

	public void OnClickMysteryShopBtn()
	{
		if (isUnlockFun(FUNCTION_TYPE.MYSTERYSHOP))
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MysteryShopRoot, delegate
			{
				SingletonUnity<MysteryShopRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(274, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_special_big_pack>();
			});
		}
	}

	public void OnClickSevenDayBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SignWeekRoot, delegate
		{
			SingletonUnity<SignWeekRootLogic>.Instance.EnableReset();
			WaitResponseUIRootLogic.OpenWaitBox(253, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.request_sign_week_info>();
		});
	}

	public void OnClickShopBtn()
	{
		if (!isUnlockFun(FUNCTION_TYPE.SHOP))
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_START)
			{
				if (GameManager.IsSupportCurDataVersion145())
				{
					SingletonUnity<ShopUIRootLogic>.Instance.OnClickToolsBtn(GameDefine.SHOP_TAB_TYPE.ITEM, GameDefine.UIBACKTYPE.NOTHINTG, "9999");
					CheckTutorialEvent();
				}
				else
				{
					SingletonUnity<ShopUIRootLogic>.Instance.Reset();
				}
			}
			else
			{
				SingletonUnity<ShopUIRootLogic>.Instance.Reset();
			}
		});
	}

	public void OnClickCoinShopBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExchangeShopRoot, delegate
		{
			SingletonUnity<ExchangeShopRootLogic>.Instance.Reset();
		});
	}

	public void OnClickMailBtn()
	{
		SingletonUnity<SocialUIRootLogic>.Instance.ShowSocialUI();
		SingletonUnity<SocialUIRootLogic>.Instance.OnClickMailBtn();
	}

	public void UpdateFollowEscortBtn()
	{
		if (UnityVersionUtil.IsActive(FollowEscortBtnScale.gameObject) && !(mMainPlayer == null))
		{
			if (mMainPlayer.IsTeamFollowState())
			{
				FollowEscortBtnSprite.spriteName = "CZ_zhuJieMianAnNiu_ZiDongGenSui_2";
				FollowEffect.alpha = 1f;
			}
			else
			{
				FollowEscortBtnSprite.spriteName = "CZ_zhuJieMianAnNiu_ZiDongGenSui_1";
				FollowEffect.alpha = 0f;
			}
			FollowEscortBtnScale.ResetToBeginning();
			FollowEscortBtnScale.enabled = mMainPlayer.IsTeamFollowState();
		}
	}

	public void UpdateAutoBtn()
	{
		if (mMainPlayer == null)
		{
			mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (!(mMainPlayer == null))
		{
			if (mMainPlayer.IsOpenAutoCombat)
			{
				AutoBtnSprite.spriteName = "CZ_zhuJieMianAnNiu_ZiDong_2";
				AutoEffect.alpha = 1f;
			}
			else
			{
				AutoBtnSprite.spriteName = "CZ_zhuJieMianAnNiu_ZiDong";
				AutoEffect.alpha = 0f;
			}
			AutoBtnScale.ResetToBeginning();
			AutoBtnScale.enabled = mMainPlayer.IsOpenAutoCombat;
		}
	}

	public void OnClickAutoCombo()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.MenuTabBtnTipIdList.Contains(3021.ToString()))
		{
			FunctionTipsRootLogic.RemoveFunctionTips(AutoBtnSprite.gameObject);
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.MenuTabBtnTipIdList.Remove(3021.ToString());
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.SetTutorialShowFinish(FUNCTION_TYPE.AUTO_FIGHT_TIP);
			UpdateUnlockTips();
		}
		if (!(mMainPlayer == null))
		{
			if (!mMainPlayer.IsOpenAutoCombat)
			{
				mMainPlayer.EnterAutoCombat();
			}
			else
			{
				mMainPlayer.LeveAutoCombat();
			}
			UpdateAutoBtn();
		}
	}

	public void OnClickLeftArrow()
	{
		OnClickLeftArrowBtn();
	}

	public void OnClickLeftArrowBtn(bool resetNow = false)
	{
		if (isOpenLeftBtn)
		{
			LeftBtnRotationTween.PlayForward();
			LeftBtnGride1.CloseGrid();
			LeftBtnGride2.CloseGrid();
			DisableBtnColor(TopBtnColorList);
		}
		else
		{
			LeftBtnRotationTween.PlayReverse();
			ResetLeftBtn();
			if (!resetNow)
			{
				LeftBtnGride1.OpenGrid();
				LeftBtnGride2.OpenGrid();
			}
			EnableBtnColor(TopBtnColorList);
		}
		isOpenLeftBtn = !isOpenLeftBtn;
	}

	public void OnClickGangBtn()
	{
		if (!isUnlockFun(FUNCTION_TYPE.GUILD))
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewGuildUIRootLogic, delegate
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.GUILD_START)
			{
				CheckTutorialEvent();
			}
		});
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("MainUI", "BtnClick", "GuildBtn");
	}

	public void OnClickSkillBtn()
	{
		if (!isUnlockFun(FUNCTION_TYPE.SKILL))
		{
			return;
		}
		if (SingletonUnity<TutorialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.CloseCheck();
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GameMenuSkillInfoRootUI, delegate
		{
			SingletonUnity<SkillInfoRootLogic>.Instance.Reset();
			if (TutorialManager.CurStep == TUTORIAL_STEP.SKILL_UPGRADE_START || TutorialManager.CurStep == TUTORIAL_STEP.SKILL_DRAG_START)
			{
				CheckTutorialEvent();
			}
		});
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("MainUI", "BtnClick", "SkillBtn");
	}

	public void OnClickCharacterBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate(bool isSuccess, object param)
		{
			if (isSuccess)
			{
			}
			SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickEquipBackPackBtn();
		});
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("MainUI", "BtnClick", "CharacterBtn");
	}

	public void OnClickBagBtn()
	{
		if (!isUnlockFun(FUNCTION_TYPE.BAG))
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.BADGE_START)
			{
				CheckTutorialEvent();
			}
			SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickEquipBackPackBtn();
		});
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("MainUI", "BtnClick", "ItemBtn");
	}

	public void OnClickSocialBtn()
	{
		SingletonUnity<SocialUIRootLogic>.Instance.ShowSocialUI();
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SOCIAL_FRIEND))
		{
			if (!SingletonUnity<MenuBaseRootLogic>.Instance.AutoClickTipsTap())
			{
				SingletonUnity<SocialUIRootLogic>.Instance.SelectFriendInfobtn();
			}
		}
		else
		{
			SingletonUnity<SocialUIRootLogic>.Instance.OnClickMailBtn();
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("MainUI", "BtnClick", "SocialBtn");
	}

	public void OnClickTitleBtn()
	{
		if (!isUnlockFun(FUNCTION_TYPE.TITLE))
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TitleUIRootLogic, delegate
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.TITLE_START)
			{
				CheckTutorialEvent();
			}
		});
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("MainUI", "BtnClick", "TitleBtn");
	}

	public void OnClickEnhaceBtn()
	{
		if (!isUnlockFun(FUNCTION_TYPE.ENHANCE))
		{
			return;
		}
		if (SingletonUnity<TutorialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.CloseCheck();
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
		{
			SingletonUnity<EquipStrengthenUIRootLogic>.Instance.Reset();
			if (TutorialManager.CurStep == TUTORIAL_STEP.ENHANCE_START || TutorialManager.CurStep == TUTORIAL_STEP.STRENGTH_STAR_START || TutorialManager.CurStep == TUTORIAL_STEP.ENHANCE_ALL_START || TutorialManager.CurStep == TUTORIAL_STEP.SKILL_UPGRADE_START || TutorialManager.CurStep == TUTORIAL_STEP.TITLE_START)
			{
				CheckTutorialEvent();
			}
		});
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("MainUI", "BtnClick", "EnhanceBtn");
	}

	public void OnClickEscortFollowBtn()
	{
		ObjManager instance = Singleton<ObjManager>.Instance;
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		AutoSearchPathManager autoSearchPath = SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath;
		CurMission escortMission = missionManager.GetEscortMission();
		string mapId = string.Empty;
		Vector3 escortNpcPos = missionManager.GetEscortNpcPos(out mapId);
		if (!mCurSceneManager.CurrentMapInofData.ID.Equals(mapId))
		{
			autoSearchPath.FindPath(new AutoSearchPathPoint(mapId, escortNpcPos), AUTO_SEARCH_PARTH_FINISHEVENT.MISSION, escortMission.MissionId);
			if (autoSearchPath.CurPath != null && autoSearchPath.CurPath.PathPointList.Count > 0)
			{
				mMainPlayer.MoveTo(autoSearchPath.CurPath.PathPointList[0].PosX, autoSearchPath.CurPath.PathPointList[0].PosZ);
			}
			return;
		}
		if (!mMainPlayer.IsTeamFollowState())
		{
			long npcId = escortMission.GetParam(1);
			if (instance.ObjDict.ContainsKey(npcId))
			{
				mMainPlayer.EnterFollowTarget(npcId);
			}
			else
			{
				mMainPlayer.MoveTo(escortNpcPos, 1f, delegate
				{
					mMainPlayer.EnterFollowTarget(npcId);
					UpdateFollowEscortBtn();
				});
			}
		}
		else
		{
			mMainPlayer.LeaveTeamFollow();
		}
		UpdateFollowEscortBtn();
		if (!mMainPlayer.IsOpenAutoCombat)
		{
			OnClickAutoCombo();
		}
	}

	public bool CheckDanceLevel()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		copyscene_info copyinfoByType = playerData.CopyInfoData.GetCopyinfoByType(26);
		string text = null;
		if (copyinfoByType != null)
		{
			text = copyinfoByType.ID;
		}
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(text);
		if (!playerData.CheckLevel(cityDanceDataById.UnlockLevel))
		{
			return false;
		}
		return true;
	}

	public void OnClickCarBtn()
	{
		if (isUnlockFun(FUNCTION_TYPE.CAR))
		{
			if (SingletonUnity<TutorialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<TutorialUIRootLogic>.Instance.CloseCheck();
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerCarRoot, delegate
			{
				SingletonUnity<PlayerCarRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(235, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_mount_info>();
			});
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("MainUI", "BtnClick", "CarBtn");
		}
	}

	public bool isUnlockFun(FUNCTION_TYPE functype)
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(functype))
		{
			return true;
		}
		int num = (int)functype;
		int condition = DataManager.GetFunctionDataById(num.ToString()).Condition;
		NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", condition));
		return false;
	}

	private void UpdateMountTime()
	{
		if (mMountTime > 0f)
		{
			mMountTime -= Time.deltaTime;
			if (mMountTime <= 0f)
			{
				mMountTime = -1f;
			}
			MountCDSprite.fillAmount = Mathf.Clamp01(mMountTime / 3f);
		}
	}

	public void OnClickMountBtn()
	{
		if (mMainPlayer == null || mMainPlayer.SkillLogic.IsUsingSkill || mMainPlayer.IsDie || mMainPlayer.IsStun())
		{
			return;
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_CLICK_MOUNT)
		{
			CheckTutorialEvent();
		}
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.CAR))
		{
			NoticeLogic.AddNotifyData("#{100642}");
		}
		else if (!(mMountTime > 0f))
		{
			mMountTime = 3f;
			if (mMainPlayer.IsDrivingMount())
			{
				mMainPlayer.SendServerDisMountCar();
			}
			else if (string.IsNullOrEmpty(mMainPlayer.MountId))
			{
				OnClickCarBtn();
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101557}"));
			}
			else
			{
				mMainPlayer.SendServerMountCar();
			}
		}
	}

	private void Update()
	{
		UpdateMountTime();
		if (mCurSceneManager.IsCiytDanceSceneActive())
		{
			if (mCurSceneManager.IsInGatherArea(mMainPlayer.Position))
			{
				EnableDanceBtn();
				if (Time.time - mEnterDanceAreaTime > (float)DANCE_WAIT_TIME)
				{
					if (mMainPlayer.CurPlayerState != PLAYER_STATE.DANCE)
					{
						if (mMainPlayer.IsMoving || mMainPlayer.SkillLogic.IsUsingSkill)
						{
							mEnterDanceAreaTime = Time.time;
						}
						else
						{
							if (!CheckDanceLevel())
							{
								return;
							}
							if (Time.time - mLastSendServerTimeCheck > mSendSerTimeInterval)
							{
								if (mPlayerData.PlayerDanceData.IsHaveDanceData() && mPlayerData.ActivityData.IsCanDance())
								{
									if (mPlayerData.PlayerDanceData.IsSpecialDanceDataEnable())
									{
										use_dance.request request = new use_dance.request();
										request.id = mPlayerData.PlayerDanceData.CurSpecialDanceData.ID;
										NetLogic.GetInstance().Send<Protocol.use_dance>(request);
										mLastSendServerTimeCheck = Time.time;
									}
									else
									{
										use_dance.request request2 = new use_dance.request();
										request2.id = mPlayerData.PlayerDanceData.CurNormalDanceData.ID;
										NetLogic.GetInstance().Send<Protocol.use_dance>(request2);
										mLastSendServerTimeCheck = Time.time;
									}
								}
								else
								{
									mLastSendServerTimeCheck = Time.time;
								}
							}
						}
					}
					else
					{
						mEnterDanceAreaTime = Time.time;
					}
				}
			}
			else
			{
				DisableDanceBtn();
			}
		}
		if (UnityVersionUtil.IsActive(FollowEscortBtnScale.gameObject) && FollowEscortBtnScale.enabled && !mMainPlayer.IsTeamFollowState())
		{
			UpdateFollowEscortBtn();
		}
		if (mCurSceneManager.isHaveSaftyArea)
		{
			if (mCurSceneManager.IsInSafeArea(mMainPlayer.Position))
			{
				if (!isInsafeArea)
				{
					isInsafeArea = true;
					NoticeLogic.AddNotifyData("#{102028}");
					mMainPlayer.SelectTarget(null);
				}
			}
			else if (isInsafeArea)
			{
				isInsafeArea = false;
				NoticeLogic.AddNotifyData("#{102029}");
			}
		}
		UpdateFollowFlag();
	}

	public void UpdateFollowFlag()
	{
		if (missionManager.IsInEscortMission())
		{
			string mapId = string.Empty;
			Vector3 escortNpcPos = missionManager.GetEscortNpcPos(out mapId);
			if (mCurSceneManager.CurrentMapInofData.ID.Equals(mapId))
			{
				if (!UnityVersionUtil.IsActive(FollowEscortBtnScale.gameObject))
				{
					EnableEscortBtn();
				}
			}
			else
			{
				DisableEscortBtn();
			}
		}
		else
		{
			DisableEscortBtn();
		}
	}

	public void ShowAutoBtn()
	{
		if (UnityVersionUtil.IsActive(FollowEscortBtnScale.gameObject) || (SingletonUnity<DanceBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DanceBtnRootLogic>.Instance.gameObject)))
		{
			if (UnityVersionUtil.IsActive(AutoBtnSprite.gameObject))
			{
				NGUITools.SetActive(AutoBtnSprite.gameObject, state: false);
			}
		}
		else
		{
			CheckFunctionBtn(AutoBtnSprite, FUNCTION_TYPE.AUTO_FIGHT);
			UpdateAutoBtn();
		}
	}

	public UISprite GetBtnIconByFunctionType(FUNCTION_TYPE type)
	{
		return type switch
		{
			FUNCTION_TYPE.RANK => RankFuncBtn.IconSp, 
			FUNCTION_TYPE.GIFT => GiftFuncBtn.IconSp, 
			FUNCTION_TYPE.SHOP => ShopFuncBtn.IconSp, 
			FUNCTION_TYPE.LOTTO => SlotFuncBtn.IconSp, 
			FUNCTION_TYPE.FIRST_PAY => FirstSaleFuncBtn.IconSp, 
			FUNCTION_TYPE.BAG => BagFuncBtn.IconSp, 
			FUNCTION_TYPE.SOCIAL => SocialFuncBtn.IconSp, 
			FUNCTION_TYPE.GUILD => GuildFuncBtn.IconSp, 
			FUNCTION_TYPE.CAR => CarFuncBtn.IconSp, 
			FUNCTION_TYPE.ENHANCE => EnhanceFuncBtn.IconSp, 
			FUNCTION_TYPE.BIGSALES => BigSaleFuncBtn.IconSp, 
			FUNCTION_TYPE.MYSTERYSHOP => MysteryFuncBtn.IconSp, 
			_ => null, 
		};
	}

	public bool CheckTitleTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.TITLE_TITLE))
		{
			return false;
		}
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		int curTitleExp = mPlayerData.MainPlayerAttrData.CurTitleExp;
		int curTitleLevel = mPlayerData.MainPlayerAttrData.CurTitleLevel;
		if (curTitleLevel < 10 && curTitleLevel >= 0)
		{
			TitleData titleDateById = DataManager.GetTitleDateById(curTitleLevel.ToString());
			if (curTitleExp >= titleDateById.EXP)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public void UpdateTitleTips()
	{
		UpdateEnhanceTipsFlag();
	}

	public void OnClickMessageFlag()
	{
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		List<GameDefine.ACTIVITY_TYPE> messagelist = new List<GameDefine.ACTIVITY_TYPE>();
		if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE))
		{
			messagelist.Add(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE);
		}
		if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.BAR_FIGHT))
		{
			messagelist.Add(GameDefine.ACTIVITY_TYPE.BAR_FIGHT);
		}
		if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.WILD_BOSS))
		{
			messagelist.Add(GameDefine.ACTIVITY_TYPE.WILD_BOSS);
		}
		if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.GUILD_BOSS))
		{
			messagelist.Add(GameDefine.ACTIVITY_TYPE.GUILD_BOSS);
		}
		if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.GUILD_BATTLE))
		{
			messagelist.Add(GameDefine.ACTIVITY_TYPE.GUILD_BATTLE);
		}
		if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.GUILD_DANCE))
		{
			messagelist.Add(GameDefine.ACTIVITY_TYPE.GUILD_DANCE);
		}
		if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.GUILD_DONMINE))
		{
			messagelist.Add(GameDefine.ACTIVITY_TYPE.GUILD_DONMINE);
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewMessageUIRoot, delegate
		{
			SingletonUnity<NewMessageUIRootLogic>.Instance.ResetInfo(messagelist);
		});
		if (TutorialManager.CurStep == TUTORIAL_STEP.FUNCTION_TIP_START)
		{
			CheckTutorialEvent();
		}
	}

	public void OnClickCityDamageBtn()
	{
		guild_map_info curinfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.GetGuildMapInfo(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID);
		if (curinfo != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CityDamageRoot, delegate
			{
				WaitResponseUIRootLogic.OpenWaitBox(318, 10f, 0f);
				request_guild_map_domine_top.request rpcReq = new request_guild_map_domine_top.request
				{
					id = curinfo.id
				};
				NetLogic.GetInstance().Send<Protocol.request_guild_map_domine_top>(rpcReq);
				SingletonUnity<CityDamageRootLogic>.Instance.EnableReset();
			});
		}
	}

	public void UpdateMessageTips()
	{
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE))
		{
			ShowMessageTips(istrue: true);
		}
		else if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.BAR_FIGHT))
		{
			ShowMessageTips(istrue: true);
		}
		else if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.WILD_BOSS))
		{
			ShowMessageTips(istrue: true);
		}
		else if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.GUILD_BOSS))
		{
			ShowMessageTips(istrue: true);
		}
		else if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.GUILD_BATTLE))
		{
			ShowMessageTips(istrue: true);
		}
		else if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.GUILD_DANCE))
		{
			ShowMessageTips(istrue: true);
		}
		else if (mPlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.GUILD_DONMINE))
		{
			ShowMessageTips(istrue: true);
		}
		else if (mPlayerData.ActivityData.IsHaveMissionTimeOut())
		{
			ShowMessageTips(istrue: true);
		}
		else if (NewMessageUIRootLogic.IsHaveTeamMessageInfo())
		{
			ShowMessageTips(istrue: true);
		}
		else
		{
			ShowMessageTips(istrue: false);
		}
	}

	private void ShowMessageTips(bool istrue)
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
		{
			istrue = false;
		}
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			NGUITools.SetActive(MessageObj, istrue);
		}
		if (istrue && !SingletonUnity<UIManager>.Instance.IsHideBaseUI)
		{
			PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			if (playerCommonData.IsTutorialCanShow(FUNCTION_TYPE.TIPBTN_TUTORIAL_TIP))
			{
				playerCommonData.CheckShowUnlockFunction();
			}
		}
	}

	private bool IsHaveActivityTips()
	{
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		return mPlayerData.ActivityData.IsHaveActTips() || mPlayerData.CopyInfoData.IsHaveDailyCopyTips() || mPlayerData.RankPVPData.IsHavePVPTips() || mPlayerData.TowerData.IsHaveTowerTips();
	}

	public void UpdateEnhanceTipsFlag()
	{
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		CheckTips(mPlayerData.IsHaveEnhanceandRefineTips() || CheckSkillUpdateTips() || CheckTitleTips(), GameDefine.TIPS_TYPE.ENHANCE);
	}

	public void UpdateTips()
	{
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		UpdateMessageTips();
		CheckTips(mPlayerData.FriendInfo.IsHaveMailTips(), GameDefine.TIPS_TYPE.MAIL);
		CheckTips(mPlayerData.FriendInfo.IshavefriendApply() || mPlayerData.FriendInfo.IsHaveMailTips(), GameDefine.TIPS_TYPE.SOCIAL);
		CheckTips(mPlayerData.ActivityData.IsHaveGuildTips(), GameDefine.TIPS_TYPE.GUILD);
		CheckTips(mPlayerData.playerSlotData.CheckTips(), GameDefine.TIPS_TYPE.SLOT);
		CheckTips(mPlayerData.playerMountData.CheckTips(), GameDefine.TIPS_TYPE.VEHICLE);
		CheckTips(CheckTitleTips(), GameDefine.TIPS_TYPE.ACHIEVEMNT);
		CheckTips(mPlayerData.IsHaveItemTips(), GameDefine.TIPS_TYPE.ITEMS);
		UpdateEnhanceTipsFlag();
		CheckTips(CheckSkillUpdateTips(), GameDefine.TIPS_TYPE.SKILL);
		CheckTips(mPlayerData.welfareData.isHaveWelfareTips(), GameDefine.TIPS_TYPE.WELFARE);
		CheckTips(mPlayerData.welfareData.HaveDailyActivityTips(), GameDefine.TIPS_TYPE.DAILYACT);
		CheckTips(mPlayerData.welfareData.HaveVipTips(), GameDefine.TIPS_TYPE.SHOP);
		UpdateCityDamageFlag();
	}

	public void CheckTips(bool istrue, GameDefine.TIPS_TYPE typetips)
	{
		switch (typetips)
		{
		case GameDefine.TIPS_TYPE.SOCIAL:
			SocialFuncBtn.SetTips(istrue, FUNCTION_TYPE.SOCIAL);
			break;
		case GameDefine.TIPS_TYPE.GUILD:
			GuildFuncBtn.SetTips(istrue, FUNCTION_TYPE.GUILD);
			break;
		case GameDefine.TIPS_TYPE.SLOT:
			SlotFuncBtn.SetTips(istrue, FUNCTION_TYPE.LOTTO);
			break;
		case GameDefine.TIPS_TYPE.VEHICLE:
			CarFuncBtn.SetTips(istrue, FUNCTION_TYPE.CAR);
			break;
		case GameDefine.TIPS_TYPE.ITEMS:
			BagFuncBtn.SetTips(istrue, FUNCTION_TYPE.BAG);
			break;
		case GameDefine.TIPS_TYPE.ENHANCE:
			EnhanceFuncBtn.SetTips(istrue, FUNCTION_TYPE.ENHANCE);
			break;
		case GameDefine.TIPS_TYPE.SKILL:
			break;
		case GameDefine.TIPS_TYPE.WELFARE:
			GiftFuncBtn.SetTips(istrue, FUNCTION_TYPE.GIFT);
			break;
		case GameDefine.TIPS_TYPE.MAIL:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SOCIAL_MAIL))
			{
				if (istrue)
				{
					SocialFuncBtn.IconSp.spriteName = "CZ_zhuJieMianAnNiu_youJian";
					SocialFuncBtn.BtnLabel.text = StrDictionary.GetDictionaryString("#{100215}");
				}
				else
				{
					SocialFuncBtn.IconSp.spriteName = "CZ_zhuJieMianAnNiu_haoYou";
					SocialFuncBtn.BtnLabel.text = StrDictionary.GetDictionaryString("#{100115}");
				}
			}
			else
			{
				SocialFuncBtn.IconSp.spriteName = "CZ_zhuJieMianAnNiu_haoYou";
				SocialFuncBtn.BtnLabel.text = StrDictionary.GetDictionaryString("#{100115}");
			}
			break;
		case GameDefine.TIPS_TYPE.SHOP:
			ShopFuncBtn.SetTips(istrue, FUNCTION_TYPE.SHOP);
			break;
		case GameDefine.TIPS_TYPE.ACHIEVEMNT:
		case GameDefine.TIPS_TYPE.CHARACTER:
		case GameDefine.TIPS_TYPE.DAILYACT:
		case GameDefine.TIPS_TYPE.SEVENDAY:
			break;
		}
	}

	public void CheckRightDirTips()
	{
	}

	public void UpdateCityDamageFlag()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.IsOpenCityCapture(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID))
		{
			NGUITools.SetActive(CityDamageFlag.gameObject, state: true);
		}
		else
		{
			NGUITools.SetActive(CityDamageFlag.gameObject, state: false);
		}
	}

	public void UpdateSkillTips()
	{
		UpdateEnhanceTipsFlag();
		if (SingletonUnity<EquipStrengthenUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<EquipStrengthenUIRootLogic>.Instance.gameObject) && SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
		}
	}

	public bool CheckSkillUpdateTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SKILL))
		{
			return false;
		}
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		for (int i = 0; i < mainPlayer.CharacterSkillData.Count; i++)
		{
			CharacterSkillData characterSkillData = mainPlayer.CharacterSkillData[i];
			if (characterSkillData == null || !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(characterSkillData.UnlockLevel))
			{
				continue;
			}
			int index = characterSkillData.Index;
			if (index >= 4 && index <= 6)
			{
				SkillData skillDataById = DataManager.GetSkillDataById(characterSkillData.ID);
				SkillupgradeData skillupgradeDataByLevel = DataManager.GetSkillupgradeDataByLevel(characterSkillData.Level + 1);
				if (skillDataById != null && skillDataById.IsUpgrade != 0 && skillupgradeDataByLevel != null && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level > characterSkillData.Level + 1 && GameMoneyHelper.GetMoneyNum(skillupgradeDataByLevel.PriceType) >= skillupgradeDataByLevel.PriceValue)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void EnableBtnColor(List<UIButtonColor> btnList)
	{
		for (int i = 0; i < btnList.Count; i++)
		{
			if (btnList[i] != null)
			{
				btnList[i].enabled = true;
			}
		}
	}

	private void DisableBtnColor(List<UIButtonColor> btnList)
	{
		for (int i = 0; i < btnList.Count; i++)
		{
			if (btnList[i] != null)
			{
				btnList[i].enabled = false;
			}
		}
	}
}
