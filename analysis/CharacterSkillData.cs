using System;

[Serializable]
public class CharacterSkillData
{
	private XorFloat cdTimeCount = new XorFloat();

	public string ID = string.Empty;

	public int Level;

	public int Index;

	public int Index2;

	public int UnlockLevel;

	public bool IsDisable;

	public float CDTimeCount
	{
		get
		{
			return cdTimeCount.value;
		}
		set
		{
			cdTimeCount.value = value;
		}
	}

	public CharacterSkillData(string skillID, int level, int pos, int pos2, bool isDisable)
	{
		ID = skillID;
		Level = level;
		Index = pos;
		Index2 = pos2;
		CDTimeCount = 0f;
		IsDisable = isDisable;
		if (!isDisable && !string.IsNullOrEmpty(skillID))
		{
			SkillData skillDataById = DataManager.GetSkillDataById(skillID);
			if (skillDataById != null)
			{
				UnlockLevel = skillDataById.Locklevel;
			}
		}
	}

	public CharacterSkillData(string skillID)
	{
		ID = skillID;
		CDTimeCount = 0f;
	}
}
