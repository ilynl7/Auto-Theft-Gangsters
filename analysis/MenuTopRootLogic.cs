using System;
using UnityEngine;

public class MenuTopRootLogic : MonoBehaviour
{
	public UILabel DiamondLabel;

	public UILabel GoldLabel;

	public UILabel CashLabel;

	private DelegateDefine.NoParamDelegate onClickBackBtn;

	private void OnEnable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
		UpdateMoney();
	}

	private void OnDisable()
	{
		UIUpdateEvent.UpdateMoneyEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateMoneyEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateMoney));
	}

	public void ResetBackBtn(DelegateDefine.NoParamDelegate func)
	{
		onClickBackBtn = func;
	}

	public void UpdateMoney()
	{
		DiamondLabel.text = $"{GameMoneyHelper.GetDiamond():N0}";
		GoldLabel.text = $"{GameMoneyHelper.GetGold():N0}";
		CashLabel.text = $"{GameMoneyHelper.GetCash():N0}";
	}

	public void OnClickBackBtn()
	{
		if (onClickBackBtn != null)
		{
			onClickBackBtn();
		}
	}

	public void OnClickAddDiamondBtn()
	{
	}

	public void OnClickGoldBtn()
	{
	}

	public void OnClickCashBtn()
	{
	}
}
