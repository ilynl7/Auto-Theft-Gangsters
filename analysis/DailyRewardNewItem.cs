using SprotoType;
using UnityEngine;

public class DailyRewardNewItem : MonoBehaviour
{
	public UISprite iconSprite;

	public UISprite qualitySprite;

	public UILabel itemCountLabel;

	private ItemData curItemData;

	public UILabel TargetLabel;

	private daily_reward curInfo;

	private DailyActiveRewardData curData;

	public GameObject ComFlagObj;

	public GameObject CanGetObj;

	public void UpdateInfo(daily_reward curinfo)
	{
		curInfo = curinfo;
		curData = DataManager.GetDailyActiveRewardDataById(curinfo.ID);
		TargetLabel.text = string.Empty + curData.Score;
		long state = curinfo.state;
		if (state >= 0 && state <= 2)
		{
			switch (state)
			{
			case 0L:
				NGUITools.SetActive(ComFlagObj, state: false);
				NGUITools.SetActive(TargetLabel.gameObject, state: true);
				NGUITools.SetActive(CanGetObj, state: false);
				break;
			case 1L:
				NGUITools.SetActive(ComFlagObj, state: false);
				NGUITools.SetActive(TargetLabel.gameObject, state: true);
				NGUITools.SetActive(CanGetObj, state: true);
				break;
			case 2L:
				NGUITools.SetActive(ComFlagObj, state: true);
				NGUITools.SetActive(TargetLabel.gameObject, state: false);
				NGUITools.SetActive(CanGetObj, state: false);
				break;
			}
		}
		curItemData = DataManager.GetItemDataByID(curData.ItemID);
		GameItem curitem = new GameItem(curData.ItemID, curItemData.QualityType, curData.ItemCount);
		UpdateItem(curitem);
	}

	public void UpdateItem(GameItem curitem)
	{
		ItemData itemData = (curItemData = DataManager.GetItemDataByID(curitem.ItemId));
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
	}

	public void OnClickGetBtn()
	{
		if (curInfo.state == 1)
		{
			WaitResponseUIRootLogic.OpenWaitBox(265, 10f, 0f);
			require_daily_active_reward.request request = new require_daily_active_reward.request();
			request.ID = curInfo.ID;
			NetLogic.GetInstance().Send<Protocol.require_daily_active_reward>(request);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OpenBoxRoot, delegate
			{
				SingletonUnity<OpenBoxRootLogic>.Instance.Reset();
			});
			return;
		}
		ItemData itemDataByID = DataManager.GetItemDataByID(curData.ItemID);
		GameItem gameItem = new GameItem(curData.ItemID, itemDataByID.QualityType, curData.ItemCount);
		int level = 0;
		if (gameItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(gameItem.ItemData.SubType);
		}
		ItemInfoRootLogicNew.ShowItemTips(gameItem, level);
	}
}
