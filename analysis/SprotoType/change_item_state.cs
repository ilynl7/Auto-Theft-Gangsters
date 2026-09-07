using Sproto;

namespace SprotoType;

public class change_item_state
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _indexId;

		private long _type;

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

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(1);

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
					type = deserialize.read_integer();
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
				serialize.write_integer(type, 1);
			}
			return serialize.close();
		}
	}
}
