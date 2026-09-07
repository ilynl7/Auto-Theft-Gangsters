using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class slot_item : SprotoTypeBase
{
	private static int max_field_count = 3;

	private string _uuid;

	private string _ID;

	private List<item> _items;

	public string uuid
	{
		get
		{
			return _uuid;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_uuid = value;
		}
	}

	public bool HasUuid => has_field.has_field(0);

	public string ID
	{
		get
		{
			return _ID;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_ID = value;
		}
	}

	public bool HasID => has_field.has_field(1);

	public List<item> items
	{
		get
		{
			return _items;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_items = value;
		}
	}

	public bool HasItems => has_field.has_field(2);

	public slot_item()
		: base(max_field_count)
	{
	}

	public slot_item(byte[] buffer)
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
				uuid = deserialize.read_string();
				break;
			case 1:
				ID = deserialize.read_string();
				break;
			case 2:
				items = deserialize.read_obj_list<item>();
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
			serialize.write_string(uuid, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_string(ID, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_obj(items, 2);
		}
		return serialize.close();
	}
}
