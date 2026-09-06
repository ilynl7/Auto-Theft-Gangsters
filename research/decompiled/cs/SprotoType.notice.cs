using Sproto;

namespace SprotoType;

public class notice
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private string _notice;

		private bool _repeate;

		public string notice
		{
			get
			{
				return _notice;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_notice = value;
			}
		}

		public bool HasNotice => has_field.has_field(0);

		public bool repeate
		{
			get
			{
				return _repeate;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_repeate = value;
			}
		}

		public bool HasRepeate => has_field.has_field(1);

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
					notice = deserialize.read_string();
					break;
				case 1:
					repeate = deserialize.read_boolean();
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
				serialize.write_string(notice, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_boolean(repeate, 1);
			}
			return serialize.close();
		}
	}
}
