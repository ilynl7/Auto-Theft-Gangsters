using Sproto;

namespace SprotoType;

public class guild_skill : SprotoTypeBase
{
	private static int max_field_count = 2;

	private long _skillType;

	private long _level;

	public long skillType
	{
		get
		{
			return _skillType;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_skillType = value;
		}
	}

	public bool HasSkillType => has_field.has_field(0);

	public long level
	{
		get
		{
			return _level;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_level = value;
		}
	}

	public bool HasLevel => has_field.has_field(1);

	public guild_skill()
		: base(max_field_count)
	{
	}

	public guild_skill(byte[] buffer)
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
				skillType = deserialize.read_integer();
				break;
			case 1:
				level = deserialize.read_integer();
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
			serialize.write_integer(skillType, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(level, 1);
		}
		return serialize.close();
	}
}
