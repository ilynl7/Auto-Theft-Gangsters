using UnityEngine;

public class UnityVersionUtil
{
	public static void SetActiveRecursive(GameObject go, bool state)
	{
		if (!(go == null))
		{
			go.SetActive(state);
		}
	}

	public static bool IsActive(GameObject go)
	{
		if (go == null)
		{
			return false;
		}
		return go.activeInHierarchy;
	}

	public static bool IsactiveInHierarchy(GameObject go)
	{
		if (go == null)
		{
			return false;
		}
		return go.activeInHierarchy;
	}
}
