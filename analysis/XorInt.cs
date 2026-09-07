using System;
using UnityEngine;

[Serializable]
public class XorInt
{
	private int key;

	private int realValue;

	public int rawValue { get; private set; }

	public int value
	{
		get
		{
			if ((rawValue ^ key) != realValue)
			{
				SingletonDontDestoryUnity<NetManager>.Instance.GameCheck();
			}
			return Xor(rawValue);
		}
		set
		{
			GenerateKey();
			realValue = value;
			rawValue = Xor(value);
		}
	}

	public XorInt()
		: this(0)
	{
	}

	public XorInt(int value)
	{
		GenerateKey();
		realValue = value;
		rawValue = Xor(value);
	}

	private void GenerateKey()
	{
		key = UnityEngine.Random.Range(0, 65535);
	}

	private int Xor(int x)
	{
		return x ^ key;
	}

	public override string ToString()
	{
		return value.ToString();
	}

	public string ToString(string format)
	{
		return value.ToString(format);
	}

	public string ToString(IFormatProvider provider)
	{
		return value.ToString(provider);
	}

	public string ToString(string format, IFormatProvider provider)
	{
		return value.ToString(format, provider);
	}

	public static implicit operator int(XorInt xor)
	{
		return xor?.value ?? 0;
	}

	public static implicit operator XorInt(int val)
	{
		return new XorInt(val);
	}

	public static XorInt operator ++(XorInt val)
	{
		val = (int)val + 1;
		return val;
	}

	public static XorInt operator --(XorInt val)
	{
		val = (int)val - 1;
		return val;
	}
}
