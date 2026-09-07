using System;
using System.Collections.Generic;
using UnityEngine;

public class QiangHuaListLogic : MonoBehaviour
{
	public List<EquipItemLogic> EquipList = new List<EquipItemLogic>();

	public UIGrid grid;

	public UIScrollView ScrollView;

	public EquipItemLogic.OnClickItem onClickEquipItem;

	public void Init()
	{
		for (int i = 0; i < EquipList.Count; i++)
		{
			EquipItemLogic equipItemLogic = EquipList[i];
			equipItemLogic.OnClick = (EquipItemLogic.OnClickItem)Delegate.Combine(equipItemLogic.OnClick, new EquipItemLogic.OnClickItem(OnClickEquipItem));
		}
	}

	public void UpdateEquipInfo(GameItem gameItem)
	{
		for (int i = 0; i < EquipList.Count; i++)
		{
			if (EquipList[i].mCurItem != null && !EquipList[i].mCurItem.IsEmpty() && gameItem != null && !gameItem.IsEmpty() && EquipList[i].mCurItem.ItemData.SubType == gameItem.ItemData.SubType)
			{
				EquipList[i].UpdateInfo(gameItem);
				break;
			}
		}
	}

	public void OnClickEquipItem(GameItem item)
	{
		if (onClickEquipItem != null)
		{
			onClickEquipItem(item);
		}
	}

	public void ResetEuipListInfo(List<GameItem> list, GameItem item, bool resetpos)
	{
		if (item == null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				EquipData equipDataById = DataManager.GetEquipDataById(list[i].ItemId);
				if (equipDataById != null && equipDataById.EquipType == EQUIP_BACKPACK_TYPE.WEAPON)
				{
					item = list[i];
					break;
				}
			}
		}
		if (item == null)
		{
			return;
		}
		int num = 0;
		for (int j = 0; j < list.Count; j++)
		{
			if (!list[j].IsEmpty())
			{
				EquipList[num].InitEuipInfo(list[j], list[j] == item);
				num++;
			}
		}
		for (int k = num; k < EquipList.Count; k++)
		{
			EquipList[k].ResetInfo();
		}
		if (resetpos)
		{
			ScrollView.ResetPosition();
		}
		grid.Reposition();
	}
}
