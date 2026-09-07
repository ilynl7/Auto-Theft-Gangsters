using Sproto;

namespace SprotoType;

public class wild_boss : SprotoTypeBase
{
	private static int max_field_count = 9;

	private long _bossId;

	private long _state;

	private long _refreshTime;

	private long _killId;

	private string _killName;

	private string _mapInfoId;

	private long _lineIndex;

	private long _posx;

	private long _posz;

	public long bossId
	{
		get
		{
			return _bossId;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_bossId = value;
		}
	}

	public bool HasBossId => has_field.has_field(0);

	public long state
	{
		get
		{
			return _state;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_state = value;
		}
	}

	public bool HasState => has_field.has_field(1);

	public long refreshTime
	{
		get
		{
			return _refreshTime;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_refreshTime = value;
		}
	}

	public bool HasRefreshTime => has_field.has_field(2);

	public long killId
	{
		get
		{
			return _killId;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_killId = value;
		}
	}

	public bool HasKillId => has_field.has_field(3);

	public string killName
	{
		get
		{
			return _killName;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_killName = value;
		}
	}

	public bool HasKillName => has_field.has_field(4);

	public string mapInfoId
	{
		get
		{
			return _mapInfoId;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_mapInfoId = value;
		}
	}

	public bool HasMapInfoId => has_field.has_field(5);

	public long lineIndex
	{
		get
		{
			return _lineIndex;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_lineIndex = value;
		}
	}

	public bool HasLineIndex => has_field.has_field(6);

	public long posx
	{
		get
		{
			return _posx;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_posx = value;
		}
	}

	public bool HasPosx => has_field.has_field(7);

	public long posz
	{
		get
		{
			return _posz;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_posz = value;
		}
	}

	public bool HasPosz => has_field.has_field(8);

	public wild_boss()
		: base(max_field_count)
	{
	}

	public wild_boss(byte[] buffer)
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
				bossId = deserialize.read_integer();
				break;
			case 1:
				state = deserialize.read_integer();
				break;
			case 2:
				refreshTime = deserialize.read_integer();
				break;
			case 3:
				killId = deserialize.read_integer();
				break;
			case 4:
				killName = deserialize.read_string();
				break;
			case 5:
				mapInfoId = deserialize.read_string();
				break;
			case 6:
				lineIndex = deserialize.read_integer();
				break;
			case 7:
				posx = deserialize.read_integer();
				break;
			case 8:
				posz = deserialize.read_integer();
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
			serialize.write_integer(bossId, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(state, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(refreshTime, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(killId, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_string(killName, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_string(mapInfoId, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(lineIndex, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(posx, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(posz, 8);
		}
		return serialize.close();
	}
}
