using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class sort_item : SprotoTypeBase
{
	private static int max_field_count = 9;

	private long _id;

	private long _score;

	private string _name;

	private long _profession;

	private string _sortType;

	private long _parm1;

	private Dictionary<string, dict_hash> _parm2;

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

	public long score
	{
		get
		{
			return _score;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_score = value;
		}
	}

	public bool HasScore => has_field.has_field(1);

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

	public long profession
	{
		get
		{
			return _profession;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_profession = value;
		}
	}

	public bool HasProfession => has_field.has_field(3);

	public string sortType
	{
		get
		{
			return _sortType;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_sortType = value;
		}
	}

	public bool HasSortType => has_field.has_field(4);

	public long parm1
	{
		get
		{
			return _parm1;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_parm1 = value;
		}
	}

	public bool HasParm1 => has_field.has_field(5);

	public Dictionary<string, dict_hash> parm2
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

	public sort_item()
		: base(max_field_count)
	{
	}

	public sort_item(byte[] buffer)
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
				score = deserialize.read_integer();
				break;
			case 2:
				name = deserialize.read_string();
				break;
			case 3:
				profession = deserialize.read_integer();
				break;
			case 4:
				sortType = deserialize.read_string();
				break;
			case 5:
				parm1 = deserialize.read_integer();
				break;
			case 6:
				parm2 = deserialize.read_map((dict_hash v) => v.id);
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
			serialize.write_integer(score, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_string(name, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(profession, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_string(sortType, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(parm1, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_obj(parm2, 6);
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
