using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_guild_star
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<string, guild_star> _guild_stars;

		public Dictionary<string, guild_star> guild_stars
		{
			get
			{
				return _guild_stars;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_guild_stars = value;
			}
		}

		public bool HasGuild_stars => has_field.has_field(0);

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
					guild_stars = deserialize.read_map((guild_star v) => v.ID);
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
				serialize.write_obj(guild_stars, 0);
			}
			return serialize.close();
		}
	}
}
