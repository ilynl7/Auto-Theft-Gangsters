using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class use_skill_buff
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private List<buff> _buffs;

		public List<buff> buffs
		{
			get
			{
				return _buffs;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_buffs = value;
			}
		}

		public bool HasBuffs => has_field.has_field(0);

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
					buffs = deserialize.read_obj_list<buff>();
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
				serialize.write_obj(buffs, 0);
			}
			return serialize.close();
		}
	}
}
