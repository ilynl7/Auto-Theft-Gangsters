using Sproto;

namespace SprotoType;

public class ret_watch_video_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _cur_times;

		private long _max_times;

		private long _every_time;

		private long _state;

		public long cur_times
		{
			get
			{
				return _cur_times;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_cur_times = value;
			}
		}

		public bool HasCur_times => has_field.has_field(0);

		public long max_times
		{
			get
			{
				return _max_times;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_max_times = value;
			}
		}

		public bool HasMax_times => has_field.has_field(1);

		public long every_time
		{
			get
			{
				return _every_time;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_every_time = value;
			}
		}

		public bool HasEvery_time => has_field.has_field(2);

		public long state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(3);

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
					cur_times = deserialize.read_integer();
					break;
				case 1:
					max_times = deserialize.read_integer();
					break;
				case 2:
					every_time = deserialize.read_integer();
					break;
				case 3:
					state = deserialize.read_integer();
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
				serialize.write_integer(cur_times, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(max_times, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(every_time, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(state, 3);
			}
			return serialize.close();
		}
	}
}
