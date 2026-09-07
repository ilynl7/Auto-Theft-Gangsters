using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RewardItem : MonoBehaviour
{
	public UISprite iconSprite;

	public UISprite qualitySprite;

	public UILabel itemCountLabel;

	public UILabel OtherLabel;

	private ItemData curItemData;

	private GameItem item;

	private int itemLevel;

	private bool IsEquipedTips;

	public GameObject StarObj;

	public List<UISprite> StarList;

	public GameObject AddObj;

	public UILabel AddLabel;

	public UISprite TimeLimitSp;

	private void Awake()
	{
		AddTriger();
	}

	private void AddTriger()
	{
		if (!(base.gameObject.GetComponent<UIEventTrigger>() != null))
		{
			base.gameObject.AddComponent<BoxCollider>();
			GetComponent<UIWidget>().autoResizeBoxCollider = true;
			base.gameObject.AddComponent<UIButtonColor>();
			UIEventTrigger uIEventTrigger = base.gameObject.AddComponent<UIEventTrigger>();
			uIEventTrigger.onClick.Add(new EventDelegate(OnClickShowItems));
		}
	}

	public void UpdateItem(string itemId, int quality, int count, int other = 0)
	{
		UpdateItem(itemId, (EQUIP_QUALITY)quality, count, other);
	}

	public void OnClickShowItems()
	{
		if (curItemData == null)
		{
			return;
		}
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(4);
		if (item.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP || item.ItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			if (IsEquipedTips)
			{
				ItemInfoRootLogicNew.ShowEquipFullTips(item, itemLevel);
			}
			else
			{
				ItemInfoRootLogicNew.ShowEquipTips(item, itemLevel);
			}
		}
		else
		{
			ItemInfoRootLogicNew.ShowItemTips(item, itemLevel);
		}
	}

	public void UpdateItem(string itemId, EQUIP_QUALITY quality, int count, int other = 0)
	{
		IsEquipedTips = false;
		itemLevel = 0;
		ItemData itemData = (curItemData = DataManager.GetItemDataByID(itemId));
		item = new GameItem(itemId, quality, count);
		if (OtherLabel != null)
		{
			OtherLabel.text = string.Empty;
		}
		if (itemData != null)
		{
			if (itemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				qualitySprite.spriteName = quality.ToString();
			}
			else
			{
				qualitySprite.spriteName = itemData.QualityType.ToString();
			}
			iconSprite.spriteName = itemData.BackPackIcon;
			iconSprite.enabled = true;
			if (count > 1)
			{
				itemCountLabel.text = $"x{count}";
			}
			else
			{
				itemCountLabel.text = string.Empty;
			}
			if (OtherLabel != null)
			{
				if (other > 0)
				{
					OtherLabel.text = string.Format("{0}+ {1}", StrDictionary.GetDictionaryString("#{102044}"), other);
				}
				else
				{
					OtherLabel.text = string.Empty;
				}
			}
			if (TimeLimitSp != null)
			{
				TimeLimitSp.enabled = false;
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
		else
		{
			NGUITools.SetActive(base.gameObject, state: false);
		}
	}

	public void UpdateItem(GameItem curItem, int level, bool isEquipTips = false)
	{
		IsEquipedTips = isEquipTips;
		itemLevel = level;
		ItemData itemData = curItem.ItemData;
		curItemData = curItem.ItemData;
		item = curItem;
		if (OtherLabel != null)
		{
			OtherLabel.text = string.Empty;
		}
		if (AddObj != null)
		{
			NGUITools.SetActive(AddObj, state: false);
		}
		if (StarObj != null)
		{
			NGUITools.SetActive(StarObj, state: false);
		}
		if (TimeLimitSp != null)
		{
			TimeLimitSp.enabled = false;
		}
		if (curItemData != null)
		{
			if (curItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				qualitySprite.spriteName = curItem.GetItemQuality().ToString();
				if (AddObj != null)
				{
					NGUITools.SetActive(AddObj, state: true);
					AddLabel.text = $"+{itemLevel}";
				}
				if (StarObj != null)
				{
					NGUITools.SetActive(StarObj, state: true);
					int num = 1;
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
			}
			else if (curItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
			{
				qualitySprite.spriteName = itemData.QualityType.ToString();
				if (TimeLimitSp != null)
				{
					TimeLimitSp.enabled = curItem.Parm[4] > 0 || curItem.ItemData.UseHour > 0;
				}
			}
			else
			{
				qualitySprite.spriteName = itemData.QualityType.ToString();
			}
			iconSprite.spriteName = itemData.BackPackIcon;
			iconSprite.enabled = true;
			if (itemCountLabel != null)
			{
				if (curItem.StackNum > 1)
				{
					itemCountLabel.text = $"x{curItem.StackNum}";
				}
				else
				{
					itemCountLabel.text = string.Empty;
				}
			}
		}
		else
		{
			NGUITools.SetActive(base.gameObject, state: false);
		}
	}

	public void SetItemEmpty()
	{
		itemLevel = 0;
		curItemData = null;
		item = null;
		IsEquipedTips = false;
		if (itemCountLabel != null)
		{
			itemCountLabel.text = string.Empty;
		}
		iconSprite.spriteName = GameDefine.EmptyItemIconName;
		qualitySprite.spriteName = EQUIP_QUALITY.KUANG_BLACK.ToString();
		if (TimeLimitSp != null)
		{
			TimeLimitSp.enabled = false;
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
}
