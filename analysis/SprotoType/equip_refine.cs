using Sproto;

namespace SprotoType;

public class equip_refine
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 6;

		private string _Id;

		private string _preId;

		private string _curId;

		private long _partId;

		private long _level;

		private bool _safe;

		public string Id
		{
			get
			{
				return _Id;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_Id = value;
			}
		}

		public bool HasId => has_field.has_field(0);

		public string preId
		{
			get
			{
				return _preId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_preId = value;
			}
		}

		public bool HasPreId => has_field.has_field(1);

		public string curId
		{
			get
			{
				return _curId;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_curId = value;
			}
		}

		public bool HasCurId => has_field.has_field(2);

		public long partId
		{
			get
			{
				return _partId;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_partId = value;
			}
		}

		public bool HasPartId => has_field.has_field(3);

		public long level
		{
			get
			{
				return _level;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_level = value;
			}
		}

		public bool HasLevel => has_field.has_field(4);

		public bool safe
		{
			get
			{
				return _safe;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_safe = value;
			}
		}

		public bool HasSafe => has_field.has_field(5);

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
					Id = deserialize.read_string();
					break;
				case 1:
					preId = deserialize.read_string();
					break;
				case 2:
					curId = deserialize.read_string();
					break;
				case 3:
					partId = deserialize.read_integer();
					break;
				case 4:
					level = deserialize.read_integer();
					break;
				case 5:
					safe = deserialize.read_boolean();
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
				serialize.write_string(Id, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(preId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(curId, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(partId, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(level, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_boolean(safe, 5);
			}
			return serialize.close();
		}
	}

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _state;

		private long _partId;

		private long _level;

		private long _allstar;

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

		public long partId
		{
			get
			{
				return _partId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_partId = value;
			}
		}

		public bool HasPartId => has_field.has_field(1);

		public long level
		{
			get
			{
				return _level;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_level = value;
			}
		}

		public bool HasLevel => has_field.has_field(2);

		public long allstar
		{
			get
			{
				return _allstar;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_allstar = value;
			}
		}

		public bool HasAllstar => has_field.has_field(3);

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
					partId = deserialize.read_integer();
					break;
				case 2:
					level = deserialize.read_integer();
					break;
				case 3:
					allstar = deserialize.read_integer();
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
				serialize.write_integer(partId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(level, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(allstar, 3);
			}
			return serialize.close();
		}
	}
}
