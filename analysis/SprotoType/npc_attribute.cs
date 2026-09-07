using Sproto;

namespace SprotoType;

public class npc_attribute : SprotoTypeBase
{
	private static int max_field_count = 28;

	private long _id;

	private string _npcdataid;

	private long _hp;

	private long _max_hp;

	private long _atk;

	private long _def;

	private long _hit;

	private long _eva;

	private long _cri;

	private long _exd;

	private long _exr;

	private long _res;

	private long _crd;

	private long _crr;

	private long _defa;

	private long _x;

	private long _z;

	private long _o;

	private long _level;

	private long _anti_stun;

	private long _anti_knock_down;

	private string _player_name;

	private long _guildId;

	private long _teamid;

	private long _dgea;

	private long _resa;

	private long _hita;

	private long _cria;

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

	public string npcdataid
	{
		get
		{
			return _npcdataid;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_npcdataid = value;
		}
	}

	public bool HasNpcdataid => has_field.has_field(1);

	public long hp
	{
		get
		{
			return _hp;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_hp = value;
		}
	}

	public bool HasHp => has_field.has_field(2);

	public long max_hp
	{
		get
		{
			return _max_hp;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_max_hp = value;
		}
	}

	public bool HasMax_hp => has_field.has_field(3);

	public long atk
	{
		get
		{
			return _atk;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_atk = value;
		}
	}

	public bool HasAtk => has_field.has_field(4);

	public long def
	{
		get
		{
			return _def;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_def = value;
		}
	}

	public bool HasDef => has_field.has_field(5);

	public long hit
	{
		get
		{
			return _hit;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_hit = value;
		}
	}

	public bool HasHit => has_field.has_field(6);

	public long eva
	{
		get
		{
			return _eva;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_eva = value;
		}
	}

	public bool HasEva => has_field.has_field(7);

	public long cri
	{
		get
		{
			return _cri;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_cri = value;
		}
	}

	public bool HasCri => has_field.has_field(8);

	public long exd
	{
		get
		{
			return _exd;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_exd = value;
		}
	}

	public bool HasExd => has_field.has_field(9);

	public long exr
	{
		get
		{
			return _exr;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_exr = value;
		}
	}

	public bool HasExr => has_field.has_field(10);

	public long res
	{
		get
		{
			return _res;
		}
		set
		{
			has_field.set_field(11, is_has: true);
			_res = value;
		}
	}

	public bool HasRes => has_field.has_field(11);

	public long crd
	{
		get
		{
			return _crd;
		}
		set
		{
			has_field.set_field(12, is_has: true);
			_crd = value;
		}
	}

	public bool HasCrd => has_field.has_field(12);

	public long crr
	{
		get
		{
			return _crr;
		}
		set
		{
			has_field.set_field(13, is_has: true);
			_crr = value;
		}
	}

	public bool HasCrr => has_field.has_field(13);

	public long defa
	{
		get
		{
			return _defa;
		}
		set
		{
			has_field.set_field(14, is_has: true);
			_defa = value;
		}
	}

	public bool HasDefa => has_field.has_field(14);

	public long x
	{
		get
		{
			return _x;
		}
		set
		{
			has_field.set_field(15, is_has: true);
			_x = value;
		}
	}

	public bool HasX => has_field.has_field(15);

	public long z
	{
		get
		{
			return _z;
		}
		set
		{
			has_field.set_field(16, is_has: true);
			_z = value;
		}
	}

	public bool HasZ => has_field.has_field(16);

	public long o
	{
		get
		{
			return _o;
		}
		set
		{
			has_field.set_field(17, is_has: true);
			_o = value;
		}
	}

	public bool HasO => has_field.has_field(17);

	public long level
	{
		get
		{
			return _level;
		}
		set
		{
			has_field.set_field(18, is_has: true);
			_level = value;
		}
	}

	public bool HasLevel => has_field.has_field(18);

	public long anti_stun
	{
		get
		{
			return _anti_stun;
		}
		set
		{
			has_field.set_field(19, is_has: true);
			_anti_stun = value;
		}
	}

	public bool HasAnti_stun => has_field.has_field(19);

	public long anti_knock_down
	{
		get
		{
			return _anti_knock_down;
		}
		set
		{
			has_field.set_field(20, is_has: true);
			_anti_knock_down = value;
		}
	}

	public bool HasAnti_knock_down => has_field.has_field(20);

	public string player_name
	{
		get
		{
			return _player_name;
		}
		set
		{
			has_field.set_field(21, is_has: true);
			_player_name = value;
		}
	}

	public bool HasPlayer_name => has_field.has_field(21);

	public long guildId
	{
		get
		{
			return _guildId;
		}
		set
		{
			has_field.set_field(22, is_has: true);
			_guildId = value;
		}
	}

	public bool HasGuildId => has_field.has_field(22);

	public long teamid
	{
		get
		{
			return _teamid;
		}
		set
		{
			has_field.set_field(23, is_has: true);
			_teamid = value;
		}
	}

	public bool HasTeamid => has_field.has_field(23);

	public long dgea
	{
		get
		{
			return _dgea;
		}
		set
		{
			has_field.set_field(24, is_has: true);
			_dgea = value;
		}
	}

	public bool HasDgea => has_field.has_field(24);

	public long resa
	{
		get
		{
			return _resa;
		}
		set
		{
			has_field.set_field(25, is_has: true);
			_resa = value;
		}
	}

	public bool HasResa => has_field.has_field(25);

	public long hita
	{
		get
		{
			return _hita;
		}
		set
		{
			has_field.set_field(26, is_has: true);
			_hita = value;
		}
	}

	public bool HasHita => has_field.has_field(26);

	public long cria
	{
		get
		{
			return _cria;
		}
		set
		{
			has_field.set_field(27, is_has: true);
			_cria = value;
		}
	}

	public bool HasCria => has_field.has_field(27);

	public npc_attribute()
		: base(max_field_count)
	{
	}

	public npc_attribute(byte[] buffer)
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
				npcdataid = deserialize.read_string();
				break;
			case 2:
				hp = deserialize.read_integer();
				break;
			case 3:
				max_hp = deserialize.read_integer();
				break;
			case 4:
				atk = deserialize.read_integer();
				break;
			case 5:
				def = deserialize.read_integer();
				break;
			case 6:
				hit = deserialize.read_integer();
				break;
			case 7:
				eva = deserialize.read_integer();
				break;
			case 8:
				cri = deserialize.read_integer();
				break;
			case 9:
				exd = deserialize.read_integer();
				break;
			case 10:
				exr = deserialize.read_integer();
				break;
			case 11:
				res = deserialize.read_integer();
				break;
			case 12:
				crd = deserialize.read_integer();
				break;
			case 13:
				crr = deserialize.read_integer();
				break;
			case 14:
				defa = deserialize.read_integer();
				break;
			case 15:
				x = deserialize.read_integer();
				break;
			case 16:
				z = deserialize.read_integer();
				break;
			case 17:
				o = deserialize.read_integer();
				break;
			case 18:
				level = deserialize.read_integer();
				break;
			case 19:
				anti_stun = deserialize.read_integer();
				break;
			case 20:
				anti_knock_down = deserialize.read_integer();
				break;
			case 21:
				player_name = deserialize.read_string();
				break;
			case 22:
				guildId = deserialize.read_integer();
				break;
			case 23:
				teamid = deserialize.read_integer();
				break;
			case 24:
				dgea = deserialize.read_integer();
				break;
			case 25:
				resa = deserialize.read_integer();
				break;
			case 26:
				hita = deserialize.read_integer();
				break;
			case 27:
				cria = deserialize.read_integer();
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
			serialize.write_string(npcdataid, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(hp, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(max_hp, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(atk, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(def, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(hit, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(eva, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(cri, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(exd, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(exr, 10);
		}
		if (has_field.has_field(11))
		{
			serialize.write_integer(res, 11);
		}
		if (has_field.has_field(12))
		{
			serialize.write_integer(crd, 12);
		}
		if (has_field.has_field(13))
		{
			serialize.write_integer(crr, 13);
		}
		if (has_field.has_field(14))
		{
			serialize.write_integer(defa, 14);
		}
		if (has_field.has_field(15))
		{
			serialize.write_integer(x, 15);
		}
		if (has_field.has_field(16))
		{
			serialize.write_integer(z, 16);
		}
		if (has_field.has_field(17))
		{
			serialize.write_integer(o, 17);
		}
		if (has_field.has_field(18))
		{
			serialize.write_integer(level, 18);
		}
		if (has_field.has_field(19))
		{
			serialize.write_integer(anti_stun, 19);
		}
		if (has_field.has_field(20))
		{
			serialize.write_integer(anti_knock_down, 20);
		}
		if (has_field.has_field(21))
		{
			serialize.write_string(player_name, 21);
		}
		if (has_field.has_field(22))
		{
			serialize.write_integer(guildId, 22);
		}
		if (has_field.has_field(23))
		{
			serialize.write_integer(teamid, 23);
		}
		if (has_field.has_field(24))
		{
			serialize.write_integer(dgea, 24);
		}
		if (has_field.has_field(25))
		{
			serialize.write_integer(resa, 25);
		}
		if (has_field.has_field(26))
		{
			serialize.write_integer(hita, 26);
		}
		if (has_field.has_field(27))
		{
			serialize.write_integer(cria, 27);
		}
		return serialize.close();
	}
}
