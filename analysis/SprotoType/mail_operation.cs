using Sproto;

namespace SprotoType;

public class mail_operation
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _mailId;

		private long _operation;

		public long mailId
		{
			get
			{
				return _mailId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_mailId = value;
			}
		}

		public bool HasMailId => has_field.has_field(0);

		public long operation
		{
			get
			{
				return _operation;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_operation = value;
			}
		}

		public bool HasOperation => has_field.has_field(1);

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
					mailId = deserialize.read_integer();
					break;
				case 1:
					operation = deserialize.read_integer();
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
				serialize.write_integer(mailId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(operation, 1);
			}
			return serialize.close();
		}
	}
}
