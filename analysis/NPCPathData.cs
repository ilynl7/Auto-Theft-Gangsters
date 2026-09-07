using UnityEngine;

public class NPCPathData
{
	public string ID = string.Empty;

	public int PointIndex = -1;

	public int PosX;

	public int PosZ;

	public float PositionX => (float)PosX / 100f;

	public float PositionZ => (float)PosZ / 100f;

	public Vector3 Position => new Vector3(PositionX, 0f, PositionZ);
}
