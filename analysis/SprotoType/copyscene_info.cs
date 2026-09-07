using Sproto;

namespace SprotoType;

public class copyscene_info : SprotoTypeBase
{
	private static int max_field_count = 8;

	private string _ID;

	private long _CurNum;

	private long _BestGrade;

	private long _Type;

	private string _str;

	private bool _enable;

	private long _state;

	private long _Type2;

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

	public long BestGrade
	{
		get
		{
			return _BestGrade;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_BestGrade = value;
		}
	}

	public bool HasBestGrade => has_field.has_field(2);

	public long Type
	{
		get
		{
			return _Type;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_Type = value;
		}
	}

	public bool HasType => has_field.has_field(3);

	public string str
	{
		get
		{
			return _str;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_str = value;
		}
	}

	public bool HasStr => has_field.has_field(4);

	public bool enable
	{
		get
		{
			return _enable;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_enable = value;
		}
	}

	public bool HasEnable => has_field.has_field(5);

	public long state
	{
		get
		{
			return _state;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_state = value;
		}
	}

	public bool HasState => has_field.has_field(6);

	public long Type2
	{
		get
		{
			return _Type2;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_Type2 = value;
		}
	}

	public bool HasType2 => has_field.has_field(7);

	public copyscene_info()
		: base(max_field_count)
	{
	}

	public copyscene_info(byte[] buffer)
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
				BestGrade = deserialize.read_integer();
				break;
			case 3:
				Type = deserialize.read_integer();
				break;
			case 4:
				str = deserialize.read_string();
				break;
			case 5:
				enable = deserialize.read_boolean();
				break;
			case 6:
				state = deserialize.read_integer();
				break;
			case 7:
				Type2 = deserialize.read_integer();
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
			serialize.write_integer(BestGrade, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(Type, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_string(str, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_boolean(enable, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(state, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(Type2, 7);
		}
		return serialize.close();
	}
}
