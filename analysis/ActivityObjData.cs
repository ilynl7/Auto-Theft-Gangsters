using SprotoType;
using UnityEngine;

public class ActivityObjData
{
	public ActivityMapData ActMapData;

	public MissionData MisData;

	public Vector3 Position;

	public bool IsMissionObj;

	public MISSION_STATE MissionState = MISSION_STATE.INVALID;

	public string MissionId
	{
		get
		{
			if (MisData == null)
			{
				return string.Empty;
			}
			return MisData.ID;
		}
		set
		{
			MisData = DataManager.GetMissionDataByID(value);
		}
	}

	public ActivityObjData(ActivityMapData actMapData)
	{
		ActMapData = actMapData;
		IsMissionObj = false;
		Position = ActMapData.Position;
		if (actMapData.ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
		{
			MisData = DataManager.GetMissionDataByID(actMapData.ActivityID);
		}
	}

	public ActivityObjData(string missionId, MISSION_STATE misState, Vector3 pos)
	{
		IsMissionObj = true;
		MissionId = missionId;
		MissionState = misState;
		Position = pos;
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID != null && missionDataByID.Class == 1 && missionDataByID.MissionLogicType == MISSION_LOGICTYPE.CAPTURE_SUCCESS)
		{
			DominData dominDataByID = DataManager.GetDominDataByID(missionDataByID.LogicID);
			if (dominDataByID != null)
			{
				Position = dominDataByID.GetPos();
			}
		}
	}

	public string GetIcon()
	{
		if (IsMissionObj)
		{
			return GameDefine.MAP_ACTIVITY_MISSION_ICON[MisData.Class];
		}
		if (ActMapData.ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
		{
			return GameDefine.MAP_ACTIVITY_MISSION_ICON[MisData.Class];
		}
		if (ActMapData.ActivityType == GameDefine.ACTIVITY_TYPE.DOMIN)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (playerData.Domin_InfoDic.ContainsKey(ActMapData.ActivityID))
			{
				domin_info domin_info = playerData.Domin_InfoDic[ActMapData.ActivityID];
				if (domin_info.state == 0L && domin_info.HasServerId && playerData.Domin_CharacterDic.ContainsKey(domin_info.serverId))
				{
					return GameDefine.Player_Icon_Small_Pic[playerData.Domin_CharacterDic[domin_info.serverId].general.profession];
				}
				return GameDefine.Player_Icon_Small_Pic[(int)playerData.Profession];
			}
			return string.Empty;
		}
		return ActMapData.Icon;
	}

	public string GetStateIcon()
	{
		if (IsMissionObj)
		{
			return GameDefine.GetMissionStateIcon(MissionState);
		}
		if (ActMapData.ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
		{
			return GameDefine.GetMissionStateIcon(MISSION_STATE.INVALID);
		}
		if (ActMapData.IsUnlock)
		{
			return string.Empty;
		}
		return "CZ_effect_suo";
	}

	public bool IsMission()
	{
		if (IsMissionObj)
		{
			return true;
		}
		if (ActMapData.ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
		{
			return true;
		}
		return false;
	}
}
