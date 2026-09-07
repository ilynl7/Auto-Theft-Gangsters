using Sproto;

namespace SprotoType;

public class ret_use_item
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _success;

		private long _indexId;

		public long success
		{
			get
			{
				return _success;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_success = value;
			}
		}

		public bool HasSuccess => has_field.has_field(0);

		public long indexId
		{
			get
			{
				return _indexId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_indexId = value;
			}
		}

		public bool HasIndexId => has_field.has_field(1);

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
					success = deserialize.read_integer();
					break;
				case 1:
					indexId = deserialize.read_integer();
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
				serialize.write_integer(success, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(indexId, 1);
			}
			return serialize.close();
		}
	}
}
