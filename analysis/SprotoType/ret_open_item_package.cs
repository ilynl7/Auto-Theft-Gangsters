using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_open_item_package
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private List<item> _items;

		public List<item> items
		{
			get
			{
				return _items;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_items = value;
			}
		}

		public bool HasItems => has_field.has_field(0);

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
				if (num == 0)
				{
					items = deserialize.read_obj_list<item>();
				}
				else
				{
					deserialize.read_unknow_data();
				}
			}
		}

		public override int encode(SprotoStream stream)
		{
			serialize.open(stream);
			if (has_field.has_field(0))
			{
				serialize.write_obj(items, 0);
			}
			return serialize.close();
		}
	}
}
