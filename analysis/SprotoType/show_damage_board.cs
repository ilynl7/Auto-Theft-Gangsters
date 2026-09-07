using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class show_damage_board
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private List<acceptdamge> _damges;

		public List<acceptdamge> damges
		{
			get
			{
				return _damges;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_damges = value;
			}
		}

		public bool HasDamges => has_field.has_field(0);

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
					damges = deserialize.read_obj_list<acceptdamge>();
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
				serialize.write_obj(damges, 0);
			}
			return serialize.close();
		}
	}
}
