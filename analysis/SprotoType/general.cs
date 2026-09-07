using Sproto;

namespace SprotoType;

public class general : SprotoTypeBase
{
	private static int max_field_count = 5;

	private string _name;

	private long _profession;

	private long _lineIndex;

	private string _mapInfoId;

	private long _tutorial;

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

	public long profession
	{
		get
		{
			return _profession;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_profession = value;
		}
	}

	public bool HasProfession => has_field.has_field(1);

	public long lineIndex
	{
		get
		{
			return _lineIndex;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_lineIndex = value;
		}
	}

	public bool HasLineIndex => has_field.has_field(2);

	public string mapInfoId
	{
		get
		{
			return _mapInfoId;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_mapInfoId = value;
		}
	}

	public bool HasMapInfoId => has_field.has_field(3);

	public long tutorial
	{
		get
		{
			return _tutorial;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_tutorial = value;
		}
	}

	public bool HasTutorial => has_field.has_field(4);

	public general()
		: base(max_field_count)
	{
	}

	public general(byte[] buffer)
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
				profession = deserialize.read_integer();
				break;
			case 2:
				lineIndex = deserialize.read_integer();
				break;
			case 3:
				mapInfoId = deserialize.read_string();
				break;
			case 4:
				tutorial = deserialize.read_integer();
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
			serialize.write_integer(profession, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(lineIndex, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_string(mapInfoId, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(tutorial, 4);
		}
		return serialize.close();
	}
}
