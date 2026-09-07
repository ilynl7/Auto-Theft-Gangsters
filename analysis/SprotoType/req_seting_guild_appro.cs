using Sproto;

namespace SprotoType;

public class req_seting_guild_appro
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _guildId;

		private bool _isNeedAppro;

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

		public bool isNeedAppro
		{
			get
			{
				return _isNeedAppro;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_isNeedAppro = value;
			}
		}

		public bool HasIsNeedAppro => has_field.has_field(1);

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
					isNeedAppro = deserialize.read_boolean();
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
				serialize.write_boolean(isNeedAppro, 1);
			}
			return serialize.close();
		}
	}
}
