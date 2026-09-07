using Sproto;

namespace SprotoType;

public class ret_buy_guild_goods
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private string _itemId;

		private long _buyCount;

		private long _cost;

		private long _leftNum;

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

		public long buyCount
		{
			get
			{
				return _buyCount;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_buyCount = value;
			}
		}

		public bool HasBuyCount => has_field.has_field(1);

		public long cost
		{
			get
			{
				return _cost;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_cost = value;
			}
		}

		public bool HasCost => has_field.has_field(2);

		public long leftNum
		{
			get
			{
				return _leftNum;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_leftNum = value;
			}
		}

		public bool HasLeftNum => has_field.has_field(3);

		public request()
			: base(max_field_count)
		{
		}

		public request(byte[] buffer)
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
					buyCount = deserialize.read_integer();
					break;
				case 2:
					cost = deserialize.read_integer();
					break;
				case 3:
					leftNum = deserialize.read_integer();
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
				serialize.write_integer(buyCount, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(cost, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(leftNum, 3);
			}
			return serialize.close();
		}
	}
}
