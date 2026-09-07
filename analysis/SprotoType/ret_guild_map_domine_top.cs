using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_guild_map_domine_top
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 6;

		private List<damage_list> _damage_list;

		private List<damage_list> _guild_damage_list;

		private long _my_rank;

		private long _my_damage;

		private long _my_rank2;

		private long _my_damage2;

		public List<damage_list> damage_list
		{
			get
			{
				return _damage_list;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_damage_list = value;
			}
		}

		public bool HasDamage_list => has_field.has_field(0);

		public List<damage_list> guild_damage_list
		{
			get
			{
				return _guild_damage_list;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_guild_damage_list = value;
			}
		}

		public bool HasGuild_damage_list => has_field.has_field(1);

		public long my_rank
		{
			get
			{
				return _my_rank;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_my_rank = value;
			}
		}

		public bool HasMy_rank => has_field.has_field(2);

		public long my_damage
		{
			get
			{
				return _my_damage;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_my_damage = value;
			}
		}

		public bool HasMy_damage => has_field.has_field(3);

		public long my_rank2
		{
			get
			{
				return _my_rank2;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_my_rank2 = value;
			}
		}

		public bool HasMy_rank2 => has_field.has_field(4);

		public long my_damage2
		{
			get
			{
				return _my_damage2;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_my_damage2 = value;
			}
		}

		public bool HasMy_damage2 => has_field.has_field(5);

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
					damage_list = deserialize.read_obj_list<damage_list>();
					break;
				case 1:
					guild_damage_list = deserialize.read_obj_list<damage_list>();
					break;
				case 2:
					my_rank = deserialize.read_integer();
					break;
				case 3:
					my_damage = deserialize.read_integer();
					break;
				case 4:
					my_rank2 = deserialize.read_integer();
					break;
				case 5:
					my_damage2 = deserialize.read_integer();
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
				serialize.write_obj(damage_list, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(guild_damage_list, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(my_rank, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(my_damage, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(my_rank2, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(my_damage2, 5);
			}
			return serialize.close();
		}
	}
}
