using Sproto;

namespace SprotoType;

public class update_queue_rank
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _rank;

		private long _remain_time;

		public long rank
		{
			get
			{
				return _rank;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_rank = value;
			}
		}

		public bool HasRank => has_field.has_field(0);

		public long remain_time
		{
			get
			{
				return _remain_time;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_remain_time = value;
			}
		}

		public bool HasRemain_time => has_field.has_field(1);

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
					rank = deserialize.read_integer();
					break;
				case 1:
					remain_time = deserialize.read_integer();
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
				serialize.write_integer(rank, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(remain_time, 1);
			}
			return serialize.close();
		}
	}
}
