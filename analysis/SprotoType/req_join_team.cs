using Sproto;

namespace SprotoType;

public class req_join_team
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _teamid;

		private bool _isapply;

		public long teamid
		{
			get
			{
				return _teamid;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_teamid = value;
			}
		}

		public bool HasTeamid => has_field.has_field(0);

		public bool isapply
		{
			get
			{
				return _isapply;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_isapply = value;
			}
		}

		public bool HasIsapply => has_field.has_field(1);

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
					teamid = deserialize.read_integer();
					break;
				case 1:
					isapply = deserialize.read_boolean();
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
				serialize.write_integer(teamid, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_boolean(isapply, 1);
			}
			return serialize.close();
		}
	}
}
