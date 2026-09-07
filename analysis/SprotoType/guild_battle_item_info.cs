using Sproto;

namespace SprotoType;

public class guild_battle_item_info : SprotoTypeBase
{
	private static int max_field_count = 7;

	private long _id;

	private string _name;

	private long _guildId;

	private string _guildName;

	private long _killNum;

	private long _continueKill;

	private long _score;

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

	public string name
	{
		get
		{
			return _name;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_name = value;
		}
	}

	public bool HasName => has_field.has_field(1);

	public long guildId
	{
		get
		{
			return _guildId;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_guildId = value;
		}
	}

	public bool HasGuildId => has_field.has_field(2);

	public string guildName
	{
		get
		{
			return _guildName;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_guildName = value;
		}
	}

	public bool HasGuildName => has_field.has_field(3);

	public long killNum
	{
		get
		{
			return _killNum;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_killNum = value;
		}
	}

	public bool HasKillNum => has_field.has_field(4);

	public long continueKill
	{
		get
		{
			return _continueKill;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_continueKill = value;
		}
	}

	public bool HasContinueKill => has_field.has_field(5);

	public long score
	{
		get
		{
			return _score;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_score = value;
		}
	}

	public bool HasScore => has_field.has_field(6);

	public guild_battle_item_info()
		: base(max_field_count)
	{
	}

	public guild_battle_item_info(byte[] buffer)
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
				name = deserialize.read_string();
				break;
			case 2:
				guildId = deserialize.read_integer();
				break;
			case 3:
				guildName = deserialize.read_string();
				break;
			case 4:
				killNum = deserialize.read_integer();
				break;
			case 5:
				continueKill = deserialize.read_integer();
				break;
			case 6:
				score = deserialize.read_integer();
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
			serialize.write_string(name, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(guildId, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_string(guildName, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(killNum, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(continueKill, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(score, 6);
		}
		return serialize.close();
	}
}
