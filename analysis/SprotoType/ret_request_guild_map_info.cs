using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_guild_map_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<string, guild_map_info> _guild_map_info;

		public Dictionary<string, guild_map_info> guild_map_info
		{
			get
			{
				return _guild_map_info;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_guild_map_info = value;
			}
		}

		public bool HasGuild_map_info => has_field.has_field(0);

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
				if (num == 0)
				{
					guild_map_info = deserialize.read_map((guild_map_info v) => v.id);
				}
				else
				{
					deserialize.read_unknow_data();
				}
			}
		}

		public override int encode(SprotoStream stream)
		{
			serialize.open(stream);
			if (has_field.has_field(0))
			{
				serialize.write_obj(guild_map_info, 0);
			}
			return serialize.close();
		}
	}
}
