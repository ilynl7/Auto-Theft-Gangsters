using Sproto;

namespace SprotoType;

public class gather_other_player
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private long _characterid;

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
					characterid = deserialize.read_integer();
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
				serialize.write_integer(characterid, 0);
			}
			return serialize.close();
		}
	}
}
