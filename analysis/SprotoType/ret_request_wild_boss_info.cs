using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_wild_boss_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<string, activity_info> _activity_info;

		public Dictionary<string, activity_info> activity_info
		{
			get
			{
				return _activity_info;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_activity_info = value;
			}
		}

		public bool HasActivity_info => has_field.has_field(0);

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
					activity_info = deserialize.read_map((activity_info v) => v.ID);
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
				serialize.write_obj(activity_info, 0);
			}
			return serialize.close();
		}
	}
}
