using Sproto;

namespace SprotoType;

public class set_mission_param
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private string _missionId;

		private long _paramindex;

		private long _param;

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

		public long paramindex
		{
			get
			{
				return _paramindex;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_paramindex = value;
			}
		}

		public bool HasParamindex => has_field.has_field(1);

		public long param
		{
			get
			{
				return _param;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_param = value;
			}
		}

		public bool HasParam => has_field.has_field(2);

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
					paramindex = deserialize.read_integer();
					break;
				case 2:
					param = deserialize.read_integer();
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
				serialize.write_integer(paramindex, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(param, 2);
			}
			return serialize.close();
		}
	}
}
