using Sproto;

namespace SprotoType;

public class req_invite_team_result
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _ok;

		private long _id;

		public long ok
		{
			get
			{
				return _ok;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_ok = value;
			}
		}

		public bool HasOk => has_field.has_field(0);

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
					ok = deserialize.read_integer();
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
				serialize.write_integer(ok, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(id, 1);
			}
			return serialize.close();
		}
	}
}
