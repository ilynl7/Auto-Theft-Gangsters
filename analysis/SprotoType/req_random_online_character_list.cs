using Sproto;

namespace SprotoType;

public class req_random_online_character_list
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private long _characterId;

		public long characterId
		{
			get
			{
				return _characterId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_characterId = value;
			}
		}

		public bool HasCharacterId => has_field.has_field(0);

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
					characterId = deserialize.read_integer();
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
				serialize.write_integer(characterId, 0);
			}
			return serialize.close();
		}
	}
}
