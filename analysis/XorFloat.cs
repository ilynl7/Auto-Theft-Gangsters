using System;

[Serializable]
public class XorFloat
{
	private byte[] key = new byte[4];

	private byte[] bytes = new byte[4];

	private float realValue;

	public float value
	{
		get
		{
			return Xor();
		}
		set
		{
			realValue = value;
			GenerateKey();
			Xor(value);
		}
	}

	public XorFloat()
		: this(0f)
	{
	}

	public XorFloat(float value)
	{
		GenerateKey();
		realValue = value;
		Xor(value);
	}

	private void GenerateKey()
	{
		new Random().NextBytes(key);
	}

	private void Xor(float x)
	{
		bytes = BitConverter.GetBytes(x);
		bytes[0] ^= key[0];
		bytes[1] ^= key[1];
		bytes[2] ^= key[2];
		bytes[3] ^= key[3];
	}

	private float Xor()
	{
		byte[] array = new byte[4]
		{
			(byte)(bytes[0] ^ key[0]),
			(byte)(bytes[1] ^ key[1]),
			(byte)(bytes[2] ^ key[2]),
			(byte)(bytes[3] ^ key[3])
		};
		if (realValue != BitConverter.ToSingle(array, 0))
		{
			SingletonDontDestoryUnity<NetManager>.Instance.GameCheck();
		}
		return BitConverter.ToSingle(array, 0);
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

	public static implicit operator float(XorFloat xor)
	{
		return xor?.value ?? 0f;
	}

	public static implicit operator XorFloat(float val)
	{
		return new XorFloat(val);
	}
}
