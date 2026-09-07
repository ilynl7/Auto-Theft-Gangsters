using System;
using System.Collections.Generic;

public class ObjNpcPoolGroup
{
	public delegate void OnNpcRecycle(ObjNPC npc);

	private Dictionary<string, ObjNpcPool> mObjNpcPoolDict = new Dictionary<string, ObjNpcPool>();

	private List<ObjNPC> mEnableNpcList = new List<ObjNPC>();

	public OnNpcRecycle onNpcRecycle;

	private List<ObjNpcPool> mObjNpcPoolList = new List<ObjNpcPool>();

	private int mPoolGroupMaxNum;

	private int mPoolMaxNum;

	public Dictionary<string, ObjNpcPool> ObjNpcPoolDict => mObjNpcPoolDict;

	public List<ObjNPC> EnableNpcList => mEnableNpcList;

	public void RegisterOnNpcRecycle(OnNpcRecycle func)
	{
		onNpcRecycle = (OnNpcRecycle)Delegate.Combine(onNpcRecycle, func);
	}

	public void DeRegisterOnNpcRecycle(OnNpcRecycle func)
	{
		if (onNpcRecycle != null)
		{
			onNpcRecycle = (OnNpcRecycle)Delegate.Remove(onNpcRecycle, func);
		}
	}

	public void Reset(int poolGroupMaxNum, int poolMaxNum)
	{
		mPoolGroupMaxNum = poolGroupMaxNum;
		mPoolMaxNum = poolMaxNum;
	}

	public void RefreshPool(int poolGroupMaxNum, int poolMaxNum)
	{
		mPoolGroupMaxNum = poolGroupMaxNum;
		mPoolMaxNum = poolMaxNum;
		List<ObjNpcPool> list = new List<ObjNpcPool>(ObjNpcPoolDict.Values);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].RefreshPoolMaxNum(mPoolMaxNum);
		}
	}

	public void GetNpc(ObjInitNpcData initData, ObjManager.OnGetNPC func, ObjManager.OnGetNPC onLoadModelDone = null)
	{
		ObjNpcPool value = null;
		mObjNpcPoolDict.TryGetValue(initData.npcInfoData.Model, out value);
		func = (ObjManager.OnGetNPC)Delegate.Combine(func, new ObjManager.OnGetNPC(OnGetNpc));
		if (value != null)
		{
			value.GetNpc(initData, func, onLoadModelDone);
			return;
		}
		ObjNpcPool objNpcPool = new ObjNpcPool();
		objNpcPool.SetPoolMaxNum(mPoolMaxNum, initData.npcInfoData.Model);
		mObjNpcPoolDict.Add(initData.npcInfoData.Model, objNpcPool);
		mObjNpcPoolList.Add(objNpcPool);
		objNpcPool.GetNpc(initData, func, onLoadModelDone);
	}

	public void OnGetNpc(ObjNPC npc)
	{
		if (npc != null)
		{
			mEnableNpcList.Add(npc);
		}
	}

	public void RecycleNpc(ObjNPC objNpc)
	{
		if (onNpcRecycle != null)
		{
			onNpcRecycle(objNpc);
		}
		objNpc.IsDie = true;
		mObjNpcPoolDict.TryGetValue(objNpc.ModelId, out var value);
		mEnableNpcList.Remove(objNpc);
		value?.RecycleNpc(objNpc);
		if (mObjNpcPoolDict.Count <= mPoolGroupMaxNum)
		{
			return;
		}
		ObjNpcPool objNpcPool = null;
		float num = float.MaxValue;
		for (int i = 0; i < mObjNpcPoolList.Count; i++)
		{
			if (mObjNpcPoolList[i].EnableNpcList.Count <= 0 && num > mObjNpcPoolList[i].LastUseTime)
			{
				num = mObjNpcPoolList[i].LastUseTime;
				objNpcPool = mObjNpcPoolList[i];
			}
		}
		if (objNpcPool != null)
		{
			objNpcPool.DestroyClearPool();
			mObjNpcPoolDict.Remove(objNpcPool.ID);
			mObjNpcPoolList.Remove(objNpcPool);
		}
	}

	public bool CheckNpcClear(ObjNPC objNpc)
	{
		for (int i = 0; i < mEnableNpcList.Count; i++)
		{
			if (mEnableNpcList[i].AttributeData.Camp != GameDefine.CAMP_TYPE.FUNCTION_NPC && !mEnableNpcList[i].IsMissionNpc() && !mEnableNpcList[i].IsDie)
			{
				return false;
			}
		}
		return true;
	}

	public void ClearNPC()
	{
		List<ObjNpcPool> list = new List<ObjNpcPool>(mObjNpcPoolDict.Values);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].Clear();
		}
		mEnableNpcList.Clear();
	}
}
