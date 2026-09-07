using System.Collections.Generic;
using UnityEngine;

public class MapInfoData
{
	public string ID = string.Empty;

	public string Name = string.Empty;

	public string SceneName = string.Empty;

	public int Type;

	public string MiniMapName = string.Empty;

	public int MapLength;

	public int MapHeight;

	public string BirthPos;

	public string RelifePos;

	public string TelePortPos;

	public int radius;

	public string luaname;

	public string Param1;

	public string Param2;

	public string Param3;

	public string Param4;

	public int Param5;

	public int RelifeCost = 1;

	public int RelifeType;

	public int DirectLoad;

	public int ChangeLightMap;

	public int AudioId = 500;

	public int OpenLv;

	public string WorldPos = string.Empty;

	public string MapIcon = string.Empty;

	public string SaftyAreaId = string.Empty;

	public string GatherAreaId = string.Empty;

	public string ExitCon = string.Empty;

	public int TargetPKMode = 1;

	public int IsLockPVP;

	public int AutoFightDis = 100;

	public string ShadowDirection;

	private string[] mShadowDir;

	private Vector3 mShadowDirV3 = new Vector3(55f, 70f, 0f);

	private float[] mBirthPos;

	private float[] mTelePortPos;

	private List<Vector3> mPlayerPathPointList;

	private List<Vector4> mBirthPosList;

	private List<Vector4> mRelifePosList;

	private List<Vector3> mTeleportPosList;

	public string ExitPos = string.Empty;

	private List<Vector3> mExitPosList;

	public int ActAudioID = -1;

	public string Starttime = string.Empty;

	public string EndTime = string.Empty;

	private int[] mEndTimes;

	private int[] mStarttimes;

	public Vector3 ShadowDir
	{
		get
		{
			if (mShadowDir == null && !string.IsNullOrEmpty(ShadowDirection))
			{
				mShadowDir = ShadowDirection.Split('#');
				mShadowDirV3 = new Vector3(int.Parse(mShadowDir[0]), int.Parse(mShadowDir[1]), int.Parse(mShadowDir[2]));
			}
			return mShadowDirV3;
		}
	}

	public string MExitCon => StrDictionary.GetDictionaryString(ExitCon);

	public Vector3 BirthPosVector3
	{
		get
		{
			if (mBirthPos == null)
			{
				string[] array = BirthPos.Split('#');
				mBirthPos = new float[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mBirthPos[i] = (float)int.Parse(array[i]) / 100f;
				}
			}
			return new Vector3(mBirthPos[0], mBirthPos[1], mBirthPos[2]);
		}
	}

	public Vector3 TelePortPosVector3
	{
		get
		{
			if (mTelePortPos == null && !string.IsNullOrEmpty(TelePortPos))
			{
				string[] array = TelePortPos.Split('#');
				mTelePortPos = new float[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mTelePortPos[i] = (float)int.Parse(array[i]) / 100f;
				}
			}
			if (mTelePortPos != null && mTelePortPos.Length > 2)
			{
				return new Vector3(mTelePortPos[0], mTelePortPos[1], mTelePortPos[2]);
			}
			return Vector3.zero;
		}
	}

	public MAPTYPE MapType => (MAPTYPE)Type;

	public float fMapLength => (float)MapLength / 100f;

	public float fMapHeight => (float)MapHeight / 100f;

	public Vector3 WoldPosVector3
	{
		get
		{
			Vector3 zero = Vector3.zero;
			string[] array = WorldPos.Split('#');
			zero.x = float.Parse(array[0]);
			zero.y = float.Parse(array[1]);
			return zero;
		}
	}

	public List<Vector3> PlayerPathPointList
	{
		get
		{
			if (mPlayerPathPointList == null)
			{
				mPlayerPathPointList = new List<Vector3>();
				string[] array = Param1.Split(';');
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split('#');
					mPlayerPathPointList.Add(new Vector3((float)int.Parse(array2[0]) / 100f, (float)int.Parse(array2[1]) / 100f, (float)int.Parse(array2[2]) / 100f));
				}
			}
			return mPlayerPathPointList;
		}
	}

	public List<Vector4> BirthPosList
	{
		get
		{
			if (mBirthPosList == null)
			{
				mBirthPosList = new List<Vector4>();
				string[] array = BirthPos.Split('#');
				for (int i = 0; i < array.Length / 4; i++)
				{
					mBirthPosList.Add(new Vector4((float)int.Parse(array[i * 4]) / 100f, (float)int.Parse(array[i * 4 + 1]) / 100f, (float)int.Parse(array[i * 4 + 2]) / 100f, (float)int.Parse(array[i * 4 + 3]) / 100f));
				}
			}
			return mBirthPosList;
		}
	}

	public List<Vector4> RelifePosList
	{
		get
		{
			if (mRelifePosList == null)
			{
				mRelifePosList = new List<Vector4>();
				string[] array = RelifePos.Split('#');
				for (int i = 0; i < array.Length / 4; i++)
				{
					mRelifePosList.Add(new Vector4((float)int.Parse(array[i * 4]) / 100f, (float)int.Parse(array[i * 4 + 1]) / 100f, (float)int.Parse(array[i * 4 + 2]) / 100f, (float)int.Parse(array[i * 4 + 3]) / 100f));
				}
			}
			return mRelifePosList;
		}
	}

	public List<Vector3> TeleportPosList
	{
		get
		{
			if (mTeleportPosList == null)
			{
				mTeleportPosList = new List<Vector3>();
				string[] array = TelePortPos.Split('#');
				for (int i = 0; i < array.Length / 3; i++)
				{
					mTeleportPosList.Add(new Vector3((float)int.Parse(array[i * 3]) / 100f, (float)int.Parse(array[i * 3 + 1]) / 100f, (float)int.Parse(array[i * 3 + 2]) / 100f));
				}
			}
			return mTeleportPosList;
		}
	}

	public List<Vector3> ExitPosList
	{
		get
		{
			if (mExitPosList == null)
			{
				mExitPosList = new List<Vector3>();
				string[] array = ExitPos.Split('#');
				for (int i = 0; i < array.Length / 3; i++)
				{
					mExitPosList.Add(new Vector3((float)int.Parse(array[i * 3]) / 100f, (float)int.Parse(array[i * 3 + 1]) / 100f, (float)int.Parse(array[i * 3 + 2]) / 100f));
				}
			}
			return mExitPosList;
		}
	}

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

	public int GetAudioID()
	{
		if (ActAudioID != -1 && !string.IsNullOrEmpty(Starttime) && !string.IsNullOrEmpty(EndTime) && TimeTools.IsTimeRange(Starttimes, EndTimes))
		{
			return ActAudioID;
		}
		return AudioId;
	}
}
