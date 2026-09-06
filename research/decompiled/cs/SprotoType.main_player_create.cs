using Sproto;

namespace SprotoType;

public class main_player_create
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private character _character;

		private movement _movement;

		public character character
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

		public movement movement
		{
			get
			{
				return _movement;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_movement = value;
			}
		}

		public bool HasMovement => has_field.has_field(1);

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
					character = deserialize.read_obj<character>();
					break;
				case 1:
					movement = deserialize.read_obj<movement>();
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
				serialize.write_obj(character, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(movement, 1);
			}
			return serialize.close();
		}
	}
}
