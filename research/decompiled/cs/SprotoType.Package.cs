using Sproto;

namespace SprotoType;

public class Package : SprotoTypeBase
{
	private static int max_field_count = 2;

	private long _type;

	private long _session;

	public long type
	{
		get
		{
			return _type;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_type = value;
		}
	}

	public bool HasType => has_field.has_field(0);

	public long session
	{
		get
		{
			return _session;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_session = value;
		}
	}

	public bool HasSession => has_field.has_field(1);

	public Package()
		: base(max_field_count)
	{
	}

	public Package(byte[] buffer)
		: base(max_field_count, buffer)
	{
		decode();
	}

	public void Reset()
	{
		has_field.set_field(0, is_has: false);
		has_field.set_field(1, is_has: false);
	}

	protected override void decode()
	{
		int num = -1;
		while ((num = deserialize.read_tag()) != -1)
		{
			switch (num)
			{
			case 0:
				type = deserialize.read_integer();
				break;
			case 1:
				session = deserialize.read_integer();
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
			serialize.write_integer(type, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(session, 1);
		}
		return serialize.close();
	}
}
