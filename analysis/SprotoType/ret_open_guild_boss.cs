using Sproto;

namespace SprotoType;

public class ret_open_guild_boss
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private bool _ok;

		private guild_boss _guild_boss;

		public bool ok
		{
			get
			{
				return _ok;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_ok = value;
			}
		}

		public bool HasOk => has_field.has_field(0);

		public guild_boss guild_boss
		{
			get
			{
				return _guild_boss;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_guild_boss = value;
			}
		}

		public bool HasGuild_boss => has_field.has_field(1);

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
					ok = deserialize.read_boolean();
					break;
				case 1:
					guild_boss = deserialize.read_obj<guild_boss>();
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
				serialize.write_boolean(ok, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(guild_boss, 1);
			}
			return serialize.close();
		}
	}
}
