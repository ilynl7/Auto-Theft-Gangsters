using Sproto;

namespace SprotoType;

public class slot_info : SprotoTypeBase
{
	private static int max_field_count = 3;

	private long _curNum;

	private long _sumNum;

	public long curNum
	{
		get
		{
			return _curNum;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_curNum = value;
		}
	}

	public bool HasCurNum => has_field.has_field(0);

	public long sumNum
	{
		get
		{
			return _sumNum;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_sumNum = value;
		}
	}

	public bool HasSumNum => has_field.has_field(1);

	public slot_info()
		: base(max_field_count)
	{
	}

	public slot_info(byte[] buffer)
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
			case 1:
				curNum = deserialize.read_integer();
				break;
			case 2:
				sumNum = deserialize.read_integer();
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
			serialize.write_integer(curNum, 1);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(sumNum, 2);
		}
		return serialize.close();
	}
}
