using System;
using System.Collections.Generic;
using SprotoType;

public class ActivityTipsRootLogic : SingletonUnity<ActivityTipsRootLogic>
{
	public enum ShowType
	{
		activity,
		shopnpc
	}

	public UILabel TitleLabel;

	public UILabel InfoLabel;

	public UISprite picSp;

	private ActivityMapData CurData;

	public TweenPosition PosAnima;

	public bool isShow;

	public UISprite BtnSp;

	public UILabel BtnLabel;

	private ShowType curType;

	private ObjNPC CurShopNpc;

	public void ShowInfo(ActivityMapData curdata)
	{
		isShow = true;
		CurData = curdata;
		curType = ShowType.activity;
		ShowLabelInfo();
		picSp.spriteName = CurData.Icon;
		PosAnima.ResetToBeginning();
		PosAnima.PlayForward();
		BtnLabel.text = StrDictionary.GetDictionaryString("#{100702}");
	}

	public void ShowShopNpcInfo(ObjNPC target)
	{
		isShow = true;
		curType = ShowType.shopnpc;
		BtnSp.alpha = 1f;
		CurShopNpc = target;
		picSp.spriteName = "CZ_zhuJieMianAnNiu_ShangCheng";
		PosAnima.ResetToBeginning();
		PosAnima.PlayForward();
		BtnLabel.text = StrDictionary.GetDictionaryString("#{301139}");
		TitleLabel.text = string.Empty;
		InfoLabel.text = string.Empty;
		NpcData nPCData = target.NPCData;
		if (nPCData != null)
		{
			switch ((GameDefine.NPC_FUNCTION_TYPE)nPCData.FunctionType)
			{
			case GameDefine.NPC_FUNCTION_TYPE.RECHARGE:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301127}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301128}");
				break;
			case GameDefine.NPC_FUNCTION_TYPE.TOOL_SHOP:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301129}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301130}");
				break;
			case GameDefine.NPC_FUNCTION_TYPE.EQUIP_SHOP:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301131}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301132}");
				break;
			case GameDefine.NPC_FUNCTION_TYPE.BIGSALE_SHOP:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301133}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301134}");
				break;
			case GameDefine.NPC_FUNCTION_TYPE.GANG_SHOP:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301135}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301136}");
				break;
			case GameDefine.NPC_FUNCTION_TYPE.MONTH_CARD:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301137}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301138}");
				break;
			}
		}
	}

	public void UpdateInfo()
	{
		if (CurData != null && curType == ShowType.activity)
		{
			ShowLabelInfo();
		}
	}

	private void ShowLabelInfo()
	{
		BtnSp.alpha = 1f;
		TitleLabel.text = string.Empty;
		InfoLabel.text = string.Empty;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
		{
			string activityID = CurData.ActivityID;
			MissionData missionDataByID = DataManager.GetMissionDataByID(activityID);
			if (missionDataByID == null)
			{
				return;
			}
			MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			MISSION_STATE missionState = missionManager.GetMissionState(activityID);
			string empty = string.Empty;
			switch (missionDataByID.Class)
			{
			case 0:
				empty = StrDictionary.GetDictionaryString("#{100168}", missionManager.GetMissionParam(activityID, 3) + 1);
				break;
			case 1:
				empty = StrDictionary.GetDictionaryString("#{100169}");
				break;
			case 2:
				empty = StrDictionary.GetDictionaryString("#{100170}");
				break;
			case 3:
			case 6:
				empty = StrDictionary.GetDictionaryString("#{100171}");
				break;
			case 4:
				empty = StrDictionary.GetDictionaryString("#{100172}");
				break;
			case 5:
				empty = StrDictionary.GetDictionaryString("#{100173}");
				break;
			case 7:
				empty = StrDictionary.GetDictionaryString("#{100200}");
				break;
			case 8:
				empty = StrDictionary.GetDictionaryString("#{100001}");
				break;
			default:
				empty = "[Need Loc]";
				break;
			}
			if (missionDataByID.Class == 0)
			{
				TitleLabel.text = StrDictionary.GetDictionaryString("#{100171}") + "[FDAE33]" + StrDictionary.GetDictionaryString(missionDataByID.TipDescribeID) + empty + "[-]";
				MissionManager.GetMissionStateLabel(missionDataByID, missionState, InfoLabel);
			}
			else if (missionDataByID.Class == 8)
			{
				if (missionState == MISSION_STATE.INVALID)
				{
					TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(missionDataByID.TimeLimitId);
					TitleLabel.text = empty + "[FDAE33]" + timeLimitMissionDataByID.MName + "[-]";
					InfoLabel.text = timeLimitMissionDataByID.MTip;
				}
				else
				{
					TitleLabel.text = empty + "[FDAE33]" + StrDictionary.GetDictionaryString(missionDataByID.TipDescribeID) + "[-]";
					MissionManager.GetMissionStateLabel(missionDataByID, missionState, InfoLabel);
				}
			}
			else
			{
				TitleLabel.text = empty + "[FDAE33]" + StrDictionary.GetDictionaryString(missionDataByID.TipDescribeID) + "[-]";
				MissionManager.GetMissionStateLabel(missionDataByID, missionState, InfoLabel);
			}
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.DAILY_COPY)
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(CurData.ActivityID);
			copyscene_info dailyCopyInfo = CurData.GetDailyCopyInfo();
			TitleLabel.text = copySceneDataById.MName;
			int num = 0;
			if (dailyCopyInfo == null)
			{
				return;
			}
			if (CurData.IsUnlock)
			{
				num = (int)dailyCopyInfo.CurNum;
				if (copySceneDataById.IsScuffleCopy || copySceneDataById.IsSingleDance)
				{
					InfoLabel.text = string.Format("{0}  {1}", StrDictionary.GetDictionaryString("#{102018}"), TimeTools.GetMinuteSecondStr(num));
				}
				else if (copySceneDataById.IsPVPMap)
				{
					InfoLabel.text = StrDictionary.GetDictionaryString("#{102081}");
				}
				else if (copySceneDataById.IsEquipCopy)
				{
					copySceneDataById = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.GetCurPlayerEquipCopyData();
					TitleLabel.text = copySceneDataById.MName;
					InfoLabel.text = string.Format("{0}  {1}/{2}", StrDictionary.GetDictionaryString("#{101508}"), num, copySceneDataById.MaxPlayNum);
				}
				else if (copySceneDataById.IsExpCopy)
				{
					copySceneDataById = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.GetCurPlayerExpCopyData();
					TitleLabel.text = copySceneDataById.MName;
					InfoLabel.text = string.Format("{0}  {1}/{2}", StrDictionary.GetDictionaryString("#{101508}"), num, copySceneDataById.MaxPlayNum);
				}
				else
				{
					InfoLabel.text = string.Format("{0}  {1}/{2}", StrDictionary.GetDictionaryString("#{101508}"), num, copySceneDataById.MaxPlayNum);
				}
			}
			else
			{
				InfoLabel.text = StrDictionary.GetDictionaryString("#{605501}", copySceneDataById.MinLevel);
				BtnSp.alpha = 0f;
			}
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT || CurData.ActivityType == GameDefine.ACTIVITY_TYPE.ESCORT)
		{
			EscortData escortDataById = DataManager.GetEscortDataById(CurData.ActivityID);
			TitleLabel.text = StrDictionary.GetDictionaryString(escortDataById.Name);
			if (CurData.IsUnlock)
			{
				activity_info activityInfo = CurData.GetActivityInfo();
				if (activityInfo != null)
				{
					int num2 = (int)activityInfo.CurNum;
					InfoLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{101508}"), num2, escortDataById.MaxPlayNum);
				}
			}
			else
			{
				InfoLabel.text = StrDictionary.GetDictionaryString("#{605501}", escortDataById.UnlockLevel);
			}
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.BAR_FIGHT)
		{
			BarFightCopyData barFightCopyDataByID = DataManager.GetBarFightCopyDataByID(CurData.ActivityID);
			TitleLabel.text = StrDictionary.GetDictionaryString(barFightCopyDataByID.Name);
			if (CurData.IsUnlock)
			{
				activity_info activityInfo2 = CurData.GetActivityInfo();
				if (activityInfo2 != null)
				{
					TimeSpan localShowTime = TimeTools.GetLocalShowTime(barFightCopyDataByID.StartTime + barFightCopyDataByID.DurationTime, playerCommonData.TimeOffset);
					TimeSpan localShowTime2 = TimeTools.GetLocalShowTime(barFightCopyDataByID.StartTime + barFightCopyDataByID.DurationTime + barFightCopyDataByID.WaitTime, playerCommonData.TimeOffset);
					InfoLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101509}"), $"{localShowTime.Hours:D2}:{localShowTime.Minutes:D2}", $"{localShowTime2.Hours:D2}:{localShowTime2.Minutes:D2}");
				}
			}
			else
			{
				InfoLabel.text = StrDictionary.GetDictionaryString("#{605501}", barFightCopyDataByID.UnlockLevel);
				BtnSp.alpha = 0f;
			}
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.CITY_DANCE)
		{
			CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(CurData.ActivityID);
			TitleLabel.text = StrDictionary.GetDictionaryString(cityDanceDataById.Name);
			if (CurData.IsUnlock)
			{
				activity_info activityInfo3 = CurData.GetActivityInfo();
				if (activityInfo3 != null)
				{
					TimeSpan localShowTime3 = TimeTools.GetLocalShowTime((int)cityDanceDataById.StartTime[0], playerCommonData.TimeOffset);
					TimeSpan localShowTime4 = TimeTools.GetLocalShowTime(cityDanceDataById.StartTime[0] + cityDanceDataById.DurationTime, playerCommonData.TimeOffset);
					InfoLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101509}"), $"{localShowTime3.Hours:D2}:{localShowTime3.Minutes:D2}", $"{localShowTime4.Hours:D2}:{localShowTime4.Minutes:D2}");
				}
			}
			else
			{
				InfoLabel.text = StrDictionary.GetDictionaryString("#{605501}", cityDanceDataById.UnlockLevel);
				BtnSp.alpha = 0f;
			}
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.SEX_MINI)
		{
			SexMiniData sexMiniDataById = DataManager.GetSexMiniDataById(CurData.ActivityID);
			TitleLabel.text = StrDictionary.GetDictionaryString(sexMiniDataById.Name);
			if (CurData.IsUnlock)
			{
				InfoLabel.text = StrDictionary.GetDictionaryString("#{102063}");
				return;
			}
			InfoLabel.text = StrDictionary.GetDictionaryString("#{605501}", sexMiniDataById.UnlockLevel);
			BtnSp.alpha = 0f;
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE)
		{
			SurviveBattleData surviveBattleDataById = DataManager.GetSurviveBattleDataById(CurData.ActivityID);
			TitleLabel.text = StrDictionary.GetDictionaryString(surviveBattleDataById.Name);
			if (CurData.IsUnlock)
			{
				activity_info activityInfo4 = CurData.GetActivityInfo();
				if (activityInfo4 != null)
				{
					List<TimeSpan> localShowTime5 = TimeTools.GetLocalShowTime(surviveBattleDataById.StartTimes, playerCommonData.TimeOffset, surviveBattleDataById.DurationTime, (int)activityInfo4.next);
					if (localShowTime5 != null && localShowTime5.Count == 2)
					{
						TimeSpan timeSpan = localShowTime5[0];
						TimeSpan timeSpan2 = localShowTime5[1];
						InfoLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101509}"), $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}", $"{timeSpan2.Hours:D2}:{timeSpan2.Minutes:D2}");
					}
					else
					{
						InfoLabel.text = string.Empty;
					}
				}
			}
			else
			{
				InfoLabel.text = StrDictionary.GetDictionaryString("#{605501}", surviveBattleDataById.UnlockLevel);
				BtnSp.alpha = 0f;
			}
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BATTLE)
		{
			GuildBattleData guildBattleDataById = DataManager.GetGuildBattleDataById(CurData.ActivityID);
			TitleLabel.text = guildBattleDataById.MName;
			if (CurData.IsUnlock)
			{
				guild_battle_info guildBattleInfo = CurData.GetGuildBattleInfo();
				if (guildBattleInfo != null)
				{
					guildBattleDataById = DataManager.GetGuildBattleDataById(guildBattleInfo.ID);
					string text = string.Empty;
					int num3 = 0;
					int num4 = 0;
					if (guildBattleInfo.state < 0 || guildBattleInfo.state >= 6)
					{
						num3 = guildBattleDataById.Week;
						num4 = guildBattleDataById.StartTime;
						text = StrDictionary.GetDictionaryString("#{105076}");
					}
					else if (guildBattleInfo.state <= 1)
					{
						num3 = guildBattleDataById.Week1;
						num4 = guildBattleDataById.StartTime1;
						text = StrDictionary.GetDictionaryString("#{105002}");
					}
					else if (guildBattleInfo.state <= 3)
					{
						num3 = guildBattleDataById.Week2;
						num4 = guildBattleDataById.StartTime2;
						text = StrDictionary.GetDictionaryString("#{105003}");
					}
					else if (guildBattleInfo.state <= 5)
					{
						num3 = guildBattleDataById.Week3;
						num4 = guildBattleDataById.StartTime3;
						text = StrDictionary.GetDictionaryString("#{105004}");
					}
					if (guildBattleInfo.state == -2)
					{
						InfoLabel.text = StrDictionary.GetDictionaryString("#{101406}");
					}
					else
					{
						TimeSpan localShowTime6 = TimeTools.GetLocalShowTime(num4, playerCommonData.TimeOffset);
						int key = (num3 - 1 + TimeTools.GetOffsetDay(num4, playerCommonData.TimeOffset) + GameDefine.WEEK_NAME.Count) % GameDefine.WEEK_NAME.Count;
						InfoLabel.text = $"{text}:{StrDictionary.GetDictionaryString(GameDefine.WEEK_NAME[key])} {localShowTime6.Hours:d2}:{localShowTime6.Minutes:d2}";
					}
					TitleLabel.text = guildBattleDataById.MName;
				}
			}
			else
			{
				int condition = DataManager.GetFunctionDataById(4077.ToString()).Condition;
				InfoLabel.text = StrDictionary.GetDictionaryString("#{605501}", condition);
				BtnSp.alpha = 0f;
			}
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BOSS)
		{
			GuildBossData guildBossDataByID = DataManager.GetGuildBossDataByID(CurData.ActivityID);
			TitleLabel.text = StrDictionary.GetDictionaryString("#{100748}");
			if (CurData.IsUnlock)
			{
				guild_boss guildBossInfo = CurData.GetGuildBossInfo();
				if (guildBossInfo != null)
				{
					guildBossDataByID = DataManager.GetGuildBossDataByID(guildBossInfo.id);
					TimeSpan localShowTime7 = TimeTools.GetLocalShowTime(guildBossDataByID.StartTime, playerCommonData.TimeOffset);
					TimeSpan localShowTime8 = TimeTools.GetLocalShowTime(guildBossDataByID.StartTime + guildBossDataByID.DurationTime, playerCommonData.TimeOffset);
					InfoLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101509}"), $"{localShowTime7.Hours:D2}:{localShowTime7.Minutes:D2}", $"{localShowTime8.Hours:D2}:{localShowTime8.Minutes:D2}");
				}
			}
			else
			{
				InfoLabel.text = StrDictionary.GetDictionaryString("#{100796}");
				BtnSp.alpha = 0f;
			}
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.WILD_BOSS)
		{
			WildBossData wildBossDataByID = DataManager.GetWildBossDataByID(CurData.ActivityID);
			TitleLabel.text = StrDictionary.GetDictionaryString("#{101519}");
			if (CurData.IsUnlock)
			{
				activity_info wildBossInfo = CurData.GetWildBossInfo();
				if (wildBossInfo != null)
				{
					wildBossDataByID = DataManager.GetWildBossDataByID(wildBossInfo.ID);
					List<TimeSpan> localShowTime9 = TimeTools.GetLocalShowTime(wildBossDataByID.StartTimes, playerCommonData.TimeOffset, wildBossDataByID.DurationTime, (int)wildBossInfo.next, wildBossInfo.State == 2);
					if (localShowTime9 != null && localShowTime9.Count == 2)
					{
						TimeSpan timeSpan3 = localShowTime9[0];
						TimeSpan timeSpan4 = localShowTime9[1];
						InfoLabel.text = string.Format("{0} {1}-{2}", StrDictionary.GetDictionaryString("#{101509}"), $"{timeSpan3.Hours:D2}:{timeSpan3.Minutes:D2}", $"{timeSpan4.Hours:D2}:{timeSpan4.Minutes:D2}");
					}
					else
					{
						InfoLabel.text = string.Empty;
					}
				}
			}
			else
			{
				InfoLabel.text = StrDictionary.GetDictionaryString("#{605501}", wildBossDataByID.LevelMin);
				BtnSp.alpha = 0f;
			}
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.TOWER)
		{
			TitleLabel.text = StrDictionary.GetDictionaryString("#{101538}");
			int condition2 = DataManager.GetFunctionDataById(4003.ToString()).Condition;
			if (CurData.IsUnlock)
			{
				tower_info playerTowerInfo = playerData.TowerData.PlayerTowerInfo;
				if (playerTowerInfo != null)
				{
					InfoLabel.text = string.Format("{0}: {1}", StrDictionary.GetDictionaryString("#{101525}"), playerTowerInfo.floor + 1);
				}
			}
			else
			{
				InfoLabel.text = StrDictionary.GetDictionaryString("#{605501}", condition2);
				BtnSp.alpha = 0f;
			}
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.RANKPVP)
		{
			TitleLabel.text = StrDictionary.GetDictionaryString("#{100106}");
			int condition3 = DataManager.GetFunctionDataById(3002.ToString()).Condition;
			if (CurData.IsUnlock)
			{
				InfoLabel.text = string.Format("{0}: {1}", StrDictionary.GetDictionaryString("#{101001}"), playerData.RankPVPData.GetRankPosStr());
				return;
			}
			InfoLabel.text = StrDictionary.GetDictionaryString("#{605501}", condition3);
			BtnSp.alpha = 0f;
		}
		else if (CurData.ActivityType == GameDefine.ACTIVITY_TYPE.SHOP_GATE)
		{
			BtnLabel.text = StrDictionary.GetDictionaryString("#{301139}");
			int condition4 = DataManager.GetFunctionDataById(4025.ToString()).Condition;
			switch ((GameDefine.SHOP_TYPE)CurData.SubType)
			{
			case GameDefine.SHOP_TYPE.DOLLAR_SHOP:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301127}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301128}");
				condition4 = DataManager.GetFunctionDataById(4025.ToString()).Condition;
				break;
			case GameDefine.SHOP_TYPE.TOOL_SHOP:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301129}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301130}");
				condition4 = DataManager.GetFunctionDataById(4021.ToString()).Condition;
				break;
			case GameDefine.SHOP_TYPE.EQUIP_SHOP:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301131}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301132}");
				condition4 = DataManager.GetFunctionDataById(4022.ToString()).Condition;
				break;
			case GameDefine.SHOP_TYPE.BIGSALE_SHOP:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301133}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301134}");
				condition4 = DataManager.GetFunctionDataById(4023.ToString()).Condition;
				break;
			case GameDefine.SHOP_TYPE.GUILD_SHOP:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301135}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301136}");
				condition4 = DataManager.GetFunctionDataById(4024.ToString()).Condition;
				break;
			case GameDefine.SHOP_TYPE.VIP_SHOP:
				TitleLabel.text = StrDictionary.GetDictionaryString("#{301137}");
				InfoLabel.text = StrDictionary.GetDictionaryString("#{301138}");
				condition4 = DataManager.GetFunctionDataById(4027.ToString()).Condition;
				break;
			}
			if (!CurData.IsUnlock)
			{
				InfoLabel.text = StrDictionary.GetDictionaryString("#{605501}", condition4);
			}
			BtnSp.alpha = 0f;
		}
	}

	public void CloseUI()
	{
		if (isShow)
		{
			isShow = false;
			PosAnima.PlayReverse();
		}
	}

	public void OnClickStartBtn()
	{
		if (curType == ShowType.activity)
		{
			OnActivityStart();
		}
		else if (curType == ShowType.shopnpc)
		{
			ShopNpcStart();
		}
	}

	public void ShopNpcStart()
	{
		CloseUI();
		if (null != Singleton<ObjManager>.Instance.MainPlayer && null != CurShopNpc)
		{
			Singleton<ObjManager>.Instance.MainPlayer.SelectTargetNPC(CurShopNpc);
		}
	}

	public void OnActivityStart()
	{
		if (!CurData.IsUnlock)
		{
			return;
		}
		CloseUI();
		switch (CurData.ActivityType)
		{
		case GameDefine.ACTIVITY_TYPE.MISSION:
			if (SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DialogMissionUI, delegate
				{
					SingletonUnity<DialogMissionUIRoot>.Instance.ResetActMissionAcceptUI(CurData.ActivityID);
				});
			}
			break;
		case GameDefine.ACTIVITY_TYPE.DAILY_COPY:
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(CurData.ActivityID);
			if (copySceneDataById.IsPVPMap)
			{
				MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(copySceneDataById.MapId);
				if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange)
				{
					SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
					enter_new_map.request request = new enter_new_map.request();
					request.mapInfoId = mapInfoDataByID.ID;
					NetLogic.GetInstance().Send<Protocol.enter_new_map>(request);
					WaitResponseUIRootLogic.OpenWaitBox(106, 10f, 0f);
				}
				break;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				if (CurData.IsNeedDailyActid())
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn((MAPTYPE)CurData.SubType, CurData.ActivityID);
				}
				else
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn((MAPTYPE)CurData.SubType);
				}
			});
			break;
		}
		case GameDefine.ACTIVITY_TYPE.GUILD_BOSS:
		case GameDefine.ACTIVITY_TYPE.GUILD_BATTLE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(CurData.ActivityType);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.ESCORT:
		case GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT:
		case GameDefine.ACTIVITY_TYPE.CITY_DANCE:
		case GameDefine.ACTIVITY_TYPE.BAR_FIGHT:
		case GameDefine.ACTIVITY_TYPE.WILD_BOSS:
		case GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(CurData.ActivityType);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.SEX_MINI:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.SEX_GAME);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.TOWER:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.INVALID, null, null, GameDefine.ACTIVITY_TYPE.TOWER);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.RANKPVP:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.ResetToPVP();
			});
			break;
		}
	}
}
