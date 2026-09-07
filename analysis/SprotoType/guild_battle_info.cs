using Sproto;

namespace SprotoType;

public class guild_battle_info : SprotoTypeBase
{
	private static int max_field_count = 8;

	private long _state;

	private guild_battle_round _battle_round_1;

	private guild_battle_round _battle_round_2;

	private guild_battle_round _battle_round_3;

	private long _time;

	private string _ID;

	private long _guildId;

	private string _championName;

	public long state
	{
		get
		{
			return _state;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_state = value;
		}
	}

	public bool HasState => has_field.has_field(0);

	public guild_battle_round battle_round_1
	{
		get
		{
			return _battle_round_1;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_battle_round_1 = value;
		}
	}

	public bool HasBattle_round_1 => has_field.has_field(1);

	public guild_battle_round battle_round_2
	{
		get
		{
			return _battle_round_2;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_battle_round_2 = value;
		}
	}

	public bool HasBattle_round_2 => has_field.has_field(2);

	public guild_battle_round battle_round_3
	{
		get
		{
			return _battle_round_3;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_battle_round_3 = value;
		}
	}

	public bool HasBattle_round_3 => has_field.has_field(3);

	public long time
	{
		get
		{
			return _time;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_time = value;
		}
	}

	public bool HasTime => has_field.has_field(4);

	public string ID
	{
		get
		{
			return _ID;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_ID = value;
		}
	}

	public bool HasID => has_field.has_field(5);

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

	public string championName
	{
		get
		{
			return _championName;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_championName = value;
		}
	}

	public bool HasChampionName => has_field.has_field(7);

	public guild_battle_info()
		: base(max_field_count)
	{
	}

	public guild_battle_info(byte[] buffer)
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
				state = deserialize.read_integer();
				break;
			case 1:
				battle_round_1 = deserialize.read_obj<guild_battle_round>();
				break;
			case 2:
				battle_round_2 = deserialize.read_obj<guild_battle_round>();
				break;
			case 3:
				battle_round_3 = deserialize.read_obj<guild_battle_round>();
				break;
			case 4:
				time = deserialize.read_integer();
				break;
			case 5:
				ID = deserialize.read_string();
				break;
			case 6:
				guildId = deserialize.read_integer();
				break;
			case 7:
				championName = deserialize.read_string();
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
			serialize.write_integer(state, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_obj(battle_round_1, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_obj(battle_round_2, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_obj(battle_round_3, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(time, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_string(ID, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(guildId, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_string(championName, 7);
		}
		return serialize.close();
	}
}
