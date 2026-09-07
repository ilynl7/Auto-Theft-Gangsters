using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildSkillItem : MonoBehaviour
{
	public UISprite iconSprite;

	public UISprite bkSprite;

	public UILabel infoLabel;

	private guild_skill curSkill;

	public void Init(guild_skill skill)
	{
		List<GuildSkillData> guildSkillDataListByType = DataManager.GetGuildSkillDataListByType((int)skill.skillType);
		int num = (int)skill.level - 1;
		if (num < 0)
		{
			num = 0;
		}
		GuildSkillData guildSkillData = guildSkillDataListByType[num];
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		int guilLevel = playerData.PlayerGuild.GuilLevel;
		if (guilLevel >= guildSkillData.LevelLimit)
		{
			infoLabel.text = $"{skill.level}/{guildSkillDataListByType.Count}";
			iconSprite.spriteName = guildSkillData.icon;
			bkSprite.alpha = 1f;
			iconSprite.alpha = 1f;
		}
		else
		{
			infoLabel.text = $"{skill.level}/{guildSkillDataListByType.Count}";
			bkSprite.alpha = 0.5f;
			iconSprite.alpha = 0.5f;
		}
		iconSprite.MakePixelPerfect();
		curSkill = skill;
	}

	public void OnClickItem()
	{
		SingletonUnity<GuildSkillRootLogic>.Instance.OnClickItem(curSkill, base.gameObject);
	}
}
