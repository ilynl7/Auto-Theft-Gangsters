using Sproto;

namespace SprotoType;

public class ret_consign_buy_item
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _id;

		private long _success;

		private string _itemId;

		public long id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(0);

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
					id = deserialize.read_integer();
					break;
				case 1:
					success = deserialize.read_integer();
					break;
				case 2:
					itemId = deserialize.read_string();
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
				serialize.write_integer(id, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(success, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(itemId, 2);
			}
			return serialize.close();
		}
	}
}
