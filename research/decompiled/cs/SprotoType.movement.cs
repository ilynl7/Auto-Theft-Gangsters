using Sproto;

namespace SprotoType;

public class movement : SprotoTypeBase
{
	private static int max_field_count = 2;

	private position _pos;

	private position _pos2;

	public position pos
	{
		get
		{
			return _pos;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_pos = value;
		}
	}

	public bool HasPos => has_field.has_field(0);

	public position pos2
	{
		get
		{
			return _pos2;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_pos2 = value;
		}
	}

	public bool HasPos2 => has_field.has_field(1);

	public movement()
		: base(max_field_count)
	{
	}

	public movement(byte[] buffer)
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
				pos = deserialize.read_obj<position>();
				break;
			case 1:
				pos2 = deserialize.read_obj<position>();
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
			serialize.write_obj(pos, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_obj(pos2, 1);
		}
		return serialize.close();
	}
}
