using SprotoType;
using UnityEngine;

public class NewMessageItemLogic : MonoBehaviour
{
	public enum NewMessageType
	{
		Activity,
		Team,
		Mission
	}

	public UISprite IconSp;

	public UILabel NameLabel;

	public GameObject teamIconObj;

	public UILabel teamLabel;

	private NewMessageType CurType;

	private GameDefine.ACTIVITY_TYPE ActType = GameDefine.ACTIVITY_TYPE.INVALID;

	private InviteTeamInfo CurTeamInfo;

	public UILabel BtnLabel;

	private string curMissionId;

	public void ResetInfo(GameDefine.ACTIVITY_TYPE type)
	{
		CurType = NewMessageType.Activity;
		ActType = type;
		CurTeamInfo = null;
		NGUITools.SetActive(IconSp.gameObject, state: true);
		NGUITools.SetActive(teamIconObj, state: false);
		BtnLabel.text = StrDictionary.GetDictionaryString("#{101501}");
		switch (ActType)
		{
		case GameDefine.ACTIVITY_TYPE.WILD_BOSS:
			IconSp.spriteName = "CZ_shiJieBOSS_PVE";
			NameLabel.text = StrDictionary.GetDictionaryString("#{101519}");
			break;
		case GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE:
			IconSp.spriteName = "CZ_shengCun_PVP";
			NameLabel.text = StrDictionary.GetDictionaryString("#{101517}");
			break;
		case GameDefine.ACTIVITY_TYPE.CITY_DANCE:
			IconSp.spriteName = "CZ_fuBenTuBiao_DANCE";
			NameLabel.text = StrDictionary.GetDictionaryString("#{101515}");
			break;
		case GameDefine.ACTIVITY_TYPE.BAR_FIGHT:
			IconSp.spriteName = "CZ_suiJiZuDui_PVE";
			NameLabel.text = StrDictionary.GetDictionaryString("#{101516}");
			break;
		case GameDefine.ACTIVITY_TYPE.GUILD_BOSS:
			IconSp.spriteName = "CZ_gongHuiBOSS_PVE";
			NameLabel.text = StrDictionary.GetDictionaryString("#{100748}");
			break;
		case GameDefine.ACTIVITY_TYPE.GUILD_BATTLE:
			IconSp.spriteName = "CZ_gongHuiZhan_PVP";
			NameLabel.text = StrDictionary.GetDictionaryString("#{105001}");
			break;
		case GameDefine.ACTIVITY_TYPE.GUILD_DANCE:
			IconSp.spriteName = "CZ_fuBenTuBiao_DANCE";
			NameLabel.text = StrDictionary.GetDictionaryString("#{105100}");
			break;
		case GameDefine.ACTIVITY_TYPE.GUILD_DONMINE:
			IconSp.spriteName = "CZ_gongHuiZhanLin_PVP";
			NameLabel.text = StrDictionary.GetDictionaryString("#{106033}");
			break;
		}
		IconSp.MakePixelPerfect();
	}

	public void ResetInfo(InviteTeamInfo teaminfo)
	{
		CurType = NewMessageType.Team;
		ActType = GameDefine.ACTIVITY_TYPE.INVALID;
		NGUITools.SetActive(IconSp.gameObject, state: false);
		NGUITools.SetActive(teamIconObj, state: true);
		CurTeamInfo = teaminfo;
		if (!teaminfo.IsUrgeFlag)
		{
			teamLabel.text = StrDictionary.GetDictionaryString("#{100275}", CurTeamInfo.inviteName);
			BtnLabel.text = StrDictionary.GetDictionaryString("#{100809}");
		}
		else
		{
			teamLabel.text = StrDictionary.GetDictionaryString("#{100295}", CurTeamInfo.inviteName);
			BtnLabel.text = StrDictionary.GetDictionaryString("#{101501}");
		}
	}

	public void ResetInfo(string missionid)
	{
		curMissionId = missionid;
		CurType = NewMessageType.Mission;
		ActType = GameDefine.ACTIVITY_TYPE.INVALID;
		NGUITools.SetActive(IconSp.gameObject, state: true);
		NGUITools.SetActive(teamIconObj, state: false);
		IconSp.spriteName = "CZ_lianXianRenWu";
		IconSp.MakePixelPerfect();
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionid);
		TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(missionDataByID.TimeLimitId);
		if (timeLimitMissionDataByID != null)
		{
			NameLabel.text = StrDictionary.GetDictionaryString("#{100003}", timeLimitMissionDataByID.MName);
		}
		else
		{
			NameLabel.text = string.Empty;
		}
		BtnLabel.text = StrDictionary.GetDictionaryString("#{100242}");
	}

	public void OnClickStart()
	{
		if (CurType == NewMessageType.Activity)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMessageUIRoot);
			ActJump();
		}
		else if (CurType == NewMessageType.Team)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMessageUIRoot);
			if (CurTeamInfo.IsUrgeFlag)
			{
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveTeam())
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew);
				}
			}
			else if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveTeam())
			{
				CheckTeamGoal();
			}
		}
		else if (CurType == NewMessageType.Mission)
		{
			SingletonUnity<NewMessageUIRootLogic>.Instance.DecMissionLine(curMissionId);
		}
	}

	public void CheckTeamGoal()
	{
		if (CurTeamInfo == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(CurTeamInfo.TeamGoalId))
		{
			teamAgreeInvite();
			return;
		}
		TeamData teamDataDataByID = DataManager.GetTeamDataDataByID(CurTeamInfo.TeamGoalId);
		if (teamDataDataByID == null || teamDataDataByID.GoalType == 0)
		{
			teamAgreeInvite();
			return;
		}
		CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(teamDataDataByID.CopyId);
		if (copySceneDataById.SubType == 12)
		{
			if (CheckLevel(copySceneDataById.MinLevel, copySceneDataById.MaxLevel))
			{
				teamAgreeInvite();
			}
			else if (CheckMinLevel(copySceneDataById.MinLevel))
			{
				string dictionaryString = StrDictionary.GetDictionaryString("#{102093}", copySceneDataById.MinLevel, copySceneDataById.MaxLevel);
				MessageBoxLogic.OpenOKCancelBox(dictionaryString, "#{100127}", teamAgreeInvite);
			}
			else if (CheckMaxLevel(copySceneDataById.MaxLevel))
			{
				string dictionaryString2 = StrDictionary.GetDictionaryString("#{102094}", copySceneDataById.MinLevel, copySceneDataById.MaxLevel);
				MessageBoxLogic.OpenOKCancelBox(dictionaryString2, "#{100127}", teamAgreeInvite);
			}
		}
		else if (copySceneDataById.SubType == 16)
		{
			if (CheckLevel(copySceneDataById.MinLevel, copySceneDataById.MaxLevel))
			{
				teamAgreeInvite();
			}
			else if (CheckMinLevel(copySceneDataById.MinLevel))
			{
				string dictionaryString3 = StrDictionary.GetDictionaryString("#{102096}", copySceneDataById.MinLevel, copySceneDataById.MaxLevel);
				MessageBoxLogic.OpenOKCancelBox(dictionaryString3, "#{100127}", teamAgreeInvite);
			}
			else if (CheckMaxLevel(copySceneDataById.MaxLevel))
			{
				string dictionaryString4 = StrDictionary.GetDictionaryString("#{102097}", copySceneDataById.MinLevel, copySceneDataById.MaxLevel);
				MessageBoxLogic.OpenOKCancelBox(dictionaryString4, "#{100127}", teamAgreeInvite);
			}
		}
	}

	public bool CheckLevel(int minLevel, int maxLevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minLevel, maxLevel);
	}

	public bool CheckMinLevel(int minLevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level < minLevel;
	}

	public bool CheckMaxLevel(int maxLevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level > maxLevel;
	}

	public void teamAgreeInvite()
	{
		if (CurTeamInfo != null)
		{
			ret_invite_join_team.request request = new ret_invite_join_team.request();
			request.ok = 1L;
			request.id = CurTeamInfo.characterId;
			NetLogic.GetInstance().Send<Protocol.ret_invite_join_team>(request);
			if (CurTeamInfo.teamId != -1)
			{
				req_join_team.request request2 = new req_join_team.request();
				request2.teamid = CurTeamInfo.teamId;
				request2.isapply = true;
				NetLogic.GetInstance().Send<Protocol.req_join_team>(request2);
			}
		}
	}

	public void ActJump()
	{
		switch (ActType)
		{
		case GameDefine.ACTIVITY_TYPE.WILD_BOSS:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.WILD_BOSS);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.CITY_DANCE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.CITY_DANCE);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.BAR_FIGHT:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.BAR_FIGHT);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.GUILD_BOSS:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_BOSS);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.GUILD_BATTLE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_BATTLE);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.GUILD_DANCE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_DANCE);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.GUILD_DONMINE:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_DONMINE);
			});
			break;
		case GameDefine.ACTIVITY_TYPE.SEX_MINI:
		case GameDefine.ACTIVITY_TYPE.DAILY_COPY:
		case GameDefine.ACTIVITY_TYPE.MISSION:
		case GameDefine.ACTIVITY_TYPE.TOWER:
		case GameDefine.ACTIVITY_TYPE.RANKPVP:
		case GameDefine.ACTIVITY_TYPE.DOMIN:
		case GameDefine.ACTIVITY_TYPE.FIRST_GUILD_DANCE:
		case GameDefine.ACTIVITY_TYPE.MISSION_TIMEOUT:
			break;
		}
	}
}
