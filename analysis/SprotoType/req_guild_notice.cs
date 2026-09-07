using Sproto;

namespace SprotoType;

public class req_guild_notice
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private string _notice;

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
					notice = deserialize.read_string();
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
				serialize.write_string(notice, 0);
			}
			return serialize.close();
		}
	}
}
