using System;
using System.Collections.Generic;
using UnityEngine;

public class AnnounceRootLogic : SingletonUnity<AnnounceRootLogic>
{
	public GameObject ObjRoot;

	public UILabel NameLabel1;

	public UILabel NameLabel2;

	public UISprite ItemSprite;

	public UISprite ItemQualitySprite;

	public UISprite FuncIconSprite;

	private AnnounceData mAnnounceData;

	public void Reset()
	{
		Refresh();
	}

	private void OnEnable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(Refresh));
	}

	private void OnDisable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(Refresh));
	}

	private void Refresh()
	{
		mAnnounceData = null;
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		List<AnnounceData> announceDataList = DataManager.GetAnnounceDataList();
		for (int i = 0; i < announceDataList.Count; i++)
		{
			if (announceDataList[i].StartLevel <= level && announceDataList[i].EndLevel > level)
			{
				mAnnounceData = announceDataList[i];
				break;
			}
		}
		if (mAnnounceData != null)
		{
			NGUITools.SetActive(ObjRoot, state: true);
			if (mAnnounceData.Type == 0)
			{
				NGUITools.SetActive(ItemSprite.gameObject, state: false);
				FuncIconSprite.spriteName = mAnnounceData.ICON;
				NameLabel1.text = mAnnounceData.MName1;
				NameLabel2.text = mAnnounceData.MName2;
				FuncIconSprite.width = 40;
				FuncIconSprite.height = 40;
				return;
			}
			if (mAnnounceData.Type == 1)
			{
				NGUITools.SetActive(ItemSprite.gameObject, state: false);
				FuncIconSprite.spriteName = mAnnounceData.ICON;
				NameLabel1.text = mAnnounceData.MName1;
				NameLabel2.text = mAnnounceData.MName2;
				FuncIconSprite.width = 36;
				FuncIconSprite.height = 36;
				return;
			}
			if (string.IsNullOrEmpty(mAnnounceData.ItemId))
			{
				NGUITools.SetActive(ObjRoot, state: false);
				return;
			}
			NGUITools.SetActive(FuncIconSprite.gameObject, state: false);
			ItemData itemDataByID = DataManager.GetItemDataByID(mAnnounceData.ItemId);
			if (itemDataByID != null)
			{
				ItemSprite.spriteName = itemDataByID.BackPackIcon;
				ItemQualitySprite.spriteName = ((EQUIP_QUALITY)mAnnounceData.quality).ToString();
			}
			NameLabel1.text = mAnnounceData.MName1;
			NameLabel2.text = mAnnounceData.MName2;
		}
		else
		{
			NGUITools.SetActive(ObjRoot, state: false);
		}
	}
}
