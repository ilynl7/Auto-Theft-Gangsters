using Sproto;

namespace SprotoType;

public class abandon_mission
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private string _missionId;

		private long _parm;

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

		public long parm
		{
			get
			{
				return _parm;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_parm = value;
			}
		}

		public bool HasParm => has_field.has_field(1);

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
					parm = deserialize.read_integer();
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
				serialize.write_integer(parm, 1);
			}
			return serialize.close();
		}
	}
}
