using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ownmission : SprotoTypeBase
{
	private static int max_field_count = 4;

	private string _missionId;

	private long _missionstate;

	private long _missionquality;

	private List<long> _parm;

	public string missionId
	{
		get
		{
			return _missionId;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_missionId = value;
		}
	}

	public bool HasMissionId => has_field.has_field(0);

	public long missionstate
	{
		get
		{
			return _missionstate;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_missionstate = value;
		}
	}

	public bool HasMissionstate => has_field.has_field(1);

	public long missionquality
	{
		get
		{
			return _missionquality;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_missionquality = value;
		}
	}

	public bool HasMissionquality => has_field.has_field(2);

	public List<long> parm
	{
		get
		{
			return _parm;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_parm = value;
		}
	}

	public bool HasParm => has_field.has_field(3);

	public ownmission()
		: base(max_field_count)
	{
	}

	public ownmission(byte[] buffer)
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
				missionId = deserialize.read_string();
				break;
			case 1:
				missionstate = deserialize.read_integer();
				break;
			case 2:
				missionquality = deserialize.read_integer();
				break;
			case 3:
				parm = deserialize.read_integer_list();
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
			serialize.write_string(missionId, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(missionstate, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(missionquality, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(parm, 3);
		}
		return serialize.close();
	}
}
