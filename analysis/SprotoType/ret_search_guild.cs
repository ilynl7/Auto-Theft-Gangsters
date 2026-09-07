using Sproto;

namespace SprotoType;

public class ret_search_guild
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private guild_info _guild;

		private long _rank;

		public guild_info guild
		{
			get
			{
				return _guild;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_guild = value;
			}
		}

		public bool HasGuild => has_field.has_field(0);

		public long rank
		{
			get
			{
				return _rank;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_rank = value;
			}
		}

		public bool HasRank => has_field.has_field(1);

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
					guild = deserialize.read_obj<guild_info>();
					break;
				case 1:
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
				serialize.write_obj(guild, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(rank, 1);
			}
			return serialize.close();
		}
	}
}
