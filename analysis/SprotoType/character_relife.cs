using Sproto;

namespace SprotoType;

public class character_relife : SprotoTypeBase
{
	private static int max_field_count = 3;

	private long _id;

	private attribute_other _attribute_other;

	private movement _movement;

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

	public attribute_other attribute_other
	{
		get
		{
			return _attribute_other;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_attribute_other = value;
		}
	}

	public bool HasAttribute_other => has_field.has_field(1);

	public movement movement
	{
		get
		{
			return _movement;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_movement = value;
		}
	}

	public bool HasMovement => has_field.has_field(2);

	public character_relife()
		: base(max_field_count)
	{
	}

	public character_relife(byte[] buffer)
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
				attribute_other = deserialize.read_obj<attribute_other>();
				break;
			case 2:
				movement = deserialize.read_obj<movement>();
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
			serialize.write_obj(attribute_other, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_obj(movement, 2);
		}
		return serialize.close();
	}
}
