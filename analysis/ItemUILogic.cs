using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ItemUILogic : MonoBehaviour
{
	public delegate void OnClickItemDelegate(GameItem item, ItemUILogic itemUILogic);

	public OnClickItemDelegate onClickItem;

	public ItemData curItemData;

	public GameItem curItem;

	public UISprite ItemIcon;

	public UILabel ItemNumLabel;

	public UISprite equipFlagSprite;

	public UISprite ItemQualityIcon;

	public UIWidget RootWidget;

	public UISprite NewflagSprite;

	public UISprite TimeLimitSp;

	public UISprite SellChoosePic;

	public GameObject UpArrowObj;

	public GameObject AppraiseObj;

	public GameObject StarObj;

	public List<UISprite> StarList;

	public GameObject AddObj;

	public UILabel AddLabel;

	public bool BadgeItemFlag;

	public void UpdateItemUI(GameItem item, bool isPowerful = false)
	{
		if (item == null)
		{
			return;
		}
		if (!item.IsEmpty())
		{
			curItemData = DataManager.GetItemDataByID(item.ItemId);
			curItem = item;
			SetDefaultShow();
			if (NewflagSprite != null)
			{
				NewflagSprite.enabled = curItem.Parm[5] == 0;
			}
			if (TimeLimitSp != null)
			{
				TimeLimitSp.enabled = curItem.Parm[4] > 0 || curItem.ItemData.UseHour > 0;
				if (curItemData.Type == GameDefine.ITEM_TYPE.DANCE_TOOL || curItemData.Type == GameDefine.ITEM_TYPE.WORLDSPEAK || curItemData.Type == GameDefine.ITEM_TYPE.ENEMYWARP_TOOL)
				{
					TimeLimitSp.transform.localPosition = new Vector3(-16f, -15f, 0f);
				}
				else
				{
					TimeLimitSp.transform.localPosition = new Vector3(16f, -15f, 0f);
				}
			}
			if (UpArrowObj != null)
			{
				NGUITools.SetActive(UpArrowObj, isPowerful);
			}
			if (curItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				if (ItemNumLabel != null)
				{
					UnityVersionUtil.SetActiveRecursive(ItemNumLabel.gameObject, state: false);
				}
				if (curItem.GetItemQuality() != EQUIP_QUALITY.INVALID)
				{
					UnityVersionUtil.SetActiveRecursive(ItemQualityIcon.gameObject, state: true);
					ItemQualityIcon.spriteName = curItem.GetItemQuality().ToString();
				}
				else
				{
					ItemQualityIcon.spriteName = curItemData.QualityType.ToString();
				}
				if (item.ContainerType == ITEM_CONTAINER_TYPE.EQUIPPACK)
				{
					if (equipFlagSprite != null)
					{
						equipFlagSprite.spriteName = "CZ_yiZhuangBei_tuBiao";
					}
				}
				else if (equipFlagSprite != null)
				{
					equipFlagSprite.spriteName = string.Empty;
				}
				if (curItem.IsAppraise)
				{
					if (AppraiseObj != null)
					{
						NGUITools.SetActive(AppraiseObj, state: false);
					}
				}
				else if (AppraiseObj != null)
				{
					NGUITools.SetActive(AppraiseObj, state: true);
				}
				if (StarObj != null)
				{
					NGUITools.SetActive(StarObj, state: true);
					int num = 7;
					num = curItem.GetStarByScore();
					for (int i = 0; i < StarList.Count; i++)
					{
						if (i < num)
						{
							StarList[i].enabled = true;
						}
						else
						{
							StarList[i].enabled = false;
						}
					}
				}
				if (AddObj != null)
				{
					NGUITools.SetActive(AddObj, state: true);
					AddLabel.text = $"+{curItem.ItemLevel}";
				}
			}
			else if (curItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
			{
				if (ItemNumLabel != null)
				{
					UnityVersionUtil.SetActiveRecursive(ItemNumLabel.gameObject, state: false);
				}
				ItemQualityIcon.spriteName = curItemData.QualityType.ToString();
				if (item.ContainerType == ITEM_CONTAINER_TYPE.EQUIPPACK)
				{
					if (equipFlagSprite != null)
					{
						equipFlagSprite.spriteName = "CZ_yiZhuangBei_tuBiao";
					}
				}
				else if (equipFlagSprite != null)
				{
					equipFlagSprite.spriteName = string.Empty;
				}
			}
			else if (curItemData.Type == GameDefine.ITEM_TYPE.ENHANCE_ITEM)
			{
				if (ItemNumLabel != null)
				{
					UnityVersionUtil.SetActiveRecursive(ItemNumLabel.gameObject, state: true);
					if (curItem.StackNum > 1)
					{
						ItemNumLabel.text = $"{curItem.StackNum}";
					}
					else
					{
						ItemNumLabel.text = string.Empty;
					}
					ItemNumLabel.color = Color.white;
				}
				ItemQualityIcon.spriteName = curItemData.QualityType.ToString();
				if (equipFlagSprite != null)
				{
					equipFlagSprite.spriteName = string.Empty;
				}
			}
			else if (curItemData.Type == GameDefine.ITEM_TYPE.BADGE)
			{
				if (ItemNumLabel != null)
				{
					UnityVersionUtil.SetActiveRecursive(ItemNumLabel.gameObject, state: true);
					if (curItem.StackNum > 1)
					{
						ItemNumLabel.text = $"{curItem.StackNum}";
					}
					else
					{
						ItemNumLabel.text = string.Empty;
					}
					ItemNumLabel.color = Color.white;
				}
				ItemQualityIcon.spriteName = curItemData.QualityType.ToString();
				if (equipFlagSprite != null)
				{
					equipFlagSprite.spriteName = string.Empty;
				}
			}
			else
			{
				if (ItemNumLabel != null)
				{
					UnityVersionUtil.SetActiveRecursive(ItemNumLabel.gameObject, state: true);
					if (curItem.StackNum > 1)
					{
						ItemNumLabel.text = $"{curItem.StackNum}";
					}
					else
					{
						ItemNumLabel.text = string.Empty;
					}
					ItemNumLabel.color = Color.white;
				}
				ItemQualityIcon.spriteName = curItemData.QualityType.ToString();
				if (equipFlagSprite != null)
				{
					equipFlagSprite.spriteName = string.Empty;
				}
			}
			ItemIcon.spriteName = curItemData.BackPackIcon;
		}
		else
		{
			SetItemEmpty(item.ContainerType, item.EquipType);
		}
	}

	public void SetSellChoose(bool active)
	{
		if (SellChoosePic != null)
		{
			NGUITools.SetActive(SellChoosePic.gameObject, active);
		}
	}

	public void SetItemEmpty(ITEM_CONTAINER_TYPE containerType, EQUIP_BACKPACK_TYPE type = EQUIP_BACKPACK_TYPE.COUNT)
	{
		curItemData = null;
		curItem = null;
		if (ItemNumLabel != null)
		{
			UnityVersionUtil.SetActiveRecursive(ItemNumLabel.gameObject, state: false);
		}
		switch (containerType)
		{
		case ITEM_CONTAINER_TYPE.BADGE_EQUIPPACK:
			ItemIcon.spriteName = GameDefine.EmptyBadgeIconName;
			break;
		case ITEM_CONTAINER_TYPE.EQUIPPACK:
			switch (type)
			{
			case EQUIP_BACKPACK_TYPE.WEAPON:
				ItemIcon.spriteName = "ZhuangBeiCao_wuQi";
				break;
			case EQUIP_BACKPACK_TYPE.HEAD:
				ItemIcon.spriteName = "ZhuangBeiCao_tou";
				break;
			case EQUIP_BACKPACK_TYPE.BODY:
				ItemIcon.spriteName = "ZhuangBeiCao_shangYi";
				break;
			case EQUIP_BACKPACK_TYPE.LEG:
				ItemIcon.spriteName = "ZhuangBeiCao_xiaYi";
				break;
			case EQUIP_BACKPACK_TYPE.BELT:
				ItemIcon.spriteName = "ZhuangBeiCao_yaoDai";
				break;
			case EQUIP_BACKPACK_TYPE.NECKLACE:
				ItemIcon.spriteName = "ZhuangBeiCao_xiangLian";
				break;
			default:
				ItemIcon.spriteName = GameDefine.EmptyEquipIconName;
				break;
			}
			break;
		default:
			ItemIcon.spriteName = GameDefine.EmptyItemIconName;
			break;
		}
		SetDefaultShow();
		ItemQualityIcon.spriteName = EQUIP_QUALITY.KUANG_BLACK.ToString();
	}

	public void SetDefaultShow()
	{
		if (NewflagSprite != null)
		{
			NewflagSprite.enabled = false;
		}
		if (TimeLimitSp != null)
		{
			TimeLimitSp.enabled = false;
		}
		SetSellChoose(active: false);
		if (UpArrowObj != null)
		{
			NGUITools.SetActive(UpArrowObj, state: false);
		}
		if (AppraiseObj != null)
		{
			NGUITools.SetActive(AppraiseObj, state: false);
		}
		if (StarObj != null)
		{
			NGUITools.SetActive(StarObj, state: false);
		}
		if (AddObj != null)
		{
			NGUITools.SetActive(AddObj, state: false);
		}
	}

	public void SetItemLock()
	{
	}

	public void OnClickItem()
	{
		if (curItem == null)
		{
			if (!ItemIcon.spriteName.Equals(GameDefine.EmptyItemIconName) && onClickItem != null)
			{
				onClickItem(curItem, this);
			}
			return;
		}
		if (NewflagSprite != null && curItem.Parm[5] == 0)
		{
			curItem.Parm[5] = 1;
			NewflagSprite.enabled = false;
			change_item_state.request request = new change_item_state.request();
			request.indexId = curItem.IndexId;
			request.type = (long)curItemData.GetContainerType();
			NetLogic.GetInstance().Send<Protocol.change_item_state>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.UpdateEquipsTips();
		}
		if (onClickItem != null)
		{
			onClickItem(curItem, this);
			SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(4);
		}
	}

	public void UpdateNewItemUI(GameItem item)
	{
		if (item == null)
		{
			return;
		}
		if (!item.IsEmpty())
		{
			curItemData = DataManager.GetItemDataByID(item.ItemId);
			curItem = item;
			if (NewflagSprite != null)
			{
				NewflagSprite.enabled = curItem.Parm[5] == 0;
			}
			ItemNumLabel.enabled = false;
			if (curItemData.Type == GameDefine.ITEM_TYPE.EQUIP || curItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
			{
				if (curItem.GetItemQuality() != EQUIP_QUALITY.INVALID)
				{
					UnityVersionUtil.SetActiveRecursive(ItemQualityIcon.gameObject, state: true);
					ItemQualityIcon.spriteName = curItem.GetItemQuality().ToString();
				}
				else
				{
					ItemQualityIcon.spriteName = curItemData.QualityType.ToString();
				}
				if (item.ContainerType == ITEM_CONTAINER_TYPE.EQUIPPACK)
				{
					if (equipFlagSprite != null)
					{
						equipFlagSprite.spriteName = "CZ_yiZhuangBei_tuBiao";
					}
				}
				else if (equipFlagSprite != null)
				{
					equipFlagSprite.spriteName = string.Empty;
				}
			}
			else if (curItemData.Type == GameDefine.ITEM_TYPE.ENHANCE_ITEM)
			{
				ItemQualityIcon.spriteName = curItemData.QualityType.ToString();
				if (equipFlagSprite != null)
				{
					equipFlagSprite.spriteName = string.Empty;
				}
			}
			else if (curItemData.Type == GameDefine.ITEM_TYPE.BADGE)
			{
				ItemQualityIcon.spriteName = curItemData.QualityType.ToString();
				if (equipFlagSprite != null)
				{
					equipFlagSprite.spriteName = string.Empty;
				}
			}
			else
			{
				ItemQualityIcon.spriteName = curItemData.QualityType.ToString();
				if (equipFlagSprite != null)
				{
					equipFlagSprite.spriteName = string.Empty;
				}
			}
			ItemIcon.spriteName = curItemData.BackPackIcon;
		}
		else
		{
			SetItemEmpty(ITEM_CONTAINER_TYPE.ITEM_BACKPACK);
		}
	}
}
