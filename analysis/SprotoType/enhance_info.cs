using Sproto;

namespace SprotoType;

public class enhance_info : SprotoTypeBase
{
	private static int max_field_count = 2;

	private long _level;

	private long _subType;

	public long level
	{
		get
		{
			return _level;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_level = value;
		}
	}

	public bool HasLevel => has_field.has_field(0);

	public long subType
	{
		get
		{
			return _subType;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_subType = value;
		}
	}

	public bool HasSubType => has_field.has_field(1);

	public enhance_info()
		: base(max_field_count)
	{
	}

	public enhance_info(byte[] buffer)
		: base(max_field_count, buffer)
	{
		decode();
	}

	protected override void decode()
	{
		int num = -1;
		while ((num = deserialize.read_tag()) != -1)
		{
			switch (num)
			{
			case 0:
				level = deserialize.read_integer();
				break;
			case 1:
				subType = deserialize.read_integer();
				break;
			default:
				deserialize.read_unknow_data();
				break;
			}
		}
	}

	public override int encode(SprotoStream stream)
	{
		serialize.open(stream);
		if (has_field.has_field(0))
		{
			serialize.write_integer(level, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(subType, 1);
		}
		return serialize.close();
	}
}
