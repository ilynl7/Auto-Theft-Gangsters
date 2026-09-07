using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildBattleResultRootLogic : SingletonUnity<GuildBattleResultRootLogic>
{
	public GameObject VictoryRoot;

	public GameObject FailRoot;

	public GameObject ResultRoot;

	public GameObject ScoreRoot;

	public UILabel BattleInfoLabel;

	public UILabel LeftGuildName;

	public UILabel LeftRankLabel;

	public UILabel RightGuildName;

	public UILabel RightRankLabel;

	public GameObject[] TopObj;

	public UILabel[] TopLabel;

	public UISprite resultBtn;

	public UISprite scoreBtn;

	public List<GuildBattleScoreLine> ScoreLines;

	private List<guild_battle_item_info> infoDatas = new List<guild_battle_item_info>();

	private guild_battle_score_info guildBattleInfo;

	public UIWrapContentNew uiWrapContent;

	private int lineMinCount = 6;

	public UIWidget WrapContentBottomWidget;

	public UIScrollView uiScrollView;

	public UISprite LeftGuildFlag;

	public UISprite RightGuildFlag;

	private bool isWin;

	protected override void Awake()
	{
		base.Awake();
		uiWrapContent.enabled = false;
		UIWrapContentNew uIWrapContentNew = uiWrapContent;
		uIWrapContentNew.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContentNew.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
	}

	public void RefreshInfo(guild_battle_finish_info.request curinfo)
	{
		infoDatas.Clear();
		guildBattleInfo = null;
		if (curinfo.HasGuild_battle_score_info)
		{
			guildBattleInfo = curinfo.guild_battle_score_info;
			if (guildBattleInfo.HasItem_info)
			{
				infoDatas = guildBattleInfo.item_info;
			}
			infoDatas.Sort((guild_battle_item_info x, guild_battle_item_info y) => (int)y.score - (int)x.score);
		}
		if (guildBattleInfo != null && guildBattleInfo.HasGuildIcon1 && guildBattleInfo.guildIcon1 >= 0 && guildBattleInfo.guildIcon1 < GameDefine.GuildIcon.Length)
		{
			LeftGuildFlag.enabled = true;
			LeftGuildFlag.spriteName = GameDefine.GuildIcon[guildBattleInfo.guildIcon1];
		}
		else
		{
			LeftGuildFlag.enabled = false;
		}
		if (guildBattleInfo != null && guildBattleInfo.HasGuildIcon2 && guildBattleInfo.guildIcon2 >= 0 && guildBattleInfo.guildIcon2 < GameDefine.GuildIcon.Length)
		{
			RightGuildFlag.enabled = true;
			RightGuildFlag.spriteName = GameDefine.GuildIcon[guildBattleInfo.guildIcon2];
		}
		else
		{
			RightGuildFlag.enabled = false;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsGuildBattleRedTeam)
		{
			if (guildBattleInfo != null && guildBattleInfo.HasWin)
			{
				isWin = guildBattleInfo.win != 1;
			}
			else
			{
				isWin = false;
			}
		}
		else if (guildBattleInfo != null && guildBattleInfo.HasWin)
		{
			isWin = guildBattleInfo.win == 1;
		}
		else
		{
			isWin = false;
		}
		NGUITools.SetActive(VictoryRoot, isWin);
		NGUITools.SetActive(FailRoot, !isWin);
		OnClickResultBtn();
	}

	public void OnClickResultBtn()
	{
		resultBtn.spriteName = "CZ_huaDongBG_1";
		scoreBtn.spriteName = "CZ_huaDongBG";
		NGUITools.SetActive(ResultRoot, state: true);
		NGUITools.SetActive(ScoreRoot, state: false);
		if (isWin)
		{
			BattleInfoLabel.text = StrDictionary.GetDictionaryString("#{105027}");
		}
		else
		{
			BattleInfoLabel.text = StrDictionary.GetDictionaryString("#{105028}");
		}
		if (guildBattleInfo == null)
		{
			return;
		}
		if (guildBattleInfo.HasGuildName1)
		{
			LeftGuildName.text = guildBattleInfo.guildName1;
			LeftRankLabel.text = string.Format("{0}: {1}", StrDictionary.GetDictionaryString("#{105021}"), guildBattleInfo.score1);
		}
		else
		{
			LeftGuildName.text = string.Empty;
			LeftRankLabel.text = string.Empty;
		}
		if (guildBattleInfo.HasGuildName2)
		{
			RightGuildName.text = guildBattleInfo.guildName2;
			RightRankLabel.text = string.Format("{0}: {1}", StrDictionary.GetDictionaryString("#{105021}"), guildBattleInfo.score2);
		}
		else
		{
			RightGuildName.text = string.Empty;
			RightRankLabel.text = string.Empty;
		}
		for (int i = 0; i < TopLabel.Length; i++)
		{
			if (i < infoDatas.Count)
			{
				NGUITools.SetActive(TopObj[i], state: true);
				TopLabel[i].text = infoDatas[i].name;
			}
			else
			{
				NGUITools.SetActive(TopObj[i], state: false);
				TopLabel[i].text = string.Empty;
			}
		}
	}

	public void OnClickScoreBtn()
	{
		resultBtn.spriteName = "CZ_huaDongBG";
		scoreBtn.spriteName = "CZ_huaDongBG_1";
		NGUITools.SetActive(ResultRoot, state: false);
		NGUITools.SetActive(ScoreRoot, state: true);
		int num = Mathf.Min(infoDatas.Count, lineMinCount) - ScoreLines.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(ScoreLines[0].gameObject) as GameObject;
				GuildBattleScoreLine component = gameObject.GetComponent<GuildBattleScoreLine>();
				gameObject.name = $"{ScoreLines.Count:D2}";
				gameObject.transform.parent = uiWrapContent.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				ScoreLines.Add(component);
			}
		}
		for (int j = 0; j < ScoreLines.Count; j++)
		{
			if (j < infoDatas.Count)
			{
				NGUITools.SetActive(ScoreLines[j].gameObject, state: true);
				ScoreLines[j].UpdateInfo(infoDatas[j], j);
			}
			else
			{
				NGUITools.SetActive(ScoreLines[j].gameObject, state: false);
			}
		}
		uiWrapContent.minIndex = 1 - ScoreLines.Count;
		WrapContentBottomWidget.height = ScoreLines.Count * uiWrapContent.itemSize;
		uiWrapContent.SortBasedOnScrollMovement();
		uiScrollView.ResetPosition();
		uiWrapContent.enabled = true;
	}

	public void OnClickExitBtn()
	{
		NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		GuildBattleScoreLine itemLogic = ScoreLines[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex));
	}

	private void ResetItemLine(GuildBattleScoreLine itemLogic, int idx)
	{
		if (idx < infoDatas.Count)
		{
			itemLogic.UpdateInfo(infoDatas[idx], idx);
		}
	}
}
