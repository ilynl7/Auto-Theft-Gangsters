using Sproto;

namespace SprotoType;

public class ret_consign_sale_item
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _indexId;

		private long _success;

		private gameitem _gameitem;

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

		public long success
		{
			get
			{
				return _success;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_success = value;
			}
		}

		public bool HasSuccess => has_field.has_field(1);

		public gameitem gameitem
		{
			get
			{
				return _gameitem;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_gameitem = value;
			}
		}

		public bool HasGameitem => has_field.has_field(2);

		public long itemType
		{
			get
			{
				return _itemType;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_itemType = value;
			}
		}

		public bool HasItemType => has_field.has_field(3);

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
					success = deserialize.read_integer();
					break;
				case 2:
					gameitem = deserialize.read_obj<gameitem>();
					break;
				case 3:
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
				serialize.write_integer(success, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_obj(gameitem, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(itemType, 3);
			}
			return serialize.close();
		}
	}
}
