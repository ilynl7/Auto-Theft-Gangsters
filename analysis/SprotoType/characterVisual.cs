using Sproto;

namespace SprotoType;

public class characterVisual : SprotoTypeBase
{
	private static int max_field_count = 17;

	private string _name;

	private string _ModeId;

	private string _HeadId;

	private string _BodyId;

	private string _LegId;

	private string _WeaponId;

	private string _Fashion_HeadId;

	private string _Fashion_BodyId;

	private string _Fashion_LegId;

	private string _Fashion_WeaponId;

	private long _showType;

	private string _MountId;

	private long _mount_state;

	private string _mount_color;

	private string _WeaponItemId;

	private string _FashionItemId;

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

	public string ModeId
	{
		get
		{
			return _ModeId;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_ModeId = value;
		}
	}

	public bool HasModeId => has_field.has_field(1);

	public string HeadId
	{
		get
		{
			return _HeadId;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_HeadId = value;
		}
	}

	public bool HasHeadId => has_field.has_field(2);

	public string BodyId
	{
		get
		{
			return _BodyId;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_BodyId = value;
		}
	}

	public bool HasBodyId => has_field.has_field(3);

	public string LegId
	{
		get
		{
			return _LegId;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_LegId = value;
		}
	}

	public bool HasLegId => has_field.has_field(4);

	public string WeaponId
	{
		get
		{
			return _WeaponId;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_WeaponId = value;
		}
	}

	public bool HasWeaponId => has_field.has_field(5);

	public string Fashion_HeadId
	{
		get
		{
			return _Fashion_HeadId;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_Fashion_HeadId = value;
		}
	}

	public bool HasFashion_HeadId => has_field.has_field(6);

	public string Fashion_BodyId
	{
		get
		{
			return _Fashion_BodyId;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_Fashion_BodyId = value;
		}
	}

	public bool HasFashion_BodyId => has_field.has_field(7);

	public string Fashion_LegId
	{
		get
		{
			return _Fashion_LegId;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_Fashion_LegId = value;
		}
	}

	public bool HasFashion_LegId => has_field.has_field(8);

	public string Fashion_WeaponId
	{
		get
		{
			return _Fashion_WeaponId;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_Fashion_WeaponId = value;
		}
	}

	public bool HasFashion_WeaponId => has_field.has_field(9);

	public long showType
	{
		get
		{
			return _showType;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_showType = value;
		}
	}

	public bool HasShowType => has_field.has_field(10);

	public string MountId
	{
		get
		{
			return _MountId;
		}
		set
		{
			has_field.set_field(11, is_has: true);
			_MountId = value;
		}
	}

	public bool HasMountId => has_field.has_field(11);

	public long mount_state
	{
		get
		{
			return _mount_state;
		}
		set
		{
			has_field.set_field(12, is_has: true);
			_mount_state = value;
		}
	}

	public bool HasMount_state => has_field.has_field(12);

	public string mount_color
	{
		get
		{
			return _mount_color;
		}
		set
		{
			has_field.set_field(13, is_has: true);
			_mount_color = value;
		}
	}

	public bool HasMount_color => has_field.has_field(13);

	public string WeaponItemId
	{
		get
		{
			return _WeaponItemId;
		}
		set
		{
			has_field.set_field(14, is_has: true);
			_WeaponItemId = value;
		}
	}

	public bool HasWeaponItemId => has_field.has_field(14);

	public string FashionItemId
	{
		get
		{
			return _FashionItemId;
		}
		set
		{
			has_field.set_field(15, is_has: true);
			_FashionItemId = value;
		}
	}

	public bool HasFashionItemId => has_field.has_field(15);

	public characterVisual()
		: base(max_field_count)
	{
	}

	public characterVisual(byte[] buffer)
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
				ModeId = deserialize.read_string();
				break;
			case 2:
				HeadId = deserialize.read_string();
				break;
			case 3:
				BodyId = deserialize.read_string();
				break;
			case 4:
				LegId = deserialize.read_string();
				break;
			case 5:
				WeaponId = deserialize.read_string();
				break;
			case 6:
				Fashion_HeadId = deserialize.read_string();
				break;
			case 7:
				Fashion_BodyId = deserialize.read_string();
				break;
			case 8:
				Fashion_LegId = deserialize.read_string();
				break;
			case 9:
				Fashion_WeaponId = deserialize.read_string();
				break;
			case 10:
				showType = deserialize.read_integer();
				break;
			case 12:
				MountId = deserialize.read_string();
				break;
			case 13:
				mount_state = deserialize.read_integer();
				break;
			case 14:
				mount_color = deserialize.read_string();
				break;
			case 15:
				WeaponItemId = deserialize.read_string();
				break;
			case 16:
				FashionItemId = deserialize.read_string();
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
			serialize.write_string(ModeId, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_string(HeadId, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_string(BodyId, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_string(LegId, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_string(WeaponId, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_string(Fashion_HeadId, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_string(Fashion_BodyId, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_string(Fashion_LegId, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_string(Fashion_WeaponId, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(showType, 10);
		}
		if (has_field.has_field(11))
		{
			serialize.write_string(MountId, 12);
		}
		if (has_field.has_field(12))
		{
			serialize.write_integer(mount_state, 13);
		}
		if (has_field.has_field(13))
		{
			serialize.write_string(mount_color, 14);
		}
		if (has_field.has_field(14))
		{
			serialize.write_string(WeaponItemId, 15);
		}
		if (has_field.has_field(15))
		{
			serialize.write_string(FashionItemId, 16);
		}
		return serialize.close();
	}
}
