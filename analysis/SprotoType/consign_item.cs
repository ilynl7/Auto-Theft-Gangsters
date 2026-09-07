using Sproto;

namespace SprotoType;

public class consign_item : SprotoTypeBase
{
	private static int max_field_count = 8;

	private long _id;

	private long _characterId;

	private string _itemId;

	private long _quality;

	private long _stack;

	private long _price;

	private long _time;

	private long _startTime;

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

	public long characterId
	{
		get
		{
			return _characterId;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_characterId = value;
		}
	}

	public bool HasCharacterId => has_field.has_field(1);

	public string itemId
	{
		get
		{
			return _itemId;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_itemId = value;
		}
	}

	public bool HasItemId => has_field.has_field(2);

	public long quality
	{
		get
		{
			return _quality;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_quality = value;
		}
	}

	public bool HasQuality => has_field.has_field(3);

	public long stack
	{
		get
		{
			return _stack;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_stack = value;
		}
	}

	public bool HasStack => has_field.has_field(4);

	public long price
	{
		get
		{
			return _price;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_price = value;
		}
	}

	public bool HasPrice => has_field.has_field(5);

	public long time
	{
		get
		{
			return _time;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_time = value;
		}
	}

	public bool HasTime => has_field.has_field(6);

	public long startTime
	{
		get
		{
			return _startTime;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_startTime = value;
		}
	}

	public bool HasStartTime => has_field.has_field(7);

	public consign_item()
		: base(max_field_count)
	{
	}

	public consign_item(byte[] buffer)
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
				characterId = deserialize.read_integer();
				break;
			case 2:
				itemId = deserialize.read_string();
				break;
			case 3:
				quality = deserialize.read_integer();
				break;
			case 4:
				stack = deserialize.read_integer();
				break;
			case 5:
				price = deserialize.read_integer();
				break;
			case 6:
				time = deserialize.read_integer();
				break;
			case 7:
				startTime = deserialize.read_integer();
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
			serialize.write_integer(characterId, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_string(itemId, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(quality, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(stack, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(price, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(time, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(startTime, 7);
		}
		return serialize.close();
	}
}
