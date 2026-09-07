using UnityEngine;

public class AssetBundleData
{
	public AssetBundle Bundle;

	public int UsingCount;

	public string SelfURL;

	public string DependURL;

	public bool IsDependObj;

	public AssetBundleData(AssetBundle bundle, int count, string selfURL, string dependURL, bool isDepend)
	{
		Bundle = bundle;
		UsingCount = count;
		SelfURL = selfURL;
		DependURL = dependURL;
		IsDependObj = isDepend;
	}

	public void SetBundle(AssetBundle bundle)
	{
		Bundle = bundle;
	}

	public bool IsBundleValid()
	{
		return Bundle != null;
	}

	public AssetBundle GetBundle()
	{
		return Bundle;
	}

	public void AddBundleUsingCount()
	{
		UsingCount++;
		if (!string.IsNullOrEmpty(DependURL))
		{
			BundleManager.ModelBundleCacheDic[DependURL].AddBundleUsingCount();
		}
	}

	public bool UnLoadBundle()
	{
		UsingCount--;
		if (!string.IsNullOrEmpty(DependURL) && BundleManager.ModelBundleCacheDic[DependURL].UnLoadBundle())
		{
			BundleManager.ModelBundleCacheDic.Remove(DependURL);
		}
		if (UsingCount <= 0)
		{
			if (IsMainPlayerPart())
			{
				return false;
			}
			if (Bundle == null)
			{
				return true;
			}
			if (BundleManager.UnloadBundle(Bundle, flag: true))
			{
				Bundle = null;
				return true;
			}
			return false;
		}
		return false;
	}

	public bool IsMainPlayerPart()
	{
		return Singleton<ObjManager>.Instance.IsMainPlayerPart(SelfURL);
	}

	public bool Clear(bool isClearAll = false)
	{
		UsingCount = 0;
		if (Bundle != null)
		{
			if (!isClearAll && IsMainPlayerPart())
			{
				return false;
			}
			if (BundleManager.UnloadBundle(Bundle, flag: true))
			{
				Bundle = null;
				UsingCount = 0;
				return true;
			}
			return false;
		}
		return true;
	}
}
