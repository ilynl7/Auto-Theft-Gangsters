using System;
using System.Collections.Generic;
using UnityEngine;

public class CopyDrugUseUIRoot : SingletonUnity<CopyDrugUseUIRoot>
{
	public UIGrid Grid;

	public List<CopyDrugItemLogic> DrugItemList = new List<CopyDrugItemLogic>();

	private Dictionary<long, float> mTimeDict = new Dictionary<long, float>();

	private List<GameItem> mList;

	private PlayerData playerData;

	private bool isFirtst = true;

	protected override void Awake()
	{
		base.Awake();
		playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
	}

	private void OnEnable()
	{
		isFirtst = true;
		mList = null;
		UIUpdateEvent.SyncBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.SyncBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdatePotion));
		UIUpdateEvent.UpdateBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdatePotion));
		Reset();
	}

	private void OnDisable()
	{
		UIUpdateEvent.SyncBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.SyncBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdatePotion));
		UIUpdateEvent.UpdateBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdatePotion));
	}

	public void Reset()
	{
		if (playerData == null)
		{
			playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		}
		List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(playerData.ItemBackPack, IsAll: false, GameDefine.ITEM_TYPE.BUFF);
		if (mList == null)
		{
			mList = new List<GameItem>();
			for (int i = 0; i < targetTypeItem.Count; i++)
			{
				mList.Add(targetTypeItem[i].ShallowCopy());
			}
		}
		else
		{
			for (int j = 0; j < mList.Count; j++)
			{
				mList[j].StackNum = 0;
			}
			for (int k = 0; k < targetTypeItem.Count; k++)
			{
				for (int l = 0; l < mList.Count; l++)
				{
					if (mList[l].ItemId == targetTypeItem[k].ItemId)
					{
						mList[l].StackNum = targetTypeItem[k].StackNum;
					}
				}
			}
		}
		for (int m = 0; m < DrugItemList.Count; m++)
		{
			NGUITools.SetActive(DrugItemList[m].gameObject, m < mList.Count);
		}
		for (int n = 0; n < mList.Count; n++)
		{
			if (n < DrugItemList.Count)
			{
				if (!mTimeDict.ContainsKey(mList[n].IndexId))
				{
					mTimeDict[mList[n].IndexId] = -1f;
				}
				float time = mTimeDict[mList[n].IndexId];
				DrugItemList[n].Reset(mList[n], time);
			}
		}
		Grid.Reposition();
	}

	public void OnResetPosition(GameItem item)
	{
		if (item.StackNum <= 0)
		{
			mList.Remove(item);
		}
		Grid.Reposition();
	}

	public void UseItem(CopyDrugItemLogic logic, GameItem item)
	{
		ItemData itemData = item.ItemData;
		EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(itemData.Function.ToString());
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (effInfoDataById != null && mainPlayer != null)
		{
			if (!mTimeDict.ContainsKey(item.IndexId))
			{
				mTimeDict[item.IndexId] = -1f;
			}
			mTimeDict[item.IndexId] = Time.realtimeSinceStartup + effInfoDataById.BuffDurationSecond;
			mainPlayer.UseBuffDrag(item);
			logic.UpdateTime(mTimeDict[item.IndexId]);
		}
	}

	public void UpdatePotion()
	{
		Reset();
	}
}
