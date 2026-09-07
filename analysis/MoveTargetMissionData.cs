using System.Collections.Generic;
using UnityEngine;

public class MoveTargetMissionData
{
	public string ID = string.Empty;

	public string MapId = string.Empty;

	public int TargetNum;

	public string TargetPoint = string.Empty;

	public int LimitTime = 120;

	private List<Vector3> mTargetPointList;

	public List<Vector3> TargetPointList
	{
		get
		{
			if (mTargetPointList == null)
			{
				mTargetPointList = new List<Vector3>();
				string[] array = TargetPoint.Split('|');
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split('#');
					int num = int.Parse(array2[0]);
					int num2 = int.Parse(array2[1]);
					Vector3 item = new Vector3((float)num / 100f, 0f, (float)num2 / 100f);
					mTargetPointList.Add(item);
				}
			}
			return mTargetPointList;
		}
	}
}
