using Sproto;

namespace SprotoType;

public class attribute : SprotoTypeBase
{
	private static int max_field_count = 25;

	private long _max_hp;

	private long _exp;

	private long _atk;

	private long _def;

	private long _hit;

	private long _eva;

	private long _cri;

	private long _res;

	private long _exd;

	private long _exr;

	private long _crd;

	private long _crr;

	private long _defa;

	private long _mov;

	private long _rec;

	private long _anti_stun;

	private long _anti_knock_down;

	private long _dgea;

	private long _resa;

	private long _hita;

	private long _cria;

	private long _ate;

	private long _satm;

	private long _satc;

	private long _satp;

	public long max_hp
	{
		get
		{
			return _max_hp;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_max_hp = value;
		}
	}

	public bool HasMax_hp => has_field.has_field(0);

	public long exp
	{
		get
		{
			return _exp;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_exp = value;
		}
	}

	public bool HasExp => has_field.has_field(1);

	public long atk
	{
		get
		{
			return _atk;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_atk = value;
		}
	}

	public bool HasAtk => has_field.has_field(2);

	public long def
	{
		get
		{
			return _def;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_def = value;
		}
	}

	public bool HasDef => has_field.has_field(3);

	public long hit
	{
		get
		{
			return _hit;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_hit = value;
		}
	}

	public bool HasHit => has_field.has_field(4);

	public long eva
	{
		get
		{
			return _eva;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_eva = value;
		}
	}

	public bool HasEva => has_field.has_field(5);

	public long cri
	{
		get
		{
			return _cri;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_cri = value;
		}
	}

	public bool HasCri => has_field.has_field(6);

	public long res
	{
		get
		{
			return _res;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_res = value;
		}
	}

	public bool HasRes => has_field.has_field(7);

	public long exd
	{
		get
		{
			return _exd;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_exd = value;
		}
	}

	public bool HasExd => has_field.has_field(8);

	public long exr
	{
		get
		{
			return _exr;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_exr = value;
		}
	}

	public bool HasExr => has_field.has_field(9);

	public long crd
	{
		get
		{
			return _crd;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_crd = value;
		}
	}

	public bool HasCrd => has_field.has_field(10);

	public long crr
	{
		get
		{
			return _crr;
		}
		set
		{
			has_field.set_field(11, is_has: true);
			_crr = value;
		}
	}

	public bool HasCrr => has_field.has_field(11);

	public long defa
	{
		get
		{
			return _defa;
		}
		set
		{
			has_field.set_field(12, is_has: true);
			_defa = value;
		}
	}

	public bool HasDefa => has_field.has_field(12);

	public long mov
	{
		get
		{
			return _mov;
		}
		set
		{
			has_field.set_field(13, is_has: true);
			_mov = value;
		}
	}

	public bool HasMov => has_field.has_field(13);

	public long rec
	{
		get
		{
			return _rec;
		}
		set
		{
			has_field.set_field(14, is_has: true);
			_rec = value;
		}
	}

	public bool HasRec => has_field.has_field(14);

	public long anti_stun
	{
		get
		{
			return _anti_stun;
		}
		set
		{
			has_field.set_field(15, is_has: true);
			_anti_stun = value;
		}
	}

	public bool HasAnti_stun => has_field.has_field(15);

	public long anti_knock_down
	{
		get
		{
			return _anti_knock_down;
		}
		set
		{
			has_field.set_field(16, is_has: true);
			_anti_knock_down = value;
		}
	}

	public bool HasAnti_knock_down => has_field.has_field(16);

	public long dgea
	{
		get
		{
			return _dgea;
		}
		set
		{
			has_field.set_field(17, is_has: true);
			_dgea = value;
		}
	}

	public bool HasDgea => has_field.has_field(17);

	public long resa
	{
		get
		{
			return _resa;
		}
		set
		{
			has_field.set_field(18, is_has: true);
			_resa = value;
		}
	}

	public bool HasResa => has_field.has_field(18);

	public long hita
	{
		get
		{
			return _hita;
		}
		set
		{
			has_field.set_field(19, is_has: true);
			_hita = value;
		}
	}

	public bool HasHita => has_field.has_field(19);

	public long cria
	{
		get
		{
			return _cria;
		}
		set
		{
			has_field.set_field(20, is_has: true);
			_cria = value;
		}
	}

	public bool HasCria => has_field.has_field(20);

	public long ate
	{
		get
		{
			return _ate;
		}
		set
		{
			has_field.set_field(21, is_has: true);
			_ate = value;
		}
	}

	public bool HasAte => has_field.has_field(21);

	public long satm
	{
		get
		{
			return _satm;
		}
		set
		{
			has_field.set_field(22, is_has: true);
			_satm = value;
		}
	}

	public bool HasSatm => has_field.has_field(22);

	public long satc
	{
		get
		{
			return _satc;
		}
		set
		{
			has_field.set_field(23, is_has: true);
			_satc = value;
		}
	}

	public bool HasSatc => has_field.has_field(23);

	public long satp
	{
		get
		{
			return _satp;
		}
		set
		{
			has_field.set_field(24, is_has: true);
			_satp = value;
		}
	}

	public bool HasSatp => has_field.has_field(24);

	public attribute()
		: base(max_field_count)
	{
	}

	public attribute(byte[] buffer)
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
				max_hp = deserialize.read_integer();
				break;
			case 1:
				exp = deserialize.read_integer();
				break;
			case 2:
				atk = deserialize.read_integer();
				break;
			case 3:
				def = deserialize.read_integer();
				break;
			case 4:
				hit = deserialize.read_integer();
				break;
			case 5:
				eva = deserialize.read_integer();
				break;
			case 6:
				cri = deserialize.read_integer();
				break;
			case 7:
				res = deserialize.read_integer();
				break;
			case 8:
				exd = deserialize.read_integer();
				break;
			case 9:
				exr = deserialize.read_integer();
				break;
			case 10:
				crd = deserialize.read_integer();
				break;
			case 11:
				crr = deserialize.read_integer();
				break;
			case 12:
				defa = deserialize.read_integer();
				break;
			case 13:
				mov = deserialize.read_integer();
				break;
			case 14:
				rec = deserialize.read_integer();
				break;
			case 15:
				anti_stun = deserialize.read_integer();
				break;
			case 16:
				anti_knock_down = deserialize.read_integer();
				break;
			case 17:
				dgea = deserialize.read_integer();
				break;
			case 18:
				resa = deserialize.read_integer();
				break;
			case 19:
				hita = deserialize.read_integer();
				break;
			case 20:
				cria = deserialize.read_integer();
				break;
			case 21:
				ate = deserialize.read_integer();
				break;
			case 22:
				satm = deserialize.read_integer();
				break;
			case 23:
				satc = deserialize.read_integer();
				break;
			case 24:
				satp = deserialize.read_integer();
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
			serialize.write_integer(max_hp, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(exp, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(atk, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(def, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(hit, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(eva, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(cri, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(res, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(exd, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(exr, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(crd, 10);
		}
		if (has_field.has_field(11))
		{
			serialize.write_integer(crr, 11);
		}
		if (has_field.has_field(12))
		{
			serialize.write_integer(defa, 12);
		}
		if (has_field.has_field(13))
		{
			serialize.write_integer(mov, 13);
		}
		if (has_field.has_field(14))
		{
			serialize.write_integer(rec, 14);
		}
		if (has_field.has_field(15))
		{
			serialize.write_integer(anti_stun, 15);
		}
		if (has_field.has_field(16))
		{
			serialize.write_integer(anti_knock_down, 16);
		}
		if (has_field.has_field(17))
		{
			serialize.write_integer(dgea, 17);
		}
		if (has_field.has_field(18))
		{
			serialize.write_integer(resa, 18);
		}
		if (has_field.has_field(19))
		{
			serialize.write_integer(hita, 19);
		}
		if (has_field.has_field(20))
		{
			serialize.write_integer(cria, 20);
		}
		if (has_field.has_field(21))
		{
			serialize.write_integer(ate, 21);
		}
		if (has_field.has_field(22))
		{
			serialize.write_integer(satm, 22);
		}
		if (has_field.has_field(23))
		{
			serialize.write_integer(satc, 23);
		}
		if (has_field.has_field(24))
		{
			serialize.write_integer(satp, 24);
		}
		return serialize.close();
	}
}
