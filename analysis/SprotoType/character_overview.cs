using Sproto;

namespace SprotoType;

public class character_overview : SprotoTypeBase
{
	private static int max_field_count = 6;

	private long _id;

	private general _general;

	private attribute_overview _attribute_other;

	private characterVisual _visual;

	private long _createtime;

	private long _forbidden;

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

	public general general
	{
		get
		{
			return _general;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_general = value;
		}
	}

	public bool HasGeneral => has_field.has_field(1);

	public attribute_overview attribute_other
	{
		get
		{
			return _attribute_other;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_attribute_other = value;
		}
	}

	public bool HasAttribute_other => has_field.has_field(2);

	public characterVisual visual
	{
		get
		{
			return _visual;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_visual = value;
		}
	}

	public bool HasVisual => has_field.has_field(3);

	public long createtime
	{
		get
		{
			return _createtime;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_createtime = value;
		}
	}

	public bool HasCreatetime => has_field.has_field(4);

	public long forbidden
	{
		get
		{
			return _forbidden;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_forbidden = value;
		}
	}

	public bool HasForbidden => has_field.has_field(5);

	public character_overview()
		: base(max_field_count)
	{
	}

	public character_overview(byte[] buffer)
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
				general = deserialize.read_obj<general>();
				break;
			case 2:
				attribute_other = deserialize.read_obj<attribute_overview>();
				break;
			case 3:
				visual = deserialize.read_obj<characterVisual>();
				break;
			case 4:
				createtime = deserialize.read_integer();
				break;
			case 5:
				forbidden = deserialize.read_integer();
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
			serialize.write_obj(general, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_obj(attribute_other, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_obj(visual, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(createtime, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(forbidden, 5);
		}
		return serialize.close();
	}
}
