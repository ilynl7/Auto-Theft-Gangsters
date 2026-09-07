using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class sync_mission
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private Dictionary<string, ownmission> _missions;

		private string _last_missionId;

		private List<long> _sidedone_mission;

		public Dictionary<string, ownmission> missions
		{
			get
			{
				return _missions;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_missions = value;
			}
		}

		public bool HasMissions => has_field.has_field(0);

		public string last_missionId
		{
			get
			{
				return _last_missionId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_last_missionId = value;
			}
		}

		public bool HasLast_missionId => has_field.has_field(1);

		public List<long> sidedone_mission
		{
			get
			{
				return _sidedone_mission;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_sidedone_mission = value;
			}
		}

		public bool HasSidedone_mission => has_field.has_field(2);

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
					missions = deserialize.read_map((ownmission v) => v.missionId);
					break;
				case 1:
					last_missionId = deserialize.read_string();
					break;
				case 2:
					sidedone_mission = deserialize.read_integer_list();
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
				serialize.write_obj(missions, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(last_missionId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(sidedone_mission, 2);
			}
			return serialize.close();
		}
	}
}
