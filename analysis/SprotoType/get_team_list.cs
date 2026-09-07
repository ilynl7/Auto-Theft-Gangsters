using Sproto;

namespace SprotoType;

public class get_team_list
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private string _goalId;

		public string goalId
		{
			get
			{
				return _goalId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_goalId = value;
			}
		}

		public bool HasGoalId => has_field.has_field(0);

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
					goalId = deserialize.read_string();
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
				serialize.write_string(goalId, 0);
			}
			return serialize.close();
		}
	}
}
