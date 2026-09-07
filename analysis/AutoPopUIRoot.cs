using System.Collections.Generic;

public class AutoPopUIRoot : SingletonUnity<AutoPopUIRoot>
{
	public static List<GameDefine.AUTOPOPTYPE> PopUIList = new List<GameDefine.AUTOPOPTYPE>();

	private GameDefine.AUTOPOPTYPE curShowUI;

	public void Reset()
	{
		PopUIList.Clear();
		if (LocalDataSaveManager.GetRateFlag() == 1)
		{
			PopUIList.Add(GameDefine.AUTOPOPTYPE.RATE);
		}
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		long push = playerCommonData.Push;
		if (push != -1)
		{
			if ((push & 1) != 0L && playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_CHECK))
			{
				PopUIList.Add(GameDefine.AUTOPOPTYPE.SIGNMONTH);
			}
			if ((push & 2) != 0L && playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_7DAY))
			{
				PopUIList.Add(GameDefine.AUTOPOPTYPE.SIGNWEEK);
			}
			if ((push & 4) != 0L && playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT_RETRIEVE))
			{
				PopUIList.Add(GameDefine.AUTOPOPTYPE.RETRIEVE);
			}
		}
		if (playerCommonData.Big_PackFlag && playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.BIGSALES))
		{
			PopUIList.Add(GameDefine.AUTOPOPTYPE.BIGSALE);
		}
		if (DataManager.GetTimerActivityTipsDataList().Count > 0)
		{
			PopUIList.Add(GameDefine.AUTOPOPTYPE.TIME_ACTIVITY_TIPS);
		}
		if (push != -1 && (push & 0x10) != 0L && playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.MYSTERYSHOP))
		{
			PopUIList.Add(GameDefine.AUTOPOPTYPE.MYSTERYSHOP);
		}
		if (PopUIList.Count > 0)
		{
			ShowPopUI();
		}
		else
		{
			Close();
		}
	}

	public void RemoveUI(GameDefine.AUTOPOPTYPE type)
	{
		PopUIList.Remove(type);
	}

	public void ShowPopUI()
	{
		curShowUI = PopUIList[0];
		PopUIList.RemoveAt(0);
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		switch (curShowUI)
		{
		case GameDefine.AUTOPOPTYPE.RATE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.RateRoot);
			break;
		case GameDefine.AUTOPOPTYPE.BIGSALE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BigPackRoot, delegate
			{
				SingletonUnity<BigPackRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(260, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_big_pack>();
				int diedFlag = LocalDataSaveManager.GetDiedFlag();
				if (diedFlag == 1)
				{
					LocalDataSaveManager.SetDiedFlag(2);
				}
			});
			break;
		case GameDefine.AUTOPOPTYPE.SIGNMONTH:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CommercialUIRoot, delegate
			{
				SingletonUnity<CommercialUIRootLogic>.Instance.OnClickMonthBtn();
			});
			break;
		case GameDefine.AUTOPOPTYPE.SIGNWEEK:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CommercialUIRoot, delegate
			{
				SingletonUnity<CommercialUIRootLogic>.Instance.OnClickWeekBtn();
			});
			break;
		case GameDefine.AUTOPOPTYPE.RETRIEVE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CommercialUIRoot, delegate
			{
				SingletonUnity<CommercialUIRootLogic>.Instance.OnClickRetrieveBtn();
			});
			break;
		case GameDefine.AUTOPOPTYPE.TIME_ACTIVITY_TIPS:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TimerActivityTipsRoot, delegate
			{
				SingletonUnity<TimerActivityTipsRootLogic>.Instance.Reset();
			});
			break;
		case GameDefine.AUTOPOPTYPE.MYSTERYSHOP:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MysteryShopRoot, delegate
			{
				SingletonUnity<MysteryShopRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(274, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_special_big_pack>();
			});
			break;
		case GameDefine.AUTOPOPTYPE.FIRSTBUY:
			break;
		}
	}

	public void NextPop(bool isUIJump = false)
	{
		if (checkCurShowUI())
		{
			return;
		}
		if (PopUIList.Count > 0)
		{
			if (!isUIJump)
			{
				ShowPopUI();
			}
		}
		else
		{
			Close(isUIJump);
		}
	}

	public void Close(bool isUIJump = false)
	{
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.PushShowFlag = true;
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.AutoPopUIRoot);
		if (!isUIJump)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckShowUnlockFunction();
		}
	}

	public bool checkCurShowUI()
	{
		switch (curShowUI)
		{
		case GameDefine.AUTOPOPTYPE.RATE:
			if (SingletonUnity<UIManager>.Instance.CheckUIExit(UIInfo.RateRoot))
			{
				return true;
			}
			break;
		case GameDefine.AUTOPOPTYPE.BIGSALE:
			if (SingletonUnity<UIManager>.Instance.CheckUIExit(UIInfo.BigPackRoot))
			{
				return true;
			}
			break;
		case GameDefine.AUTOPOPTYPE.SIGNMONTH:
			if (SingletonUnity<UIManager>.Instance.CheckUIExit(UIInfo.CommercialUIRoot))
			{
				return true;
			}
			break;
		case GameDefine.AUTOPOPTYPE.SIGNWEEK:
			if (SingletonUnity<UIManager>.Instance.CheckUIExit(UIInfo.CommercialUIRoot))
			{
				return true;
			}
			break;
		case GameDefine.AUTOPOPTYPE.RETRIEVE:
			if (SingletonUnity<UIManager>.Instance.CheckUIExit(UIInfo.CommercialUIRoot))
			{
				return true;
			}
			break;
		case GameDefine.AUTOPOPTYPE.TIME_ACTIVITY_TIPS:
			if (SingletonUnity<UIManager>.Instance.CheckUIExit(UIInfo.TimerActivityTipsRoot))
			{
				return true;
			}
			break;
		case GameDefine.AUTOPOPTYPE.MYSTERYSHOP:
			if (SingletonUnity<UIManager>.Instance.CheckUIExit(UIInfo.MysteryShopRoot))
			{
				return true;
			}
			break;
		}
		return false;
	}
}
