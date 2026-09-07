using Sproto;

namespace SprotoType;

public class ret_accept_mission
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private string _missionId;

		private long _missionquality;

		private long _ret;

		private ownmission _mission;

		public string missionId
		{
			get
			{
				return _missionId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_missionId = value;
			}
		}

		public bool HasMissionId => has_field.has_field(0);

		public long missionquality
		{
			get
			{
				return _missionquality;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_missionquality = value;
			}
		}

		public bool HasMissionquality => has_field.has_field(1);

		public long ret
		{
			get
			{
				return _ret;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_ret = value;
			}
		}

		public bool HasRet => has_field.has_field(2);

		public ownmission mission
		{
			get
			{
				return _mission;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_mission = value;
			}
		}

		public bool HasMission => has_field.has_field(3);

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
					missionId = deserialize.read_string();
					break;
				case 1:
					missionquality = deserialize.read_integer();
					break;
				case 2:
					ret = deserialize.read_integer();
					break;
				case 3:
					mission = deserialize.read_obj<ownmission>();
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
				serialize.write_string(missionId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(missionquality, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(ret, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_obj(mission, 3);
			}
			return serialize.close();
		}
	}
}
