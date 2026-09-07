using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_domin_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private Dictionary<string, domin_info> _domin_infos;

		private Dictionary<long, character_look> _characters;

		public Dictionary<string, domin_info> domin_infos
		{
			get
			{
				return _domin_infos;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_domin_infos = value;
			}
		}

		public bool HasDomin_infos => has_field.has_field(0);

		public Dictionary<long, character_look> characters
		{
			get
			{
				return _characters;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_characters = value;
			}
		}

		public bool HasCharacters => has_field.has_field(1);

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
					domin_infos = deserialize.read_map((domin_info v) => v.id);
					break;
				case 1:
					characters = deserialize.read_map((character_look v) => v.id);
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
				serialize.write_obj(domin_infos, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(characters, 1);
			}
			return serialize.close();
		}
	}
}
