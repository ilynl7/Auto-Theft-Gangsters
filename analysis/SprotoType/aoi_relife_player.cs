using Sproto;

namespace SprotoType;

public class aoi_relife_player
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private character_relife _character;

		public character_relife character
		{
			get
			{
				return _character;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_character = value;
			}
		}

		public bool HasCharacter => has_field.has_field(0);

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
					character = deserialize.read_obj<character_relife>();
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
				serialize.write_obj(character, 0);
			}
			return serialize.close();
		}
	}
}
