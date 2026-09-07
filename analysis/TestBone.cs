using UnityEngine;

public class TestBone : MonoBehaviour
{
	private void Start()
	{
		Transform[] bones = GetComponent<SkinnedMeshRenderer>().bones;
		for (int i = 0; i < bones.Length; i++)
		{
			Debug.Log(bones[i]);
		}
	}

	private void Update()
	{
	}
}
