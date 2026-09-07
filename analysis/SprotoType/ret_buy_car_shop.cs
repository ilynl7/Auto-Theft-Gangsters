using Sproto;

namespace SprotoType;

public class ret_buy_car_shop
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private string _mountId;

		private long _state;

		public string mountId
		{
			get
			{
				return _mountId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_mountId = value;
			}
		}

		public bool HasMountId => has_field.has_field(0);

		public long state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(1);

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
					mountId = deserialize.read_string();
					break;
				case 1:
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
				serialize.write_string(mountId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(state, 1);
			}
			return serialize.close();
		}
	}
}
