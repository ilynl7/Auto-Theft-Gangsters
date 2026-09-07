using Sproto;

namespace SprotoType;

public class search_guild
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private string _name;

		private long _guildId;

		private long _rank;

		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_name = value;
			}
		}

		public bool HasName => has_field.has_field(0);

		public long guildId
		{
			get
			{
				return _guildId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_guildId = value;
			}
		}

		public bool HasGuildId => has_field.has_field(1);

		public long rank
		{
			get
			{
				return _rank;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_rank = value;
			}
		}

		public bool HasRank => has_field.has_field(2);

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
					name = deserialize.read_string();
					break;
				case 1:
					guildId = deserialize.read_integer();
					break;
				case 2:
					rank = deserialize.read_integer();
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
				serialize.write_string(name, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(guildId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(rank, 2);
			}
			return serialize.close();
		}
	}
}
