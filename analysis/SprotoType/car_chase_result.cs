using Sproto;

namespace SprotoType;

public class car_chase_result
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private bool _state;

		private long _param1;

		private string _param2;

		public bool state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(0);

		public long param1
		{
			get
			{
				return _param1;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_param1 = value;
			}
		}

		public bool HasParam1 => has_field.has_field(1);

		public string param2
		{
			get
			{
				return _param2;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_param2 = value;
			}
		}

		public bool HasParam2 => has_field.has_field(2);

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
					state = deserialize.read_boolean();
					break;
				case 1:
					param1 = deserialize.read_integer();
					break;
				case 2:
					param2 = deserialize.read_string();
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
				serialize.write_boolean(state, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(param1, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(param2, 2);
			}
			return serialize.close();
		}
	}
}
