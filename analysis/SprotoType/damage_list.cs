using Sproto;

namespace SprotoType;

public class damage_list : SprotoTypeBase
{
	private static int max_field_count = 3;

	private string _name;

	private long _damage;

	private long _id;

	public string name
	{
		get
		{
			return _name;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_name = value;
		}
	}

	public bool HasName => has_field.has_field(0);

	public long damage
	{
		get
		{
			return _damage;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_damage = value;
		}
	}

	public bool HasDamage => has_field.has_field(1);

	public long id
	{
		get
		{
			return _id;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_id = value;
		}
	}

	public bool HasId => has_field.has_field(2);

	public damage_list()
		: base(max_field_count)
	{
	}

	public damage_list(byte[] buffer)
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
				name = deserialize.read_string();
				break;
			case 1:
				damage = deserialize.read_integer();
				break;
			case 2:
				id = deserialize.read_integer();
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
			serialize.write_string(name, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(damage, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(id, 2);
		}
		return serialize.close();
	}
}
