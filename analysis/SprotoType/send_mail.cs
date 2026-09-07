using Sproto;

namespace SprotoType;

public class send_mail
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _receiveId;

		private string _context;

		public long receiveId
		{
			get
			{
				return _receiveId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_receiveId = value;
			}
		}

		public bool HasReceiveId => has_field.has_field(0);

		public string context
		{
			get
			{
				return _context;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_context = value;
			}
		}

		public bool HasContext => has_field.has_field(1);

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
					receiveId = deserialize.read_integer();
					break;
				case 1:
					context = deserialize.read_string();
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
				serialize.write_integer(receiveId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(context, 1);
			}
			return serialize.close();
		}
	}
}
