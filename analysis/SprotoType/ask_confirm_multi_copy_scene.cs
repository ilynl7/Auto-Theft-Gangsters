using Sproto;

namespace SprotoType;

public class ask_confirm_multi_copy_scene
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private string _id;

		private long _type1;

		private long _session;

		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(0);

		public long type1
		{
			get
			{
				return _type1;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_type1 = value;
			}
		}

		public bool HasType1 => has_field.has_field(1);

		public long session
		{
			get
			{
				return _session;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_session = value;
			}
		}

		public bool HasSession => has_field.has_field(2);

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
					id = deserialize.read_string();
					break;
				case 1:
					type1 = deserialize.read_integer();
					break;
				case 2:
					session = deserialize.read_integer();
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
				serialize.write_string(id, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(type1, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(session, 2);
			}
			return serialize.close();
		}
	}
}
