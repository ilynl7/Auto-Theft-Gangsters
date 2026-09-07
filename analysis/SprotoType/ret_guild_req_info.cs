using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_guild_req_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 5;

		private guild_info _guild_info;

		private Dictionary<string, donate_record> _donate_records;

		private long _all_contribute;

		private bool _exist;

		private long _contribute;

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

		public Dictionary<string, donate_record> donate_records
		{
			get
			{
				return _donate_records;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_donate_records = value;
			}
		}

		public bool HasDonate_records => has_field.has_field(1);

		public long all_contribute
		{
			get
			{
				return _all_contribute;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_all_contribute = value;
			}
		}

		public bool HasAll_contribute => has_field.has_field(2);

		public bool exist
		{
			get
			{
				return _exist;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_exist = value;
			}
		}

		public bool HasExist => has_field.has_field(3);

		public long contribute
		{
			get
			{
				return _contribute;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_contribute = value;
			}
		}

		public bool HasContribute => has_field.has_field(4);

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
					donate_records = deserialize.read_map((donate_record v) => v.id);
					break;
				case 2:
					all_contribute = deserialize.read_integer();
					break;
				case 3:
					exist = deserialize.read_boolean();
					break;
				case 4:
					contribute = deserialize.read_integer();
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
				serialize.write_obj(donate_records, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(all_contribute, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_boolean(exist, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(contribute, 4);
			}
			return serialize.close();
		}
	}
}
