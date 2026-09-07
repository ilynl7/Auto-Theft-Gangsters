using UnityEngine;

public class TransformUtil
{
	public static GameObject FindChildGameObject(GameObject gameObject, string name)
	{
		Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
		int num = 0;
		for (num = 0; num < componentsInChildren.Length; num++)
		{
			if (componentsInChildren[num].gameObject.name.CompareTo(name) == 0)
			{
				return componentsInChildren[num].gameObject;
			}
		}
		return null;
	}

	public static Transform FindChildTransform(Transform[] trans, string name)
	{
		int num = 0;
		for (num = 0; num < trans.Length; num++)
		{
			if (trans[num].name.CompareTo(name) == 0)
			{
				return trans[num];
			}
		}
		return null;
	}

	public static Transform FindChildTransform(Transform transform, string name, bool active = false)
	{
		Transform[] componentsInChildren = transform.gameObject.GetComponentsInChildren<Transform>(active);
		int num = 0;
		for (num = 0; num < componentsInChildren.Length; num++)
		{
			if (componentsInChildren[num].name.CompareTo(name) == 0)
			{
				return componentsInChildren[num];
			}
		}
		return null;
	}
}
