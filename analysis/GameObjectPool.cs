using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
	private class LoadParm
	{
		public LoadPoolObjDelegate mDelFun;

		public object mParm1;

		public UIPathData mUIData;

		public string mObjName;

		public LoadParm(string name, UIPathData data, LoadPoolObjDelegate del, object parm1)
		{
			mObjName = name;
			mUIData = data;
			mDelFun = del;
			mParm1 = parm1;
		}
	}

	public delegate void LoadPoolObjDelegate(GameObject newObj, object parm1);

	private string mPoolName;

	private Dictionary<string, List<GameObject>> mEnablePool;

	private Dictionary<string, List<GameObject>> mDisablePool;

	private int mMaxSize = 64;

	public string PoolName => mPoolName;

	public GameObjectPool(string poolName, int maxSize = 64)
	{
		mPoolName = poolName;
		mMaxSize = maxSize;
		mEnablePool = new Dictionary<string, List<GameObject>>();
		mDisablePool = new Dictionary<string, List<GameObject>>();
	}

	public void LoadUIItem(UIPathData uiData, string name, LoadPoolObjDelegate del, object parm1)
	{
		GameObject gameObject = ReUseItem(name);
		if (gameObject == null)
		{
			if (SingletonUnity<UIManager>.Instance == null)
			{
				Debug.Log("SingletonUnity<UIManager>.Instance == null");
			}
			else
			{
				SingletonUnity<UIManager>.Instance.LoadUIItem(uiData, LoadItem, new LoadParm(name, uiData, del, parm1));
			}
		}
		else
		{
			NGUITools.SetActive(gameObject, state: true);
			del?.Invoke(gameObject, parm1);
		}
	}

	private void LoadItem(GameObject resObj, object param)
	{
		LoadParm loadParm = param as LoadParm;
		if (param == null)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate(resObj) as GameObject;
		if (gameObject != null)
		{
			gameObject.name = loadParm.mObjName;
			if (!InsertItem(mDisablePool, gameObject))
			{
				Object.Destroy(gameObject);
			}
			if (loadParm.mDelFun != null)
			{
				loadParm.mDelFun(gameObject, loadParm.mParm1);
			}
		}
	}

	public void Remove(GameObject item)
	{
		if (!(item == null))
		{
			UnityVersionUtil.SetActiveRecursive(item, state: false);
			if (RemoveItem(mDisablePool, item) && !InsertItem(mEnablePool, item))
			{
				Object.Destroy(item);
			}
		}
	}

	public GameObject ReUseItem(string name)
	{
		GameObject gameObject = null;
		if (mEnablePool.ContainsKey(name))
		{
			List<GameObject> list = mEnablePool[name];
			if (list == null || list.Count <= 0)
			{
				return null;
			}
			gameObject = list[0];
			if (gameObject == null)
			{
				list.RemoveAt(0);
				return null;
			}
			list.Remove(gameObject);
			if (InsertItem(mDisablePool, gameObject))
			{
				return gameObject;
			}
			Object.Destroy(gameObject);
		}
		return null;
	}

	private bool RemoveItem(Dictionary<string, List<GameObject>> pool, GameObject item)
	{
		if (item == null || pool == null)
		{
			return false;
		}
		if (pool.ContainsKey(item.name))
		{
			List<GameObject> list = pool[item.name];
			if (list != null && list.Contains(item))
			{
				list.Remove(item);
				return true;
			}
		}
		return false;
	}

	private bool InsertItem(Dictionary<string, List<GameObject>> pool, GameObject item)
	{
		if (item == null || pool == null)
		{
			return false;
		}
		List<GameObject> value = null;
		if (!pool.TryGetValue(item.name, out value))
		{
			value = new List<GameObject>();
			pool.Add(item.name, value);
		}
		if (value != null)
		{
			if (value.Count >= mMaxSize)
			{
				Debug.LogWarning("Max Pool");
				return false;
			}
			value.Add(item);
			return true;
		}
		return false;
	}

	public void ClearAllPool()
	{
		if (mDisablePool != null)
		{
			foreach (KeyValuePair<string, List<GameObject>> item in mDisablePool)
			{
				List<GameObject> value = item.Value;
				if (value != null)
				{
					for (int i = 0; i < value.Count; i++)
					{
						value[i] = null;
					}
				}
				value.Clear();
			}
		}
		if (mEnablePool == null)
		{
			return;
		}
		foreach (KeyValuePair<string, List<GameObject>> item2 in mEnablePool)
		{
			List<GameObject> value2 = item2.Value;
			if (value2 != null)
			{
				for (int j = 0; j < value2.Count; j++)
				{
					value2[j] = null;
				}
			}
			value2.Clear();
		}
	}
}
