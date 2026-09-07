using UnityEngine;

public class MovingCircleItem : SingletonUnity<MovingCircleItem>
{
	public void ActiveMovingCircle(Vector3 pos)
	{
		base.transform.position = pos + Vector3.up * 0.2f;
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
	}

	public void DisactiveMovingCircle()
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
		}
	}
}
