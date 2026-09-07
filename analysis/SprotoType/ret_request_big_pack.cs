using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_big_pack
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private string _ID;

		private long _state;

		private long _end_time;

		private Dictionary<string, special_big_pack> _special_big_packs;

		public string ID
		{
			get
			{
				return _ID;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_ID = value;
			}
		}

		public bool HasID => has_field.has_field(0);

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

		public long end_time
		{
			get
			{
				return _end_time;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_end_time = value;
			}
		}

		public bool HasEnd_time => has_field.has_field(2);

		public Dictionary<string, special_big_pack> special_big_packs
		{
			get
			{
				return _special_big_packs;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_special_big_packs = value;
			}
		}

		public bool HasSpecial_big_packs => has_field.has_field(3);

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
					ID = deserialize.read_string();
					break;
				case 1:
					state = deserialize.read_integer();
					break;
				case 2:
					end_time = deserialize.read_integer();
					break;
				case 3:
					special_big_packs = deserialize.read_map((special_big_pack v) => v.ID);
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
				serialize.write_string(ID, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(state, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(end_time, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_obj(special_big_packs, 3);
			}
			return serialize.close();
		}
	}
}
