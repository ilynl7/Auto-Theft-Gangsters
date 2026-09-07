using Sproto;

namespace SprotoType;

public class dance_state_info : SprotoTypeBase
{
	private static int max_field_count = 9;

	private long _uuid;

	private string _ID;

	private long _start_time;

	private long _end_time;

	private long _state;

	private long _parm;

	private long _duration;

	private long _reset_time;

	private long _parm2;

	public long uuid
	{
		get
		{
			return _uuid;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_uuid = value;
		}
	}

	public bool HasUuid => has_field.has_field(0);

	public string ID
	{
		get
		{
			return _ID;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_ID = value;
		}
	}

	public bool HasID => has_field.has_field(1);

	public long start_time
	{
		get
		{
			return _start_time;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_start_time = value;
		}
	}

	public bool HasStart_time => has_field.has_field(2);

	public long end_time
	{
		get
		{
			return _end_time;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_end_time = value;
		}
	}

	public bool HasEnd_time => has_field.has_field(3);

	public long state
	{
		get
		{
			return _state;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_state = value;
		}
	}

	public bool HasState => has_field.has_field(4);

	public long parm
	{
		get
		{
			return _parm;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_parm = value;
		}
	}

	public bool HasParm => has_field.has_field(5);

	public long duration
	{
		get
		{
			return _duration;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_duration = value;
		}
	}

	public bool HasDuration => has_field.has_field(6);

	public long reset_time
	{
		get
		{
			return _reset_time;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_reset_time = value;
		}
	}

	public bool HasReset_time => has_field.has_field(7);

	public long parm2
	{
		get
		{
			return _parm2;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_parm2 = value;
		}
	}

	public bool HasParm2 => has_field.has_field(8);

	public dance_state_info()
		: base(max_field_count)
	{
	}

	public dance_state_info(byte[] buffer)
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
				uuid = deserialize.read_integer();
				break;
			case 1:
				ID = deserialize.read_string();
				break;
			case 2:
				start_time = deserialize.read_integer();
				break;
			case 3:
				end_time = deserialize.read_integer();
				break;
			case 4:
				state = deserialize.read_integer();
				break;
			case 5:
				parm = deserialize.read_integer();
				break;
			case 6:
				duration = deserialize.read_integer();
				break;
			case 7:
				reset_time = deserialize.read_integer();
				break;
			case 8:
				parm2 = deserialize.read_integer();
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
			serialize.write_integer(uuid, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_string(ID, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(start_time, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(end_time, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(state, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(parm, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(duration, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(reset_time, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(parm2, 8);
		}
		return serialize.close();
	}
}
