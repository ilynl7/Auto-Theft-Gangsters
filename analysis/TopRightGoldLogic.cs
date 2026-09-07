using System;
using UnityEngine;

public class TopRightGoldLogic : MonoBehaviour
{
	public UILabel GoldLable;

	public UILabel CashLabel;

	private void OnEnable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
		UpdateMoney();
	}

	private void OnDisable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
	}

	private void UpdateMoney()
	{
		GoldLable.text = GameMoneyHelper.GetGold().ToString();
		CashLabel.text = GameMoneyHelper.GetCash().ToString();
	}
}
