using Sproto;

namespace SprotoType;

public class ret_sign_week
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _cur_sign;

		private bool _cur_sign_state;

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

		public bool cur_sign_state
		{
			get
			{
				return _cur_sign_state;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_cur_sign_state = value;
			}
		}

		public bool HasCur_sign_state => has_field.has_field(1);

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
					cur_sign_state = deserialize.read_boolean();
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
				serialize.write_boolean(cur_sign_state, 1);
			}
			return serialize.close();
		}
	}
}
