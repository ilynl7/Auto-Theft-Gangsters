using Sproto;

namespace SprotoType;

public class single_copy_scene_npc_die
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 5;

		private long _characterId;

		private string _npcdataid;

		private long _pos_x;

		private long _pos_z;

		private long _type;

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

		public string npcdataid
		{
			get
			{
				return _npcdataid;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_npcdataid = value;
			}
		}

		public bool HasNpcdataid => has_field.has_field(1);

		public long pos_x
		{
			get
			{
				return _pos_x;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_pos_x = value;
			}
		}

		public bool HasPos_x => has_field.has_field(2);

		public long pos_z
		{
			get
			{
				return _pos_z;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_pos_z = value;
			}
		}

		public bool HasPos_z => has_field.has_field(3);

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(4);

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
					characterId = deserialize.read_integer();
					break;
				case 1:
					npcdataid = deserialize.read_string();
					break;
				case 2:
					pos_x = deserialize.read_integer();
					break;
				case 3:
					pos_z = deserialize.read_integer();
					break;
				case 4:
					type = deserialize.read_integer();
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
				serialize.write_integer(characterId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(npcdataid, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(pos_x, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(pos_z, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(type, 4);
			}
			return serialize.close();
		}
	}
}
