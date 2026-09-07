using Sproto;

namespace SprotoType;

public class mail_delete
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private long _mailId;

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
					mailId = deserialize.read_integer();
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
				serialize.write_integer(mailId, 0);
			}
			return serialize.close();
		}
	}
}
