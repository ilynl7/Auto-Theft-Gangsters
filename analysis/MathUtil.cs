using System;
using UnityEngine;

public class MathUtil
{
	public static float WrapDegrees(float angle)
	{
		while (angle > 180f)
		{
			angle -= 360f;
		}
		while (angle < -180f)
		{
			angle += 360f;
		}
		return angle;
	}

	public static float RotationDir(float a, float b)
	{
		return WrapDegrees(b - a);
	}

	public static float Heading(Vector3 dir)
	{
		if (dir == Vector3.zero)
		{
			return 0f;
		}
		return Mathf.Atan2(dir.x, dir.z) * 57.29578f;
	}

	public static float Pitch(Vector3 dir)
	{
		if (dir == Vector3.zero)
		{
			return 0f;
		}
		return Mathf.Asin(Mathf.Clamp(dir.y / dir.magnitude, -1f, 1f)) * 57.29578f;
	}

	public static Vector3 HeadingToVector3(float heading)
	{
		float x = Mathf.Sin(heading * ((float)Math.PI / 180f));
		float z = Mathf.Cos(heading * ((float)Math.PI / 180f));
		return new Vector3(x, 0f, z);
	}

	public static float ClampScale(float f, float c1, float c2, float s1, float s2)
	{
		if (c2 == c1)
		{
			return s2;
		}
		if (c2 > c1)
		{
			return (Mathf.Clamp(f, c1, c2) - c1) / (c2 - c1) * (s2 - s1) + s1;
		}
		return (0f - Mathf.Clamp(0f - f, 0f - c1, 0f - c2) - c1) / (c2 - c1) * (s2 - s1) + s1;
	}

	public static float SmoothLerp(float fo, float to, float t)
	{
		t = Mathf.Clamp01(t);
		if (t <= 0.5f)
		{
			t = 2f * t * t;
		}
		else
		{
			t = 1f - t;
			t = 1f - 2f * t * t;
		}
		return Mathf.Lerp(fo, to, t);
	}

	public static float SmoothLerpForm(float fo, float to, float t)
	{
		return SmoothLerp(fo, 2f * to - fo, t * 0.5f);
	}

	public static float SmoothLerpTo(float fo, float to, float t)
	{
		return fo + to - SmoothLerpForm(fo, to, 1f - t);
	}

	public static Vector3 SmoothStep(Vector3 current, Vector3 dest, float time)
	{
		float x = Mathf.SmoothStep(current.x, dest.x, time);
		float y = Mathf.SmoothStep(current.y, dest.y, time);
		float z = Mathf.SmoothStep(current.z, dest.z, time);
		return new Vector3(x, y, z);
	}
}
