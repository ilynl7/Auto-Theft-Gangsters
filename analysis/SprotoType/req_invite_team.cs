using Sproto;

namespace SprotoType;

public class req_invite_team
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 6;

		private long _characterid;

		private string _goalId;

		private long _minLevel;

		private long _maxLevel;

		private long _isVerfiy;

		private long _recruit;

		public long characterid
		{
			get
			{
				return _characterid;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_characterid = value;
			}
		}

		public bool HasCharacterid => has_field.has_field(0);

		public string goalId
		{
			get
			{
				return _goalId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_goalId = value;
			}
		}

		public bool HasGoalId => has_field.has_field(1);

		public long minLevel
		{
			get
			{
				return _minLevel;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_minLevel = value;
			}
		}

		public bool HasMinLevel => has_field.has_field(2);

		public long maxLevel
		{
			get
			{
				return _maxLevel;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_maxLevel = value;
			}
		}

		public bool HasMaxLevel => has_field.has_field(3);

		public long isVerfiy
		{
			get
			{
				return _isVerfiy;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_isVerfiy = value;
			}
		}

		public bool HasIsVerfiy => has_field.has_field(4);

		public long recruit
		{
			get
			{
				return _recruit;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_recruit = value;
			}
		}

		public bool HasRecruit => has_field.has_field(5);

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
					characterid = deserialize.read_integer();
					break;
				case 1:
					goalId = deserialize.read_string();
					break;
				case 2:
					minLevel = deserialize.read_integer();
					break;
				case 3:
					maxLevel = deserialize.read_integer();
					break;
				case 4:
					isVerfiy = deserialize.read_integer();
					break;
				case 5:
					recruit = deserialize.read_integer();
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
				serialize.write_integer(characterid, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(goalId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(minLevel, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(maxLevel, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(isVerfiy, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(recruit, 5);
			}
			return serialize.close();
		}
	}
}
