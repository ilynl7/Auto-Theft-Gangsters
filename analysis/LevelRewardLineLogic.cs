using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class LevelRewardLineLogic : MonoBehaviour
{
	public ShowRewardItems ShowRewardroot;

	public UISprite GetBtnSp;

	public UILabel GetLabel;

	public UILabel infoLabel;

	public UISprite CompleteFlag;

	private int TargetLevel;

	private int gettype;

	private int pricetype;

	private int pricenum;

	private bool isComplete;

	private LevelRewardData CurData;

	private int CurIndex;

	public void UpdateInfo(LevelRewardData curdata, int index, level_reward curinfo)
	{
		CurData = curdata;
		CurIndex = index;
		ShowRewardData showRewardData = null;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		switch (index)
		{
		case 0:
			TargetLevel = curdata.TargetLevel1;
			gettype = curdata.GetType1;
			pricetype = curdata.PriceType1;
			pricenum = curdata.PriceNum1;
			infoLabel.text = StrDictionary.GetDictionaryString(curdata.Info1, curdata.TargetLevel1);
			switch (playerData.Profession)
			{
			case PROFESSION_TYPE.XD:
				showRewardData = DataManager.GetShowRewardDataByID(curdata.XDShowReward1);
				break;
			case PROFESSION_TYPE.QJ:
				showRewardData = DataManager.GetShowRewardDataByID(curdata.QJShowReward1);
				break;
			case PROFESSION_TYPE.NQS:
				showRewardData = DataManager.GetShowRewardDataByID(curdata.NQSShowReward1);
				break;
			}
			break;
		case 1:
			TargetLevel = curdata.TargetLevel2;
			gettype = curdata.GetType2;
			pricetype = curdata.PriceType2;
			pricenum = curdata.PriceNum2;
			infoLabel.text = StrDictionary.GetDictionaryString(curdata.Info2, curdata.TargetLevel2);
			switch (playerData.Profession)
			{
			case PROFESSION_TYPE.XD:
				showRewardData = DataManager.GetShowRewardDataByID(curdata.XDShowReward2);
				break;
			case PROFESSION_TYPE.QJ:
				showRewardData = DataManager.GetShowRewardDataByID(curdata.QJShowReward2);
				break;
			case PROFESSION_TYPE.NQS:
				showRewardData = DataManager.GetShowRewardDataByID(curdata.NQSShowReward2);
				break;
			}
			break;
		case 2:
			TargetLevel = curdata.TargetLevel3;
			gettype = curdata.GetType3;
			pricetype = curdata.PriceType3;
			pricenum = curdata.PriceNum3;
			infoLabel.text = StrDictionary.GetDictionaryString(curdata.Info3, curdata.TargetLevel3);
			switch (playerData.Profession)
			{
			case PROFESSION_TYPE.XD:
				showRewardData = DataManager.GetShowRewardDataByID(curdata.XDShowReward3);
				break;
			case PROFESSION_TYPE.QJ:
				showRewardData = DataManager.GetShowRewardDataByID(curdata.QJShowReward3);
				break;
			case PROFESSION_TYPE.NQS:
				showRewardData = DataManager.GetShowRewardDataByID(curdata.NQSShowReward3);
				break;
			}
			break;
		}
		if (showRewardData != null)
		{
			NGUITools.SetActive(ShowRewardroot.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			NGUITools.SetActive(ShowRewardroot.gameObject, state: false);
		}
		isComplete = (curinfo.state & (1 << index)) != 0;
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		if (isComplete)
		{
			CompleteFlag.enabled = true;
			GetLabel.text = StrDictionary.GetDictionaryString("#{300403}");
			GetBtnSp.spriteName = GameDefine.BtnIconNew[1];
			return;
		}
		CompleteFlag.enabled = false;
		if (gettype == 0)
		{
			GetLabel.text = StrDictionary.GetDictionaryString("#{300402}");
		}
		else
		{
			GetLabel.text = GameMoneyHelper.GetMoneyValStr(pricenum, pricetype);
		}
		if (level >= TargetLevel)
		{
			GetBtnSp.spriteName = GameDefine.BtnIconNew[0];
		}
		else
		{
			GetBtnSp.spriteName = GameDefine.BtnIconNew[1];
		}
	}

	private void SetRewardItem(List<string> itemIds, List<EQUIP_QUALITY> qualitys, List<int> counts)
	{
		ShowRewardroot.ShowRewards(itemIds, qualitys, counts);
	}

	public void OnClickReceiveBtn()
	{
		if (isComplete)
		{
			return;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level < TargetLevel)
		{
			NoticeLogic.AddNotifyData("#{100834}");
		}
		else if (gettype == 1)
		{
			if (GameMoneyHelper.BeforeCheckBuy(pricetype, pricenum))
			{
				WaitResponseUIRootLogic.OpenWaitBox(297, 10f, 0f);
				receive_level_reward.request request = new receive_level_reward.request();
				request.ID = CurData.ID;
				request.index = CurIndex;
				NetLogic.GetInstance().Send<Protocol.receive_level_reward>(request);
			}
		}
		else
		{
			WaitResponseUIRootLogic.OpenWaitBox(297, 10f, 0f);
			receive_level_reward.request request2 = new receive_level_reward.request();
			request2.ID = CurData.ID;
			request2.index = CurIndex;
			NetLogic.GetInstance().Send<Protocol.receive_level_reward>(request2);
		}
	}
}
