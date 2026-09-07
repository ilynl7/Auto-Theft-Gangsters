using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class guild_boss : SprotoTypeBase
{
	private static int max_field_count = 5;

	private string _id;

	private long _state;

	private long _time;

	private long _curNum;

	private List<sort_item> _sort_item;

	public string id
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

	public long time
	{
		get
		{
			return _time;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_time = value;
		}
	}

	public bool HasTime => has_field.has_field(2);

	public long curNum
	{
		get
		{
			return _curNum;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_curNum = value;
		}
	}

	public bool HasCurNum => has_field.has_field(3);

	public List<sort_item> sort_item
	{
		get
		{
			return _sort_item;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_sort_item = value;
		}
	}

	public bool HasSort_item => has_field.has_field(4);

	public guild_boss()
		: base(max_field_count)
	{
	}

	public guild_boss(byte[] buffer)
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
				id = deserialize.read_string();
				break;
			case 1:
				state = deserialize.read_integer();
				break;
			case 2:
				time = deserialize.read_integer();
				break;
			case 3:
				curNum = deserialize.read_integer();
				break;
			case 4:
				sort_item = deserialize.read_obj_list<sort_item>();
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
			serialize.write_string(id, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(state, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(time, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(curNum, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_obj(sort_item, 4);
		}
		return serialize.close();
	}
}
