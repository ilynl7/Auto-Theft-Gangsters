using Sproto;

namespace SprotoType;

public class shop_item : SprotoTypeBase
{
	private static int max_field_count = 10;

	private string _ID;

	private string _ItemID;

	private long _Quality;

	private long _PriceType;

	private long _Price;

	private long _Limit;

	private long _curNum;

	private long _Discount;

	private long _Class;

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

	public string ItemID
	{
		get
		{
			return _ItemID;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_ItemID = value;
		}
	}

	public bool HasItemID => has_field.has_field(1);

	public long Quality
	{
		get
		{
			return _Quality;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_Quality = value;
		}
	}

	public bool HasQuality => has_field.has_field(2);

	public long PriceType
	{
		get
		{
			return _PriceType;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_PriceType = value;
		}
	}

	public bool HasPriceType => has_field.has_field(3);

	public long Price
	{
		get
		{
			return _Price;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_Price = value;
		}
	}

	public bool HasPrice => has_field.has_field(4);

	public long Limit
	{
		get
		{
			return _Limit;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_Limit = value;
		}
	}

	public bool HasLimit => has_field.has_field(5);

	public long curNum
	{
		get
		{
			return _curNum;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_curNum = value;
		}
	}

	public bool HasCurNum => has_field.has_field(6);

	public long Discount
	{
		get
		{
			return _Discount;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_Discount = value;
		}
	}

	public bool HasDiscount => has_field.has_field(7);

	public long Class
	{
		get
		{
			return _Class;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_Class = value;
		}
	}

	public bool HasClass => has_field.has_field(8);

	public shop_item()
		: base(max_field_count)
	{
	}

	public shop_item(byte[] buffer)
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
				ItemID = deserialize.read_string();
				break;
			case 2:
				Quality = deserialize.read_integer();
				break;
			case 3:
				PriceType = deserialize.read_integer();
				break;
			case 4:
				Price = deserialize.read_integer();
				break;
			case 5:
				Limit = deserialize.read_integer();
				break;
			case 7:
				curNum = deserialize.read_integer();
				break;
			case 8:
				Discount = deserialize.read_integer();
				break;
			case 9:
				Class = deserialize.read_integer();
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
			serialize.write_string(ItemID, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(Quality, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(PriceType, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(Price, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(Limit, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(curNum, 7);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(Discount, 8);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(Class, 9);
		}
		return serialize.close();
	}
}
