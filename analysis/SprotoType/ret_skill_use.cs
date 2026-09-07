using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_skill_use
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _sendderId;

		private long _targetId;

		private string _skillId;

		private List<attack_list> _attack_list;

		public long sendderId
		{
			get
			{
				return _sendderId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_sendderId = value;
			}
		}

		public bool HasSendderId => has_field.has_field(0);

		public long targetId
		{
			get
			{
				return _targetId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_targetId = value;
			}
		}

		public bool HasTargetId => has_field.has_field(1);

		public string skillId
		{
			get
			{
				return _skillId;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_skillId = value;
			}
		}

		public bool HasSkillId => has_field.has_field(2);

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
					sendderId = deserialize.read_integer();
					break;
				case 1:
					targetId = deserialize.read_integer();
					break;
				case 2:
					skillId = deserialize.read_string();
					break;
				case 3:
					attack_list = deserialize.read_obj_list<attack_list>();
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
				serialize.write_integer(sendderId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(targetId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(skillId, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_obj(attack_list, 3);
			}
			return serialize.close();
		}
	}
}
