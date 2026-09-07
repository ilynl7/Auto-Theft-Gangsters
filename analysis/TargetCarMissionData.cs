using UnityEngine;

public class TargetCarMissionData
{
	public string ID;

	public string SceneID;

	public int PosX;

	public int PosZ;

	public string Rot;

	public string Mountid;

	public int RequireNum;

	private float[] mRot;

	public Vector3 Pos
	{
		get
		{
			float x = (float)PosX / 100f;
			float z = (float)PosZ / 100f;
			return new Vector3(x, SceneManager.GetHitHeight(x, z), z);
		}
	}

	public Vector3 GetRot
	{
		get
		{
			if (mRot == null)
			{
				string[] array = Rot.Split('#');
				mRot = new float[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mRot[i] = (float)int.Parse(array[i]) / 100f;
				}
			}
			return new Vector3(mRot[0], mRot[1], mRot[2]);
		}
	}
}
