using System.IO;

namespace Sproto;

public abstract class SprotoTypeBase
{
	protected SprotoTypeFieldOP has_field;

	protected SprotoTypeSerialize serialize;

	protected SprotoTypeDeserialize deserialize;

	public SprotoTypeBase(int max_field_count)
	{
		has_field = new SprotoTypeFieldOP(max_field_count);
		serialize = new SprotoTypeSerialize(max_field_count);
		deserialize = new SprotoTypeDeserialize();
	}

	public SprotoTypeBase(int max_field_count, byte[] buffer)
	{
		has_field = new SprotoTypeFieldOP(max_field_count);
		serialize = new SprotoTypeSerialize(max_field_count);
		deserialize = new SprotoTypeDeserialize(buffer);
	}

	public int init(byte[] buffer, int offset = 0, int len = 0)
	{
		clear();
		deserialize.init(buffer, offset, len);
		decode();
		return deserialize.size();
	}

	public long init(SprotoTypeReader reader)
	{
		clear();
		deserialize.init(reader);
		decode();
		return deserialize.size();
	}

	public abstract int encode(SprotoStream stream);

	public byte[] encode()
	{
		SprotoStream sprotoStream = new SprotoStream();
		encode(sprotoStream);
		int position = sprotoStream.Position;
		byte[] array = new byte[position];
		sprotoStream.Seek(0, SeekOrigin.Begin);
		sprotoStream.Read(array, 0, position);
		return array;
	}

	protected abstract void decode();

	public void clear()
	{
		has_field.clear_field();
		deserialize.clear();
	}
}
