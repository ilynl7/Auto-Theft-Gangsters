using Sproto;

namespace SprotoType;

public class send_mail_box
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private string _subject;

		private string _context;

		private string _email;

		public string subject
		{
			get
			{
				return _subject;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_subject = value;
			}
		}

		public bool HasSubject => has_field.has_field(0);

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

		public string email
		{
			get
			{
				return _email;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_email = value;
			}
		}

		public bool HasEmail => has_field.has_field(2);

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
					subject = deserialize.read_string();
					break;
				case 1:
					context = deserialize.read_string();
					break;
				case 2:
					email = deserialize.read_string();
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
				serialize.write_string(subject, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(context, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(email, 2);
			}
			return serialize.close();
		}
	}
}
