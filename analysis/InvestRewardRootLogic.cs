using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class InvestRewardRootLogic : SingletonUnity<InvestRewardRootLogic>
{
	public UIWrapContentNew uiWrapContent;

	private int lineMinCount = 6;

	public UIWidget WrapContentBottomWidget;

	public UIScrollView uiScrollView;

	public List<InvestLineItem> LineItems;

	private List<invest_pack> invest_list;

	private List<InvestData> InvestDataList = new List<InvestData>();

	private Dictionary<string, invest_pack> invest_Dic;

	public UITexture Texturebanner;

	public UISprite BuyBtn;

	public UILabel BtnLabel;

	public UISprite CompleteFlag;

	protected override void Awake()
	{
		base.Awake();
		uiWrapContent.enabled = false;
		UIWrapContentNew uIWrapContentNew = uiWrapContent;
		uIWrapContentNew.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContentNew.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
	}

	public void EnableReset()
	{
		for (int i = 0; i < LineItems.Count; i++)
		{
			UnityVersionUtil.SetActiveRecursive(LineItems[i].gameObject, state: false);
		}
		NGUITools.SetActive(BuyBtn.gameObject, state: false);
		CompleteFlag.enabled = false;
		InitTexture();
	}

	public void InitTexture()
	{
		if (Texturebanner.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(GameDefine.TextrueBannerInvest, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(string name, Texture textureObj)
	{
		Texturebanner.mainTexture = textureObj;
	}

	public void Reset(ret_request_invest_pack.request request)
	{
		InvestDataList.Clear();
		invest_Dic = request.invest_pack;
		invest_list = new List<invest_pack>(request.invest_pack.Values);
		if (invest_Dic.ContainsKey("0") && invest_Dic["0"].state == -1)
		{
			NGUITools.SetActive(BuyBtn.gameObject, state: true);
			CompleteFlag.enabled = false;
			InvestData investDataBuyId = DataManager.GetInvestDataBuyId("0");
			if (string.IsNullOrEmpty(investDataBuyId.Dollor))
			{
				BtnLabel.text = $"$19.99";
			}
			else
			{
				BtnLabel.text = $"${investDataBuyId.Dollor}";
			}
		}
		else
		{
			NGUITools.SetActive(BuyBtn.gameObject, state: false);
			CompleteFlag.enabled = true;
		}
		for (int i = 0; i < invest_list.Count; i++)
		{
			InvestDataList.Add(DataManager.GetInvestDataBuyId(invest_list[i].ID));
		}
		InvestDataList.Sort((InvestData x, InvestData y) => x.LvTarget - y.LvTarget);
		int num = Mathf.Min(InvestDataList.Count, lineMinCount) - LineItems.Count;
		if (num > 0)
		{
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(LineItems[0].gameObject) as GameObject;
				InvestLineItem component = gameObject.GetComponent<InvestLineItem>();
				gameObject.name = $"{LineItems.Count:D2}";
				gameObject.transform.parent = uiWrapContent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				LineItems.Add(component);
			}
		}
		for (int k = 0; k < LineItems.Count; k++)
		{
			if (k < InvestDataList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(LineItems[k].gameObject, state: true);
				LineItems[k].UpdateInfo(invest_Dic[InvestDataList[k].ID], InvestDataList[k], k);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(LineItems[k].gameObject, state: false);
			}
		}
		uiWrapContent.minIndex = 1 - InvestDataList.Count;
		WrapContentBottomWidget.height = InvestDataList.Count * uiWrapContent.itemSize;
		if (InvestDataList.Count == 1)
		{
			uiWrapContent.maxIndex = 1;
		}
		uiWrapContent.SortBasedOnScrollMovement();
		uiScrollView.ResetPosition();
		uiWrapContent.enabled = true;
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Invest", "open");
	}

	public void UpdateInfo(string id)
	{
		if (invest_Dic.ContainsKey(id))
		{
			invest_Dic[id].state = 2L;
		}
		for (int i = 0; i < InvestDataList.Count; i++)
		{
			if (InvestDataList[i].ID.Equals(id))
			{
				for (int j = 0; j < LineItems.Count; j++)
				{
					LineItems[j].refershinfo(invest_Dic[id], InvestDataList[i], i);
				}
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Invest", $"require_{id}");
				break;
			}
		}
	}

	public void UpdateInfo(ret_buy_invest_pack.request request)
	{
		InvestDataList.Clear();
		invest_Dic = request.invest_pack;
		invest_list = new List<invest_pack>(request.invest_pack.Values);
		if (invest_Dic.ContainsKey("0") && invest_Dic["0"].state == -1)
		{
			NGUITools.SetActive(BuyBtn.gameObject, state: true);
			CompleteFlag.enabled = false;
			InvestData investDataBuyId = DataManager.GetInvestDataBuyId("0");
			BtnLabel.text = $"${investDataBuyId.Dollor}";
		}
		else
		{
			NGUITools.SetActive(BuyBtn.gameObject, state: false);
			CompleteFlag.enabled = true;
		}
		for (int i = 0; i < invest_list.Count; i++)
		{
			InvestDataList.Add(DataManager.GetInvestDataBuyId(invest_list[i].ID));
		}
		InvestDataList.Sort((InvestData x, InvestData y) => x.LvTarget - y.LvTarget);
		int num = Mathf.Min(InvestDataList.Count, lineMinCount) - LineItems.Count;
		if (num > 0)
		{
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(LineItems[0].gameObject) as GameObject;
				InvestLineItem component = gameObject.GetComponent<InvestLineItem>();
				gameObject.name = $"{LineItems.Count:D2}";
				gameObject.transform.parent = uiWrapContent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				LineItems.Add(component);
			}
		}
		for (int k = 0; k < LineItems.Count; k++)
		{
			if (k < InvestDataList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(LineItems[k].gameObject, state: true);
				LineItems[k].UpdateInfo(invest_Dic[InvestDataList[k].ID], InvestDataList[k], k);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(LineItems[k].gameObject, state: false);
			}
		}
		uiWrapContent.minIndex = 1 - InvestDataList.Count;
		WrapContentBottomWidget.height = InvestDataList.Count * uiWrapContent.itemSize;
		if (InvestDataList.Count == 1)
		{
			uiWrapContent.maxIndex = 1;
		}
		uiWrapContent.SortBasedOnScrollMovement();
		uiScrollView.ResetPosition();
		uiWrapContent.enabled = true;
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Invest", "invest_buy");
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		InvestLineItem itemLogic = LineItems[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex));
	}

	private void ResetItemLine(InvestLineItem itemLogic, int idx)
	{
		if (idx < InvestDataList.Count)
		{
			itemLogic.UpdateInfo(invest_Dic[InvestDataList[idx].ID], InvestDataList[idx], idx);
		}
	}

	public void OnClickBuyInvestBtn()
	{
		if (!invest_Dic.ContainsKey("0") || invest_Dic["0"].state != -1)
		{
			return;
		}
		InvestData investDataBuyId = DataManager.GetInvestDataBuyId("0");
		if (!string.IsNullOrEmpty(investDataBuyId.ProductId))
		{
			SingletonDontDestoryUnity<GameManager>.Instance.Billing(investDataBuyId.ProductId);
			if (GameSettingData.IsTestBilling)
			{
				WaitResponseUIRootLogic.OpenWaitBox(266, 10f, 0f);
				check_purchase.request request = new check_purchase.request();
				request.productId = investDataBuyId.ProductId;
				NetLogic.GetInstance().Send<Protocol.check_purchase>(request);
			}
		}
	}
}
