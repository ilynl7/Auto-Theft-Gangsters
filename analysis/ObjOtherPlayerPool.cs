using System.Collections.Generic;
using UnityEngine;

public class ObjOtherPlayerPool
{
	private List<ObjOtherPlayer> mDisableOtherPlayerList = new List<ObjOtherPlayer>();

	private int mMaxPoolNum;

	public List<ObjOtherPlayer> DisableOtherPlayerList => mDisableOtherPlayerList;

	public int MaxPoolNum
	{
		get
		{
			return mMaxPoolNum;
		}
		set
		{
			mMaxPoolNum = value;
		}
	}

	public ObjOtherPlayerPool()
	{
		Reset();
	}

	public void Reset()
	{
		for (int i = 0; i < mDisableOtherPlayerList.Count; i++)
		{
			Object.Destroy(mDisableOtherPlayerList[i]);
		}
		mDisableOtherPlayerList.Clear();
	}

	public void SetPoolMaxNum(int maxNum)
	{
		mMaxPoolNum = maxNum;
	}

	public ObjOtherPlayer GetOtherPlayer(ObjInitPlayerData initData)
	{
		if (CheckInPool(initData.mServerID))
		{
			return GetInPool(initData.mServerID);
		}
		return null;
	}

	public void RecycleOtherPlayer(ObjOtherPlayer objOtherPlayer)
	{
		UnityVersionUtil.SetActiveRecursive(objOtherPlayer.gameObject, state: false);
		if (mDisableOtherPlayerList.Count < MaxPoolNum)
		{
			mDisableOtherPlayerList.Add(objOtherPlayer);
		}
		else if (mDisableOtherPlayerList.Count > 0)
		{
			ObjOtherPlayer objOtherPlayer2 = mDisableOtherPlayerList[0];
			mDisableOtherPlayerList.RemoveAt(0);
			mDisableOtherPlayerList.Add(objOtherPlayer);
			objOtherPlayer2.RecycleUnloadModelBundle();
			Object.Destroy(objOtherPlayer2.gameObject);
		}
		else
		{
			objOtherPlayer.RecycleUnloadModelBundle();
			Object.Destroy(objOtherPlayer.gameObject);
		}
	}

	private bool CheckInPool(long serverId)
	{
		for (int i = 0; i < mDisableOtherPlayerList.Count; i++)
		{
			if (mDisableOtherPlayerList[i].ServerId == serverId)
			{
				return true;
			}
		}
		return false;
	}

	private ObjOtherPlayer GetInPool(long serverId)
	{
		for (int i = 0; i < mDisableOtherPlayerList.Count; i++)
		{
			if (mDisableOtherPlayerList[i].ServerId == serverId)
			{
				ObjOtherPlayer result = mDisableOtherPlayerList[i];
				mDisableOtherPlayerList.RemoveAt(i);
				return result;
			}
		}
		return null;
	}

	public void Clear()
	{
		mDisableOtherPlayerList.Clear();
	}

	~ObjOtherPlayerPool()
	{
	}
}
