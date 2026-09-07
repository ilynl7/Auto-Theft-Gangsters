using Sproto;

namespace SprotoType;

public class tower_floor : SprotoTypeBase
{
	private static int max_field_count = 2;

	private long _floorID;

	private long _complete;

	public long floorID
	{
		get
		{
			return _floorID;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_floorID = value;
		}
	}

	public bool HasFloorID => has_field.has_field(0);

	public long complete
	{
		get
		{
			return _complete;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_complete = value;
		}
	}

	public bool HasComplete => has_field.has_field(1);

	public tower_floor()
		: base(max_field_count)
	{
	}

	public tower_floor(byte[] buffer)
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
				floorID = deserialize.read_integer();
				break;
			case 1:
				complete = deserialize.read_integer();
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
			serialize.write_integer(floorID, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(complete, 1);
		}
		return serialize.close();
	}
}
