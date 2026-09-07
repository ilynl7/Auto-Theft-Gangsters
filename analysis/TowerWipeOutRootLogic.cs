using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class TowerWipeOutRootLogic : SingletonUnity<TowerWipeOutRootLogic>
{
	public ShowRewardItems ShowRewardItem;

	public UILabel WipeOutTargetLabel;

	public UILabel NeedTimeLabel;

	public UILabel WipeOutCostLabel;

	public GameObject NormalWipeOutBtn;

	public GameObject SpecialWipeOutBtn;

	public GameObject GetRewardBtn;

	public UISprite GetRewardSprite;

	private bool mWipingFlag;

	private float mRestTime;

	private bool mCanGet;

	private int curNeedDiamond;

	private int mGrantRewardType;

	private int curFloorId;

	private string remainStr;

	private int curTime;

	public void Reset(int curFloor, int TargetFloor)
	{
		mWipingFlag = false;
		Dictionary<string, item> dictionary = new Dictionary<string, item>();
		ShowRewardData showRewardData = null;
		TowerData towerData = null;
		int num = 0;
		for (int i = curFloor; i <= TargetFloor; i++)
		{
			showRewardData = DataManager.GetShowRewardDataByID(DataManager.GetTowerDataByFloorID(i).ShowRewardID);
			towerData = DataManager.GetTowerDataByFloorID(i);
			num += towerData.ExistTime;
			for (int j = 0; j < showRewardData.ItemIdList.Count; j++)
			{
				if (dictionary.ContainsKey(showRewardData.ItemIdList[j]))
				{
					dictionary[showRewardData.ItemIdList[j]].itemCount += showRewardData.CountList[j];
					continue;
				}
				item item = new item();
				item.itemId = showRewardData.ItemIdList[j];
				item.quality = (long)showRewardData.QualityList[j];
				item.itemCount = showRewardData.CountList[j];
				dictionary.Add(item.itemId, item);
			}
		}
		ShowRewardItem.ShowRewards(dictionary);
		WipeOutTargetLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{101525}"), TargetFloor + 1);
		NeedTimeLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{101580}"), new TimeSpan(0, 0, num));
		curNeedDiamond = num / 60;
		WipeOutCostLabel.text = GameMoneyHelper.GetMoneyValStr(curNeedDiamond, GameDefine.MONEY_TYPE.DIAMOND);
		NGUITools.SetActive(GetRewardBtn, state: false);
	}

	public void ResetWipingPage(int curFloor, int targetFloor, int restTime)
	{
		remainStr = StrDictionary.GetDictionaryString("#{101580}");
		mWipingFlag = true;
		mRestTime = restTime;
		WipeOutTargetLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{101525}"), targetFloor + 1);
		NeedTimeLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{101580}"), new TimeSpan(0, 0, restTime));
		Dictionary<string, item> dictionary = new Dictionary<string, item>();
		ShowRewardData showRewardData = null;
		TowerData towerData = null;
		int num = 0;
		for (int i = curFloor; i <= targetFloor; i++)
		{
			showRewardData = DataManager.GetShowRewardDataByID(DataManager.GetTowerDataByFloorID(i).ShowRewardID);
			towerData = DataManager.GetTowerDataByFloorID(i);
			num += towerData.ExistTime;
			for (int j = 0; j < showRewardData.ItemIdList.Count; j++)
			{
				if (dictionary.ContainsKey(showRewardData.ItemIdList[j]))
				{
					dictionary[showRewardData.ItemIdList[j]].itemCount += showRewardData.CountList[j];
					continue;
				}
				item item = new item();
				item.itemId = showRewardData.ItemIdList[j];
				item.quality = (long)showRewardData.QualityList[j];
				item.itemCount = showRewardData.CountList[j];
				dictionary.Add(item.itemId, item);
			}
		}
		ShowRewardItem.ShowRewards(dictionary);
		curNeedDiamond = num / 60;
		WipeOutCostLabel.text = GameMoneyHelper.GetMoneyValStr(curNeedDiamond, GameDefine.MONEY_TYPE.DIAMOND);
		NGUITools.SetActive(NormalWipeOutBtn, state: false);
		SpecialWipeOutBtn.transform.localPosition = new Vector3(0f, SpecialWipeOutBtn.transform.localPosition.y, 0f);
		NGUITools.SetActive(GetRewardBtn, state: false);
	}

	public void ResetWipeOutRewardPage(int curFloor, int targetFloor)
	{
		Dictionary<string, item> dictionary = new Dictionary<string, item>();
		ShowRewardData showRewardData = null;
		TowerData towerData = null;
		int num = 0;
		for (int i = curFloor; i <= targetFloor; i++)
		{
			showRewardData = DataManager.GetShowRewardDataByID(DataManager.GetTowerDataByFloorID(i).ShowRewardID);
			towerData = DataManager.GetTowerDataByFloorID(i);
			num += towerData.ExistTime;
			for (int j = 0; j < showRewardData.ItemIdList.Count; j++)
			{
				if (dictionary.ContainsKey(showRewardData.ItemIdList[j]))
				{
					dictionary[showRewardData.ItemIdList[j]].itemCount += showRewardData.CountList[j];
					continue;
				}
				item item = new item();
				item.itemId = showRewardData.ItemIdList[j];
				item.quality = (long)showRewardData.QualityList[j];
				item.itemCount = showRewardData.CountList[j];
				dictionary.Add(item.itemId, item);
			}
		}
		ShowRewardItem.ShowRewards(dictionary);
		mGrantRewardType = 0;
		WipeOutTargetLabel.text = $"WipeOut Reward:";
		NGUITools.SetActive(NeedTimeLabel.gameObject, state: false);
		NGUITools.SetActive(NormalWipeOutBtn.gameObject, state: false);
		NGUITools.SetActive(SpecialWipeOutBtn.gameObject, state: false);
		NGUITools.SetActive(GetRewardBtn, state: true);
		mCanGet = true;
		if (mCanGet)
		{
			GetRewardSprite.spriteName = "CZ_anNiu_1";
		}
		else
		{
			GetRewardSprite.spriteName = "CZ_anNiu_2+";
		}
	}

	public void ResetGetSpecialRewardPage(int floorId, bool canGet)
	{
		curFloorId = floorId;
		TowerData towerDataByFloorID = DataManager.GetTowerDataByFloorID(floorId);
		if (towerDataByFloorID == null)
		{
			OnClickCloseBtn();
			return;
		}
		ShowRewardData showRewardDataByID = DataManager.GetShowRewardDataByID(towerDataByFloorID.SpecialRewardId);
		ShowRewardItem.ShowRewards(showRewardDataByID.ItemIdList, showRewardDataByID.QualityList, showRewardDataByID.CountList);
		mGrantRewardType = 1;
		WipeOutTargetLabel.text = StrDictionary.GetDictionaryString("#{101531}", floorId + 1);
		NGUITools.SetActive(NeedTimeLabel.gameObject, state: false);
		NGUITools.SetActive(NormalWipeOutBtn.gameObject, state: false);
		NGUITools.SetActive(SpecialWipeOutBtn.gameObject, state: false);
		NGUITools.SetActive(GetRewardBtn, state: true);
		mCanGet = canGet;
		if (canGet)
		{
			GetRewardSprite.spriteName = "CZ_anNiu_1";
		}
		else
		{
			GetRewardSprite.spriteName = "CZ_anNiu_2+";
		}
	}

	public void OnClickWipeOutNormalBtn()
	{
		tower_wipe_out.request request = new tower_wipe_out.request();
		request.wipeType = 0L;
		NetLogic.GetInstance().Send<Protocol.tower_wipe_out>(request);
		OnClickCloseBtn();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tower", "wipe_out", "wipe_out_time");
	}

	public void OnClickWipeOutSpecialBtn()
	{
		if (GameMoneyHelper.BeforeCheckBuyTop(GameDefine.MONEY_TYPE.DIAMOND, curNeedDiamond))
		{
			tower_wipe_out.request request = new tower_wipe_out.request();
			request.wipeType = 1L;
			NetLogic.GetInstance().Send<Protocol.tower_wipe_out>(request);
			WaitResponseUIRootLogic.OpenWaitBox(208, 10f, 0f);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tower", "wipe_out", "wipe_out_buy");
		}
	}

	public void OnClickGetRewardBtn()
	{
		if (mCanGet)
		{
			grant_tower_reward.request request = new grant_tower_reward.request();
			request.type = mGrantRewardType;
			request.id = curFloorId;
			NetLogic.GetInstance().Send<Protocol.grant_tower_reward>(request);
			OnClickCloseBtn();
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TowerWipeOutRootLogic);
	}

	private void Update()
	{
		if (mWipingFlag)
		{
			curTime = (int)(mRestTime - Time.deltaTime);
			if (curTime != (int)mRestTime)
			{
				NeedTimeLabel.text = $"{remainStr}:{new TimeSpan(0, 0, curTime)}";
			}
			mRestTime -= Time.deltaTime;
		}
	}
}
