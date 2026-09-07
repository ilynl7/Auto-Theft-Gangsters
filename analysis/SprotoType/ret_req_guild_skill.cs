using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_req_guild_skill
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<long, guild_skill> _guild_skill;

		public Dictionary<long, guild_skill> guild_skill
		{
			get
			{
				return _guild_skill;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_guild_skill = value;
			}
		}

		public bool HasGuild_skill => has_field.has_field(0);

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
					guild_skill = deserialize.read_map((guild_skill v) => v.skillType);
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
				serialize.write_obj(guild_skill, 0);
			}
			return serialize.close();
		}
	}
}
