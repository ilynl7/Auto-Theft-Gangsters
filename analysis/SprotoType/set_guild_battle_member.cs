using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class set_guild_battle_member
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private List<long> _list;

		public List<long> list
		{
			get
			{
				return _list;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_list = value;
			}
		}

		public bool HasList => has_field.has_field(0);

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
					list = deserialize.read_integer_list();
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
				serialize.write_integer(list, 0);
			}
			return serialize.close();
		}
	}
}
