using Sproto;

namespace SprotoType;

public class special_big_pack : SprotoTypeBase
{
	private static int max_field_count = 4;

	private string _ID;

	private long _state;

	private long _remain_times;

	private long _end_time;

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

	public long state
	{
		get
		{
			return _state;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_state = value;
		}
	}

	public bool HasState => has_field.has_field(1);

	public long remain_times
	{
		get
		{
			return _remain_times;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_remain_times = value;
		}
	}

	public bool HasRemain_times => has_field.has_field(2);

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

	public special_big_pack()
		: base(max_field_count)
	{
	}

	public special_big_pack(byte[] buffer)
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
				state = deserialize.read_integer();
				break;
			case 2:
				remain_times = deserialize.read_integer();
				break;
			case 3:
				end_time = deserialize.read_integer();
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
			serialize.write_integer(state, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(remain_times, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(end_time, 3);
		}
		return serialize.close();
	}
}
