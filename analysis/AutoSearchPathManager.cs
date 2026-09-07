using System.Collections.Generic;
using UnityEngine;

public class AutoSearchPathManager
{
	private class PathNode
	{
		public string SceneId;

		public int Dis;

		public string PreNode;

		public PathNode()
		{
			SceneId = string.Empty;
			Dis = int.MaxValue;
			PreNode = string.Empty;
		}
	}

	private AutoSearchPathPoint mCurTargetPoint = new AutoSearchPathPoint("-1", 0f, 0f, 0f);

	private AutoSearchPath mCurPath = new AutoSearchPath();

	private string mCurMissionId = string.Empty;

	private AUTO_SEARCH_PARTH_FINISHEVENT mFinishEventType = AUTO_SEARCH_PARTH_FINISHEVENT.INVALID;

	private bool mIsAutoMovingFlag;

	private bool mIsInTelePortCircle = true;

	private ObjMainPlayer mainPlayer;

	private List<MapConnectInfoData> mMapConnectInfoList;

	public AutoSearchPath CurPath => mCurPath;

	public string CurMissionId
	{
		get
		{
			return mCurMissionId;
		}
		set
		{
			mCurMissionId = value;
		}
	}

	public AUTO_SEARCH_PARTH_FINISHEVENT FinishEventType
	{
		get
		{
			return mFinishEventType;
		}
		set
		{
			mFinishEventType = value;
		}
	}

	public bool IsAutoMovingFlag
	{
		get
		{
			return mIsAutoMovingFlag;
		}
		set
		{
			mIsAutoMovingFlag = value;
		}
	}

	public bool IsInTelePortCircle
	{
		get
		{
			return mIsInTelePortCircle;
		}
		set
		{
			mIsInTelePortCircle = value;
		}
	}

	public string TargetSceneId => mCurTargetPoint.SceneId;

	public void FindPath(string sceneId, float posX, float posY, float posZ, AUTO_SEARCH_PARTH_FINISHEVENT finishEvent)
	{
		FindPath(new AutoSearchPathPoint(sceneId, posX, posY, posZ), finishEvent);
	}

	public void FindPath(AutoSearchPathPoint targetPoint, AUTO_SEARCH_PARTH_FINISHEVENT finishEvent, string missionId = null)
	{
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		if (mCurTargetPoint.Equal(targetPoint) && mCurPath.PathPointList.Count > 0 && mCurPath.PathPointList[0].SceneId.Equals(instance.RunningMapIdStr))
		{
			mIsAutoMovingFlag = true;
			return;
		}
		mCurTargetPoint = targetPoint;
		mCurPath.ResetPath();
		mCurMissionId = missionId;
		mFinishEventType = finishEvent;
		ObjMainPlayer objMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (objMainPlayer == null)
		{
			Debug.Log("if(mainPlayer == null)");
		}
		else if (instance.RunningMapIdStr.Equals(mCurTargetPoint.SceneId))
		{
			mCurPath.AddPathPoint(mCurTargetPoint);
			mIsAutoMovingFlag = true;
			if (Vector3.Distance(new Vector3(mCurTargetPoint.PosX, mCurTargetPoint.PosY, mCurTargetPoint.PosZ), objMainPlayer.Position) / objMainPlayer.NavMeshAgent.speed > 5f)
			{
				objMainPlayer.EnterAutoMoving(Time.time);
			}
			else
			{
				objMainPlayer.EnterAutoMoving(float.MaxValue);
			}
		}
		else
		{
			AutoSearchPathPoint startPoint = AutoSearchPathPoint.CreatePoint(objMainPlayer.gameObject);
			if (FindPath(startPoint, targetPoint))
			{
				mIsAutoMovingFlag = true;
				objMainPlayer.EnterAutoMoving(Time.time);
			}
		}
	}

	private bool FindPath(AutoSearchPathPoint startPoint, AutoSearchPathPoint targetPoint)
	{
		MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(startPoint.SceneId);
		AutoSearchPathPoint point = new AutoSearchPathPoint(mapInfoDataByID.ID, mapInfoDataByID.TelePortPosVector3);
		mCurPath.AddPathPoint(point);
		mCurPath.AddPathPoint(targetPoint);
		return true;
	}

	public void InitMapConnection()
	{
		if (mMapConnectInfoList == null)
		{
			mMapConnectInfoList = DataManager.GetMapConnectData();
		}
	}

	private int GetDisBySceneId(string resId, string tarId)
	{
		for (int i = 0; i < mMapConnectInfoList.Count; i++)
		{
			if (mMapConnectInfoList[i].SourceSceneId.Equals(resId) && mMapConnectInfoList[i].TargetSceneId.Equals(tarId))
			{
				return 1;
			}
		}
		return int.MaxValue;
	}

	public void Finish()
	{
		mCurMissionId = null;
		mCurPath.ResetPath();
		mCurTargetPoint.Reset();
		mIsAutoMovingFlag = false;
		mFinishEventType = AUTO_SEARCH_PARTH_FINISHEVENT.INVALID;
	}
}
