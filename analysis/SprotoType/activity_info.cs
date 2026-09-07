using Sproto;

namespace SprotoType;

public class activity_info : SprotoTypeBase
{
	private static int max_field_count = 9;

	private string _ID;

	private long _CurNum;

	private long _Type;

	private long _State;

	private long _Parm;

	private string _Parmstr;

	private long _sign;

	private long _time;

	private long _next;

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

	public long CurNum
	{
		get
		{
			return _CurNum;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_CurNum = value;
		}
	}

	public bool HasCurNum => has_field.has_field(1);

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

	public long State
	{
		get
		{
			return _State;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_State = value;
		}
	}

	public bool HasState => has_field.has_field(3);

	public long Parm
	{
		get
		{
			return _Parm;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_Parm = value;
		}
	}

	public bool HasParm => has_field.has_field(4);

	public string Parmstr
	{
		get
		{
			return _Parmstr;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_Parmstr = value;
		}
	}

	public bool HasParmstr => has_field.has_field(5);

	public long sign
	{
		get
		{
			return _sign;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_sign = value;
		}
	}

	public bool HasSign => has_field.has_field(6);

	public long time
	{
		get
		{
			return _time;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_time = value;
		}
	}

	public bool HasTime => has_field.has_field(7);

	public long next
	{
		get
		{
			return _next;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_next = value;
		}
	}

	public bool HasNext => has_field.has_field(8);

	public activity_info()
		: base(max_field_count)
	{
	}

	public activity_info(byte[] buffer)
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
				CurNum = deserialize.read_integer();
				break;
			case 2:
				Type = deserialize.read_integer();
				break;
			case 3:
				State = deserialize.read_integer();
				break;
			case 4:
				Parm = deserialize.read_integer();
				break;
			case 5:
				Parmstr = deserialize.read_string();
				break;
			case 6:
				sign = deserialize.read_integer();
				break;
			case 7:
				time = deserialize.read_integer();
				break;
			case 8:
				next = deserialize.read_integer();
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
			serialize.write_integer(CurNum, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(Type, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(State, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(Parm, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_string(Parmstr, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(sign, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(time, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(next, 8);
		}
		return serialize.close();
	}
}
