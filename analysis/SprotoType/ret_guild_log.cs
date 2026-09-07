using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_guild_log
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private List<string> _logs;

		public List<string> logs
		{
			get
			{
				return _logs;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_logs = value;
			}
		}

		public bool HasLogs => has_field.has_field(0);

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
					logs = deserialize.read_string_list();
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
				serialize.write_string(logs, 0);
			}
			return serialize.close();
		}
	}
}
