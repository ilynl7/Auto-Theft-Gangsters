using Sproto;

namespace SprotoType;

public class item : SprotoTypeBase
{
	private static int max_field_count = 6;

	private string _itemId;

	private long _itemCount;

	private long _quality;

	private string _id;

	private long _count2;

	public string itemId
	{
		get
		{
			return _itemId;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_itemId = value;
		}
	}

	public bool HasItemId => has_field.has_field(0);

	public long itemCount
	{
		get
		{
			return _itemCount;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_itemCount = value;
		}
	}

	public bool HasItemCount => has_field.has_field(1);

	public long quality
	{
		get
		{
			return _quality;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_quality = value;
		}
	}

	public bool HasQuality => has_field.has_field(2);

	public string id
	{
		get
		{
			return _id;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_id = value;
		}
	}

	public bool HasId => has_field.has_field(3);

	public long count2
	{
		get
		{
			return _count2;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_count2 = value;
		}
	}

	public bool HasCount2 => has_field.has_field(4);

	public item()
		: base(max_field_count)
	{
	}

	public item(byte[] buffer)
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
				itemId = deserialize.read_string();
				break;
			case 1:
				itemCount = deserialize.read_integer();
				break;
			case 3:
				quality = deserialize.read_integer();
				break;
			case 4:
				id = deserialize.read_string();
				break;
			case 5:
				count2 = deserialize.read_integer();
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
			serialize.write_string(itemId, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(itemCount, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(quality, 3);
		}
		if (has_field.has_field(3))
		{
			serialize.write_string(id, 4);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(count2, 5);
		}
		return serialize.close();
	}
}
