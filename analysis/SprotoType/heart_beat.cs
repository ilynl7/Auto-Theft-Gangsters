using Sproto;

namespace SprotoType;

public class heart_beat
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _time;

		private long _time2;

		public long time
		{
			get
			{
				return _time;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_time = value;
			}
		}

		public bool HasTime => has_field.has_field(0);

		public long time2
		{
			get
			{
				return _time2;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_time2 = value;
			}
		}

		public bool HasTime2 => has_field.has_field(1);

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
					time = deserialize.read_integer();
					break;
				case 1:
					time2 = deserialize.read_integer();
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
				serialize.write_integer(time, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(time2, 1);
			}
			return serialize.close();
		}
	}

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _time;

		private long _serverTime;

		public long time
		{
			get
			{
				return _time;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_time = value;
			}
		}

		public bool HasTime => has_field.has_field(0);

		public long serverTime
		{
			get
			{
				return _serverTime;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_serverTime = value;
			}
		}

		public bool HasServerTime => has_field.has_field(1);

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
					time = deserialize.read_integer();
					break;
				case 1:
					serverTime = deserialize.read_integer();
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
				serialize.write_integer(time, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(serverTime, 1);
			}
			return serialize.close();
		}
	}
}
