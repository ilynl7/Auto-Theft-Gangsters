using Sproto;

namespace SprotoType;

public class character_aoi : SprotoTypeBase
{
	private static int max_field_count = 7;

	private long _id;

	private characterVisual _visual;

	private general _general;

	private attribute_other _attribute_other;

	private movement _movement;

	private runtime_agent _runtime;

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

	public characterVisual visual
	{
		get
		{
			return _visual;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_visual = value;
		}
	}

	public bool HasVisual => has_field.has_field(1);

	public general general
	{
		get
		{
			return _general;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_general = value;
		}
	}

	public bool HasGeneral => has_field.has_field(2);

	public attribute_other attribute_other
	{
		get
		{
			return _attribute_other;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_attribute_other = value;
		}
	}

	public bool HasAttribute_other => has_field.has_field(3);

	public movement movement
	{
		get
		{
			return _movement;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_movement = value;
		}
	}

	public bool HasMovement => has_field.has_field(4);

	public runtime_agent runtime
	{
		get
		{
			return _runtime;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_runtime = value;
		}
	}

	public bool HasRuntime => has_field.has_field(5);

	public character_aoi()
		: base(max_field_count)
	{
	}

	public character_aoi(byte[] buffer)
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
				visual = deserialize.read_obj<characterVisual>();
				break;
			case 2:
				general = deserialize.read_obj<general>();
				break;
			case 3:
				attribute_other = deserialize.read_obj<attribute_other>();
				break;
			case 5:
				movement = deserialize.read_obj<movement>();
				break;
			case 6:
				runtime = deserialize.read_obj<runtime_agent>();
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
			serialize.write_obj(visual, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_obj(general, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_obj(attribute_other, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_obj(movement, 5);
		}
		if (has_field.has_field(5))
		{
			serialize.write_obj(runtime, 6);
		}
		return serialize.close();
	}
}
