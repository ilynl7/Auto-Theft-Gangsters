using Sproto;

namespace SprotoType;

public class consign_sale_item
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 5;

		private long _indexId;

		private long _itemCount;

		private long _price;

		private long _timeType;

		private long _itemType;

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

		public long price
		{
			get
			{
				return _price;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_price = value;
			}
		}

		public bool HasPrice => has_field.has_field(2);

		public long timeType
		{
			get
			{
				return _timeType;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_timeType = value;
			}
		}

		public bool HasTimeType => has_field.has_field(3);

		public long itemType
		{
			get
			{
				return _itemType;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_itemType = value;
			}
		}

		public bool HasItemType => has_field.has_field(4);

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
					indexId = deserialize.read_integer();
					break;
				case 1:
					itemCount = deserialize.read_integer();
					break;
				case 2:
					price = deserialize.read_integer();
					break;
				case 3:
					timeType = deserialize.read_integer();
					break;
				case 4:
					itemType = deserialize.read_integer();
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
				serialize.write_integer(itemCount, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(price, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(timeType, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(itemType, 4);
			}
			return serialize.close();
		}
	}
}
