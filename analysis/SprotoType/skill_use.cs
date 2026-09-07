using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class skill_use
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 5;

		private long _targetId;

		private string _skillId;

		private bool _combo;

		private List<attack_list> _attack_list;

		private long _parm;

		public long targetId
		{
			get
			{
				return _targetId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_targetId = value;
			}
		}

		public bool HasTargetId => has_field.has_field(0);

		public string skillId
		{
			get
			{
				return _skillId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_skillId = value;
			}
		}

		public bool HasSkillId => has_field.has_field(1);

		public bool combo
		{
			get
			{
				return _combo;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_combo = value;
			}
		}

		public bool HasCombo => has_field.has_field(2);

		public List<attack_list> attack_list
		{
			get
			{
				return _attack_list;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_attack_list = value;
			}
		}

		public bool HasAttack_list => has_field.has_field(3);

		public long parm
		{
			get
			{
				return _parm;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_parm = value;
			}
		}

		public bool HasParm => has_field.has_field(4);

		public request()
			: base(max_field_count)
		{
		}

		public request(byte[] buffer)
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
					targetId = deserialize.read_integer();
					break;
				case 1:
					skillId = deserialize.read_string();
					break;
				case 2:
					combo = deserialize.read_boolean();
					break;
				case 3:
					attack_list = deserialize.read_obj_list<attack_list>();
					break;
				case 4:
					parm = deserialize.read_integer();
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
				serialize.write_integer(targetId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(skillId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_boolean(combo, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_obj(attack_list, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(parm, 4);
			}
			return serialize.close();
		}
	}
}
