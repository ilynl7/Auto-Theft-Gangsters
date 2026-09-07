using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class gameitem : SprotoTypeBase
{
	private static int max_field_count = 11;

	private long _indexId;

	private string _itemId;

	private bool _bindflag;

	private long _level;

	private long _flags;

	private long _stack;

	private long _quality;

	private List<long> _parm;

	private long _appraise;

	private Dictionary<long, random_attri> _random_attri;

	private Dictionary<long, inlay> _inlay;

	public long indexId
	{
		get
		{
			return _indexId;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_indexId = value;
		}
	}

	public bool HasIndexId => has_field.has_field(0);

	public string itemId
	{
		get
		{
			return _itemId;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_itemId = value;
		}
	}

	public bool HasItemId => has_field.has_field(1);

	public bool bindflag
	{
		get
		{
			return _bindflag;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_bindflag = value;
		}
	}

	public bool HasBindflag => has_field.has_field(2);

	public long level
	{
		get
		{
			return _level;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_level = value;
		}
	}

	public bool HasLevel => has_field.has_field(3);

	public long flags
	{
		get
		{
			return _flags;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_flags = value;
		}
	}

	public bool HasFlags => has_field.has_field(4);

	public long stack
	{
		get
		{
			return _stack;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_stack = value;
		}
	}

	public bool HasStack => has_field.has_field(5);

	public long quality
	{
		get
		{
			return _quality;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_quality = value;
		}
	}

	public bool HasQuality => has_field.has_field(6);

	public List<long> parm
	{
		get
		{
			return _parm;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_parm = value;
		}
	}

	public bool HasParm => has_field.has_field(7);

	public long appraise
	{
		get
		{
			return _appraise;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_appraise = value;
		}
	}

	public bool HasAppraise => has_field.has_field(8);

	public Dictionary<long, random_attri> random_attri
	{
		get
		{
			return _random_attri;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_random_attri = value;
		}
	}

	public bool HasRandom_attri => has_field.has_field(9);

	public Dictionary<long, inlay> inlay
	{
		get
		{
			return _inlay;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_inlay = value;
		}
	}

	public bool HasInlay => has_field.has_field(10);

	public gameitem()
		: base(max_field_count)
	{
	}

	public gameitem(byte[] buffer)
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
				indexId = deserialize.read_integer();
				break;
			case 1:
				itemId = deserialize.read_string();
				break;
			case 2:
				bindflag = deserialize.read_boolean();
				break;
			case 3:
				level = deserialize.read_integer();
				break;
			case 4:
				flags = deserialize.read_integer();
				break;
			case 5:
				stack = deserialize.read_integer();
				break;
			case 6:
				quality = deserialize.read_integer();
				break;
			case 7:
				parm = deserialize.read_integer_list();
				break;
			case 8:
				appraise = deserialize.read_integer();
				break;
			case 9:
				random_attri = deserialize.read_map((random_attri v) => v.index);
				break;
			case 10:
				inlay = deserialize.read_map((inlay v) => v.index);
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
			serialize.write_integer(indexId, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_string(itemId, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_boolean(bindflag, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(level, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(flags, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(stack, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(quality, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(parm, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(appraise, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_obj(random_attri, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_obj(inlay, 10);
		}
		return serialize.close();
	}
}
