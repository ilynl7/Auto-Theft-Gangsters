using System.Collections.Generic;
using UnityEngine;

public class CrashDoorController : MonoBehaviour
{
	private float mRecycleTime = 5f;

	private GameObject mSingleMeshObj;

	private List<GameObject> mCrashObjList = new List<GameObject>();

	private bool mInitFlag;

	private void Start()
	{
		if (mInitFlag)
		{
			return;
		}
		mInitFlag = true;
		GameObject gameObject = null;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			gameObject = base.transform.GetChild(i).gameObject;
			if (gameObject.name.StartsWith("@"))
			{
				mSingleMeshObj = gameObject;
			}
			else
			{
				mCrashObjList.Add(gameObject);
			}
		}
	}

	public void OpenBlock(float delayTime = 0f)
	{
		if (delayTime > 0f)
		{
			Invoke("Play", delayTime);
		}
		else
		{
			Play();
		}
	}

	private void Play()
	{
		Invoke("DestroyMyself", mRecycleTime);
	}

	private void DestroyMyself()
	{
		Object.Destroy(base.gameObject);
	}
}
