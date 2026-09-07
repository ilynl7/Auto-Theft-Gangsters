using Sproto;

namespace SprotoType;

public class ask_confirm
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _type;

		private string _id;

		private long _parm1;

		private long _param2;

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

		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(1);

		public long parm1
		{
			get
			{
				return _parm1;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_parm1 = value;
			}
		}

		public bool HasParm1 => has_field.has_field(2);

		public long param2
		{
			get
			{
				return _param2;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_param2 = value;
			}
		}

		public bool HasParam2 => has_field.has_field(3);

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
					id = deserialize.read_string();
					break;
				case 2:
					parm1 = deserialize.read_integer();
					break;
				case 3:
					param2 = deserialize.read_integer();
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
				serialize.write_string(id, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(parm1, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(param2, 3);
			}
			return serialize.close();
		}
	}

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private long _state;

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
				if (num == 0)
				{
					state = deserialize.read_integer();
				}
				else
				{
					deserialize.read_unknow_data();
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
			return serialize.close();
		}
	}
}
