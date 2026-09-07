using Sproto;

namespace SprotoType;

public class update_misison_parm
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private string _missionId;

		private long _paramType;

		private long _paramValue;

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

		public long paramType
		{
			get
			{
				return _paramType;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_paramType = value;
			}
		}

		public bool HasParamType => has_field.has_field(1);

		public long paramValue
		{
			get
			{
				return _paramValue;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_paramValue = value;
			}
		}

		public bool HasParamValue => has_field.has_field(2);

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
					paramType = deserialize.read_integer();
					break;
				case 2:
					paramValue = deserialize.read_integer();
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
				serialize.write_integer(paramType, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(paramValue, 2);
			}
			return serialize.close();
		}
	}
}
