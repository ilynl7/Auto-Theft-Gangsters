using Sproto;

namespace SprotoType;

public class guild_info : SprotoTypeBase
{
	private static int max_field_count = 23;

	private long _guildId;

	private string _guildName;

	private string _guildChiefName;

	private long _guildChiefId;

	private long _guildExp;

	private long _guildSkillPoint;

	private long _guildLevel;

	private long _guildMemberNum;

	private long _guildCombo;

	private long _guildApplyNum;

	private long _guildApplyMaxNum;

	private long _guildMaxPlayer;

	private string _notice;

	private bool _isNeedAppro;

	private long _createTime;

	private bool _guildBoss;

	private bool _guildBattle;

	private long _viceNum;

	private long _elderNum;

	private long _playerJob;

	private long _icon;

	private long _disactiveState;

	public long guildId
	{
		get
		{
			return _guildId;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_guildId = value;
		}
	}

	public bool HasGuildId => has_field.has_field(0);

	public string guildName
	{
		get
		{
			return _guildName;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_guildName = value;
		}
	}

	public bool HasGuildName => has_field.has_field(1);

	public string guildChiefName
	{
		get
		{
			return _guildChiefName;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_guildChiefName = value;
		}
	}

	public bool HasGuildChiefName => has_field.has_field(2);

	public long guildChiefId
	{
		get
		{
			return _guildChiefId;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_guildChiefId = value;
		}
	}

	public bool HasGuildChiefId => has_field.has_field(3);

	public long guildExp
	{
		get
		{
			return _guildExp;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_guildExp = value;
		}
	}

	public bool HasGuildExp => has_field.has_field(4);

	public long guildSkillPoint
	{
		get
		{
			return _guildSkillPoint;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_guildSkillPoint = value;
		}
	}

	public bool HasGuildSkillPoint => has_field.has_field(5);

	public long guildLevel
	{
		get
		{
			return _guildLevel;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_guildLevel = value;
		}
	}

	public bool HasGuildLevel => has_field.has_field(6);

	public long guildMemberNum
	{
		get
		{
			return _guildMemberNum;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_guildMemberNum = value;
		}
	}

	public bool HasGuildMemberNum => has_field.has_field(7);

	public long guildCombo
	{
		get
		{
			return _guildCombo;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_guildCombo = value;
		}
	}

	public bool HasGuildCombo => has_field.has_field(8);

	public long guildApplyNum
	{
		get
		{
			return _guildApplyNum;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_guildApplyNum = value;
		}
	}

	public bool HasGuildApplyNum => has_field.has_field(9);

	public long guildApplyMaxNum
	{
		get
		{
			return _guildApplyMaxNum;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_guildApplyMaxNum = value;
		}
	}

	public bool HasGuildApplyMaxNum => has_field.has_field(10);

	public long guildMaxPlayer
	{
		get
		{
			return _guildMaxPlayer;
		}
		set
		{
			has_field.set_field(11, is_has: true);
			_guildMaxPlayer = value;
		}
	}

	public bool HasGuildMaxPlayer => has_field.has_field(11);

	public string notice
	{
		get
		{
			return _notice;
		}
		set
		{
			has_field.set_field(12, is_has: true);
			_notice = value;
		}
	}

	public bool HasNotice => has_field.has_field(12);

	public bool isNeedAppro
	{
		get
		{
			return _isNeedAppro;
		}
		set
		{
			has_field.set_field(13, is_has: true);
			_isNeedAppro = value;
		}
	}

	public bool HasIsNeedAppro => has_field.has_field(13);

	public long createTime
	{
		get
		{
			return _createTime;
		}
		set
		{
			has_field.set_field(14, is_has: true);
			_createTime = value;
		}
	}

	public bool HasCreateTime => has_field.has_field(14);

	public bool guildBoss
	{
		get
		{
			return _guildBoss;
		}
		set
		{
			has_field.set_field(15, is_has: true);
			_guildBoss = value;
		}
	}

	public bool HasGuildBoss => has_field.has_field(15);

	public bool guildBattle
	{
		get
		{
			return _guildBattle;
		}
		set
		{
			has_field.set_field(16, is_has: true);
			_guildBattle = value;
		}
	}

	public bool HasGuildBattle => has_field.has_field(16);

	public long viceNum
	{
		get
		{
			return _viceNum;
		}
		set
		{
			has_field.set_field(17, is_has: true);
			_viceNum = value;
		}
	}

	public bool HasViceNum => has_field.has_field(17);

	public long elderNum
	{
		get
		{
			return _elderNum;
		}
		set
		{
			has_field.set_field(18, is_has: true);
			_elderNum = value;
		}
	}

	public bool HasElderNum => has_field.has_field(18);

	public long playerJob
	{
		get
		{
			return _playerJob;
		}
		set
		{
			has_field.set_field(19, is_has: true);
			_playerJob = value;
		}
	}

	public bool HasPlayerJob => has_field.has_field(19);

	public long icon
	{
		get
		{
			return _icon;
		}
		set
		{
			has_field.set_field(20, is_has: true);
			_icon = value;
		}
	}

	public bool HasIcon => has_field.has_field(20);

	public long disactiveState
	{
		get
		{
			return _disactiveState;
		}
		set
		{
			has_field.set_field(21, is_has: true);
			_disactiveState = value;
		}
	}

	public bool HasDisactiveState => has_field.has_field(21);

	public guild_info()
		: base(max_field_count)
	{
	}

	public guild_info(byte[] buffer)
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
				guildId = deserialize.read_integer();
				break;
			case 1:
				guildName = deserialize.read_string();
				break;
			case 2:
				guildChiefName = deserialize.read_string();
				break;
			case 3:
				guildChiefId = deserialize.read_integer();
				break;
			case 4:
				guildExp = deserialize.read_integer();
				break;
			case 5:
				guildSkillPoint = deserialize.read_integer();
				break;
			case 6:
				guildLevel = deserialize.read_integer();
				break;
			case 7:
				guildMemberNum = deserialize.read_integer();
				break;
			case 8:
				guildCombo = deserialize.read_integer();
				break;
			case 9:
				guildApplyNum = deserialize.read_integer();
				break;
			case 10:
				guildApplyMaxNum = deserialize.read_integer();
				break;
			case 11:
				guildMaxPlayer = deserialize.read_integer();
				break;
			case 12:
				notice = deserialize.read_string();
				break;
			case 13:
				isNeedAppro = deserialize.read_boolean();
				break;
			case 14:
				createTime = deserialize.read_integer();
				break;
			case 16:
				guildBoss = deserialize.read_boolean();
				break;
			case 17:
				guildBattle = deserialize.read_boolean();
				break;
			case 18:
				viceNum = deserialize.read_integer();
				break;
			case 19:
				elderNum = deserialize.read_integer();
				break;
			case 20:
				playerJob = deserialize.read_integer();
				break;
			case 21:
				icon = deserialize.read_integer();
				break;
			case 22:
				disactiveState = deserialize.read_integer();
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
			serialize.write_integer(guildId, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_string(guildName, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_string(guildChiefName, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(guildChiefId, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(guildExp, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(guildSkillPoint, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(guildLevel, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(guildMemberNum, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(guildCombo, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(guildApplyNum, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(guildApplyMaxNum, 10);
		}
		if (has_field.has_field(11))
		{
			serialize.write_integer(guildMaxPlayer, 11);
		}
		if (has_field.has_field(12))
		{
			serialize.write_string(notice, 12);
		}
		if (has_field.has_field(13))
		{
			serialize.write_boolean(isNeedAppro, 13);
		}
		if (has_field.has_field(14))
		{
			serialize.write_integer(createTime, 14);
		}
		if (has_field.has_field(15))
		{
			serialize.write_boolean(guildBoss, 16);
		}
		if (has_field.has_field(16))
		{
			serialize.write_boolean(guildBattle, 17);
		}
		if (has_field.has_field(17))
		{
			serialize.write_integer(viceNum, 18);
		}
		if (has_field.has_field(18))
		{
			serialize.write_integer(elderNum, 19);
		}
		if (has_field.has_field(19))
		{
			serialize.write_integer(playerJob, 20);
		}
		if (has_field.has_field(20))
		{
			serialize.write_integer(icon, 21);
		}
		if (has_field.has_field(21))
		{
			serialize.write_integer(disactiveState, 22);
		}
		return serialize.close();
	}
}
