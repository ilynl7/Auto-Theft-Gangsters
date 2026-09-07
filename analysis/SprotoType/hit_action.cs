using Sproto;

namespace SprotoType;

public class hit_action
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _targetid;

		private long _senderId;

		private string _effinfoId;

		public long targetid
		{
			get
			{
				return _targetid;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_targetid = value;
			}
		}

		public bool HasTargetid => has_field.has_field(0);

		public long senderId
		{
			get
			{
				return _senderId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_senderId = value;
			}
		}

		public bool HasSenderId => has_field.has_field(1);

		public string effinfoId
		{
			get
			{
				return _effinfoId;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_effinfoId = value;
			}
		}

		public bool HasEffinfoId => has_field.has_field(2);

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
					targetid = deserialize.read_integer();
					break;
				case 1:
					senderId = deserialize.read_integer();
					break;
				case 2:
					effinfoId = deserialize.read_string();
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
				serialize.write_integer(targetid, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(senderId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(effinfoId, 2);
			}
			return serialize.close();
		}
	}
}
