using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SignMonthRootLogic : SingletonUnity<SignMonthRootLogic>
{
	public UISprite BtnSp;

	public UILabel BtnLabel;

	public List<DayRewardItem> DayItems;

	private List<SignInMonthData> SignDataList;

	public UIGrid ParentGrid;

	public UISprite PriceFlag;

	public UILabel PriceLabel;

	private int curSign;

	private int sysSign;

	private int ReplenishTimes;

	private bool curSignState;

	private bool ReplenishState;

	private int MonthDays;

	private string ReplenishStr = string.Empty;

	public Transform DayEffect;

	public void EnableReset()
	{
		curSignState = false;
		ReplenishState = false;
		UnityVersionUtil.SetActiveRecursive(PriceLabel.gameObject, state: false);
		SignDataList = DataManager.GetSignInMonthDataList();
		int num = SignDataList.Count - DayItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(DayItems[0].gameObject) as GameObject;
				DayRewardItem component = gameObject.GetComponent<DayRewardItem>();
				gameObject.name = $"dayitem{DayItems.Count:D2}";
				gameObject.transform.parent = ParentGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				DayItems.Add(component);
			}
		}
		ParentGrid.Reposition();
		for (int j = 0; j < DayItems.Count; j++)
		{
			DayItems[j].ResetPos(j);
			if (j < SignDataList.Count)
			{
				DayItems[j].UpdateItem(SignDataList[j].ItemID, SignDataList[j].ItemCount);
			}
			UnityVersionUtil.SetActiveRecursive(DayItems[j].gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(DayEffect.gameObject, state: false);
	}

	public void UpdateInfo(ret_request_30_day_info.request request)
	{
		curSign = (int)request.cur_sign;
		sysSign = (int)request.sys_sign;
		ReplenishTimes = (int)request.replenish;
		curSignState = request.cur_sign_state;
		ReplenishState = request.replenish_sign_state;
		MonthDays = (int)request.count;
		if (request.HasStr)
		{
			ReplenishStr = request.str;
		}
		else
		{
			ReplenishStr = string.Empty;
		}
		RefershUI();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Sign30", "open");
	}

	public void UpdateInfo(ret_sign_30_day.request request)
	{
		if (curSignState)
		{
			LocalDataSaveManager.SetRewardFlag();
		}
		curSign = (int)request.cur_sign;
		sysSign = (int)request.sys_sign;
		ReplenishTimes = (int)request.replenish;
		curSignState = request.cur_sign_state;
		ReplenishState = request.replenish_sign_state;
		MonthDays = (int)request.count;
		if (request.HasStr)
		{
			ReplenishStr = request.str;
		}
		else
		{
			ReplenishStr = string.Empty;
		}
		RefershUI();
		int num = curSign - 1;
		ItemData itemDataByID = DataManager.GetItemDataByID(SignDataList[num].ItemID);
		SimpleRewardRootLogic.AddReward(itemDataByID, SignDataList[num].ItemCount, itemDataByID.Quality);
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Sign30", $"sign_{num + 1}");
	}

	public void RefershUI()
	{
		DayEffect.transform.parent = ParentGrid.transform.parent;
		UnityVersionUtil.SetActiveRecursive(DayEffect.gameObject, state: false);
		string[] array = ReplenishStr.Split('#');
		for (int i = 0; i < DayItems.Count; i++)
		{
			if (i < MonthDays)
			{
				UnityVersionUtil.SetActiveRecursive(DayItems[i].gameObject, state: true);
				if (i > sysSign - 1)
				{
					DayItems[i].UpdataStateInfo(DayItemState.NONE);
					continue;
				}
				if (i > curSign - 1 && i <= sysSign - 1)
				{
					DayItems[i].UpdataStateInfo(DayItemState.CAN_REPLENISH);
					continue;
				}
				if (i < curSign - 1)
				{
					bool isRepget = false;
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j].Equals((i + 1).ToString()))
						{
							isRepget = true;
							break;
						}
					}
					DayItems[i].UpdataStateInfo(DayItemState.ISGET, isRepget);
					continue;
				}
				if (curSignState)
				{
					DayItems[i].UpdataStateInfo(DayItemState.CURSIGN);
					DayEffect.transform.parent = DayItems[i].transform;
					DayEffect.localPosition = Vector3.zero;
					UnityVersionUtil.SetActiveRecursive(DayEffect.gameObject, state: true);
					continue;
				}
				bool isRepget2 = false;
				for (int k = 0; k < array.Length; k++)
				{
					if (array[k].Equals((i + 1).ToString()))
					{
						isRepget2 = true;
						break;
					}
				}
				DayItems[i].UpdataStateInfo(DayItemState.ISGET, isRepget2);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(DayItems[i].gameObject, state: false);
			}
		}
		ParentGrid.Reposition();
		if (curSignState)
		{
			BtnLabel.text = StrDictionary.GetDictionaryString("#{300201}");
			BtnSp.spriteName = GameDefine.BtnIconNew[0];
			UnityVersionUtil.SetActiveRecursive(PriceLabel.gameObject, state: false);
		}
		else if (ReplenishState)
		{
			BtnLabel.text = StrDictionary.GetDictionaryString("#{300202}");
			BtnSp.spriteName = GameDefine.BtnIconNew[0];
			SignInMonthData signInMonthData = SignDataList[ReplenishTimes];
			PriceFlag.spriteName = GameMoneyHelper.GetMoneyIcon(signInMonthData.PriceType);
			PriceLabel.text = $"{signInMonthData.PriceCost}";
			UnityVersionUtil.SetActiveRecursive(PriceLabel.gameObject, state: true);
		}
		else
		{
			BtnLabel.text = StrDictionary.GetDictionaryString("#{300403}");
			BtnSp.spriteName = GameDefine.BtnIconNew[1];
			UnityVersionUtil.SetActiveRecursive(PriceLabel.gameObject, state: false);
		}
	}

	public void OnClickSignBtn()
	{
		if (!curSignState && !ReplenishState)
		{
			return;
		}
		if (!curSignState)
		{
			SignInMonthData signInMonthData = SignDataList[ReplenishTimes];
			if (!GameMoneyHelper.BeforeCheckBuy(signInMonthData.PriceType, signInMonthData.PriceCost))
			{
				return;
			}
		}
		WaitResponseUIRootLogic.OpenWaitBox(254, 10f, 0f);
		sign_30_day.request request = new sign_30_day.request();
		request.day = curSign;
		NetLogic.GetInstance().Send<Protocol.sign_30_day>();
	}
}
