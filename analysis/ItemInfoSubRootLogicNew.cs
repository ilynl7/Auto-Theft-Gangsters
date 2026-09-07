using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ItemInfoSubRootLogicNew : MonoBehaviour
{
	public UISprite ItemIcon;

	public UISprite ItemQualityIcon;

	public UILabel ItemNameLabel;

	public UILabel ItemEnhanceLabel;

	public UILabel LevelLabel;

	public UILabel LevelNameLabel;

	public UILabel ProfessionNameLabel;

	public UILabel ProfessionLabel;

	public UILabel PriceLabel;

	public UILabel ItemLevelLabel;

	public UISprite IsEquipedSprite;

	public GameObject StarObj;

	public List<UISprite> StarList;

	public GameObject AttRoot;

	public GameObject BaseRoot;

	public UIGrid BaseAttributeGrid;

	public List<InfoLineItemLogic> BaseList;

	public UIGrid RandomAttGrid;

	public List<InfoLineItemLogic> RandomList;

	public GameObject InlayObj;

	public UIGrid InlayGrid;

	public List<InfoLineItemLogic> InlayList;

	public GameObject SkillRoot;

	public UIGrid SkillGrid;

	public List<InfoLineItemLogic> SkillList;

	public GameObject[] JumpBtnRoot;

	public UILabel[] JumpBtnLabel;

	public UIGrid JumpBtnRootGrid;

	public GameObject BadgeRoot;

	public UILabel TitleLabel;

	public UILabel DescLabel;

	public UILabel[] LeftAttrLabelList;

	public UILabel[] RightAttrLabelList;

	public UISprite[] LefeAttrIconList;

	public GameObject PowerUpArrow;

	public GameObject PowerDownArrow;

	public void ShowBaseAtt(GameItem mCurItem, bool isEquiped)
	{
		NGUITools.SetActive(BaseRoot, state: true);
		if (BadgeRoot != null)
		{
			NGUITools.SetActive(BadgeRoot, state: false);
		}
		if (TitleLabel != null)
		{
			NGUITools.SetActive(TitleLabel.gameObject, state: false);
		}
		if (ItemLevelLabel != null)
		{
			if (isEquiped && mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				ItemLevelLabel.text = $"+{mCurItem.ItemLevel}";
			}
			else
			{
				ItemLevelLabel.text = string.Empty;
			}
		}
		EquipData equipDataById = DataManager.GetEquipDataById(mCurItem.ItemId);
		int num = equipDataById.GetBaseAttCount() - BaseList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(BaseList[0].gameObject) as GameObject;
				InfoLineItemLogic component = gameObject.GetComponent<InfoLineItemLogic>();
				gameObject.name = $"ItemInfoNameLabel{BaseList.Count:D2}";
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
		bool flag = true;
		if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			flag = false;
		}
		else if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			flag = true;
		}
		int[] array = new int[4] { -1, -1, -1, -1 };
		if (equipDataById.BaseStatusType != 0)
		{
			string attributeIcon = GameDefine.GetAttributeIcon((int)equipDataById.BaseStatusType);
			string attributeName_S = GameDefine.GetAttributeName_S((int)equipDataById.BaseStatusType);
			array[0] = equipDataById.GetAttrValueByQuality(0, (int)mCurItem.GetItemQuality());
			string attributeValueStr = GameDefine.GetAttributeValueStr((int)equipDataById.BaseStatusType, array[0]);
			string enhancestr = string.Empty;
			if (flag)
			{
				enhancestr = string.Format("({0} +{1})", StrDictionary.GetDictionaryString("#{100934}"), equipDataById.GetAttrEnhanceVal(0, (int)mCurItem.GetItemQuality(), mCurItem.ItemLevel));
			}
			BaseList[0].ResetBase(attributeIcon, attributeName_S, attributeValueStr, enhancestr);
		}
		if (equipDataById.Status1 != 0)
		{
			string attributeIcon2 = GameDefine.GetAttributeIcon(equipDataById.Status1);
			string attributeName_S2 = GameDefine.GetAttributeName_S(equipDataById.Status1);
			array[1] = equipDataById.GetAttrValueByQuality(1, (int)mCurItem.GetItemQuality());
			string attributeValueStr2 = GameDefine.GetAttributeValueStr(equipDataById.Status1, array[1]);
			string enhancestr2 = string.Empty;
			if (flag)
			{
				enhancestr2 = string.Format("({0} +{1})", StrDictionary.GetDictionaryString("#{100934}"), equipDataById.GetAttrEnhanceVal(1, (int)mCurItem.GetItemQuality(), mCurItem.ItemLevel));
			}
			BaseList[1].ResetBase(attributeIcon2, attributeName_S2, attributeValueStr2, enhancestr2);
		}
		if (equipDataById.Status2 != 0)
		{
			string attributeIcon3 = GameDefine.GetAttributeIcon(equipDataById.Status2);
			string attributeName_S3 = GameDefine.GetAttributeName_S(equipDataById.Status2);
			array[2] = equipDataById.GetAttrValueByQuality(2, (int)mCurItem.GetItemQuality());
			string attributeValueStr3 = GameDefine.GetAttributeValueStr(equipDataById.Status2, array[2]);
			string enhancestr3 = string.Empty;
			if (flag)
			{
				enhancestr3 = string.Format("({0} +{1})", StrDictionary.GetDictionaryString("#{100934}"), equipDataById.GetAttrEnhanceVal(2, (int)mCurItem.GetItemQuality(), mCurItem.ItemLevel));
			}
			BaseList[2].ResetBase(attributeIcon3, attributeName_S3, attributeValueStr3, enhancestr3);
		}
		if (equipDataById.ExStatus != 0)
		{
			string attributeIcon4 = GameDefine.GetAttributeIcon(equipDataById.ExStatus);
			string attributeName_S4 = GameDefine.GetAttributeName_S(equipDataById.ExStatus);
			array[3] = equipDataById.GetAttrValueByQuality(3, (int)mCurItem.GetItemQuality());
			string attributeValueStr4 = GameDefine.GetAttributeValueStr(equipDataById.ExStatus, array[3]);
			string enhancestr4 = string.Empty;
			if (flag)
			{
				enhancestr4 = string.Format("({0} +{1})", StrDictionary.GetDictionaryString("#{100934}"), equipDataById.GetAttrEnhanceVal(3, (int)mCurItem.GetItemQuality(), mCurItem.ItemLevel));
			}
			BaseList[3].ResetBase(attributeIcon4, attributeName_S4, attributeValueStr4, enhancestr4);
		}
		BaseAttributeGrid.Reposition();
	}

	public void ShowOtherEquipInfo(GameItem mCurItem)
	{
		if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			NGUITools.SetActive(StarObj, state: true);
			ShowStar(mCurItem.GetStarByScore());
			if (mCurItem.ItemData.SubType == 0)
			{
				NGUITools.SetActive(SkillRoot, state: true);
				NGUITools.SetActive(AttRoot, state: false);
				if (mCurItem.IsHaveRandomAtt)
				{
					int num = mCurItem.Random_AttriDic.Count - SkillList.Count;
					if (num > 0)
					{
						for (int i = 0; i < num; i++)
						{
							GameObject gameObject = Object.Instantiate(SkillList[0].gameObject) as GameObject;
							InfoLineItemLogic component = gameObject.GetComponent<InfoLineItemLogic>();
							gameObject.name = $"ItemInfoNameLabel{SkillList.Count:D2}";
							gameObject.transform.parent = SkillGrid.transform;
							gameObject.transform.localScale = Vector3.one;
							gameObject.transform.localPosition = Vector3.zero;
							SkillList.Add(component);
						}
					}
					List<random_attri> list = new List<random_attri>(mCurItem.Random_AttriDic.Values);
					for (int num2 = list.Count - 1; num2 >= 0; num2--)
					{
						if (string.IsNullOrEmpty(list[num2].skillId))
						{
							list.RemoveAt(num2);
						}
					}
					list.Sort((random_attri x, random_attri y) => (int)x.index - (int)y.index);
					for (int j = 0; j < SkillList.Count; j++)
					{
						if (j < list.Count)
						{
							NGUITools.SetActive(SkillList[j].gameObject, state: true);
							SkillList[j].ResetSkill(list[j], mCurItem);
						}
						else
						{
							NGUITools.SetActive(SkillList[j].gameObject, state: false);
						}
					}
					SkillGrid.Reposition();
				}
				else
				{
					for (int k = 0; k < SkillList.Count; k++)
					{
						NGUITools.SetActive(SkillList[k].gameObject, state: false);
					}
				}
				return;
			}
			NGUITools.SetActive(SkillRoot, state: false);
			NGUITools.SetActive(AttRoot, state: true);
			if (mCurItem.IsHaveRandomAtt)
			{
				int num3 = mCurItem.Random_AttriDic.Count - RandomList.Count;
				if (num3 > 0)
				{
					for (int l = 0; l < num3; l++)
					{
						GameObject gameObject2 = Object.Instantiate(RandomList[0].gameObject) as GameObject;
						InfoLineItemLogic component2 = gameObject2.GetComponent<InfoLineItemLogic>();
						gameObject2.name = $"ItemInfoNameLabel{RandomList.Count:D2}";
						gameObject2.transform.parent = RandomAttGrid.transform;
						gameObject2.transform.localScale = Vector3.one;
						gameObject2.transform.localPosition = Vector3.zero;
						RandomList.Add(component2);
					}
				}
				List<random_attri> list2 = new List<random_attri>(mCurItem.Random_AttriDic.Values);
				for (int num4 = list2.Count - 1; num4 >= 0; num4--)
				{
					if (list2[num4].id == 0L)
					{
						list2.RemoveAt(num4);
					}
				}
				list2.Sort((random_attri x, random_attri y) => (int)x.index - (int)y.index);
				for (int m = 0; m < RandomList.Count; m++)
				{
					if (m < list2.Count)
					{
						NGUITools.SetActive(RandomList[m].gameObject, state: true);
						RandomList[m].ResetAtt(list2[m], mCurItem);
					}
					else
					{
						NGUITools.SetActive(RandomList[m].gameObject, state: false);
					}
				}
				RandomAttGrid.Reposition();
			}
			else
			{
				for (int n = 0; n < RandomList.Count; n++)
				{
					NGUITools.SetActive(RandomList[n].gameObject, state: false);
				}
			}
			if (mCurItem.IsHaveInlay)
			{
				NGUITools.SetActive(InlayObj.gameObject, state: true);
				int num5 = mCurItem.InlayDic.Count - InlayList.Count;
				if (num5 > 0)
				{
					for (int num6 = 0; num6 < num5; num6++)
					{
						GameObject gameObject3 = Object.Instantiate(InlayList[0].gameObject) as GameObject;
						InfoLineItemLogic component3 = gameObject3.GetComponent<InfoLineItemLogic>();
						gameObject3.name = $"ItemInfoNameLabel{InlayList.Count:D2}";
						gameObject3.transform.parent = InlayGrid.transform;
						gameObject3.transform.localScale = Vector3.one;
						gameObject3.transform.localPosition = Vector3.zero;
						InlayList.Add(component3);
					}
				}
				List<inlay> list3 = new List<inlay>(mCurItem.InlayDic.Values);
				list3.Sort((inlay x, inlay y) => (int)x.index - (int)y.index);
				for (int num7 = 0; num7 < InlayList.Count; num7++)
				{
					if (num7 < list3.Count)
					{
						NGUITools.SetActive(InlayList[num7].gameObject, state: true);
						InlayList[num7].ResetInlay(list3[num7]);
					}
					else
					{
						NGUITools.SetActive(InlayList[num7].gameObject, state: false);
					}
				}
				InlayGrid.Reposition();
			}
			else
			{
				NGUITools.SetActive(InlayObj.gameObject, state: false);
			}
		}
		else
		{
			NGUITools.SetActive(StarObj, state: false);
			NGUITools.SetActive(SkillRoot, state: false);
			NGUITools.SetActive(AttRoot, state: false);
		}
	}

	public void ShowBadgeInfo(GameItem mCurItem)
	{
		NGUITools.SetActive(BadgeRoot, state: true);
		NGUITools.SetActive(StarObj, state: false);
		NGUITools.SetActive(SkillRoot, state: false);
		NGUITools.SetActive(AttRoot, state: false);
		NGUITools.SetActive(BaseRoot, state: false);
		if (TitleLabel != null)
		{
			NGUITools.SetActive(TitleLabel.gameObject, state: false);
		}
		BadgeData badgeDataById = DataManager.GetBadgeDataById(mCurItem.ItemId);
		for (int i = 0; i < LeftAttrLabelList.Length; i++)
		{
			NGUITools.SetActive(LeftAttrLabelList[i].gameObject, state: false);
		}
		if (badgeDataById.Status1 != -1)
		{
			NGUITools.SetActive(LeftAttrLabelList[0].gameObject, state: true);
			LeftAttrLabelList[0].text = GameDefine.GetAttributeName_S(badgeDataById.Status1);
			RightAttrLabelList[0].text = GameDefine.GetAttributeValueStr(badgeDataById.Status1, badgeDataById.Value1);
			LefeAttrIconList[0].spriteName = GameDefine.GetAttributeIcon(badgeDataById.Status1);
		}
		if (badgeDataById.Status2 != -1)
		{
			NGUITools.SetActive(LeftAttrLabelList[1].gameObject, state: true);
			LeftAttrLabelList[1].text = GameDefine.GetAttributeName_S(badgeDataById.Status2);
			RightAttrLabelList[1].text = GameDefine.GetAttributeValueStr(badgeDataById.Status2, badgeDataById.Value2);
			LefeAttrIconList[1].spriteName = GameDefine.GetAttributeIcon(badgeDataById.Status2);
		}
	}

	public void ShowDesInfo(GameItem mCurItem)
	{
		if (BadgeRoot != null)
		{
			NGUITools.SetActive(BadgeRoot, state: false);
		}
		NGUITools.SetActive(StarObj, state: false);
		NGUITools.SetActive(SkillRoot, state: false);
		NGUITools.SetActive(AttRoot, state: false);
		NGUITools.SetActive(BaseRoot, state: false);
		if (TitleLabel != null)
		{
			NGUITools.SetActive(TitleLabel.gameObject, state: true);
			TitleLabel.text = StrDictionary.GetDictionaryString("#{100611}");
			DescLabel.text = mCurItem.ItemData.MDescription;
		}
	}

	public void ShowStar(int star)
	{
		for (int i = 0; i < StarList.Count; i++)
		{
			if (i < star)
			{
				StarList[i].color = Color.white;
			}
			else
			{
				StarList[i].color = Color.black;
			}
		}
	}

	public void ClearItemLevel()
	{
		if (ItemLevelLabel != null)
		{
			ItemLevelLabel.text = string.Empty;
		}
	}
}
