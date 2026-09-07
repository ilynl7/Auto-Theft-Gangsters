using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_grant_tower_reward
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 5;

		private long _type;

		private long _id;

		private List<item> _items;

		private tower_info _tower_info;

		private List<tower_special_reward> _tower_special_reward;

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(0);

		public long id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(1);

		public List<item> items
		{
			get
			{
				return _items;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_items = value;
			}
		}

		public bool HasItems => has_field.has_field(2);

		public tower_info tower_info
		{
			get
			{
				return _tower_info;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_tower_info = value;
			}
		}

		public bool HasTower_info => has_field.has_field(3);

		public List<tower_special_reward> tower_special_reward
		{
			get
			{
				return _tower_special_reward;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_tower_special_reward = value;
			}
		}

		public bool HasTower_special_reward => has_field.has_field(4);

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
					type = deserialize.read_integer();
					break;
				case 1:
					id = deserialize.read_integer();
					break;
				case 2:
					items = deserialize.read_obj_list<item>();
					break;
				case 3:
					tower_info = deserialize.read_obj<tower_info>();
					break;
				case 4:
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
				serialize.write_integer(type, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(id, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_obj(items, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_obj(tower_info, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_obj(tower_special_reward, 4);
			}
			return serialize.close();
		}
	}
}
