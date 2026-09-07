using Sproto;

namespace SprotoType;

public class sync_random_team_state
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _state;

		private string _id;

		private long _type;

		public long state
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

		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(1);

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(2);

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
					state = deserialize.read_integer();
					break;
				case 1:
					id = deserialize.read_string();
					break;
				case 2:
					type = deserialize.read_integer();
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
				serialize.write_integer(state, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(id, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(type, 2);
			}
			return serialize.close();
		}
	}
}
