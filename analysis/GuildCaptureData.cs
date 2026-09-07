using UnityEngine;

public class GuildCaptureData
{
	public string ID;

	public string MapID;

	public int DurationTime;

	public int Week;

	public int StartTime;

	public string ShowRewardID;

	public string DropID;

	public string NpcID;

	public string NpcPos;

	public int Basestatus;

	public int BSValue;

	public string Icon;

	public string Name;

	public string Rule;

	public string Description;

	public string Background;

	private int[] mPos;

	public Vector3 GetNpcPos()
	{
		if (mPos == null)
		{
			string[] array = NpcPos.Split('#');
			mPos = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				mPos[i] = int.Parse(array[i]);
			}
		}
		float x = (float)mPos[0] / 100f;
		float z = (float)mPos[1] / 100f;
		return new Vector3(x, SceneManager.GetHitHeight(new Vector3(x, 0f, z)), z);
	}
}
