using Sproto;

namespace SprotoType;

public class enter_teleport_point
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _x;

		private long _y;

		private long _z;

		private long _index;

		public long x
		{
			get
			{
				return _x;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_x = value;
			}
		}

		public bool HasX => has_field.has_field(0);

		public long y
		{
			get
			{
				return _y;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_y = value;
			}
		}

		public bool HasY => has_field.has_field(1);

		public long z
		{
			get
			{
				return _z;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_z = value;
			}
		}

		public bool HasZ => has_field.has_field(2);

		public long index
		{
			get
			{
				return _index;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_index = value;
			}
		}

		public bool HasIndex => has_field.has_field(3);

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
					x = deserialize.read_integer();
					break;
				case 1:
					y = deserialize.read_integer();
					break;
				case 2:
					z = deserialize.read_integer();
					break;
				case 3:
					index = deserialize.read_integer();
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
				serialize.write_integer(x, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(y, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(z, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(index, 3);
			}
			return serialize.close();
		}
	}
}
