using UnityEngine;

public class TestOpenBlock : MonoBehaviour
{
	public GameObject[] blockList;

	public GameObject[] ParticleList;

	public void OpenBlock(object ins)
	{
		int num = (int)ins;
		if (blockList[num] != null)
		{
			for (int i = 0; i < blockList[num].transform.childCount; i++)
			{
				blockList[num].transform.GetChild(i).GetChild(0).animation.Play();
			}
		}
		if (ParticleList[num] != null)
		{
			UnityVersionUtil.SetActiveRecursive(ParticleList[num].gameObject, state: true);
		}
	}

	private void Start()
	{
		for (int i = 0; i < ParticleList.Length; i++)
		{
			if (ParticleList[i] != null)
			{
				UnityVersionUtil.SetActiveRecursive(ParticleList[i], state: false);
			}
		}
		SingletonUnity<MyEvent>.Instance.Register("OpenBlock", this, "OpenBlock");
	}

	private void Update()
	{
	}
}
