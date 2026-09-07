using Sproto;

namespace SprotoType;

public class count_down
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _type;

		private long _count_value;

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

		public long count_value
		{
			get
			{
				return _count_value;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_count_value = value;
			}
		}

		public bool HasCount_value => has_field.has_field(1);

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
					count_value = deserialize.read_integer();
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
				serialize.write_integer(count_value, 1);
			}
			return serialize.close();
		}
	}
}
