using System.Collections.Generic;
using UnityEngine;

public class EasyCitySimController : SingletonUnity<EasyCitySimController>
{
	public List<Vector3> PointPosList;

	public List<int>[] PointLinkList;

	public Vector2 CityLeftBottomPos;

	public Vector2 CitySize;

	public int BlockSize;

	public List<int>[][] BlockData;

	public bool runFlag;

	private List<string> mNpcIDList = new List<string> { "201", "202", "204", "206" };

	private List<int> mLastCreatePointList = new List<int>();

	private bool flag;

	private int difIndex = 3;

	private new void Awake()
	{
		base.Awake();
		InitPath();
	}

	private void InitPath()
	{
		if (PointLinkList == null)
		{
			PointLinkList = new List<int>[PointPosList.Count];
		}
		BlockData = new List<int>[(int)CitySize.x / BlockSize][];
		for (int i = 0; i < BlockData.Length; i++)
		{
			BlockData[i] = new List<int>[(int)CitySize.y / BlockSize];
		}
		for (int j = 0; j < PointPosList.Count; j++)
		{
			GetBlockPos(PointPosList[j], out var x, out var y);
			if (BlockData[x][y] == null)
			{
				BlockData[x][y] = new List<int>();
			}
			BlockData[x][y].Add(j);
		}
		List<int> list = new List<int>();
		for (int k = 0; k < PointPosList.Count; k++)
		{
			list.Clear();
			GetBlockPos(PointPosList[k], out var x2, out var y2);
			int num = x2 - 1;
			int num2 = x2 + 1;
			int num3 = y2 - 1;
			int num4 = y2 + 1;
			for (int l = num; l <= num2; l++)
			{
				if (l < 0 || l >= BlockData.Length)
				{
					continue;
				}
				for (int m = num3; m <= num4; m++)
				{
					if (m >= 0 && m < BlockData[l].Length && BlockData[l][m] != null)
					{
						for (int n = 0; n < BlockData[l][m].Count; n++)
						{
							list.Add(BlockData[l][m][n]);
						}
					}
				}
			}
			if (list.Count > 0 && PointLinkList[k] == null)
			{
				PointLinkList[k] = new List<int>();
			}
			for (int num5 = 0; num5 < list.Count; num5++)
			{
				if (k != list[num5] && Vector3.SqrMagnitude(PointPosList[k] - PointPosList[list[num5]]) < 900f)
				{
					PointLinkList[k].Add(list[num5]);
				}
			}
		}
	}

	public void GetBlockPos(Vector3 pos, out int x, out int y)
	{
		x = (int)(pos.x - CityLeftBottomPos.x) / BlockSize;
		y = (int)(pos.z - CityLeftBottomPos.y) / BlockSize;
	}

	public void FreshNPC(Transform targetTrans)
	{
		ObjManager instance = Singleton<ObjManager>.Instance;
		GetBlockPos(targetTrans.position, out var x, out var y);
		List<int> list = new List<int>();
		list.Clear();
		GetGeneRatePoint(x, y, list);
		mLastCreatePointList.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			if (PointLinkList[list[i]] == null || PointLinkList[list[i]].Count == 0)
			{
				continue;
			}
			Vector3 vector = targetTrans.InverseTransformPoint(PointPosList[list[i]]);
			if (vector.z < 0f || Mathf.Abs(vector.x) > 50f)
			{
				continue;
			}
			mLastCreatePointList.Add(list[i]);
			NpcData npcDataByID = DataManager.GetNpcDataByID(mNpcIDList[Random.Range(0, mNpcIDList.Count)]);
			int linkPoint = PointLinkList[list[i]][Random.Range(0, PointLinkList[list[i]].Count)];
			ObjInitNpcData objInitNpcData = new ObjInitNpcData();
			objInitNpcData.mServerID = UUID.GenUUID();
			objInitNpcData.mPos = PointPosList[list[i]];
			objInitNpcData.mDir = (PointPosList[linkPoint] - objInitNpcData.mPos).normalized;
			objInitNpcData.HP = npcDataByID.Hp;
			objInitNpcData.MaxHP = npcDataByID.Hp;
			objInitNpcData.npcInfoData = npcDataByID;
			objInitNpcData.mCharacterModelId = npcDataByID.Model;
			instance.GetRagdollNPC(objInitNpcData, delegate(ObjNPC npc)
			{
				if (npc != null)
				{
					if (Random.Range(0, 100) > 50)
					{
						npc.WalkMoveTo(PointPosList[linkPoint], 3f, delegate
						{
							npc.MoveTo(PointPosList[GetNextPoint(linkPoint)]);
						});
					}
					else
					{
						npc.MoveTo(PointPosList[linkPoint], 3f, delegate
						{
							npc.WalkMoveTo(PointPosList[GetNextPoint(linkPoint)]);
						});
					}
				}
			});
		}
	}

	private int GetNextPoint(int curPoint)
	{
		if (PointLinkList != null)
		{
			return PointLinkList[curPoint][Random.Range(0, PointLinkList[curPoint].Count)];
		}
		return 0;
	}

	private void AddPoint(int x, int y, List<int> pointList)
	{
		if (BlockData[x][y] == null)
		{
			return;
		}
		for (int i = 0; i < BlockData[x][y].Count; i++)
		{
			if (Random.Range(0, 100) > 50 && !mLastCreatePointList.Contains(BlockData[x][y][i]))
			{
				pointList.Add(BlockData[x][y][i]);
			}
		}
	}

	private void GetGeneRatePoint(int x, int y, List<int> pointList)
	{
		int num = x - difIndex;
		int num2 = x + difIndex;
		int num3 = y + difIndex;
		int num4 = y - difIndex;
		for (int i = num; i < num2 + 1; i++)
		{
			if (i < 0 || i >= BlockData.Length)
			{
				continue;
			}
			for (int j = num4; j < num3 + 1; j++)
			{
				if (j >= 0 && j < BlockData[i].Length && (i == num || i == num2 || j == num4 || j == num3))
				{
					AddPoint(i, j, pointList);
				}
			}
		}
	}
}
