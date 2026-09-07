using System;
using UnityEngine;

[Serializable]
public class CityPathPointData
{
	public Vector3 PointPos;

	public Vector3 PointForward;

	public Vector3 PointRight;

	public int[] LinkPointIndex = new int[4];

	public float[] LinkPointDis = new float[4];

	public bool IsWalkable = true;

	public bool IsCross;

	public bool IsFork;

	public bool IsNsCross;

	public bool IsFourLines;

	public float MinWalkDis;

	public float MaxWalkDis;

	public int SelfIndex;

	public Transform pathTrans;

	public float GetLinkDisByIndex(int index)
	{
		for (int i = 0; i < LinkPointIndex.Length; i++)
		{
			if (LinkPointIndex[i] == index)
			{
				return LinkPointDis[i];
			}
		}
		return 0f;
	}
}
