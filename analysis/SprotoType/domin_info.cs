using Sproto;

namespace SprotoType;

public class domin_info : SprotoTypeBase
{
	private static int max_field_count = 13;

	private string _id;

	private long _max_donmin;

	private long _donmin_time;

	private long _end_time;

	private long _res_time1;

	private long _res_count1;

	private long _res_time2;

	private long _res_count2;

	private long _state;

	private long _serverId;

	private long _serverType;

	private long _res_end_time1;

	private long _res_end_time2;

	public string id
	{
		get
		{
			return _id;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_id = value;
		}
	}

	public bool HasId => has_field.has_field(0);

	public long max_donmin
	{
		get
		{
			return _max_donmin;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_max_donmin = value;
		}
	}

	public bool HasMax_donmin => has_field.has_field(1);

	public long donmin_time
	{
		get
		{
			return _donmin_time;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_donmin_time = value;
		}
	}

	public bool HasDonmin_time => has_field.has_field(2);

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

	public long res_time1
	{
		get
		{
			return _res_time1;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_res_time1 = value;
		}
	}

	public bool HasRes_time1 => has_field.has_field(4);

	public long res_count1
	{
		get
		{
			return _res_count1;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_res_count1 = value;
		}
	}

	public bool HasRes_count1 => has_field.has_field(5);

	public long res_time2
	{
		get
		{
			return _res_time2;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_res_time2 = value;
		}
	}

	public bool HasRes_time2 => has_field.has_field(6);

	public long res_count2
	{
		get
		{
			return _res_count2;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_res_count2 = value;
		}
	}

	public bool HasRes_count2 => has_field.has_field(7);

	public long state
	{
		get
		{
			return _state;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_state = value;
		}
	}

	public bool HasState => has_field.has_field(8);

	public long serverId
	{
		get
		{
			return _serverId;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_serverId = value;
		}
	}

	public bool HasServerId => has_field.has_field(9);

	public long serverType
	{
		get
		{
			return _serverType;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_serverType = value;
		}
	}

	public bool HasServerType => has_field.has_field(10);

	public long res_end_time1
	{
		get
		{
			return _res_end_time1;
		}
		set
		{
			has_field.set_field(11, is_has: true);
			_res_end_time1 = value;
		}
	}

	public bool HasRes_end_time1 => has_field.has_field(11);

	public long res_end_time2
	{
		get
		{
			return _res_end_time2;
		}
		set
		{
			has_field.set_field(12, is_has: true);
			_res_end_time2 = value;
		}
	}

	public bool HasRes_end_time2 => has_field.has_field(12);

	public domin_info()
		: base(max_field_count)
	{
	}

	public domin_info(byte[] buffer)
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
				id = deserialize.read_string();
				break;
			case 1:
				max_donmin = deserialize.read_integer();
				break;
			case 2:
				donmin_time = deserialize.read_integer();
				break;
			case 3:
				end_time = deserialize.read_integer();
				break;
			case 4:
				res_time1 = deserialize.read_integer();
				break;
			case 5:
				res_count1 = deserialize.read_integer();
				break;
			case 6:
				res_time2 = deserialize.read_integer();
				break;
			case 7:
				res_count2 = deserialize.read_integer();
				break;
			case 8:
				state = deserialize.read_integer();
				break;
			case 9:
				serverId = deserialize.read_integer();
				break;
			case 10:
				serverType = deserialize.read_integer();
				break;
			case 11:
				res_end_time1 = deserialize.read_integer();
				break;
			case 12:
				res_end_time2 = deserialize.read_integer();
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
			serialize.write_string(id, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(max_donmin, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(donmin_time, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(end_time, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(res_time1, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(res_count1, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(res_time2, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(res_count2, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(state, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(serverId, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(serverType, 10);
		}
		if (has_field.has_field(11))
		{
			serialize.write_integer(res_end_time1, 11);
		}
		if (has_field.has_field(12))
		{
			serialize.write_integer(res_end_time2, 12);
		}
		return serialize.close();
	}
}
