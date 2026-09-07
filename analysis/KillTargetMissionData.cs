using UnityEngine;

public class KillTargetMissionData
{
	public string ID = string.Empty;

	public string SceneID = string.Empty;

	public int PosX;

	public int PosZ;

	public int Range;

	public string NpcID = string.Empty;

	public int FlashNum;

	public int RequireNum;

	public Vector3 Pos
	{
		get
		{
			float x = (float)PosX / 100f;
			float z = (float)PosZ / 100f;
			return new Vector3(x, SceneManager.GetHitHeight(x, z), z);
		}
	}

	public float PosRange => (float)Range / 100f;
}
