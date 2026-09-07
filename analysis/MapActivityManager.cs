using System.Collections.Generic;
using UnityEngine;

public class MapActivityManager : MonoBehaviour
{
	private List<ActivityMapData> CurActivityMapDataList = new List<ActivityMapData>();

	public Dictionary<string, ActivityPoint> EnableDic = new Dictionary<string, ActivityPoint>();

	private List<string> CurShowList = new List<string>();

	private Transform ParentTra;

	public void Reset()
	{
		CurActivityMapDataList.Clear();
		EnableDic.Clear();
		CurShowList.Clear();
	}

	public void InitMapInfo(List<ActivityMapData> curdatalist, Transform parenttra)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		CurActivityMapDataList.Clear();
		EnableDic.Clear();
		CurShowList.Clear();
		for (int i = 0; i < curdatalist.Count; i++)
		{
			if (curdatalist[i].IsVisible)
			{
				CurActivityMapDataList.Add(curdatalist[i]);
			}
		}
		ParentTra = parenttra;
		if (CurActivityMapDataList == null || CurActivityMapDataList.Count == 0)
		{
			return;
		}
		for (int j = 0; j < CurActivityMapDataList.Count; j++)
		{
			if (!CheckIsHaveAndEnbale(CurActivityMapDataList[j]))
			{
				InitObj(CurActivityMapDataList[j]);
			}
		}
	}

	public void RefershMapInfo(List<ActivityMapData> curdatalist, Transform parenttra)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		CurActivityMapDataList.Clear();
		for (int i = 0; i < curdatalist.Count; i++)
		{
			if (curdatalist[i].IsVisible)
			{
				CurActivityMapDataList.Add(curdatalist[i]);
			}
		}
		ParentTra = parenttra;
		if (CurActivityMapDataList == null || CurActivityMapDataList.Count == 0)
		{
			return;
		}
		for (int j = 0; j < CurActivityMapDataList.Count; j++)
		{
			if (!CheckIsHaveAndEnbale(CurActivityMapDataList[j]))
			{
				InitObj(CurActivityMapDataList[j]);
			}
		}
	}

	public void AcceptMapActivity(string missionid)
	{
		List<ActivityPoint> list = new List<ActivityPoint>(EnableDic.Values);
		List<string> list2 = new List<string>();
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionid);
		bool flag = false;
		if (missionDataByID.Class == 8)
		{
			flag = true;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].CurActData.ActivityType != GameDefine.ACTIVITY_TYPE.MISSION)
			{
				continue;
			}
			if (flag)
			{
				MissionData missionDataByID2 = DataManager.GetMissionDataByID(list[i].CurActData.ActivityID);
				if (missionDataByID2.Class == 8)
				{
					list2.Add(list[i].ID);
				}
			}
			else if (list[i].CurActData.ActivityID.Equals(missionid))
			{
				list2.Add(list[i].ID);
			}
		}
		for (int j = 0; j < list2.Count; j++)
		{
			string text = list2[j];
			if (!string.IsNullOrEmpty(text))
			{
				if (EnableDic.ContainsKey(text))
				{
					Object.Destroy(EnableDic[text].gameObject);
					EnableDic.Remove(text);
				}
				CurShowList.Remove(text);
			}
		}
		if (list2.Count > 0 && !UIManager.IsUnlockTutorialEnable())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.UnlockFunctionRoot, delegate
			{
				SingletonUnity<UnlockFunctionRootLogic>.Instance.ShowAcceptMissionEffect(missionid);
			});
		}
	}

	public void UpdateTimeActivityMapFlag()
	{
		UpdateActivityMapFlag(GameDefine.ACTIVITY_TYPE.ESCORT);
		UpdateActivityMapFlag(GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT);
		UpdateActivityMapFlag(GameDefine.ACTIVITY_TYPE.CITY_DANCE);
		UpdateActivityMapFlag(GameDefine.ACTIVITY_TYPE.BAR_FIGHT);
		UpdateActivityMapFlag(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE);
		UpdateActivityMapFlag(GameDefine.ACTIVITY_TYPE.WILD_BOSS);
		UpdateActivityMapFlag(GameDefine.ACTIVITY_TYPE.GUILD_BOSS);
		UpdateActivityMapFlag(GameDefine.ACTIVITY_TYPE.GUILD_BATTLE);
	}

	public void UpdateActivityMapFlag(GameDefine.ACTIVITY_TYPE type)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ResetActivityTips(playerData.ActivityData.IsTimeActivityCanPlay(type), type);
	}

	public void ResetActivityTips(bool isOpen, GameDefine.ACTIVITY_TYPE type = GameDefine.ACTIVITY_TYPE.INVALID)
	{
		List<ActivityPoint> list = new List<ActivityPoint>(EnableDic.Values);
		List<ActivityPoint> list2 = new List<ActivityPoint>();
		string empty = string.Empty;
		if (list != null && list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].CurActData.ActivityType == type)
				{
					list2.Add(list[i]);
				}
			}
		}
		for (int j = 0; j < list2.Count; j++)
		{
			ActivityPoint activityPoint = list2[j];
			empty = activityPoint.CurActData.ID;
			if (activityPoint != null && activityPoint.IsEnable != isOpen)
			{
				ActivityMapData curActData = activityPoint.CurActData;
				if (EnableDic.ContainsKey(empty))
				{
					Object.Destroy(EnableDic[empty].gameObject);
					EnableDic.Remove(empty);
				}
				CurShowList.Remove(empty);
				InitObj(curActData);
			}
		}
	}

	public bool CheckIsHaveAndEnbale(ActivityMapData needdata)
	{
		if (EnableDic.ContainsKey(needdata.ID))
		{
			ActivityPoint activityPoint = EnableDic[needdata.ID];
			bool flag = true;
			if (needdata.IsUnlock)
			{
				if (needdata.IsTimeActivity())
				{
					PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
					if (!playerData.ActivityData.IsTimeActivityCanPlay(needdata.ActivityType))
					{
						flag = false;
					}
				}
			}
			else
			{
				flag = false;
			}
			if (activityPoint != null && activityPoint.IsEnable == flag)
			{
				return true;
			}
			Object.Destroy(EnableDic[needdata.ID].gameObject);
			EnableDic.Remove(needdata.ID);
			CurShowList.Remove(needdata.ID);
			return false;
		}
		return false;
	}

	public void InitObj(ActivityMapData initdata)
	{
		GameObject gameObject = null;
		bool isenable = true;
		if (initdata.Color != 0 || initdata.IsShowDoorFlag())
		{
			if (initdata.IsShowDoorFlag())
			{
				gameObject = ResourcesManager.LoadAndInstantiate("Items/City_jinRu") as GameObject;
			}
			else if (initdata.IsUnlock)
			{
				if (initdata.IsTimeActivity())
				{
					PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
					if (playerData.ActivityData.IsTimeActivityCanPlay(initdata.ActivityType))
					{
						gameObject = ResourcesManager.LoadAndInstantiate("Items/" + GameDefine.ACTIVITYPOINTNAME[initdata.Color]) as GameObject;
					}
					else
					{
						isenable = false;
						gameObject = ResourcesManager.LoadAndInstantiate("Items/Activity_Point_6") as GameObject;
					}
				}
				else
				{
					gameObject = ResourcesManager.LoadAndInstantiate("Items/" + GameDefine.ACTIVITYPOINTNAME[initdata.Color]) as GameObject;
				}
			}
			else
			{
				isenable = false;
				gameObject = ResourcesManager.LoadAndInstantiate("Items/Activity_Point_6") as GameObject;
			}
		}
		if (gameObject != null)
		{
			ActivityPoint activityPoint = null;
			activityPoint = gameObject.GetComponent<ActivityPoint>();
			if (activityPoint != null)
			{
				activityPoint.Reset(initdata, ParentTra, isenable);
				EnableDic.Add(initdata.ID, activityPoint);
				CurShowList.Add(initdata.ID);
			}
		}
	}
}
