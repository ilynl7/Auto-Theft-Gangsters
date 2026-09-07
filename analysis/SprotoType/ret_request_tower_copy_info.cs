using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_tower_copy_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private tower_info _tower_info;

		private List<tower_special_reward> _tower_special_reward;

		public tower_info tower_info
		{
			get
			{
				return _tower_info;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_tower_info = value;
			}
		}

		public bool HasTower_info => has_field.has_field(0);

		public List<tower_special_reward> tower_special_reward
		{
			get
			{
				return _tower_special_reward;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_tower_special_reward = value;
			}
		}

		public bool HasTower_special_reward => has_field.has_field(1);

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
					tower_info = deserialize.read_obj<tower_info>();
					break;
				case 2:
					tower_special_reward = deserialize.read_obj_list<tower_special_reward>();
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
				serialize.write_obj(tower_info, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(tower_special_reward, 2);
			}
			return serialize.close();
		}
	}
}
