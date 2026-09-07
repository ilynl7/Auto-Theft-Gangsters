using System.Collections.Generic;
using SprotoType;

public class CopyData
{
	private Dictionary<string, copyscene_info> mNormalCopyInfoDic = new Dictionary<string, copyscene_info>();

	private Dictionary<string, copyscene_info> mDailyCopyInfoDic = new Dictionary<string, copyscene_info>();

	public Dictionary<string, copyscene_info> NormalCopyInfoDic => mNormalCopyInfoDic;

	public List<copyscene_info> NormalCopyInfoList
	{
		get
		{
			List<copyscene_info> list = new List<copyscene_info>(mNormalCopyInfoDic.Values);
			list.Sort(SortFun);
			return list;
		}
	}

	public Dictionary<string, copyscene_info> DailyCopyInfoDic => mDailyCopyInfoDic;

	public List<copyscene_info> DailyCopyInfoList
	{
		get
		{
			List<copyscene_info> list = new List<copyscene_info>(mDailyCopyInfoDic.Values);
			list.Sort(SortFun);
			return list;
		}
	}

	private static int SortFun(copyscene_info a1, copyscene_info a2)
	{
		if (a1.ID.Length == a2.ID.Length)
		{
			return a1.ID.CompareTo(a2.ID);
		}
		return a1.ID.Length - a2.ID.Length;
	}

	public void Reset()
	{
		mNormalCopyInfoDic.Clear();
		mDailyCopyInfoDic.Clear();
	}

	public copyscene_info GetCopyinfoByID(string id)
	{
		if (mDailyCopyInfoDic.ContainsKey(id))
		{
			return mDailyCopyInfoDic[id];
		}
		return null;
	}

	public void DecTimesByID(string id)
	{
		if (mDailyCopyInfoDic.ContainsKey(id) && mDailyCopyInfoDic[id].CurNum > 0)
		{
			mDailyCopyInfoDic[id].CurNum--;
		}
	}

	public copyscene_info GetCopyinfoByType(int type)
	{
		for (int i = 0; i < DailyCopyInfoList.Count; i++)
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(DailyCopyInfoList[i].ID);
			if (copySceneDataById.SubType == type)
			{
				return DailyCopyInfoList[i];
			}
		}
		return null;
	}

	public copyscene_info GetCopyinfoByID(int ID)
	{
		for (int i = 0; i < DailyCopyInfoList.Count; i++)
		{
			if (DailyCopyInfoList[i].ID.Equals(ID))
			{
				return DailyCopyInfoList[i];
			}
		}
		return null;
	}

	public void SyncCopyInfo(copyscene_info info)
	{
		SyncData(new KeyValuePair<string, copyscene_info>(info.ID, info));
		UpdateTips();
	}

	public void SyncCopyInfo(Dictionary<string, copyscene_info> newCopyInfoDic)
	{
		List<KeyValuePair<string, copyscene_info>> list = new List<KeyValuePair<string, copyscene_info>>(newCopyInfoDic);
		mDailyCopyInfoDic.Clear();
		mNormalCopyInfoDic.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Value.enable)
			{
				SyncData(list[i]);
			}
		}
		UpdateTips();
	}

	private void SyncData(KeyValuePair<string, copyscene_info> info)
	{
		switch (info.Value.Type)
		{
		case 0L:
			SyncDataInRightDic(mNormalCopyInfoDic, info);
			break;
		case 1L:
			SyncDataInRightDic(mDailyCopyInfoDic, info);
			break;
		}
	}

	public CopySceneData GetCurPlayerEquipCopyData()
	{
		List<CopySceneData> copySceneDataByType = DataManager.GetCopySceneDataByType(16);
		CopySceneData result = copySceneDataByType[0];
		for (int i = 0; i < copySceneDataByType.Count; i++)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(copySceneDataByType[i].MinLevel, copySceneDataByType[i].MaxLevel))
			{
				result = copySceneDataByType[i];
			}
		}
		return result;
	}

	public CopySceneData GetCurPlayerExpCopyData()
	{
		List<CopySceneData> copySceneDataByType = DataManager.GetCopySceneDataByType(12);
		CopySceneData result = copySceneDataByType[0];
		for (int i = 0; i < copySceneDataByType.Count; i++)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(copySceneDataByType[i].MinLevel, copySceneDataByType[i].MaxLevel))
			{
				result = copySceneDataByType[i];
			}
		}
		return result;
	}

	public bool IsDailyCopyCanPlay(MAPTYPE dailytype)
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_DAILY))
		{
			return false;
		}
		for (int i = 0; i < DailyCopyInfoList.Count; i++)
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(DailyCopyInfoList[i].ID);
			if (copySceneDataById != null && copySceneDataById.SubType == (int)dailytype && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(copySceneDataById.MinLevel))
			{
				if (dailytype == MAPTYPE.CAR_CHASE_COPY)
				{
					return true;
				}
				if (DailyCopyInfoList[i].CurNum > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsHaveDailyItemTips()
	{
		return IsHaveDailyCopyTips() || SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TowerData.IsHaveTowerTips();
	}

	public bool IsHaveDailyCopyTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_DAILY))
		{
			return false;
		}
		for (int i = 0; i < DailyCopyInfoList.Count; i++)
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(DailyCopyInfoList[i].ID);
			if (copySceneDataById != null && copySceneDataById.SubType != 1 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(copySceneDataById.MinLevel) && DailyCopyInfoList[i].CurNum > 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsHaveDailyCopyTipsBytype(MAPTYPE type)
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_DAILY))
		{
			return false;
		}
		for (int i = 0; i < DailyCopyInfoList.Count; i++)
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(DailyCopyInfoList[i].ID);
			if (copySceneDataById != null && copySceneDataById.SubType == (int)type && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(copySceneDataById.MinLevel) && DailyCopyInfoList[i].CurNum > 0)
			{
				return true;
			}
		}
		return false;
	}

	public void SyncTimeslocal(string id)
	{
		CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(id);
		if (copySceneDataById.SubType != 11)
		{
			return;
		}
		for (int i = 0; i < DailyCopyInfoList.Count; i++)
		{
			CopySceneData copySceneDataById2 = DataManager.GetCopySceneDataById(DailyCopyInfoList[i].ID);
			if (copySceneDataById2 != null && copySceneDataById2.SubType == copySceneDataById.SubType)
			{
				mDailyCopyInfoDic[DailyCopyInfoList[i].ID].CurNum--;
			}
		}
		UpdateTips();
	}

	public void UpdateTips()
	{
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateMessageTips();
		}
		if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
		}
		if (SingletonUnity<ActivityTipsRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ActivityTipsRootLogic>.Instance.gameObject))
		{
			SingletonUnity<ActivityTipsRootLogic>.Instance.UpdateInfo();
		}
	}

	private void SyncDataInRightDic(Dictionary<string, copyscene_info> dic, KeyValuePair<string, copyscene_info> info)
	{
		if (dic.ContainsKey(info.Key))
		{
			dic[info.Key] = info.Value;
		}
		else
		{
			dic.Add(info.Key, info.Value);
		}
	}

	public void ResetCopyInfo()
	{
	}

	private void ClassifyCopyInfo(KeyValuePair<string, CopySceneData> info)
	{
		copyscene_info copyscene_info = new copyscene_info();
		copyscene_info.Type = info.Value.Type;
		copyscene_info.CurNum = 0L;
		copyscene_info.ID = info.Value.ID;
		copyscene_info.BestGrade = -1L;
		switch (info.Value.Type)
		{
		case 0:
			mNormalCopyInfoDic.Add(info.Key, copyscene_info);
			break;
		case 1:
			mDailyCopyInfoDic.Add(info.Key, copyscene_info);
			break;
		}
	}
}
