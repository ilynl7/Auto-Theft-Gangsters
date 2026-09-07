using Sproto;

namespace SprotoType;

public class drop_item_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 7;

		private long _serverId;

		private long _pos_x;

		private long _pos_z;

		private long _type;

		private item _item;

		private long _ownServerId;

		public long serverId
		{
			get
			{
				return _serverId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_serverId = value;
			}
		}

		public bool HasServerId => has_field.has_field(0);

		public long pos_x
		{
			get
			{
				return _pos_x;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_pos_x = value;
			}
		}

		public bool HasPos_x => has_field.has_field(1);

		public long pos_z
		{
			get
			{
				return _pos_z;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_pos_z = value;
			}
		}

		public bool HasPos_z => has_field.has_field(2);

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(3);

		public item item
		{
			get
			{
				return _item;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_item = value;
			}
		}

		public bool HasItem => has_field.has_field(4);

		public long ownServerId
		{
			get
			{
				return _ownServerId;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_ownServerId = value;
			}
		}

		public bool HasOwnServerId => has_field.has_field(5);

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
					serverId = deserialize.read_integer();
					break;
				case 1:
					pos_x = deserialize.read_integer();
					break;
				case 2:
					pos_z = deserialize.read_integer();
					break;
				case 3:
					type = deserialize.read_integer();
					break;
				case 4:
					item = deserialize.read_obj<item>();
					break;
				case 7:
					ownServerId = deserialize.read_integer();
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
				serialize.write_integer(serverId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(pos_x, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(pos_z, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(type, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_obj(item, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(ownServerId, 7);
			}
			return serialize.close();
		}
	}
}
