using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class MenuBaseRootLogic : SingletonUnity<MenuBaseRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UISprite ExitBtn;

	public UILabel DiamondLabel;

	public UILabel GoldLabel;

	public UILabel CashLabel;

	public GameObject GangObj;

	public UILabel GangLabel;

	public UILabel PageNameLabel;

	public GameObject LeftTabRoot;

	public GameObject VideoObjs;

	public GameObject VideoCanObj;

	public GameObject VideoNoObj;

	public UILabel[] VideoGetLabel;

	public UIGrid LeftTabGrid;

	public MenuBaseTabBtnLogic BtnPrefab;

	public List<MenuBaseTabBtnLogic> mCurBtnList;

	private DelegateDefine.NoParamDelegate onClickBackBtn;

	private List<MenuTabBtnInfo> mCurMenuTabBtnInfoList;

	public TweenScale CashAnima;

	public TweenScale GoldAnima;

	public TweenScale DiamondAnima;

	public TweenScale GangAnima;

	private long curCashnum;

	private long curGoldnum;

	private long curDiamondnum;

	private long curGangnum;

	private long targetCashnum;

	private long targetGoldnum;

	private long targetDiamondnum;

	private long targetGangnum;

	private bool showMoneyAnima;

	private long cashratio;

	private long goldratio;

	private long diamondratio;

	private long gangratio;

	private bool[] isplayanima = new bool[3];

	private long delaTimeAnima = 1L;

	private float temptime;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (SingletonUnity<TutorialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.CloseCheck();
		}
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn(needDelayFlag: true);
			mOnClickTutorialBtn = null;
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	private void Start()
	{
		SingletonDontDestoryUnity<GameManager>.Instance.OnUnityAdsStateChange = VideoBtnUpdate;
	}

	public void ResetPage(List<MenuTabBtnInfo> leftBtnInfo, DelegateDefine.NoParamDelegate backBtnFunc, bool hideTab = false, string titleStr = null)
	{
		if (hideTab)
		{
			UnityVersionUtil.SetActiveRecursive(LeftTabRoot, state: false);
			PageNameLabel.text = titleStr;
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(LeftTabRoot, state: true);
		}
		if (leftBtnInfo != null)
		{
			mCurMenuTabBtnInfoList = leftBtnInfo;
			int num = mCurMenuTabBtnInfoList.Count - mCurBtnList.Count;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(BtnPrefab.gameObject) as GameObject;
					MenuBaseTabBtnLogic component = gameObject.GetComponent<MenuBaseTabBtnLogic>();
					gameObject.name = mCurBtnList.Count.ToString();
					gameObject.transform.parent = LeftTabGrid.transform;
					gameObject.transform.localScale = Vector3.one;
					mCurBtnList.Add(component);
				}
			}
			for (int j = 0; j < mCurBtnList.Count; j++)
			{
				if (j < mCurMenuTabBtnInfoList.Count)
				{
					UnityVersionUtil.SetActiveRecursive(mCurBtnList[j].gameObject, state: true);
					mCurBtnList[j].Reset(mCurMenuTabBtnInfoList[j]);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(mCurBtnList[j].gameObject, state: false);
				}
			}
			LeftTabGrid.Reposition();
		}
		onClickBackBtn = backBtnFunc;
		hideGangMoney();
	}

	private void UpdateBtnAlpha(int index)
	{
		for (int i = 0; i < mCurBtnList.Count; i++)
		{
			if (index == i)
			{
				mCurBtnList[i].BtnWidget.alpha = 1f;
			}
			else
			{
				mCurBtnList[i].BtnWidget.alpha = 0.4f;
			}
		}
	}

	public bool AutoClickTipsTap()
	{
		for (int i = 0; i < mCurBtnList.Count; i++)
		{
			if (UnityVersionUtil.IsActive(mCurBtnList[i].gameObject) && mCurBtnList[i].isTips)
			{
				mCurBtnList[i].OnClickBtn();
				return true;
			}
		}
		return false;
	}

	public void RefershTips()
	{
		for (int i = 0; i < mCurBtnList.Count; i++)
		{
			if (UnityVersionUtil.IsActive(mCurBtnList[i].gameObject))
			{
				mCurBtnList[i].UpdateTips();
			}
		}
	}

	public void SetTargetBtnToggleEnable(int index)
	{
		for (int i = 0; i < mCurBtnList.Count; i++)
		{
			if (i == index)
			{
				mCurBtnList[i].BtnToggle.alpha = 1f;
				mCurBtnList[i].BtnWidget.alpha = 1f;
			}
			else
			{
				mCurBtnList[i].BtnToggle.alpha = 0f;
				mCurBtnList[i].BtnWidget.alpha = 0.4f;
			}
		}
		if (index < mCurMenuTabBtnInfoList.Count)
		{
			SetPageLabel(mCurMenuTabBtnInfoList[index].PageName);
		}
	}

	private void OnEnable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
		showMoneyAnima = false;
		curCashnum = GameMoneyHelper.GetCash();
		curGoldnum = GameMoneyHelper.GetGold();
		curDiamondnum = GameMoneyHelper.GetDiamond();
		curGangnum = GameMoneyHelper.GetGuildContribute();
		DiamondLabel.text = $"{GameMoneyHelper.GetDiamond():N0}";
		GoldLabel.text = $"{GameMoneyHelper.GetGold():N0}";
		CashLabel.text = $"{GameMoneyHelper.GetCash():N0}";
		GangLabel.text = $"{GameMoneyHelper.GetGuildContribute():N0}";
	}

	private void OnDisable()
	{
		showMoneyAnima = false;
		StopMoneyAnima();
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
		FunctionTipsRootLogic.ClearHandTip();
		ClearTutorialEvent();
	}

	public void ResetBackBtn(DelegateDefine.NoParamDelegate func)
	{
		onClickBackBtn = func;
	}

	public void OnClickBackBtn()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.SKILL_UPGRADE_EXIT || TutorialManager.CurStep == TUTORIAL_STEP.ENHANCE_EXIT || TutorialManager.CurStep == TUTORIAL_STEP.CAR_CLICK_BACK || TutorialManager.CurStep == TUTORIAL_STEP.ENHANCE_ALL_EXIT || TutorialManager.CurStep == TUTORIAL_STEP.CREATE_TEAM_CLICK_BACK || TutorialManager.CurStep == TUTORIAL_STEP.CAR_GIFT_CLICK_BACK || TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_CLICK_BACK)
		{
			CheckTutorialEvent();
		}
		if (onClickBackBtn != null)
		{
			onClickBackBtn();
		}
	}

	private void Update()
	{
		if (showMoneyAnima)
		{
			temptime += Time.deltaTime;
			if (diamondratio != 0L)
			{
				DiamondLabel.text = $"{(float)curDiamondnum + temptime * (float)diamondratio:N0}";
			}
			if (goldratio != 0L)
			{
				GoldLabel.text = $"{(float)curGoldnum + temptime * (float)goldratio:N0}";
			}
			if (cashratio != 0L)
			{
				CashLabel.text = $"{(float)curCashnum + temptime * (float)cashratio:N0}";
			}
			if (gangratio != 0L)
			{
				GangLabel.text = $"{(float)curGangnum + temptime * (float)gangratio:N0}";
			}
			if (temptime >= (float)delaTimeAnima)
			{
				showMoneyAnima = false;
				StopMoneyAnima();
			}
		}
	}

	public void UpdateMoney()
	{
		if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			showMoneyAnima = true;
			temptime = 0f;
			targetCashnum = GameMoneyHelper.GetCash();
			targetGoldnum = GameMoneyHelper.GetGold();
			targetDiamondnum = GameMoneyHelper.GetDiamond();
			targetGangnum = GameMoneyHelper.GetGuildContribute();
			if (Mathf.Abs(curCashnum - targetCashnum) > float.Epsilon)
			{
				if (!CashAnima.enabled)
				{
					CashAnima.PlayForward();
				}
				cashratio = (targetCashnum - curCashnum) / delaTimeAnima;
			}
			else
			{
				cashratio = 0L;
			}
			if (Mathf.Abs(curGoldnum - targetGoldnum) > float.Epsilon)
			{
				if (!GoldAnima.enabled)
				{
					GoldAnima.PlayForward();
				}
				goldratio = (targetGoldnum - curGoldnum) / delaTimeAnima;
			}
			else
			{
				goldratio = 0L;
			}
			if (Mathf.Abs(curDiamondnum - targetDiamondnum) > float.Epsilon)
			{
				if (!DiamondAnima.enabled)
				{
					DiamondAnima.PlayForward();
				}
				diamondratio = (targetDiamondnum - curDiamondnum) / delaTimeAnima;
			}
			else
			{
				diamondratio = 0L;
			}
			if (Mathf.Abs(curGangnum - targetGangnum) > float.Epsilon)
			{
				if (UnityVersionUtil.IsActive(GangObj) && !GangAnima.enabled)
				{
					GangAnima.PlayForward();
				}
				gangratio = (targetGangnum - curGangnum) / delaTimeAnima;
			}
			else
			{
				gangratio = 0L;
			}
		}
		else
		{
			showMoneyAnima = false;
			curCashnum = GameMoneyHelper.GetCash();
			curGoldnum = GameMoneyHelper.GetGold();
			curDiamondnum = GameMoneyHelper.GetDiamond();
			curGangnum = GameMoneyHelper.GetGuildContribute();
			targetCashnum = curCashnum;
			targetGoldnum = curGoldnum;
			targetDiamondnum = curDiamondnum;
			targetGangnum = curGangnum;
			DiamondLabel.text = $"{curDiamondnum:N0}";
			GoldLabel.text = $"{curGoldnum:N0}";
			CashLabel.text = $"{curCashnum:N0}";
			GangLabel.text = $"{curGangnum:N0}";
		}
	}

	public void StopMoneyAnima()
	{
		if (CashAnima != null)
		{
			if (CashAnima.mAmountPerDelta < 0f)
			{
				CashAnima.mAmountPerDelta = 0f - CashAnima.mAmountPerDelta;
			}
			CashAnima.ResetToBeginning();
			CashAnima.enabled = false;
		}
		if (GoldAnima != null)
		{
			if (GoldAnima.mAmountPerDelta < 0f)
			{
				GoldAnima.mAmountPerDelta = 0f - GoldAnima.mAmountPerDelta;
			}
			GoldAnima.ResetToBeginning();
			GoldAnima.enabled = false;
		}
		if (DiamondAnima != null)
		{
			if (DiamondAnima.mAmountPerDelta < 0f)
			{
				DiamondAnima.mAmountPerDelta = 0f - DiamondAnima.mAmountPerDelta;
			}
			DiamondAnima.ResetToBeginning();
			DiamondAnima.enabled = false;
		}
		if (GangAnima != null)
		{
			if (GangAnima.mAmountPerDelta < 0f)
			{
				GangAnima.mAmountPerDelta = 0f - GangAnima.mAmountPerDelta;
			}
			GangAnima.ResetToBeginning();
			GangAnima.enabled = false;
		}
		curDiamondnum = targetDiamondnum;
		curGoldnum = targetGoldnum;
		curCashnum = targetCashnum;
		curGangnum = targetGangnum;
		DiamondLabel.text = $"{curDiamondnum:N0}";
		GoldLabel.text = $"{curGoldnum:N0}";
		CashLabel.text = $"{curCashnum:N0}";
		GangLabel.text = $"{curGangnum:N0}";
	}

	public void OnClickDiamondBtn()
	{
		if (CheckShopOpen())
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopShopRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopDiamondBuyRoot, delegate
			{
				SingletonUnity<PopDiamondBuyRootLogic>.Instance.EnableReset();
				ask_shop_list.request rpcReq = new ask_shop_list.request
				{
					type = 4L,
					subType = 1L
				};
				NetLogic.GetInstance().Send<Protocol.ask_shop_list>(rpcReq);
				WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			});
		}
	}

	public void OnClickGoldBtn()
	{
		if (CheckShopOpen())
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopDiamondBuyRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopShopRoot, delegate
			{
				SingletonUnity<PopShopRootLogic>.Instance.EnableReset();
				ask_shop_list.request request = new ask_shop_list.request();
				ItemData itemDataByID = DataManager.GetItemDataByID("5006");
				GameDefine.SHOP_TYPE itemShopType = itemDataByID.GetItemShopType();
				request.type = (long)itemShopType;
				request.itemId = "5006";
				request.subType = 1L;
				SingletonUnity<PopShopRootLogic>.Instance.ShowItemProdect(request.itemId);
				NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request);
				WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			});
		}
	}

	public void OnClickCashBtn()
	{
		if (!CheckShopOpen())
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopDiamondBuyRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopShopRoot, delegate
		{
			SingletonUnity<PopShopRootLogic>.Instance.EnableReset();
			ask_shop_list.request request = new ask_shop_list.request();
			string text = GameMoneyHelper.GetShopMoneyItemID(GameDefine.MONEY_TYPE.CASH);
			if (string.IsNullOrEmpty(text))
			{
				text = "5001";
			}
			ItemData itemDataByID = DataManager.GetItemDataByID(text);
			GameDefine.SHOP_TYPE itemShopType = itemDataByID.GetItemShopType();
			request.type = (long)itemShopType;
			request.itemId = text;
			request.subType = 1L;
			SingletonUnity<PopShopRootLogic>.Instance.ShowItemProdect(request.itemId);
			NetLogic.GetInstance().Send<Protocol.ask_shop_list>(request);
			WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
		});
	}

	public void OnClickGangBtn()
	{
		NoticeLogic.AddNotifyData("#{200508}");
	}

	private bool CheckShopOpen()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (!playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP))
		{
			int condition = DataManager.GetFunctionDataById(3006.ToString()).Condition;
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100649}", condition));
			return false;
		}
		return true;
	}

	public void SetPageLabel(string nameStr)
	{
		PageNameLabel.text = nameStr;
	}

	public void ShowGangMoney()
	{
		NGUITools.SetActive(GangObj.gameObject, state: true);
		NGUITools.SetActive(VideoObjs.gameObject, state: false);
	}

	public void hideGangMoney()
	{
		NGUITools.SetActive(GangObj.gameObject, state: false);
		NGUITools.SetActive(VideoObjs.gameObject, state: true);
		VideoBtnUpdate(SingletonDontDestoryUnity<GameManager>.Instance.IsUnityAdsReady);
	}

	public void VideoBtnUpdate(bool enabled)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		bool flag = false;
		if (playerData.VideoTimes >= playerData.VideoMaxTimes)
		{
			flag = true;
		}
		for (int i = 0; i < VideoGetLabel.Length; i++)
		{
			VideoGetLabel[i].text = "+" + playerData.VideoDiamond;
		}
		if (enabled && !flag)
		{
			NGUITools.SetActive(VideoCanObj, state: true);
			NGUITools.SetActive(VideoNoObj, state: false);
		}
		else
		{
			NGUITools.SetActive(VideoCanObj, state: false);
			NGUITools.SetActive(VideoNoObj, state: true);
		}
	}

	public void OnClickVideoBtn()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.IsUnityAdsReady)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.OnUnityAdsFinished = VideoFinish;
			SingletonDontDestoryUnity<GameManager>.Instance.ShowUnityAds();
		}
	}

	private void VideoFinish(bool IsFinish)
	{
		if (IsFinish)
		{
			watch_video_info.request rpcReq = new watch_video_info.request();
			NetLogic.GetInstance().Send<Protocol.watch_video_info>(rpcReq);
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.OnUnityAdsStateChange != null)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.OnUnityAdsStateChange(SingletonDontDestoryUnity<GameManager>.Instance.IsUnityAdsReady);
		}
	}
}
