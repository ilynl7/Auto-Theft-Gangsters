using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_level_reward
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<string, level_reward> _level_reward;

		public Dictionary<string, level_reward> level_reward
		{
			get
			{
				return _level_reward;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_level_reward = value;
			}
		}

		public bool HasLevel_reward => has_field.has_field(0);

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
					level_reward = deserialize.read_map((level_reward v) => v.ID);
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
				serialize.write_obj(level_reward, 0);
			}
			return serialize.close();
		}
	}
}
