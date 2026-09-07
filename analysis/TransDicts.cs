using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TransDicts : MonoBehaviour
{
	public List<Transform> boneList = new List<Transform>();

	public Dictionary<string, Transform> TransDict = new Dictionary<string, Transform>();

	private void Awake()
	{
		if (Application.isEditor && boneList.Count == 0)
		{
			Transform[] componentsInChildren = base.transform.gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				boneList.Add(componentsInChildren[i]);
			}
		}
		else if (TransDict.Count == 0)
		{
			for (int j = 0; j < boneList.Count; j++)
			{
				TransDict.Add(boneList[j].name, boneList[j]);
			}
		}
	}
}
