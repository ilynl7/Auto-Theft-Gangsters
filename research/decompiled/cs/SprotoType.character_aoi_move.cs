using Sproto;

namespace SprotoType;

public class character_aoi_move : SprotoTypeBase
{
	private static int max_field_count = 3;

	private long _id;

	private movement _movement;

	private bool _walk;

	public long id
	{
		get
		{
			return _id;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_id = value;
		}
	}

	public bool HasId => has_field.has_field(0);

	public movement movement
	{
		get
		{
			return _movement;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_movement = value;
		}
	}

	public bool HasMovement => has_field.has_field(1);

	public bool walk
	{
		get
		{
			return _walk;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_walk = value;
		}
	}

	public bool HasWalk => has_field.has_field(2);

	public character_aoi_move()
		: base(max_field_count)
	{
	}

	public character_aoi_move(byte[] buffer)
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
				id = deserialize.read_integer();
				break;
			case 1:
				movement = deserialize.read_obj<movement>();
				break;
			case 2:
				walk = deserialize.read_boolean();
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
			serialize.write_integer(id, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_obj(movement, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_boolean(walk, 2);
		}
		return serialize.close();
	}
}
