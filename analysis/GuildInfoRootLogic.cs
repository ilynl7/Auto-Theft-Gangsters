using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildInfoRootLogic : SingletonUnity<GuildInfoRootLogic>
{
	public UISprite GuildIcon;

	public UILabel Name;

	public UILabel Bossname;

	public UILabel LevelLab;

	public UILabel MemberNum;

	public UILabel NoticeLab;

	public UILabel ComboValueLab;

	public UILabel ExpLab;

	public GameObject NoticeTips;

	public Guild curInfo;

	public UIInput Notice;

	public UILabel JobLabel;

	public UILabel ContributeLabel;

	public UILabel AllContributeLabel;

	public DonateItem[] DonateItems;

	public UISlider expSlider;

	public UILabel ActiveLabel;

	public GameObject ActiveObj;

	public void UpdateChangeTips()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		Notice.enabled = playerData.PlayerGuild.CanEditNotice();
		UnityVersionUtil.SetActiveRecursive(NoticeTips, playerData.PlayerGuild.CanEditNotice());
	}

	public void UpdateDonate(Dictionary<string, donate_record> records)
	{
		List<donate_record> list = new List<donate_record>(records.Values);
		list.Sort((donate_record d1, donate_record d2) => d1.id.CompareTo(d2.id));
		for (int i = 0; i < list.Count; i++)
		{
			if (i < DonateItems.Length)
			{
				GuildDonateData guildDonateDataByID = DataManager.GetGuildDonateDataByID(list[i].id);
				DonateItems[i].Init(guildDonateDataByID, list[i]);
			}
			else
			{
				Debug.LogError("Donate max 3333333!!!");
			}
		}
	}

	public void UpDateGuildInfo(Guild info)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		curInfo = info;
		GuildIcon.spriteName = GameDefine.GuildIcon[info.GuildIcon];
		Name.text = $"{info.GuilName}";
		Bossname.text = $"{info.GuildChiefName}";
		LevelLab.text = $"LV:{info.GuilLevel}";
		MemberNum.text = $"{info.GuildMemberNum}/{info.GuildMaxPlayer}";
		NoticeLab.text = $"{info.Notice}";
		ExpLab.text = info.GuildExp.ToString();
		List<GuildLevelData> guildLevelDataList = DataManager.GetGuildLevelDataList();
		if (info.GuilLevel < guildLevelDataList.Count)
		{
			float num = 0f;
			if (info.GuilLevel > 1)
			{
				num = DataManager.GetGuildLevelDataByLevel(info.GuilLevel - 2).GuildExp;
			}
			GuildLevelData guildLevelDataByLevel = DataManager.GetGuildLevelDataByLevel(info.GuilLevel - 1);
			float value = Mathf.Clamp01(((float)info.GuildExp - num) / ((float)guildLevelDataByLevel.GuildExp - num));
			expSlider.value = value;
		}
		else
		{
			expSlider.value = 1f;
		}
		ComboValueLab.text = info.GuildCombo.ToString();
		Notice.defaultText = info.Notice;
		JobLabel.text = StrDictionary.GetDictionaryString(GameDefine.GuildJobStr[(int)info.PlayerJob]);
		ContributeLabel.text = playerData.GuildContribute.ToString();
		AllContributeLabel.text = info.GuildAllContribute.ToString();
		UpdateDonate(info.DonateRecord);
		UpdateChangeTips();
		ShowActiveInfo(curInfo.DisactiveState == 0);
	}

	private void ShowActiveInfo(bool isActive)
	{
		if (isActive)
		{
			NGUITools.SetActive(ActiveObj, state: true);
			ActiveLabel.color = new Color(1f, 11f / 15f, 0f);
		}
		else
		{
			NGUITools.SetActive(ActiveObj, state: false);
			ActiveLabel.color = Color.gray;
		}
	}

	public void OnClickTiShiBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
		{
			SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", "#{100798}", null);
		});
	}

	public void OnClickChangeNoticeBtn()
	{
		if (!string.IsNullOrEmpty(Notice.value))
		{
			Singleton<ObjManager>.Instance.MainPlayer.ChangeGuildNotice(Notice.value);
		}
	}
}
