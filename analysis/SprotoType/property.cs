using Sproto;

namespace SprotoType;

public class property : SprotoTypeBase
{
	private static int max_field_count = 7;

	private long _money1;

	private long _money2;

	private long _money3;

	private long _money4;

	private long _money5;

	private long _money6;

	public long money1
	{
		get
		{
			return _money1;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_money1 = value;
		}
	}

	public bool HasMoney1 => has_field.has_field(0);

	public long money2
	{
		get
		{
			return _money2;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_money2 = value;
		}
	}

	public bool HasMoney2 => has_field.has_field(1);

	public long money3
	{
		get
		{
			return _money3;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_money3 = value;
		}
	}

	public bool HasMoney3 => has_field.has_field(2);

	public long money4
	{
		get
		{
			return _money4;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_money4 = value;
		}
	}

	public bool HasMoney4 => has_field.has_field(3);

	public long money5
	{
		get
		{
			return _money5;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_money5 = value;
		}
	}

	public bool HasMoney5 => has_field.has_field(4);

	public long money6
	{
		get
		{
			return _money6;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_money6 = value;
		}
	}

	public bool HasMoney6 => has_field.has_field(5);

	public property()
		: base(max_field_count)
	{
	}

	public property(byte[] buffer)
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
			case 13:
				money1 = deserialize.read_integer();
				break;
			case 14:
				money2 = deserialize.read_integer();
				break;
			case 15:
				money3 = deserialize.read_integer();
				break;
			case 16:
				money4 = deserialize.read_integer();
				break;
			case 17:
				money5 = deserialize.read_integer();
				break;
			case 18:
				money6 = deserialize.read_integer();
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
			serialize.write_integer(money1, 13);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(money2, 14);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(money3, 15);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(money4, 16);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(money5, 17);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(money6, 18);
		}
		return serialize.close();
	}
}
