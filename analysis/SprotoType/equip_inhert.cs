using Sproto;

namespace SprotoType;

public class equip_inhert
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _indexId1;

		private long _containertype1;

		private long _indexId2;

		private long _containertype2;

		public long indexId1
		{
			get
			{
				return _indexId1;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_indexId1 = value;
			}
		}

		public bool HasIndexId1 => has_field.has_field(0);

		public long containertype1
		{
			get
			{
				return _containertype1;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_containertype1 = value;
			}
		}

		public bool HasContainertype1 => has_field.has_field(1);

		public long indexId2
		{
			get
			{
				return _indexId2;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_indexId2 = value;
			}
		}

		public bool HasIndexId2 => has_field.has_field(2);

		public long containertype2
		{
			get
			{
				return _containertype2;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_containertype2 = value;
			}
		}

		public bool HasContainertype2 => has_field.has_field(3);

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
					indexId1 = deserialize.read_integer();
					break;
				case 1:
					containertype1 = deserialize.read_integer();
					break;
				case 2:
					indexId2 = deserialize.read_integer();
					break;
				case 3:
					containertype2 = deserialize.read_integer();
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
				serialize.write_integer(indexId1, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(containertype1, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(indexId2, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(containertype2, 3);
			}
			return serialize.close();
		}
	}

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 5;

		private long _state;

		private long _containertype1;

		private long _containertype2;

		private gameitem _item1;

		private gameitem _item2;

		public long state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(0);

		public long containertype1
		{
			get
			{
				return _containertype1;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_containertype1 = value;
			}
		}

		public bool HasContainertype1 => has_field.has_field(1);

		public long containertype2
		{
			get
			{
				return _containertype2;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_containertype2 = value;
			}
		}

		public bool HasContainertype2 => has_field.has_field(2);

		public gameitem item1
		{
			get
			{
				return _item1;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_item1 = value;
			}
		}

		public bool HasItem1 => has_field.has_field(3);

		public gameitem item2
		{
			get
			{
				return _item2;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_item2 = value;
			}
		}

		public bool HasItem2 => has_field.has_field(4);

		public response()
			: base(max_field_count)
		{
		}

		public response(byte[] buffer)
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
					state = deserialize.read_integer();
					break;
				case 1:
					containertype1 = deserialize.read_integer();
					break;
				case 2:
					containertype2 = deserialize.read_integer();
					break;
				case 3:
					item1 = deserialize.read_obj<gameitem>();
					break;
				case 4:
					item2 = deserialize.read_obj<gameitem>();
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
				serialize.write_integer(state, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(containertype1, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(containertype2, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_obj(item1, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_obj(item2, 4);
			}
			return serialize.close();
		}
	}
}
