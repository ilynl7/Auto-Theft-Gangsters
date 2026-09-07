using Sproto;

namespace SprotoType;

public class runtime_agent : SprotoTypeBase
{
	private static int max_field_count = 3;

	private attribute _attribute;

	private attribute _attribute_all;

	public attribute attribute
	{
		get
		{
			return _attribute;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_attribute = value;
		}
	}

	public bool HasAttribute => has_field.has_field(0);

	public attribute attribute_all
	{
		get
		{
			return _attribute_all;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_attribute_all = value;
		}
	}

	public bool HasAttribute_all => has_field.has_field(1);

	public runtime_agent()
		: base(max_field_count)
	{
	}

	public runtime_agent(byte[] buffer)
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
			case 6:
				attribute = deserialize.read_obj<attribute>();
				break;
			case 7:
				attribute_all = deserialize.read_obj<attribute>();
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
			serialize.write_obj(attribute, 6);
		}
		if (has_field.has_field(1))
		{
			serialize.write_obj(attribute_all, 7);
		}
		return serialize.close();
	}
}
