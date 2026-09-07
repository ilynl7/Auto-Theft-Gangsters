using Sproto;

namespace SprotoType;

public class ret_buy_shop_item
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _state;

		private shop_item _shop_item;

		private long _type;

		private long _count;

		public long state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(0);

		public shop_item shop_item
		{
			get
			{
				return _shop_item;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_shop_item = value;
			}
		}

		public bool HasShop_item => has_field.has_field(1);

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(2);

		public long count
		{
			get
			{
				return _count;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_count = value;
			}
		}

		public bool HasCount => has_field.has_field(3);

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
					state = deserialize.read_integer();
					break;
				case 1:
					shop_item = deserialize.read_obj<shop_item>();
					break;
				case 2:
					type = deserialize.read_integer();
					break;
				case 3:
					count = deserialize.read_integer();
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
				serialize.write_integer(state, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(shop_item, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(type, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(count, 3);
			}
			return serialize.close();
		}
	}
}
