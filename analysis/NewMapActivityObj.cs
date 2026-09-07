using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class NewMapActivityObj : MonoBehaviour
{
	public UISprite IconPic;

	public UISprite LockPic;

	public GameObject ActiveObj;

	private ActivityMapData mCurActivityMapData;

	private bool isUnLock;

	private MissionData mCurMissionData;

	private List<string> mMissionIdList;

	private bool isMissionObj;

	public bool IsMissionObj => isMissionObj;

	public void Reset(ActivityMapData actMapData)
	{
		isMissionObj = false;
		mCurActivityMapData = actMapData;
		if (mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.DOMIN)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (!playerData.Domin_InfoDic.ContainsKey(actMapData.ActivityID))
			{
				NGUITools.SetActive(base.gameObject, state: false);
				return;
			}
			domin_info domin_info = playerData.Domin_InfoDic[actMapData.ActivityID];
			if (domin_info.state == 0L && domin_info.HasServerId && playerData.Domin_CharacterDic.ContainsKey(domin_info.serverId))
			{
				IconPic.spriteName = GameDefine.Player_Icon_Small_Pic[playerData.Domin_CharacterDic[domin_info.serverId].general.profession];
			}
			else
			{
				IconPic.spriteName = GameDefine.Player_Icon_Small_Pic[(int)playerData.Profession];
			}
		}
		else
		{
			IconPic.spriteName = actMapData.Icon;
		}
		mMissionIdList = null;
		isUnLock = mCurActivityMapData.IsUnlock;
		IconPic.MakePixelPerfect();
		if (mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
		{
			NGUITools.SetActive(LockPic.gameObject, state: true);
			LockPic.spriteName = GameDefine.GetMissionStateIcon(MISSION_STATE.INVALID);
			LockPic.pivot = UIWidget.Pivot.Left;
			LockPic.MakePixelPerfect();
			LockPic.width = (int)((float)LockPic.width * 0.6f);
			LockPic.height = (int)((float)LockPic.height * 0.6f);
			LockPic.transform.localPosition = new Vector3(10f, 0f, 0f);
			LockPic.color = Color.white;
		}
		else if (isUnLock)
		{
			NGUITools.SetActive(LockPic.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(LockPic.gameObject, state: true);
			LockPic.spriteName = "CZ_effect_suo";
			LockPic.pivot = UIWidget.Pivot.Center;
			LockPic.transform.localPosition = Vector3.zero;
			LockPic.color = Color.red;
			LockPic.width = 43;
			LockPic.height = 43;
		}
		SetNormalState();
		base.transform.localScale = Vector3.one * 0.5f;
	}

	public void Reset(MissionData missionData, List<string> missionIdList)
	{
		isMissionObj = true;
		mCurMissionData = missionData;
		mMissionIdList = missionIdList;
		IconPic.spriteName = GameDefine.MAP_ACTIVITY_MISSION_ICON[missionData.Class];
		IconPic.MakePixelPerfect();
		NGUITools.SetActive(LockPic.gameObject, state: true);
		MISSION_STATE missionState = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetMissionState(missionData.ID);
		LockPic.spriteName = GameDefine.GetMissionStateIcon(missionState);
		LockPic.pivot = UIWidget.Pivot.Left;
		LockPic.MakePixelPerfect();
		LockPic.width = (int)((float)LockPic.width * 0.6f);
		LockPic.height = (int)((float)LockPic.height * 0.6f);
		LockPic.transform.localPosition = new Vector3(10f, 0f, 0f);
		LockPic.color = Color.white;
		SetNormalState();
		base.transform.localScale = Vector3.one * 0.5f;
	}

	public void OnClickBtn()
	{
		if (UnityVersionUtil.IsActive(ActiveObj.gameObject))
		{
			return;
		}
		if (isUnLock)
		{
			NewMapUIRootLogic instance = SingletonUnity<NewMapUIRootLogic>.Instance;
			if (instance != null && UnityVersionUtil.IsActive(instance.gameObject) && instance.CurMapInfo.ID.Equals(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID))
			{
				instance.OnClickLocalMap();
			}
		}
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (mCurActivityMapData != null)
		{
			if (!mCurActivityMapData.CheckCanGoTo())
			{
				return;
			}
			if (mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
			{
				if (SingletonUnity<NewMissionUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMissionUIRootLogic>.Instance.gameObject))
				{
					SingletonUnity<NewMissionUIRootLogic>.Instance.ClickTargetMission(mCurActivityMapData.ActivityID);
				}
				else
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickMissionBtn(mCurActivityMapData.ActivityID, SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID);
				}
			}
			else if (mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.DAILY_COPY || mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.SEX_MINI)
			{
				if (mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.SEX_MINI)
				{
					mCurActivityMapData.SubType = 27;
				}
				if (SingletonUnity<NewDailyCopyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyCopyUIRootLogic>.Instance.gameObject))
				{
					if (mCurActivityMapData.IsNeedDailyActid())
					{
						SingletonUnity<NewDailyCopyUIRootLogic>.Instance.ClickTargetDailyCopy((MAPTYPE)mCurActivityMapData.SubType, mCurActivityMapData.ActivityID);
					}
					else
					{
						SingletonUnity<NewDailyCopyUIRootLogic>.Instance.ClickTargetDailyCopy((MAPTYPE)mCurActivityMapData.SubType);
					}
				}
				else if (mCurActivityMapData.IsNeedDailyActid())
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn((MAPTYPE)mCurActivityMapData.SubType, mCurActivityMapData.ActivityID, SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID);
				}
				else
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn((MAPTYPE)mCurActivityMapData.SubType, null, SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID);
				}
			}
			else if (mCurActivityMapData.IsInTimeActivityUI())
			{
				if (SingletonUnity<NewDailyActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyActivityUIRootLogic>.Instance.gameObject))
				{
					if (mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.WILD_BOSS)
					{
						SingletonUnity<NewDailyActivityUIRootLogic>.Instance.WildBossLineList[0].ClickTargetBtn();
						return;
					}
					if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
					{
						SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjDisable();
					}
					SingletonUnity<NewDailyActivityUIRootLogic>.Instance.ClickTargetType(mCurActivityMapData.ActivityType);
					SetActiveState();
				}
				else
				{
					if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
					{
						SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjDisable();
					}
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(mCurActivityMapData.ActivityType, SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID, isMapClick: true);
					SetActiveState();
				}
			}
			else if (mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BOSS || mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.GUILD_BATTLE)
			{
				if (SingletonUnity<NewDailyActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyActivityUIRootLogic>.Instance.gameObject))
				{
					SingletonUnity<NewDailyActivityUIRootLogic>.Instance.OnClickTargetType(mCurActivityMapData.ActivityType);
					return;
				}
				if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
				{
					SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjDisable();
				}
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(mCurActivityMapData.ActivityType, SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID, isMapClick: true);
				SetActiveState();
			}
			else if (mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.TOWER)
			{
				if (SingletonUnity<NewDailyCopyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyCopyUIRootLogic>.Instance.gameObject))
				{
					SingletonUnity<NewDailyCopyUIRootLogic>.Instance.ClickTargetType(GameDefine.ACTIVITY_TYPE.TOWER);
				}
				else
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.INVALID, null, SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID, GameDefine.ACTIVITY_TYPE.TOWER);
				}
			}
			else if (mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.RANKPVP)
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickRankBtn();
			}
			else if (mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.DOMIN)
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDominBtn(mCurActivityMapData.ActivityID);
			}
		}
		else if (mCurMissionData != null)
		{
			if (SingletonUnity<NewMissionUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMissionUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewMissionUIRootLogic>.Instance.ClickTargetMission(mCurMissionData.ID);
			}
			else
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickMissionBtn(mCurMissionData.ID, SingletonUnity<NewMapUIRootLogic>.Instance.CurMapInfo.ID);
			}
		}
	}

	public void SetActiveState()
	{
		IconPic.color = Color.white;
		NGUITools.SetActive(ActiveObj, state: true);
		IconPic.depth = 81;
		LockPic.depth = 82;
	}

	public void SetDisActiveState()
	{
		IconPic.color = Color.gray;
		IconPic.alpha = 0.7f;
		NGUITools.SetActive(ActiveObj, state: false);
		IconPic.depth = 71;
		LockPic.depth = 72;
	}

	public void SetNormalState()
	{
		IconPic.color = Color.white;
		IconPic.alpha = 0.7f;
		NGUITools.SetActive(ActiveObj, state: false);
		IconPic.depth = 71;
		LockPic.depth = 72;
	}

	public void UpdateSelection(int actType, int subType, string actId)
	{
		if (!isMissionObj)
		{
			if (mCurActivityMapData.Type == actType && mCurActivityMapData.SubType == subType)
			{
				if (string.IsNullOrEmpty(actId))
				{
					SetActiveState();
				}
				else if (mCurActivityMapData.ActivityID.Equals(actId))
				{
					SetActiveState();
				}
				else
				{
					SetDisActiveState();
				}
			}
			else
			{
				SetDisActiveState();
			}
		}
		else
		{
			SetDisActiveState();
		}
	}

	public void UpdateSelection(string actId, bool isMission)
	{
		if (isMission)
		{
			if (isMissionObj)
			{
				if (mMissionIdList != null)
				{
					if (mMissionIdList.Contains(actId))
					{
						SetActiveState();
					}
					else
					{
						SetDisActiveState();
					}
				}
				else if (mCurMissionData.ID.Equals(actId))
				{
					SetActiveState();
				}
				else
				{
					SetDisActiveState();
				}
			}
			else if (mCurActivityMapData.ActivityType == GameDefine.ACTIVITY_TYPE.MISSION)
			{
				if (mCurActivityMapData.ActivityID.Equals(actId))
				{
					SetActiveState();
				}
				else
				{
					SetDisActiveState();
				}
			}
			else
			{
				SetDisActiveState();
			}
		}
		else if (isMissionObj)
		{
			SetDisActiveState();
		}
		else if (mCurActivityMapData.ActivityID.Equals(actId))
		{
			SetActiveState();
		}
		else
		{
			SetDisActiveState();
		}
	}
}
