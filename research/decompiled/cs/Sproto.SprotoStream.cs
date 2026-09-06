using System;
using System.IO;

namespace Sproto;

public class SprotoStream
{
	private int size;

	private int pos;

	private byte[] buffer;

	public int Position => pos;

	public byte[] Buffer => buffer;

	public byte this[int i]
	{
		get
		{
			if (i < 0 || i >= size)
			{
				throw new Exception("invalid idx:" + i + "@get");
			}
			return buffer[i];
		}
		set
		{
			if (i < 0 || i >= size)
			{
				throw new Exception("invalid idx:" + i + "@set");
			}
			buffer[i] = value;
		}
	}

	public SprotoStream()
	{
		size = 128;
		pos = 0;
		buffer = new byte[size];
	}

	private void _expand(int sz = 0)
	{
		if (size - pos < sz)
		{
			long num = size;
			while (size - pos < sz)
			{
				size *= 2;
			}
			if (size >= SprotoTypeSize.encode_max_size)
			{
				SprotoTypeSize.error("object is too large (>" + SprotoTypeSize.encode_max_size + ")");
			}
			byte[] array = new byte[size];
			for (long num2 = 0L; num2 < num; num2++)
			{
				array[num2] = buffer[num2];
			}
			buffer = array;
		}
	}

	public void WriteByte(byte v)
	{
		_expand(1);
		buffer[pos++] = v;
	}

	public void Write(byte[] data, int offset, int count)
	{
		_expand(count);
		for (int i = 0; i < count; i++)
		{
			buffer[pos++] = data[offset + i];
		}
	}

	public int Seek(int offset, SeekOrigin loc)
	{
		switch (loc)
		{
		case SeekOrigin.Begin:
			pos = offset;
			break;
		case SeekOrigin.Current:
			pos += offset;
			break;
		case SeekOrigin.End:
			pos = size + offset;
			break;
		}
		_expand();
		return pos;
	}

	public void Read(byte[] buffer, int offset, int count)
	{
		for (int i = 0; i < count; i++)
		{
			buffer[offset + i] = this.buffer[pos++];
		}
	}

	public void MoveUp(int position, int up_count)
	{
		if (up_count > 0)
		{
			long num = pos - position;
			for (int i = 0; i < num; i++)
			{
				buffer[position - up_count + i] = buffer[position + i];
			}
			pos -= up_count;
		}
	}
}
