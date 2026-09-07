using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class team : SprotoTypeBase
{
	private static int max_field_count = 9;

	private long _id;

	private teammember _teamleader;

	private long _count;

	private long _isVerfiy;

	private Dictionary<long, teammember> _teammembers;

	private string _goalId;

	private long _minLevel;

	private long _maxLevel;

	private long _recruit;

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

	public teammember teamleader
	{
		get
		{
			return _teamleader;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_teamleader = value;
		}
	}

	public bool HasTeamleader => has_field.has_field(1);

	public long count
	{
		get
		{
			return _count;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_count = value;
		}
	}

	public bool HasCount => has_field.has_field(2);

	public long isVerfiy
	{
		get
		{
			return _isVerfiy;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_isVerfiy = value;
		}
	}

	public bool HasIsVerfiy => has_field.has_field(3);

	public Dictionary<long, teammember> teammembers
	{
		get
		{
			return _teammembers;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_teammembers = value;
		}
	}

	public bool HasTeammembers => has_field.has_field(4);

	public string goalId
	{
		get
		{
			return _goalId;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_goalId = value;
		}
	}

	public bool HasGoalId => has_field.has_field(5);

	public long minLevel
	{
		get
		{
			return _minLevel;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_minLevel = value;
		}
	}

	public bool HasMinLevel => has_field.has_field(6);

	public long maxLevel
	{
		get
		{
			return _maxLevel;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_maxLevel = value;
		}
	}

	public bool HasMaxLevel => has_field.has_field(7);

	public long recruit
	{
		get
		{
			return _recruit;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_recruit = value;
		}
	}

	public bool HasRecruit => has_field.has_field(8);

	public team()
		: base(max_field_count)
	{
	}

	public team(byte[] buffer)
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
				teamleader = deserialize.read_obj<teammember>();
				break;
			case 2:
				count = deserialize.read_integer();
				break;
			case 3:
				isVerfiy = deserialize.read_integer();
				break;
			case 4:
				teammembers = deserialize.read_map((teammember v) => v.id);
				break;
			case 5:
				goalId = deserialize.read_string();
				break;
			case 6:
				minLevel = deserialize.read_integer();
				break;
			case 7:
				maxLevel = deserialize.read_integer();
				break;
			case 8:
				recruit = deserialize.read_integer();
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
			serialize.write_obj(teamleader, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(count, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(isVerfiy, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_obj(teammembers, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_string(goalId, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(minLevel, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(maxLevel, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(recruit, 8);
		}
		return serialize.close();
	}
}
