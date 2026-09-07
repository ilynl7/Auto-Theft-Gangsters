using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ConsignRootLogic : SingletonUnity<ConsignRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	private int currentPage;

	public ConsignBuyLogic consignBuyLogic;

	public ConsignOnSaleLogic consignOnSaleLogic;

	public ConsignSellLogic consignSellLogic;

	public GameObject SelectFlag;

	public LabelState BuyLabelState;

	public LabelState SaleLabelState;

	public LabelState SellLabelState;

	public GameObject Tab1Obj;

	public GameObject Tab2Obj;

	public GameObject Tab3Obj;

	public int CurrentPage => currentPage;

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

	private void OnEnable()
	{
		currentPage = 0;
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			List<MenuTabBtnInfo> leftBtnInfo = new List<MenuTabBtnInfo>();
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(leftBtnInfo, CloseUI, hideTab: true, StrDictionary.GetDictionaryString("#{100105}"));
		});
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Trade", "open", "opentimes");
	}

	private void ChangeTab(int subIndex = 0)
	{
		switch (currentPage)
		{
		case 0:
		{
			UnityVersionUtil.SetActiveRecursive(consignBuyLogic.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(consignOnSaleLogic.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(consignSellLogic.gameObject, state: false);
			consignBuyLogic.Reset();
			SelectFlag.transform.parent = Tab1Obj.transform;
			Vector3 localPosition3 = SelectFlag.transform.localPosition;
			localPosition3.x = 0f;
			SelectFlag.transform.localPosition = localPosition3;
			BuyLabelState.active = true;
			SaleLabelState.active = false;
			SellLabelState.active = false;
			break;
		}
		case 1:
		{
			UnityVersionUtil.SetActiveRecursive(consignBuyLogic.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(consignOnSaleLogic.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(consignSellLogic.gameObject, state: true);
			consignSellLogic.Reset(subIndex);
			SelectFlag.transform.parent = Tab2Obj.transform;
			Vector3 localPosition2 = SelectFlag.transform.localPosition;
			localPosition2.x = 0f;
			SelectFlag.transform.localPosition = localPosition2;
			BuyLabelState.active = false;
			SaleLabelState.active = false;
			SellLabelState.active = true;
			break;
		}
		case 2:
		{
			UnityVersionUtil.SetActiveRecursive(consignBuyLogic.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(consignOnSaleLogic.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(consignSellLogic.gameObject, state: false);
			RequestOnSaleList();
			consignOnSaleLogic.Reset();
			SelectFlag.transform.parent = Tab3Obj.transform;
			Vector3 localPosition = SelectFlag.transform.localPosition;
			localPosition.x = 0f;
			SelectFlag.transform.localPosition = localPosition;
			BuyLabelState.active = false;
			SaleLabelState.active = true;
			SellLabelState.active = false;
			break;
		}
		}
	}

	public void RefreshOnSaleNow()
	{
		if (currentPage == 2)
		{
			consignOnSaleLogic.Reset();
		}
	}

	public void RefreshOnBuyList(ret_consign_ask_items_info.request request)
	{
		if (currentPage == 0)
		{
			consignBuyLogic.UpdateList(request);
		}
	}

	public void BuySuccess(long id)
	{
		if (currentPage == 0)
		{
			consignBuyLogic.BuySuccess(id);
		}
	}

	public void SellSuccess()
	{
		if (currentPage == 1)
		{
			consignSellLogic.UpdateInfo(isNeedResetPos: false);
		}
	}

	private void RequestOnSaleList()
	{
		consign_ask_my_items.request rpcReq = new consign_ask_my_items.request();
		NetLogic.GetInstance().Send<Protocol.consign_ask_my_items>(rpcReq);
	}

	public void Reset()
	{
		ChangeTab();
	}

	public void Reset(int tapIndex, int subIndex = 0)
	{
		currentPage = tapIndex;
		ChangeTab(subIndex);
	}

	public void ClickTab1()
	{
		if (currentPage != 0)
		{
			currentPage = 0;
			ChangeTab();
		}
	}

	public void ClickTab2()
	{
		if (currentPage != 1)
		{
			currentPage = 1;
			ChangeTab();
			if (TutorialManager.CurStep == TUTORIAL_STEP.SELL_ITEM_CLICK_TAB)
			{
				CheckTutorialEvent();
			}
		}
	}

	public void ClickTab3()
	{
		if (currentPage != 2)
		{
			currentPage = 2;
			ChangeTab();
		}
	}

	public void CloseUI()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ConsignUIRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
	}
}
