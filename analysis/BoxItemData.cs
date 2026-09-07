using UnityEngine;

public class BoxItemData
{
	public enum BOXTYPE
	{
		BLOCK
	}

	public string ID;

	public string Name;

	public int Type;

	public float PositionX;

	public float PositionY;

	public float PositionZ;

	public float AngelX;

	public float AngelY;

	public float AngelZ;

	public Vector3 Position => new Vector3(PositionX, PositionY, PositionZ);

	public Vector3 Angel => new Vector3(AngelX, AngelY, AngelZ);
}
