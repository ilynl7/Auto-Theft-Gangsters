using Sproto;

namespace SprotoType;

public class open_multi_tower_reward
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _index;

		private long _floor;

		public long index
		{
			get
			{
				return _index;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_index = value;
			}
		}

		public bool HasIndex => has_field.has_field(0);

		public long floor
		{
			get
			{
				return _floor;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_floor = value;
			}
		}

		public bool HasFloor => has_field.has_field(1);

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
					index = deserialize.read_integer();
					break;
				case 1:
					floor = deserialize.read_integer();
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
				serialize.write_integer(index, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(floor, 1);
			}
			return serialize.close();
		}
	}
}
