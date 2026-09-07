using Sproto;

namespace SprotoType;

public class skill_level_up
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private string _skillId;

		private long _curLevel;

		private long _all;

		public string skillId
		{
			get
			{
				return _skillId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_skillId = value;
			}
		}

		public bool HasSkillId => has_field.has_field(0);

		public long curLevel
		{
			get
			{
				return _curLevel;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_curLevel = value;
			}
		}

		public bool HasCurLevel => has_field.has_field(1);

		public long all
		{
			get
			{
				return _all;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_all = value;
			}
		}

		public bool HasAll => has_field.has_field(2);

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
					skillId = deserialize.read_string();
					break;
				case 1:
					curLevel = deserialize.read_integer();
					break;
				case 2:
					all = deserialize.read_integer();
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
				serialize.write_string(skillId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(curLevel, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(all, 2);
			}
			return serialize.close();
		}
	}
}
