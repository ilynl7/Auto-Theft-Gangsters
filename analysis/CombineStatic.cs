using UnityEngine;

public class CombineStatic : MonoBehaviour
{
	private void Awake()
	{
		StaticBatchingUtility.Combine(base.gameObject);
	}
}
