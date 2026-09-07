using System.Collections.Generic;

public class SimplePoolGroup<T>
{
	private Dictionary<string, SimplePool<T>> mObjPoolDic = new Dictionary<string, SimplePool<T>>();

	private List<T> mEnableList = new List<T>();

	private int mPoolCount = 20;

	private SimplePool<T>.CreateFunc mCreateFunc;

	private SimplePool<T>.DestroyFunc mDestroyFunc;

	public Dictionary<string, SimplePool<T>> ObjPoolDic => mObjPoolDic;

	public List<T> EnableList => mEnableList;

	public void Reset(SimplePool<T>.CreateFunc createFunc, SimplePool<T>.DestroyFunc destroyFunc, int poolCount)
	{
		mCreateFunc = createFunc;
		mDestroyFunc = destroyFunc;
		mPoolCount = poolCount;
	}

	public void Init(string poolKey, object data = null)
	{
		T t = Get(poolKey, data);
		Recycle(t, poolKey);
	}

	public T Get(string poolKey, object data = null)
	{
		SimplePool<T> value = null;
		mObjPoolDic.TryGetValue(poolKey, out value);
		T val = default(T);
		if (value != null)
		{
			val = value.Get(data);
		}
		else
		{
			value = new SimplePool<T>();
			value.Reset(mCreateFunc, mDestroyFunc, mPoolCount);
			mObjPoolDic.Add(poolKey, value);
			val = value.Get(data);
		}
		if (val != null)
		{
			mEnableList.Add(val);
		}
		return val;
	}

	public void Recycle(T t, string poolKey)
	{
		mEnableList.Remove(t);
		mObjPoolDic.TryGetValue(poolKey, out var value);
		value?.Recycle(t);
	}

	public void Clear()
	{
		List<SimplePool<T>> list = new List<SimplePool<T>>(mObjPoolDic.Values);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].Clear();
		}
		mEnableList.Clear();
	}
}
