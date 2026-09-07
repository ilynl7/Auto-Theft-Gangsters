using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ObjInitPlayerData : ObjInitData
{
	public PROFESSION_TYPE Profession;

	public attribute Attribute;

	public attribute AttributeAll;

	public characterVisual visual;

	public int HP;

	public long EXP;

	public int Level;

	public float Speed = 5f;

	public float WalkSpeed = 1f;

	public int Rec;

	public int TitleLevel;

	public int TitleExp;

	public int ComboValue;

	public Dictionary<string, skill_info> skills;

	public int SkillIndex;

	public int BackPackSize;

	public int StorageSize;

	public int RefineNeckLevel;

	public int RefineRing1Level;

	public int RefineRing2Level;

	public int RefineBeltLevel;

	public int RefineLevel;

	public int[] EnhanceLevelList = new int[6];

	public GameDefine.CAMP_TYPE Camp;

	public int PkMode;

	public new long GuildId = -1L;

	public long TeamId = -1L;

	public int DanceState;

	public string DanceId = string.Empty;

	public string AIID = string.Empty;

	public string NpcId = string.Empty;

	public bool IsVisible;

	public bool IsShowFashion => visual.showType == 1;

	public string HeadId
	{
		get
		{
			if (visual.showType == 0L)
			{
				return visual.HeadId;
			}
			return (!visual.HasFashion_HeadId) ? visual.HeadId : visual.Fashion_HeadId;
		}
	}

	public string LegId
	{
		get
		{
			if (visual.showType == 0L)
			{
				return visual.LegId;
			}
			return (!visual.HasFashion_LegId) ? visual.LegId : visual.Fashion_LegId;
		}
	}

	public string BodyId
	{
		get
		{
			if (visual.showType == 0L)
			{
				return visual.BodyId;
			}
			return (!visual.HasFashion_BodyId) ? visual.BodyId : visual.Fashion_BodyId;
		}
	}

	public string WeaponItemId => (!visual.HasWeaponItemId) ? null : visual.WeaponItemId;

	public string FashionItemId => (!visual.HasFashionItemId) ? null : visual.FashionItemId;

	public void SetVisible(bool isVisible)
	{
		IsVisible = isVisible;
	}

	public string GetWeaponID()
	{
		if (visual.showType == 0L)
		{
			return visual.WeaponId;
		}
		if (CheckWeaponIsSame())
		{
			return (!visual.HasFashion_WeaponId) ? visual.WeaponId : visual.Fashion_WeaponId;
		}
		return visual.WeaponId;
	}

	public bool CheckWeaponIsSame()
	{
		if (!string.IsNullOrEmpty(WeaponItemId) && !string.IsNullOrEmpty(FashionItemId))
		{
			EquipData equipDataById = DataManager.GetEquipDataById(WeaponItemId);
			EquipData equipDataById2 = DataManager.GetEquipDataById(FashionItemId);
			return equipDataById.WeaponType == equipDataById2.WeaponType;
		}
		if (visual.HasWeaponId && visual.HasFashion_WeaponId)
		{
			if (GameDefine.GetWeaponName(visual.WeaponId).Equals(GameDefine.GetWeaponName(visual.Fashion_WeaponId)))
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public void InitData(character_aoi character)
	{
		HP = (int)character.attribute_other.hp;
		EXP = character.attribute_other.exp;
		Level = (int)character.attribute_other.level;
		Attribute = character.runtime.attribute;
		AttributeAll = character.runtime.attribute_all;
		Speed = (float)AttributeAll.mov / 100f;
		ComboValue = (int)character.attribute_other.combValue;
		Profession = (PROFESSION_TYPE)character.general.profession;
		mPos = new Vector3((float)character.movement.pos.x / 100f, (float)character.movement.pos.y / 100f, (float)character.movement.pos.z / 100f);
		mDir = MathUtil.HeadingToVector3((float)character.movement.pos.o / 100f);
		mServerID = character.id;
		Name = character.visual.name;
		if (character.attribute_other.HasGuildId)
		{
			GuildName = character.attribute_other.guildName;
			GuildId = character.attribute_other.guildId;
		}
		mCharacterModelId = character.visual.ModeId;
		visual = character.visual;
		Camp = (GameDefine.CAMP_TYPE)character.attribute_other.camp;
		PkMode = (int)character.attribute_other.pkMode;
		GuildId = character.attribute_other.guildId;
		DanceState = (int)character.attribute_other.dance_state;
		DanceId = character.attribute_other.dance_id;
	}

	public void InitData(character character)
	{
		mPos = new Vector3((float)character.movement.pos.x / 100f, (float)character.movement.pos.y / 100f, (float)character.movement.pos.z / 100f);
		mDir = MathUtil.HeadingToVector3((float)character.movement.pos.o / 100f);
		mServerID = character.id;
		Profession = (PROFESSION_TYPE)character.general.profession;
		HP = (int)character.attribute_other.hp;
		EXP = character.attribute_other.exp;
		Level = (int)character.attribute_other.level;
		TitleLevel = (int)character.attribute_other.title_level;
		TitleExp = (int)character.attribute_other.title_exp;
		Attribute = character.runtime.attribute;
		AttributeAll = character.runtime.attribute_all;
		Speed = (float)AttributeAll.mov / 100f;
		TitleExp = (int)character.attribute_other.title_exp;
		TitleLevel = (int)character.attribute_other.title_level;
		ComboValue = (int)character.attribute_other.combValue;
		skills = character.skills;
		SkillIndex = (int)character.skill_index;
		Name = character.general.name;
		if (character.attribute_other.HasGuildId)
		{
			GuildId = character.attribute_other.guildId;
			GuildName = character.attribute_other.guildName;
		}
		mCharacterModelId = character.visual.ModeId;
		visual = character.visual;
		RefineLevel = (int)character.attribute_other.refineLevel;
		RefineNeckLevel = (int)character.attribute_other.refineNeckLevel;
		RefineRing1Level = (int)character.attribute_other.refineRing1Level;
		RefineRing2Level = (int)character.attribute_other.refineRing2Level;
		RefineBeltLevel = (int)character.attribute_other.refineBeltLevel;
		Camp = (GameDefine.CAMP_TYPE)character.attribute_other.camp;
		PkMode = (int)character.attribute_other.pkMode;
		GuildId = character.attribute_other.guildId;
		DanceState = (int)character.attribute_other.dance_state;
		DanceId = character.attribute_other.dance_id;
		if (!character.HasEquip_enhance)
		{
			return;
		}
		for (int i = 0; i < EnhanceLevelList.Length; i++)
		{
			if (character.equip_enhance.ContainsKey(i))
			{
				EnhanceLevelList[i] = (int)character.equip_enhance[i].level;
			}
			else
			{
				EnhanceLevelList[i] = 0;
			}
		}
	}

	public void InitData(character_aoi_attribute character)
	{
		if (character.HasAttribute_other)
		{
			int hP = (int)character.attribute_other.hp;
			HP = hP;
			EXP = character.attribute_other.exp;
			Level = (int)character.attribute_other.level;
			TitleLevel = (int)character.attribute_other.title_level;
			TitleExp = (int)character.attribute_other.title_exp;
			ComboValue = (int)character.attribute_other.combValue;
			Camp = (GameDefine.CAMP_TYPE)character.attribute_other.camp;
			PkMode = (int)character.attribute_other.pkMode;
			GuildId = character.attribute_other.guildId;
			DanceState = (int)character.attribute_other.dance_state;
			DanceId = character.attribute_other.dance_id;
		}
		if (character.HasAttribute_all)
		{
			AttributeAll = character.attribute_all;
			Speed = (float)character.attribute_all.mov / 100f;
		}
		if (character.HasAttribute)
		{
			Attribute = character.attribute;
		}
		if (character.HasVisual)
		{
			visual = character.visual;
		}
	}
}
