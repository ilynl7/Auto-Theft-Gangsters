using Sproto;

namespace SprotoType;

public class daily_active : SprotoTypeBase
{
	private static int max_field_count = 3;

	private string _ID;

	private long _count;

	private long _Type;

	public string ID
	{
		get
		{
			return _ID;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_ID = value;
		}
	}

	public bool HasID => has_field.has_field(0);

	public long count
	{
		get
		{
			return _count;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_count = value;
		}
	}

	public bool HasCount => has_field.has_field(1);

	public long Type
	{
		get
		{
			return _Type;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_Type = value;
		}
	}

	public bool HasType => has_field.has_field(2);

	public daily_active()
		: base(max_field_count)
	{
	}

	public daily_active(byte[] buffer)
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
				ID = deserialize.read_string();
				break;
			case 1:
				count = deserialize.read_integer();
				break;
			case 2:
				Type = deserialize.read_integer();
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
			serialize.write_string(ID, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(count, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(Type, 2);
		}
		return serialize.close();
	}
}
