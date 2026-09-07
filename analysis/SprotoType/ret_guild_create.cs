using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_guild_create
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private guild_info _guild_info;

		private long _state;

		private Dictionary<string, donate_record> _donate_records;

		public guild_info guild_info
		{
			get
			{
				return _guild_info;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_guild_info = value;
			}
		}

		public bool HasGuild_info => has_field.has_field(0);

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

		public Dictionary<string, donate_record> donate_records
		{
			get
			{
				return _donate_records;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_donate_records = value;
			}
		}

		public bool HasDonate_records => has_field.has_field(2);

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
					guild_info = deserialize.read_obj<guild_info>();
					break;
				case 1:
					state = deserialize.read_integer();
					break;
				case 2:
					donate_records = deserialize.read_map((donate_record v) => v.id);
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
				serialize.write_obj(guild_info, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(state, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_obj(donate_records, 2);
			}
			return serialize.close();
		}
	}
}
