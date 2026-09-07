using Sproto;

namespace SprotoType;

public class guild_battle_finish_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private guild_battle_score_info _guild_battle_score_info;

		public guild_battle_score_info guild_battle_score_info
		{
			get
			{
				return _guild_battle_score_info;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_guild_battle_score_info = value;
			}
		}

		public bool HasGuild_battle_score_info => has_field.has_field(0);

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
				if (num == 0)
				{
					guild_battle_score_info = deserialize.read_obj<guild_battle_score_info>();
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
				serialize.write_obj(guild_battle_score_info, 0);
			}
			return serialize.close();
		}
	}
}
