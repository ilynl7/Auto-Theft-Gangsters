using System.Collections.Generic;

public class SimplePool<T>
{
	public delegate T CreateFunc(object data);

	public delegate void DestroyFunc(T t);

	private int mPoolCount = 20;

	private List<T> mEnableObjList = new List<T>();

	private List<T> mDisableObjList = new List<T>();

	private CreateFunc mCreateFunc;

	private DestroyFunc mDestroyFunc;

	public int PoolCount => mPoolCount;

	public List<T> EnableObjList => mEnableObjList;

	public List<T> DisableObjList => mDisableObjList;

	public void SimpleClear()
	{
		mEnableObjList.Clear();
		mDisableObjList.Clear();
	}

	public void Reset(CreateFunc createFunc, DestroyFunc destroyFunc, int poolCount)
	{
		SimpleClear();
		mCreateFunc = createFunc;
		mDestroyFunc = destroyFunc;
		mPoolCount = poolCount;
	}

	public void Clear()
	{
		for (int i = 0; i < mEnableObjList.Count; i++)
		{
			if (mDestroyFunc != null)
			{
				mDestroyFunc(mEnableObjList[i]);
			}
		}
		for (int j = 0; j < mDisableObjList.Count; j++)
		{
			if (mDestroyFunc != null)
			{
				mDestroyFunc(mDisableObjList[j]);
			}
		}
		mEnableObjList.Clear();
		mDisableObjList.Clear();
	}

	public void Init(object data = null)
	{
		T t = Get(data);
		Recycle(t);
	}

	public T Get(object data = null)
	{
		T val = default(T);
		if (mDisableObjList.Count > 0)
		{
			val = mDisableObjList[0];
			mDisableObjList.RemoveAt(0);
			mEnableObjList.Add(val);
		}
		else if (mEnableObjList.Count < mPoolCount && mCreateFunc != null)
		{
			val = mCreateFunc(data);
			if (val != null)
			{
				mEnableObjList.Add(val);
			}
		}
		return val;
	}

	public void Recycle(T t)
	{
		mEnableObjList.Remove(t);
		if (!mDisableObjList.Contains(t))
		{
			if (mDisableObjList.Count + mEnableObjList.Count <= mPoolCount)
			{
				mDisableObjList.Add(t);
			}
			else
			{
				mDestroyFunc(t);
			}
		}
	}
}
