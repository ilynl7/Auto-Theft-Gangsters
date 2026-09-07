using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class character_look : SprotoTypeBase
{
	private static int max_field_count = 11;

	private long _id;

	private general _general;

	private attribute _attribute;

	private attribute_other _attribute_other;

	private characterVisual _visual;

	private Dictionary<long, gameitem> _equip;

	private Dictionary<long, gameitem> _fashion_equip;

	private Dictionary<long, gameitem> _badge_equip;

	private Dictionary<long, enhance_info> _equip_enhance;

	private movement _movement;

	private Dictionary<string, skill_info> _skills;

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

	public attribute attribute
	{
		get
		{
			return _attribute;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_attribute = value;
		}
	}

	public bool HasAttribute => has_field.has_field(2);

	public attribute_other attribute_other
	{
		get
		{
			return _attribute_other;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_attribute_other = value;
		}
	}

	public bool HasAttribute_other => has_field.has_field(3);

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

	public Dictionary<long, gameitem> equip
	{
		get
		{
			return _equip;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_equip = value;
		}
	}

	public bool HasEquip => has_field.has_field(5);

	public Dictionary<long, gameitem> fashion_equip
	{
		get
		{
			return _fashion_equip;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_fashion_equip = value;
		}
	}

	public bool HasFashion_equip => has_field.has_field(6);

	public Dictionary<long, gameitem> badge_equip
	{
		get
		{
			return _badge_equip;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_badge_equip = value;
		}
	}

	public bool HasBadge_equip => has_field.has_field(7);

	public Dictionary<long, enhance_info> equip_enhance
	{
		get
		{
			return _equip_enhance;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_equip_enhance = value;
		}
	}

	public bool HasEquip_enhance => has_field.has_field(8);

	public movement movement
	{
		get
		{
			return _movement;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_movement = value;
		}
	}

	public bool HasMovement => has_field.has_field(9);

	public Dictionary<string, skill_info> skills
	{
		get
		{
			return _skills;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_skills = value;
		}
	}

	public bool HasSkills => has_field.has_field(10);

	public character_look()
		: base(max_field_count)
	{
	}

	public character_look(byte[] buffer)
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
				attribute = deserialize.read_obj<attribute>();
				break;
			case 3:
				attribute_other = deserialize.read_obj<attribute_other>();
				break;
			case 4:
				visual = deserialize.read_obj<characterVisual>();
				break;
			case 5:
				equip = deserialize.read_map((gameitem v) => v.indexId);
				break;
			case 6:
				fashion_equip = deserialize.read_map((gameitem v) => v.indexId);
				break;
			case 7:
				badge_equip = deserialize.read_map((gameitem v) => v.indexId);
				break;
			case 8:
				equip_enhance = deserialize.read_map((enhance_info v) => v.subType);
				break;
			case 9:
				movement = deserialize.read_obj<movement>();
				break;
			case 10:
				skills = deserialize.read_map((skill_info v) => v.skillId);
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
			serialize.write_obj(attribute, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_obj(attribute_other, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_obj(visual, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_obj(equip, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_obj(fashion_equip, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_obj(badge_equip, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_obj(equip_enhance, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_obj(movement, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_obj(skills, 10);
		}
		return serialize.close();
	}
}
