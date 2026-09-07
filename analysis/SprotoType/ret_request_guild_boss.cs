using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_guild_boss
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 5;

		private Dictionary<string, guild_boss> _guild_boss;

		private guild_battle_info _guild_battle_info;

		private long _level;

		private dance_state_info _dance_state_info;

		private Dictionary<string, guild_map_info> _guild_map_info;

		public Dictionary<string, guild_boss> guild_boss
		{
			get
			{
				return _guild_boss;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_guild_boss = value;
			}
		}

		public bool HasGuild_boss => has_field.has_field(0);

		public guild_battle_info guild_battle_info
		{
			get
			{
				return _guild_battle_info;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_guild_battle_info = value;
			}
		}

		public bool HasGuild_battle_info => has_field.has_field(1);

		public long level
		{
			get
			{
				return _level;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_level = value;
			}
		}

		public bool HasLevel => has_field.has_field(2);

		public dance_state_info dance_state_info
		{
			get
			{
				return _dance_state_info;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_dance_state_info = value;
			}
		}

		public bool HasDance_state_info => has_field.has_field(3);

		public Dictionary<string, guild_map_info> guild_map_info
		{
			get
			{
				return _guild_map_info;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_guild_map_info = value;
			}
		}

		public bool HasGuild_map_info => has_field.has_field(4);

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
					guild_boss = deserialize.read_map((guild_boss v) => v.id);
					break;
				case 1:
					guild_battle_info = deserialize.read_obj<guild_battle_info>();
					break;
				case 2:
					level = deserialize.read_integer();
					break;
				case 3:
					dance_state_info = deserialize.read_obj<dance_state_info>();
					break;
				case 4:
					guild_map_info = deserialize.read_map((guild_map_info v) => v.id);
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
				serialize.write_obj(guild_boss, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(guild_battle_info, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(level, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_obj(dance_state_info, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_obj(guild_map_info, 4);
			}
			return serialize.close();
		}
	}
}
