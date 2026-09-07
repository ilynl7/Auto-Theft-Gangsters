using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_guild_req_list
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 5;

		private Dictionary<long, guild_info> _guild_info;

		private List<long> _applyGuildId;

		private long _curPage;

		private long _maxPage;

		private long _leave_time;

		public Dictionary<long, guild_info> guild_info
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

		public List<long> applyGuildId
		{
			get
			{
				return _applyGuildId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_applyGuildId = value;
			}
		}

		public bool HasApplyGuildId => has_field.has_field(1);

		public long curPage
		{
			get
			{
				return _curPage;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_curPage = value;
			}
		}

		public bool HasCurPage => has_field.has_field(2);

		public long maxPage
		{
			get
			{
				return _maxPage;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_maxPage = value;
			}
		}

		public bool HasMaxPage => has_field.has_field(3);

		public long leave_time
		{
			get
			{
				return _leave_time;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_leave_time = value;
			}
		}

		public bool HasLeave_time => has_field.has_field(4);

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
					guild_info = deserialize.read_map((guild_info v) => v.guildId);
					break;
				case 1:
					applyGuildId = deserialize.read_integer_list();
					break;
				case 2:
					curPage = deserialize.read_integer();
					break;
				case 3:
					maxPage = deserialize.read_integer();
					break;
				case 4:
					leave_time = deserialize.read_integer();
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
				serialize.write_integer(applyGuildId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(curPage, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(maxPage, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(leave_time, 4);
			}
			return serialize.close();
		}
	}
}
