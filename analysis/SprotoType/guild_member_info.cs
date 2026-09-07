using Sproto;

namespace SprotoType;

public class guild_member_info : SprotoTypeBase
{
	private static int max_field_count = 13;

	private long _guildId;

	private long _characterId;

	private string _name;

	private long _vip;

	private long _profession;

	private long _level;

	private long _contribute;

	private long _lastLogout;

	private long _state;

	private long _job;

	private long _combValue;

	private long _all_contribute;

	private long _battle;

	public long guildId
	{
		get
		{
			return _guildId;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_guildId = value;
		}
	}

	public bool HasGuildId => has_field.has_field(0);

	public long characterId
	{
		get
		{
			return _characterId;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_characterId = value;
		}
	}

	public bool HasCharacterId => has_field.has_field(1);

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

	public long vip
	{
		get
		{
			return _vip;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_vip = value;
		}
	}

	public bool HasVip => has_field.has_field(3);

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

	public long level
	{
		get
		{
			return _level;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_level = value;
		}
	}

	public bool HasLevel => has_field.has_field(5);

	public long contribute
	{
		get
		{
			return _contribute;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_contribute = value;
		}
	}

	public bool HasContribute => has_field.has_field(6);

	public long lastLogout
	{
		get
		{
			return _lastLogout;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_lastLogout = value;
		}
	}

	public bool HasLastLogout => has_field.has_field(7);

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

	public long job
	{
		get
		{
			return _job;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_job = value;
		}
	}

	public bool HasJob => has_field.has_field(9);

	public long combValue
	{
		get
		{
			return _combValue;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_combValue = value;
		}
	}

	public bool HasCombValue => has_field.has_field(10);

	public long all_contribute
	{
		get
		{
			return _all_contribute;
		}
		set
		{
			has_field.set_field(11, is_has: true);
			_all_contribute = value;
		}
	}

	public bool HasAll_contribute => has_field.has_field(11);

	public long battle
	{
		get
		{
			return _battle;
		}
		set
		{
			has_field.set_field(12, is_has: true);
			_battle = value;
		}
	}

	public bool HasBattle => has_field.has_field(12);

	public guild_member_info()
		: base(max_field_count)
	{
	}

	public guild_member_info(byte[] buffer)
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
				guildId = deserialize.read_integer();
				break;
			case 1:
				characterId = deserialize.read_integer();
				break;
			case 2:
				name = deserialize.read_string();
				break;
			case 3:
				vip = deserialize.read_integer();
				break;
			case 4:
				profession = deserialize.read_integer();
				break;
			case 5:
				level = deserialize.read_integer();
				break;
			case 6:
				contribute = deserialize.read_integer();
				break;
			case 7:
				lastLogout = deserialize.read_integer();
				break;
			case 8:
				state = deserialize.read_integer();
				break;
			case 9:
				job = deserialize.read_integer();
				break;
			case 10:
				combValue = deserialize.read_integer();
				break;
			case 11:
				all_contribute = deserialize.read_integer();
				break;
			case 12:
				battle = deserialize.read_integer();
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
			serialize.write_integer(guildId, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(characterId, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_string(name, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(vip, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(profession, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(level, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(contribute, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(lastLogout, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(state, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(job, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(combValue, 10);
		}
		if (has_field.has_field(11))
		{
			serialize.write_integer(all_contribute, 11);
		}
		if (has_field.has_field(12))
		{
			serialize.write_integer(battle, 12);
		}
		return serialize.close();
	}
}
