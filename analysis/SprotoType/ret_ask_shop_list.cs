using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_ask_shop_list
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 5;

		private long _type;

		private long _curPage;

		private long _maxPage;

		private List<shop_item> _shop_list;

		private long _subType;

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

		public long curPage
		{
			get
			{
				return _curPage;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_curPage = value;
			}
		}

		public bool HasCurPage => has_field.has_field(1);

		public long maxPage
		{
			get
			{
				return _maxPage;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_maxPage = value;
			}
		}

		public bool HasMaxPage => has_field.has_field(2);

		public List<shop_item> shop_list
		{
			get
			{
				return _shop_list;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_shop_list = value;
			}
		}

		public bool HasShop_list => has_field.has_field(3);

		public long subType
		{
			get
			{
				return _subType;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_subType = value;
			}
		}

		public bool HasSubType => has_field.has_field(4);

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
					curPage = deserialize.read_integer();
					break;
				case 2:
					maxPage = deserialize.read_integer();
					break;
				case 3:
					shop_list = deserialize.read_obj_list<shop_item>();
					break;
				case 4:
					subType = deserialize.read_integer();
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
				serialize.write_integer(curPage, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(maxPage, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_obj(shop_list, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(subType, 4);
			}
			return serialize.close();
		}
	}
}
