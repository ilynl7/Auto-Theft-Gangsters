using SprotoType;
using UnityEngine;

public class DailyActiveRewardItem : MonoBehaviour
{
	public UILabel TargetLabel;

	public UISprite BgSp;

	private daily_reward curInfo;

	private DailyActiveRewardData curData;

	public GameObject ComFlagObj;

	public GameObject CanGetObj;

	public void UpdateInfo(daily_reward curinfo)
	{
		curInfo = curinfo;
		curData = DataManager.GetDailyActiveRewardDataById(curinfo.ID);
		if (curData == null)
		{
			return;
		}
		TargetLabel.text = string.Empty + curData.Score;
		long state = curinfo.state;
		if (state >= 0 && state <= 2)
		{
			switch (state)
			{
			case 0L:
				UnityVersionUtil.SetActiveRecursive(ComFlagObj, state: false);
				UnityVersionUtil.SetActiveRecursive(TargetLabel.gameObject, state: true);
				UnityVersionUtil.SetActiveRecursive(CanGetObj, state: false);
				BgSp.enabled = true;
				break;
			case 1L:
				UnityVersionUtil.SetActiveRecursive(ComFlagObj, state: false);
				UnityVersionUtil.SetActiveRecursive(TargetLabel.gameObject, state: true);
				UnityVersionUtil.SetActiveRecursive(CanGetObj, state: true);
				BgSp.enabled = false;
				break;
			case 2L:
				UnityVersionUtil.SetActiveRecursive(ComFlagObj, state: true);
				UnityVersionUtil.SetActiveRecursive(TargetLabel.gameObject, state: false);
				UnityVersionUtil.SetActiveRecursive(CanGetObj, state: false);
				BgSp.enabled = false;
				break;
			}
		}
	}

	public void OnClickGetBtn()
	{
		if (curData == null)
		{
			return;
		}
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
