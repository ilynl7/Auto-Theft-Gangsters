using Sproto;

namespace SprotoType;

public class attribute_aoi : SprotoTypeBase
{
	private static int max_field_count = 12;

	private long _hp;

	private long _exp;

	private long _level;

	private long _combValue;

	private long _title_level;

	private long _title_exp;

	private long _refineNeckLevel;

	private long _refineRing1Level;

	private long _refineRing2Level;

	private long _refineBeltLevel;

	private long _refineLevel;

	public long hp
	{
		get
		{
			return _hp;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_hp = value;
		}
	}

	public bool HasHp => has_field.has_field(0);

	public long exp
	{
		get
		{
			return _exp;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_exp = value;
		}
	}

	public bool HasExp => has_field.has_field(1);

	public long level
	{
		get
		{
			return _level;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_level = value;
		}
	}

	public bool HasLevel => has_field.has_field(2);

	public long combValue
	{
		get
		{
			return _combValue;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_combValue = value;
		}
	}

	public bool HasCombValue => has_field.has_field(3);

	public long title_level
	{
		get
		{
			return _title_level;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_title_level = value;
		}
	}

	public bool HasTitle_level => has_field.has_field(4);

	public long title_exp
	{
		get
		{
			return _title_exp;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_title_exp = value;
		}
	}

	public bool HasTitle_exp => has_field.has_field(5);

	public long refineNeckLevel
	{
		get
		{
			return _refineNeckLevel;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_refineNeckLevel = value;
		}
	}

	public bool HasRefineNeckLevel => has_field.has_field(6);

	public long refineRing1Level
	{
		get
		{
			return _refineRing1Level;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_refineRing1Level = value;
		}
	}

	public bool HasRefineRing1Level => has_field.has_field(7);

	public long refineRing2Level
	{
		get
		{
			return _refineRing2Level;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_refineRing2Level = value;
		}
	}

	public bool HasRefineRing2Level => has_field.has_field(8);

	public long refineBeltLevel
	{
		get
		{
			return _refineBeltLevel;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_refineBeltLevel = value;
		}
	}

	public bool HasRefineBeltLevel => has_field.has_field(9);

	public long refineLevel
	{
		get
		{
			return _refineLevel;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_refineLevel = value;
		}
	}

	public bool HasRefineLevel => has_field.has_field(10);

	public attribute_aoi()
		: base(max_field_count)
	{
	}

	public attribute_aoi(byte[] buffer)
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
				hp = deserialize.read_integer();
				break;
			case 1:
				exp = deserialize.read_integer();
				break;
			case 2:
				level = deserialize.read_integer();
				break;
			case 3:
				combValue = deserialize.read_integer();
				break;
			case 4:
				title_level = deserialize.read_integer();
				break;
			case 5:
				title_exp = deserialize.read_integer();
				break;
			case 9:
				refineNeckLevel = deserialize.read_integer();
				break;
			case 10:
				refineRing1Level = deserialize.read_integer();
				break;
			case 11:
				refineRing2Level = deserialize.read_integer();
				break;
			case 12:
				refineBeltLevel = deserialize.read_integer();
				break;
			case 13:
				refineLevel = deserialize.read_integer();
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
			serialize.write_integer(hp, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(exp, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(level, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(combValue, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(title_level, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(title_exp, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(refineNeckLevel, 9);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(refineRing1Level, 10);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(refineRing2Level, 11);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(refineBeltLevel, 12);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(refineLevel, 13);
		}
		return serialize.close();
	}
}
