using System.Collections.Generic;
using UnityEngine;

public class ActivityBossData
{
	public string Key;

	public string ID;

	public string MapID;

	public string NpcID;

	public string NpcPos;

	public int ReBirthTime;

	public string DropID1;

	public int Rank1;

	public string DropID2;

	public int Rank2;

	public string DropID3;

	public int Rank3;

	public string DropID4;

	public int Rank4;

	public string DropID5;

	public int Rank5;

	public string DropID6;

	public int Rank6;

	private List<Vector3> mNpcPosList = new List<Vector3>();

	public List<Vector3> NpcPosList
	{
		get
		{
			if (mNpcPosList.Count == 0)
			{
				string[] array = NpcPos.Split('#');
				int[] array2 = new int[array.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = int.Parse(array[i]);
				}
				for (int j = 0; j < array2.Length / 2; j++)
				{
					mNpcPosList.Add(new Vector3((float)array2[j * 2] / 100f, 0f, (float)array2[j * 2 + 1] / 100f));
				}
			}
			return mNpcPosList;
		}
	}
}
