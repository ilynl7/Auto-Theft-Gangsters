using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class character : SprotoTypeBase
{
	private static int max_field_count = 16;

	private long _id;

	private general _general;

	private attribute_other _attribute_other;

	private property _property;

	private characterVisual _visual;

	private movement _movement;

	private Dictionary<string, skill_info> _skills;

	private Dictionary<long, gameitem> _equip;

	private Dictionary<long, gameitem> _badge_equip;

	private Dictionary<long, gameitem> _fashion_equip;

	private long _potionIndex;

	private runtime_agent _runtime;

	private Dictionary<long, enhance_info> _equip_enhance;

	private long _download;

	private long _skill_index;

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

	public general general
	{
		get
		{
			return _general;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_general = value;
		}
	}

	public bool HasGeneral => has_field.has_field(1);

	public attribute_other attribute_other
	{
		get
		{
			return _attribute_other;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_attribute_other = value;
		}
	}

	public bool HasAttribute_other => has_field.has_field(2);

	public property property
	{
		get
		{
			return _property;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_property = value;
		}
	}

	public bool HasProperty => has_field.has_field(3);

	public characterVisual visual
	{
		get
		{
			return _visual;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_visual = value;
		}
	}

	public bool HasVisual => has_field.has_field(4);

	public movement movement
	{
		get
		{
			return _movement;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_movement = value;
		}
	}

	public bool HasMovement => has_field.has_field(5);

	public Dictionary<string, skill_info> skills
	{
		get
		{
			return _skills;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_skills = value;
		}
	}

	public bool HasSkills => has_field.has_field(6);

	public Dictionary<long, gameitem> equip
	{
		get
		{
			return _equip;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_equip = value;
		}
	}

	public bool HasEquip => has_field.has_field(7);

	public Dictionary<long, gameitem> badge_equip
	{
		get
		{
			return _badge_equip;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_badge_equip = value;
		}
	}

	public bool HasBadge_equip => has_field.has_field(8);

	public Dictionary<long, gameitem> fashion_equip
	{
		get
		{
			return _fashion_equip;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_fashion_equip = value;
		}
	}

	public bool HasFashion_equip => has_field.has_field(9);

	public long potionIndex
	{
		get
		{
			return _potionIndex;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_potionIndex = value;
		}
	}

	public bool HasPotionIndex => has_field.has_field(10);

	public runtime_agent runtime
	{
		get
		{
			return _runtime;
		}
		set
		{
			has_field.set_field(11, is_has: true);
			_runtime = value;
		}
	}

	public bool HasRuntime => has_field.has_field(11);

	public Dictionary<long, enhance_info> equip_enhance
	{
		get
		{
			return _equip_enhance;
		}
		set
		{
			has_field.set_field(12, is_has: true);
			_equip_enhance = value;
		}
	}

	public bool HasEquip_enhance => has_field.has_field(12);

	public long download
	{
		get
		{
			return _download;
		}
		set
		{
			has_field.set_field(13, is_has: true);
			_download = value;
		}
	}

	public bool HasDownload => has_field.has_field(13);

	public long skill_index
	{
		get
		{
			return _skill_index;
		}
		set
		{
			has_field.set_field(14, is_has: true);
			_skill_index = value;
		}
	}

	public bool HasSkill_index => has_field.has_field(14);

	public character()
		: base(max_field_count)
	{
	}

	public character(byte[] buffer)
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
				general = deserialize.read_obj<general>();
				break;
			case 2:
				attribute_other = deserialize.read_obj<attribute_other>();
				break;
			case 5:
				property = deserialize.read_obj<property>();
				break;
			case 6:
				visual = deserialize.read_obj<characterVisual>();
				break;
			case 7:
				movement = deserialize.read_obj<movement>();
				break;
			case 8:
				skills = deserialize.read_map((skill_info v) => v.skillId);
				break;
			case 9:
				equip = deserialize.read_map((gameitem v) => v.indexId);
				break;
			case 10:
				badge_equip = deserialize.read_map((gameitem v) => v.indexId);
				break;
			case 11:
				fashion_equip = deserialize.read_map((gameitem v) => v.indexId);
				break;
			case 12:
				potionIndex = deserialize.read_integer();
				break;
			case 13:
				runtime = deserialize.read_obj<runtime_agent>();
				break;
			case 14:
				equip_enhance = deserialize.read_map((enhance_info v) => v.subType);
				break;
			case 15:
				download = deserialize.read_integer();
				break;
			case 16:
				skill_index = deserialize.read_integer();
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
			serialize.write_obj(general, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_obj(attribute_other, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_obj(property, 5);
		}
		if (has_field.has_field(4))
		{
			serialize.write_obj(visual, 6);
		}
		if (has_field.has_field(5))
		{
			serialize.write_obj(movement, 7);
		}
		if (has_field.has_field(6))
		{
			serialize.write_obj(skills, 8);
		}
		if (has_field.has_field(7))
		{
			serialize.write_obj(equip, 9);
		}
		if (has_field.has_field(8))
		{
			serialize.write_obj(badge_equip, 10);
		}
		if (has_field.has_field(9))
		{
			serialize.write_obj(fashion_equip, 11);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(potionIndex, 12);
		}
		if (has_field.has_field(11))
		{
			serialize.write_obj(runtime, 13);
		}
		if (has_field.has_field(12))
		{
			serialize.write_obj(equip_enhance, 14);
		}
		if (has_field.has_field(13))
		{
			serialize.write_integer(download, 15);
		}
		if (has_field.has_field(14))
		{
			serialize.write_integer(skill_index, 16);
		}
		return serialize.close();
	}
}
