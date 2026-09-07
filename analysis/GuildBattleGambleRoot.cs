using System.Collections.Generic;
using SprotoType;

public class GuildBattleGambleRoot : SingletonUnity<GuildBattleGambleRoot>
{
	public UILabel TitleLabel;

	public UILabel[] GuildNameLabelList;

	public UISprite[] VoteBtnList;

	public UILabel[] VoteLabelList;

	public UISprite ConfirmBtn;

	private int VoteId = -1;

	private guild_battle_round CurRoundInfo;

	private long curVotedGuildId = -1L;

	private string curGuildName = string.Empty;

	private long[] guildIdList;

	public void Reset()
	{
		VoteId = -1;
		TitleLabel.text = StrDictionary.GetDictionaryString("#{105041}");
	}

	public void RefreshInfo(guild_battle_round roundInfo)
	{
		CurRoundInfo = roundInfo;
		for (int i = 0; i < GuildNameLabelList.Length; i++)
		{
			GuildNameLabelList[i].text = StrDictionary.GetDictionaryString("#{105070}");
			NGUITools.SetActive(VoteBtnList[i].gameObject, state: false);
		}
		if (roundInfo == null || !roundInfo.HasBattle_team)
		{
			return;
		}
		List<guild_battle_team> list = new List<guild_battle_team>(roundInfo.battle_team.Values);
		guildIdList = new long[list.Count];
		for (int j = 0; j < list.Count; j++)
		{
			if (string.IsNullOrEmpty(list[j].guildName))
			{
				GuildNameLabelList[list[j].index - 1].text = StrDictionary.GetDictionaryString("#{105070}");
				NGUITools.SetActive(VoteBtnList[list[j].index - 1].gameObject, state: false);
			}
			else
			{
				GuildNameLabelList[list[j].index - 1].text = list[j].guildName;
				guildIdList[list[j].index - 1] = list[j].guildId;
				NGUITools.SetActive(VoteBtnList[list[j].index - 1].gameObject, state: true);
			}
		}
	}

	public void OnClickVoteBtn0()
	{
		OnClickVote(0);
	}

	public void OnClickVoteBtn1()
	{
		OnClickVote(1);
	}

	public void OnClickVoteBtn2()
	{
		OnClickVote(2);
	}

	public void OnClickVoteBtn3()
	{
		OnClickVote(3);
	}

	public void OnClickVoteBtn4()
	{
		OnClickVote(4);
	}

	public void OnClickVoteBtn5()
	{
		OnClickVote(5);
	}

	public void OnClickVoteBtn6()
	{
		OnClickVote(6);
	}

	public void OnClickVoteBtn7()
	{
		OnClickVote(7);
	}

	public void OnClickVote(int index)
	{
		VoteId = index;
		for (int i = 0; i < VoteBtnList.Length; i++)
		{
			if (i == index)
			{
				VoteBtnList[i].spriteName = GameDefine.BtnIcon[0];
				VoteLabelList[i].text = StrDictionary.GetDictionaryString("#{105046}");
			}
			else
			{
				VoteBtnList[i].spriteName = GameDefine.BtnIcon[1];
				VoteLabelList[i].text = StrDictionary.GetDictionaryString("#{105042}");
			}
		}
		curVotedGuildId = guildIdList[index];
		curGuildName = GuildNameLabelList[index].text;
	}

	public void OnClickConfirmBtn()
	{
		if (VoteId != -1 || CurRoundInfo != null)
		{
			GuildBattleData battleData = DataManager.GetGuildBattleDataById(SingletonUnity<GuildBattleRootLogic>.Instance.CurBattleInfo.ID);
			if (curVotedGuildId > 0)
			{
				MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{105044}", GameMoneyHelper.GetMoneyValStr(battleData.GuessCost, battleData.GuessID), curGuildName), "#{100127}", delegate
				{
					if (GameMoneyHelper.BeforeCheckBuy(GameDefine.ITEM_ID_MONEYTYPR[battleData.GuessID], battleData.GuessCost))
					{
						guild_battle_guess.request rpcReq = new guild_battle_guess.request
						{
							guildId = curVotedGuildId
						};
						NetLogic.GetInstance().Send<Protocol.guild_battle_guess>(rpcReq);
						SingletonUnity<GuildBattleRootLogic>.Instance.CurBattleInfo.guildId = curVotedGuildId;
						SingletonUnity<GuildBattleRootLogic>.Instance.UpdatePage();
						SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleGambleRoot);
					}
				});
			}
			else
			{
				NoticeLogic.AddNotifyData("#{105069}");
			}
		}
		else
		{
			NoticeLogic.AddNotifyData("#{105069}");
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleGambleRoot);
	}
}
