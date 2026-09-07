using Sproto;

namespace SprotoType;

public class tower_info : SprotoTypeBase
{
	private static int max_field_count = 7;

	private long _floor;

	private long _cur_floor;

	private long _times;

	private long _sum_time;

	private long _wipe_out_state;

	private long _wipe_time;

	private long _max_floor;

	public long floor
	{
		get
		{
			return _floor;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_floor = value;
		}
	}

	public bool HasFloor => has_field.has_field(0);

	public long cur_floor
	{
		get
		{
			return _cur_floor;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_cur_floor = value;
		}
	}

	public bool HasCur_floor => has_field.has_field(1);

	public long times
	{
		get
		{
			return _times;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_times = value;
		}
	}

	public bool HasTimes => has_field.has_field(2);

	public long sum_time
	{
		get
		{
			return _sum_time;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_sum_time = value;
		}
	}

	public bool HasSum_time => has_field.has_field(3);

	public long wipe_out_state
	{
		get
		{
			return _wipe_out_state;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_wipe_out_state = value;
		}
	}

	public bool HasWipe_out_state => has_field.has_field(4);

	public long wipe_time
	{
		get
		{
			return _wipe_time;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_wipe_time = value;
		}
	}

	public bool HasWipe_time => has_field.has_field(5);

	public long max_floor
	{
		get
		{
			return _max_floor;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_max_floor = value;
		}
	}

	public bool HasMax_floor => has_field.has_field(6);

	public tower_info()
		: base(max_field_count)
	{
	}

	public tower_info(byte[] buffer)
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
				floor = deserialize.read_integer();
				break;
			case 1:
				cur_floor = deserialize.read_integer();
				break;
			case 2:
				times = deserialize.read_integer();
				break;
			case 3:
				sum_time = deserialize.read_integer();
				break;
			case 4:
				wipe_out_state = deserialize.read_integer();
				break;
			case 5:
				wipe_time = deserialize.read_integer();
				break;
			case 6:
				max_floor = deserialize.read_integer();
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
			serialize.write_integer(floor, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(cur_floor, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(times, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(sum_time, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(wipe_out_state, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(wipe_time, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(max_floor, 6);
		}
		return serialize.close();
	}
}
