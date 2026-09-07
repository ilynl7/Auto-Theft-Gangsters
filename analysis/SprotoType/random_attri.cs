using Sproto;

namespace SprotoType;

public class random_attri : SprotoTypeBase
{
	private static int max_field_count = 6;

	private long _index;

	private long _id;

	private long _value;

	private long _quality;

	private string _skillId;

	private string _qualityId;

	public long index
	{
		get
		{
			return _index;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_index = value;
		}
	}

	public bool HasIndex => has_field.has_field(0);

	public long id
	{
		get
		{
			return _id;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_id = value;
		}
	}

	public bool HasId => has_field.has_field(1);

	public long value
	{
		get
		{
			return _value;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_value = value;
		}
	}

	public bool HasValue => has_field.has_field(2);

	public long quality
	{
		get
		{
			return _quality;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_quality = value;
		}
	}

	public bool HasQuality => has_field.has_field(3);

	public string skillId
	{
		get
		{
			return _skillId;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_skillId = value;
		}
	}

	public bool HasSkillId => has_field.has_field(4);

	public string qualityId
	{
		get
		{
			return _qualityId;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_qualityId = value;
		}
	}

	public bool HasQualityId => has_field.has_field(5);

	public random_attri()
		: base(max_field_count)
	{
	}

	public random_attri(byte[] buffer)
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
				index = deserialize.read_integer();
				break;
			case 1:
				id = deserialize.read_integer();
				break;
			case 2:
				value = deserialize.read_integer();
				break;
			case 3:
				quality = deserialize.read_integer();
				break;
			case 4:
				skillId = deserialize.read_string();
				break;
			case 5:
				qualityId = deserialize.read_string();
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
			serialize.write_integer(index, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(id, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(value, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(quality, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_string(skillId, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_string(qualityId, 5);
		}
		return serialize.close();
	}
}
