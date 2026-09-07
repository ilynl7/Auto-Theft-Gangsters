using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class sync_dance_state_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _state;

		private long _open;

		private Dictionary<long, dance_state_info> _dance_state_info;

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

		public long open
		{
			get
			{
				return _open;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_open = value;
			}
		}

		public bool HasOpen => has_field.has_field(1);

		public Dictionary<long, dance_state_info> dance_state_info
		{
			get
			{
				return _dance_state_info;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_dance_state_info = value;
			}
		}

		public bool HasDance_state_info => has_field.has_field(2);

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
					open = deserialize.read_integer();
					break;
				case 2:
					dance_state_info = deserialize.read_map((dance_state_info v) => v.uuid);
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
				serialize.write_integer(open, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_obj(dance_state_info, 2);
			}
			return serialize.close();
		}
	}
}
