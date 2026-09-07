using Sproto;

namespace SprotoType;

public class ret_sign_30_day
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 7;

		private long _cur_sign;

		private long _replenish;

		private long _sys_sign;

		private bool _cur_sign_state;

		private bool _replenish_sign_state;

		private long _count;

		private string _str;

		public long cur_sign
		{
			get
			{
				return _cur_sign;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_cur_sign = value;
			}
		}

		public bool HasCur_sign => has_field.has_field(0);

		public long replenish
		{
			get
			{
				return _replenish;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_replenish = value;
			}
		}

		public bool HasReplenish => has_field.has_field(1);

		public long sys_sign
		{
			get
			{
				return _sys_sign;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_sys_sign = value;
			}
		}

		public bool HasSys_sign => has_field.has_field(2);

		public bool cur_sign_state
		{
			get
			{
				return _cur_sign_state;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_cur_sign_state = value;
			}
		}

		public bool HasCur_sign_state => has_field.has_field(3);

		public bool replenish_sign_state
		{
			get
			{
				return _replenish_sign_state;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_replenish_sign_state = value;
			}
		}

		public bool HasReplenish_sign_state => has_field.has_field(4);

		public long count
		{
			get
			{
				return _count;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_count = value;
			}
		}

		public bool HasCount => has_field.has_field(5);

		public string str
		{
			get
			{
				return _str;
			}
			set
			{
				has_field.set_field(6, is_has: true);
				_str = value;
			}
		}

		public bool HasStr => has_field.has_field(6);

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
					cur_sign = deserialize.read_integer();
					break;
				case 1:
					replenish = deserialize.read_integer();
					break;
				case 2:
					sys_sign = deserialize.read_integer();
					break;
				case 3:
					cur_sign_state = deserialize.read_boolean();
					break;
				case 4:
					replenish_sign_state = deserialize.read_boolean();
					break;
				case 5:
					count = deserialize.read_integer();
					break;
				case 6:
					str = deserialize.read_string();
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
				serialize.write_integer(cur_sign, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(replenish, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(sys_sign, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_boolean(cur_sign_state, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_boolean(replenish_sign_state, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(count, 5);
			}
			if (has_field.has_field(6))
			{
				serialize.write_string(str, 6);
			}
			return serialize.close();
		}
	}
}
