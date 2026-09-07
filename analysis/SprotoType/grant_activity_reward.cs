using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class grant_activity_reward
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 7;

		private long _type;

		private long _state;

		private bool _win;

		private List<item> _items;

		private battle_info _battle_info;

		private string _ID;

		private List<item> _items2;

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

		public long state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(1);

		public bool win
		{
			get
			{
				return _win;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_win = value;
			}
		}

		public bool HasWin => has_field.has_field(2);

		public List<item> items
		{
			get
			{
				return _items;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_items = value;
			}
		}

		public bool HasItems => has_field.has_field(3);

		public battle_info battle_info
		{
			get
			{
				return _battle_info;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_battle_info = value;
			}
		}

		public bool HasBattle_info => has_field.has_field(4);

		public string ID
		{
			get
			{
				return _ID;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_ID = value;
			}
		}

		public bool HasID => has_field.has_field(5);

		public List<item> items2
		{
			get
			{
				return _items2;
			}
			set
			{
				has_field.set_field(6, is_has: true);
				_items2 = value;
			}
		}

		public bool HasItems2 => has_field.has_field(6);

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
					state = deserialize.read_integer();
					break;
				case 2:
					win = deserialize.read_boolean();
					break;
				case 3:
					items = deserialize.read_obj_list<item>();
					break;
				case 4:
					battle_info = deserialize.read_obj<battle_info>();
					break;
				case 5:
					ID = deserialize.read_string();
					break;
				case 6:
					items2 = deserialize.read_obj_list<item>();
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
				serialize.write_integer(state, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_boolean(win, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_obj(items, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_obj(battle_info, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_string(ID, 5);
			}
			if (has_field.has_field(6))
			{
				serialize.write_obj(items2, 6);
			}
			return serialize.close();
		}
	}
}
