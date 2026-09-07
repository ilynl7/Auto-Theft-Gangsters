using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_guild_battle_member
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private List<guild_member_info> _guild_member_info;

		public List<guild_member_info> guild_member_info
		{
			get
			{
				return _guild_member_info;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_guild_member_info = value;
			}
		}

		public bool HasGuild_member_info => has_field.has_field(0);

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
					guild_member_info = deserialize.read_obj_list<guild_member_info>();
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
				serialize.write_obj(guild_member_info, 0);
			}
			return serialize.close();
		}
	}
}
