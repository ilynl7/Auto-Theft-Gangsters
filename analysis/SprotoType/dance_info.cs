using Sproto;

namespace SprotoType;

public class dance_info : SprotoTypeBase
{
	private static int max_field_count = 4;

	private string _ID;

	private bool _enable;

	private long _useType;

	private long _endTime;

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

	public bool enable
	{
		get
		{
			return _enable;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_enable = value;
		}
	}

	public bool HasEnable => has_field.has_field(1);

	public long useType
	{
		get
		{
			return _useType;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_useType = value;
		}
	}

	public bool HasUseType => has_field.has_field(2);

	public long endTime
	{
		get
		{
			return _endTime;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_endTime = value;
		}
	}

	public bool HasEndTime => has_field.has_field(3);

	public dance_info()
		: base(max_field_count)
	{
	}

	public dance_info(byte[] buffer)
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
				enable = deserialize.read_boolean();
				break;
			case 2:
				useType = deserialize.read_integer();
				break;
			case 3:
				endTime = deserialize.read_integer();
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
			serialize.write_boolean(enable, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(useType, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(endTime, 3);
		}
		return serialize.close();
	}
}
