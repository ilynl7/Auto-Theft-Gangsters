public class CurMission
{
	public const int MAX_MISSION_PARAM_NUM = 8;

	private string mMissionId = string.Empty;

	private MISSION_STATE mMissionState = MISSION_STATE.INVALID;

	private long[] mMissionParam = new long[8];

	private long mLastChangeTime = -1L;

	public string MissionId
	{
		get
		{
			return mMissionId;
		}
		set
		{
			mMissionId = value;
		}
	}

	public MISSION_STATE MissionState
	{
		get
		{
			return mMissionState;
		}
		set
		{
			mMissionState = value;
		}
	}

	public long[] MissionParam
	{
		get
		{
			return mMissionParam;
		}
		set
		{
			mMissionParam = value;
		}
	}

	public long LastChangeTime
	{
		get
		{
			return mLastChangeTime;
		}
		set
		{
			mLastChangeTime = value;
		}
	}

	public CurMission()
	{
		Reset();
	}

	public void Reset()
	{
		MissionId = string.Empty;
		MissionState = MISSION_STATE.INVALID;
		for (int i = 0; i < mMissionParam.Length; i++)
		{
			mMissionParam[i] = 0L;
		}
	}

	public void SetParam(int paramIndex, long val)
	{
		if (paramIndex >= 0 && paramIndex < 8)
		{
			mMissionParam[paramIndex] = val;
		}
	}

	public long GetParam(int paramIndex)
	{
		if (paramIndex >= 0 && paramIndex < 8)
		{
			return mMissionParam[paramIndex];
		}
		return -1L;
	}

	public bool SetMissionState(MISSION_STATE state)
	{
		mMissionState = state;
		return true;
	}

	public MISSION_STATE GetMissionState()
	{
		return mMissionState;
	}
}
