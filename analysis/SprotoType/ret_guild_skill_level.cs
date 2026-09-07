using Sproto;

namespace SprotoType;

public class ret_guild_skill_level
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _state;

		private long _guildSkillType;

		private long _contribute;

		private long _level;

		public long state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(0);

		public long guildSkillType
		{
			get
			{
				return _guildSkillType;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_guildSkillType = value;
			}
		}

		public bool HasGuildSkillType => has_field.has_field(1);

		public long contribute
		{
			get
			{
				return _contribute;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_contribute = value;
			}
		}

		public bool HasContribute => has_field.has_field(2);

		public long level
		{
			get
			{
				return _level;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_level = value;
			}
		}

		public bool HasLevel => has_field.has_field(3);

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
					state = deserialize.read_integer();
					break;
				case 1:
					guildSkillType = deserialize.read_integer();
					break;
				case 2:
					contribute = deserialize.read_integer();
					break;
				case 3:
					level = deserialize.read_integer();
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
				serialize.write_integer(state, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(guildSkillType, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(contribute, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(level, 3);
			}
			return serialize.close();
		}
	}
}
