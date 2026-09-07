using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_daily_buy
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<string, daily_buy> _daily_buys;

		public Dictionary<string, daily_buy> daily_buys
		{
			get
			{
				return _daily_buys;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_daily_buys = value;
			}
		}

		public bool HasDaily_buys => has_field.has_field(0);

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
					daily_buys = deserialize.read_map((daily_buy v) => v.ID);
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
				serialize.write_obj(daily_buys, 0);
			}
			return serialize.close();
		}
	}
}
