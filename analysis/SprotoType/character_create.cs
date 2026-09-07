using Sproto;

namespace SprotoType;

public class character_create
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private general _character;

		public general character
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
					character = deserialize.read_obj<general>();
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

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private character_overview _character;

		private long _errno;

		public character_overview character
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

		public long errno
		{
			get
			{
				return _errno;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_errno = value;
			}
		}

		public bool HasErrno => has_field.has_field(1);

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
					character = deserialize.read_obj<character_overview>();
					break;
				case 1:
					errno = deserialize.read_integer();
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
				serialize.write_integer(errno, 1);
			}
			return serialize.close();
		}
	}
}
