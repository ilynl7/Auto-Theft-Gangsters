using System.Collections.Generic;
using UnityEngine;

public class AutoSearchPath
{
	private List<AutoSearchPathPoint> mPathPointList = new List<AutoSearchPathPoint>();

	public List<AutoSearchPathPoint> PathPointList => mPathPointList;

	public void ResetPath()
	{
		mPathPointList.Clear();
	}

	public void AddPathPoint(AutoSearchPathPoint point)
	{
		if (mPathPointList != null)
		{
			mPathPointList.Add(point);
		}
	}

	public bool IsFinish(Vector3 pos)
	{
		if (mPathPointList.Count > 1)
		{
			return false;
		}
		if (mPathPointList.Count == 1)
		{
			return IsArrivePoint(pos);
		}
		return true;
	}

	public bool IsArrivePoint(Vector3 pos)
	{
		if (mPathPointList.Count == 0)
		{
			return false;
		}
		AutoSearchPathPoint autoSearchPathPoint = mPathPointList[0];
		if (autoSearchPathPoint.SceneId.Equals(SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr) && VectorXZ.Distance(pos, new VectorXZ(autoSearchPathPoint.PosX, autoSearchPathPoint.PosZ)) < 1f)
		{
			return true;
		}
		return false;
	}
}
