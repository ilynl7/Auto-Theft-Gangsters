using System.Collections;
using UnityEngine;

public class TestBundleUnload : MonoBehaviour
{
	private AssetBundle bundle;

	private IEnumerator Start()
	{
		string url = BundleManager.GetLocalUrl(BundleManager.BundleModelRootPath, "NPC_Nan_001.bundle");
		WWW www = new WWW(url);
		yield return www;
		bundle = www.assetBundle;
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		bundle.Unload(unloadAllLoadedObjects: false);
	}
}
