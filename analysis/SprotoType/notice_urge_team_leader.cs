using Sproto;

namespace SprotoType;

public class notice_urge_team_leader
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private string _name;

		private long _id;

		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_name = value;
			}
		}

		public bool HasName => has_field.has_field(0);

		public long id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(1);

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
					name = deserialize.read_string();
					break;
				case 1:
					id = deserialize.read_integer();
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
				serialize.write_string(name, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(id, 1);
			}
			return serialize.close();
		}
	}
}
