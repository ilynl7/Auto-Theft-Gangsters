using UnityEngine;

public class AreaCheckTool
{
	public static bool CheckInCircle(Vector3 targetPos, Vector3 centralPos, float range)
	{
		if (VectorXZ.Distance(targetPos, centralPos) <= range)
		{
			return true;
		}
		return false;
	}

	public static bool CheckInCenterRectangle(Vector3 targetPos, Transform sourceTransform, float forwardDistance, float sidewardDistance)
	{
		Vector3 vector = sourceTransform.InverseTransformPoint(targetPos);
		if (Mathf.Abs(vector.z) < forwardDistance / 2f && Mathf.Abs(vector.x) <= sidewardDistance / 2f)
		{
			return true;
		}
		return false;
	}

	public static bool CheckInForwardRectangle(Vector3 targetPos, Transform sourceTransform, float forwardDistance, float sidewardDistance)
	{
		Vector3 vector = sourceTransform.InverseTransformPoint(targetPos);
		if (vector.z > 0f && vector.z < forwardDistance && Mathf.Abs(vector.x) <= sidewardDistance / 2f)
		{
			return true;
		}
		return false;
	}

	public static bool CheckInSector(Vector3 targetPos, Transform sourceTransform, float forwardDistance, float angle)
	{
		Vector3 from = targetPos - sourceTransform.position;
		if (from.magnitude < forwardDistance)
		{
			if (Vector3.Angle(from, sourceTransform.forward) <= angle / 2f + 1f)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static bool CheckInRectangle(Vector2 targetPos, Vector2 leftUpPos, Vector2 leftDownPos, Vector2 rightDownPos, Vector2 rightUpPos)
	{
		Vector2 vector = leftDownPos - leftUpPos;
		Vector2 vector2 = rightDownPos - leftDownPos;
		Vector2 vector3 = rightUpPos - rightDownPos;
		Vector2 vector4 = leftUpPos - rightUpPos;
		Vector2 vector5 = targetPos - leftUpPos;
		Vector2 vector6 = targetPos - leftDownPos;
		Vector2 vector7 = targetPos - rightDownPos;
		Vector2 vector8 = targetPos - rightUpPos;
		if ((vector.x * vector5.y - vector.y * vector5.x) * (vector3.x * vector7.y - vector3.y * vector7.x) > 0f && (vector2.x * vector6.y - vector2.y * vector6.x) * (vector4.x * vector8.y - vector4.y * vector8.x) > 0f)
		{
			return true;
		}
		return false;
	}

	public static bool CheckInCircle(Vector2 targetPos, Vector2 sourcePos, float Range)
	{
		if ((targetPos - sourcePos).sqrMagnitude <= Range * Range)
		{
			return true;
		}
		return false;
	}
}
