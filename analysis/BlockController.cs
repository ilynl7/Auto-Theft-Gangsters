using UnityEngine;

public class BlockController : MonoBehaviour
{
	private Animation[] array1;

	private ParticleSystem[] array2;

	public float time = 2f;

	private float startTime;

	public void Init()
	{
		array1 = GetComponentsInChildren<Animation>();
		array2 = GetComponentsInChildren<ParticleSystem>();
		Play(isEnable: false);
	}

	private void Play(bool isEnable)
	{
		if (isEnable)
		{
			for (int i = 0; i < array1.Length; i++)
			{
				array1[i].Play();
			}
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].Play();
			}
			return;
		}
		for (int k = 0; k < array1.Length; k++)
		{
			array1[k].Stop();
		}
		for (int l = 0; l < array2.Length; l++)
		{
			array2[l].Clear();
			array2[l].Stop();
		}
	}

	public void OpenBlock(float delayTime = 0f)
	{
		if (delayTime > 0f)
		{
			Invoke("InternalOpenBlock", delayTime);
		}
		else
		{
			InternalOpenBlock();
		}
	}

	private void InternalOpenBlock()
	{
		Play(isEnable: true);
		Invoke("DestroyMyself", time);
	}

	private void DestroyMyself()
	{
		Object.Destroy(base.gameObject);
	}
}
