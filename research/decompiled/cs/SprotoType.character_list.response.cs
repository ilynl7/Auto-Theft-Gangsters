using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class character_list
{
	public class response : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<long, character_overview> _character;

		public Dictionary<long, character_overview> character
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

		public response()
			: base(max_field_count)
		{
		}

		public response(byte[] buffer)
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
					character = deserialize.read_map((character_overview v) => v.id);
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
