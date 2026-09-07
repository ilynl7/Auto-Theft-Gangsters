using System.Collections.Generic;
using UnityEngine;

public class CurMissionDictionary
{
	public const int MAX_MISSION_NUM = 50;

	private const int DATA_SIZE = 64;

	private List<string> mCurTargetNpcIdList = new List<string>();

	private List<string> mCurCompleteNpcIdList = new List<string>();

	private Dictionary<string, CurMission> mCurMissionDic = new Dictionary<string, CurMission>();

	private CurMission mEscortMission;

	private CurMission mROBBERYMission;

	private CurMission mMainMission;

	private CurMission mTimeLimitMission;

	private List<long> mMissionCompleteFlag = new List<long>();

	private Dictionary<string, int> mTimeLimitMissionCompleteDic = new Dictionary<string, int>();

	private int SideMissionPreIndex = 40000;

	private int MainMissionMaxIndex = 2000;

	private long mLastMainMissionId = -1L;

	public List<string> CurTargetNpcIdList => mCurTargetNpcIdList;

	public List<string> CurCompleteNpcIdList => mCurCompleteNpcIdList;

	public Dictionary<string, CurMission> CurMissionDic => mCurMissionDic;

	public List<long> MissionCompleteFlag
	{
		get
		{
			return mMissionCompleteFlag;
		}
		set
		{
			mMissionCompleteFlag = value;
		}
	}

	public long LastMainMissionId
	{
		get
		{
			return mLastMainMissionId;
		}
		set
		{
			mLastMainMissionId = value;
		}
	}

	public CurMissionDictionary()
	{
		Reset();
	}

	public void Reset()
	{
		mCurMissionDic.Clear();
		mROBBERYMission = null;
		mEscortMission = null;
		mMainMission = null;
		mTimeLimitMission = null;
		mMissionCompleteFlag.Clear();
		mTimeLimitMissionCompleteDic.Clear();
		mCurTargetNpcIdList.Clear();
		mCurCompleteNpcIdList.Clear();
	}

	public bool IsMissionAccepted(string missionId)
	{
		return mCurMissionDic.ContainsKey(missionId);
	}

	public bool IsMissionFull()
	{
		if (mCurMissionDic.Count >= 50)
		{
			return true;
		}
		return false;
	}

	public bool SetMissionComplete(string missionId)
	{
		if (string.IsNullOrEmpty(missionId))
		{
			return false;
		}
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID.Class == 1)
		{
			mLastMainMissionId = int.Parse(missionId);
			return true;
		}
		if (missionDataByID.Class == 2)
		{
			int num = int.Parse(missionId) - SideMissionPreIndex;
			int num2 = num / 64;
			int index;
			long num3;
			if (num2 < mMissionCompleteFlag.Count)
			{
				List<long> list;
				List<long> list2 = (list = mMissionCompleteFlag);
				int index2 = (index = num2);
				num3 = list[index];
				list2[index2] = num3 | (1L << ((num % 64) & 0x3F));
				return true;
			}
			int num4 = num2 + 1 - mMissionCompleteFlag.Count;
			for (int i = 0; i < num4; i++)
			{
				mMissionCompleteFlag.Add(0L);
			}
			List<long> list3;
			List<long> list4 = (list3 = mMissionCompleteFlag);
			int index3 = (index = num2);
			num3 = list3[index];
			list4[index3] = num3 | (1L << num % 64);
			return true;
		}
		if (missionDataByID.Class == 8)
		{
			mTimeLimitMissionCompleteDic.Add(missionId, 1);
			TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(missionDataByID.TimeLimitId);
			if (timeLimitMissionDataByID != null && missionId.Equals(timeLimitMissionDataByID.MissionList[timeLimitMissionDataByID.MissionList.Count - 1]))
			{
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.SetTimeMissionFinish(timeLimitMissionDataByID.MissionList[0]);
				for (int j = 0; j < timeLimitMissionDataByID.MissionList.Count; j++)
				{
					if (mTimeLimitMissionCompleteDic.ContainsKey(timeLimitMissionDataByID.MissionList[j]))
					{
						mTimeLimitMissionCompleteDic.Remove(timeLimitMissionDataByID.MissionList[j]);
					}
				}
			}
			return true;
		}
		return false;
	}

	public bool IsMissionCompleted(string missionId)
	{
		if (string.IsNullOrEmpty(missionId))
		{
			return false;
		}
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID == null)
		{
			return false;
		}
		if (missionDataByID.Class == 8)
		{
			if (mTimeLimitMissionCompleteDic.ContainsKey(missionId))
			{
				return true;
			}
			return false;
		}
		int num = int.Parse(missionId);
		if (num < SideMissionPreIndex)
		{
			if (num < MainMissionMaxIndex)
			{
				if (LastMainMissionId == -1)
				{
					return false;
				}
				if (LastMainMissionId > 1000)
				{
					if (num < 1000)
					{
						return false;
					}
					if (num <= mLastMainMissionId)
					{
						return true;
					}
					return false;
				}
				if (num > 1000)
				{
					return true;
				}
				if (num <= mLastMainMissionId)
				{
					return true;
				}
				return false;
			}
			return false;
		}
		num -= SideMissionPreIndex;
		int num2 = num / 64;
		if (num2 < mMissionCompleteFlag.Count)
		{
			long num3 = (1L << num % 64) & mMissionCompleteFlag[num2];
			return num3 != 0;
		}
		return false;
	}

	public bool AddMission(string missionId, long serverTime)
	{
		if (string.IsNullOrEmpty(missionId))
		{
			return false;
		}
		if (mCurMissionDic.ContainsKey(missionId))
		{
			return false;
		}
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID == null)
		{
			return false;
		}
		CurMission curMission = new CurMission();
		curMission.Reset();
		curMission.MissionId = missionId;
		mCurMissionDic.Add(missionId, curMission);
		mCurMissionDic[missionId].LastChangeTime = serverTime;
		if (missionDataByID.Class == 4)
		{
			mEscortMission = curMission;
		}
		else if (missionDataByID.Class == 5)
		{
			mROBBERYMission = curMission;
		}
		else if (missionDataByID.Class == 1)
		{
			mMainMission = curMission;
		}
		else if (missionDataByID.Class == 8)
		{
			mTimeLimitMission = curMission;
		}
		ObjNPC objNPC = null;
		if (curMission.MissionState == MISSION_STATE.ACCEPTED)
		{
			mCurTargetNpcIdList.Add(missionDataByID.Target);
		}
		else if (curMission.MissionState == MISSION_STATE.COMPLETE)
		{
			mCurCompleteNpcIdList.Add(missionDataByID.Submit);
		}
		return true;
	}

	public bool RemoveMission(string missionId)
	{
		if (string.IsNullOrEmpty(missionId))
		{
			return false;
		}
		if (!mCurMissionDic.ContainsKey(missionId))
		{
			Debug.Log("not have this mission id :" + missionId);
			return false;
		}
		MISSION_STATE missionState = mCurMissionDic[missionId].MissionState;
		bool flag = mCurMissionDic.Remove(missionId);
		if (flag)
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
			if (missionDataByID.Class == 4)
			{
				mEscortMission = null;
			}
			else if (missionDataByID.Class == 5)
			{
				mROBBERYMission = null;
			}
			else if (missionDataByID.Class == 1)
			{
				mMainMission = null;
			}
			else if (missionDataByID.Class == 8)
			{
				TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(missionDataByID.TimeLimitId);
				if (timeLimitMissionDataByID != null)
				{
					for (int i = 0; i < timeLimitMissionDataByID.MissionList.Count; i++)
					{
						if (mTimeLimitMissionCompleteDic.ContainsKey(timeLimitMissionDataByID.MissionList[i]))
						{
							mTimeLimitMissionCompleteDic.Remove(timeLimitMissionDataByID.MissionList[i]);
						}
					}
				}
				mTimeLimitMission = null;
			}
			ObjNPC objNPC = null;
			switch (missionState)
			{
			case MISSION_STATE.ACCEPTED:
				mCurTargetNpcIdList.Remove(missionDataByID.Target);
				break;
			case MISSION_STATE.COMPLETE:
				mCurCompleteNpcIdList.Remove(missionDataByID.Submit);
				break;
			}
		}
		return flag;
	}

	public void SetMissionParam(string missionId, int paramIndex, long val)
	{
		if (mCurMissionDic.ContainsKey(missionId))
		{
			mCurMissionDic[missionId].SetParam(paramIndex, val);
		}
	}

	public long GetMissionParam(string missionId, int paramIndex)
	{
		if (!mCurMissionDic.ContainsKey(missionId))
		{
			return -1L;
		}
		return mCurMissionDic[missionId].GetParam(paramIndex);
	}

	public bool SetMissionState(string missionId, MISSION_STATE state, long changeTime)
	{
		if (!mCurMissionDic.ContainsKey(missionId))
		{
			return false;
		}
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (mCurMissionDic[missionId].MissionState == MISSION_STATE.ACCEPTED)
		{
			mCurTargetNpcIdList.Remove(missionDataByID.Target);
		}
		else if (mCurMissionDic[missionId].MissionState == MISSION_STATE.COMPLETE)
		{
			mCurCompleteNpcIdList.Remove(missionDataByID.Submit);
		}
		switch (state)
		{
		case MISSION_STATE.ACCEPTED:
			mCurTargetNpcIdList.Add(missionDataByID.Target);
			break;
		case MISSION_STATE.COMPLETE:
			mCurCompleteNpcIdList.Add(missionDataByID.Submit);
			break;
		}
		if (changeTime > -1)
		{
			mCurMissionDic[missionId].LastChangeTime = changeTime;
		}
		return mCurMissionDic[missionId].SetMissionState(state);
	}

	public long GetMissionChangeTime(string missionId)
	{
		if (!mCurMissionDic.ContainsKey(missionId))
		{
			return -1L;
		}
		return mCurMissionDic[missionId].LastChangeTime;
	}

	public MISSION_STATE GetMissionState(string missionId)
	{
		if (!mCurMissionDic.ContainsKey(missionId))
		{
			return MISSION_STATE.INVALID;
		}
		return mCurMissionDic[missionId].MissionState;
	}

	public CurMission GetCurMissionByClassType(MISSION_CLASS_TYPE classType)
	{
		return classType switch
		{
			MISSION_CLASS_TYPE.ESCORT => mEscortMission, 
			MISSION_CLASS_TYPE.ROBBERY => mROBBERYMission, 
			MISSION_CLASS_TYPE.MAIN => mMainMission, 
			MISSION_CLASS_TYPE.TIME_LIMIT => mTimeLimitMission, 
			_ => null, 
		};
	}
}
