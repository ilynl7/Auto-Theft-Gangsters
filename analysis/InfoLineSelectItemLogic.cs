using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class InfoLineSelectItemLogic : MonoBehaviour
{
	public UISprite Icon;

	public UILabel NameLavel;

	public UILabel ValueLabel;

	public UISprite SelectFlag;

	public GameObject Infoobj;

	public UILabel Emptylabel;

	public UILabel DesLabel;

	public UISprite ColorSp;

	private int CurIndex;

	private int CurQuality;

	private DelegateDefine.ThreeParamDelegate ClickFun;

	public List<UISprite> SkillLabelPic;

	public UILabel LevelLabel;

	private string CurKey = string.Empty;

	public void ResetAtt(random_attri attinfo, int index, GameItem curitem, DelegateDefine.ThreeParamDelegate clickfun = null)
	{
		CurIndex = index;
		ClickFun = clickfun;
		SelectFlag.enabled = false;
		if (attinfo != null)
		{
			string attributeIcon = GameDefine.GetAttributeIcon((int)attinfo.id);
			string attributeName_S = GameDefine.GetAttributeName_S((int)attinfo.id);
			string attributeValueStr = GameDefine.GetAttributeValueStr((int)attinfo.id, (int)attinfo.value);
			if (!attinfo.HasQualityId)
			{
				EquipData equipDataById = DataManager.GetEquipDataById(curitem.ItemId);
				attinfo.qualityId = equipDataById.QualityID;
			}
			CurQuality = curitem.GetEquipAttQuality((int)attinfo.quality, attinfo.qualityId);
			Color colorByQuality = GameDefine.GetColorByQuality(CurQuality);
			CurKey = attinfo.id.ToString();
			ResetAtt(attributeIcon, attributeName_S, attributeValueStr, colorByQuality);
		}
		else
		{
			ResetEmpty();
		}
	}

	public void ResetSkill(random_attri attinfo, int index, GameItem curitem, DelegateDefine.ThreeParamDelegate clickfun = null)
	{
		CurIndex = index;
		ClickFun = clickfun;
		SelectFlag.enabled = false;
		if (attinfo != null)
		{
			if (!attinfo.HasQualityId)
			{
				EquipData equipDataById = DataManager.GetEquipDataById(curitem.ItemId);
				attinfo.qualityId = equipDataById.QualityID;
			}
			CurQuality = curitem.GetEquipAttQuality((int)attinfo.quality, attinfo.qualityId);
			SkillData skillDataById = DataManager.GetSkillDataById(attinfo.skillId);
			CurKey = skillDataById.TeamID.ToString();
			Color colorByQuality = GameDefine.GetColorByQuality(CurQuality);
			int playerSkillLevelByPos = Singleton<ObjManager>.Instance.MainPlayer.GetPlayerSkillLevelByPos((int)attinfo.index + 4);
			ResetSkill(skillDataById, colorByQuality, playerSkillLevelByPos);
		}
		else
		{
			ResetEmpty();
		}
	}

	public void ResetSkill(SkillData curdata, Color needcolor, int level)
	{
		if (curdata == null)
		{
			return;
		}
		NGUITools.SetActive(Infoobj, state: true);
		Emptylabel.enabled = false;
		Icon.spriteName = curdata.Icon;
		NameLavel.text = $"{curdata.CDSecond}'s";
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
		ColorSp.spriteName = "CZ_effect_liuGuang";
		ColorSp.color = needcolor;
	}

	public void ResetAtt(string iconname, string name, string value, Color showcolor)
	{
		NGUITools.SetActive(Infoobj, state: true);
		Emptylabel.enabled = false;
		Icon.spriteName = iconname;
		NameLavel.text = name;
		ValueLabel.text = value;
		NameLavel.color = showcolor;
		ValueLabel.color = showcolor;
	}

	public void ResetEmpty()
	{
		NGUITools.SetActive(Infoobj, state: false);
		Emptylabel.enabled = true;
		CurKey = string.Empty;
		if (ColorSp != null)
		{
			ColorSp.spriteName = "CZ_shengJi_ShuXingTiao";
			ColorSp.color = Color.white;
		}
	}

	public void UpdateSelect(int selectindex)
	{
		if (selectindex == CurIndex)
		{
			SelectFlag.enabled = true;
		}
		else
		{
			SelectFlag.enabled = false;
		}
	}

	public void OnClickSelect()
	{
		if (ClickFun != null)
		{
			ClickFun(CurIndex, CurQuality, CurKey, val4: true);
		}
	}

	public bool AutoSelectKey(string needkey)
	{
		if (string.IsNullOrEmpty(needkey))
		{
			return true;
		}
		if (string.IsNullOrEmpty(CurKey))
		{
			return false;
		}
		if (CurKey.Equals(needkey))
		{
			if (ClickFun != null)
			{
				ClickFun(CurIndex, CurQuality, needkey, val4: false);
			}
			return true;
		}
		return false;
	}

	public bool IsEqualsKey(string needkey)
	{
		if (!string.IsNullOrEmpty(needkey) && !string.IsNullOrEmpty(CurKey) && CurKey.Equals(needkey))
		{
			return true;
		}
		return false;
	}

	public bool SelectEmpty()
	{
		if (string.IsNullOrEmpty(CurKey))
		{
			if (ClickFun != null)
			{
				ClickFun(CurIndex, CurQuality, CurKey, val4: false);
			}
			return true;
		}
		return false;
	}
}
