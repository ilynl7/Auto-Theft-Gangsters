namespace UnityEngine;

public struct VectorXZ
{
	public const float kEpsilon = 1E-05f;

	public float x;

	public float z;

	public float this[int index]
	{
		get
		{
			switch (index)
			{
			case 0:
				return x;
			default:
				Debug.LogError("Invalid VectorXZ index!");
				break;
			case 1:
				break;
			}
			return z;
		}
	}

	public VectorXZ normalized
	{
		get
		{
			VectorXZ result = new VectorXZ(x, z);
			result.Normalize();
			return result;
		}
	}

	public float magnitude => Mathf.Sqrt(x * x + z * z);

	public float sqrMagnitude => x * x + z * z;

	public static VectorXZ zero => new VectorXZ(0f, 0f);

	public static VectorXZ one => new VectorXZ(1f, 1f);

	public VectorXZ(float x, float z)
	{
		this.x = x;
		this.z = z;
	}

	public VectorXZ(VectorXZ a)
	{
		x = a.x;
		z = a.z;
	}

	public void Set(float new_x, float new_z)
	{
		x = new_x;
		z = new_z;
	}

	public static VectorXZ Lerp(VectorXZ from, VectorXZ to, float t)
	{
		t = Mathf.Clamp01(t);
		return new VectorXZ(from.x + (to.x - from.x) * t, from.z + (to.z - from.z) * t);
	}

	public void Scale(VectorXZ scale)
	{
		x *= scale.x;
		z *= scale.z;
	}

	public void Normalize()
	{
		float num = magnitude;
		if (num > 1E-05f)
		{
			this /= num;
		}
		else
		{
			this = zero;
		}
	}

	public override string ToString()
	{
		return string.Format("({0:F1}, {1:F1})", new object[2] { x, z });
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ (z.GetHashCode() << 2);
	}

	public override bool Equals(object other)
	{
		if (!(other is VectorXZ vectorXZ))
		{
			return false;
		}
		return x.Equals(vectorXZ.x) && z.Equals(vectorXZ.z);
	}

	public static float Dot(VectorXZ lhs, VectorXZ rhs)
	{
		return lhs.x * rhs.x + lhs.z * rhs.z;
	}

	public static float Distance(VectorXZ a, VectorXZ b)
	{
		return (a - b).magnitude;
	}

	public static float SqrMagnitude(VectorXZ a)
	{
		return a.x * a.x + a.z * a.z;
	}

	public Vector3 toVector3(float y)
	{
		return new Vector3(x, y, z);
	}

	public static Vector3 operator +(VectorXZ a, Vector3 b)
	{
		return new Vector3(a.x + b.x, b.y, a.z + b.z);
	}

	public static VectorXZ operator +(VectorXZ a, VectorXZ b)
	{
		return new VectorXZ(a.x + b.x, a.z + b.z);
	}

	public static VectorXZ operator -(VectorXZ a, VectorXZ b)
	{
		return new VectorXZ(a.x - b.x, a.z - b.z);
	}

	public static VectorXZ operator -(VectorXZ a)
	{
		return new VectorXZ(0f - a.x, 0f - a.z);
	}

	public static VectorXZ operator *(VectorXZ a, float d)
	{
		return new VectorXZ(a.x * d, a.z * d);
	}

	public static VectorXZ operator *(float d, VectorXZ a)
	{
		return new VectorXZ(a.x * d, a.z * d);
	}

	public static VectorXZ operator /(VectorXZ a, float d)
	{
		return new VectorXZ(a.x / d, a.z / d);
	}

	public static bool operator ==(VectorXZ lhs, VectorXZ rhs)
	{
		return SqrMagnitude(lhs - rhs) < 9.9999994E-11f;
	}

	public static bool operator !=(VectorXZ lhs, VectorXZ rhs)
	{
		return SqrMagnitude(lhs - rhs) >= 9.9999994E-11f;
	}

	public static implicit operator VectorXZ(Vector3 v)
	{
		return new VectorXZ(v.x, v.z);
	}

	public static implicit operator Vector3(VectorXZ v)
	{
		return new Vector3(v.x, 0f, v.z);
	}
}
