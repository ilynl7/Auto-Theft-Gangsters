using Sproto;

namespace SprotoType;

public class sample_activity_result
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private bool _win;

		private long _type;

		private string _id;

		public bool win
		{
			get
			{
				return _win;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_win = value;
			}
		}

		public bool HasWin => has_field.has_field(0);

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(1);

		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(2);

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
					win = deserialize.read_boolean();
					break;
				case 1:
					type = deserialize.read_integer();
					break;
				case 2:
					id = deserialize.read_string();
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
				serialize.write_boolean(win, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(type, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(id, 2);
			}
			return serialize.close();
		}
	}
}
