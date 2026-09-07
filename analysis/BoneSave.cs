using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BoneSave : MonoBehaviour
{
	public List<string> list = new List<string>();

	private void Start()
	{
		if (Application.isEditor && list.Count == 0)
		{
			SkinnedMeshRenderer component = GetComponent<SkinnedMeshRenderer>();
			Transform[] bones = component.bones;
			for (int i = 0; i < bones.Length; i++)
			{
				list.Add(bones[i].name);
			}
		}
	}
}
