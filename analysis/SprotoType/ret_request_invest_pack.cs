using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_invest_pack
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<string, invest_pack> _invest_pack;

		public Dictionary<string, invest_pack> invest_pack
		{
			get
			{
				return _invest_pack;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_invest_pack = value;
			}
		}

		public bool HasInvest_pack => has_field.has_field(0);

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
					invest_pack = deserialize.read_map((invest_pack v) => v.ID);
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
				serialize.write_obj(invest_pack, 0);
			}
			return serialize.close();
		}
	}
}
