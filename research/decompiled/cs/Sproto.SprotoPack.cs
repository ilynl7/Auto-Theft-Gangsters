using System.IO;

namespace Sproto;

public class SprotoPack
{
	private MemoryStream buffer;

	private byte[] tmp;

	public SprotoPack()
	{
		buffer = new MemoryStream();
		tmp = new byte[8];
	}

	private void write_ff(byte[] src, int offset, long pos, int n)
	{
		int num = (n + 7) & -8;
		long position = buffer.Position;
		buffer.Seek(pos, SeekOrigin.Begin);
		buffer.WriteByte(byte.MaxValue);
		buffer.WriteByte((byte)(num / 8 - 1));
		buffer.Write(src, offset, n);
		for (int i = 0; i < num - n; i++)
		{
			buffer.WriteByte(0);
		}
		buffer.Seek(position, SeekOrigin.Begin);
	}

	private int pack_seg(byte[] src, long offset, int ff_n)
	{
		byte b = 0;
		int num = 0;
		long position = buffer.Position;
		buffer.Seek(1L, SeekOrigin.Current);
		for (int i = 0; i < 8; i++)
		{
			if (src[offset + i] != 0)
			{
				num++;
				b |= (byte)(1 << i);
				buffer.WriteByte(src[offset + i]);
			}
		}
		if ((num == 7 || num == 6) && ff_n > 0)
		{
			num = 8;
		}
		if (num == 8)
		{
			if (ff_n > 0)
			{
				buffer.Seek(position, SeekOrigin.Begin);
				return 8;
			}
			buffer.Seek(position, SeekOrigin.Begin);
			return 10;
		}
		buffer.Seek(position, SeekOrigin.Begin);
		buffer.WriteByte(b);
		buffer.Seek(position, SeekOrigin.Begin);
		return num + 1;
	}

	public byte[] pack(byte[] data, byte[] sends, ref int len)
	{
		clear();
		int num = len;
		byte[] array = null;
		int num2 = 0;
		long pos = 0L;
		int num3 = 0;
		byte[] array2 = data;
		int num4 = 0;
		int num6;
		for (int i = 0; i < num; buffer.Seek(num6, SeekOrigin.Current), i += 8)
		{
			num4 = i;
			int num5 = i + 8 - num;
			if (num5 > 0)
			{
				for (int j = 0; j < 8 - num5; j++)
				{
					tmp[j] = array2[i + j];
				}
				for (int k = 0; k < num5; k++)
				{
					tmp[7 - k] = 0;
				}
				array2 = tmp;
				num4 = 0;
			}
			num6 = pack_seg(array2, num4, num3);
			switch (num6)
			{
			case 10:
				array = array2;
				num2 = num4;
				pos = buffer.Position;
				num3 = 1;
				continue;
			case 8:
				if (num3 > 0)
				{
					num3++;
					if (num3 == 256)
					{
						write_ff(array, num2, pos, 2048);
						num3 = 0;
					}
					continue;
				}
				break;
			}
			if (num3 > 0)
			{
				write_ff(array, num2, pos, num3 * 8);
				num3 = 0;
			}
		}
		if (num3 == 1)
		{
			write_ff(array, num2, pos, 8);
		}
		else if (num3 > 1)
		{
			int num7 = ((array != data) ? array.Length : num);
			write_ff(array, num2, pos, num7 - num2);
		}
		long num8 = (num + 2047) / 2048 * 2 + num + 2;
		if (num8 < buffer.Position)
		{
			SprotoTypeSize.error("packing error, return size=" + buffer.Position);
		}
		if (sends.Length < buffer.Position)
		{
			int num9 = (int)buffer.Position;
			sends = new byte[num9 * 2];
		}
		len = (int)buffer.Position;
		buffer.Seek(0L, SeekOrigin.Begin);
		buffer.Read(sends, 0, len);
		return sends;
	}

	public byte[] unpack(byte[] data, ref int len)
	{
		clear();
		int num = len;
		int num2 = len;
		while (num > 0)
		{
			byte b = data[len - num];
			num--;
			if (b == byte.MaxValue)
			{
				if (num < 0)
				{
					SprotoTypeSize.error("invalid unpack stream.");
				}
				int num3 = (data[len - num] + 1) * 8;
				if (num < num3 + 1)
				{
					SprotoTypeSize.error("invalid unpack stream.");
				}
				buffer.Write(data, len - num + 1, num3);
				num -= num3 + 1;
				continue;
			}
			for (int i = 0; i < 8; i++)
			{
				int num4 = (b >> i) & 1;
				if (num4 == 1)
				{
					if (num < 0)
					{
						SprotoTypeSize.error("invalid unpack stream.");
					}
					buffer.WriteByte(data[len - num]);
					num--;
				}
				else
				{
					buffer.WriteByte(0);
				}
			}
		}
		len = (int)buffer.Position;
		buffer.Seek(0L, SeekOrigin.Begin);
		buffer.Read(data, 0, len);
		return data;
	}

	private void clear()
	{
		buffer.Seek(0L, SeekOrigin.Begin);
		for (int i = 0; i < tmp.Length; i++)
		{
			tmp[i] = 0;
		}
	}
}
