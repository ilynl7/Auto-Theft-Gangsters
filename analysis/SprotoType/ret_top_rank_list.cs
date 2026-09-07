using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_top_rank_list
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private List<sort_item> _sort_items;

		private long _sortType;

		public List<sort_item> sort_items
		{
			get
			{
				return _sort_items;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_sort_items = value;
			}
		}

		public bool HasSort_items => has_field.has_field(0);

		public long sortType
		{
			get
			{
				return _sortType;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_sortType = value;
			}
		}

		public bool HasSortType => has_field.has_field(1);

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
					sort_items = deserialize.read_obj_list<sort_item>();
					break;
				case 1:
					sortType = deserialize.read_integer();
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
				serialize.write_obj(sort_items, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(sortType, 1);
			}
			return serialize.close();
		}
	}
}
