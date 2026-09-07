using Sproto;

namespace SprotoType;

public class notify_copy_start_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _end_time;

		private long _type;

		private long _wave_time;

		private long _curWave;

		public long end_time
		{
			get
			{
				return _end_time;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_end_time = value;
			}
		}

		public bool HasEnd_time => has_field.has_field(0);

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(1);

		public long wave_time
		{
			get
			{
				return _wave_time;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_wave_time = value;
			}
		}

		public bool HasWave_time => has_field.has_field(2);

		public long curWave
		{
			get
			{
				return _curWave;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_curWave = value;
			}
		}

		public bool HasCurWave => has_field.has_field(3);

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
					end_time = deserialize.read_integer();
					break;
				case 1:
					type = deserialize.read_integer();
					break;
				case 2:
					wave_time = deserialize.read_integer();
					break;
				case 3:
					curWave = deserialize.read_integer();
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
				serialize.write_integer(end_time, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(type, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(wave_time, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(curWave, 3);
			}
			return serialize.close();
		}
	}
}
