using Sproto;

namespace SprotoType;

public class open_item_package
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _indexId;

		private long _indexId2;

		private long _count;

		public long indexId
		{
			get
			{
				return _indexId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_indexId = value;
			}
		}

		public bool HasIndexId => has_field.has_field(0);

		public long indexId2
		{
			get
			{
				return _indexId2;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_indexId2 = value;
			}
		}

		public bool HasIndexId2 => has_field.has_field(1);

		public long count
		{
			get
			{
				return _count;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_count = value;
			}
		}

		public bool HasCount => has_field.has_field(2);

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
					indexId = deserialize.read_integer();
					break;
				case 1:
					indexId2 = deserialize.read_integer();
					break;
				case 2:
					count = deserialize.read_integer();
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
				serialize.write_integer(indexId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(indexId2, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(count, 2);
			}
			return serialize.close();
		}
	}
}
