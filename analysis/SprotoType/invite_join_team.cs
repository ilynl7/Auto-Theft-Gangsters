using Sproto;

namespace SprotoType;

public class invite_join_team
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _teamid;

		private teammember _member;

		private string _goalId;

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

		public string goalId
		{
			get
			{
				return _goalId;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_goalId = value;
			}
		}

		public bool HasGoalId => has_field.has_field(2);

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
				case 2:
					goalId = deserialize.read_string();
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
			if (has_field.has_field(2))
			{
				serialize.write_string(goalId, 2);
			}
			return serialize.close();
		}
	}
}
