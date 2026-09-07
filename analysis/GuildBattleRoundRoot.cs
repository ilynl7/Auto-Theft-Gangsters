using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildBattleRoundRoot : MonoBehaviour
{
	public UILabel[] GuildNameLabel;

	public UISprite[] WinPic;

	public UISprite BottomPic;

	public UILabel EndLabel;

	public UISprite StateFlag;

	public UILabel FlagLabel;

	private int RoundId;

	public TweenAlpha BgAnima;

	public void EnableReset()
	{
		for (int i = 0; i < GuildNameLabel.Length; i++)
		{
			GuildNameLabel[i].color = Color.gray;
			GuildNameLabel[i].text = StrDictionary.GetDictionaryString("#{105070}");
			WinPic[i].enabled = false;
		}
		EndLabel.enabled = false;
		NGUITools.SetActive(StateFlag.gameObject, state: false);
	}

	public void Reset(guild_battle_round roundInfo, bool isFinal, long targetGuildId, int curstate, int roundid)
	{
		EnableReset();
		RoundId = roundid;
		bool flag = false;
		int num = -1;
		bool flag2 = curstate % 2 == 0 && curstate >= 0;
		switch (curstate)
		{
		case 0:
		case 1:
			num = 1;
			break;
		case 2:
		case 3:
			num = 2;
			break;
		case 4:
		case 5:
			num = 3;
			break;
		}
		flag = num == RoundId;
		if (roundInfo != null && roundInfo.HasBattle_team)
		{
			List<guild_battle_team> list = new List<guild_battle_team>(roundInfo.battle_team.Values);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].HasGuildName)
				{
					GuildNameLabel[list[i].index - 1].text = list[i].guildName;
					if (list[i].state == 4)
					{
						GuildNameLabel[list[i].index - 1].color = Color.gray;
					}
					else
					{
						GuildNameLabel[list[i].index - 1].color = Color.white;
					}
					if (flag && targetGuildId != -1 && list[i].guildId == targetGuildId && flag)
					{
						GuildNameLabel[list[i].index - 1].color = Color.green;
					}
				}
				if (list[i].state == 3)
				{
					if (list[i].HasGuildName)
					{
						WinPic[list[i].index - 1].enabled = true;
					}
					if (isFinal)
					{
						if ((list[i].index - 1) / 2 == 0L)
						{
							WinPic[list[i].index - 1].spriteName = "CZ_gongHui_GuanJun";
						}
						else if ((list[i].index - 1) / 2 == 1)
						{
							WinPic[list[i].index - 1].spriteName = "CZ_gongHui_JiJun";
						}
						else
						{
							WinPic[list[i].index - 1].enabled = false;
						}
					}
				}
				else if (isFinal)
				{
					if (list[i].state == 4)
					{
						if ((list[i].index - 1) / 2 == 0L)
						{
							if (list[i].HasGuildName)
							{
								WinPic[list[i].index - 1].enabled = true;
								WinPic[list[i].index - 1].spriteName = "CZ_gongHui_YaJun";
							}
							else
							{
								WinPic[list[i].index - 1].enabled = false;
							}
						}
						else
						{
							WinPic[list[i].index - 1].enabled = false;
						}
					}
					else
					{
						WinPic[list[i].index - 1].enabled = false;
					}
				}
				else
				{
					WinPic[list[i].index - 1].enabled = false;
				}
			}
		}
		BgAnima.ResetToBeginning();
		BgAnima.enabled = false;
		if (num == -1)
		{
			BottomPic.spriteName = "CZ_huaDongBG_2_XuanDing";
		}
		else if (num > RoundId)
		{
			BottomPic.spriteName = "CZ_huaDongBG_2_XuanDing";
			EndLabel.enabled = true;
		}
		else if (num == RoundId)
		{
			BottomPic.spriteName = "CZ_huaDongBG_1";
			NGUITools.SetActive(StateFlag.gameObject, state: true);
			if (flag2)
			{
				FlagLabel.text = StrDictionary.GetDictionaryString("#{105081}");
				return;
			}
			BgAnima.PlayForward();
			FlagLabel.text = StrDictionary.GetDictionaryString("#{105079}");
		}
		else
		{
			BottomPic.spriteName = "CZ_huaDongBG";
			NGUITools.SetActive(StateFlag.gameObject, state: true);
			FlagLabel.text = StrDictionary.GetDictionaryString("#{105080}");
		}
	}
}
