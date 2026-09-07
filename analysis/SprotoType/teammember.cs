using Sproto;

namespace SprotoType;

public class teammember : SprotoTypeBase
{
	private static int max_field_count = 18;

	private long _id;

	private long _teamid;

	private string _name;

	private long _level;

	private long _profession;

	private long _combValue;

	private long _mapInfoId;

	private long _lineIndex;

	private long _memberType;

	private long _hp;

	private long _max_hp;

	private long _vip;

	private long _time;

	private long _apply_time;

	private characterVisual _visual;

	private long _curNum;

	private long _guildId;

	private string _guildName;

	public long id
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

	public long teamid
	{
		get
		{
			return _teamid;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_teamid = value;
		}
	}

	public bool HasTeamid => has_field.has_field(1);

	public string name
	{
		get
		{
			return _name;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_name = value;
		}
	}

	public bool HasName => has_field.has_field(2);

	public long level
	{
		get
		{
			return _level;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_level = value;
		}
	}

	public bool HasLevel => has_field.has_field(3);

	public long profession
	{
		get
		{
			return _profession;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_profession = value;
		}
	}

	public bool HasProfession => has_field.has_field(4);

	public long combValue
	{
		get
		{
			return _combValue;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_combValue = value;
		}
	}

	public bool HasCombValue => has_field.has_field(5);

	public long mapInfoId
	{
		get
		{
			return _mapInfoId;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_mapInfoId = value;
		}
	}

	public bool HasMapInfoId => has_field.has_field(6);

	public long lineIndex
	{
		get
		{
			return _lineIndex;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_lineIndex = value;
		}
	}

	public bool HasLineIndex => has_field.has_field(7);

	public long memberType
	{
		get
		{
			return _memberType;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_memberType = value;
		}
	}

	public bool HasMemberType => has_field.has_field(8);

	public long hp
	{
		get
		{
			return _hp;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_hp = value;
		}
	}

	public bool HasHp => has_field.has_field(9);

	public long max_hp
	{
		get
		{
			return _max_hp;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_max_hp = value;
		}
	}

	public bool HasMax_hp => has_field.has_field(10);

	public long vip
	{
		get
		{
			return _vip;
		}
		set
		{
			has_field.set_field(11, is_has: true);
			_vip = value;
		}
	}

	public bool HasVip => has_field.has_field(11);

	public long time
	{
		get
		{
			return _time;
		}
		set
		{
			has_field.set_field(12, is_has: true);
			_time = value;
		}
	}

	public bool HasTime => has_field.has_field(12);

	public long apply_time
	{
		get
		{
			return _apply_time;
		}
		set
		{
			has_field.set_field(13, is_has: true);
			_apply_time = value;
		}
	}

	public bool HasApply_time => has_field.has_field(13);

	public characterVisual visual
	{
		get
		{
			return _visual;
		}
		set
		{
			has_field.set_field(14, is_has: true);
			_visual = value;
		}
	}

	public bool HasVisual => has_field.has_field(14);

	public long curNum
	{
		get
		{
			return _curNum;
		}
		set
		{
			has_field.set_field(15, is_has: true);
			_curNum = value;
		}
	}

	public bool HasCurNum => has_field.has_field(15);

	public long guildId
	{
		get
		{
			return _guildId;
		}
		set
		{
			has_field.set_field(16, is_has: true);
			_guildId = value;
		}
	}

	public bool HasGuildId => has_field.has_field(16);

	public string guildName
	{
		get
		{
			return _guildName;
		}
		set
		{
			has_field.set_field(17, is_has: true);
			_guildName = value;
		}
	}

	public bool HasGuildName => has_field.has_field(17);

	public teammember()
		: base(max_field_count)
	{
	}

	public teammember(byte[] buffer)
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
				id = deserialize.read_integer();
				break;
			case 1:
				teamid = deserialize.read_integer();
				break;
			case 2:
				name = deserialize.read_string();
				break;
			case 3:
				level = deserialize.read_integer();
				break;
			case 4:
				profession = deserialize.read_integer();
				break;
			case 5:
				combValue = deserialize.read_integer();
				break;
			case 6:
				mapInfoId = deserialize.read_integer();
				break;
			case 7:
				lineIndex = deserialize.read_integer();
				break;
			case 8:
				memberType = deserialize.read_integer();
				break;
			case 9:
				hp = deserialize.read_integer();
				break;
			case 10:
				max_hp = deserialize.read_integer();
				break;
			case 11:
				vip = deserialize.read_integer();
				break;
			case 12:
				time = deserialize.read_integer();
				break;
			case 13:
				apply_time = deserialize.read_integer();
				break;
			case 14:
				visual = deserialize.read_obj<characterVisual>();
				break;
			case 15:
				curNum = deserialize.read_integer();
				break;
			case 16:
				guildId = deserialize.read_integer();
				break;
			case 17:
				guildName = deserialize.read_string();
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
			serialize.write_integer(id, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(teamid, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_string(name, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(level, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(profession, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(combValue, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(mapInfoId, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(lineIndex, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(memberType, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(hp, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(max_hp, 10);
		}
		if (has_field.has_field(11))
		{
			serialize.write_integer(vip, 11);
		}
		if (has_field.has_field(12))
		{
			serialize.write_integer(time, 12);
		}
		if (has_field.has_field(13))
		{
			serialize.write_integer(apply_time, 13);
		}
		if (has_field.has_field(14))
		{
			serialize.write_obj(visual, 14);
		}
		if (has_field.has_field(15))
		{
			serialize.write_integer(curNum, 15);
		}
		if (has_field.has_field(16))
		{
			serialize.write_integer(guildId, 16);
		}
		if (has_field.has_field(17))
		{
			serialize.write_string(guildName, 17);
		}
		return serialize.close();
	}
}
