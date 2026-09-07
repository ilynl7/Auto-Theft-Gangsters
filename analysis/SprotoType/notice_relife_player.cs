using Sproto;

namespace SprotoType;

public class notice_relife_player
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 5;

		private long _type;

		private long _cost;

		private string _itemId;

		private long _characterid;

		private string _name;

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

		public long cost
		{
			get
			{
				return _cost;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_cost = value;
			}
		}

		public bool HasCost => has_field.has_field(1);

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

		public long characterid
		{
			get
			{
				return _characterid;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_characterid = value;
			}
		}

		public bool HasCharacterid => has_field.has_field(3);

		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_name = value;
			}
		}

		public bool HasName => has_field.has_field(4);

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
					cost = deserialize.read_integer();
					break;
				case 2:
					itemId = deserialize.read_string();
					break;
				case 3:
					characterid = deserialize.read_integer();
					break;
				case 4:
					name = deserialize.read_string();
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
				serialize.write_integer(cost, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(itemId, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(characterid, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_string(name, 4);
			}
			return serialize.close();
		}
	}
}
