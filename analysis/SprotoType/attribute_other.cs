using Sproto;

namespace SprotoType;

public class attribute_other : SprotoTypeBase
{
	private static int max_field_count = 19;

	private long _hp;

	private long _exp;

	private long _level;

	private long _combValue;

	private long _title_level;

	private long _title_exp;

	private long _guildId;

	private long _guildJob;

	private string _guildName;

	private long _refineNeckLevel;

	private long _refineRing1Level;

	private long _refineRing2Level;

	private long _refineBeltLevel;

	private long _refineLevel;

	private long _vip;

	private long _camp;

	private long _pkMode;

	private long _dance_state;

	private string _dance_id;

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

	public long guildId
	{
		get
		{
			return _guildId;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_guildId = value;
		}
	}

	public bool HasGuildId => has_field.has_field(6);

	public long guildJob
	{
		get
		{
			return _guildJob;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_guildJob = value;
		}
	}

	public bool HasGuildJob => has_field.has_field(7);

	public string guildName
	{
		get
		{
			return _guildName;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_guildName = value;
		}
	}

	public bool HasGuildName => has_field.has_field(8);

	public long refineNeckLevel
	{
		get
		{
			return _refineNeckLevel;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_refineNeckLevel = value;
		}
	}

	public bool HasRefineNeckLevel => has_field.has_field(9);

	public long refineRing1Level
	{
		get
		{
			return _refineRing1Level;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_refineRing1Level = value;
		}
	}

	public bool HasRefineRing1Level => has_field.has_field(10);

	public long refineRing2Level
	{
		get
		{
			return _refineRing2Level;
		}
		set
		{
			has_field.set_field(11, is_has: true);
			_refineRing2Level = value;
		}
	}

	public bool HasRefineRing2Level => has_field.has_field(11);

	public long refineBeltLevel
	{
		get
		{
			return _refineBeltLevel;
		}
		set
		{
			has_field.set_field(12, is_has: true);
			_refineBeltLevel = value;
		}
	}

	public bool HasRefineBeltLevel => has_field.has_field(12);

	public long refineLevel
	{
		get
		{
			return _refineLevel;
		}
		set
		{
			has_field.set_field(13, is_has: true);
			_refineLevel = value;
		}
	}

	public bool HasRefineLevel => has_field.has_field(13);

	public long vip
	{
		get
		{
			return _vip;
		}
		set
		{
			has_field.set_field(14, is_has: true);
			_vip = value;
		}
	}

	public bool HasVip => has_field.has_field(14);

	public long camp
	{
		get
		{
			return _camp;
		}
		set
		{
			has_field.set_field(15, is_has: true);
			_camp = value;
		}
	}

	public bool HasCamp => has_field.has_field(15);

	public long pkMode
	{
		get
		{
			return _pkMode;
		}
		set
		{
			has_field.set_field(16, is_has: true);
			_pkMode = value;
		}
	}

	public bool HasPkMode => has_field.has_field(16);

	public long dance_state
	{
		get
		{
			return _dance_state;
		}
		set
		{
			has_field.set_field(17, is_has: true);
			_dance_state = value;
		}
	}

	public bool HasDance_state => has_field.has_field(17);

	public string dance_id
	{
		get
		{
			return _dance_id;
		}
		set
		{
			has_field.set_field(18, is_has: true);
			_dance_id = value;
		}
	}

	public bool HasDance_id => has_field.has_field(18);

	public attribute_other()
		: base(max_field_count)
	{
	}

	public attribute_other(byte[] buffer)
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
			case 6:
				guildId = deserialize.read_integer();
				break;
			case 7:
				guildJob = deserialize.read_integer();
				break;
			case 8:
				guildName = deserialize.read_string();
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
			case 14:
				vip = deserialize.read_integer();
				break;
			case 15:
				camp = deserialize.read_integer();
				break;
			case 16:
				pkMode = deserialize.read_integer();
				break;
			case 17:
				dance_state = deserialize.read_integer();
				break;
			case 18:
				dance_id = deserialize.read_string();
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
			serialize.write_integer(guildId, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(guildJob, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_string(guildName, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(refineNeckLevel, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(refineRing1Level, 10);
		}
		if (has_field.has_field(11))
		{
			serialize.write_integer(refineRing2Level, 11);
		}
		if (has_field.has_field(12))
		{
			serialize.write_integer(refineBeltLevel, 12);
		}
		if (has_field.has_field(13))
		{
			serialize.write_integer(refineLevel, 13);
		}
		if (has_field.has_field(14))
		{
			serialize.write_integer(vip, 14);
		}
		if (has_field.has_field(15))
		{
			serialize.write_integer(camp, 15);
		}
		if (has_field.has_field(16))
		{
			serialize.write_integer(pkMode, 16);
		}
		if (has_field.has_field(17))
		{
			serialize.write_integer(dance_state, 17);
		}
		if (has_field.has_field(18))
		{
			serialize.write_string(dance_id, 18);
		}
		return serialize.close();
	}
}
