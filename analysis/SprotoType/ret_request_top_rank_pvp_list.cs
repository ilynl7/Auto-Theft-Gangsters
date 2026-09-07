using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_top_rank_pvp_list
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _curPage;

		private long _maxPage;

		private List<sort_item> _sort_items;

		public long curPage
		{
			get
			{
				return _curPage;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_curPage = value;
			}
		}

		public bool HasCurPage => has_field.has_field(0);

		public long maxPage
		{
			get
			{
				return _maxPage;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_maxPage = value;
			}
		}

		public bool HasMaxPage => has_field.has_field(1);

		public List<sort_item> sort_items
		{
			get
			{
				return _sort_items;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_sort_items = value;
			}
		}

		public bool HasSort_items => has_field.has_field(2);

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
					curPage = deserialize.read_integer();
					break;
				case 1:
					maxPage = deserialize.read_integer();
					break;
				case 2:
					sort_items = deserialize.read_obj_list<sort_item>();
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
				serialize.write_integer(curPage, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(maxPage, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_obj(sort_items, 2);
			}
			return serialize.close();
		}
	}
}
