using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildSkillRootLogic : SingletonUnity<GuildSkillRootLogic>
{
	public UILabel contributeLabel;

	public UILabel guildLevelLabel;

	public UISprite selectSprite;

	public List<GuildSkillItem> skillItems = new List<GuildSkillItem>();

	public UIGrid skllGrid;

	private Dictionary<long, guild_skill> mSkills;

	private GameObject curSelectObj;

	private guild_skill curSkill;

	public GameObject SkillInfo;

	public UILabel SkillNameLabel;

	public UILabel SKillLevelLabel;

	public UILabel SkillLevelLabel2;

	public UILabel SKillAttributeValue;

	public UISprite SkillAttributeIcon;

	public UISprite SkillInfoIconSprite;

	public UILabel NextSKillAttributeValue;

	public UISprite NextSKillAttributeIcon;

	public UILabel CostContributeLabel;

	public UILabel CostMoneyLabel;

	public GameObject upgradeBtn;

	public UISprite UpgradeBtnSprite;

	private GuildSkillData curSelectSkillData;

	private GuildSkillData nextSkillData;

	private void OnEnable()
	{
		curSelectObj = null;
	}

	public void UpdateGuildSKill()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		contributeLabel.text = playerData.GuildContribute.ToString();
		guildLevelLabel.text = $"Lv.{playerData.PlayerGuild.GuilLevel.ToString()}";
		List<guild_skill> list = new List<guild_skill>((mSkills = playerData.PlayerGuild.GuildSkills).Values);
		list.Sort((guild_skill a, guild_skill b) => (int)a.skillType - (int)b.skillType);
		int num = list.Count - skillItems.Count;
		int count = skillItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(skillItems[0].gameObject) as GameObject;
				gameObject.name = $"jiNeng_{count + i:D2}";
				skllGrid.AddChild(gameObject.transform);
				gameObject.transform.localScale = Vector3.one;
				skillItems.Add(gameObject.GetComponent<GuildSkillItem>());
			}
		}
		for (int j = 0; j < skillItems.Count; j++)
		{
			NGUITools.SetActive(skillItems[j].gameObject, j < list.Count);
		}
		skllGrid.Reposition();
		int index = 0;
		for (int k = 0; k < list.Count; k++)
		{
			if (curSelectObj == null)
			{
				curSkill = list[k];
				curSelectObj = skillItems[index].gameObject;
			}
			skillItems[index++].Init(list[k]);
		}
		UpdateSelect(curSkill, curSelectObj);
	}

	public void UpdateSelect(guild_skill skillType, GameObject obj)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		selectSprite.transform.position = obj.transform.position;
		if (skillType.level > 0)
		{
			curSelectSkillData = DataManager.GetGuildSkillDataByTypeLevel((int)skillType.skillType, (int)skillType.level - 1);
		}
		else
		{
			curSelectSkillData = null;
		}
		GuildSkillData guildSkillData = (nextSkillData = DataManager.GetGuildSkillDataByTypeLevel((int)skillType.skillType, (int)skillType.level));
		List<GuildSkillData> guildSkillDataListByType = DataManager.GetGuildSkillDataListByType((int)skillType.skillType);
		if (playerData.PlayerGuild.GuilLevel >= guildSkillDataListByType[0].LevelLimit)
		{
			if (guildSkillData != null)
			{
				CostContributeLabel.text = GameMoneyHelper.GetMoneyValStr(guildSkillData.SkillCost, GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE);
			}
			SkillInfoIconSprite.spriteName = guildSkillDataListByType[0].icon;
			SkillInfoIconSprite.MakePixelPerfect();
			SkillNameLabel.text = GameDefine.GetBoldStr(StrDictionary.GetDictionaryString(guildSkillDataListByType[0].name));
			SKillLevelLabel.text = skillType.level.ToString();
			SkillLevelLabel2.text = $"{skillType.level}/{guildSkillDataListByType.Count}";
			if (curSelectSkillData != null)
			{
				SkillAttributeIcon.spriteName = GameDefine.GetAttributeIcon(curSelectSkillData.Parm1);
				SKillAttributeValue.text = GameDefine.GetAttributeValueStr(curSelectSkillData.Parm1, curSelectSkillData.Parm2) + GameDefine.GetAttributeName_S(curSelectSkillData.Parm1);
			}
			else
			{
				SkillAttributeIcon.spriteName = GameDefine.GetAttributeIcon(guildSkillData.Parm1);
				SKillAttributeValue.text = GameDefine.GetAttributeValueStr(guildSkillData.Parm1, 0) + GameDefine.GetAttributeName_S(guildSkillData.Parm1);
			}
			if (guildSkillData != null)
			{
				NGUITools.SetActive(upgradeBtn, state: true);
				NGUITools.SetActive(NextSKillAttributeIcon.gameObject, state: true);
				NextSKillAttributeIcon.spriteName = GameDefine.GetAttributeIcon(guildSkillData.Parm1);
				NextSKillAttributeValue.text = GameDefine.GetAttributeValueStr(guildSkillData.Parm1, guildSkillData.Parm2) + GameDefine.GetAttributeName_S(guildSkillData.Parm1);
			}
			else
			{
				NextSKillAttributeValue.text = string.Empty;
				NGUITools.SetActive(NextSKillAttributeIcon.gameObject, state: false);
				NGUITools.SetActive(upgradeBtn, state: false);
			}
			if (skillType.level >= playerData.PlayerGuild.GuilLevel)
			{
				UpgradeBtnSprite.spriteName = "CZ_anNiu_2+";
			}
			else
			{
				UpgradeBtnSprite.spriteName = "CZ_anNiu_2";
			}
		}
		else
		{
			UpgradeBtnSprite.spriteName = "CZ_anNiu_2+";
		}
	}

	public void OnClickItem(guild_skill skill, GameObject obj)
	{
		curSkill = skill;
		curSelectObj = obj;
		UpdateSelect(curSkill, curSelectObj);
	}

	public void OnClickUpgrade()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (curSkill.level >= playerData.PlayerGuild.GuilLevel)
		{
			NoticeLogic.AddNotifyData("#{101144}");
		}
		else if (GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.GUILD_CONTRIBUTE, nextSkillData.SkillCost) && GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.CASH, nextSkillData.SkillCost))
		{
			guild_skill_level.request request = new guild_skill_level.request();
			request.guildSkillType = curSkill.skillType;
			NetLogic.GetInstance().Send<Protocol.guild_skill_level>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Guild", $"Skill_{curSkill.skillType}", $"skilllevel_{curSkill.level + 1}");
		}
	}

	public void OnClickTips()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
		{
			SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", "#{100791}", null);
		});
	}
}
