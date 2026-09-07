using Sproto;

namespace SprotoType;

public class update_player_map_info
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

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _state;

		private string _mapid;

		private position _pos;

		public long state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(0);

		public string mapid
		{
			get
			{
				return _mapid;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_mapid = value;
			}
		}

		public bool HasMapid => has_field.has_field(1);

		public position pos
		{
			get
			{
				return _pos;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_pos = value;
			}
		}

		public bool HasPos => has_field.has_field(2);

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
				switch (num)
				{
				case 0:
					state = deserialize.read_integer();
					break;
				case 1:
					mapid = deserialize.read_string();
					break;
				case 2:
					pos = deserialize.read_obj<position>();
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
				serialize.write_integer(state, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(mapid, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_obj(pos, 2);
			}
			return serialize.close();
		}
	}
}
