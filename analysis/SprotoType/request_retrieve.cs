using Sproto;

namespace SprotoType;

public class request_retrieve
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private string _ID;

		private long _Type;

		public string ID
		{
			get
			{
				return _ID;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_ID = value;
			}
		}

		public bool HasID => has_field.has_field(0);

		public long Type
		{
			get
			{
				return _Type;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_Type = value;
			}
		}

		public bool HasType => has_field.has_field(1);

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
					ID = deserialize.read_string();
					break;
				case 1:
					Type = deserialize.read_integer();
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
				serialize.write_string(ID, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(Type, 1);
			}
			return serialize.close();
		}
	}
}
