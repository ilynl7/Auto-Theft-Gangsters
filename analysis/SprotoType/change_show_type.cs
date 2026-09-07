using Sproto;

namespace SprotoType;

public class change_show_type
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private long _showType;

		public long showType
		{
			get
			{
				return _showType;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_showType = value;
			}
		}

		public bool HasShowType => has_field.has_field(0);

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
					showType = deserialize.read_integer();
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
				serialize.write_integer(showType, 0);
			}
			return serialize.close();
		}
	}
}
