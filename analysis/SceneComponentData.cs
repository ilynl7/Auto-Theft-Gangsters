using UnityEngine;

public class SceneComponentData
{
	public string MapID;

	public string POS;

	public string Angle;

	public string Scale;

	public string PrefabName = string.Empty;

	public int IsDanceShow;

	public int CloseQiqiuren;

	public int IsCombinStatic = 1;

	public string DependName = string.Empty;

	public string Starttime = string.Empty;

	public string EndTime = string.Empty;

	private int[] mEndTimes;

	private int[] mStarttimes;

	private float[] mPos;

	private float[] mAngle;

	private float[] mScale;

	public bool CloseQiqiuFlag => CloseQiqiuren == 1;

	public bool StaticCombine => IsCombinStatic == 1;

	public int[] EndTimes
	{
		get
		{
			if ((mEndTimes == null || mEndTimes.Length == 0) && !string.IsNullOrEmpty(EndTime))
			{
				string[] array = EndTime.Split('-');
				mEndTimes = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mEndTimes[i] = int.Parse(array[i]);
				}
			}
			return mEndTimes;
		}
	}

	public int[] Starttimes
	{
		get
		{
			if ((mStarttimes == null || mStarttimes.Length == 0) && !string.IsNullOrEmpty(Starttime))
			{
				string[] array = Starttime.Split('-');
				mStarttimes = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mStarttimes[i] = int.Parse(array[i]);
				}
			}
			return mStarttimes;
		}
	}

	public Vector3 FPos
	{
		get
		{
			if (mPos == null)
			{
				string[] array = POS.Split('#');
				mPos = new float[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mPos[i] = (float)int.Parse(array[i]) / 100f;
				}
			}
			return new Vector3(mPos[0], mPos[1], mPos[2]);
		}
	}

	public Vector3 FAngle
	{
		get
		{
			if (mAngle == null)
			{
				string[] array = Angle.Split('#');
				mAngle = new float[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mAngle[i] = (float)int.Parse(array[i]) / 100f;
				}
			}
			return new Vector3(mAngle[0], mAngle[1], mAngle[2]);
		}
	}

	public Vector3 FScale
	{
		get
		{
			if (mScale == null)
			{
				string[] array = Scale.Split('#');
				mScale = new float[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mScale[i] = (float)int.Parse(array[i]) / 100f;
				}
			}
			return new Vector3(mScale[0], mScale[1], mScale[2]);
		}
	}
}
