using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class mount : SprotoTypeBase
{
	private static int max_field_count = 4;

	private string _ID;

	private long _state;

	private Dictionary<string, color> _colors;

	private string _select;

	public string ID
	{
		get
		{
			return _ID;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_ID = value;
		}
	}

	public bool HasID => has_field.has_field(0);

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

	public Dictionary<string, color> colors
	{
		get
		{
			return _colors;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_colors = value;
		}
	}

	public bool HasColors => has_field.has_field(2);

	public string select
	{
		get
		{
			return _select;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_select = value;
		}
	}

	public bool HasSelect => has_field.has_field(3);

	public mount()
		: base(max_field_count)
	{
	}

	public mount(byte[] buffer)
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
				ID = deserialize.read_string();
				break;
			case 1:
				state = deserialize.read_integer();
				break;
			case 2:
				colors = deserialize.read_map((color v) => v.ID);
				break;
			case 3:
				select = deserialize.read_string();
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
			serialize.write_string(ID, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(state, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_obj(colors, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_string(select, 3);
		}
		return serialize.close();
	}
}
