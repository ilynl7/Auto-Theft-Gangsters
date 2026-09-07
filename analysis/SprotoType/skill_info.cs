using Sproto;

namespace SprotoType;

public class skill_info : SprotoTypeBase
{
	private static int max_field_count = 6;

	private string _skillId;

	private long _skillLevel;

	private long _indexPos;

	private long _unlockLevel;

	private long _indexPos2;

	private bool _disable;

	public string skillId
	{
		get
		{
			return _skillId;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_skillId = value;
		}
	}

	public bool HasSkillId => has_field.has_field(0);

	public long skillLevel
	{
		get
		{
			return _skillLevel;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_skillLevel = value;
		}
	}

	public bool HasSkillLevel => has_field.has_field(1);

	public long indexPos
	{
		get
		{
			return _indexPos;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_indexPos = value;
		}
	}

	public bool HasIndexPos => has_field.has_field(2);

	public long unlockLevel
	{
		get
		{
			return _unlockLevel;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_unlockLevel = value;
		}
	}

	public bool HasUnlockLevel => has_field.has_field(3);

	public long indexPos2
	{
		get
		{
			return _indexPos2;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_indexPos2 = value;
		}
	}

	public bool HasIndexPos2 => has_field.has_field(4);

	public bool disable
	{
		get
		{
			return _disable;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_disable = value;
		}
	}

	public bool HasDisable => has_field.has_field(5);

	public skill_info()
		: base(max_field_count)
	{
	}

	public skill_info(byte[] buffer)
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
				skillId = deserialize.read_string();
				break;
			case 1:
				skillLevel = deserialize.read_integer();
				break;
			case 2:
				indexPos = deserialize.read_integer();
				break;
			case 3:
				unlockLevel = deserialize.read_integer();
				break;
			case 4:
				indexPos2 = deserialize.read_integer();
				break;
			case 5:
				disable = deserialize.read_boolean();
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
			serialize.write_string(skillId, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(skillLevel, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(indexPos, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(unlockLevel, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(indexPos2, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_boolean(disable, 5);
		}
		return serialize.close();
	}
}
