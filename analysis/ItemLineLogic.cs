using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemLineLogic : MonoBehaviour
{
	public ItemUILogic.OnClickItemDelegate onClickItem;

	public List<ItemUILogic> ItemObjList = new List<ItemUILogic>();

	public int CellWidth;

	public int CellHeight;

	public int CurIndex;

	private bool InitFlag;

	private void Awake()
	{
		Init();
	}

	public void Init()
	{
		if (!InitFlag)
		{
			InitFlag = true;
			for (int i = 0; i < ItemObjList.Count; i++)
			{
				ItemUILogic itemUILogic = ItemObjList[i];
				itemUILogic.onClickItem = (ItemUILogic.OnClickItemDelegate)Delegate.Combine(itemUILogic.onClickItem, new ItemUILogic.OnClickItemDelegate(OnClickItem));
			}
		}
	}

	private void OnClickItem(GameItem curItem, ItemUILogic curUIItem)
	{
		if (onClickItem != null)
		{
			onClickItem(curItem, curUIItem);
		}
	}

	public void Reset(List<GameItem> itemList, bool needShowEmpty, int curLineIndex, int startIndex, int maxIndex)
	{
		PlayerModelPageRootLogic instance = SingletonUnity<PlayerModelPageRootLogic>.Instance;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		CurIndex = curLineIndex;
		for (int i = 0; i < ItemObjList.Count; i++)
		{
			if (itemList[i] != null)
			{
				if (itemList[i].IsEmpty() && !needShowEmpty)
				{
					NGUITools.SetActive(ItemObjList[i].gameObject, state: false);
					continue;
				}
				NGUITools.SetActive(ItemObjList[i].gameObject, state: true);
				if (startIndex + i < maxIndex)
				{
					if (instance != null && itemList[i].ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
					{
						EquipData equipDataById = DataManager.GetEquipDataById(itemList[i].ItemId);
						if (equipDataById.profession == playerData.Profession)
						{
							ItemObjList[i].UpdateItemUI(itemList[i], itemList[i].GetItemCombatVal() > instance.GetTargetTypeEquipCombatVal((EQUIP_BACKPACK_TYPE)itemList[i].ItemData.SubType));
						}
						else
						{
							ItemObjList[i].UpdateItemUI(itemList[i]);
						}
					}
					else
					{
						ItemObjList[i].UpdateItemUI(itemList[i]);
					}
				}
				else
				{
					ItemObjList[i].SetItemLock();
				}
			}
			else if (needShowEmpty)
			{
				NGUITools.SetActive(ItemObjList[i].gameObject, state: true);
				if (startIndex + i < maxIndex)
				{
					ItemObjList[i].SetItemEmpty(ITEM_CONTAINER_TYPE.ITEM_BACKPACK);
				}
				else
				{
					ItemObjList[i].SetItemLock();
				}
			}
			else
			{
				NGUITools.SetActive(ItemObjList[i].gameObject, state: false);
			}
		}
	}

	[ContextMenu("ResetItemLine")]
	public void ResetPosition()
	{
		if (ItemObjList.Count == 0)
		{
			GameObject gameObject = null;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				gameObject = base.transform.GetChild(i).gameObject;
				ItemObjList.Add(gameObject.GetComponent<ItemUILogic>());
			}
		}
		int count = ItemObjList.Count;
		int num = 0;
		num = ((count % 2 != 0) ? (-((count + 1) / 2 - 1) * CellWidth) : (-(count / 2 - 1) * CellWidth - CellWidth / 2));
		for (int j = 0; j < ItemObjList.Count; j++)
		{
			ItemObjList[j].transform.localPosition = new Vector3(num + j * CellWidth, 0f, 0f);
		}
	}
}
