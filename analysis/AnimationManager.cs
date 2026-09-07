using UnityEngine;

public class AnimationManager
{
	public static bool initStreamFlag;

	public static bool initStreamDoneFlag;

	public static bool initDownloadFlag;

	public static bool initDownloadDoneFlag;

	public static void initStreamAnimationData(MonoBehaviour mono)
	{
		if (!initStreamFlag)
		{
			initStreamFlag = true;
			initStreamDoneFlag = false;
			if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
			{
				mono.StartCoroutine(BundleManager.LoadStreamAnimationBundle(StreamAnimationFinish));
			}
		}
	}

	private static void StreamAnimationFinish()
	{
		initStreamDoneFlag = true;
	}

	public static void initDownloadAnimationData(MonoBehaviour mono)
	{
		if (!initDownloadFlag)
		{
			initDownloadFlag = true;
			initDownloadDoneFlag = false;
			if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
			{
				mono.StartCoroutine(BundleManager.LoadDownloadAnimationBundle(DownloadAnimationFinish));
			}
		}
	}

	private static void DownloadAnimationFinish()
	{
		initDownloadDoneFlag = true;
	}

	public static void ReImportDownloadAnimationData(MonoBehaviour mono)
	{
		initDownloadFlag = true;
		initDownloadDoneFlag = false;
		if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
		{
			mono.StartCoroutine(BundleManager.LoadDownloadAnimationBundle(DownloadAnimationFinish));
		}
	}

	public static Object LoadAnimation(string modelname, string actionname)
	{
		Object @object = null;
		string path = $"Animation/{modelname}/{actionname}";
		@object = ResourcesManager.LoadAnimation(path);
		if (@object == null)
		{
			@object = BundleManager.LoadAnimation(modelname, actionname);
		}
		return @object;
	}
}
