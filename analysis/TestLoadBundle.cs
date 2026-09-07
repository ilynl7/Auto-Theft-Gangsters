using System.Collections;
using UnityEngine;

public class TestLoadBundle : MonoBehaviour
{
	private void Start()
	{
		if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(TestLoad());
		}
	}

	private void Update()
	{
	}

	public IEnumerator TestLoad()
	{
		WWW www2 = new WWW("file:///" + Application.streamingAssetsPath + "/Bundle/Model/BOSS_Nan_001.bundle");
		yield return www2;
		(Object.Instantiate(www2.assetBundle.mainAsset) as GameObject).transform.position = Vector3.zero;
		www2.assetBundle.Unload(unloadAllLoadedObjects: true);
		www2.assetBundle.ToString();
		yield return new WaitForSeconds(2f);
		www2 = new WWW("file:///" + Application.streamingAssetsPath + "/Bundle/Model/BOSS_Nan_001.bundle");
		yield return www2;
		(Object.Instantiate(www2.assetBundle.mainAsset) as GameObject).transform.position = Vector3.zero;
		www2.assetBundle.Unload(unloadAllLoadedObjects: false);
	}
}
