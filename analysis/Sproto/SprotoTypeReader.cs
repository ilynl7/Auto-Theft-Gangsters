namespace Sproto;

public class SprotoTypeReader
{
	private byte[] buffer;

	private int begin;

	private int pos;

	private int size;

	public byte[] Buffer => buffer;

	public int Position => pos - begin;

	public int Offset => pos;

	public int Length => size - begin;

	public SprotoTypeReader(byte[] buffer, int offset, int size)
	{
		Init(buffer, offset, size);
	}

	public SprotoTypeReader()
	{
	}

	public void Init(byte[] buffer, int offset, int size)
	{
		begin = offset;
		pos = offset;
		this.buffer = buffer;
		this.size = offset + size;
		check();
	}

	private void check()
	{
		if (pos > size || begin > pos)
		{
			SprotoTypeSize.error("invalid pos.");
		}
	}

	public byte ReadByte()
	{
		check();
		return buffer[pos++];
	}

	public void Seek(int offset)
	{
		pos = begin + offset;
		check();
	}

	public void Read(byte[] data, int offset, int size)
	{
		int num = pos;
		pos += size;
		check();
		for (int i = num; i < pos; i++)
		{
			data[offset + i - num] = buffer[i];
		}
	}
}
