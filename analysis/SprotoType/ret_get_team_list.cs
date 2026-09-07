using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_get_team_list
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<long, team> _teams;

		public Dictionary<long, team> teams
		{
			get
			{
				return _teams;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_teams = value;
			}
		}

		public bool HasTeams => has_field.has_field(0);

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
					teams = deserialize.read_map((team v) => v.id);
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
				serialize.write_obj(teams, 0);
			}
			return serialize.close();
		}
	}
}
