using Sproto;

namespace SprotoType;

public class ret_guild_leave
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _guildId;

		private long _dismissTime;

		public long guildId
		{
			get
			{
				return _guildId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_guildId = value;
			}
		}

		public bool HasGuildId => has_field.has_field(0);

		public long dismissTime
		{
			get
			{
				return _dismissTime;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_dismissTime = value;
			}
		}

		public bool HasDismissTime => has_field.has_field(1);

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
					guildId = deserialize.read_integer();
					break;
				case 1:
					dismissTime = deserialize.read_integer();
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
				serialize.write_integer(guildId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(dismissTime, 1);
			}
			return serialize.close();
		}
	}
}
