using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class sync_skill_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private Dictionary<string, skill_info> _skill_dict;

		private bool _isLevelUp;

		public Dictionary<string, skill_info> skill_dict
		{
			get
			{
				return _skill_dict;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_skill_dict = value;
			}
		}

		public bool HasSkill_dict => has_field.has_field(0);

		public bool isLevelUp
		{
			get
			{
				return _isLevelUp;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_isLevelUp = value;
			}
		}

		public bool HasIsLevelUp => has_field.has_field(1);

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
					skill_dict = deserialize.read_map((skill_info v) => v.skillId);
					break;
				case 1:
					isLevelUp = deserialize.read_boolean();
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
				serialize.write_obj(skill_dict, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_boolean(isLevelUp, 1);
			}
			return serialize.close();
		}
	}

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private bool _isLevelUp;

		public bool isLevelUp
		{
			get
			{
				return _isLevelUp;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_isLevelUp = value;
			}
		}

		public bool HasIsLevelUp => has_field.has_field(0);

		public response()
			: base(max_field_count)
		{
		}

		public response(byte[] buffer)
			: base(max_field_count, buffer)
		{
			decode();
		}

		protected override void decode()
		{
			int num = -1;
			while ((num = deserialize.read_tag()) != -1)
			{
				if (num == 0)
				{
					isLevelUp = deserialize.read_boolean();
				}
				else
				{
					deserialize.read_unknow_data();
				}
			}
		}

		public override int encode(SprotoStream stream)
		{
			serialize.open(stream);
			if (has_field.has_field(0))
			{
				serialize.write_boolean(isLevelUp, 0);
			}
			return serialize.close();
		}
	}
}
