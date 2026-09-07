using Sproto;

namespace SprotoType;

public class update_team_member
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _teamid;

		private teammember _member;

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

		public teammember member
		{
			get
			{
				return _member;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_member = value;
			}
		}

		public bool HasMember => has_field.has_field(1);

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
					member = deserialize.read_obj<teammember>();
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
				serialize.write_obj(member, 1);
			}
			return serialize.close();
		}
	}
}
