using UnityEngine;

public class WeekDayRewardItem : MonoBehaviour
{
	public UISprite iconSprite;

	public UISprite qualitySprite;

	public UILabel itemCountLabel;

	private ItemData curItemData;

	private GameItem item;

	public UISprite Comflag;

	public UILabel DayLabel;

	public GameObject GetSpObj;

	private DelegateDefine.OneIntParamDelegate clickFun;

	private int curDay;

	public void UpdataStateInfo(DayItemState itemstate)
	{
		switch (itemstate)
		{
		case DayItemState.ISGET:
			UnityVersionUtil.SetActiveRecursive(Comflag.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(GetSpObj, state: false);
			break;
		case DayItemState.CURSIGN:
			UnityVersionUtil.SetActiveRecursive(Comflag.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(GetSpObj, state: true);
			break;
		case DayItemState.CAN_REPLENISH:
			UnityVersionUtil.SetActiveRecursive(Comflag.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(GetSpObj, state: false);
			break;
		case DayItemState.NONE:
			UnityVersionUtil.SetActiveRecursive(Comflag.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(GetSpObj, state: false);
			break;
		}
	}

	public void UpdateItem(GameItem curitem, int dayi, DelegateDefine.OneIntParamDelegate clickbtn)
	{
		ItemData itemData = (curItemData = DataManager.GetItemDataByID(curitem.ItemId));
		item = curitem;
		clickFun = clickbtn;
		curDay = dayi;
		if (itemData != null)
		{
			if (itemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				qualitySprite.spriteName = curitem.GetItemQuality().ToString();
			}
			else
			{
				qualitySprite.spriteName = itemData.QualityType.ToString();
			}
			iconSprite.spriteName = itemData.BackPackIcon;
			if (curitem.StackNum > 1)
			{
				itemCountLabel.text = $"x{curitem.StackNum}";
			}
			else
			{
				itemCountLabel.text = string.Empty;
			}
		}
		else
		{
			NGUITools.SetActive(base.gameObject, state: false);
		}
		DayLabel.text = StrDictionary.GetDictionaryString("#{300901}", dayi);
	}

	public void OnClickDayItem()
	{
		if (clickFun != null)
		{
			clickFun(curDay);
		}
	}
}
