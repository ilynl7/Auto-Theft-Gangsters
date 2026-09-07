using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_special_big_pack
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<string, special_big_pack> _special_big_packs;

		public Dictionary<string, special_big_pack> special_big_packs
		{
			get
			{
				return _special_big_packs;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_special_big_packs = value;
			}
		}

		public bool HasSpecial_big_packs => has_field.has_field(0);

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
					special_big_packs = deserialize.read_map((special_big_pack v) => v.ID);
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
				serialize.write_obj(special_big_packs, 0);
			}
			return serialize.close();
		}
	}
}
