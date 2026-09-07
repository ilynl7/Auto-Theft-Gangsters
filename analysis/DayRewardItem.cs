using UnityEngine;

public class DayRewardItem : MonoBehaviour
{
	public UISprite iconSprite;

	public UISprite qualitySprite;

	public UILabel itemCountLabel;

	private ItemData curItemData;

	private GameItem item;

	public UISprite ComBg;

	public UISprite Comflag;

	public UISprite ReplenishSp;

	public void ResetPos(int listindex)
	{
		ComBg.enabled = false;
		Comflag.enabled = false;
	}

	public void UpdataStateInfo(DayItemState itemstate, bool isRepget = false)
	{
		switch (itemstate)
		{
		case DayItemState.ISGET:
			ComBg.enabled = true;
			Comflag.enabled = true;
			if (isRepget)
			{
				ReplenishSp.enabled = true;
			}
			else
			{
				ReplenishSp.enabled = false;
			}
			break;
		case DayItemState.CURSIGN:
			ComBg.enabled = false;
			Comflag.enabled = false;
			ReplenishSp.enabled = false;
			break;
		case DayItemState.CAN_REPLENISH:
			ComBg.enabled = false;
			Comflag.enabled = false;
			ReplenishSp.enabled = true;
			break;
		case DayItemState.NONE:
			ComBg.enabled = true;
			Comflag.enabled = false;
			ReplenishSp.enabled = false;
			break;
		}
	}

	public void UpdateItem(string itemId, int count)
	{
		ItemData itemData = (curItemData = DataManager.GetItemDataByID(itemId));
		item = new GameItem(itemId, itemData.QualityType, count);
		if (itemData != null)
		{
			if (itemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				qualitySprite.spriteName = itemData.QualityType.ToString();
				Debug.LogWarning("Sign 30 have equip item!");
			}
			else
			{
				qualitySprite.spriteName = itemData.QualityType.ToString();
			}
			iconSprite.spriteName = itemData.BackPackIcon;
			if (count > 1)
			{
				itemCountLabel.text = $"x{count}";
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
	}

	public void OnClickShowItems()
	{
		if (curItemData != null)
		{
			int level = 0;
			if (item.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(item.ItemData.SubType);
			}
			ItemInfoRootLogicNew.ShowItemTips(item, level);
		}
	}
}
