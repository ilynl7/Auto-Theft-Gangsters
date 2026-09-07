using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class RetrieveLineLogic : MonoBehaviour
{
	public UILabel TitleLabel;

	private retrieve_info curInfo;

	private RetrieveData curData;

	public UISprite normalbtnSp;

	public UISprite perbtnSp;

	public UILabel normalPrice;

	public UILabel perPrice;

	public ShowRewardItems ShowRewardItemsScripts;

	public UISprite completeFlag;

	private long normal_Price;

	private long per_Price;

	public void UpdateInfo(retrieve_info curinfo, RetrieveData curdata)
	{
		curInfo = curinfo;
		curData = curdata;
		TitleLabel.text = StrDictionary.GetDictionaryString(curData.Name);
		normal_Price = curData.PriceCost2 + curData.AddCost2 * curinfo.count;
		if (normal_Price > curData.MaxCost2)
		{
			normal_Price = curData.MaxCost2;
		}
		normalPrice.text = GameMoneyHelper.GetMoneyValStr(normal_Price, curData.PriceType2);
		per_Price = curData.PriceCost1 + curData.AddCost1 * curinfo.count;
		if (per_Price > curData.MaxCost1)
		{
			per_Price = curData.MaxCost1;
		}
		perPrice.text = GameMoneyHelper.GetMoneyValStr(per_Price, curData.PriceType1);
		if (curInfo.state == 0L)
		{
			normalbtnSp.color = new Color(0f, 0.8745098f, 1f);
			perbtnSp.spriteName = GameDefine.BtnIconNew[0];
		}
		else if (curInfo.state == 1)
		{
			normalbtnSp.color = Color.white;
			perbtnSp.spriteName = GameDefine.BtnIconNew[1];
		}
		if (curInfo.state == 2 || curInfo.state == 1)
		{
			completeFlag.enabled = true;
			UnityVersionUtil.SetActiveRecursive(normalbtnSp.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(perbtnSp.gameObject, state: false);
		}
		else
		{
			completeFlag.enabled = false;
			UnityVersionUtil.SetActiveRecursive(normalbtnSp.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(perbtnSp.gameObject, state: true);
		}
		ShowRewardData showRewardData = null;
		string id = DataManager.GetAdaptDataByID(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.Level).DorpKeyDic[curData.ShowRewardID];
		showRewardData = DataManager.GetShowRewardDataByID(id);
		if (showRewardData != null)
		{
			List<int> counts = new List<int>(showRewardData.CountList);
			if (curdata.isDanceOrExp)
			{
			}
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), counts);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemsScripts.gameObject, state: false);
		}
	}

	private void SetRewardItem(List<string> itemIds, List<EQUIP_QUALITY> qualitys, List<int> counts)
	{
		ShowRewardItemsScripts.ShowRewards(itemIds, qualitys, counts);
	}

	public void OnClickNormalBtn()
	{
		if (curInfo.state == 0L && GameMoneyHelper.BeforeCheckBuy(curData.PriceType2, (int)normal_Price))
		{
			WaitResponseUIRootLogic.OpenWaitBox(279, 10f, 0f);
			request_retrieve.request request = new request_retrieve.request();
			request.ID = curInfo.ID;
			request.Type = curData.PriceType2;
			NetLogic.GetInstance().Send<Protocol.request_retrieve>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Retrive", $"require_{curInfo.ID}_normal");
		}
	}

	public void OnClickPerfectBtn()
	{
		if (curInfo.state == 0L && GameMoneyHelper.BeforeCheckBuy(curData.PriceType1, (int)per_Price))
		{
			WaitResponseUIRootLogic.OpenWaitBox(279, 10f, 0f);
			request_retrieve.request request = new request_retrieve.request();
			request.ID = curInfo.ID;
			request.Type = curData.PriceType1;
			NetLogic.GetInstance().Send<Protocol.request_retrieve>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Retrive", $"require_{curInfo.ID}_perfect");
		}
	}
}
