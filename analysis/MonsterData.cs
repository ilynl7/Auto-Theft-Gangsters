using UnityEngine;

public class MonsterData
{
	public string MapID;

	public int Group;

	public string NpcID;

	public int PosX;

	public int PosZ;

	public int PosO;

	public int ReBirthType;

	public int ReBirthTime;

	public string PathId;

	public float PositionX => (float)PosX / 100f;

	public float PositionZ => (float)PosZ / 100f;

	public float PositionO => (float)PosO / 100f;

	public Vector3 GetNpcPos()
	{
		return new Vector3(PositionX, SceneManager.GetHitHeight(new Vector3(PositionX, 0f, PositionZ)), PositionZ);
	}

	public Vector3 GetNpcXZPos()
	{
		return new Vector3(PositionX, 0f, PositionZ);
	}
}
