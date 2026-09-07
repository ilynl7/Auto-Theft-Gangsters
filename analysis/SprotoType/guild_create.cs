using Sproto;

namespace SprotoType;

public class guild_create
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private string _guildName;

		private long _Icon;

		private string _notice;

		private long _costType;

		public string guildName
		{
			get
			{
				return _guildName;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_guildName = value;
			}
		}

		public bool HasGuildName => has_field.has_field(0);

		public long Icon
		{
			get
			{
				return _Icon;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_Icon = value;
			}
		}

		public bool HasIcon => has_field.has_field(1);

		public string notice
		{
			get
			{
				return _notice;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_notice = value;
			}
		}

		public bool HasNotice => has_field.has_field(2);

		public long costType
		{
			get
			{
				return _costType;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_costType = value;
			}
		}

		public bool HasCostType => has_field.has_field(3);

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
					guildName = deserialize.read_string();
					break;
				case 1:
					Icon = deserialize.read_integer();
					break;
				case 2:
					notice = deserialize.read_string();
					break;
				case 3:
					costType = deserialize.read_integer();
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
				serialize.write_string(guildName, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(Icon, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(notice, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(costType, 3);
			}
			return serialize.close();
		}
	}
}
