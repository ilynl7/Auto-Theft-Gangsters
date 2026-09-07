using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class guild_battle_score_info : SprotoTypeBase
{
	private static int max_field_count = 10;

	private List<guild_battle_item_info> _item_info;

	private long _score1;

	private long _score2;

	private long _selfKillNum;

	private long _selfScore;

	private string _guildName1;

	private string _guildName2;

	private long _win;

	private long _guildIcon1;

	private long _guildIcon2;

	public List<guild_battle_item_info> item_info
	{
		get
		{
			return _item_info;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_item_info = value;
		}
	}

	public bool HasItem_info => has_field.has_field(0);

	public long score1
	{
		get
		{
			return _score1;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_score1 = value;
		}
	}

	public bool HasScore1 => has_field.has_field(1);

	public long score2
	{
		get
		{
			return _score2;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_score2 = value;
		}
	}

	public bool HasScore2 => has_field.has_field(2);

	public long selfKillNum
	{
		get
		{
			return _selfKillNum;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_selfKillNum = value;
		}
	}

	public bool HasSelfKillNum => has_field.has_field(3);

	public long selfScore
	{
		get
		{
			return _selfScore;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_selfScore = value;
		}
	}

	public bool HasSelfScore => has_field.has_field(4);

	public string guildName1
	{
		get
		{
			return _guildName1;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_guildName1 = value;
		}
	}

	public bool HasGuildName1 => has_field.has_field(5);

	public string guildName2
	{
		get
		{
			return _guildName2;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_guildName2 = value;
		}
	}

	public bool HasGuildName2 => has_field.has_field(6);

	public long win
	{
		get
		{
			return _win;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_win = value;
		}
	}

	public bool HasWin => has_field.has_field(7);

	public long guildIcon1
	{
		get
		{
			return _guildIcon1;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_guildIcon1 = value;
		}
	}

	public bool HasGuildIcon1 => has_field.has_field(8);

	public long guildIcon2
	{
		get
		{
			return _guildIcon2;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_guildIcon2 = value;
		}
	}

	public bool HasGuildIcon2 => has_field.has_field(9);

	public guild_battle_score_info()
		: base(max_field_count)
	{
	}

	public guild_battle_score_info(byte[] buffer)
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
				item_info = deserialize.read_obj_list<guild_battle_item_info>();
				break;
			case 1:
				score1 = deserialize.read_integer();
				break;
			case 2:
				score2 = deserialize.read_integer();
				break;
			case 3:
				selfKillNum = deserialize.read_integer();
				break;
			case 4:
				selfScore = deserialize.read_integer();
				break;
			case 5:
				guildName1 = deserialize.read_string();
				break;
			case 6:
				guildName2 = deserialize.read_string();
				break;
			case 7:
				win = deserialize.read_integer();
				break;
			case 8:
				guildIcon1 = deserialize.read_integer();
				break;
			case 9:
				guildIcon2 = deserialize.read_integer();
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
			serialize.write_obj(item_info, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(score1, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(score2, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(selfKillNum, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(selfScore, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_string(guildName1, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_string(guildName2, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(win, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(guildIcon1, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(guildIcon2, 9);
		}
		return serialize.close();
	}
}
