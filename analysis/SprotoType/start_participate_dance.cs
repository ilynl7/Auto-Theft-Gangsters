using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class start_participate_dance
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private string _curUse;

		private Dictionary<string, dance_info> _dance_info;

		public string curUse
		{
			get
			{
				return _curUse;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_curUse = value;
			}
		}

		public bool HasCurUse => has_field.has_field(0);

		public Dictionary<string, dance_info> dance_info
		{
			get
			{
				return _dance_info;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_dance_info = value;
			}
		}

		public bool HasDance_info => has_field.has_field(1);

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
					curUse = deserialize.read_string();
					break;
				case 1:
					dance_info = deserialize.read_map((dance_info v) => v.ID);
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
				serialize.write_string(curUse, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(dance_info, 1);
			}
			return serialize.close();
		}
	}
}
