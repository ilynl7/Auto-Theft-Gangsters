using Sproto;

namespace SprotoType;

public class ask_shop_list
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 6;

		private long _type;

		private long _curPage;

		private string _itemId;

		private long _subType;

		private long _class1;

		private long _special;

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

		public string itemId
		{
			get
			{
				return _itemId;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_itemId = value;
			}
		}

		public bool HasItemId => has_field.has_field(2);

		public long subType
		{
			get
			{
				return _subType;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_subType = value;
			}
		}

		public bool HasSubType => has_field.has_field(3);

		public long class1
		{
			get
			{
				return _class1;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_class1 = value;
			}
		}

		public bool HasClass1 => has_field.has_field(4);

		public long special
		{
			get
			{
				return _special;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_special = value;
			}
		}

		public bool HasSpecial => has_field.has_field(5);

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
					itemId = deserialize.read_string();
					break;
				case 3:
					subType = deserialize.read_integer();
					break;
				case 4:
					class1 = deserialize.read_integer();
					break;
				case 5:
					special = deserialize.read_integer();
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
				serialize.write_string(itemId, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(subType, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(class1, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(special, 5);
			}
			return serialize.close();
		}
	}
}
