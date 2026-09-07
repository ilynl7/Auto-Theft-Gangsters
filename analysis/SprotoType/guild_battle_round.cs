using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class guild_battle_round : SprotoTypeBase
{
	private static int max_field_count = 2;

	private Dictionary<long, guild_battle_team> _battle_team;

	private long _state;

	public Dictionary<long, guild_battle_team> battle_team
	{
		get
		{
			return _battle_team;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_battle_team = value;
		}
	}

	public bool HasBattle_team => has_field.has_field(0);

	public long state
	{
		get
		{
			return _state;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_state = value;
		}
	}

	public bool HasState => has_field.has_field(1);

	public guild_battle_round()
		: base(max_field_count)
	{
	}

	public guild_battle_round(byte[] buffer)
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
				battle_team = deserialize.read_map((guild_battle_team v) => v.index);
				break;
			case 1:
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
			serialize.write_obj(battle_team, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(state, 1);
		}
		return serialize.close();
	}
}
