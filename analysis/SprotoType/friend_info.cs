using Sproto;

namespace SprotoType;

public class friend_info : SprotoTypeBase
{
	private static int max_field_count = 12;

	private long _characterId;

	private long _friendId;

	private string _name;

	private long _level;

	private long _profession;

	private long _combValue;

	private long _state;

	private long _timeInfo;

	private long _friendType;

	private long _guildId;

	private string _guildName;

	private long _friendScore;

	public long characterId
	{
		get
		{
			return _characterId;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_characterId = value;
		}
	}

	public bool HasCharacterId => has_field.has_field(0);

	public long friendId
	{
		get
		{
			return _friendId;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_friendId = value;
		}
	}

	public bool HasFriendId => has_field.has_field(1);

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

	public long timeInfo
	{
		get
		{
			return _timeInfo;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_timeInfo = value;
		}
	}

	public bool HasTimeInfo => has_field.has_field(7);

	public long friendType
	{
		get
		{
			return _friendType;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_friendType = value;
		}
	}

	public bool HasFriendType => has_field.has_field(8);

	public long guildId
	{
		get
		{
			return _guildId;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_guildId = value;
		}
	}

	public bool HasGuildId => has_field.has_field(9);

	public string guildName
	{
		get
		{
			return _guildName;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_guildName = value;
		}
	}

	public bool HasGuildName => has_field.has_field(10);

	public long friendScore
	{
		get
		{
			return _friendScore;
		}
		set
		{
			has_field.set_field(11, is_has: true);
			_friendScore = value;
		}
	}

	public bool HasFriendScore => has_field.has_field(11);

	public friend_info()
		: base(max_field_count)
	{
	}

	public friend_info(byte[] buffer)
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
				characterId = deserialize.read_integer();
				break;
			case 1:
				friendId = deserialize.read_integer();
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
				state = deserialize.read_integer();
				break;
			case 7:
				timeInfo = deserialize.read_integer();
				break;
			case 8:
				friendType = deserialize.read_integer();
				break;
			case 9:
				guildId = deserialize.read_integer();
				break;
			case 10:
				guildName = deserialize.read_string();
				break;
			case 11:
				friendScore = deserialize.read_integer();
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
			serialize.write_integer(characterId, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(friendId, 1);
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
			serialize.write_integer(state, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(timeInfo, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(friendType, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(guildId, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_string(guildName, 10);
		}
		if (has_field.has_field(11))
		{
			serialize.write_integer(friendScore, 11);
		}
		return serialize.close();
	}
}
