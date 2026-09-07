using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class MissionManager
{
	private bool LocalTestFlag;

	private CurMissionDictionary mCurMissionDictionary = new CurMissionDictionary();

	private MissionData curDailyMissionData;

	private MissionData curMainMissionData;

	private List<string> mFunctionMissionIdList;

	public MissionManager()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(LevelUpMissionCheck));
	}

	public void SetFunctionMissionIdList(List<string> idList)
	{
		mFunctionMissionIdList = idList;
	}

	public void RemoveUnLockFunctionMission(string[] idList)
	{
		if (mFunctionMissionIdList == null || mFunctionMissionIdList.Count <= 0)
		{
			return;
		}
		for (int num = idList.Length - 1; num > -1; num--)
		{
			for (int num2 = mFunctionMissionIdList.Count - 1; num2 > -1; num2--)
			{
				if (mFunctionMissionIdList[num2].Equals(idList[num]))
				{
					mFunctionMissionIdList.RemoveAt(num2);
				}
			}
		}
	}

	public string GetLastMainMissionId()
	{
		return mCurMissionDictionary.LastMainMissionId.ToString();
	}

	public long GetLastMainMissionIdLong()
	{
		return mCurMissionDictionary.LastMainMissionId;
	}

	public Dictionary<string, CurMission> GetCurrentMissionDictionary()
	{
		return mCurMissionDictionary.CurMissionDic;
	}

	public List<CurMission> GetCurMissionList()
	{
		return new List<CurMission>(mCurMissionDictionary.CurMissionDic.Values);
	}

	public bool IsCurTargetNpc(string npcId)
	{
		return mCurMissionDictionary.CurTargetNpcIdList.Contains(npcId);
	}

	public bool IsCurMissionNeedNpc(ObjNPC objnpc)
	{
		List<CurMission> curMissionList = GetCurMissionList();
		for (int i = 0; i < curMissionList.Count; i++)
		{
			if (curMissionList[i].MissionState == MISSION_STATE.ACCEPTED)
			{
				MissionData missionDataByID = DataManager.GetMissionDataByID(curMissionList[i].MissionId);
				if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MASSACRE_NPC)
				{
					return true;
				}
				if ((missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER_DROP || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER_DROP || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC) && objnpc.NPCData.ID.Equals(missionDataByID.Target))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsHaveDestroyCarMission()
	{
		List<CurMission> curMissionList = GetCurMissionList();
		for (int i = 0; i < curMissionList.Count; i++)
		{
			if (curMissionList[i].MissionState == MISSION_STATE.ACCEPTED)
			{
				MissionData missionDataByID = DataManager.GetMissionDataByID(curMissionList[i].MissionId);
				if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.DESTROY_CAR)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool isCurMissionTypeEnable(MISSION_LOGICTYPE type)
	{
		return true;
	}

	public bool IsCurCompleteNpc(string npcId)
	{
		return mCurMissionDictionary.CurCompleteNpcIdList.Contains(npcId);
	}

	private void AcceptDailyMissionData(MissionData data)
	{
		if (data.Class == 0 || data.Class == 6)
		{
			curDailyMissionData = data;
			AutoDailyMissionCheck();
		}
	}

	public void AutoDailyMissionCheck()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsOpenAutoCombat && curDailyMissionData != null && (curDailyMissionData.Class == 0 || curDailyMissionData.Class == 6))
		{
			SimulationClickMission(curDailyMissionData);
		}
	}

	public void AcceptMainMissionCheck(MissionData data)
	{
		if (data.Class == 1)
		{
			curMainMissionData = data;
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level >= curMainMissionData.MinLv)
			{
				AutoMissionCheck(curMainMissionData);
			}
		}
	}

	public void AutoMissionCheck(MissionData curCheckMission)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsOpenAutoCombat && curCheckMission != null && curCheckMission.Class == 1)
		{
			SimulationClickMission(curCheckMission);
		}
	}

	private void SimulationClickMission(MissionData mCurSelectMissionData)
	{
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer != null)
		{
			mainPlayer.SkillLogic.BreakCurSkill();
		}
		MissionFindPath(mCurSelectMissionData);
	}

	public bool IsMissionAcceptable(string missionId)
	{
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID == null)
		{
			return false;
		}
		if (IsMissionFull())
		{
			return false;
		}
		if (IsMissionAccepted(missionId))
		{
			return false;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.Level < missionDataByID.MinLv)
		{
			return false;
		}
		if (playerData.Level < missionDataByID.DisplayLv)
		{
			return false;
		}
		if (missionDataByID.Class == 8)
		{
			if (mCurMissionDictionary.GetCurMissionByClassType(MISSION_CLASS_TYPE.TIME_LIMIT) != null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(missionDataByID.TimeLimitId))
			{
				return false;
			}
			TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(missionDataByID.TimeLimitId);
			if (timeLimitMissionDataByID == null)
			{
				return false;
			}
			if (IsMissionCompleted(missionId))
			{
				return false;
			}
			if (!string.IsNullOrEmpty(missionDataByID.PreID) && !IsMissionCompleted(missionDataByID.PreID))
			{
				return false;
			}
			if (missionId.Equals(timeLimitMissionDataByID.MissionList[0]))
			{
				if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsTimeMissionCanAccept(missionId))
				{
					return false;
				}
				for (int i = 0; i < timeLimitMissionDataByID.MissionList.Count; i++)
				{
					if (IsMissionAccepted(timeLimitMissionDataByID.MissionList[i]))
					{
						return false;
					}
				}
			}
			return true;
		}
		if (missionDataByID.Class == 1 && mCurMissionDictionary.GetCurMissionByClassType(MISSION_CLASS_TYPE.MAIN) != null)
		{
			return false;
		}
		if (missionDataByID.Repeat == 0 && IsMissionCompleted(missionId))
		{
			return false;
		}
		if (!string.IsNullOrEmpty(missionDataByID.PreID) && !IsMissionCompleted(missionDataByID.PreID))
		{
			return false;
		}
		if (mFunctionMissionIdList.Contains(missionId))
		{
			return false;
		}
		if (missionDataByID.Class == 4 || missionDataByID.Class == 5)
		{
			if (playerData.ActivityData.CurActivityDataDic != null)
			{
				activity_info activity_info = null;
				activity_info = ((missionDataByID.Class != 4) ? playerData.ActivityData.GetActivityInfoByType(2) : playerData.ActivityData.GetActivityInfoByType(1));
				if (activity_info == null || activity_info.CurNum <= 0)
				{
					return false;
				}
			}
			if (missionDataByID.Class == 4)
			{
				if (mCurMissionDictionary.GetCurMissionByClassType(MISSION_CLASS_TYPE.ROBBERY) != null)
				{
					return false;
				}
			}
			else if (missionDataByID.Class == 5 && mCurMissionDictionary.GetCurMissionByClassType(MISSION_CLASS_TYPE.ESCORT) != null)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsMissionFull()
	{
		return mCurMissionDictionary.IsMissionFull();
	}

	public bool IsMissionCompleted(string missionId)
	{
		return mCurMissionDictionary.IsMissionCompleted(missionId);
	}

	public bool IsMissionAccepted(string missionId)
	{
		return mCurMissionDictionary.IsMissionAccepted(missionId);
	}

	private bool AddMission(string missionId, long serverTime)
	{
		if (mCurMissionDictionary.AddMission(missionId, serverTime))
		{
			if (SingletonUnity<MissionTeamTipLogic>.Exists)
			{
				SingletonUnity<MissionTeamTipLogic>.Instance.AddMission(missionId);
			}
			return true;
		}
		return false;
	}

	private bool RemoveMission(string missionId)
	{
		if (mCurMissionDictionary.RemoveMission(missionId))
		{
			if (SingletonUnity<MissionTeamTipLogic>.Exists)
			{
				SingletonUnity<MissionTeamTipLogic>.Instance.RemoveMission(missionId);
			}
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.MissionCheckMoveTarget(missionId);
			return true;
		}
		return false;
	}

	private bool SetMissionComplete(string missionId)
	{
		if (mCurMissionDictionary.SetMissionComplete(missionId))
		{
			if (SingletonUnity<MissionTeamTipLogic>.Exists)
			{
				SingletonUnity<MissionTeamTipLogic>.Instance.UpdateMission(missionId);
			}
			return true;
		}
		return false;
	}

	public void SetMissionParam(string missionId, int paramIndex, long val)
	{
		mCurMissionDictionary.SetMissionParam(missionId, paramIndex, val);
		if (SingletonUnity<MissionTeamTipLogic>.Exists)
		{
			SingletonUnity<MissionTeamTipLogic>.Instance.UpdateMission(missionId);
		}
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.ARRIVE_TARGET)
		{
			MoveTargetMissionData moveTargetMissionDataById = DataManager.GetMoveTargetMissionDataById(missionDataByID.LogicID);
			if (moveTargetMissionDataById.TargetNum > val && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager != null)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.UpdateArriveTargetPoint();
			}
		}
	}

	public long GetMissionParam(string missionId, int paramIndex)
	{
		return mCurMissionDictionary.GetMissionParam(missionId, paramIndex);
	}

	public bool SetMissionState(string missionId, MISSION_STATE state, long changeTime)
	{
		if (!mCurMissionDictionary.SetMissionState(missionId, state, changeTime))
		{
			return false;
		}
		UpdateMissionUI(missionId);
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID.ShowStoryState == (int)state)
		{
			StoryDialogRootLogic.ShowStory(missionDataByID.StoryID, DataManager.GetNpcDataByID(missionDataByID.Submit));
		}
		if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.STORY && missionDataByID.IsMultiMission != 1 && state == MISSION_STATE.COMPLETE && missionDataByID.Class != 6)
		{
			Singleton<DialogManager>.Instance.ShowMissionDialogUI(missionDataByID.ID);
		}
		else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.ESCORT && state != MISSION_STATE.COMPLETE)
		{
		}
		if (missionDataByID.Class == 8 && state == MISSION_STATE.COMPLETE)
		{
			CompleteMission(missionId);
			if (!string.IsNullOrEmpty(missionDataByID.NextID))
			{
				AcceptMission(missionDataByID.NextID);
			}
		}
		else if (missionDataByID.IsMultiMission == 1 && state == MISSION_STATE.COMPLETE)
		{
			CompleteMission(missionId);
			AcceptMission(missionDataByID.NextID);
		}
		if ((missionDataByID.Class == 0 || missionDataByID.Class == 6 || missionDataByID.Class == 7) && state == MISSION_STATE.COMPLETE)
		{
			complete_mission.request request = new complete_mission.request();
			request.missionId = missionId;
			NetLogic.GetInstance().Send<Protocol.complete_mission>(request);
		}
		if (state == MISSION_STATE.COMPLETE && (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.ARRIVE_TARGET || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.IMPACT_NPC))
		{
			ClickMissionAction(missionDataByID.ID);
		}
		if (state == MISSION_STATE.COMPLETE && missionDataByID.Class == 1)
		{
			if (missionDataByID.IsMultiMission != 1 && (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.COLLECTITEM || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER_DROP || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.SURVEY || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER_DROP || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC))
			{
				AcceptMainMissionCheck(missionDataByID);
			}
			SetFirstSceneMissionTarget(missionId);
		}
		if (state != MISSION_STATE.ACCEPTED && missionDataByID.MissionLogicType != MISSION_LOGICTYPE.DELIVERY)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager?.InitCurAvailableMissionList();
		}
		if (missionDataByID.Class == 2 && state == MISSION_STATE.COMPLETE && string.IsNullOrEmpty(missionDataByID.Submit))
		{
			CompleteMission(missionId);
		}
		MissionTipRootLogic.ShowMissionTips(missionId, state);
		if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC && state == MISSION_STATE.COMPLETE)
		{
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			sceneManager.UpdateKillTargetMission(sceneManager.CurrentMapInofData.ID);
		}
		else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.TARGET_ROB_CAR && state == MISSION_STATE.COMPLETE)
		{
			SceneManager sceneManager2 = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			sceneManager2.UpdateTargetCarMission(sceneManager2.CurrentMapInofData.ID);
		}
		return true;
	}

	public MISSION_STATE GetMissionState(string missionId)
	{
		return mCurMissionDictionary.GetMissionState(missionId);
	}

	public long GetMissionChangeTime(string missionId)
	{
		return mCurMissionDictionary.GetMissionChangeTime(missionId);
	}

	public void AcceptMission(string missionId)
	{
		if (!string.IsNullOrEmpty(missionId) && !IsMissionAccepted(missionId))
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
			if (GameManager.OnLineState)
			{
				accept_mission.request request = new accept_mission.request();
				request.missionId = missionId;
				NetLogic.GetInstance().Send<Protocol.accept_mission>(request);
			}
		}
	}

	public void AcceptMission(string missionId, string onlineid)
	{
		if (string.IsNullOrEmpty(missionId) || IsMissionAccepted(missionId))
		{
			return;
		}
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (GameManager.OnLineState)
		{
			accept_mission.request request = new accept_mission.request();
			request.missionId = missionId;
			if (!string.IsNullOrEmpty(onlineid))
			{
				request.onlineId = onlineid;
			}
			NetLogic.GetInstance().Send<Protocol.accept_mission>(request);
		}
	}

	public void AcceptMissionSucess(send_daily_mission.request request)
	{
		if (AcceptMissionSuccess(request.mission.missionId, PlayerCommonData.GetServerTime(), refresh: false))
		{
			SetMissionParam(request.mission.missionId, 0, request.mission.parm[0]);
			SetMissionParam(request.mission.missionId, 1, request.mission.parm[1]);
			SetMissionParam(request.mission.missionId, 2, request.mission.parm[2]);
			SetMissionParam(request.mission.missionId, 3, request.mission.parm[3]);
			if (SingletonUnity<MissionTeamTipLogic>.Exists)
			{
				SingletonUnity<MissionTeamTipLogic>.Instance.UpdateMission(request.mission.missionId);
			}
			MissionData missionDataByID = DataManager.GetMissionDataByID(request.mission.missionId);
			AcceptDailyMissionData(missionDataByID);
		}
	}

	public bool AcceptMissionSuccess(ownmission mission, long serverTime, bool refresh = true)
	{
		if (AcceptMissionSuccess(mission.missionId, serverTime, refresh))
		{
			if (mission.HasParm)
			{
				for (int i = 0; i < mission.parm.Count; i++)
				{
					SetMissionParam(mission.missionId, i, mission.parm[i]);
				}
			}
			return true;
		}
		return false;
	}

	public bool AcceptMissionSuccess(string missionId, long serverTime, bool refresh = true)
	{
		if (!AddMission(missionId, serverTime))
		{
			return false;
		}
		if (!SetMissionState(missionId, MISSION_STATE.ACCEPTED, serverTime))
		{
			return false;
		}
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID != null)
		{
			SetFirstSceneMissionTarget(missionId);
			if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.DELIVERY)
			{
				if (!SetMissionState(missionId, MISSION_STATE.COMPLETE, serverTime))
				{
					return false;
				}
			}
			else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MASSACRE_NPC || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.IMPACT_NPC || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.ROB_CAR || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.DESTROY_CAR)
			{
				SetMissionParam(missionId, 0, 0L);
			}
			else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.ARRIVE_TARGET)
			{
				SetMissionParam(missionId, 0, 0L);
			}
			else if (missionDataByID.MissionLogicType != 0)
			{
				if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.SURVEY)
				{
					SetMissionParam(missionId, 0, 0L);
					SurveyMissionData surveyMissionDataById = DataManager.GetSurveyMissionDataById(missionDataByID.LogicID);
					if (surveyMissionDataById.SceneID.Equals(SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr))
					{
						Singleton<SurveyItemManager>.Instance.InitSurveyItem(SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr);
					}
				}
				else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.COLLECTITEM)
				{
					SetMissionParam(missionId, 0, 0L);
				}
				else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER_DROP || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER_DROP)
				{
					SetMissionParam(missionId, 0, 0L);
				}
				else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.ESCORT)
				{
					SetMissionParam(missionId, 0, 0L);
				}
				else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
				{
					SetMissionParam(missionId, 0, 0L);
				}
				else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.OPERATION)
				{
					SetMissionParam(missionId, 0, 0L);
				}
				else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.DOWNLOAD_MISSION)
				{
					if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload && !SetMissionState(missionId, MISSION_STATE.COMPLETE, serverTime))
					{
						return false;
					}
				}
				else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC)
				{
					SetMissionParam(missionId, 0, 0L);
					SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
					sceneManager.UpdateKillTargetMission(sceneManager.CurrentMapInofData.ID);
				}
				else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.TARGET_ROB_CAR)
				{
					SetMissionParam(missionId, 0, 0L);
					SceneManager sceneManager2 = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
					sceneManager2.UpdateTargetCarMission(sceneManager2.CurrentMapInofData.ID);
				}
			}
			if (refresh && SingletonUnity<MissionTeamTipLogic>.Exists)
			{
				SingletonUnity<MissionTeamTipLogic>.Instance.UpdateMission(missionId);
			}
			SceneManager sceneManager3 = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			if (sceneManager3 != null)
			{
				sceneManager3.MapActivityManager.AcceptMapActivity(missionId);
				sceneManager3.InitCurAvailableMissionList();
			}
			AcceptMissionFlurry(missionDataByID);
			return true;
		}
		return false;
	}

	private void AcceptMissionFlurry(MissionData tempmissiondata)
	{
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		int num = int.Parse(tempmissiondata.ID);
		num--;
		int num2 = num / 5 * 5 + 1;
		int num3 = num / 5 * 5 + 5;
		if (tempmissiondata.Class == 1)
		{
			num = int.Parse(tempmissiondata.ID);
			if (num > 1000 && num < 2000)
			{
				instance.FlurryLogEventMap("Mission", "MainLineNew", $"accept_{tempmissiondata.ID}");
				return;
			}
			if (num <= 37)
			{
				instance.FlurryLogEventMap("Mission", "MainLine37", $"accept_{tempmissiondata.ID}");
				return;
			}
			num -= 38;
			num2 = num / 5 * 5 + 38;
			num3 = num / 5 * 5 + 42;
			instance.FlurryLogEventMap("Mission", "MainLine", $"accept_{num2}_{num3}");
		}
		else if (tempmissiondata.Class == 0)
		{
			instance.FlurryLogEventMap("Mission", "DailyLine", $"accept_{num2}_{num3}");
		}
		else if (tempmissiondata.Class == 2)
		{
			instance.FlurryLogEventMap("Mission", "SideLine", $"accept_{num2}_{num3}");
		}
		else if (tempmissiondata.Class == 4)
		{
			instance.FlurryLogEventMap("TimeActivity", "activity_1", "accept");
		}
		else if (tempmissiondata.Class == 5)
		{
			instance.FlurryLogEventMap("TimeActivity", "activity_2", "accept");
		}
		else if (tempmissiondata.Class == 7)
		{
			instance.FlurryLogEventMap("Mission", "Online", "accept" + tempmissiondata.ID);
		}
		else if (tempmissiondata.Class == 8)
		{
			instance.FlurryLogEventMap("Mission", "TimeLimit", "accept" + tempmissiondata.ID);
		}
	}

	public bool CompleteMission(string missionId)
	{
		if (!IsMissionAccepted(missionId))
		{
			return false;
		}
		if (GetMissionState(missionId) != MISSION_STATE.COMPLETE)
		{
			return false;
		}
		if (GameManager.OnLineState)
		{
			complete_mission.request request = new complete_mission.request();
			request.missionId = missionId;
			MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
			if (missionDataByID != null && missionDataByID.Class == 8)
			{
				request.parm = missionDataByID.GetTimeLimitMissionStar();
			}
			NetLogic.GetInstance().Send<Protocol.complete_mission>(request);
			return true;
		}
		return false;
	}

	public bool CompleteMissionSuccess(string missionId)
	{
		MissionData curMissionData = DataManager.GetMissionDataByID(missionId);
		long lastMissionParam = GetMissionParam(missionId, 7);
		if (curMissionData.Class == 1)
		{
			if (!SetMissionComplete(missionId))
			{
				return false;
			}
		}
		else if (curMissionData.Class == 2 || curMissionData.Class == 8)
		{
			SetMissionComplete(missionId);
		}
		if (curMissionData.Class == 7)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
			{
				OnlineMissionData onlineMissionDataByID = DataManager.GetOnlineMissionDataByID(GetMissionParam(missionId, 1).ToString());
				if (onlineMissionDataByID != null)
				{
					SingletonUnity<MissionPassShowRootLogic>.Instance.ResetSideMissionReward(onlineMissionDataByID.ShowReward);
				}
			});
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.IsHaveDailyCopyTipsBytype(MAPTYPE.SCUFFLE_AREA_1))
			{
				SingletonUnity<UIManager>.Instance.CloseAllPOPUI();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.SCUFFLE_AREA_1);
				});
			}
		}
		RemoveMission(missionId);
		UpdateMissionUI(missionId);
		if (curMissionData.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC)
		{
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			sceneManager.UpdateKillTargetMission(sceneManager.CurrentMapInofData.ID);
		}
		else if (curMissionData.MissionLogicType == MISSION_LOGICTYPE.TARGET_ROB_CAR)
		{
			SceneManager sceneManager2 = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			sceneManager2.UpdateTargetCarMission(sceneManager2.CurrentMapInofData.ID);
		}
		if (curMissionData != null)
		{
			if (curMissionData.Class == 1 || curMissionData.Class == 2)
			{
				if (string.IsNullOrEmpty(curMissionData.Submit) && curMissionData.IsMultiMission == 0)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
					{
						string text = string.Empty;
						PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
						if (playerData.Profession == PROFESSION_TYPE.XD)
						{
							text = curMissionData.XDShowID;
						}
						else if (playerData.Profession == PROFESSION_TYPE.QJ)
						{
							text = curMissionData.QJShowID;
						}
						else if (playerData.Profession == PROFESSION_TYPE.NQS)
						{
							text = curMissionData.NQSShowID;
						}
						ShowRewardData showRewardDataByID = DataManager.GetShowRewardDataByID(text);
						if (showRewardDataByID == null)
						{
							SingletonUnity<MissionPassShowRootLogic>.Instance.Reset(missionId);
						}
						else
						{
							SingletonUnity<MissionPassShowRootLogic>.Instance.ResetSideMissionReward(text);
						}
					});
				}
				else
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
					{
						SingletonUnity<MissionPassShowRootLogic>.Instance.Reset(missionId);
					});
				}
				if (!string.IsNullOrEmpty(curMissionData.NextID))
				{
					if (curMissionData.IsMultiMission == 0)
					{
						MissionData missionDataByID = DataManager.GetMissionDataByID(curMissionData.NextID);
						if (missionDataByID != null && !UIManager.IsUnlockTutorialEnable())
						{
							Singleton<DialogManager>.Instance.ShowMissionDialogUI(curMissionData.NextID);
						}
					}
					AcceptMission(curMissionData.NextID);
				}
				else
				{
					SingletonDontDestoryUnity<GameManager>.Instance.SceneManager?.InitCurAvailableMissionList();
				}
				if (!string.IsNullOrEmpty(curMissionData.NextSideID))
				{
					for (int i = 0; i < curMissionData.NextSideIDList.Length; i++)
					{
						AcceptMission(curMissionData.NextSideIDList[i]);
					}
				}
			}
			else if (curMissionData.Class == 8)
			{
				TimeLimitMissionData tlData = DataManager.GetTimeLimitMissionDataByID(curMissionData.TimeLimitId);
				if (tlData != null)
				{
					if (missionId.Equals(tlData.MissionList[tlData.MissionList.Count - 1]))
					{
						SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
						{
							SingletonUnity<MissionPassShowRootLogic>.Instance.ResetTimeLimitMissionPassRoot(tlData, tlData.LimitTime - (PlayerCommonData.GetServerTime() - lastMissionParam));
						});
					}
					else
					{
						SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
						{
							SingletonUnity<MissionPassShowRootLogic>.Instance.Reset(missionId);
						});
					}
				}
				SingletonDontDestoryUnity<GameManager>.Instance.SceneManager?.InitCurAvailableMissionList();
			}
			else if (curMissionData.Class == 4)
			{
				SingletonUnity<FunctionBtnRootLogic>.Instance.DisableEscortBtn();
				if (Singleton<ObjManager>.Instance.MainPlayer != null)
				{
					Singleton<ObjManager>.Instance.MainPlayer.LeaveTeamFollow();
				}
			}
			else if (curMissionData.Class == 6)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.SceneManager?.InitCurAvailableMissionList();
			}
			MissionCompleteFlurry(curMissionData);
		}
		curDailyMissionData = null;
		return true;
	}

	private void MissionCompleteFlurry(MissionData tempmissiondata)
	{
		int num = int.Parse(tempmissiondata.ID);
		num--;
		int num2 = num / 5 * 5 + 1;
		int num3 = num / 5 * 5 + 5;
		if (tempmissiondata.Class == 1)
		{
			if (num > 1000 && num < 2000)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Mission", "MainLineNew", $"complete_{tempmissiondata.ID}");
			}
			else if (int.Parse(tempmissiondata.ID) <= 37)
			{
				if (num == 0)
				{
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Mission", "MainLine37", $"accept_{tempmissiondata.ID}");
				}
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Mission", "MainLine37", $"complete_{tempmissiondata.ID}");
			}
			else
			{
				num = int.Parse(tempmissiondata.ID);
				num -= 38;
				num2 = num / 5 * 5 + 38;
				num3 = num / 5 * 5 + 42;
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Mission", "MainLine", $"complete_{num2}_{num3}");
			}
		}
		else if (tempmissiondata.Class == 0)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Mission", "DailyLine", $"complete_{num2}_{num3}");
		}
		else if (tempmissiondata.Class == 2)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Mission", "SideLine", $"complete_{num2}_{num3}");
		}
		else if (tempmissiondata.Class == 4)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "activity_1", "finish");
		}
		else if (tempmissiondata.Class == 5)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "activity_2", "finish");
		}
		else if (tempmissiondata.Class == 7)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Mission", "Online", "complete" + tempmissiondata.ID);
		}
		else if (tempmissiondata.Class == 8)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Mission", "TimeLimit", "complete" + tempmissiondata.ID);
		}
	}

	public bool AbandonMission(string missionId, bool isForced = false)
	{
		if (!IsMissionAccepted(missionId))
		{
			return false;
		}
		abandon_mission.request request = new abandon_mission.request();
		request.missionId = missionId;
		if (isForced)
		{
			request.parm = 1L;
		}
		NetLogic.GetInstance().Send<Protocol.abandon_mission>(request);
		Debug.Log("AbandonMission :: " + missionId);
		return true;
	}

	public bool AbandonMissionSuccess(string missionId)
	{
		if (!RemoveMission(missionId))
		{
			return false;
		}
		UpdateMissionUI(missionId);
		MissionAbandonMissionFlurry(missionId);
		SingletonDontDestoryUnity<GameManager>.Instance.SceneManager?.InitCurAvailableMissionList();
		return true;
	}

	private void MissionAbandonMissionFlurry(string missionId)
	{
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		int num = int.Parse(missionDataByID.ID);
		num--;
		int num2 = num / 5 * 5 + 1;
		int num3 = num / 5 * 5 + 5;
		if (missionDataByID.Class == 1)
		{
			return;
		}
		if (missionDataByID.Class == 0)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Mission", "DailyLine", $"abandon_{num2}_{num3}");
		}
		else if (missionDataByID.Class != 2)
		{
			if (missionDataByID.Class == 4)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "activity_1", "abandon");
			}
			else if (missionDataByID.Class == 5)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "activity_2", "abandon");
			}
			else if (missionDataByID.Class == 7)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Mission", "Online", "abandon" + missionDataByID.ID);
			}
			else if (missionDataByID.Class == 8)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Mission", "TimeLimit", "abandon" + missionDataByID.ID);
			}
		}
	}

	public List<string> GetAllMissionId()
	{
		return new List<string>(mCurMissionDictionary.CurMissionDic.Keys);
	}

	public void SyncMissionList(sync_mission.request request)
	{
		mCurMissionDictionary.Reset();
		List<ownmission> list = new List<ownmission>(request.missions.Values);
		bool flag = false;
		string empty = string.Empty;
		for (int i = 0; i < list.Count; i++)
		{
			string missionId = list[i].missionId;
			MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
			if (missionDataByID == null)
			{
				continue;
			}
			if (AddMission(missionId, list[i].parm[7]))
			{
				mCurMissionDictionary.SetMissionState(missionId, (MISSION_STATE)list[i].missionstate, list[i].parm[7]);
				List<long> parm = list[i].parm;
				for (int j = 0; j < parm.Count; j++)
				{
					SetMissionParam(missionId, j, parm[j]);
				}
			}
			if (missionDataByID.Class == 4)
			{
				ObjManager instance = Singleton<ObjManager>.Instance;
				ObjCharacter objCharacter = instance.FindObjInScene(GetMissionParam(missionId, 1));
				if (objCharacter != null)
				{
					instance.RemoveFromTargetCampList(objCharacter);
					objCharacter.AttributeData.Camp = GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC;
				}
			}
			if (missionDataByID.Class == 1)
			{
				flag = true;
				empty = missionDataByID.ID;
			}
		}
		if (request.HasLast_missionId)
		{
			mCurMissionDictionary.LastMainMissionId = int.Parse(request.last_missionId);
		}
		if (!flag && request.HasLast_missionId)
		{
			MissionData missionDataByID2 = DataManager.GetMissionDataByID(request.last_missionId);
			if (missionDataByID2 != null)
			{
				AcceptMission(missionDataByID2.NextID);
			}
		}
		if (request.HasSidedone_mission)
		{
			mCurMissionDictionary.MissionCompleteFlag = request.sidedone_mission;
		}
		Singleton<SurveyItemManager>.Instance.InitSurveyItem(SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr);
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		List<string> list2 = null;
		for (int k = 1; k <= level; k++)
		{
			list2 = DataManager.GetLvAutoAcceptMissionListByLevel(k);
			if (list2 == null)
			{
				continue;
			}
			for (int l = 0; l < list2.Count; l++)
			{
				if (IsMissionAcceptable(list2[l]))
				{
					AcceptMission(list2[l]);
				}
			}
		}
	}

	public void StopAutoMoveToMission()
	{
		SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath.Finish();
	}

	public void ContinueAutoMoveToMission()
	{
		AutoSearchPathManager autoSearchPath = SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath;
		if (autoSearchPath.CurPath.PathPointList.Count > 0 && SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr.Equals(autoSearchPath.CurPath.PathPointList[0].SceneId))
		{
			Vector3 pos = new Vector3(autoSearchPath.CurPath.PathPointList[0].PosX, SceneManager.GetHitHeight(autoSearchPath.CurPath.PathPointList[0].PosX, autoSearchPath.CurPath.PathPointList[0].PosZ), autoSearchPath.CurPath.PathPointList[0].PosZ);
			Singleton<ObjManager>.Instance.MainPlayer.MoveTo(pos, Singleton<ObjManager>.Instance.MainPlayer.GetStopDistance(), MoveToNextPoint);
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.SetMoveTarget(pos, autoSearchPath.CurMissionId);
		}
		else if (autoSearchPath.CurPath.PathPointList.Count > 1 && autoSearchPath.CurPath.PathPointList[0].SceneId.Equals(LoadingWindow.preSceneId) && autoSearchPath.CurPath.PathPointList[1].SceneId.Equals(SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr))
		{
			MoveToNextPoint(null);
		}
		else
		{
			StopAutoMoveToMission();
		}
	}

	public void MissionFindPath(string missionId)
	{
		if (!string.IsNullOrEmpty(missionId))
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
			MissionFindPath(missionDataByID);
		}
	}

	public void MissionFindPath(MissionData missionData)
	{
		if (missionData == null)
		{
			return;
		}
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		if ((bool)Singleton<ObjManager>.Instance.MainPlayer && instance.SceneManager.IsCopyScene() && !instance.SceneManager.IsTutorialScene())
		{
			return;
		}
		Vector3 vector = default(Vector3);
		string text = string.Empty;
		string empty = string.Empty;
		switch (GetMissionState(missionData.ID))
		{
		case MISSION_STATE.INVALID:
		case MISSION_STATE.FAIL:
		{
			if (missionData.Class == 0)
			{
				return;
			}
			text = missionData.AcceptMapId;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			Vector3 nPCPosInMonsterData2 = DataManager.GetNPCPosInMonsterData(text, missionData.Accept);
			vector.x = nPCPosInMonsterData2.x;
			vector.z = nPCPosInMonsterData2.z;
			empty = missionData.Accept;
			break;
		}
		case MISSION_STATE.ACCEPTED:
		{
			if (missionData.MissionLogicType == MISSION_LOGICTYPE.SURVEY)
			{
				SurveyMissionData surveyMissionDataById = DataManager.GetSurveyMissionDataById(missionData.LogicID);
				text = surveyMissionDataById.SceneID;
				vector.x = surveyMissionDataById.PosX;
				vector.z = surveyMissionDataById.PosZ;
				vector.y = SceneManager.GetHitHeight(vector.x, vector.z);
				break;
			}
			if (missionData.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
			{
				MultiDeliveryMissionData curMultiDeliveryTargetData = GetCurMultiDeliveryTargetData(missionData.ID);
				if (curMultiDeliveryTargetData != null)
				{
					text = curMultiDeliveryTargetData.TargetMapId;
					if (string.IsNullOrEmpty(text))
					{
						return;
					}
					Vector3 nPCPosInMonsterData3 = DataManager.GetNPCPosInMonsterData(text, curMultiDeliveryTargetData.TargetNpcId);
					vector.x = nPCPosInMonsterData3.x;
					vector.z = nPCPosInMonsterData3.z;
					vector.y = SceneManager.GetHitHeight(nPCPosInMonsterData3);
					empty = curMultiDeliveryTargetData.TargetNpcId;
				}
				break;
			}
			if (missionData.MissionLogicType == MISSION_LOGICTYPE.ARRIVE_TARGET)
			{
				MoveTargetMissionData moveTargetMissionDataById = DataManager.GetMoveTargetMissionDataById(missionData.LogicID);
				if (moveTargetMissionDataById != null)
				{
					text = moveTargetMissionDataById.MapId;
					if (string.IsNullOrEmpty(text))
					{
						return;
					}
					int num = (int)GetMissionParam(missionData.ID, 0);
					if (num > moveTargetMissionDataById.TargetPointList.Count)
					{
						num = moveTargetMissionDataById.TargetPointList.Count - 1;
					}
					Vector3 pos = moveTargetMissionDataById.TargetPointList[num];
					vector.x = pos.x;
					vector.y = SceneManager.GetHitHeight(pos);
					vector.z = pos.z;
					break;
				}
				return;
			}
			if (missionData.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC)
			{
				KillTargetMissionData killTargetMissionDataById = DataManager.GetKillTargetMissionDataById(missionData.LogicID);
				text = killTargetMissionDataById.SceneID;
				vector = killTargetMissionDataById.Pos;
				break;
			}
			if (missionData.MissionLogicType == MISSION_LOGICTYPE.TARGET_ROB_CAR)
			{
				TargetCarMissionData targetCarMissionDataById = DataManager.GetTargetCarMissionDataById(missionData.LogicID);
				text = targetCarMissionDataById.SceneID;
				vector = targetCarMissionDataById.Pos;
				break;
			}
			if (missionData.MissionLogicType == MISSION_LOGICTYPE.CAPTURE_SUCCESS)
			{
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
				{
					DominData dominDataByID = DataManager.GetDominDataByID(missionData.LogicID);
					if (dominDataByID.IsOpen == 0)
					{
						NoticeLogic.AddNotifyData("#{103019}");
						return;
					}
					if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(dominDataByID.LevelMin))
					{
						text = dominDataByID.AcceptMapID;
						vector = dominDataByID.GetPos();
						break;
					}
					NoticeLogic.AddNotifyData("#{103009}");
					return;
				}
				SingletonUnity<UIManager>.Instance.CloseAllPOPUI();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DownLoadResRoot);
				return;
			}
			if (missionData.MissionLogicType == MISSION_LOGICTYPE.ROB_CAR)
			{
				if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.MapType != MAPTYPE.TUTORIAL_CAR)
				{
					text = "11";
					vector = new Vector3(281f, 0f, -100f);
					break;
				}
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsTutorialCanShow(FUNCTION_TYPE.ROB_CAR_TIP) && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager is NewTutorialSceneManager newTutorialSceneManager && newTutorialSceneManager.mTutorialCar != null)
				{
					text = "11";
					vector = newTutorialSceneManager.mTutorialCar.PlayerCar.DummyPlayerPoint.position;
					break;
				}
				return;
			}
			if (missionData.Class == 4)
			{
				if (!SingletonUnity<FunctionBtnRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<FunctionBtnRootLogic>.Instance.gameObject))
				{
					break;
				}
				SingletonUnity<FunctionBtnRootLogic>.Instance.OnClickEscortFollowBtn();
				return;
			}
			if (missionData.Class == 5)
			{
				ClickMissionAction(missionData.ID);
				break;
			}
			text = missionData.TargetMapId;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (string.IsNullOrEmpty(missionData.Target))
			{
				if (!SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr.Equals(text))
				{
					MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(text);
					if (mapInfoDataByID != null)
					{
						AutoMoveDest(text, mapInfoDataByID.BirthPosVector3, AUTO_SEARCH_PARTH_FINISHEVENT.CHANGE_MAP);
					}
				}
				return;
			}
			Vector3 nPCPosInMonsterData4 = DataManager.GetNPCPosInMonsterData(text, missionData.Target);
			vector.x = nPCPosInMonsterData4.x;
			vector.z = nPCPosInMonsterData4.z;
			vector.y = SceneManager.GetHitHeight(nPCPosInMonsterData4);
			empty = missionData.Target;
			break;
		}
		case MISSION_STATE.COMPLETE:
		{
			text = missionData.SubmitMapId;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			Vector3 nPCPosInMonsterData = DataManager.GetNPCPosInMonsterData(text, missionData.Submit);
			vector.x = nPCPosInMonsterData.x;
			vector.z = nPCPosInMonsterData.z;
			vector.y = SceneManager.GetHitHeight(nPCPosInMonsterData);
			empty = missionData.Submit;
			break;
		}
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		AutoSearchPathPoint targetPoint = new AutoSearchPathPoint(text, vector.x, vector.y, vector.z);
		if (instance != null && instance.AutoSearchPath != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.BreakAutoCombatState();
			Singleton<ObjManager>.Instance.MainPlayer.SkillLogic.BreakCurSkill();
			instance.AutoSearchPath.FindPath(targetPoint, AUTO_SEARCH_PARTH_FINISHEVENT.MISSION, missionData.ID);
		}
		AutoSearchPath curPath = instance.AutoSearchPath.CurPath;
		if (curPath == null || curPath.PathPointList.Count <= 0)
		{
			return;
		}
		Vector3 vector2 = new Vector3(curPath.PathPointList[0].PosX, SceneManager.GetHitHeight(curPath.PathPointList[0].PosX, curPath.PathPointList[0].PosZ), curPath.PathPointList[0].PosZ);
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer.IsLocalDrivingCar)
		{
			if (Vector3.Distance(vector2, mainPlayer.Position) < 10f)
			{
				MoveToNextPoint(mainPlayer);
			}
		}
		else
		{
			mainPlayer.MoveTo(vector2, mainPlayer.GetStopDistance(), MoveToNextPoint);
		}
		instance.SceneManager.SetMoveTarget(vector2, missionData.ID);
	}

	public void MoveToNextPoint(ObjCharacter objCha)
	{
		AutoSearchPathManager autoSearchPath = SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath;
		if (autoSearchPath.CurPath.PathPointList.Count > 0)
		{
			autoSearchPath.CurPath.PathPointList.RemoveAt(0);
		}
		if (autoSearchPath.CurPath.PathPointList.Count == 0)
		{
			FinishPathFindEvent();
			autoSearchPath.Finish();
			return;
		}
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr.Equals(autoSearchPath.CurPath.PathPointList[0].SceneId) && mainPlayer != null)
		{
			Vector3 pos = new Vector3(autoSearchPath.CurPath.PathPointList[0].PosX, SceneManager.GetHitHeight(autoSearchPath.CurPath.PathPointList[0].PosX, autoSearchPath.CurPath.PathPointList[0].PosZ), autoSearchPath.CurPath.PathPointList[0].PosZ);
			mainPlayer.MoveTo(pos, Singleton<ObjManager>.Instance.MainPlayer.GetStopDistance(), MoveToNextPoint);
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.SetMoveTarget(pos, autoSearchPath.CurMissionId);
		}
	}

	public void AutoMoveDest(string mapId, Vector3 pos, AUTO_SEARCH_PARTH_FINISHEVENT Type, string param = null)
	{
		AutoSearchPathPoint targetPoint = new AutoSearchPathPoint(mapId, pos.x, pos.y, pos.z);
		AutoSearchPathManager autoSearchPath = SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath;
		if (autoSearchPath != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.BreakAutoCombatState();
			Singleton<ObjManager>.Instance.MainPlayer.SkillLogic.BreakCurSkill();
			SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath.FindPath(targetPoint, Type, param);
			AutoSearchPath curPath = autoSearchPath.CurPath;
			if (curPath != null && curPath.PathPointList.Count > 0)
			{
				Vector3 pos2 = new Vector3(curPath.PathPointList[0].PosX, SceneManager.GetHitHeight(curPath.PathPointList[0].PosX, curPath.PathPointList[0].PosZ), curPath.PathPointList[0].PosZ);
				Singleton<ObjManager>.Instance.MainPlayer.MoveTo(pos2, Singleton<ObjManager>.Instance.MainPlayer.GetStopDistance(), MoveToNextPoint);
				SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.SetMoveTarget(pos2, autoSearchPath.CurMissionId);
			}
		}
	}

	public void FinishPathFindEvent()
	{
		AutoSearchPathManager autoSearchPath = SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath;
		if (Singleton<ObjManager>.Instance.MainPlayer == null)
		{
			return;
		}
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (autoSearchPath.FinishEventType == AUTO_SEARCH_PARTH_FINISHEVENT.MISSION)
		{
			string curMissionId = autoSearchPath.CurMissionId;
			if (string.IsNullOrEmpty(curMissionId))
			{
				return;
			}
			MissionData missionDataByID = DataManager.GetMissionDataByID(curMissionId);
			MISSION_STATE missionState = GetMissionState(curMissionId);
			if (missionDataByID.Class == 4 && missionState == MISSION_STATE.ACCEPTED && SingletonUnity<FunctionBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionBtnRootLogic>.Instance.gameObject))
			{
				SingletonUnity<FunctionBtnRootLogic>.Instance.OnClickEscortFollowBtn();
				return;
			}
			string text = string.Empty;
			switch (missionState)
			{
			case MISSION_STATE.INVALID:
			case MISSION_STATE.FAIL:
				text = missionDataByID.Accept;
				break;
			case MISSION_STATE.ACCEPTED:
				text = missionDataByID.Target;
				if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
				{
					MultiDeliveryMissionData curMultiDeliveryTargetData = GetCurMultiDeliveryTargetData(curMissionId);
					text = curMultiDeliveryTargetData.TargetNpcId;
				}
				else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.CAPTURE_SUCCESS)
				{
					ObjZombieRagdollPlayer objZombieRagdollPlayer = FindDominNPC(text);
					if (objZombieRagdollPlayer != null && objZombieRagdollPlayer.CheckInDialogRange())
					{
						objZombieRagdollPlayer.ShowAcitvityDialog();
					}
					return;
				}
				break;
			case MISSION_STATE.COMPLETE:
				text = missionDataByID.Submit;
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				ObjNPC objNPC = FindMissionNPC(text);
				if (objNPC != null && objNPC.CheckInDialogRange())
				{
					Singleton<DialogManager>.Instance.ShowDialog(objNPC, curMissionId);
				}
				if (objNPC == null && missionState != MISSION_STATE.COMPLETE && (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER_DROP || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER_DROP || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MASSACRE_NPC || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC))
				{
					SingletonUnity<JueseJiNengQuLogic>.Instance.UseSkill_1_Onclick();
					if (mainPlayer.IsOpenAutoCombat)
					{
						mainPlayer.ReturnAutoCombatState();
					}
					else if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.AUTO_FIGHT))
					{
						mainPlayer.EnterAutoCombat();
						SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateAutoBtn();
					}
				}
			}
			else if (missionDataByID.Class == 0)
			{
				if (missionDataByID.MissionLogicType != MISSION_LOGICTYPE.SURVEY || missionState != MISSION_STATE.ACCEPTED)
				{
					return;
				}
				SurveyMissionData surveyMissionDataById = DataManager.GetSurveyMissionDataById(missionDataByID.LogicID);
				GameObject gameObject = Singleton<ObjManager>.Instance.FindOtherObjInDic(surveyMissionDataById.GetSurveyItemName());
				if (gameObject != null && Vector3.Distance(gameObject.transform.position, mainPlayer.Position) < 3f)
				{
					SurveyItemObj component = gameObject.GetComponent<SurveyItemObj>();
					if (component != null)
					{
						Singleton<SurveyItemManager>.Instance.StartSurveyItem(component);
					}
					else
					{
						Debug.Log("SurveyItem == null");
					}
				}
			}
			else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.SURVEY && missionState == MISSION_STATE.ACCEPTED)
			{
				SurveyMissionData surveyMissionDataById2 = DataManager.GetSurveyMissionDataById(missionDataByID.LogicID);
				GameObject gameObject2 = Singleton<ObjManager>.Instance.FindOtherObjInDic(surveyMissionDataById2.GetSurveyItemName());
				float num = 3.1f;
				if (mainPlayer.IsLocalDrivingCar)
				{
					num = 8f;
				}
				if (!(gameObject2 != null) || !(Vector3.Distance(gameObject2.transform.position, mainPlayer.Position) < num))
				{
					return;
				}
				SurveyItemObj surveyItemObj = gameObject2.GetComponent<SurveyItemObj>();
				if (surveyItemObj != null)
				{
					if (mainPlayer.IsLocalDrivingCar)
					{
						if (!SingletonUnity<CitySimController>.Exists)
						{
							return;
						}
						SingletonUnity<CitySimController>.Instance.RobCar(delegate
						{
							mainPlayer.MoveTo(surveyItemObj.transform.position, 1f, delegate
							{
								mainPlayer.FaceToPub(surveyItemObj.transform.position);
								Singleton<SurveyItemManager>.Instance.StartSurveyItem(surveyItemObj);
							});
						});
					}
					else
					{
						Singleton<SurveyItemManager>.Instance.StartSurveyItem(surveyItemObj);
					}
				}
				else
				{
					Debug.Log("SurveyItem == null");
				}
			}
			else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MASSACRE_NPC || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC)
			{
				SingletonUnity<JueseJiNengQuLogic>.Instance.UseSkill_1_Onclick();
				if (mainPlayer.IsOpenAutoCombat)
				{
					mainPlayer.ReturnAutoCombatState();
				}
				else if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.AUTO_FIGHT))
				{
					mainPlayer.EnterAutoCombat();
					SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateAutoBtn();
				}
			}
		}
		else if (autoSearchPath.FinishEventType == AUTO_SEARCH_PARTH_FINISHEVENT.CITY_DANCE)
		{
			if (SingletonUnity<DanceBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DanceBtnRootLogic>.Instance.DanceBtnScale.gameObject))
			{
				SingletonUnity<DanceBtnRootLogic>.Instance.OnClickDanceBtn();
			}
		}
		else if (autoSearchPath.FinishEventType == AUTO_SEARCH_PARTH_FINISHEVENT.FIND_NPC)
		{
			ObjNPC objNPC2 = FindMissionNPC(autoSearchPath.CurMissionId);
			if (objNPC2 != null && objNPC2.CheckInDialogRange())
			{
				Singleton<DialogManager>.Instance.ShowDialog(objNPC2, string.Empty);
			}
		}
	}

	private ObjNPC FindMissionNPC(string npcId)
	{
		List<Obj> list = new List<Obj>(Singleton<ObjManager>.Instance.ObjDict.Values);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].ObjType == GameDefine.OBJ_TYPE.OBJ_NPC && (list[i] as ObjNPC).IsMissionNpc())
			{
				ObjNPC objNPC = list[i] as ObjNPC;
				if (objNPC.NPCDataID.Equals(npcId))
				{
					return objNPC;
				}
			}
		}
		return null;
	}

	private ObjZombieRagdollPlayer FindDominNPC(string npcId)
	{
		List<Obj> list = new List<Obj>(Singleton<ObjManager>.Instance.ObjDict.Values);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL && (list[i] as ObjZombieRagdollPlayer).IsMissionNpc)
			{
				ObjZombieRagdollPlayer objZombieRagdollPlayer = list[i] as ObjZombieRagdollPlayer;
				if (objZombieRagdollPlayer.NpcId.Equals(npcId))
				{
					return objZombieRagdollPlayer;
				}
			}
		}
		return null;
	}

	public void UpdateMissionUI(string missionId)
	{
		if (SingletonUnity<MissionTeamTipLogic>.Exists)
		{
			SingletonUnity<MissionTeamTipLogic>.Instance.UpdateMission(missionId);
		}
		if (SingletonUnity<MissionPageRootLogic>.Exists)
		{
			SingletonUnity<MissionPageRootLogic>.Instance.Reset(isRefersh: true);
		}
		if (SingletonUnity<NewMissionUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMissionUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewMissionUIRootLogic>.Instance.Reset(string.Empty);
		}
		if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.InitMissionActivityPic();
			SingletonUnity<NewMapUIRootLogic>.Instance.InitMapActivityPic();
		}
	}

	public void PrintMissionList()
	{
		List<CurMission> list = new List<CurMission>(mCurMissionDictionary.CurMissionDic.Values);
		for (int i = 0; i < list.Count; i++)
		{
			Debug.Log("==================");
			PrintMission(list[i].MissionId);
		}
	}

	public void PrintMission(string missionId)
	{
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		Debug.Log("Mission Id : " + missionId);
		Debug.Log("Param 0 : " + mCurMissionDictionary.GetMissionParam(missionId, 0));
		Debug.Log("Mission State : " + mCurMissionDictionary.GetMissionState(missionId));
		Debug.Log("MissionFinished : " + IsMissionCompleted(missionId));
	}

	public void PickMissionItem(string ItemId)
	{
		List<CurMission> list = new List<CurMission>(mCurMissionDictionary.CurMissionDic.Values);
		for (int i = 0; i < list.Count; i++)
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(list[i].MissionId);
			if (missionDataByID.MissionLogicType != MISSION_LOGICTYPE.COLLECTITEM && missionDataByID.MissionLogicType != MISSION_LOGICTYPE.KILLMONSTER_DROP && missionDataByID.MissionLogicType != MISSION_LOGICTYPE.LOCAL_KILL_MONSTER_DROP)
			{
				continue;
			}
			MissionRequireData missionRequireDataByID = DataManager.GetMissionRequireDataByID(missionDataByID.LogicID);
			if (missionRequireDataByID.RequireItemID.Equals(ItemId))
			{
				SetMissionParam(missionDataByID.ID, 0, GetMissionParam(missionDataByID.ID, 0) + 1);
				if (list[i].GetParam(0) >= missionRequireDataByID.RequireNum)
				{
					list[i].SetMissionState(MISSION_STATE.COMPLETE);
				}
				SingletonUnity<MissionTeamTipLogic>.Instance.UpdateMission(list[i].MissionId);
				break;
			}
		}
	}

	public bool IsHaveEscortMission()
	{
		List<CurMission> list = new List<CurMission>(mCurMissionDictionary.CurMissionDic.Values);
		for (int i = 0; i < list.Count; i++)
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(list[i].MissionId);
			if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.ESCORT)
			{
				return true;
			}
		}
		return false;
	}

	public MultiDeliveryMissionData GetCurMultiDeliveryTargetData(string missionId)
	{
		if (IsMissionAccepted(missionId))
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
			if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
			{
				List<MultiDeliveryMissionData> multiDeliveryMissionDataListById = DataManager.GetMultiDeliveryMissionDataListById(missionDataByID.LogicID);
				int num = (int)GetMissionParam(missionId, 0);
				if (multiDeliveryMissionDataListById.Count >= num)
				{
					return multiDeliveryMissionDataListById[num];
				}
			}
		}
		return null;
	}

	public static void GetMissionStateLabel(MissionData mCurMissionData, MISSION_STATE curMissionState, UILabel MissionStateLabel)
	{
		if (mCurMissionData == null)
		{
			return;
		}
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		switch (curMissionState)
		{
		case MISSION_STATE.ACCEPTED:
			MissionStateLabel.color = Color.white;
			if (playerData.Level < mCurMissionData.MinLv)
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100320}", mCurMissionData.MinLv);
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER || mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER || mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER || mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER_DROP)
			{
				NpcData npcDataByID = DataManager.GetNpcDataByID(mCurMissionData.Target);
				MissionRequireData missionRequireDataByID = DataManager.GetMissionRequireDataByID(mCurMissionData.LogicID);
				if (npcDataByID != null && missionRequireDataByID != null)
				{
					MissionStateLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{100315}", npcDataByID.MName), missionManager.GetMissionParam(mCurMissionData.ID, 0), missionRequireDataByID.RequireNum);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC)
			{
				KillTargetMissionData killTargetMissionDataById = DataManager.GetKillTargetMissionDataById(mCurMissionData.LogicID);
				NpcData npcDataByID2 = DataManager.GetNpcDataByID(killTargetMissionDataById.NpcID);
				if (npcDataByID2 != null && killTargetMissionDataById != null)
				{
					MissionStateLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{100315}", npcDataByID2.MName), missionManager.GetMissionParam(mCurMissionData.ID, 0), killTargetMissionDataById.RequireNum);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.TARGET_ROB_CAR)
			{
				TargetCarMissionData targetCarMissionDataById = DataManager.GetTargetCarMissionDataById(mCurMissionData.LogicID);
				if (targetCarMissionDataById != null)
				{
					MissionStateLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{100327}"), missionManager.GetMissionParam(mCurMissionData.ID, 0), targetCarMissionDataById.RequireNum);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.IMPACT_NPC)
			{
				MissionRequireData missionRequireDataByID2 = DataManager.GetMissionRequireDataByID(mCurMissionData.LogicID);
				if (missionRequireDataByID2 != null)
				{
					MissionStateLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{100328}"), missionManager.GetMissionParam(mCurMissionData.ID, 0), missionRequireDataByID2.RequireNum);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.DESTROY_CAR)
			{
				MissionRequireData missionRequireDataByID3 = DataManager.GetMissionRequireDataByID(mCurMissionData.LogicID);
				if (missionRequireDataByID3 != null)
				{
					MissionStateLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{100329}"), missionManager.GetMissionParam(mCurMissionData.ID, 0), missionRequireDataByID3.RequireNum);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.ROB_CAR)
			{
				MissionRequireData missionRequireDataByID4 = DataManager.GetMissionRequireDataByID(mCurMissionData.LogicID);
				if (missionRequireDataByID4 != null)
				{
					MissionStateLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{100327}"), missionManager.GetMissionParam(mCurMissionData.ID, 0), missionRequireDataByID4.RequireNum);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.MASSACRE_NPC)
			{
				MissionRequireData missionRequireDataByID5 = DataManager.GetMissionRequireDataByID(mCurMissionData.LogicID);
				if (missionRequireDataByID5 != null)
				{
					MissionStateLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{100325}"), missionManager.GetMissionParam(mCurMissionData.ID, 0), missionRequireDataByID5.RequireNum);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.ARRIVE_TARGET)
			{
				MoveTargetMissionData moveTargetMissionDataById = DataManager.GetMoveTargetMissionDataById(mCurMissionData.LogicID);
				if (moveTargetMissionDataById != null)
				{
					MissionStateLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{100330}"), missionManager.GetMissionParam(mCurMissionData.ID, 0), moveTargetMissionDataById.TargetNum);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.SURVEY)
			{
				SurveyMissionData surveyMissionDataById = DataManager.GetSurveyMissionDataById(mCurMissionData.LogicID);
				if (surveyMissionDataById != null)
				{
					MissionStateLabel.text = $"{StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID)} {missionManager.GetMissionParam(mCurMissionData.ID, 0)}/{surveyMissionDataById.NeedNum}";
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.COLLECTITEM || mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER_DROP || mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER_DROP)
			{
				NpcData npcDataByID3 = DataManager.GetNpcDataByID(mCurMissionData.Target);
				MissionRequireData missionRequireDataByID6 = DataManager.GetMissionRequireDataByID(mCurMissionData.LogicID);
				if (npcDataByID3 != null && missionRequireDataByID6 != null)
				{
					MissionStateLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{100317}", npcDataByID3.MName), missionManager.GetMissionParam(mCurMissionData.ID, 0), missionRequireDataByID6.RequireNum);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.COPYSCENE_KILLMONSTER)
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
			}
			else if (mCurMissionData.Class == 4)
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
			}
			else if (mCurMissionData.Class == 5)
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
			{
				MultiDeliveryMissionData curMultiDeliveryTargetData = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetCurMultiDeliveryTargetData(mCurMissionData.ID);
				if (curMultiDeliveryTargetData != null)
				{
					NpcData npcDataByID4 = DataManager.GetNpcDataByID(curMultiDeliveryTargetData.TargetNpcId);
					if (npcDataByID4 != null)
					{
						MissionStateLabel.text = $"{npcDataByID4.MName} 0/1";
					}
				}
				else
				{
					Debug.Log("MultiDeliveryMissionData ERROR!!!!!!!!!!!!!!!!!!!!!");
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.STORY)
			{
				NpcData npcDataByID5 = DataManager.GetNpcDataByID(mCurMissionData.Target);
				if (npcDataByID5 != null)
				{
					MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100314}", npcDataByID5.MName);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.OPERATION)
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.CAPTURE_SUCCESS)
			{
				DominData dominDataByID = DataManager.GetDominDataByID(mCurMissionData.LogicID);
				if (dominDataByID != null)
				{
					MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100331}", dominDataByID.GetName);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.LEVEL_UP)
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100320}", mCurMissionData.LogicID);
			}
			else
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
			}
			return;
		case MISSION_STATE.COMPLETE:
			MissionStateLabel.color = Color.white;
			if (playerData.Level < mCurMissionData.MinLv)
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100313}", mCurMissionData.MinLv);
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.DELIVERY)
			{
				NpcData npcDataByID6 = DataManager.GetNpcDataByID(mCurMissionData.Target);
				if (npcDataByID6 != null)
				{
					MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100316}", npcDataByID6.MName);
				}
				MissionStateLabel.color = Color.white;
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.LEVEL_UP)
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100320}", mCurMissionData.LogicID);
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.SINGLE_DANCE)
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
			}
			else
			{
				NpcData npcDataByID7 = DataManager.GetNpcDataByID(mCurMissionData.Submit);
				if (npcDataByID7 != null)
				{
					MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100318}", npcDataByID7.MName);
					MissionStateLabel.color = Color.green;
				}
				else
				{
					MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
				}
			}
			return;
		}
		MissionStateLabel.color = Color.white;
		if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER || mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER)
		{
			NpcData npcDataByID8 = DataManager.GetNpcDataByID(mCurMissionData.Target);
			if (npcDataByID8 != null)
			{
				MissionStateLabel.text = string.Format("{0}", StrDictionary.GetDictionaryString("#{100315}", npcDataByID8.MName));
			}
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.MASSACRE_NPC)
		{
			MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100325}");
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.DESTROY_CAR)
		{
			MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100325}");
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.ROB_CAR)
		{
			MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100325}");
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.IMPACT_NPC)
		{
			MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100325}");
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.SURVEY)
		{
			MissionStateLabel.text = $"{StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID)}";
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.COLLECTITEM || mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER_DROP || mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER_DROP)
		{
			NpcData npcDataByID9 = DataManager.GetNpcDataByID(mCurMissionData.Target);
			if (npcDataByID9 != null)
			{
				MissionStateLabel.text = string.Format("{0}", StrDictionary.GetDictionaryString("#{100317}", npcDataByID9.MName));
			}
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.COPYSCENE_KILLMONSTER)
		{
			MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
		}
		else if (mCurMissionData.Class == 4)
		{
			MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
		}
		else if (mCurMissionData.Class == 5)
		{
			MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
		{
			MultiDeliveryMissionData curMultiDeliveryTargetData2 = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetCurMultiDeliveryTargetData(mCurMissionData.ID);
			if (curMultiDeliveryTargetData2 != null)
			{
				NpcData npcDataByID10 = DataManager.GetNpcDataByID(curMultiDeliveryTargetData2.TargetNpcId);
				if (npcDataByID10 != null)
				{
					MissionStateLabel.text = $"{npcDataByID10.MName}";
				}
			}
			else
			{
				Debug.Log("MultiDeliveryMissionData ERROR!!!!!!!!!!!!!!!!!!!!!");
			}
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.STORY)
		{
			NpcData npcDataByID11 = DataManager.GetNpcDataByID(mCurMissionData.Target);
			if (npcDataByID11 != null)
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100314}", npcDataByID11.MName);
			}
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.OPERATION)
		{
			MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.DELIVERY)
		{
			NpcData npcDataByID12 = DataManager.GetNpcDataByID(mCurMissionData.Accept);
			if (npcDataByID12 != null)
			{
				MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100316}", npcDataByID12.MName);
				MissionStateLabel.color = Color.white;
			}
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.LEVEL_UP)
		{
			MissionStateLabel.text = StrDictionary.GetDictionaryString("#{100320}", mCurMissionData.LogicID);
		}
		else
		{
			MissionStateLabel.text = StrDictionary.GetDictionaryString(mCurMissionData.TagDescribeID);
		}
	}

	public string GetCurDailyMissionID()
	{
		List<string> allMissionId = GetAllMissionId();
		for (int i = 0; i < allMissionId.Count; i++)
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(allMissionId[i]);
			if (missionDataByID.Class == 0)
			{
				return allMissionId[i];
			}
		}
		return string.Empty;
	}

	public bool IsHaveDailyMission()
	{
		List<string> allMissionId = GetAllMissionId();
		for (int i = 0; i < allMissionId.Count; i++)
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(allMissionId[i]);
			if (missionDataByID.Class == 0 || missionDataByID.Class == 3 || missionDataByID.Class == 6)
			{
				return true;
			}
		}
		return false;
	}

	public CurMission GetCurMissionByClassType(MISSION_CLASS_TYPE type)
	{
		return mCurMissionDictionary.GetCurMissionByClassType(type);
	}

	public bool IsInEscortMission()
	{
		return mCurMissionDictionary.GetCurMissionByClassType(MISSION_CLASS_TYPE.ESCORT) != null;
	}

	public CurMission GetEscortMission()
	{
		return mCurMissionDictionary.GetCurMissionByClassType(MISSION_CLASS_TYPE.ESCORT);
	}

	public Vector3 GetEscortNpcPos(out string mapId)
	{
		CurMission escortMission = GetEscortMission();
		mapId = escortMission.GetParam(3).ToString();
		if (escortMission != null)
		{
			ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(escortMission.GetParam(1));
			if (objCharacter != null)
			{
				return objCharacter.Position;
			}
			return new Vector3((float)escortMission.GetParam(4) / 100f, 0f, (float)escortMission.GetParam(5) / 100f);
		}
		return Vector3.zero;
	}

	public HEAD_PIC_TYPE GetMissionNpcHeadPicType(string npcId)
	{
		List<string> allMissionId = GetAllMissionId();
		HEAD_PIC_TYPE result = HEAD_PIC_TYPE.INVALID;
		for (int i = 0; i < allMissionId.Count; i++)
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(allMissionId[i]);
			if (missionDataByID.Submit.Equals(npcId) && GetMissionState(allMissionId[i]) == MISSION_STATE.COMPLETE)
			{
				return HEAD_PIC_TYPE.MISSION_COMPLETE_NPC;
			}
			if (missionDataByID.Target.Equals(npcId) && GetMissionState(allMissionId[i]) == MISSION_STATE.ACCEPTED)
			{
				result = HEAD_PIC_TYPE.MISSION_TARGET_NPC;
			}
		}
		return result;
	}

	~MissionManager()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(LevelUpMissionCheck));
	}

	public void LevelUpMissionCheck()
	{
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		List<string> list = null;
		for (int i = 1; i <= level; i++)
		{
			list = DataManager.GetLvAutoAcceptMissionListByLevel(i);
			if (list == null)
			{
				continue;
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (IsMissionAcceptable(list[j]))
				{
					AcceptMission(list[j]);
				}
			}
		}
	}

	public Vector3 GetMoveTargetMissionPos(string missionId)
	{
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID.MissionLogicType != MISSION_LOGICTYPE.ARRIVE_TARGET)
		{
			return Vector3.zero;
		}
		MoveTargetMissionData moveTargetMissionDataById = DataManager.GetMoveTargetMissionDataById(missionDataByID.LogicID);
		if (missionDataByID == null)
		{
			return Vector3.zero;
		}
		if (!mCurMissionDictionary.CurMissionDic.ContainsKey(missionId))
		{
			return Vector3.zero;
		}
		CurMission curMission = mCurMissionDictionary.CurMissionDic[missionId];
		if (curMission.MissionState != MISSION_STATE.ACCEPTED)
		{
			return Vector3.zero;
		}
		int num = (int)curMission.GetParam(0);
		if (num < moveTargetMissionDataById.TargetPointList.Count)
		{
			Vector3 pos = new Vector3(moveTargetMissionDataById.TargetPointList[num].x, 0f, moveTargetMissionDataById.TargetPointList[num].z);
			return new Vector3(pos.x, SceneManager.GetHitHeight(pos), pos.z);
		}
		num = moveTargetMissionDataById.TargetPointList.Count - 1;
		Vector3 pos2 = new Vector3(moveTargetMissionDataById.TargetPointList[num].x, 0f, moveTargetMissionDataById.TargetPointList[num].z);
		return new Vector3(pos2.x, SceneManager.GetHitHeight(pos2), pos2.z);
	}

	public void SetFirstSceneMissionTarget()
	{
		CurMission curMissionByClassType = GetCurMissionByClassType(MISSION_CLASS_TYPE.MAIN);
		if (curMissionByClassType != null)
		{
			SetFirstSceneMissionTarget(curMissionByClassType.MissionId);
		}
	}

	public static void SetFirstSceneMissionTarget(string misId)
	{
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		int level = instance.PlayerData.Level;
		int num = 15;
		FunctionData functionDataById = DataManager.GetFunctionDataById(4087.ToString());
		if (functionDataById != null)
		{
			num = functionDataById.Condition;
		}
		if (level > num)
		{
			return;
		}
		Vector3 pos = Vector3.zero;
		string text = string.Empty;
		MissionData missionDataByID = DataManager.GetMissionDataByID(misId);
		if (missionDataByID.Class != 1 || SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.MapType != MAPTYPE.TUTORIAL_CAR)
		{
			return;
		}
		instance.SceneManager.ClearMoveTarget();
		MISSION_STATE missionState = instance.MissionManager.GetMissionState(missionDataByID.ID);
		if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.DOWNLOAD_MISSION && missionState == MISSION_STATE.ACCEPTED && !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
		{
			instance.SceneManager.ClearMoveTarget();
			return;
		}
		if ((missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER) && missionState == MISSION_STATE.ACCEPTED)
		{
			ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			MissionRequireData missionRequireDataByID = DataManager.GetMissionRequireDataByID(missionDataByID.LogicID);
			ObjNPC objNPC = Singleton<ObjManager>.Instance.FindNearestFightNPCInScene(missionRequireDataByID.NPCID);
			if (objNPC != null)
			{
				text = instance.SceneManager.CurrentMapInofData.ID;
				pos = objNPC.Position;
			}
		}
		MissionData missionData = missionDataByID;
		switch (missionState)
		{
		case MISSION_STATE.ACCEPTED:
			if (missionData.MissionLogicType == MISSION_LOGICTYPE.SURVEY)
			{
				SurveyMissionData surveyMissionDataById = DataManager.GetSurveyMissionDataById(missionData.LogicID);
				text = surveyMissionDataById.SceneID;
				pos.x = surveyMissionDataById.PosX;
				pos.z = surveyMissionDataById.PosZ;
			}
			else if (missionData.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
			{
				MultiDeliveryMissionData curMultiDeliveryTargetData = instance.MissionManager.GetCurMultiDeliveryTargetData(missionData.ID);
				if (curMultiDeliveryTargetData != null)
				{
					text = curMultiDeliveryTargetData.TargetMapId;
					if (string.IsNullOrEmpty(text))
					{
						instance.SceneManager.ClearMoveTarget();
						return;
					}
					Vector3 nPCPosInMonsterData = DataManager.GetNPCPosInMonsterData(text, curMultiDeliveryTargetData.TargetNpcId);
					pos.x = nPCPosInMonsterData.x;
					pos.z = nPCPosInMonsterData.z;
					pos.y = SceneManager.GetHitHeight(nPCPosInMonsterData);
				}
			}
			else if (missionData.MissionLogicType == MISSION_LOGICTYPE.ARRIVE_TARGET)
			{
				MoveTargetMissionData moveTargetMissionDataById = DataManager.GetMoveTargetMissionDataById(missionData.LogicID);
				if (moveTargetMissionDataById != null)
				{
					text = moveTargetMissionDataById.MapId;
					if (string.IsNullOrEmpty(text))
					{
						instance.SceneManager.ClearMoveTarget();
						return;
					}
					int num2 = (int)instance.MissionManager.GetMissionParam(missionData.ID, 0);
					if (num2 > moveTargetMissionDataById.TargetPointList.Count)
					{
						num2 = moveTargetMissionDataById.TargetPointList.Count - 1;
					}
					Vector3 pos3 = moveTargetMissionDataById.TargetPointList[num2];
					pos.x = pos3.x;
					pos.y = SceneManager.GetHitHeight(pos3);
					pos.z = pos3.z;
				}
			}
			else if (missionData.MissionLogicType == MISSION_LOGICTYPE.KILL_TARGET_NPC)
			{
				KillTargetMissionData killTargetMissionDataById = DataManager.GetKillTargetMissionDataById(missionData.LogicID);
				text = killTargetMissionDataById.SceneID;
				pos = killTargetMissionDataById.Pos;
			}
			else if (missionData.MissionLogicType == MISSION_LOGICTYPE.TARGET_ROB_CAR)
			{
				TargetCarMissionData targetCarMissionDataById = DataManager.GetTargetCarMissionDataById(missionData.LogicID);
				text = targetCarMissionDataById.SceneID;
				pos = targetCarMissionDataById.Pos;
			}
			else if (missionData.MissionLogicType == MISSION_LOGICTYPE.ROB_CAR)
			{
				if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.MapType != MAPTYPE.TUTORIAL_CAR)
				{
					text = "11";
					pos = new Vector3(281f, 0f, -100f);
				}
				else if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsTutorialCanShow(FUNCTION_TYPE.ROB_CAR_TIP) && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager is NewTutorialSceneManager newTutorialSceneManager && newTutorialSceneManager.mTutorialCar != null)
				{
					text = "11";
					pos = newTutorialSceneManager.mTutorialCar.PlayerCar.DummyPlayerPoint.position;
				}
			}
			else if (missionData.MissionLogicType == MISSION_LOGICTYPE.CAPTURE_SUCCESS)
			{
				DominData dominDataByID = DataManager.GetDominDataByID(missionData.LogicID);
				if (dominDataByID != null)
				{
					text = dominDataByID.AcceptMapID;
					pos = dominDataByID.GetPos();
				}
				else
				{
					text = string.Empty;
				}
			}
			else
			{
				text = missionData.TargetMapId;
				if (string.IsNullOrEmpty(text))
				{
					instance.SceneManager.ClearMoveTarget();
					return;
				}
				Vector3 nPCPosInMonsterData2 = DataManager.GetNPCPosInMonsterData(text, missionData.Target);
				pos.x = nPCPosInMonsterData2.x;
				pos.z = nPCPosInMonsterData2.z;
				pos.y = SceneManager.GetHitHeight(nPCPosInMonsterData2);
			}
			break;
		case MISSION_STATE.COMPLETE:
		{
			text = missionData.SubmitMapId;
			if (string.IsNullOrEmpty(text))
			{
				instance.SceneManager.ClearMoveTarget();
				return;
			}
			if (!DataManager.GetNPCPosInMonsterData2(text, missionData.Submit, out var pos2))
			{
				return;
			}
			pos.x = pos2.x;
			pos.z = pos2.z;
			pos.y = SceneManager.GetHitHeight(pos2);
			break;
		}
		}
		if (string.IsNullOrEmpty(text))
		{
			instance.SceneManager.ClearMoveTarget();
		}
		else
		{
			instance.SceneManager.SetMoveTarget(pos, missionData.ID);
		}
	}

	public static void ClickMissionAction(string misId)
	{
		MissionData missionDataByID = DataManager.GetMissionDataByID(misId);
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		instance.SceneManager.ClearMoveTarget();
		int level = instance.PlayerData.Level;
		if (missionDataByID == null)
		{
			return;
		}
		if (!instance.MissionManager.IsMissionAccepted(misId))
		{
			if (string.IsNullOrEmpty(missionDataByID.Accept))
			{
				if (string.IsNullOrEmpty(missionDataByID.AcceptMapId))
				{
					return;
				}
				List<ActivityMapData> acitvityMapDataByMapId = DataManager.GetAcitvityMapDataByMapId(missionDataByID.AcceptMapId);
				ActivityMapData activityMapData = null;
				for (int i = 0; i < acitvityMapDataByMapId.Count; i++)
				{
					if (acitvityMapDataByMapId[i].ActivityType == GameDefine.ACTIVITY_TYPE.MISSION && acitvityMapDataByMapId[i].ActivityID.Equals(missionDataByID.ID))
					{
						activityMapData = acitvityMapDataByMapId[i];
						break;
					}
				}
				if (activityMapData == null)
				{
					return;
				}
				Vector3 position = activityMapData.Position;
				SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath.FindPath(activityMapData.MapId, position.x, position.y, position.z, AUTO_SEARCH_PARTH_FINISHEVENT.CHANGE_MAP);
				AutoSearchPath curPath = SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath.CurPath;
				if (curPath != null && curPath.PathPointList.Count > 0)
				{
					ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
					mainPlayer.BreakAutoCombatState();
					mainPlayer.SkillLogic.BreakCurSkill();
					if (curPath.PathPointList != null && curPath.PathPointList.Count > 0)
					{
						SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.SetMoveTarget(new Vector3(curPath.PathPointList[0].PosX, 0f, curPath.PathPointList[0].PosZ), misId);
						mainPlayer.MoveTo(curPath.PathPointList[0].PosX, curPath.PathPointList[0].PosZ, mainPlayer.GetStopDistance(), SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.MoveToNextPoint);
					}
				}
			}
			else
			{
				instance.MissionManager.MissionFindPath(missionDataByID);
			}
			return;
		}
		if (level < missionDataByID.MinLv)
		{
			NoticeLogic.AddNotifyData2Client(false, "#{100313}", true, missionDataByID.MinLv);
			if (missionDataByID.Class != 1)
			{
				return;
			}
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn();
				});
			}
			else
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickMissionBtn();
				});
			}
			return;
		}
		MISSION_STATE missionState = instance.MissionManager.GetMissionState(missionDataByID.ID);
		MissionTipRootLogic.ShowMissionTips(missionDataByID.ID, missionState);
		if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.DOWNLOAD_MISSION && missionState == MISSION_STATE.ACCEPTED)
		{
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload && SingletonUnity<DownloadTipRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DownloadTipRootLogic>.Instance.gameObject))
			{
				SingletonUnity<DownloadTipRootLogic>.Instance.OnClickDownloadTipBtn();
			}
			return;
		}
		if ((missionDataByID.MissionLogicType == MISSION_LOGICTYPE.KILLMONSTER || missionDataByID.MissionLogicType == MISSION_LOGICTYPE.LOCAL_KILL_MONSTER) && missionState == MISSION_STATE.ACCEPTED)
		{
			ObjMainPlayer mainPlayer2 = Singleton<ObjManager>.Instance.MainPlayer;
			mainPlayer2.SelectTarget(null);
			MissionRequireData missionRequireDataByID = DataManager.GetMissionRequireDataByID(missionDataByID.LogicID);
			ObjNPC objNPC = Singleton<ObjManager>.Instance.FindNearestFightNPCInScene(missionRequireDataByID.NPCID);
			if (objNPC != null)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.SetMoveTarget(objNPC.Position, misId);
				if (!mainPlayer2.IsLocalDrivingCar)
				{
					mainPlayer2.BreakAutoCombatState();
					mainPlayer2.SkillLogic.BreakCurSkill();
					mainPlayer2.MoveTo(objNPC.Position, 1f, delegate
					{
						SingletonUnity<JueseJiNengQuLogic>.Instance.UseSkill_1_Onclick();
					});
				}
				return;
			}
		}
		if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MASSACRE_NPC && missionState == MISSION_STATE.ACCEPTED)
		{
			if (instance.SceneManager.CurrentMapInofData.ID.Equals(missionDataByID.TargetMapId))
			{
				if (!Singleton<ObjManager>.Instance.MainPlayer.IsLocalDrivingCar)
				{
					ObjNPC objNPC2 = Singleton<ObjManager>.Instance.FindNearestFightNPCInScene();
					if (objNPC2 != null)
					{
						Singleton<ObjManager>.Instance.MainPlayer.SelectTarget(objNPC2);
					}
					SingletonUnity<JueseJiNengQuLogic>.Instance.UseSkill_1_Onclick();
				}
			}
			else
			{
				MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(missionDataByID.TargetMapId);
				if (mapInfoDataByID != null)
				{
					instance.MissionManager.AutoMoveDest(missionDataByID.TargetMapId, mapInfoDataByID.BirthPosVector3, AUTO_SEARCH_PARTH_FINISHEVENT.CHANGE_MAP);
				}
			}
		}
		else if ((missionDataByID.Class == 0 || missionDataByID.Class == 6 || missionDataByID.Class == 7) && missionState == MISSION_STATE.COMPLETE)
		{
			complete_mission.request request = new complete_mission.request();
			request.missionId = missionDataByID.ID;
			NetLogic.GetInstance().Send<Protocol.complete_mission>(request);
		}
		else if (missionDataByID.Class == 4)
		{
			if (SingletonUnity<FunctionBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionBtnRootLogic>.Instance.gameObject))
			{
				SingletonUnity<FunctionBtnRootLogic>.Instance.OnClickEscortFollowBtn();
			}
		}
		else if (missionDataByID.Class == 5)
		{
			if (instance.SceneManager.CurrentMapInofData.ID.Equals(missionDataByID.AcceptMapId))
			{
				ObjNPC nearestEscortNPC = Singleton<ObjManager>.Instance.GetNearestEscortNPC();
				if (nearestEscortNPC != null)
				{
					ObjMainPlayer mainPlayer3 = Singleton<ObjManager>.Instance.MainPlayer;
					mainPlayer3.BreakAutoCombatState();
					mainPlayer3.SkillLogic.BreakCurSkill();
					mainPlayer3.SelectTarget(nearestEscortNPC);
					mainPlayer3.MoveTo(nearestEscortNPC.Position, 1f, delegate
					{
						SingletonUnity<JueseJiNengQuLogic>.Instance.UseSkill_1_Onclick();
					});
				}
				else
				{
					NoticeLogic.AddNotifyData("#{102047}");
				}
			}
			else
			{
				instance.MissionManager.AutoMoveDest(missionDataByID.AcceptMapId, Vector3.right * 30f, AUTO_SEARCH_PARTH_FINISHEVENT.CHANGE_MAP);
			}
		}
		else if (missionDataByID.Class == 2 || missionDataByID.Class == 1)
		{
			TutorialAction(missionDataByID);
		}
		else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.COPYSCENE_KILLMONSTER && missionState == MISSION_STATE.ACCEPTED && string.IsNullOrEmpty(missionDataByID.Target))
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
			enter_copy_scene.request request2 = new enter_copy_scene.request();
			request2.mapInfoId = missionDataByID.LogicID;
			NetLogic.GetInstance().Send<Protocol.enter_copy_scene>(request2);
		}
		else
		{
			instance.MissionManager.MissionFindPath(missionDataByID);
		}
	}

	public static void TutorialAction(MissionData mCurMissionData)
	{
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		MISSION_STATE missionState = instance.MissionManager.GetMissionState(mCurMissionData.ID);
		if (missionState == MISSION_STATE.COMPLETE)
		{
			if (string.IsNullOrEmpty(mCurMissionData.Submit))
			{
				complete_mission.request request = new complete_mission.request();
				request.missionId = mCurMissionData.ID;
				NetLogic.GetInstance().Send<Protocol.complete_mission>(request);
			}
			else
			{
				instance.MissionManager.MissionFindPath(mCurMissionData);
			}
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.SKILL_UPGRADE)
		{
			if (TutorialManager.IsTutorialCanShow() && Singleton<ObjManager>.Instance.MainPlayer.CheckSkillCanUpdate())
			{
				TutorialManager.ShowTutorial(TUTORIAL_STEP.SKILL_UPGRADE_START);
			}
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.EQUIP_UPGRADE)
		{
			if (TutorialManager.IsTutorialCanShow())
			{
				TutorialManager.ShowTutorial(TUTORIAL_STEP.ENHANCE_START);
			}
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.CAR_COPY)
		{
			if (TutorialManager.IsTutorialCanShow_ClickMission())
			{
				TutorialManager.ShowTutorial(TUTORIAL_STEP.CAR_COPY_START);
			}
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.STRENGTHEN_STAR)
		{
			if (TutorialManager.IsTutorialCanShow())
			{
				TutorialManager.ShowTutorial(TUTORIAL_STEP.STRENGTH_STAR_START);
			}
		}
		else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.DAILY_GUIDE)
		{
			if (TutorialManager.IsTutorialCanShow_ClickMission())
			{
				TutorialManager.ShowTutorial(TUTORIAL_STEP.DAILY_MISSION_CLICK_START);
			}
		}
		else
		{
			if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.TEAM_GUIDE)
			{
				return;
			}
			if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.EXP_COPY)
			{
				if (TutorialManager.IsTutorialCanShow_ClickMission())
				{
					TutorialManager.ShowTutorial(TUTORIAL_STEP.EXP_COPY_START);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.GOLD_COPY)
			{
				if (TutorialManager.IsTutorialCanShow_ClickMission())
				{
					TutorialManager.ShowTutorial(TUTORIAL_STEP.GOLD_COPY_START);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.TOWER_COPY)
			{
				if (TutorialManager.IsTutorialCanShow_ClickMission())
				{
					TutorialManager.ShowTutorial(TUTORIAL_STEP.TOWER_START);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.SCUFFLE_COPY)
			{
				if (TutorialManager.IsTutorialCanShow_ClickMission())
				{
					TutorialManager.ShowTutorial(TUTORIAL_STEP.SCUFFLE_COPY_START);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.GUILD_GUIDE)
			{
				if (TutorialManager.IsTutorialCanShow())
				{
					TutorialManager.ShowTutorial(TUTORIAL_STEP.GUILD_START);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.PVP_GUIDE)
			{
				if (TutorialManager.IsTutorialCanShow_ClickMission())
				{
					TutorialManager.ShowTutorial(TUTORIAL_STEP.RANK_PVP_START);
				}
			}
			else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.TITLE_GUIDE)
			{
				if (TutorialManager.IsTutorialCanShow())
				{
					TutorialManager.ShowTutorial(TUTORIAL_STEP.TITLE_START);
				}
			}
			else
			{
				if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.BADGE_GUIDE)
				{
					return;
				}
				if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.EQUIP_COPY)
				{
					if (TutorialManager.IsTutorialCanShow_ClickMission())
					{
						TutorialManager.ShowTutorial(TUTORIAL_STEP.EQUIP_COPY_START);
					}
				}
				else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.WORLD_BOSS)
				{
					if (TutorialManager.IsTutorialCanShow_ClickMission())
					{
						TutorialManager.ShowTutorial(TUTORIAL_STEP.WORLD_BOSS_START);
					}
				}
				else
				{
					if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.SELL_ITEM || mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.BAR_FIGHT)
					{
						return;
					}
					if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.ACCEPT_ESSCORT)
					{
						if (TutorialManager.IsTutorialCanShow_ClickMission())
						{
							TutorialManager.ShowTutorial(TUTORIAL_STEP.ESCORT_START);
						}
					}
					else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.DRAG_SKILL)
					{
						if (TutorialManager.IsTutorialCanShow())
						{
							TutorialManager.ShowTutorial(TUTORIAL_STEP.SKILL_DRAG_START);
						}
					}
					else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.ACCEPT_ROBBORY)
					{
						if (TutorialManager.IsTutorialCanShow_ClickMission())
						{
							TutorialManager.ShowTutorial(TUTORIAL_STEP.ROBBORY_START);
						}
					}
					else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.SURVIVAL_BATTLE)
					{
						if (TutorialManager.IsTutorialCanShow_ClickMission())
						{
							TutorialManager.ShowTutorial(TUTORIAL_STEP.SURVIVAL_BATTLE_START);
						}
					}
					else
					{
						if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.GUILD_BOSS)
						{
							return;
						}
						if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.SLOT)
						{
							if (TutorialManager.IsTutorialCanShow())
							{
								TutorialManager.ShowTutorial(TUTORIAL_STEP.SLOT_START);
							}
						}
						else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.CAPTURE)
						{
							if (TutorialManager.IsTutorialCanShow_ClickMission())
							{
								TutorialManager.ShowTutorial(TUTORIAL_STEP.CAPTURE_START);
							}
						}
						else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.UPGRADE_ARMOR)
						{
							ItemContainer equipPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipPack;
							GameItem item2 = equipPack.GetEquipByEquipType(EQUIP_BACKPACK_TYPE.BODY);
							if (item2 == null)
							{
								item2 = equipPack.GetEquipByEquipType(EQUIP_BACKPACK_TYPE.LEG);
							}
							if (item2 == null)
							{
								item2 = equipPack.GetEquipByEquipType(EQUIP_BACKPACK_TYPE.HEAD);
							}
							if (item2 == null)
							{
								NoticeLogic.AddNotifyData("#{200095}");
								return;
							}
							SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
							{
								SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance(item2);
							});
						}
						else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.UPGRADE_WEAPON)
						{
							SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
							{
								SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance();
							});
						}
						else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.UPGRADE_JEWELRY)
						{
							ItemContainer equipPack2 = instance.PlayerData.EquipPack;
							GameItem item = equipPack2.GetEquipByEquipType(EQUIP_BACKPACK_TYPE.BELT);
							if (item == null)
							{
								item = equipPack2.GetEquipByEquipType(EQUIP_BACKPACK_TYPE.NECKLACE);
							}
							if (item == null)
							{
								NoticeLogic.AddNotifyData("#{200096}");
								return;
							}
							SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
							{
								SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance(item);
							});
						}
						else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.UPGRADE_STARS)
						{
							SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
							{
								SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipRefine();
							});
						}
						else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.UPGRADE_BADGE)
						{
							SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
							{
								SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowBadgeMerge();
							});
						}
						else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.UNLOCK_VEHICLE)
						{
							SingletonUnity<FunctionBtnRootLogic>.Instance.OnClickCarBtn();
						}
						else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.UPGRADE_SKILL)
						{
							SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
							{
								SingletonUnity<EquipStrengthenUIRootLogic>.Instance.OnClickSkillBtn();
							});
						}
						else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.SINGLE_DANCE)
						{
							SingletonUnity<UIManager>.Instance.CloseAllPOPUI();
							SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AutoMoveDest("101", Vector3.right * -5f, AUTO_SEARCH_PARTH_FINISHEVENT.CITY_DANCE);
						}
						else if (mCurMissionData.MissionLogicType == MISSION_LOGICTYPE.COPYSCENE_KILLMONSTER && missionState == MISSION_STATE.ACCEPTED && string.IsNullOrEmpty(mCurMissionData.Target))
						{
							SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
							enter_copy_scene.request request2 = new enter_copy_scene.request();
							request2.mapInfoId = mCurMissionData.LogicID;
							NetLogic.GetInstance().Send<Protocol.enter_copy_scene>(request2);
						}
						else
						{
							instance.MissionManager.MissionFindPath(mCurMissionData);
						}
					}
				}
			}
		}
	}

	public long GetMissionRestTime(string missionId)
	{
		if (!IsMissionAccepted(missionId))
		{
			return -1L;
		}
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID == null || missionDataByID.Class != 8)
		{
			return -1L;
		}
		TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(missionDataByID.TimeLimitId);
		if (timeLimitMissionDataByID == null)
		{
			return -1L;
		}
		long missionParam = mCurMissionDictionary.GetMissionParam(missionId, 7);
		long limitTime = timeLimitMissionDataByID.LimitTime;
		long num = limitTime - (PlayerCommonData.GetServerTime() - missionParam);
		if (num < 0)
		{
			num = 0L;
		}
		return num;
	}
}
