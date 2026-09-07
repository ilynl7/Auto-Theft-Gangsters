using Sproto;

namespace SprotoType;

public class acceptdamge : SprotoTypeBase
{
	private static int max_field_count = 9;

	private long _id;

	private long _damage;

	private string _skillId;

	private string _effinfoId;

	private bool _cri;

	private long _parm;

	private long _parm2;

	private long _parm3;

	private long _parm4;

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

	public long damage
	{
		get
		{
			return _damage;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_damage = value;
		}
	}

	public bool HasDamage => has_field.has_field(1);

	public string skillId
	{
		get
		{
			return _skillId;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_skillId = value;
		}
	}

	public bool HasSkillId => has_field.has_field(2);

	public string effinfoId
	{
		get
		{
			return _effinfoId;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_effinfoId = value;
		}
	}

	public bool HasEffinfoId => has_field.has_field(3);

	public bool cri
	{
		get
		{
			return _cri;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_cri = value;
		}
	}

	public bool HasCri => has_field.has_field(4);

	public long parm
	{
		get
		{
			return _parm;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_parm = value;
		}
	}

	public bool HasParm => has_field.has_field(5);

	public long parm2
	{
		get
		{
			return _parm2;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_parm2 = value;
		}
	}

	public bool HasParm2 => has_field.has_field(6);

	public long parm3
	{
		get
		{
			return _parm3;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_parm3 = value;
		}
	}

	public bool HasParm3 => has_field.has_field(7);

	public long parm4
	{
		get
		{
			return _parm4;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_parm4 = value;
		}
	}

	public bool HasParm4 => has_field.has_field(8);

	public acceptdamge()
		: base(max_field_count)
	{
	}

	public acceptdamge(byte[] buffer)
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
				damage = deserialize.read_integer();
				break;
			case 2:
				skillId = deserialize.read_string();
				break;
			case 3:
				effinfoId = deserialize.read_string();
				break;
			case 4:
				cri = deserialize.read_boolean();
				break;
			case 5:
				parm = deserialize.read_integer();
				break;
			case 6:
				parm2 = deserialize.read_integer();
				break;
			case 7:
				parm3 = deserialize.read_integer();
				break;
			case 8:
				parm4 = deserialize.read_integer();
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
			serialize.write_integer(damage, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_string(skillId, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_string(effinfoId, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_boolean(cri, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(parm, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(parm2, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(parm3, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(parm4, 8);
		}
		return serialize.close();
	}
}
