using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class sync_badgepack_item
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<long, gameitem> _gameitems;

		public Dictionary<long, gameitem> gameitems
		{
			get
			{
				return _gameitems;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_gameitems = value;
			}
		}

		public bool HasGameitems => has_field.has_field(0);

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
					gameitems = deserialize.read_map((gameitem v) => v.indexId);
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
				serialize.write_obj(gameitems, 0);
			}
			return serialize.close();
		}
	}
}
