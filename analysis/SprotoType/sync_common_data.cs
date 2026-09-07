using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class sync_common_data
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 15;

		private long _serverTime;

		private long _time_offset;

		private long _daily_mission_refresh_time;

		private long _pvp_scale;

		private long _first_buy;

		private long _big_pack;

		private long _adfree;

		private long _tips;

		private Dictionary<string, function_info> _func_info;

		private long _push;

		private long _guildId;

		private long _seed;

		private long _server_level;

		private long _start_time;

		public long serverTime
		{
			get
			{
				return _serverTime;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_serverTime = value;
			}
		}

		public bool HasServerTime => has_field.has_field(0);

		public long time_offset
		{
			get
			{
				return _time_offset;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_time_offset = value;
			}
		}

		public bool HasTime_offset => has_field.has_field(1);

		public long daily_mission_refresh_time
		{
			get
			{
				return _daily_mission_refresh_time;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_daily_mission_refresh_time = value;
			}
		}

		public bool HasDaily_mission_refresh_time => has_field.has_field(2);

		public long pvp_scale
		{
			get
			{
				return _pvp_scale;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_pvp_scale = value;
			}
		}

		public bool HasPvp_scale => has_field.has_field(3);

		public long first_buy
		{
			get
			{
				return _first_buy;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_first_buy = value;
			}
		}

		public bool HasFirst_buy => has_field.has_field(4);

		public long big_pack
		{
			get
			{
				return _big_pack;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_big_pack = value;
			}
		}

		public bool HasBig_pack => has_field.has_field(5);

		public long adfree
		{
			get
			{
				return _adfree;
			}
			set
			{
				has_field.set_field(6, is_has: true);
				_adfree = value;
			}
		}

		public bool HasAdfree => has_field.has_field(6);

		public long tips
		{
			get
			{
				return _tips;
			}
			set
			{
				has_field.set_field(7, is_has: true);
				_tips = value;
			}
		}

		public bool HasTips => has_field.has_field(7);

		public Dictionary<string, function_info> func_info
		{
			get
			{
				return _func_info;
			}
			set
			{
				has_field.set_field(8, is_has: true);
				_func_info = value;
			}
		}

		public bool HasFunc_info => has_field.has_field(8);

		public long push
		{
			get
			{
				return _push;
			}
			set
			{
				has_field.set_field(9, is_has: true);
				_push = value;
			}
		}

		public bool HasPush => has_field.has_field(9);

		public long guildId
		{
			get
			{
				return _guildId;
			}
			set
			{
				has_field.set_field(10, is_has: true);
				_guildId = value;
			}
		}

		public bool HasGuildId => has_field.has_field(10);

		public long seed
		{
			get
			{
				return _seed;
			}
			set
			{
				has_field.set_field(11, is_has: true);
				_seed = value;
			}
		}

		public bool HasSeed => has_field.has_field(11);

		public long server_level
		{
			get
			{
				return _server_level;
			}
			set
			{
				has_field.set_field(12, is_has: true);
				_server_level = value;
			}
		}

		public bool HasServer_level => has_field.has_field(12);

		public long start_time
		{
			get
			{
				return _start_time;
			}
			set
			{
				has_field.set_field(13, is_has: true);
				_start_time = value;
			}
		}

		public bool HasStart_time => has_field.has_field(13);

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
					serverTime = deserialize.read_integer();
					break;
				case 2:
					time_offset = deserialize.read_integer();
					break;
				case 3:
					daily_mission_refresh_time = deserialize.read_integer();
					break;
				case 4:
					pvp_scale = deserialize.read_integer();
					break;
				case 5:
					first_buy = deserialize.read_integer();
					break;
				case 6:
					big_pack = deserialize.read_integer();
					break;
				case 7:
					adfree = deserialize.read_integer();
					break;
				case 8:
					tips = deserialize.read_integer();
					break;
				case 9:
					func_info = deserialize.read_map((function_info v) => v.ID);
					break;
				case 10:
					push = deserialize.read_integer();
					break;
				case 11:
					guildId = deserialize.read_integer();
					break;
				case 12:
					seed = deserialize.read_integer();
					break;
				case 13:
					server_level = deserialize.read_integer();
					break;
				case 14:
					start_time = deserialize.read_integer();
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
				serialize.write_integer(serverTime, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(time_offset, 2);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(daily_mission_refresh_time, 3);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(pvp_scale, 4);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(first_buy, 5);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(big_pack, 6);
			}
			if (has_field.has_field(6))
			{
				serialize.write_integer(adfree, 7);
			}
			if (has_field.has_field(7))
			{
				serialize.write_integer(tips, 8);
			}
			if (has_field.has_field(8))
			{
				serialize.write_obj(func_info, 9);
			}
			if (has_field.has_field(9))
			{
				serialize.write_integer(push, 10);
			}
			if (has_field.has_field(10))
			{
				serialize.write_integer(guildId, 11);
			}
			if (has_field.has_field(11))
			{
				serialize.write_integer(seed, 12);
			}
			if (has_field.has_field(12))
			{
				serialize.write_integer(server_level, 13);
			}
			if (has_field.has_field(13))
			{
				serialize.write_integer(start_time, 14);
			}
			return serialize.close();
		}
	}
}
