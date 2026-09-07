using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_slot_sum_reward
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private List<item> _items;

		private long _sumNum;

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

		public long sumNum
		{
			get
			{
				return _sumNum;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_sumNum = value;
			}
		}

		public bool HasSumNum => has_field.has_field(1);

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
					items = deserialize.read_obj_list<item>();
					break;
				case 1:
					sumNum = deserialize.read_integer();
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
				serialize.write_obj(items, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(sumNum, 1);
			}
			return serialize.close();
		}
	}
}
