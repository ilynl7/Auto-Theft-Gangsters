using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class battle_info : SprotoTypeBase
{
	private static int max_field_count = 6;

	private List<damage_list> _damage_list;

	private long _my_rank;

	private long _my_damage;

	private long _all_damage;

	private string _lastKill;

	private long _end_time;

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

	public long my_rank
	{
		get
		{
			return _my_rank;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_my_rank = value;
		}
	}

	public bool HasMy_rank => has_field.has_field(1);

	public long my_damage
	{
		get
		{
			return _my_damage;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_my_damage = value;
		}
	}

	public bool HasMy_damage => has_field.has_field(2);

	public long all_damage
	{
		get
		{
			return _all_damage;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_all_damage = value;
		}
	}

	public bool HasAll_damage => has_field.has_field(3);

	public string lastKill
	{
		get
		{
			return _lastKill;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_lastKill = value;
		}
	}

	public bool HasLastKill => has_field.has_field(4);

	public long end_time
	{
		get
		{
			return _end_time;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_end_time = value;
		}
	}

	public bool HasEnd_time => has_field.has_field(5);

	public battle_info()
		: base(max_field_count)
	{
	}

	public battle_info(byte[] buffer)
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
				my_rank = deserialize.read_integer();
				break;
			case 2:
				my_damage = deserialize.read_integer();
				break;
			case 3:
				all_damage = deserialize.read_integer();
				break;
			case 4:
				lastKill = deserialize.read_string();
				break;
			case 5:
				end_time = deserialize.read_integer();
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
			serialize.write_integer(my_rank, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(my_damage, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(all_damage, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_string(lastKill, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(end_time, 5);
		}
		return serialize.close();
	}
}
