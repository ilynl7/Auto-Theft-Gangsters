using Sproto;

namespace SprotoType;

public class move
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private position _pos;

		private bool _moving;

		private long _index;

		private long _parm;

		public position pos
		{
			get
			{
				return _pos;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_pos = value;
			}
		}

		public bool HasPos => has_field.has_field(0);

		public bool moving
		{
			get
			{
				return _moving;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_moving = value;
			}
		}

		public bool HasMoving => has_field.has_field(1);

		public long index
		{
			get
			{
				return _index;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_index = value;
			}
		}

		public bool HasIndex => has_field.has_field(2);

		public long parm
		{
			get
			{
				return _parm;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_parm = value;
			}
		}

		public bool HasParm => has_field.has_field(3);

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
					pos = deserialize.read_obj<position>();
					break;
				case 1:
					moving = deserialize.read_boolean();
					break;
				case 2:
					index = deserialize.read_integer();
					break;
				case 3:
					parm = deserialize.read_integer();
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
				serialize.write_obj(pos, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_boolean(moving, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(index, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(parm, 3);
			}
			return serialize.close();
		}
	}

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private position _pos;

		public position pos
		{
			get
			{
				return _pos;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_pos = value;
			}
		}

		public bool HasPos => has_field.has_field(0);

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
					pos = deserialize.read_obj<position>();
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
				serialize.write_obj(pos, 0);
			}
			return serialize.close();
		}
	}
}
