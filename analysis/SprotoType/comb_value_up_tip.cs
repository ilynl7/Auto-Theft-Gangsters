using Sproto;

namespace SprotoType;

public class comb_value_up_tip
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _current;

		private long _next;

		public long current
		{
			get
			{
				return _current;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_current = value;
			}
		}

		public bool HasCurrent => has_field.has_field(0);

		public long next
		{
			get
			{
				return _next;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_next = value;
			}
		}

		public bool HasNext => has_field.has_field(1);

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
					current = deserialize.read_integer();
					break;
				case 1:
					next = deserialize.read_integer();
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
				serialize.write_integer(current, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(next, 1);
			}
			return serialize.close();
		}
	}
}
