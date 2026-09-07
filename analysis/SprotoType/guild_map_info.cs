using Sproto;

namespace SprotoType;

public class guild_map_info : SprotoTypeBase
{
	private static int max_field_count = 6;

	private string _id;

	private long _guildId;

	private string _guildName;

	private long _guildIcon;

	private long _requireState;

	private long _state;

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

	public long guildId
	{
		get
		{
			return _guildId;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_guildId = value;
		}
	}

	public bool HasGuildId => has_field.has_field(1);

	public string guildName
	{
		get
		{
			return _guildName;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_guildName = value;
		}
	}

	public bool HasGuildName => has_field.has_field(2);

	public long guildIcon
	{
		get
		{
			return _guildIcon;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_guildIcon = value;
		}
	}

	public bool HasGuildIcon => has_field.has_field(3);

	public long requireState
	{
		get
		{
			return _requireState;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_requireState = value;
		}
	}

	public bool HasRequireState => has_field.has_field(4);

	public long state
	{
		get
		{
			return _state;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_state = value;
		}
	}

	public bool HasState => has_field.has_field(5);

	public guild_map_info()
		: base(max_field_count)
	{
	}

	public guild_map_info(byte[] buffer)
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
				guildId = deserialize.read_integer();
				break;
			case 2:
				guildName = deserialize.read_string();
				break;
			case 3:
				guildIcon = deserialize.read_integer();
				break;
			case 4:
				requireState = deserialize.read_integer();
				break;
			case 5:
				state = deserialize.read_integer();
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
			serialize.write_integer(guildId, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_string(guildName, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(guildIcon, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(requireState, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(state, 5);
		}
		return serialize.close();
	}
}
