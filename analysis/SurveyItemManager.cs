using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SurveyItemManager : Singleton<SurveyItemManager>
{
	private string mCurMissionId = string.Empty;

	private SurveyItemObj mCurSurveyItem;

	private MissionManager mCacheMissionManager;

	private MissionManager mMissionManager
	{
		get
		{
			if (mCacheMissionManager == null)
			{
				mCacheMissionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			}
			return mCacheMissionManager;
		}
	}

	public void InitSurveyItem(string sceneId)
	{
		if (string.IsNullOrEmpty(sceneId) || sceneId.Equals(0.ToString()))
		{
			return;
		}
		ObjManager instance = Singleton<ObjManager>.Instance;
		List<SurveyMissionData> surveyMissionDataBySceneId = DataManager.GetSurveyMissionDataBySceneId(sceneId);
		for (int i = 0; i < surveyMissionDataBySceneId.Count; i++)
		{
			if (surveyMissionDataBySceneId[i].IsAlwaysExist == 1)
			{
				string surveyItemName = surveyMissionDataBySceneId[i].GetSurveyItemName();
				if (instance.FindOtherObjInDic(surveyItemName) == null)
				{
					instance.CreateSurveyItem(surveyMissionDataBySceneId[i]);
				}
			}
		}
		List<string> allMissionId = mMissionManager.GetAllMissionId();
		for (int j = 0; j < allMissionId.Count; j++)
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(allMissionId[j]);
			if (missionDataByID.MissionLogicType != MISSION_LOGICTYPE.SURVEY)
			{
				continue;
			}
			SurveyMissionData surveyMissionDataById = DataManager.GetSurveyMissionDataById(missionDataByID.LogicID);
			if (surveyMissionDataById.IsAlwaysExist == 1 || !surveyMissionDataById.SceneID.Equals(sceneId) || mMissionManager.GetMissionState(missionDataByID.ID) == MISSION_STATE.COMPLETE)
			{
				continue;
			}
			string surveyItemName2 = surveyMissionDataById.GetSurveyItemName();
			GameObject gameObject = instance.FindOtherObjInDic(surveyItemName2);
			if (gameObject == null)
			{
				instance.CreateSurveyItem(surveyMissionDataById);
				continue;
			}
			SurveyItemObj component = gameObject.GetComponent<SurveyItemObj>();
			if (component != null)
			{
				component.Reset(surveyMissionDataById);
			}
		}
	}

	public void StartSurveyItem(SurveyItemObj surveyItemObj)
	{
		if (surveyItemObj == null || !surveyItemObj.IsEnable())
		{
			return;
		}
		List<string> allMissionId = mMissionManager.GetAllMissionId();
		for (int i = 0; i < allMissionId.Count; i++)
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(allMissionId[i]);
			if (missionDataByID.MissionLogicType != MISSION_LOGICTYPE.SURVEY || mMissionManager.GetMissionState(missionDataByID.ID) == MISSION_STATE.COMPLETE)
			{
				continue;
			}
			SurveyMissionData surveyMissionDataById = DataManager.GetSurveyMissionDataById(missionDataByID.LogicID);
			if (!surveyMissionDataById.ID.Equals(surveyItemObj.SurveyMissionData.ID))
			{
				continue;
			}
			mCurMissionId = missionDataByID.ID;
			mCurSurveyItem = surveyItemObj;
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SurveyProgressLine, delegate(bool isSuccess, object param)
			{
				if (isSuccess)
				{
					int num = (int)mMissionManager.GetMissionParam(mCurMissionId, 0);
					SingletonUnity<SurveyProgressLineLogic>.Instance.Reset(surveyItemObj.SurveyMissionData, surveyItemObj.SurveyMissionData.NeedNum - num);
				}
			});
			if (!string.IsNullOrEmpty(surveyMissionDataById.ModelName))
			{
				CameraController cameraController = Singleton<ObjManager>.Instance.MainPlayer.CameraController;
				cameraController.IsCamCanUse = false;
				cameraController.IdealYaw = (surveyMissionDataById.PosO + 270f) % 360f;
				cameraController.IdealPitch = 15f;
				cameraController.smoothOrbitSpeed = 5f;
			}
		}
	}

	public void FinishSurveyItem()
	{
		if (mCurSurveyItem != null && !string.IsNullOrEmpty(mCurMissionId))
		{
			mCurSurveyItem.CollectOne();
			update_misison_parm.request request = new update_misison_parm.request();
			request.missionId = mCurMissionId;
			request.paramType = 1L;
			request.paramValue = 1L;
			NetLogic.GetInstance().Send<Protocol.update_misison_parm>(request);
		}
	}

	public void CompleteSurveyItem()
	{
		if (mCurSurveyItem.MeshRoot != null)
		{
			mCurSurveyItem.PlayEffect();
			vp_Timer.In(mCurSurveyItem.SurveyMissionData.DelayRecycleTime, delegate
			{
				if (mCurSurveyItem != null)
				{
					Singleton<ObjManager>.Instance.RecycleSurveyItem(mCurSurveyItem);
				}
			});
		}
		else
		{
			Singleton<ObjManager>.Instance.RecycleSurveyItem(mCurSurveyItem);
		}
	}
}
