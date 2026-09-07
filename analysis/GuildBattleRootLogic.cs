using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildBattleRootLogic : SingletonUnity<GuildBattleRootLogic>
{
	public GuildBattleRoundRoot BattleRound1;

	public GuildBattleRoundRoot BattleRound2;

	public GuildBattleRoundRoot BattleRound3;

	public ShowRewardItems ShowRewardItems;

	private GuildBattleData mCurGuildBattleData;

	public UISprite GuildMemberBtn;

	public UISprite StartBtn;

	public UISprite GambleBtn;

	public UILabel GambleLabel;

	public UILabel ShowTimeLabel;

	private bool IsShowTimeCount;

	public GameObject FirstRankObj;

	public GameObject NoFirstObj;

	public UILabel FirstRankLabel;

	private guild_battle_info battleInfo;

	private bool mCanEnterBattle;

	private PlayerCommonData mPlayerCommonData;

	private float lastCheckTime;

	private long restTime;

	private bool requestStateFlag;

	public guild_battle_info CurBattleInfo => battleInfo;

	public void EnableReset()
	{
		mPlayerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		BattleRound1.EnableReset();
		BattleRound2.EnableReset();
		BattleRound3.EnableReset();
		NGUITools.SetActive(StartBtn.gameObject, state: false);
		NGUITools.SetActive(GambleBtn.gameObject, state: false);
		NGUITools.SetActive(ShowRewardItems.gameObject, state: false);
		ShowTimeLabel.text = string.Empty;
		IsShowTimeCount = false;
		NGUITools.SetActive(FirstRankObj.gameObject, state: false);
		NGUITools.SetActive(NoFirstObj.gameObject, state: true);
		mCanEnterBattle = false;
		ResetFinishBtn();
	}

	private void InitBattleData()
	{
		battleInfo = new guild_battle_info();
		battleInfo.time = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime() + 6000;
		battleInfo.ID = "1501";
		battleInfo.state = 4L;
		Guild playerGuild = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild;
		battleInfo.battle_round_1 = new guild_battle_round();
		battleInfo.battle_round_1.battle_team = new Dictionary<long, guild_battle_team>();
		guild_battle_team guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = playerGuild.ServerId;
		guild_battle_team.guildName = playerGuild.GuilName;
		guild_battle_team.state = 3L;
		guild_battle_team.index = 1L;
		battleInfo.battle_round_1.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 222L;
		guild_battle_team.guildName = "222";
		guild_battle_team.state = 4L;
		guild_battle_team.index = 2L;
		battleInfo.battle_round_1.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 333L;
		guild_battle_team.guildName = "333";
		guild_battle_team.state = 4L;
		guild_battle_team.index = 3L;
		battleInfo.battle_round_1.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 444L;
		guild_battle_team.guildName = "444";
		guild_battle_team.state = 3L;
		guild_battle_team.index = 4L;
		battleInfo.battle_round_1.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 555L;
		guild_battle_team.guildName = "555";
		guild_battle_team.state = 3L;
		guild_battle_team.index = 5L;
		battleInfo.battle_round_1.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 666L;
		guild_battle_team.guildName = "666";
		guild_battle_team.state = 4L;
		guild_battle_team.index = 6L;
		battleInfo.battle_round_1.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 777L;
		guild_battle_team.guildName = "777";
		guild_battle_team.state = 4L;
		guild_battle_team.index = 7L;
		battleInfo.battle_round_1.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 888L;
		guild_battle_team.guildName = "888";
		guild_battle_team.state = 3L;
		guild_battle_team.index = 8L;
		battleInfo.battle_round_1.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		battleInfo.battle_round_2 = new guild_battle_round();
		battleInfo.battle_round_2.battle_team = new Dictionary<long, guild_battle_team>();
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = playerGuild.ServerId;
		guild_battle_team.guildName = playerGuild.GuilName;
		guild_battle_team.state = 3L;
		guild_battle_team.index = 1L;
		battleInfo.battle_round_2.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 222L;
		guild_battle_team.guildName = "222";
		guild_battle_team.state = 4L;
		guild_battle_team.index = 2L;
		battleInfo.battle_round_2.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 333L;
		guild_battle_team.guildName = "333";
		guild_battle_team.state = 3L;
		guild_battle_team.index = 3L;
		battleInfo.battle_round_2.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 444L;
		guild_battle_team.guildName = "444";
		guild_battle_team.state = 4L;
		guild_battle_team.index = 4L;
		battleInfo.battle_round_2.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 555L;
		guild_battle_team.guildName = "555";
		guild_battle_team.state = 4L;
		guild_battle_team.index = 5L;
		battleInfo.battle_round_2.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 666L;
		guild_battle_team.guildName = "666";
		guild_battle_team.state = 3L;
		guild_battle_team.index = 6L;
		battleInfo.battle_round_2.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 777L;
		guild_battle_team.guildName = "777";
		guild_battle_team.state = 4L;
		guild_battle_team.index = 7L;
		battleInfo.battle_round_2.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 888L;
		guild_battle_team.guildName = "888";
		guild_battle_team.state = 3L;
		guild_battle_team.index = 8L;
		battleInfo.battle_round_2.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		battleInfo.battle_round_3 = new guild_battle_round();
		battleInfo.battle_round_3.battle_team = new Dictionary<long, guild_battle_team>();
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = playerGuild.ServerId;
		guild_battle_team.guildName = playerGuild.GuilName;
		guild_battle_team.state = 0L;
		guild_battle_team.index = 1L;
		battleInfo.battle_round_3.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 222L;
		guild_battle_team.guildName = "222";
		guild_battle_team.state = 0L;
		guild_battle_team.index = 2L;
		battleInfo.battle_round_3.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 333L;
		guild_battle_team.guildName = "333";
		guild_battle_team.state = 0L;
		guild_battle_team.index = 3L;
		battleInfo.battle_round_3.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 444L;
		guild_battle_team.guildName = "444";
		guild_battle_team.state = 0L;
		guild_battle_team.index = 4L;
		battleInfo.battle_round_3.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 555L;
		guild_battle_team.guildName = "555";
		guild_battle_team.state = 0L;
		guild_battle_team.index = 5L;
		battleInfo.battle_round_3.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 666L;
		guild_battle_team.guildName = "666";
		guild_battle_team.state = 0L;
		guild_battle_team.index = 6L;
		battleInfo.battle_round_3.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 777L;
		guild_battle_team.guildName = "777";
		guild_battle_team.state = 0L;
		guild_battle_team.index = 7L;
		battleInfo.battle_round_3.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
		guild_battle_team = new guild_battle_team();
		guild_battle_team.guildId = 888L;
		guild_battle_team.guildName = "888";
		guild_battle_team.state = 0L;
		guild_battle_team.index = 8L;
		battleInfo.battle_round_3.battle_team.Add(guild_battle_team.guildId, guild_battle_team);
	}

	public void Reset(ret_guild_battle_info.request request)
	{
		battleInfo = request.battle_info;
		UpdatePage();
		requestStateFlag = false;
	}

	public void UpdatePage()
	{
		mCurGuildBattleData = DataManager.GetGuildBattleDataById(battleInfo.ID);
		if (mCurGuildBattleData != null)
		{
			ShowRewardData showRewardDataByID = DataManager.GetShowRewardDataByID(mCurGuildBattleData.ShowRewardID);
			if (showRewardDataByID != null)
			{
				NGUITools.SetActive(ShowRewardItems.gameObject, state: true);
				ShowRewardItems.ShowRewards(showRewardDataByID.ItemIdList, showRewardDataByID.QualityList, showRewardDataByID.CountList);
			}
			else
			{
				NGUITools.SetActive(ShowRewardItems.gameObject, state: false);
			}
		}
		ResetBtn();
		if (battleInfo.state == 0L || battleInfo.state == 2 || battleInfo.state == 4)
		{
			IsShowTimeCount = false;
			string dictionaryString = StrDictionary.GetDictionaryString(GameDefine.RoundName[battleInfo.state / 2]);
			int num = 0;
			int num2 = 0;
			if (battleInfo.state == 0L)
			{
				num = mCurGuildBattleData.Week1;
				num2 = mCurGuildBattleData.StartTime1;
			}
			else if (battleInfo.state == 2)
			{
				num = mCurGuildBattleData.Week2;
				num2 = mCurGuildBattleData.StartTime2;
			}
			else if (battleInfo.state == 4)
			{
				num = mCurGuildBattleData.Week3;
				num2 = mCurGuildBattleData.StartTime3;
			}
			PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			int key = (num - 1 + TimeTools.GetOffsetDay(num2, playerCommonData.TimeOffset) + GameDefine.WEEK_NAME.Count) % GameDefine.WEEK_NAME.Count;
			TimeSpan localShowTime = TimeTools.GetLocalShowTime(num2, playerCommonData.TimeOffset);
			TimeSpan localShowTime2 = TimeTools.GetLocalShowTime(num2 + mCurGuildBattleData.DurationTime, playerCommonData.TimeOffset);
			string arg = $"{StrDictionary.GetDictionaryString(GameDefine.WEEK_NAME[key])} {$"{localShowTime.Hours:D2}:{localShowTime.Minutes:D2}"}-{$"{localShowTime2.Hours:D2}:{localShowTime2.Minutes:D2}"}";
			ShowTimeLabel.text = string.Format("{0} {1} {2}", dictionaryString, StrDictionary.GetDictionaryString("#{105085}"), arg);
		}
		else if (battleInfo.state == 1 || battleInfo.state == 3 || battleInfo.state == 5)
		{
			IsShowTimeCount = true;
			string dictionaryString2 = StrDictionary.GetDictionaryString(GameDefine.RoundName[battleInfo.state / 2]);
			if (battleInfo.HasTime)
			{
				ShowTimeLabel.text = string.Format("{0} {1}", dictionaryString2, StrDictionary.GetDictionaryString("#{105055}", TimeTools.GetDaySecondStr(battleInfo.time - mPlayerCommonData.GetCurServerTime())));
			}
			else
			{
				ShowTimeLabel.text = string.Empty;
			}
		}
		else if (battleInfo.state < 0 || battleInfo.state >= 6)
		{
			IsShowTimeCount = false;
			int num3 = 0;
			int num4 = 0;
			num3 = mCurGuildBattleData.Week;
			num4 = mCurGuildBattleData.StartTime;
			PlayerCommonData playerCommonData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			int key2 = (num3 - 1 + TimeTools.GetOffsetDay(num4, playerCommonData2.TimeOffset) + GameDefine.WEEK_NAME.Count) % GameDefine.WEEK_NAME.Count;
			TimeSpan localShowTime3 = TimeTools.GetLocalShowTime(num4, playerCommonData2.TimeOffset);
			ShowTimeLabel.text = string.Format("{0}:{1} {2:d2}:{3:d2}", StrDictionary.GetDictionaryString("#{105076}"), StrDictionary.GetDictionaryString(GameDefine.WEEK_NAME[key2]), localShowTime3.Hours, localShowTime3.Minutes);
		}
		long targetGuildId = -1L;
		if (mCanEnterBattle)
		{
			targetGuildId = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.ServerId;
		}
		else if (battleInfo.HasGuildId)
		{
			targetGuildId = battleInfo.guildId;
		}
		if (battleInfo.HasBattle_round_1)
		{
			BattleRound1.Reset(battleInfo.battle_round_1, isFinal: false, targetGuildId, (int)battleInfo.state, 1);
		}
		else
		{
			BattleRound1.Reset(null, isFinal: false, targetGuildId, (int)battleInfo.state, 1);
		}
		if (battleInfo.HasBattle_round_2)
		{
			BattleRound2.Reset(battleInfo.battle_round_2, isFinal: false, targetGuildId, (int)battleInfo.state, 2);
		}
		else
		{
			BattleRound2.Reset(null, isFinal: false, targetGuildId, (int)battleInfo.state, 2);
		}
		if (battleInfo.HasBattle_round_3)
		{
			BattleRound3.Reset(battleInfo.battle_round_3, isFinal: true, targetGuildId, (int)battleInfo.state, 3);
		}
		else
		{
			BattleRound3.Reset(null, isFinal: true, targetGuildId, (int)battleInfo.state, 3);
		}
		if (battleInfo.state == 6 || battleInfo.state == -1)
		{
			string text = string.Empty;
			if (battleInfo.HasChampionName)
			{
				text = battleInfo.championName;
			}
			if (!string.IsNullOrEmpty(text))
			{
				NGUITools.SetActive(FirstRankObj, state: true);
				NGUITools.SetActive(NoFirstObj, state: false);
				FirstRankLabel.text = text;
			}
			else
			{
				NGUITools.SetActive(FirstRankObj, state: false);
				NGUITools.SetActive(NoFirstObj, state: true);
			}
		}
	}

	private void ResetBtn()
	{
		if (battleInfo == null)
		{
			return;
		}
		mCanEnterBattle = false;
		if (battleInfo.state == 0L || battleInfo.state == 1)
		{
			if (battleInfo.battle_round_1 != null)
			{
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
				{
					long serverId = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.ServerId;
					List<guild_battle_team> list = new List<guild_battle_team>(battleInfo.battle_round_1.battle_team.Values);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].guildId == serverId)
						{
							mCanEnterBattle = true;
							SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsGuildBattleRedTeam = list[i].index % 2 == 0;
							break;
						}
					}
				}
				else
				{
					mCanEnterBattle = false;
				}
			}
		}
		else if (battleInfo.state == 2 || battleInfo.state == 3)
		{
			if (battleInfo.battle_round_2 != null)
			{
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
				{
					long serverId2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.ServerId;
					List<guild_battle_team> list2 = new List<guild_battle_team>(battleInfo.battle_round_2.battle_team.Values);
					for (int j = 0; j < list2.Count; j++)
					{
						if (list2[j].guildId == serverId2)
						{
							mCanEnterBattle = true;
							SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsGuildBattleRedTeam = list2[j].index % 2 == 0;
							break;
						}
					}
				}
				else
				{
					mCanEnterBattle = false;
				}
			}
		}
		else if ((battleInfo.state == 4 || battleInfo.state == 5) && battleInfo.battle_round_3 != null)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				long serverId3 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.ServerId;
				List<guild_battle_team> list3 = new List<guild_battle_team>(battleInfo.battle_round_3.battle_team.Values);
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].guildId == serverId3)
					{
						mCanEnterBattle = true;
						SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsGuildBattleRedTeam = list3[k].index % 2 == 0;
						break;
					}
				}
			}
			else
			{
				mCanEnterBattle = false;
			}
		}
		if (mCanEnterBattle)
		{
			ResetFightPage();
		}
		else
		{
			ResetGamblePage();
		}
	}

	private void ResetFinishBtn()
	{
		NGUITools.SetActive(StartBtn.gameObject, state: false);
		NGUITools.SetActive(GuildMemberBtn.gameObject, state: false);
		NGUITools.SetActive(GambleBtn.gameObject, state: false);
	}

	private void ResetGamblePage()
	{
		NGUITools.SetActive(StartBtn.gameObject, state: false);
		NGUITools.SetActive(GuildMemberBtn.gameObject, state: false);
		if (battleInfo.state >= 6 || battleInfo.state < 0)
		{
			NGUITools.SetActive(GambleBtn.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(GambleBtn.gameObject, state: true);
		}
		if (battleInfo.HasGuildId || battleInfo.state == 6)
		{
			GambleBtn.spriteName = "CZ_anNiu_2+";
			GambleLabel.text = StrDictionary.GetDictionaryString(StrDictionary.GetDictionaryString("#{105046}"));
		}
		else
		{
			GambleBtn.spriteName = "CZ_anNiu_2";
			GambleLabel.text = StrDictionary.GetDictionaryString(StrDictionary.GetDictionaryString("#{105023}"));
		}
	}

	private void ResetFightPage()
	{
		NGUITools.SetActive(StartBtn.gameObject, state: true);
		NGUITools.SetActive(GuildMemberBtn.gameObject, state: true);
		NGUITools.SetActive(GambleBtn.gameObject, state: false);
		if (battleInfo.state == 6)
		{
			StartBtn.spriteName = "CZ_anNiu_2+";
		}
		else
		{
			StartBtn.spriteName = "CZ_anNiu_2";
		}
	}

	public void OnClickStartBtn()
	{
		if (battleInfo.state == 1 || battleInfo.state == 3 || battleInfo.state == 5)
		{
			enter_guild_battle.request rpcReq = new enter_guild_battle.request();
			WaitResponseUIRootLogic.OpenWaitBox(286, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.enter_guild_battle>(rpcReq);
		}
		else
		{
			NoticeLogic.AddNotifyData("#{105011}");
		}
	}

	public void OnClickRankBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildBattleRankRoot, delegate
		{
			SingletonUnity<GuildBattleRankRoot>.Instance.EnableReset();
			WaitResponseUIRootLogic.OpenWaitBox(287, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.req_guild_battle_rank>();
		});
	}

	public void OnClickMemberBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildBattleMemberRoot, delegate
		{
			SingletonUnity<GuildBattleMemberRootLogic>.Instance.EnableReset();
			WaitResponseUIRootLogic.OpenWaitBox(288, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.req_guild_battle_member>();
		});
	}

	public void OnClickRuleBtn()
	{
		if (mCurGuildBattleData != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
			{
				SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", mCurGuildBattleData.MRule, null);
			});
		}
	}

	public void OnClickGambleBtn()
	{
		if (battleInfo.HasGuildId && battleInfo.guildId != -1)
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{105068}"));
		}
		else if (battleInfo.state == 0L || battleInfo.state == 2 || battleInfo.state == 4)
		{
			if (battleInfo.HasGuildId)
			{
				NoticeLogic.AddNotifyData("#{105068}");
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildBattleGambleRoot, delegate
			{
				SingletonUnity<GuildBattleGambleRoot>.Instance.Reset();
				if (battleInfo.state == 0L)
				{
					SingletonUnity<GuildBattleGambleRoot>.Instance.RefreshInfo(battleInfo.battle_round_1);
				}
				else if (battleInfo.state == 2)
				{
					SingletonUnity<GuildBattleGambleRoot>.Instance.RefreshInfo(battleInfo.battle_round_2);
				}
				else if (battleInfo.state == 4)
				{
					SingletonUnity<GuildBattleGambleRoot>.Instance.RefreshInfo(battleInfo.battle_round_3);
				}
			});
		}
		else
		{
			NoticeLogic.AddNotifyData("#{105071}");
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRoot);
		if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewActivityUIRootLogic>.Instance.CurPageIndex = -1;
			SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn();
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildActivityRootLogic, delegate
		{
			SingletonUnity<GuildActivityRootLogic>.Instance.EnableReset();
			WaitResponseUIRootLogic.OpenWaitBox(195, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.request_guild_boss>();
		});
	}

	private void Update()
	{
		if (battleInfo != null && Time.realtimeSinceStartup - lastCheckTime > 1f)
		{
			lastCheckTime = Time.realtimeSinceStartup;
			UpdateTimeLabel();
		}
	}

	private void UpdateTimeLabel()
	{
		if (battleInfo == null || !battleInfo.HasTime)
		{
			return;
		}
		restTime = battleInfo.time - mPlayerCommonData.GetCurServerTime();
		if (restTime > 0)
		{
			if (IsShowTimeCount)
			{
				string dictionaryString = StrDictionary.GetDictionaryString(GameDefine.RoundName[battleInfo.state / 2]);
				ShowTimeLabel.text = string.Format("{0} {1}", dictionaryString, StrDictionary.GetDictionaryString("#{105055}", TimeTools.GetDaySecondStr(battleInfo.time - mPlayerCommonData.GetCurServerTime())));
			}
		}
		else if (!requestStateFlag)
		{
			requestStateFlag = true;
			WaitResponseUIRootLogic.OpenWaitBox(285, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.req_guild_battle_info>();
		}
	}

	private string GetBattleTimeStr(long state, string timeStr)
	{
		if (state >= 0 && state <= 6)
		{
			switch (state)
			{
			case 0L:
				return StrDictionary.GetDictionaryString("#{105056}", timeStr);
			case 2L:
				return StrDictionary.GetDictionaryString("#{105053}", timeStr);
			case 4L:
				return StrDictionary.GetDictionaryString("#{105054}", timeStr);
			case 6L:
				return StrDictionary.GetDictionaryString("#{105052}", timeStr);
			}
		}
		return StrDictionary.GetDictionaryString("#{105055}", timeStr);
	}

	public bool IsPreparingState()
	{
		if (battleInfo != null && (battleInfo.state == 0L || battleInfo.state == 2 || battleInfo.state == 4))
		{
			return true;
		}
		return false;
	}
}
