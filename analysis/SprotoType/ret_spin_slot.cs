using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_spin_slot
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private slot_info _slot_info;

		private Dictionary<string, slot_item> _slot_items;

		public slot_info slot_info
		{
			get
			{
				return _slot_info;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_slot_info = value;
			}
		}

		public bool HasSlot_info => has_field.has_field(0);

		public Dictionary<string, slot_item> slot_items
		{
			get
			{
				return _slot_items;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_slot_items = value;
			}
		}

		public bool HasSlot_items => has_field.has_field(1);

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
					slot_info = deserialize.read_obj<slot_info>();
					break;
				case 1:
					slot_items = deserialize.read_map((slot_item v) => v.uuid);
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
				serialize.write_obj(slot_info, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(slot_items, 1);
			}
			return serialize.close();
		}
	}
}
