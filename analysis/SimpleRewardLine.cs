using UnityEngine;

public class SimpleRewardLine : MonoBehaviour
{
	public delegate void OnFinished(SimpleRewardLine obj);

	public UILabel NumLabel;

	public UISprite IconSprite;

	public UISprite QualitySprite;

	public TweenPosition TwPosition;

	public TweenAlpha TwAlph;

	public OnFinished onFinished;

	public void Reset(ItemData curItem, int count, EQUIP_QUALITY quality, OnFinished func)
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		string empty = string.Empty;
		empty = curItem.Type switch
		{
			GameDefine.ITEM_TYPE.EQUIP => StrDictionary.GetDictionaryString("#{100139}", curItem.MName) + $"[ffff00]+{count}[-]", 
			GameDefine.ITEM_TYPE.ADD_DIAMOND => GameMoneyHelper.GetMoneyPre(GameDefine.MONEY_TYPE.DIAMOND) + $" [ffff00]+{count}[-]", 
			GameDefine.ITEM_TYPE.ADD_GOLD => GameMoneyHelper.GetMoneyPre(GameDefine.MONEY_TYPE.GOLD) + $" [ffff00]+{count}[-]", 
			GameDefine.ITEM_TYPE.ADD_COIN => GameMoneyHelper.GetMoneyPre(GameDefine.MONEY_TYPE.CASH) + $" [ffff00]+{count}[-]", 
			_ => StrDictionary.GetDictionaryString("#{100139}", curItem.MName) + $"[ffff00]+{count}[-]", 
		};
		onFinished = func;
		Reset(empty);
	}

	private void Reset(string label)
	{
		NumLabel.text = label;
		TwPosition.ResetToBeginning();
		TwAlph.ResetToBeginning();
		TwPosition.PlayForward();
		TwAlph.PlayForward();
	}

	public void OnFlyFinished()
	{
		if (onFinished != null)
		{
			onFinished(this);
		}
	}
}
