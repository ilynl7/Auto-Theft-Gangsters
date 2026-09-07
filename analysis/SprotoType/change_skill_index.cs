using Sproto;

namespace SprotoType;

public class change_skill_index
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private long _index;

		public long index
		{
			get
			{
				return _index;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_index = value;
			}
		}

		public bool HasIndex => has_field.has_field(0);

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
					index = deserialize.read_integer();
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
				serialize.write_integer(index, 0);
			}
			return serialize.close();
		}
	}
}
