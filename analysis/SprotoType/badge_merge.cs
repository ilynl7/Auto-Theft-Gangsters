using Sproto;

namespace SprotoType;

public class badge_merge
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _indexId;

		private string _nextItemId;

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

		public string nextItemId
		{
			get
			{
				return _nextItemId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_nextItemId = value;
			}
		}

		public bool HasNextItemId => has_field.has_field(1);

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
					nextItemId = deserialize.read_string();
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
				serialize.write_string(nextItemId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(count, 2);
			}
			return serialize.close();
		}
	}

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private long _state;

		public long state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(0);

		public response()
			: base(max_field_count)
		{
		}

		public response(byte[] buffer)
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
					state = deserialize.read_integer();
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
				serialize.write_integer(state, 0);
			}
			return serialize.close();
		}
	}
}
