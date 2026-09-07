using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildBattleInfoRoot : SingletonUnity<GuildBattleInfoRoot>
{
	public UILabel infoNamelabel;

	public Transform ScoreBtn;

	public Transform KillBtn;

	public Transform selectPic;

	private int ShowTopNum = 6;

	public UIGrid uiGrid;

	public List<GuildBattleInfoLine> infoLines;

	public UILabel EnemyScoreLabel;

	public UILabel OurScoreLabel;

	public UILabel selfRanklabel;

	public UILabel selfNamelabel;

	public UILabel SelfInfoLabel;

	public GameObject SliderObj;

	public UISlider BlueLineSlider;

	public UISlider RedLineSlider;

	private guild_battle_score_info curInfo;

	private bool isScoreRank = true;

	private List<guild_battle_item_info> ItemsList = new List<guild_battle_item_info>();

	public TweenRotation leftRotAni;

	public TweenPosition leftPosAni;

	private bool leftisopen;

	public UIAnchor leftAnchor;

	public GameObject InfoObj;

	public GameObject TeamObj;

	public List<TeamTipMemberLineLogic> TeamMemberLineList;

	public UISprite InfoSelectSp;

	public UISprite teamSelectSp;

	private int CurPage;

	private Color blueColor = new Color(31f / 85f, 0.7058824f, 1f);

	private Color redColor = Color.red;

	public void EnableReset()
	{
		CurPage = 0;
		isScoreRank = true;
		OnClickInfoBtn();
	}

	public void OnClickInfoBtn()
	{
		if (CurPage != 1)
		{
			NGUITools.SetActive(InfoObj.gameObject, state: true);
			NGUITools.SetActive(TeamObj.gameObject, state: false);
			CurPage = 1;
			InfoSelectSp.enabled = true;
			teamSelectSp.enabled = false;
			for (int i = 0; i < infoLines.Count; i++)
			{
				NGUITools.SetActive(infoLines[i].gameObject, state: false);
			}
		}
	}

	public void OnClickTeamBtn()
	{
		if (CurPage != 2)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (!playerData.IsHaveTeam())
			{
				NoticeLogic.AddNotifyData("#{103207}");
				return;
			}
			NGUITools.SetActive(InfoObj.gameObject, state: false);
			NGUITools.SetActive(TeamObj.gameObject, state: true);
			CurPage = 2;
			InfoSelectSp.enabled = false;
			teamSelectSp.enabled = true;
			ResetTeam();
		}
	}

	public void ResetTeam()
	{
		if (CurPage == 2)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (playerData.IsHaveTeam())
			{
				ResetTeamMember();
			}
		}
	}

	public void ResetTeamMember()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsTeamLeader())
		{
			for (int i = 0; i < playerData.TeamInfo.TeamMembers.Length; i++)
			{
				TeamMemberLineList[i].Reset(playerData.TeamInfo.TeamMembers[i]);
			}
			return;
		}
		int num = 0;
		TeamMemberLineList[num].Reset(playerData.TeamInfo.TeamLeader);
		num++;
		for (int j = 0; j < playerData.TeamInfo.TeamMembers.Length; j++)
		{
			if (playerData.TeamInfo.TeamMembers[j].ServerId != PlayerData.MainPlayerServerId)
			{
				TeamMemberLineList[num].Reset(playerData.TeamInfo.TeamMembers[j]);
				num++;
			}
		}
	}

	private TeamTipMemberLineLogic GetMemberLineById(long serverId)
	{
		for (int i = 0; i < TeamMemberLineList.Count; i++)
		{
			if (TeamMemberLineList[i].CurMember != null && TeamMemberLineList[i].CurMember.IsValid() && TeamMemberLineList[i].CurMember.ServerId == serverId)
			{
				return TeamMemberLineList[i];
			}
		}
		return null;
	}

	public void UpdateMemberInfo(TeamMember member)
	{
		if (CurPage == 2)
		{
			TeamTipMemberLineLogic memberLineById = GetMemberLineById(member.ServerId);
			if (memberLineById != null)
			{
				memberLineById.Reset(member);
			}
		}
	}

	public void UpdateInfo(guild_battle_score_info infodata)
	{
		if (CurPage == 1)
		{
			curInfo = infodata;
			ItemsList.Clear();
			if (infodata.HasItem_info)
			{
				ItemsList = infodata.item_info;
			}
			EnemyScoreLabel.text = curInfo.score2.ToString();
			OurScoreLabel.text = curInfo.score1.ToString();
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			selfNamelabel.text = playerData.MainPlayerAttrData.Name;
			if (!UnityVersionUtil.IsActive(base.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
				leftisopen = false;
				leftAnchor.enabled = true;
				leftRotAni.ResetToBeginning();
				leftPosAni.ResetToBeginning();
				OnClickLefthidebtn();
				NGUITools.SetActive(InfoObj.gameObject, state: true);
				NGUITools.SetActive(TeamObj.gameObject, state: false);
			}
			RefershLeftInfo();
		}
	}

	private void RefershLeftInfo()
	{
		if (curInfo.score1 != 0L || curInfo.score2 != 0L)
		{
			BlueLineSlider.value = (float)curInfo.score1 / (float)(curInfo.score1 + curInfo.score2);
			RedLineSlider.value = (float)curInfo.score2 / (float)(curInfo.score1 + curInfo.score2);
		}
		else
		{
			BlueLineSlider.value = 0.5f;
			RedLineSlider.value = 0.5f;
		}
		if (isScoreRank)
		{
			ItemsList.Sort((guild_battle_item_info x, guild_battle_item_info y) => (int)y.score - (int)x.score);
			infoNamelabel.text = StrDictionary.GetDictionaryString("#{105021}");
			for (int i = 0; i < ItemsList.Count; i++)
			{
				if (ItemsList[i].id == PlayerData.MainPlayerServerId)
				{
					selfRanklabel.text = $"NO.{i + 1}";
					SelfInfoLabel.text = ItemsList[i].score.ToString();
					break;
				}
			}
		}
		else
		{
			ItemsList.Sort((guild_battle_item_info x, guild_battle_item_info y) => (int)y.killNum - (int)x.killNum);
			infoNamelabel.text = StrDictionary.GetDictionaryString("#{105036}");
			for (int j = 0; j < ItemsList.Count; j++)
			{
				if (ItemsList[j].id == PlayerData.MainPlayerServerId)
				{
					selfRanklabel.text = $"NO.{j + 1}";
					SelfInfoLabel.text = ItemsList[j].killNum.ToString();
					break;
				}
			}
		}
		int num = ShowTopNum - infoLines.Count;
		if (num > 0)
		{
			for (int k = 0; k < num; k++)
			{
				GameObject gameObject = Object.Instantiate(infoLines[0].gameObject) as GameObject;
				GuildBattleInfoLine component = gameObject.GetComponent<GuildBattleInfoLine>();
				gameObject.name = $"{infoLines.Count:D2}";
				gameObject.transform.parent = uiGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				infoLines.Add(component);
			}
		}
		for (int l = 0; l < infoLines.Count; l++)
		{
			if (l < ItemsList.Count)
			{
				NGUITools.SetActive(infoLines[l].gameObject, state: true);
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsGuildBattleRedTeam)
				{
					if (ItemsList[l].guildId == SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.ServerId)
					{
						infoLines[l].updateItem(ItemsList[l], l, isScoreRank, isBlue: false);
					}
					else
					{
						infoLines[l].updateItem(ItemsList[l], l, isScoreRank, isBlue: true);
					}
				}
				else if (ItemsList[l].guildId == SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.ServerId)
				{
					infoLines[l].updateItem(ItemsList[l], l, isScoreRank, isBlue: true);
				}
				else
				{
					infoLines[l].updateItem(ItemsList[l], l, isScoreRank, isBlue: false);
				}
			}
			else
			{
				NGUITools.SetActive(infoLines[l].gameObject, state: false);
			}
		}
		uiGrid.Reposition();
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsGuildBattleRedTeam)
		{
			selfNamelabel.color = redColor;
			selfRanklabel.color = redColor;
			SelfInfoLabel.color = redColor;
		}
		else
		{
			selfNamelabel.color = blueColor;
			selfRanklabel.color = blueColor;
			SelfInfoLabel.color = blueColor;
		}
	}

	public void OnClickScoreBtn()
	{
		selectPic.parent = ScoreBtn;
		selectPic.localPosition = Vector3.zero;
		selectPic.localScale = Vector3.one;
		isScoreRank = true;
		RefershLeftInfo();
	}

	public void OnClickKillBtn()
	{
		selectPic.parent = KillBtn;
		selectPic.localPosition = Vector3.zero;
		selectPic.localScale = Vector3.one;
		isScoreRank = false;
		RefershLeftInfo();
	}

	public void OnClickLefthidebtn()
	{
		if (leftisopen)
		{
			leftRotAni.PlayReverse();
			leftPosAni.PlayReverse();
			leftisopen = false;
		}
		else
		{
			leftRotAni.PlayForward();
			leftPosAni.PlayForward();
			leftisopen = true;
		}
	}
}
