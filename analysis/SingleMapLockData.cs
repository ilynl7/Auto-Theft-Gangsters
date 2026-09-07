using UnityEngine;

public class SingleMapLockData
{
	public int ID = -1;

	public int UnlockLevel;

	public string AreaName = string.Empty;

	public float UnlockAlph = 1.1f;

	public int UIPosX;

	public int UIPosZ;

	public Vector3 UIPos => new Vector3((float)UIPosX / 100f, 0f, (float)UIPosZ / 100f);
}
