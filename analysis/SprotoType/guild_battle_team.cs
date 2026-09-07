using Sproto;

namespace SprotoType;

public class guild_battle_team : SprotoTypeBase
{
	private static int max_field_count = 4;

	private long _guildId;

	private string _guildName;

	private long _index;

	private long _state;

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

	public string guildName
	{
		get
		{
			return _guildName;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_guildName = value;
		}
	}

	public bool HasGuildName => has_field.has_field(1);

	public long index
	{
		get
		{
			return _index;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_index = value;
		}
	}

	public bool HasIndex => has_field.has_field(2);

	public long state
	{
		get
		{
			return _state;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_state = value;
		}
	}

	public bool HasState => has_field.has_field(3);

	public guild_battle_team()
		: base(max_field_count)
	{
	}

	public guild_battle_team(byte[] buffer)
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
				guildName = deserialize.read_string();
				break;
			case 2:
				index = deserialize.read_integer();
				break;
			case 3:
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
			serialize.write_integer(guildId, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_string(guildName, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(index, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(state, 3);
		}
		return serialize.close();
	}
}
