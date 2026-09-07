using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class LevelPackLineItem : MonoBehaviour
{
	public UILabel InfoLabel;

	public List<RewardItem> rewardItems;

	private level_pack curInfo;

	private LevelPackageData curData;

	public UILabel BtnLabel;

	public UISprite btnSp;

	public UIGrid parentGrid;

	private List<GameItem> ItemList = new List<GameItem>();

	public UISprite completeFlag;

	public void refershInfo(level_pack curinfo, LevelPackageData curdata)
	{
		if (curinfo.ID.Equals(curInfo.ID))
		{
			UpdateInfo(curinfo, curdata);
		}
	}

	public void UpdateInfo(level_pack curinfo, LevelPackageData curdata)
	{
		curInfo = curinfo;
		curData = curdata;
		InfoLabel.text = StrDictionary.GetDictionaryString("#{300401}", curData.LvTarget);
		if (curInfo.state == 1)
		{
			btnSp.spriteName = GameDefine.BtnIconNew[0];
		}
		else
		{
			btnSp.spriteName = GameDefine.BtnIconNew[1];
		}
		if (curInfo.state == 2)
		{
			BtnLabel.text = StrDictionary.GetDictionaryString("#{300403}");
			completeFlag.enabled = true;
		}
		else
		{
			BtnLabel.text = StrDictionary.GetDictionaryString("#{300402}");
			completeFlag.enabled = false;
		}
		ItemList.Clear();
		if (!string.IsNullOrEmpty(curData.ItemID1))
		{
			ItemList.Add(new GameItem(curData.ItemID1, (EQUIP_QUALITY)curData.Quality1, curData.ItemCount1));
		}
		if (!string.IsNullOrEmpty(curData.ItemID2))
		{
			ItemList.Add(new GameItem(curData.ItemID2, (EQUIP_QUALITY)curData.Quality2, curData.ItemCount2));
		}
		if (!string.IsNullOrEmpty(curData.ItemID3))
		{
			ItemList.Add(new GameItem(curData.ItemID3, (EQUIP_QUALITY)curData.Quality3, curData.ItemCount3));
		}
		if (!string.IsNullOrEmpty(curData.ItemID4))
		{
			ItemList.Add(new GameItem(curData.ItemID4, (EQUIP_QUALITY)curData.Quality4, curData.ItemCount4));
		}
		if (!string.IsNullOrEmpty(curData.ItemID5))
		{
			ItemList.Add(new GameItem(curData.ItemID5, (EQUIP_QUALITY)curData.Quality5, curData.ItemCount5));
		}
		int num = ItemList.Count - rewardItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(rewardItems[0].gameObject) as GameObject;
				RewardItem component = gameObject.GetComponent<RewardItem>();
				gameObject.name = $"reward{rewardItems.Count:D2}";
				gameObject.transform.parent = parentGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				rewardItems.Add(component);
			}
		}
		for (int j = 0; j < rewardItems.Count; j++)
		{
			if (j < ItemList.Count)
			{
				int level = 0;
				if (ItemList[j].ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(ItemList[j].ItemData.SubType);
				}
				rewardItems[j].UpdateItem(ItemList[j], level);
				UnityVersionUtil.SetActiveRecursive(rewardItems[j].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(rewardItems[j].gameObject, state: false);
			}
		}
		parentGrid.Reposition();
	}

	public void OnClickBtn()
	{
		if (curInfo.state == 1)
		{
			WaitResponseUIRootLogic.OpenWaitBox(262, 10f, 0f);
			require_level_reward.request request = new require_level_reward.request();
			request.ID = curInfo.ID;
			NetLogic.GetInstance().Send<Protocol.require_level_reward>(request);
		}
		else if (curInfo.state == 0L)
		{
			NoticeLogic.AddNotifyData("#{300404}");
		}
	}
}
