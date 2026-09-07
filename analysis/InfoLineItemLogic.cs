using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class InfoLineItemLogic : MonoBehaviour
{
	public UISprite Icon;

	public UILabel NameLavel;

	public UILabel ValueLabel;

	public UILabel EnhanceLabel;

	public GameObject UpArrowObj;

	public GameObject DownArrowObj;

	public UILabel CDlabel;

	public UILabel DesLabel;

	public UILabel LevelLabel;

	public UISprite ColorSp;

	public List<UISprite> SkillLabelPic;

	public void ResetBase(string iconname, string name, string value, string enhancestr)
	{
		Icon.spriteName = iconname;
		NameLavel.text = name;
		ValueLabel.text = value;
		if (EnhanceLabel != null)
		{
			EnhanceLabel.text = enhancestr;
		}
	}

	public void ResetAtt(random_attri randomatt, GameItem curitem)
	{
		string attributeIcon = GameDefine.GetAttributeIcon((int)randomatt.id);
		string attributeName_S = GameDefine.GetAttributeName_S((int)randomatt.id);
		string attributeValueStr = GameDefine.GetAttributeValueStr((int)randomatt.id, (int)randomatt.value);
		if (!randomatt.HasQualityId)
		{
			EquipData equipDataById = DataManager.GetEquipDataById(curitem.ItemId);
			randomatt.qualityId = equipDataById.QualityID;
		}
		Color colorByQuality = GameDefine.GetColorByQuality(curitem.GetEquipAttQuality((int)randomatt.quality, randomatt.qualityId));
		ResetAtt(attributeIcon, attributeName_S, attributeValueStr, colorByQuality);
	}

	public void ResetSkill(random_attri randomatt, GameItem curitem)
	{
		SkillData skillDataById = DataManager.GetSkillDataById(randomatt.skillId);
		if (!randomatt.HasQualityId)
		{
			EquipData equipDataById = DataManager.GetEquipDataById(curitem.ItemId);
			randomatt.qualityId = equipDataById.QualityID;
		}
		Color colorByQuality = GameDefine.GetColorByQuality(curitem.GetEquipAttQuality((int)randomatt.quality, randomatt.qualityId));
		int num = 0;
		num = ((!SingletonUnity<OtherPlayerInfoUILogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<OtherPlayerInfoUILogic>.Instance.gameObject)) ? Singleton<ObjManager>.Instance.MainPlayer.GetPlayerSkillLevelByPos((int)randomatt.index + 4) : SingletonUnity<OtherPlayerInfoUILogic>.Instance.GetPlayerSkillLevelByPos((int)randomatt.index + 4));
		ResetSkill(skillDataById, colorByQuality, num);
	}

	public void ResetInlay(inlay inlayinfo)
	{
		ItemData itemDataByID = DataManager.GetItemDataByID(inlayinfo.itemId);
	}

	public void ResetAtt(string iconname, string name, string value, Color showcolor, bool showArrow = false, bool isup = false)
	{
		Icon.spriteName = iconname;
		NameLavel.text = name;
		ValueLabel.text = value;
		NameLavel.color = showcolor;
		ValueLabel.color = showcolor;
		if (showArrow)
		{
			if (UpArrowObj != null && DownArrowObj != null)
			{
				if (isup)
				{
					NGUITools.SetActive(UpArrowObj, state: true);
					NGUITools.SetActive(DownArrowObj, state: false);
				}
				else
				{
					NGUITools.SetActive(UpArrowObj, state: false);
					NGUITools.SetActive(DownArrowObj, state: true);
				}
			}
		}
		else if (UpArrowObj != null && DownArrowObj != null)
		{
			NGUITools.SetActive(UpArrowObj, state: false);
			NGUITools.SetActive(DownArrowObj, state: false);
		}
	}

	public void ResetSkill(SkillData curdata, Color needcolor, int level)
	{
		if (curdata == null)
		{
			return;
		}
		Icon.spriteName = curdata.Icon;
		CDlabel.text = $"{curdata.CDSecond}'s";
		DesLabel.text = $"{curdata.TraceDistanceMeter}";
		ValueLabel.text = $"{curdata.MaxAttackCount}";
		LevelLabel.text = $"Lv.{level + 1}";
		for (int i = 0; i < SkillLabelPic.Count; i++)
		{
			if (i < curdata.LabelIdList.Count)
			{
				SkillLabelData skillLabelDataByID = DataManager.GetSkillLabelDataByID(curdata.LabelIdList[i]);
				SkillLabelPic[i].color = skillLabelDataByID.LabelColor;
			}
			else
			{
				SkillLabelPic[i].color = Color.white;
			}
		}
		ColorSp.color = needcolor;
	}
}
