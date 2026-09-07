using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_open_guild_shop
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _type;

		private Dictionary<string, shop_item> _shop_list;

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(0);

		public Dictionary<string, shop_item> shop_list
		{
			get
			{
				return _shop_list;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_shop_list = value;
			}
		}

		public bool HasShop_list => has_field.has_field(1);

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
					type = deserialize.read_integer();
					break;
				case 1:
					shop_list = deserialize.read_map((shop_item v) => v.ID);
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
				serialize.write_integer(type, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(shop_list, 1);
			}
			return serialize.close();
		}
	}
}
