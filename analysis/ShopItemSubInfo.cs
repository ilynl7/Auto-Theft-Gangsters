using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ShopItemSubInfo : MonoBehaviour
{
	public UISprite IconSprite;

	public UISprite QualitySprite;

	public GameObject TimeLimitObj;

	public UILabel TimeLimitLabel;

	public UILabel RemainLabel;

	public UILabel ItemNameLabel;

	public UILabel PowerLabel;

	public GameObject ItemDesObj;

	public UILabel ItemDesLabel;

	public GameObject BaseRoot;

	public UIGrid BaseAttributeGrid;

	public List<InfoLineItemLogic> BaseList;

	public GameObject RandomRoot;

	public UILabel RandomLabel;

	public void UpdateSelectItem(shop_item curSelectItem)
	{
		ItemData itemDataByID = DataManager.GetItemDataByID(curSelectItem.ItemID);
		int num = (int)curSelectItem.Quality;
		if (itemDataByID == null)
		{
			return;
		}
		IconSprite.spriteName = itemDataByID.BackPackIcon;
		ItemNameLabel.text = itemDataByID.MName;
		if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			QualitySprite.spriteName = ((EQUIP_QUALITY)curSelectItem.Quality).ToString();
			ItemNameLabel.color = GameDefine.GetColorByQuality((EQUIP_QUALITY)curSelectItem.Quality);
		}
		else
		{
			QualitySprite.spriteName = itemDataByID.QualityType.ToString();
			ItemNameLabel.color = GameDefine.GetColorByQuality(itemDataByID.QualityType);
		}
		if (itemDataByID.UseHour > 0)
		{
			NGUITools.SetActive(TimeLimitObj, state: true);
			TimeLimitLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100643}"), TimeTools.GetFormateTime(itemDataByID.UseHour * 3600));
		}
		else
		{
			NGUITools.SetActive(TimeLimitObj, state: false);
			TimeLimitLabel.text = string.Empty;
		}
		if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP || itemDataByID.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			PowerLabel.enabled = true;
			UnityVersionUtil.SetActiveRecursive(BaseRoot, state: true);
			UnityVersionUtil.SetActiveRecursive(ItemDesObj, state: false);
			UnityVersionUtil.SetActiveRecursive(RandomRoot, state: true);
			RandomLabel.text = itemDataByID.MDescription;
			string text = string.Format("[FFFF00] ({0})[-]", StrDictionary.GetDictionaryString("#{101213}"));
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				text = string.Format("[FFFF00] ({0})[-]", StrDictionary.GetDictionaryString("#{101213}"));
				num = (int)curSelectItem.Quality;
				GameItem gameItem = new GameItem(itemDataByID.ID, (EQUIP_QUALITY)num, 1);
				gameItem.ItemLevel = 80;
				PowerLabel.text = string.Format("[FFFF00]{0} +{1}[-]\nlv.80[FFFF00] ({2})[-]", StrDictionary.GetDictionaryString("#{100421}"), gameItem.GetItemCombatVal(), StrDictionary.GetDictionaryString("#{101213}"));
			}
			else if (itemDataByID.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
			{
				text = string.Empty;
				num = itemDataByID.Quality;
				GameItem gameItem2 = new GameItem(itemDataByID.ID, (EQUIP_QUALITY)num, 1);
				PowerLabel.text = string.Format("[FFFF00]{0} +{1}[-]", StrDictionary.GetDictionaryString("#{100421}"), gameItem2.GetItemCombatVal());
			}
			EquipData equipDataById = DataManager.GetEquipDataById(itemDataByID.ID);
			int num2 = equipDataById.GetBaseAttCount() - BaseList.Count;
			if (num2 > 0)
			{
				for (int i = 0; i < num2; i++)
				{
					GameObject gameObject = Object.Instantiate(BaseList[0].gameObject) as GameObject;
					InfoLineItemLogic component = gameObject.GetComponent<InfoLineItemLogic>();
					gameObject.name = $"{BaseList.Count:D2}";
					gameObject.transform.parent = BaseAttributeGrid.transform;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					BaseList.Add(component);
				}
			}
			for (int j = 0; j < BaseList.Count; j++)
			{
				if (j < equipDataById.GetBaseAttCount())
				{
					UnityVersionUtil.SetActiveRecursive(BaseList[j].gameObject, state: true);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(BaseList[j].gameObject, state: false);
				}
			}
			int[] array = new int[4] { -1, -1, -1, -1 };
			if (equipDataById.BaseStatusType != 0)
			{
				string attributeIcon = GameDefine.GetAttributeIcon((int)equipDataById.BaseStatusType);
				string text2 = GameDefine.GetAttributeName_S((int)equipDataById.BaseStatusType) + text;
				if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					array[0] = equipDataById.GetAttrValByQualityAndLevel(0, num, 80);
				}
				else
				{
					array[0] = equipDataById.GetAttrValByQualityAndLevel(0, num, 0);
				}
				string attributeValueStr = GameDefine.GetAttributeValueStr((int)equipDataById.BaseStatusType, array[0]);
				string empty = string.Empty;
				BaseList[0].ResetBase(attributeIcon, text2, attributeValueStr, empty);
			}
			if (equipDataById.Status1 != 0)
			{
				string attributeIcon2 = GameDefine.GetAttributeIcon(equipDataById.Status1);
				string text3 = GameDefine.GetAttributeName_S(equipDataById.Status1) + text;
				if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					array[1] = equipDataById.GetAttrValByQualityAndLevel(1, num, 80);
				}
				else
				{
					array[1] = equipDataById.GetAttrValByQualityAndLevel(1, num, 0);
				}
				string attributeValueStr2 = GameDefine.GetAttributeValueStr(equipDataById.Status1, array[1]);
				string empty2 = string.Empty;
				BaseList[1].ResetBase(attributeIcon2, text3, attributeValueStr2, empty2);
			}
			if (equipDataById.Status2 != 0)
			{
				string attributeIcon3 = GameDefine.GetAttributeIcon(equipDataById.Status2);
				string text4 = GameDefine.GetAttributeName_S(equipDataById.Status2) + text;
				if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					array[2] = equipDataById.GetAttrValByQualityAndLevel(2, num, 80);
				}
				else
				{
					array[2] = equipDataById.GetAttrValByQualityAndLevel(2, num, 0);
				}
				string attributeValueStr3 = GameDefine.GetAttributeValueStr(equipDataById.Status2, array[2]);
				string empty3 = string.Empty;
				BaseList[2].ResetBase(attributeIcon3, text4, attributeValueStr3, empty3);
			}
			if (equipDataById.ExStatus != 0)
			{
				string attributeIcon4 = GameDefine.GetAttributeIcon(equipDataById.ExStatus);
				string text5 = GameDefine.GetAttributeName_S(equipDataById.ExStatus) + text;
				if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					array[3] = equipDataById.GetAttrValByQualityAndLevel(3, num, 80);
				}
				else
				{
					array[3] = equipDataById.GetAttrValByQualityAndLevel(3, num, 0);
				}
				string attributeValueStr4 = GameDefine.GetAttributeValueStr(equipDataById.ExStatus, array[3]);
				string empty4 = string.Empty;
				BaseList[3].ResetBase(attributeIcon4, text5, attributeValueStr4, empty4);
			}
			BaseAttributeGrid.Reposition();
		}
		else if (itemDataByID.Type == GameDefine.ITEM_TYPE.BADGE)
		{
			PowerLabel.enabled = false;
			UnityVersionUtil.SetActiveRecursive(BaseRoot, state: true);
			UnityVersionUtil.SetActiveRecursive(ItemDesObj, state: false);
			UnityVersionUtil.SetActiveRecursive(RandomRoot, state: false);
			BadgeData badgeDataById = DataManager.GetBadgeDataById(itemDataByID.ID);
			int num3 = badgeDataById.GetBaseAttCount() - BaseList.Count;
			if (num3 > 0)
			{
				for (int k = 0; k < num3; k++)
				{
					GameObject gameObject2 = Object.Instantiate(BaseList[0].gameObject) as GameObject;
					InfoLineItemLogic component2 = gameObject2.GetComponent<InfoLineItemLogic>();
					gameObject2.name = $"{BaseList.Count:D2}";
					gameObject2.transform.parent = BaseAttributeGrid.transform;
					gameObject2.transform.localScale = Vector3.one;
					gameObject2.transform.localPosition = Vector3.zero;
					BaseList.Add(component2);
				}
			}
			for (int l = 0; l < BaseList.Count; l++)
			{
				if (l < badgeDataById.GetBaseAttCount())
				{
					UnityVersionUtil.SetActiveRecursive(BaseList[l].gameObject, state: true);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(BaseList[l].gameObject, state: false);
				}
			}
			if (badgeDataById.Status1 != -1)
			{
				string attributeName_S = GameDefine.GetAttributeName_S(badgeDataById.Status1);
				string attributeValueStr5 = GameDefine.GetAttributeValueStr(badgeDataById.Status1, badgeDataById.Value1);
				string attributeIcon5 = GameDefine.GetAttributeIcon(badgeDataById.Status1);
				BaseList[0].ResetBase(attributeIcon5, attributeName_S, attributeValueStr5, string.Empty);
			}
			if (badgeDataById.Status2 != -1)
			{
				string attributeName_S2 = GameDefine.GetAttributeName_S(badgeDataById.Status2);
				string attributeValueStr6 = GameDefine.GetAttributeValueStr(badgeDataById.Status2, badgeDataById.Value2);
				string attributeIcon6 = GameDefine.GetAttributeIcon(badgeDataById.Status2);
				BaseList[0].ResetBase(attributeIcon6, attributeName_S2, attributeValueStr6, string.Empty);
			}
		}
		else if (itemDataByID.Type == GameDefine.ITEM_TYPE.EXCHANGE)
		{
			UnityVersionUtil.SetActiveRecursive(BaseRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(ItemDesObj, state: true);
			UnityVersionUtil.SetActiveRecursive(RandomRoot, state: false);
			ItemDesLabel.text = itemDataByID.MDescription;
			PowerLabel.enabled = false;
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(BaseRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(ItemDesObj, state: true);
			UnityVersionUtil.SetActiveRecursive(RandomRoot, state: false);
			ItemDesLabel.text = itemDataByID.MDescription;
			PowerLabel.enabled = false;
		}
	}
}
