using Sproto;

namespace SprotoType;

public class change_scene_line
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private long _line_index;

		public long line_index
		{
			get
			{
				return _line_index;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_line_index = value;
			}
		}

		public bool HasLine_index => has_field.has_field(0);

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
					line_index = deserialize.read_integer();
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
				serialize.write_integer(line_index, 0);
			}
			return serialize.close();
		}
	}
}
